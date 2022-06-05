<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="CashRequisitionOrBillVoucherInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.CashRequisitionOrBillVoucherInformation" %>

<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>

        var CashRequisitionTable;
        var GlobalDeletedId = 0;
        var GlobalCreatedBy = 0;

        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;

        $(document).ready(function () {
            CommonHelper.ApplyDecimalValidation();
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            $("#ContentPlaceHolder1_ddlAssignEmployee").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            CommonHelper.ApplyDecimalValidation();
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;

            $('#ContentPlaceHolder1_txtSearchFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                // defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchToDate').datepicker("option", "minDate", selectedDate);
                }
            });
            //}).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtSearchToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            //}).datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_ddlTransactionType").change(function () {
                if ($(this).val() == "") {

                    $("#TransactionNoDiv").show();
                    $("#AdjustmentNoDiv").show();
                }
                else if ($(this).val() == "Cash Requisition Adjustment") {
                    $("#TransactionNoDiv").hide();
                    $("#AdjustmentNoDiv").show();
                }
                else {
                    $("#TransactionNoDiv").show();
                    $("#AdjustmentNoDiv").hide();
                }

            });

            CashRequisitionTable = $("#tblCashRequisition").DataTable({
                data: [],
                columns: [
                    { title: "", "data": "Id", visible: false },
                    { title: "#", "data": "SerialNumber", sWidth: '3%' },
                    { title: "Tran. No.", "data": "TransactionNo", sWidth: '8%' },
                    //{ title: "Tran. Type", "data": "TransactionType", sWidth: '8%' },
                    { title: "Date", "data": "CreatedDate", sWidth: '5%' },
                    { title: "Employee", "data": "EmployeeName", sWidth: '15%' },
                    { title: "Amount", "data": "Amount", sWidth: '5%' },
                    { title: "Remarks", "data": "Remarks", sWidth: '15%' },
                    { title: "Status", "data": "ApprovedStatus", sWidth: '5%' },
                    { title: "Authorized By", "data": "AuthorizedByList", sWidth: '15%' },
                    { title: "Adjustment No.", "data": "AdjustmentIdList", sWidth: '8%' },
                    { title: "Remaining Day", "data": null, sWidth: '6%' },
                    { title: "Action", "data": null, sWidth: '15%' }
                ],
                columnDefs: [
                    {
                        "targets": 3,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            //debugger;
                            return CommonHelper.DateFromDateTimeToDisplay(data, innBoarDateFormat);
                        }
                    }],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var row = '';

                    if (IsCanEdit && aData.IsCanEdit && aData.ApprovedStatus != "Cancel" && (aData.ApprovedStatus == "Pending" || aData.IsCanCheck || aData.IsCanApprove)) {
                        if (aData.TransactionType == "Bill Voucher") {
                            row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformBillEdit('" + aData.Id + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";

                        }
                        else {
                            row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformEdit('" + aData.Id + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";
                        }
                    }
                    if (aData.IsCanDelete && IsCanDelete && aData.ApprovedStatus != "Cancel" && (aData.ApprovedStatus == "Pending" || aData.IsCanCheck || aData.IsCanApprove)) {
                        row += "&nbsp;&nbsp;<a href='javascript:void();' onclick= \"DeleteCashRequisition('" + aData.Id + "','" + aData.CreatedBy + "','" + aData.TransactionNo + "','" + aData.TransactionType + "');\"> <img alt='Cancel' src='../Images/delete.png' title='Cancel' /></a>";
                    }
                    if (aData.IsCanCheck && aData.ApprovedStatus != "Cancel") {
                        row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return Approval('" + aData.IsCanCheck + "','" + aData.IsCanApprove + "','" + aData.Id + "','" + aData.ApprovedStatus + "');\"><img alt=\"Check\" src=\"../Images/checked.png\" title='Check' /></a>";
                    }
                    else if (aData.IsCanApprove && aData.ApprovedStatus != "Cancel") {
                        row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return Approval('" + aData.IsCanCheck + "','" + aData.IsCanApprove + "','" + aData.Id + "','" + aData.ApprovedStatus + "');\"><img alt=\"Approve\" src=\"../Images/approved.png\" title='Approve' /></a>";

                    }

                    if (aData.TransactionType == "Cash Requisition" && aData.ApprovedStatus == "Approved") {

                        if (!aData.HaveCashRequisitionAdjustment) {
                            row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformAdustment('" + aData.Id + "');\"><img alt=\"Adjustment\" src=\"../Images/cashAdjustment.png\" title='Adjustment Required' /></a>";
                        }
                        else if (aData.RemainingAmount <= 0) {
                            row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformAdustmentDetails('" + aData.Id + "', '" + aData.CreatedBy + "');\"><img alt=\"Details\" src=\"../Images/detailsInfo.png\" title='Details'  /></a>";
                        }
                        else {
                            row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformAdustmentDetails('" + aData.Id + "', '" + aData.CreatedBy + "');\"><img alt=\"Adjustment\" src=\"../Images/cashAdjustmentDetails.png\" title='Adjustment' /></a>";
                        }
                    }
                    if (aData.TransactionType == "Cash Requisition" && aData.ApprovedStatus != "Cancel") {
                        row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return ShowCashRequisitionDocuments('" + aData.Id + "');\"> <img alt=\"Documents\" src=\"../Images/document.png\" title='Documents' /> </a>";
                    }
                    if (aData.TransactionType == "Bill Voucher" && aData.ApprovedStatus != "Cancel") {
                        row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return ShowDocuments('" + aData.Id + "');\"> <img alt=\"Documents\" src=\"../Images/document.png\" title='Documents' /> </a>";
                    }
                    if (aData.ApprovedStatus != "Cancel") {
                        row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return ShowInvoice('" + aData.Id + "');\"> <img alt=\"Invoice\" src=\"../Images/ReportDocument.png\" title='Invoice' /> </a>";
                    }
                    $('td:eq(' + (nRow.children.length - 1) + ')', nRow).html(row);
                    if (aData.TransactionType == "Cash Requisition" && aData.ApprovedStatus == "Approved" && aData.LastAdjustmentDate != null && aData.RemainingAmount > 0) {
                        var str = "";
                        if (aData.RemainingAdjustmentDay > 1) {
                            str = "" + aData.RemainingAdjustmentDay + " days.";
                        }
                        else if (aData.RemainingAdjustmentDay > 0) {
                            str = "" + aData.RemainingAdjustmentDay + " day.";
                        }
                        else if (aData.RemainingAdjustmentDay >= -1) {
                            str = "Pass : " + (-aData.RemainingAdjustmentDay) + " day.";
                        }
                        else if (aData.RemainingAdjustmentDay < -1) {
                            str = "Pass : " + (-aData.RemainingAdjustmentDay) + " days.";
                        }
                        else if (aData.RemainingAdjustmentDay == 0) {
                            str = "Today is the last day for adjustment.";
                        }

                        $('td:eq(' + (nRow.children.length - 2) + ')', nRow).html(str);
                    }
                    else {
                        $('td:eq(' + (nRow.children.length - 2) + ')', nRow).html("");
                    }

                    if (aData.HasPermissionForChild) {
                        $('td', nRow).css('background-color', '#fdda7e');
                    }
                    else if (aData.TransactionType == "Cash Requisition" && aData.ApprovedStatus == "Approved" && aData.HaveCashRequisitionAdjustment && aData.RemainingAdjustmentDay > 0 && aData.RemainingAmount > 0) {
                        $('td', nRow).css('background-color', '#cafaf0');
                    }
                    else if (aData.TransactionType == "Cash Requisition" && aData.ApprovedStatus == "Approved" && aData.RemainingAdjustmentDay <= 0 && aData.RemainingAmount > 0) {
                        $('td', nRow).css('background-color', '#f9c0b4');
                    }

                    else if (aData.TransactionType == "Cash Requisition" && aData.ApprovedStatus == "Approved" && !aData.HaveCashRequisitionAdjustment) {
                        $('td', nRow).css('background-color', '#B7C1ED');
                    }
                    else if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }
                },
                pageLength: UserInfoFromDB.GridViewPageSize,
                filter: false,
                info: false,
                ordering: false,
                processing: true,
                retrieve: true,
                bAutoWidth: false,
                bLengthChange: false,
                bInfo: false,
                bPaginate: false,
                language: {
                    emptyTable: "No Data Found"
                },

            });
            //GridPaging(1, 1);
        });

        function Approval(isCanCheck, isCanApprove, id, status) {
            var temp = "";
            var updatedStatus = "";
            if (isCanCheck == "true") {
                temp = "check";
                updatedStatus = "Checked";
            }
            else if (isCanApprove == "true") {
                temp = "approve";
                updatedStatus = "Approved";
            }

            if (confirm("Want to " + temp + "?")) {
                PageMethods.ApprovalStatusUpdate(id, updatedStatus, OnApprovalSucceed, OnFailed);
            }

            return false;
        }

        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '/Common/WebMethodPage.aspx/GetCommonCheckByApproveByListForSMS',
                    data: JSON.stringify({ tableName: 'CashRequisition', primaryKeyName: 'Id', primaryKeyValue: result.PrimaryKeyValue, featuresValue: 'CashRequisition', statusColumnName: 'ApproveStatus' }),
                    dataType: "json",
                    success: function (data) {

                        SendSMSToUserList(data.d, result.PrimaryKeyValue, result.TransactionNo, result.TransactionType, result.TransactionStatus);

                    },
                    error: function (result) {
                        toastr.error("Can not load Check or Approve By List.");
                    }
                });

                if ($("#ContentPlaceHolder1_hfPageNumber").val() == "") {
                    GridPaging($("#GridPagingContainer").find("li.active").index(), 1);
                }
                else {
                    var pageNumber = $("#ContentPlaceHolder1_hfPageNumber").val();
                    GridPaging(pageNumber, 1);
                }
            }
            return false;
        }
        function SendSMSToUserList(UserList, PrimaryKeyValue, TransactionNo, TransactionType, TransactionStatus) {
            var str = '';
            if (TransactionStatus == 'Approved') {
                str += TransactionType + ' No.(' + TransactionNo + ')  is Approved.';
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
                        //CommonHelper.AlertMessage(data.d.AlertMessage);
                    },
                    error: function (result) {
                        //alert("Error");
                    }
                });
            }

            return false;
        }

        function OnFailed(error) {
            toastr.error(error.get_message());
            return false;
        }

        function PerformDelete() {
            var reason = $("#ContentPlaceHolder1_txtReason").val();
            if (reason == "") {
                toastr.warning("Give Some Reason.");
                $("#ContentPlaceHolder1_txtReason").focus();
                return false;
            }

            PageMethods.DeleteCashRequisition(GlobalDeletedId, OnSuccessDelete, OnFailedDelete);
        }

        function PerformDeleteCancel() {
            GlobalDeletedId = 0;
            GlobalCreatedBy = 0;
            $('#ReasonDialogue').dialog('close');
        }

        function DeleteCashRequisition(id, createdBy, TransactionNo, TransactionType) {
            if (confirm("Want to Cancel?")) {
                GlobalDeletedId = id;
                GlobalCreatedBy = createdBy;

                $("#ReasonDialogue").dialog({
                    autoOpen: true,
                    modal: true,
                    width: "83%",
                    height: 400,
                    closeOnEscape: false,
                    resizable: false,
                    title: "Cancel : " + TransactionType + " ( " + TransactionNo + " ) ",
                    show: 'slide'
                });

                $("#ContentPlaceHolder1_txtTitle").val(TransactionType + " (" + TransactionNo + ") ");
                $("#ContentPlaceHolder1_txtReason").val("");
            }

            return false;
        }

        function OnSuccessDelete(result) {
            if (result.IsSuccess) {
                debugger;

                var CommonMessage = {
                    Subjects: $("#ContentPlaceHolder1_txtTitle").val() + " has been canceled.",
                    MessageBody: $("#ContentPlaceHolder1_txtReason").val()
                };

                var messageDetails = [];

                messageDetails.push({
                    MessageTo: GlobalCreatedBy
                });

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '/HMCommon/frmCommonMessage.aspx/SendMailByID',
                    data: JSON.stringify({ CMB: CommonMessage, CMD: messageDetails }),
                    dataType: "json",
                    success: function (data) {

                        CommonHelper.AlertMessage(data.d.AlertMessage);

                    },
                    error: function (result) {
                        //alert("Error");

                    }
                });

                PerformDeleteCancel();
                CommonHelper.AlertMessage(result.AlertMessage);
                //Clear();
                $("#ContentPlaceHolder1_btnSearch").trigger("click");
            }
            return false;
        }

        function OnFailedDelete(error) {
            toastr.error(error.get_message());
            return false;
        }

        function PerformEdit(Id) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }

            var iframeid = 'frmPrint';
            var url = "./CashRequisitionIframe.aspx?crid=" + Id;
            document.getElementById(iframeid).src = url;
            $("#RequsitionDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 500,
                closeOnEscape: false,
                resizable: false,
                title: "Edit Cash Requisition",
                show: 'slide'
            });
            return false;
        }

        function PerformAdustment(Id) {
            if (!confirm("Want to Adjust?")) {
                return false;
            }

            var iframeid = 'frmPrint';
            var url = "./CashRequisitionApprovalIframe.aspx?craid=" + Id + "&craeid=" + "";
            document.getElementById(iframeid).src = url;
            $("#RequsitionDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 800,
                closeOnEscape: false,
                resizable: false,
                title: "Requisition Cash Adjustment",
                show: 'slide'
            });
            return false;
        }

        function PerformAdustmentDetails(Id, CreatedBy) {
            if (!confirm("Want to Adjust?")) {
                return false;
            }

            var iframeid = 'frmPrint';
            var url = "./CashRequisitionApprovalDetailsIframe.aspx?crid=" + Id + "&cbid=" + CreatedBy;
            document.getElementById(iframeid).src = url;
            $("#RequsitionDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "90%",
                height: 800,
                closeOnEscape: false,
                resizable: false,
                title: "Requisition Cash Adjustment",
                show: 'slide'
            });
            return false;
        }
        function PerformBillEdit(Id) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }

            var iframeid = 'frmPrint';
            var url = "./BillVoucherIframe.aspx?bvid=" + Id;
            document.getElementById(iframeid).src = url;
            $("#RequsitionDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 800,
                closeOnEscape: false,
                resizable: false,
                title: "Edit Bill Voucher",
                show: 'slide'
            });
            return false;
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchInformation(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function SearchInformation(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = CashRequisitionTable.data().length;
            $("#ContentPlaceHolder1_hfPageNumber").val(pageNumber);
            var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
            if (projectId == null)
                projectId = "0";
            var employeeId = $("#ContentPlaceHolder1_ddlAssignEmployee").val();
            var transactionType = $("#ContentPlaceHolder1_ddlTransactionType").val();

            var fromDate = $("#ContentPlaceHolder1_txtSearchFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtSearchToDate").val();
            //if (fromDate == "")
            //    fromDate = new Date();
            //if (toDate == "")
            //    toDate = new Date();

            //fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');
            //toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            var fromAmount = $("#<%=txtSearchFromAmount.ClientID%>").val();
            var toAmount = $("#<%=txtSearchToAmount.ClientID%>").val();
            if (fromAmount != '' && toAmount == '') {
                toastr.warning("Enter To Amount");
                $("#<%=txtSearchToAmount.ClientID%>").focus();
                return false;
            }
            if (fromAmount == '' && toAmount != '') {
                toastr.warning("Enter From Amount");
                $("#<%=txtSearchFromAmount.ClientID%>").focus();
                return false;
            }
            var status = $("#<%=ddlSearchStatus.ClientID%>").val();
            var transactionNo = $("#<%=txtSrcTransactionNo.ClientID%>").val();
            var adjustmentNo = $("#<%=txtSrcAdjustmentNo.ClientID%>").val();
            var Remarks = $("#<%=txtRemarks.ClientID%>").val();
            PageMethods.GetCashRequisition(companyId, projectId, employeeId, transactionType, fromDate, toDate, fromAmount, toAmount, transactionNo, adjustmentNo, status, Remarks, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnCashRequisitionLoadingSucceed, OnCashRequisitioLoadingFailed);
            return false;
        }

        function OnCashRequisitionLoadingSucceed(result) {

            CashRequisitionTable.clear();
            CashRequisitionTable.rows.add(result.GridData);
            CashRequisitionTable.draw();

            $("#GridPagingContainer ul").html("");
            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
            return false;
        }

        function OnCashRequisitioLoadingFailed(error) {
            toastr.error(error.get_message());
            return false;
        }

        //function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
        //    var gridRecordsCount = $("#ContentPlaceHolder1_tblRequisitionInfo tbody tr").length;
        //    PageMethods.LoadLeaveInformation(gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLeaveInfoLoadSucceed, OnFailed);
        //    return false;
        //}
        function ShowAlert(Message, PrimaryKeyValue, TransactionNo, TransactionType, TransactionStatus) {

            CommonHelper.AlertMessage(Message);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '/Common/WebMethodPage.aspx/GetCommonCheckByApproveByListForSMS',
                data: JSON.stringify({ tableName: 'CashRequisition', primaryKeyName: 'Id', primaryKeyValue: PrimaryKeyValue, featuresValue: 'CashRequisition', statusColumnName: 'ApproveStatus' }),
                dataType: "json",
                success: function (data) {

                    SendSMSToUserList(data.d, PrimaryKeyValue, TransactionNo, TransactionType, TransactionStatus);

                },
                error: function (result) {
                    toastr.error("Can not load Check or Approve By List.");
                }
            });
        }

        function CreateNewCashRequisition() {
            var iframeid = 'frmPrint';
            var url = "./CashRequisitionIframe.aspx?crid=" + "";
            document.getElementById(iframeid).src = url;
            $("#RequsitionDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 500,
                closeOnEscape: false,
                resizable: false,
                title: "Cash Requisition",
                show: 'slide'
            });
            return false;
        }


        function CreateNewBillVoucher() {
            var iframeid = 'frmPrint';
            var url = "./BillVoucherIframe.aspx?bvid=" + "";
            document.getElementById(iframeid).src = url;
            $("#RequsitionDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 800,
                closeOnEscape: false,
                resizable: false,
                title: "Bill Voucher",
                show: 'slide'
            });
            return false;
        }
        function CloseDialog() {
            $("#RequsitionDialouge").dialog('close');
            return false;
        }

        function ShowDocuments(id) {
            PageMethods.LoadDocument(id, OnLoadDocumentSucceeded, OnLoadDocumentFailed);

            return false;
        }
        function ShowCashRequisitionDocuments(id) {
            PageMethods.LoadCashRequisitionDocument(id, OnLoadDocumentSucceeded, OnLoadDocumentFailed);

            return false;
        }
        function ShowInvoice(id) {
            var iframeid = 'frmPrint';
            var url = "../payroll/reports/CashRequsitionInvoice.aspx?Id=" + id;
            document.getElementById(iframeid).src = url;
            $("#RequsitionDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 500,
                closeOnEscape: false,
                resizable: false,
                title: "Cash Requisition Invoice",
                show: 'slide'
            });
            return false;
        }
        function OnLoadDocumentSucceeded(result) {
            $("#imageDiv").html(result);

            $("#showDocumentsDiv").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Documents",
                show: 'slide'
            });

            return false;
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }

        function AdminApprovalPanel(result) {
            $("#<%=txtApprovalTransactionNo.ClientID%>").val("");
            $("#<%=ddlAdminApprovalStatus.ClientID%>").val("0");

            $("#AdminApprovalDiv").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Admin Approval Information",
                show: 'slide'
            });

            return false;
        }

        function CloseDialogAdminApprovalPanel() {
            $("#AdminApprovalDiv").dialog('close');
            return false;
        }

        function AdminApprovalProcess() {
            var r = confirm("Do you want to continue process?");
            if (r == true) {
                var transactionNo = $("#<%=txtApprovalTransactionNo.ClientID%>").val();
                var status = $("#<%=ddlAdminApprovalStatus.ClientID%>").val();

                if (transactionNo == '') {
                    toastr.warning("Please Enter Process Number.");
                    $("#<%=txtApprovalTransactionNo.ClientID%>").focus();
                    return false;
                }
                if (status == "0") {
                    toastr.warning("Please Select Status.");
                    $("#<%=ddlAdminApprovalStatus.ClientID%>").focus();
                    return false;
                }

                PageMethods.AdminApprovalStatus(transactionNo, status, OnAdminApprovalProcessSucceed, OnAdminApprovalProcessFailed);
            }

            return false;
        }

        function OnAdminApprovalProcessSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            return false;
        }

        function OnAdminApprovalProcessFailed(error) {
            toastr.error(error.get_message());
            return false;
        }
    </script>
    <div id="showDocumentsDiv" style="display: none;">
        <div id="imageDiv"></div>
    </div>
    <asp:HiddenField ID="hfPageNumber" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <div id="RequsitionDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="ReasonDialogue" class="panel panel-default" style="display: none">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label " Text="Delete"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblReason" runat="server" class="control-label required-field" Text="Reason"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtReason" runat="server" Height="170px" TextMode="multiline" CssClass="form-control"
                            TabIndex="8">
                        </asp:TextBox>
                    </div>
                </div>
                <div class="row" id="saveBtnDiv">
                    <div class="col-md-12">
                        <input type="button" value="Cancel" style="width: 100px;" id="btnSave" onclick="PerformDelete()" class="TransactionalButton btn btn-primary btn-sm" />
                        &nbsp;
                        <input type="button" value="Close" style="width: 100px;" id="btnClear" onclick="PerformDeleteCancel()" class="TransactionalButton btn btn-primary btn-sm" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="AdminApprovalDiv" class="panel panel-default" style="display: none;">
        <div class="panel-body">
            <div class="form-horizontal">
                <div id="dvSearchAdminApprovalStatus" class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Process Number</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtApprovalTransactionNo" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Status</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlAdminApprovalStatus" CssClass="form-control" runat="server">
                            <asp:ListItem Value="0">-- Please Select --</asp:ListItem>
                            <asp:ListItem Value="Checked">Checked</asp:ListItem>
                            <asp:ListItem Value="Approved">Approved</asp:ListItem>
                            <asp:ListItem Value="Cancel">Cancel</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="Button1" runat="server" TabIndex="3" Text="Process" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return AdminApprovalProcess();" />

                    <asp:Button ID="Button2" runat="server" TabIndex="4" Text="Close" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return CloseDialogAdminApprovalPanel();" />
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Cash Requisition / Bill Voucher Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblAssignEmployee" runat="server" class="control-label" Text="Employee"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlAssignEmployee" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblTransactionType" runat="server" class="control-label" Text="Transaction Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTransactionType" CssClass="form-control" runat="server">
                            <asp:ListItem Value="">--- All ---</asp:ListItem>
                            <asp:ListItem Value="Cash Requisition">Cash Requisition</asp:ListItem>
                            <asp:ListItem Value="Bill Voucher">Bill Voucher</asp:ListItem>
                            <asp:ListItem Value="Cash Requisition Adjustment">Cash Requisition Adjustment</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="dvSearchDateTime" class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Date</label>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtSearchFromDate" placeholder="From Date" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtSearchToDate" placeholder="To Date" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Amount</label>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtSearchFromAmount" placeholder="From Amount" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtSearchToAmount" placeholder="To Amount" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div id="dvSearchStatus" class="form-group">
                    <div id="TransactionNoDiv">
                        <div class="col-md-2">
                            <label class="control-label">Transaction No</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSrcTransactionNo" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div id="AdjustmentNoDiv">
                        <div class="col-md-2">
                            <label class="control-label">Adjustment No</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSrcAdjustmentNo" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Status</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSearchStatus" CssClass="form-control" runat="server">
                            <asp:ListItem Value="">-- All --</asp:ListItem>
                            <asp:ListItem Value="Pending">Pending</asp:ListItem>
                            <asp:ListItem Value="Partially Checked">Partially Checked</asp:ListItem>
                            <asp:ListItem Value="Checked">Checked</asp:ListItem>
                            <asp:ListItem Value="Partially Approved">Partially Approved</asp:ListItem>
                            <asp:ListItem Value="Approved">Approved</asp:ListItem>
                            <%--<asp:ListItem Value="Adjustment">Adjustment</asp:ListItem>--%>
                            <asp:ListItem Value="Closed">Closed</asp:ListItem>
                            <asp:ListItem Value="Cancel">Cancel</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="dvRemarks" class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Remarks</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return SearchInformation(1,1);" />
                    <asp:Button ID="btnCreateNewCashRequisition" runat="server" TabIndex="4" Text="New Cash Requisition" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return CreateNewCashRequisition();" />
                    <asp:Button ID="btnCreateNewBillVoucher" runat="server" TabIndex="4" Text="New Bill Voucher" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return CreateNewBillVoucher();" />
                    <asp:Button ID="btnAdminApproval" runat="server" TabIndex="4" Text="Admin Approval" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return AdminApprovalPanel();" />
                </div>
            </div>
        </div>
        <div>
            <table id="tblCashRequisition" class="table table-bordered table-condensed table-responsive">
            </table>
            <div class="childDivSection">
                <div class="text-center" id="GridPagingContainer">
                    <ul class="pagination">
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#ContentPlaceHolder1_companyProjectUserControl_hfIsSingle").val() != "1") {

                if ($("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val() == "0")
                    $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val($("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val()).trigger("change");
                //$("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val($("#ContentPlaceHolder1_hfProjectId").val()).trigger("change");



            }
        });
    </script>
</asp:Content>
