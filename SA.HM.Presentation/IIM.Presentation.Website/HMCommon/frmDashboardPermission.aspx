<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmDashboardPermission.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.frmDashboardPermission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Dashboard</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Dashboard Management</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });

    </script>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Dashboard Management Details</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">                        
                        <asp:Label ID="lblUserGroupId" runat="server" class="control-label" Text="User Group"></asp:Label>
                    </div>
                    <div class="col-md-4">                       
                        <asp:DropDownList ID="ddlUserGroupId" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlUserGroupId_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <div style="padding-top:5px;">
                    <asp:GridView ID="gvDashboardManagement" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                        ForeColor="#333333" PageSize="50" OnRowDataBound="gvDashboardManagement_RowDataBound"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblItemId" runat="server" Text='<%#Eval("Id") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Item Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblUserMappingId" runat="server" Text='<%# Eval("UserMappingId") %>'
                                        Visible="False"></asp:Label>
                                    <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("ItemName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Added Status" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkIsActiveStatus" runat="server" />
                                    <asp:Label ID="lblchkIsActiveStatus" runat="server" Text='<%#Eval("ActiveStatus") %>'
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
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary btn-sm"
                            OnClick="btnSave_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
