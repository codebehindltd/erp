<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmPMSalesReturn.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesManagment.frmPMSalesReturn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Sales Managment</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Sales Return</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });

        function EntryPanelVisibleTrue() {
            $('#btnNewProduct').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewProduct').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }

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
    <div id="EntryPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Product Return Information
        </a>
        <div class="HMBodyContainer">
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblSalesNumber" runat="server" Text="Bill Number"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlBillNumber" runat="server" AutoPostBack="True" CssClass="ThreeColumnDropDownList"
                        OnSelectedIndexChanged="ddlBillNumber_SelectedIndexChanged" TabIndex="1">
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlReturnType" runat="server" TabIndex="1" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlReturnType_SelectedIndexChanged" Visible="False">
                        <asp:ListItem> Sales</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblTransactionFor" runat="server" Text="Product Name"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtControlTransationFor" runat="server" CssClass="ThreeColumnTextBox" TabIndex="2"
                        ReadOnly="True"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblServiceType" runat="server" Text="Product Name"></asp:Label>
                    <span class="MandatoryField">*</span>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlProductId" runat="server" CssClass="ThreeColumnDropDownList"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlProductId_SelectedIndexChanged"
                        TabIndex="3">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblQuantityLabel" runat="server" Text="Quantity Ordered"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:Label ID="lblPOQuantity" runat="server" Text=""></asp:Label>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblReceivedQuantityLabel" runat="server" Text="Received Quantity"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:Label ID="lblReceivedQuantity" runat="server" Text=""></asp:Label>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblQuantity_Serial" runat="server" Text="Quantity"></asp:Label>
                    <span class="MandatoryField">*</span>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtQuantity_Serial" runat="server" TabIndex="4"></asp:TextBox>
                    <asp:Label ID="lblHiddenOrderDetailtId" runat="server" Visible="False"></asp:Label>
                          <div style="height:5px"></div>      
                    <asp:Button ID="btnAdd" runat="server" Text="Add"  CssClass="TransactionalButton btn btn-primary"
                        OnClick="btnAdd_Click" TabIndex="5" />
                </div>
            </div>

              <div class="HMContainerRowButton">
              
              </div>
            <div class="divClear">
            </div>
            <div class="block-body collapse in">
                <asp:GridView ID="gvProductReceive" Width="100%" runat="server" AllowPaging="True"
                    AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                    ForeColor="#333333" PageSize="30" OnPageIndexChanging="gvProductReceive_PageIndexChanging"
                    OnRowCommand="gvProductReceive_RowCommand" OnRowDataBound="gvProductReceive_RowDataBound">
                    <RowStyle BackColor="#E3EAEB" />
                    <Columns>
                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblid" runat="server" Text='<%#Eval("ReturnId") %>'></asp:Label></ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" ItemStyle-Width="25%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SerialNumber" HeaderText="Serial Number" ItemStyle-Width="35%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" ItemStyle-Width="35%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="35%">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                    CommandArgument='<%# bind("ReturnId") %>' ImageUrl="~/Images/delete.png" Text=""
                                    AlternateText="Delete" ToolTip="Delete" />
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
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblRemarks" runat="server" Text="Remarks"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtRemarks" CssClass="ThreeColumnTextBox" runat="server" TabIndex="6" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="HMContainerRowButton">
                <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="9" CssClass="TransactionalButton btn btn-primary"
                    OnClick="btnSave_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="10" CssClass="TransactionalButton btn btn-primary"
                    OnClientClick="javascript: return PerformClearAction();" />
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var x = '<%=btnPadding%>';
        var isMessageBoxEnable = '<%=isMessageBoxEnable%>';

        if (x > -1) {

            $("#<%=btnAdd.ClientID %>").animate({ marginTop: '10px' });
        }
        else {

        }

        if (isMessageBoxEnable == 1) {
            MessagePanelShow();
        }
        else {
            MessagePanelHide();
        }
    </script>
</asp:Content>
