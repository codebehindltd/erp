<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="true"
    CodeBehind="frmEmployeeBillReceive.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmployeeBillReceive" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var EmployeeGeneratedBill = new Array();
        var hht = "";

        $(function () {
            var ddlCurrency = '<%=ddlCurrency.ClientID%>'
            var txtConversionRate = '<%=txtConversionRate.ClientID%>'
            var txtLedgerAmount = '<%=txtLedgerAmount.ClientID%>'
            var txtCalculatedLedgerAmount = '<%=txtCalculatedLedgerAmount.ClientID%>'
            var txtCalculatedLedgerAmountHiddenField = '<%=txtCalculatedLedgerAmountHiddenField.ClientID%>'

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_ddlEmployeeForSearch").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $('#ContentPlaceHolder1_gvPaymentInfo_ChkAllSelect').click(function () {
                if ($('#ContentPlaceHolder1_gvPaymentInfo_ChkAllSelect').is(':checked')) {
                    CheckAllCheckBoxCreate()
                }
                else {
                    UnCheckAllCheckBoxCreate();
                }
            });

            CommonHelper.ApplyDecimalValidation();

            CommonHelper.AutoSearchClientDataSource("txtBankId", "ContentPlaceHolder1_ddlBankId", "ContentPlaceHolder1_ddlBankId");
            CommonHelper.AutoSearchClientDataSource("txtCompanyBank", "ContentPlaceHolder1_ddlCompanyBank", "ContentPlaceHolder1_ddlCompanyBank");
            CommonHelper.AutoSearchClientDataSource("txtBankPayment", "ContentPlaceHolder1_ddlBankPayment", "ContentPlaceHolder1_ddlBankPayment");
            CommonHelper.AutoSearchClientDataSource("txtCashPayment", "ContentPlaceHolder1_ddlCashPayment", "ContentPlaceHolder1_ddlCashPayment");

            $('#txtCompanySearch').blur(function () {
                if ($(this).val() != "") {
                    var cmpId = $("#ContentPlaceHolder1_hfEmployeeId").val();
                    if (cmpId != "") {
                        LoadCompanyInfo(cmpId);
                        LoadEmployeeBill();
                        $("#CompanyInfo").show();
                    }
                    else {
                        $("#CompanyInfo").hide();
                    }
                }
            });

            var transactionType = '<%=ddlTransactionType.ClientID%>'
            $('#' + transactionType).change(function () {
                if ($('#' + transactionType).val() == "Receive") {
                    $('#EmployeeBillDiv').show();
                    $('#EmployeeBillDetailDiv').show();
                    $("#<%=lblAdvanceAmount.ClientID %>").text("Receive Amount");
                }
                else {
                    $('#EmployeeBillDiv').hide();
                    $('#EmployeeBillDetailDiv').hide();
                    $("#<%=lblAdvanceAmount.ClientID %>").text("Advance Amount");
                }
            });

            $("#txtEmployeeName").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '/Common/WebMethodPage.aspx/SearchEmployeeByName',
                        data: "{'employeeName':'" + request.term + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.EmployeeName,
                                    value: m.EmpId,
                                    EmployeeName: m.EmployeeName,
                                    Department: m.Department,
                                    Designation: m.Designation,
                                    GradeName: m.GradeName,
                                    PresentPhone: m.PresentPhone
                                };
                            });
                            response(searchData);
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                },
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#CompanyInfo").show();

                    $("#ContentPlaceHolder1_hfEmployeeId").val(ui.item.value);
                    $("#ContentPlaceHolder1_txtDepartment").val(ui.item.Department);
                    $("#ContentPlaceHolder1_txtDesignation").val(ui.item.Designation);
                    $("#ContentPlaceHolder1_txtGrade").val(ui.item.GradeName);
                    $("#ContentPlaceHolder1_txtPresentPhone").val(ui.item.PresentPhone);

                    LoadEmployeeBill();
                }
            });

            $('#txtBankPayment').blur(function () {
                if ($(this).val() == "") {
                    $("#<%=ddlBankPayment.ClientID %>").val("0");
            }
            });

            $('#txtCashPayment').blur(function () {
                if ($(this).val() == "") {
                    $("#<%=ddlCashPayment.ClientID %>").val("0");
                }
            });

            var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
            if (currencyType == "Local") {
                $('#' + txtConversionRate).val("")
                $('#' + txtCalculatedLedgerAmount).val("");
                $('#ConversionPanel').hide();
                $('#' + txtCalculatedLedgerAmount).attr("disabled", false);
            }
            else {
                $('#ConversionPanel').show();
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
                    $('#' + txtCalculatedLedgerAmount).val("");
                }
                CurrencyRateInfoEnable();
                CurrencyConvertion();
            }

            function OnLoadConversionRateFailed() {
            }

            $('#' + txtLedgerAmount).blur(function () {
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

            var ddlPayMode = '<%=ddlPayMode.ClientID%>'

            var lblPaymentAccountHead = '<%=lblPaymentAccountHead.ClientID%>'
            $('#' + ddlPayMode).change(function () {
                PaymentModeShowHideInformation();
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
            }).datepicker("setDate", DayOpenDate);;

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);;
            $('#ContentPlaceHolder1_txtPaymentDate2').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);;

            $("#ContentPlaceHolder1_ddlEmployeeBill").change(function () {

                var employeeBillId = $(this).val();

                var bill = _.findWhere(EmployeeGeneratedBill, { EmployeeBillId: parseInt(employeeBillId, 10) });
                if (bill != null) {
                    $("#ContentPlaceHolder1_ddlCurrency").val(bill.BillCurrencyId);
                    $("#ContentPlaceHolder1_ddlCurrency").trigger("change");
                }

                SearchEmployeeBill();
            });

            $('#ContentPlaceHolder1_txtAdjustmentAmount').blur(function () {
                var paymentTotal = $("#ContentPlaceHolder1_txtTotalAmount").val();
                var adjustmentAmount = $("#ContentPlaceHolder1_txtAdjustmentAmount").val();
                var paymentAmount = parseFloat(paymentTotal) - parseFloat(adjustmentAmount);

                $("#ContentPlaceHolder1_txtLedgerAmount").val(paymentAmount);
            });

        });

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Membership</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Member Bill Payment</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            //EnableDisable For DropDown Change event--------------

            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            if ($('#' + ddlPayMode).val() == "Cash") {
                $('#CashPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').show();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Cheque") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Bank") {
                $('#BankPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Adjustment") {
                $('#CashPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
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
        });

        function CurrencyRateInfoEnable() {
            var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
            if (ddlCurrency == 0) {
                $('#ConversionPanel').hide()
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
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').show();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Cheque") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Bank") {
                $('#BankPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $('#CashReceiveAccountsInfo').show();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Adjustment") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
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
        //For FillForm-------------------------   
        function FIllForEdit(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {
            $("#ContentPlaceHolder1_ddlTransactionType").val(result.EmployeePayment.PaymentFor).trigger('change');
            $("#BillInfo tbody").html("");
            $("#ContentPlaceHolder1_hfPaymentId").val(result.EmployeePayment.PaymentId);
            $("#ContentPlaceHolder1_hfEmployeeBillId").val(result.EmployeePayment.EmployeeBillId + "");
            $("#ContentPlaceHolder1_hfEmployeeId").val(result.EmployeePayment.EmployeeId);

            LoadEmployeeBill();

            $("#<%=hfCurrencyType.ClientID %>").val(result.EmployeePayment.CurrencyType);
            $("#<%=hfEmployeePaymentId.ClientID %>").val(result.EmployeePayment.PaymentId);
            $("#CompanyInfo").show();

            $("#txtEmployeeName").val(result.Employee.EmployeeName);
            $("#ContentPlaceHolder1_txtDepartment").val(result.Employee.Department);
            $("#ContentPlaceHolder1_txtDesignation").val(result.Employee.Designation);
            $("#ContentPlaceHolder1_txtGrade").val(result.Employee.GradeName);
            $("#ContentPlaceHolder1_txtPresentPhone").val(result.Employee.PresentPhone);

            $("#<%=ddlCurrency.ClientID %>").val(result.EmployeePayment.CurrencyId);
            $("#<%=txtLedgerAmount.ClientID %>").val(result.EmployeePayment.CurrencyAmount);
            $("#<%=txtPaymentDate2.ClientID %>").val(GetStringFromDateTime(result.EmployeePayment.PaymentDate));

            $("#<%=ddlBankPayment.ClientID %>").val(result.EmployeePayment.AccountsPostingHeadId);
            $('#txtBankPayment').val($("#<%=ddlBankPayment.ClientID %> option:selected").text());

            $("#<%=ddlCashPayment.ClientID %>").val(result.EmployeePayment.AccountsPostingHeadId);
            $('#txtCashPayment').val($("#<%=ddlCashPayment.ClientID %> option:selected").text());

            $("#ContentPlaceHolder1_txtAdjustmentAmount").val(result.EmployeePayment.PaymentAdjustmentAmount);
            $("#ContentPlaceHolder1_ddlAdjustmentNodeHead").val(result.EmployeePayment.AdjustmentAccountHeadId + '').trigger('change');

            $("#<%=txtRemarks.ClientID %>").val(result.EmployeePayment.Remarks);

            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewBIll').hide();
            $('#EntryPanel').show();

            if (result.EmployeePayment.PaymentType == "Cash") {
                $("#ContentPlaceHolder1_ddlCashPayment").val(result.EmployeePayment.AccountingPostingHeadId + "");
                $("#txtCashPayment").val($("#ContentPlaceHolder1_ddlCashPayment option:selected").text());
            }
            else if (result.EmployeePayment.PaymentType == "Bank") {
                $("#ContentPlaceHolder1_ddlBankPayment").val(result.EmployeePayment.AccountingPostingHeadId + "");
                $("#txtCompanyBank").val($("#ContentPlaceHolder1_ddlBankPayment option:selected").text());
            }
            else if (result.EmployeePayment.PaymentType == "Cheque") {
                $("#ContentPlaceHolder1_ddlCompanyBank").val(result.EmployeePayment.AccountingPostingHeadId + "");
                $("#txtCompanyBank").val($("#ContentPlaceHolder1_ddlCompanyBank option:selected").text());
            }
            else if ($('#ContentPlaceHolder1_ddlPayMode').val() == "Adjustment") {

            }

            $("#<%=ddlPayMode.ClientID %>").val(result.EmployeePayment.PaymentType)

            if (result.EmployeePayment.PaymentType == "Cash") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
            }
            else if (result.EmployeePayment.PaymentType == "Bank") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
            }
            else if (result.EmployeePayment.PaymentType == "Cheque") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
            }
            else if (result.EmployeePayment.PaymentType == "Bank") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').show();
                IntegratedGeneralLedgerDivPanelHide();
            }
            else if (result.EmployeePayment.PaymentType == "Adjustment") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
            }

            $("#ContentPlaceHolder1_txtAdvanceAmount").val(result.EmployeePayment.AdvanceAmount);

            var row = 0, tr = "", chk = "checked='checked'", isChecked = "0";
            var totalPaymentAmount = 0.00;

            if (result.EmployeePaymentDetails.length > 0) {

                for (row = 0; row < result.EmployeePaymentDetails.length; row++) {

                    isChecked = result.EmployeePaymentDetails[row].PaymentDetailsId > 0 ? "1" : "0";

                    if ((row + 1) % 2 == 0) {
                        tr += "<tr style=\"background-color:#FFFFFF;\">";
                    }
                    else {
                        tr += "<tr style=\"background-color:#E3EAEB;\">";
                    }

                    tr += "<td style=\"width: 7%\"> ";

                    if (isChecked == "1") {
                        tr += "<input type=\"checkbox\" id=\"pay" + result.EmployeePaymentDetails[row].EmployeePaymentId + "\" checked=\"checked\" onclick=\"CheckRow(this)\" />";
                        totalPaymentAmount += result.EmployeePaymentDetails[row].DueAmount;
                    }
                    else {
                        tr += "<input type=\"checkbox\" id=\"pay" + result.EmployeePaymentDetails[row].EmployeePaymentId + "\" onclick=\"CheckRow(this)\" />";
                    }

                    tr += "</td>";

                    tr += "<td style=\"width: 20%\">" + result.EmployeePaymentDetails[row].ModuleName + "</td>";
                    tr += "<td style=\"width: 20%\">" + GetStringFromDateTime(result.EmployeePaymentDetails[row].PaymentDate) + "</td>";
                    tr += "<td style=\"width: 20%\">" + result.EmployeePaymentDetails[row].BillNumber + "</td>";
                    tr += "<td style=\"width: 15%\">" + result.EmployeePaymentDetails[row].DueAmount + "</td>";

                    if (isChecked == "1") {
                        tr += "<td style=\"width: 18%\">  <input type=\"text\" class=\"form-control quantitydecimal\" id=\"p" + result.EmployeePaymentDetails[row].EmployeePaymentId + "\" value=\"" + result.EmployeePaymentDetails[row].PaymentAmount + "\" onblur=\"CheckPayment(this)\" /></td>";
                    }
                    else {
                        tr += "<td style=\"width: 18%\">  <input type=\"text\" disabled = \"disabled\" class=\"form-control quantitydecimal\" id=\"p" + result[row].EmployeePaymentId + "\" value=\"" + result.EmployeePaymentDetails[row].PaymentAmount + "\" onblur=\"CheckPayment(this)\" /></td>";
                    }

                    tr = tr + "<td style=\"display:none;\" >" + result.EmployeePaymentDetails[row].PaymentDetailsId + "</td>";
                    tr = tr + "<td style=\"display:none;\" >" + result.EmployeePaymentDetails[row].EmployeeBillId + "</td>";
                    tr = tr + "<td style=\"display:none;\" >" + result.EmployeePaymentDetails[row].EmployeeBillDetailsId + "</td>";
                    tr = tr + "<td style=\"display:none;\" >" + result.EmployeePaymentDetails[row].EmployeePaymentId + "</td>";
                    tr = tr + "<td style=\"display:none;\" >" + result.EmployeePaymentDetails[row].BillId + "</td>";

                    tr += "</tr>";

                    $("#BillInfo > tbody").append(tr);
                    tr = "";
                    isChecked = "0"
                }
            }

            CalculatePayment();
            $("#myTabs").tabs({ active: 0 });

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
            window.location = "frmReservationBillPayment.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            alert(error.get_message());
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
        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show();
        }
        function MessagePanelHide() {
            $('#MessageBox').hide();
        }

        //Integrated Gl Visible True/False-------------------
        function IntegratedGeneralLedgerDivPanelShow() {
            $('#IntegratedGeneralLedgerDiv').show();
        }
        function IntegratedGeneralLedgerDivPanelHide() {
            $('#IntegratedGeneralLedgerDiv').hide();
        }

        function ValidateForm() {
            //if ($("#ContentPlaceHolder1_ddlEmployeeBill").val() == "0") {
            //    toastr.info("Please Select Company Bill.");
            //    return false;
            //}

            if ($("#ContentPlaceHolder1_txtPaymentDate2").val() == "") {
                toastr.info("Please Give Payment Date.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cash") {
                if ($("#ContentPlaceHolder1_ddlCashPayment").val() == "0" && $("#txtCashPayment").val() == "") {
                    toastr.info("Please Select Cash Payment Account.");
                    return false;
                }
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Bank") {
                if ($("#ContentPlaceHolder1_ddlBankPayment").val() == "0" && $("#txtBankPayment").val() == "") {
                    toastr.info("Please Select Bank Payment Account.");
                    return false;
                }
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cheque") {
                if ($("#ContentPlaceHolder1_ddlCompanyBank").val() == "0" && $("#txtCompanyBank").val() == "") {
                    toastr.info("Please Select Bank Payment Account.");
                    return false;
                }
            }

            if ($("#ContentPlaceHolder1_txtRemarks").val() == "") {
                toastr.warning("Please Provide Description.");
                $("#ContentPlaceHolder1_txtRemarks").focus();
                return false;
            }

            CommonHelper.SpinnerOpen();

            var totalPayment = 0.00, advanceAmount = 0.00, detailsId = 0, paymentId = 0, accountingPostingHeadId = 0;
            var employeeBillId = 0.00, adjustmentAccountHeadId = 0, paymentAdjustmentAmount = 0.00;
            var EmployeePaymentDetails = new Array();
            var EmployeePaymentDetailsEdited = new Array();
            var EmployeePaymentDetailsDeleted = new Array();

            adjustmentAccountHeadId = $("#ContentPlaceHolder1_ddlAdjustmentNodeHead").val();
            paymentAdjustmentAmount = $("#ContentPlaceHolder1_txtAdjustmentAmount").val() != "" ? parseFloat($("#ContentPlaceHolder1_txtAdjustmentAmount").val()) : 0.00;
            paymentId = $("#ContentPlaceHolder1_hfPaymentId").val();
            employeeBillId = $("#ContentPlaceHolder1_ddlEmployeeBill").val();

            if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cash") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlCashPayment").val();
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Bank") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlBankPayment").val();
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cheque") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlCompanyBank").val();
            }

            advanceAmount = $("#ContentPlaceHolder1_txtAdvanceAmount").val() == "" ? 0.00 : parseFloat($("#ContentPlaceHolder1_txtAdvanceAmount").val());

            var EmployeePayment = {
                PaymentId: paymentId,
                EmployeeBillId: employeeBillId,
                PaymentFor: $("#ContentPlaceHolder1_ddlTransactionType").val(),
                EmployeeId: $("#ContentPlaceHolder1_hfEmployeeId").val(),
                PaymentDate: CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtPaymentDate2").val(), '/'),
                AdvanceAmount: advanceAmount,
                PaymentType: $("#ContentPlaceHolder1_ddlPayMode").val(),
                AccountingPostingHeadId: accountingPostingHeadId,
                Remarks: $("#ContentPlaceHolder1_txtRemarks").val(),
                ChequeNumber: $("#ContentPlaceHolder1_txtChecqueNumber").val(),
                CurrencyId: $("#ContentPlaceHolder1_ddlCurrency").val(),
                ConvertionRate: $("#ContentPlaceHolder1_txtConversionRate").val() == "" ? "0" : $("#ContentPlaceHolder1_txtConversionRate").val(),
                AdjustmentAccountHeadId: adjustmentAccountHeadId,
                PaymentAdjustmentAmount: paymentAdjustmentAmount
            };

            $("#BillInfo tbody tr").each(function () {

                detailsId = parseInt($(this).find("td:eq(6)").text(), 10);

                if ($(this).find("td:eq(0)").find("input").is(":checked") == true && detailsId == 0) {
                    EmployeePaymentDetails.push({
                        PaymentDetailsId: 0,
                        EmployeeBillDetailsId: $(this).find("td:eq(8)").text(),
                        EmployeePaymentId: $(this).find("td:eq(9)").text(),
                        BillId: $(this).find("td:eq(10)").text(),
                        PaymentAmount: $(this).find("td:eq(5)").find("input").val()
                    });
                }
                else if ($(this).find("td:eq(0)").find("input").is(":checked") == true && detailsId > 0) {
                    EmployeePaymentDetailsEdited.push({
                        PaymentDetailsId: detailsId,
                        EmployeeBillDetailsId: $(this).find("td:eq(8)").text(),
                        EmployeePaymentId: $(this).find("td:eq(9)").text(),
                        BillId: $(this).find("td:eq(10)").text(),
                        PaymentAmount: $(this).find("td:eq(5)").find("input").val()
                    });
                }
                else if ($(this).find("td:eq(0)").find("input").is(":checked") == false && detailsId > 0) {
                    EmployeePaymentDetailsDeleted.push({
                        PaymentDetailsId: detailsId,
                        EmployeeBillDetailsId: $(this).find("td:eq(8)").text(),
                        EmployeePaymentId: $(this).find("td:eq(9)").text(),
                        BillId: $(this).find("td:eq(10)").text(),
                        PaymentAmount: $(this).find("td:eq(5)").find("input").val()
                    });
                }
            });

            PageMethods.SaveEmployeeBillPayment(EmployeePayment, EmployeePaymentDetails, EmployeePaymentDetailsEdited, EmployeePaymentDetailsDeleted, OnSupplierPaymentSucceeded, OnSupplierPaymentFailed);

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

        function SearchPayment() {

            var dateFrom = null, dateTo = null, companyId = 0;

            employeeId = $("#ContentPlaceHolder1_hfEmployeeId").val();

            if ($("#ContentPlaceHolder1_txtFromDate").val() != "") {
                dateFrom = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtFromDate").val(), '/')
            }
            if ($("#ContentPlaceHolder1_txtToDate").val() != "") {
                dateTo = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtToDate").val(), '/')
            }

            var transactionType = $("#ContentPlaceHolder1_ddlSrcTransactionType").val();
            $("#BillInfoSearch tbody").html("");
            PageMethods.GetEmployeePaymentBySearch(transactionType, employeeId, dateFrom, dateTo, OnSearchPaymentSucceeded, OnSearchPaymentFailed);

            return false;
        }

        function OnSearchPaymentSucceeded(result) {
            $("#BillInfoSearch tbody").html("");
            var row = 0, tr = "";

            for (row = 0; row < result.length; row++) {

                if ((row + 1) % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width: 10%'>" + result[row].LedgerNumber + "</td>";
                tr += "<td style='width: 15%'>" + result[row].PaymentFor + "</td>";
                tr += "<td style='width: 10%'>" + GetStringFromDateTime(result[row].PaymentDate) + "</td>";
                tr += "<td style='width: 20%'>" + result[row].EmployeeName + "</td>";
                tr += "<td style='width: 30%'>" + result[row].Remarks + "</td>";
                tr += "<td style='width:15%;'>";
                if (result[row].ApprovedStatus == null) {

                    tr += "<a href='javascript:void();' onclick= 'javascript:return ApprovedPayment(" + result[row].PaymentId + ")' ><img alt='approved' src='../Images/approved.png' /></a>";
                    tr += "&nbsp;&nbsp;";
                    tr += "<a onclick=\"javascript:return FIllForEdit(" + result[row].PaymentId + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                    tr += "&nbsp;&nbsp;";
                    tr += "<a href='javascript:void();' onclick= 'javascript:return DeleteEmployeePayment(" + result[row].PaymentId + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                    tr += "&nbsp;&nbsp;";
                    //tr += "<a href='javascript:void();' onclick= 'javascript:return EmployeePaymentInvoice(" + result[row].PaymentId + ")' ><img alt='Invoice' src='../Images/ReportDocument.png' /></a>";
                    tr += "</td>";
                }
                else {
                    tr += "&nbsp;&nbsp;";
                    //tr += "<a href='javascript:void();' onclick= 'javascript:return EmployeePaymentInvoice(" + result[row].PaymentId + ")' ><img alt='Invoice' src='../Images/ReportDocument.png' /></a>";
                    tr += "</td>";
                }

                tr += "<td style=display:none;'>" + result[row].PaymentId + "</td>";
                tr += "</tr>";

                $("#BillInfoSearch tbody").append(tr);
                tr = "";
            }
        }
        function OnSearchPaymentFailed() { }

        function ApprovedPayment(paymentId) {
            PageMethods.ApprovedPayment(paymentId, OnApporavalSucceed, OnApporavalFailed);
            return false;
        }
        function OnApporavalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchPayment();
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnApporavalFailed(error) {
            toastr.error(error.get_message());
        }

        function DeleteEmployeePayment(paymentId) {
            PageMethods.DeleteEmployeePayment(paymentId, OnReceiveDeleteSucceed, OnReceiveDeleteFailed);
            return false;
        }
        function OnReceiveDeleteSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchPayment();
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnReceiveDeleteFailed(error) {
            toastr.error(error.get_message());
        }

        function PerformClearActionForbutton() {

            if (!confirm("Do you want to clear?")) {
                return false;
            }
            PerformClearAction();
        }

        //For ClearForm-------------------------
        function PerformClearAction() {

            $("#BillInfo tbody").html("");
            $("#ContentPlaceHolder1_hfEmployeePaymentId").val("0");
            $("#ContentPlaceHolder1_hfEmployeeId").val("0");
            $("#<%=txtLedgerAmount.ClientID %>").val('');
            $("#<%=txtDealId.ClientID %>").val('');
            $("#<%=ddlCurrency.ClientID %>").val('');
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
            $("#txtEmployeeName").val("");
            $("#ContentPlaceHolder1_txtDepartment").val("");
            $("#ContentPlaceHolder1_txtDesignation").val("");
            $("#ContentPlaceHolder1_txtGrade").val("");
            $("#ContentPlaceHolder1_txtPresentPhone").val("");
            $("#ContentPlaceHolder1_txtAddress").val("");
            $("#ContentPlaceHolder1_ddlEmployeeBill").val("0");
            $("#ContentPlaceHolder1_txtPaymentDate2").val("");

            $('#ConversionPanel').hide();
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
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
            var txtCardNumber = '<%=txtCardNumber.ClientID%>'
            var ddlCardType = '<%=ddlCardType.ClientID%>'
            var cardNumber = $('#' + txtCardNumber).val();
            var cardType = $('#' + ddlCardType).val();
            var isTrue = true;
            var messege = "";

            var txtCardValidation = '<%=txtCardValidation.ClientID%>'
            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            if ($('#' + ddlPayMode).val() != "Card") {
                return true;
            }

            if ($('#' + txtCardValidation).val() == 0) {
                return true;
            }

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

        function LoadPopUp() {
            //popup(1, 'DivPaymentSelect', '', 600, 500);
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

        function LoadCompanyInfo(companyId) {
            PageMethods.LoadCompanyInfo(companyId, OnLoadCompanySucceeded, OnLoadCompanyFailed);
            return false;
        }
        function OnLoadCompanySucceeded(result) {

        }
        function OnLoadCompanyFailed(error) {
            toastr.error(error.get_message());
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
                var url = "/HotelManagement/Reports/frmReportCompanyPayment.aspx?PaymentIdList=" + paymentIdList;
                var popup_window = "Preview";
                window.open(url, popup_window, "width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
            }
            else {
                toastr.warning('Select payments to preview');
            }
        }

        //------------ Bill Payment

        function CheckRow(control) {
            var tr = $(control).parent().parent();

            if ($(control).is(":checked")) {
                $(tr).find("td:eq(5)").find("input").prop("disabled", false);
            }
            else {
                $(tr).find("td:eq(5)").find("input").prop("disabled", true);
            }

            CalculatePayment();
        }

        function SearchEmployeeBill() {
            CommonHelper.SpinnerOpen();
            var companyId = $("#ContentPlaceHolder1_hfEmployeeId").val();
            var companyBillId = $("#ContentPlaceHolder1_ddlEmployeeBill").val();

            $("#BillInfo tbody").html("");
            PageMethods.EmployeeBillBySearch(companyBillId, companyId, OnLoadEmployeeBillSucceeded, OnEmployeeBillFailed);
        }

        function OnLoadEmployeeBillSucceeded(result) {

            var row = 0, tr = "", chk = "checked='checked'", isChecked = 0;
            var totalPaymentAmount = 0.00;

            for (row = 0; row < result.length; row++) {

                isChecked = result[row].EmployeeBillId > 0 ? "1" : "0";

                if ((row + 1) % 2 == 0) {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }

                tr += "<td style=\"width: 7%\"> ";

                if (isChecked == "1") {
                    tr += "<input type=\"checkbox\" id=\"pay" + result[row].EmployeePaymentId + "\" checked=\"checked\" onclick=\"CheckRow(this)\" />";
                    totalPaymentAmount += result[row].DueAmount;
                }
                else {
                    tr += "<input type=\"checkbox\" id=\"pay" + result[row].EmployeePaymentId + "\" onclick=\"CheckRow(this)\" />";
                }

                tr += "</td>";

                tr += "<td style=\"width: 20%\">" + result[row].ModuleName + "</td>";
                tr += "<td style=\"width: 20%\">" + GetStringFromDateTime(result[row].PaymentDate) + "</td>";
                tr += "<td style=\"width: 20%\">" + result[row].BillNumber + "</td>";
                tr += "<td style=\"width: 15%\">" + result[row].DueAmount + "</td>";

                if (isChecked == "1") {
                    tr += "<td style=\"width: 18%\">  <input type=\"text\" class=\"form-control quantitydecimal\" id=\"p" + result[row].EmployeePaymentId + "\" value=\"" + result[row].DueAmount + "\" onblur=\"CheckPayment(this)\" /></td>";
                }
                else {
                    tr += "<td style=\"width: 18%\">  <input type=\"text\" disabled class=\"form-control quantitydecimal\" id=\"p" + result[row].EmployeePaymentId + "\" value=\"" + result[row].DueAmount + "\" onblur=\"CheckPayment(this)\" /></td>";
                }

                tr += "<td style=\"display:none;\">" + result[row].PaymentDetailsId + "</td>";
                tr += "<td style=\"display:none;\">" + result[row].EmployeeBillId + "</td>";
                tr += "<td style=\"display:none;\">" + result[row].EmployeeBillDetailsId + "</td>";
                tr += "<td style=\"display:none;\">" + result[row].EmployeePaymentId + "</td>";
                tr += "<td style=\"display:none;\">" + result[row].BillId + "</td>";

                tr += "</tr>";

                $("#BillInfo tbody").append(tr);
                tr = "";
            }

            $("#ContentPlaceHolder1_txtLedgerAmount").val(totalPaymentAmount);
            $("#ContentPlaceHolder1_txtTotalAmount").val(totalPaymentAmount);
            CommonHelper.ApplyDecimalValidation();

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnEmployeeBillFailed(error) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        function CalculatePayment() {

            var totalPayment = 0.00;

            $("#BillInfo tbody tr").each(function () {

                if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {
                    totalPayment += parseFloat($(this).find("td:eq(5)").find("input").val());
                }
            });

            var paymentAdjustmentAmount = $("#ContentPlaceHolder1_txtAdjustmentAmount").val() != "" ? parseFloat($("#ContentPlaceHolder1_txtAdjustmentAmount").val()) : 0.00;
            var advanceAmount = $("#ContentPlaceHolder1_txtAdvanceAmount").val() != "" ? parseFloat($("#ContentPlaceHolder1_txtAdvanceAmount").val()) : 0.00;

            $("#ContentPlaceHolder1_txtTotalAmount").val(toFixed((totalPayment + advanceAmount), 2));
            $("#ContentPlaceHolder1_txtLedgerAmount").val(toFixed(((totalPayment + advanceAmount) - paymentAdjustmentAmount), 2));
        }

        function CheckPayment(control) {

            var tr = $(control).parent().parent();
            var value = parseFloat($(control).val());
            var bdValue = parseFloat($(tr).find("td:eq(4)").text());

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

        function LoadEmployeeBill() {
            var employeeId = $("#ContentPlaceHolder1_hfEmployeeId").val();
            PageMethods.GetEmployeeGeneratedBillByBillStatus(employeeId, OnLoadLoadCompanyBillSucceeded, OnLoadLoadCompanyBillFailed);
        }
        function OnLoadLoadCompanyBillSucceeded(result) {
            var list = result;
            EmployeeGeneratedBill = result;

            var control = $('#ContentPlaceHolder1_ddlEmployeeBill');

            control.empty();
            if (list != null) {
                if (list.length > 0) {

                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].EmployeeBillNumber + '" value="' + list[i].EmployeeBillId + '">' + list[i].EmployeeBillNumber + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            if ($("#ContentPlaceHolder1_hfEmployeeBillId").val() != 0) {
                control.val($("#ContentPlaceHolder1_hfEmployeeBillId").val());
                $("#ContentPlaceHolder1_ddlCurrency").trigger("change");
            }

            return false;
        }
        function OnLoadLoadCompanyBillFailed() { }

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
        function EmployeePaymentInvoice(paymentId) {
            if (paymentId != "") {
                var url = "/Payroll/Reports/frmReportEmployeePaymentInvoiceVoucher.aspx?PaymentIdList=" + paymentId;
                var popup_window = "Preview";
                window.open(url, popup_window, "width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
            }
            else {
                toastr.warning('Select payments to preview');
            }
            return false;
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
    <asp:HiddenField ID="hfPaymentId" runat="server" Value="0" />
    <asp:HiddenField ID="hfEmployeePaymentId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCompanyBill" runat="server" />
    <asp:HiddenField ID="hfEmployeeBillId" runat="server" Value="0" />
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtPaymentId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtDealId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfEmployeeId" runat="server" Value="0" />

    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
        <asp:HiddenField ID="txtCardValidation" runat="server"></asp:HiddenField>
    </div>

    <div id="DivPaymentSelect" style="display: none;">
        <div id="Div1" class="panel panel-default">
            <div class="panel-body">
                <asp:HiddenField ID="hfCurrencyType" runat="server" />
                <asp:HiddenField ID="hfConversionRate" runat="server" />
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
                                <asp:Label ID="lblid" runat="server" Text='<%#Eval("CompanyPaymentId") %>'></asp:Label>
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
    <div id="btnNewBIll" class="btn-toolbar" style="display: none;">
        <button onclick="javascript: return EntryPanelVisibleTrue();" class="btn btn-primary">
            <i class="icon-plus"></i>New Payment</button>
        <div class="btn-group">
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
                    Employee Transaction
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group" style="display: none;">
                            <div class="col-md-2">
                                <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblRoomNumber" runat="server" class="control-label" Text="Room Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRoomId" runat="server" CssClass="form-control" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlRoomId_SelectedIndexChanged">
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
                                <asp:DropDownList ID="ddlCompanyPaymentAccountHead" runat="server" CssClass="childDivSectionDivThreeColumnDropDownList">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label9" runat="server" class="control-label" Text="Transaction Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <div>
                                    <asp:DropDownList ID="ddlTransactionType" TabIndex="1" CssClass="form-control" runat="server">
                                        <asp:ListItem>Receive</asp:ListItem>
                                        <asp:ListItem>Payment</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSrcRoomNumber" runat="server" class="control-label required-field"
                                    Text="Employee Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <input id="txtEmployeeName" type="text" class="form-control" name="employeeName" />
                            </div>
                        </div>
                        <div id="CompanyInfo" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblDepartment" runat="server" class="control-label" Text="Department"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control" TabIndex="4"
                                        disabled></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblDesignation" runat="server" class="control-label" Text="Designation"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDesignation" runat="server" TabIndex="6" CssClass="form-control"
                                        disabled></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblGrade" runat="server" class="control-label" Text="Grade"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtGrade" runat="server" TabIndex="7" CssClass="form-control"
                                        disabled></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblContactPerson" runat="server" class="control-label" Text="Contact Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPresentPhone" runat="server" CssClass="form-control" TabIndex="5"
                                        disabled></asp:TextBox>
                                </div>
                            </div>

                        </div>
                        <div class="form-group" id="EmployeeBillDiv">
                            <div class="col-md-2">
                                <asp:Label ID="Label6" runat="server" class="control-label" Text="Employee Bill"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlEmployeeBill" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="EmployeeBillDetailDiv">
                            <div class="col-md-12">
                                <table id="BillInfo" class="table table-bordered table-condensed table-hover table-responsive">
                                    <thead>
                                        <tr>
                                            <th style="width: 7%;">Select</th>
                                            <th style="width: 20%;">Payment Mode</th>
                                            <th style="width: 20%;">Bill Date</th>
                                            <th style="width: 20%;">Bill Number</th>
                                            <th style="width: 15%;">Due Amount</th>
                                            <th style="width: 18%;">Payment Amount</th>
                                            <th style="display: none;"></th>
                                            <th style="display: none;"></th>
                                            <th style="display: none;"></th>
                                            <th style="display: none;"></th>
                                            <th style="display: none;"></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                    <tfoot>
                                        <%--<tr>
                                            <td colspan="5" style="width: 82%; text-align: right;">Advance Amount</td>
                                            <td style="width: 18%">
                                                <asp:TextBox ID="txtAdvanceAmount" runat="server" onblur="CheckAdvance(this)" CssClass="form-control quantitydecimal" TabIndex="7"> </asp:TextBox>
                                            </td>
                                        </tr>--%>
                                    </tfoot>
                                </table>
                            </div>
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

                        <div class="form-group">
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
                                <asp:Label ID="lblPayMode" runat="server" class="control-label" Text="Payment Mode"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPayMode" runat="server" CssClass="form-control" TabIndex="5">
                                    <%--<asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="Cash">Cash</asp:ListItem>
                                    <asp:ListItem Value="Card">Card</asp:ListItem>
                                    <asp:ListItem>Cheque</asp:ListItem>
                                    <asp:ListItem Value="Adjustment">Adjustment</asp:ListItem>
                                    <asp:ListItem Value="Loan">Company Loan</asp:ListItem>--%>
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
                                    <asp:DropDownList ID="ddlGLCompany" runat="server" CssClass="form-control" onchange="PopulateProjects();">
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
                                <asp:DropDownList ID="ddlCurrency" TabIndex="6" disabled CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblReceiveAmount" runat="server" class="control-label required-field"
                                    Text="Payment Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtLedgerAmount" runat="server" disabled CssClass="form-control quantitydecimal" TabIndex="7"> </asp:TextBox>
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
                        <div id="ChecquePaymentAccountHeadDiv" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblChecquePaymentAccountHeadId" runat="server" class="control-label required-field"
                                        Text="Company Name"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlChecquePaymentAccountHeadId" runat="server" CssClass="form-control"
                                        TabIndex="6">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblChecqueNumber" runat="server" class="control-label required-field"
                                        Text="Cheque Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
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
                                        <asp:DropDownList ID="ddlCompanyBank" runat="server" CssClass="form-control" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
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
                        <div id="BankPaymentAccountHeadDiv" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label7" runat="server" class="control-label required-field"
                                        Text="Bank Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <input id="txtBankPayment" type="text" class="form-control" />
                                    <div style="display: none;">
                                        <asp:DropDownList ID="ddlBankPayment" runat="server" CssClass="form-control" AutoPostBack="false">
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
                                    <%--<asp:DropDownList ID="ddlBankId" runat="server" CssClass="ThreeColumnDropDownList"
                            TabIndex="10">
                        </asp:DropDownList>--%>
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
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="12" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return ValidateForm();" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="13" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return PerformClearActionForbutton();" />
                    <asp:Button ID="btnCancel" runat="server" TabIndex="14" Text="Close" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return EntryPanelVisibleFalse();" Visible="False" />
                    <asp:Button ID="btnGroupPaymentPreview1" runat="server" Text="Payment Preview" TabIndex="15"
                        CssClass="TransactionalButton btn btn-primary btn-sm" OnClientClick="javascript: return LoadPopUp();" />
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
                                <asp:Label ID="Label8" runat="server" class="control-label" Text="Employee"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlEmployeeForSearch" runat="server" CssClass="form-control">
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
                                <asp:Label ID="Label12" runat="server" class="control-label" Text="Transaction Type"></asp:Label>
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
                                    OnClientClick="javascript: return SearchPayment()" />
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
                                    <th style="width: 10%;">Ledger Number</th>
                                    <th style="width: 15%;">Transaction Type</th>
                                    <th style="width: 10%;">Payment Date</th>
                                    <th style="width: 20%;">Employee Name</th>
                                    <th style="width: 30%;">Remarks</th>
                                    <th style="width: 15%;">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var isIntegrated = '<%=isIntegratedGeneralLedgerDiv%>';
            if (isIntegrated > -1) {
                IntegratedGeneralLedgerDivPanelShow();
            }
            else {
                IntegratedGeneralLedgerDivPanelHide();
            }

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

            //            var x = '<%=isSearchPanelEnable%>';
            //            if (x > -1) {
            //                $('#SearchResult').show();

            //            }
            //            else {
            //                $('#SearchResult').hide();
            //            }

            var single = '<%=isSingle%>';
            if (single == "True") {
                $('#CompanyProjectPanel').hide();
            }
            else {
                $('#CompanyProjectPanel').show();
            }
        });
    </script>
</asp:Content>
