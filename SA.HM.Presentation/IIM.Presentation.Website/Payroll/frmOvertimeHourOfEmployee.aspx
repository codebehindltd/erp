<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmOvertimeHourOfEmployee.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmOvertimeHourOfEmployee" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithDesignation.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="EmployeeForLeaveSearch" Src="~/HMCommon/UserControl/EmployeeSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Over Time</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#myTabs").tabs();

            $('#ContentPlaceHolder1_txtAttenDanceDate').datepicker("option", {
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });


            $("#<%=ddlOverTimeProcess.ClientID %>").change(function () {
                if ($("#<%=ddlOverTimeProcess.ClientID %>").val() == 0) {
                    $('#DepartmentInfo').hide("slow");
                    $('#IndividualEmployeeInfo').show("slow");
                }
                else {
                    $('#IndividualEmployeeInfo').hide("slow");
                    $('#DepartmentInfo').show("slow");
                }
            });
        });

        function PerformSaveAction() {

            var EmpOverTime = new Array();
            var empId = '', overTimeDate = '', entryTime = '', exitTime = '', totalHour = '0', oTHour = '0';
            var approvedOTHour = '0', departmentId = '', attendanceDate = '';

            empId = $("#ContentPlaceHolder1_employeeeSearch_hfEmployeeId").val();
            departmentId = $("#ContentPlaceHolder1_ddlDepartment").val();
            attendanceDate = $("#ContentPlaceHolder1_txtAttenDanceDate").val();

            $("#OvertimeTbl tbody tr").each(function () {

                empId = $.trim($(this).find("td:eq(0)").text());
                overTimeDate = $.trim($(this).find("td:eq(5)").text());
                entryTime = $.trim($(this).find("td:eq(11)").text());
                exitTime = $.trim($(this).find("td:eq(12)").text());
                totalHour = $.trim($(this).find("td:eq(8)").text());

                oTHour = $.trim($(this).find("td:eq(9)").text());
                approvedOTHour = $.trim($(this).find("td:eq(10)").find("input").val());

                EmpOverTime.push({
                    OverTimeId: '0',
                    EmpId: empId,
                    OverTimeDate: overTimeDate,
                    EntryTime: entryTime,
                    ExitTime: exitTime,
                    TotalHour: totalHour,
                    OTHour: oTHour,
                    ApprovedOTHour: approvedOTHour
                });

            });

            PageMethods.OvertimeSave(EmpOverTime, OnSaveOvertimeSucceeded, OnSaveOvertimeFailed);

            return false;
        }
        function OnSaveOvertimeSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $("#frmHotelManagement")[0].reset();
                //ClearForm();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnSaveOvertimeFailed(error) { }


        function WorkAfterSearchEmployee() {

        }

    </script>
    <div style="height: 45px">
    </div>
    <asp:HiddenField ID="hfTransferId" runat="server" Value=""></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="entry" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-1">Employee Overtime</a></li>
            <li id="search" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-2">Overtime search</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Employee Overtime</a>
                <div class="HMBodyContainer">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblOverTimeProcess" runat="server" Text="Over Time Process"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlOverTimeProcess" runat="server" CssClass="customMediumDropDownSize"
                                TabIndex="5">
                                <asp:ListItem Value="0">Individual Employee</asp:ListItem>
                                <asp:ListItem Value="1">Department</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="Label6" runat="server" Text="Attendance Date"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtAttenDanceDate" runat="server" CssClass="datepicker" TabIndex="7"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div id="IndividualEmployeeInfo">
                        <UserControl:EmployeeSearch ID="employeeeSearch" runat="server" />
                    </div>
                    <div class="divClear">
                    </div>
                    <div id="DepartmentInfo" class="divSection" style="display: none;">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblLeaveTypeId" runat="server" Text="Department"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="ThreeColumnDropDownList"
                                TabIndex="5">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="HMContainerRowButton">
                        <asp:Button ID="btnSearchOt" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary"
                            OnClick="btnSearchOt_Click" />
                    </div>
                    <div class="divClear">
                    </div>
                    <div>
                        <asp:Literal ID="litOtDetails" runat="server"></asp:Literal>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="HMContainerRowButton">
                        <%--Right Left--%>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                            TabIndex="11" OnClientClick="return PerformSaveAction();" OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" TabIndex="12" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                            OnClientClick="javascript: return PerformClearAction();" />
                    </div>
                </div>
            </div>
            <div class="divClear">
            </div>
        </div>
        <div id="tab-2">
            <div class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Transfer Search
                </a>
                <div class="HMBodyContainer">
                    <div class="divClear">
                    </div>
                    <UserControl:EmployeeForLeaveSearch ID="employeeForLeaveSearch" runat="server" />
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="Label1" runat="server" Text="Joined Date"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtDateFrom" runat="server" CssClass="datepicker" TabIndex="6"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="Label2" runat="server" Text="Reporting To"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtDateTo" runat="server" CssClass="datepicker" TabIndex="6"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="HMContainerRowButton">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary"
                            OnClick="btnSearch_Click" />
                    </div>
                    <div class="divClear">
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Information
                </a>
                <div class="block-body collapse in">
                    <asp:GridView ID="gvEmployeeTransfer" Width="100%" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                        TabIndex="13" OnRowCommand="gvEmployeeTransfer_RowCommand" OnRowDataBound="gvEmployeeTransfer_RowDataBound">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:BoundField DataField="EmployeeName" HeaderText="Employee Name" ItemStyle-Width="25">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Transfer Date" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("TransferDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="PreviousDepartmentName" HeaderText="Previous Department"
                                ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CurrentDepartmentName" HeaderText="Current Departmen"
                                ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Reporting Date" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ReportingDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    &nbsp;<asp:ImageButton ID="ImgApproved" runat="server" CausesValidation="False" CommandName="CmdApproved"
                                        CommandArgument='<%# bind("TransferId") %>' ImageUrl="~/Images/approved.png"
                                        Text="" AlternateText="Details" ToolTip="Approve Item" />
                                    &nbsp;&nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False"
                                        CommandName="CmdDelete" CommandArgument='<%# bind("TransferId") %>' ImageUrl="~/Images/cancel.png"
                                        Text="" AlternateText="Details" ToolTip="Cancel Item" />
                                    &nbsp;<asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("TransferId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                </ItemTemplate>
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
                    <div class="divClear">
                    </div>
                    <div class="childDivSection">
                        <div class="pagination pagination-centered" id="GridPagingContainer">
                            <ul>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#<%=ddlOverTimeProcess.ClientID %>").val() == 0) {
                $('#DepartmentInfo').hide();
                $('#IndividualEmployeeInfo').show();
            }
            else {
                $('#IndividualEmployeeInfo').hide();
                $('#DepartmentInfo').show();
            }

        });
    </script>
</asp:Content>
