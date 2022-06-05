<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmSupplierBillPayment.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.frmSupplierBillPayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Purchase Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Supplier Bill Payment</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            //EnableDisable For DropDown Change event--------------

            var ddlCurrency = '<%=ddlCurrency.ClientID%>'
            var txtConversionRate = '<%=txtConversionRate.ClientID%>'
            var txtLedgerAmount = '<%=txtLedgerAmount.ClientID%>'
            var txtCalculatedLedgerAmount = '<%=txtCalculatedLedgerAmount.ClientID%>'
            var txtCalculatedLedgerAmountHiddenField = '<%=txtCalculatedLedgerAmountHiddenField.ClientID%>'

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $('#ContentPlaceHolder1_gvPaymentInfo_ChkAllSelect').click(function () {
                if ($('#ContentPlaceHolder1_gvPaymentInfo_ChkAllSelect').is(':checked')) {
                    CheckAllCheckBoxCreate()
                }
                else {
                    UnCheckAllCheckBoxCreate();
                }
            });

            CommonHelper.AutoSearchClientDataSource("txtBankId", "ContentPlaceHolder1_ddlBankId", "ContentPlaceHolder1_ddlBankId");
            CommonHelper.AutoSearchClientDataSource("txtCompanyBank", "ContentPlaceHolder1_ddlCompanyBank", "ContentPlaceHolder1_ddlCompanyBank");
            CommonHelper.AutoSearchClientDataSource("txtSupplierName", "ContentPlaceHolder1_ddlSupplierName", "ContentPlaceHolder1_ddlSupplierName");
            CommonHelper.AutoSearchClientDataSource("txtCashPayment", "ContentPlaceHolder1_ddlCashPayment", "ContentPlaceHolder1_ddlCashPayment");

            $('#txtCompanyBank').blur(function () {
                if ($(this).val() == "") {
                    $("#<%=ddlCompanyBank.ClientID %>").val("0");
                }
            });

            $('#txtCashPayment').blur(function () {
                if ($(this).val() == "") {
                    $("#<%=ddlCashPayment.ClientID %>").val("0");
                }
            });

            var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
            if (currencyType == "Local") {
                //$('#' + txtLedgerAmount).val("")
                $('#' + txtConversionRate).val("")
                $('#' + txtCalculatedLedgerAmount).val("");
                $('#ConversionPanel').hide();
                $('#' + txtCalculatedLedgerAmount).attr("disabled", false);
            }
            else {
                $('#ConversionPanel').show();
                //$('#ConversionPanel').hide();
                $('#' + txtCalculatedLedgerAmount).val("");
            }

            var ddlCurrencyId = $("#<%=ddlCurrency.ClientID %>").val();
            PageMethods.LoadCurrencyType(ddlCurrencyId, OnLoadCurrencyTypeSucceeded, OnLoadCurrencyTypeFailed);

            $('#' + ddlCurrency).change(function () {
                var v = $("#<%=ddlCurrency.ClientID %>").val();
                PageMethods.LoadCurrencyType(v, OnLoadCurrencyTypeSucceeded, OnLoadCurrencyTypeFailed);
            });

            function OnLoadCurrencyTypeSucceeded(result) {
                $("#<%=hfCurrencyType.ClientID %>").val(result.CurrencyType);
                PageMethods.LoadCurrencyConversionRate(result.CurrencyId, OnLoadConversionRateSucceeded, OnLoadConversionRateFailed);
            }

            function OnLoadCurrencyTypeFailed() {

            }

            function OnLoadConversionRateSucceeded(result) {

                if ($("#<%=hfCurrencyType.ClientID %>").val() == "Local") {
                    $("#<%=txtConversionRate.ClientID %>").val('');
                    //$('#<%=txtConversionRate.ClientID%>').attr("disabled", true);
                    //$('#' + txtConversionRate).val("")
                    $('#' + txtCalculatedLedgerAmount).val("");
                    $('#ConversionPanel').hide();
                    $('#' + txtCalculatedLedgerAmount).attr("disabled", false);
                }
                else {
                    //$('#<%=txtConversionRate.ClientID%>').attr("disabled", false);
                    $("#<%=txtConversionRate.ClientID %>").val(result.ConversionRate);
                    $("#<%=hfConversionRate.ClientID %>").val(result.ConversionRate);

                    $('#ConversionPanel').show();
                    $('#' + txtCalculatedLedgerAmount).val("");
                }
                CurrencyRateInfoEnable();
            }

            if(IsCanSave)
            {
                $('#ContentPlaceHolder1_btnSave').show();
            }
            else
            {
                $('#ContentPlaceHolder1_btnSave').hide();
            }

            function OnLoadConversionRateFailed() {
            }

            $('#' + txtLedgerAmount).blur(function () {
                //            var selectedIndex = parseFloat($('#' + ddlCurrency).prop("selectedIndex"));
                //                if (selectedIndex < 1) {

                var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
                if (currencyType == "Local") {
                    var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val());

                    $('#' + txtCalculatedLedgerAmount).val(LedgerAmount.toFixed(2));
                    $('#' + txtCalculatedLedgerAmountHiddenField).val(LedgerAmount.toFixed(2));
                }
                else {
                    var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val()) * parseFloat($('#' + txtConversionRate).val());
                    if (isNaN(LedgerAmount.toString())) {
                        LedgerAmount = 0;
                    }
                    $('#' + txtCalculatedLedgerAmount).val(LedgerAmount.toFixed(2));
                    $('#' + txtCalculatedLedgerAmountHiddenField).val(LedgerAmount.toFixed(2));
                    $('#' + txtCalculatedLedgerAmount).attr("disabled", true);
                }
            });

            $('#' + txtConversionRate).blur(function () {
                //            var selectedIndex = parseFloat($('#' + ddlCurrency).prop("selectedIndex"));
                //                if (selectedIndex < 1) {

                var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
                if (currencyType == "Local") {
                    var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val());
                    $('#' + txtCalculatedLedgerAmount).val(LedgerAmount.toFixed(2));
                    $('#' + txtCalculatedLedgerAmountHiddenField).val(LedgerAmount.toFixed(2));
                }
                else {
                    var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val()) * parseFloat($('#' + txtConversionRate).val());
                    if (isNaN(LedgerAmount.toString())) {
                        LedgerAmount = 0;
                    }
                    $('#' + txtCalculatedLedgerAmount).val(LedgerAmount.toFixed(2));
                    $('#' + txtCalculatedLedgerAmountHiddenField).val(LedgerAmount.toFixed(2));
                    $('#' + txtCalculatedLedgerAmount).attr("disabled", true);
                }
            });

            var ddlPayMode = '<%=ddlPayMode.ClientID%>'

            var lblPaymentAccountHead = '<%=lblPaymentAccountHead.ClientID%>'
            $('#' + ddlPayMode).change(function () {
                PaymentModeShowHideInformation();
            });


            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            if ($('#' + ddlPayMode).val() == "Cash") {
                $('#CashPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').show();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Cheque") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $('#CashReceiveAccountsInfo').show();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Adjustment") {
                $('#CashPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }

            ddlGLProject = '<%=ddlGLProject.ClientID%>'
            var ddlGLCompany = '<%=ddlGLCompany.ClientID%>'
            var selectedIndex = parseFloat($('#' + ddlGLCompany).prop("selectedIndex"));
            if (selectedIndex > 0) {
                $("#" + ddlGLProject).attr("disabled", false);
            }
            else {
                $("#" + ddlGLProject).attr("disabled", true);
            }

            var txtCardNumber = '<%=txtCardNumber.ClientID%>'
            $('#' + txtCardNumber).blur(function () {
                //validateCard();
            });
            var ddlCardType = '<%=ddlCardType.ClientID%>'
            $('#' + ddlCardType).change(function () {
                //validateCard();
            });

            $(function () {
                $("#myTabs").tabs();
            });

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtPaymentDate2').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
        });
        //        $(function () {
        //            
        //        });       

        function CurrencyRateInfoEnable() {
            var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
            if (ddlCurrency == 0) {
                $('#ConversionPanel').hide()
            }
        }

        function PaymentModeShowHideInformation() {
            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            var lblPaymentAccountHead = '<%=lblPaymentAccountHead.ClientID%>'

            if ($('#' + ddlPayMode).val() == "Cash") {
                $('#CashPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').show();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Cheque") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $('#CashReceiveAccountsInfo').show();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Adjustment") {
                $('#CashPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
        }
        function PopulateProjects() {
            $("#<%=ddlGLProject.ClientID%>").attr("disabled", "disabled");
            if ($('#<%=ddlGLCompany.ClientID%>').val() == "0") {
                $('#<%=ddlGLProject.ClientID %>').empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
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
        //For ClearForm-------------------------
        function PerformClearActionForButton() {
            if(!confirm("Do you want to clear?"))
            {
                return false;
            }
            PerformClearAction();
        }

        function PerformClearAction() {

            $("#<%=txtLedgerAmount.ClientID %>").val('');
            $("#<%=hfSupplierPaymentId.ClientID %>").val('');
            $("#<%=txtDealId.ClientID %>").val('');
            // $("#<%=ddlCurrency.ClientID %>").val('45');
            $("#<%=ddlCurrency.ClientID %>").val('');
            $("#<%=txtCalculatedLedgerAmount.ClientID %>").val('');
            $("#<%=txtConversionRate.ClientID %>").val('');

            $("#<%=txtCardNumber.ClientID %>").val('');
            $("#<%=ddlCardType.ClientID %>").val('');
            $("#<%=txtCardHolderName.ClientID %>").val('');
            $("#<%=txtExpireDate.ClientID %>").val('');

            $("#<%=txtRemarks.ClientID %>").val('');

            $('#ConversionPanel').hide();
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            return false;
        }
        function EntryPanelVisibleFalse() {
            PerformClearAction();
            return false;
        }
        //Integrated Gl Visible True/False-------------------
        function IntegratedGeneralLedgerDivPanelShow() {
            $('#IntegratedGeneralLedgerDiv').show();
        }
        function IntegratedGeneralLedgerDivPanelHide() {
            $('#IntegratedGeneralLedgerDiv').hide();
        }

        //Integrated Gl Visible True/False-------------------
        function IntegratedGeneralLedgerDivPanelShow() {
            $('#IntegratedGeneralLedgerDiv').show();
        }
        function IntegratedGeneralLedgerDivPanelHide() {
            $('#IntegratedGeneralLedgerDiv').hide();
        }

        //For FillForm-------------------------   
        function PerformFillFormAction(editId) {
            PageMethods.FillForm(editId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {
            $("#<%=hfCurrencyType.ClientID %>").val(result.CurrencyType);
            $("#<%=hfSupplierPaymentId.ClientID %>").val(result.SupplierPaymentId);
            $("#<%=hfSupplierId.ClientID %>").val(result.SupplierId);
            if ($("#<%=hfCurrencyType.ClientID %>").val() != "Local") {
                $('#ConversionPanel').show();
                $("#<%=txtCalculatedLedgerAmount.ClientID %>").val(result.DRAmount);
                $("#<%=txtCalculatedLedgerAmount.ClientID %>").attr("disabled", true);
                $("#<%=txtConversionRate.ClientID %>").val(result.ConvertionRate);
            }
            else {
                $('#ConversionPanel').hide();
                $("#<%=txtCalculatedLedgerAmount.ClientID %>").attr("disabled", false);
                $("#<%=txtConversionRate.ClientID %>").val('');
            }

            $("#<%=ddlCurrency.ClientID %>").val(result.CurrencyId);
            $("#<%=txtLedgerAmount.ClientID %>").val(result.CurrencyAmount);
            $("#<%=txtPaymentDate2.ClientID %>").val(GetStringFromDateTime(result.PaymentDate));
            //$("#<%=txtLedgerAmount.ClientID %>").val(result.CurrencyAmount);
            //$("#<%=txtPaymentId.ClientID %>").val(result.PaymentId);
            //$("#<%=txtDealId.ClientID %>").val(result.DealId);
            //$("#<%=ddlBankId.ClientID %>").val(result.BankId);

            //$('#txtBankId').val($("#<%=ddlBankId.ClientID %> option:selected").text());
            //$('#txtCompanyBank').val($("#<%=ddlCompanyBank.ClientID %> option:selected").text());
            //$("#<%=txtCardNumber.ClientID %>").val(result.CardNumber);
            //$("#<%=ddlCardType.ClientID %>").val(result.CardType);
            //$("#<%=txtCardHolderName.ClientID %>").val(result.CardHolderName);
            //$("#<%=txtExpireDate.ClientID %>").val(GetStringFromDateTime(result.ExpireDate));

            $("#<%=txtRemarks.ClientID %>").val(result.Remarks);
            $("#<%=txtChecqueNumber.ClientID %>").val(result.ChequeNumber);
            if (IsCanUpdate) {
                $('#ContentPlaceHolder1_btnSave').show();
            }
            else {
                $('#ContentPlaceHolder1_btnSave').hide();
            }
            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewBIll').hide();
            $('#EntryPanel').show();

            $("#<%=ddlPayMode.ClientID %>").val(result.PaymentType)
            //$("#<%=txtCardNumber.ClientID %>").val(result.CardNumber)

            $("#<%=ddlCompanyBank.ClientID %>").val(result.AccountsPostingHeadId);
            $('#txtCompanyBank').val($("#<%=ddlCompanyBank.ClientID %> option:selected").text());

            $("#<%=ddlCashPayment.ClientID %>").val(result.AccountsPostingHeadId);
            $('#txtCashPayment').val($("#<%=ddlCashPayment.ClientID %> option:selected").text());

            $("#<%=ddlSupplierName.ClientID %>").val(result.SupplierId);
            $('#txtSupplierName').val($("#<%=ddlSupplierName.ClientID %> option:selected").text());

            if (result.PaymentType == "Cash") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
            }
            else if (result.PaymentType == "Card") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
            }
            else if (result.PaymentType == "Cheque") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
            }
            else if ($('#' + ddlPayMode).val() == "Adjustment") {
                $('#CashReceiveAccountsInfo').show();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
            $("#myTabs").tabs({ active: 0 });
            return false;
        }

        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function LoadPopUp() {
            $("#DivPaymentSelect").dialog({
                width: 600,
                height: 500,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "", //TODO add title
                show: 'slide'
            });
            return false;
        }

        function PaymentPreview() {
            var paymentIdList = "";
            $("#ContentPlaceHolder1_gvPaymentInfo tbody tr").each(function () {
                if ($(this).find("td:eq(0)").find("input").is(":checked")) {
                    var id = $.trim($(this).find("td:eq(1)").text());
                    if (paymentIdList == "") {
                        paymentIdList = id;
                    }
                    else {
                        paymentIdList += ',' + id;
                    }
                }
            });
            if (paymentIdList != "") {
                var url = "/PurchaseManagment/Reports/frmReportSupplierPayment.aspx?PaymentIdList=" + paymentIdList;
                var popup_window = "Preview";
                window.open(url, popup_window, "width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
            }
            else {
                toastr.warning('Select payments to preview');
            }
        }

        function CheckAllCheckBoxCreate() {
            $('.Chk_Select input[type=checkbox]').each(function () {
                $(this).prop("checked", true);
            });
        }

        function UnCheckAllCheckBoxCreate() {
            $('.Chk_Select input[type=checkbox]').each(function () {
                $(this).prop("checked", false);
            });
        }
    </script>
    <style>
        .ChkAllSelect {
            padding-left: 20px;
        }

        .lblHeader {
        }

        .ShowHide {
            display: none;
        }
    </style>
    <asp:HiddenField ID="txtCardValidation" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCurrencyType" runat="server" />
    <asp:HiddenField ID="hfConversionRate" runat="server" />
    <asp:HiddenField ID="hfSupplierId" runat="server" />
    <asp:HiddenField ID="hfSupplierPaymentId" runat="server" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <div id="DivPaymentSelect" style="display: none;">
        <div id="Div1" class="panel panel-default">
            <div class="panel-body">
                <asp:HiddenField ID="HiddenField4" runat="server" />
                <asp:HiddenField ID="HiddenField5" runat="server" />
                <asp:HiddenField ID="txtSelectedRoomNumbers" runat="server" />
                <asp:HiddenField ID="txtSelectedRoomId" runat="server" />
                <asp:HiddenField ID="HiddenFieldCompanyPaymentButtonInfo" runat="server"></asp:HiddenField>
                <asp:GridView ID="gvPaymentInfo" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="100"
                    CssClass="table table-bordered table-condensed table-responsive">
                    <RowStyle BackColor="#E3EAEB" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select" ItemStyle-Width="05%">
                            <HeaderTemplate>
                                <asp:CheckBox ID="ChkAllSelect" CssClass="ChkAllSelect" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ChkSelect" CssClass="Chk_Select" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IDNO" ItemStyle-CssClass="ShowHide" HeaderStyle-CssClass="ShowHide">
                            <ItemTemplate>
                                <asp:Label ID="lblid" runat="server" Text='<%#Eval("SupplierPaymentId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Payment Date" ItemStyle-Width="15%">
                            <ItemTemplate>
                                <asp:Label ID="lblPLI_DATE" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("PaymentDate"))) %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="PaymentType" HeaderText="Payment Type" ItemStyle-Width="20%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CurrencyAmount" HeaderText="Payment Amount" ItemStyle-Width="20%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
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
                <asp:Button ID="btnPaymentPreview" runat="server" Text="Preview" TabIndex="3" CssClass="TransactionalButton btn btn-primary"
                    OnClientClick="javascript: return PaymentPreview();" />
            </div>
        </div>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Bill Payment</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Bill Payment</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Supplier Bill Payment
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group" style="display: none;">
                            <div class="col-md-2">
                                <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblRoomNumber" runat="server" class="control-label" Text="Room Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRoomId" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:HiddenField ID="HiddenField2" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="HiddenField3" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblPaymentDate" runat="server" class="control-label" Text="Payment Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPaymentDate" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div id="CompanyPaymentAccountHeadDiv" style="display: none;">
                                <asp:DropDownList ID="ddlCompanyPaymentAccountHead" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="lblSrcSupplier" runat="server" class="control-label required-field"
                                    Text="Supplier Id"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <div class="col-md-6" style="padding-left: 0">
                                    <asp:TextBox ID="txtSrcSupplierId" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                </div>
                                <div class="col-md-6" style="padding-left: 0">
                                    <asp:Button ID="btnSrcSupplierId" runat="server" Text="Search" TabIndex="2" CssClass="btn btn-primary btn-sm"
                                        OnClick="btnSrcSupplier_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtPaymentId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtDealId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblSupplierName" runat="server" class="control-label" Text="Supplier Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <input id="txtSupplierName" type="text" class="form-control" name="cmpName" />
                                <div style="display: none;">
                                    <asp:DropDownList ID="ddlSupplierName" TabIndex="1" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPayMode" runat="server" class="control-label" Text="Payment Mode"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPayMode" runat="server" CssClass="form-control" TabIndex="5">
                                    <%--<asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="Cash">Cash</asp:ListItem>
                                    <asp:ListItem Value="Card">Card</asp:ListItem>
                                    <asp:ListItem Value="Cheque">Cheque</asp:ListItem>
                                    <asp:ListItem Value="Adjustment">Adjustment</asp:ListItem>
                                    <asp:ListItem Value="Loan">Supplier Loan</asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPaymentDate2" runat="server" class="control-label required-field"
                                    Text="Payment Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPaymentDate2" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div id="IntegratedGeneralLedgerDiv" style="display: none;">
                            <div id="AdvanceDivPanel" style="display: none;">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblPaymentAccountHead" runat="server" class="control-label required-field"
                                            Text="Payment Receive In"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <div id="CashReceiveAccountsInfo">
                                            <asp:DropDownList ID="ddlCashReceiveAccountsInfo" runat="server" CssClass="form-control"
                                                TabIndex="6">
                                            </asp:DropDownList>
                                        </div>
                                        <div id="CardReceiveAccountsInfo" style="display: none;">
                                            <asp:DropDownList ID="ddlCardReceiveAccountsInfo" CssClass="form-control" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                        <div id="ChequeReceiveAccountsInfo" style="display: none;">
                                            <asp:DropDownList ID="ddlChequeReceiveAccountsInfo" CssClass="form-control" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Income Purpose"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlIncomeSourceAccountsInfo" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div id="CashOutDivPanel" style="display: none;">
                                <div class="form-group" style="display: none;">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblPaymentFrom" runat="server" class="control-label required-field"
                                            Text="Payment From"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlPaymentFromAccountsInfo" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblPaymentTo" runat="server" class="control-label required-field"
                                            Text="Payment To"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlPaymentToAccountsInfo" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group" id="CompanyProjectPanel" style="display: none;">
                                <div class="col-md-2">
                                    <asp:Label ID="lblGLCompany" runat="server" class="control-label" Text="Company"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlGLCompany" CssClass="form-control" runat="server" onchange="PopulateProjects();">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblGLProject" runat="server" class="control-label" Text="Project"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlGLProject" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCurrency" runat="server" class="control-label required-field" Text="Currency Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCurrency" TabIndex="6" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                                <%--<asp:Label ID="lblDisplayConvertionRate" runat="server" class="control-label" Text=""></asp:Label>--%>
                                <asp:HiddenField ID="ddlCurrencyHiddenField" runat="server"></asp:HiddenField>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblReceiveAmount" runat="server" class="control-label required-field"
                                    Text="Payment Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtLedgerAmount" runat="server" CssClass="form-control" TabIndex="7"> </asp:TextBox>
                            </div>
                        </div>
                        <div id="ConversionPanel" class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblConversionRate" runat="server" class="control-label required-field"
                                    Text="Conversion Rate"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtConversionRate" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:HiddenField ID="txtConversionRateHiddenField" runat="server"></asp:HiddenField>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblCurrencyAmount" runat="server" class="control-label" Text="Calculated Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:HiddenField ID="txtCalculatedLedgerAmountHiddenField" runat="server"></asp:HiddenField>
                                <asp:TextBox ID="txtCalculatedLedgerAmount" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div id="CashPaymentAccountHeadDiv" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label5" runat="server" class="control-label required-field"
                                        Text="Cash Head"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <input id="txtCashPayment" type="text" class="form-control" />
                                    <div style="display: none;">
                                        <asp:DropDownList ID="ddlCashPayment" runat="server" CssClass="form-control" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="ChecquePaymentAccountHeadDiv" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblChecqueNumber" runat="server" class="control-label required-field"
                                        Text="Cheque Number"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtChecqueNumber" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblCompanyBank" runat="server" class="control-label required-field"
                                        Text="Bank Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <input id="txtCompanyBank" type="text" class="form-control" />
                                    <div style="display: none;">
                                        <asp:DropDownList ID="ddlCompanyBank" CssClass="form-control" runat="server" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="CardPaymentAccountHeadDiv" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Card Type"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlCardType" runat="server" TabIndex="8" CssClass="form-control">
                                        <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                        <asp:ListItem Value="a">American Express</asp:ListItem>
                                        <asp:ListItem Value="m">Master Card</asp:ListItem>
                                        <asp:ListItem Value="v">Visa Card</asp:ListItem>
                                        <asp:ListItem Value="d">Discover Card</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblCardNumber" runat="server" class="control-label" Text="Card Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCardNumber" runat="server" CssClass="form-control" TabIndex="9"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblBankId" runat="server" class="control-label required-field" Text="Bank Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <input id="txtBankId" type="text" class="form-control" />
                                    <div style="display: none;">
                                        <asp:DropDownList ID="ddlBankId" runat="server" CssClass="form-control" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div style="display: none;">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label3" runat="server" class="control-label" Text="Expiry Date"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtExpireDate" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="Label4" runat="server" class="control-label" Text="Card Holder Name"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtCardHolderName" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" CssClass="form-control" TextMode="MultiLine" runat="server"
                                    TabIndex="11"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="12" CssClass="btn btn-primary btn-sm"
                                    OnClick="btnSave_Click" OnClientClick="javascript: return ValidateForm();" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="13" CssClass="btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearActionForButton();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Bill Payment Details
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSearch_Click" />
                                <asp:Button ID="btnGroupPaymentPreview" runat="server" Text="Payment Preview" TabIndex="15"
                                    CssClass="btn btn-primary btn-sm" OnClientClick="javascript: return LoadPopUp();" />
                            </div>
                        </div>
                    </div>
                </div>
                <div id="SearchResult" class="panel panel-default">
                    <div class="panel-heading">
                        Search Information
                    </div>
                    <div class="panel-body">
                        <asp:GridView ID="gvGuestHouseService" Width="100%" runat="server" AllowPaging="True"
                            AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                            ForeColor="#333333" OnRowDataBound="gvGuestHouseService_RowDataBound"
                            OnRowCommand="gvGuestHouseService_RowCommand" PageSize="100"
                            CssClass="table table-bordered table-condensed table-responsive">
                            <RowStyle BackColor="#E3EAEB" />
                            <Columns>
                                <asp:TemplateField HeaderText="IDNO" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblid" runat="server" Text='<%#Eval("SupplierPaymentId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPLI_DATE" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("PaymentDate"))) %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="BillNumber" HeaderText="Invoice No." ItemStyle-Width="15%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PaymentType" HeaderText="Payment Type" ItemStyle-Width="20%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CurrencyName" HeaderText="Currency" ItemStyle-Width="10%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CurrencyAmount" HeaderText="Amount" ItemStyle-Width="15%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgUpdate" OnClientClick="return confirm('Do tou want to edit?');" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                            ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                        <%--<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                            ImageUrl="~/Images/delete.png" Text="" AlternateText="Delete" ToolTip="Delete" />--%>
                                        <asp:ImageButton ID="ImgPaymentPreview" runat="server" CausesValidation="False" CommandArgument='<%# bind("SupplierPaymentId") %>'
                                            CommandName="CmdPaymentPreview" ImageUrl="~/Images/ReportDocument.png" Text=""
                                            AlternateText="Payment Preview" ToolTip="Payment Preview" />
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
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            //            var x = '<%=isSearchPanelEnable%>';
            //            if (x > -1) {
            //                $('#SearchResult').show();
            //            }
            //            else {
            //                $('#SearchResult').hide();
            //            }
        });
    </script>
</asp:Content>
