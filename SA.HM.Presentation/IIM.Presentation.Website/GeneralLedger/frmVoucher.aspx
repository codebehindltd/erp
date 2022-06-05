<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmVoucher.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.frmVoucher"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            $("#txtSearch").focus();
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Accounts</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Voucher Entry</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_txtVoucherDate").datepicker("option", {
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtToDate").datepicker("option", "minDate", selectedDate);
                }
            });
        });


        function ValidateLedgerAmount() {
            var ddlLedgerMode = '<%=ddlLedgerMode.ClientID%>'
            var ddlVoucherType = '<%=ddlVoucherType.ClientID%>'
            var ledgerValidationCheck = 0

            if ($('#' + ddlVoucherType).val() == "JV") {
                ledgerValidationCheck = 1;
            }
            else if ($('#' + ddlVoucherType).val() == "CV") {
                ledgerValidationCheck = 1;
            }


            var txtLedgerAmount = '<%=txtLedgerAmount.ClientID%>'
            var LedgerAmount = $('#' + txtLedgerAmount).val();
            var floatLedger = parseFloat(LedgerAmount);

            if (LedgerAmount == "" || floatLedger <= 0) {
                toastr.info("Please Provide Currect Ledger Amount.");
                return false;
            }
            else if (ledgerValidationCheck == 1) {
                if ($('#' + ddlLedgerMode).val() == "Select One") {
                    toastr.info("Please Select Correct Ledger Mode.");
                    return false;
                }
            }
            else {
                return true;
            }
        }

        function PerformAddClickValidation() {
            var returnStatus = true;
            var isValid = "";
            var NodeId = 0;
            var ChequeNumber = "";
            var ddlNodeId = '<%=ddlNodeId.ClientID%>'
            var ddlVoucherType = '<%=ddlVoucherType.ClientID%>'
            var txtJVChequeNumber = '<%=txtJVChequeNumber.ClientID%>'
            var ddlVoucherType = '<%=ddlVoucherType.ClientID%>'
            var VoucherType = $('#' + ddlVoucherType).val();
            var hfConfigureAccountHead = '<%=hfConfigureAccountHead.ClientID%>'
            var txtChequeNumber = '<%=txtChequeNumber.ClientID%>'
            var ddlVoucherType = '<%=ddlVoucherType.ClientID%>'

            var txtLedgerAmount = '<%=txtLedgerAmount.ClientID%>'
            var LedgerAmount = $('#' + txtLedgerAmount).val();

            if (VoucherType == "None") {
                DisableAddButton();
            }
            else if (VoucherType == "JV" || VoucherType == "CV") {
                NodeId = $('#' + ddlNodeId).val();
                ChequeNumber = $('#' + txtJVChequeNumber).val();

                if (NodeId != "" && ChequeNumber != "") {
                    ValidateChequeNumber(NodeId, ChequeNumber);
                }
                EnableAddButton();
            }
            else if (VoucherType == "CP" || VoucherType == "CR") {
                //Ledger Amount
                EnableAddButton();
            }
            else if (VoucherType == "BP" || VoucherType == "BR") {
                NodeId = $('#' + hfConfigureAccountHead).val();
                ChequeNumber = $('#' + txtChequeNumber).val();
                if (NodeId != "" && ChequeNumber != "") {
                    ValidateChequeNumber(NodeId, ChequeNumber);
                }
                EnableAddButton();
            }

        }


        function ValidateChequeNumber(NodeId, ChequeNumber) {
            PageMethods.ValidateChequeNumber(NodeId, ChequeNumber, ShowResult, ShowError);
            return false;
        }
        function ShowResult(result) {
            if (result == "true") {
                EnableAddButton();
            }
            else {
                toastr.info("You have entered existing Cheque Number.");
                DisableAddButton();
            }
        }
        function ShowError(error) {
        }

        function DisableAddButton() {
            $('#<%=btnAddDetail.ClientID%>').attr("disabled", true);
        }

        function EnableAddButton() {
            $('#<%=btnAddDetail.ClientID%>').attr("disabled", false);
        }



        $(document).ready(function () {

            PerformAddClickValidation();
            var ddlCurrency = '<%=ddlCurrency.ClientID%>'
            var txtCalculatedAmount = '<%=txtCalculatedAmount.ClientID%>'
            var txtConversionRate = '<%=txtConversionRate.ClientID%>'
            var txtLedgerAmount = '<%=txtLedgerAmount.ClientID%>'
            $('#ConversionPanel').hide();

            var ddlGLProject = '<%=ddlGLProject.ClientID%>'
            var ddlGLCompany = '<%=ddlGLCompany.ClientID%>'
            var selectedIndex = parseFloat($('#' + ddlGLCompany).prop("selectedIndex"));
            if (selectedIndex > 0) {
                $("#" + ddlGLProject).attr("disabled", false);
            }
            else {
                $("#" + ddlGLProject).attr("disabled", true);
            }

            var selectedIndex = parseFloat($('#' + ddlCurrency).prop("selectedIndex"));
            if (selectedIndex < 1) {
                $('#' + txtCalculatedAmount).val("")
                $('#' + txtConversionRate).val("")
                $('#' + txtLedgerAmount).val("");
                $('#ConversionPanel').hide();
                $('#' + txtLedgerAmount).attr("disabled", false);
            }
            else {
                $('#ConversionPanel').show();
                $('#' + txtLedgerAmount).val("");
            }

            var goValue = $("#<%=txtGoToScrolling.ClientID %>").val();
            window.scrollTo(0, $("#" + goValue).offset().top);

            //EnableDisable For DropDown Change event--------------
            //--Gl Company Change----------------------------------
            var ddlGLCompany = '<%=ddlGLCompany.ClientID%>'
            $('#' + ddlGLCompany).change(function () {
                PerformGLCompanyChangeAction($('#' + ddlGLCompany).val());
            });

            var ddlVoucherType = '<%=ddlVoucherType.ClientID%>'
            var ddlNodeId = '<%=ddlNodeId.ClientID%>'
            var txtJVChequeNumber = '<%=txtJVChequeNumber.ClientID%>'
            var txtChequeNumber = '<%=txtChequeNumber.ClientID%>'

            $('#' + ddlVoucherType).change(function () {
                PerformAddClickValidation();
            });
            $('#' + ddlNodeId).change(function () {
                PerformAddClickValidation();
            });
            $('#' + txtJVChequeNumber).blur(function () {
                var jvChequeNumber = '<%=txtJVChequeNumber.ClientID%>'
                if ($('#' + jvChequeNumber).val() != "") {
                    PerformAddClickValidation();
                }
                else {
                    EnableAddButton();
                }
            });
            $('#' + txtChequeNumber).blur(function () {
                var chequeNumber = '<%=txtChequeNumber.ClientID%>'
                if ($('#' + chequeNumber).val() != "") {
                    PerformAddClickValidation();
                }
                else {
                    EnableAddButton();
                }
            });



            $('#' + ddlCurrency).change(function () {
                var selectedIndex = parseFloat($('#' + ddlCurrency).prop("selectedIndex"));
                if (selectedIndex < 1) {
                    $('#' + txtCalculatedAmount).val("")
                    $('#' + txtConversionRate).val("")
                    $('#' + txtLedgerAmount).val("");
                    $('#ConversionPanel').hide();
                    $('#' + txtLedgerAmount).attr("disabled", false);
                }
                else {
                    $('#ConversionPanel').show();
                    $('#' + txtLedgerAmount).val("");
                }
            });

            $('#' + txtLedgerAmount).blur(function () {

                var selectedIndex = parseFloat($('#' + ddlCurrency).prop("selectedIndex"));
                if (selectedIndex < 1) {
                    var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val());

                    $('#' + txtCalculatedAmount).val(LedgerAmount.toFixed(2));
                }
                else {
                    var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val()) * parseFloat($('#' + txtConversionRate).val());
                    if (isNaN(LedgerAmount.toString())) {
                        LedgerAmount = 0;
                    }
                    $('#' + txtCalculatedAmount).attr("disabled", true);
                    $('#' + txtCalculatedAmount).val(LedgerAmount.toFixed(2));
                }
            });

            $('#' + txtConversionRate).blur(function () {
                var selectedIndex = parseFloat($('#' + ddlCurrency).prop("selectedIndex"));
                if (selectedIndex < 1) {
                    var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val());
                    $('#' + txtCalculatedAmount).val(LedgerAmount.toFixed(2));
                }
                else {
                    var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val()) * parseFloat($('#' + txtConversionRate).val());
                    if (isNaN(LedgerAmount.toString())) {
                        LedgerAmount = 0;
                    }
                    $('#' + txtCalculatedAmount).attr("disabled", true);
                    $('#' + txtCalculatedAmount).val(LedgerAmount.toFixed(2));
                }
            });

            //--Gl Common Change----------------------------------
            $(function () {
                var ddlVoucherType = '<%=ddlVoucherType.ClientID%>'
                var ddlVoucherMode = '<%=ddlVoucherMode.ClientID%>'
                var ddlCashChequeMode = '<%=ddlCashChequeMode.ClientID%>'
                var ddlGLProject = '<%=ddlGLProject.ClientID%>'
                var voucherType = 1;

                var isPopulatedConfigureAccountHead = 'No';
                $('#' + ddlVoucherType).change(function () {
                    $("#<%=txtIsCheckFirstTimeValidation.ClientID %>").val("1");

                    if ($('#' + ddlVoucherType).val() == "CP" || $('#' + ddlVoucherType).val() == "CR") {
                        if ($('#' + ddlVoucherType).val() == "CP") {
                            $('#' + ddlVoucherMode).val("Payment");
                        }
                        else {
                            $('#' + ddlVoucherMode).val("Received");
                        }
                        $('#' + ddlCashChequeMode).val("Cash");
                        $('#LedgerMode').hide();
                        voucherType = 1;
                        $("#ConfigureAccountHead").show();
                        isPopulatedConfigureAccountHead = 'Yes';
                        $("#ChequeInformation").hide();
                        $("#JVChequeInformation").hide();
                    }
                    else if ($('#' + ddlVoucherType).val() == "BP" || $('#' + ddlVoucherType).val() == "BR") {
                        if ($('#' + ddlVoucherType).val() == "BP") {
                            $('#' + ddlVoucherMode).val("Payment");
                        }
                        else {
                            $('#' + ddlVoucherMode).val("Received");
                        }
                        $('#' + ddlCashChequeMode).val("Bank");
                        $('#LedgerMode').hide();
                        voucherType = 1;
                        $("#ConfigureAccountHead").show();
                        isPopulatedConfigureAccountHead = 'Yes';
                        $("#ChequeInformation").show();
                        $("#JVChequeInformation").hide();
                    }
                    else if ($('#' + ddlVoucherType).val() == "JV") {
                        voucherType = 0;
                        $('#' + ddlVoucherMode).val("Journal");
                        $('#' + ddlCashChequeMode).val("Cash");
                        $('#LedgerMode').show();
                        $("#ConfigureAccountHead").hide();
                        isPopulatedConfigureAccountHead = 'No';
                        $("#ChequeInformation").hide();
                        $("#JVChequeInformation").show();
                    }
                    else if ($('#' + ddlVoucherType).val() == "CV") {
                        voucherType = 0;
                        $('#' + ddlVoucherMode).val("Cash & Bank Contra");
                        $('#' + ddlCashChequeMode).val("Cash");
                        $('#LedgerMode').show();
                        $("#ConfigureAccountHead").hide();
                        isPopulatedConfigureAccountHead = 'No';
                        $("#ChequeInformation").hide();
                        $("#JVChequeInformation").show();
                    }
                    else if ($('#' + ddlVoucherType).val() == "None") {
                        isPopulatedConfigureAccountHead = 'No';
                    }

                    PerformChangeAction();
                    if (isPopulatedConfigureAccountHead == 'Yes') {
                        PopulateConfigureAccountHeadAction($('#' + ddlGLProject).val(), $('#' + ddlVoucherType).val(), voucherType);
                        IsVisibleChangeButtonAction($('#' + ddlGLProject).val(), $('#' + ddlVoucherType).val(), voucherType);
                    }
                    $("#<%=lblConfigureAccountHead.ClientID %>").text($('#' + ddlVoucherType).val() + ' Account Head');
                    if (voucherType == 1) {
                        $('#LedgerMode').hide();
                    }
                    else {
                        $('#LedgerMode').show();
                    }
                    if ($("#<%=txtVoucherNo.ClientID %>").val() != '') {
                        $("#txtSearch").focus();
                    }
                    else {
                        $("#<%=txtVoucherNo.ClientID %>").focus();
                    }
                });

                //--Gl Project Change----------------------------------
                $('#' + ddlGLProject).change(function () {
                    $("#<%=txtIsCheckFirstTimeValidation.ClientID %>").val("1");
                    PerformChangeAction();
                });
            });

            PerformChangeAction();

            //--Perform Action-----------------------------------------
            function PerformGLCompanyChangeAction(mCompanyId) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "/GeneralLedger/frmVoucher.aspx/PopulateGLProject",
                    data: "{'companyId':'" + mCompanyId + "'}",
                    dataType: "json",
                    success: OnGLCompanyPopulated,
                    error: function (result) {
                        //alert("Error");
                    }
                });
            }
            function OnGLCompanyPopulated(response) {
                PopulateControlForVoucher(response.d, $("#<%=ddlGLProject.ClientID %>"));
            }

            function PopulateConfigureAccountHeadAction(mProjectId, mAccountType, mVoucherType) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "/GeneralLedger/frmVoucher.aspx/PopulateConfigureAccountHead",
                    data: "{'projectId':'" + mProjectId + "', 'accountType':'" + mAccountType + "', 'voucherType':'" + mVoucherType + "'}",
                    dataType: "json",
                    success: OnConfigureAccountHeadPopulated,
                    error: function (result) {
                        //alert("Error");
                    }
                });
            }
            function OnConfigureAccountHeadPopulated(response) {
                $("#<%=ddlConfigureAccountHead.ClientID %>").empty();
                if (response.d.length > 0) {
                    PopulateControlForVoucher(response.d, $("#<%=ddlConfigureAccountHead.ClientID %>"));
                    $("#<%=lblConfigureAccountHeadText.ClientID %>").text($("#<%=ddlConfigureAccountHead.ClientID %>").text());
                    $("#<%=hfConfigureAccountHead.ClientID %>").val($("#<%=ddlConfigureAccountHead.ClientID %>").val());
                }
                else {
                    $("#<%=ddlVoucherType.ClientID %>").val('None');
                    toastr.info('Please Configure for this voucher type');
                }
            }

            // Change Button Visible True/ False ----------------------
            function IsVisibleChangeButtonAction(mProjectId, mAccountType, mVoucherType) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "/GeneralLedger/frmVoucher.aspx/IsVisibleChangeButton",
                    data: "{'projectId':'" + mProjectId + "', 'accountType':'" + mAccountType + "', 'voucherType':'" + mVoucherType + "'}",
                    dataType: "json",
                    success: OnIsVisibleChangeButton,
                    error: function (result) {
                        //alert("Error");
                    }
                });
            }
            function OnIsVisibleChangeButton(response) {
                $("#<%=ddlConfigureAccountHead.ClientID %>").empty();
                if (response.d > 1) {
                    $("#<%=IsActiveChangeAccountHead.ClientID %>").val(5);
                    $("#<%=btnChangeAccountHead.ClientID %>").show();
                }
                else {
                    $("#<%=IsActiveChangeAccountHead.ClientID %>").val(1);
                    $("#<%=btnChangeAccountHead.ClientID %>").hide();
                }
            }

            //-- Common Populate Control -----------------------------
            function PopulateControlForVoucher(list, control) {
                if (list.length > 0) {
                    control.empty();
                    control.removeAttr("disabled");
                    if (list.length != 1) {
                        control.empty().append('<option selected="selected" value="0">--- Please Select ---</option>');
                    }
                    $.each(list, function () {
                        control.append($("<option></option>").val(this['Value']).html(this['Text']));
                    });
                }
                else {
                    control.empty().append('<option selected="selected" value="0">Not Available<option>');
                    control.attr("disabled", true);
                }
            }

            function PerformChangeAction() {

                var isCheckFirstTimeValidation = $("#<%=txtIsCheckFirstTimeValidation.ClientID %>").val();
                if (isCheckFirstTimeValidation > 0) {
                    var mProjectId = $("#<%=ddlGLProject.ClientID %>").val();
                    var mVoucherType = $("#<%=ddlVoucherType.ClientID %>").val();
                    var mVoucherDate = $("#<%=txtVoucherDate.ClientID %>").val();
                    var mUserId = 1;

                    if (mProjectId == '0') {
                        //alert('Please Select Project Name.');
                        toastr.info("Please Select Project Name.");
                        $("#<%=txtVoucherNo.ClientID %>").val('');
                        $("#<%=txtVoucherNo.ClientID %>").attr('readonly', false);
                        $("#<%=ddlGLProject.ClientID %>").focus();
                        return false;
                    }
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "/GeneralLedger/frmVoucher.aspx/GenerateVoucherNumber",
                        data: "{'projectId':'" + mProjectId + "','voucherType':'" + mVoucherType + "','voucherDate':'" + mVoucherDate + "','userId':'" + mUserId + "'}",
                        dataType: "json",
                        success: function (data) {
                            //if (data.d != 'None') {
                            if (data.d != 'Manual') {
                                $("#<%=txtVoucherNo.ClientID %>").attr('readonly', true);
                                $("#<%=txtVoucherNo.ClientID %>").val(data.d);
                                $("#<%=txtVoucherNumberGenerated.ClientID %>").val('Auto');
                            }
                            else {
                                $("#<%=txtVoucherNo.ClientID %>").attr('readonly', false);
                                $("#<%=txtVoucherNo.ClientID %>").val('');
                                $("#<%=txtVoucherNumberGenerated.ClientID %>").val('Manual');
                            }
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                }
            }

            var VoucherDate = '<%=txtVoucherDate.ClientID%>'


            //--AutoComplete Mode----------
            SearchText();
            var ddlNodeId = '<%=ddlNodeId.ClientID%>'

            $("#txtSearch").blur(function () {
                SearchTextForId();
            });

            $("#<%=txtVoucherNo.ClientID %>").blur(function () {
                if ($("#<%=txtVoucherNumberGenerated.ClientID %>").val() == 'Manual') {
                    //alert($("#<%=txtVoucherNo.ClientID %>").val());
                    if ($("#<%=txtVoucherNo.ClientID %>").val() != '') {
                        CheckDuplicateVoucherNumber();
                    }
                }
            });
        });

        function CheckDuplicateVoucherNumber() {
            var mVoucherNo = $("#<%=txtVoucherNo.ClientID %>").val();
            var mProjectId = $("#<%=ddlGLProject.ClientID %>").val();
            var mVoucherType = $("#<%=ddlVoucherType.ClientID %>").val();

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/GeneralLedger/frmVoucher.aspx/CheckDuplicateVoucherNumber",
                data: "{'voucherNo':'" + mVoucherNo + "','projectId':'" + mProjectId + "','voucherType':'" + mVoucherType + "'}",
                dataType: "json",
                success: function (data) {
                    if (data.d != 'Valid') {
                        toastr.info("Voucher Number: " + $("#<%=txtVoucherNo.ClientID %>").val() + ", Already Exist.", "Duplicate Voucher No");
                        $("#<%=txtVoucherNo.ClientID %>").val('');
                    }
                },
                error: function (result) {
                    //alert("Error");
                }
            });
        }

        function FilteringSearchText(str) {
            return str.replace(/[']/g, escape).replace(/\*/g, "%2A");
        }

        function fixedEncodeURIComponent(str) {
            return encodeURIComponent(str).replace(/[!'()]/g, escape).replace(/\*/g, "%2A");
        }

        //---------------------------
        function SearchTextForId() {
            var vdata = document.getElementById('txtSearch').value;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/GeneralLedger/frmVoucher.aspx/FillForm",
                data: "{'searchText':'" + FilteringSearchText(vdata) + "'}",
                dataType: "json",
                success: function (data) {
                    var ddlNodeId = '<%=ddlNodeId.ClientID%>'
                    $('#' + ddlNodeId).val(data.d);
                },
                error: function (result) {
                    //alert("Error");
                }
            });
        }
        //---------------------------
        function SearchText() {
            $('.SearchAccountHeadTextBox').autocomplete({
                source: function (request, response) {
                    var vdata = document.getElementById('txtSearch').value;

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "/GeneralLedger/frmVoucher.aspx/GetAutoCompleteData1",
                        data: "{'searchText':'" + FilteringSearchText(vdata) + "'}",
                        dataType: "json",
                        success: function (data) {
                            response(data.d);

                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                }
            });
        }        

        //CompanyProjectPanel Div Visible True/False-------------------
        function CompanyProjectPanelShow() {
            $('#CompanyProjectPanel').show("slow");
        }
        function CompanyProjectPanelHide() {
            $('#CompanyProjectPanel').hide("slow");
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }

        //Change Account Head-----------------------------
        function ChangeAccountHead() {
            PerformChangeAccountHeadAction();
            popup(1, 'TouchKeypad', '', 412, 210);
            return false;
        }

        function PerformChangeAccountHeadAction() {
            var ddlVoucherType = '<%=ddlVoucherType.ClientID%>'
            var ddlGLProject = '<%=ddlGLProject.ClientID%>'

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/GeneralLedger/frmVoucher.aspx/PopulateChangeConfigureAccountHead",
                data: "{'projectId':'" + $('#' + ddlGLProject).val() + "', 'voucherType':'" + $('#' + ddlVoucherType).val() + "'}",
                dataType: "json",
                success: OnChangeAccountHeadPopulated,
                error: function (result) {
                    //alert("Error");
                }
            });
        }
        function OnChangeAccountHeadPopulated(response) {
            PopulateControlForChangeAccountHead(response.d, $("#<%=ddlChangeAccountHead.ClientID %>"));
            $("#<%=ddlChangeAccountHead.ClientID %>").val($("#<%=ddlConfigureAccountHead.ClientID %>").val());
        }

        //-- Populate for Change Selection Control -----------------------------
        function PopulateControlForChangeAccountHead(list, control) {
            if (list.length > 0) {
                control.empty();
                control.removeAttr("disabled");
                if (list.length != 1) {
                }
                $.each(list, function () {
                    control.append($("<option></option>").val(this['Value']).html(this['Text']));
                });
            }
            else {
                control.empty().append('<option selected="selected" value="0">Not Available<option>');
                control.attr("disabled", true);
            }
        }

        function ChangeAccountHeadAction() {
            PerformSetAccountHeadAction();
            return false;
        }
        function PerformSetAccountHeadAction() {
            var ddlChangeAccountHead = '<%=ddlChangeAccountHead.ClientID%>'
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/GeneralLedger/frmVoucher.aspx/PopulateAccountHeadByNodeId",
                data: "{'NodeId':'" + $('#' + ddlChangeAccountHead).val() + "'}",
                dataType: "json",
                success: OnSetAccountHeadPopulated,
                error: function (result) {
                    //alert("Error");
                }
            });
        }
        function OnSetAccountHeadPopulated(response) {
            PopulateControlForSetAccountHead(response.d, $("#<%=ddlConfigureAccountHead.ClientID %>"));
            $("#<%=lblConfigureAccountHeadText.ClientID %>").text($("#<%=ddlConfigureAccountHead.ClientID %>").text());
            $("#<%=hfConfigureAccountHead.ClientID %>").val($("#<%=ddlConfigureAccountHead.ClientID %>").val());
            popup(-1);
        }

        //-- Populate for Set Selection Control -----------------------------
        function PopulateControlForSetAccountHead(list, control) {
            if (list.length > 0) {
                control.empty();
                control.removeAttr("disabled");
                if (list.length != 1) {
                }
                $.each(list, function () {
                    control.append($("<option></option>").val(this['Value']).html(this['Text']));
                });
            }
            else {
                control.empty().append('<option selected="selected" value="0">Not Available<option>');
                control.attr("disabled", true);
            }
        }
     
    </script>    
    <div id="TouchKeypad" style="display: none;">
        <div id="Div1" class="block">
            <a href="#page-stats" class="block-heading" style="color: #FFFFFF;" data-toggle="collapse">
                Change Account Head</a>
            <div class="HMBodyContainer">
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="txtChangeAccountHeadId" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblChangeAccountHead" runat="server" Text="Account Head"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlChangeAccountHead" runat="server" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="HMContainerRowButton">
                    <input type="button" id="btnChange" value="Change" tabindex="2" class="TransactionalButton btn btn-primary"
                        onclick="javascript:return ChangeAccountHeadAction()" />
                    <input type="button" id="btnPopUpCancel" value="Cancel" tabindex="3" class="TransactionalButton btn btn-primary"
                        onclick="javascript:popup(-1);" />
                </div>
            </div>
        </div>
    </div>
    <div id="EntryPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Voucher Information</a>
        <div class="HMBodyContainer">
            <div class="divSection" id="CompanyProjectPanel" style="display: none;">
                <div class="divBox divSectionLeftLeft">
                    <asp:HiddenField ID="txtDealId" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="txtIsCheckFirstTimeValidation" runat="server"></asp:HiddenField>
                    <asp:Label ID="lblGLCompany" runat="server" Text="Company"></asp:Label>
                    <span class="MandatoryField">*</span>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlGLCompany" runat="server" TabIndex="4">
                    </asp:DropDownList>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblGLProject" runat="server" Text="Project"></asp:Label>
                    <span class="MandatoryField">*</span>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:DropDownList ID="ddlGLProject" runat="server" TabIndex="5">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:HiddenField ID="SingleprojectId" runat="server"></asp:HiddenField>
                    <asp:Label ID="lblVoucherDate" runat="server" Text="Voucher Date"></asp:Label>
                    <span class="MandatoryField">*</span>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtVoucherDate" runat="server" CssClass="datepicker" TabIndex="6"></asp:TextBox>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblVoucherType" runat="server" Text="Voucher Type"></asp:Label>
                    <span class="MandatoryField">*</span>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:DropDownList ID="ddlVoucherType" runat="server" TabIndex="7">
                        <asp:ListItem Value="None">Select One</asp:ListItem>
                        <asp:ListItem Value="CP">Cash Payment (CP) </asp:ListItem>
                        <asp:ListItem Value="BP">Bank Payment (BP)</asp:ListItem>
                        <asp:ListItem Value="CR">Cash Receive (CR)</asp:ListItem>
                        <asp:ListItem Value="BR">Bank Receive (BR)</asp:ListItem>
                        <asp:ListItem Value="JV">Journal Voucher (JV)</asp:ListItem>
                        <asp:ListItem Value="CV">Contra Voucher (CV)</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div id="VoucherNumberPanel" style="display: none;">
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblVoucherNo" runat="server" Text="Voucher No"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <div style="display: none;">
                            <asp:TextBox ID="txtVoucherNumberGenerated" runat="server"></asp:TextBox>
                        </div>
                        <asp:TextBox ID="txtVoucherNo" runat="server" CssClass="ThreeColumnTextBox" TabIndex="8"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
            </div>
            <div class="divSection" style="display: none;">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblVoucherMode" runat="server" Text="Voucher Mode"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlVoucherMode" runat="server" TabIndex="9">
                        <asp:ListItem>Select One</asp:ListItem>
                        <asp:ListItem>Payment</asp:ListItem>
                        <asp:ListItem>Received</asp:ListItem>
                        <asp:ListItem>Journal</asp:ListItem>
                        <asp:ListItem>Cash & Bank Contra</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblTransactionMode" runat="server" Text="Payment Mode"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:DropDownList ID="ddlCashChequeMode" runat="server" TabIndex="10">
                        <asp:ListItem>Not Applicable</asp:ListItem>
                        <asp:ListItem>Cash</asp:ListItem>
                        <asp:ListItem>Bank</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblPayerOrPayee" runat="server" Text="Payer/ Payee"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtPayerOrPayee" runat="server" CssClass="ThreeColumnTextBox" TabIndex="11"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection" style="display: none;">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblGoToScrolling" runat="server" Text="Go To Scrolling"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtGoToScrolling" TabIndex="12" runat="server" CssClass="customLargeTextBoxSize"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="childDivSection">
                <div id="TransectionDetailsInformation" class="block">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Details Information
                    </a>
                    <div class="HMBodyContainer">
                        <div class="divSection" id="ConfigureAccountHead">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblConfigureAccountHead" runat="server" Text="Account Head"></asp:Label>
                                <span class="MandatoryField">*</span>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <div id="ConfigureAccountHeadLabel">
                                    <asp:Label ID="lblConfigureAccountHeadText" runat="server" Text="Not Available" Font-Bold="True"></asp:Label>
                                    <asp:HiddenField ID="IsActiveChangeAccountHead" runat="server"></asp:HiddenField>
                                    <asp:Button ID="btnChangeAccountHead" runat="server" Text="Change" OnClientClick="javascript:return ChangeAccountHead()"
                                        CssClass="TransactionalButton btn btn-primary" TabIndex="13" />
                                </div>
                                <div id="ConfigureAccountHeadControl" style="display: none;">
                                    <asp:HiddenField ID="hfConfigureAccountHead" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hfChequeNumberForConfigureAccountHead" runat="server"></asp:HiddenField>
                                    <asp:DropDownList ID="ddlConfigureAccountHead" CssClass="ThreeColumnDropDownList"
                                        TabIndex="14" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div id="ChequeInformation" style="display: none;">
                                <div class="divBox divSectionRightLeft">
                                    <asp:Label ID="lblChequeNumber" runat="server" Text="Cheque Number"></asp:Label>
                                </div>
                                <div class="divBox divSectionRightRight">
                                    <asp:TextBox ID="txtChequeNumber" runat="server" TabIndex="15"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection" style="display: none;">
                        <div class="divBox divSectionLeftLeft">
                            <asp:HiddenField ID="txtEditNodeId" runat="server"></asp:HiddenField>
                            <asp:Label ID="lblNodeId" runat="server" Text="Account Head"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlNodeId" CssClass="ThreeColumnDropDownList" runat="server"
                                TabIndex="16">
                            </asp:DropDownList>
                            <asp:HiddenField ID="SelectedNodeId" runat="server"></asp:HiddenField>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblSrcNodeHead" runat="server" Text="Account Head"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <input type="text" id="txtSearch" class="ThreeColumnTextBox SearchAccountHeadTextBox"
                                tabindex="17" />
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div id="JVChequeInformation" class="divSection" style="display: none;">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="Label1" runat="server" Text="Cheque Number"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtJVChequeNumber" runat="server" class="ThreeColumnTextBox" TabIndex="18"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblCurrency" runat="server" Text="Currency Type"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlCurrency" runat="server" TabIndex="19">
                            </asp:DropDownList>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblLedgerAmount" runat="server" Text="Ledger Amount"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtLedgerAmount" runat="server" TabIndex="20"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div id="ConversionPanel" class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblConversionRate" runat="server" Text="Conversion Rate"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtConversionRate" runat="server" TabIndex="21"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblCalculatedAmount" runat="server" Text="Calculated Ammount"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtCalculatedAmount" runat="server" TabIndex="22"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div id="LedgerMode" style="display: none;">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblLedgerMode" runat="server" Text="Transection Mode"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:DropDownList ID="ddlLedgerMode" runat="server" TabIndex="23">
                                    <asp:ListItem>Select One</asp:ListItem>
                                    <asp:ListItem>Debit</asp:ListItem>
                                    <asp:ListItem>Credit</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblNodeNarration" runat="server" Text="Narration"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtNodeNarration" runat="server" CssClass="ThreeColumnTextBox" TabIndex="24"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="HMContainerRowButton">
                        <%--Right Left--%>
                        <asp:Button ID="btnAddDetail" runat="server" OnClick="btnAddDetail_Click" Text="Add"
                            CssClass="TransactionalButton btn btn-primary" TabIndex="25" OnClientClick="javascript: return ValidateLedgerAmount();" />
                        <asp:Label ID="lblHiddenId" runat="server" Text='' Visible="False"></asp:Label>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="childDivSection">
                        <asp:GridView ID="gvDetail" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="500"
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
                    <div id="TotalCalculateDebitCreditAmount" runat="server">
                        <div class="divClear">
                        </div>
                        <div style="text-align: left">
                            <asp:Label ID="lblTotalDebitAmount" runat="server" Text="Total Debit Amount :" Font-Bold="True"></asp:Label>
                            <asp:Label ID="lblTotalCalculateDebitAmount" runat="server" Font-Bold="True"></asp:Label>
                        </div>
                        <div class="divClear">
                        </div>
                        <div style="text-align: left">
                            <asp:Label ID="lblTotalCreditAmount" runat="server" Text="Total Credit Amount :"
                                Font-Bold="True"></asp:Label>
                            <asp:Label ID="lblTotalCalculateCreditAmount" runat="server" Font-Bold="True"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblNarration" runat="server" Text="Voucher Narration"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtNarration" runat="server" CssClass="ThreeColumnTextBox" TextMode="MultiLine"
                        TabIndex="26"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblCheckedBy" runat="server" Text="Checked By"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlCheckedBy" runat="server" TabIndex="27">
                    </asp:DropDownList>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblApprovedBy" runat="server" Text="Approved By"></asp:Label>
                    <span class="MandatoryField">*</span>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:DropDownList ID="ddlApprovedBy" runat="server" TabIndex="28">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="HMContainerRowButton">
                <%--Right Left--%>
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                    TabIndex="29" OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                    TabIndex="30" OnClick="btnCancel_Click" />
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var ddlVoucherType = '<%=ddlVoucherType.ClientID%>'
            $("#<%=lblConfigureAccountHead.ClientID %>").text($('#' + ddlVoucherType).val() + ' Account Head');
            if ($("#<%=txtVoucherNo.ClientID %>").val() != '') {
                $("#txtSearch").focus();
            }
            else {
                $("#<%=txtVoucherNo.ClientID %>").focus();
            }
        });

        var xDdl = '<%=isProjectDdlEnable%>';
        if (xDdl > -1) {
            var ddlGLProject = '<%=ddlGLProject.ClientID%>'
            $("#" + ddlGLProject).attr("disabled", false);
            $('#LedgerMode').show();
            $('ConfigureAccountHead').hide();
        }        

        var isReceivedOrPaymentVoucher = '<%=isReceivedOrPaymentVoucher%>';
        if (isReceivedOrPaymentVoucher > -1) {
            $('#LedgerMode').hide();
            $('ConfigureAccountHead').show();
        }
        else {
            $('#LedgerMode').show();
            $('ConfigureAccountHead').hide();
        }

        if ($("#<%=txtVoucherNo.ClientID %>").val() != '') {
            $("#txtSearch").focus();
        }
        else {
            $("#<%=txtVoucherNo.ClientID %>").focus();
        }

        var isEnableTransectionMode = '<%=isEnableTransectionMode%>';
        var ddlVoucherType = '<%=ddlVoucherType.ClientID%>'
        if (isEnableTransectionMode > -1) {
            $('#LedgerMode').show();
            $("#ConfigureAccountHead").hide();
            if ($('#' + ddlVoucherType).val() == "JV") {
                $("#ChequeInformation").hide();
                $("#JVChequeInformation").show();
                $("#ConfigureAccountHead").hide();
            }
            else if ($('#' + ddlVoucherType).val() == "BP") {
                $("#ChequeInformation").show();
                $("#JVChequeInformation").hide();
                $("#ConfigureAccountHead").show();
            }
            else if ($('#' + ddlVoucherType).val() == "BR") {
                $("#ChequeInformation").show();
                $("#JVChequeInformation").hide();
                $("#ConfigureAccountHead").show();
            }
            else if ($('#' + ddlVoucherType).val() == "CV") {
                $("#ChequeInformation").hide();
                $("#JVChequeInformation").show();
                $("#ConfigureAccountHead").hide();
            }
        }
        else {
            $('#LedgerMode').hide();
            $("#ConfigureAccountHead").show();
            if ($('#' + ddlVoucherType).val() == "JV") {
                $("#ChequeInformation").hide();
                $("#JVChequeInformation").show();
                $("#ConfigureAccountHead").hide();
            }
            else if ($('#' + ddlVoucherType).val() == "BP") {
                $("#ChequeInformation").show();
                $("#JVChequeInformation").hide();
                $("#ConfigureAccountHead").show();
            }
            else if ($('#' + ddlVoucherType).val() == "BR") {
                $("#ChequeInformation").show();
                $("#JVChequeInformation").hide();
                $("#ConfigureAccountHead").show();
            }
            else if ($('#' + ddlVoucherType).val() == "CV") {
                $("#ChequeInformation").hide();
                $("#JVChequeInformation").show();
                $("#ConfigureAccountHead").hide();
            }
        }

        var single = '<%=isSingle%>';
        if (single == "True") {
            $('#CompanyProjectPanel').hide();
            $('#SearchTypePanel').show();
        }
        else {
            $('#CompanyProjectPanel').show();
            $('#SearchTypePanel').hide();
        }
    </script>
</asp:Content>
