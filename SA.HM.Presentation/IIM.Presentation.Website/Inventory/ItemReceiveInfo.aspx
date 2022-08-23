<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="ItemReceiveInfo.aspx.cs" EnableEventValidation="false"
    Inherits="HotelManagement.Presentation.Website.PurchaseManagment.ItemReceiveInfo" %>

<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;

        $(document).ready(function () {

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            $("#ContentPlaceHolder1_ddlSearchSupplier").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlCostCenterSearch").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            //$('#ContentPlaceHolder1_txtFromDate').datepicker({
            //    changeMonth: true,
            //    changeYear: true,
            //    dateFormat: innBoarDateFormat,
            //    defaultDate: DayOpenDate,
            //    onClose: function (selectedDate) {
            //        $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
            //    }
            //}).datepicker("setDate", DayOpenDate);

            //$('#ContentPlaceHolder1_txtToDate').datepicker({
            //    changeMonth: true,
            //    changeYear: true,
            //    defaultDate: DayOpenDate,
            //    dateFormat: innBoarDateFormat,
            //    onClose: function (selectedDate) {
            //        $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
            //    }
            //}).datepicker("setDate", DayOpenDate);

            var txtFromDate = '<%=txtFromDate.ClientID%>'
            var txtToDate = '<%=txtToDate.ClientID%>'

            $('#' + txtFromDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtToDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtToDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtFromDate).datepicker("option", "maxDate", selectedDate);
                }
            });

            if (Cookies.getJSON('recsearchoption') != undefined) {
                var SearchOption = Cookies.getJSON('recsearchoption');

                $("#ContentPlaceHolder1_txtSReceiveNumber").val(SearchOption.ReceiveNumber);
                $("#ContentPlaceHolder1_ddlStatus").val(SearchOption.Status);
                $("#ContentPlaceHolder1_ddlReceiveType").val(SearchOption.ReceiveType);
                $("#ContentPlaceHolder1_ddlCostCenterSearch").val(SearchOption.CostCenterId + '');
                $("#ContentPlaceHolder1_txtFromDate").val(SearchOption.FromDate);
                $("#ContentPlaceHolder1_txtToDate").val(SearchOption.ToDate);
                $("#ContentPlaceHolder1_ddlSearchSupplier").val(SearchOption.supplierId + '').trigger("change");

                var pageIndex = SearchOption.PageIndex;

                Cookies.remove('recsearchoption');
                SearchReceiveOrder(pageIndex, 1);
            }

        });

        function ReceiveOrderEdit(ReceiveType, ReceivedId, SupplierId, CostCenterId, POrderId) {

            Cookies.set('rid', ReceivedId, { expires: 1 });

            var receiveNumber = "0", receiveType = "", supplierId = "0", costCenterId = "", fromDate = null, toDate = null,
                status = "";

            var pageIndex = $("#GridPagingContainer").find("li.active").index();

            if (pageIndex < 0)
                pageIndex = 1;

            receiveNumber = $("#ContentPlaceHolder1_txtSReceiveNumber").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val();
            receiveType = $("#ContentPlaceHolder1_ddlReceiveType").val();
            costCenterId = $("#ContentPlaceHolder1_ddlCostCenterSearch").val();
            fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            toDate = $("#ContentPlaceHolder1_txtToDate").val();
            supplierId = $("#ContentPlaceHolder1_ddlSearchSupplier").val();

            var SearchOption = {
                ReceiveNumber: receiveNumber,
                Status: status,
                ReceiveType: receiveType,
                CostCenterId: costCenterId,
                FromDate: fromDate,
                ToDate: toDate,
                SupplierId: supplierId,
                PageIndex: pageIndex
            };

            Cookies.set('recsearchoption', SearchOption, { expires: 1 });

            var url = "/Inventory/ItemReceive.aspx?rid=" + ReceivedId + "&pid=" + POrderId;
            window.location = url;
            return false;
        }

        function ReceiveOrderEditWithConfirmation(ReceiveType, ReceivedId, SupplierId, CostCenterId, POrderId) {
            if (!confirm("Do you Want To Edit?")) {
                return false;
            }
            ReceiveOrderEdit(ReceiveType, ReceivedId, SupplierId, CostCenterId, POrderId);
        }

        function SearchReceiveOrder(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#ReceiveOrderGrid tbody tr").length;
            var receiveNumber = "0", status = "", receiveType = "", supplierId = "0", costCenterId = "", fromDate = null, toDate = null;

            var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

            if (companyId == "0") {
                companyId = 0;
                projectId = 0;
            }

            receiveNumber = $("#ContentPlaceHolder1_txtSReceiveNumber").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val();
            receiveType = $("#ContentPlaceHolder1_ddlReceiveType").val();
            costCenterId = $("#ContentPlaceHolder1_ddlCostCenterSearch").val();
            fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            toDate = $("#ContentPlaceHolder1_txtToDate").val();
            supplierId = $("#ContentPlaceHolder1_ddlSearchSupplier").val();

            if (fromDate != "")
                fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');

            if (toDate != "")
                toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            if (costCenterId == "0")
                costCenterId = null;

            if (supplierId == "0")
                supplierId = null;

            $("#GridPagingContainer ul").html("");
            $("#ReceiveOrderGrid tbody").html("");

            if (pageNumber < 0)
                pageNumber = 1;

            PageMethods.SearchReceiveOrder(companyId, projectId, receiveType, fromDate, toDate, receiveNumber, status, costCenterId, supplierId,
                gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchPurchaseOrderSucceed, OnSearchPurchaseOrderFailed);

            return false;
        }
        function OnSearchPurchaseOrderSucceed(result) {

            var tr = "";

            $.each(result.GridData, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style='width:10%;'>" + gridObject.ReceiveNumber + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.ReceiveType + "</td>";

                if (gridObject.ReceivedDate != null)
                    tr += "<td style='width:5%;'>" + CommonHelper.DateFromDateTimeToDisplay(gridObject.ReceivedDate, innBoarDateFormat) + "</td>";
                else
                    tr += "<td style='width:5%;'></td>";

                tr += "<td style='width:10%;'>" + gridObject.CostCenter + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.LocationName + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.SupplierName + "</td>";
                tr += "<td style='width:5%;'>" + gridObject.TotalReceivedAmount + "</td>";

                if (gridObject.Remarks != null)
                    tr += "<td style='width:20%;'>" + gridObject.Remarks + "</td>";
                else
                    tr += "<td style='width:20%;'></td>";
                tr += "<td style='width:10%;'>" + (gridObject.Status == 'Pending' ? 'Submitted' : gridObject.Status) + "</td>";
                tr += "<td style=\"text-align: center; width:10%; cursor:pointer;\">";

                if (gridObject.IsCanEdit && IsCanEdit) {
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return ReceiveOrderEditWithConfirmation('" + gridObject.ReceiveType + "'," + gridObject.ReceivedId + "," + gridObject.SupplierId + "," + gridObject.CostCenterId + "," + gridObject.POrderId + ")\" alt='Edit'  title='Edit' border='0' />";
                }

                if (gridObject.IsCanDelete && IsCanDelete) {
                    tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return ReceiveOrderDelete('" + gridObject.ReceiveType + "'," + gridObject.ReceivedId + ",'" + gridObject.Status + "'," + gridObject.SupplierId + "," + gridObject.CostCenterId + "," + gridObject.CreatedBy + ")\" alt='Delete'  title='Delete' border='0' />";
                }

                if (gridObject.IsCanChecked && IsCanSave) {
                    tr += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return ReceiveOrderApprovalWithConfirmation('" + gridObject.ReceiveType + "','" + 'Checked' + "'," + gridObject.ReceivedId + "," + gridObject.SupplierId + "," + gridObject.POrderId + ")\" alt='Checked'  title='Check' border='0' />";
                }

                if (gridObject.IsCanApproved && IsCanSave) {
                    tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return ReceiveOrderApprovalWithConfirmation('" + gridObject.ReceiveType + "','" + 'Approved' + "', " + gridObject.ReceivedId + "," + gridObject.SupplierId + "," + gridObject.POrderId + ")\" alt='Approved'  title='Approve' border='0' />";
                }

                tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return ShowReport('" + gridObject.ReceiveType + "'," + gridObject.ReceivedId + ",'" + gridObject.Status + "'," + gridObject.SupplierId + "," + gridObject.CostCenterId + "," + gridObject.CreatedBy + ")\" alt='Invoice' title='Receive Order Info' border='0' />";

                tr += "&nbsp;&nbsp;<img src='../Images/note.png'  onClick= \"javascript:return ShowDealDocuments('" + gridObject.ReceivedId + "')\" alt='Invoice' title='Receive Order Info' border='0' />";
                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.ReceivedId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.SupplierId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.CostCenterId + "</td>";

                tr += "</tr>";

                $("#ReceiveOrderGrid tbody").append(tr);
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

        function ShowDealDocuments(id) {
            console.log("abc");
            PageMethods.LoadDealDocument(id, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }
        function OnLoadDocumentSucceeded(result) {
            $("#imageDiv").html(result);

            $("#dealDocuments").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                minHeight: 400,
                closeOnEscape: true,
                resizable: false,
                title: "Deal Documents",
                show: 'slide'
            });

            return false;
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }

        function ClearSearch() {
            $("#ContentPlaceHolder1_txtSReceiveNumber").val("");
            $("#ContentPlaceHolder1_ddlStatus").val("All");
            $("#ContentPlaceHolder1_ddlReceiveType").val("All");
            $("#ContentPlaceHolder1_ddlCostCenterSearch").val("0");
            $("#ContentPlaceHolder1_txtFromDate").val("");
            $("#ContentPlaceHolder1_txtToDate").val("");
            $("#ContentPlaceHolder1_ddlSearchSupplier").val("0").trigger("change");
        }
        function NewReceiveOrder() {
            window.location = "/Inventory/ItemReceive.aspx";
        }

        function ReceiveOrderDelete(ReceiveType, ReceivedId, ApprovedStatus, SupplierId, CostCenterId, CreatedBy) {
            if (!confirm("Do you Want To Delete?")) {
                return false;
            }

            PageMethods.ReceiveOrderDelete(ReceiveType, ReceivedId, ApprovedStatus, CreatedBy, OnApprovalSucceed, OnApprovalFailed);
        }

        function ReceiveOrderApproval(ReceiveType, ApprovedStatus, ReceivedId, SupplierId, POrderId) {
            PageMethods.ReceiveOrderApproval(ReceiveType, ReceivedId, ApprovedStatus, POrderId, OnApprovalSucceed, OnApprovalFailed);
        }
        function ReceiveOrderApprovalWithConfirmation(ReceiveType, ApprovedStatus, ReceivedId, SupplierId, POrderId) {


            if (!confirm("Do you want to " + (ApprovedStatus == "Checked" ? 'check' : 'approve') + "?")) {
                return false;
            }
            ReceiveOrderApproval(ReceiveType, ApprovedStatus, ReceivedId, SupplierId, POrderId);
        }
        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '/Common/WebMethodPage.aspx/GetCommonCheckByApproveByListForSMS',
                    data: JSON.stringify({ tableName: 'PMProductReceived', primaryKeyName: 'ReceivedId', primaryKeyValue: result.PrimaryKeyValue, featuresValue: 'Receive', statusColumnName: 'Status' }),
                    dataType: "json",
                    success: function (data) {
                        debugger;

                        SendSMSToUserList(data.d, result.PrimaryKeyValue, result.TransactionNo, result.TransactionType, result.TransactionStatus);

                    },
                    error: function (result) {
                        toastr.error("Can not load Check or Approve By List.");
                    }
                });

                SearchReceiveOrder($("#GridPagingContainer").find("li.active").index(), 1);

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

        function ShowReport(ReceiveType, ReceivedId, ApprovedStatus, SupplierId, CostCenterId, CreatedBy) {
            var iframeid = 'printDoc';
            var url = "Reports/frmReportProductReceive.aspx?PRId=" + ReceivedId;
            parent.document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Receive Order Bill",
                show: 'slide'
            });
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchReceiveOrder(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
    </script>

     <div id="dealDocuments" style="display: none;">
        <div id="imageDiv"></div>
    </div>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <div class="panel panel-default">
        <div class="panel-heading">
            Item Receive Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label14" runat="server" class="control-label" Text="Supplier"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlSearchSupplier" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lbl1000" runat="server" class="control-label" Text="Receive Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSReceiveNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblStatus" runat="server" class="control-label" Text="Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" TabIndex="2">
                            <asp:ListItem Text="--- All ---" Value="All"></asp:ListItem>
                            <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                            <asp:ListItem Text="Checked" Value="Checked"></asp:ListItem>
                            <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                            <asp:ListItem Text="Cancel" Value="Cancel"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label10" runat="server" class="control-label" Text="Receive Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReceiveType" CssClass="form-control" runat="server">
                            <asp:ListItem Text="All" Value="All"></asp:ListItem>
                            <asp:ListItem Text="AdHoc" Value="AdHoc"></asp:ListItem>
                            <asp:ListItem Text="Receive From Purchase" Value="Purchase"></asp:ListItem>
                            <asp:ListItem Text="Receive From LC" Value="LC"></asp:ListItem>
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
                        <input type="button" id="btnNewPurchaseInfo" class="TransactionalButton btn btn-primary btn-sm" value="New Item Receive" onclick="NewReceiveOrder()" />
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
            <table id="ReceiveOrderGrid" class="table table-bordered table-condensed table-responsive" width="100%">
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                        <th style="width: 10%;">Receive Number
                        </th>
                        <th style="width: 10%;">Order Type
                        </th>
                        <th style="width: 5%;">Date
                        </th>
                        <th style="width: 10%;">Cost Center
                        </th>
                        <th style="width: 10%;">Location
                        </th>
                        <th style="width: 10%;">Supplier
                        </th>                        
                        <th style="width: 5%;">Amount
                        </th>
                        <th style="width: 20%;">Remarks
                        </th>
                        <th style="width: 10%;">Status
                        </th>                      
                        <th style="width: 10%;">Action
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
