<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmRestaurantPaidService.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmRestaurantPaidService" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            /*var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Paid Service</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);*/
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

        

        function ActionForAchievement() {
            $('# AchievementDivInfo').show("slow");
        }
    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div
    <div id="SearchPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Paid Service Information
        </a>
        <div class="HMBodyContainer">
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblCostCentreId" runat="server" Text="Cost Centre"></asp:Label>
                    <span class="MandatoryField">*</span>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlCostCentre" runat="server" CssClass="ThreeColumnDropDownList">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="Label1" runat="server" Text="Room Number"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtSrcRoomNumber" runat="server" CssClass="CustomTextBox" TabIndex="1"></asp:TextBox>
                    <asp:Button ID="btnSrcRoomNumber" runat="server" Text="Search" TabIndex="2" CssClass="TransactionalButton btn btn-primary"
                        OnClick="btnSrcRoomNumber_Click" />
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblRegistrationNumber" runat="server" Text="Registration Number"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:HiddenField ID="RoomIdForCurrentBillHiddenField" runat="server" />
                    <asp:HiddenField ID="hfddlRegistrationId" runat="server" />
                    <asp:DropDownList ID="ddlRegistrationId" runat="server" TabIndex="3" CssClass="customMediupXLTextBoxSize"
                        Enabled="False">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:HiddenField ID="txtCheckInDateHiddenField" runat="server" />
                    <asp:HiddenField ID="txtDepartureDateHiddenField" runat="server" />
                    <asp:HiddenField ID="ddlCurrencyHiddenField" runat="server" />
                    <asp:HiddenField ID="ddlBusinessPromotionIdHiddenField" runat="server" />
                    <asp:HiddenField ID="ddlCompanyNameHiddenField" runat="server" />
                    <asp:Label ID="lblGuestName" runat="server" Text="Guest Name"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtGuestNameInfo" TabIndex="5" runat="server" CssClass="ThreeColumnTextBox"
                        Enabled="False"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblRoomType" runat="server" Text="Room Type"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtRoomTypeInfo" TabIndex="6" runat="server" CssClass="ThreeColumnTextBox"
                        Enabled="False"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <a href="#page-stats" class="block-heading" data-toggle="collapse">Service Details Information
            </a>
            <div class="block-body collapse in">
                <asp:GridView ID="gvRestaurantServiceBill" Width="100%" runat="server" AllowPaging="True"
                    AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                    ForeColor="#333333" TabIndex="9" OnPageIndexChanging="gvRestaurantServiceBill_PageIndexChanging"
                    OnRowDataBound="gvRestaurantServiceBill_RowDataBound" OnRowCommand="gvRestaurantServiceBill_RowCommand"
                    PageSize="500">
                    <RowStyle BackColor="#E3EAEB" />
                    <Columns>
                        <asp:TemplateField HeaderText="ApprovedId" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblApprovedId" runat="server" Text='<%#Eval("ApprovedId") %>'></asp:Label></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblid" runat="server" Text='<%#Eval("ServiceBillId") %>'></asp:Label></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="RegiId" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblRegiId" runat="server" Text='<%#Eval("RegistrationId") %>'></asp:Label></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ServiceApprovedStatus" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceApprovedStatus" runat="server" Text='<%#Eval("ApprovedStatus") %>'></asp:Label></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IsPaidServiceAchieved" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblIsPaidServiceAchieved" runat="server" Text='<%#Eval("IsPaidServiceAchieved") %>'></asp:Label></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ServiceType" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceType" runat="server" Text='<%#Eval("ServiceType") %>'></asp:Label></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ServiceId" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>'></asp:Label></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Service Name" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label></ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rate" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceRate" runat="server" Text='<%#Eval("ServiceRate") %>'></asp:Label></ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qty" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceQuantity" runat="server" Text='<%#Eval("ServiceQuantity") %>'></asp:Label></ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Disc." ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblDiscountAmount" runat="server" Text='<%#Eval("DiscountAmount") %>'></asp:Label></ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vat" ItemStyle-Width="6%">
                            <ItemTemplate>
                                <asp:Label ID="lblgvVatAmount" runat="server" Text='<%# Bind("VatAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="S. Charge" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="lblgvServiceCharge" runat="server" Text='<%# Bind("ServiceCharge") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="lblgvTotalCalculatedAmount" runat="server" Text='<%# Bind("TotalCalculatedAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="VatAmountPercent" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblgvVatAmountPercent" runat="server" Text='<%# Bind("VatAmountPercent") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ServiceChargePercent" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblgvServiceChargePercent" runat="server" Text='<%# Bind("ServiceChargePercent") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CalculatedPercentAmount" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblgvCalculatedPercentAmount" runat="server" Text='<%# Bind("CalculatedPercentAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IsPaidService" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblgvIsPaidService" runat="server" Text='<%# Bind("IsPaidService") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="20%">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgApprove" runat="server" CausesValidation="False" CommandName="CmdApprove"
                                    CommandArgument='<%# bind("ServiceBillId") %>' ImageUrl="~/Images/save.png" Text=""
                                    AlternateText="Achieve" ToolTip="Achieve" OnClientClick="return ConfirmApproveMessege();" />
                                &nbsp;<asp:ImageButton ID="ImgApproved" runat="server" CausesValidation="False" CommandName="CmdApproved"
                                    ImageUrl="~/Images/select.png" Text="" AlternateText="Achieved" ToolTip="Achieved"
                                    Enabled="False" />
                                &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                    CommandArgument='<%# bind("ServiceBillId") %>' ImageUrl="~/Images/delete.png"
                                    Text="" AlternateText="Cancel" ToolTip="Cancel" OnClientClick="return confirm('Do you want to Cancel Achieved?');" />
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
            <div class="divClear">
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
