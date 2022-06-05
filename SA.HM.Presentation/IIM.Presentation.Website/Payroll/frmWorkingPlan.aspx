<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmWorkingPlan.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmWorkingPlan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Attendance & Time Keeping</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Working Plan</li>";
            var breadCrumbs = moduleName + formName;

            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $('#ContentPlaceHolder1_txtStartHour').timepicker({
                showPeriod: is12HourFormat
            });
            $('#ContentPlaceHolder1_txtEndHour').timepicker({
                showPeriod: is12HourFormat
            });

            $("#<%=ddlWorkingPlan.ClientID %>").change(function () {
                if ($("#<%=ddlWorkingPlan.ClientID %>").val() == "Fixed") {
                    $('#FixedWorkingPlanInfoDiv').show("slow");
                }
                else {
                    $('#FixedWorkingPlanInfoDiv').hide("slow");
                }
            });

        });

        function CheckValidity() {
            var regex = /^(?:(?:0?\d|1[0-2]):[0-5]\d)+$/;
            var planType = $("#<%=ddlWorkingPlan.ClientID %>").val();
            var start = $("#<%=txtStartHour.ClientID %>").val();
            var end = $("#<%=txtEndHour.ClientID %>").val();
            var offOne = $("#<%=ddlDayOffOne.ClientID %>").val();
            var offTwo = $("#<%=ddlDayOffTwo.ClientID %>").val();

            //if ((start.includes("AM")) || (start.includes("PM")) ) {
            //    start = start.slice(0, -3);
            //}
            //else if ((end.includes("AM")) || (end.includes("PM"))) {
            //    end = end.slice(0, -3);
            //}
            if (start == "" && planType == "Fixed") {
                toastr.warning("Please provide start hour.");
                $("#<%=txtStartHour.ClientID %>").focus();
                return false;
            }
            else if (end == "" && planType == "Fixed") {
                toastr.warning("Please provide end hour");
                $("#<%=txtEndHour.ClientID %>").focus();
                return false;
            }
            else if (planType == "Fixed") {
                var isValidStart = CommonHelper.IsValidTime(start);
                var isValidEnd = CommonHelper.IsValidTime(end);


                if (!isValidStart) {
                    toastr.warning("Please provide correct time format");
                    $("#<%=txtStartHour.ClientID %>").val("");
                    $("#<%=txtStartHour.ClientID %>").focus();
                    return false;
                }
                else if (!isValidEnd) {
                    toastr.warning("Please provide correct time format");
                    $("#<%=txtEndHour.ClientID %>").val("");
                    $("#<%=txtEndHour.ClientID %>").focus();
                    return false;
                }
            }
            if (offOne != "None" && offTwo != "None" && planType == "Fixed") {
                if (offOne == offTwo) {
                    toastr.warning("Both Off Days Cannot Be Same.");
                    return false;
                }
            }


            return true;
        }

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {
            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewBank').hide("slow");
            $('#EntryPanel').show("slow");
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
            window.location = "frmWorkingDay.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            alert(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=ddlCategoryId.ClientID %>").val(0);
            $("#<%=ddlWorkingPlan.ClientID %>").val('Fixed');
            $("#<%=txtStartHour.ClientID %>").val('');


            $("#<%=txtEndHour.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").text('Save');

            $("#<%=ddlDayOffOne.ClientID %>").val('None');
            $("#<%=ddlDayOffTwo.ClientID %>").val('None');
            $("#<%=btnSave.ClientID %>").val("Save");

            $("#ContentPlaceHolder1_txtWorkingDayId").val("");
            //$("#form1")[0].reset();
            return false;
        }

        function PerformClearActionWithConfirmation() {
            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            PerformClearAction();
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewBank').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewBank').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }

        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewBank').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewBank').hide("slow");
        }
        $(function () {
            $("#myTabs").tabs();
        });
    </script>

    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Working Plan Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Working Plan</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Working Plan Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="txtWorkingDayId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblCategoryId" runat="server" class="control-label required-field" Text="Employee Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCategoryId" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblWorkingPlan" runat="server" class="control-label" Text="Plan Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlWorkingPlan" CssClass="form-control" runat="server">
                                    <asp:ListItem>Fixed</asp:ListItem>
                                    <asp:ListItem>Roster</asp:ListItem>
                                    <asp:ListItem>Attendance</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="FixedWorkingPlanInfoDiv">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lbltxtStart" runat="server" class="control-label required-field" Text="Start Time"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtStartHour" placeholder="10" TabIndex="9" CssClass="form-control"
                                        runat="server"></asp:TextBox>
                                    <%--<asp:TextBox ID="txtStartMinute" placeholder="00" CssClass="CustomMinuteSize" TabIndex="10"
                                        runat="server"></asp:TextBox>
                                    <asp:DropDownList ID="ddlStartAMPM" CssClass="CustomAMPMSize" runat="server" TabIndex="11">
                                        <asp:ListItem>AM</asp:ListItem>
                                        <asp:ListItem>PM</asp:ListItem>
                                    </asp:DropDownList>
                                    (10:00AM)--%>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblEndTime" runat="server" class="control-label required-field" Text="End Time"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtEndHour" placeholder="06" TabIndex="9" CssClass="form-control"
                                        runat="server"></asp:TextBox>
                                    <%--<asp:TextBox ID="txtEndMinute" placeholder="00" CssClass="CustomMinuteSize" TabIndex="10"
                                        runat="server"></asp:TextBox>
                                    <asp:DropDownList ID="ddlEndAMPM" CssClass="CustomAMPMSize" runat="server" TabIndex="11">
                                        <asp:ListItem>AM</asp:ListItem>
                                        <asp:ListItem>PM</asp:ListItem>
                                    </asp:DropDownList>
                                    (06:00PM)--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblDayOffOne" runat="server" class="control-label" Text="Day Off One"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlDayOffOne" runat="server" CssClass="form-control" TabIndex="3">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblDayOffTwo" runat="server" class="control-label" Text="Day Off Two"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlDayOffTwo" runat="server" CssClass="form-control" TabIndex="4">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSave_Click" OnClientClick="javascript:return CheckValidity()" TabIndex="4" />
                                <input type="button" id="btnClear" onclick="PerformClearActionWithConfirmation()" class="TransactionalButton btn btn-primary btn-sm" value="Clear"/>
                                <%--<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearActionWithConfirmation();" TabIndex="5" />--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Working Plan Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSCategory" runat="server" class="control-label" Text="Employee Type"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSCategory" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSDayOffOne" runat="server" class="control-label" Text="Day Off One"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSDayOffOne" runat="server" CssClass="form-control" TabIndex="3">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSDayOffTwo" runat="server" class="control-label" Text="Day Off Two"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddSDayOffTwo" runat="server" CssClass="form-control" TabIndex="4">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSearch_Click" TabIndex="4" />
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
                    <asp:GridView ID="gvWorkingDay" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        GridLines="None" PageSize="30" AllowSorting="True" ForeColor="#333333"
                        OnPageIndexChanging="gvWorkingDay_PageIndexChanging" OnRowDataBound="gvWorkingDay_RowDataBound"
                        OnRowCommand="gvWorkingDay_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("WorkingDayId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TypeName" HeaderText="Category Name" ItemStyle-Width="35%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DayOffOne" HeaderText="Day Off One" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DayOffTwo" HeaderText="Day Off Two" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("WorkingDayId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" OnClientClick="return confirm('Do you want to Edit?');" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("WorkingDayId") %>' ImageUrl="~/Images/delete.png"
                                        Text="" AlternateText="Delete" ToolTip="Delete" OnClientClick="return confirm('Do you want to Delete?');" />
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
    <script type="text/javascript">
        var xNewAdd = '<%=isFixedWorkingPlanInfoDivEnable%>';
        if (xNewAdd > -1) {
            $('#FixedWorkingPlanInfoDiv').show("slow");
        }
        else {
            $('#FixedWorkingPlanInfoDiv').hide("slow");
        }
    </script>
</asp:Content>
