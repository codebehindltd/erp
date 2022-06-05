<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/Common/Innboard.Master"
    CodeBehind="frmGuestCheckOut.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmGuestCheckOut" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Pax Out</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });
    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <asp:HiddenField ID="hfRegistrationId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfRegistrationNumber" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCheckInDate" runat="server"></asp:HiddenField>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Guest Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <label for="RoomNumber" class="control-label col-md-2 required-field">
                        Room Number</label>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtSrcRoomNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnSrcRoomNumber" runat="server" Text="Search" TabIndex="2" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClick="btnSrcRoomNumber_Click" />
                    </div>
                    <label for="RegistrationNumber" class="control-label col-md-2 required-field">
                        Registration Number</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRegistrationId" runat="server" CssClass="form-control" TabIndex="3"
                            Enabled="False">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Guest List</div>
        <div class="panel-body">
            <asp:GridView ID="gvGuestCheckOut" Width="100%" runat="server" AllowPaging="True"
                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                ForeColor="#333333" PageSize="100" OnRowDataBound="gvGuestCheckOut_RowDataBound"
                OnRowCommand="gvGuestCheckOut_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                <RowStyle BackColor="#E3EAEB" />
                <Columns>
                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="GuestName" HeaderText="Guest Name" ItemStyle-Width="45%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Check In" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblCheckInDate" runat="server" Text='<%# Eval("CheckInDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Check Out" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblCheckOutDate" runat="server" Text='<%#Eval("CheckOutDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImgCheckOut" runat="server" CausesValidation="False" CommandName="CmdCheckOut"
                                CommandArgument='<%# bind("Id") %>' ImageUrl="~/Images/delete.png" Text="" AlternateText="Check Out"
                                ToolTip="Check Out" OnClientClick="return confirm('Do you want to Check-Out?');" />
                            &nbsp;<asp:ImageButton ID="ImgCheckIn" runat="server" CausesValidation="False" CommandName="CmdCheckIn"
                                CommandArgument='<%# bind("Id") %>' ImageUrl="~/Images/select.png" Text="" AlternateText="Check In"
                                ToolTip="Check In" OnClientClick="return confirm('Do you want to Check-In?');" />
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
</asp:Content>
