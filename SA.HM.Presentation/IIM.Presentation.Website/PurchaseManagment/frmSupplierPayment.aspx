<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmSupplierPayment.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.frmSupplierPayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Purchase Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Supplier Transaction</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            //EnableDisable For DropDown Change event--------------

            var ddlCurrency = '<%=ddlCurrency.ClientID%>'
            var txtConversionRate = '<%=txtConversionRate.ClientID%>'
            var txtLedgerAmount = '<%=txtLedgerAmount.ClientID%>'
            var txtCalculatedLedgerAmount = '<%=txtCalculatedLedgerAmount.ClientID%>'
            var txtCalculatedLedgerAmountHiddenField = '<%=txtCalculatedLedgerAmountHiddenField.ClientID%>'
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            if (IsCanSave) {
                $('#ContentPlaceHolder1_btnSave').show();
            } else {
                $('#ContentPlaceHolder1_btnSave').hide();
            }

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_ddlAdjustmentNodeHead").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#chkAll").change(function () {
                if ($(this).is(":checked")) {
                    $("#BillInfo tbody tr").find("td:eq(0)").find("input").prop("checked", true);
                }
                else {
                    $("#BillInfo tbody tr").find("td:eq(0)").find("input").prop("checked", false);
                }
                CalculatePayment();
            });

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
            CommonHelper.AutoSearchClientDataSource("txtCashAndCashEquivalantPayment", "ContentPlaceHolder1_ddlCashAndCashEquivalantPayment", "ContentPlaceHolder1_ddlCashAndCashEquivalantPayment");

            $('#txtSupplierName').blur(function () {
                if ($(this).val() != "") {
                    $("#chkAll").prop("checked", false);
                    var cmpId = $("#ContentPlaceHolder1_ddlSupplierName").val();
                    if (cmpId != "") {
                        //LoadCompanyInfo(cmpId);
                        SearchCompanyBill();
                        $("#CompanyInfo").show();
                    }
                    else {
                        $("#CompanyInfo").hide();
                    }
                }
            });

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

            $('#txtCashAndCashEquivalantPayment').blur(function () {
                if ($(this).val() == "") {
                    $("#<%=ddlCashAndCashEquivalantPayment.ClientID %>").val("0");
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
                    $('#' + txtCalculatedLedgerAmount).val("");
                    $('#ConversionPanel').hide();
                    $('#' + txtCalculatedLedgerAmount).attr("disabled", false);
                }
                else {
                    $("#<%=txtConversionRate.ClientID %>").val(result.ConversionRate);
                    $("#<%=hfConversionRate.ClientID %>").val(result.ConversionRate);

                    $('#ConversionPanel').show();
                }
                CurrencyRateInfoEnable();
                CurrencyConvertion();
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

            var transactionType = '<%=ddlTransactionType.ClientID%>'
            $('#' + transactionType).change(function () {
                if ($('#' + transactionType).val() == "Receive") {
                    $('#AdjustmentHeadDiv').hide();
                    $("#<%=lblAdvanceAmount.ClientID %>").text("Receive Amount");
                }
                else {
                    $('#AdjustmentHeadDiv').show();
                    $("#<%=lblAdvanceAmount.ClientID %>").text("Advance Amount");
                }
            });

            var lblPaymentAccountHead = '<%=lblPaymentAccountHead.ClientID%>'

            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
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
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').show();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Cheque") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $('#CashReceiveAccountsInfo').show();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Adjustment") {
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').show();
                $('#CashPaymentAccountHeadDiv').hide();
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

            var txtFromDate = '<%=txtFromDate.ClientID%>'
            var txtToDate = '<%=txtToDate.ClientID%>'

            $('#' + txtFromDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtToDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtToDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtFromDate).datepicker("option", "maxDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtPaymentDate2').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtChecqueDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);


            $('#ContentPlaceHolder1_txtAdjustmentAmount').blur(function () {
                var paymentTotal = $("#ContentPlaceHolder1_txtTotalAmount").val();
                var adjustmentAmount = $.trim($("#ContentPlaceHolder1_txtAdjustmentAmount").val());

                adjustmentAmount = (adjustmentAmount == "" ? "0" : adjustmentAmount);

                var paymentAmount = parseFloat(paymentTotal) - parseFloat(adjustmentAmount);

                $("#ContentPlaceHolder1_txtLedgerAmount").val(paymentAmount);
            });

        });

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
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').show();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Cheque") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $('#CashReceiveAccountsInfo').show();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Adjustment") {
                $('#CashPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            if ($('#' + ddlPayMode).val() == "Loan") {
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').show();
                $('#CashPaymentAccountHeadDiv').hide();
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

            $("#ContentPlaceHolder1_txtAdjustmentAmount").val(result.CompanyPayment.PaymentAdjustmentAmount);
            $("#ContentPlaceHolder1_ddlAdjustmentNodeHead").val(result.CompanyPayment.AdjustmentAccountHeadId + '').trigger('change');

            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewBIll').hide();
            $('#EntryPanel').show();

            $("#<%=ddlPayMode.ClientID %>").val(result.PaymentType)
            //$("#<%=txtCardNumber.ClientID %>").val(result.CardNumber)

            $("#<%=ddlCompanyBank.ClientID %>").val(result.AccountsPostingHeadId);
            $('#txtCompanyBank').val($("#<%=ddlCompanyBank.ClientID %> option:selected").text());

            $("#<%=ddlCashPayment.ClientID %>").val(result.AccountsPostingHeadId);
            $('#txtCashPayment').val($("#<%=ddlCashPayment.ClientID %> option:selected").text());

            $("#<%=ddlCashAndCashEquivalantPayment.ClientID %>").val(result.AccountsPostingHeadId);
            $('#txtCashAndCashEquivalantPayment').val($("#<%=ddlCashAndCashEquivalantPayment.ClientID %> option:selected").text());

            $("#<%=ddlSupplierName.ClientID %>").val(result.SupplierId);
            $('#txtSupplierName').val($("#<%=ddlSupplierName.ClientID %> option:selected").text());

            if (result.PaymentType == "Cash") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if (result.PaymentType == "Card") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if (result.PaymentType == "Cheque") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Adjustment") {
                $('#CashReceiveAccountsInfo').show();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if (result.PaymentType == "Loan") {
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
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


        ///Bill Receive
        function SearchCompanyBill() {
            CommonHelper.SpinnerOpen();
            var supplierId = $("#ContentPlaceHolder1_ddlSupplierName").val();
            $("#BillInfo tbody").html("");
            PageMethods.SupplierBillBySearch(supplierId, OnLoadCompanyBillSucceeded, OnCompanyBillFailed);
        }

        function OnLoadCompanyBillSucceeded(result) {

            var row = 0, tr = "", chk = "checked='checked'", isChecked = 0;
            var totalPaymentAmount = 0.00;

            for (row = 0; row < result.length; row++) {

                isChecked = result[row].IsBillGenerated == true ? "1" : "0";

                if ((row + 1) % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width: 7%'> ";

                if (result[row].IsBillGenerated) {
                    tr += "<input type='checkbox' id='pay" + result[row].SupplierPaymentId + "'" + chk + " onclick='CheckRow(this)' />";
                    totalPaymentAmount += result[row].DueAmount;
                }
                else {
                    tr += "<input type='checkbox' id='pay" + result[row].SupplierPaymentId + "' onclick='CheckRow(this)' />";
                }

                tr += "</td>";

                tr += "<td style='width: 20%'>" + GetStringFromDateTime(result[row].PaymentDate) + "</td>";
                tr += "<td style='width: 40%'>" + result[row].BillNumber + "</td>";
                tr += "<td style='width: 15%'>" + result[row].DueAmount + "</td>";

                if (result[row].IsBillGenerated) {
                    tr += "<td style='width: 18%'>  <input type='text' class='form-control quantitydecimal' id='p" + result[row].SupplierPaymentId + "' value='" + result[row].DueAmount + "' onblur='CheckPayment(this)' /></td>";
                }
                else {
                    tr += "<td style='width: 18%'>  <input type='text' disabled class='form-control quantitydecimal' id='p" + result[row].SupplierPaymentId + "' value='" + result[row].DueAmount + "' onblur='CheckPayment(this)' /></td>";
                }

                tr += "<td style=display:none;'>" + result[row].SupplierPaymentId + "</td>";
                tr += "<td style=display:none;'>" + result[row].BillId + "</td>";
                tr += "<td style=display:none;'>" + isChecked + "</td>";
                tr += "<td style=display:none;'>" + result[row].PaymentDetailsId + "</td>";

                tr += "</tr>";

                $("#BillInfo tbody").append(tr);
                tr = "";
            }

            $("#ContentPlaceHolder1_txtLedgerAmount").val(totalPaymentAmount);
            CommonHelper.ApplyDecimalValidation();

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnCompanyBillFailed(error) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        function CalculatePayment() {

            var totalPayment = 0.00;

            $("#BillInfo tbody tr").each(function () {

                if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {
                    totalPayment += parseFloat($(this).find("td:eq(4)").find("input").val());
                }
            });

            var paymentAdjustmentAmount = $("#ContentPlaceHolder1_txtAdjustmentAmount").val() != "" ? parseFloat($("#ContentPlaceHolder1_txtAdjustmentAmount").val()) : 0.00;
            var advanceAmount = $("#ContentPlaceHolder1_txtAdvanceAmount").val() != "" ? parseFloat($("#ContentPlaceHolder1_txtAdvanceAmount").val()) : 0.00;

            $("#ContentPlaceHolder1_txtTotalAmount").val(toFixed((totalPayment + advanceAmount), 2));
            $("#ContentPlaceHolder1_txtLedgerAmount").val(toFixed(((totalPayment + advanceAmount) - paymentAdjustmentAmount), 2));

            if ($("#ContentPlaceHolder1_hfPaymentId").val() != 0) {
                $("#ContentPlaceHolder1_ddlCurrency").trigger("change");
            }
        }

        function CheckPayment(control) {

            var tr = $(control).parent().parent();
            var value = parseFloat($(control).val());
            var bdValue = parseFloat($(tr).find("td:eq(3)").text());

            if (value > bdValue) {
                $(control).val(bdValue);
                toastr.warning("Payment Amount Cannot Greater Than Due Amount.");
            }
            else if ($.trim(value) == "" || $.trim(value) == "0") {
                $(control).val(bdValue);
                toastr.warning("Payment Amount Cannot Zero(0) Or Empty.");
            }
            CalculatePayment();
        }

        function CheckAdvance(control) {

            CalculatePayment();

            //var value = parseFloat($(control).val());

            //if ($.trim(value) == "" || $.trim(value) == "0") {
            //    $(control).val(bdValue);
            //    toastr.warning("Advance Amount Cannot Zero(0) Or Empty.");
            //}
        }

        function CheckRow(control) {
            var tr = $(control).parent().parent();

            if ($(control).is(":checked")) {
                $(tr).find("td:eq(4)").find("input").prop("disabled", false);
            }
            else {
                $(tr).find("td:eq(4)").find("input").prop("disabled", true);
            }

            CalculatePayment();
        }

        function ValidateForm() {

            if ($("#ContentPlaceHolder1_txtPaymentDate2").val() == "") {
                toastr.warning("Please Provide Payment Date.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "0") {
                toastr.warning("Please Provide Payment Mode.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cash") {
                if ($("#ContentPlaceHolder1_ddlCashPayment").val() == "0" || $("#txtCashPayment").val() == "") {
                    toastr.warning("Please Select Cash Payment Account.");
                    return false;
                }
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Card") {
                if ($("#ContentPlaceHolder1_ddlCompanyBank").val() == "0" || $("#txtCompanyBank").val() == "") {
                    toastr.warning("Please Select Bank Payment Account.");
                    return false;
                }
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cheque") {
                if ($("#ContentPlaceHolder1_ddlCompanyBank").val() == "0" || $("#txtCompanyBank").val() == "") {
                    toastr.warning("Please Select Bank Payment Account.");
                    return false;
                }
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Loan") {
                if ($("#ContentPlaceHolder1_ddlCashAndCashEquivalantPayment").val() == "0" && $("#txttxtCashAndCashEquivalantPayment").val() == "") {
                    toastr.warning("Please Select Account Head.");
                    return false;
                }
            }

            if ($("#ContentPlaceHolder1_txtAdjustmentAmount").val() != "" && $("#ContentPlaceHolder1_txtAdjustmentAmount").val() != "0") {
                if ($("#ContentPlaceHolder1_ddlAdjustmentNodeHead").val() == "0") {
                    toastr.warning("Please Select Adjustment Account Head.");
                    return false;
                }
            }
            if ($("#ContentPlaceHolder1_txtChecqueDate").val() == "") {
                toastr.warning("Please Add Checque Date.");
                $("#ContentPlaceHolder1_txtChecqueDate").focus();
                return false;
            }

            if ($("#ContentPlaceHolder1_txtRemarks").val() == "") {
                toastr.warning("Please Provide Description.");
                $("#ContentPlaceHolder1_txtRemarks").focus();
                return false;
            }

            CommonHelper.SpinnerOpen();

            var totalPayment = 0.00, advanceAmount = 0.00, detailsId = 0, paymentId = 0, accountingPostingHeadId = 0;
            var adjustmentAccountHeadId = 0, paymentAdjustmentAmount = 0.00;

            paymentId = $("#ContentPlaceHolder1_hfPaymentId").val();
            var SupplierPaymentDetails = new Array();
            var SupplierPaymentDetailsEdited = new Array();
            var SupplierPaymentDetailsDeleted = new Array();

            if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cash") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlCashPayment").val();
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Card") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlCompanyBank").val();
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cheque") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlCompanyBank").val();
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Loan") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlCashAndCashEquivalantPayment").val();
            }

            adjustmentAccountHeadId = $("#ContentPlaceHolder1_ddlAdjustmentNodeHead").val();
            paymentAdjustmentAmount = $("#ContentPlaceHolder1_txtAdjustmentAmount").val() != "" ? parseFloat($("#ContentPlaceHolder1_txtAdjustmentAmount").val()) : 0.00;

            advanceAmount = $("#ContentPlaceHolder1_txtAdvanceAmount").val() == "" ? 0.00 : parseFloat($("#ContentPlaceHolder1_txtAdvanceAmount").val());

            var SupplierPayment = {
                PaymentId: paymentId,
                PaymentFor: $("#ContentPlaceHolder1_ddlTransactionType").val(),
                SupplierId: $("#ContentPlaceHolder1_ddlSupplierName").val(),
                PaymentDate: CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtPaymentDate2").val(), '/'),
                AdvanceAmount: advanceAmount,
                PaymentType: $("#ContentPlaceHolder1_ddlPayMode").val(),
                AccountingPostingHeadId: accountingPostingHeadId,
                Remarks: $("#ContentPlaceHolder1_txtRemarks").val(),
                ChequeNumber: $("#ContentPlaceHolder1_txtChecqueNumber").val(),
                //ChecqueDate: $("#ContentPlaceHolder1_txtChecqueDate").val(),
                ChecqueDate: CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtChecqueDate").val(), '/'),
                CurrencyId: $("#ContentPlaceHolder1_ddlCurrency").val(),
                ConvertionRate: $("#ContentPlaceHolder1_txtConversionRate").val() == "" ? "0" : $("#ContentPlaceHolder1_txtConversionRate").val(),
                AdjustmentAccountHeadId: adjustmentAccountHeadId,
                PaymentAdjustmentAmount: paymentAdjustmentAmount
            };

            $("#BillInfo tbody tr").each(function () {

                detailsId = parseInt($(this).find("td:eq(8)").text(), 10);

                if ($(this).find("td:eq(0)").find("input").is(":checked") == true && detailsId == 0) {
                    SupplierPaymentDetails.push({
                        PaymentDetailsId: 0,
                        SupplierPaymentId: $(this).find("td:eq(5)").text(),
                        BillId: $(this).find("td:eq(6)").text(),
                        PaymentAmount: $(this).find("td:eq(4)").find("input").val()
                    });
                }
                else if ($(this).find("td:eq(0)").find("input").is(":checked") == true && detailsId > 0) {
                    SupplierPaymentDetailsEdited.push({
                        PaymentDetailsId: detailsId,
                        SupplierPaymentId: $(this).find("td:eq(5)").text(),
                        BillId: $(this).find("td:eq(6)").text(),
                        PaymentAmount: $(this).find("td:eq(4)").find("input").val()
                    });
                }
                else if ($(this).find("td:eq(0)").find("input").is(":checked") == false && detailsId > 0) {
                    SupplierPaymentDetailsDeleted.push({
                        PaymentDetailsId: detailsId,
                        SupplierPaymentId: $(this).find("td:eq(5)").text(),
                        BillId: $(this).find("td:eq(6)").text(),
                        PaymentAmount: $(this).find("td:eq(4)").find("input").val()
                    });
                }
            });

            PageMethods.SaveSupplierBillPayment(SupplierPayment, SupplierPaymentDetails, SupplierPaymentDetailsEdited, SupplierPaymentDetailsDeleted, OnSupplierPaymentSucceeded, OnSupplierPaymentFailed);

            return false;
        }
        function OnSupplierPaymentSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            CommonHelper.SpinnerClose();
        }
        function OnSupplierPaymentFailed(result) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchPayment(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function SearchPayment(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#BillInfoSearch tbody tr").length;
            var dateFrom = null, dateTo = null, supplierId = 0;

            if ($("#ContentPlaceHolder1_txtFromDate").val() != "") {
                dateFrom = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtFromDate").val(), '/')
            }
            if ($("#ContentPlaceHolder1_txtToDate").val() != "") {
                dateTo = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtToDate").val(), '/')
            }

            var transactionType = $("#ContentPlaceHolder1_ddlSrcTransactionType").val();
            supplierId = $("#ContentPlaceHolder1_ddlSupplier").val();

            $("#GridPagingContainer ul").html("");
            $("#BillInfoSearch tbody").html("");
            debugger;
            PageMethods.GetSupplierPaymentBySearch(transactionType, supplierId, dateFrom, dateTo, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchPaymentSucceeded, OnSearchPaymentFailed);

            return false;
        }

        function ShowReport(paymentId) {
            GridPagingContainer
            var iframeid = 'printDoc';
            var url = "Reports/frmSupplierPamenyInvoice.aspx?PId=" + paymentId;
            parent.document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 800,
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Supplier Invoice",
                show: 'slide'
            });
        }

        function OnSearchPaymentSucceeded(result) {
            //var pagination = result;
            //result = result.GridData;
            var tr = "";

            $.each(result.GridData, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style='width:10%;'>" + gridObject.LedgerNumber + "</td>";
                if (gridObject.PaymentDate != null)
                    tr += "<td style='width:10%;'>" + CommonHelper.DateFromDateTimeToDisplay(gridObject.PaymentDate, innBoarDateFormat) + "</td>";
                else
                    tr += "<td style='width:10%;'></td>";

                tr += "<td style='width:10%;'>" + gridObject.PaymentFor + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.SupplierName + "</td>";

                if (gridObject.Remarks != null)
                    tr += "<td style='width:30%;'>" + gridObject.Remarks + "</td>";
                else
                    tr += "<td style='width:30%;'></td>";
                tr += "<td style='width:10%;'>" + (gridObject.ApprovedStatus == 'Pending' ? 'Submitted' : gridObject.ApprovedStatus) + "</td>";
                tr += "<td style=\"text-align: center; width:10%; cursor:pointer;\">";
                if (gridObject.IsCanEdit && IsCanEdit) {
                    tr += "<a onclick=\"javascript:return FIllForEditWithConfirmation(" + gridObject.PaymentId + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                }

                if (gridObject.IsCanDelete && IsCanDelete) {
                    tr += "<a href='javascript:void();' onclick= 'javascript:return DeleteItemReceivedDetails(" + gridObject.PaymentId + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                }

                if (gridObject.IsCanChecked && IsCanSave) {
                    tr += "<a href='javascript:void();' onclick= 'javascript:return CheckedPaymentWithConfirmation(" + gridObject.PaymentId + ")' ><img alt='Checked' src='../Images/checked.png' /></a>";
                }

                if (gridObject.IsCanApproved && IsCanSave) {
                    tr += "<a href='javascript:void();' onclick= 'javascript:return ApprovedPaymentWithConfirmation(" + gridObject.PaymentId + ")' ><img alt='approved' src='../Images/approved.png' /></a>";
                }

                tr += "&nbsp;&nbsp;";

                tr += "<a href='javascript:void();' onclick= 'javascript:return ShowReport(" + gridObject.PaymentId + ")' ><img alt='approved' src='../Images/ReportDocument.png' /></a>";

                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.PaymentId + "</td>";

                tr += "</tr>";

                $("#BillInfoSearch tbody").append(tr);
                tr = "";
            });
            $("#GridPagingContainer ul").append(pagination.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(pagination.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(pagination.GridPageLinks.NextButton);
        }
        function OnSearchPaymentFailed() { }

        function FIllForEdit(paymentId) {
            $("#BillInfo tbody").html("");
            $("#ContentPlaceHolder1_hfPaymentId").val("0");
            PageMethods.FillForm(paymentId, OnFillFormSucceed, OnFillFormFailed);
            return false;
        }
        function FIllForEditWithConfirmation(paymentId) {
            if (!confirm("Do you Want To Edit?")) {
                return false;
            }
            FIllForEdit(paymentId);
        }
        function OnFillFormSucceed(result) {
            $("#ContentPlaceHolder1_ddlTransactionType").val(result.SupplierPayment.PaymentFor).trigger('change');
            $("#ContentPlaceHolder1_hfPaymentId").val(result.SupplierPayment.PaymentId);
            $("#ContentPlaceHolder1_ddlSupplierName").val(result.SupplierPayment.SupplierId + ""),
                $("#ContentPlaceHolder1_txtRemarks").val(result.SupplierPayment.Remarks);
            $("#ContentPlaceHolder1_txtAdvanceAmount").val(result.SupplierPayment.AdvanceAmount);
            $("#txtSupplierName").val($("#ContentPlaceHolder1_ddlSupplierName option:selected").text());
            $("#ContentPlaceHolder1_txtPaymentDate2").val(GetStringFromDateTime(result.SupplierPayment.PaymentDate));
            $("#ContentPlaceHolder1_txtChecqueDate").val(GetStringFromDateTime(result.SupplierPayment.ChecqueDate));

            $("#ContentPlaceHolder1_txtAdjustmentAmount").val(result.SupplierPayment.PaymentAdjustmentAmount);
            $("#ContentPlaceHolder1_ddlAdjustmentNodeHead").val(result.SupplierPayment.AdjustmentAccountHeadId + '').trigger('change');

            if (result.SupplierPayment.PaymentType == "Cash") {
                $("#ContentPlaceHolder1_ddlCashPayment").val(result.SupplierPayment.AccountingPostingHeadId + "");
                $("#txtCashPayment").val($("#ContentPlaceHolder1_ddlCashPayment option:selected").text());
            }
            else if (result.SupplierPayment.PaymentType == "Card") {
                $("#ContentPlaceHolder1_ddlCompanyBank").val(result.SupplierPayment.AccountingPostingHeadId + "");
                $("#txtCompanyBank").val($("#ContentPlaceHolder1_ddlCompanyBank option:selected").text());
            }
            else if (result.SupplierPayment.PaymentType == "Cheque") {
                $("#ContentPlaceHolder1_ddlCompanyBank").val(result.SupplierPayment.AccountingPostingHeadId + "");
                $("#txtCompanyBank").val($("#ContentPlaceHolder1_ddlCompanyBank option:selected").text());
            }
            else if ($('#ContentPlaceHolder1_ddlPayMode').val() == "Adjustment") {

            }

            //------------ Details

            var row = 0, tr = "", chk = "checked='checked'", isChecked = 0;
            var totalPaymentAmount = 0.00;

            for (row = 0; row < result.SupplierBill.length; row++) {

                var pd = _.findWhere(result.PaymentDetailsInfo, { SupplierPaymentId: result.SupplierBill[row].SupplierPaymentId });

                isChecked = result.SupplierBill[row].IsBillGenerated == true ? "1" : "0";

                if (pd != null) {
                    isChecked = "1";
                }

                if ((row + 1) % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width: 10%'> ";

                if (isChecked == "1") {
                    tr += "<input type='checkbox' id='pay" + result.SupplierBill[row].SupplierPaymentId + "'" + chk + " onclick='CheckRow(this)' />";
                    totalPaymentAmount += result.SupplierBill[row].DueAmount;
                }
                else {
                    tr += "<input type='checkbox' id='pay" + result.SupplierBill[row].SupplierPaymentId + "' onclick='CheckRow(this)' />";
                }

                tr += "</td>";

                tr += "<td style='width: 20%'>" + GetStringFromDateTime(result.SupplierBill[row].PaymentDate) + "</td>";
                tr += "<td style='width: 40%'>" + result.SupplierBill[row].BillNumber + "</td>";
                tr += "<td style='width: 15%'>" + result.SupplierBill[row].DueAmount + "</td>";

                if (isChecked == "1") {
                    tr += "<td style='width: 18%'>  <input type='text' class='form-control quantitydecimal' id='p" + result.SupplierBill[row].SupplierPaymentId + "' value='" + result.SupplierBill[row].DueAmount + "' onblur='CheckPayment(this)' /></td>";
                }
                else {
                    tr += "<td style='width: 18%'>  <input type='text' disabled class='form-control quantitydecimal' id='p" + result.SupplierBill[row].SupplierPaymentId + "' value='" + result.SupplierBill[row].DueAmount + "' onblur='CheckPayment(this)' /></td>";
                }

                tr += "<td style=display:none;'>" + result.SupplierBill[row].SupplierPaymentId + "</td>";
                tr += "<td style=display:none;'>" + result.SupplierBill[row].BillId + "</td>";
                tr += "<td style=display:none;'>" + isChecked + "</td>";

                if (pd != null)
                    tr += "<td style=display:none;'>" + pd.PaymentDetailsId + "</td>";
                else
                    tr += "<td style=display:none;'>0</td>";

                tr += "</tr>";

                $("#BillInfo tbody").append(tr);
                tr = "";
                isChecked = "0";
            }

            $("#ContentPlaceHolder1_txtLedgerAmount").val(totalPaymentAmount);
            CommonHelper.ApplyDecimalValidation();

            if (IsCanEdit) {
                $('#ContentPlaceHolder1_btnSave').show();
            } else {
                $('#ContentPlaceHolder1_btnSave').hide();
            }

            $("#ContentPlaceHolder1_btnSave").val("Update");

            //-------------Previous FIll
            $("#<%=hfCurrencyType.ClientID %>").val(result.SupplierPayment.CurrencyType);
            $("#<%=hfSupplierPaymentId.ClientID %>").val(result.SupplierPayment.SupplierPaymentId);
            $("#<%=hfSupplierId.ClientID %>").val(result.SupplierPayment.SupplierId);
            if ($("#<%=hfCurrencyType.ClientID %>").val() != "Local") {
                $('#ConversionPanel').show();
                $("#<%=txtCalculatedLedgerAmount.ClientID %>").val(result.SupplierPayment.DRAmount);
                $("#<%=txtCalculatedLedgerAmount.ClientID %>").attr("disabled", true);
                $("#<%=txtConversionRate.ClientID %>").val(result.SupplierPayment.ConvertionRate);
            }
            else {
                $('#ConversionPanel').hide();
                $("#<%=txtCalculatedLedgerAmount.ClientID %>").attr("disabled", false);
                $("#<%=txtConversionRate.ClientID %>").val('');
            }

            $("#<%=ddlCurrency.ClientID %>").val(result.SupplierPayment.CurrencyId);
            $("#<%=txtChecqueNumber.ClientID %>").val(result.SupplierPayment.ChequeNumber);

            $('#btnNewBIll').hide();
            $('#EntryPanel').show();

            $("#ContentPlaceHolder1_ddlPayMode").val(result.SupplierPayment.PaymentType);

            $("#<%=ddlSupplierName.ClientID %>").val(result.SupplierPayment.SupplierId);
            $('#txtSupplierName').val($("#<%=ddlSupplierName.ClientID %> option:selected").text());

            if (result.SupplierPayment.PaymentType == "Cash") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if (result.SupplierPayment.PaymentType == "Card") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if (result.SupplierPayment.PaymentType == "Cheque") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if ($('#ContentPlaceHolder1_ddlPayMode').val() == "Adjustment") {
                $('#CashReceiveAccountsInfo').show();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if (result.SupplierPayment.PaymentType == "Loan") {
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
            }

            CalculatePayment();
            $("#myTabs").tabs({ active: 0 });
        }
        function OnFillFormFailed(error) {
            toastr.error(error.get_message());
        }

        function DeleteItemReceivedDetails(paymentId) {
            if (!confirm("Do you Want To Delete?")) {
                return false;
            }
            DeletePaymentInfo(paymentId);
        }
        function DeletePaymentInfo(paymentId) {
            PageMethods.DeletePaymentInfo(paymentId, OnPaymentDeleteSucceed, OnPaymentDeleteFailed);
            return false;
        }
        function OnPaymentDeleteSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchPayment(1, 1);
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnPaymentDeleteFailed(error) {
            toastr.error(error.get_message());
        }

        function CheckedPayment(paymentId) {
            PageMethods.CheckedPayment(paymentId, OnCheckedSucceed, OnCheckedFailed);
            return false;
        }
        function CheckedPaymentWithConfirmation(paymentId) {
            if (!confirm("Do you Want To Check ?")) {
                return false;
            }
            CheckedPayment(paymentId);
        }
        function OnCheckedSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchPayment(1, 1);
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnCheckedFailed(error) {
            toastr.error(error.get_message());
        }

        function ApprovedPayment(paymentId) {
            PageMethods.ApprovedPayment(paymentId, OnApporavalSucceed, OnApporavalFailed);
            return false;
        }
        function ApprovedPaymentWithConfirmation(paymentId) {
            if (!confirm("Do you Want To Approve ?")) {
                return false;
            }
            ApprovedPayment(paymentId);
        }
        function OnApporavalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchPayment(1, 1);
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnApporavalFailed(error) {
            toastr.error(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {

            $("#BillInfo tbody").html("");
            $("#ContentPlaceHolder1_hfPaymentId").val("0");

            $("#<%=txtLedgerAmount.ClientID %>").val('');
            $("#<%=hfSupplierPaymentId.ClientID %>").val('');
            $("#<%=txtDealId.ClientID %>").val('');
            // $("#<%=ddlCurrency.ClientID %>").val('45');
            $("#<%=ddlCurrency.ClientID %>").val('1');
            $("#<%=txtCalculatedLedgerAmount.ClientID %>").val('');
            $("#<%=txtConversionRate.ClientID %>").val('');

            $("#<%=txtCardNumber.ClientID %>").val('');
            $("#<%=ddlCardType.ClientID %>").val('');
            $("#<%=txtCardHolderName.ClientID %>").val('');
            $("#<%=txtExpireDate.ClientID %>").val('');

            $("#<%=txtRemarks.ClientID %>").val('');
            $("#ContentPlaceHolder1_txtAdvanceAmount").val("");

            $("#ContentPlaceHolder1_ddlCashPayment").val();
            $("#ContentPlaceHolder1_ddlCompanyBank").val();
            $("#txtCashPayment").val("");
            $("#txtCompanyBank").val("")
            $("#ContentPlaceHolder1_txtChecqueNumber").val("");
            $("#txtSupplierName").val("");

            $("#ContentPlaceHolder1_txtTotalAmount").val("");
            $("#ContentPlaceHolder1_txtAdjustmentAmount").val("");
            $("#ContentPlaceHolder1_ddlAdjustmentNodeHead").val('0').trigger('change');
            $("#chkAll").prop("checked", false);
            $('#ConversionPanel').hide();
            $("#<%=btnSave.ClientID %>").val("Save");

            $("#ContentPlaceHolder1_ddlTransactionType").val('Payment').trigger('change');
            return false;
        }
        function PerformClearActionWithConfirmation() {
            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            PerformClearAction();
        }
        function CurrencyConvertion() {
            var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
            if (currencyType == "Local") {
                var LedgerAmount = parseFloat($('#ContentPlaceHolder1_txtLedgerAmount').val());

                $('#ContentPlaceHolder1_txtCalculatedLedgerAmount').val(LedgerAmount.toFixed(2));
            }
            else {
                var LedgerAmount = parseFloat($('#ContentPlaceHolder1_txtLedgerAmount').val()) * parseFloat($('#ContentPlaceHolder1_txtConversionRate').val());
                if (isNaN(LedgerAmount.toString())) {
                    LedgerAmount = 0;
                }
                $('#ContentPlaceHolder1_txtCalculatedLedgerAmount').val(LedgerAmount.toFixed(2));
                $('#ContentPlaceHolder1_txtCalculatedLedgerAmount').val(LedgerAmount.toFixed(2));
                $('#ContentPlaceHolder1_txtCalculatedLedgerAmount').attr("disabled", true);
            }
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
    <asp:HiddenField ID="hfPaymentId" runat="server" Value="0" />

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
                href="#tab-1">Transaction Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Transaction</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Supplier Transaction
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
                                <asp:Label ID="Label7" runat="server" class="control-label" Text="Transaction Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <div>
                                    <asp:DropDownList ID="ddlTransactionType" TabIndex="1" CssClass="form-control" runat="server">
                                        <asp:ListItem>Payment</asp:ListItem>
                                        <asp:ListItem>Receive</asp:ListItem>
                                    </asp:DropDownList>
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
                        <div class="form-group" id="BillInfoDiv" runat="server">
                            <div class="col-md-12">
                                <table id="BillInfo" class="table table-bordered table-condensed table-hover table-responsive">
                                    <thead>
                                        <tr>
                                            <th style="width: 7%; text-align: center;">Select<br />
                                                <input type="checkbox" id="chkAll" />
                                            </th>
                                            <th style="width: 20%;">Bill Date</th>
                                            <th style="width: 40%;">Bill Number</th>
                                            <th style="width: 15%;">Due Amount</th>
                                            <th style="width: 18%;">Amount</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                    <%--<tfoot>
                                        <tr>
                                            <td colspan="4" style="width: 82%; text-align: right;">Advance Amount</td>
                                            <td style="width: 18%">
                                                <asp:TextBox ID="txtAdvanceAmount" runat="server" onblur="CheckAdvance(this)" CssClass="form-control quantitydecimal" TabIndex="7"> </asp:TextBox>
                                            </td>
                                        </tr>
                                    </tfoot>--%>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">Transaction Details</div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblAdvanceAmount" runat="server" class="control-label"
                                        Text="Advance Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtAdvanceAmount" runat="server" onblur="CheckAdvance(this)" CssClass="form-control quantitydecimal" TabIndex="7"> </asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="Label13" runat="server" class="control-label required-field"
                                        Text="Total Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtTotalAmount" disabled CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" id="AdjustmentHeadDiv">
                                <div class="col-md-2">
                                    <asp:Label ID="Label10" runat="server" class="control-label" Text="Adjustment Head"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlAdjustmentNodeHead" runat="server" CssClass="form-control" TabIndex="5">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="Label11" runat="server" class="control-label"
                                        Text="Adjustment Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtAdjustmentAmount" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblPayMode" runat="server" class="control-label required-field" Text="Payment Mode"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlPayMode" runat="server" CssClass="form-control" TabIndex="5">
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
                                    <asp:HiddenField ID="ddlCurrencyHiddenField" runat="server"></asp:HiddenField>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblReceiveAmount" runat="server" class="control-label required-field"
                                        Text="Payment Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtLedgerAmount" disabled runat="server" CssClass="form-control" TabIndex="7"> </asp:TextBox>
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
                            <div id="CashAndCashEquivalantPaymentAccountHeadDiv" style="display: none;">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label6" runat="server" class="control-label required-field"
                                            Text="Account Head"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <input id="txtCashAndCashEquivalantPayment" type="text" class="form-control" />
                                        <div style="display: none;">
                                            <asp:DropDownList ID="ddlCashAndCashEquivalantPayment" runat="server" CssClass="form-control" AutoPostBack="false">
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
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtChecqueNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblChecqueDate" runat="server" class="control-label"
                                            Text="Cheque Date"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtChecqueDate" runat="server" CssClass="form-control"></asp:TextBox>
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
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblRemarks" runat="server" class="control-label required-field" Text="Description"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtRemarks" CssClass="form-control" TextMode="MultiLine" runat="server"
                            TabIndex="11"></asp:TextBox>
                    </div>
                </div>
                <div class="row" style="padding-bottom: 5px;">
                    <div class="col-md-12">
                        <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="12" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClick="btnSave_Click" OnClientClick="javascript: return ValidateForm();" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="13" CssClass="btn btn-primary btn-sm"
                            OnClientClick="javascript: return PerformClearActionWithConfirmation();" />
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Transaction Details
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSupplier" runat="server" class="control-label" Text="Supplier"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
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
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label8" runat="server" class="control-label" Text="Transaction Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <div>
                                    <asp:DropDownList ID="ddlSrcTransactionType" TabIndex="1" CssClass="form-control" runat="server">
                                        <asp:ListItem Value="0">--- All ---</asp:ListItem>
                                        <asp:ListItem>Payment</asp:ListItem>
                                        <asp:ListItem>Receive</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSearch_Click" OnClientClick="javascript: return SearchPayment(1, 1)" />
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
                        <table id="BillInfoSearch" class="table table-bordered table-condensed table-hover table-responsive">
                            <thead>
                                <tr>
                                    <th style="width: 10%;">Transaction No.</th>
                                    <th style="width: 10%;">Transaction Date</th>
                                    <th style="width: 10%;">Transaction Type</th>
                                    <th style="width: 15%;">Supplier Name</th>
                                    <th style="width: 30%;">Description</th>
                                    <th style="width: 10%;">Status</th>
                                    <th style="width: 10%;">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
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
                                        <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                            ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                        <%--<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                            ImageUrl="~/Images/delete.png" Text="" AlternateText="Delete" ToolTip="Delete" />--%>
                                        <asp:ImageButton ID="ImgPaymentPreview" runat="server" CausesValidation="False" CommandArgument='<%# Bind("SupplierPaymentId") %>'
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
                    <div class="childDivSection">
                        <div class="text-center" id="GridPagingContainer">
                            <ul class="pagination">
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="displayBill" style="display: none;">
        <iframe id="printDoc" name="printDoc" width="1000" height="800" frameborder="0" style="overflow: hidden;"></iframe>
        <div id="bottomPrint">
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
