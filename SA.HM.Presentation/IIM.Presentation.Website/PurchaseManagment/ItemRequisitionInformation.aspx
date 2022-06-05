<%@ Page Title="Item Requisition Information" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ItemRequisitionInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.ItemRequisitionInformation" %>

<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var pendingItem = [], approvedItem = [];
        var str = "";
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {

            //read cookies for click event after requisition edit
            str = Cookies.get('frmtoDate');
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            
            if (IsCanSave) {
                $('#btnApprove').show();
            } else {
                $('#btnApprove').hide();
            }

            if (str != undefined) {
                var str = Cookies.get('frmtoDate');
                str = str.split(',');
                $("#ContentPlaceHolder1_txtFromDate").val(str[0]);
                $("#ContentPlaceHolder1_txtToDate").val(str[1]);
                $("#ContentPlaceHolder1_ddlStatus").val(str[2]);
                Cookies.remove('frmtoDate');
                //$("#ContentPlaceHolder1_btnSearch").trigger("click");
            }
            $("#ContentPlaceHolder1_txtFromDate").datepicker({
                changeMonth: true,
                changeYear: true,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            }); //.datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_txtToDate").datepicker({
                changeMonth: true,
                changeYear: true,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            }); //.datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_ddlRequisitionBy").select2({
                tags: "true",
                allowClear: true,
                width: "99.75%",
            });
            $("#btnApprove").click(function () {

                var requsitionId = "0", requsitionDetailsId = "0";
                var quantity = "", approvedQuantity = "";

                pendingItem = []; approvedItem = [];

                requsitionId = $("#ContentPlaceHolder1_hfRequisitionId").val();

                $("#DetailsRequisitionGrid tbody tr").each(function (index, item) {

                    if ($(item).find("td:eq(0)").find("input").is(':checkbox')) {

                        quantity = $.trim($(item).find("td:eq(3)").text());
                        approvedQuantity = $.trim($(item).find("td:eq(4)").find("input").val());
                        requsitionDetailsId = $.trim($(item).find("td:eq(6)").text());

                        if ($(item).find("td:eq(0)").find("input").is(":checked") && !$(item).find("td:eq(0)").find("input").is(":disabled")) {

                            if (approvedQuantity == "" || approvedQuantity == "0") {
                                approvedQuantity = quantity;
                            }

                            approvedItem.push({
                                RequisitionDetailsId: parseInt(requsitionDetailsId, 10),
                                RequisitionId: parseInt(requsitionId, 10),
                                Quantity: parseFloat(quantity),
                                ApprovedQuantity: parseFloat(approvedQuantity)

                            });
                        }
                        else if (!$(item).find("td:eq(0)").find("input").is(":disabled")) {
                            pendingItem.push({
                                RequisitionDetailsId: parseInt(requsitionDetailsId, 10),
                                RequisitionId: parseInt(requsitionId, 10),
                                ApprovedQuantity: parseFloat(0),
                                ApprovedStatus: "Cancel",
                                Remarks: "Canceled"
                            });
                        }
                    }
                });

                if (approvedItem.length < 1) {
                    toastr.warning("Please Check Some Items From Requisition Details.");
                    return false;
                }
                var text = $("#btnApprove").val();
                if (text == "Check Requisition") {
                    if (!confirm("Do you Want To Check?")) {
                        return false;
                    }
                }
                else if (text == "Approve Requisition") {
                    if (!confirm("Do you Want To Approve?")) {
                        return false;
                    }

                }

                CommonHelper.SpinnerOpen();


                var hfIsRequisitionCheckedByEnable = $("#ContentPlaceHolder1_hfIsRequisitionCheckedByEnable").val();

                PageMethods.ApprovedRequsition(hfIsRequisitionCheckedByEnable, requsitionId, approvedItem, pendingItem, OnApprovedRequsitionSucceed, OnApprovedRequsitionFailed);

                return false;
            });
        });
        
        function OnApprovedRequsitionSucceed(result) {
            if (result.IsSuccess) {
                debugger;
                CommonHelper.AlertMessage(result.AlertMessage);
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '/Common/WebMethodPage.aspx/GetCommonCheckByApproveByListForSMS',
                    data: JSON.stringify({ tableName: 'PMRequisition', primaryKeyName: 'RequisitionId', primaryKeyValue: result.PrimaryKeyValue, featuresValue: 'Requisition', statusColumnName: 'ApprovedStatus' }),
                    dataType: "json",
                    success: function (data) {

                        SendSMSToUserList(data.d, result.PrimaryKeyValue, result.TransactionNo, result.TransactionType, result.TransactionStatus);

                    },
                    error: function (result) {
                        toastr.error("Can not load Check or Approve By List.");
                    }
                });

                $("#DetailsRequisitionDialog").dialog("close");

                $("#ContentPlaceHolder1_hfRequisitionId").val("0");
                pendingItem = [];
                approvedItem = [];

                //PerformClearAction();
                //-----$("#ContentPlaceHolder1_btnSearch").trigger("click");

                if ($("#ContentPlaceHolder1_hfPageNumber").val() == "") {
                    LoadRequisitionDetails($("#GridPagingContainer").find("li.active").index(), 1);
                }
                else {
                    var pageNumber = $("#ContentPlaceHolder1_hfPageNumber").val();
                    //toastr.info('66666');
                    LoadRequisitionDetails(pageNumber, 1);
                }
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            CommonHelper.SpinnerClose();
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

        function OnApprovedRequsitionFailed(error) {
            CommonHelper.SpinnerClose();
        }

        function RequisitonApproval(requisitionId, ApprovedStatus) {
            PageMethods.RequisitionOrderApproval(requisitionId, ApprovedStatus, OnApprovalSucceed, OnApprovalFailed);
        }

        function RequisitonApprovalWithConfirmation(requisitionId, ApprovedStatus) {

            if (!confirm('Do you want to ' + (ApprovedStatus == 'Checked' ? 'check' : 'approve') + " ?")) {
                return false;
            }
            RequisitonApproval(requisitionId, ApprovedStatus);
        }
        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                LoadRequisitionDetails($("#GridPagingContainer").find("li.active").index(), 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnApprovalFailed() {

        }

        function LoadRequisitionDetails(pageNumber, IsCurrentOrPreviousPage) {
            $("#ContentPlaceHolder1_hfPageNumber").val(pageNumber);
            var company = $("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val();
            var project = $("#ContentPlaceHolder1_companyProjectUserControl_hfGLProjectId").val();

            var gridRecordsCount = $("#tbRequisitionDetails tbody tr").length;
            var fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();
            var requisitionNo = $("#ContentPlaceHolder1_srcRequisitionNo").val();
            var status = $("#ContentPlaceHolder1_ddlStatus").val();
            var requisitionBy = $("#ContentPlaceHolder1_ddlRequisitionBy").val();
            //var requisitionBy = $("#ContentPlaceHolder1_ddlRequisitionBy").val();
            //if (fromDate == "") {
            //    $("#ContentPlaceHolder1_txtFromDate").val(GetStringFromDateTime(new Date));
            //    fromDate = GetStringFromDateTime(new Date);
            //}
            //if (toDate == "") {
            //    $("#ContentPlaceHolder1_txtToDate").val(GetStringFromDateTime(new Date));
            //    toDate = GetStringFromDateTime(new Date);
            //}
            //if (company != "0" || company == "") {
            //    if (project == "0" || project == "") {
            //        toastr.warning("Please Select Project");
            //        return false;
            //    }
            //}
            
            PageMethods.GetRequisitionDetails(fromDate, toDate, requisitionNo, status, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, company, project, requisitionBy, OnsuccessLoading, OnfailLoading);
            return false;
        }

        function OnsuccessLoading(result) {

            $("#tbRequisitionDetails tbody tr").remove();
            $("#GridPagingContainer ul").html("");
            var tr = "", totalRow = 0, editLink = "", deleteLink = "", invoiceLink = "", approvalLink = "", poLink = "", poButton = "";
            var editPermission = true, deletePermission = true, approvalPermission = true;

            if (result.GridData.length == 0) {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"10\" >No Data Found</td> </tr>";
                $("#tbRequisitionDetails tbody ").append(emptyTr);
                return false;
            }

            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#tbRequisitionDetails tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:10%;\">" + gridObject.PRNumber + "</td>";
                tr += "<td align='left' style=\"width:8%;\">" + GetStringFromDateTime(gridObject.CreatedDate) + "</td>";
                tr += "<td align='left' style=\"width:10%\">" + gridObject.FromCostCenter + "</td>";
                tr += "<td align='left' style=\"width:10%\">" + gridObject.ToCostCenter + "</td>";
                tr += "<td align='left' style=\"width:10%\">" + gridObject.CreatedByName + "</td>";
                tr += "<td align='left' style=\"width:8%\">" + (gridObject.ApprovedStatus == 'Submit' ? 'Submitted' : gridObject.ApprovedStatus) + "</td>";

                if (gridObject.POStatus != null)
                    tr += "<td align='left' style=\"width:10%\">" + gridObject.POStatus + "</td>";
                else
                    tr += "<td align='left' style=\"width:10%\"></td>";

                if (gridObject.TransferStatus != null)
                    tr += "<td align='left' style=\"width:10%\">" + gridObject.TransferStatus + "</td>";
                else
                    tr += "<td align='left' style=\"width:10%\"></td>";

                if (gridObject.ReceiveStatus != null)
                    tr += "<td align='left' style=\"width:10%\">" + gridObject.ReceiveStatus + "</td>";
                else
                    tr += "<td align='left' style=\"width:10%\"></td>";

                if (gridObject.IsCanEdit && IsCanEdit)
                    editLink = "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformEditWithConfirmation('" + gridObject.RequisitionId + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";

                if (gridObject.IsCanDelete && IsCanDelete)
                    deleteLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformCancelAction('" + gridObject.RequisitionId + "');\"><img alt=\"Cancel\" src=\"../Images/delete.png\" title='Delete' /></a>";

                if (gridObject.IsCanChecked && IsCanSave) {
                    $("#btnApprove").val("Check Requisition");
                }
                else if (gridObject.IsCanApproved && IsCanSave) {
                    $("#btnApprove").val("Approve Requisition");
                }

                if ((gridObject.IsCanChecked || gridObject.IsCanApproved) && IsCanSave) {
                    approvalLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return ApproveRequisition('" + gridObject.RequisitionId + "');\"><img alt=\"Approve\" src=\"../Images/detailsInfo.png\" title='Approve' /></a>";
                }
                //debugger;
                if (gridObject.ApprovedStatus == 'Approved') {
                    if (gridObject.POStatus != 'PurchaseComplete') {
                        poButton = "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return CompleteRequisitionOrderWithConfirmation('" + gridObject.RequisitionId + "');\"><img alt=\"Purchase Complete\" src=\"/Images/completerequisition.png\" title='Purchase Complete' /></a>";
                    }
                    if (gridObject.TransferStatus != 'TransferComplete') {
                        poButton += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return CompleteRequisitionTransferOrderWithConfirmation('" + gridObject.RequisitionId + "');\"><img alt=\"Transfer Complete\" src=\"/Images/ItemTransfer.png\" title='Transfer Complete' /></a>";
                    }
                }

                if (gridObject.IsCanChecked && IsCanSave) {
                    poLink += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return RequisitonApprovalWithConfirmation(" + gridObject.RequisitionId + ",'" + 'Checked' + "')\" alt='Check'  title='Check' border='0' />";
                }

                if (gridObject.IsCanApproved && IsCanSave) {
                    poLink += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return RequisitonApprovalWithConfirmation(" + gridObject.RequisitionId + ",'" + 'Approved' + "')\" alt='Approve'  title='Approve' border='0' />";
                }

                invoiceLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return ViewInvoice('" + gridObject.RequisitionId + "');\"><img alt=\"Invoice\" src=\"../Images/ReportDocument.png\" title='Invoice' /></a>";

                //tr += "<td align='center' style=\"width:15%;\">" + editLink + deleteLink + approvalLink + invoiceLink + poLink + "</td>";
                tr += "<td align='center' style=\"width:15%;\">" + editLink + deleteLink + approvalLink + invoiceLink + poButton + "</td>";

                tr += "</tr>";

                $("#tbRequisitionDetails tbody").append(tr);
                tr = "";
                editLink = "";
                deleteLink = "";
                approvalLink = "";
                poLink = "";
                poButton = '';
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

        }
        function OnfailLoading() {

        }

        function ViewInvoice(RequisitionId) {
            var url = "/PurchaseManagment/Reports/frmReportRequisitionOrderInvoice.aspx?RequisitionId=" + RequisitionId;
            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=800,height=680,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");

        }

        function PerformEdit(RequisitionId) {

            var temp = $("#ContentPlaceHolder1_txtFromDate").val() + ',' +
                $("#ContentPlaceHolder1_txtToDate").val() + ',' +
                $("#ContentPlaceHolder1_ddlStatus").val();

            Cookies.set('frmtoDate', temp, { expires: 1 });
            var url = "/PurchaseManagment/frmPMRequisition.aspx?RqId=" + RequisitionId;
            window.location = url;
            // window.open(url, popup_window, "width=800,height=680,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
        }

        function PerformEditWithConfirmation(RequisitionId) {
            if (!confirm("Do you Want To Edit?")) {
                return false;
            }
            PerformEdit(RequisitionId);
        }

        function ApproveRequisition(requisitionId) {
            CommonHelper.SpinnerOpen();
            $("#ContentPlaceHolder1_hfRequisitionId").val(requisitionId);
            PageMethods.GetRequisitionDetailsForApproval(requisitionId, OnFillRequisitionDetailsSucceed, OnFillRequisitionDetailsFailed);
            return false;
        }
        function ApproveRequisitionWithConfirmation(requisitionId) {
            if (!confirm("Do you Want To Approve?")) {
                return false;
            }
            ApproveRequisition(requisitionId);
        }

        function OnFillRequisitionDetailsSucceed(result) {

            $("#DetailsRequisitionGridContaiiner").html(result);

            var InitialApprovedStatus = DetailsRequisitionGrid.rows[1].cells[8].innerHTML;
            if (InitialApprovedStatus == "Submit" || InitialApprovedStatus == "Partially Checked") {
                $("#btnApprove").val("Check Requisition");
            }
            else if (InitialApprovedStatus == "Checked" || InitialApprovedStatus == "Partially Approved") {
                $("#btnApprove").val("Approve Requisition");
            }
            //var trAlreadyAdded = $("#AddedItem tbody tr:eq(" + index + ")");
            //unitDiscount = parseFloat($.trim($(trAlreadyAdded).find("td:eq(13)").text()));
            $("#DetailsRequisitionDialog").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 1000,
                maxWidth: 1100,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Requisition Details",
                show: 'slide'
            });
            CommonHelper.SpinnerClose();
        }
        function OnFillRequisitionDetailsFailed(error) {
            toastr.error(error.get_message());
            CommonHelper.SpinnerClose();
        }

        function PerformCancelAction(RequisitionId) {
            var doCancel = confirm("Do you want to cancel the reqistion?");
            if (doCancel)
                PageMethods.CancelRequisition(RequisitionId, OnsuccessCancel, OnfailCancel);
            return false;
        }

        function OnsuccessCancel(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                debugger;

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '/Common/WebMethodPage.aspx/GetCommonCheckByApproveByListForSMS',
                    data: JSON.stringify({ tableName: 'PMRequisition', primaryKeyName: 'RequisitionId', primaryKeyValue: result.PrimaryKeyValue, featuresValue: 'Requisition', statusColumnName: 'ApprovedStatus' }),
                    dataType: "json",
                    success: function (data) {
                        debugger;

                        SendSMSToUserList(data.d, result.PrimaryKeyValue, result.TransactionNo, result.TransactionType, result.TransactionStatus);

                    },
                    error: function (result) {
                        toastr.error("Can not load Check or Approve By List.");
                    }
                });

                $("#ContentPlaceHolder1_hfRequisitionId").val("0");
                ////PerformClearAction();
                //$("#ContentPlaceHolder1_btnSearch").trigger("click");

                if ($("#ContentPlaceHolder1_hfPageNumber").val() == "") {
                    LoadRequisitionDetails($("#GridPagingContainer").find("li.active").index(), 1);
                }
                else {
                    var pageNumber = $("#ContentPlaceHolder1_hfPageNumber").val();
                    LoadRequisitionDetails(pageNumber, 1);
                }
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            CommonHelper.SpinnerClose();
        }
        function OnfailCancel(error) {
            toastr.error(error.get_message());
        }

        function CheckInputValue(approvedTextBox) {

            if ($.trim($(approvedTextBox).val()) != "") {

                if (CommonHelper.IsDecimal($.trim($(approvedTextBox).val())) == false) {
                    toastr.warning("Please give valid quantity.");
                    $(approvedTextBox).val("");
                    return false;
                }
            }

            return false;
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadRequisitionDetails(pageNumber, IsCurrentOrPreviousPage);
        }

        function CompleteRequisitionOrder(requisitionId) {
            PageMethods.CompleteRequisitionOrder(requisitionId, OnCompleteRequisitionDetailsSucceed, OnCompleteRequisitionDetailsFailed);
        }
        function CompleteRequisitionOrderWithConfirmation(requisitionId) {
            if (!confirm("Do you Want To Complete Purchase?")) {
                return false;
            }
            CompleteRequisitionOrder(requisitionId);
        }
        function OnCompleteRequisitionDetailsSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                //----$("#ContentPlaceHolder1_btnSearch").trigger("click");
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnCompleteRequisitionDetailsFailed() {
        }
        function CompleteRequisitionTransferOrder(requisitionId) {
            PageMethods.CompleteRequisitionTransferOrder(requisitionId, OnCompleteRequisitionDetailsSucceed, OnCompleteRequisitionDetailsFailed);
        }
        function CompleteRequisitionTransferOrderWithConfirmation(requisitionId) {
            if (!confirm("Do you Want To Complete Transfer?")) {
                return false;
            }
            CompleteRequisitionTransferOrder(requisitionId);
        }
        function CheckAllOrder() {
            if ($("#OrderCheck").is(":checked")) {
                $("#DetailsRequisitionGrid tbody tr").find("td:eq(0)").find("input").prop("checked", true);
            }
            else {
                $("#DetailsRequisitionGrid tbody tr").find("td:eq(0)").find("input").prop("checked", false);
            }
        }
    </script>
    <asp:HiddenField ID="hfPageNumber" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />

    <div id="DetailsRequisitionDialog" style="display: none;">
        <div id="DetailsRequisitionGridContaiiner">
        </div>
        <div class="HMContainerRowButton" style="padding-bottom: 0; padding-top: 10px;">
            <input id="btnApprove" type="button" class="TransactionalButton btn btn-primary btn-sm"
                value="Approve Requisition" />
        </div>
    </div>

    <asp:HiddenField ID="hfIsSingle" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyAll" runat="server" />
    <asp:HiddenField ID="SingleprojectId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfRequisitionId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsRequisitionCheckedByEnable" runat="server" Value="0" />
    <div class="panel panel-default">
        <div class="panel-heading">
            Requisition Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label CssClass="control-label" runat="server" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox CssClass="form-control" ID="txtFromDate" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox CssClass="form-control" ID="txtToDate" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label CssClass="control-label" runat="server" Text="Requisition No."></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control" ID="srcRequisitionNo" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label CssClass="control-label" runat="server" Text="Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlStatus" CssClass="form-control" runat="server">
                            <asp:ListItem Value="">-- All --</asp:ListItem>
                            <asp:ListItem Value="Pending">Pending</asp:ListItem>
                            <asp:ListItem Value="Submit">Submitted</asp:ListItem>
                            <asp:ListItem Value="Partially Checked">Partially Checked</asp:ListItem>
                            <asp:ListItem Value="Checked">Checked</asp:ListItem>
                            <asp:ListItem Value="Partially Approved">Partially Approved</asp:ListItem>
                            <asp:ListItem Value="Approved">Approved</asp:ListItem>
                            <asp:ListItem Value="Cancel">Cancel</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblRequisitionBy" runat="server" class="control-label"
                            Text="Requisition By"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRequisitionBy" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>                
                    <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />
                <br />
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="return LoadRequisitionDetails(1,1);"
                            CssClass="TransactionalButton btn btn-primary btn-sm" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel"
                            CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return PerformClearAction();" />
                        <asp:Button ID="btnNewRequisition" runat="server" Text="New Requisition"
                            CssClass="TransactionalButton btn btn-primary btn-sm" OnClick="btnNewRequisition_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-heading">
                Search Information
            </div>
            <div class="panel-body">
                <div style="overflow-y: scroll;">
                    <table class="table table-bordered table-condensed table-responsive" id='tbRequisitionDetails'>
                        <colgroup>
                            <col style="width: 10%;" />
                            <col style="width: 7%;" />
                            <col style="width: 10%;" />
                            <col style="width: 10%;" />
                            <col style="width: 10%;" />
                            <col style="width: 8%;" />
                            <col style="width: 10%;" />
                            <col style="width: 10%;" />
                            <col style="width: 10%;" />
                            <col style="width: 15%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>Requisition No
                                </td>
                                <td>Date
                                </td>
                                <td>From
                                </td>
                                <td>To
                                </td>
                                <td>Requisition By
                                </td>
                                <td>Status
                                </td>
                                <td>Purchase Status
                                </td>
                                <td>Transfer Status
                                </td>
                                <td>Receive Status
                                </td>
                                <td style="text-align: center;">Action
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>

                <div class="childDivSection">
                    <div class="text-center" id="GridPagingContainer">
                        <ul class="pagination">
                        </ul>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
