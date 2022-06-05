<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmLeaveInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmLeaveInformation" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="EmployeeForLeaveSearch" Src="~/HMCommon/UserControl/EmployeeSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Leave Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Leave</li>";
            var breadCrumbs = moduleName + formName;

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

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
        });

        $(document).ready(function () {
            $("#myTabs").tabs();

            $('#ContentPlaceHolder1_txtFromDateSearch').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDateSearch').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtToDateSearch').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDateSearch').datepicker("option", "maxDate", selectedDate);
                }
            });

            var txtFromDate = '<%=txtFromDate.ClientID%>'
            var txtToDate = '<%=txtToDate.ClientID%>'

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

            $("#ContentPlaceHolder1_gvUserInformation tbody tr:eq(1)").remove();

            $("#ContentPlaceHolder1_btnSearch").click(function () {
                GridPaging(1, 1);
                return false;
            });
            $("#ContentPlaceHolder1_ddlReportingTo").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            <%--$("#<%=ddlLeaveStatus.ClientID %>").change(function () {
                var leaveStatus = $("#<%=ddlLeaveStatus.ClientID %>").val();
                if (leaveStatus == 'Pending') {
                    $("#reportingTo").show();
                }
                else {
                    $("#reportingTo").hide();
                }
            });--%>

            $("#ContentPlaceHolder1_employeeForLeaveSearch_ddlEmployee").change(function () {
                var type = $("#ContentPlaceHolder1_employeeForLeaveSearch_ddlEmployee").val();
                if (type == '1') {
                    $("#<%=ddlLeaveTypeIdSearch.ClientID %>").val('0');
                    $("#<%=ddlLeaveModeSearch.ClientID %>").val('0');
                    $("#<%=ddlLeaveStatusSearch.ClientID %>").val('--- Please Select ---');
                    $("#<%=txtFromDateSearch.ClientID %>").val('');
                    $("#<%=txtToDateSearch.ClientID %>").val('');
                }

            });
        });


        function PerformSaveAction() {
            var employeeId = $("#ContentPlaceHolder1_employeeeSearch_hfEmployeeId").val();
            var leaveMode = $("#<%=ddlLeaveMode.ClientID %>").val();
            //var leaveMode = 'Leave With Pay';
            var leaveTypeId = $("#<%=ddlLeaveTypeId.ClientID %>").val();
            var fromDate = $("#<%=txtFromDate.ClientID %>").val();
            var toDate = $("#<%=txtToDate.ClientID %>").val();
            var noOfdays = $("#<%=txtNoOfDays.ClientID %>").val();
            var status = $("#<%=ddlLeaveStatus.ClientID %>").val();
            var leaveId = $("#<%=hfLeaveId.ClientID %>").val();
            var reportingTo = $("#<%=ddlReportingTo.ClientID %>").val();

            if (employeeId == "" || employeeId == "0") {
                toastr.warning("Please Provide an employee.");
                return false;
            }
            else if (leaveTypeId == "0") {
                toastr.warning("Please select leave type.");
                return false;
            }
            else if (leaveMode == '--- Please Select ---') {
                toastr.warning("Please select leave Mode.");
                return false;
            }
            else if (status == '--- Please Select ---') {
                toastr.warning("Please select leave status.");
                return false;
            }
            else if (noOfdays == "") {
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

            PageMethods.SaveLeaveInformation(employeeId, leaveId, leaveMode, leaveTypeId, fromDate, toDate, noOfdays, reportingTo, status, OnSaveLeaveInformationSucceed, OnSaveLeaveInformationFailed);
            return false;
        }
        function OnSaveLeaveInformationSucceed(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            var employeeId = $("#ContentPlaceHolder1_employeeeSearch_hfEmployeeId").val();
            PerformClearAction();
            PageMethods.GetLeaveTakenNBalanceByEmployee(employeeId, OnLoadLeaveBalanceSuccess, OnLoadLeaveBalanceFailed);
        }
        function OnSaveLeaveInformationFailed(error) {
            toastr.error(error.get_message());
        }

        function OnLeaveInfoLoadSucceed(result) {

            //var today = new Date();

            $("#ContentPlaceHolder1_gvUserInformation tr:not(:first-child)").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"9\" >No Data Found</td> </tr>";
                $("#ContentPlaceHolder1_gvUserInformation tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#ContentPlaceHolder1_gvUserInformation tbody tr").length + 1;
                totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:10%;\">" + gridObject.EmpCode + "</td>";
                tr += "<td align='left' style=\"width:20%;\">" + gridObject.EmployeeName + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + gridObject.TypeName + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + gridObject.LeaveMode + "</td>";

                tr += "<td align='left' style=\"width:10%;\">" + GetStringFromDateTime(gridObject.FromDate) + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + GetStringFromDateTime(gridObject.ToDate) + "</td>";

                tr += "<td align='left' style=\"width:10%;\">" + gridObject.LeaveStatus + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + gridObject.NoOfDays + "</td>";

                if (gridObject.IsCanEdit) {
                    editLink = "<a href=\"javascript:void();\" onclick=\"javascript:return PerformFillFormAction('" + gridObject.LeaveId + "', '" + result.GridPageLinks.CurrentPageNumber + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" /> </a>";
                }
                if (gridObject.IsCanDelete)
                    deleteLink = "<a href=\"javascript:void();\" onclick=\"javascript:return PerformDeleteAction('" + gridObject.LeaveId + "', '" + result.GridPageLinks.CurrentPageNumber + "');\"><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";

                if (gridObject.LeaveStatus == "Approved" && gridObject.ToDate < new Date() && IsCanDelete) {
                    tr += "<td align='center' style=\"width:10%;\">" + deleteLink + "</td>";
                }
                else {
                    tr += "<td align='center' style=\"width:10%;\">"
                    if (IsCanEdit) {
                        tr += editLink;
                    }
                    if (IsCanDelete) {
                        tr += deleteLink;
                    }
                    //tr += "<td align='center' style=\"width:10%;\">"
                    if (gridObject.IsCanCheck) {
                        tr += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return LeaveApprovalWithConfirmation('" + 'Checked' + "'," + gridObject.LeaveId + ")\" alt='Checked'  title='Checked' border='0' />";

                    }
                    if (gridObject.IsCanApprove) {
                        tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return LeaveApprovalWithConfirmation('" + 'Approved' + "', " + gridObject.LeaveId + ")\" alt='Approved'  title='Approved' border='0' />";
                    }
                    //tr += "</td>";
                    tr += "</td>"

                }

                tr += "</tr>"

                $("#ContentPlaceHolder1_gvUserInformation tbody ").append(tr);
                tr = "";

            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

        }
        function OnLeaveInfoLoadFailed(error) {

        }
        function LeaveApprovalWithConfirmation(ApprovedStatus, Id) {
            if (!confirm('Do you want to ' + (ApprovedStatus == 'Checked' ? 'check' : 'approve') + " ?")) {
                return false;
            }
            LeaveApproval(ApprovedStatus, Id);
        }
        function LeaveApproval(ApprovedStatus, Id) {

            PageMethods.LeaveApproval(Id, ApprovedStatus, OnApprovalSucceed, OnApprovalFailed);
        }

        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);

                GridPaging($("#GridPagingContainer").find("li.active").index(), 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnApprovalFailed() {

        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var empId = $("#ContentPlaceHolder1_employeeForLeaveSearch_hfEmployeeId").val();
            var leaveType = $("#ContentPlaceHolder1_ddlLeaveTypeIdSearch").val();
            var leaveMode = $("#ContentPlaceHolder1_ddlLeaveModeSearch").val();
            var fromDate = $("#ContentPlaceHolder1_txtFromDateSearch").val();
            var toDate = $("#ContentPlaceHolder1_txtToDateSearch").val();
            var leaveStatus = $("#ContentPlaceHolder1_ddlLeaveStatusSearch").val();

            if (leaveMode == '--- Please Select ---') {
                leaveMode = '';
            }

            if (leaveStatus == '--- Please Select ---') {
                leaveStatus = '';
            }

            //            if (empId == "0")
            //                empId = "";

            if (leaveType == "0")
                leaveType = "";

            var gridRecordsCount = $("#ContentPlaceHolder1_gvUserInformation tbody tr").length - 1;
            PageMethods.LoadLeaveInformation(empId, leaveType, leaveMode, fromDate, toDate, leaveStatus, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLeaveInfoLoadSucceed, OnLeaveInfoLoadFailed);
            return false;
        }

        //----Numeric Validation-----------------------
        function fixedlength(textboxID, keyEvent, maxlength) {
            //validation for digits upto 'maxlength' defined by caller function
            if (textboxID.value.length > maxlength) {
                textboxID.value = textboxID.value.substr(0, maxlength);
            }
            else if (textboxID.value.length < maxlength || textboxID.value.length == maxlength) {
                textboxID.value = textboxID.value.replace(/[^\d]+/g, '');
                return true;
            }
            else
                return false;
        }
        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            if (!confirm("Do You Want to Edit?"))
                return false;
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {

            if (IsCanEdit) {
                $('#ContentPlaceHolder1_btnSave').show();
            } else {
                $('#ContentPlaceHolder1_btnSave').hide();
            }

            $("#ContentPlaceHolder1_employeeeSearch_hfEmployeeId").val(result.EmpId);
            $("#ContentPlaceHolder1_employeeeSearch_txtSearchEmployee").val(result.EmpCode);
            $("#ContentPlaceHolder1_employeeeSearch_txtEmployeeName").val(result.EmployeeName);

            $("#<%=ddlLeaveMode.ClientID %>").val(result.LeaveMode).trigger('change');
            $("#<%=ddlLeaveTypeId.ClientID %>").val(result.LeaveTypeId);

            $("#<%=txtFromDate.ClientID %>").val(GetStringFromDateTime(result.FromDate));
            $("#<%=txtToDate.ClientID %>").val(GetStringFromDateTime(result.ToDate));

            $("#<%=txtNoOfDays.ClientID %>").val(result.NoOfDays);
            $("#<%=ddlLeaveStatus.ClientID %>").val(result.LeaveStatus);
            $("#<%=hfLeaveId.ClientID %>").val(result.LeaveId);
            $("#<%=btnSave.ClientID %>").val("Update");

            PageMethods.GetLeaveTakenNBalanceByEmployee(result.EmpId, OnLoadLeaveBalanceSuccess, OnLoadLeaveBalanceFailed);

            //$("#myTabs").tabs('select', 0);
            $("#myTabs").tabs({ active: 0 });

            return false;
        }
        function OnFillFormObjectFailed(error) {
            alert(error.get_message());
        }

        //For ClearForm-------------------------
        function PerformClearAction() {
            if (IsCanSave) {
                $('#ContentPlaceHolder1_btnSave').show();
            } else {
                $('#ContentPlaceHolder1_btnSave').hide();
            }
            $("#<%=ddlLeaveTypeId.ClientID %>").val(0);
            $("#<%=ddlReportingTo.ClientID %>").val("0").trigger('change');
            $("#<%=ddlLeaveMode.ClientID %>").val(0);
            $("#<%=txtFromDate.ClientID %>").val('');
            $("#<%=txtToDate.ClientID %>").val('');
            $("#<%=txtNoOfDays.ClientID %>").val('');
            $("#<%=ddlLeaveStatus.ClientID %>").val('--- Please Select ---');
            $("#<%=hfLeaveId.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");
            $("#ContentPlaceHolder1_employeeeSearch_txtSearchEmployee").val("");
            $("#ContentPlaceHolder1_employeeeSearch_txtEmployeeName").val("");
            $("#ContentPlaceHolder1_employeeeSearch_hfEmployeeId").val("0");
            return false;
        }

        function PerformClearActionWithConfirmation() {
            if (!confirm("Do You Want to Clear?"))
                return false;
            PerformClearAction();
        }
        //For Delete-------------------------        
        function PerformDeleteAction(actionId, pageNumber) {

            if (!confirm('Do you want to Delete?'))
                return false;

            PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
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

        function ReloadGrid(IsCurrentOrPreviousPage) {
            var currentPageNumber = $("#GridPagingContainer ul li[class='active']").text();

            if (currentPageNumber == "")
                currentPageNumber = 1;

            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }

        function WorkAfterSearchEmployee() {
            var employeeId = $("#ContentPlaceHolder1_employeeeSearch_hfEmployeeId").val();
            PageMethods.GetLeaveTakenNBalanceByEmployee(employeeId, OnLoadLeaveBalanceSuccess, OnLoadLeaveBalanceFailed);
        }

        function OnLoadLeaveBalanceSuccess(result) {
            $("#LeaveBalanceGrid tbody tr").remove();

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"3\" >No Data Found</td> </tr>";
                $("#LeaveBalanceGrid tbody").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0;

            $.each(result, function (count, gridObject) {

                totalRow = $("#LeaveBalanceGrid tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:40%;\">" + gridObject.LeaveTypeName + "</td>";
                tr += "<td align='left' style=\"width:30%;\">" + gridObject.TotalTakenLeave + "</td>";
                tr += "<td align='left' style=\"width:30%;\">" + gridObject.RemainingLeave + "</td>";
                tr += "</tr>"

                $("#LeaveBalanceGrid tbody").append(tr);
                tr = "";
            });

        }

        function OnLoadLeaveBalanceFailed() { }

    </script>
    <asp:HiddenField ID="hfLeaveId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Leave Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Leave</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">Employee Leave Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <UserControl:EmployeeSearch ID="employeeeSearch" runat="server" />
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblLeaveMode" runat="server" class="control-label required-field" Text="Leave Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlLeaveTypeId" runat="server" CssClass="form-control"
                                    TabIndex="4">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblLeaveStatus" runat="server" class="control-label required-field" Text="Leave Status"></asp:Label>
                                <div style="display: none;">
                                    <asp:Label ID="lblLeaveTypeId" runat="server" class="control-label required-field" Text="Leave Mode"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlLeaveStatus" runat="server" CssClass="form-control required-field"
                                    TabIndex="9">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFromDate" runat="server" class="control-label required-field" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TabIndex="6" autocomplete="off"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblToDate" runat="server" class="control-label required-field" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TabIndex="7" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblNoOfDays" runat="server" class="control-label required-field" Text="No Of Days"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNoOfDays" runat="server" CssClass="form-control" TabIndex="8"
                                    onblur="return fixedlength(this, event, 2);" onkeypress="return fixedlength(this, event, 2);"
                                    onkeyup="return fixedlength(this, event, 2);"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Leave Mode"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlLeaveMode" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="reportingTo" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="lblReportingTo" runat="server" class="control-label" Text="Approved By"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlReportingTo" TabIndex="10" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <%--Right Left--%>
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                                    TabIndex="11" OnClientClick="return PerformSaveAction();" />
                                <input type="button" class="TransactionalButton btn btn-primary" id="btnClear" value="Clear" onclick="PerformClearActionWithConfirmation()" />
                                <%--<asp:Button ID="btnClear" runat="server" TabIndex="12" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                                    OnClientClick="javascript: return PerformClearActionWithConfirmation();" />--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="Div1" class="panel panel-default">
                <div class="panel-heading">Leave Balance Info</div>
                <div class="panel-body">
                    <table id="LeaveBalanceGrid" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
                        <colgroup>
                            <col style="width: 40%;" />
                            <col style="width: 30%;" />
                            <col style="width: 30%;" />
                        </colgroup>
                        <thead style="background-color: #44545E; color: #fff; text-align: left;">
                            <tr>
                                <th>Leave Type
                                </th>
                                <th>Total Leave Taken
                                </th>
                                <th>Leave Remaining
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">Search Leave Info</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <UserControl:EmployeeForLeaveSearch ID="employeeForLeaveSearch" runat="server" />
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblLeaveModeSearch" runat="server" class="control-label" Text="Leave Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlLeaveTypeIdSearch" runat="server" CssClass="form-control"
                                    TabIndex="4">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label" Text="Leave Status"></asp:Label>

                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlLeaveStatusSearch" runat="server" CssClass="form-control"
                                    TabIndex="9">
                                </asp:DropDownList>

                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblLeaveTypeIdSearch" runat="server" class="control-label" Text="Leave Mode"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlLeaveModeSearch" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFromDateSearch" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtToDateSearch" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">Search Information</div>
                <div class="panel-body">
                    <asp:GridView ID="gvUserInformation" Width="100%" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                        TabIndex="13" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:BoundField DataField="EmpCode" HeaderText="ID" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EmployeeName" HeaderText="Employee Name" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TypeName" HeaderText="Leave Type" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LeaveMode" HeaderText="Leave Mode" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FromDate" HeaderText="From Date" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ToDate" HeaderText="To Date" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LeaveStatus" HeaderText="Leave Status" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NoOfDays" HeaderText="No of Days" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="10%">
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
