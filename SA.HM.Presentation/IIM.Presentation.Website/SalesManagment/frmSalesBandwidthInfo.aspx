<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmSalesBandwidthInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesManagment.frmSalesBandwidthInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var moduleName = "<a href=\"/HMCommon/frmHMHome.aspx\">Sales Management</a>";
            var formName = "<span class=\"divider\">/</span><li class=\"active\">Bandwidth Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#myTabs").tabs();

        });

        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=txtBandwidthInfoId.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");

            $("#frmHotelManagement")[0].reset();

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
            x
        </button>
        <asp:Label ID="lblMessage" runat="server" Font-Bold="true"></asp:Label>
    </div>
    <div style="height: 45px;">
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none;"><a
                href="#tab-1">Bandwidth Enty</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none;"><a
                href="#tab-2">Bandwidth Search</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Bandwidth Information</a>
                <div class="HMBodyContainer">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:HiddenField ID="txtBandwidthInfoId" runat="server" />
                            <label id="lblBandwidthName">
                                Name</label><span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtBandwidthName" runat="server" CssClass="ThreeColumnTextBox" TabIndex="1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <label id="lblBandwidthType">
                                Type</label><span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlBandwidthType" runat="server" TabIndex="2">
                                <asp:ListItem Text="Bandwidth Type" Value="BandwidthType"></asp:ListItem>
                                <asp:ListItem Text="Bandwidth" Value="Bandwidth"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <label id="lblActiveStat">
                                Status</label><span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:DropDownList ID="ddlActiveStat" runat="server" TabIndex="3">
                                <asp:ListItem Text="Active" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Inactive" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="HMContainerRowButton">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                            TabIndex="4" OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                            TabIndex="5" OnClientClick="javascript: return PerformClearAction();" />
                    </div>
                </div>
            </div>
            <div class="divClear">
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Bandwidth Information</a>
                <div class="HMBodyContainer">
                    <div class="divSection ">
                        <div class="divBox divSectionLeftLeft">
                            <label id="lblSBandwidthName">
                                Name
                            </label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtSBandwidthName" runat="server" CssClass="ThreeColumnTextBox"
                                TabIndex="1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <label id="lblSBandwidthType">
                                Type</label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlSBandwidthType" runat="server" TabIndex="2">
                                <asp:ListItem Text="Bandwidth Type" Value="BandwidthType"></asp:ListItem>
                                <asp:ListItem Text="Bandwidth" Value="Bandwidth"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <label id="lblSActiveStat">
                                Status</label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:DropDownList ID="ddlSActiveStat" runat="server" TabIndex="3">
                                <asp:ListItem Text="Active" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Inactive" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="HMContainerRowButton">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" TabIndex="4" CssClass="TransactionalButton btn btn-primary"
                            OnClick="btnSearch_Click" />
                    </div>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div id="SearchPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Information</a>
                <div class="block-body collapse in">
                    <asp:GridView ID="gvBandwidthType" runat="server" Width="100%" AllowPaging="true"
                        AutoGenerateColumns="false" CellPadding="4" GridLines="None" PageSize="30" AllowSorting="true"
                        ForeColor="#333333" OnPageIndexChanging="gvBandwidthType_PageIndexChanging" OnRowCommand="gvBandwidthType_RowCommand"
                        OnRowDataBound="gvBandwidthType_RowDataBound">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("BandwidthInfoId") %>'></asp:Label>
                                    <asp:Label ID="lblBandwidthTypeId" runat="server" Text='<%#Eval("BandwidthType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="BandwidthName" HeaderText="Bandwidth Name" ItemStyle-Width="40%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BandwidthType" HeaderText="Bandwidth Type" ItemStyle-Width="30%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActiveStatus" HeaderText="Status" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("BandwidthInfoId") %>' ImageUrl="~/Images/edit.png"
                                        Text="" AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("BandwidthInfoId") %>' ImageUrl="~/Images/delete.png"
                                        Text="" AlternateText="Delete" ToolTip="Delete" OnClientClick="return confirm('Do you want to Delete?');" />
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

    </script>
</asp:Content>
