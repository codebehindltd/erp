<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmVoucherEntry.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.frmVoucherEntry"
    EnableEventValidation="false" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var NodeMatrix = new Array();
        var Currency = new Array();
        var ComanyList = new Array();
        var FilterData = new Array();
        var SelectedNode = null;
        var CPBPCRBRNode = new Array();
        var VoucherCpBpCrBr = new Array();
        var ApprovedNCheckedBy = new Array();
        var LedgerDetails = new Array();
        var LedgerMaster = null;
        var LedgerNodeDelete = new Array();
        var IsEdited = false, EditedRow = null;
        var isCpCrBpBr = false;

        var vcc = {};
        var ed = null;
        var ComanyList = new Array();
        $(document).ready(function () {

            if ($("#ContentPlaceHolder1_hfNodeMatrix").val() != "") {
                NodeMatrix = JSON.parse($("#ContentPlaceHolder1_hfNodeMatrix").val());
                $("#ContentPlaceHolder1_hfNodeMatrix").val("");
            }

            var company = null;

            if ($("#ContentPlaceHolder1_companyProjectUserControl_hfCompanyAll").val()) {
                ComanyList = JSON.parse($("#ContentPlaceHolder1_companyProjectUserControl_hfCompanyAll").val());
                $("#ContentPlaceHolder1_companyProjectUserControl_hfCompanyAll").val("");

                company = _.findWhere(ComanyList, { CompanyId: parseInt($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val()) });

                if (company != null) {
                    if (company.IsProfitableOrganization) {
                        $("#DonorContainer").hide();
                    }
                    else { $("#DonorContainer").show(); }
                }
            }

            $("#myTabs").tabs();

            CommonHelper.ApplyDecimalValidation();

            if ($("#ContentPlaceHolder1_hfCurrencyAll").val() != "") {
                Currency = JSON.parse($("#ContentPlaceHolder1_hfCurrencyAll").val());
                $("#ContentPlaceHolder1_hfCurrencyAll").val("");

                var corncy = _.findWhere(Currency, { CurrencyId: parseInt($("#ContentPlaceHolder1_ddlCurrency").val()) });

                if (corncy != null) {

                    if (corncy.CurrencyType == "Local") {
                        $("#ContentPlaceHolder1_txtConversionRate").val("");
                        $("#convertionRateContainer").hide();
                    }
                    else {
                        $("#convertionRateContainer").show();
                    }
                }
            }

            $('#ContentPlaceHolder1_txtVoucherDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtChequeDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#ContentPlaceHolder1_txtFromDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            });
            //}).datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_txtToDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            //}).datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").change(function () {

                var company = _.findWhere(ComanyList, { CompanyId: parseInt($(this).val()) });

                if (company != null) {

                    if (company.IsProfitableOrganization) {
                        $("#DonorContainer").hide();
                    }
                    else { $("#DonorContainer").show(); }
                }
            });

            $("#ContentPlaceHolder1_ddlCurrency").change(function () {

                var corncy = _.findWhere(Currency, { CurrencyId: parseInt($(this).val()) });

                if (corncy != null) {

                    if (corncy.CurrencyType == "Local") {
                        $("#ContentPlaceHolder1_txtConversionRate").val("");
                        $("#convertionRateContainer").hide();
                    }
                    else {
                        $("#convertionRateContainer").show();
                        PageMethods.LoadCurrencyConversionRate(corncy.CurrencyId, OnLoadConversionRateSucceeded, OnLoadConversionRateFailed);
                    }
                }
            });

            $("#ContentPlaceHolder1_ddlVoucherType").change(function () {
                ClearBeforeVoucher();
                var voucherType = $(this).val();

                if (voucherType == "CP" || voucherType == "BP" || voucherType == "CR" || voucherType == "BR") {
                    isCpCrBpBr = true;
                }
                else { isCpCrBpBr = false; }

                LoadCpBpCrBrAccountHead();
                CPBPCRBRAccountHeadTypeAssign($(this).val());
            });

            $('#txtAccountName').autocomplete({
                source: function (request, response) {

                    if ($("#ContentPlaceHolder1_ddlVoucherType").val() == "") {
                        toastr.info("Please Select Voucher Type.");
                        return false;
                    }

                    CommonHelper.SpinnerOpen();
                    //EditedRow = null;
                    //IsEdited = false;
                    //var v = _.filter(NodeMatrix, function (q) { return q.HeadWithCode.match(/sal/i) });  //Like Search By reqular expression

                    FilterData = _.filter(NodeMatrix, function (obj) {
                        return ~obj.HeadWithCode.toLowerCase().indexOf((request.term).toLowerCase()); // like search. ~ with indexOf means contains
                    });

                    var searchData = $.map(FilterData, function (obj) {
                        return {
                            label: obj.HeadWithCode,
                            value: obj.NodeId
                        };
                    });
                    response(searchData);
                    CommonHelper.SpinnerClose();
                },
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    $(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);

                    SelectedNode = _.findWhere(FilterData, { NodeId: parseInt(ui.item.value, 10) });
                }
            });

            var single = $("#ContentPlaceHolder1_companyProjectSearchUserControl_hfIsSingle").val() == "1";
            if (single) {
                $('#CompanyProjectSearchPanel').hide();
                //$('#SearchTypePanel').show();
            }
            else {
                $('#CompanyProjectSearchPanel').show();
                //$('#SearchTypePanel').hide();
            }
            if ($("#ContentPlaceHolder1_hfIsShowReferenceVoucherNumber").val() == "1") {
                $("#lblReferenceVoucherNumber").show();
                $("#dvReferenceVoucherNumber").show();
            }
            else {
                $("#lblReferenceVoucherNumber").hide();
                $("#dvReferenceVoucherNumber").hide();
            }
            $("#ContentPlaceHolder1_companyProjectUserControl_lblGLCompany").parent().addClass('text-right');
            $("#ContentPlaceHolder1_companyProjectUserControl_lblGLProject").parent().addClass('text-right');
            $("#ContentPlaceHolder1_companyProjectSearchUserControl_lblGLCompany").parent().addClass('text-right');
            $("#ContentPlaceHolder1_companyProjectSearchUserControl_lblGLProject").parent().addClass('text-right');
        });

        function CPBPCRBRAccountHeadTypeAssign(nodeType) {
            var CpBpCrBrAccountCaption = "", accountTransactionType = "";

            if (nodeType == "CP") {
                $("#cpcrbpbrMainHead").show();
                $("#txtDrAmount").attr("disabled", false);
                $("#txtCrAmount").attr("disabled", true);

                CpBpCrBrAccountCaption = nodeType + ' Account Head';

                $("#ChequeNChequeNumber").hide();
                $("#cpbpcrbraccounthead").show();

            }
            else if (nodeType == "BP") {
                $("#cpcrbpbrMainHead").show();
                $("#txtDrAmount").attr("disabled", false);
                $("#txtCrAmount").attr("disabled", true);
                CpBpCrBrAccountCaption = nodeType + ' Account Head';

                $("#ChequeNChequeNumber").show();
                $("#cpbpcrbraccounthead").show();
            }
            else if (nodeType == "CR") {
                $("#cpcrbpbrMainHead").show();
                $("#txtDrAmount").attr("disabled", true);
                $("#txtCrAmount").attr("disabled", false);
                CpBpCrBrAccountCaption = nodeType + ' Account Head';

                $("#ChequeNChequeNumber").hide();
                $("#cpbpcrbraccounthead").show();
            }
            else if (nodeType == "BR") {
                $("#cpcrbpbrMainHead").show();
                $("#txtDrAmount").attr("disabled", true);
                $("#txtCrAmount").attr("disabled", false);
                CpBpCrBrAccountCaption = nodeType + ' Account Head';

                $("#ChequeNChequeNumber").show();
                $("#cpbpcrbraccounthead").show();
            }
            else {
                $("#cpcrbpbrMainHead").hide();
                CpBpCrBrAccountCaption = 'Account Head';
                $("#txtDrAmount").attr("disabled", false);
                $("#txtCrAmount").attr("disabled", false);
                $("#ChequeNChequeNumber").hide();

                $("#cpbpcrbraccounthead").hide();
            }

            $("#lblCpBpCrBr").text(CpBpCrBrAccountCaption);
        }

        function OnLoadConversionRateSucceeded(result) {
            if (result != null)
                $("#ContentPlaceHolder1_txtConversionRate").val(result.ConversionRate);
            else {
                toastr.info("No Convertion Rate Is Found.");
            }
        }
        function OnLoadConversionRateFailed() {
            toastr.info("No Convertion Rate Is Found.");
        }


        function LoadCpBpCrBrAccountHead() {
            var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
            var voucherType = $("#ContentPlaceHolder1_ddlVoucherType").val();

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/GeneralLedger/frmVoucherEntry.aspx/AccountHeadForCPCRBPBR",
                data: "{'projectId':'" + projectId + "', 'voucherType':'" + voucherType + "'}",
                dataType: "json",
                success: OnChangeAccountHeadPopulated,
                error: function (result) {
                }
            });
        }
        function OnChangeAccountHeadPopulated(response) {
            PopulateControlForChangeAccountHead(response.d, $("#ContentPlaceHolder1_ddlCpBpCrBr"));
        }

        function PopulateControlForChangeAccountHead(list, control) {

            CPBPCRBRNode = list;
            var controlChnage = $("#ContentPlaceHolder1_ddlCPCRBPBRChange");

            if (list.length > 0) {
                control.empty();
                controlChnage.empty();

                if ($(control).is(':disabled')) {
                    control.removeAttr("disabled");
                    controlChnage.removeAttr("disabled");
                }

                $.each(list, function () {
                    control.append($("<option></option>").val(this['NodeId']).html(this['NodeHead']));
                    controlChnage.append($("<option></option>").val(this['NodeId']).html(this['NodeHead']));
                });
            }
            else {
                control.empty().append('<option selected="selected" value="0">Not Available<option>');
                control.attr("disabled", true);

                controlChnage.empty().append('<option selected="selected" value="0">Not Available<option>');
                controlChnage.attr("disabled", true);
            }
        }

        function AddVoucherNodeDetails() {

            if (SelectedNode == null) {
                toastr.warning("Please Select Accounts.");
                return false;
            }

            var voucherType = "";
            voucherType = $("#ContentPlaceHolder1_ddlVoucherType").val();

            if (voucherType == "CP" || voucherType == "BP" || voucherType == "CR" || voucherType == "BR") {

                var isExist = _.findWhere(LedgerDetails, { NodeId: SelectedNode.NodeId });

                if (isExist != null && IsEdited == false) {
                    toastr.warning("Same Accounts Already Exists.");
                    return false;
                }
                else if (isExist != null && IsEdited == true) {
                    var existsIndex = 0, insertedAccountIndex = 0;

                    existsIndex = $(EditedRow).index();
                    insertedAccountIndex = _.findIndex(LedgerDetails, { NodeId: SelectedNode.NodeId });

                    if (existsIndex != insertedAccountIndex) {
                        toastr.warning("Same Accounts Already Exists.");
                        return false;
                    }
                }
            }

            var tr = "", amountDr = "", amountCr = "", nodeNarration = "";
            var amountOtherDr = "", amountOtherCr = "", bankAccountId = null, chequeNumber = null, chequeDate = null;
            var currencyTypeId = "", nodeId = 0, convertionrate = "0", currencyAmount = 0, ledgerMasterId = "0";

            if (LedgerMaster != null) {
                ledgerMasterId = LedgerMaster.LedgerMasterId;
            }
            else { ledgerMasterId = "0"; }

            convertionrate = $("#ContentPlaceHolder1_txtConversionRate").val();
            currencyTypeId = $("#ContentPlaceHolder1_ddlCurrency").val();

            if (convertionrate == "")
                convertionrate = "0";

            amountDr = $("#VoucherGrid thead tr:eq(1)").find("th:eq(1)").find("input").val();
            amountCr = $("#VoucherGrid thead tr:eq(1)").find("th:eq(2)").find("input").val();

            if (amountDr == "")
                amountDr = "0";

            if (amountCr == "")
                amountCr = "0";

            if (CommonHelper.IsDecimal(amountDr) == false) {
                toastr.warning("Please Give Valid DR Amount.");
                return false;
            }
            else if (CommonHelper.IsDecimal(amountCr) == false) {
                toastr.warning("Please Give Valid CR Amount.");
                return false;
            }
            else if (parseFloat(amountDr) == 0 && parseFloat(amountCr) == 0) {
                toastr.warning("Please Give Valid CR/DR Amount.");
                return false;
            }
            else if (parseFloat(amountDr) > 0 && parseFloat(amountCr) > 0) {
                toastr.warning("Both CR/DR Amount Cannot Given.");
                return false;
            }

            if (convertionrate != "0") {
                currencyAmount = amountDr == "0" ? amountCr : amountDr;

                amountDr = (parseFloat(amountDr) * parseFloat(convertionrate)).toFixed(2);
                amountCr = parseFloat(amountCr) * parseFloat(convertionrate).toFixed(2);
            }
            else {
                currencyAmount = amountDr == "0" ? amountCr : amountDr;
            }

            nodeNarration = $("#VoucherGrid thead tr:eq(1)").find("th:eq(3)").find("input").val();

            if ((voucherType == "BR" || voucherType == "BP") && $("#ContentPlaceHolder1_txtChequeDate").val() != "") {
                chequeNumber = $("#ContentPlaceHolder1_txtChequeNumber").val();
                chequeDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtChequeDate").val(), '/');
            }

            if ((voucherType == "CP" || voucherType == "BP" || voucherType == "CR" || voucherType == "BR") && LedgerDetails.length == 0) {

                nodeId = parseInt($("#ContentPlaceHolder1_ddlCpBpCrBr").val(), 10);
                var frstNode = _.findWhere(CPBPCRBRNode, { NodeId: nodeId });

                LedgerDetails.push({
                    LedgerDetailsId: '0',
                    LedgerMasterId: ledgerMasterId,
                    NodeId: frstNode.NodeId,
                    NodeHead: frstNode.NodeHead,
                    BankAccountId: bankAccountId,
                    ChequeNumber: chequeNumber,
                    ChequeDate: chequeDate,
                    DRAmount: "0.00",
                    CRAmount: "0.00",
                    NodeNarration: '',
                    CostCenterId: '0',
                    CurrencyAmount: '0',
                    NodeType: frstNode.NodeType,
                    Hierarchy: frstNode.Hierarchy,
                    ParentId: null,
                    ParentLedgerId: null,
                    IsEdited: false
                });

                tr += "<tr>";
                tr += "<td style='width:40%;'>" + frstNode.NodeHead + "</td>";
                tr += "<td class='text-right' style='width:10%;'>" + 0.00 + "</td>";
                tr += "<td class='text-right' style='width:10%;'>" + 0.00 + "</td>";
                tr += "<td style='width:32%;'>" + "" + "</td>";
                tr += "<td style='width:8%;'> <a onclick=\"javascript:return EditCPCRBPBRVocuherNode(this);\" title='Edit' href='javascript:void(0);'><img src='../Images/edit.png' alt='Edit'></a>"
                tr += "</td>";
                tr += "</tr>";

                $('#VoucherGrid tbody').append(tr);
            }

            tr = "";

            if (IsEdited == false) {

                if (voucherType == "CP" || voucherType == "BP" || voucherType == "CR" || voucherType == "BR") {

                    if (SelectedNode.NodeId == LedgerDetails[0].NodeId) {
                        toastr.warning("Same Account Head Cannot Be Added.");
                        return false;
                    }
                }

                LedgerDetails.push({
                    LedgerDetailsId: '0',
                    LedgerMasterId: ledgerMasterId,
                    NodeId: SelectedNode.NodeId,
                    NodeHead: SelectedNode.NodeHead,
                    BankAccountId: null,
                    ChequeNumber: null,
                    ChequeDate: null,
                    DRAmount: amountDr,
                    CRAmount: amountCr,
                    NodeNarration: nodeNarration,
                    CostCenterId: '0',
                    CurrencyAmount: currencyAmount,
                    NodeType: SelectedNode.NodeType,
                    Hierarchy: SelectedNode.Hierarchy,
                    ParentId: null,
                    ParentLedgerId: null,
                    IsEdited: false
                });

                //LedgerDetails.splice(1, 0, {
                //    LedgerDetailsId: '0',
                //    LedgerMasterId: ledgerMasterId,
                //    NodeId: SelectedNode.NodeId,
                //    NodeHead: SelectedNode.NodeHead,
                //    BankAccountId: null,
                //    ChequeNumber: null,
                //    ChequeDate: null,
                //    DRAmount: amountDr,
                //    CRAmount: amountCr,
                //    NodeNarration: nodeNarration,
                //    CostCenterId: '0',
                //    CurrencyAmount: currencyAmount,
                //    NodeType: SelectedNode.NodeType,
                //    Hierarchy: SelectedNode.Hierarchy,
                //    ParentId: null,
                //    ParentLedgerId: null,
                //    IsEdited: false
                //});

                tr += "<tr>";
                tr += "<td style='width:40%;'>" + SelectedNode.NodeHead + "</td>";
                tr += "<td class='text-right' style='width:10%;'>" + amountDr + "</td>";
                tr += "<td class='text-right' style='width:10%;'>" + amountCr + "</td>";
                tr += "<td style='width:32%;'>" + nodeNarration + "</td>";

                tr += "<td style='width:8%;'> <a onclick=\"javascript:return EditVocuherNode(this);\" title='Edit' href='javascript:void(0);'><img src='../Images/edit.png' alt='Edit'></a>"
                tr += "&nbsp;&nbsp;<a href='javascript:void(0);' title='Delete' onclick= 'javascript:return DeleteVocuherNode(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                tr += "</td>";

                tr += "</tr>";

                if ($('#VoucherGrid tbody tr').length > 0 && isCpCrBpBr == true) {
                    //$('#VoucherGrid tbody tr:first').after(tr);
                    $('#VoucherGrid tbody').append(tr);
                }
                else
                    $('#VoucherGrid tbody').append(tr);
            }
            else {

                if (voucherType == "CP" || voucherType == "BP" || voucherType == "CR" || voucherType == "BR") {
                    var firstNodeForCPBPCRBR = 0;
                    firstNodeForCPBPCRBR = LedgerDetails[0].NodeId;

                    if (SelectedNode.NodeId == firstNodeForCPBPCRBR) {
                        toastr.warning("Same Account Head Cannot Be Added.");
                        return false;
                    }
                }

                var editedAccountIndex = 0;
                editedAccountIndex = $(EditedRow).index();

                if (LedgerMaster == null) {
                    LedgerDetails[editedAccountIndex].LedgerDetailsId = '0';
                    LedgerDetails[editedAccountIndex].LedgerMasterId = '0';
                }
                else {
                    LedgerDetails[editedAccountIndex].LedgerMasterId = LedgerMaster.LedgerMasterId;
                }

                LedgerDetails[editedAccountIndex].NodeId = SelectedNode.NodeId;
                LedgerDetails[editedAccountIndex].NodeHead = SelectedNode.NodeHead;
                LedgerDetails[editedAccountIndex].BankAccountId = null;
                LedgerDetails[editedAccountIndex].ChequeNumber = null;
                LedgerDetails[editedAccountIndex].ChequeDate = null;
                LedgerDetails[editedAccountIndex].DRAmount = amountDr;
                LedgerDetails[editedAccountIndex].CRAmount = amountCr;
                LedgerDetails[editedAccountIndex].NodeNarration = nodeNarration;
                LedgerDetails[editedAccountIndex].CostCenterId = '0';
                LedgerDetails[editedAccountIndex].CurrencyAmount = currencyAmount;
                LedgerDetails[editedAccountIndex].NodeType = SelectedNode.NodeType;
                LedgerDetails[editedAccountIndex].Hierarchy = SelectedNode.Hierarchy;
                LedgerDetails[editedAccountIndex].ParentId = null;
                LedgerDetails[editedAccountIndex].ParentLedgerId = null;

                if (LedgerMaster != null) {
                    LedgerDetails[editedAccountIndex].IsEdited = true;
                }
                else { LedgerDetails[editedAccountIndex].IsEdited = false; }

                $(EditedRow).find("td:eq(0)").text(SelectedNode.NodeHead);
                $(EditedRow).find("td:eq(1)").text(amountDr);
                $(EditedRow).find("td:eq(2)").text(amountCr);
                $(EditedRow).find("td:eq(3)").text(nodeNarration);
            }

            $("#VoucherGrid thead tr:eq(1)").find("th:eq(0)").find("input").val("");
            $("#VoucherGrid thead tr:eq(1)").find("th:eq(1)").find("input").val("");
            $("#VoucherGrid thead tr:eq(1)").find("th:eq(2)").find("input").val("");
            $("#VoucherGrid thead tr:eq(1)").find("th:eq(3)").find("input").val("");

            $("#VoucherGrid thead tr:eq(1)").find("th:eq(0)").find("input").focus();

            CalculateTotalDrCr();
            SelectedNode = null;
            IsEdited = false;

            return false;
        }

        function CalculateTotalDrCr() {

            var dr = "", cr = "", totalDr = 0.00, totalCr = 0.00, totalCurrencyAmount = 0.00;
            var drAmount = 0.00, crAmount = 0.00, currencyAmount = 0.00, voucherType = "", index = 0;
            var LedgerDetailsIndex = 0;

            //LedgerDetailsIndex = LedgerDetails.length - 1;
            voucherType = $("#ContentPlaceHolder1_ddlVoucherType").val();

            if (voucherType == "CP" || voucherType == "BP" || voucherType == "CR" || voucherType == "BR") {

                $("#VoucherGrid tbody tr:not(:first)").each(function () {
                    dr = $(this).find("td:eq(1)").text();
                    cr = $(this).find("td:eq(2)").text();

                    index = $(this).index();

                    drAmount = dr == "" ? 0.00 : parseFloat(dr);
                    crAmount = cr == "" ? 0.00 : parseFloat(cr);

                    totalDr += drAmount;
                    totalCr += crAmount;
                    totalCurrencyAmount += parseFloat(LedgerDetails[index].CurrencyAmount);
                });
            }
            else {

                $("#VoucherGrid tbody tr").each(function () {
                    dr = $(this).find("td:eq(1)").text();
                    cr = $(this).find("td:eq(2)").text();

                    index = $(this).index();

                    drAmount = dr == "" ? 0.00 : parseFloat(dr);
                    crAmount = cr == "" ? 0.00 : parseFloat(cr);

                    totalDr += drAmount;
                    totalCr += crAmount;
                    totalCurrencyAmount += parseFloat(LedgerDetails[index].CurrencyAmount);
                });
            }

            totalDr = toFixed(totalDr, 2);
            totalCr = toFixed(totalCr, 2);

            if (voucherType == "CP" || voucherType == "BP") {
                $("#VoucherGrid tbody tr:eq(0)").find("td:eq(2)").text(totalDr);
                LedgerDetails[LedgerDetailsIndex].CRAmount = parseFloat(totalDr);
                LedgerDetails[LedgerDetailsIndex].CurrencyAmount = totalDr;
            }
            else if (voucherType == "CR" || voucherType == "BR") {
                $('#VoucherGrid tbody tr:eq(0)').find("td:eq(1)").text(totalCr);
                LedgerDetails[LedgerDetailsIndex].DRAmount = parseFloat(totalCr);
                LedgerDetails[LedgerDetailsIndex].CurrencyAmount = totalCr;
            }

            if (voucherType == "CP" || voucherType == "BP") {
                $("#lblTotalDrAmount").text(totalDr);
                $("#lblTotalCrAmount").text(totalDr);
                $("#tfTotalDrAmount").text(totalDr);
                $("#tfTotalCrAmount").text(totalDr);

                LedgerDetails[LedgerDetailsIndex].CurrencyAmount = totalDr;

                if (LedgerMaster != null) {
                    LedgerDetails[LedgerDetailsIndex].IsEdited = true;
                }
                else { LedgerDetails[LedgerDetailsIndex].IsEdited = false; }

            }
            else if (voucherType == "CR" || voucherType == "BR") {
                $("#lblTotalDrAmount").text(totalCr);
                $("#lblTotalCrAmount").text(totalCr);
                $("#tfTotalDrAmount").text(totalCr);
                $("#tfTotalCrAmount").text(totalCr);
                LedgerDetails[LedgerDetailsIndex].CurrencyAmount = totalCr;

                if (LedgerMaster != null) {
                    LedgerDetails[LedgerDetailsIndex].IsEdited = true;
                }
                else { LedgerDetails[LedgerDetailsIndex].IsEdited = false; }
            }
            else {
                $("#lblTotalDrAmount").text(totalDr);
                $("#lblTotalCrAmount").text(totalCr);
                $("#tfTotalDrAmount").text(totalDr);
                $("#tfTotalCrAmount").text(totalCr);
            }
        }

        function SaveVoucher() {
            debugger;
            ApprovedNCheckedBy = new Array();

            var companyId = "", projectId = "", donorId = null, voucherType = "", isBankExist = false, voucherNo = "", voucherDate = "",
                narration = "", payerOrPayee = "", gLStatus = "", currencyId = "", convertionRate = 0,
                ledgerMasterId = "0", totalDrAmount = 0.00, totalCrAMount = 0.00, referenceNumber = "", voucherChequeNumber = "", voucherChequeDate = "";

            totalDrAmount = parseFloat($("#lblTotalDrAmount").text());
            totalCrAMount = parseFloat($("#lblTotalCrAmount").text());

            companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
            donorId = $("#ContentPlaceHolder1_ddlDonor").val();
            voucherType = $("#ContentPlaceHolder1_ddlVoucherType").val();
            voucherDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtVoucherDate").val(), '/'); //$("#ContentPlaceHolder1_txtVoucherDate").val();
            narration = $("#ContentPlaceHolder1_txtVoucherNarration").val();
            gLStatus = 'Pending';
            currencyId = $("#ContentPlaceHolder1_ddlCurrency").val();
            convertionrate = $("#ContentPlaceHolder1_txtConversionRate").val();
            payerOrPayee = $("#ContentPlaceHolder1_txtPayerOrPayee").val();
            referenceNumber = $("#ContentPlaceHolder1_txtReferenceNumber").val();
            

            if (companyId == "0") {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").focus();
                toastr.warning("Please Select Company.");
                return false;
            }
            if (projectId == "0") {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").focus();
                toastr.warning("Please Select Project.");
                return false;
            }

            if ($("#VoucherGrid tbody tr").length == 0) {
                toastr.warning("Please Add Voucher Details.");
                return false;
            }
            else if ((totalDrAmount > 0 || totalCrAMount > 0) && (totalDrAmount != totalCrAMount)) {
                toastr.warning("DR Amount & CR Amount Must Be Equall.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtVoucherDate").val() == "") {
                toastr.warning("Please Enter Voucher Date.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtVoucherNarration").val() == "") {
                toastr.warning("Please Enter Voucher Narration.");
                return false;
            }
            var corncy = _.findWhere(Currency, { CurrencyId: parseInt(currencyId) });

            if (corncy != null) {
                if (corncy.CurrencyType != "Local" && convertionrate == "") {
                    toastr.warning("Please Give Convertion Rate.");
                    return false;
                }
            }
            if (voucherType == "BP" || voucherType == "BR") {
                voucherChequeNumber = $("#ContentPlaceHolder1_txtChequeNumber").val();
                //chequeDate = $("#ContentPlaceHolder1_txtChequeDate").val();
                if ($("#ContentPlaceHolder1_txtChequeDate").val() != "") {
                    voucherChequeDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtChequeDate").val(), '/');
                }

                if (voucherChequeNumber == "") {
                    toastr.warning("Please Give Cheque Number.");
                    return false;
                }
                if (voucherChequeDate == "") {
                    toastr.warning("Please Give Cheque Date.");
                    return false;
                }
            }
            if (convertionrate == "")
                convertionrate = 0;

            if (donorId == "0")
                donorId = null;

            if (LedgerMaster != null) {
                ledgerMasterId = LedgerMaster.LedgerMasterId;
            }

            LedgerMaster = {
                LedgerMasterId: ledgerMasterId,
                CompanyId: companyId,
                ProjectId: projectId,
                DonorId: donorId,
                VoucherType: voucherType,
                IsBankExist: isBankExist,
                VoucherNo: voucherNo,
                VoucherDate: voucherDate,
                CurrencyId: currencyId,
                ConvertionRate: convertionrate,
                Narration: narration,
                PayerOrPayee: payerOrPayee,
                ReferenceNumber: referenceNumber,
                GLStatus: gLStatus,
                ChequeNumber: voucherChequeNumber,
                ChequeDate: voucherChequeDate
            };

            var deletedDocument = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();
            if (ledgerMasterId == "0" || ledgerMasterId == '') {
                PageMethods.SaveVoucher(LedgerMaster, LedgerDetails, deletedDocument, OnSucceedVocuherSave, OnFailedVocuherSave);
            }
            else {
                PageMethods.UpdateVoucher(LedgerMaster, LedgerDetails, LedgerNodeDelete, deletedDocument, OnSucceedVocuherSave, OnFailedVocuherSave);
            }

            return false;
        }
        function OnSucceedVocuherSave(result) {
            if (result.IsSuccess) {
                $("#ContentPlaceHolder1_RandomLedgerMasterId").val(result.DataStr);
                CommonHelper.AlertMessage(result.AlertMessage);
                ClearAll();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            return false;
        }
        function OnFailedVocuherSave(result) { toastr.error("Failed"); }

        function EditVocuherNode(editNode) {

            ed = editNode;

            var tr = $(editNode).parent().parent();
            var index = $(tr).index();

            SelectedNode = _.findWhere(NodeMatrix, { NodeId: LedgerDetails[index].NodeId });
            $("#txtAccountName").val(SelectedNode.HeadWithCode);
            $("#txtDrAmount").val(LedgerDetails[index].DRAmount);
            $("#txtCrAmount").val(LedgerDetails[index].CRAmount);
            $("#txtNarration").val(LedgerDetails[index].NodeNarration);

            var voucherType = $("#ContentPlaceHolder1_ddlVoucherType").val();

            if ((voucherType == "CP" || voucherType == "BP" || voucherType == "CR" || voucherType == "BR")) {
                if (LedgerDetails[index].DRAmount > 0) {
                    $("#txtDrAmount").attr("disabled", false);
                    $("#txtCrAmount").attr("disabled", true);
                }
                else if (LedgerDetails[index].CRAmount > 0) {
                    $("#txtDrAmount").attr("disabled", true);
                    $("#txtCrAmount").attr("disabled", false);
                }
            }
            else {
                $("#txtDrAmount").attr("disabled", false);
                $("#txtCrAmount").attr("disabled", false);
            }

            EditedRow = tr;
            IsEdited = true;
        }

        function DeleteVocuherNode(deleteNode) {
            var tr = $(deleteNode).parent().parent();
            var index = $(tr).index();

            if (LedgerDetails[index].LedgerDetailsId != "0") {
                LedgerNodeDelete.push(LedgerDetails[index]);
            }

            $(tr).remove();
            LedgerDetails.splice(index, 1);
            CalculateTotalDrCr();
        }

        function ClearAll() {
            FilterData = new Array();
            SelectedNode = null;
            CPBPCRBRNode = new Array();
            VoucherCpBpCrBr = new Array();
            ApprovedNCheckedBy = new Array();
            LedgerDetails = new Array();
            LedgerMaster = null;
            IsEdited = false; EditedRow = null;
            $('#doctablelist tbody tr').each(function (i, row) {
                $(this).find("td:eq(2) img").trigger('click')
            });
            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './frmVoucherEntry.aspx/ChangeRandomId',
                dataType: "json",
                async: false,
                success: function (data) {
                    $("#ContentPlaceHolder1_RandomLedgerMasterId").val(data.d);
                },
                error: function (error) {
                }
            });
            $('#VoucherGrid tbody').html("");
            $("#VoucherDocumentInfo").html("");
            $("#ContentPlaceHolder1_txtVoucherNarration").val("");
            $("#ContentPlaceHolder1_txtVoucherDate").datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_txtChequeNumber").val("");
            $("#ContentPlaceHolder1_txtChequeDate").val("");

            $("#ContentPlaceHolder1_ddlCpBpCrBr").empty();
            $("#ContentPlaceHolder1_ddlCpBpCrBr").append('<option selected="selected" value="0">Not Available<option>');
            $("#ContentPlaceHolder1_ddlCpBpCrBr").attr("disabled", true);

            $("#ContentPlaceHolder1_ddlCPCRBPBRChange").empty();
            $("#ContentPlaceHolder1_ddlCPCRBPBRChange").append('<option selected="selected" value="0">Not Available<option>');

            $("#cpcrbpbrMainHead").hide();
            $("#txtDrAmount").attr("disabled", false);
            $("#txtCrAmount").attr("disabled", false);
            $("#lblCpBpCrBr").text('Account Head');

            if ($("#ContentPlaceHolder1_companyProjectUserControl_hfIsSingle").val() != "1") {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").attr("disabled", false).val('0').trigger('change');
            }
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").attr("disabled", false);
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").attr("disabled", false);
            $("#ChequeNChequeNumber").hide();
            $("#cpbpcrbraccounthead").hide();

            //$("#ContentPlaceHolder1_txtConversionRate").val("");
            //$("#convertionRateContainer").hide();
            //$("#ContentPlaceHolder1_ddlCurrency").val("1");
            $("#ContentPlaceHolder1_ddlVoucherType").val("");

            $("#lblTotalDrAmount").text("0");
            $("#lblTotalCrAmount").text("0");
            $("#tfTotalDrAmount").text("0");
            $("#tfTotalCrAmount").text("0");

            $("#VoucherGrid thead tr:eq(1)").find("th:eq(0)").find("input").val("");
            $("#VoucherGrid thead tr:eq(1)").find("th:eq(1)").find("input").val("");
            $("#VoucherGrid thead tr:eq(1)").find("th:eq(2)").find("input").val("");
            $("#VoucherGrid thead tr:eq(1)").find("th:eq(3)").find("input").val("");

            $("#VoucherGrid thead tr:eq(1)").find("th:eq(0)").find("input").focus();
            $("#btnSaveVoucher").val("Save Voucher");
        }

        function ClearBeforeVoucher() {
            FilterData = new Array();
            SelectedNode = null;
            CPBPCRBRNode = new Array();
            VoucherCpBpCrBr = new Array();
            ApprovedNCheckedBy = new Array();
            LedgerDetails = new Array();
            LedgerMaster = null;
            IsEdited = false; EditedRow = null;
            isCpCrBpBr = false;

            $('#VoucherGrid tbody').html("");
            $("#ContentPlaceHolder1_txtVoucherNarration").val("");
            $("#ContentPlaceHolder1_ddlCpBpCrBr").empty();
            $("#ContentPlaceHolder1_ddlCpBpCrBr").append('<option selected="selected" value="0">Not Available<option>');
            $("#ContentPlaceHolder1_ddlCpBpCrBr").attr("disabled", true);

            $("#ContentPlaceHolder1_ddlCPCRBPBRChange").empty();
            $("#ContentPlaceHolder1_ddlCPCRBPBRChange").append('<option selected="selected" value="0">Not Available<option>');

            $("#cpcrbpbrMainHead").hide();
            $("#txtDrAmount").attr("disabled", false);
            $("#txtCrAmount").attr("disabled", false);
            $("#lblCpBpCrBr").text('Account Head');
            $("#ChequeNChequeNumber").hide();
            $("#cpbpcrbraccounthead").hide();
            $("#lblTotalDrAmount").text("0");
            $("#lblTotalCrAmount").text("0");
            $("#tfTotalDrAmount").text("0");
            $("#tfTotalCrAmount").text("0");
            $("#btnSaveVoucher").val("Save Voucher");
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            CommonHelper.SpinnerOpen();
            var gridRecordsCount = $("#VoucherSearchGrid tbody tr").length;

            var companyId = $("#ContentPlaceHolder1_companyProjectSearchUserControl_ddlGLCompany").val();
            var projectId = $("#ContentPlaceHolder1_companyProjectSearchUserControl_ddlGLProject").val();
            var voucherNo = $("#ContentPlaceHolder1_txtVoucherNumber").val();
            var fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();
            var voucherType = $("#ContentPlaceHolder1_ddlVoucherTypeSearch").val();
            var voucherStatus = $("#ContentPlaceHolder1_ddlVoucherTypeStatus").val();
            var referenceNo = $("#ContentPlaceHolder1_txtSearchReferenceNumber").val();
            var referenceVoucherNo = $("#ContentPlaceHolder1_txtReferenceVoucherNumber").val();
            var narration = $("#ContentPlaceHolder1_txtSrcNarration").val();

            if (companyId == "0") {
                companyId = 0;
                projectId = 0;
            }

            PageMethods.GetVoucherBySearchCriteria(companyId, projectId, voucherType, voucherStatus, voucherNo, fromDate, toDate, referenceNo, referenceVoucherNo, narration, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {

            $("#VoucherSearchGrid tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"6\" >No Data Found</td> </tr>";
                $("#VoucherSearchGrid tbody ").append(emptyTr);
                CommonHelper.SpinnerClose();
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                tr += "<tr>";

                //tr += "<td style=\"width:15%;\">" + new Date(gridObject.VoucherDate).format(innBoarDateFormat) + "</td>";
                tr += "<td style=\"width:10%;\">" + gridObject.VoucherDateString + "</td>";
                tr += "<td style=\"width:12%;\">" + gridObject.VoucherNo + "</td>";
                tr += "<td style=\"width:40%;\">" + gridObject.Narration + "</td>";
                tr += "<td style=\"width:15%;\">" + gridObject.VoucherTotalAmount + "</td>";
                tr += "<td style=\"width:10%;\">" + gridObject.GLStatus + "</td>";
                tr += "<td style=\"text-align: center; width:20%; cursor:pointer;\">";
                if (gridObject.GLStatus == 'Submit' || gridObject.GLStatus == 'Pending') {
                    if (gridObject.IsCanEdit)
                        tr += "<img src='../Images/edit.png' onClick= \"javascript:return VoucherEdit('" + gridObject.LedgerMasterId + "')\" alt='Edit'  title='Edit' border='0' />";
                    if (gridObject.IsCanDelete)
                        tr += "&nbsp;&nbsp;<img src='../Images/delete.png'  onClick= \"javascript:return VoucherDelete('" + gridObject.LedgerMasterId + "')\" alt='Delete' title='Delete' border='0' />";

                }
                else if (result.UserGroupId != 0 && gridObject.CanEditDeleteAfterApproved == 1) {
                    tr += "<img src='../Images/edit.png' onClick= \"javascript:return VoucherEdit('" + gridObject.LedgerMasterId + "')\" alt='Edit'  title='Edit' border='0' />";
                    tr += "&nbsp;&nbsp;<img src='../Images/delete.png'  onClick= \"javascript:return VoucherDelete('" + gridObject.LedgerMasterId + "')\" alt='Delete' title='Delete' border='0' />";

                }
                tr += "&nbsp;&nbsp;<img src='../Images/detailsInfo.png'  onClick= \"javascript:return VoucherDetails('" + gridObject.LedgerMasterId + "')\" alt='Details' title='Details' border='0' />";
                tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return VoucherPreviewOption('" + gridObject.LedgerMasterId + "')\" alt='Voucher' title='Voucher' border='0' />";


                tr += '&nbsp;&nbsp;<a href="javascript:void();" onclick= "javascript:return ShowDocuments(' + gridObject.LedgerMasterId + ');" title="Documents"><img style="width:16px;height:16px;" alt="Documents" src="../Images/document.png" /></a>';
                tr += "</td>";
                tr += "<td align='right' style=\"display:none;\">" + gridObject.LedgerMasterId + "</td>";

                tr += "</tr>"

                $("#VoucherSearchGrid tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnLoadObjectFailed(error) {
            toastr.warning("Error");
            CommonHelper.SpinnerClose();
        }

        function VoucherEdit(ledgerMasterId) {

            CommonHelper.SpinnerOpen();
            PageMethods.GetVoucherDetailsForEdit(ledgerMasterId, OnSucceedEditDetailsLoad, OnFailedEditDetailsLoad);

            $("#myTabs").tabs({ active: 0 });
        }
        function OnSucceedEditDetailsLoad(result) {

            $("#VoucherGrid tbody").html("");
            $("#btnSaveVoucher").val("Update Voucher");

            LedgerMaster = result.LedgerMaster;
            LedgerDetails = result.LedgerMasterDetails;

            $("#lblTotalDrAmount").text(result.TotalDrAmount);
            $("#lblTotalCrAmount").text(result.TotalCrAmount);
            $("#tfTotalDrAmount").text(result.TotalDrAmount);
            $("#tfTotalCrAmount").text(result.TotalCrAmount);

            var control = $("#ContentPlaceHolder1_ddlCpBpCrBr");
            var chnageControl = $("#ContentPlaceHolder1_ddlCPCRBPBRChange");

            control.empty();
            chnageControl.empty();

            if ((LedgerMaster.VoucherType == "CP" || LedgerMaster.VoucherType == "BP" || LedgerMaster.VoucherType == "CR" || LedgerMaster.VoucherType == "BR")) {

                CPBPCRBRNode = result.CpBpCrBrAccountHead;

                if (result.CpBpCrBrAccountHead.length > 0) {
                    $.each(result.CpBpCrBrAccountHead, function (index, obj) {
                        control.append($("<option></option>").val(obj.NodeId).html(obj.NodeHead));
                        chnageControl.append($("<option></option>").val(obj.NodeId).html(obj.NodeHead));
                    });
                }
                else {
                    control.empty().append('<option selected="selected" value="0">Not Available<option>');
                    chnageControl.empty().append('<option selected="selected" value="0">Not Available<option>');
                    control.attr("disabled", true);
                }

                $(control).val(result.LedgerMasterDetails[0].NodeId + '');
                $(chnageControl).val(result.LedgerMasterDetails[0].NodeId + '');

                $(control).attr("disabled", true);
            }
            else {
                $(control).attr("disabled", false);
            }

            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val(LedgerMaster.CompanyId).trigger('change').prop('disabled', true);
            setTimeout(() => {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val(LedgerMaster.ProjectId).trigger('change').prop('disabled', true);
            }, 1000);
            $("#ContentPlaceHolder1_ddlDonor").val(LedgerMaster.DonorId);
            $("#ContentPlaceHolder1_ddlVoucherType").val(LedgerMaster.VoucherType);

            $("#ContentPlaceHolder1_ddlCurrency").val(LedgerMaster.CurrencyId);

            $("#ContentPlaceHolder1_txtPayerOrPayee").val(LedgerMaster.PayerOrPayee);
            $("#ContentPlaceHolder1_txtReferenceNumber").val(LedgerMaster.ReferenceNumber);

            $("#ContentPlaceHolder1_txtVoucherNarration").val(LedgerMaster.Narration);

            $("#ContentPlaceHolder1_txtVoucherDate").val(GetStringFromDateTime(LedgerMaster.VoucherDate));

            CPBPCRBRAccountHeadTypeAssign(LedgerMaster.VoucherType);

            var corncy = _.findWhere(Currency, { CurrencyId: parseInt(LedgerMaster.CurrencyId, 10) });

            if (corncy != null) {

                if (corncy.CurrencyType == "Local") {
                    $("#ContentPlaceHolder1_txtConversionRate").val("");
                    $("#convertionRateContainer").hide();
                }
                else {
                    $("#convertionRateContainer").show();
                    $("#ContentPlaceHolder1_txtConversionRate").val(LedgerMaster.ConvertionRate);
                }
            }

            var company = _.findWhere(ComanyList, { CompanyId: LedgerMaster.CompanyId });

            if (company != null) {

                if (company.IsProfitableOrganization) {
                    $("#DonorContainer").hide();
                }
                else { $("#DonorContainer").show(); }
            }

            var tr = "";

            $.each(result.LedgerMasterDetails, function (index, obj) {

                tr += "<tr>";
                tr += "<td style='width:40%;'>" + obj.NodeHead + "</td>";
                tr += "<td class='text-right' style='width:10%;'>" + obj.DRAmount + "</td>";
                tr += "<td class='text-right' style='width:10%;'>" + obj.CRAmount + "</td>";
                tr += "<td style='width:32%;'>" + obj.NodeNarration + "</td>";

                if ((LedgerMaster.VoucherType == "CP" || LedgerMaster.VoucherType == "BP" || LedgerMaster.VoucherType == "CR" || LedgerMaster.VoucherType == "BR") && index > 0) {
                    tr += "<td style='width:8%;'> <a onclick=\"javascript:return EditVocuherNode(this);\" title='Edit' href='javascript:void(0);'><img src='../Images/edit.png' alt='Edit'></a>"
                    tr += "&nbsp;&nbsp;<a href='javascript:void(0);' title='Delete' onclick= 'javascript:return DeleteVocuherNode(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                    tr += "</td>";

                    if (obj.ChequeNumber != "" && obj.ChequeNumber != null) {
                        $("#ContentPlaceHolder1_txtChequeNumber").val(obj.ChequeNumber);
                    }

                    if (obj.ChequeDate != null) {
                        $("#ContentPlaceHolder1_txtChequeDate").val(GetStringFromDateTime(obj.ChequeDate));
                    }
                }
                else if (LedgerMaster.VoucherType == "CV" || LedgerMaster.VoucherType == "JV") {
                    tr += "<td style='width:8%;'> <a onclick=\"javascript:return EditVocuherNode(this);\" title='Edit' href='javascript:void(0);'><img src='../Images/edit.png' alt='Edit'></a>"
                    tr += "&nbsp;&nbsp;<a href='javascript:void(0);' title='Delete' onclick= 'javascript:return DeleteVocuherNode(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                    tr += "</td>";

                    $("#ContentPlaceHolder1_txtChequeNumber").val(obj.ChequeNumber);

                    if (obj.ChequeDate != null)
                        $("#ContentPlaceHolder1_txtChequeDate").val(GetStringFromDateTime(obj.ChequeDate));
                }
                else {
                    tr += "<td style='width:8%;'> <a onclick=\"javascript:return EditCPCRBPBRVocuherNode();\" title='Edit' href='javascript:void(0);'><img src='../Images/edit.png' alt='Edit'></a>"
                    tr += "</td>";

                    if ((LedgerMaster.VoucherType == "CP" || LedgerMaster.VoucherType == "BP" || LedgerMaster.VoucherType == "CR" || LedgerMaster.VoucherType == "BR")) {
                        if (obj.ChequeNumber != "" && obj.ChequeNumber != null) {
                            $("#ContentPlaceHolder1_txtChequeNumber").val(obj.ChequeNumber);
                        }

                        if (obj.ChequeDate != null)
                            $("#ContentPlaceHolder1_txtChequeDate").val(GetStringFromDateTime(obj.ChequeDate));
                    }
                }

                tr += "</tr>";
            });
            $("#ContentPlaceHolder1_RandomLedgerMasterId").val(result.RandomLedgerMasterId);
            UploadComplete();
            $('#VoucherGrid tbody').append(tr);
            CommonHelper.SpinnerClose();
        }
        function OnFailedEditDetailsLoad(result) {
            toastr.info("Error");
            CommonHelper.SpinnerClose();
        }

        function VoucherDelete(ledgerMasterId) {
            CommonHelper.SpinnerOpen();
            PageMethods.DeleteVoucher(ledgerMasterId, OnSucceedDetailsLoadDelete, OnFailedDetailsLoadDelete);
            return false;
        }
        function OnSucceedDetailsLoadDelete(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                CommonHelper.SpinnerClose();
                GridPaging($("#GridPagingContainer").find("li.active").index(), 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
                CommonHelper.SpinnerClose();
            }

            return false;
        }
        function OnFailedDetailsLoadDelete(result) {
            toastr.error("Error");
            CommonHelper.SpinnerClose();
        }

        function VoucherDetails(ledgerMasterId) {
            CommonHelper.SpinnerOpen();
            PageMethods.GetVoucherDetailsForDisplay(ledgerMasterId, OnSucceedDetailsLoadSave, OnFailedDetailsLoadSave);
        }
        function OnSucceedDetailsLoadSave(result) {

            $("#VoucherDetailsDisplayGrid tbody").html("");

            $("#lblCompany").text(result.LedgerMaster.CompanyName);
            $("#lblProject").text(result.LedgerMaster.ProjectName);
            $("#lblDonor").text(result.LedgerMaster.DonorName);
            $("#lblPayerOrPayee").text(result.LedgerMaster.PayerOrPayee);
            $("#lblReferenceNumber").text(result.LedgerMaster.ReferenceNumber);
            $("#lblVoucherDate").text(GetStringFromDateTime(result.LedgerMaster.VoucherDate));
            $("#lblVoucherType").text(result.LedgerMaster.VoucherTypeName);
            $("#lblCurrencyType").text(result.LedgerMaster.CurrencyName);
            $("#lblConvertionRate").text(result.LedgerMaster.ConvertionRate);

            var tr = "";

            $.each(result.LedgerMasterDetails, function (index, obj) {
                tr += "<tr>";
                tr += "<td style='width:25%;'>" + obj.NodeHead + "</td>";
                tr += "<td class='text-right' style='width:10%;'>" + obj.DRAmount + "</td>";
                tr += "<td class='text-right' style='width:10%;'>" + obj.CRAmount + "</td>";
                tr += "<td style='width:15%;'>" + (obj.ChequeNumber == null ? '' : obj.ChequeNumber) + "</td>";
                tr += "<td style='width:40%;'>" + obj.NodeNarration + "</td>";
                tr += "</tr>";
            });

            $("#TotalDR").text(result.TotalDrAmount);
            $("#TotalCR").text(result.TotalCrAmount);

            $("#VoucherDetailsDisplayGrid tbody").append(tr);

            $("#VoucherDetailsGrid").dialog({
                autoOpen: true,
                modal: true,
                width: '1000px',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Voucher Details",
                show: 'slide'
            });

            CommonHelper.SpinnerClose();
        }
        function OnFailedDetailsLoadSave(result) {
            toastr.warning("Data Error.");
            CommonHelper.SpinnerClose();
        }
        function VoucherPreviewOption(ledgerMasterId) {
            var iframeid = 'printDoc';
            var url = "Reports/frmReportVoucherInfo.aspx?DealId=" + ledgerMasterId;
            parent.document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 730,
                minHeight: 555,
                width: 'auto',
                closeOnEscape: false,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Voucher Preview",
                show: 'slide'
                //,close: ClosePrintDialog
            });

            setTimeout(function () { ScrollToBottom(); }, 1000);
        }

        function EditCPCRBPBRVocuherNode(control) {
            $("#CPCRBPBRHeadChangeContainer").dialog({
                autoOpen: true,
                modal: true,
                width: 450,
                height: 150,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Account Head",
                show: 'slide'
            });
        }

        function ChangeCPCRBPBRAccountHead() {

            var nodeId = parseInt($("#ContentPlaceHolder1_ddlCPCRBPBRChange").val(), 10);
            var frstNode = _.findWhere(CPBPCRBRNode, { NodeId: nodeId });

            $("#ContentPlaceHolder1_ddlCpBpCrBr").val(nodeId + "");

            LedgerDetails[0].NodeId = frstNode.NodeId;
            LedgerDetails[0].NodeHead = frstNode.NodeHead;
            LedgerDetails[0].NodeType = frstNode.NodeType;
            LedgerDetails[0].Hierarchy = frstNode.Hierarchy;
            LedgerDetails[0].IsEdited = true;

            $('#VoucherGrid tbody tr:eq(0)').find("td:eq(0)").text(frstNode.NodeHead);

            $("#CPCRBPBRHeadChangeContainer").dialog("close");
        }
        function UploadComplete() {
            var randomId = +$("#ContentPlaceHolder1_RandomLedgerMasterId").val();
            var id = LedgerMaster != null ? LedgerMaster.LedgerMasterId : 0;
            var deletedDoc = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();
            PageMethods.LoadVoucherDocument(id, randomId, deletedDoc, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }

        function OnLoadDocumentSucceeded(result) {

            $("#VoucherDocumentInfo").html("");

            var guestDoc = result;
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";
            if (totalDoc == 0)
                return true;
            guestDocumentTable += "<table id='voucherDocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            guestDocumentTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }

                guestDocumentTable += "<td align='left' style='width: 50%'>" + guestDoc[row].Name + "</td>";

                if (guestDoc[row].Path != "") {
                    if (guestDoc[row].Extention == ".jpg")
                        imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    else
                        imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                guestDocumentTable += "<td align='left' style='width: 30%'>" + imagePath + "</td>";

                guestDocumentTable += "<td align='left' style='width: 20%'>";
                guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteGuestDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                guestDocumentTable += "</td>";
                guestDocumentTable += "</tr>";
            }
            guestDocumentTable += "</table>";

            $("#VoucherDocumentInfo").html(guestDocumentTable);
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }
        function DeleteGuestDoc(docId, rowIndex) {
            if (confirm("Want to delete?")) {
                var deletedDoc = $("#<%=hfGuestDeletedDoc.ClientID %>").val();

                if (deletedDoc != "")
                    deletedDoc += "," + docId;
                else
                    deletedDoc = docId;

                $("#<%=hfGuestDeletedDoc.ClientID %>").val(deletedDoc);

                $("#trdoc" + rowIndex).remove();
            }
        }
        function LoadDocUploader() {

            var randomId = +$("#ContentPlaceHolder1_RandomLedgerMasterId").val();
            var path = "/GeneralLedger/File/Voucher/";
            var category = "GLVoucherDocuments";
            var iframeid = 'frmPrint';
            //var url = "/HMCommon/FileUploadTest.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            var url = "/Common/FileUpload.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            document.getElementById(iframeid).src = url;

            $("#DocumentDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 300,
                closeOnEscape: false,
                resizable: false,
                title: "Documents Upload",
                show: 'slide'
            });

        }
        function AttachFile() {
            $("#dvVoucherdocuments").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Voucher Documents",
                show: 'slide'
            });
        }

        function PopulateProjects(control) {
            $("#SaveContent").hide();
            $("#balanceTable").html("");
            let companyId = $(control).val();
            if (companyId == 0) {
                PopulateControlWithValueNTextField([], $("#ContentPlaceHolder1_ddlSearchProject"), $("#<%=CommonDropDownHiddenField.ClientID %>").val(), "Name", "ProjectId");
            }
            $("#ContentPlaceHolder1_ddlSearchProject").empty().append('<option selected="selected" value="0">Loading...</option>');

            $.ajax({
                type: "POST",
                url: "./frmBudget.aspx/GetGLProjectByGLCompanyId",
                data: JSON.stringify({ companyId: companyId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    PopulateControlWithValueNTextField(response.d, $("#ContentPlaceHolder1_ddlSearchProject"), $("#<%=CommonDropDownHiddenField.ClientID %>").val(), "Name", "ProjectId");
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
            }

            function ShowDocuments(id) {
                PageMethods.LoadVoucherDocumentById(id, OnLoadDocumentByIdSucceeded, OnLoadDocumentByIdFailed);
                return false;
            }
            function OnLoadDocumentByIdSucceeded(result) {
                $("#imageDiv").html(result);

                $("#voucherDocuments").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 900,
                    minHeight: 400,
                    closeOnEscape: true,
                    resizable: false,
                    title: "Voucher Documents",
                    show: 'slide'
                });

                return false;
            }

            function OnLoadDocumentByIdFailed(error) {
                toastr.error(error.get_message());
            }
    </script>
    <asp:HiddenField ID="hfIsShowReferenceVoucherNumber" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfNodeMatrix" runat="server" />
    <asp:HiddenField ID="hfCurrencyAll" runat="server" />
    <asp:HiddenField ID="hfCompanyAll" runat="server" />
    <asp:HiddenField ID="hfGuestDeletedDoc" runat="server" />
    <div id="voucherDocuments" style="display: none;">
        <div id="imageDiv"></div>
    </div>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="dvVoucherdocuments" style="display: none;">
        <label for="Attachment" class="control-label col-md-2">
            Attachment</label>
        <div class="col-md-4">
            <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
                <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                    FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
            </asp:Panel>
        </div>
    </div>
    <div id="CPCRBPBRHeadChangeContainer" style="display: none;">
        <div class="form-horizontal">
            <div class="form-group">
                <label id="lblCPCRBPBRChange" class="control-label required-field col-md-3 no-gutter-column">Account Head</label>
                <div class="col-md-9">
                    <asp:DropDownList ID="ddlCPCRBPBRChange" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-12">
                    <input type="button" value="Change" class="btn btn-primary btn-lg" onclick="ChangeCPCRBPBRAccountHead()" />
                </div>
            </div>
        </div>
    </div>
    <div id="displayBill" style="display: none;">
        <iframe id="printDoc" name="printDoc" width="730" height="650" frameborder="0" style="overflow: hidden;"></iframe>
        <div id="bottomPrint">
        </div>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" style="border: 1px solid #AAAAAA; border-bottom: none"><a href="#tab-1">Voucher
                Entry</a></li>
            <li id="B" style="border: 1px solid #AAAAAA; border-bottom: none"><a href="#tab-2">Voucher
                Search</a></li>
        </ul>
        <div id="tab-1">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Voucher Information
                </div>
                <div class="panel panel-body">
                    <div class="form-horizontal">
                        <fieldset>
                            <legend>Company/Project</legend>
                            <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />
                            <div class="form-group" id="DonorContainer" style="display: none;">
                                <label for="" class="control-label col-md-2">
                                    Donor</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlDonor" runat="server" CssClass="form-control" TabIndex="3">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-md-2">Payer Or Payee</label>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtPayerOrPayee" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-md-2">Reference Number</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtReferenceNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>Voucher Info</legend>
                            <div class="form-group">
                                <label for="" class="control-label required-field col-md-2">
                                    Voucher Date</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtVoucherDate" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                                </div>
                                <label for="" class="control-label col-md-2 required-field">
                                    Voucher Type</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlVoucherType" runat="server" CssClass="form-control" TabIndex="5">
                                        <asp:ListItem Value="">Select One</asp:ListItem>
                                        <asp:ListItem Value="CP">Cash Payment (CP) </asp:ListItem>
                                        <asp:ListItem Value="BP">Bank Payment (BP)</asp:ListItem>
                                        <asp:ListItem Value="CR">Cash Receive (CR)</asp:ListItem>
                                        <asp:ListItem Value="BR">Bank Receive (BR)</asp:ListItem>
                                        <asp:ListItem Value="JV">Journal Voucher (JV)</asp:ListItem>
                                        <asp:ListItem Value="CV">Contra Voucher (CV)</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="" class="control-label required-field col-md-2">
                                    Currency Type</label>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control" TabIndex="6">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-6 col-padding-left-none" id="convertionRateContainer">
                                            <div class="input-group">
                                                <span class="input-group-addon">C.Rate</span>
                                                <asp:TextBox ID="txtConversionRate" runat="server" CssClass="form-control" TabIndex="7"
                                                    Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="cpbpcrbraccounthead">
                                    <label for="" id="lblCpBpCrBr" class="control-label required-field col-md-2">
                                        Account Head</label>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlCpBpCrBr" runat="server" CssClass="form-control" TabIndex="8">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group" id="ChequeNChequeNumber">
                                <label for="" class="control-label required-field col-md-2">
                                    Cheque Number</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtChequeNumber" runat="server" CssClass="form-control" TabIndex="9"></asp:TextBox>
                                </div>
                                <label for="" class="control-label required-field col-md-2">
                                    Cheque Date</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtChequeDate" runat="server" CssClass="form-control" TabIndex="10"></asp:TextBox>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-md-5" style="font-weight: bold;">
                            Voucher Details
                        </div>
                        <div class="col-md-7 pull-right">
                            <div class="row">
                                <div class="col-md-2 text-right" style="font-weight: bold;">
                                    Total:
                                </div>
                                <div class="col-md-2 text-right" style="font-weight: bold;">
                                    Dr. Amount:
                                </div>
                                <div class="col-md-3 text-right" style="font-weight: bold;">
                                    <label id="lblTotalDrAmount" class="form-control text-right">
                                        0.00
                                    </label>
                                </div>
                                <div class="col-md-2 text-right" style="font-weight: bold;">
                                    Cr. Amount:
                                </div>
                                <div class="col-md-3 text-right" style="font-weight: bold;">
                                    <label id="lblTotalCrAmount" class="form-control text-right">
                                        0.00
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-body">
                    <div class="form-horizontal">
                        <table id="VoucherGrid" class="table table-hover table-condensed table-bordered table-responsive">
                            <thead>
                                <tr>
                                    <th style="width: 40%;">
                                        <label for="" class="control-label">
                                            Account Name</label>
                                    </th>
                                    <th style="width: 10%;">
                                        <label for="" class="control-label">
                                            Dr. Amount</label>
                                    </th>
                                    <th style="width: 10%;">
                                        <label for="" class="control-label">
                                            Cr. Amount</label>
                                    </th>
                                    <th style="width: 32%;">
                                        <label for="" class="control-label">
                                            Narration</label>
                                    </th>
                                    <th style="width: 8%;">
                                        <label for="" class="control-label">
                                            Action</label>
                                    </th>
                                </tr>
                                <tr>
                                    <th style="width: 40%; font-weight: normal;">
                                        <input type="text" class="form-control" id="txtAccountName" />
                                    </th>
                                    <th style="width: 10%; font-weight: normal;">
                                        <input type="text" class="form-control text-right quantitydecimal" id="txtDrAmount" />
                                    </th>
                                    <th style="width: 10%; font-weight: normal;">
                                        <input type="text" class="form-control text-right quantitydecimal" id="txtCrAmount" />
                                    </th>
                                    <th style="width: 35%; font-weight: normal;">
                                        <input type="text" class="form-control" id="txtNarration" />
                                    </th>
                                    <th style="width: 5%;">
                                        <input type="button" class="TransactionalButton btn btn-sm btn-primary" value="Add"
                                            onclick="AddVoucherNodeDetails()" />
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                            <tfoot>
                                <tr>
                                    <td style="width: 40%;" class="text-right">
                                        <label class="control-label">
                                            Total</label>
                                    </td>
                                    <td style="width: 10%;" class="text-right">
                                        <label class="control-label" id="tfTotalDrAmount">
                                            0
                                        </label>
                                    </td>
                                    <td style="width: 10%;" class="text-right">
                                        <label class="control-label" id="tfTotalCrAmount">
                                            0
                                        </label>
                                    </td>
                                    <td style="width: 32%;"></td>
                                    <td style="width: 8%;"></td>
                                </tr>
                            </tfoot>
                        </table>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label for="" class="control-label col-md-2 required-field">
                                    Voucher Narration</label>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtVoucherNarration" TextMode="MultiLine" runat="server" CssClass="form-control"
                                        TabIndex="11"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:HiddenField ID="RandomLedgerMasterId" runat="server"></asp:HiddenField>
                                <label for="" class="control-label col-md-2">
                                    Attachment</label>
                                <div class="col-md-4">
                                    <input type="button" id="btnAttachment" class="btn btn-primary btn-sm" value="Attach" onclick="LoadDocUploader()" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div id="VoucherDocumentInfo" class="col-md-12">
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <input type="button" id="btnSaveVoucher" class="TransactionalButton btn btn-primary"
                                        value="Save Voucher" onclick="SaveVoucher()" tabindex="14" />
                                    <input type="button" id="btnCancelVoucher" class="TransactionalButton btn btn-primary"
                                        value="Cancel" onclick="ClearAll()" tabindex="14" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="CompanyProjectSearchPanel" class="form-group">
                <UserControl:CompanyProjectUserControl ID="companyProjectSearchUserControl" runat="server" />
            </div>
            <div class="form-horizontal">
                <div class="form-group">
                    <label for="" class="control-label col-md-2">
                        Voucher Date</label>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtFromDate" CssClass="form-control" placeholder="From Date" runat="server" TabIndex="1"></asp:TextBox>
                    </div>
                    
                    <div class="col-md-2">
                        <asp:TextBox ID="txtToDate" CssClass="form-control" placeholder="To Date" runat="server"></asp:TextBox>
                    </div>
                    <label for="" class="control-label required-field col-md-2">
                        Voucher Status</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlVoucherTypeStatus" runat="server" CssClass="form-control" TabIndex="5">
                            <asp:ListItem Value="">--- All Status ---</asp:ListItem>
                            <asp:ListItem Value="Pending">Pending </asp:ListItem>
                            <asp:ListItem Value="Checked">Checked</asp:ListItem>
                            <asp:ListItem Value="Approved">Approved</asp:ListItem>                            
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-md-2">
                        Voucher Number</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtVoucherNumber" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <label for="" class="control-label required-field col-md-2">
                        Voucher Type</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlVoucherTypeSearch" runat="server" CssClass="form-control" TabIndex="5">
                            <asp:ListItem Value="">--- All Type Voucher ---</asp:ListItem>
                            <asp:ListItem Value="CP">Cash Payment (CP) </asp:ListItem>
                            <asp:ListItem Value="BP">Bank Payment (BP)</asp:ListItem>
                            <asp:ListItem Value="CR">Cash Receive (CR)</asp:ListItem>
                            <asp:ListItem Value="BR">Bank Receive (BR)</asp:ListItem>
                            <asp:ListItem Value="JV">Journal Voucher (JV)</asp:ListItem>
                            <asp:ListItem Value="CV">Contra Voucher (CV)</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-md-2">Reference Number</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchReferenceNumber" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <label id="lblReferenceVoucherNumber" class="control-label col-md-2">
                        Reference Voucher Number</label>
                    <div id="dvReferenceVoucherNumber" class="col-md-4">
                        <asp:TextBox ID="txtReferenceVoucherNumber" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-md-2">
                        Narration</label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtSrcNarration" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <input type="button" value="Search Voucher" class="TransactionalButton btn btn-primary btn-large"
                        onclick="GridPaging(1, 1)" />
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-12">
                    <table id="VoucherSearchGrid" class="table table-hover table-bordered table-condensed table-responsive">
                        <thead>
                            <tr>
                                <td style="width: 10%;">Voucher Date
                                </td>
                                <td style="width: 12%;">Voucher No
                                </td>
                                <td style="width: 40%;">Narration
                                </td>
                                <td style="width: 15%;">Voucher Total
                                </td>
                                <td style="width: 10%;">Status
                                </td>
                                <td style="width: 20%;">Action
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <div class="text-center" id="GridPagingContainer">
                        <ul class="pagination">
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="VoucherDetailsGrid" style="display: none;">
        <div class="form-horizontal">
            <fieldset>
                <legend>Company/Project</legend>
                <div class="form-group">
                    <label for="" class="control-label col-md-2">
                        Company</label>
                    <div class="col-md-4">
                        <label id="lblCompany" class="form-control">
                        </label>
                    </div>
                    <label for="" class="control-label col-md-2">
                        Project</label>
                    <div class="col-md-4">
                        <label id="lblProject" class="form-control">
                        </label>
                    </div>
                </div>
                <div class="form-group">
                    <label for="" class="control-label col-md-2">
                        Donor</label>
                    <div class="col-md-4">
                        <label id="lblDonor" class="form-control">
                        </label>
                    </div>
                </div>
                <div class="form-group">
                    <label for="" class="control-label col-md-2">
                        Payer Or Payee</label>
                    <div class="col-md-10">
                        <label id="lblPayerOrPayee" class="form-control">
                        </label>
                    </div>
                </div>
                <div class="form-group">
                    <label for="" class="control-label col-md-2">
                        Reference Number</label>
                    <div class="col-md-4">
                        <label id="lblReferenceNumber" class="form-control">
                        </label>
                    </div>
                </div>
            </fieldset>
            <fieldset>
                <legend>Voucher Info</legend>
                <div class="form-group">
                    <label class="control-label col-md-2">
                        Voucher Date</label>
                    <div class="col-md-4">
                        <label id="lblVoucherDate" class="form-control">
                        </label>
                    </div>
                    <label for="" class="control-label col-md-2">
                        Voucher Type</label>
                    <div class="col-md-4">
                        <label id="lblVoucherType" class="form-control">
                        </label>
                    </div>
                </div>
                <div class="form-group">
                    <label for="" class="control-label col-md-2">
                        Currency Type</label>
                    <div class="col-md-4">
                        <div class="row">
                            <div class="col-md-6">
                                <label id="lblCurrencyType" class="form-control">
                                </label>
                            </div>
                            <div class="col-md-6 col-padding-left-none">
                                <div class="input-group">
                                    <span class="input-group-addon">C.Rate</span>
                                    <label id="lblConvertionRate" class="form-control">
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
            <table id="VoucherDetailsDisplayGrid" class="table table-bordered table-hover table-condensed table-responsive">
                <thead>
                    <tr>
                        <td style="width: 25%;">
                            <label for="" class="control-label">
                                Account Name</label>
                        </td>
                        <td style="width: 10%;">
                            <label for="" class="control-label">
                                Dr. Amount</label>
                        </td>
                        <td style="width: 10%;">
                            <label for="" class="control-label">
                                Cr. Amount</label>
                        </td>
                        <td style="width: 15%;">
                            <label for="" class="control-label">
                                Cheque Number</label>
                        </td>
                        <td style="width: 40%;">
                            <label for="" class="control-label">
                                Narration</label>
                        </td>
                    </tr>
                </thead>
                <tbody>
                </tbody>
                <tfoot>
                    <tr>
                        <th style="width: 25%; text-align: right;">
                            <label class="control-label text-right">
                                Total
                            </label>
                        </th>
                        <th style="width: 10%; text-align: right;">
                            <label class="control-label text-right" id="TotalDR">
                            </label>
                        </th>
                        <th style="width: 10%; text-align: right;">
                            <label class="control-label text-right" id="TotalCR">
                            </label>
                        </th>
                        <th style="width: 15%;"></th>
                        <th style="width: 40%;"></th>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {

            var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val();
            debugger;
            if (companyId == "0") {

                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val($("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val()).trigger("change");

            }
            var companyId = $("#ContentPlaceHolder1_companyProjectSearchUserControl_hfGLCompanyId").val();
            debugger;
            if (companyId == "0") {

                $("#ContentPlaceHolder1_companyProjectSearchUserControl_ddlGLCompany").val($("#ContentPlaceHolder1_companyProjectSearchUserControl_hfGLCompanyId").val()).trigger("change");

            }
        });

    </script>
</asp:Content>
