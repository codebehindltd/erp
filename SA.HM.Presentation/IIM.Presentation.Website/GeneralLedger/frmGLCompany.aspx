<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmGLCompany.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.frmGLCompany" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Accounts</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Company Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_ddlInterCompanyName").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

        });


        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {
            $("#<%=txtName.ClientID %>").val(result.Name);
            $("#<%=txtShortName.ClientID %>").val(result.ShortName);
            $("#<%=txtCode.ClientID %>").val(result.Code);
            $("#<%=txtDescription.ClientID %>").val(result.Description);
            $("#<%=txtCompanyId.ClientID %>").val(result.CompanyId);
            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewCompany').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }

        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
        }
        //For Delete-------------------------        
        function PerformDeleteAction(actionId) {

            $.confirm({
                title: 'Confirm!',
                content: 'Do you want to delete this record?',
                buttons: {
                    confirm: function () {
                        PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
                    },
                    cancel: function () {
                    }
                }
            });

            return false;
        }

        function OnDeleteObjectSucceeded(result) {
            window.location = "frmGLCompany.aspx?DeleteConfirmation=Deleted"
        }
        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=txtName.ClientID %>").val('');
            $("#<%=txtShortName.ClientID %>").val('');
            $("#<%=txtCode.ClientID %>").val('');
            $("#<%=txtDescription.ClientID %>").val('');
            $("#<%=txtCompanyId.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewCompany').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewCompany').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }

        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewCompany').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewCompany').hide("slow");
        }
        $(function () {
            $("#myTabs").tabs();
        });
    </script>

    <asp:HiddenField ID="hfValidationFlag" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsInterCompanyExist" runat="server"></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Company Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Company </a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Company Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="txtCompanyId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblName" runat="server" class="control-label required-field" Text="Company Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCode" runat="server" class="control-label required-field" Text="Company Code"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCode" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblShortName" runat="server" class="control-label" Text="Short Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtShortName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Company Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtCompanyAddress" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="Web Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtWebAddress" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" id="InterCompanyNameDiv">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Inter Company Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlInterCompanyName" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblBinNumber" runat="server" class="control-label" Text="BIN Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtBinNumber" runat="server" CssClass="form-control" TabIndex="36"
                                    MaxLength="30"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblTinNumber" runat="server" class="control-label" Text="TIN Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTinNumber" runat="server" CssClass="form-control" TabIndex="36"
                                    MaxLength="20"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDescription" runat="server" class="control-label" Text="Description"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label" Text="Budget Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlBudgetType" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
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
                    Company Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSName" runat="server" class="control-label required-field" Text="Company Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSCode" runat="server" class="control-label required-field" Text="Company Code"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSCode" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSShortName" runat="server" class="control-label" Text="Short Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSShortName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSearch_Click" />
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
                    <asp:GridView ID="gvGLCompany" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True" ForeColor="#333333"
                        OnPageIndexChanging="gvGLCompany_PageIndexChanging" OnRowDataBound="gvGLCompany_RowDataBound"
                        OnRowCommand="gvGLCompany_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("CompanyId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Company Name" ItemStyle-Width="50%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Code" HeaderText="Company Code" ItemStyle-Width="30%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("CompanyId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("CompanyId") %>' ImageUrl="~/Images/delete.png" Text=""
                                        AlternateText="Delete" ToolTip="Delete" OnClientClick="javascript:return CheckConfirmation(this);" />
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
        $(document).ready(function () {
            var single = $("#ContentPlaceHolder1_hfIsInterCompanyExist").val();
            if (single == "0") {
                $('#InterCompanyNameDiv').hide();

            }
            else {
                $('#InterCompanyNameDiv').show();
            }

            $('#InterCompanyNameDiv').show();
        });

        var xNewAdd = '<%=isNewAddButtonEnable%>';
        if (xNewAdd > -1) {
            NewAddButtonPanelShow();
        }
        else {
            NewAddButtonPanelHide();
        }

        function CheckConfirmation(id) {

            if ($("#ContentPlaceHolder1_hfValidationFlag").val() == "1") { $("#ContentPlaceHolder1_hfValidationFlag").val("0"); return true; }
            var gridCommandId = $(id).attr('id');

            $.confirm({
                title: 'Confirm!',
                content: 'Do you want to Delete?',
                buttons: {
                    confirm: function () {

                        $("#ContentPlaceHolder1_hfValidationFlag").val("1");
                        $("#" + gridCommandId).trigger("click");

                        return true;
                    },
                    cancel: function () {
                        $("#hfValidationFlag").val("0");
                    }
                }
            });

            return false;
        }

    </script>
</asp:Content>
