<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmEmpTimeSlab.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpTimeSlab" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>HR Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Time Slab</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#<%=ddlTimeSlabType.ClientID %>").val() == 'Fixed') {
                $('#RosterDiv').hide("slow");
                $('#FixedDiv').show("slow");
            }
            else {
                $('#RosterDiv').show("slow");
                $('#FixedDiv').hide("slow");
            }
        });

        $(document).ready(function () {
            var txtSlabEffectDate = '<%=txtSlabEffectDate.ClientID%>'
            $('#' + txtSlabEffectDate).datepicker("option", {
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtSlabEffectDate).datepicker("option", "minDate", selectedDate);
                }
            });
            $("#<%=ddlWeekEndMode.ClientID %>").change(function () {
                WeekModeChange();
            });

            $("#<%=ddlTimeSlabType.ClientID %>").change(function () {
                if ($("#<%=ddlTimeSlabType.ClientID %>").val() =='Fixed') {
                    $('#RosterDiv').hide("slow");
                    $('#FixedDiv').show("slow");
                }
                else {
                    $('#RosterDiv').show("slow");
                    $('#FixedDiv').hide("slow");
                }
            });


        });


        function WeekModeChange() {
            if ($("#<%=ddlWeekEndMode.ClientID %>").val() != "Double") {
                $('#SecondWeekend').hide();
            } else {
                $('#SecondWeekend').show();
            }
        }

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {
            MessagePanelHide();
            $("#<%=ddlActiveStat.ClientID %>").val(result.ActiveStat == true ? 0 : 1);
            $("#<%=txtSlabEffectDate.ClientID %>").val(result.EffectDate);
            $("#<%=ddlEmpId.ClientID %>").val(result.EmpId);
            $("#<%=ddlTimeSlabId.ClientID %>").val(result.TimeSlabId);
            $("#<%=ddlWeekEndMode.ClientID %>").val(result.WeekEndMode);
            $("#<%=ddlWeekEndFirst.ClientID %>").val(result.WeekEndFirst);
            $("#<%=ddlWeekEndSecond.ClientID %>").val(result.WeekEndSecond);
            $("#<%=txtEmpTimeSlabId.ClientID %>").val(result.EmpTimeSlabId);
            WeekModeChange();
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
            window.location = "frmEmpTimeSlab.aspx?DeleteConfirmation=Deleted"
        }
        function OnDeleteObjectFailed(error) {
            alert(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=lblMessage.ClientID %>").text('');
            $("#<%=txtSlabEffectDate.ClientID %>").val('');
            $("#<%=ddlActiveStat.ClientID %>").val(0);
            $("#<%=ddlEmpId.ClientID %>").val(0);
            $("#<%=ddlTimeSlabId.ClientID %>").val(0);
            $("#<%=ddlWeekEndMode.ClientID %>").val(0);
            $("#<%=ddlWeekEndFirst.ClientID %>").val(0);
            $("#<%=ddlWeekEndSecond.ClientID %>").val(0);
            $("#<%=txtEmpTimeSlabId.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");

            $("#frmHotelManagement")[0].reset();

            MessagePanelHide();
            return false;
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

        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
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
    <div id="MessageBox" class="alert alert-info" style="display: none">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div style="height: 45px">
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Time Slab Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Time Slab </a></li>
        </ul>
        <div id="tab-1">
        <div id="EntryPanel" class="block" >
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Time Slab Information
        </a>
        <div class="HMBodyContainer">
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <%--Left Left--%>
                    <asp:HiddenField ID="txtEmpTimeSlabId" runat="server"></asp:HiddenField>
                    <asp:Label ID="lblSlabEffectDate" runat="server" Text="Effect Date"></asp:Label>
                    <span class="MandatoryField">*</span>
                </div>
                <div class="divBox divSectionLeftRight">
                    <%--Right Left--%>
                    <asp:TextBox ID="txtSlabEffectDate" runat="server" CssClass="datepicker" TabIndex="1"></asp:TextBox>
                </div>

                <div class="divBox divSectionRightLeft">
                    <%--Left Left--%>
                    <asp:Label ID="llbTimeSlabType" runat="server" Text="Time Slab Type"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <%--Right Left--%>
                    <asp:DropDownList ID="ddlTimeSlabType" runat="server" CssClass="dropdown" TabIndex="2">
                        <asp:ListItem Value="Fixed">Fixed</asp:ListItem>
                        <asp:ListItem Value="Roster">Roster</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <%--Left Left--%>
                    <asp:Label ID="lblEmpId" runat="server" Text="Employee"></asp:Label>
                    <span class="MandatoryField">*</span>
                </div>
                <div class="divBox divSectionLeftRight">
                    <%--Right Left--%>
                    <asp:DropDownList ID="ddlEmpId" runat="server" CssClass="ThreeColumnDropDownList" TabIndex="2">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>

            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <%-- Left Left--%>
                    <asp:Label ID="lblWeekEndMode" runat="server" Text="Weekend Mode"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlWeekEndMode" runat="server" CssClass="tdLeftAlignWithSize" TabIndex="3">
                        <asp:ListItem Value="Single">Single</asp:ListItem>
                        <asp:ListItem Value="Double">Double</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="divBox divSectionRightLeft">
                    <%--Right Left--%>
                    <asp:Label ID="lblTimeSlabId" runat="server" Text="Time Slab"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <%--Left Right--%>
                    <asp:DropDownList ID="ddlTimeSlabId" runat="server" CssClass="tdLeftAlignWithSize" TabIndex="4">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <%-- Left Left--%>
                    <asp:Label ID="lblWeekEndFirst" runat="server" Text="First Weekend"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <%--Right Right--%>
                    <asp:DropDownList ID="ddlWeekEndFirst" runat="server" CssClass="tdLeftAlignWithSize" TabIndex="6">
                        <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
                        <asp:ListItem Value="Monday">Monday</asp:ListItem>
                        <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                        <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                        <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                        <asp:ListItem Value="Friday">Friday</asp:ListItem>
                        <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div id="SecondWeekend">
                <div class="divBox divSectionRightLeft">
                    <%--Right Left--%>
                    <asp:Label ID="lblWeekEndSecond" runat="server" Text="Second Weekend"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <%--Left Right--%>
                                        <asp:DropDownList ID="ddlWeekEndSecond" runat="server" CssClass="tdLeftAlignWithSize" TabIndex="5">
                        <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
                        <asp:ListItem Value="Monday">Monday</asp:ListItem>
                        <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                        <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                        <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                        <asp:ListItem Value="Friday">Friday</asp:ListItem>
                        <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                    </asp:DropDownList>



                </div>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div id="FixedDiv">



            <div class="divClear">
            </div>
            </div>
            <div id="RosterDiv">
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <%-- Left Left--%>
                    <asp:Label ID="Label1" runat="server" Text="Day One"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlSunDay" runat="server" CssClass="tdLeftAlignWithSize" TabIndex="4">
                        <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
                        <asp:ListItem Value="Monday">Monday</asp:ListItem>
                        <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                        <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                        <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                        <asp:ListItem Value="Friday">Friday</asp:ListItem>
                        <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="divBox divSectionRightLeft">
                    <%--Right Left--%>
                    <asp:Label ID="Label2" runat="server" Text="Time Slab"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <%--Left Right--%>
                    <asp:DropDownList ID="ddlDayOneTS" runat="server" CssClass="tdLeftAlignWithSize" TabIndex="4">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>       
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <%-- Left Left--%>
                    <asp:Label ID="Label3" runat="server" Text="Day Two"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlMonDay" runat="server" CssClass="tdLeftAlignWithSize" TabIndex="3">
                        <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
                        <asp:ListItem Value="Monday">Monday</asp:ListItem>
                        <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                        <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                        <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                        <asp:ListItem Value="Friday">Friday</asp:ListItem>
                        <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="divBox divSectionRightLeft">
                    <%--Right Left--%>
                    <asp:Label ID="Label4" runat="server" Text="Time Slab"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <%--Left Right--%>
                    <asp:DropDownList ID="ddlDayTwoTS" runat="server" CssClass="tdLeftAlignWithSize" TabIndex="4">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>    
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <%-- Left Left--%>
                    <asp:Label ID="Label5" runat="server" Text="Day Three"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlTuesDay" runat="server" CssClass="tdLeftAlignWithSize" TabIndex="3">
                        <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
                        <asp:ListItem Value="Monday">Monday</asp:ListItem>
                        <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                        <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                        <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                        <asp:ListItem Value="Friday">Friday</asp:ListItem>
                        <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="divBox divSectionRightLeft">
                    <%--Right Left--%>
                    <asp:Label ID="Label6" runat="server" Text="Time Slab"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <%--Left Right--%>
                    <asp:DropDownList ID="ddlDayThreeTS" runat="server" CssClass="tdLeftAlignWithSize" TabIndex="4">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            
             <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <%-- Left Left--%>
                    <asp:Label ID="Label7" runat="server" Text="Day Four"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlWedDay" runat="server" CssClass="tdLeftAlignWithSize" TabIndex="3">
                        <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
                        <asp:ListItem Value="Monday">Monday</asp:ListItem>
                        <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                        <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                        <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                        <asp:ListItem Value="Friday">Friday</asp:ListItem>
                        <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="divBox divSectionRightLeft">
                    <%--Right Left--%>
                    <asp:Label ID="Label8" runat="server" Text="Time Slab"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <%--Left Right--%>
                    <asp:DropDownList ID="ddlDayFourTS" runat="server" CssClass="tdLeftAlignWithSize" TabIndex="4">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>                                               
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <%-- Left Left--%>
                    <asp:Label ID="Label9" runat="server" Text="Day Five"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlThuDay" runat="server" CssClass="tdLeftAlignWithSize" TabIndex="3">
                        <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
                        <asp:ListItem Value="Monday">Monday</asp:ListItem>
                        <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                        <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                        <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                        <asp:ListItem Value="Friday">Friday</asp:ListItem>
                        <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="divBox divSectionRightLeft">
                    <%--Right Left--%>
                    <asp:Label ID="Label10" runat="server" Text="Time Slab"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <%--Left Right--%>
                    <asp:DropDownList ID="ddlDayFiveTS" runat="server" CssClass="tdLeftAlignWithSize" TabIndex="4">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <%-- Left Left--%>
                    <asp:Label ID="Label11" runat="server" Text="Day Six"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlFriday" runat="server" CssClass="tdLeftAlignWithSize" TabIndex="3">
                        <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
                        <asp:ListItem Value="Monday">Monday</asp:ListItem>
                        <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                        <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                        <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                        <asp:ListItem Value="Friday">Friday</asp:ListItem>
                        <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="divBox divSectionRightLeft">
                    <%--Right Left--%>
                    <asp:Label ID="Label12" runat="server" Text="Time Slab"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <%--Left Right--%>
                    <asp:DropDownList ID="ddlDaySixTS" runat="server" CssClass="tdLeftAlignWithSize" TabIndex="4">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <%-- Left Left--%>
                    <asp:Label ID="Label13" runat="server" Text="Day Seven"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlSatDay" runat="server" CssClass="tdLeftAlignWithSize" TabIndex="3">
                        <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
                        <asp:ListItem Value="Monday">Monday</asp:ListItem>
                        <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                        <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                        <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                        <asp:ListItem Value="Friday">Friday</asp:ListItem>
                        <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="divBox divSectionRightLeft">
                    <%--Right Left--%>
                    <asp:Label ID="Label14" runat="server" Text="Time Slab"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <%--Left Right--%>
                    <asp:DropDownList ID="ddlDaySevenTS" runat="server" CssClass="tdLeftAlignWithSize" TabIndex="4">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>

            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <%--Left Left--%>
                    <asp:Label ID="lblActiveStat" runat="server" Text="Status"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <%--Right Left--%>
                    <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="tdLeftAlignWithSize" TabIndex="7">
                        <asp:ListItem Value="0">Active</asp:ListItem>
                        <asp:ListItem Value="1">Inactive</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

        </div>

            <div class="divClear">
            </div>
            <div class="HMContainerRowButton">
                <%--Right Left--%>
                <asp:Button ID="btnSave" runat="server" TabIndex="8" Text="Save" CssClass="TransactionalButton btn btn-primary"
                    OnClick="btnSave_Click" />
                <asp:Button ID="btnClear" runat="server" TabIndex="9" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                    OnClientClick="javascript: return PerformClearAction();" />
                
            </div>
    </div>
    <div class="divClear">
    </div>
     </div>
     </div>
        <div id="tab-2">
    <div id="SearchPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Information
        </a>
        <div class="block-body collapse in">
            <asp:GridView ID="gvTimeSlab" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True" ForeColor="#333333"
                OnPageIndexChanging="gvTimeSlab_PageIndexChanging" OnRowDataBound="gvTimeSlab_RowDataBound"  OnRowCommand="gvTimeSlab_RowCommand">
                <RowStyle BackColor="#E3EAEB" />
                <Columns>
                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("EmpTimeSlabId") %>'></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="WeekEndMode" HeaderText="Weekend Mode" ItemStyle-Width="50%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SlabEffectDate" HeaderText="Slab EffectDate" ItemStyle-Width="35%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                        <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("EmpTimeSlabId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("EmpTimeSlabId") %>' ImageUrl="~/Images/delete.png" Text=""
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
        var x = '<%=isMessageBoxEnable%>';
        if (x > -1) {
            MessagePanelShow();
            if (x == 2) {
                $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
            } 
        }
        else {
            MessagePanelHide();
        }
        var isRoster = '<%=isRoster%>';
        if (isRoster > -1) {

            $('#RosterDiv').show("slow");
            $('#FixedDiv').hide("slow");  
        }
        else {

            $('#RosterDiv').hide("slow");
            $('#FixedDiv').show("slow");     
        }
        var xNewAdd = '<%=isNewAddButtonEnable%>';
        if (xNewAdd > -1) {
            NewAddButtonPanelShow();
        }
        else {
            NewAddButtonPanelHide();
        }

        var double = '<%=isDoubleDay%>';

        if (double == -1) {

            $('#SecondWeekend').hide();
        } else {
            $('#SecondWeekend').show();
        }

    </script>
</asp:Content>
