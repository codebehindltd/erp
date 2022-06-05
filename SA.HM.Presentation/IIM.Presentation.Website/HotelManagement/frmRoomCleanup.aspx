<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="true"
    CodeBehind="frmRoomCleanup.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmRoomCleanup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            $('#ContentPlaceHolder1_txtApprovedDate').click(function () {
            });

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Room CleanUp</li>";
            var breadCrumbs = moduleName + formName;

            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            if ($("#<%=txtRoomId.ClientID %>").val() != "") {
                PerformFillFormAction($("#<%=txtRoomId.ClientID %>").val());
            }

            var ddlCleanStatus = '<%=ddlCleanStatus.ClientID%>'
            var txtRemarks = '<%=txtRemarks.ClientID%>'

            $('#' + ddlCleanStatus).change(function () {

                if ($('#' + ddlCleanStatus).val() == "OutOfOrder") {
                    $('#OutOfServiceDivInfo').show("slow");
                    $('#WithoutOutOfServiceDivInfo').hide("slow");
                }
                else {
                    if ($('#' + ddlCleanStatus).val() == "Cleaned") {
                        $('#' + txtRemarks).val("");
                    }
                    $('#OutOfServiceDivInfo').hide("slow");
                    $('#WithoutOutOfServiceDivInfo').show("slow");
                }
            });


            var txtStartDate = '<%=txtFromDate.ClientID%>'
            var txtEndDate = '<%=txtToDate.ClientID%>'
            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                minDate: 0,
                onSelect: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                minDate: 0,
                dateFormat: innBoarDateFormat,
                onSelect: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtApprovedDate').datepicker({
                changeMonth: true,
                changeYear: true,
                minDate: 0,
                dateFormat: innBoarDateFormat               
            });

        });

        //For FillForm-------------------------
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        //-- Common Populate Control -----------------------------
        function PopulateddlCleanStatusControl(statusId, cleanupStatus, control) {
            control.empty();
            if (statusId == 1) {
                if (cleanupStatus == "Not Cleaned") {
                    control.append($("<option></option>").val("Cleaned").html("Cleaned"));
                    control.append($("<option></option>").val("OutOfOrder").html("Out of Order"));
                }
                else {
                    control.append($("<option></option>").val("Dirty").html("Dirty"));
                    control.append($("<option></option>").val("Cleaned").html("Cleaned"));
                    control.append($("<option></option>").val("OutOfOrder").html("Out of Order"));
                }
            }
            else if (statusId == 2) {
                control.append($("<option></option>").val("Cleaned").html("Cleaned"));
            }
            else if (statusId == 3) {
                control.append($("<option></option>").val("Available").html("Available"));
            }
        }

        function OnFillFormObjectSucceeded(result) {
            var guest = eval(result);
            $("#<%=txtRoomId.ClientID %>").val(guest.RoomId);
            $("#<%=txtLastCleanDate.ClientID %>").val(guest.LastCleanDate);
            $("#<%=txtRoomNumber.ClientID %>").val(guest.RoomNumber);
            $("#<%=txtRemarks.ClientID %>").val(result.Remarks);

            PopulateddlCleanStatusControl(guest.StatusId, guest.CleanupStatus, $("#<%=ddlCleanStatus.ClientID %>"));

            var now = new Date();
            var cHours = now.getHours();
            var cMinutes = now.getMinutes();

            if (cHours > 12) {
                var calculateValue = (cHours % 12);
                $("#<%=txtProbableHour.ClientID %>").val(calculateValue);
                //$("#<%=ddlProbableAMPM.ClientID %>").val(1);
            }
            else {
                $("#<%=txtProbableHour.ClientID %>").val(cHours);
                //$("#<%=ddlProbableAMPM.ClientID %>").val(0);
            }

            $("#<%=txtProbableMinute.ClientID %>").val(now.getMinutes());
            $("#<%=txtRoomNumber.ClientID %>").attr('readonly', true);
            $("#<%=txtApprovedDate.ClientID %>").attr('readonly', true);

            var ddlCleanStatus = '<%=ddlCleanStatus.ClientID%>'
            var txtRemarks = '<%=txtRemarks.ClientID%>'
            if ($('#' + ddlCleanStatus).val() == "Cleaned") {
                $('#' + txtRemarks).val("");
            }

            //popup(1, 'EntryPanel', '', 900, 350);
            $("#EntryPanel").dialog({
                width: 900,
                height: 350,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "", ////TODO add title
                show: 'slide'
            });
            $('#OutOfServiceDivInfo').hide("slow");
            $('#WithoutOutOfServiceDivInfo').show("slow");
        }

        function OnFillFormObjectFailed(error) {
            toastr.error(error);
        }

        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=lblMessage.ClientID %>").text('');
            $("#<%=txtRoomId.ClientID %>").val('');
            $("#<%=txtRoomNumber.ClientID %>").val('');
            $("#<%=txtRemarks.ClientID %>").val('');
            return false;
        }

        function ValidateTimee() {

            var phour = $.trim($("#ContentPlaceHolder1_txtProbableHour").val());
            var pminute = $.trim($("#ContentPlaceHolder1_txtProbableMinute").val());

            if (CommonHelper.IsInt(phour) == false) {
                toastr.warning("Invalid Hour");
                return false;
            }
            else if (CommonHelper.IsInt(pminute) == false) {
                toastr.warning("Invalid Minute");
                return false;
            }

            return true;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
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
    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="EntryPanel" class="panel panel-default" style="display: none;">
        <div class="panel-heading">Clean Status Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:HiddenField ID="txtRoomId" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="txtLastCleanDate" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblRoomNumber" runat="server" class="control-label" Text="Room Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtRoomNumber" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblCleanStatus" runat="server" class="control-label" Text="Clean Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCleanStatus" CssClass="form-control" runat="server">
                            <asp:ListItem Value="Cleaned">Cleaned</asp:ListItem>
                            <asp:ListItem Value="Dirty">Dirty</asp:ListItem>
                            <asp:ListItem Value="Available">Available</asp:ListItem>
                            <asp:ListItem Value="OutOfOrder">Out of Order</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="OutOfServiceDivInfo" style="display: none;">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblToDate" runat="server" class="control-label required-field" Text="To Date"></asp:Label>                            
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div id="WithoutOutOfServiceDivInfo">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblApprovedDate" runat="server" class="control-label" Text="Clean Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtApprovedDate" runat="server" CssClass="form-control" Enabled="false"
                                TabIndex="4"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblProbableInTime" runat="server" class="control-label" Text="Clean Time"></asp:Label>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtProbableHour" placeholder="10" Text="10" CssClass="form-control"
                                runat="server"></asp:TextBox>&nbsp;:                                                
                        </div>
                        <div class="col-md-1">
                            <asp:TextBox ID="txtProbableMinute" placeholder="00" Text="00" CssClass="form-control"
                                runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <asp:DropDownList ID="ddlProbableAMPM" CssClass="form-control" runat="server">
                                <asp:ListItem Value="0">AM</asp:ListItem>
                                <asp:ListItem Value="1">PM</asp:ListItem>
                            </asp:DropDownList>
                            (10:00AM)
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                    <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="btn btn-primary"
                        PostBackUrl="~/HotelManagement/frmRoomCleanup.aspx" OnClick="btnSave_Click" OnClientClick="javascript:return ValidateTimee()" />
                        </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">Search Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2"">
                        <asp:Label ID="lblSrcCleanStatus" runat="server" class="control-label" Text="Clean Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSrcCleanStatus" CssClass="form-control" runat="server" TabIndex="1">
                            <asp:ListItem Value="All">--- All ---</asp:ListItem>
                            <asp:ListItem Value="Cleaned">Cleaned</asp:ListItem>
                            <asp:ListItem Value="Dirty">Dirty</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblSrcRoomNumber" runat="server" class="control-label" Text="Room Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSrcRoomNumber" runat="server" CssClass="form-control"
                            TabIndex="2"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                    <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search Room" CssClass="btn btn-primary btn-sm"
                        OnClick="btnSearch_Click" />
                        </div>
                </div>
                <div>
                    <asp:GridView ID="gvAvailableGuestList" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                        ForeColor="#333333" PageSize="110" OnPageIndexChanging="gvAvailableGuestList_PageIndexChanging"
                        OnRowDataBound="gvAvailableGuestList_RowDataBound" OnRowCommand="gvAvailableGuestList_RowCommand"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("RoomId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="05%">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBoxAccept" Checked="False" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Room Number">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvRoomNumber" runat="server" Text='<%# Bind("RoomNumber") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Room Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvActiveStatus" runat="server" Text='<%# Bind("ActiveStatus") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cleanup Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvCleanupStatus" runat="server" Text='<%# Bind("CleanupStatus") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Next Clean Date Time">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvCleanDate" runat="server" Text='<%# Bind("CleanDate") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Clean Date Time">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvLastCleanDate" runat="server" Text='<%# Bind("LastCleanDate") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remarks">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvRemarks" runat="server" Text='<%# Bind("Remarks") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgApprove" runat="server" CausesValidation="False" CommandName="CmdApprove"
                                        CommandArgument='<%# bind("RoomId") %>' ImageUrl="~/Images/save.png" Text=""
                                        AlternateText="Clean" ToolTip="Clean" />
                                    &nbsp;<asp:ImageButton ID="ImgApproved" runat="server" CausesValidation="False" CommandName="CmdApproved"
                                        ImageUrl="~/Images/select.png" Text="" AlternateText="Cleaned" ToolTip="Cleaned"
                                        Enabled="False" />
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
                <div class="row">
                    <div class="col-md-12">
                    <asp:Button ID="btnSaveAll" runat="server" Text="Clean" CssClass="btn btn-primary btn-sm"
                        OnClick="btnSaveAll_Click" />
                </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
