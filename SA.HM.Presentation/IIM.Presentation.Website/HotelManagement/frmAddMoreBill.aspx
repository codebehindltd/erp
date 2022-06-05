<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmAddMoreBill.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmAddMoreBill" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Add More Bill</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            var chkIsGroupPayment = '<%=chkIsGroupPayment.ClientID%>'
            if ($('#' + chkIsGroupPayment).attr('checked')) {
                $('#IndividualBillPayment').hide();
                $('#btnAddDiv').hide();
                $('#btnGroupSaveDiv').show();
                $('#GroupBillPayment').show();
                $('#btnGroupSave').show();
            }
            else {
                $('#IndividualBillPayment').show();
                $('#btnAddDiv').show();
                $('#btnGroupSaveDiv').hide();
                $('#GroupBillPayment').hide();
                $('#btnGroupSave').hide();
            }
        });


        function ToggleFieldVisible(ctrl) {
            if ($(ctrl).is(':checked')) {
                $('#IndividualBillPayment').hide();
                $('#btnAddDiv').hide();
                $('#btnGroupSaveDiv').show();
                $('#GroupBillPayment').show();
                $('#btnGroupSave').show();
            }
            else {
                $('#IndividualBillPayment').show();
                $('#btnAddDiv').show();
                $('#btnGroupSaveDiv').hide();
                $('#GroupBillPayment').hide();
                $('#btnGroupSave').hide();
            }
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
            var currentURL = $(location).attr('href');
            //window.location = "frmAddMoreBill.aspx?DeleteConfirmation=Deleted";
            window.location = currentURL + "&DeleteConfirmation=Deleted";
        }

        function OnDeleteObjectFailed(error) {
            alert(error.get_message());
        }

        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show();
        }
        function MessagePanelHide() {
            $('#MessageBox').hide();
        }

        $(function () {
            $("#myTabs").tabs();
        });
    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="btnNewBill" class="btn-toolbar" style="display: none;">
        <button onclick="javascript: return EntryPanelVisibleTrue();" class="btn btn-primary">
            <i class="icon-plus"></i>Add More Bill</button>
        <asp:Button ID="Button1" runat="server" Text="<< Back to CheckOut Form" CssClass="btn btn-primary"
            Visible="False" OnClick="btnBackToCheckOutForm_Click" />
        <div class="btn-group">
        </div>
    </div>
    <%-- <div id="EntryPanel" class="block" style="display: none;">--%>
    <div id="EntryPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Add More Room Bill Information </a>
        <div class="HMBodyContainer">
            <div class="block-body collapse in">
                <div class="HMContainerRow">
                    <div class="left-float">
                        <div class="l-left">
                            <%-- Left Left--%>
                            <asp:HiddenField ID="txtServiceBillId" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="txtAddedRoomId" runat="server"></asp:HiddenField>
                            <asp:Label ID="lblServiceDate" runat="server" Text="Paid Date"></asp:Label>
                        </div>
                        <div class="r-right">
                            <%--Right Right--%>
                            <asp:CheckBox ID="chkIsGroupPayment" runat="server" Text="" onclick="javascript: return ToggleFieldVisible(this);" />
                            <asp:Label ID="lblIsGroupPayment" runat="server" Text="Group Payment"></asp:Label>
                        </div>
                    </div>
                    <div class="right-float">
                        <div class="r-left">
                            <%--Right Left--%>
                            <asp:TextBox ID="txtPaidDate" runat="server" CssClass="customSmallTextBoxSize"></asp:TextBox>
                        </div>
                        <div class="l-right">
                            <%--Left Right--%>
                            <asp:Label ID="lblGroupPayment" runat="server" Text="Group Payment" Visible="False"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div id="IndividualBillPayment">
                    <div class="HMContainerRow" style="display: none;">
                        <div class="left-float">
                            <div class="l-left">
                                <%-- Left Left--%>
                                <asp:Label ID="lblRoomNumber" runat="server" Text="Paid By"></asp:Label>
                            </div>
                            <div class="r-right">
                                <%--Right Right--%>
                                <asp:DropDownList ID="ddlPaidByRegistrationId" runat="server" CssClass="customMediupXLTextBoxSize"
                                    Enabled="False">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="right-float">
                            <div class="r-left">
                                <%--Right Left--%>
                                <asp:DropDownList ID="ddlPaidByRoomId" runat="server" CssClass="customMediupXLTextBoxSize"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlRoomId_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="l-right">
                                <%--Left Right--%>
                                <asp:Label ID="lblRegistrationNumber" runat="server" Text="Registration Number"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="HMContainerRow">
                        <div class="left-float">
                            <div class="l-left">
                                <%-- Left Left--%>
                                <asp:Label ID="lblNewRoomNumber" runat="server" Text="Room Number"></asp:Label>
                            </div>
                            <div class="r-right">
                                <%--Right Right--%>
                                <asp:DropDownList ID="ddlRegistrationId" runat="server" CssClass="customMediupXLTextBoxSize"
                                    Enabled="False">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="right-float">
                            <div class="r-left">
                                <%--Right Left--%>
                                <asp:DropDownList ID="ddlRoomId" runat="server" CssClass="customMediupXLTextBoxSize"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlRoomId_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="l-right">
                                <%--Left Right--%>
                                <asp:Label ID="lblNewRegiNumber" runat="server" Text="Registration Number"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div style="padding-top: 40px;"></div>
                    <div class="divSection">
                        <div id="myTabs">
                            <ul id="tabPage" class="ui-style">
                                <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none;"><a
                                    href="#tab-1">Bill Summary Information</a></li>
                                <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none; display: none;"><a
                                    href="#tab-2">Room Details </a></li>
                                <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none; display: none;"><a
                                    href="#tab-3">Service Details</a></li>
                                <li id="D" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none; display: none;"><a
                                    href="#tab-4">Restaurant Details</a></li>
                                <%--<li id="E" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                                    href="#tab-5">More Room </a></li>--%>
                            </ul>
                            <div id="tab-1">
                                <div id="ChecquePaymentAccountHeadDiv" style="display: none;">
                                    <div class="divSection">
                                        <div class="divBox divSectionLeftLeft">
                                            <asp:Label ID="lblBankId" runat="server" Text="Bank Name"></asp:Label>
                                        </div>
                                        <div class="divBox divSectionLeftRight">
                                            <asp:DropDownList ID="ddlBankId" runat="server" AutoPostBack="True" TabIndex="6">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="divBox divSectionRightLeft">
                                            <asp:Label ID="lblChecqueNumber" runat="server" Text="Cheque Number"></asp:Label>
                                        </div>
                                        <div class="divBox divSectionRightRight">
                                            <asp:TextBox ID="txtChecqueNumber" runat="server" TabIndex="7"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="divClear">
                                    </div>
                                </div>
                                <div id="CardPaymentAccountHeadDiv" style="display: none;">
                                    <div class="divSection">
                                        <div class="divBox divSectionLeftLeft">
                                            <asp:Label ID="lblCardReference" runat="server" Text="Card Reference"></asp:Label>
                                        </div>
                                        <div class="divBox divSectionLeftRight">
                                            <asp:TextBox ID="txtCardReference" runat="server" CssClass="ThreeColumnTextBox" TabIndex="8"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="divClear">
                                    </div>
                                    <div class="divSection">
                                        <div class="divBox divSectionLeftLeft">
                                            <asp:Label ID="lblCardNumber" runat="server" Text="Card Number"></asp:Label>
                                        </div>
                                        <div class="divBox divSectionLeftRight">
                                            <asp:TextBox ID="txtCardNumber" runat="server" CssClass="customLargeTextBoxSize"
                                                TabIndex="9"></asp:TextBox>
                                        </div>
                                        <div class="divBox divSectionRightLeft">
                                            <asp:Label ID="lblBranchName" runat="server" Text="Branch Name"></asp:Label>
                                        </div>
                                        <div class="divBox divSectionRightRight">
                                            <asp:TextBox ID="txtBranchName" runat="server" CssClass="customLargeXXLTextBoxSize"
                                                TabIndex="10"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="divClear">
                                    </div>
                                </div>
                                <div class="divSection">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="lblVatTotal" runat="server" Text="Vat Amount"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtVatTotal" runat="server" CssClass="customLargeTextBoxSize" ReadOnly="true">0</asp:TextBox>
                                    </div>
                                    <div class="divBox divSectionRightLeft">
                                        <asp:Label ID="lblServiceChargeTotal" runat="server" Text="Service Charge"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionRightRight">
                                        <asp:TextBox ID="txtServiceChargeTotal" runat="server" CssClass="customLargeTextBoxSize"
                                            ReadOnly="true">0</asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                                <div class="divSection">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="lblDiscountAmountTotal" runat="server" Text="Discount Amount"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtDiscountAmountTotal" runat="server" CssClass="customLargeTextBoxSize"
                                            ReadOnly="true">0</asp:TextBox>
                                    </div>
                                    <div class="divBox divSectionRightLeft">
                                        <asp:Label ID="lblAdvancePaymentAmount" runat="server" Text="Advance Amount"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionRightRight">
                                        <asp:TextBox ID="txtAdvancePaymentAmount" runat="server" CssClass="customLargeTextBoxSize"
                                            ReadOnly="true">0</asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                                <div class="divSection">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtGrandTotal" runat="server" CssClass="customLargeTextBoxSize"
                                            ReadOnly="true">0</asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                                <div class="divSection" style="display: none;">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="lblReceivedAmount" runat="server" Text="Received Amount"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtReceivedAmount" runat="server" CssClass="customLargeTextBoxSize"
                                            TabIndex="11">0</asp:TextBox>
                                    </div>
                                    <div class="divBox divSectionRightLeft">
                                        <asp:Label ID="lblDueAmount" runat="server" Text="Due Amount"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionRightRight">
                                        <asp:TextBox ID="txtDueAmount" runat="server" CssClass="customLargeTextBoxSize" TabIndex="12">0</asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                                <div id="AccountsPostingPanel">
                                    <div class="divSection" style="display: none;">
                                        <div class="divBox divSectionLeftLeft">
                                            <asp:Label ID="lblReceiveAmount" runat="server" Text="Receive Amount"></asp:Label>
                                        </div>
                                        <div class="divBox divSectionLeftRight">
                                            <asp:TextBox ID="txtReceiveAmount" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="divBox divSectionRightLeft">
                                            <asp:Label ID="lblConvertionRate" runat="server" Text="Convertion Rate" Visible="false"></asp:Label>
                                        </div>
                                        <div class="divBox divSectionRightRight">
                                            <asp:TextBox ID="txtConvertionRate" runat="server" Visible="false"> </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="divClear">
                                    </div>
                                    <div id="IntegratedGeneralLedgerDiv" style="display: none;">
                                        <div class="divSection">
                                            <div class="divBox divSectionLeftLeft">
                                                <asp:Label ID="lblPaymentAccountHead" runat="server" Text="Account Head"></asp:Label>
                                            </div>
                                            <div class="divBox divSectionLeftRight">
                                                <div id="CashPaymentAccountHeadDiv">
                                                    <asp:DropDownList ID="ddlCashPaymentAccountHead" runat="server" CssClass="ThreeColumnDropDownList">
                                                    </asp:DropDownList>
                                                </div>
                                                <div id="BankPaymentAccountHeadDiv" style="display: none;">
                                                    <asp:DropDownList ID="ddlBankPaymentAccountHead" runat="server" CssClass="ThreeColumnDropDownList">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="divClear">
                                        </div>
                                        <div class="divSection" id="CompanyProjectPanel" style="display: none;">
                                            <div class="divBox divSectionLeftLeft">
                                                <asp:Label ID="lblGLCompany" runat="server" Text="Company"></asp:Label>
                                            </div>
                                            <div class="divBox divSectionLeftRight">
                                                <asp:DropDownList ID="ddlGLCompany" runat="server" onchange="PopulateProjects();">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="divBox divSectionRightLeft">
                                                <asp:Label ID="lblGLProject" runat="server" Text="Project"></asp:Label>
                                            </div>
                                            <div class="divBox divSectionRightRight">
                                                <asp:DropDownList ID="ddlGLProject" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="divClear">
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                            </div>
                            <div id="tab-2" style="display: none;">
                                <div id="Div2" class="block">
                                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Room Detail Information
                                    </a>
                                    <div class="block-body collapse in">
                                        <div>
                                            <asp:Label ID="lblHiddenId" runat="server" Text='' Visible="False"></asp:Label>
                                            <asp:GridView ID="gvRoomDetail" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="50">
                                                <RowStyle BackColor="#E3EAEB" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Date" ItemStyle-Width="15%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblServiceDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ServiceDate"))) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ServiceName" HeaderText="Service Name" ItemStyle-Width="65%">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Room Rent" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblServiceRate" runat="server" Text='<%# bind("ServiceRate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="15%"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Vat" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblVatAmount" runat="server" Text='<%# bind("VatAmount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="8%"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="S. Charge" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblServiceCharge" runat="server" Text='<%# bind("ServiceCharge") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="10%"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Discount" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDiscountAmount" runat="server" Text='<%# bind("DiscountAmount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="8%"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotalAmount" runat="server" Text='<%# bind("TotalAmount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="20%"></ItemStyle>
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
                                <div class="divClear">
                                </div>
                                <div class="divSection" style="display: none;">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="lblIndividualRoomVatAmount" runat="server" Text="Vat Amount"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtIndividualRoomVatAmount" runat="server" CssClass="customLargeTextBoxSize"
                                            ReadOnly="True">0</asp:TextBox>
                                    </div>
                                    <div class="divBox divSectionRightLeft">
                                        <asp:Label ID="lblIndividualRoomServiceCharge" runat="server" Text="Service Charge"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionRightRight">
                                        <asp:TextBox ID="txtIndividualRoomServiceCharge" runat="server" CssClass="customLargeTextBoxSize"
                                            ReadOnly="True">0</asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                                <div class="divSection" style="display: none;">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="lblIndividualRoomDiscountAmount" runat="server" Text="Discount Amount"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtIndividualRoomDiscountAmount" runat="server" CssClass="customLargeTextBoxSize"
                                            ReadOnly="True">0</asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                                <div class="divSection">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="lblIndividualRoomGrandTotal" runat="server" Text="Room Total"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtIndividualRoomGrandTotal" runat="server" CssClass="customLargeTextBoxSize"
                                            ReadOnly="true">0</asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                            </div>
                            <div id="tab-3" style="display: none;">
                                <div id="Div3" class="block">
                                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Service Detail Information
                                    </a>
                                    <div class="block-body collapse in">
                                        <div>
                                            <asp:Label ID="Label2" runat="server" Text='' Visible="False"></asp:Label>
                                            <asp:GridView ID="gvServiceDetail" Width="100%" runat="server" AllowPaging="True"
                                                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                                ForeColor="#333333" PageSize="50">
                                                <RowStyle BackColor="#E3EAEB" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("ServiceBillId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Date" ItemStyle-Width="15%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblServiceDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ServiceDate"))) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ServiceType" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblServiceType" runat="server" Text='<%#Eval("ServiceType") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ServiceId" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Service Name" ItemStyle-Width="65%" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Rate" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblServiceRate" runat="server" Text='<%# bind("ServiceRate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="10%"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quantity" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblServiceQuantity" runat="server" Text='<%# bind("ServiceQuantity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="6%"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Vat" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblVatAmount" runat="server" Text='<%# bind("VatAmount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="8%"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="S. Charge" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblServiceCharge" runat="server" Text='<%# bind("ServiceCharge") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="10%"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Discount" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDiscountAmount" runat="server" Text='<%# bind("DiscountAmount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="8%"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotalAmount" runat="server" Text='<%# bind("TotalAmount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="20%"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="NightAudit" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNightAuditApproved" runat="server" Text='<%# bind("NightAuditApproved") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="20%"></ItemStyle>
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
                                <div class="divClear">
                                </div>
                                <div class="divSection" style="display: none;">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="lblIndividualServiceVatAmount" runat="server" Text="Vat Amount"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtIndividualServiceVatAmount" runat="server" CssClass="customLargeTextBoxSize"
                                            ReadOnly="True">0</asp:TextBox>
                                    </div>
                                    <div class="divBox divSectionRightLeft">
                                        <asp:Label ID="lblIndividualServiceServiceCharge" runat="server" Text="Service Charge"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionRightRight">
                                        <asp:TextBox ID="txtIndividualServiceServiceCharge" runat="server" CssClass="customLargeTextBoxSize"
                                            ReadOnly="True">0</asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                                <div class="divSection" style="display: none;">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="lblIndividualServiceDiscountAmount" runat="server" Text="Discount Amount"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtIndividualServiceDiscountAmount" runat="server" CssClass="customLargeTextBoxSize"
                                            ReadOnly="True">0</asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                                <div class="divSection">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="lblIndividualServiceGrandTotal" runat="server" Text="Service Total"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtIndividualServiceGrandTotal" runat="server" CssClass="customLargeTextBoxSize"
                                            ReadOnly="true">0</asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                            </div>
                            <div id="tab-4" style="display: none;">
                                <div id="RestaurantDivPanel" class="block">
                                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Restaurant Detail
                                        Information </a>
                                    <div>
                                        <asp:Label ID="Label1" runat="server" Text='' Visible="False"></asp:Label>
                                        <asp:GridView ID="gvRestaurantDetail" Width="100%" runat="server" AllowPaging="True"
                                            AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                            ForeColor="#333333" PageSize="50">
                                            <RowStyle BackColor="#E3EAEB" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Date" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBillDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("BillDate"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ItemName" HeaderText="Item Name" ItemStyle-Width="65%">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Unit" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemUnit" runat="server" Text='<%# bind("ItemUnit") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="20%"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit Rate" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitRate" runat="server" Text='<%# bind("UnitRate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="20%"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Discount" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDiscountAmount" runat="server" Text='<%# bind("DiscountAmount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="20%"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRestaurantAmount" runat="server" Text='<%# bind("Amount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="20%"></ItemStyle>
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
                                <div class="divClear">
                                </div>
                                <div class="divSection" style="display: none;">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="lblIndividualRestaurantVatAmount" runat="server" Text="Vat Amount"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtIndividualRestaurantVatAmount" runat="server" CssClass="customLargeTextBoxSize"
                                            ReadOnly="True">0</asp:TextBox>
                                    </div>
                                    <div class="divBox divSectionRightLeft">
                                        <asp:Label ID="lblIndividualRestaurantServiceCharge" runat="server" Text="Service Charge"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionRightRight">
                                        <asp:TextBox ID="txtIndividualRestaurantServiceCharge" runat="server" CssClass="customLargeTextBoxSize"
                                            ReadOnly="True">0</asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                                <div class="divSection" style="display: none;">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="lblIndividualRestaurantDiscountAmount" runat="server" Text="Discount Amount"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtIndividualRestaurantDiscountAmount" runat="server" CssClass="customLargeTextBoxSize"
                                            ReadOnly="True">0</asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                                <div class="divSection">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="lblIndividualRestaurantGrandTotal" runat="server" Text="Restaurant Total"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtIndividualRestaurantGrandTotal" runat="server" CssClass="customLargeTextBoxSize"
                                            ReadOnly="true">0</asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                            </div>
                            <div id="tab-5" style="display: none;">
                                <div id="RoomDeltailInformation" class="block">
                                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Room Detail Information
                                    </a>
                                    <div class="block-body collapse in">
                                        <div>
                                            <asp:GridView ID="gvExtraRoomDetail" Width="100%" runat="server" AllowPaging="True"
                                                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                                ForeColor="#333333" PageSize="5">
                                                <RowStyle BackColor="#E3EAEB" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("CheckOutId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="RoomNumber" HeaderText="Room Number" ItemStyle-Width="20%">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Vat Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblExtraVatAmount" runat="server" Text='<%# bind("VatAmount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="20%"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Service Charge">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblExtraServiceCharge" runat="server" Text='<%# bind("ServiceCharge") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="20%"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Discount Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblExtraDiscountAmount" runat="server" Text='<%# bind("DiscountAmount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="20%"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Total">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblExtraTotalAmount" runat="server" Text='<%# bind("TotalAmount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="25%"></ItemStyle>
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
                                <div class="divClear">
                                </div>
                                <div class="divSection">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="lblIndividualExtraRoomVatAmount" runat="server" Text="Vat Amount"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtIndividualExtraRoomVatAmount" runat="server" CssClass="customLargeTextBoxSize"
                                            ReadOnly="True">0</asp:TextBox>
                                    </div>
                                    <div class="divBox divSectionRightLeft">
                                        <asp:Label ID="lblIndividualExtraRoomServiceCharge" runat="server" Text="Service Charge"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionRightRight">
                                        <asp:TextBox ID="txtIndividualExtraRoomServiceCharge" runat="server" CssClass="customLargeTextBoxSize"
                                            ReadOnly="True">0</asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                                <div class="divSection">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="lblIndividualExtraRoomDiscountAmount" runat="server" Text="Discount Amount"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtIndividualExtraRoomDiscountAmount" runat="server" CssClass="customLargeTextBoxSize"
                                            ReadOnly="True">0</asp:TextBox>
                                    </div>
                                    <div class="divBox divSectionRightLeft">
                                        <asp:Label ID="lblIndividualExtraRoomGrandTotal" runat="server" Text="Total Amount"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionRightRight">
                                        <asp:TextBox ID="txtIndividualExtraRoomGrandTotal" runat="server" CssClass="customLargeTextBoxSize"
                                            ReadOnly="true">0</asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="divClear">
        </div>
        <div id="GroupBillPayment" style="display: none;">
            <div id="Div1" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Room List Information
                </a>
                <div class="block-body collapse in">
                    <asp:GridView ID="gvGroupWiseGuestHouseRoomList" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                        ForeColor="#333333">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblGroupRegistrationId" runat="server" Text='<%#Eval("RegistrationId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBoxAccept" Checked="true" runat="server" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Room Number">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvRoomNumber" runat="server" Text='<%# Bind("RoomNumber") %>'></asp:Label>
                                </ItemTemplate>
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
            </div>
        </div>
        <div class="HMContainerRowButton" style="display: none;">
            <div class="HMContainerRow">
                <div class="l-left" style="text-align: right;">
                    <div id="btnAddDiv">
                    </div>
                    <div id="btnGroupSaveDiv" style="display: none;">
                        <asp:Button ID="btnGroupSave" runat="server" Text="Save All" CssClass="btn btn-primary"
                            TabIndex="7" OnClick="btnGroupSave_Click" Visible="False" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-primary"
                            OnClientClick="javascript: return PerformClearAction();" Visible="False" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary"
                            OnClientClick="javascript: return PerformClearAction();" Visible="False" />
                    </div>
                </div>
            </div>
        </div>
        <div class="divClear">
        </div>
        <div>
            <asp:Button ID="btnSave" runat="server" Text="Add" CssClass="btn btn-primary" TabIndex="7" Width="220px"
                OnClick="btnSave_Click" />
            <asp:Button ID="btnBackToCheckOutForm" runat="server" Width="220px" Text="<< Back to Check Out Form"
                CssClass="btn btn-primary" OnClick="btnBackToCheckOutForm_Click" />
        </div>
    </div>
    <div id="SearchPanel" class="block" style="display: none;">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Room wise Bill Details
            Information </a>
        <div class="block-body collapse in">
            <asp:GridView ID="gvGHServiceBill" Width="100%" runat="server" AllowPaging="True"
                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                ForeColor="#333333" PageSize="5" TabIndex="9" OnPageIndexChanging="gvGHServiceBill_PageIndexChanging"
                OnRowDataBound="gvGHServiceBill_RowDataBound">
                <RowStyle BackColor="#E3EAEB" />
                <Columns>
                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("CheckOutId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RoomNumber" HeaderText="Room Number" ItemStyle-Width="20%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" ItemStyle-Width="15%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="VatAmount" HeaderText="Vat Amount" ItemStyle-Width="15%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DiscountAmount" HeaderText="Discount Amount" ItemStyle-Width="15%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TotalAmount" HeaderText="Total Total" ItemStyle-Width="25%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" Visible="False" />
                            &nbsp;
                            <asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                ImageUrl="~/Images/delete.png" Text="" AlternateText="Delete" ToolTip="Delete" />
                        </ItemTemplate>
                        <ControlStyle Font-Size="Small" />
                        <HeaderStyle HorizontalAlign="Left" />
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
