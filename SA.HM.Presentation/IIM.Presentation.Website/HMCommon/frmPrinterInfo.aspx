<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmPrinterInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.frmPrinterInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Company Information</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Printer Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            var ddlStockType = '<%=ddlStockType.ClientID%>'
            $('#' + ddlStockType).change(function () {
                if ($('#' + ddlStockType).val() == "KitchenItem") {
                    $('#KitchenInformationDiv').show("slow");
                }
                else {
                    $('#KitchenInformationDiv').hide("slow");
                }
            });
        });

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {
            $("#<%=txtPrinterName.ClientID %>").val(result.PrinterName);
            $("#<%=ddlCostCenterId.ClientID %>").val(result.CostCenterId);
            $("#<%=ddlActiveStat.ClientID %>").val(result.ActiveStat == true ? 0 : 1);
            $("#<%=txtPrinterInfoId.ClientID %>").val(result.PrinterInfoId);
            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewType').hide("slow");
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
            window.location = "frmRestaurantItemType.aspx?DeleteConfirmation=Deleted"
        }
        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=ddlCostCenterId.ClientID %>").val(0);
            $("#<%=txtPrinterName.ClientID %>").val('');
            $("#<%=ddlActiveStat.ClientID %>").val(0);
            $("#<%=txtPrinterInfoId.ClientID %>").val('');
            $("#<%=ddlStockType.ClientID %>").val('StockItem');
            $('#KitchenInformationDiv').hide("slow");
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewType').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewType').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }

        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewType').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewType').hide("slow");
        }
        $(function () {
            $("#myTabs").tabs();
        });
    </script>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Printer Info Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Printer Info</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">               
                <div class="panel-heading">Printer Information</div>
                <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblCostCenterId" runat="server" class="control-label required-field" Text="Cost Center"></asp:Label>                           
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlCostCenterId" runat="server" CssClass="form-control"
                                TabIndex="5">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblPrinterName" runat="server" class="control-label required-field" Text="Printer Name"></asp:Label>                           
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtPrinterName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblStockType" runat="server" class="control-label required-field" Text="Printer Type"></asp:Label>                           
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlStockType" runat="server" CssClass="form-control"
                                TabIndex="4">
                                <asp:ListItem Value="StockItem">Stock Printer</asp:ListItem>
                                <asp:ListItem Value="KitchenItem">Kitchen Printer</asp:ListItem>
                                <asp:ListItem Value="InvoiceItem">Invoice Printer</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblActiveStat" runat="server" class="control-label required-field" Text="Status"></asp:Label>                           
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="form-control"
                                TabIndex="3">
                                <asp:ListItem Value="0">Active</asp:ListItem>
                                <asp:ListItem Value="1">Inactive</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group" id="KitchenInformationDiv" style="display: none;">
                        <div class="col-md-2">
                            <asp:Label ID="lblKitchen" runat="server" class="control-label required-field" Text="Kitchen Name"></asp:Label>                            
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlKitchen" runat="server" CssClass="form-control"
                                TabIndex="4">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnSave" runat="server" TabIndex="4" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
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
            <div id="SearchEntry" class="panel panel-default">                
                <div class="panel-heading">Printer Information</div>
                <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:HiddenField ID="txtPrinterInfoId" runat="server"></asp:HiddenField>
                            <asp:Label ID="lblSrcCostCenterId" runat="server" class="control-label" Text="Cost Center"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlSrcCostCenterId" runat="server" CssClass="form-control"
                                TabIndex="5">
                            </asp:DropDownList>
                        </div>
                    </div>                    
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblSPrinterName" runat="server" class="control-label" Text="Printer Name"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtSPrinterName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>                    
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblSActineStat" runat="server" class="control-label" Text="Status"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlSActiveStat" runat="server" CssClass="form-control"
                                TabIndex="3">
                                <asp:ListItem Value="0">Active</asp:ListItem>
                                <asp:ListItem Value="1">Inactive</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>                    
                    <div class="row">
                        <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" TabIndex="4" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClick="btnSearch_Click" />
                    </div>
                    </div>
                </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">                
                <div class="panel-heading">Search Information</div>
                <div class="panel-body">
                    <asp:GridView ID="gvRestaurentTypeService" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True"
                        ForeColor="#333333" OnPageIndexChanging="gvRestaurentTypeService_PageIndexChanging"
                        OnRowDataBound="gvRestaurentTypeService_RowDataBound" OnRowCommand="gvRestaurentTypeService_RowCommand"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB"/>
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("PrinterInfoId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CostCenter" HeaderText="Cost Center" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PrinterName" HeaderText="Printer Name" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActiveStat" HeaderText="Status" ItemStyle-Width="30%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("PrinterInfoId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("PrinterInfoId") %>' ImageUrl="~/Images/delete.png"
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
                $('#btnNewType').hide();
                $('#EntryPanel').show();
            }
        }
        else {
            NewAddButtonPanelHide();
        }

        var ddlStockType = '<%=ddlStockType.ClientID%>'
        if ($('#' + ddlStockType).val() == "KitchenItem") {
            $('#KitchenInformationDiv').show("slow");
        }
        else {
            $('#KitchenInformationDiv').hide("slow");
        }
    </script>
</asp:Content>
