<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmGuestPaymentTransfer.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmGuestPaymentTransfer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Guest Payment Transfer</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_ddlRoomId").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
        });
    </script>
    <asp:HiddenField ID="hfLocalCurrencyId" runat="server" />
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Payment Transfer Information
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
                    <div id="lblRegistrationNumberDiv" runat="server">
                        <label for="RegistrationNumber" class="control-label col-md-2">
                            Registration Number</label>
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
                    <label for="GuestName" class="control-label col-md-2">
                        Guest Name</label>
                    <div class="col-md-10">
                        <asp:HiddenField ID="txtCheckInDateHiddenField" runat="server" />
                        <asp:HiddenField ID="txtDepartureDateHiddenField" runat="server" />
                        <asp:HiddenField ID="ddlCurrencyHiddenField" runat="server" />
                        <asp:HiddenField ID="ddlBusinessPromotionIdHiddenField" runat="server" />
                        <asp:HiddenField ID="ddlCompanyNameHiddenField" runat="server" />
                        <asp:TextBox ID="txtGuestNameInfo" TabIndex="5" runat="server" CssClass="form-control"
                            Enabled="False"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="RoomType" class="control-label col-md-2">
                        Room Type</label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtRoomTypeInfo" TabIndex="6" runat="server" CssClass="form-control"
                            Enabled="False"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" id="GridInformation">
                    <asp:GridView ID="gvGHServiceBill" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="50"
                        TabIndex="9" OnPageIndexChanging="gvGHServiceBill_PageIndexChanging" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkBox" runat="server" CssClass="chkBox_Approve" Text="" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("PaymentId")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PaymentDate" HeaderText="Date" SortExpression="PaymentDate"
                                DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <%--<asp:TemplateField HeaderText="Date" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblPLI_DATE" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("PaymentDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="BillNumber" HeaderText="Invoice No." ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <%--<asp:BoundField DataField="PaymentType" HeaderText="Payment Type" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>--%>
                            <asp:BoundField DataField="CreatedByName" HeaderText="Received By" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CurrencyType" HeaderText="Currency" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CurrencyAmount" HeaderText="Amount" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Transfer Amount" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtTransferAmount" runat="server" CssClass="form-control"></asp:TextBox>
                                </ItemTemplate>
                                <ControlStyle Font-Size="Small" />
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
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
                <asp:Panel ID="pnlBillTransferedInfo" runat="server" Height="50px">
                    <div class="form-group" style="padding-top: 10px;">
                        <label for="TransferedRoom" class="control-label col-md-2 required-field">
                            Transfered Room</label>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlRoomId" runat="server" TabIndex="4" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="Remarks" class="control-label col-md-2 required-field">
                            Description</label>
                        <div class="col-md-8">
                            <div>
                                <asp:TextBox ID="txtRemarks" CssClass="form-control" TextMode="MultiLine" runat="server"
                                    TabIndex="8"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div style="padding-top: 10px;">
                                <asp:Button ID="btnBillTransfer" runat="server" Text="Transfer" CssClass="btn btn-primary btn-sm"
                                    TabIndex="8" OnClick="btnBillTransfer_Click" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
