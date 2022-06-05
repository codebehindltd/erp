<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmMemberBillReceive.aspx.cs" Inherits="HotelManagement.Presentation.Website.Membership.frmMemberBillReceive" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(function () {
            var ddlCurrency = '<%=ddlCurrency.ClientID%>'
            var txtConversionRate = '<%=txtConversionRate.ClientID%>'
            var txtLedgerAmount = '<%=txtLedgerAmount.ClientID%>'
            var txtCalculatedLedgerAmount = '<%=txtCalculatedLedgerAmount.ClientID%>'
            var txtCalculatedLedgerAmountHiddenField = '<%=txtCalculatedLedgerAmountHiddenField.ClientID%>'

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
            $('#ContentPlaceHolder1_txtAdjustmentAmount').blur(function () {
                var paymentTotal = $("#ContentPlaceHolder1_txtTotalAmount").val();
                var adjustmentAmount = $("#ContentPlaceHolder1_txtAdjustmentAmount").val();
                var paymentAmount = parseFloat(paymentTotal) - parseFloat(adjustmentAmount);

                $("#ContentPlaceHolder1_txtLedgerAmount").val(paymentAmount);
            });
            $("#ContentPlaceHolder1_ddlMemberBill").change(function () {

                var MemberBillId = $(this).val();

                var bill = _.findWhere(MemberGeneratedBill, { MemberBillId: parseInt(MemberBillId, 10) });
                if (bill != null) {
                    $("#ContentPlaceHolder1_ddlCurrency").val(bill.BillCurrencyId);
                    $("#ContentPlaceHolder1_ddlCurrency").trigger("change");
                    CurrencyConvertion();
                }

                SearchMemberBill();
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

            CommonHelper.AutoSearchClientDataSource("txtBankPayment", "ContentPlaceHolder1_ddlBankPayment", "ContentPlaceHolder1_ddlBankPayment");
            CommonHelper.AutoSearchClientDataSource("txtCashPayment", "ContentPlaceHolder1_ddlCashPayment", "ContentPlaceHolder1_ddlCashPayment");
            CommonHelper.AutoSearchClientDataSource("txtMemberSearch", "ContentPlaceHolder1_ddlMember", "ContentPlaceHolder1_hfMemberId");
            $('#txtMemberSearch').blur(function () {
                if ($(this).val() != "") {
                    var cmpId = $("#ContentPlaceHolder1_hfMemberId").val();
                    if (cmpId != "") {
                        LoadMemberInfo(cmpId);
                        LoadMemberBill();
                        $("#MemberInfo").show();
                    }
                    else {
                        $("#MemberInfo").hide();
                    }
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
        });
        function CurrencyRateInfoEnable() {
            var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
            if (ddlCurrency == 0) {
                $('#ConversionPanel').hide()
            }
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
        $(document).ready(function () {
            $("#myTabs").tabs();

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);
            $('#ContentPlaceHolder1_txtPaymentDate2').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);
            var ddlPayMode = '<%=ddlPayMode.ClientID%>'

            var lblPaymentAccountHead = '<%=lblPaymentAccountHead.ClientID%>'
            $('#' + ddlPayMode).change(function () {
                PaymentModeShowHideInformation();
            });
        });
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
                $('#MemberPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').show();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#MemberPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Cheque") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#PaidByOtherRoomDiv').hide();
                $('#MemberPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Bank") {
                $('#BankPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#MemberPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $('#CashReceiveAccountsInfo').show();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#MemberPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Adjustment") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#MemberPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Loan") {
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#MemberPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').show();
            }
        }
        function SearchMemberBill() {
            CommonHelper.SpinnerOpen();
            var MemberId = $("#ContentPlaceHolder1_hfMemberId").val();
            var MemberBillId = $("#ContentPlaceHolder1_ddlMemberBill").val();

            $("#BillInfo tbody").html("");
            PageMethods.MemberBillBySearch(MemberBillId, MemberId, OnLoadMemberBillSucceeded, OnMemberBillFailed);
        }

        function OnLoadMemberBillSucceeded(result) {
            var row = 0, tr = "", chk = "checked='checked'", isChecked = 0;
            var totalPaymentAmount = 0.00;

            for (row = 0; row < result.length; row++) {

                isChecked = result[row].MemberBillId > 0 ? "1" : "0";

                if ((row + 1) % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width: 7%'> ";

                if (isChecked == "1") {
                    tr += "<input type='checkbox' id='pay" + result[row].MemberPaymentId + "'" + chk + " onclick='CheckRow(this)' />";
                    totalPaymentAmount += result[row].DueAmount;
                }
                else {
                    tr += "<input type='checkbox' id='pay" + result[row].MemberPaymentId + "' onclick='CheckRow(this)' />";
                }

                tr += "</td>";

                tr += "<td style='width: 20%'>" + result[row].ModuleName + "</td>";
                tr += "<td style='width: 20%'>" + GetStringFromDateTime(result[row].PaymentDate) + "</td>";
                tr += "<td style='width: 20%'>" + result[row].BillNumber + "</td>";
                tr += "<td style='width: 15%'>" + result[row].DueAmount + "</td>";

                if (isChecked == "1") {
                    tr += "<td style='width: 18%'>  <input type='text' class='form-control quantitydecimal' id='p" + result[row].MemberPaymentId + "' value='" + result[row].DueAmount + "' onblur='CheckPayment(this)' /></td>";
                }
                else {
                    tr += "<td style='width: 18%'>  <input type='text' disabled class='form-control quantitydecimal' id='p" + result[row].MemberPaymentId + "' value='" + result[row].DueAmount + "' onblur='CheckPayment(this)' /></td>";
                }

                tr += "<td style='display:none;'>" + "0" + "</td>"; //result[row].PaymentDetailsId
                tr += "<td style='display:none;'>" + result[row].MemberBillId + "</td>";
                tr += "<td style='display:none;'>" + result[row].MemberBillDetailsId + "</td>";
                tr += "<td style='display:none;'>" + result[row].MemberPaymentId + "</td>";
                tr += "<td style='display:none;'>" + result[row].BillId + "</td>";

                tr += "</tr>";

                $("#BillInfo tbody").append(tr);
                tr = "";
            }

            $("#ContentPlaceHolder1_txtTotalAmount").val(totalPaymentAmount);
            $("#ContentPlaceHolder1_txtLedgerAmount").val(totalPaymentAmount);

            CommonHelper.ApplyDecimalValidation();

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnMemberBillFailed(error) {
            toastr.warning("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        //After Auto search 
        function LoadMemberInfo(MemberId) {
            PageMethods.LoadMemberInfo(MemberId, OnLoadMemberSucceeded, OnFailed);
            return false;
        }
        function OnLoadMemberSucceeded(result) {
            $("#<%=txtPersonalEmail.ClientID %>").val(result.PersonalEmail);
            $("#<%=txtDesignation.ClientID %>").val(result.Designation);
            $("#<%=txtMembershipNumber.ClientID %>").val(result.MembershipNumber);
            $("#<%=txtMobileNumber.ClientID %>").val(result.MobileNumber);
            $("#<%=txtOccupation.ClientID %>").val(result.Occupation);
            $("#<%=txtAddress.ClientID %>").val(result.MemberAddress);
            $("#<%=hfMemberId.ClientID %>").val(result.MemberId);
        }
        function SearchPayment() {
            //debugger;
            var dateFrom = null, dateTo = null, MemberId = 0;

            MemberId = $("#ContentPlaceHolder1_ddlMemberForSearch").val();

            if ($("#ContentPlaceHolder1_txtFromDate").val() != "") {
                dateFrom = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtFromDate").val(), '/')
            }
            if ($("#ContentPlaceHolder1_txtToDate").val() != "") {
                dateTo = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtToDate").val(), '/')
            }

            //$("#BillInfoSearch tbody").html("");
            PageMethods.GetMemberPaymentBySearch(MemberId, dateFrom, dateTo, OnSearchPaymentSucceeded, OnSearchPaymentFailed);

            return false;
        }
        function OnSearchPaymentSucceeded(result) {
            $("#BillInfoSearch tbody").html("");
            var row = 0, tr = "";

            var isUpdatepermission = false, isDeletePermission = false;

            if ($("#ContentPlaceHolder1_hfIsUpdatePermission").val() == "1")
                isUpdatepermission = true;
            if ($("#ContentPlaceHolder1_hfIsDeletePermission").val() == "1")
                isDeletePermission = true;

            for (row = 0; row < result.length; row++) {

                if ((row + 1) % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width: 20%'>" + result[row].LedgerNumber + "</td>";
                tr += "<td style='width: 15%'>" + GetStringFromDateTime(result[row].PaymentDate) + "</td>";
                tr += "<td style='width: 20%'>" + result[row].FullName + "</td>";

                if (result[row].Remarks != "" && result[row].Remarks != null)
                    tr += "<td style='width: 20%'>" + result[row].Remarks + "</td>";
                else
                    tr += "<td style='width: 20%'></td>";
                if (result[row].ApprovedStatus != null)
                    tr += "<td style='width: 15%'>" + result[row].ApprovedStatus + "</td>";
                else
                    tr += "<td style='width: 15%'></td>";
                 tr += "<td style='width:10%;'>";
                if (result[row].ApprovedStatus == null) {
                    //tr += "<td style='width:10%;'>";
                    tr += "<a href='javascript:void();' onclick= 'javascript:return ApprovedPayment(" + result[row].PaymentId + ")' ><img alt='approved' src='../Images/approved.png' /></a>";
                    tr += "&nbsp;&nbsp;";
                    if (isUpdatepermission) {
                        tr += "<a onclick=\"javascript:return FIllForEdit(" + result[row].PaymentId + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                        tr += "&nbsp;&nbsp;";
                    }
                    if (isDeletePermission) {
                        tr += "<a href='javascript:void();' onclick= 'javascript:return DeleteMemberPayment(" + result[row].PaymentId + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                        tr += "</td>";
                    }

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
            if (confirm("Want to Approve?")) {
                PageMethods.ApprovedPayment(paymentId, OnApporavalSucceed, OnApporavalFailed);
            }

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
        //For Delete-------------------------        
        function DeleteMemberPayment(actionId) {

            var answer = confirm("Do you want to delete this record?")
            if (answer) {
                PageMethods.DeleteMemberPayment(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            }
            return false;
        }

        function OnDeleteObjectSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchPayment();
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }

        function OnDeleteObjectFailed(error) {
            alert(error.get_message());
        }
        //Integrated Gl Visible True/False-------------------
        function IntegratedGeneralLedgerDivPanelShow() {
            $('#IntegratedGeneralLedgerDiv').show();
        }
        function IntegratedGeneralLedgerDivPanelHide() {
            $('#IntegratedGeneralLedgerDiv').hide();
        }
        function LoadMemberBill() {
            var MemberId = $("#ContentPlaceHolder1_hfMemberId").val();
            PageMethods.GetMemberGeneratedBillByBillStatus(MemberId, OnLoadLoadMemberBillSucceeded, OnFailed);
        }
        function OnLoadLoadMemberBillSucceeded(result) {
            MemberGeneratedBill = result;
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlMemberBill');

            control.empty();
            if (list != null) {
                if (list.length > 0) {

                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].MemberBillNumber + '" value="' + list[i].MemberBillId + '">' + list[i].MemberBillNumber + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            if ($("#ContentPlaceHolder1_hfMemberBillId").val() != 0) {
                control.val($("#ContentPlaceHolder1_hfMemberBillId").val());
                $("#ContentPlaceHolder1_ddlCurrency").trigger("change");
            }

            return false;
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
        }
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
        //For FillForm-------------------------   
        function FIllForEdit(actionId) {

            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {
            $("#ContentPlaceHolder1_hfPaymentId").val(result.MemberPayment.PaymentId);
            $("#ContentPlaceHolder1_hfMemberBillId").val(result.MemberPayment.MemberBillId + "");
            $("#ContentPlaceHolder1_hfMemberId").val(result.MemberPayment.MemberId);
            LoadMemberBill();
            $("#<%=hfCurrencyType.ClientID %>").val(result.MemberPayment.CurrencyType);
            $("#<%=hfMemberPaymentId.ClientID %>").val(result.MemberPayment.MemberPaymentId);
            $("#MemberInfo").show();
            $("#btnSave").val("Update");
            $("#txtMemberSearch").val(result.Member.FullName);

            $("#<%=txtPersonalEmail.ClientID %>").val(result.Member.PersonalEmail);
            $("#<%=txtDesignation.ClientID %>").val(result.Member.Designation);
            $("#<%=txtAddress.ClientID %>").val(result.Member.MemberAddress);
            $("#<%=txtMobileNumber.ClientID %>").val(result.Member.MobileNumber);
            $("#<%=txtMembershipNumber.ClientID %>").val(result.Member.MembershipNumber);
            $("#<%=txtOccupation.ClientID %>").val(result.Member.TelephoneNumber);

            $("#<%=ddlCurrency.ClientID %>").val(result.MemberPayment.CurrencyId);
            $("#<%=txtLedgerAmount.ClientID %>").val(result.MemberPayment.CurrencyAmount);
            $("#<%=txtPaymentDate2.ClientID %>").val(GetStringFromDateTime(result.MemberPayment.PaymentDate));

            $("#<%=ddlBankPayment.ClientID %>").val(result.MemberPayment.AccountsPostingHeadId);
            $('#txtBankPayment').val($("#<%=ddlBankPayment.ClientID %> option:selected").text());

            $("#<%=ddlCashPayment.ClientID %>").val(result.MemberPayment.AccountsPostingHeadId);
            $('#txtCashPayment').val($("#<%=ddlCashPayment.ClientID %> option:selected").text());
            $("#<%=txtRemarks.ClientID %>").val(result.MemberPayment.Remarks);

            $("#ContentPlaceHolder1_txtAdjustmentAmount").val(result.MemberPayment.PaymentAdjustmentAmount);
            $("#ContentPlaceHolder1_ddlAdjustmentNodeHead").val(result.MemberPayment.AdjustmentAccountHeadId + '').trigger('change');

            if ($("#ContentPlaceHolder1_hfIsUpdatePermission").val() == "1")
                $("#ContentPlaceHolder1_btnSave").val("Update").show();
            else
                $("#ContentPlaceHolder1_btnSave").hide();
            $('#btnNewBIll').hide();
            $('#EntryPanel').show();
            if (result.MemberPayment.PaymentType == "Cash") {
                $("#ContentPlaceHolder1_ddlCashPayment").val(result.MemberPayment.AccountingPostingHeadId + "");
                $("#txtCashPayment").val($("#ContentPlaceHolder1_ddlCashPayment option:selected").text());
            }
            else if (result.MemberPayment.PaymentType == "Bank") {
                $("#ContentPlaceHolder1_ddlBankPayment").val(result.MemberPayment.AccountingPostingHeadId + "");
                $("#txtBankPayment").val($("#ContentPlaceHolder1_ddlBankPayment option:selected").text());
            }
            else if (result.MemberPayment.PaymentType == "Cheque") {
                $("#ContentPlaceHolder1_ddlMemberBank").val(result.MemberPayment.AccountingPostingHeadId + "");
                $("#txtMemberBank").val($("#ContentPlaceHolder1_ddlMemberBank option:selected").text());
            }
            else if ($('#ContentPlaceHolder1_ddlPayMode').val() == "Adjustment") {

            }
            else if (result.MemberPayment.PaymentType == "Loan") {
                $("#ContentPlaceHolder1_ddlCashAndCashEquivalantPayment").val(result.MemberPayment.AccountingPostingHeadId + "");
                $("#txtCashAndCashEquivalantPayment").val($("#ContentPlaceHolder1_ddlCashAndCashEquivalantPayment option:selected").text());
            }

            $("#<%=ddlPayMode.ClientID %>").val(result.MemberPayment.PaymentType)

            if (result.MemberPayment.PaymentType == "Cash") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#MemberPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if (result.MemberPayment.PaymentType == "Card") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#MemberPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if (result.MemberPayment.PaymentType == "Cheque") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#PaidByOtherRoomDiv').hide();
                $('#MemberPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if (result.MemberPayment.PaymentType == "Bank") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#MemberPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').show();
                IntegratedGeneralLedgerDivPanelHide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            else if (result.MemberPayment.PaymentType == "Adjustment") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#MemberPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
            }
            if (result.MemberPayment.PaymentType == "Loan") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#MemberPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').show();
            }

            $("#BillInfo tbody").html("");
            $("#ContentPlaceHolder1_txtAdvanceAmount").val(result.MemberPayment.AdvanceAmount);

            if (result.MemberPaymentDetails.length > 0) {

                var row = 0, tr = "", chk = "checked='checked'", isChecked = 0;
                var totalPaymentAmount = 0.00;

                for (row = 0; row < result.MemberPaymentDetails.length; row++) {

                    isChecked = result.MemberPaymentDetails[row].MemberBillDetailsId > 0 ? "1" : "0";

                    if ((row + 1) % 2 == 0) {
                        tr += "<tr style='background-color:#FFFFFF;'>";
                    }
                    else {
                        tr += "<tr style='background-color:#E3EAEB;'>";
                    }

                    tr += "<td style='width: 7%'> ";

                    if (isChecked == "1") {
                        tr += "<input type='checkbox' id='pay" + result.MemberPaymentDetails[row].MemberPaymentId + "'" + chk + " onclick='CheckRow(this)' />";
                        totalPaymentAmount += result.MemberPaymentDetails[row].DueAmount;
                    }
                    else {
                        tr += "<input type='checkbox' id='pay" + result.MemberPaymentDetails[row].MemberPaymentId + "' onclick='CheckRow(this)' />";
                    }

                    tr += "</td>";

                    tr += "<td style='width: 20%'>" + result.MemberPaymentDetails[row].ModuleName + "</td>";
                    tr += "<td style='width: 20%'>" + GetStringFromDateTime(result.MemberPaymentDetails[row].PaymentDate) + "</td>";
                    tr += "<td style='width: 20%'>" + result.MemberPaymentDetails[row].BillNumber + "</td>";
                    tr += "<td style='width: 15%'>" + result.MemberPaymentDetails[row].DueAmount + "</td>";

                    if (isChecked == "1") {
                        tr += "<td style='width: 18%'>  <input type='text' class='form-control quantitydecimal' id='p" + result.MemberPaymentDetails[row].MemberPaymentId + "' value='" + result.MemberPaymentDetails[row].PaymentAmount + "' onblur='CheckPayment(this)' /></td>";
                    }
                    else {
                        tr += "<td style='width: 18%'>  <input type='text' disabled class='form-control quantitydecimal' id='p" + result[row].MemberPaymentId + "' value='" + result.MemberPaymentDetails[row].PaymentAmount + "' onblur='CheckPayment(this)' /></td>";
                    }

                    tr += "<td style='display:none;'>" + result.MemberPaymentDetails[row].PaymentDetailsId + "</td>";
                    tr += "<td style='display:none;'>" + result.MemberPaymentDetails[row].MemberBillId + "</td>";
                    tr += "<td style='display:none;'>" + result.MemberPaymentDetails[row].MemberBillDetailsId + "</td>";
                    tr += "<td style='display:none;'>" + result.MemberPaymentDetails[row].MemberPaymentId + "</td>";
                    tr += "<td style='display:none;'>" + result.MemberPaymentDetails[row].BillId + "</td>";

                    tr += "</tr>";

                    $("#BillInfo tbody").append(tr);
                    tr = "";
                }
            }
            CalculatePayment();
            $("#myTabs").tabs({ active: 0 });

            return false;
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#BillInfo tbody").html("");
            $("#ContentPlaceHolder1_hfCompanyPaymentId").val("0");
            $("#ContentPlaceHolder1_hfMemberId").val("0");
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

            $("#ContentPlaceHolder1_ddlCashPayment").val("");
            $("#ContentPlaceHolder1_ddlBankPayment").val("");
            $("#ContentPlaceHolder1_ddlCashAndCashEquivalantPayment").val("");
            //$("#ContentPlaceHolder1_ddlCompanyBank").val("");

            $("#txtCashPayment").val("");
            //$("#txtCompanyBank").val("");
            $("#txtBankPayment").val("");
            $("#ContentPlaceHolder1_txtChecqueNumber").val("");
            //$("#txtSupplierName").val("");
            $("#<%=txtPersonalEmail.ClientID %>").val("");
            $("#<%=txtDesignation.ClientID %>").val("");
            $("#<%=txtAddress.ClientID %>").val("");
            $("#<%=txtMobileNumber.ClientID %>").val("");
            $("#<%=txtMembershipNumber.ClientID %>").val("");
            $("#<%=txtOccupation.ClientID %>").val("");
            $("#txtMemberSearch").val("");
            $("#ContentPlaceHolder1_ddlMemberBill").val("0");
            $("#ContentPlaceHolder1_txtPaymentDate2").val("");

            $("#ContentPlaceHolder1_txtTotalAmount").val("");
            $("#ContentPlaceHolder1_txtAdjustmentAmount").val("");
            $("#ContentPlaceHolder1_ddlAdjustmentNodeHead").val('0').trigger('change');

            $("#ContentPlaceHolder1_hfPaymentId").val("0");
            $("#ContentPlaceHolder1_hfMemberBillId").val("0");
            $("#ContentPlaceHolder1_hfMemberId").val("0");

            $('#ConversionPanel').hide();
            if ($("#ContentPlaceHolder1_hfIsSavePermission").val() == "0")
                $("#btnSave").hide();
            else
                $("#btnSave").val("Save").show();

            $('#ContentPlaceHolder1_txtPaymentDate2').datepicker("setDate", DayOpenDate);
            $("#ContentPlaceHolder1_ddlPayMode").val("0");
            $('#ContentPlaceHolder1_ddlMemberBill').empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
            $("#btnSave").val("Save");
            return false;
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
        function ValidateForm() {
            if ($.trim($("#ContentPlaceHolder1_txtLedgerAmount").val()) == "" || $.trim($("#ContentPlaceHolder1_txtLedgerAmount").val()) == "0") {
                if ($("#ContentPlaceHolder1_ddlPayMode").val() != "0") {
                    toastr.warning("Please Give Receive Amount.");
                    $("#ContentPlaceHolder1_txtLedgerAmount").focus();
                    return false;
                }
            }
            else if ($("#ContentPlaceHolder1_txtPaymentDate2").val() == "") {
                toastr.warning("Please Give Payment Date.");
                return false;
            }
             else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "0") {
                if ($.trim($("#ContentPlaceHolder1_txtLedgerAmount").val()) != "" || $.trim($("#ContentPlaceHolder1_txtLedgerAmount").val()) != "0") {
                    toastr.warning("Please Select Payment Mode.");
                    $("#ContentPlaceHolder1_ddlPayMode").focus();
                    return false;
                }
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cash") {
                if ($("#ContentPlaceHolder1_ddlCashPayment").val() == "0" && $("#txtCashPayment").val() == "") {
                    toastr.warning("Please Select Cash Payment Account.");
                    return false;
                }
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Bank") {
                if ($("#ContentPlaceHolder1_ddlBankPayment").val() == "0" && $("#txtBankPayment").val() == "") {
                    toastr.warning("Please Select Bank Payment Account.");
                    return false;
                }
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cheque") {
                if ($("#ContentPlaceHolder1_ddlBankPayment").val() == "0" && $("#txtMemberBank").val() == "") {
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

            CommonHelper.SpinnerOpen();

            var totalPayment = 0.00, advanceAmount = 0.00, detailsId = 0, paymentId = 0, accountingPostingHeadId = 0;
            var MemberBillId = 0.00, adjustmentAccountHeadId = 0, paymentAdjustmentAmount = 0.00;
            var MemberPaymentDetails = new Array();
            var MemberPaymentDetailsEdited = new Array();
            var MemberPaymentDetailsDeleted = new Array();

            paymentId = $("#ContentPlaceHolder1_hfPaymentId").val();
            MemberBillId = $("#ContentPlaceHolder1_ddlMemberBill").val();
            adjustmentAccountHeadId = $("#ContentPlaceHolder1_ddlAdjustmentNodeHead").val();

            if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cash") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlCashPayment").val();
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Bank") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlBankPayment").val();
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Card") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlBankPayment").val();
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Loan") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlCashAndCashEquivalantPayment").val();
            }

            paymentAdjustmentAmount = $("#ContentPlaceHolder1_txtAdjustmentAmount").val() != "" ? parseFloat($("#ContentPlaceHolder1_txtAdjustmentAmount").val()) : 0.00;
            advanceAmount = $("#ContentPlaceHolder1_txtAdvanceAmount").val() == "" ? 0.00 : parseFloat($("#ContentPlaceHolder1_txtAdvanceAmount").val());
            if (paymentAdjustmentAmount != "") {
                if (adjustmentAccountHeadId == "0") {
                    toastr.warning("Please select adjustment Head");
                    $("#ContentPlaceHolder1_ddlAdjustmentNodeHead").focus();
                    return false;
                }
            }
            if (adjustmentAccountHeadId != "0") {
                if (paymentAdjustmentAmount == "") {
                    toastr.warning("Please insert adjustment amount.");
                    $("#ContentPlaceHolder1_txtAdjustmentAmount").focus();
                    return false;
                }
            }
            var MemberPayment = {
                PaymentId: paymentId,
                MemberBillId: MemberBillId,
                PaymentFor: 'Payment',
                MemberId: $("#ContentPlaceHolder1_hfMemberId").val(),
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
                    MemberPaymentDetails.push({
                        PaymentDetailsId: 0,
                        MemberBillDetailsId: $(this).find("td:eq(8)").text(),
                        MemberPaymentId: $(this).find("td:eq(9)").text(),
                        BillId: $(this).find("td:eq(10)").text(),
                        PaymentAmount: $(this).find("td:eq(5)").find("input").val()
                    });
                }
                else if ($(this).find("td:eq(0)").find("input").is(":checked") == true && detailsId > 0) {
                    MemberPaymentDetailsEdited.push({
                        PaymentDetailsId: detailsId,
                        MemberBillDetailsId: $(this).find("td:eq(8)").text(),
                        MemberPaymentId: $(this).find("td:eq(9)").text(),
                        BillId: $(this).find("td:eq(10)").text(),
                        PaymentAmount: $(this).find("td:eq(5)").find("input").val()
                    });
                }
                else if ($(this).find("td:eq(0)").find("input").is(":checked") == false && detailsId > 0) {
                    MemberPaymentDetailsDeleted.push({
                        PaymentDetailsId: detailsId,
                        MemberBillDetailsId: $(this).find("td:eq(8)").text(),
                        MemberPaymentId: $(this).find("td:eq(9)").text(),
                        BillId: $(this).find("td:eq(10)").text(),
                        PaymentAmount: $(this).find("td:eq(5)").find("input").val()
                    });
                }
            });

            PageMethods.SaveMemberBillPayment(MemberPayment, MemberPaymentDetails, MemberPaymentDetailsEdited, MemberPaymentDetailsDeleted, OnSupplierPaymentSucceeded, OnSupplierPaymentFailed);

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
            toastr.warning("Error On Bill Receive.");
            CommonHelper.SpinnerClose();
        }
        function OnFailed(error) {
            toastr.error(error.get_message());
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
    </script>
    <asp:HiddenField ID="hfPaymentId" runat="server" Value="0" />
    <asp:HiddenField ID="hfMemberPaymentId" runat="server" Value="0" />
    <asp:HiddenField ID="hfMemberBill" runat="server" />
    <asp:HiddenField ID="hfMemberBillId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsSavePermission" runat="server" />
    <asp:HiddenField ID="hfIsUpdatePermission" runat="server" />
    <asp:HiddenField ID="hfIsDeletePermission" runat="server" />
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
        <asp:HiddenField ID="txtCardValidation" runat="server"></asp:HiddenField>
    </div>
    <asp:HiddenField ID="hfMemberId" runat="server" />
    <div id="DivPaymentSelect" style="display: none;">
        <div id="Div1" class="panel panel-default">
            <div class="panel-body">
                <asp:HiddenField ID="hfCurrencyType" runat="server" />
                <asp:HiddenField ID="hfConversionRate" runat="server" />
                <asp:HiddenField ID="txtSelectedRoomNumbers" runat="server" />
                <asp:HiddenField ID="txtSelectedRoomId" runat="server" />
                <asp:HiddenField ID="HiddenFieldMemberPaymentButtonInfo" runat="server"></asp:HiddenField>
                <%--<asp:GridView ID="gvPaymentInfo" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
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
                                <asp:Label ID="lblid" runat="server" Text='<%#Eval("MemberPaymentId") %>'></asp:Label>
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
                </asp:GridView>--%>
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
                href="#tab-1">Payment Receive</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Bill Receive</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Member Payment Receive Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <%--<div class="form-group" style="display: none;">
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
                            <div id="MemberPaymentAccountHeadDiv" style="display: none;">
                                <asp:DropDownList ID="ddlMemberPaymentAccountHead" runat="server" CssClass="childDivSectionDivThreeColumnDropDownList">
                                </asp:DropDownList>
                            </div>
                        </div>--%>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSrcRoomNumber" runat="server" class="control-label required-field"
                                    Text="Member Name"></asp:Label>
                                <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtPaymentId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtDealId" runat="server"></asp:HiddenField>
                            </div>
                            <div class="col-md-10">
                                <input id="txtMemberSearch" type="text" class="form-control" name="cmpName" />
                                <div style="display: none;">
                                    <asp:DropDownList ID="ddlMember" TabIndex="1" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <%--<div class="col-md-2" style="width: 20%; display: none;">
                                <asp:Button ID="btnSrcCmpPayment" runat="server" Text="Search Payment" TabIndex="2"
                                    CssClass="TransactionalButton btn btn-primary" OnClick="btnSrcCmpPayment_Click" />
                            </div>--%>
                        </div>
                        <div id="MemberInfo" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblEmailAddress" runat="server" class="control-label" Text="Member Email"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPersonalEmail" runat="server" CssClass="form-control" TabIndex="3"
                                        disabled="disabled"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblWebAddress" runat="server" class="control-label" Text="Designation"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control" TabIndex="4"
                                        disabled="disabled"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblContactNumber" runat="server" class="control-label" Text="Contact Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtMobileNumber" runat="server" TabIndex="6" CssClass="form-control"
                                        disabled="disabled"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblTelephoneNumber" runat="server" class="control-label" Text="Occupation"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtOccupation" runat="server" TabIndex="7" CssClass="form-control"
                                        disabled="disabled"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblContactPerson" runat="server" class="control-label" Text="Membership No."></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtMembershipNumber" runat="server" CssClass="form-control" TabIndex="5"
                                        disabled="disabled"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblAddress" runat="server" class="control-label" Text="Address"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine"
                                        TabIndex="2" disabled="disabled"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label6" runat="server" class="control-label" Text="Member Bill"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlMemberBill" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
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
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                    <tfoot>
                                        <tr>
                                            <td colspan="5" style="width: 82%; text-align: right;">Advance Amount</td>
                                            <td style="width: 18%">
                                                <asp:TextBox ID="txtAdvanceAmount" runat="server" onblur="CheckAdvance(this)" CssClass="form-control quantitydecimal" TabIndex="7"> </asp:TextBox>
                                            </td>
                                        </tr>
                                    </tfoot>
                                </table>
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
                    </div>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-heading">Receive Info</div>
                <div class="panel-body">
                    <div class="form-horizontal">

                        <div class="form-group">
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label13" runat="server" class="control-label required-field"
                                    Text="Total Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTotalAmount" disabled="disabled" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
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
                                    <asp:ListItem Value="Loan">Member Loan</asp:ListItem>--%>
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
                            <div class="form-group" id="MemberProjectPanel" style="display: none;">
                                <div class="col-md-2">
                                    <asp:Label ID="lblGLMember" runat="server" class="control-label" Text="Member"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlGLMember" runat="server" CssClass="form-control" onchange="PopulateProjects();">
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
                                <asp:DropDownList ID="ddlCurrency" TabIndex="6" disabled="disabled" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                                <%--<asp:Label ID="lblDisplayConvertionRate" runat="server" class="control-label" Text=""></asp:Label>--%>
                                <asp:HiddenField ID="ddlCurrencyHiddenField" runat="server"></asp:HiddenField>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblReceiveAmount" runat="server" class="control-label required-field"
                                    Text="Payment Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtLedgerAmount" disabled="disabled" runat="server" CssClass="form-control quantitydecimal" TabIndex="7"> </asp:TextBox>
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
                                        Text="Member Name"></asp:Label>
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
                                    <asp:Label ID="lblMemberBank" runat="server" class="control-label required-field"
                                        Text="Bank Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <input id="txtMemberBank" type="text" class="form-control" />
                                    <div style="display: none;">
                                        <asp:DropDownList ID="ddlMemberBank" runat="server" CssClass="form-control" AutoPostBack="false">
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
                        <div id="CashAndCashEquivalantPaymentAccountHeadDiv" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label9" runat="server" class="control-label required-field"
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

            <div class="row">
                <div class="col-md-12">
                    <input type="button" value="Save" id="btnSave" class="TransactionalButton btn btn-primary btn-sm" onclick="ValidateForm()" />
                    <input type="button" value="Clear" id="btnClear" class="TransactionalButton btn btn-primary btn-sm" onclick="PerformClearAction()" />
                    <%--<asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="12" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClick="btnSave_Click" OnClientClick="javascript: return ValidateForm();" />--%>
                    <%--<asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="13" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return PerformClearAction();" />--%>
                    <%--<asp:Button ID="btnCancel" runat="server" TabIndex="14" Text="Close" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return EntryPanelVisibleFalse();" Visible="False" />--%>
                    <%-- <asp:Button ID="btnGroupPaymentPreview1" runat="server" Text="Payment Preview" TabIndex="15"
                        CssClass="TransactionalButton btn btn-primary btn-sm" OnClientClick="javascript: return LoadPopUp();" />--%>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Bill Receive Details
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label8" runat="server" class="control-label" Text="Member"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlMemberForSearch" runat="server" CssClass="form-control">
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
                                    <th style="width: 20%;">Ledger Number</th>
                                    <th style="width: 15%;">Payment Date</th>
                                    <th style="width: 20%;">Member Name</th>
                                    <th style="width: 20%;">Remarks</th>
                                    <th style="width: 15%;">Status</th>
                                    <th style="width: 10%;">Action</th>
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
</asp:Content>
