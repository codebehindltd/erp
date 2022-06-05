<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="PurchaseInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.PurchaseInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">

        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Purchase</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Product PO Approval</li>";
            var breadCrumbs = moduleName + formName;

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            });//.datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });//.datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_ddlGLCompanyId").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlSupplier").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            if (Cookies.getJSON('posearchoption') != undefined) {
                var SearchOption = Cookies.getJSON('posearchoption');

                $("#ContentPlaceHolder1_txtSPONumber").val(SearchOption.PONumber);
                $("#ContentPlaceHolder1_ddlStatus").val(SearchOption.Status);
                $("#ContentPlaceHolder1_ddlPOType").val(SearchOption.POType);
                $("#ContentPlaceHolder1_ddlCostCenterSearch").val(SearchOption.CostCenterId + '');
                $("#ContentPlaceHolder1_txtFromDate").val(SearchOption.FromDate);
                $("#ContentPlaceHolder1_txtToDate").val(SearchOption.ToDate);
                $("#ContentPlaceHolder1_ddlSupplier").val(SearchOption.supplierId + '').trigger("change");

                var pageIndex = SearchOption.PageIndex;

                Cookies.remove('posearchoption');
                SearchPurchaseOrder(pageIndex, 1);
            }

        });

        function PurchaseOrderEdit(POType, POrderId, SupplierId, CostCenterId) {
            if (!confirm("Do you Want To Edit?")) {
                return false;
            }
            Cookies.set('poid', POrderId, { expires: 1 });

            var poNumber = "0", poType = "", supplierId = "0", costCenterId = "", fromDate = null, toDate = null,
                status = "";

            var pageIndex = $("#GridPagingContainer").find("li.active").index();

            if (pageIndex < 0)
                pageIndex = 1;

            poNumber = $("#ContentPlaceHolder1_txtSPONumber").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val();
            poType = $("#ContentPlaceHolder1_ddlPOType").val();
            costCenterId = $("#ContentPlaceHolder1_ddlCostCenterSearch").val();
            fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            toDate = $("#ContentPlaceHolder1_txtToDate").val();
            supplierId = $("#ContentPlaceHolder1_ddlSupplier").val();

            var SearchOption = {
                PONumber: poNumber,
                Status: status,
                POType: poType,
                CostCenterId: costCenterId,
                FromDate: fromDate,
                ToDate: toDate,
                SupplierId: supplierId,
                PageIndex: pageIndex
            };

            Cookies.set('posearchoption', SearchOption, { expires: 1 });

            var url = "/PurchaseManagment/PurchaseOrder.aspx?poid=" + POrderId;
            window.location = url;
            return false;
        }

        function SearchPurchaseOrder(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#PurchaseOrderGrid tbody tr").length;
            var companyId = 0, poNumber = "0", requisitionNumber = "0", poType = "", supplierId = "0", orderType = "0", costCenterId = "", fromDate = null, toDate = null,
                status = "";

            companyId = $("#ContentPlaceHolder1_ddlGLCompanyId").val();
            poNumber = $("#ContentPlaceHolder1_txtSPONumber").val();
            requisitionNumber = $("#ContentPlaceHolder1_txtRequisitionNumber").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val();
            poType = $("#ContentPlaceHolder1_ddlPOType").val();
            costCenterId = $("#ContentPlaceHolder1_ddlCostCenterSearch").val();
            fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            toDate = $("#ContentPlaceHolder1_txtToDate").val();
            supplierId = $("#ContentPlaceHolder1_ddlSupplier").val();
            orderType = $("#ContentPlaceHolder1_ddlOrderType").val();

            if (fromDate != "")
                fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');

            if (toDate != "")
                toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            if (costCenterId == "0")
                costCenterId = null;

            if (supplierId == "0")
                supplierId = null;

            $("#GridPagingContainer ul").html("");
            $("#PurchaseOrderGrid tbody").html("");

            if (pageNumber < 0)
                pageNumber = 1;

            PageMethods.SearchPurchaseOrder(companyId, orderType, poType, fromDate, toDate, poNumber, requisitionNumber, status, costCenterId, supplierId,
                gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchPurchaseOrderSucceed, OnSearchPurchaseOrderFailed);

            return false;
        }
        function OnSearchPurchaseOrderSucceed(result) {

            var tr = "";

            $.each(result.GridData, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style='width:8%;'>" + gridObject.PONumber + "</td>";
                tr += "<td style='width:8%;'>" + gridObject.POType + "</td>";

                if (gridObject.PODate !== null)
                    tr += "<td style='width:8%;'>" + CommonHelper.DateFromDateTimeToDisplay(gridObject.PODate, innBoarDateFormat) + "</td>";
                else
                    tr += "<td style='width:8%;'></td>";

                tr += "<td style='width:10%;'>" + gridObject.CostCenter + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.SupplierName + "</td>";

                tr += "<td style='width:10%;'>" + (gridObject.ApprovedStatus == 'Pending' ? 'Submitted' : gridObject.ApprovedStatus) + "</td>";
                if (gridObject.ReceiveStatus != null) {
                    tr += "<td style='width:10%;'>" + gridObject.ReceiveStatus + "</td>";
                }
                else {
                    tr += "<td style='width:10%;'></td>";
                }
                tr += "<td style='width:21%;'>" + gridObject.PODescription + "</td>";
                tr += "<td style='width:5%;'>" + gridObject.PurchaseOrderAmount + "</td>";

                tr += "<td style=\"text-align: center; width:15%; cursor:pointer;\">";

                if (gridObject.IsCanEdit && IsCanEdit) {
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return PurchaseOrderEdit('" + gridObject.POType + "'," + gridObject.POrderId + "," + gridObject.SupplierId + "," + gridObject.CostCenterId + ")\" alt='Edit'  title='Edit' border='0' />";
                }

                if (gridObject.IsCanDelete && IsCanDelete) {
                    tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return PurchaseOrderDelete('" + gridObject.POType + "'," + gridObject.POrderId + ",'" + gridObject.ApprovedStatus + "'," + gridObject.SupplierId + "," + gridObject.CostCenterId + "," + gridObject.CreatedBy + ")\" alt='Delete'  title='Delete' border='0' />";
                }

                if (gridObject.IsCanChecked && IsCanSave) {
                    tr += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return PurchaseOrderApproval('" + gridObject.POType + "','" + 'Checked' + "'," + gridObject.POrderId + "," + gridObject.SupplierId + "," + gridObject.CostCenterId + ")\" alt='Check'  title='Check' border='0' />";
                }

                if (gridObject.IsCanApproved && IsCanSave) {
                    tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return PurchaseOrderApproval('" + gridObject.POType + "','" + 'Approved' + "', " + gridObject.POrderId + "," + gridObject.SupplierId + "," + gridObject.CostCenterId + ")\" alt='Approve'  title='Approve' border='0' />";
                }
                if (gridObject.IsCanPOReOpen && $("#ContentPlaceHolder1_hfIsAdminUser").val() == "1") {
                    tr += "&nbsp;&nbsp;<img src='../Images/reOpen.png' style='width:18px;height:18px' onClick= \"javascript:return PurchaseOrderReOpen(" + gridObject.POrderId + ",'" + gridObject.PONumber + "')\" alt='Re Open'  title='Re Open' border='0' />";
                }

                if (gridObject.CurrencyType == "Local") {
                    tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return ShowReport('" + gridObject.LocalCurrencyId + "','" + gridObject.POType + "'," + gridObject.POrderId + ",'" + gridObject.ApprovedStatus + "'," + gridObject.SupplierId + "," + gridObject.CostCenterId + "," + gridObject.CreatedBy + ")\" alt='Invoice' title='Purchase Order' border='0' />";
                }
                else {
                    tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return ShowReport('" + gridObject.LocalCurrencyId + "','" + gridObject.POType + "'," + gridObject.POrderId + ",'" + gridObject.ApprovedStatus + "'," + gridObject.SupplierId + "," + gridObject.CostCenterId + "," + gridObject.CreatedBy + ")\" alt='Invoice' title='Purchase Order (" + gridObject.LocalCurrencyName + ")' border='0' />";
                    tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return ShowReport('" + gridObject.CurrencyId + "','" + gridObject.POType + "'," + gridObject.POrderId + ",'" + gridObject.ApprovedStatus + "'," + gridObject.SupplierId + "," + gridObject.CostCenterId + "," + gridObject.CreatedBy + ")\" alt='Invoice' title='Purchase Order (" + gridObject.CurrencyName + ")' border='0' />";
                }

                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.POrderId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.SupplierId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.CostCenterId + "</td>";

                tr += "</tr>";

                $("#PurchaseOrderGrid tbody").append(tr);
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
            $("#ContentPlaceHolder1_txtSPONumber").val("");
            $("#ContentPlaceHolder1_ddlStatus").val("All");
            $("#ContentPlaceHolder1_ddlPOType").val("All");
            $("#ContentPlaceHolder1_ddlCostCenterSearch").val("0");
            $("#ContentPlaceHolder1_txtFromDate").val("");
            $("#ContentPlaceHolder1_txtToDate").val("");
            $("#ContentPlaceHolder1_ddlSupplier").val("0").trigger("change");
        }
        function NewPurchaseOrder() {
            window.location = "/PurchaseManagment/PurchaseOrder.aspx";
        }

        function PurchaseOrderDelete(POType, POrderId, ApprovedStatus, SupplierId, CostCenterId, CreatedBy) {
            if (!confirm("Do You Want To Delete.")) { return false; }
            PageMethods.PurchaseOrderDelete(POType, POrderId, ApprovedStatus, CreatedBy, OnApprovalSucceed, OnApprovalFailed);
        }

        function PurchaseOrderApproval(POType, ApprovedStatus, POrderId, SupplierId, CostCenterId) {
            if (!confirm('Do you want to ' + (ApprovedStatus == 'Checked' ? 'check' : 'approve') + " ?")) {
                return false;
            }
            PageMethods.PurchaseOrderApproval(POType, POrderId, ApprovedStatus, OnApprovalSucceed, OnApprovalFailed);
        }
        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '/Common/WebMethodPage.aspx/GetCommonCheckByApproveByListForSMS',
                    data: JSON.stringify({ tableName: 'PMPurchaseOrder', primaryKeyName: 'POrderId', primaryKeyValue: result.PrimaryKeyValue, featuresValue: 'Purchase', statusColumnName: 'ApprovedStatus' }),
                    dataType: "json",
                    success: function (data) {
                        debugger;

                        SendSMSToUserList(data.d, result.PrimaryKeyValue, result.TransactionNo, result.TransactionType, result.TransactionStatus);

                    },
                    error: function (result) {
                        toastr.error("Can not load Check or Approve By List.");
                    }
                });

                SearchPurchaseOrder($("#GridPagingContainer").find("li.active").index(), 1);

            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function SendSMSToUserList(UserList, PrimaryKeyValue, TransactionNo, TransactionType, TransactionStatus) {
            debugger;

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

                        // CommonHelper.AlertMessage(data.d.AlertMessage);

                    },
                    error: function (result) {
                        //alert("Error");

                    }
                });

            }

            return false;
        }
        function OnApprovalFailed() {

        }

        function ShowReport(CurrencyId, POType, POrderId, ApprovedStatus, SupplierId, CostCenterId, CreatedBy) {
            var iframeid = 'printDoc';
            var url = "Reports/frmReportPurchaseOrderInvoice.aspx?POrderId=" + POrderId + "&SupId=" + SupplierId + "&CurrencyId=" + CurrencyId;
            parent.document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 800,
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Purchase Order Info",
                show: 'slide'
            });
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchPurchaseOrder(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function PurchaseOrderReOpen(POrderId, PONumber) {
            if (!confirm("Do you Want To Re-Open " + PONumber + "?")) {
                return false;
            }
            PageMethods.ReOpenPurchaseOrder(POrderId, OnReOpenPurchaseOrderSucceed, OnReOpenPurchaseOrderFailed);
            return false;
        }
        function OnReOpenPurchaseOrderSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                //LoadRequisition();
                SearchPurchaseOrder($("#GridPagingContainer").find("li.active").index(), 1);
            }
        }
        function OnReOpenPurchaseOrderFailed(error) {
            CommonHelper.SpinnerClose();
            toastr.error(error.get_message());
        }
    </script>

    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsAdminUser" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsItemDescriptionSuggestInPurchaseOrder" runat="server" Value="0" />

    <div class="panel panel-default">
        <div class="panel-heading">
            Purchase Order Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group" id="CompanyDiv" style="display: none;">
                    <div class="col-md-2">
                        <asp:HiddenField ID="hfIsSingleGLCompany" runat="server"></asp:HiddenField>
                        <asp:Label ID="Label17" runat="server" class="control-label" Text="Company"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlGLCompanyId" runat="server" CssClass="form-control"
                            TabIndex="7">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSupplier" runat="server" class="control-label" Text="Supplier"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSPONumber" runat="server" class="control-label" Text="PO Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSPONumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblRequisitionNumber" runat="server" class="control-label" Text="Requisition Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtRequisitionNumber" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label10" runat="server" class="control-label" Text="PO Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlPOType" CssClass="form-control" runat="server">
                            <asp:ListItem Text="--- All ---" Value="All"></asp:ListItem>
                            <asp:ListItem Text="AdHoc" Value="AdHoc"></asp:ListItem>
                            <asp:ListItem Text="Order From Requisition" Value="Requisition"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label12" runat="server" class="control-label" Text="Cost Center"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCostCenterSearch" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" Placeholder="From Date" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" Placeholder="To Date" CssClass="form-control" runat="server" TabIndex="4"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblOrderType" runat="server" class="control-label" Text="Order Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList runat="server" ID="ddlOrderType" CssClass="form-control">
                            <asp:ListItem Text="--- All ---" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Local" Value="Local"></asp:ListItem>
                            <asp:ListItem Text="Foreign" Value="Foreign"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-2">
                        <asp:Label ID="lblStatus" runat="server" class="control-label" Text="Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" TabIndex="5">
                            <asp:ListItem Text="--- All ---" Value="All"></asp:ListItem>
                            <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                            <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                            <asp:ListItem Text="Cancel" Value="Cancel"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm" value="Search" onclick="SearchPurchaseOrder(1, 1)" />
                        <input type="button" id="btnSearchCancel" class="TransactionalButton btn btn-primary btn-sm" value="Clear" onclick="ClearSearch()" />
                        <input type="button" id="btnNewPurchaseInfo" class="TransactionalButton btn btn-primary btn-sm" value="New Purchase Order" onclick="NewPurchaseOrder()" />
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
            <table id="PurchaseOrderGrid" class="table table-bordered table-condensed table-responsive" width="100%">
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                        <th style="width: 8%;">PO Number
                        </th>
                        <th style="width: 8%;">Order Type
                        </th>
                        <th style="width: 5%;">Order Date
                        </th>
                        <th style="width: 10%;">Cost Center
                        </th>
                        <th style="width: 10%;">Supplier
                        </th>
                        <th style="width: 8%;">Status
                        </th>
                        <th style="width: 10%;">Receive Status
                        </th>
                        <th style="width: 20%;">Description
                        </th>
                        <th style="width: 8%;">PO Amount
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
    <script type="text/javascript">

        $(document).ready(function () {
            if ($("#<%=hfIsSingleGLCompany.ClientID %>").val() == "1") {
                $('#CompanyDiv').hide();
            }
            else {
                $('#CompanyDiv').show();
            }
        });
    </script>
</asp:Content>
