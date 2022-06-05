<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmPOSConfiguration.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmPOSConfiguration" %>

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
            /*var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>POS</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Configuration</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);*/

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
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
        <div class="col-md-6">
            <div id="EmployeeBasicPanel" class="panel panel-default" style="height: 238px; display: none">
                <div class="panel-heading">
                    Service Bill Configuration
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-4">
                                <asp:HiddenField ID="txtGuestServiceVatId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblGuestServiceVat" runat="server" class="control-label" Text="Service Vat"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtGuestServiceVat" TabIndex="5" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4">
                                <asp:HiddenField ID="txtGuestServiceServiceChargeId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblGuestServiceServiceCharge" runat="server" class="control-label"
                                    Text="Service Charge"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtGuestServiceServiceCharge" TabIndex="6" CssClass="form-control"
                                    runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="chkInclusiveGuestServiceBillId" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="chkInclusiveGuestServiceBill" TabIndex="7" runat="Server" Text=""
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Inclusive?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfChkIsRackRatCalculation" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="ChkIsRackRatCalculation" TabIndex="7" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Rack Rate Calculation?
                                </div>
                            </div>
                        </div>
                        <div class="row" style="padding-top: 5px;">
                            <div class="col-md-12">
                                <asp:Button ID="btnServiceBillCon" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    TabIndex="8" OnClick="btnServiceBillCon_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="restuarantConfigPanel" class="panel panel-default" runat="server">
                <div class="panel-heading">
                    Restuarant Configuration
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-8">
                                <asp:HiddenField ID="hfRestaurantComplementaryFoodCost" runat="server"></asp:HiddenField>
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Restaurant Complementary Food Cost"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="RestaurantComplementaryFoodCost" TabIndex="1" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-8">
                                <asp:HiddenField ID="hfHowMuchMoneySpendToGetOnePoint" runat="server"></asp:HiddenField>
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="How much money spend to get one point?"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="HowMuchMoneySpendToGetOnePoint" TabIndex="2" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-8">
                                <asp:HiddenField ID="hfHowMuchPointNeedToGetOneMoney" runat="server"></asp:HiddenField>
                                <asp:Label ID="Label5" runat="server" class="control-label" Text="How Much Point Need To Get One Money?"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="HowMuchPointNeedToGetOneMoney" TabIndex="2" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-6">
                                <asp:HiddenField ID="hfPOSRefundConfiguration" runat="server"></asp:HiddenField>
                                <asp:Label ID="Label6" runat="server" class="control-label"
                                    Text="POS Refund Configuration"></asp:Label>
                            </div>
                            <div class="col-md-6">
                                <asp:DropDownList ID="POSRefundConfiguration" CssClass="form-control" runat="server" TabIndex="3">
                                    <asp:ListItem Value="1">Full Amount Refund</asp:ListItem>
                                    <asp:ListItem Value="2">Pertial Amount Refund</asp:ListItem>
                                    <asp:ListItem Value="3">No Amount Refund</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-6">
                                <asp:HiddenField ID="hfRestaurantBillPrintAndPreview" runat="server"></asp:HiddenField>
                                <asp:Label ID="Label2" runat="server" class="control-label"
                                    Text="Restaurant Bill Print And Preview"></asp:Label>
                            </div>
                            <div class="col-md-6">
                                <asp:DropDownList ID="RestaurantBillPrintAndPreview" CssClass="form-control" runat="server" TabIndex="3">
                                    <asp:ListItem Value="0">No Print & Preview </asp:ListItem>
                                    <asp:ListItem Value="1">Bill Preview Only</asp:ListItem>
                                    <asp:ListItem Value="2">Direct Print</asp:ListItem>
                                    <asp:ListItem Value="3">Print & Preview</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-6">
                                <asp:HiddenField ID="hfKotPrintAndPreview" runat="server"></asp:HiddenField>
                                <asp:Label ID="Label4" runat="server" class="control-label"
                                    Text="Kot Print & Preview"></asp:Label>
                            </div>
                            <div class="col-md-6">
                                <asp:DropDownList ID="KotPrintAndPreview" CssClass="form-control" runat="server" TabIndex="3">
                                    <asp:ListItem Value="0">Kot Not Print</asp:ListItem>
                                    <asp:ListItem Value="1">Kot Print</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsRestaurantKotContinueWithDiferentWaiter" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsRestaurantKotContinueWithDiferentWaiter" TabIndex="4" runat="Server" Text=""
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Restaurant Kot Continue With Diferent Waiter?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsRestaurantBillAmountWillRound" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsRestaurantBillAmountWillRound" TabIndex="5" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Restaurant Bill Amount Will Round?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsRestaurantBillClassificationWiseDevideForFOInvoice" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsRestaurantBillClassificationWiseDevideForFOInvoice" TabIndex="6" runat="Server" Text=""
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Restaurant Bill Classification Wise Devide For FOInvoice?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsRestaurantPaxConfirmationEnable" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsRestaurantPaxConfirmationEnable" TabIndex="7" runat="Server" Text=""
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Restaurant Pax Confirmation Enable?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsRestaurantWaiterConfirmationEnable" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsRestaurantWaiterConfirmationEnable" TabIndex="8" runat="Server" Text=""
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Restaurant Waiter Confirmation Enable?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsRestaurantItemImageEnable" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsRestaurantItemImageEnable" TabIndex="9" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Restaurant Item Image Enable?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsWaterMarkImageDisplayEnableInRestaurant" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsWaterMarkImageDisplayEnableInRestaurant" TabIndex="10" runat="Server" Text=""
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Water Mark Image Display Enable In Restaurant?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsCompanyNameShowOnRestaurantInvoice" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsCompanyNameShowOnRestaurantInvoice" TabIndex="11" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Company Name Show On Restaurant Invoice?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsRestaurantIntegrateWithGuestCompany" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsRestaurantIntegrateWithGuestCompany" TabIndex="12" runat="Server" Text=""
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Restaurant Integrate With Guest Company?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsRestaurantIntegrateWithMembership" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsRestaurantIntegrateWithMembership" TabIndex="13" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Restaurant Integrate With Membership?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsRestaurantIntegrateWithFrontOffice" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsRestaurantIntegrateWithFrontOffice" TabIndex="14" runat="Server" Text=""
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Restaurant Integrate With Front Office?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsRestaurantIntegrateWithPayroll" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsRestaurantIntegrateWithPayroll" TabIndex="15" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Restaurant Integrate With Payroll?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsMembershipPaymentEnable" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsMembershipPaymentEnable" TabIndex="16" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Membership Payment Enable?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsRestaurantBillContinueWithoutKotSubmit" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsRestaurantBillContinueWithoutKotSubmit" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Restaurant Bill Continue Without Kot Submit?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsRestaurantOrderSubmitDisable" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsRestaurantOrderSubmitDisable" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Restaurant Order Submit Disable?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsRestaurantTokenInfoDisable" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsRestaurantTokenInfoDisable" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Restaurant Token Info Disable?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsRecipeIncludedInInventory" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsRecipeIncludedInInventory" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Recipe Included In Inventory?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsItemNameDisplayInRestaurantOrderScreen" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsItemNameDisplayInRestaurantOrderScreen" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Item Name Display In Restaurant Order Screen?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsItemNameAutoSearchWithCode" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsItemNameAutoSearchWithCode" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Item Name Auto Search With Code?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsGuestNameAndRoomNoTextShowInInvoice" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsGuestNameAndRoomNoTextShowInInvoice" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Guest Name and Room No. Text Show In Invoice?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsPredefinedRemarksEnable" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsPredefinedRemarksEnable" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Predefined Remarks Enable?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsPOSIntegrateWithAccounts" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsPOSIntegrateWithAccounts" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is POS Integrate With Accounts?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsFoodNBeverageSalesRelatedDataHide" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsFoodNBeverageSalesRelatedDataHide" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Food & Beverage Sales Related Data Hide?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsRestaurantReportRestrictionForAllUser" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsRestaurantReportRestrictionForAllUser" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Report Restriction For All User?
                                </div>
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
        <div class="col-md-6">

            <div id="billingConfigPanel" class="panel panel-default" runat="server">
                <div class="panel-heading">
                    Billing Configuration
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsItemCodeHideForBilling" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsItemCodeHideForBilling" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Item Code Hide?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsStockHideForBilling" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsStockHideForBilling" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Item Stock Hide?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsStockByHideForBilling" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsStockByHideForBilling" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Item Stock By Hide?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsRemarksHideForBilling" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsRemarksHideForBilling" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Item Remarks Hide?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsItemAutoSave" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsItemAutoSave" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Item Auto Save?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsTaskAutoGenarate" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsTaskAutoGenarate" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Task Auto Genarate?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsCashPaymentShow" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsCashPaymentShow" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Cash Payment Show?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsAmexCardPaymentShow" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsAmexCardPaymentShow" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Amex Card Payment Show?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsMasterCardPaymentShow" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsMasterCardPaymentShow" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Master Card Payment Show?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsVisaCardPaymentShow" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsVisaCardPaymentShow" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Visa Card Payment Show?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsDiscoverCardPaymentShow" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsDiscoverCardPaymentShow" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Discover Card Payment Show?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsCompanyPaymentShow" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsCompanyPaymentShow" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Company Payment Show?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsBillingInvoiceTemplateWithoutHeader" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsBillingInvoiceTemplateWithoutHeader" TabIndex="7" runat="Server" Text=""
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Billing Invoice Without Header?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsSubjectShow" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsSubjectShow" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Subject Show?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsRemarkShow" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsRemarkShow" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Remarks Show?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsAutoCompanyBillGenerationProcessEnable" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsAutoCompanyBillGenerationProcessEnable" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Auto Company Bill Generation Process Enable?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsSDCIntegrationEnable" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsSDCIntegrationEnable" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is SDC Integration Enable?
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Label ID="Label7" runat="server" class="control-label" Text="Remarks Detail:"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:HiddenField ID="hfRemarksDetails" runat="server"></asp:HiddenField>
                                <asp:TextBox ID="txtRemarksDetails" Width="100%" TabIndex="2" TextMode="MultiLine" CssClass="form-control"
                                    runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Label ID="Label8" runat="server" class="control-label" Text="Support Remarks Detail:"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:HiddenField ID="hfSTRemarksDetails" runat="server"></asp:HiddenField>
                                <asp:TextBox ID="txtSTRemarksDetails" Width="100%" TabIndex="2" TextMode="MultiLine" CssClass="form-control"
                                    runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Label ID="lblBillDeclaration" runat="server" class="control-label" Text="Bill Declaration:"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:HiddenField ID="hfBillDeclaration" runat="server"></asp:HiddenField>
                                <asp:TextBox ID="txtBillDeclaration" Width="100%" TabIndex="2" TextMode="MultiLine" CssClass="form-control"
                                    runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row" style="padding-top: 5px;">
                            <div class="col-md-12">
                                <asp:Button ID="btnSaveConfigForBilling" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    TabIndex="16" OnClick="btnSaveConfigForBilling_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div id="Div1" class="panel panel-default" style="height: 580px;">
                <div class="panel-heading">
                    Restuarant Default Discount
                </div>
                <div class="panel-body">
                    <asp:Panel ID="pnlPercentageDiscountCategoryGrid" runat="server" Height="400px" ScrollBars="Both">
                        <asp:GridView ID="gvPercentageDiscountCategory" runat="server" AllowPaging="True"
                            AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="30" ForeColor="#333333"
                            OnRowDataBound="gvPercentageDiscountCategory_RowDataBound" CssClass="table table-bordered table-condensed table-responsive">
                            <RowStyle BackColor="#E3EAEB" />
                            <Columns>
                                <asp:TemplateField HeaderText="IDNO" HeaderStyle-CssClass="invisible" ItemStyle-CssClass="invisible" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblid" runat="server" Text='<%#Eval("CategoryId") %>'></asp:Label>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("ActiveStat") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Checked" ItemStyle-Width="05%">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="ChkChecked" CssClass="ChkCreate" runat="server" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsCheckedPermission" CssClass="chk_View" runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name" HeaderText="Classification" ItemStyle-Width="40%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CategoryId" HeaderStyle-CssClass="invisible" ItemStyle-CssClass="invisible" Visible="False"></asp:BoundField>
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
                    </asp:Panel>
                    <div class="row" style="padding-top: 10px;">
                        <div class="col-md-12">
                            <asp:Button ID="btnPercentageDiscountCategory" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                TabIndex="8" OnClick="btnPercentageDiscountCategory_Click" />
                        </div>
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
