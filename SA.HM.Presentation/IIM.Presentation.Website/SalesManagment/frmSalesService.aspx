<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmSalesService.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesManagment.frmSalesService" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Sales Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Service Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });

        $(function () {
            $("#myTabs").tabs();
        });

        function PerformClearAction() {
            $("#<%=lblMessage.ClientID %>").text('');
            $("#<%=txtServiceId.ClientID %>").val('');
            $("#<%=txtCode.ClientID %>").val('');
            $("#<%=txtName.ClientID %>").val('');
            $("#<%=ddlCategoryId.ClientID %>").val(0);
            $("#<%=ddlFrequency.ClientID %>").val(0);
            $("#<%=txtPurchasePrice.ClientID %>").val('');
            $("#<%=ddlSellingPriceLocal.ClientID %>").val(45);
            $("#<%=txtSellingPriceLocal.ClientID %>").val('');
            $("#<%=ddlSellingPriceUsd.ClientID %>").val(46);
            $("#<%=txtSellingPriceUsd.ClientID %>").val('');
            $("#<%=txtDescription.ClientID %>").val('');
            $("#<%= ddlBandwidthType.ClientID %>").index(0);
            $("#<%= ddlBandwidth.ClientID %>").index(0);

            $("#<%=btnSave.ClientID %>").val("Save");
            MessagePanelHide();
            return false;
        }

        function PerformSearchClearAction() {
            $("#<%=lblMessage.ClientID %>").text('');
            $("#<%=txtSearchCode.ClientID %>").val('');
            $("#<%=txtSearchName.ClientID %>").val('');
            MessagePanelHide();
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
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Service Info</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Service </a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Creat Service</a>
                <div class="HMBodyContainer">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:HiddenField ID="txtServiceId" runat="server"></asp:HiddenField>
                            <asp:Label ID="lblName" runat="server" Text="Service Name"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtName" runat="server" TabIndex="1" CssClass="ThreeColumnTextBox"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblCode" runat="server" Text="Code"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtCode" TabIndex="2" runat="server"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblCategoryId" runat="server" Text="Category"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:DropDownList ID="ddlCategoryId" TabIndex="3" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblFrequency" runat="server" Text="Service Type"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlFrequency" TabIndex="4" runat="server">
                                <asp:ListItem Value="One Time">One Time</asp:ListItem>
                                <asp:ListItem>Recurring</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblSellingPriceLocal" runat="server" Text="Selling Price One"></asp:Label>
                            <%--<span class="MandatoryField">*</span>--%>
                            <asp:DropDownList ID="ddlSellingPriceLocal" runat="server" CssClass="customSmallDropDownSize"
                                TabIndex="5" Visible="False">
                            </asp:DropDownList>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtSellingPriceLocal" runat="server" TabIndex="6"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div id="bandwidthTypeHide" runat="server">
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblBandwidthType" runat="server" Text="Bandwidth Type"> </asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:DropDownList ID="ddlBandwidthType" TabIndex="7" runat="server" CssClass="customSmallDropDownSize">
                                </asp:DropDownList>
                            </div>
                            <div class=" divBox divSectionRightLeft">
                                <asp:Label ID="lblBandwidth" runat="server" Text="Bandwidth"></asp:Label>
                            </div>
                            <div class="divBox divSectionRightRight">
                                <asp:DropDownList ID="ddlBandwidth" TabIndex="8" runat="server" CssClass="customSmallDropDownSize">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div id="USDCurrencyInfo" runat="server">
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblPurchasePrice" runat="server" Text="Purchase Price (USD)"></asp:Label>
                                <%--<span class="MandatoryField">*</span>--%>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtPurchasePrice" TabIndex="9" runat="server" ></asp:TextBox>
                            </div>
                            <div class="divBox divSectionRightLeft">
                                <asp:Label ID="lblSellingPriceUsd" runat="server" Text="Selling Price Two"></asp:Label>
                                <%--<span class="MandatoryField">*</span>--%>
                                <asp:DropDownList ID="ddlSellingPriceUsd" TabIndex="10" runat="server" CssClass="customSmallDropDownSize"
                                     Visible="False">
                                </asp:DropDownList>
                            </div>
                            <div class="divBox divSectionRightRight">
                                <asp:TextBox ID="txtSellingPriceUsd" runat="server" TabIndex="11"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="ThreeColumnTextBox" TextMode="MultiLine"
                                TabIndex="12"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="HMContainerRowButton">
                        <%--Right Left--%>
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="javascript: return SaveValidation();"
                            OnClick="btnSave_Click" CssClass="TransactionalButton btn btn-primary" TabIndex="13" />
                        <asp:Button ID="btnClear" runat="server" TabIndex="14" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                            OnClientClick="javascript: return PerformClearAction();" />
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="InfoPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Customer
                </a>
                <div class="HMBodyContainer">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblSearchName" runat="server" Text="Service Name"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtSearchName" runat="server" TabIndex="1" CssClass="ThreeColumnTextBox"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblSearchCode" runat="server" Text="Service Code"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtSearchCode" TabIndex="2" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                </div>
                <div class="HMContainerRowButton">
                    <%--Right Left--%>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                        CssClass="TransactionalButton btn btn-primary" TabIndex="3" />
                    <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                        OnClientClick="javascript: return PerformSearchClearAction();" TabIndex="4" />
                </div>
            </div>
            <div id="SearchPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Information
                </a>
                <div class="block-body collapse in">
                    <asp:GridView ID="gvSalesService" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                        ForeColor="#333333" PageSize="5" OnPageIndexChanging="gvSalesService_PageIndexChanging"
                        OnRowDataBound="gvSalesService_RowDataBound" OnRowCommand="gvSalesService_RowCommand"
                        TabIndex="9">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("ServiceId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Service Name" ItemStyle-Width="30%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Code" HeaderText="Code" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PurchasePrice" HeaderText="Purchase Price" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UnitPriceLocal" HeaderText="Selling Price Local" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UnitPriceUsd" HeaderText="Selling Price USD" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("ServiceId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("ServiceId") %>' ImageUrl="~/Images/delete.png" Text=""
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
    </script>
</asp:Content>
