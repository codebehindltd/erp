<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpLeaveApplication.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpLeaveApplication" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Leave Application</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });

        $(document).ready(function () {

            $("#myTabs").tabs();

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
        });


        function PerformSaveAction() {

            if ($("#ContentPlaceHolder1_ddlLeaveTypeId").val() == "0") {
                toastr.warning("Please Select Leave Type");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlLeaveMode").val() == "0") {
                toastr.warning("Please Select Leave Mode");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtFromDate").val() == "0") {
                toastr.warning("Please Give From Date");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtToDate").val() == "0") {
                toastr.warning("Please Give To Date");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtRemarks").val() == "0") {
                toastr.warning("Please Give Reason");
                return false;
            }

            return true;
        }

    </script>
    <asp:HiddenField ID="hfLeaveId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfEmpId" runat="server" Value=""></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="entry" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-1">Leave Application</a></li>
            <li id="search" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-2">Leave Application Search</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Employee Leave Application</div>
                <div class="panel-body">
                    <div class="form-horizontal">
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
                                <asp:Label ID="lblLeaveTypeId" runat="server" class="control-label required-field" Text="Leave Mode"></asp:Label>                                
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlLeaveMode" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                            </div>
                        </div>
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
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblNoOfDays" runat="server" class="control-label required-field" Text="No Of Days"></asp:Label>                                
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNoOfDays" runat="server" CssClass="form-control" TabIndex="8"
                                    ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblReportingTo" runat="server" class="control-label" Text="Reporting To"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlReportingTo" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Application Body"></asp:Label>                                
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="multiline" CssClass="form-control"
                                    TabIndex="8">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <%--Right Left--%>
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    TabIndex="11" OnClientClick="return PerformSaveAction();" OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="12" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearAction();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="Div1" class="panel panel-default">
                <div class="panel-heading">
                    Leave Balance Info</div>
                <div class="panel-body">
                    <asp:GridView ID="gvLeaveBalance" Width="100%" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" TabIndex="9"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("EmpId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="LeaveTypeName" HeaderText="Leave Type" ItemStyle-Width="50%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TotalTakenLeave" HeaderText="Total Taken Leave" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RemainingLeave" HeaderText="Remaining Leave" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
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
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Search Leave Application</div>
                <div class="panel-body">
                    <div class="form-horizontal">
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
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label41" runat="server" class="control-label" Text="Leave Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlLeaveStatusSearch" runat="server" CssClass="form-control"
                                    TabIndex="9">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnSearch_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default" runat="server">
                <div class="panel-heading">
                    Search Information</div>
                <div class="panel-body">
                    <asp:GridView ID="gvEmployeeLeave" Width="100%" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" TabIndex="13"
                        OnRowDataBound="gvEmployeeLeave_RowDataBound" OnRowCommand="gvEmployeeLeave_RowCommand"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:BoundField DataField="TypeName" HeaderText="Leave Type" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LeaveMode" HeaderText="Leave Mode" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Date From" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="Label432" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("FromDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date To" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="Label421" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ToDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="NoOfDays" HeaderText="No of Days" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LeaveStatus" HeaderText="Leave Status" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    &nbsp;<asp:ImageButton ID="ImgApproved" OnClientClick="return confirm('Do you want to approve?');"  runat="server" CausesValidation="False" CommandName="CmdApproved"
                                        CommandArgument='<%# bind("LeaveId") %>' ImageUrl="~/Images/approved.png" Text=""
                                        AlternateText="Details" ToolTip="Approve Item" />
                                    &nbsp;&nbsp;<asp:ImageButton ID="ImgCancel" OnClientClick="return confirm('Do you want to cancel?');"  runat="server" CausesValidation="False"
                                        CommandName="CmdCancel" CommandArgument='<%# bind("LeaveId") %>' ImageUrl="~/Images/cancel.png"
                                        Text="" AlternateText="Details" ToolTip="Cancel Item" />
                                    &nbsp;<asp:ImageButton ID="ImgUpdate" OnClientClick="return confirm('Do you want to update?');"  runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("LeaveId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;
                                    <asp:ImageButton ID="ImgApplicationPreview" runat="server" CausesValidation="False"
                                        CommandArgument='<%# bind("LeaveId") %>' CommandName="CmdApplicationPreview"
                                        ImageUrl="~/Images/ReportDocument.png" Text="" AlternateText="Application" ToolTip="Application" />
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
