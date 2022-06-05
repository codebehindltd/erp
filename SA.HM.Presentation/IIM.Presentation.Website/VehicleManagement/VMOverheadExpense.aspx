<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="VMOverheadExpense.aspx.cs" Inherits="HotelManagement.Presentation.Website.VehicleManagement.VMOverheadExpense" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        var hasCNF = false;
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Service Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            CommonHelper.ApplyDecimalValidation();
            $('#ConversionPanel').hide();
            $("#<%=ddlPaymentMode.ClientID%>").change(function () {
                if ($(this).val() == "Cheque") {
                    $("#ChecquePaymentAccountHeadDiv").show();
                }
                else {
                    $("#ChecquePaymentAccountHeadDiv").hide();
                }
            });
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            $("#ContentPlaceHolder1_ddlOverHeadId").select2({
                tags: true,
                width: "100%",
                dropdownParent: $("#CreateNewDialog")
            });
            $("#ContentPlaceHolder1_ddlIncomeAccountHead").select2({
                tags: true,
                width: "100%",
                dropdownParent: $("#CreateNewDialog")
            });
            $("#ContentPlaceHolder1_ddlSrcVehicleId").select2();
            $("#ContentPlaceHolder1_ddlSrcOverHeadId").select2();
            $(<%=ddlCompanyBank.ClientID%>).select2({
                width: "100%",
                tags: true,
                dropdownParent: $("#CreateNewDialog")
            });
            $("#ContentPlaceHolder1_ddlVehicleId").select2({
                width: "100%",
                tags: true,
                dropdownParent: $("#CreateNewDialog")
            });
            $("#ContentPlaceHolder1_ddlCNFName").select2({
                width: "100%",
                tags: true,
                dropdownParent: $("#CreateNewDialog")
            });

            $('#ContentPlaceHolder1_txtStartDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtEndDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtEndDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtStartDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            CommonHelper.ApplyDecimalValidation();

            var txtExpenseDate = '<%=txtExpenseDate.ClientID%>'
            $('#' + txtExpenseDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#btnSearch").click(function () {
                GridPaging(1, 1);
            });
            GridPaging(1, 1);
            $("#gvPaidServiceInformation").delegate("td > img.PaidServiceDelete", "click", function () {
                var answer = confirm("Do you want to delete this record?")
                if (answer) {
                    toastr.info(itemId);
                    var itemId = $.trim($(this).parent().parent().find("td:eq(3)").text());
                    var params = JSON.stringify({ sEmpId: itemId });

                    var $row = $(this).parent().parent();
                    $.ajax({
                        type: "POST",
                        url: "/VehicleManagement/VMOverheadExpense.aspx/DeletePaidServiceById",
                        data: params,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $row.remove();
                            CommonHelper.AlertMessage(data.d.AlertMessage);
                            $("#myTabs").tabs('load', 1);
                        },
                        error: function (error) {
                        }
                    });
                }
            });
            $("#ContentPlaceHolder1_ddlTransactionType").change(function () {
                if ($("#ContentPlaceHolder1_ddlTransactionType").val() == 'Payment') {
                    $("#ContentPlaceHolder1_lblIncomeAccountHead").text("Payment Head");
                }
                else {
                    $("#ContentPlaceHolder1_lblIncomeAccountHead").text("Receive Head");
                }
            });

        });
        $(function () {
            var ddlCurrency = '<%=ddlCurrencyType.ClientID%>'
            var txtConversionRate = '<%=txtConversionRate.ClientID%>'
            var txtLedgerAmount = '<%=txtPaymentAmount.ClientID%>'
            var txtCalculatedLedgerAmount = '<%=txtCalculatedLedgerAmount.ClientID%>'
            var txtCalculatedLedgerAmountHiddenField = '<%=txtCalculatedLedgerAmountHiddenField.ClientID%>'

            $('#' + ddlCurrency).change(function () {
                var v = $("#<%=ddlCurrencyType.ClientID %>").val();
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
                    $("#<%=txtCalculatedLedgerAmount.ClientID %>").attr("disabled", false);
                }
                else {
                    var registrationWiseConversionRate = 0;
                    if ($("#<%=txtConversionRateHiddenField.ClientID %>").val() == "") {
                        registrationWiseConversionRate = result.ConversionRate;
                    }
                    else if (parseFloat($("#<%=txtConversionRateHiddenField.ClientID %>").val()) == 0) {
                        registrationWiseConversionRate = result.ConversionRate;
                    }
                    else {
                        registrationWiseConversionRate = $("#<%=txtConversionRateHiddenField.ClientID %>").val();
                    }

                    if ($("#<%=ddlCurrencyType.ClientID %>").val() == "2") {
                        $("#<%=txtConversionRate.ClientID %>").val(registrationWiseConversionRate);
                        $("#<%=hfConversionRate.ClientID %>").val(registrationWiseConversionRate);
                    }
                    else {
                        $("#<%=txtConversionRate.ClientID %>").val(result.ConversionRate);
                        $("#<%=hfConversionRate.ClientID %>").val(result.ConversionRate);
                    }

                    $('#ConversionPanel').show();
                    $('#' + txtCalculatedLedgerAmount).val("");
                }
                CurrencyRateInfoEnable();
                if ($('#' + txtLedgerAmount).val() != "" && $('#' + txtConversionRate).val() != "") {
                    CalculateTotalAmount();
                }
            }
            function OnLoadConversionRateFailed() {
            }
            $('#' + txtLedgerAmount).blur(function () {
                currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
                if (currencyType == "Local") {
                    var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val());
                    $('#' + txtCalculatedLedgerAmount).val(LedgerAmount.toFixed(2));
                    $('#' + txtCalculatedLedgerAmountHiddenField).val(LedgerAmount.toFixed(2));
                }
                else {
                    CalculateTotalAmount();
                }
            });

            $('#' + txtConversionRate).blur(function () {
                currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
                if (currencyType == "Local") {
                    var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val());
                    $('#' + txtCalculatedLedgerAmount).val(LedgerAmount.toFixed(2));
                    $('#' + txtCalculatedLedgerAmountHiddenField).val(LedgerAmount.toFixed(2));
                }
                else {
                    CalculateTotalAmount();
                }
            });
            function CalculateTotalAmount() {
                var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val()) * parseFloat($('#' + txtConversionRate).val());
                if (isNaN(LedgerAmount.toString())) {
                    LedgerAmount = 0;
                }
                $('#' + txtCalculatedLedgerAmount).val(LedgerAmount.toFixed(2));
                $('#' + txtCalculatedLedgerAmountHiddenField).val(LedgerAmount.toFixed(2));
                $('#' + txtCalculatedLedgerAmount).attr("disabled", true);
            }
        });

        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }
        $(function () {
            $("#myTabs").tabs();
        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#gvPaidServiceInformation tbody tr").length;

            var fromDate = $("#ContentPlaceHolder1_txtStartDate").val();
            var toDate = $("#ContentPlaceHolder1_txtEndDate").val();
            if (fromDate == "")
                fromDate = '01/01/1970';
            if (toDate == "") {
                toDate = new Date();
                toDate = ((toDate.getMonth() > 8) ? (toDate.getMonth() + 1) : ('0' + (toDate.getMonth() + 1))) + '/' + ((toDate.getDate() > 9) ? toDate.getDate() : ('0' + toDate.getDate())) + '/' + toDate.getFullYear();
            }
            else {
                toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');
            }
            //if (filterBy == "Custom" || filterBy == "Overdue" || filterBy == "Completed") {
            fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');


            var serviceName = $("#<%=ddlSrcVehicleId.ClientID %>").val();
            var overHeadId = $("#<%=ddlSrcOverHeadId.ClientID %>").val();

            PageMethods.SearchPaidServiceAndLoadGridInformation(fromDate, toDate, serviceName, overHeadId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {
            $("#gvPaidServiceInformation tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvPaidServiceInformation tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#gvPaidServiceInformation tbody tr").length;
                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:10%; cursor:pointer;\">" + gridObject.TransactionNo + "</td>";
                tr += "<td align='left' style=\"width:40%; cursor:pointer;\">" + gridObject.OverheadName + "</td>";
                tr += "<td align='left' style=\"width:10%; cursor:pointer;\">" + gridObject.ExpenseAmount + "</td>";

                tr += "<td style=\"text-align: center; width:15%; cursor:pointer;\">";
                if (gridObject.IsCanEdit) {
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction(" + gridObject.Id + ");\" alt='Edit'  title='Edit' border='0' />";
                }
                if (gridObject.IsCanDelete) {
                    tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'DeleteOverHeadExpense(" + gridObject.Id + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                }
                if (gridObject.IsCanChecked) {
                    tr += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return ExpenseApprovalWithConfirmation('" + 'Checked' + "'," + gridObject.Id + ")\" alt='Checked'  title='Checked' border='0' />";

                }
                if (gridObject.IsCanApproved) {
                    tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return ExpenseApprovalWithConfirmation('" + 'Approved' + "', " + gridObject.Id + ")\" alt='Approved'  title='Approved' border='0' />";
                }
                tr += "</td>";
                tr += "<td align='right' style=\"width:5%; display:none;\">" + gridObject.Id + "</td>";
                tr += "</tr>"

                $("#gvPaidServiceInformation tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            return false;
        }
        function OnLoadObjectFailed(error) {
            alert(error.get_message());
        }
        function PerformEditAction(itemId) {
            var possiblePath = "VMOverheadExpense.aspx?editId=" + itemId;
            window.location = possiblePath;
        }
        function SaveOrUpdateOverHeadExpense() {
            var id = $("#ContentPlaceHolder1_hfId").val()
            var VehicleId = $("#ContentPlaceHolder1_ddlVehicleId").val();
            if (VehicleId == "0" || !($.isNumeric(VehicleId))) {
                toastr.warning("Select Vehicle Number");
                $("#ContentPlaceHolder1_ddlVehicleId").focus();
                return false;
            }
            var transactionType = $("#ContentPlaceHolder1_ddlTransactionType").val();
            var transactionAccountHeadId = $("#ContentPlaceHolder1_ddlIncomeAccountHead").val();
            if (transactionAccountHeadId == "0") {
                if (transactionType == 'Payment') {
                    toastr.warning("Select Payment Head");
                    $("#ContentPlaceHolder1_ddlIncomeAccountHead").focus();
                }
                else {
                    toastr.warning("Select Receive Head");
                    $("#ContentPlaceHolder1_ddlIncomeAccountHead").focus();
                }
                return false;
            }
            var overHeadId = $("#ContentPlaceHolder1_ddlOverHeadId").val();
            if (overHeadId == "0" || !($.isNumeric(overHeadId))) {
                toastr.warning("Select Expense Head");
                $("#ContentPlaceHolder1_ddlOverHeadId").focus();
                return false;
            }
            var cNFId = $("#ContentPlaceHolder1_ddlCNFName").val();
            if (hasCNF && cNFId == "0") {
                toastr.warning("Select CNF Name");
                $("#ContentPlaceHolder1_ddlCNFName").focus();
                return false;
            }
            var expenseDate = $("#ContentPlaceHolder1_txtExpenseDate").val();

            if (expenseDate == "") {
                toastr.warning("Enter Expense Date");
                $("#ContentPlaceHolder1_txtExpenseDate").focus();
                return false;
            }
            expenseDate = CommonHelper.DateFormatToMMDDYYYY(expenseDate, '/');
            var expenseAmount = $("#ContentPlaceHolder1_txtExpenseAmount").val();
            if (expenseAmount == "") {
                toastr.warning("Enter Expense Amount");
                $("#ContentPlaceHolder1_txtExpenseAmount").focus();
                return false;
            }
            var PaymentMode = $("#ContentPlaceHolder1_ddlPaymentMode").val();

            var ChequeNumber = $("#ContentPlaceHolder1_txtChecqueNumber").val();
            if (PaymentMode == "Cheque" && ChequeNumber == "") {
                toastr.warning("Enter the Cheque Number");
                $("#ContentPlaceHolder1_txtChecqueNumber").focus();
                return false;
            }
            var BankId = $("#ContentPlaceHolder1_ddlCompanyBank").val();
            if (PaymentMode == "Cheque" && (BankId == "0" || !($.isNumeric(BankId)))) {
                toastr.warning("Select the Bank Name");
                $("#ContentPlaceHolder1_ddlCompanyBank").focus();
                return false;
            }
            if (PaymentMode == "Cash") {
                ChequeNumber = "";
                BankId = "0";
            }
            var CurrencyId = $("#ContentPlaceHolder1_ddlCurrencyType").val();
            if (CurrencyId == "0") {
                toastr.warning("Select the Currency Type");
                $("#ContentPlaceHolder1_ddlCurrencyType").focus();
                return false;
            }
            var PaymentAmount = $("#ContentPlaceHolder1_txtPaymentAmount").val();
            if (PaymentAmount == "" || parseFloat(PaymentAmount) == 0) {
                toastr.warning("Enter payment amount");
                $("#ContentPlaceHolder1_txtPaymentAmount").focus();
                return false;
            }
            var ConversionRate = $("#ContentPlaceHolder1_txtConversionRate").val();
            var description = $("#ContentPlaceHolder1_txtDescription").val();

            var VMOverheadExpenseBO = {
                Id: id,
                VehicleId: VehicleId,
                TransactionType: transactionType,
                TransactionAccountHeadId: transactionAccountHeadId,
                OverHeadId: overHeadId,
                CNFId: cNFId,
                PaymentMode: PaymentMode,
                ChequeNumber: ChequeNumber,
                BankId: BankId,
                CurrencyId: CurrencyId,
                ConversionRate: ConversionRate,
                ExpenseDate: expenseDate,
                ExpenseAmount: PaymentAmount,
                Description: description,
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../VehicleManagement/VMOverheadExpense.aspx/SaveOrUpdateOverHeadExpense',

                data: JSON.stringify({ VMOverheadExpenseBO: VMOverheadExpenseBO }),
                dataType: "json",
                success: function (data) {
                    GridPaging(1, 1);
                    PerformClearAction();
                    CommonHelper.AlertMessage(data.d.AlertMessage);
                    if (flag == 1) {
                        $('#CreateNewDialog').dialog('close');
                    }
                    flag = 0;
                },
                error: function (result) {

                }
            });
            return false;
        }
        function CreateNew() {
            PerformClearAction();
            $("#ContentPlaceHolder1_ddlVehicleId").val($("#ContentPlaceHolder1_ddlSrcVehicleId").val()).trigger('change');
            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '85%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Over Head Expense",
                show: 'slide'
            });

            return false;
        }
        function PerformEditAction(Id) {
            FillForm(Id);
        }
        function FillForm(Id) {
            $("#ContentPlaceHolder1_btnClear").hide();

            CommonHelper.SpinnerOpen();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../VehicleManagement/VMOverheadExpense.aspx/FillForm',

                data: "{'Id':'" + Id + "'}",
                dataType: "json",
                success: function (data) {
                    if (!confirm("Do you want to edit ")) {
                        return false;
                    }
                    $("#CreateNewDialog").dialog({
                        autoOpen: true,
                        modal: true,
                        width: '75%',
                        closeOnEscape: true,
                        resizable: false,
                        height: 'auto',
                        fluid: true,
                        title: "Update Over Head Expense",
                        show: 'slide'
                    });
                    $("#btnSave").val("Update And Close");
                    $("#btnClear").hide();
                    $("#btnSaveNContinue").hide();
                    // $("#AddNewStatusContaiiner").dialog({ title: "Edit Source - " + data.d.SourceName + " " });
                    $("#ContentPlaceHolder1_hfId").val(data.d.Id);
                    $("#ContentPlaceHolder1_ddlVehicleId").val(data.d.VehicleId).trigger('change');
                    $("#ContentPlaceHolder1_ddlTransactionType").val(data.d.TransactionType).trigger('change');
                    $("#ContentPlaceHolder1_ddlOverHeadId").val(data.d.OverheadId).trigger('change');
                    $("#ContentPlaceHolder1_txtExpenseDate").val(moment(data.d.ExpenseDate).format("DD/MM/YYYY"));
                    $("#ContentPlaceHolder1_txtDescription").val(data.d.Description);
                    $("#ContentPlaceHolder1_ddlIncomeAccountHead").val(data.d.TransactionAccountHeadId).trigger('change');
                    $("#ContentPlaceHolder1_ddlPaymentMode").val(data.d.PaymentMode).trigger('change');
                    $("#ContentPlaceHolder1_txtChecqueNumber").val(data.d.ChequeNumber);
                    $("#ContentPlaceHolder1_ddlCompanyBank").val(data.d.BankId).trigger('change');
                    $("#ContentPlaceHolder1_ddlCurrencyType").val(data.d.CurrencyId).trigger('change');
                    $("#ContentPlaceHolder1_txtPaymentAmount").val(data.d.ExpenseAmount).trigger('change');
                    $("#ContentPlaceHolder1_txtConversionRate").val(data.d.ConversionRate).trigger('change');
                    CommonHelper.SpinnerClose();
                },
                error: function (result) {

                }
            });
            return false;
        }
        function SaveAndClose() {
            flag = 1;
            SaveOrUpdateOverHeadExpense();
            return false;

        }
        function DeleteOverHeadExpense(Id) {
            if (!confirm("Do you want to Delete?")) {
                return false;
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../VehicleManagement/VMOverheadExpense.aspx/DeleteVehicleOverHeadExpense',
                data: "{'Id':'" + Id + "'}",
                dataType: "json",
                success: function (data) {
                    CommonHelper.AlertMessage(data.d.AlertMessage);
                    GridPaging(1, 1);
                },
                error: function (result) {

                }
            });
            return false;
        }
        function PerformClearAction() {
            $("#ContentPlaceHolder1_hfId").val("0");
            $("#ContentPlaceHolder1_ddlVehicleId").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlTransactionType").val("Payment").trigger('change');
            $("#ContentPlaceHolder1_ddlOverHeadId").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlCNFName").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtExpenseDate").val("");
            $("#ContentPlaceHolder1_txtExpenseAmount").val("");
            $("#ContentPlaceHolder1_txtDescription").val("");
            $("#ContentPlaceHolder1_ddlIncomeAccountHead").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlPaymentMode").val("Cash").trigger('change');
            $("#ContentPlaceHolder1_txtChecqueNumber").val("");
            $("#ContentPlaceHolder1_ddlCompanyBank").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlCurrencyType").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtPaymentAmount").val("");
            $("#ContentPlaceHolder1_txtConversionRate").val("");
        }
        function Clean() {
            $("#ContentPlaceHolder1_ddlSrcVehicleId").val("0").trigger('change');
            return false;
        }
        function ExpenseApprovalWithConfirmation(ApprovedStatus, Id) {
            if (!confirm('Do you want to ' + (ApprovedStatus == 'Checked' ? 'check' : 'approve') + " ?")) {
                return false;
            }
            ExpenseApproval(ApprovedStatus, Id);
        }
        function ExpenseApproval(ApprovedStatus, Id) {

            PageMethods.ExpenseApproval(Id, ApprovedStatus, OnApprovalSucceed, OnApprovalFailed);
        }

        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);

                GridPaging($("#GridPagingContainer").find("li.active").index(), 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnApprovalFailed() {

        }
        function CurrencyRateInfoEnable() {
            var ddlCurrency = $("#<%=ddlCurrencyType.ClientID %>").val();
            if (ddlCurrency == 0) {
                $('#ConversionPanel').hide()
            }
        }
    </script>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCurrencyType" runat="server" />
    <asp:HiddenField ID="hfConversionRate" runat="server" />

    <div id="MessageBox" class="alert alert-info" style="display: none">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="CreateNewDialog" style="display: none; overflow: unset">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2 ">
                    <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Vehicle Number"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlVehicleId" runat="server" CssClass="form-control" AutoPostBack="false">
                    </asp:DropDownList>
                </div>
                <div class="col-md-2 ">
                    <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Transaction Type"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlTransactionType" runat="server" CssClass="form-control">
                        <asp:ListItem Value="Payment">Payment</asp:ListItem>
                        <asp:ListItem Value="Receive">Receive</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <label class=" required-field">Payment Mode</label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlPaymentMode" runat="server" CssClass="form-control">
                        <asp:ListItem Value="Cash">Cash</asp:ListItem>
                        <asp:ListItem Value="Cheque">Cheque</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div id="ChecquePaymentAccountHeadDiv" style="display: none;">
                <div class="form-group" style="display: none;">
                    <label for="CompanyName" class=" col-md-2">
                        Company Name</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlChecquePaymentAccountHeadId" runat="server" CssClass="form-control"
                            TabIndex="6">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label for="ChequeNumber" class=" col-md-2 required-field">
                        Cheque Number</label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtChecqueNumber" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="BankName" class=" col-md-2 required-field">
                        Bank Name</label>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlCompanyBank" runat="server" CssClass="form-control" AutoPostBack="false">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-2">
                    <label class=" required-field">Currency Type</label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlCurrencyType" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <label class=" required-field">Payment Amount</label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtPaymentAmount" runat="server" CssClass="quantitydecimal form-control required-field"></asp:TextBox>
                </div>
            </div>

            <div id="ConversionPanel" class="form-group">
                <label for="ConversionRate" class="col-md-2">
                    Conversion Rate</label>
                <div class="col-md-4">
                    <asp:TextBox ID="txtConversionRate" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:HiddenField ID="txtConversionRateHiddenField" runat="server"></asp:HiddenField>
                </div>
                <label for="CalculatedAmount" class="col-md-2">
                    Calculated Amount</label>
                <div class="col-md-4">
                    <asp:HiddenField ID="txtCalculatedLedgerAmountHiddenField" runat="server"></asp:HiddenField>
                    <asp:TextBox ID="txtCalculatedLedgerAmount" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2 ">
                    <asp:Label ID="lblOverHeadId" runat="server" class="control-label required-field" Text="Expense Head"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlOverHeadId" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="col-md-2 ">
                    <asp:Label ID="lblIncomeAccountHead" runat="server" class="control-label required-field" Text="Payment Head"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlIncomeAccountHead" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2 ">
                    <asp:Label ID="Label5" runat="server" class="control-label required-field" Text="Expense Date"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtExpenseDate" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2 ">
                    <asp:Label ID="lblDescription" runat="server" class="control-label" Text="Description"></asp:Label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
            <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                <div class="col-md-12">
                    <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="SaveAndClose()" value="Save & Close" />
                    <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return PerformClearAction();" />
                    <input id="btnSaveNContinue" type="button" value="Save & Continue" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return SaveOrUpdateOverHeadExpense();" />
                </div>
            </div>
        </div>
    </div>

    <div id="SearchEntry" class="panel panel-default">
        <div class="panel-heading">
            Search Overhead Expense
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStartDate" autocomplete="off" CssClass="form-control" runat="server"></asp:TextBox><input
                            type="hidden" id="hidFromDate" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEndDate" autocomplete="off" CssClass="form-control" runat="server"></asp:TextBox>
                        <input type="hidden" id="hidToDate" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label7" runat="server" class="control-label" Text="Vehicle Number"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlSrcVehicleId" runat="server" CssClass="form-control" AutoPostBack="false">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSrcOverHeadId" runat="server" class="control-label required-field" Text="Expense Head"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlSrcOverHeadId" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <button type="button" id="btnSearch" class="btn btn-primary btn-sm">
                            Search</button>
                        <asp:Button ID="btnClean" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return Clean();" />
                        <asp:Button ID="btnCreateNew" runat="server" Text="New Vehicle Overhead" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateNew();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information
        </div>
        <div class="panel-body">
            <table class="table table-bordered table-condensed table-responsive" id="gvPaidServiceInformation" width="100%">
                <colgroup>
                    <col style="width: 10%;" />
                    <col style="width: 40%;" />
                    <col style="width: 10%;" />
                    <col style="width: 15%;" />
                </colgroup>
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                        <td>Transaction No
                        </td>
                        <td>Expense Head
                        </td>
                        <td>Amount
                        </td>
                        <td style="text-align: center;">Option
                        </td>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
            <div class="childDivSection">
                <div class="text-center" id="GridPagingContainer">
                    <ul class="pagination">
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
