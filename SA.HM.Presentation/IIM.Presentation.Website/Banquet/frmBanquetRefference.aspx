﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmBanquetRefference.aspx.cs" Inherits="HotelManagement.Presentation.Website.Banquet.frmBanquetRefference" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Banquet Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Banquet Reference</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            CommonHelper.ApplyDecimalValidation();
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#<%=txtSalesCommission.ClientID %>").blur(function () {                
                var checkValue = CheckDiscountValidationForFixedRPercentage($("#ContentPlaceHolder1_hfSalesCommissionType"), $("#ContentPlaceHolder1_txtSalesCommission"), $("#ContentPlaceHolder1_txtSalesCommission"), "Sales Commission");
                if (checkValue == false) {
                    return false; 
                }
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
            $("#<%=txtSalesCommission.ClientID %>").val(result.DiscountAmount);
            $("#<%=txtRefferenceId.ClientID %>").val(result.RefferenceId);
            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewCatagory').hide("slow");
            $('#EntryPanel').show("slow");
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
            window.location = "frmBanquetRefference.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=txtDescription.ClientID %>").val('');
            $("#<%=txtName.ClientID %>").val("");
            $("#<%=txtSalesCommission.ClientID %>").val('');
            $("#<%=txtRefferenceId.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }

        function PerformClearSearchAction() {
            $("#<%=txtSearchName.ClientID %>").val('');
            return false;
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

    </script>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Reference Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Reference</a></li>
            <%--<li id="C" runat="server"><a href="#tab-3">Title 3</a></li>--%>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Banquet Reference Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="txtRefferenceId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hfSalesCommissionType" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblName" runat="server" class="control-label required-field" Text="Reference Name"></asp:Label>                               
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSalesCommission" runat="server" class="control-label required-field" Text="Sales Commission"></asp:Label>                                
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSalesCommission" runat="server" CssClass="form-control quantitydecimal" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-4" style="padding-left:0">
                            %
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDescription" runat="server" class="control-label" Text="Description"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="4" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="5" CssClass="TransactionalButton btn btn-primary btn-sm"
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
                    Search Banquet Reference Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSearchName" runat="server" class="control-label" Text="Reference Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSearchName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" TabIndex="4" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSearch_Click" />
                                <asp:Button ID="btnClearSearch" runat="server" Text="Clear" TabIndex="5" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearSearchAction();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information</div>
                <div class="panel-body">
                    <asp:GridView ID="gvBanquetRefference" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                        ForeColor="#333333" PageSize="30" OnPageIndexChanging="gvBanquetRefference_PageIndexChanging"
                        OnRowDataBound="gvBanquetRefference_RowDataBound" OnRowCommand="gvBanquetRefference_RowCommand"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("Id") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Refference Name" ItemStyle-Width="35%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="40%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="25%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("Id") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("Id") %>' ImageUrl="~/Images/delete.png"
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
