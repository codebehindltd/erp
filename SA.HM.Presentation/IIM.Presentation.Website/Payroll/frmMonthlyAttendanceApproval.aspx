<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmMonthlyAttendanceApproval.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmMonthlyAttendanceApproval" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithBasicInfo.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="EmployeeForLeaveSearch" Src="~/HMCommon/UserControl/EmployeeSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var op = {};

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Leave Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Leave</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $('.timespan').timepicker({
                showPeriod: is12HourFormat
            });

        });

        function PerformSaveAction() {

            var employeeId = $("#ContentPlaceHolder1_searchEmployee_hfEmployeeId").val();
            var monthId = $("#ContentPlaceHolder1_ddlReportMonth").val();
            var yearId = $("#ContentPlaceHolder1_ddlYear").val();

            if (employeeId == "") {
                toastr.info("Please Search Employee");
                return false;
            }
            else if (monthId == "") {
                toastr.info("Please Select Month");
                return false;
            }
            else if (yearId == "") {
                toastr.info("Please Select Year");
                return false;
            }

            var dayDate = "", attendanceId = "", inTime = "", outTime = "", leaveId = "", leaveTypeId = "",
                leaveMode = "", remarks = "", column = 0, totalDays = 0, leaveMessage = "";

            var LeaveEntry = new Array();
            var AttendanceEntry = new Array();
            var AttendanceUpdate = new Array();

            $("#TblAttendanceAndLeave tbody tr").each(function () {

                dayDate = CommonHelper.DateFormatToMMDDYYYY($(this).find("td:eq(1)").text(), '/');

                if ($(this).find("td:eq(2)").find("input").is(":input")) {
                    inTime = $(this).find("td:eq(2)").find("input").val();
                }
                if ($(this).find("td:eq(3)").find("input").is(":input")) {
                    outTime = $(this).find("td:eq(3)").find("input").val();
                }

                if ($(this).find("td:eq(4)").find("select").is("select")) {
                    leaveTypeId = $(this).find("td:eq(4)").find("select").val();
                    leaveMode = $(this).find("td:eq(5)").find("select").val();
                }

                attendanceId = $(this).find("td:eq(7)").text();
                leaveId = $(this).find("td:eq(8)").text();
                remarks = $(this).find("td:eq(6)").find("input").val();

                if ($(this).find("td:eq(0)").find("input").is(":checked")) {

                    if (attendanceId == "" && inTime != "" && outTime != "") {
                        AttendanceEntry.push({
                            AttendanceId: 0,
                            EmpId: employeeId,
                            AttendanceDate: dayDate,
                            EntryTime: (dayDate + " " + inTime),
                            ExitTime: (dayDate + " " + outTime),
                            Remark: (remarks == "" ? null : remarks)
                        });
                    }
                    else if (attendanceId != "") {
                        AttendanceUpdate.push({
                            AttendanceId: attendanceId,
                            EmpId: employeeId,
                            AttendanceDate: dayDate,
                            EntryTime: (dayDate + " " + inTime),
                            ExitTime: (dayDate + " " + outTime),
                            Remark: (remarks == "" ? null : remarks)
                        });
                    }

                    if (leaveId == "" && inTime == "" && outTime == "" && leaveTypeId != "" && leaveMode != "") {
                        LeaveEntry.push({
                            LeaveId: 0,
                            EmpId: employeeId,
                            LeaveMode: leaveMode,
                            LeaveTypeId: leaveTypeId,
                            FromDate: dayDate,
                            ToDate: dayDate,
                            TransactionType: null,
                            NoOfDays: 1,
                            ExpireDate: null,
                            LeaveStatus: 'Approved',
                            Reason: (remarks == "" ? null : remarks),
                            ReportingTo: 0
                        });
                    }
                    else if (leaveId == "" && inTime == "" && outTime == "" && leaveTypeId == "" && leaveMode != "") {
                        leaveMessage = leaveMessage + "For Date " + dayDate + ", Pleasse Select Leave Type. ";
                    }
                    else if (leaveId == "" && inTime == "" && outTime == "" && leaveTypeId != "" && leaveMode == "") {
                        leaveMessage = leaveMessage + "For Date " + dayDate + ", Pleasse Select Leave Mode. ";
                    }
                }

            });

            if (leaveMessage != "") {
                toastr.warning(leaveMessage);
                return false;
            }

            PageMethods.SaveLeaveAndAttendanceInformation(AttendanceEntry, AttendanceUpdate, LeaveEntry, OnSaveLeaveInformationSucceed, OnSaveLeaveInformationFailed);
            return false;
        }
        function OnSaveLeaveInformationSucceed(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            var employeeId = $("#ContentPlaceHolder1_searchEmployee_hfEmployeeId").val();
            PerformClearAction();
            //PageMethods.GetLeaveTakenNBalanceByEmployee(employeeId, OnLoadLeaveBalanceSuccess, OnLoadLeaveBalanceFailed);
        }
        function OnSaveLeaveInformationFailed(error) {
            toastr.error(error.get_message());
        }

        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#form1")[0].reset();
            $("#ContentPlaceHolder1_searchEmployee_hfEmployeeId").val("0");
            $("#TblAttendanceAndLeave tbody").html("");

            return false;
        }

        function WorkAfterSearchEmployee() {

        }

        function SearchAttendanceAndLeaveInfo() {

            var employeeId = $("#ContentPlaceHolder1_searchEmployee_hfEmployeeId").val();
            var monthId = $("#ContentPlaceHolder1_ddlReportMonth").val();
            var yearId = $("#ContentPlaceHolder1_ddlYear").val();

            if (employeeId == "") {
                toastr.info("Please Search Employee");
                return false;
            }
            else if (monthId == "") {
                toastr.info("Please Select Month");
                return false;
            }
            else if (yearId == "") {
                toastr.info("Please Select Year");
                return false;
            }

            $("#TblAttendanceAndLeave tbody").html("");
            PageMethods.GetAttendanceAndLeaveApproval(employeeId, monthId, yearId, OnLoadLeaveBalanceSuccess, OnLoadLeaveBalanceFailed);
        }
        function OnLoadLeaveBalanceSuccess(result) {

            var tr = "", selectOption = "", option = "", leaveModeOption = "";

            selectOption = "<select class=\"form-control\" id=\"LeaveTypeId\">";
            selectOption += "<option value=\"\">---Please Select---</option>";
            $.each(result.LeaveType, function (index, obj) {
                option = option + "<option value=\"" + obj.LeaveTypeId + "\">" + obj.TypeName + "</option>";
            });
            selectOption += option + "</select>";

            option = "";

            leaveModeOption = "<select class=\"form-control\" id=\"LeaveTypeId\">";
            leaveModeOption += "<option value=\"\">---Please Select---</option>";
            $.each(result.LeaveMode, function (index, obj) {
                option = option + "<option value=\"" + obj.FieldValue + "\">" + obj.Description + "</option>";
            });
            leaveModeOption += option + "</select>";

            $.each(result.AttendanceAndLeaveApproval, function (index, obj) {
                tr = "<tr>";

                tr += "<td style=\"width: 8%;\">";
                tr += "<input type=\"checkbox\" class=\"form-control\" />";
                tr += "</td>";

                tr += "<td style=\"width: 12%;\">" + CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(obj.AttendanceDateStr, innBoarDateFormat) + "</td>";

                tr += "<td style=\"width: 15%;\">";
                if (obj.InTime != null) {
                    tr += "<input type=\"text\" class=\"form-control timespan\" value = \'" + obj.InTimeStr + "'\ />";
                }
                else {
                    tr += obj.LeaveId == null ? "<input type=\"text\" class=\"form-control timespan\" />" : "";
                }
                tr += "</td>";

                tr += "<td style=\"width: 15%;\">";
                if (obj.OutTime != null) {
                    tr += "<input type=\"text\" class=\"form-control timespan\" value = \'" + obj.OutTimeStr + "'\ />";
                }
                else {
                    tr += obj.LeaveId == null ? "<input type=\"text\" class=\"form-control timespan\" />" : "";
                }
                tr += "</td>";

                tr += "<td style=\"width: 15%;\">";
                if (obj.LeaveId == null) {
                    tr += obj.AttendanceId == null ? selectOption : "";
                }
                else {
                    tr += obj.LeaveType;
                }
                tr += "</td>";

                tr += "<td style=\"width: 15%;\">";
                if (obj.LeaveId == null) {
                    tr += obj.AttendanceId == null ? leaveModeOption : "";
                }
                else {
                    tr += obj.LeaveMode;
                }
                tr += "</td>";

                tr += "<td style=\"width: 20%;\">";
                tr += "<input type=\"text\" class=\"form-control\" />";
                tr += "</td>";

                tr += "<td style=\"display: none;\">";
                tr += obj.AttendanceId == null ? "" : obj.AttendanceId;
                tr += "</td>";

                tr += "<td style=\"display: none;\">";
                tr += obj.LeaveId == null ? "" : obj.LeaveId;
                tr += "</td>";

                tr += "<td style=\"display: none;\">";
                tr += obj.LeaveTypeId == null ? "" : obj.LeaveTypeId;
                tr += "</td>";

                tr += "</tr>";

                $("#TblAttendanceAndLeave tbody").append(tr);
                tr = "";
            });

            $('.timespan').timepicker({
                showPeriod: is12HourFormat
            });

        }
        function OnLoadLeaveBalanceFailed() { }

    </script>
    <asp:HiddenField ID="hfLeaveId" runat="server" Value=""></asp:HiddenField>

    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">Employee Leave Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <UserControl:EmployeeSearch runat="server" ID="searchEmployee" />

                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblProcessDate" runat="server" class="control-label required-field" Text="Process Month"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportMonth" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Process Year"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlYear" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <input type="button" onclick="SearchAttendanceAndLeaveInfo()" value="Search Details" class="TransactionalButton btn btn-primary" />
                    </div>
                </div>

            </div>
        </div>
    </div>
    <div id="Div1" class="panel panel-default">
        <div class="panel-heading">Leave Balance Info</div>
        <div class="panel-body">
            <table id="TblAttendanceAndLeave" class="table table-bordered table-condensed table-hover">
                <thead>
                    <tr>
                        <th style="width: 8%;">Select</th>
                        <th style="width: 12%;">Date</th>
                        <th style="width: 15%;">In Time</th>
                        <th style="width: 15%;">Out Time</th>
                        <th style="width: 15%;">Leave Type</th>
                        <th style="width: 15%;">Leave Mode</th>
                        <th style="width: 20%;">Remarks</th>
                        <th style="display: none;">Attendance Id</th>
                        <th style="display: none;">Leave Id</th>
                        <th style="display: none;">Leave Type Id</th>
                    </tr>
                </thead>
                <tbody></tbody>
            </table>

        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <input type="button" onclick="PerformSaveAction()" value="Save" class="TransactionalButton btn btn-primary" />
            <input type="button" onclick="PerformClearAction()" value="Clear" class="TransactionalButton btn btn-primary" />
        </div>
    </div>
</asp:Content>
