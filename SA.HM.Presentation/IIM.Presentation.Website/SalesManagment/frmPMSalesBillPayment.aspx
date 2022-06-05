<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmPMSalesBillPayment.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesManagment.frmPMSalesBillPayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Sales Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Bill Receive</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);



            $("#btnAddDetailGuestPayment").click(function () {


                var txtReceiveLeadgerAmount = '<%=txtReceiveLeadgerAmount.ClientID%>'
                var txtCardNumber = '<%=txtCardNumber.ClientID%>'
                var amount = $('#' + txtReceiveLeadgerAmount).val();
                var number = $('#' + txtCardNumber).val()
                var isValid = ValidateForm();
                if (isValid == false) {
                    return;
                }
                else if (amount == "") {
                    MessagePanelShow();
                    $('#ContentPlaceHolder1_lblMessage').text('Please provide Receive Amount.');
                    return;
                }
                else {
                    SaveGuestPaymentDetailsInformationByWebMethod();
                }

            });

            $(function () {
                var ddlPayMode = '<%=ddlPayMode.ClientID%>'
                var lblPaymentAccountHead = '<%=lblPaymentAccountHead.ClientID%>'
                $('#' + ddlPayMode).change(function () {

                    if ($('#' + ddlPayMode).val() == "Cash") {
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').show();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).show();
                        $('#ComPaymentDiv').hide();
                        $('#PrintPreviewDiv').show();
                    }
                    else if ($('#' + ddlPayMode).val() == "Card") {
                        $('#CardPaymentAccountHeadDiv').show();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).hide();
                        $('#ComPaymentDiv').hide();
                        $('#PrintPreviewDiv').show();
                    }
                    else if ($('#' + ddlPayMode).val() == "Cheque") {
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').show();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).hide();
                        $('#ComPaymentDiv').hide();
                        $('#PrintPreviewDiv').show();
                    }
                    else if ($('#' + ddlPayMode).val() == "Company") {
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').show();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#lblPaymentAccountHead').hide();
                        $('#' + lblPaymentAccountHead).show();
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#ComPaymentDiv').show();
                        $('#PrintPreviewDiv').hide();
                        popup(1, 'BillSplitPopUpForm', '', 600, 518);
                    }
                    else if ($('#' + ddlPayMode).val() == "Other Room") {
                        $('#PaidByOtherRoomDiv').show();
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).show();
                        $('#ComPaymentDiv').hide();
                        $('#PrintPreviewDiv').show();
                    }
                });
            });



            var ddlGLProject = '<%=ddlGLProject.ClientID%>'
            var ddlGLCompany = '<%=ddlGLCompany.ClientID%>'
            var selectedIndex = parseFloat($('#' + ddlGLCompany).prop("selectedIndex"));
            if (selectedIndex > 0) {
                $("#" + ddlGLProject).attr("disabled", false);
            }
            else {
                $("#" + ddlGLProject).attr("disabled", true);
            }

        });





        function ValidateForm() {

            var isCardValid = validateCard();
            var isDateValid = ValidateExpireDate();
            if (isCardValid != true) {
                return false;
            }
            else if (isDateValid != true) {
                var lblMessage = '<%=lblMessage.ClientID%>'
                $('#' + lblMessage).text("Please fill the Expiry Date");
                MessagePanelShow();
                return false;
            }
            else {
                return true;
            }
        }
        function ValidateExpireDate() {
            var isValid = true;
            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            var txtExpireDate = '<%=txtExpireDate.ClientID%>'
            if ($('#' + ddlPayMode).val() == "Card") {
                if ($('#' + txtExpireDate).val() == "") {
                    isValid = false;
                }
            }
            return isValid;
        }


        function validateCard() {
            var txtCardValidation = '<%=txtCardValidation.ClientID%>'
            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            if ($('#' + ddlPayMode).val() != "Card") {
                return true;
            }

            if ($('#' + txtCardValidation).val() == 0) {
                return true;
            }


            var txtCardNumber = '<%=txtCardNumber.ClientID%>'
            var ddlCardType = '<%=ddlCardType.ClientID%>'
            var cardNumber = $('#' + txtCardNumber).val();
            var cardType = $('#' + ddlCardType).val();
            var isTrue = true;
            var messege = "";

            if (!cardType) {
                isTrue = false;
                messege = "Card number must not be empty.";
            }

            if (cardNumber.length == 0) {						//most of these checks are self explanitory

                //alert("Please enter a valid card number.");
                isTrue = false;
                messege = "Please enter a valid card number."

            }
            for (var i = 0; i < cardNumber.length; ++i) {		// make sure the number is all digits.. (by design)
                var c = cardNumber.charAt(i);


                if (c < '0' || c > '9') {

                    isTrue = false;
                    messege = "Please enter a valid card number. Use only digits. do not use spaces or hyphens.";
                }
            }
            var length = cardNumber.length; 		//perform card specific length and prefix tests

            switch (cardType) {
                case 'a':
                    if (length != 15) {

                        //alert("");
                        isTrue = false;
                        messege = "Please enter a valid American Express Card number.";
                    }
                    var prefix = parseInt(cardNumber.substring(0, 2));

                    if (prefix != 34 && prefix != 37) {


                        isTrue = false;
                        messege = "Please enter a valid American Express Card number.";
                    }
                    break;
                case 'd':

                    if (length != 16) {

                        //alert("");
                        isTrue = false;
                        messege = "Please enter a valid Discover Card number.";
                    }
                    var prefix = parseInt(cardNumber.substring(0, 4));

                    if (prefix != 6011) {

                        //alert("Please enter a valid Discover Card number.");
                        isTrue = false;
                        messege = "Please enter a valid Discover Card number.";
                    }
                    break;
                case 'm':

                    if (length != 16) {

                        //alert("");
                        isTrue = false;
                        messege = "Please enter a valid MasterCard number.";
                    }
                    var prefix = parseInt(cardNumber.substring(0, 2));

                    if (prefix < 51 || prefix > 55) {

                        isTrue = false;
                        messege = "Please enter a valid MasterCard number.";
                    }
                    break;
                case 'v':

                    if (length != 16 && length != 13) {

                        //alert("");
                        isTrue = false;
                        messege = "Please enter a valid Visa Card number.";
                    }
                    var prefix = parseInt(cardNumber.substring(0, 1));

                    if (prefix != 4) {
                        isTrue = false;
                        messege = "Please enter a valid Visa Card number.";
                    }
                    break;
            }
            if (!mod10(cardNumber)) {
                //alert("");
                isTrue = false;
                messege = "Sorry! this is not a valid credit card number.";
            }

            if (isTrue == false) {
                MessagePanelShow();
                var lblMessage = '<%=lblMessage.ClientID%>'
                $('#' + lblMessage).text(messege);
                alert(messege);
                return false;
            }
            else {
                MessagePanelHide();
                return true;
            }
        }


        function mod10(cardNumber) { // LUHN Formula for validation of credit card numbers.
            var ar = new Array(cardNumber.length);
            var i = 0, sum = 0;

            for (i = 0; i < cardNumber.length; ++i) {
                ar[i] = parseInt(cardNumber.charAt(i));
            }
            for (i = ar.length - 2; i >= 0; i -= 2) { // you have to start from the right, and work back.
                ar[i] *= 2; 						 // every second digit starting with the right most (check digit)
                if (ar[i] > 9) ar[i] -= 9; 		 // will be doubled, and summed with the skipped digits.
            } 									 // if the double digit is > 9, ADD those individual digits together 


            for (i = 0; i < ar.length; ++i) {
                sum += ar[i]; 					 // if the sum is divisible by 10 mod10 succeeds
            }
            return (((sum % 10) == 0) ? true : false);
        }


        function SaveGuestPaymentDetailsInformationByWebMethod() {

            var Amount = $("#<%=txtReceiveLeadgerAmount.ClientID %>").val();
            var floatAmout = parseFloat(Amount);
            if (floatAmout <= 0) {
                MessagePanelShow();
                $('#ContentPlaceHolder1_lblMessage').text('Receive Amount is not in correct format.');
                return;
            }
            else {
                MessagePanelHide();
                $('#ContentPlaceHolder1_lblMessage').text('');
            }


            var isEdit = false;
            if ($('#btnAddDetailGuestPayment').val() == "Edit") {
                $("#<%=lblHiddenIdDetailGuestPayment.ClientID %>").val("5");
                isEdit = true;
            }
            else {
                $("#<%=lblHiddenIdDetailGuestPayment.ClientID %>").val("0");
            }

            var ddlPayMode = $("#<%=ddlPayMode.ClientID %>").val();
            var txtReceiveLeadgerAmount = $("#<%=txtReceiveLeadgerAmount.ClientID %>").val();
            var ddlCashReceiveAccountsInfo = $("#<%=ddlCashReceiveAccountsInfo.ClientID %>").val();

            var txtCardNumber = $("#<%=txtCardNumber.ClientID %>").val();
            var ddlCardType = $("#<%=ddlCardType.ClientID %>").val();
            var txtExpireDate = $("#<%=txtExpireDate.ClientID %>").val();

            var ddlBankName = $("#<%=ddlBankName.ClientID %>").val();

            var txtCardHolderName = $("#<%=txtCardHolderName.ClientID %>").val();
            var txtChecqueNumber = $("#<%=txtChecqueNumber.ClientID %>").val();
            var ddlBankId = $("#<%=ddlBankId.ClientID %>").val();
            var ddlCompanyPaymentAccountHead = $("#<%=ddlCompanyPaymentAccountHead.ClientID %>").val();



            $('#btnAddDetailGuestPayment').val("Add");

            PageMethods.PerformSaveGuestPaymentDetailsInformationByWebMethod(isEdit, ddlPayMode, txtReceiveLeadgerAmount, ddlCashReceiveAccountsInfo, txtCardNumber, ddlCardType, txtExpireDate, txtCardHolderName, txtChecqueNumber, ddlBankId, ddlCompanyPaymentAccountHead, OnPerformSaveGuestPaymentDetailsInformationSucceeded, OnPerformSaveGuestPaymentDetailsInformationFailed)

            return false;
        }
        function OnPerformSaveGuestPaymentDetailsInformationFailed(error) {
            alert(error.get_message());
        }
        function OnPerformSaveGuestPaymentDetailsInformationSucceeded(result) {
            $("#GuestPaymentDetailGrid").html(result);
            ClearDetailsPart();
            GetTotalPaidAmount()
        }


        function GetTotalPaidAmount() {
            PageMethods.PerformGetTotalPaidAmountByWebMethod(OnPerformGetTotalPaidAmountSucceeded, PerformGetTotalPaidAmountFailed)
            return false;
        }

        function PerformGetTotalPaidAmountFailed(error) {
            alert(error.get_message());
        }
        function OnPerformGetTotalPaidAmountSucceeded(result) {

        }

        function ClearDetailsPart() {

            $("#<%=txtReceiveLeadgerAmount.ClientID %>").val('');

            $("#<%=txtCardNumber.ClientID %>").val('');
            $("#<%=ddlCardType.ClientID %>").val('a');
            $("#<%=txtExpireDate.ClientID %>").val('');
            $("#<%=txtCardHolderName.ClientID %>").val('');

            $("#<%=txtChecqueNumber.ClientID %>").val('');
        }


        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }

        function PopulateProjects() {
            $("#<%=ddlGLProject.ClientID%>").attr("disabled", "disabled");
            if ($('#<%=ddlGLCompany.ClientID%>').val() == "0") {
                $('#<%=ddlGLProject.ClientID %>').empty().append('<option selected="selected" value="0">Please select</option>');
            }
            else {
                $('#<%=ddlGLProject.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
                $.ajax({
                    type: "POST",
                    url: "/GeneralLedger/frmGLProject.aspx/PopulateProjects",
                    data: '{companyId: ' + $('#<%=ddlGLCompany.ClientID%>').val() + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnProjectsPopulated,
                    failure: function (response) {
                        alert(response.d);
                    }
                });
            }
        }

        function OnProjectsPopulated(response) {
            var ddlGLProject = '<%=ddlGLProject.ClientID%>'
            $("#" + ddlGLProject).attr("disabled", false);

            PopulateControl(response.d, $("#<%=ddlGLProject.ClientID %>"), $("#<%=CommonDropDownHiddenField.ClientID %>").val());
        }

    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
        <asp:HiddenField ID="txtCardValidation" runat="server"></asp:HiddenField>
    </div>
    <div id="EntryPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Bill Receive Information
        </a>
        <div class="HMBodyContainer">
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                    <asp:Label ID="lblInvoiceNumber" runat="server" Text="Invoice Number"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtInvoiceNumber" runat="server" TabIndex="1"> </asp:TextBox>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblCustomerId" runat="server" Text="Customer Id"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:HiddenField ID="txtCustomerId" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="txtInvoiceId" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="txtPaymentId" runat="server"></asp:HiddenField>
                    <asp:TextBox ID="txtCustomerCode" runat="server" TabIndex="2" > </asp:TextBox>
                    <asp:HiddenField ID="txtHiddenFieldId" runat="server"></asp:HiddenField>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblSCustomerName" runat="server" Text="Customer Name"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtSCustomerName" CssClass="ThreeColumnTextBox" runat="server" TabIndex="3" > </asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="HMContainerRowButton">
                <%--Right Left--%>
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                    CssClass="TransactionalButton btn btn-primary" TabIndex="4" />
            </div>
        </div>
        <div class="divFullSectionWithTwoDvie">
            <div class="divBox divSectionLeftRightSameWidth">
                <div id="Div1" class="block">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Customer Information
                    </a>
                    <asp:Panel ID="pnlCustomerInformation" runat="server" Height="162px">
                        <div class="HMBodyContainer">
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblHCustomerName" runat="server" Text="Customer Name"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:Label ID="lblCustomerName" runat="server" Font-Bold="True"></asp:Label>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblHCode" runat="server" Text="CustomerCode"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:Label ID="lblCode" runat="server" Font-Bold="True"></asp:Label>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblHBillForm" runat="server" Text="Bill Form"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:Label ID="lblBillForm" runat="server" Font-Bold="True"></asp:Label>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblHBillTo" runat="server" Text="Bill To"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:Label ID="lblBillTo" runat="server" Font-Bold="True"></asp:Label>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblHInvoiceAmount" runat="server" Text="Invoice Amount"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:Label ID="lblInvoiceAmount" runat="server" Font-Bold="True"></asp:Label>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
            <div class="divBox divSectionLeftRightSameWidth">
                <div id="ProfitLossInformation" class="block">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Payment Information
                        (Last 5 Payments) </a>
                    <asp:Panel ID="pnlPaymentInformation" runat="server" Height="162px">
                        <div class="HMBodyContainerGridView">
                            <asp:GridView ID="gvPaymentDetails" Width="100%" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                ForeColor="#333333" PageSize="5" TabIndex="9">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("PaymentId") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PaymentDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Date"
                                        ItemStyle-Width="45%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PaymentLocalAmount" HeaderText="Amount" ItemStyle-Width="40%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="15%">
                                        <ItemTemplate>
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
                            <div class="divClear">
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
        <div id="ActionPanel">
            <div class="HMBodyContainer">
                <!--   <div style=" display:none">
                <div id="GLCompanyAndProjectInformation" runat="server">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblGLCompany" runat="server" Text="Company"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlGLCompany" runat="server" onchange="PopulateProjects();">
                            </asp:DropDownList>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblGLProject" runat="server" Text="Project"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:DropDownList ID="ddlGLProject" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblPaymentMode" runat="server" Text="Payment Mode"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlPaymentMode" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPaymentMode_SelectedIndexChanged">
                            <asp:ListItem>Cash</asp:ListItem>
                            <asp:ListItem>Bank</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="lblPaymentAccountHeadii" runat="server" Text="Account Head"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:DropDownList ID="ddlPaymentAccountHead" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblIncomeAccountHead" runat="server" Text="Item Name"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlIncomeAccountHead" runat="server" CssClass="ThreeColumnDropDownList">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblReceiveAmount" runat="server" Text="Receive Amount"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtReceiveAmount" runat="server"> </asp:TextBox>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="lblConvertionRate" runat="server" Text="Convertion Rate"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:TextBox ID="txtConvertionRate" runat="server"> </asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="HMContainerRowButton">
                    <%--Right Left--%>
                    <asp:Button ID="btnAddDetail" runat="server" Text="Add" CssClass="TransactionalButton btn btn-primary"
                        TabIndex="13" OnClick="btnAddDetail_Click" />
                    <asp:Label ID="lblHiddenId" runat="server" Text='' Visible="False"></asp:Label>
                </div>
                <div class="divClear">
                </div>
                <div class="childDivSection">
                    <asp:GridView ID="gvDetail" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                        OnRowCommand="gvDetail_RowCommand" OnRowDataBound="gvDetail_RowDataBound">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("NodeId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="NodeHead" HeaderText="Account Head" ItemStyle-Width="55%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Debit Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblLedgerDebitAmount" runat="server" Text='<%# bind("LedgerDebitAmount") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="15%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Credit Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblLedgerCreditAmount" runat="server" Text='<%# bind("LedgerCreditAmount") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="15%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# bind("NodeId") %>'
                                        CommandName="CmdDelete" ImageUrl="~/Images/delete.png" OnClientClick="return confirm('Do you want to Delete?');"
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
                <div class="divClear">
                </div>
            </div> 
            -->
                <div class="divClear">
                </div>
                <div id="PaymentDetailsInformation" class="childDivSection">
                    <div class="block" style="min-height: 160px">
                        <a href="#page-stats" class="block-heading" data-toggle="collapse">Payment Information
                        </a>
                        <div class="childDivSectionDivBlockBody">
                            <div class="divSection" style="display: none;">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="Label1" runat="server" Text="Currency Type"></asp:Label>
                                    <span class="MandatoryField">*</span>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:DropDownList ID="DropDownList1" runat="server" TabIndex="5">
                                    </asp:DropDownList>
                                </div>
                                <div id="CurrencyAmountInformationDiv">
                                    <div class="divBox divSectionRightLeft">
                                        <asp:Label ID="lblCurrencyAmount" runat="server" Text="Conversion Rate"></asp:Label>
                                        <span class="MandatoryField">*</span>
                                    </div>
                                    <div class="divBox divSectionRightRight">
                                        <asp:TextBox ID="txtConversionRate" runat="server" Text="6"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblPayMode" runat="server" Text="Payment Mode"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:DropDownList ID="ddlPayMode" runat="server" TabIndex="7">
                                        <asp:ListItem>Cash</asp:ListItem>
                                        <asp:ListItem>Card</asp:ListItem>
                                        <asp:ListItem>Cheque</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="divBox divSectionRightLeft">
                                    <asp:Label ID="lblReceiveLeadgerAmount" runat="server" Text="Receive Amount"></asp:Label>
                                </div>
                                <div class="divBox divSectionRightRight">
                                    <asp:TextBox ID="txtReceiveLeadgerAmount" runat="server" TabIndex="8"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblPaymentAccountHead" runat="server" Text="Account Head"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <div id="CashPaymentAccountHeadDiv">
                                        <asp:DropDownList ID="ddlCashReceiveAccountsInfo" TabIndex="9" runat="server" CssClass="childDivSectionDivThreeColumnDropDownList">
                                        </asp:DropDownList>
                                    </div>
                                    <div id="BankPaymentAccountHeadDiv" style="display: none;">
                                        <asp:DropDownList ID="ddlCardReceiveAccountsInfo" TabIndex="9" runat="server" CssClass="childDivSectionDivThreeColumnDropDownList">
                                        </asp:DropDownList>
                                    </div>
                                    <div id="CompanyPaymentAccountHeadDiv" style="display: none;">
                                        <asp:DropDownList ID="ddlCompanyPaymentAccountHead" TabIndex="9" runat="server" CssClass="childDivSectionDivThreeColumnDropDownList">
                                        </asp:DropDownList>
                                    </div>
                                    <div id="PaidByOtherRoomDiv" style="display: none">
                                        <asp:DropDownList ID="ddlPaidByRegistrationId"  TabIndex="9" runat="server" CssClass="childDivSectionDivThreeColumnDropDownList">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div id="ChecquePaymentAccountHeadDiv" style="display: none;">
                                <div class="divClear">
                                </div>
                                <div class="divSection">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="lblBankId" runat="server" Text="Bank Name"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:DropDownList ID="ddlBankId" runat="server" AutoPostBack="True" TabIndex="10">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="divBox divSectionRightLeft">
                                        <asp:Label ID="lblChecqueNumber" runat="server" Text="Cheque Number"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionRightRight">
                                        <asp:TextBox ID="txtChecqueNumber" runat="server" TabIndex="11"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div id="CardPaymentAccountHeadDiv" style="display: none;">
                                <div class="divClear">
                                </div>
                                <div class="divSection">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="lblBankName" runat="server" Text="Bank Name"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:DropDownList ID="ddlBankName" runat="server" TabIndex="12" CssClass="ThreeColumnDropDownList"
                                            AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                                <div class="divSection">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="Label3" runat="server" Text="Card Type"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:DropDownList ID="ddlCardType" runat="server" TabIndex="13" CssClass="tdLeftAlignWithSize">
                                            <asp:ListItem Value="a">American Express</asp:ListItem>
                                            <asp:ListItem Value="m">Master Card</asp:ListItem>
                                            <asp:ListItem Value="v">Visa Card</asp:ListItem>
                                            <asp:ListItem Value="d">Discover Card</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="divBox divSectionRightLeft">
                                        <asp:Label ID="lblCardNumber" runat="server" Text="Card Number"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionRightRight">
                                        <asp:TextBox ID="txtCardNumber" TabIndex="14" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                                <div class="divSection">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="Label4" runat="server" Text="Expiry Date"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtExpireDate" TabIndex="15" CssClass="datepicker" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="divBox divSectionRightLeft">
                                        <asp:Label ID="Label5" runat="server" Text="Card Holder Name"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionRightRight">
                                        <asp:TextBox ID="txtCardHolderName" TabIndex="16" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection" style="padding-left: 10px;">
                                <%--Right Left--%>
                                <input type="button" id="btnAddDetailGuestPayment" value="Add" tabindex="17" class="TransactionalButton btn btn-primary"
                                    onclientclick="javascript: return ValidateForm();" />
                                <asp:Label ID="lblHiddenIdDetailGuestPayment" runat="server" Text='' Visible="False"></asp:Label>
                            </div>
                            <div class="divClear">
                            </div>
                            <div id="GuestPaymentDetailGrid" class="childDivSection">
                            </div>
                            <div id="TotalPaid" class="totalAmout">
                            </div>
                            <div class="divClear">
                            </div>
                            <div>
                                <asp:Label ID="AlartMessege" runat="server" Style="color: Red;" Text='Grand Total and Payment Amount is not Equal.'
                                    CssClass="totalAmout"></asp:Label>
                            </div>
                            <div class="divClear">
                            </div>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="HMContainerRowButton">
                    <%--Right Left--%>
                    <asp:Button ID="btnReceive" runat="server" Text="Receive" OnClick="btnReceive_Click"
                        CssClass="TransactionalButton btn btn-primary" TabIndex="18" />
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
