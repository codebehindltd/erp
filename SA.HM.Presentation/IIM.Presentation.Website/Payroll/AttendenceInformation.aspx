<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="AttendenceInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.AttendenceInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#btnNewApplication").click(function () {
                AttendanceApplication("Attendance Application", "");
            });

            $('#ContentPlaceHolder1_txtEntryHour').timepicker({
                showPeriod: is12HourFormat
            });
            $('#ContentPlaceHolder1_txtExitHour').timepicker({
                showPeriod: is12HourFormat
            });
            $("#ContentPlaceHolder1_txtAttendanceDate").datepicker({
                changeMonth: true,
                changeYear: true,
                maxDate: DayOpenDate + 1,
                dateFormat: innBoarDateFormat
            });
            $("#<%=txtAttendanceDate.ClientID %>").blur(function () {
                var date = $("#<%=txtAttendanceDate.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtAttendanceDate.ClientID %>").focus();
                        $("#<%=txtAttendanceDate.ClientID %>").val("");
                        return false;
                    }
                }

            });

            GridPaging(1, 1);

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

            $('#ContentPlaceHolder1_txtTime').timepicker({
                showPeriod: is12HourFormat
            });
            $('#ContentPlaceHolder1_txtDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", $('#ContentPlaceHolder1_txtToDate').val());
                },
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
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

            $("#ContentPlaceHolder1_ddlEmployee").select2({
                tags: "true",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlApplicationEmployee").select2({
                tags: "true",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlApplicationEmployee").change(function () {
                PerformAttendanceApplicationFillFormAction();
            });

            $("#ContentPlaceHolder1_txtAttendanceDate").change(function () {
                PerformAttendanceApplicationFillFormAction();
            });

            <%--var isVisible = $("#<%=hfIsEmpListVisible.ClientID %>").val();
            if (isVisible == "0") {
                $("#employeeDiv").hide();
            }
            else {
                $("#employeeDiv").show();
            }--%>

        })
        function AttendanceApplication(msg, status) {
            $("#AttendanceApplication").show();

            $("#AttendanceApplication").dialog({
                autoOpen: true,
                dialogClass: 'no-close',
                modal: false,
                width: 800,
                height: 500,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                open: function (event, ui) {
                    $('#AttendanceApplication').css('overflow', 'hidden');
                    $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                },
                title: msg,
                show: 'slide'
            });
            return false;

        }

        //For FillForm-------------------------   
        function PerformAttendanceApplicationFillFormAction() {
            if ($("#<%=ddlApplicationEmployee.ClientID %>").val() == 0) {
                toastr.info("Please Provide Employee");
                return false;
            }

            if ($("#<%=txtAttendanceDate.ClientID %>").val() == "") {
                toastr.info("Please Provide Attendance Date");
                return false;
            }

            var empId = $("#<%=ddlApplicationEmployee.ClientID %>").val();
            var attendanceDate = $("#<%=txtAttendanceDate.ClientID %>").val();
            PageMethods.AttendanceApplicationFillForm(empId, attendanceDate, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {
            $("#<%=hfAttendanceId.ClientID %>").val(result.AttendanceId);
            //$("#<%=txtAttendanceDate.ClientID %>").val(result.stringAttendenceDate);
            $("#<%=txtEntryHour.ClientID %>").val(result.StartHour);
            $("#<%=txtExitHour.ClientID %>").val(result.EndHour);
            $("#<%=txtRemarks.ClientID %>").val(result.Remark);

            $('#btnSaveAttendanceApplication').show();

            if (result.AttendanceId > 0) {
                if (result.AttendenceStatus == "Pending") {
                    $('#btnSaveAttendanceApplication').show();
                } else {

                    if ($("#<%=hfIsAdminUser.ClientID %>").val() == "1") {
                        $('#btnSaveAttendanceApplication').show();
                    }
                    else {
                        $('#btnSaveAttendanceApplication').hide();
                    }
                }
            }
            return false;
        }

        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function PerformSaveAttendanceApplication() {
            var applicationEmployeeId = $("#<%=ddlApplicationEmployee.ClientID %>").val();
            var attendanceDate = $("#<%=txtAttendanceDate.ClientID %>").val();
            var entryHour = $("#<%=txtEntryHour.ClientID %>").val();
            var exitHour = $("#<%=txtExitHour.ClientID %>").val();
            var remarks = $("#<%=txtRemarks.ClientID %>").val();

            if (applicationEmployeeId == 0) {
                toastr.warning("Please Provide Employee.");
                return false;
            }

            if ($("#<%=txtAttendanceDate.ClientID %>").val() == "") {
                toastr.warning("Please Provide Attendance Date.");
                return false;
            }

            if ($("#<%=txtRemarks.ClientID %>").val() == "") {
                toastr.warning("Please Provide Remarks.");
                return false;
            }

            if (entryHour != "") {
                var isValid = IsValidTime(entryHour);
                if (!isValid) {
                    toastr.warning("Please Provide Correct Time Format.");
                    $("#<%=txtEntryHour.ClientID %>").focus();
                    return false;
                }
            }
            if (exitHour != "") {
                var isValid = IsValidTime(exitHour);
                if (!isValid) {
                    toastr.warning("Please Provide Correct Time Format.");
                    $("#<%=txtExitHour.ClientID %>").focus();
                    return false;
                }
            }
            PageMethods.PerformAttendanceSaveAction(applicationEmployeeId, attendanceDate, entryHour, exitHour, remarks, OnSaveAttendanceSucceed, OnSaveAttendanceFailed);
            return false;
        }
        function OnSaveAttendanceSucceed(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            PerformClearAttendanceApplication();
            GridPaging(1, 1);
        }
        function OnSaveAttendanceFailed(error) {
            toastr.error(error.get_message());
        }
        function IsValidTime(time) {
            var regex = /^(?:(?:0?\d|1[0-2]):[0-5]\d)+$/;

            if ((time.includes("AM")) || (time.includes("PM"))) {
                time = time.slice(0, -3);
            }

            var isValidTime = regex.test(time);

            if (!isValidTime) {
                return false;
            } else {
                return true;
            }
        }
        function PerformClearAttendanceApplication() {
            $("#<%=txtAttendanceDate.ClientID %>").val("");
            $("#<%=txtEntryHour.ClientID %>").val("");
            $("#<%=txtExitHour.ClientID %>").val("");
            $("#<%=txtRemarks.ClientID %>").val("");

            var isVisible = $("#<%=hfIsEmpListVisible.ClientID %>").val();
            if (isVisible == "1") {
                $("#<%=ddlApplicationEmployee.ClientID %>").val("0").trigger('change');
            }
        }
        function PerformAttendanceApplicationClose() {
            $('#AttendanceApplication').dialog('close');
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var fromDate = "", toDate = "";
            fromDate = $("#<%=txtFromDate.ClientID %>").val();
            toDate = $("#<%=txtToDate.ClientID %>").val();
            var empId = $("#<%=ddlEmployee.ClientID %>").val();
            var searchStatus = $("#<%=ddlSearchStatus.ClientID %>").val();
            var isAdminUser = $("#<%=hfIsAdminUser.ClientID %>").val();
            var gridRecordsCount = $("#ContentPlaceHolder1_tblAttendenceInfo tbody tr").length;
            PageMethods.LoadAttendenceInformation(isAdminUser, fromDate, toDate, empId, searchStatus, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLeaveInfoLoadSucceed, OnFailed);
            return false;
        }
        function OnLeaveInfoLoadSucceed(result) {
            var rowLength = $("#tblAttendenceInfo tbody tr").length;
            var dataLength = result.length;
            $("#tblAttendenceInfo tbody").empty();
            $("#GridPagingContainer ul").empty();
            i = 0;

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"9\" >No Data Found</td> </tr>";
                $("#tblAttendenceInfo tbody ").append(emptyTr);
                return false;
            }
            $.each(result.GridData, function (count, gridObject) {
                var tr = "", editLink = "", deleteLink = "", invoiceLink = "", approvalLink = "", leaveDetailsLink = "", poLink = "", poButton = "";

                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }
                var splitStr = gridObject.LateTime.split(':');
                var status = gridObject.AttendanceStatus == null ? "" : gridObject.AttendanceStatus;
                tr += "<td style='width:5%;'>" + gridObject.EmpCode + "</td>";
                tr += "<td style='width:17%;'>" + gridObject.DisplayName + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.Designation + "</td>";
                tr += "<td style='width:8%;'>" + GetStringFromDateTime(gridObject.AttendanceDate) + "</td>";
                tr += "<td style='width:5%;'>" + gridObject.InTime + "</td>";
                if (gridObject.LateTime == "") {
                    tr += "<td style='width:5%;'>" + "" + "</td>";
                }
                else {
                    tr += "<td style='width:5%;'>" + gridObject.LateTime + "</td>";
                }

                if (gridObject.OutTime == "") {
                    tr += "<td style='width:7%;'>" + "" + "</td>";
                }
                else {
                    tr += "<td style='width:7%;'>" + gridObject.OutTime + "</td>";
                }

                if (gridObject.LateApplicationDateDisplay == "")
                {
                    tr += "<td style='width:8%;'>" + "" + "</td>";
                }
                else
                {
                    tr += "<td style='width:8%;'>" + gridObject.LateApplicationDateDisplay + "</td>";
                }

                tr += "<td style='width:8%;'>" + status + "</td>";

                if ((gridObject.AttendanceStatus == null) && (splitStr[0] > "0" || splitStr[1] > "0")) {
                    deleteLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformLateAttenApplication('" + gridObject.AttendanceId + "');\"><img alt=\"Cancel\" src=\"../Images/remarksadd.png\" title='Late Application' /></a>";
                }
                //if (gridObject.IsCanEdit) {
                //    editLink = "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return FillForm('" + gridObject.AttendanceId + "');\"> <img alt=\"Late Application\" src=\"../Images/cashAdjustmentDetails.png\" title='Late Application' /> </a>";
                //}
                if (gridObject.IsCanCheck) {
                    approvalLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return ApproveLeaveApp('" + gridObject.EmpCode + " - " + gridObject.DisplayName + " (" + GetStringFromDateTime(gridObject.AttendanceDate) + ")" + "','" + gridObject.AttendanceId + "','" + status + "');\"><img alt=\"Approval\" src=\"../Images/quotation.png\" title='Approval' /></a>";
                }
                else if (gridObject.IsCanApprove) {
                    approvalLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return ApproveLeaveApp('" + gridObject.EmpCode + " - " + gridObject.DisplayName + " (" + GetStringFromDateTime(gridObject.AttendanceDate) + ")" + "','" + gridObject.AttendanceId + "','" + status + "');\"><img alt=\"Approval\" src=\"../Images/quotation.png\" title='Approval' /></a>";
                }

                if (gridObject.LateTime != "") {
                    if (gridObject.IsCanEdit) {
                        editLink = "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return FillForm('" + gridObject.AttendanceId + "');\"> <img alt=\"Late Application\" src=\"../Images/cashAdjustmentDetails.png\" title='Late Application' /> </a>";
                    }
                    invoiceLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return ViewInvoice('" + gridObject.AttendanceId + "');\"><img alt=\"Invoice\" src=\"../Images/ReportDocument.png\" title='Late Application Report' /></a>";
                    leaveDetailsLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return ViewLeaveDetails('" + gridObject.EmpCode + " - " + gridObject.DisplayName + " (" + GetStringFromDateTime(gridObject.AttendanceDate) + ")" + "','" + gridObject.AttendanceId + "','" + status + "');\"><img alt=\"Approve\" src=\"../Images/detailsInfo.png\" title='Details' /></a>";
                }



                tr += "<td align='center' style=\"width:10%;\">" + editLink + deleteLink + approvalLink + leaveDetailsLink + invoiceLink + "</td>";
                tr += "</tr>";

                $("#tblAttendenceInfo tbody").append(tr);
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
        function ViewInvoice(Id) {
            var url = "/Payroll/Reports/LateAttendenceApplication.aspx?aId=" + Id;
            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
        }
        function PerformLateAttenApplication(attenId) {
            GetEmpAttendenceInfo(attenId);
            $("#btnSave").val("Save");
            RequestOff("Late Attendence Application", "");
            $("#<%=txtTime.ClientID %>").attr("disabled", true);
            $("#<%=txtDate.ClientID %>").attr("disabled", true);
        }
        function GetEmpAttendenceInfo(attenId) {
            PageMethods.FillForm(attenId, OnGetEmpAttendenceInfoSucceed, OnFailed);
        }
        function OnGetEmpAttendenceInfoSucceed(result) {
            $("#<%=txtTime.ClientID %>").val(result.StartHour);
            $("#<%=txtDate.ClientID %>").val(result.stringAttendenceDate);
            $("#<%=txtDescription.ClientID %>").val(result.Remark);
            if (result.AttendenceStatus == 'Pending') {
                $("#<%=ddlStatus.ClientID %>").val("0");
            }
            else {
                $("#<%=ddlStatus.ClientID %>").val(result.AttendenceStatus);
            }
            $("#<%=hfAttendanceId.ClientID %>").val(result.AttendanceId);

            $("#<%=txtCancelReason.ClientID %>").val(result.CancelReason);
        }
        function PerformSave() {
            var date = "", time = "", description = "", id = 0;
            var attendenceBO = new Array();
            id = $("#<%=hfAttendanceId.ClientID %>").val();
            date = $("#<%=txtDate.ClientID %>").val();
            time = $("#<%=txtTime.ClientID %>").val();
            description = $("#<%=txtDescription.ClientID %>").val();
            if (date == "") {
                toastr.warning("Please provide Date.");
                return false;
            }
            else if (time == "") {
                toastr.warning("Please provide Time.");
                return false;
            }
            else if (description == "") {
                toastr.warning("Please provide Description.");
                return false;
            }
            if (date != "") {
                date = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(date, innBoarDateFormat);
            }
            attendenceBO = {
                AttendanceDate: date,
                EntryTime: time,
                Remark: description,
                AttendanceId: id
            }

            PageMethods.SaveLeaveInformation(attendenceBO, OnSaveSucceed, OnFailed);
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
        }
        function OnFillFormSucceed(result) {
            CommonHelper.SpinnerClose();
            RequestOff("Late Attendence Application - Edit", "");
            $("#ContentPlaceHolder1_hfEmployeeId").val(result.EmpId);
            $("#<%=txtTime.ClientID %>").val(result.StartHour).attr("disabled", true);
            $("#<%=txtDate.ClientID %>").val(result.stringAttendenceDate).attr("disabled", true);
            $("#<%=txtDescription.ClientID %>").val(result.Remark);
            $("#<%=hfAttendanceId.ClientID %>").val(result.AttendanceId);
            $("#btnSave").val("Update");
        }
        function RequestOff(msg, status) {
            $("#lateAttendanceDiv").show();

            $("#lateAttendanceDiv").dialog({
                autoOpen: true,
                dialogClass: 'no-close',
                modal: true,
                width: 800,
                height: 500,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                open: function (event, ui) {
                    $('#lateAttendanceDiv').css('overflow', 'hidden');
                    $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                },
                title: msg,
                show: 'slide'
            });

            var drop = document.getElementById("ContentPlaceHolder1_ddlStatus");

            if (status == "Pending") {
                if (drop.options[2].value == "Approved") {
                    drop.options[2].hidden = true;
                }
            }
            else if (status == "Checked") {
                if (drop.options[1].value == "Checked") {
                    drop.options[1].hidden = true;
                }
            }
            else {
                $("#ApprovalDiv").hide;
            }
            return false;

        }
        function PerformClearClose() {
            $('#lateAttendanceDiv').dialog('close');
            PerformEnable();
            $("#<%=txtDate.ClientID %>").val("");
            $("#<%=txtTime.ClientID %>").val("");
            $("#<%=txtDescription.ClientID %>").val("");
            $("#<%=ddlStatus.ClientID %>").val("0");
            $("#<%=txtCancelReason.ClientID %>").val("");
            $("#<%=hfAttendanceId.ClientID %>").val("0");
            $("#<%=hfEmployeeId.ClientID %>").val("0");
            var drop = document.getElementById("ContentPlaceHolder1_ddlStatus");
            for (var i = 0; i < drop.length; i++) {
                drop.options[i].hidden = false;
            }
            $("#btnSave").val("Save");
        }
        function PerformDisable() {
            $("#<%=txtDate.ClientID %>").attr("disabled", true);
            $("#<%=txtTime.ClientID %>").attr("disabled", true);
            $("#<%=txtDescription.ClientID %>").attr("disabled", true);
        }
        function PerformEnable() {
            $("#<%=txtDate.ClientID %>").attr("disabled", false);
            $("#<%=txtTime.ClientID %>").attr("disabled", false);
            $("#<%=txtDescription.ClientID %>").attr("disabled", false);
        }
        function ApproveLeaveApp(titleBar, AttendanceId, status) {
            if (!confirm("Do you Want To Update Status?")) {
                return false;
            }

            GetEmpAttendenceInfo(AttendanceId);
            RequestOff(titleBar, status);
            PerformDisable();
            $("#btnApprove ").show();
            $("#saveBtnDiv ").show();
            $("#saveBtnDiv ").hide();
            $("#ApprovalDiv").show();
            $("#statusDiv").show();
        }

        function ViewLeaveDetails(titleBar, AttendanceId, status) {
            GetEmpAttendenceInfo(AttendanceId);
            RequestOff(titleBar, status);
            PerformDisable();

            if (status == 'Cancel') {
                $("#statusDiv ").show();
                $("#btnSave ").hide();
                $("#ApprovalDiv").show();
                $("#saveBtnDiv ").hide();
                $("#cancelDiv").show();
                $("#btnApprove").hide();

                $("#<%=ddlStatus.ClientID %>").attr("disabled", true);
                $("#<%=txtCancelReason.ClientID %>").attr("disabled", true);
            }
            else {
                $("#statusDiv ").hide();
                $("#ApprovalDiv").show();
                $("#saveBtnDiv ").hide();
                $("#cancelDiv").hide();
                $("#btnApprove").hide();
            }
        }
        function Approval() {
            var action = $("#<%=ddlStatus.ClientID %>").val();
            var cnacelReason = $("#<%=txtCancelReason.ClientID%>").val();
            var attenId = $("#<%=hfAttendanceId.ClientID%>").val();
            if (action == "0") {
                toastr.warning("Please Select a Status.")
                return false;
            }
            if (action == "Cancel" && cnacelReason == "") {
                toastr.warning("Please Provide Reason to Cancel.")
                return false;
            }
            PageMethods.MakeApproval(attenId, action, cnacelReason, OnApprovalSucceed, OnFailed);
            return false;
        }
        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearClose();
                GridPaging(1, 1);
                $("#ApprovalDiv").hide();
                $("#saveBtnDiv ").show();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage)
            }
        }
        function OnFailed(error) {
        }
    </script>
    <asp:HiddenField ID="hfAttendanceId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfEmployeeId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsEmpListVisible" runat="server" Value="0" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsAdminUser" runat="server" Value="0" />
    <div id="AttendanceApplication" class="panel panel-default" style="display: none">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label5" runat="server" class="control-label required-field" Text="Employee"></asp:Label>
                    </div>
                    <div class="col-md-10" style="padding-bottom: 5px;">
                        <asp:DropDownList ID="ddlApplicationEmployee" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblAttendanceDate" runat="server" class="control-label required-field" Text="Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtAttendanceDate" runat="server" TabIndex="2" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblEntryTime" runat="server" class="control-label" Text="Entry Time"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEntryHour" placeholder="10" TabIndex="3" CssClass="form-control"
                            runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblExitTime" runat="server" class="control-label" Text="Exit Time"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtExitHour" placeholder="06" TabIndex="7" CssClass="form-control"
                            runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblRemark" runat="server" class="control-label required-field" Text="Remarks"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtRemarks" runat="server" TabIndex="10" CssClass="form-control"
                            TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <input type="button" value="Apply" style="width: 100px;" id="btnSaveAttendanceApplication" onclick="PerformSaveAttendanceApplication()" class="TransactionalButton btn btn-primary btn-sm" />
                        <input type="button" value="Clear" style="width: 100px;" id="btnClearAttendanceApplication" onclick="PerformClearAttendanceApplication()" class="TransactionalButton btn btn-primary btn-sm" />
                        <input type="button" value="Close" style="width: 100px;" id="btnCloseAttendanceApplication" onclick="PerformAttendanceApplicationClose()" class="TransactionalButton btn btn-primary btn-sm" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Attendance Information
            <input style="float: right; padding: 0px 20px 0px 20px;" id="btnNewApplication" type="button" class="TransactionalButton btn btn-primary btn-sm"
                value="Attendance Application" />
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
                        <asp:Label ID="lblFromDate" runat="server" class="control-label required-field" Text="Search Date"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtFromDate" runat="server" Placeholder="From Date" CssClass="form-control" TabIndex="6"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtToDate" runat="server" Placeholder="To Date" CssClass="form-control" TabIndex="7"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label" Text="Status"></asp:Label>
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
                <table class="table table-bordered table-condensed table-responsive" id='tblAttendenceInfo'>
                    <thead>
                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                            <td style="width: 5%;">Code
                            </td>
                            <td style="width: 17%;">Name
                            </td>
                            <td style="width: 15%;">Designation
                            </td>
                            <td style="width: 8%;">Date
                            </td>
                            <td style="width: 5%;">In Time
                            </td>
                            <td style="width: 5%;">Late
                            </td>
                            <td style="width: 7%;">Out Time
                            </td>
                            <td style="width: 7%;">Late App. Date
                            </td>
                            <td style="width: 7%;">Status
                            </td>
                            <td style="text-align: center; width: 10%;">Action
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
    <div id="lateAttendanceDiv" class="panel panel-default" style="display: none">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Time"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtTime" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" id="description">
                    <div class="col-md-2">
                        <asp:Label ID="Label8" runat="server" class="control-label required-field" Text="Description"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="multiline" CssClass="form-control"
                            TabIndex="8">
                        </asp:TextBox>
                    </div>
                </div>
                <div id="ApprovalDiv" style="display: none">
                    <div class="form-group" id="statusDiv">
                        <div class="col-md-2">
                            <asp:Label ID="Label7" runat="server" class="control-label" Text="Approval Status"></asp:Label>
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
                            <asp:Label ID="Label3" runat="server" class="control-label" Text="Cancel Reason"></asp:Label>
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
