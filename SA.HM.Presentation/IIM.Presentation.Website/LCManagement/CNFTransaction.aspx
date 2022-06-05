<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="CNFTransaction.aspx.cs" Inherits="HotelManagement.Presentation.Website.LCManagement.CNFTransaction" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var currencyType = "";
        var flag = 0;

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

        function CurrencyRateInfoEnable() {
            var ddlCurrency = $("#<%=ddlCurrencyType.ClientID %>").val();
            if (ddlCurrency == 0) {
                $('#ConversionPanel').hide()
            }
        }
        $(document).ready(function () {
            CommonHelper.ApplyDecimalValidation();
            $('#ConversionPanel').hide();
            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtCalculatedLedgerAmount').attr("disabled", true);
            $('#ContentPlaceHolder1_ddlCNFName').select2({
                tags: true,
                width: "100%",
                dropdownParent: $("#CreateNewDialog")
            });
            $('#ContentPlaceHolder1_ddlCompanyBank').select2({
                tags: true,
                width: "100%",
                dropdownParent: $("#CreateNewDialog")
            });
            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtPaymentDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
            });
            $("#<%=ddlPaymentMode.ClientID%>").change(function () {
                if ($(this).val() == "Cheque") {
                    $("#ChecquePaymentAccountHeadDiv").show();
                }
                else {
                    $("#ChecquePaymentAccountHeadDiv").hide();
                }
            });
            GridPaging(1, 1);
        });

        function CreateNew() {
            PerformClearAction();
            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '75%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "CNF Transaction Information",
                show: 'slide'
            });

            return false;
        }
        function SaveOrUpdateCNFTransaction() {
            var Id = $("#ContentPlaceHolder1_hfId").val();
            var CNFId = $("#ContentPlaceHolder1_ddlCNFName").val();
            if (CNFId == "0" || !($.isNumeric(CNFId))) {
                toastr.warning("Select the CNF Name");
                $("#ContentPlaceHolder1_ddlCNFName").focus();
                return false;
            }
            var TransactionType = $("#ContentPlaceHolder1_ddlTransactionType").val();
            var PaymentMode = $("#ContentPlaceHolder1_ddlPaymentMode").val();
            var PaymentDate = $("#ContentPlaceHolder1_txtPaymentDate").val();
            if (PaymentDate == "") {
                toastr.warning("Select the Payment Date");
                $("#ContentPlaceHolder1_txtPaymentDate").focus();
                return false;
            }

            PaymentDate = CommonHelper.DateFormatToMMDDYYYY(PaymentDate, '/');

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
            var Remarks = $("#ContentPlaceHolder1_txtRemarks").val();

            if (Remarks == "") {
                toastr.warning("Select Enter Remarks.");
                $("#ContentPlaceHolder1_txtRemarks").focus();
                return false;
            }

            var CNFTransactionBO = {
                Id: Id,
                CNFId: CNFId,
                TransactionType: TransactionType,
                PaymentMode: PaymentMode,
                PaymentDate: PaymentDate,
                ChequeNumber: ChequeNumber,
                BankId: BankId,
                CurrencyId: CurrencyId,
                PaymentAmount: PaymentAmount,
                ConversionRate: ConversionRate,
                Remarks: Remarks,
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../LCManagement/CNFTransaction.aspx/SaveUpdateCNFTransaction',

                data: JSON.stringify({ CNFTransactionBO: CNFTransactionBO }),
                dataType: "json",
                success: function (data) {
                    GridPaging($("#GridPagingContainer").find("li.active").index(), 1);
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
            $("#ContentPlaceHolder1_ddlCNFName").focus();
        }
        function SaveAndClose() {
            flag = 1;
            SaveOrUpdateCNFTransaction();
            return false;

        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadGrid(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function LoadGrid(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#CNFTransactionTable tbody tr").length;
            var fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();
            var transactionType = $("#ContentPlaceHolder1_ddlTransactionTypeSearch").val();
            if (fromDate == "") {
                fromDate = "01/01/1970";
            }
            if (toDate == "") {
                toDate = new Date();
                toDate = ((toDate.getMonth() > 8) ? (toDate.getMonth() + 1) : ('0' + (toDate.getMonth() + 1))) + '/' + ((toDate.getDate() > 9) ? toDate.getDate() : ('0' + toDate.getDate())) + '/' + toDate.getFullYear();

            }
            fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../LCManagement/CNFTransaction.aspx/LoadTransactionSearch',

                data: "{'fromDate':'" + fromDate + "','toDate':'" + toDate + "','transactionType':'" + transactionType + "', 'gridRecordsCount':'" + gridRecordsCount + "', 'pageNumber':'" + pageNumber + "', 'isCurrentOrPreviousPage':'" + IsCurrentOrPreviousPage + "'}",
                dataType: "json",
                success: function (data) {
                    LoadTable(data);
                },
                error: function (result) {
                    PerformClearAction();
                }
            });
            return false;
        }
        function LoadTable(data) {
            $("#CNFTransactionTable tbody").empty();
            $("#GridPagingContainer ul").empty();
            i = 0;

            $.each(data.d.GridData, function (count, gridObject) {

                var tr = "";

                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:25%;'>" + gridObject.TransactionNo + "</td>";
                tr += "<td style='width:20%;'>" + moment(gridObject.PaymentDate).format("DD/MM/YYYY") + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.TransactionType + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.PaymentAmount + "</td>";

                tr += "<td style=\"text-align: center; width:15%; cursor:pointer;\">";
                if (gridObject.IsCanEdit) {
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return FillFormEdit(" + gridObject.Id + ");\" alt='Edit'  title='Edit' border='0' />";
                }

                if (gridObject.IsCanDelete) {
                    tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return DeleteTransaction(" + gridObject.Id + ");\" alt='Delete'  title='Delete' border='0' />";
                }

                if (gridObject.IsCanChecked) {
                    tr += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return TransactionApprovalWithConfirmation('" + 'Checked' + "'," + gridObject.Id + ")\" alt='Checked'  title='Checked' border='0' />";

                }

                if (gridObject.IsCanApproved) {
                    tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return TransactionApprovalWithConfirmation('" + 'Approved' + "', " + gridObject.Id + ")\" alt='Approved'  title='Approved' border='0' />";
                }
                tr += "</td>";
                tr += "<td style='display:none'>" + gridObject.Id + "</td>";


                tr += "</tr>";

                $("#CNFTransactionTable tbody").append(tr);

                tr = "";
                i++;
            });
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.NextButton);
            return false;
        }
        function FillFormEdit(id) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../LCManagement/CNFTransaction.aspx/FillForm',

                data: "{ 'id':'" + id + "'}",
                dataType: "json",
                success: function (data) {
                    $("#CreateNewDialog").dialog({
                        autoOpen: true,
                        modal: true,
                        width: '75%',
                        closeOnEscape: true,
                        resizable: false,
                        height: 'auto',
                        fluid: true,
                        title: "CNF Transaction Information",
                        show: 'slide'
                    });
                    $("#ContentPlaceHolder1_hfId").val(data.d.Id);
                    $("#ContentPlaceHolder1_ddlCNFName").val(data.d.CNFId).trigger('change');
                    $("#ContentPlaceHolder1_ddlTransactionType").val(data.d.TransactionType).trigger('change');
                    $("#ContentPlaceHolder1_ddlPaymentMode").val(data.d.PaymentMode).trigger('change');
                    $("#ContentPlaceHolder1_txtPaymentDate").val(moment(data.d.PaymentDate).format("DD/MM/YYYY"));
                    $("#ContentPlaceHolder1_txtChecqueNumber").val(data.d.ChequeNumber);
                    $("#ContentPlaceHolder1_ddlCompanyBank").val(data.d.BankId).trigger('change');
                    $("#ContentPlaceHolder1_ddlCurrencyType").val(data.d.CurrencyId).trigger('change');
                    $("#ContentPlaceHolder1_txtPaymentAmount").val(data.d.PaymentAmount);
                    $("#ContentPlaceHolder1_txtConversionRate").val(data.d.ConversionRate).trigger('change');
                    $("#ContentPlaceHolder1_txtRemarks").val(data.d.Remarks);
                },
                error: function (result) {
                    PerformClearAction();
                }
            });
            return false;
        }
        function PerformClearAction() {
            $("#ContentPlaceHolder1_hfId").val("0");
            $("#ContentPlaceHolder1_ddlCNFName").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlTransactionType").val("Payment");
            $("#ContentPlaceHolder1_ddlPaymentMode").val("Cash").trigger('change');
            $("#ContentPlaceHolder1_txtPaymentDate").val("");
            $("#ContentPlaceHolder1_txtChecqueNumber").val("");
            $("#ContentPlaceHolder1_ddlCompanyBank").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlCurrencyType").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtPaymentAmount").val("");
            $("#ContentPlaceHolder1_txtConversionRate").val("");
            $("#ContentPlaceHolder1_txtRemarks").val("");
        }
        function DeleteTransaction(id) {
            if (!confirm("Do you want to Delete?")) {
                return false;
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../LCManagement/CNFTransaction.aspx/DeleteTransaction',
                data: "{'Id':'" + id + "'}",
                dataType: "json",
                success: function (data) {
                    CommonHelper.AlertMessage(data.d.AlertMessage);
                    GridPaging($("#GridPagingContainer").find("li.active").index(), 1);
                },
                error: function (result) {

                }
            });
            return false;
        }
        function TransactionApprovalWithConfirmation(ApprovedStatus, Id) {
            if (!confirm('Do you want to ' + (ApprovedStatus == 'Checked' ? 'check' : 'approve') + " ?")) {
                return false;
            }
            TransactionApproval(ApprovedStatus, Id);
        }
        function TransactionApproval(ApprovedStatus, Id) {

            PageMethods.TransactionApproval(Id, ApprovedStatus, OnApprovalSucceed, OnApprovalFailed);
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
        function Clean() {
            $("#ContentPlaceHolder1_txtFromDate").val("");
            $("#ContentPlaceHolder1_txtToDate").val("");
            $("#ContentPlaceHolder1_ddlTransactionTypeSearch").val("0");
            return false;
        }
    </script>
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCurrencyType" runat="server" />
    <asp:HiddenField ID="hfConversionRate" runat="server" />
    <div class="panel panel-default">
        <div class="panel-heading">
            CNF Transaction Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="">From Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="">To Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="">Transaction Type</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTransactionTypeSearch" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0">All</asp:ListItem>
                            <asp:ListItem Value="Payment">Payment</asp:ListItem>
                            <asp:ListItem Value="Receive">Receive</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return GridPaging(1,1);" />
                        <asp:Button ID="btnClean" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return Clean();" />
                        <asp:Button ID="btnCreateNew" runat="server" Text="New CNF Transaction" CssClass="TransactionalButton btn btn-primary btn-sm"
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
            <div class="form-group" id="CNFTransactionTableContainer">
                <table class="table table-bordered table-condensed table-responsive" id="CNFTransactionTable"
                    style="width: 100%;">
                    <thead>
                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                            <th style="width: 25%;">Invoice No
                            </th>
                            <th style="width: 20%;">Date
                            </th>
                            <th style="width: 20%;">Transaction Type
                            </th>
                            <th style="width: 20%;">Amount
                            </th>
                            <th style="width: 15%;">Action
                            </th>
                            <th style="display: none">Id
                            </th>
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
    </div>
    <div id="CreateNewDialog" style="display: none; overflow: unset">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2">
                    <label class=" required-field">CNF Name</label>
                </div>
                <div class="col-md-10">
                    <asp:DropDownList ID="ddlCNFName" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <label class=" required-field">Transaction Type</label>
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
                <div class="col-md-2">
                    <label class=" required-field">Payment Date</label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtPaymentDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
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
                <div class="col-md-2">
                    <label class="required-field">Remarks</label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" Style="resize: none;"></asp:TextBox>
                </div>
            </div>
            <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                <div class="col-md-12">
                    <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="SaveAndClose()" value="Save & Close" />
                    <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return PerformClearAction();" />
                    <input id="btnSaveNContinue" type="button" value="Save & Continue" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return SaveOrUpdateCNFTransaction();" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
