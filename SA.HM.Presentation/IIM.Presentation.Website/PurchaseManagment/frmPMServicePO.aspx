<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmPMServicePO.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.frmPMServicePO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Purchase</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Service Purchase Order</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });
        $(document).ready(function () {
            var txtReceivedByDate = '<%=txtReceivedByDate.ClientID%>'

            var ddlProductId = '<%=ddlProductId.ClientID%>'
            var txtPurchasePrice = '<%=txtPurchasePrice.ClientID%>'

            var txtReceivedByDate = '<%=txtReceivedByDate.ClientID%>'
            var ddlProductId = '<%=ddlProductId.ClientID%>'
            var txtPurchasePrice = '<%=txtPurchasePrice.ClientID%>'

            var ddlCategory = '<%=ddlCategory.ClientID%>'
            var lblCategory = '<%=lblCategory.ClientID%>'


            var txtPOrderId = '<%=txtPOrderId.ClientID%>'
            var pOrderId = $('#' + txtPOrderId).val();
            if (pOrderId != "") {
                LoadPMProductDetailOnEditMode(pOrderId);
            }


            LoadProductList();


            $('#' + ddlCategory).change(function () {
                LoadProductList();
            });


            $('#' + ddlProductId).change(function () {
                var ProductId = $('#' + ddlProductId).val();
                SetSelectedItem(ProductId);
            });

            $('#' + txtReceivedByDate).datepicker({
                dateFormat: innBoarDateFormat
            });

            var txtToDate = '<%=txtToDate.ClientID%>'
            $('#' + txtToDate).datepicker({
                dateFormat: innBoarDateFormat
            });
            var txtFromDate = '<%=txtFromDate.ClientID%>'
            $('#' + txtFromDate).datepicker({
                dateFormat: innBoarDateFormat
            });


            $('#btnAdd').click(function () {

                var detailId = '<%=detailId.ClientID%>'
                var DetailtId = $('#' + detailId).val();

                var ddlProductId = '<%=ddlProductId.ClientID%>'
                var ProductId = $('#' + ddlProductId).val();

                var txtPurchasePrice = '<%=txtPurchasePrice.ClientID%>'
                var PurchasePrice = $('#' + txtPurchasePrice).val();


                var ddlCategory = '<%=ddlCategory.ClientID%>'
                var Category = $('#' + ddlCategory).val();



                var txtQuantity = '<%=txtQuantity.ClientID%>'
                var Quantity = $('#' + txtQuantity).val();

                if (ProductId == "0" || PurchasePrice == "" || Quantity == "" || PurchasePrice <= 0 || Quantity <= 0) {
                    MessagePanelShow();
                    $('#ContentPlaceHolder1_lblMessage').text('Please Select Mendatory Field.');
                    return;
                }

                var detailId = '<%=detailId.ClientID%>'
                $('#' + detailId).val("");

                var txtHidenProductId = '<%=txtHidenProductId.ClientID%>'
                $('#' + txtHidenProductId).val("");

                PageMethods.SavePMServiceOrderInformation(DetailtId, ProductId, PurchasePrice, Quantity, Category, OnSavePMProductOutInformationSucceeded, OnSavePMProductOutInformationFailed);
                return false;

            });

        });
        $(function () {
            $("#myTabs").tabs();
        });

        function PerformFillFormActionWAW(DetailId, PurchasePrice, Quantity, RequisitionId, CategoryId, ProductId) {
            var ddlCategory = '<%=ddlCategory.ClientID%>'
            var lblCategory = '<%=lblCategory.ClientID%>'
            var detailId = '<%=detailId.ClientID%>'
            $('#' + detailId).val(DetailId);

            var txtHidenProductId = '<%=txtHidenProductId.ClientID%>'
            $('#' + txtHidenProductId).val(ProductId);

            var ddlCategory = '<%=ddlCategory.ClientID%>'
            $('#' + ddlCategory).val(CategoryId);

            LoadProductList();
            var txtQuantity = '<%=txtQuantity.ClientID%>'
            $('#' + txtQuantity).val(Quantity);

            $('#btnAdd').text("Edit");
            $('#btnAdd').val("Edit");
            var txtPurchasePrice = '<%=txtPurchasePrice.ClientID%>'
            $('#' + txtPurchasePrice).val(PurchasePrice);
            return false;
        }


        function LoadProductList() {
            var ddlCategory = '<%=ddlCategory.ClientID%>'
            var Category = $('#' + ddlCategory).val();
            PageMethods.LoadProductListOnPONumberChange(Category, OnLoadProductListOnPONumberChangeSucceeded, OnLoadProductListOnPONumberChangeFailed);
            return false;
        }

        function OnLoadProductListOnPONumberChangeSucceeded(result) {
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


            var txtHidenProductId = '<%=txtHidenProductId.ClientID%>'
            var product = $('#' + txtHidenProductId).val();

            $('#' + controlId).val(product);
            SetSelectedItem(product);
        }
        function OnLoadProductListOnPONumberChangeFailed(error) {
        }
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }

        function SetSelectedItem(ProductId) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/PurchaseManagment/frmPMServicePO.aspx/GetPurchasePrice",
                data: "{'ProductId':'" + ProductId + "'}",
                dataType: "json",
                success: OnSetSelected,
                error: function (result) {
                }
            });
        }
        function OnSetSelected(response) {
            if (response.d != "" || response.d > 0) {
                $("#<%=txtPurchasePrice.ClientID %>").val(response.d);
            }
        }

        function ValidatePlease() {
            var purchase = $("#<%=txtPurchasePrice.ClientID %>").val();
            var unit = $("#<%=txtQuantity.ClientID %>").val();

            if (allLetter(purchase) == false) {
                ShowErrorMessege("purchase price is not in correct format");
                return false;
            }
            else if (allLetter(unit) == false) {
                ShowErrorMessege("quantity is not in correct format");
                return false;
            }
            else if (unit == "") {
                ShowErrorMessege(" quantity must not be empty");
                return false;
            }
            else if (purchase == "") {
                ShowErrorMessege(" purchase must not be empty");
                return false;
            }
            else {

                return true;
            }
        }

        function PerformProductReceiveDelete(ReceivedId) {
            PageMethods.PerformProductReceiveDelete(ReceivedId, OnSavePMProductOutInformationSucceeded, OnSavePMProductOutInformationFailed);
            return false;
        }


        function ShowErrorMessege(Messege) {
            $("#<%=lblMessage.ClientID %>").text(Messege);
            MessagePanelShow();

        }
        function allLetter(inputtxt) {
            var IsNumber = true;
            for (var i = 0; i < inputtxt.length; i++) {
                if (inputtxt.charAt(i) == "0" || inputtxt.charAt(i) == "1" || inputtxt.charAt(i) == "2" || inputtxt.charAt(i) == "3" || inputtxt.charAt(i) == "4" || inputtxt.charAt(i) == "5" || inputtxt.charAt(i) == "6" || inputtxt.charAt(i) == "7" || inputtxt.charAt(i) == "8" || inputtxt.charAt(i) == "9" || inputtxt.charAt(i) == ".") {
                }
                else {
                    IsNumber = false;
                }
            }
            return IsNumber;
        }

        function OnSavePMProductOutInformationSucceeded(result) {
            $('#productDetailsGrid').html(result);
            PageMethods.GetCalculatedTotal(OnGetCalculatedTotalSucceeded, OnGetCalculatedTotalFailed);
            return false;
        }
        function OnSavePMProductOutInformationFailed(error) {

        }

        function OnGetCalculatedTotalSucceeded(result) {
            $("#<%=txtPurchasePrice.ClientID %>").val("");
            $("#<%=txtQuantity.ClientID %>").val("");
            $("#<%=txtlblRequisitionQuantity.ClientID %>").val("");
            $("#<%=txtlblPurchaseQuantity.ClientID %>").val("");

            $("#<%=ddlProductId.ClientID %>").val("0");
            $("#<%=ddlCategory.ClientID %>").val("0");
            $("#<%=lblTotalCalculateAmount.ClientID %>").text(result);
        }
        function OnGetCalculatedTotalFailed(error) {
        }

        function LoadPMProductDetailOnEditMode(pOrderId) {
            PageMethods.PerformLoadPMProductDetailOnEditMode(pOrderId, OnSavePMProductOutInformationSucceeded, OnSavePMProductOutInformationFailed);
            return false;
        }

        function PerformClearAction() {
            $("#frmHotelManagement")[0].reset();
            $("#productDetailsGrid").html("");
            $("#<%=txtRemarks.ClientID %>").text("");
            $("#<%=txtReceivedByDate.ClientID %>").val("");
            $("#<%=ddlSupplier.ClientID %>").val("0");

            return false;
        }

    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div style="height: 45px">
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Order Information</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Order</a></li>
        </ul>
        <div id="tab-1">
            <%-- <div id="EntryPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Purchase Order Information</a>
                <div class="HMBodyContainer">--%>
            <div class="HMBodyContainer">
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblReceivedByDate" runat="server" Text="Received By Date"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtReceivedByDate" TabIndex="1" runat="server"></asp:TextBox>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="lblSupplier" runat="server" Text="Supplier"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:DropDownList ID="ddlSupplier" TabIndex="2" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div>
                <div id="Commissionformation" class="block">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Order Detail Information
                    </a>
                    <div class="HMBodyContainer">
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:HiddenField ID="detailId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtHidenProductId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtPOrderId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblCategory" runat="server" Text="Product Category"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:DropDownList ID="ddlCategory" TabIndex="3" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="divBox divSectionRightLeft">
                            </div>
                            <div class="divBox divSectionRightRight">
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblProduct" runat="server" Text="Product"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:DropDownList ID="ddlProductId" TabIndex="4" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="divBox divSectionRightLeft">
                                <asp:Label ID="lblRequisitionQuantity" runat="server" Text="Requisition Quantity"></asp:Label>
                            </div>
                            <div class="divBox divSectionRightRight">
                                <asp:TextBox ID="txtlblRequisitionQuantity" TabIndex="5" runat="server" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <%--<asp:Label ID="lblPurchasePrice" runat="server" Text="Unit Price(USD)"></asp:Label>--%>
                                <asp:Label ID="lblPurchasePriceLocal" runat="server" Text="Selling Price One"></asp:Label>
                                <asp:DropDownList ID="ddlPurchasePriceLocal" runat="server" CssClass="customSmallDropDownSize"
                                    TabIndex="6" Visible="False">
                                </asp:DropDownList>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtPurchasePrice" TabIndex="7" runat="server"></asp:TextBox>
                            </div>
                            <div class="divBox divSectionRightLeft">
                                <asp:Label ID="lblPurchaseQuantity" runat="server" Text="Purchase Quantity"></asp:Label>
                            </div>
                            <div class="divBox divSectionRightRight">
                                <asp:TextBox ID="txtlblPurchaseQuantity" TabIndex="8" runat="server" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblQuantity" runat="server" Text="Quantity"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtQuantity" TabIndex="9" runat="server"></asp:TextBox>
                            </div>
                            <div class="divBox divSectionRightLeft">
                            </div>
                            <div class="divBox divSectionRightRight">
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="HMContainerRowButton">
                            <%--Right Left--%>
                            <input id="btnAdd" type="button" value="Add" tabindex="10" class="TransactionalButton btn btn-primary" />
                            <asp:Label ID="lblHiddenOrderDetailtId" runat="server" Visible="False"></asp:Label>
                        </div>
                        <div class="divClear">
                        </div>
                        <div id="SearchPanel">
                            <%--<a href="#page-stats" class="block-heading" data-toggle="collapse">Search Information
                                    </a>--%>
                            <div class="block-body collapse in">
                                <div id="productDetailsGrid">
                                </div>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div style="text-align: left">
                            <asp:Label ID="lblTitleTotalAmount" runat="server" Text="Total Price :" Font-Bold="True"></asp:Label>
                            <asp:Label ID="lblTotalCalculateAmount" runat="server" Font-Bold="True"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="HMBodyContainerMaster">
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblRemarks" runat="server" Text="Remarks"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="ThreeColumnTextBox" TextMode="MultiLine"
                            TabIndex="11"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="HMContainerRowButton">
                    <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="12" CssClass="TransactionalButton btn btn-primary"
                        OnClick="btnSave_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="13" CssClass="TransactionalButton btn btn-primary"
                        OnClientClick="javascript: return PerformClearAction();" />
                </div>
            </div>
            <%--  </div>
            </div>--%>
        </div>
        <div id="tab-2">
            <div id="InfoPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Order Information
                </a>
                <div class="HMBodyContainer">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblSPONumber" runat="server" Text="PO Number"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtSPONumber" runat="server" TabIndex="14"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="15">
                                <asp:ListItem>All</asp:ListItem>
                                <asp:ListItem>Submitted</asp:ListItem>
                                <asp:ListItem>Approved</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtFromDate" CssClass="datepicker" runat="server" TabIndex="16"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtToDate" CssClass="datepicker" runat="server" TabIndex="17"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="HMContainerRowButton">
                    <%--Right Left--%>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary"
                        TabIndex="18" OnClick="btnSearch_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary"
                        OnClientClick="javascript: return PerformClearAction();" TabIndex="19" />
                </div>
            </div>
            <div id="Div1" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Information
                </a>
                <div class="block-body collapse in">
                    <asp:GridView ID="gvOrderInfo" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                        OnRowCommand="gvOrderInfo_RowCommand" OnPageIndexChanging="gvOrderInfo_PageIndexChanging"
                        TabIndex="20" OnRowDataBound="gvOrderInfo_RowDataBound">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("POrderId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PONumber" HeaderText="PO Number" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Order Date" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvOrderDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("CreatedDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Received By Date" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvReceivedByDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ReceivedByDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblApprovedStatus" runat="server" Text='<%#Eval("ApprovedStatus") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("POrderId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("POrderId") %>' ImageUrl="~/Images/delete.png" Text=""
                                        AlternateText="Delete" ToolTip="Delete" />
                                    &nbsp;<asp:ImageButton ID="ImgBillPreview" runat="server" CausesValidation="False"
                                        CommandArgument='<%# bind("POrderId") %>' CommandName="CmdOrderPreview" ImageUrl="~/Images/ReportDocument.png"
                                        Text="" AlternateText="Bill Preview" ToolTip="Bill Preview" />
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

        
    </script>
</asp:Content>
