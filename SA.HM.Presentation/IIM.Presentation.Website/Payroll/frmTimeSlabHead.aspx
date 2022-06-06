<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmTimeSlabHead.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmTimeSlabHead" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Attendance & Time Keeping</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Time Slab Head</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $('#ContentPlaceHolder1_txtSlabStartHour').timepicker({
                showPeriod: is12HourFormat
            });
            $('#ContentPlaceHolder1_txtSlabEndTimeHour').timepicker({
                showPeriod: is12HourFormat
            });
            $("#<%=txtSlabStartHour.ClientID%>").blur(function () {
                if ($("#<%=txtSlabStartHour.ClientID %>").val() != "") {
                    var isValid = CommonHelper.IsValidTime($("#<%=txtSlabStartHour.ClientID %>").val());
                    if (!isValid) {
                        toastr.warning("Please Provide Correct Time Format.");
                        $("#<%=txtSlabStartHour.ClientID %>").focus();
                        $("#<%=txtSlabStartHour.ClientID %>").val("");
                     return false;
                 }
                }
                
            });
            $("#<%=txtSlabEndTimeHour.ClientID %>").blur(function () {
                if ($("#<%=txtSlabEndTimeHour.ClientID %>").val() != "") {
                    var isValid = CommonHelper.IsValidTime($("#<%=txtSlabEndTimeHour.ClientID %>").val());
                    if (!isValid) {
                        toastr.warning("Please Provide Correct Time Format.");
                        $("#<%=txtSlabEndTimeHour.ClientID %>").focus();
                        $("#<%=txtSlabEndTimeHour.ClientID %>").val("");
                     return false;
                 }
                }
                
             });
        });
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
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {

            $("#<%=txtTimeSlabId.ClientID %>").val(result.TimeSlabId);
            $("#<%=ddlActiveStat.ClientID %>").val(result.ActiveStat == true ? 0 : 1);
            $("#<%=txtTimeSlabHead.ClientID %>").val(result.TimeSlabHead);

            $("#<%=txtSlabStartHour.ClientID %>").val(result.StartHour);                     
            $("#<%=txtSlabEndTimeHour.ClientID %>").val(result.EndHour);            

            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewTimeSlab').hide("slow");
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
            window.location = "frmTimeSlabHead.aspx?DeleteConfirmation=Deleted"
        }
        function OnDeleteObjectFailed(error) {
            alert(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=txtTimeSlabId.ClientID %>").val('');
            $("#<%=ddlActiveStat.ClientID %>").val(0);
            $("#<%=txtTimeSlabHead.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");

            $("#frmHotelManagement")[0].reset();

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
            $('#btnNewTimeSlab').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewTimeSlab').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }

        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewTimeSlab').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewTimeSlab').hide("slow");
        }
        $(function () {
            $("#myTabs").tabs();
        });
    </script>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Time Slab Head Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Time Slab Head </a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">               
                <div class="panel-heading">Time Slab Head Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:HiddenField ID="txtTimeSlabId" runat="server"></asp:HiddenField>
                            <asp:Label ID="lblTimeSlabHead" runat="server" class="control-label required-field" Text="Time Slab Name"></asp:Label>                            
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtTimeSlabHead" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                        </div>
                    </div>                    
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblSlabStartTime" runat="server" class="control-label required-field" Text="Start Time"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSlabStartHour" placeholder="10" CssClass="form-control" runat="server"
                                TabIndex="2"></asp:TextBox>
                            <%--<asp:TextBox ID="txtSlabStartMinute" placeholder="00" CssClass="CustomMinuteSize"
                                TabIndex="3" runat="server"></asp:TextBox>
                            <asp:DropDownList ID="ddlSlabStartAMPM" CssClass="CustomAMPMSize" runat="server"
                                TabIndex="4">
                                <asp:ListItem>AM</asp:ListItem>
                                <asp:ListItem>PM</asp:ListItem>
                            </asp:DropDownList>
                            (10:00AM)--%>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblEndTime" runat="server" class="control-label required-field" Text="End Time"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSlabEndTimeHour" placeholder="06" CssClass="form-control" runat="server"
                                TabIndex="5"></asp:TextBox>
                            <%--<asp:TextBox ID="txtSlabEndTimeMinute" placeholder="00" CssClass="CustomMinuteSize"
                                TabIndex="6" runat="server"></asp:TextBox>
                            <asp:DropDownList ID="ddlSlabEndAMPM" CssClass="CustomAMPMSize" runat="server" TabIndex="7">
                                <asp:ListItem>AM</asp:ListItem>
                                <asp:ListItem>PM</asp:ListItem>
                            </asp:DropDownList>
                            (06:00PM)--%>
                        </div>
                    </div>                   
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblEmpId" runat="server" class="control-label" Text="Status"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="form-control"
                                TabIndex="8">
                                <asp:ListItem Value="0">Active</asp:ListItem>
                                <asp:ListItem Value="1">Inactive</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>                    
                    <div class="row">
 <div class="col-md-12">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                            TabIndex="9" OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" TabIndex="10" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                            OnClientClick="javascript: return PerformClearActionWithConfirmation();" />
                    </div>
                    </div>
                    </div>
                </div>
            </div>           
        </div>
        <div id="tab-2">
            <div id="SearchPanel" class="panel panel-default">                
                <div class="panel-heading">Search Information</div>
                <div class="panel-body">
                    <asp:GridView ID="gvTimeSlabHead" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True"
                        ForeColor="#333333" OnPageIndexChanging="gvTimeSlabHead_PageIndexChanging" OnRowDataBound="gvTimeSlabHead_RowDataBound"
                        OnRowCommand="gvTimeSlabHead_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("TimeSlabId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TimeSlabHead" HeaderText="Time Slab " ItemStyle-Width="40%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SlabStartTimeDisplay" HeaderText="Start Time" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SlabEndTimeDisplay" HeaderText="End Time" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActiveStatus" HeaderText="Status" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("TimeSlabId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" OnClientClick="return confirm('Do you want to Edit?');" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("TimeSlabId") %>' ImageUrl="~/Images/delete.png" Text=""
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
    <script type="text/javascript">
        var xNewAdd = '<%=isNewAddButtonEnable%>';
        if (xNewAdd > -1) {
            NewAddButtonPanelShow();
        }
        else {
            NewAddButtonPanelHide();
        }
        
    </script>
</asp:Content>
