<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmCostCentre.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.frmCostCentre" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Company Information</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Cost Center</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            CommonHelper.ApplyDecimalValidation();

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $('#ContentPlaceHolder1_gvCategoryCostCenterInfo_ChkCreate').click(function () {
                if ($('#ContentPlaceHolder1_gvCategoryCostCenterInfo_ChkCreate').is(':checked')) {
                    CheckAllCheckBoxCreate()
                }
                else {
                    UnCheckAllCheckBoxCreate();
                }
            });

            var ddlCostCenterType = '<%=ddlCostCenterType.ClientID%>'
            var lblDefaultView = '<%=lblDefaultView.ClientID%>'
            var ddlDefaultView = '<%=ddlDefaultView.ClientID%>'
            var lblIsDefaultCostCenter = '<%=lblIsDefaultCostCenter.ClientID%>'
            var ddlIsDefaultCostCenter = '<%=ddlIsDefaultCostCenter.ClientID%>'
            var lblBillingTime = '<%=lblBillingTime.ClientID%>'
            var ddlBillingTime = '<%=ddlBillingTime.ClientID%>'
            var lblInvoiceTemplate = '<%=lblInvoiceTemplate.ClientID%>'
            var ddlInvoiceTemplate = '<%=ddlInvoiceTemplate.ClientID%>'

            if ($('#' + ddlCostCenterType).val() == "Inventory") {
                $('#' + lblBillingTime).hide('Slow');
                $('#' + ddlBillingTime).hide('Slow');
                $('#' + lblInvoiceTemplate).hide('Slow');
                $('#' + ddlInvoiceTemplate).hide('Slow');
            }
            else {
                $('#' + lblBillingTime).show('Slow');
                $('#' + ddlBillingTime).show('Slow');
                $('#' + lblInvoiceTemplate).show('Slow');
                $('#' + ddlInvoiceTemplate).show('Slow');
            }

            if ($('#ContentPlaceHolder1_ddlCostCenterType').val() == "Restaurant" || $('#ContentPlaceHolder1_ddlCostCenterType').val() == "Billing") {
                debugger;
                $('#IsEnableItemAutoDeductFromStoreDiv').show("Slow");
            }
            else {
                $('#IsEnableItemAutoDeductFromStoreDiv').hide("Slow");
            }

            if ($('#' + ddlCostCenterType).val() != "Restaurant") {
                if ($('#' + ddlCostCenterType).val() == "OtherOutlet") {
                    $('#' + lblDefaultView).show("Slow");
                    $('#' + lblIsDefaultCostCenter).show("Slow");
                    $('#' + ddlIsDefaultCostCenter).show("Slow");
                    $('#' + ddlDefaultView).hide("Slow");
                    $("#DdlDefaultViewTable").hide();
                    $('#' + ddlDefaultView).show("Slow");
                }
                else {
                    $('#' + lblDefaultView).hide("Slow");
                    $('#' + ddlDefaultView).hide("Slow");
                    $('#' + lblIsDefaultCostCenter).hide("Slow");
                    $('#' + ddlIsDefaultCostCenter).hide("Slow");
                    if ($('#' + ddlCostCenterType).val() == "FrontOffice") {
                        $("#<%=lblGuestServiceSDCharge.ClientID %>").text("City Charge");
                    }
                }
                $('#IsDiscountEnableDiv').hide("Slow");
            }
            else {
                $('#IsDiscountEnableDiv').show("Slow");
                $('#' + ddlDefaultView).hide("Slow");
                $("#DdlDefaultViewTable").show();
                $('#' + lblDefaultView).show("Slow");
                $('#' + ddlDefaultView).show("Slow");
                $('#' + lblIsDefaultCostCenter).show("Slow");
                $('#' + ddlIsDefaultCostCenter).show("Slow");
            }

            $('#' + ddlCostCenterType).change(function () {
                if ($('#ContentPlaceHolder1_ddlCostCenterType').val() == "Restaurant" || $('#ContentPlaceHolder1_ddlCostCenterType').val() == "Billing") {
                    debugger;
                    $('#IsEnableItemAutoDeductFromStoreDiv').show("Slow");
                }
                else {
                    $('#IsEnableItemAutoDeductFromStoreDiv').hide("Slow");
                }
                $("#<%=lblGuestServiceSDCharge.ClientID %>").text("SD Charge");
                if ($('#' + ddlCostCenterType).val() != "Restaurant") {
                    if ($('#' + ddlCostCenterType).val() == "OtherOutlet") {
                        $('#' + lblDefaultView).show("Slow");
                        $('#' + lblIsDefaultCostCenter).show("Slow");
                        $('#' + ddlIsDefaultCostCenter).show("Slow");
                        $('#' + ddlDefaultView).hide("Slow");
                        $("#DdlDefaultViewTable").hide();
                        $('#' + ddlDefaultView).show("Slow");
                        $('#' + ddlDefaultView).val("Token");
                    }
                    else {
                        $('#' + lblDefaultView).hide("Slow");
                        $('#' + ddlDefaultView).hide("Slow");
                        $('#' + lblIsDefaultCostCenter).hide("Slow");
                        $('#' + ddlIsDefaultCostCenter).hide("Slow");
                        $('#' + ddlDefaultView).val("Token");
                        if ($('#' + ddlCostCenterType).val() == "FrontOffice") {
                            $("#<%=lblGuestServiceSDCharge.ClientID %>").text("City Charge");
                        }
                    }
                    $('#IsDiscountEnableDiv').hide("Slow");
                }
                else {
                    $('#IsDiscountEnableDiv').show("Slow");
                    $('#' + ddlDefaultView).hide("Slow");
                    $("#DdlDefaultViewTable").show();
                    $('#' + lblDefaultView).show("Slow");
                    $('#' + ddlDefaultView).show("Slow");
                    $('#' + lblIsDefaultCostCenter).show("Slow");
                    $('#' + ddlIsDefaultCostCenter).show("Slow");
                    $('#' + ddlDefaultView).val("Token");
                }
                if ($(this).val() == "Inventory") {
                    $('#' + lblBillingTime).hide('Slow');
                    $('#' + ddlBillingTime).hide('Slow');
                    $('#' + lblInvoiceTemplate).hide('Slow');
                    $('#' + ddlInvoiceTemplate).hide('Slow');
                }
                else {
                    $('#' + lblBillingTime).show('Slow');
                    $('#' + ddlBillingTime).show('Slow');
                    $('#' + lblInvoiceTemplate).show('Slow');
                    $('#' + ddlInvoiceTemplate).show('Slow');
                }
            });
        });

        function CheckAllCheckBoxCreate() {
            $('.Chk_Create input[type=checkbox]').each(function () {
                $(this).prop("checked", true);
            });
        }

        function UnCheckAllCheckBoxCreate() {
            $('.Chk_Create input[type=checkbox]').each(function () {
                $(this).prop("checked", false);
            });
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
            window.location = "frmCostCentre.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            alert(error.get_message());
        }

        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=ddlCompanyId.ClientID %>").val(0);
            $("#<%=txtCostCentre.ClientID %>").val('');
            $("#<%=hfCostCentreId.ClientID %>").val('');
            $("#<%=ddlCostCenterType.ClientID %>").val(0);
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewCostCenter').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }

        function EntryPanelVisibleFalse() {
            $('#btnNewCostCenter').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }

        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewCostCenter').show("slow");
        }

        function NewAddButtonPanelHide() {
            $('#btnNewCostCenter').hide("slow");
        }

        $(function () {
            $("#myTabs").tabs();
        });

        function ToggleChargeEnableDisable(ctrl) {
            if ($('#ContentPlaceHolder1_cbServiceCharge').is(':checked')) {
                $('#ContentPlaceHolder1_txtServiceCharge').attr("disabled", false);
            }
            else {
                $('#ContentPlaceHolder1_txtServiceCharge').val("0");
                $('#ContentPlaceHolder1_txtServiceCharge').attr("disabled", true);
            }

            if ($('#ContentPlaceHolder1_cbSDCharge').is(':checked')) {
                $('#ContentPlaceHolder1_txtSDCharge').attr("disabled", false);
            }
            else {
                $('#ContentPlaceHolder1_txtSDCharge').val("0");
                $('#ContentPlaceHolder1_txtSDCharge').attr("disabled", true);
            }

            if ($('#ContentPlaceHolder1_cbVatAmount').is(':checked')) {
                $('#ContentPlaceHolder1_txtVatAmount').attr("disabled", false);
            }
            else {
                $('#ContentPlaceHolder1_txtVatAmount').val("0");
                $('#ContentPlaceHolder1_txtVatAmount').attr("disabled", true);
            }

            if ($('#ContentPlaceHolder1_cbAdditionalCharge').is(':checked')) {
                $('#ContentPlaceHolder1_txtAdditionalChargeAmount').attr("disabled", false);
            }
            else {
                $('#ContentPlaceHolder1_txtAdditionalChargeAmount').val("0");
                $('#ContentPlaceHolder1_txtAdditionalChargeAmount').attr("disabled", true);
            }
        }
    </script>
    <asp:HiddenField ID="hfEditedId" runat="server" Value="0"></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Cost Center </a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Cost Center </a></li>
        </ul>
        <div id="tab-1">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Cost Center Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="hfCostCentreId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblRoomTypeId" runat="server" class="control-label required-field" Text="Company"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCompanyId" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:HiddenField ID="hfIsSingleGLCompany" runat="server"></asp:HiddenField>
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Accounts Company"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlGLCompanyId" runat="server" CssClass="form-control"
                                    TabIndex="7">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCostCentre" runat="server" class="control-label required-field" Text="Cost Center"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtCostCentre" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblIsRestaurant" runat="server" class="control-label required-field" Text="Cost Center Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:HiddenField ID="hfCostCenterTypeInformation" runat="server" />
                                <asp:DropDownList ID="ddlCostCenterType" runat="server" CssClass="form-control" TabIndex="7">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblServiceCharge" runat="server" class="control-label required-field" Text="Service Charge"></asp:Label>
                            </div>
                            <div id="ServiceChargeControl" class="col-md-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtServiceCharge" runat="server" CssClass="form-control quantitydecimal" TabIndex="3"></asp:TextBox>
                                    <asp:HiddenField ID="hfServiceCharge" runat="server"></asp:HiddenField>
                                    <span class="input-group-addon">
                                        <asp:CheckBox ID="cbServiceCharge" runat="server" Text="" onclick="javascript: return ToggleChargeEnableDisable(this);" TabIndex="8" Checked="True" />
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblGuestServiceSDCharge" runat="server" class="control-label required-field" Text="SD Charge"></asp:Label>
                            </div>
                            <div id="CityChargeControl" class="col-md-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtSDCharge" TabIndex="2" CssClass="form-control quantitydecimal"
                                        runat="server"></asp:TextBox>
                                    <span class="input-group-addon">
                                        <asp:CheckBox ID="cbSDCharge" runat="server" onclick="javascript: return ToggleChargeEnableDisable(this);" Checked="True" />
                                    </span>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblVatAmount" runat="server" class="control-label required-field" Text="Vat Amount"></asp:Label>
                            </div>
                            <div id="VatAmountControl" class="col-md-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtVatAmount" runat="server" CssClass="form-control quantitydecimal" TabIndex="5"></asp:TextBox>
                                    <asp:HiddenField ID="hfVatAmount" runat="server"></asp:HiddenField>
                                    <span class="input-group-addon">
                                        <asp:CheckBox ID="cbVatAmount" runat="server" Text="" onclick="javascript: return ToggleChargeEnableDisable(this);" TabIndex="8" Checked="True" />
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblIsVatSChargeInclusive" runat="server" class="control-label required-field" Text="S. Charge & Vat"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlIsVatSChargeInclusive" runat="server" CssClass="form-control"
                                    TabIndex="7">
                                    <asp:ListItem Value="1">Inclusive</asp:ListItem>
                                    <asp:ListItem Value="0">Exclusive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlGuestServiceRateIsPlusPlus" runat="server" CssClass="form-control"
                                    TabIndex="7">
                                    <asp:ListItem Value="1">+++ Rate</asp:ListItem>
                                    <asp:ListItem Value="0">Flat Rate</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblAdditionalCharge" runat="server" class="control-label required-field" Text="Additional Charge"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlAdditionalChargeType" CssClass="form-control" runat="server" TabIndex="11">
                                    <asp:ListItem>Fixed</asp:ListItem>
                                    <asp:ListItem>Percentage</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <div class="input-group">
                                    <asp:TextBox ID="txtAdditionalChargeAmount" TabIndex="1" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                                    <span class="input-group-addon">
                                        <asp:CheckBox ID="cbAdditionalCharge" runat="server" onclick="javascript: return ToggleChargeEnableDisable(this);" Checked="True" />
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 text-left">
                                <asp:Label ID="lblValEnableOnSd" runat="server" CssClass="control-label text-left required-field" Text="Is Vat On SD Charge"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlVatEnableOnSDCharge" runat="server" CssClass="form-control"
                                    TabIndex="7">
                                    <asp:ListItem Value="1">Vat Enable On SD/City Charge</asp:ListItem>
                                    <asp:ListItem Value="0">Vat Not Enable On SD/City Charge</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 text-left">
                                <asp:Label ID="Label2" runat="server" CssClass="control-label text-left required-field" Text="Department"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPayrollDept" runat="server" CssClass="form-control"
                                    TabIndex="8">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDefaultView" runat="server" class="control-label required-field" Text="Default View"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlDefaultView" runat="server" CssClass="form-control" TabIndex="7">
                                    <asp:ListItem Value="Token">Token</asp:ListItem>
                                    <asp:ListItem id="DdlDefaultViewTable" Value="Table">Table</asp:ListItem>
                                    <asp:ListItem Value="Room">Room</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblIsDefaultCostCenter" runat="server" class="control-label required-field" Text="Is Default CostCenter"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIsDefaultCostCenter" runat="server" CssClass="form-control"
                                    TabIndex="7">
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblOutletType" runat="server" class="control-label required-field" Text="Outlet Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlOutletType" runat="server" CssClass="form-control" TabIndex="7">
                                    <asp:ListItem Value="0">None</asp:ListItem>
                                    <asp:ListItem Value="1">Requisition</asp:ListItem>
                                    <asp:ListItem Value="2">Purchase Order</asp:ListItem>
                                    <asp:ListItem Value="3">Food & Beverage</asp:ListItem>
                                    <asp:ListItem Value="4">Others Outlet</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Cost Center Prefix"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtBillNumberPrefix" runat="server" CssClass="form-control" MaxLength="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblInvoiceTemplate" runat="server" class="control-label required-field" Text="Invoice Template"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlInvoiceTemplate" runat="server" CssClass="form-control" TabIndex="7">
                                    <asp:ListItem Value="1">POS Format</asp:ListItem>
                                    <asp:ListItem Value="4">POS with KOT Format</asp:ListItem>
                                    <asp:ListItem Value="2">A4-01 Format</asp:ListItem>
                                    <asp:ListItem Value="5">A4-02 Format</asp:ListItem>
                                    <asp:ListItem Value="3">A4 Two Column Format</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblBillingTime" runat="server" class="control-label required-field" Text="Billing Time"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlBillingTime" runat="server" CssClass="form-control" TabIndex="7">
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                    <asp:ListItem Value="6">6</asp:ListItem>
                                    <asp:ListItem Value="7">7</asp:ListItem>
                                    <asp:ListItem Value="8">8</asp:ListItem>
                                    <asp:ListItem Value="9">9</asp:ListItem>
                                    <asp:ListItem Value="10">10</asp:ListItem>
                                    <asp:ListItem Value="11">11</asp:ListItem>
                                    <asp:ListItem Value="-1">12</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="IsDiscountEnableDiv" style="display: none">
                            <div class="col-md-2">
                                <asp:Label ID="lblIsDiscountEnable" runat="server" class="control-label required-field" Text="Is Discount Enable ? "></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIsDiscountEnable" runat="server" CssClass="form-control"
                                    TabIndex="7">
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="IsEnableItemAutoDeductFromStoreDiv" style="display: none">
                            <div class="col-md-4">
                                <asp:Label ID="lblIsEnableItemAutoDeductFromStore" runat="server" class="control-label" Text="Is Enable Item Auto Deduct From Store ? "></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlIsEnableItemAutoDeductFromStore" runat="server" CssClass="form-control"
                                    TabIndex="7">
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="CategoryCostCenterInformationDiv" class="panel panel-default" style="display: none;">
                            <div class="panel-body">
                                <div>
                                    <asp:GridView ID="gvCategoryCostCenterInfo" Width="100%" runat="server" AllowPaging="True"
                                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                        ForeColor="#333333" PageSize="200" CssClass="table table-bordered table-condensed table-responsive">
                                        <RowStyle BackColor="#E3EAEB" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCategoryId" runat="server" Text='<%#Eval("FieldId") %>'></asp:Label>
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
                                            <asp:TemplateField HeaderText="Classification Information" ItemStyle-Width="55%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvCategory" runat="server" Text='<%# Bind("FieldValue") %>'></asp:Label>
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
                        <asp:Panel ID="pnlDefaultStockDeductionLocationInfo" runat="server">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblDefaultStockDeductionLocationId" runat="server" class="control-label"
                                        Text="Default Stock Location"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlDefaultStockDeductionLocationId" runat="server" CssClass="form-control"
                                        TabIndex="7">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" TabIndex="8" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="9" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearAction();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <asp:GridView ID="gvCostCenter" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" PageSize="300" AllowSorting="True" ForeColor="#333333"
                        OnPageIndexChanging="gvCostCenter_PageIndexChanging" OnRowCommand="gvCostCenter_RowCommand"
                        OnRowDataBound="gvCostCenter_RowDataBound" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("CostCenterId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CostCenter" HeaderText="Cost Center" ItemStyle-Width="70%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CostCenterType" HeaderText="Type" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("CostCenterId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
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
                $('#btnNewCostCenter').hide();
                $('#EntryPanel').show();
            }
        }
        else {
            NewAddButtonPanelHide();
        }
    </script>
</asp:Content>
