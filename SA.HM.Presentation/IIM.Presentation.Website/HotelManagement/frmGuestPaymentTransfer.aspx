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
            var transferTypeId = $("#ContentPlaceHolder1_ddlTransferType").val();
            if (transferTypeId == "1") {
                $("#InhouseGuestPayment").show();
                $("#ReservationGuestPayment").hide();
            }
            else if (transferTypeId == "2") {
                $("#InhouseGuestPayment").hide();
                $("#ReservationGuestPayment").show();
            }
            else if (transferTypeId == "0") {
                $("#InhouseGuestPayment").hide();
                $("#ReservationGuestPayment").hide();
            }

            $("#ContentPlaceHolder1_ddlTransferType").change(function () {
                if ($("#ContentPlaceHolder1_ddlTransferType").val() == "0") {
                    $("#InhouseGuestPayment").hide();
                    $("#ReservationGuestPayment").hide();
                }
                else if ($("#ContentPlaceHolder1_ddlTransferType").val() == "1") {
                    $("#InhouseGuestPayment").show();
                    $("#ReservationGuestPayment").hide();
                }
                else if ($("#ContentPlaceHolder1_ddlTransferType").val() == "2") {
                    $("#InhouseGuestPayment").hide();
                    $("#ReservationGuestPayment").show();
                }
                return false;
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
                    <label for="TransferType" class="control-label col-md-2 required-field">
                        Transfer Type
                    </label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTransferType" runat="server" CssClass="form-control" TabIndex="1">
                            <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Inhouse Guest Payment" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Reservation Guest Payment" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div id="InhouseGuestPayment">
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
            <div id="ReservationGuestPayment">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label for="ReservationNumber" class="control-label col-md-2 required-field">
                            Reservation Number</label>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtSrcReservationNumber" runat="server" CssClass="form-control" TabIndex="21"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Button ID="btnSrcReservationNumber" runat="server" Text="Search" TabIndex="22" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClick="btnSrcReservationNumber_Click" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="GuestName" class="control-label col-md-2">
                            Guest Name</label>
                        <div class="col-md-10">
                            <asp:HiddenField ID="HiddenField3" runat="server" />
                            <asp:TextBox ID="txtReservedGuestName" TabIndex="23" runat="server" CssClass="form-control"
                                Enabled="False"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="lblArrivalDate" class="control-label col-md-2">
                            Arrival Date</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtArrivalDate" TabIndex="24" runat="server" CssClass="form-control"
                                Enabled="False"></asp:TextBox>
                        </div>
                        <label for="lblExpectedDepartureDate" class="control-label col-md-2">
                            Departure Date</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtExpectedDepartureDate" TabIndex="25" runat="server" CssClass="form-control"
                                Enabled="False"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group" id="GridInformationReservation" runat="server">
                        <asp:GridView ID="gvReservationInfo" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="50"
                            TabIndex="26" OnPageIndexChanging="gvReservationInfo_PageIndexChanging" CssClass="table table-bordered table-condensed table-responsive">
                            <RowStyle BackColor="#E3EAEB" />
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="2%">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkBoxReservation" runat="server" CssClass="chkBox_Approve" Text="" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="IDNO" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblidReservation" runat="server" Text='<%#Eval("PaymentId")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ReservationDate" HeaderText="Date" SortExpression="ReservationDate"
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
                                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No." ItemStyle-Width="15%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <%--<asp:BoundField DataField="PaymentType" HeaderText="Payment Type" ItemStyle-Width="10%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="ReceivedBy" HeaderText="Received By" ItemStyle-Width="15%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CurrencyName" HeaderText="Currency" ItemStyle-Width="10%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CurrencyAmount" HeaderText="Amount" ItemStyle-Width="15%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Transfer Amount" ShowHeader="False" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTransferAmountReservation" runat="server" CssClass="form-control"></asp:TextBox>
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
                    <asp:Panel ID="pnlBillTransferForReservation" runat="server" Height="50px">
                        <div class="form-group" style="padding-top: 10px;">
                            <label for="TransferedReservation" class="control-label col-md-2 required-field">
                                Transfered Reservation</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlReservationId" runat="server" TabIndex="27" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Remarks" class="control-label col-md-2 required-field">
                                Description</label>
                            <div class="col-md-8">
                                <div>
                                    <asp:TextBox ID="txtTransferReservationRemarks" CssClass="form-control" TextMode="MultiLine" runat="server"
                                        TabIndex="28"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div style="padding-top: 10px;">
                                    <asp:Button ID="btnReservationBillTransfer" runat="server" Text="Transfer" CssClass="btn btn-primary btn-sm"
                                        TabIndex="29" OnClick="btnReservationBillTransfer_Click" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
