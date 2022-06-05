<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmUserInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.UserInformation.frmUserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false
        $(document).ready(function () {
            IsCanSave = $("#<%=hfSavePermission.ClientID %>").val() == '1' ? true : false;
            IsCanEdit = $("#<%=hfEditPermission.ClientID %>").val() == '1' ? true : false;
            IsCanDelete = $("#<%=hfDeletePermission.ClientID %>").val() == '1' ? true : false;
            IsCanView = $("#<%=hfViewPermission.ClientID %>").val() == '1' ? true : false;
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Authorization Panel</a>";
            var formName = "<span class='divider'>/</span><li class='active'>User Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("[id=ContentPlaceHolder1_gvAdminAuthorization_ChkCreate]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#ContentPlaceHolder1_gvAdminAuthorization tbody tr").find("td:eq(0) > span").find("input").prop("checked", true);
                }
                else {
                    $("#ContentPlaceHolder1_gvAdminAuthorization tbody tr").find("td:eq(0) > span").find("input").prop("checked", false);
                }
            });

            $("#<%=txtUserEmail.ClientID %>").blur(function () {
                var A = validEmail($("#<%=txtUserEmail.ClientID %>").val());
                if (A == true) {
                }
                else {
                    toastr.warning("Email is not in correct format.");
                }
            });

            $("#ContentPlaceHolder1_ddlEmployee").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlEmployee").change(function () {
                if ($("#ContentPlaceHolder1_ddlEmployee").val() != "0") {
                    $("#ContentPlaceHolder1_txtUserName").val($("#ContentPlaceHolder1_ddlEmployee option:selected").text());
                }
            });
        });

        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=ddlUserGroupId.ClientID %>").val(0);
            $("#<%=txtUserName.ClientID %>").val('');
            $("#<%=txtUserId.ClientID %>").val('');
            $("#<%=txtUserPassword.ClientID %>").val('');
            $("#<%=txtUserEmail.ClientID %>").val('');
            $("#<%=txtUserPhone.ClientID %>").val('');
            $("#<%=txtUserDesignation.ClientID %>").val('');
            $("#<%=ddlActiveStat.ClientID %>").val(0);
            $("#<%=txtUserInfoId.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }
        //For Password Validation-------------------------
        function confirmPass() {
            if ($("#<%=txtUserPassword.ClientID %>").val() != $("#<%=txtUserConfirmPassword.ClientID %>").val()) {
                toastr.warning('Wrong confirm password !');
                return false;
            }
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewUser').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewUser').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }

        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewUser').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewUser').hide("slow");
        }

        $(function () {
            $("#myTabs").tabs();
        });
    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">User Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search User </a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    User Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Employee"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="txtUserInfoId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblUserName" runat="server" class="control-label" Text="Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblUserGroupId" runat="server" class="control-label required-field" Text="User Group"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlUserGroupId" runat="server" CssClass="form-control" TabIndex="2">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblUserId" runat="server" class="control-label required-field" Text="User Id"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtUserId" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblUserPassword" runat="server" class="control-label required-field" Text="Password"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtUserPassword" runat="server" CssClass="form-control" TabIndex="4"
                                    TextMode="Password"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblUserConfirmPassword" runat="server" class="control-label required-field" Text="Confirm Pass."></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtUserConfirmPassword" runat="server" onblur="javascript: return confirmPass();"
                                    CssClass="form-control" TabIndex="5" TextMode="Password"></asp:TextBox>
                            </div>
                        </div>
                        <asp:Panel ID="pnlDesignationInformation" runat="server" Visible="false">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblUserDesignation" runat="server" class="control-label" Text="Designation"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtUserDesignation" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblUserEmail" runat="server" class="control-label" Text="User Email"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtUserEmail" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblUserPhone" runat="server" class="control-label" Text="User Phone"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtUserPhone" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblActiveStat" runat="server" class="control-label required-field" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="form-control" TabIndex="8">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2" runat="server" id="IsAdminUserLabelDiv" style="display:none;">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Is Admin User"></asp:Label>
                            </div>
                            <div class="col-md-4" runat="server" id="IsAdminUserDropDownDiv" style="display:none;">
                                <asp:DropDownList ID="ddlIsAdminUser" runat="server" CssClass="form-control" TabIndex="8">
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="AdminAuthorizationInformationDiv" runat="server" class="panel panel-default">
                            <div class="panel-heading">
                                Admin Authorization
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="gvAdminAuthorization" Width="100%" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                    ForeColor="#333333" PageSize="200" CssClass="table table-bordered table-condensed table-responsive">
                                    <RowStyle BackColor="#E3EAEB" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModuleId" runat="server" Text='<%#Eval("ModuleId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Create/Update" ItemStyle-Width="05%">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ChkCreate" CssClass="ChkCreate" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSavePermission" CssClass="Chk_Create" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Module Name" ItemStyle-Width="55%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvModuleName" runat="server" Text='<%# Bind("ModuleName") %>'></asp:Label>
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



                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    TabIndex="9" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="10" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearAction();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    User Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblSName" runat="server" class="control-label" Text="Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSUserGroup" runat="server" class="control-label" Text="User Group"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSUserGroup" runat="server" CssClass="form-control" TabIndex="2">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSUserId" runat="server" class="control-label" Text="User Id"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSUserId" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSUserEMail" runat="server" class="control-label" Text="User Email"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSUserEMail" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSUserPhone" runat="server" class="control-label" Text="User Phone"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSUserPhone" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSActiveStat" runat="server" CssClass="form-control" TabIndex="8">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                    CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="9" />
                                <asp:Button ID="btnClearSearch" runat="server" OnClick="btnClearSearch_Click" TabIndex="10"
                                    Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <asp:GridView ID="gvUserInformation" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                        ForeColor="#333333" PageSize="100" OnPageIndexChanging="gvUserInformation_PageIndexChanging"
                        TabIndex="12" OnRowDataBound="gvUserInformation_RowDataBound" OnRowCommand="gvUserInformation_RowCommand"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("UserInfoId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="UserId" HeaderText="User Id" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UserName" HeaderText="Name" ItemStyle-Width="50%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActiveStatus" HeaderText="Status" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("UserInfoId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("UserInfoId") %>' ImageUrl="~/Images/delete.png" Text=""
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
        var xNewAdd = '<%=isNewAddButtonEnable%>';
        if (xNewAdd > -1) {
            NewAddButtonPanelShow();
        }
        else {
            NewAddButtonPanelHide();
        }
    </script>
</asp:Content>
