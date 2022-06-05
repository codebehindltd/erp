<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmKitchenInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmKitchenInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            /*var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Restaurant Kitchen</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);*/

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {

            $("#<%=txtBankName.ClientID %>").val(result.KitchenName);
            $("#<%=ddlActiveStat.ClientID %>").val(result.ActiveStat == true ? 0 : 1);
            $("#<%=hfKitchenId.ClientID %>").val(result.KitchenId);
            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewBank').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }

        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
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
            CommonHelper.AlertMessage(resizeBy.AlertMessage);
        }
        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());

        }
        //For ClearForm-------------------------
        function PerformClearActionForButton() {
             if (!confirm("Do you want to clear?")) {
                return false;
            }
            PerformClearAction();

        }

        function PerformClearAction() {
            $("#<%=txtBankName.ClientID %>").val('');
            $("#<%=ddlActiveStat.ClientID %>").val(0);
            $("#<%=hfKitchenId.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");

            return false;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewBank').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewBank').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }

        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewBank').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewBank').hide("slow");
        }
        $(function () {
            $("#myTabs").tabs();
        });
    </script>

    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Kitchen Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Kitchen </a></li>
        </ul>
        <div id="tab-1">
            <%--<div id="EntryPanel" class="block">--%>
            <%--<a href="#page-stats" class="block-heading" data-toggle="collapse">Kitchen Information
                </a>--%>
            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="form-horizontal">
                        <%--<div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblCostCenter" runat="server" Text="Cost Center"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="ThreeColumnDropDownList">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>--%>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="hfKitchenId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblBankName" runat="server" class="control-label required-field" Text="Kitchen Name"></asp:Label>                                
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="form-control"
                                    TabIndex="2">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="CategoryCostCenterInformationDiv" class="panel panel-default">
                            <div class="panel-body">
                                <div>
                                    <asp:GridView ID="gvCategoryCostCenterInfo" Width="100%" runat="server" AllowPaging="True"
                                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                        ForeColor="#333333" PageSize="200" CssClass="table table-bordered table-condensed table-responsive">
                                        <RowStyle BackColor="#E3EAEB" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCostCentreId" runat="server" Text='<%#Eval("CostCenterId") %>'></asp:Label></ItemTemplate>
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
                                            <asp:TemplateField HeaderText="Cost Center Information" ItemStyle-Width="55%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvCostCentre" runat="server" Text='<%# Bind("CostCenter") %>'></asp:Label>
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
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" TabIndex="3" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="4" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearActionForButton();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--</div>--%>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Kitchen Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSBankName" runat="server" class="control-label" Text="Kitchen Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSBankName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSActiveStat" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSearch_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information</div>
                <div class="panel-body">
                    <asp:GridView ID="gvGuestHouseService" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True"
                        ForeColor="#333333" OnPageIndexChanging="gvGuestHouseService_PageIndexChanging"
                        OnRowCommand="gvGuestHouseService_RowCommand" OnRowDataBound="gvGuestHouseService_RowDataBound"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("KitchenId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="KitchenName" HeaderText="Kitchen Name" ItemStyle-Width="30%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CostCenter" HeaderText="Cost Center" ItemStyle-Width="30%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActiveStatus" HeaderText="Status" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" OnClientClick="return confirm('Do you want to edit?');" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("KitchenId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("KitchenId") %>' ImageUrl="~/Images/delete.png" Text=""
                                        AlternateText="Delete" ToolTip="Delete" OnClientClick="return confirm('Do you want to delete?');" />
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
            if (parseInt(xNewAdd) == 2) {
                $('#btnNewBank').hide();
                $('#EntryPanel').show();
            }
        }
        else {
            NewAddButtonPanelHide();
        }
        
    </script>
</asp:Content>
