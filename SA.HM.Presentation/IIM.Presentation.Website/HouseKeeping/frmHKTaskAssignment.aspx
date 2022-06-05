<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmHKTaskAssignment.aspx.cs" Inherits="HotelManagement.Presentation.Website.HouseKeeping.frmHKTaskAssignment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            CommonHelper.ApplyIntValidation();
            CommonHelper.ApplyDecimalValidation();
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>House Keeping</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Task Assignment</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#myTabs").tabs();
            $('#RoomInformation').show();

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("[id=ContentPlaceHolder1_gvTaskAssignment_ChkCreate]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#ContentPlaceHolder1_gvTaskAssignment tbody tr").find("td:eq(0) > span").find("input").prop("checked", true);
                }
                else {
                    $("#ContentPlaceHolder1_gvTaskAssignment tbody tr").find("td:eq(0) > span").find("input").prop("checked", false);
                }
            });

            $("[id=ContentPlaceHolder1_gvEmployee_ChkCreate]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#ContentPlaceHolder1_gvEmployee tbody tr").find("td:eq(0) > span").find("input").prop("checked", true);
                }
                else {
                    $("#ContentPlaceHolder1_gvEmployee tbody tr").find("td:eq(0) > span").find("input").prop("checked", false);
                }
            });

            $("#ContentPlaceHolder1_ddlStatusType").change(function () {
                if ($(this).val() == "1") {
                    $('#FloorInformation').show();
                    $('#RoomInformation').hide();
                }
                else {
                    $('#FloorInformation').hide();
                    $('#RoomInformation').show();
                }
            });

            if ($("#ContentPlaceHolder1_hfStatusType").val() == "1") {
                $('#FloorInformation').show();
                $('#RoomInformation').hide();
            }
            else {
                $('#FloorInformation').hide();
                $('#RoomInformation').show();
            }
            <%-- $("#<%=ddlStatusType.ClientID %>").change(function () {
                var statusType = $("#<%=ddlStatusType.ClientID %>").val();
                if (statusType == "0") {
                    $("#<%=ddlSrcFloorId.ClientID %>").val(0);
                }
            });--%>


        });

        function Validate() {
            if ($("#ContentPlaceHolder1_gvTaskAssignment tbody tr").find("td:eq(0)").find("input").is(":checked") == true && $("#ContentPlaceHolder1_gvEmployee tbody tr").find("td:eq(0)").find("input").is(":checked") == true)
                return true;
            else
            {
                toastr.warning("Select Task and Employee");
                return false;
            }
               

        }

    </script>

    <asp:HiddenField ID="hfTaskId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Task Assignment</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Task Assignment Search</a></li>
        </ul>
        <div id="tab-1">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="LblShift" runat="server" class="control-label" Text="Shift"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlShift" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Value="AM">AM</asp:ListItem>
                                    <asp:ListItem Value="PM">PM</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblActiveStat" runat="server" class="control-label" Text="Search Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:HiddenField ID="hfStatusType" runat="server" />
                                <asp:DropDownList ID="ddlStatusType" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Value="0">Room Number Wise</asp:ListItem>
                                    <asp:ListItem Value="1">Floor Wise</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFOStatus" runat="server" class="control-label" Text="FO Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlFOStatus" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Value="0">All</asp:ListItem>
                                    <asp:ListItem Value="1">Vacant</asp:ListItem>
                                    <asp:ListItem Value="2">Occupied</asp:ListItem>
                                    <asp:ListItem Value="3">Out of Order</asp:ListItem>
                                    <asp:ListItem Value="4">Out of Service</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="RoomInformation" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Room Number From"></asp:Label>
                            </div>
                            <div class="col-md-4 ">
                                <asp:TextBox ID="txtRoomNumber" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="Room Number To"></asp:Label>
                            </div>
                            <div class="col-md-4 ">
                                <asp:TextBox ID="txtRoomNumberTo" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" id="FloorInformation" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="lblSrcFloorId" runat="server" class="control-label" Text="Floor Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSrcFloorId" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" CssClass="control-label" Text="Block"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlFloorBlock" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" TabIndex="4" CssClass="btn btn-primary btn-sm"
                                OnClick="btnSearch_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="RoomInformationDiv" class="panel panel-default" style="display: none;">
                <div class="panel-heading">Room Info</div>
                <div class="panel-body">
                    <div id="TaskAssignment" style="height: 550px; overflow-y: scroll;">
                        <asp:GridView ID="gvTaskAssignment" Width="100%" runat="server" AllowPaging="True"
                            AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                            ForeColor="#333333" PageSize="1000" OnPageIndexChanging="gvTaskAssignment_PageIndexChanging"
                            OnRowDataBound="gvTaskAssignment_RowDataBound" CssClass="table table-bordered table-condensed table-responsive">
                            <RowStyle BackColor="#E3EAEB" />
                            <Columns>
                                <asp:TemplateField HeaderText="IDNO" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaskId" runat="server" Text='<%#Eval("TaskId") %>'></asp:Label>
                                        <asp:Label ID="lblRoomTaskId" runat="server" Text='<%#Eval("RoomTaskId") %>'></asp:Label>
                                        <asp:Label ID="lblRoomId" runat="server" Text='<%#Eval("RoomId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Create/Update" ItemStyle-Width="02%">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="ChkCreate" CssClass="ChkCreate" runat="server" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsSavePermission" CssClass="Chk_Create" runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="RoomNumber" HeaderText="Room" ItemStyle-Width="10%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RoomType" HeaderText="Type" ItemStyle-Width="10%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="FO Status" ShowHeader="False" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFORoomStatus" runat="server" Text='<%# Eval("FORoomStatus") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DateIn" HeaderText="Check In" ItemStyle-Width="10%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DateOut" HeaderText="Ex. Check Out" ItemStyle-Width="10%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="HK Status" ShowHeader="False" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHKRoomStatus" runat="server" Text='<%# Eval("HKRoomStatus") %>'
                                            Visible="false" />
                                        <asp:DropDownList ID="ddlHKRoomStatus" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Task Instructions" ShowHeader="False" ItemStyle-Width="40%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTaskInstructions" runat="server" CssClass="form-control" Text='<%#Eval("TaskDetails") %>'></asp:TextBox>
                                    </ItemTemplate>
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
            <div id="EmployeeInformationDiv" class="panel panel-default" style="display: none;">
                <div class="panel-heading">Employee Info</div>
                <div class="panel-body">
                    <div id="EmployeeAssignment" style="height: 350px; overflow-y: scroll;">
                        <asp:GridView ID="gvEmployee" Width="100%" runat="server" AllowPaging="True"
                            AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                            ForeColor="#333333" PageSize="1000"
                            CssClass="table table-bordered table-condensed table-responsive" OnRowDataBound="gvEmployee_RowDataBound">
                            <RowStyle BackColor="#E3EAEB" />
                            <Columns>
                                <asp:TemplateField HeaderText="IDNO" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaskId" runat="server" Text='<%#Eval("TaskId") %>'></asp:Label>
                                        <asp:Label ID="lblEmpTaskId" runat="server" Text='<%#Eval("EmpTaskId") %>'></asp:Label>
                                        <asp:Label ID="lblEmpId" runat="server" Text='<%#Eval("EmpId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Create/Update" ItemStyle-Width="02%">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="ChkCreate" CssClass="ChkCreate" runat="server" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsSavePermission" CssClass="Chk_Create" runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="DisplayName" HeaderText="Employee Name" ItemStyle-Width="30%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="EmpType" HeaderText="Employee Type" ItemStyle-Width="15%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Designation" HeaderText="Designation" ItemStyle-Width="15%">
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
            <div id="SaveDiv" class="row" style="display: none;">
                <div class="col-md-12">
                    <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="4" CssClass="btn btn-primary btn-sm" OnClientClick="return Validate()"
                        OnClick="btnSave_Click" />
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label5" runat="server" class="control-label" Text="Shift"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlShiftForSearch" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Value="AM">AM</asp:ListItem>
                                    <asp:ListItem Value="PM">PM</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearchTaskAssignment" runat="server" Text="Search" TabIndex="4" CssClass="btn btn-primary btn-sm" OnClick="btnSearchTaskAssignment_Click" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:GridView ID="gvTaskAssignmentList" Width="100%" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True"
                                    ForeColor="#333333"
                                    CssClass="table table-bordered table-condensed table-responsive" OnRowCommand="gvTaskAssignmentList_RowCommand">
                                    <RowStyle BackColor="#E3EAEB" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTaskId" runat="server" Text='<%#Eval("TaskId") %>'></asp:Label>
                                                <asp:Label ID="lblTaskSequence" runat="server" Text='<%#Eval("TaskSequence") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Assign Date" ItemStyle-Width="35%" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="Label115" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("AssignDate"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TaskSequence" HeaderText="Task Sequence" ItemStyle-Width="25%">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Shift" HeaderText="Shift" ItemStyle-Width="25%">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgApproved" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                                    CommandArgument='<%# bind("TaskId") %>' ImageUrl="~/Images/edit.png"
                                                    AlternateText="Approved" ToolTip="Approved Salary Process" />
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
    </div>
    <script type="text/javascript">
        var x = '<%=IsSearchSuccess%>';
        if (x > -1) {
            $('#EmployeeInformationDiv').show();
            $('#RoomInformationDiv').show();
            $('#SaveDiv').show();
        }
    </script>
</asp:Content>
