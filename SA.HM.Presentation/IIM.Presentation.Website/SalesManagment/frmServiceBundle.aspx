<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmServiceBundle.aspx.cs" Inherits="HotelManagement.Presentation.Website.Restaurant.frmServiceBundle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Sales Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Service Bundle</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            LoadInitialGridView();
            var txtSellingPriceUsd = '<%=txtSellingPriceUsd.ClientID%>'
            var lblSellingPriceUsd = '<%=lblSellingPriceUsd.ClientID%>'
            var txtCurrencySetup = '<%=txtCurrencySetup.ClientID%>'

            var setupValue = $('#' + txtCurrencySetup).val();
            var txtCurrentId = '<%=txtCurrentId.ClientID%>';
            $('#' + txtCurrentId).val(0);

            if (setupValue == "Single") {
                $('#' + lblSellingPriceUsd).hide();
                $('#' + txtSellingPriceUsd).hide();
                $('#MandatoryField').hide();
            }
            else {
                $('#' + txtSellingPriceUsd).show();
                $('#' + lblSellingPriceUsd).show();
                $('#MandatoryField').show();
            }

            var ddlFrequency = '<%=ddlFrequency.ClientID%>'
            var frequencyId = $('#' + ddlFrequency).val();
            var ddlIsProductOrService = '<%=ddlIsProductOrService.ClientID%>'
            var ProductOrService = $('#' + ddlIsProductOrService).val();
            LoadddlProductId(frequencyId, ProductOrService);



            $('#' + ddlFrequency).change(function () {
                var frequencyId = $('#' + ddlFrequency).val();
                var serviceType = $('#' + ddlIsProductOrService).val();

                LoadddlProductId(frequencyId, serviceType);
            });

            var ddlIsProductOrService = '<%=ddlIsProductOrService.ClientID%>'
            $('#' + ddlIsProductOrService).change(function () {
                var frequencyId = $('#' + ddlFrequency).val();
                var serviceType = $('#' + ddlIsProductOrService).val();
                LoadddlProductId(frequencyId, serviceType);
            });

            $('#btnOwnerDetails').click(function () {
                var txtQuantity = '<%=txtQuantity.ClientID%>'
                var Quantity = $('#' + txtQuantity).val();

                var ddlIsProductOrService = '<%=ddlIsProductOrService.ClientID%>'
                var ProductOrService = $('#' + ddlIsProductOrService).val();

                var ddlProductId = '<%=ddlProductId.ClientID%>'
                var ProductId = $('#' + ddlProductId).val();

                var txtBundleId = '<%=txtBundleId.ClientID%>'
                var BundleId = $('#' + txtBundleId).val();

                if (typeof ProductId === "undefined") {
                    ProductId = -1;
                }


                if (typeof ServiceId === "undefined") {
                    ServiceId = -1;
                }

                var txtCurrencySetup = '<%=txtCurrencySetup.ClientID%>'
                var CurrencySetup = $('#' + txtCurrencySetup).val();

                var txtDetailsId = '<%=txtDetailsId.ClientID%>'
                var DetailsId = $('#' + txtDetailsId).val();

                var productName = $("#ContentPlaceHolder1_ddlProductId option:selected").text();
                $('#btnOwnerDetails').text("Add");
                $('#btnOwnerDetails').val("Add");
                PageMethods.SaveBundleDetailsInformation(BundleId,ProductOrService, ProductId, productName, Quantity, CurrencySetup, DetailsId, OnSaveBundleDetailsInformationSucceeded, OnSaveBundleDetailsInformationFailed);
                return false;
            });

        });



        $(function () {
            $("#myTabs").tabs();
        });


        function LoadInitialGridView() {
            var txtBundleId = '<%=txtBundleId.ClientID%>'
            var txtCurrencySetup = '<%=txtCurrencySetup.ClientID%>'
            var Currency = $('#' + txtCurrencySetup).val();
            var BundleId = $('#' + txtBundleId).val();
            if (BundleId != "") {
                PageMethods.LoadBundleDetailGridView(BundleId, Currency, OnLoadSalesDetailGridViewSucceeded, OnLoadSalesDetailGridViewFailed);
                return false;
            }
        }

        function OnLoadSalesDetailGridViewSucceeded(result) {
            $('#productDetailGrid').html(result);
            GetTotalAmount();
            return false;
        }

        function OnLoadSalesDetailGridViewFailed(error) {
        }
        function LoadddlProductId(frequencyId, serviceType) {
            PageMethods.GetServiceByCriteria(frequencyId, serviceType, OnFillServiceSucceeded, OnFillServiceFailed);
            return false;
        }

        function OnFillServiceSucceeded(result) {
            var ddlServiceType = '<%=ddlIsProductOrService.ClientID%>'
            var txtCurrentId = '<%=txtCurrentId.ClientID%>'

            var serviceType = $('#' + ddlServiceType).val();
            var list = result;
            var controlId = '<%=ddlProductId.ClientID%>';
            var control = $('#' + controlId);
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].ItemName + '" value="' + list[i].ItemId + '">' + list[i].ItemName + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            var id = $('#' + txtCurrentId).val();
            $('#' + controlId).val(id);

            return false;
        }

        function OnFillServiceFailed(error) {
            alert(error.get_message());
        }

        function OnSaveBundleDetailsInformationSucceeded(result) {
            $('#productDetailGrid').html(result);
            GetTotalAmount();
            ClearBundleDetails();

            return false;
        }
        function OnSaveBundleDetailsInformationFailed(error) {
        }
        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }



        function ClearBundleDetails() {
            var txtQuantity = '<%=txtQuantity.ClientID%>'
            var ddlIsProductOrService = '<%=ddlIsProductOrService.ClientID%>'
            var ddlProductId = '<%=ddlProductId.ClientID%>'
            var ddlFrequency = '<%=ddlFrequency.ClientID%>'
            var txtCurrentId = '<%=txtCurrentId.ClientID%>'
            $('#' + txtCurrentId).val('0');
            $('#' + ddlIsProductOrService).val('Product');
            var frequencyId = $('#' + ddlFrequency).val();
            LoadddlProductId(frequencyId, 'Product');

            $('#' + txtQuantity).val('');
        }

        function GetTotalAmount() {
            PageMethods.GetTotalAmount(OnGetTotalAmountSucceeded, OnGetTotalAmountFailed);
            return false;
        }

        function OnGetTotalAmountSucceeded(result) {
            var txtSellingPriceUsd = '<%=txtSellingPriceUsd.ClientID%>'
            var txtSellingPriceLocal = '<%=txtSellingPriceLocal.ClientID%>'
            var txtCurrencySetup = '<%=txtCurrencySetup.ClientID%>'
            var setupValue = $('#' + txtCurrencySetup).val();

            if (setupValue == "Single") {
                $('#' + txtSellingPriceLocal).val(result.TotalUnitPriceLocal);
            }
            else {
                $('#' + txtSellingPriceLocal).val(result.TotalUnitPriceLocal);
                $('#' + txtSellingPriceUsd).val(result.TotalUnitPriceUsd);
            }
        }

        function OnGetTotalAmountFailed(error) {

        }


        function PerformFillBundleDetailsWAW(DetailsId, Type, ServiceId, ProductId, Quantity) {

            var txtDetailsId = '<%=txtDetailsId.ClientID%>'
            var txtCurrentId = '<%=txtCurrentId.ClientID%>'
            var isProductOrService = '<%=ddlIsProductOrService.ClientID%>'
            var txtDetailsId = '<%=txtDetailsId.ClientID%>'
            var ddlFrequency = '<%=ddlFrequency.ClientID%>'
            var ddlProductId = '<%=ddlProductId.ClientID%>'
            var txtQuantity = '<%=txtQuantity.ClientID%>'
            if (Type == "Product") {
                $('#' + txtCurrentId).val(ProductId);
            }
            else {
                $('#' + txtCurrentId).val(ProductId);
            }

            $('#' + txtQuantity).val(Quantity);

            $('#' + txtDetailsId).val(DetailsId);
            $('#' + isProductOrService).val(Type);
            var frequencyId = $('#' + ddlFrequency).val();
            LoadddlProductId(frequencyId, Type)


            $('#btnOwnerDetails').text("Edit");
            $('#btnOwnerDetails').val("Edit");
            return false;
        }

        function OnFillFormObjectSucceeded(result) {
            MessagePanelHide();

            $('#btnNew').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
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

        function PerformeleteDetailsBundleDelete(detailId, Currency) {
            PageMethods.DeleteBundleDetails(detailId, Currency, OnSaveBundleDetailsInformationSucceeded, OnDeleteObjectFailed);
        }



        function OnDeleteObjectSucceeded(result) {
            window.location = "frmServiceBundle.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            alert(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {

            $("#<%=btnSave.ClientID %>").val("Save");
            MessagePanelHide();
            return false;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNew').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNew').show("slow");
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
            $('#btnNew').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNew').hide("slow");
        }
    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
        <asp:HiddenField ID="txtCurrencySetup" runat="server" />
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Service Bundle Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Service Bundle Search</a></li>
            <%--<li id="C" runat="server"><a href="#tab-3">Title 3</a></li>--%>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Service Bundle Information</a>
                <div class="HMBodyContainer">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:HiddenField ID="txtBundleId" runat="server" />
                            <asp:Label ID="lblBundleName" runat="server" Text="Bundle Name"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtBundleName" runat="server" CssClass="ThreeColumnTextBox" TabIndex="1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblBundleCode" runat="server" Text="Bundle Code"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtBundleCode" runat="server" CssClass="customLargeTextBoxSize"
                                TabIndex="2"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblFrequency" runat="server" Text="Frequency"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:DropDownList ID="ddlFrequency" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFrequency_SelectedIndexChanged" TabIndex="3">
                                <asp:ListItem Value="One Time">One Time</asp:ListItem>
                                <asp:ListItem>Monthly</asp:ListItem>
                                <asp:ListItem>Quaterly</asp:ListItem>
                                <asp:ListItem>Half Yearly</asp:ListItem>
                                <asp:ListItem>Yearly</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="childDivSection">
                        <div id="Commissionformation" class="block">
                            <a href="#page-stats" class="block-heading" data-toggle="collapse">Item Detail Information
                            </a>
                            <div class="HMBodyContainer">
                                <div class="divSection">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="Label1" runat="server" Text="Item Type"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:HiddenField ID="txtCurrentId" runat="server" />
                                        <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                                        <asp:DropDownList ID="ddlIsProductOrService" runat="server" CssClass="customLargeDropDownSize"
                                            TabIndex="4">
                                            <asp:ListItem>Product</asp:ListItem>
                                            <asp:ListItem>Service</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="divBox divSectionRightLeft">
                                        <asp:Label ID="lblItemId" runat="server" Text="Item Name"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionRightRight">
                                        <div id="ProductDropDownList">
                                            <asp:DropDownList ID="ddlProductId" runat="server" CssClass="customLargeDropDownSize"
                                                TabIndex="5">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                                <div class="divSection">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="lblQuantity" runat="server" Text="Quantity"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtQuantity" CssClass="CustomTextBox" runat="server" TabIndex="6"></asp:TextBox>
                                        <asp:HiddenField ID="txtDetailsId" runat="server" />
                                        <input id="btnOwnerDetails" type="button" value="Add" tabindex="7" class="TransactionalButton btn btn-primary" />
                                        <asp:Label ID="lblHiddenOwnerDetailtId" runat="server" Visible="False"></asp:Label>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                                <div style="text-align: center;">
                                    <div id="productDetailGrid">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblSellingPriceLocal" runat="server" Text="Selling Price One"></asp:Label>
                                <span class="MandatoryField">*</span>
                                <asp:DropDownList ID="ddlSellingPriceLocal" runat="server" CssClass="customSmallDropDownSize"
                                    TabIndex="8" Visible="False">
                                </asp:DropDownList>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtSellingPriceLocal" runat="server" TabIndex="9"></asp:TextBox>
                            </div>
                            <div class="divBox divSectionRightLeft">
                                <asp:Label ID="lblSellingPriceUsd" runat="server" Text="Selling Price Two"></asp:Label>
                                <span id="MandatoryField" class="MandatoryField">*</span>
                                <asp:DropDownList ID="ddlSellingPriceUsd" runat="server" CssClass="customSmallDropDownSize"
                                    TabIndex="10" Visible="False">
                                </asp:DropDownList>
                            </div>
                            <div class="divBox divSectionRightRight">
                                <asp:TextBox ID="txtSellingPriceUsd" runat="server" TabIndex="11"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="HMContainerRowButton">
                            <%--Right Left--%>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                                TabIndex="12" OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                                TabIndex="13" OnClick="btnCancel_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="divClear">
        </div>
        <div id="tab-2">
            <div id="SearchServiceBundle" class="block" style="">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Service Bundle</a>
                <div class="HMBodyContainer">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblSName" runat="server" Text="Name"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtSName" runat="server" TabIndex="1"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblCode" runat="server" Text="Code"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtSCode" runat="server" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="HMContainerRowButton">
                        <%--Right Left--%>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                            CssClass="TransactionalButton btn btn-primary" TabIndex="3" />
                    </div>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div id="SearchPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Information
                </a>
                <div class="block-body collapse in">
                    <asp:GridView ID="gvRoomOwner" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="20"
                        OnRowCommand="gvRoomOwner_RowCommand" OnRowDataBound="gvRoomOwner_RowDataBound">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("BundleId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="BundleName" HeaderText="Bundle Name" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BundleCode" HeaderText="Bundle Code" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UnitPriceLocal" HeaderText="Selling Price Local" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UnitPriceUsd" HeaderText="Selling Price USD" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# bind("BundleId") %>'
                                        CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit"
                                        ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# bind("BundleId") %>'
                                        CommandName="CmdDelete" ImageUrl="~/Images/delete.png" OnClientClick="return confirm('Do you want to save?');"
                                        Text="" AlternateText="Delete" ToolTip="Delete" />
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
        var ddlIsProductOrService = '<%=ddlIsProductOrService.ClientID%>'
        var x = '<%=isMessageBoxEnable%>';
        var IsService = '<%=IsService%>';

        //        if (IsService == 1) {
        //            $('#ProductDropDownList').hide("slow");
        //            $('#ServiceDropDownList').show("slow");
        //        }
        //        else {
        //            $('#ProductDropDownList').show("slow");
        //            $('#ServiceDropDownList').hide("slow");
        //        }

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
