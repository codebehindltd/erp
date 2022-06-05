<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpAttendance.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpAttendance" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfAttendanceId" runat="server"></asp:HiddenField>
    <script type="text/javascript">
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Attendance & Time Keeping</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Manual Attendance</li>";
            var breadCrumbs = moduleName + formName;

            if (IsCanSave) {
                $('#ContentPlaceHolder1_btnSave').show();
            } else {
                $('#ContentPlaceHolder1_btnSave').hide();
            }
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $('#ContentPlaceHolder1_txtEntryHour').timepicker({
                showPeriod: is12HourFormat
            });
            $('#ContentPlaceHolder1_txtExitHour').timepicker({
                showPeriod: is12HourFormat
            });
            $("#ContentPlaceHolder1_txtAttendanceDate").datepicker({
                changeMonth: true,
                changeYear: true,
                maxDate: DayOpenDate,
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
        });

        $(document).ready(function () {

            var txtAttendanceDate = '<%=txtAttendanceDate.ClientID%>';

            $("#ContentPlaceHolder1_gvEmpAttendance tbody tr:eq(1)").remove();
        });
        function CheckJoinDate(empId, attendanceDate) {
            var returnInfo = false;
            attendanceDate = CommonHelper.DateFormatToMMDDYYYY(attendanceDate, '/');
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Payroll/frmEmpAttendance.aspx/CheckJoinDate',
                data: JSON.stringify({ empId: empId, attendanceDate: attendanceDate }),
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d > 0) {

                        returnInfo = true;
                    }
                    else {
                        returnInfo = false;
                    }

                },
                error: function (result) {
                    CommonHelper.AlertMessage(result.d.AlertMessage);
                }
            });
            return returnInfo;
        }
        function PerformSaveAction() {
            var attendanceId = $("#<%=hfAttendanceId.ClientID %>").val();
            var employeeId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
            var attendanceDate = $("#<%=txtAttendanceDate.ClientID %>").val();
            var entryHour = $("#<%=txtEntryHour.ClientID %>").val();
            var exitHour = $("#<%=txtExitHour.ClientID %>").val();
            var remarks = $("#<%=txtRemark.ClientID %>").val();
            if (employeeId == "0") {
                toastr.warning("Please Select an Employee.");
                return false;
            }
            else if ($("#<%=txtAttendanceDate.ClientID %>").val() == "") {
                toastr.info("Please Provide Attendance Date");
                return false;
            }
            var isInvalidDate = CheckJoinDate(employeeId, attendanceDate);
            if (isInvalidDate) {
                toastr.info("Attendance Date is earlier than Join Date. Please Provide Valid Attendance Date");
                return false;
            }
            if (entryHour == "") {
                toastr.info("Please Provide Entry Time");
                return false;
            }
            else if (entryHour != "") {
                var isValid = IsValidTime(entryHour);
                if (!isValid) {
                    toastr.warning("Please Provide Correct Time Format.");
                    $("#<%=txtEntryHour.ClientID %>").focus();
                    return false;
                }
            }
            if (exitHour == "") {
                toastr.info("Please Provide Exit Time");
                return false;
            }
            else if (exitHour != "") {
                var isValid = IsValidTime(exitHour);
                if (!isValid) {
                    toastr.warning("Please Provide Correct Time Format.");
                    $("#<%=txtExitHour.ClientID %>").focus();
                    return false;
                }
            }
            PageMethods.PerformAttendanceSaveAction(employeeId, attendanceId, attendanceDate, entryHour, exitHour, remarks, OnSaveAttendanceSucceed, OnSaveAttendanceFailed);
            return false;
        }
        function OnSaveAttendanceSucceed(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            ReloadGrid(1);
            PerformClearAction();
        }
        function OnSaveAttendanceFailed(error) {
            toastr.error(error.get_message());
        }

        function OnEmployeeAttendanceSucceed(result) {

            $("#ContentPlaceHolder1_gvEmpAttendance tr:not(:first-child)").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"3\" >No Data Found</td> </tr>";
                $("#ContentPlaceHolder1_gvEmpAttendance tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#ContentPlaceHolder1_gvEmpAttendance tbody tr").length + 1;
                totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:17%;\">" + gridObject.stringEntryTime + "</td>";
                tr += "<td align='left' style=\"width:51%;\">" + gridObject.stringExitTime + "</td>";

                editLink = "<a href=\"javascript:void();\" onclick=\"javascript:return PerformFillFormAction('" + gridObject.AttendanceId + "', '" + result.GridPageLinks.CurrentPageNumber + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" /> </a>";
                deleteLink = "<a href=\"javascript:void();\" onclick=\"javascript:return PerformDeleteAction('" + gridObject.AttendanceId + "', '" + result.GridPageLinks.CurrentPageNumber + "');\"><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";

                tr += "<td align='center' style=\"width:15%;\">";

                if (IsCanEdit)
                    tr += editLink;
                if (IsCanDelete)
                    tr += deleteLink;
                tr += "</td>";

                tr += "</tr>"

                $("#ContentPlaceHolder1_gvEmpAttendance tbody ").append(tr);
                tr = "";

            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

        }
        function OnEmployeeAttendanceFailed(error) {

        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#ContentPlaceHolder1_gvEmpAttendance tbody tr").length - 1;
            PageMethods.LoadEmployeeAttendance($("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val(), gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnEmployeeAttendanceSucceed, OnEmployeeAttendanceFailed);
            return false;
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
        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {

            $("#<%=hfAttendanceId.ClientID %>").val(result.AttendanceId);
            $("#<%=txtAttendanceDate.ClientID %>").val(result.stringAttendenceDate);
            $("#<%=txtEntryHour.ClientID %>").val(result.StartHour);
            $("#<%=txtExitHour.ClientID %>").val(result.EndHour);

            $("#<%=txtRemark.ClientID %>").val(result.Remark);
            if (IsCanEdit) {
                $('#ContentPlaceHolder1_btnSave').show();
            } else {
                $('#ContentPlaceHolder1_btnSave').hide();
            }

            $("#<%=btnSave.ClientID %>").val("Update");

            return false;
        }
        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
        }

        //For Delete-------------------------        
        function PerformDeleteAction(actionId) {
            var answer = confirm("Do you want to delete this record?")
            if (answer) {
                PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            }
            return false;
        }
        function OnDeleteObjectSucceeded(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            ReloadGrid(0);
            PerformClearAction();
        }
        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function PerformClearActionForButton() {
            if (!confirm("Do you want to clear?")) {
                return false;
            }

            PerformClearAction();
        }

        //For ClearForm-------------------------
        function PerformClearAction() {

            $("#<%=hfAttendanceId.ClientID %>").val('');
            $("#<%=txtAttendanceDate.ClientID %>").val('');

            $("#<%=txtEntryHour.ClientID %>").val('');
            $("#<%=txtExitHour.ClientID %>").val('');

            $("#<%=txtRemark.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");

            $("#ContentPlaceHolder1_employeeSearch_txtSearchEmployee").val("");
            $("#ContentPlaceHolder1_employeeSearch_txtEmployeeName").val("");
            $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val("0");
            $("#ContentPlaceHolder1_employeeSearch_hfEmployeeName").val("");
            return false;
        }

        function ReloadGrid(IsCurrentOrPreviousPage) {
            var currentPageNumber = $("#GridPagingContainer ul li[class='active']").text();

            if (currentPageNumber == "")
                currentPageNumber = 1;

            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }

        function WorkAfterSearchEmployee() {
            var employeeId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
            PageMethods.LoadEmployeeAttendance(employeeId, 1, 1, 1, OnEmployeeAttendanceSucceed, OnEmployeeAttendanceFailed);
        }

    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <div id="AttendanceInformation" class="panel panel-default">
        <div class="panel-heading">Employee Attendance</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <UserControl:EmployeeSearch ID="employeeSearch" runat="server" />
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblAttendanceDate" runat="server" class="control-label required-field" Text="Attendance Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtAttendanceDate" runat="server" TabIndex="2" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblEntryTime" runat="server" class="control-label required-field" Text="Entry Time"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEntryHour" placeholder="10" TabIndex="3" CssClass="form-control"
                            runat="server"></asp:TextBox>
                        <%--<asp:TextBox ID="txtEntryMinute" placeholder="00" TabIndex="4" CssClass="CustomMinuteSize"
                        runat="server"></asp:TextBox>
                    <asp:DropDownList ID="ddlEntryAMPM" CssClass="CustomAMPMSize" TabIndex="5" runat="server">
                        <asp:ListItem Value="AM" Text="AM"></asp:ListItem>
                        <asp:ListItem Value="PM" Text="PM"></asp:ListItem>
                    </asp:DropDownList>
                    (10:00AM)--%>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblExitTime" runat="server" class="control-label required-field" Text="Exit Time"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtExitHour" placeholder="06" TabIndex="7" CssClass="form-control"
                            runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblRemark" runat="server" class="control-label" Text="Remarks"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtRemark" runat="server" TabIndex="10" CssClass="form-control"
                            TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <%--Right Left--%>
                        <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="11" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="return PerformSaveAction();" />
                        <asp:Button ID="btnClear" runat="server" TabIndex="12" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return PerformClearActionForButton();" />
                    </div>
                </div>

                <div class="childDivSection" style="padding-top: 5px;">
                    <asp:GridView ID="gvEmpAttendance" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True"
                        ForeColor="#333333" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:BoundField DataField="EntryTime" HeaderText="Entry Time" ItemStyle-Width="50%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ExitTime" HeaderText="Exit Time" ItemStyle-Width="35%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ControlStyle Font-Size="Small" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            <asp:Label ID="lblRecordNotFound" runat="server" Text="Record Not Found."></asp:Label>
                        </EmptyDataTemplate>
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#44545E" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#7C6F57" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
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
