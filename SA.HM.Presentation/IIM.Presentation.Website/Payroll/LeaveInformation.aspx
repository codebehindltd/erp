<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="LeaveInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.LeaveInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false, IsCanCheck = false, IsCanApprove = false;
        $(document).ready(function () {

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $('#ContentPlaceHolder1_txtSrcFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSrcFromDate').datepicker("option", "maxDate", $('#ContentPlaceHolder1_txtSrcToDate').val());
                    $('#ContentPlaceHolder1_txtSrcToDate').datepicker("option", "minDate", $('#ContentPlaceHolder1_txtSrcFromDate').val());
                },
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $('#ContentPlaceHolder1_txtSrcToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSrcFromDate').datepicker("option", "maxDate", selectedDate);
                    $('#ContentPlaceHolder1_txtSrcToDate').datepicker("option", "minDate", $('#ContentPlaceHolder1_txtSrcFromDate').val());
                },
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", $('#ContentPlaceHolder1_txtToDate').val());
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", $('#ContentPlaceHolder1_txtFromDate').val());

                    if ($('#ContentPlaceHolder1_txtToDate').val() != '')
                        $('#ContentPlaceHolder1_txtNoOfDays').val(CommonHelper.DateDifferenceInDays($('#ContentPlaceHolder1_txtFromDate').val(), $('#ContentPlaceHolder1_txtToDate').val()) + 1);
                },
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", $('#ContentPlaceHolder1_txtFromDate').val());

                    $('#ContentPlaceHolder1_txtNoOfDays').val(CommonHelper.DateDifferenceInDays($('#ContentPlaceHolder1_txtFromDate').val(), selectedDate) + 1);
                },
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            $("#ContentPlaceHolder1_ddlEmployee").select2({
                tags: "true",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlWorkHandover").select2({
                tags: "true",
                allowClear: true,
                width: "99.75%"
            });            

            $("#ContentPlaceHolder1_ddlApplicationEmployee").select2({
                tags: "true",
                allowClear: true,
                width: "99.75%"
            });

            $("#btnSearch").click(function () {
                GridPaging(1, 1);
            });

            $("#ContentPlaceHolder1_ddlStatus").change(function () {
                var action = $("#ContentPlaceHolder1_ddlStatus").val();
                if (action == "Cancel") {
                    $("#cancelDiv").show();
                }
                else {
                    $("#cancelDiv").hide();
                }
            });
            $("#btnNewApplication").click(function () {
                RequestOff("Leave Application", "");
            });
            GridPaging(1, 1);

        });
        function PerformSave() {
            var leaveInfo = new Array;
            var applicationEmployeeId = 0, leaveId = 0, leaveMode = 0;
            var leaveTypeId = 0, fromDate = "", toDate = "";
            var numberOfDays = 0, workHandover = 0, leaveStatus, description;

            applicationEmployeeId = $("#<%=ddlApplicationEmployee.ClientID %>").val();
            leaveId = $("#<%=hfLeaveId.ClientID %>").val();
            leaveTypeId = $("#<%=ddlLeaveTypeId.ClientID %>").val();
            leaveMode = $("#<%=ddlLeaveMode.ClientID %>").val();
            description = $("#<%=txtRemarks.ClientID %>").val();

            workHandover = $("#<%=ddlWorkHandover.ClientID %>").val();

            fromDate = $("#<%=txtFromDate.ClientID %>").val();
            if (fromDate != "") {
                fromDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(fromDate, innBoarDateFormat);
            }
            toDate = $("#<%=txtToDate.ClientID %>").val();
            if (toDate != "") {
                toDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(toDate, innBoarDateFormat);
            }
            numberOfDays = $("#<%=txtNoOfDays.ClientID %>").val();

            if (applicationEmployeeId == 0) {
                toastr.info("Please Provide Employee");
                return false;
            }

            if (leaveTypeId == "0") {
                toastr.warning("Please select leave type.");
                return false;
            }
            else if (leaveMode == "0") {
                toastr.warning("Please select leave mode.");
                return false;
            }
            else if (numberOfDays == "") {
                toastr.warning("Please provide no of days.");
                return false;
            }
            else if (fromDate == "") {
                toastr.warning("Please provide from date.");
                return false;
            }
            else if (toDate == "") {
                toastr.warning("Please provide to date.");
                return false;
            }
            else if (description == "") {
                toastr.warning("Please provide description");
                return false;
            }

            leaveInfo = {
                EmpId: applicationEmployeeId,
                LeaveId: leaveId,
                LeaveMode: leaveMode,
                LeaveTypeId: leaveTypeId,
                FromDate: fromDate,
                ToDate: toDate,
                NoOfDays: numberOfDays,
                WorkHandover: workHandover,
                Reason: description

            }
            PageMethods.SaveLeaveInformation(leaveInfo, OnSaveSucceed, OnFailed);
        }
        function OnSaveSucceed(result) {
            if (result.IsSuccess) {
                PerformClearClose();
                CommonHelper.AlertMessage(result.AlertMessage)

                GridPaging(1, 1);

            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage)
            }
        }
        function FillForm(Id) {
            CommonHelper.SpinnerOpen();
            PageMethods.FillForm(Id, OnFillFormSucceed, OnFailed);

            return false;
        }
        function OnFillFormSucceed(result) {
            CommonHelper.SpinnerClose();
            if (IsCanEdit) {
                $('#ContentPlaceHolder1_btnSave').show();
            } else {
                $('#ContentPlaceHolder1_btnSave').hide();
            }
            RequestOff("Leave Application - Edit", "");
            $("#ContentPlaceHolder1_hfEmployeeId").val(result.EmpId);

            $("#<%=ddlApplicationEmployee.ClientID %>").val(result.EmpId).trigger('change');
            $("#<%=ddlLeaveMode.ClientID %>").val(result.LeaveMode);
            $("#<%=ddlWorkHandover.ClientID %>").val(result.WorkHandover).trigger('change');

            if (result.LeaveStatus == 'Pending') {
                $("#<%=ddlStatus.ClientID %>").val("0");
            }
            else {
                $("#<%=ddlStatus.ClientID %>").val(result.LeaveStatus);
            }

            $("#<%=ddlLeaveTypeId.ClientID %>").val(result.LeaveTypeId);
            $("#<%=txtFromDate.ClientID %>").val(GetStringFromDateTime(result.FromDate));
            $("#<%=txtToDate.ClientID %>").val(GetStringFromDateTime(result.ToDate));

            $("#<%=txtNoOfDays.ClientID %>").val(result.NoOfDays);
            $("#<%=txtRemarks.ClientID %>").val(result.Reason);

            $("#<%=hfLeaveId.ClientID %>").val(result.LeaveId);
            $("#btnSave").val("Update");
        }
        function GetEmpLeaveInfo(leaveId) {
            PageMethods.FillForm(leaveId, OnGetEmpLeaveInfoSucceed, OnFailed);
        }
        function OnGetEmpLeaveInfoSucceed(result) {
            $("#ContentPlaceHolder1_hfEmployeeId").val(result.EmpId);
            $("#<%=ddlApplicationEmployee.ClientID %>").val(result.EmpId).trigger('change');
            $("#<%=ddlLeaveMode.ClientID %>").val(result.LeaveMode);
            $("#<%=ddlWorkHandover.ClientID %>").val(result.WorkHandover).trigger('change');

            <%--if (result.LeaveStatus == 'Pending') {
                $("#<%=ddlStatus.ClientID %>").val("0");
            }
            else {
                $("#<%=ddlStatus.ClientID %>").val(result.LeaveStatus);
            }--%>

            $("#<%=ddlLeaveTypeId.ClientID %>").val(result.LeaveTypeId);
            $("#<%=txtFromDate.ClientID %>").val(GetStringFromDateTime(result.FromDate));
            $("#<%=txtToDate.ClientID %>").val(GetStringFromDateTime(result.ToDate));

            $("#<%=txtNoOfDays.ClientID %>").val(result.NoOfDays);
            $("#<%=txtRemarks.ClientID %>").val(result.Reason);

            $("#<%=hfLeaveId.ClientID %>").val(result.LeaveId);
        }
        function ApproveLeaveApp(leaveId, status) {
            if (!confirm("Do you Want To Update Status?")) {
                return false;
            }
            //FillForm(leaveId);
            RequestOff("Leave Application Approval", status);
            GetEmpLeaveInfo(leaveId);
            PerformDisable();
            $("#ApprovalDiv").show();
            $("#saveBtnDiv ").hide();
        }
        function Approval() {
            var action = $("#<%=ddlStatus.ClientID %>").val();
            var cnacelReason = $("#<%=txtCancelReason.ClientID %>").val();
            var leaveId = $("#ContentPlaceHolder1_hfLeaveId").val();
            if (action == "0") {
                toastr.warning("Please Provide Status.");
                return false;
            }
            if (action == "Cancel" && cnacelReason == "") {
                toastr.warning("Please Provide Reason to Cancel.");
                return false;
            }
            PageMethods.GetLeaveApproval(leaveId, action, cnacelReason, OnApprovalSucceed, OnFailed);
            return false;
        }
        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                GridPaging(1, 1);
                PerformEnable();
                PerformClearClose();
                $("#ApprovalDiv").hide();
                $("#saveBtnDiv ").show();

            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);

            }
        }
        function PerformDisable() {
            $("#<%=ddlApplicationEmployee.ClientID %>").attr("disabled", true);
            $("#<%=ddlLeaveMode.ClientID %>").attr("disabled", true);
            $("#<%=ddlWorkHandover.ClientID %>").attr("disabled", true);
            $("#<%=ddlLeaveTypeId.ClientID %>").attr("disabled", true);

            $("#<%=txtFromDate.ClientID %>").attr("disabled", true);
            $("#<%=txtToDate.ClientID %>").attr("disabled", true);

            $("#<%=txtNoOfDays.ClientID %>").attr("disabled", true);
            $("#<%=txtRemarks.ClientID %>").attr("disabled", true);
        }
        function PerformEnable() {
            $("#<%=ddlApplicationEmployee.ClientID %>").attr("disabled", false);
            $("#<%=ddlLeaveMode.ClientID %>").attr("disabled", false);
            $("#<%=ddlWorkHandover.ClientID %>").attr("disabled", false);
            $("#<%=ddlLeaveTypeId.ClientID %>").attr("disabled", false);

            $("#<%=txtFromDate.ClientID %>").attr("disabled", false);
            $("#<%=txtToDate.ClientID %>").attr("disabled", false);

            $("#<%=txtNoOfDays.ClientID %>").attr("disabled", false);
            $("#<%=txtRemarks.ClientID %>").attr("disabled", false);

        }
        function ViewInvoice(leaveId) {
            var url = "/Payroll/Reports/frmReportLeaveApplication.aspx?LeaveId=" + leaveId;
            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
        }
        function PerformClearClose() {
            $('#NewEntryPanel').dialog('close');
            PerformEnable();
            $("#ApprovalDiv").hide();
            $("#saveBtnDiv ").show();
            $("#ContentPlaceHolder1_hfEmployeeId").val("0");

            //$("#<%=ddlLeaveMode.ClientID %>").val("0");
            $("#<%=ddlStatus.ClientID %>").val("0");
            $("#<%=txtCancelReason.ClientID %>").val("");

            $("#<%=ddlLeaveTypeId.ClientID %>").val("0");

            $("#<%=txtFromDate.ClientID %>").val("");
            $("#<%=txtToDate.ClientID %>").val("");

            $("#<%=txtNoOfDays.ClientID %>").val("");
            $("#<%=txtRemarks.ClientID %>").val("");

            $("#<%=hfLeaveId.ClientID %>").val("0");

            var drop = document.getElementById("ContentPlaceHolder1_ddlStatus");
            for (var i = 0; i < drop.length; i++) {
                drop.options[i].hidden = false;
            }

            var isVisible = $("#<%=hfIsEmpListVisible.ClientID %>").val();
            if (isVisible == "1") {
                $("#<%=ddlApplicationEmployee.ClientID %>").val("0").trigger('change');
            }

            $("#btnSave").val("Save");
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var fromDate = "", toDate = "";
            fromDate = $("#<%=txtSrcFromDate.ClientID %>").val();
            toDate = $("#<%=txtSrcToDate.ClientID %>").val();
            var empId = $("#<%=ddlEmployee.ClientID %>").val();
            var searchStatus = $("#<%=ddlSearchStatus.ClientID %>").val();
            var isAdminUser = $("#<%=hfIsAdminUser.ClientID %>").val();
            var gridRecordsCount = $("#ContentPlaceHolder1_tblLeaveInfo tbody tr").length;
            PageMethods.LoadLeaveInformation(isAdminUser, fromDate, toDate, empId, searchStatus, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLeaveInfoLoadSucceed, OnFailed);
            return false;
        }
        function OnLeaveInfoLoadSucceed(result) {
            var rowLength = $("#tblLeaveInfo tbody tr").length;
            var dataLength = result.length;
            $("#tblLeaveInfo tbody").empty();
            $("#GridPagingContainer ul").empty();
            i = 0;

            if (dataLength == 0) {
                $("#SearchPanel").hide();
            }
            else {
                $("#SearchPanel").show();
            }
            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"8\" >No Data Found</td> </tr>";
                $("#tblLeaveInfo tbody ").append(emptyTr);
                return false;
            }
            if (result.GridData != "") {


                $.each(result.GridData, function (count, gridObject) {
                    var tr = "", editLink = "", deleteLink = "", invoiceLink = "", approvalLink = "", poLink = "", poButton = "";

                    if (i % 2 == 0) {
                        tr = "<tr style='background-color:#FFFFFF;'>";
                    }
                    else {
                        tr = "<tr style='background-color:#E3EAEB;'>";
                    }

                    tr += "<td style='width:20%;'>" + (gridObject.EmployeeName == null ? "" : (gridObject.EmpCode + " - " + gridObject.EmployeeName)) + "</td>";
                    tr += "<td style='width:10%;'>" + (gridObject.TypeName == null ? "" : gridObject.TypeName) + "</td>";
                    //tr += "<td style='width:10%;'>" + gridObject.LeaveMode + "</td>";                    
                    tr += "<td style='width:10%;'>" + gridObject.CreatedDateString + "</td>";
                    tr += "<td style='width:10%;'>" + GetStringFromDateTime(gridObject.FromDate) + "</td>";
                    tr += "<td style='width:10%;'>" + GetStringFromDateTime(gridObject.ToDate) + "</td>";
                    tr += "<td style='width:7%;'>" + gridObject.NoOfDays + "</td>";
                    tr += "<td style='width:8%;'>" + gridObject.LeaveStatus + "</td>";
                    if (gridObject.LeaveStatus == "Pending" && gridObject.IsCanEdit) {
                        editLink = "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return FillForm('" + gridObject.LeaveId + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";
                    }
                    if (gridObject.IsCanCheck || gridObject.IsCanApprove) {
                        approvalLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return ApproveLeaveApp('" + gridObject.LeaveId + "','" + gridObject.LeaveStatus + "');\"><img alt=\"Approve\" src=\"../Images/detailsInfo.png\" title='Details' /></a>";
                    }
                    invoiceLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return ViewInvoice('" + gridObject.LeaveId + "');\"><img alt=\"Invoice\" src=\"../Images/ReportDocument.png\" title='Report' /></a>";

                    tr += "<td align='center' style=\"width:15%;\">" + editLink + deleteLink + approvalLink + invoiceLink + "</td>";
                    tr += "</tr>";

                    $("#tblLeaveInfo tbody").append(tr);
                    tr = "";
                    editLink = "";
                    deleteLink = "";
                    approvalLink = "";
                    poLink = "";

                });
                $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
                $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
                $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
            }
        }
        function RequestOff(msg, status) {
            $("#NewEntryPanel").show();

            $("#NewEntryPanel").dialog({
                autoOpen: true,
                dialogClass: 'no-close',
                modal: false,
                width: 800,
                height: 550,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                open: function (event, ui) {
                    $('#NewEntryPanel').css('overflow', 'hidden');
                    $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                },
                title: msg,
                show: 'slide'
            });
            var drop = document.getElementById("ContentPlaceHolder1_ddlStatus");
            $("#<%=ddlStatus.ClientID %>").val(0);
            if (status == "Pending" || status == "Partially Checked") {
                if (drop.options[2].value == "Approved") {
                    drop.options[2].hidden = true;
                }
            }
            else if (status == "Checked" || status == "Partially Approved") {
                if (drop.options[1].value == "Checked") {
                    drop.options[1].hidden = true;
                }
            }
            else {
                $("#ApprovalDiv").hide;
            }
            return false;

        }
        function OnFailed(error) {

        }
    </script>
    <asp:HiddenField ID="hfLeaveId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfEmployeeId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsEmpListVisible" runat="server" Value="0" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsAdminUser" runat="server" Value="0" />
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Leave Information
            <input style="float: right; padding: 0px 20px 0px 20px;" id="btnNewApplication" type="button" class="TransactionalButton btn btn-primary btn-sm"
                value="Leave Application" />
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group" id="employeeDiv">
                    <div class="col-md-2">
                        <asp:Label ID="Label4" runat="server" class="control-label" Text="Employee"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Search Date"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtSrcFromDate" runat="server" Placeholder="From Date" CssClass="form-control" TabIndex="6"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtSrcToDate" runat="server" Placeholder="To Date" CssClass="form-control" TabIndex="7"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label5" runat="server" class="control-label" Text="Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSearchStatus" CssClass="form-control" runat="server">
                            <asp:ListItem Value="">-- All --</asp:ListItem>
                            <asp:ListItem Value="Pending">Pending</asp:ListItem>
                            <asp:ListItem Value="Partially Checked">Partially Checked</asp:ListItem>
                            <asp:ListItem Value="Checked">Checked</asp:ListItem>
                            <asp:ListItem Value="Partially Approved">Partially Approved</asp:ListItem>
                            <asp:ListItem Value="Approved">Approved</asp:ListItem>
                            <asp:ListItem Value="Cancel">Cancel</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <input type="button" name="Search" value="Search" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information           
        </div>
        <div class="panel-body">
            <div style="overflow-y: scroll;">
                <table class="table table-bordered table-condensed table-responsive" id='tblLeaveInfo'>
                    <thead>
                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                            <td style="width: 20%;">Employee
                            </td>
                            <td style="width: 10%;">Leave Type
                            </td>
                            <%--<td style="width: 10%;">Leave Mode
                            </td>--%>
                            <td style="width: 10%;">App. Date
                            </td>
                            <td style="width: 10%;">From Date
                            </td>
                            <td style="width: 10%;">To Date
                            </td>
                            <td style="width: 7%;">No of Days
                            </td>
                            <td style="width: 8%;">Leave Status
                            </td>
                            <td style="width: 15%; text-align: center;">Action
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
    <div id="NewEntryPanel" class="panel panel-default" style="display: none">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label6" runat="server" class="control-label required-field" Text="Employee"></asp:Label>
                    </div>
                    <div class="col-md-10" style="padding-bottom: 5px;">
                        <asp:DropDownList ID="ddlApplicationEmployee" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label runat="server" class="control-label required-field" Text="Leave Type"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlLeaveTypeId" runat="server" CssClass="form-control"
                            TabIndex="4">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2" style="display: none;">
                        <asp:Label runat="server" class="control-label required-field" Text="Leave Mode"></asp:Label>
                    </div>
                    <div class="col-md-4" style="display: none;">
                        <asp:DropDownList ID="ddlLeaveMode" runat="server" CssClass="form-control"
                            TabIndex="5">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label required-field" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" autocomplete="off" TabIndex="6"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label required-field" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" autocomplete="off" TabIndex="7"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblNoOfDays" runat="server" class="control-label required-field" Text="No Of Days"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtNoOfDays" runat="server" CssClass="form-control" TabIndex="8"
                            ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label runat="server" class="control-label" Text="Work Handover"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlWorkHandover" runat="server" CssClass="form-control"
                            TabIndex="5">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Description"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtRemarks" runat="server" Height="170px" TextMode="multiline" CssClass="form-control"
                            TabIndex="8">
                        </asp:TextBox>
                    </div>
                </div>
                <div id="ApprovalDiv" style="display: none">
                    <div class="form-group" id="statusDiv">
                        <div class="col-md-2">
                            <asp:Label ID="Label7" runat="server" class="control-label" Text="Leave Status"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"
                                TabIndex="5">
                                <asp:ListItem Value="0">-- Please Select --</asp:ListItem>
                                <asp:ListItem Value="Checked">Check</asp:ListItem>
                                <asp:ListItem Value="Approved">Approve</asp:ListItem>
                                <asp:ListItem Value="Cancel">Cancel</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group" id="cancelDiv" style="display: none">
                        <div class="col-md-2">
                            <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Cancel Reason"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtCancelReason" Height="90px" runat="server" TextMode="multiline" CssClass="form-control"
                                TabIndex="8">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <input type="button" value="Save" style="width: 100px;" id="btnApprove" onclick="Approval()" class="TransactionalButton btn btn-primary btn-sm" />
                            &nbsp;
                        <input type="button" value="Close" style="width: 100px;" id="btnClose" onclick="PerformClearClose()" class="TransactionalButton btn btn-primary btn-sm" />
                        </div>
                    </div>
                </div>
                <div class="row" id="saveBtnDiv">
                    <div class="col-md-12">
                        <input type="button" value="Save" style="width: 100px;" id="btnSave" onclick="PerformSave()" class="TransactionalButton btn btn-primary btn-sm" />
                        &nbsp;
                        <input type="button" value="Close" style="width: 100px;" id="btnClear" onclick="PerformClearClose()" class="TransactionalButton btn btn-primary btn-sm" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
