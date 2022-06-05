<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmObjectPermission.aspx.cs" Inherits="HotelManagement.Presentation.Website.UserInformation.frmObjectPermission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            IsCanSave = $("#<%=hfSavePermission.ClientID %>").val() == '1' ? true : false;
            IsCanEdit = $("#<%=hfEditPermission.ClientID %>").val() == '1' ? true : false;
            IsCanDelete = $("#<%=hfDeletePermission.ClientID %>").val() == '1' ? true : false;
            IsCanView = $("#<%=hfViewPermission.ClientID %>").val() == '1' ? true : false;
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Authorization Panel</a>";
            var formName = "<span class='divider'>/</span><li class='active'>User Permission</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $('#ContentPlaceHolder1_gvObjectPermission_ChkCreate').click(function () {
                if ($('#ContentPlaceHolder1_gvObjectPermission_ChkCreate').is(':checked')) {
                    CheckAllCheckBoxCreate()
                }
                else {
                    UnCheckAllCheckBoxCreate();
                }
            });


            $('#ContentPlaceHolder1_gvObjectPermission_chkDelete').click(function () {
                if ($('#ContentPlaceHolder1_gvObjectPermission_chkDelete').is(':checked')) {
                    CheckAllCheckBoxDelete()
                }
                else {
                    UnCheckAllCheckBoxDelete();
                }
            });

            $('#ContentPlaceHolder1_gvObjectPermission_ChkView').click(function () {
                if ($('#ContentPlaceHolder1_gvObjectPermission_ChkView').is(':checked')) {
                    CheckAllCheckBoxView()
                }
                else {
                    UnCheckAllCheckBoxView();
                }
            });


        });


        function CheckAllCheckBoxCreate() {
            $('.Chk_Create input[type=checkbox]').each(function () {
                $(this).prop("checked", true);
            });
        }

        function UnCheckAllCheckBoxCreate() {
            $('.Chk_Create input[type=checkbox]').each(function () {
                $(this).prop("checked", false);
            });
        }


        function CheckAllCheckBoxDelete() {
            $('.chk_Delete input[type=checkbox]').each(function () {
                $(this).prop("checked", true);
            });
        }

        function UnCheckAllCheckBoxDelete() {
            $('.chk_Delete input[type=checkbox]').each(function () {
                $(this).prop("checked", false);
            });
        }

        function CheckAllCheckBoxView() {
            $('.chk_View input[type=checkbox]').each(function () {
                $(this).prop("checked", true);
            });
        }

        function UnCheckAllCheckBoxView() {
            $('.chk_View input[type=checkbox]').each(function () {
                $(this).prop("checked", false);
            });
        }
        
    </script>
    <style>
        .ChkCreate
        {
            padding-left: 12px;
        }
        
        .chkDelete
        {
            padding-left: 11px;
        }
        .chkView
        {
            padding-left: 10px;
        }
        .lblHeader
        {

        }
    </style> 
       <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <div class="divClear">
    </div>
    <div id="SearchPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">User Permission Details
            Information </a>
        <div class="HMBodyContainer">
            <div class="block-body collapse in">
                <div class="HMContainerRow">
                    <div class="l-left">
                        <%--Right Left--%>
                        <asp:Label ID="lblUserGroupId" runat="server" Text="User Group"></asp:Label>
                    </div>
                    <div class="r-left">
                        <%--Right Left--%>
                        <asp:DropDownList ID="ddlUserGroupId" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlUserGroupId_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div id="Div1" class="block">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Module Wise Permission
                    </a>
                    <div class="HMBodyContainer">
                        <div class="block-body collapse in">
                            <div class="HMContainerRow">
                                <div class="l-left">
                                    <%--Left Left--%>
                                    <asp:Label ID="lblSelectSetup" runat="server" Text="Module Name"></asp:Label>
                                </div>
                                <div class="r-left">
                                    <asp:DropDownList ID="ddlGroupHead" runat="server" CssClass="ThreeColumnDropDownList"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlGroupHead_SelectedIndexChanged">
                                        <%--<asp:ListItem Value="None">--- None ---</asp:ListItem>--%>
                                        <%--<asp:ListItem Value="grpDashboard">Company Information</asp:ListItem>
                                        <asp:ListItem Value="grpGeneralLedger">General Ledger</asp:ListItem>
                                        <asp:ListItem Value="grpReportGeneralLedger">Report: General Ledger</asp:ListItem>
                                        <asp:ListItem Value="grpHotelManagement">Hotel Management</asp:ListItem>
                                        <asp:ListItem Value="grpReportHotelManagement">Report: Hotel Management</asp:ListItem>
                                        <asp:ListItem Value="grpRestaurantManagement">Restaurant Management</asp:ListItem>
                                        <asp:ListItem Value="grpReportRestaurant">Report: Restaurant Management</asp:ListItem>
                                        <asp:ListItem Value="grpInventoryManagement">Inventory Management</asp:ListItem>
                                        <asp:ListItem Value="grpReportInventoryManagement">Report: Inventory Management</asp:ListItem>
                                        <asp:ListItem Value="grpPurchaseManagement">Purchase Management</asp:ListItem>
                                        <asp:ListItem Value="grpReportPurchaseManagement">Report: Purchase Management</asp:ListItem>
                                        <asp:ListItem Value="grpSalesManagement">Sales Management</asp:ListItem>
                                        <asp:ListItem Value="grpReportSalesManagement">Report: Sales Management</asp:ListItem>                                        
                                        <asp:ListItem Value="grpStockManagement">Stock Management</asp:ListItem>
                                        <asp:ListItem Value="grpReportStockManagement">Report: Stock Management</asp:ListItem>
                                        <asp:ListItem Value="grpPayrollManagement">Payroll Management</asp:ListItem>
                                        <asp:ListItem Value="grpReportPayrollManagement">Report: Payroll Management</asp:ListItem>
                                        <asp:ListItem Value="grpUserPanel">User Information</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div>
                                <asp:GridView ID="gvObjectPermission" Width="100%" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                    ForeColor="#333333" PageSize="200" OnPageIndexChanging="gvObjectPermission_PageIndexChanging"
                                    OnRowDataBound="gvObjectPermission_RowDataBound">
                                    <RowStyle BackColor="#E3EAEB" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblObjectPermissionId" runat="server" Text='<%#Eval("ObjectPermissionId") %>'></asp:Label></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ObjectTabId" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblObjectTabId" runat="server" Text='<%#Eval("ObjectTabId") %>'></asp:Label></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Menu Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvMenuHead" runat="server" Text='<%# Bind("MenuHead") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type" ItemStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvObjectType" runat="server" Text='<%# Bind("ObjectType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Create/Update" ItemStyle-Width="05%">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label2" CssClass="lblHeader" runat="server">Create</asp:Label>
                                                <asp:CheckBox ID="ChkCreate" CssClass="ChkCreate" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSavePermission" CssClass="Chk_Create" runat="server" />
                                                <asp:Label ID="lblchkIsSavePermission" runat="server" Text='<%#Eval("IsSavePermission") %>'
                                                    Visible="False"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete" ItemStyle-Width="05%">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" CssClass="lblHeader" >Delete</asp:Label>
                                                <asp:CheckBox ID="chkDelete" CssClass="chkDelete" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsDeletePermission" CssClass="chk_Delete" runat="server" />
                                                <asp:Label ID="lblchkIsDeletePermission" runat="server" Text='<%#Eval("IsDeletePermission") %>'
                                                    Visible="False"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="View" ItemStyle-Width="05%">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label1"  CssClass="lblHeader" runat="server">View</asp:Label>
                                                <asp:CheckBox ID="ChkView" CssClass="chkView" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsViewPermission" CssClass="chk_View" runat="server" />
                                                <asp:Label ID="lblchkIsViewPermission" runat="server" Text='<%#Eval("IsViewPermission") %>'
                                                    Visible="False"></asp:Label>
                                            </ItemTemplate>
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
                            <div class="divClear">
                            </div>
                            <div class="HMContainerRowButton">
                                <%--Right Left--%>
                                <asp:Button ID="btnSaveAll" runat="server" Text="Approve Permission" CssClass="btn btn-primary"
                                    OnClick="btnSaveAll_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>    
</asp:Content>
