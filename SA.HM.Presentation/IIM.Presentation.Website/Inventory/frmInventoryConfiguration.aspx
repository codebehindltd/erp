<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmInventoryConfiguration.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.frmInventoryConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .mycheckbox input[type="checkbox"] {
            margin-right: 10px;
            padding-top: 5px;
        }
    </style>
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            /*var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Configuration</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);*/

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            $("#ContentPlaceHolder1_InventoryTransactionSetup").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_SupplierAccountsHeadId").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
        });
        $(function () {
            $("#myTabs").tabs();
        });

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
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div id="restuarantConfigPanel" class="panel panel-default" runat="server">
                <div class="panel-heading">
                    Inventory Configuration
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsPurchaseOrderApprovalEnable" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsPurchaseOrderApprovalEnable" TabIndex="4" runat="Server" Text=""
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Purchase Order Approval Enable?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsReceivedProductApprovalEnable" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsReceivedProductApprovalEnable" TabIndex="5" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Received Item Approval Enable?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsItemVarianceApprovalEnable" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsItemVarianceApprovalEnable" TabIndex="6" runat="Server" Text=""
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Item Variance Approval Enable?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsProductAdjustmentApprovalEnable" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsProductAdjustmentApprovalEnable" TabIndex="7" runat="Server" Text=""
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Item Adjustment Approval Enable?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsProductOutApprovalEnable" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsProductOutApprovalEnable" TabIndex="8" runat="Server" Text=""
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Item Out Approval Enable?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsRequisitionCheckedByEnable" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsRequisitionCheckedByEnable" TabIndex="9" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Requisition Checked By Enable?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsPurchaseOrderCheckedByEnable" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsPurchaseOrderCheckedByEnable" TabIndex="10" runat="Server" Text=""
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Purchase Order Checked By Enable?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsInventoryIntegrateWithAccounts" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsInventoryIntegrateWithAccounts" TabIndex="11" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Inventory Integrate With Accounts?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsStockSummaryEnableInStockReport" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsStockSummaryEnableInStockReport" TabIndex="12" runat="Server" Text=""
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Stock Summary Enable In Stock Report?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsInventoryReportItemCostRescitionForNonAdminUsers" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsInventoryReportItemCostRescitionForNonAdminUsers" TabIndex="12" runat="Server" Text=""
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Item Cost Will Hide For Non Admin Users in Stock Report?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsRequisitionApprovalEnable" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsRequisitionApprovalEnable" TabIndex="13" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Requisition Approval Enable?
                                </div>
                            </div>
                        </div>
                         <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsInvCategoryCodeAutoGenerate" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsInvCategoryCodeAutoGenerate" TabIndex="14" runat="Server" Text=""
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Category Code Auto Generate?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsInvItemCodeAutoGenerate" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsInvItemCodeAutoGenerate" TabIndex="14" runat="Server" Text=""
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Item Code Auto Generate?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsSupplierCodeAutoGenerate" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsSupplierCodeAutoGenerate" TabIndex="14" runat="Server" Text=""
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Supplier Code Auto Generate?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsItemOriginHide" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsItemOriginHide" TabIndex="14" runat="Server" Text=""
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Item Origin Hide?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsItemSerialFillWithAutoSearch" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsItemSerialFillWithAutoSearch" TabIndex="15" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Item Serial With Auto Search?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsTransferProductReceiveDisable" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsTransferProductReceiveDisable" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Transfer Item Receive Disable?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsInventoryIntegratationWithAccountsAutomated" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsInventoryIntegratationWithAccountsAutomated" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Inventory Integratation With Accounts Automated?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsSupplierUserPanalEnable" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsSupplierUserPanalEnable" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Supplier User Panel Enable?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsItemDescriptionSuggestInPurchaseOrder" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsItemDescriptionSuggestInPurchaseOrder" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Item Description Suggest In Purchase Order?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsItemAttributeEnable" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsItemAttributeEnable" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Item Attribute Enable?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4">
                                <asp:HiddenField ID="hfSupplierAccountsHeadId" runat="server"></asp:HiddenField>
                                    <asp:Label ID="Label1" runat="server" class="control-label"
                                    Text="Supplier Accounts Head Name"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="SupplierAccountsHeadId" CssClass="form-control" runat="server"></asp:DropDownList>
                            </div>
                        </div>                       
                        <div class="form-group">
                            <div class="col-md-4">
                                <asp:HiddenField ID="hfInventoryTransactionSetup" runat="server"></asp:HiddenField>
                                <asp:Label ID="Label2" runat="server" class="control-label"
                                    Text="Inventory Transaction Setup"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="InventoryTransactionSetup" CssClass="form-control" runat="server" TabIndex="3">
                                    <asp:ListItem Value="0">Same person can create, check and approve</asp:ListItem>
                                    <asp:ListItem Value="1">Creator can't check/ approve but Same person can check and approve both</asp:ListItem>
                                    <asp:ListItem Value="2">Same person can't create, check and approve both</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>                        
                        <div class="row" style="padding-top: 5px;">
                            <div class="col-md-12">
                                <asp:Button ID="btnSaveConfig" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    TabIndex="16" OnClick="btnSaveConfig_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6" style="display: none;">
            <div id="Div1" class="panel panel-default" style="height: 580px;">
                <div class="panel-heading">
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        </div>
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
    </script>
</asp:Content>
