﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="SupportCallInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.SupportAndTicket.SupportCallInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var ClientSelected = null;
        var SupportCallInformationTable;
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;

        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;

            SupportCallInformationTable = $("#tblSupportCallInformation").DataTable({
                data: [],
                columns: [
                    { title: "", "data": "Id", visible: false },
                    { title: "Ticket Number", "data": "CaseNumber", sWidth: '7%' },
                    { title: "Date", "data": "CreatedDate", sWidth: '6%' },
                    { title: "Case", "data": "CaseName", sWidth: '20%' },
                    { title: "Client", "data": "CompanyName", sWidth: '15%' },
                    { title: "Ticket Status", "data": "SupportStatus", sWidth: '5%' },
                    { title: "Bill Status", "data": "BillStatus", sWidth: '5%' },
                    { title: "Imp. Status", "data": "TaskStatus", sWidth: '5%' },
                    { title: "Created By", "data": "CreatedByName", sWidth: '7%' },
                    { title: "Pass Day", "data": "PassDay", sWidth: '4%' },
                    { title: "Action", "data": null, sWidth: '15%' }
                ],
                columnDefs: [
                    {
                        "targets": 2,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            //debugger;
                            return CommonHelper.DateFromDateTimeToDisplay(data, innBoarDateFormat);
                        }
                    }],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var row = '';
                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }

                    var isAllOptionDilabled = "0";
                    if (aData.SupportStatus == "Decline") {
                        isAllOptionDilabled = "1";
                    }

                    //if (isAllOptionDilabled == "0") {
                    //    if (aData.SupportStatus == "Closed") {
                    //        isAllOptionDilabled = "1";
                    //    }
                    //}

                    if (IsCanEdit) {
                        if (aData.SupportStatus != "Done") {
                            if (isAllOptionDilabled == "0") {
                                row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformEditOperation('" + aData.Id + "', '" + aData.CaseNumber + "', '" + aData.TaskId + "', '" + "New" + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";
                                row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformEditOperation('" + aData.Id + "', '" + aData.CaseNumber + "', '" + aData.TaskId + "', '" + "Details" + "');\"> <img alt=\"Edit Details\" src=\"../Images/detailsInfo.png\" title='Edit Details' /> </a>";
                                row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformEditOperationForFeedback('" + aData.Id + "', '" + aData.CaseNumber + "');\"> <img alt=\"Feedback\" src=\"../Images/remarksadd.png\" title='Feedback' /> </a>";
                            }
                            else {
                                row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformEditOperation('" + aData.Id + "', '" + aData.CaseNumber + "', '" + aData.TaskId + "', '" + "New" + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";
                            }
                        }
                    }

                    if (IsCanDelete && (aData.BillStatus == "Pending" || aData.BillStatus == "") && (aData.TaskStatus == "Pending")) {
                        if (aData.SupportStatus != "Done") {
                            if (isAllOptionDilabled == "0") {
                                row += "&nbsp;&nbsp;<a href='javascript:void();' onclick= \"DeleteSupportCallInformation('" + aData.Id + "', '" + aData.CaseNumber + "', '" + aData.CreatedBy + "');\"> <img alt='Cancel' src='../Images/delete.png' title='Cancel' /></a>";
                            }
                        }
                    }

                    row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformTicketView('" + aData.Id + "');\"> <img alt=\"Ticket\" src=\"../Images/ReportDocument.png\" title='Ticket' /> </a>";

                    if (IsCanEdit && aData.BillStatus == 'Generate') {
                        if (isAllOptionDilabled == "0") {
                            row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformBillPreviewAction('" + aData.Id + "');\"> <img alt=\"Preview Bill\" src=\"../Images/ReportDocument.png\" title='Preview Bill' /> </a>";
                        }
                    }

                    row += '&nbsp;&nbsp;<a href="javascript:void();" onclick= "javascript:return ShowDocuments(' + aData.Id + ');" title="Documents"><img style="width:16px;height:16px;" alt="Documents" src="../Images/document.png" /></a>';

                    if (aData.SupportStatus == "Done") {
                        $('td:eq(' + (nRow.children.length - 1) + ')', nRow).html(row);
                    }
                    else {
                        $('td:eq(' + (nRow.children.length - 1) + ')', nRow).html(row);
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
            $("#ContentPlaceHolder1_txtClientName").autocomplete({
                minLength: 3,
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SupportAndTicket/SupportCallInformation.aspx/ClientSearch',
                        data: JSON.stringify({ searchTerm: request.term }),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.CompanyName,
                                    value: m.CompanyId,
                                    CompanyId: m.CompanyId
                                };
                            });
                            response(searchData);
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                },
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field

                    ClientSelected = ui.item;

                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfClientId").val(ui.item.value);
                }
            });
            $("#ContentPlaceHolder1_ddlSupportStage").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlSupportCategory").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlCase").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlCaseOwner").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $('#ContentPlaceHolder1_txtSearchFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                // defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchToDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtSearchToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });
        });

        function ClearSupportCallInformation() {
            $("#ContentPlaceHolder1_txtClientName").val("");
            $("#ContentPlaceHolder1_hfClientId").val("0");
            $("#ContentPlaceHolder1_ddlCase").val('0').trigger('change');
            $("#ContentPlaceHolder1_txtCaseNumber").val("");

            $("#ContentPlaceHolder1_txtSearchFromDate").val(DayOpenDate);
            $("#ContentPlaceHolder1_txtSearchToDate").val(DayOpenDate);

            return false;
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchInformation(pageNumber, IsCurrentOrPreviousPage);
        }

        function SearchInformation(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = SupportCallInformationTable.data().length;

            var clientName = $("#ContentPlaceHolder1_txtClientName").val();
            var clientId = $("#ContentPlaceHolder1_hfClientId").val();
            var caseId = $("#ContentPlaceHolder1_ddlCase").val();
            var caseNumber = $("#ContentPlaceHolder1_txtCaseNumber").val();
            var fromDate = $("#ContentPlaceHolder1_txtSearchFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtSearchToDate").val();
            var caseStatus = $("#ContentPlaceHolder1_ddlCaseStatus").val();
            var billStatus = $("#ContentPlaceHolder1_ddlBillStatus").val();
            var impStatus = $("#ContentPlaceHolder1_ddlImpStatus").val();

            if (clientName == "") {
                clientId = 0;
            }

            PageMethods.GetSupportCallInformation(caseStatus, billStatus, impStatus, clientId, caseId, caseNumber, fromDate, toDate, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSupportCallInformationLoadingSucceed, OnSupportCallInformationLoadingFailed);
            return false;
        }
        function ShowAlert(Message) {

            CommonHelper.AlertMessage(Message);
        }
        function OnSupportCallInformationLoadingSucceed(result) {
            SupportCallInformationTable.clear();
            SupportCallInformationTable.rows.add(result.GridData);
            SupportCallInformationTable.draw();

            $("#GridPagingContainer ul").html("");
            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
            return false;
        }

        function OnSupportCallInformationLoadingFailed(error) {
            toastr.error(error.get_message());
            return false;
        }

        function PerformTicketView(Id) {
            var type = "STicket";
            var url = "/SupportAndTicket/Reports/frmSupportAndTicketReport.aspx?TId=" + Id + "," + type;
            var popup_window = "Support Ticket";
            window.open(url, popup_window, "width=760,height=780,left=300,top=50,resizable=yes");
        }

        function PerformBillPreviewAction(billId) {
            var url = "";
            var popup_window = "Print Preview";
            url = "/SupportAndTicket/Reports/frmSTBillInfo.aspx?billID=" + billId;
            window.open(url, popup_window, "width=750,height=680,left=300,top=50,resizable=yes");
        }

        function PerformEditOperation(Id, CaseNumber, TaskId, SpportCallType) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }
            //debugger;

            var iframeid = 'frmPrint';
            var url = "./SupportCallIframe.aspx?sc=" + Id + "&tid=" + TaskId + "&sct=" + SpportCallType;
            document.getElementById(iframeid).src = url;
            $("#SupportCallDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "95%",
                height: 700,
                closeOnEscape: false,
                resizable: false,
                title: "Edit Ticket : " + CaseNumber,
                show: 'slide'
            });
            return false;
        }

        function PerformEditOperationForFeedback(Id, CaseNumber) {
            if (!confirm("Want to feedback?")) {
                return false;
            }
            //debugger;

            var iframeid = 'frmPrint';
            var url = "./SupportCallCenterFeedbackIframe.aspx?sc=" + Id;
            document.getElementById(iframeid).src = url;
            $("#SupportCallDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 500,
                closeOnEscape: false,
                resizable: false,
                title: "Feedback Support Call : " + CaseNumber,
                show: 'slide'
            });
            return false;
        }

        function DeleteSupportCallInformation(Id, CaseNumber, createdBy) {
            if (!confirm("Want to delete " + CaseNumber + " ?")) {
                return false;
            }
            //debugger;

            PageMethods.DeleteSupportCallInformation(Id, OnSuccessDelete, OnFailedDelete);

            return false;
        }
        function OnSuccessDelete(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                //Clear();
                SearchInformation(1, 1);
            }
        }
        function OnFailedDelete(error) {
            toastr.error(error.get_message());
            return false;
        }

        function CreateNewSupportCall() {
            var iframeid = 'frmPrint';
            var url = "./SupportCallIframe.aspx?sc=" + "&sct=" + "New";
            document.getElementById(iframeid).src = url;
            $("#SupportCallDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "95%",
                height: 700,
                closeOnEscape: false,
                resizable: false,
                title: "New Ticket Information",
                show: 'slide'
            });
            return false;
        }
        function CloseDialog() {
            $("#SupportCallDialouge").dialog('close');
            return false;
        }
        function ShowDocuments(id) {
            PageMethods.LoadVoucherDocumentById(id, OnLoadDocumentByIdSucceeded, OnLoadDocumentByIdFailed);
            return false;
        }
        function OnLoadDocumentByIdSucceeded(result) {
            $("#imageDiv").html(result);

            $("#voucherDocuments").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                minHeight: 400,
                closeOnEscape: true,
                resizable: false,
                title: "Support Documents",
                show: 'slide'
            });

            return false;
        }

        function OnLoadDocumentByIdFailed(error) {
            toastr.error(error.get_message());
        }
        function TicketUnapprovalPanel(result) {
            $("#<%=txtApprovalTransactionNo.ClientID%>").val("");

            $("#AdminApprovalDiv").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Ticket Unapproval Information",
                show: 'slide'
            });

            return false;
        }

        function CloseDialogTicketUnapprovalPanel() {
            $("#AdminApprovalDiv").dialog('close');
            return false;
        }
        function AdminApprovalProcess() {
            var r = confirm("Do you want to continue Voucher Unapproval?");
            if (r == true) {
                var transactionNo = $("#<%=txtApprovalTransactionNo.ClientID%>").val();
                
                var status = "Pending";

                if (transactionNo == '') {
                    toastr.warning("Please Enter Ticket Number.");
                    $("#<%=txtApprovalTransactionNo.ClientID%>").focus();
                    return false;
                }

                PageMethods.AdminApprovalStatus(transactionNo, status, OnAdminApprovalProcessSucceed, OnAdminApprovalProcessFailed);
            }

            return false;
        }

        function OnAdminApprovalProcessSucceed(result) {
            toastr.success("Ticket Unapprove Successfull.");
            return false;
        }

        function OnAdminApprovalProcessFailed(error) {
            toastr.error(error.get_message());
            return false;
        }
    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <div id="voucherDocuments" style="display: none;">
        <div id="imageDiv" style="overflow: auto; height: 500px;"></div>
    </div>
    <div id="AdminApprovalDiv" class="panel panel-default" style="display: none;">
        <div class="panel-body">
            <div class="form-horizontal">                
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Ticket Number</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtApprovalTransactionNo" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="Button2" runat="server" TabIndex="3" Text="Unapprove" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return AdminApprovalProcess();" />
                    <asp:Button ID="Button3" runat="server" TabIndex="4" Text="Close" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return CloseDialogTicketUnapprovalPanel();" />
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Ticket Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <asp:HiddenField ID="hfClientId" runat="server" Value="0" />
                <div id="SupportCallDialouge" style="display: none;">
                    <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
                        clientidmode="static" scrolling="yes"></iframe>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Client Name</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtClientName" runat="server" placeholder="Enter minimum 3 characters" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Case</label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList runat="server" ID="ddlCase" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="dvSearchDateTime" class="form-group">
                    <div class="col-md-2 ">
                        <label class="control-label ">From Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchFromDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-2 ">
                        <label class="control-label">To Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchToDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Ticket Number</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCaseNumber" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Bill Status</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList runat="server" ID="ddlBillStatus" CssClass="form-control">
                            <asp:ListItem Text="--- ALL ---" Value=""></asp:ListItem>
                            <asp:ListItem Text="Full Payment" Value="Full"></asp:ListItem>
                            <asp:ListItem Text="Partial Payment" Value="Partial"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Ticket Status</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList runat="server" ID="ddlCaseStatus" CssClass="form-control">
                            <asp:ListItem Text="--- ALL ---" Value=""></asp:ListItem>
                            <asp:ListItem Text="Done" Value="Done"></asp:ListItem>
                            <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Imp. Status</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList runat="server" ID="ddlImpStatus" CssClass="form-control">
                            <asp:ListItem Text="--- ALL ---" Value=""></asp:ListItem>
                            <asp:ListItem Text="Done" Value="Done"></asp:ListItem>
                            <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div style="display: none">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Contact Name</label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtContactName" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Contact Number</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtContactNumber" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label">Contact Email</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtContactEmail" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Ticket Owner</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList runat="server" ID="ddlCaseOwner" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Support Stage</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList runat="server" ID="ddlSupportStage" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label">Support Category</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList runat="server" ID="ddlSupportCategory" CssClass="form-control">
                            </asp:DropDownList>
                        </div>

                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return SearchInformation(1,1);" />
                        <asp:Button ID="btnClear" runat="server" TabIndex="3" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return ClearSupportCallInformation();" />
                        <asp:Button ID="btnNewSupportCall" runat="server" TabIndex="4" Text="New Ticket" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateNewSupportCall();" />
                        <asp:Button ID="btnAdminApproval" runat="server" TabIndex="4" Text="Ticket Unapproval" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return TicketUnapprovalPanel();" />
                    </div>
                </div>
                <div>
                    <table id="tblSupportCallInformation" class="table table-bordered table-condensed table-responsive">
                    </table>
                    <div class="childDivSection">
                        <div class="text-center" id="GridPagingContainer">
                            <ul class="pagination">
                            </ul>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
