<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmInvManufacturer.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesManagment.frmInvManufacturer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Manufacturer Or Brand</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

        });

        $(document).ready(function () {
            //alert("Are U Ok TransactionalButton btn btn-primary TransactionalButton btn btn-primary TransactionalButton btn btn-primary TransactionalButton btn btn-primary TransactionalButton btn btn-primary", "Confirmation", "OK");
            var txtName = '<%=txtName.ClientID%>'
            var lblMessage = '<%=lblMessage.ClientID%>'


        });


        $(function () {
            $("#myTabs").tabs();
        });
        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {

            if (result.StatusId != 2) {

                $("#<%=txtManufacturerId.ClientID %>").val(result.ManufacturerId);
                $("#<%=txtName.ClientID %>").val(result.Name);
                $("#<%=txtCode.ClientID %>").val(result.Code);

                $("#<%=txtRemarks.ClientID %>").val(result.Remarks);
                $("#<%=btnSave.ClientID %>").val("Update");
                $('#btnNewManufacturer').hide("slow");
                $('#EntryPanel').show("slow");
            }
            else {
                alert("This is not edditable.");
            }
        }


        function OnFillFormObjectFailed(error) {
            alert(error.get_message());
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
            window.location = "frmPMManufacturer.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            alert(error.get_message());
        }
        function PerformClearActionOnButtonClick() {
            if (!confirm("Do you want to clear?")) {
                return false;
            }
            PerformClearAction();
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=lblMessage.ClientID %>").text('');
            $("#<%=txtName.ClientID %>").val('');
            $("#<%=txtCode.ClientID %>").val('');
            $("#<%=txtManufacturerId.ClientID %>").val('');
            $("#<%=txtRemarks.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");
            MessagePanelHide();
            return false;
        }

        function PerformSearchClearAction() {
            $("#<%=lblMessage.ClientID %>").text('');
            $("#<%=txtSearchName.ClientID %>").val('');
            $("#<%=txtSearchCode.ClientID %>").val('');
            MessagePanelHide();
            return false;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewManufacturer').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewManufacturer').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }
        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }
        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewManufacturer').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewManufacturer').hide("slow");
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
                href="#tab-1">Manufacturer Info</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Manufacturer </a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Manufacturer Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:HiddenField ID="txtManufacturerId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblName" runat="server" class="control-label required-field" Text="Name"></asp:Label>                                
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblCode" runat="server" class="control-label required-field" Text="Code"></asp:Label>                                
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <%--Right Left--%>
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary"
                                    OnClick="btnSave_Click" TabIndex="4" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-primary"
                                    OnClientClick="javascript: return PerformClearActionOnButtonClick();" TabIndex="5" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="InfoPanel" class="panel panel-default">
                <div class="panel-heading">
                    Manufacturer Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSearchName" runat="server" class="control-label" Text="Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="lblSearchCode" runat="server" class="control-label" Text="Code"></asp:Label>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:TextBox ID="txtSearchCode" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <%--Right Left--%>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                    CssClass="btn btn-primary" TabIndex="4" />
                                <asp:Button ID="btnSearchClear" runat="server" Text="Cancel" CssClass="btn btn-primary"
                                    OnClientClick="javascript: return PerformSearchClearAction();" TabIndex="9" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information</div>
                <div class="panel-body">
                    <asp:GridView ID="gvManufacturer" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                        ForeColor="#333333" PageSize="30" OnPageIndexChanging="gvManufacturer_PageIndexChanging"
                        OnRowDataBound="gvManufacturer_RowDataBound" OnRowCommand="gvManufacturer_RowCommand"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("ManufacturerId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Manufacturer Name" ItemStyle-Width="35%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Code" HeaderText="Manufacturer Code" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" ItemStyle-Width="30%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" OnClientClick="return confirm('Do you want to edit?');" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("ManufacturerId") %>' ImageUrl="~/Images/edit.png"
                                        Text="" AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("ManufacturerId") %>' ImageUrl="~/Images/delete.png"
                                        Text="" AlternateText="Delete" ToolTip="Delete" OnClientClick="return confirm('Do you want to delete?');" />
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

        var xNewAdd = '<%=isNewAddButtonEnable%>';
        if (xNewAdd > -1) {
            NewAddButtonPanelShow();
        }
        else {
            NewAddButtonPanelHide();
        }
    </script>
</asp:Content>
