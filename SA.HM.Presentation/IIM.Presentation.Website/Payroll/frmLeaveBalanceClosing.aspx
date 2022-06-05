<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmLeaveBalanceClosing.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmLeaveBalanceClosing" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<asp:Content ID="header" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Leave Balance Closing</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#myTabs").tabs();

            $("#btnLeaveBalanceSearch").click(function () {
                $("#LeaveBalance").show();
            });

        });

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {
            return false;
        }
        function OnFillFormObjectFailed(error) {
            alert(error.get_message());
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
            window.location = "frmLeaveType.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            alert(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {

            return false;
        }

        function ValidateLeaveType() {

            return true;
        }

        function WorkAfterSearchEmployee() { }

    </script>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Leave Clossing</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search</a></li>
        </ul>
        <div id="tab-1">
            <div class="panel panel-default">
                <div class="panel-heading">Leave Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <UserControl:EmployeeSearch ID="employeeSearch" runat="server" />
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Button ID="btnLeaveBalanceSearch" runat="server" Text="Search" TabIndex="2"
                                    CssClass="TransactionalButton btn btn-primary" OnClick="btnSearch_Click" />
                            </div>
                        </div>
                        <div id="LeaveBalance" class="row">
                            <div class="col-md-12">
                                <asp:GridView ID="gvLeaveBalance" Width="100%" runat="server" AutoGenerateColumns="False"
                                    CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                                    TabIndex="13" CssClass="table table-bordered table-condensed table-responsive"
                                    OnRowCommand="gvLeaveBalance_RowCommand">
                                    <RowStyle BackColor="#E3EAEB" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblid" runat="server" Text='<%#Eval("EmpId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="LeaveType" HeaderText="Leave Type" ItemStyle-Width="10%">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="YearlyLeave" HeaderText="Yearly Leave" ItemStyle-Width="10%">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LeaveTaken" HeaderText="Leave Taken" ItemStyle-Width="10%">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RemainingLeave" HeaderText="Remaining Leave" ItemStyle-Width="10%">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MaxDayCanKeepAsCarryForwardLeave" HeaderText="Max Day Can Keep As Carry Forward Leave" ItemStyle-Width="15%">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MaxDayCanCarryForwardYearly" HeaderText="Max Day Can Carry Forward Yearly" ItemStyle-Width="15%">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CarryForwardedLeave" HeaderText="Carry Forwarded Leave" ItemStyle-Width="15%">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MaxDayCanEncash" HeaderText="Max Day Can Encash" ItemStyle-Width="15%">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TotalCarryforwardLeave" HeaderText="Total Carry forward Leave" ItemStyle-Width="15%">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgDetailsApproved" runat="server" CausesValidation="False" CommandName="CmdApprove"
                                                    CommandArgument='<%# bind("LeaveTypeId") %>' ImageUrl="~/Images/approved.png" Text=""
                                                    AlternateText="approve" ToolTip="approve" />
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
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">Search Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <asp:GridView ID="gvGuestHouseService" Width="100%" runat="server" AutoGenerateColumns="False"
                            CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                            TabIndex="13" CssClass="table table-bordered table-condensed table-responsive">
                            <RowStyle BackColor="#E3EAEB" />
                            <Columns>
                                <asp:TemplateField HeaderText="IDNO" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblid" runat="server" Text='<%#Eval("LeaveTypeId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TypeName" HeaderText="Type Name" ItemStyle-Width="35%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MaxDayCanCarryForwardYearly" HeaderText="Carry Forward"
                                    ItemStyle-Width="25%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ActiveStatus" HeaderText="Status" ItemStyle-Width="25%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                            CommandArgument='<%# bind("LeaveTypeId") %>' ImageUrl="~/Images/edit.png" Text=""
                                            AlternateText="Edit" ToolTip="Edit" />
                                        &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                            CommandArgument='<%# bind("LeaveTypeId") %>' ImageUrl="~/Images/delete.png" Text=""
                                            AlternateText="Delete" ToolTip="Delete" OnClientClick="return confirm('Do you want to Delete?');" />
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
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
