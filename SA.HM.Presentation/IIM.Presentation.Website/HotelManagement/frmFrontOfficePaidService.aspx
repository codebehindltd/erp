<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmFrontOfficePaidService.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmFrontOfficePaidService" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Paid Service</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });

        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to update data?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">Paid Service Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2 label-align">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Room Number"></asp:Label>
                    </div>
                    <div class="col-md-2 label-align">
                        <asp:TextBox ID="txtSrcRoomNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>                        
                    </div>
                    <div class="col-md-2 label-align">
                        <asp:Button ID="btnSrcRoomNumber" runat="server" Text="Search" TabIndex="2" CssClass="btn btn-primary btn-sm"
                            OnClick="btnSrcRoomNumber_Click" />
                    </div>
                    <div class="col-md-2 label-align">
                        <asp:Label ID="lblRegistrationNumber" runat="server" class="control-label" Text="Registration Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:HiddenField ID="RoomIdForCurrentBillHiddenField" runat="server" />
                        <asp:HiddenField ID="hfddlRegistrationId" runat="server" />
                        <asp:DropDownList ID="ddlRegistrationId" runat="server" TabIndex="3" CssClass="form-control"
                            Enabled="False">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2 label-align" >
                        <asp:HiddenField ID="txtCheckInDateHiddenField" runat="server" />
                        <asp:HiddenField ID="txtDepartureDateHiddenField" runat="server" />
                        <asp:HiddenField ID="ddlCurrencyHiddenField" runat="server" />
                        <asp:HiddenField ID="ddlBusinessPromotionIdHiddenField" runat="server" />
                        <asp:HiddenField ID="ddlCompanyNameHiddenField" runat="server" />
                        <asp:Label ID="lblGuestName" runat="server" class="control-label" Text="Guest Name"></asp:Label>
                    </div>
                    <div class="col-md-10" >
                        <asp:TextBox ID="txtGuestNameInfo" TabIndex="5" runat="server" CssClass="form-control"
                            Enabled="False"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2 label-align" >
                        <asp:Label ID="lblRoomType" runat="server" class="control-label" Text="Room Type"></asp:Label>
                    </div>
                    <div class="col-md-10" >
                        <asp:TextBox ID="txtRoomTypeInfo" TabIndex="6" runat="server" CssClass="form-control"
                            Enabled="False"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" id="GridInformation">
                    <asp:GridView ID="gvPSConfirm" Width="95%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="50"
                        TabIndex="9" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkBox" runat="server" CssClass="chkBox_Approve" Text="" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("Id")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ServiceName" HeaderText="Service Name" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ServiceType" HeaderText="Service Type" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" ItemStyle-Width="15%">
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
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSave" runat="server" Text="Achieve" TabIndex="2" CssClass="btn btn-primary btn-sm"
                            OnClick="btnSave_Click" OnClientClick="Confirm()" />
                    </div>
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
    </script>
</asp:Content>
