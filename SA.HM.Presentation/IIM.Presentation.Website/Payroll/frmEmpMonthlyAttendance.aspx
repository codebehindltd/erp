<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpMonthlyAttendance.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpMonthlyAttendance" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithBasicInfo.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="EmployeeForLeaveSearch" Src="~/HMCommon/UserControl/EmployeeSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            if (IsCanSave) {
                $('#save_button').show();
            } else {
                $('#save_button').hide();
            }
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Leave Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Leave</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });

        $(document).ready(function () {

            var txtFromDate = '<%=txtFromDate.ClientID%>'
            var txtToDate = '<%=txtToDate.ClientID%>'

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtToDate).datepicker("option", "minDate", selectedDate);
                    var strDate = new Date(CommonHelper.DateFormatToMMDDYYYY($('#' + txtFromDate).val(), '/'));
                    var endDate = new Date(CommonHelper.DateFormatToMMDDYYYY($('#' + txtToDate).val(), '/'));

                    $('#ContentPlaceHolder1_txtNoOfDays').val(CommonHelper.DateDifferenceInDays(strDate, endDate));
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
                    $('#' + txtFromDate).datepicker("option", "maxDate", selectedDate);

                    var strDate = new Date(CommonHelper.DateFormatToMMDDYYYY($('#' + txtFromDate).val(), '/'));
                    var endDate = new Date(CommonHelper.DateFormatToMMDDYYYY($('#' + txtToDate).val(), '/'));

                    $('#ContentPlaceHolder1_txtNoOfDays').val(CommonHelper.DateDifferenceInDays(strDate, endDate));
                },
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

        });

        function PerformSaveAction() {

            var employeeId = $("#ContentPlaceHolder1_searchEmployee_hfEmployeeId").val();
            var dateFrom = $("#ContentPlaceHolder1_txtFromDate").val();
            var dateTo = $("#ContentPlaceHolder1_txtToDate").val();

            var dateFrom = CommonHelper.DateFormatToMMDDYYYY(dateFrom, '/');
            var dateTo = CommonHelper.DateFormatToMMDDYYYY(dateTo, '/');

            if (employeeId == "") {
                toastr.info("Please Search Employee");
                return false;
            }
            else if (dateFrom == "") {
                toastr.info("Please Select Date From");
                return false;
            }
            else if (dateTo == "") {
                toastr.info("Please Select Date To");
                return false;
            }

            var day = "", leaveTypeId = "", column = 0, totalDays = 0;
            var LeaveInformation = new Array();
            var Attendance = new Array();

            $("#LeaveInfoTable tbody tr").each(function () {

                leaveTypeId = $(this).find("td:eq(2)").text();
                totalDays = parseInt($(this).find("td:eq(4)").text());

                for (column = 9; column < (totalDays + 9); column++) {

                    day = $(this).find("td:eq(" + column + ")").find("span").text();

                    Attendance.push({
                        AttendanceId: 0,
                        EmpId: employeeId,
                        AttendanceDate: day,
                        EntryTime: day,
                        ExitTime: day
                    });

                    if ($(this).find("td:eq(" + column + ")").find("input").is(":checked") == false) {
                        LeaveInformation.push({
                            LeaveId: 0,
                            EmpId: employeeId,
                            LeaveMode: $(this).find("td:eq(6)").text(),
                            LeaveModeId: $(this).find("td:eq(5)").text(),
                            LeaveTypeId: leaveTypeId,
                            FromDate: day,
                            ToDate: day,
                            NoOfDays: 1,
                            LeaveStatus: 'Approved'
                        });
                    }
                }

            });

            PageMethods.SaveLeaveInformation(employeeId, dateFrom, dateTo, Attendance, LeaveInformation, OnSaveLeaveInformationSucceed, OnSaveLeaveInformationFailed);
            return false;
        }
        function OnSaveLeaveInformationSucceed(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            var employeeId = $("#ContentPlaceHolder1_searchEmployee_hfEmployeeId").val();
            PerformClearAction();
            PageMethods.GetLeaveTakenNBalanceByEmployee(employeeId, OnLoadLeaveBalanceSuccess, OnLoadLeaveBalanceFailed);
        }
        function OnSaveLeaveInformationFailed(error) {
            toastr.error(error.get_message());
        }

        //For ClearForm-------------------------
        function PerformClearActionForButton() {
            if (!confirm("Do you want to clear?")) {
                return false;
            }
            PerformClearAction();
        }

        function PerformClearAction() {
            $("#form1")[0].reset();
            $("#<%=txtFromDate.ClientID %>").val('');
            $("#<%=txtToDate.ClientID %>").val('');
            $("#ContentPlaceHolder1_searchEmployee_hfEmployeeId").val("0");
            $("#leaveContainer").html("");

            return false;
        }

        function WorkAfterSearchEmployee() {

        }

        function SearchLeaveInfo() {
            var employeeId = $("#ContentPlaceHolder1_searchEmployee_hfEmployeeId").val();
            var dateFrom = $("#ContentPlaceHolder1_txtFromDate").val();
            var dateTo = $("#ContentPlaceHolder1_txtToDate").val();

            if (employeeId == "") {
                toastr.info("Please Search Employee");
                return false;
            }
            else if (dateFrom == "") {
                toastr.info("Please Select Date From");
                return false;
            }
            else if (dateTo == "") {
                toastr.info("Please Select Date To");
                return false;
            }

            var dateFrom = CommonHelper.DateFormatToMMDDYYYY(dateFrom, '/');
            var dateTo = CommonHelper.DateFormatToMMDDYYYY(dateTo, '/');

            $("#leaveContainer").html("");

            PageMethods.GetLeaveTakenNBalanceByEmployee(employeeId, dateFrom, dateTo, OnLoadLeaveBalanceSuccess, OnLoadLeaveBalanceFailed);
        }
        function OnLoadLeaveBalanceSuccess(result) {
            $("#leaveContainer").html(result);
        }
        function OnLoadLeaveBalanceFailed() { }

    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfLeaveId" runat="server" Value=""></asp:HiddenField>

    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">Employee Leave Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <UserControl:EmployeeSearch runat="server" ID="searchEmployee" />

                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label required-field" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label required-field" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <input type="button" onclick="SearchLeaveInfo()" value="Search Leave Details" class="TransactionalButton btn btn-primary" />
                    </div>
                </div>

            </div>
        </div>
    </div>
    <div id="Div1" class="panel panel-default">
        <div class="panel-heading">Leave Balance Info</div>
        <div class="panel-body">
            <div id="leaveContainer"></div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <input type="button" id="save_button" onclick="PerformSaveAction()" value="Save" class="TransactionalButton btn btn-primary" />
            <input type="button" onclick="PerformClearActionForButton()" value="Clear" class="TransactionalButton btn btn-primary" />
        </div>
    </div>

</asp:Content>
