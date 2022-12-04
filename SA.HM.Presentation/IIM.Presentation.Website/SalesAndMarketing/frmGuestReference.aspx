<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmGuestReference.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.frmGuestReference" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Sales & Marketing</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Guest Reference</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_txtSalesCommission").keypress(function (e) {
                //if the letter is not digit then display error and don't type anything
                // “.” CHECK DOT, AND ONLY ONE.
                if ((e.which != 46 || $("#ContentPlaceHolder1_txtSalesCommission").val().indexOf('.') != -1) && (e.which < 48 || e.which > 57)) {
                    //display error message
                    toastr.warning("Numbers Only");
                    return false;
                }
            });

            $("#ContentPlaceHolder1_txtSalesCommission").change(function () {
                CheckValidation();
            });

        });

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {

            $("#<%=txtDescription.ClientID %>").val(result.Description);
            $("#<%=txtName.ClientID %>").val(result.Name);
            $("#<%=txtSalesCommission.ClientID %>").val(result.SalesCommission);
            $("#<%=txtReferenceId.ClientID %>").val(result.ReferenceId);
            $("#<%=txtEmail.ClientID %>").val(result.Email);
            $("#<%=txtOrganization.ClientID %>").val(result.Organization);
            $("#<%=txtDesignation.ClientID %>").val(result.Designation);
            $("#<%=txtCellNumber.ClientID %>").val(result.CellNumber);
            $("#<%=txtTelephoneNumber.ClientID %>").val(result.TelephoneNumber);
            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewCatagory').hide("slow");
            $('#EntryPanel').show("slow");
        }

        function OnFillFormObjectFailed(error) {
            toastr.error(error);
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
            window.location = "frmGuestReference.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            toastr.error(error);
        }

        function CheckValidation() {
            var discountVal = $("#ContentPlaceHolder1_txtSalesCommission").val();
            if (!CommonHelper.IsDecimal(discountVal)) {
                toastr.warning("Please Enter Valid Sales Commission.");
                return false;
            }
        }


        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=txtDescription.ClientID %>").val('');
            $("#<%=txtName.ClientID %>").val("");

            $("#<%=txtEmail.ClientID %>").val("");
            $("#<%=txtOrganization.ClientID %>").val("");
            $("#<%=txtDesignation.ClientID %>").val("");
            $("#<%=txtCellNumber.ClientID %>").val("");
            $("#<%=txtTelephoneNumber.ClientID %>").val("");

            $("#<%=txtSalesCommission.ClientID %>").val('');
            $("#<%=txtReferenceId.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }

        function PerformClearSearchAction() {
            $("#<%=txtSearchName.ClientID %>").val('');
        }
        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewCatagory').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewCatagory').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }

        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewCatagory').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewCatagory').hide("slow");
        }

        $(function () {
            $("#myTabs").tabs();
        });
        function ConfirmEdit(Name) {
            if (!confirm("Do you want to edit - " + Name + "?")) {
                return false;
            }
        }
    </script>
    <asp:HiddenField ID="txtReferenceId" runat="server"></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Guest Reference </a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Reference</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Guest Reference Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="ReferenceName" class="control-label col-md-2 required-field">
                                Reference Name</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Email" class="control-label col-md-2">
                                Email</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Organization" class="control-label col-md-2">
                                Organization</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtOrganization" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Designation" class="control-label col-md-2">
                                Designation</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="CellNumber" class="control-label col-md-2">
                                Cell Number</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCellNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <label for="TelephoneNumber" class="control-label col-md-2">
                                Telephone Number</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTelephoneNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="SalesCommission" class="control-label col-md-2 required-field">
                                Sales Commission</label>
                            <div class="row">
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtSalesCommission" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                                </div>
                                <div class="col-md-2" style="padding-top: 6px; margin-left: -20px;">
                                    %
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Description" class="control-label col-md-2">
                                Description</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="4"></asp:TextBox>
                            </div>
                        </div>                        
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="4" CssClass="btn btn-primary btn-sm"
                                    OnClientClick="javascript:return CheckValidation();" OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="5" CssClass="btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearAction();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="Search" class="panel panel-default">
                <div class="panel-heading">
                    Search Guest Reference Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="ReferenceName" class="control-label col-md-2">
                                Reference Name</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSearchName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" TabIndex="4" CssClass="btn btn-primary btn-sm"
                                    OnClick="btnSearch_Click" />
                                <asp:Button ID="btnClearSearch" runat="server" Text="Clear" TabIndex="5" CssClass="btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearSearchAction();" />
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
                    <asp:GridView ID="gvGuestReference" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                        ForeColor="#333333" PageSize="30" OnPageIndexChanging="gvGuestReference_PageIndexChanging"
                        OnRowDataBound="gvGuestReference_RowDataBound" OnRowCommand="gvGuestReference_RowCommand"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("ReferenceId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Reference Name" ItemStyle-Width="50%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SalesCommission" HeaderText="S. Commission(%)" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActiveStatus" HeaderText="Status" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" OnClientClick='<%# string.Format("return ConfirmEdit(\"{0}\");", Eval("Name")) %>' CommandName="CmdEdit"
                                        CommandArgument='<%#bind("ReferenceId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%#bind("ReferenceId") %>' ImageUrl="~/Images/delete.png" Text=""
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
            if (parseInt(xNewAdd) == 2) {
                $('#btnNewCatagory').hide();
                $('#EntryPanel').show();
            }
        }
        else {
            NewAddButtonPanelHide();
        }

    </script>
</asp:Content>
