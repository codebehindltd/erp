<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReservationBillPayment.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmReservationBillPayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var editId = 0;
        $(function () {

            var ddlCurrency = '<%=ddlCurrency.ClientID%>'
            var txtCurrencyAmount = '<%=txtCurrencyAmount.ClientID%>'
            var txtConversionRate = '<%=txtConversionRate.ClientID%>'
            var txtLedgerAmount = '<%=txtLedgerAmount.ClientID%>'
            var txtLedgerAmountHiddenField = '<%=txtLedgerAmountHiddenField.ClientID%>'

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            CommonHelper.AutoSearchClientDataSource("txtBank", "ContentPlaceHolder1_ddlBank", "ContentPlaceHolder1_ddlBank");
            CommonHelper.AutoSearchClientDataSource("txtBankId", "ContentPlaceHolder1_ddlBankId", "ContentPlaceHolder1_ddlBankId");
            CommonHelper.AutoSearchClientDataSource("txtMBankingBankId", "ContentPlaceHolder1_ddlMBankingBankId", "ContentPlaceHolder1_ddlMBankingBankId");

            CommonHelper.ApplyDecimalValidation();
            $("#ContentPlaceHolder1_txtRsvCheckInDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $("#ContentPlaceHolder1_txtCheckOutDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $('#ContentPlaceHolder1_txtExpireDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#btnReservationSearch").click(function () {
                var check = true;
                $("#ContentPlaceHolder1_chkAllActiveReservation").attr("checked", true);
                //PopulateReservation(1);
                var guestName = $("#<%=txtResvGuestName.ClientID %>").val();
                var companyName = $("#<%=txtResvCompanyName.ClientID %>").val();
                var reservNumber = $("#<%=txtReservationNo.ClientID %>").val();
                var checkInDate = $("#<%=txtRsvCheckInDate.ClientID %>").val();
                var checkOutDate = $("#<%=txtCheckOutDate.ClientID %>").val();

                if (guestName == "" && companyName == "" && reservNumber == "" && checkInDate == "" && checkOutDate == "") {
                    check = false;
                }
                if (!check) {
                    toastr.warning("Please Give Atleast One Information");
                    return check;
                }

                LoadReservationInformation();
            });

            var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
            if (currencyType == "Local") {
                $('#' + txtCurrencyAmount).val("")
                $('#' + txtConversionRate).val("")
                $('#' + txtLedgerAmount).val("");
                $('#ConversionPanel').hide();
                $('#' + txtLedgerAmount).attr("disabled", false);
                $('#lblReceiveAmount').text("Payment Amount");
            }
            else {
                $('#ConversionPanel').show();
                $('#' + txtLedgerAmount).val("");
                $('#lblReceiveAmount').text("Calculated Amount");
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

            //function PopulateReservation(IsAllActiveReservation) {
            //    $.ajax({
            //        type: "POST",
            //        url: "/HotelManagement/frmRoomRegistrationNew.aspx/PopulateReservationDropDown",
            //        data: '{IsAllActiveReservation: ' + IsAllActiveReservation + '}',
            //        contentType: "application/json; charset=utf-8",
            //        dataType: "json",
            //        success: OnPopulateReservationPopulated,
            //        failure: function (response) {
            //            toastr.info(response.d);
            //        }
            //    });
            //}

            function LoadReservationInformation() {
                var guestName = $.trim($("#<%=txtResvGuestName.ClientID %>").val());
                var companyName = $.trim($("#<%=txtResvCompanyName.ClientID %>").val());
                var reservNumber = $.trim($("#<%=txtReservationNo.ClientID %>").val());
                var checkInDate = $.trim($("#<%=txtRsvCheckInDate.ClientID %>").val());
                var checkOutDate = $.trim($("#<%=txtCheckOutDate.ClientID %>").val());

                PageMethods.SearchNLoadReservationInfo(0, guestName, companyName, reservNumber, checkInDate, checkOutDate, OnLoadReservInfoSucceeded, OnLoadReservInfoFailed);
                return false;
            }
            function OnLoadReservInfoSucceeded(result) {
                $("#ltlReservationInformation").html(result);
                return false;
            }
            function OnLoadReservInfoFailed(error) {
            }

            function OnLoadConversionRateSucceeded(result) {
                if ($("#<%=hfCurrencyType.ClientID %>").val() == "Local") {
                    $("#<%=txtConversionRate.ClientID %>").val('');
                    //$('#<%=txtConversionRate.ClientID%>').attr("disabled", true);
                    $('#' + txtLedgerAmount).val("");
                    $('#ConversionPanel').hide();
                    $('#' + txtLedgerAmount).attr("disabled", false);
                    $('#lblReceiveAmount').text("Payment Amount");
                }
                else {
                    $('#ConversionPanel').show();
                    // $('#<%=txtConversionRate.ClientID%>').attr("disabled", false);
                    $("#<%=txtConversionRate.ClientID %>").val(result.ConversionRate);
                    $('#' + txtLedgerAmount).val("");
                    $('#lblReceiveAmount').text("Calculated Amount");
                }

                var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
                if (ddlCurrency == 0) {
                    $('#ConversionPanel').hide();
                    $('#lblReceiveAmount').text("Payment Amount");
                }
            }
            function OnLoadConversionRateFailed() {
            }

            var ddlSearchType = '<%=ddlSearchType.ClientID%>'

            $('#' + ddlSearchType).change(function () {
                var txtSearchReservationCodeOrGusetValue = '<%=txtSearchReservationCodeOrGuset.ClientID%>'
                if ($(this).val() == "RESERVATIONCODE") {
                    $('#lblReservation').text("Reservation Number");
                    $('#' + txtSearchReservationCodeOrGusetValue).attr("placeholder", "Enter Reservation Number...").placeholder();
                }
                else {
                    $('#lblReservation').text("Guest Name");
                    $('#' + txtSearchReservationCodeOrGusetValue).attr("placeholder", "Enter Reservation Guest Name...").placeholder();
                }

                $("#ContentPlaceHolder1_hfReservationId").val("");
                $("#gvGuestHouseService tr:not(:first-child)").remove();
                $("#GridPagingContainer ul").html("");
            });

            $('#' + txtCurrencyAmount).blur(function () {
                var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
                if (currencyType == "Local") {
                    var LedgerAmount = parseFloat($('#' + txtCurrencyAmount).val());

                    $('#' + txtLedgerAmount).val(LedgerAmount.toFixed(2));
                    $('#' + txtLedgerAmountHiddenField).val(LedgerAmount.toFixed(2));
                }
                else {
                    var LedgerAmount = parseFloat($('#' + txtCurrencyAmount).val()) * parseFloat($('#' + txtConversionRate).val());
                    if (isNaN(LedgerAmount.toString())) {
                        LedgerAmount = 0;
                    }
                    $('#' + txtLedgerAmount).attr("disabled", true);
                    $('#' + txtLedgerAmount).val(LedgerAmount.toFixed(2));
                    $('#' + txtLedgerAmountHiddenField).val(LedgerAmount.toFixed(2));
                }
            });

            $('#' + txtConversionRate).blur(function () {
                var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
                if (currencyType == "Local") {
                    var LedgerAmount = parseFloat($('#' + txtCurrencyAmount).val());
                    $('#' + txtLedgerAmount).val(LedgerAmount.toFixed(2));
                    $('#' + txtLedgerAmountHiddenField).val(LedgerAmount.toFixed(2));
                }
                else {
                    var LedgerAmount = parseFloat($('#' + txtCurrencyAmount).val()) * parseFloat($('#' + txtConversionRate).val());
                    if (isNaN(LedgerAmount.toString())) {
                        LedgerAmount = 0;
                    }
                    $('#' + txtLedgerAmount).attr("disabled", true);
                    $('#' + txtLedgerAmount).val(LedgerAmount.toFixed(2));
                    $('#' + txtLedgerAmountHiddenField).val(LedgerAmount.toFixed(2));
                }
            });

            $('#' + txtLedgerAmount).blur(function () {
                $('#' + txtLedgerAmountHiddenField).val($('#' + txtLedgerAmount).val());
            });


            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            $('#' + ddlPayMode).change(function () {

                if ($('#' + ddlPayMode).val() == "Cash") {
                    $('#CashReceiveAccountsInfo').show();
                    $('#CardReceiveAccountsInfo').hide();
                    $('#ChequeReceiveAccountsInfo').hide();
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $('#MBankingPaymentAccountHeadDiv').hide();
                    $('#PaidByOtherRoomDiv').hide();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                }
                else if ($('#' + ddlPayMode).val() == "Card") {
                    $('#CashReceiveAccountsInfo').hide();
                    $('#CardReceiveAccountsInfo').show();
                    $('#ChequeReceiveAccountsInfo').hide();
                    $('#CardPaymentAccountHeadDiv').show();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $('#MBankingPaymentAccountHeadDiv').hide();
                    $('#PaidByOtherRoomDiv').hide();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                }
                else if ($('#' + ddlPayMode).val() == "Cheque") {
                    $('#CashReceiveAccountsInfo').hide();
                    $('#CardReceiveAccountsInfo').hide();
                    $('#ChequeReceiveAccountsInfo').show();
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').show();
                    $('#MBankingPaymentAccountHeadDiv').hide();
                    $('#PaidByOtherRoomDiv').hide();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                }
                else if ($('#' + ddlPayMode).val() == "M-Banking") {
                    $('#MBankingPaymentAccountHeadDiv').show();
                    $('#CashReceiveAccountsInfo').hide();
                    $('#CardReceiveAccountsInfo').hide();
                    $('#ChequeReceiveAccountsInfo').hide();
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $('#PaidByOtherRoomDiv').hide();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                }

                LedgerAmountShowHide();
            });
            if ($("#ContentPlaceHolder1_hfIsSavePermission").val() == "0")
                $("#ContentPlaceHolder1_btnSave").hide();
        });

        $(document).ready(function () {
            $('#ContentPlaceHolder1_btnSrcRoomNumber').click(function () {
                if ($("#ContentPlaceHolder1_txtSrcRoomNumber").val() == "") {
                    toastr.warning("Please Provide Valid Reservation Number..");
                    return false;
                }
            });

            $("#ContentPlaceHolder1_ddlReservationPaymentType").val('Advance');
            $("#ContentPlaceHolder1_ddlPaymentType option[value=" + 'Refund' + "]").hide();

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Reservation Payment</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#gvGuestHouseService tbody tr:eq(1)").remove();

            var reservationId = $("#ContentPlaceHolder1_hfReservationId").val();

            if (reservationId == "") {

                $("#gvGuestHouseService tr:not(:first-child)").remove();
                $("#GridPagingContainer ul").html("");

                return false;
            }
            else {

                $("#<%=hfResvIdForPrevw.ClientID%>").val(reservationId);
                PageMethods.LoadReservationBillInfo(reservationId, 1, 1, 1, OnLoadReservationBillInfoSucceed, OnLoadReservationBillInfoFailed);
            }

            //EnableDisable For DropDown Change event--------------
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
            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            if ($('#' + ddlPayMode).val() == "Cash") {
                $('#CashReceiveAccountsInfo').show();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').show();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').show();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Cheque") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "M-Banking") {
                $('#MBankingPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }

            $("#ContentPlaceHolder1_txtSearchReservationCodeOrGuset").blur(function () {
                var reservationId = $("#ContentPlaceHolder1_hfReservationId").val();

                if (reservationId == "") {

                    $("#gvGuestHouseService tr:not(:first-child)").remove();
                    $("#GridPagingContainer ul").html("");

                    return false;
                }

                $("#<%=hfResvIdForPrevw.ClientID%>").val(reservationId);
                PageMethods.LoadReservationBillInfo(reservationId, 1, 1, 1, OnLoadReservationBillInfoSucceed, OnLoadReservationBillInfoFailed);

            });

            $("#ContentPlaceHolder1_txtSearchReservationCodeOrGuset").autocomplete({
                source: function (request, response) {

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "/HotelManagement/frmReservationBillPayment.aspx/SearchGuestByReservationOrGuestName",
                        data: "{'searchText':'" + request.term + "','searchType':'" + $("#<%=ddlSearchType.ClientID %>").val() + "', 'paymentType':'" + $("#<%=ddlPaymentType.ClientID %>").val() + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.ReservedCompany,
                                    value: m.ReservationId
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
                    event.preventDefault();
                    $(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    event.preventDefault();
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfReservationId").val(ui.item.value);
                }
            });
            $("#ContentPlaceHolder1_ddlReservationPaymentType").change(function () {
                var paymentType = $("#ContentPlaceHolder1_ddlReservationPaymentType").val();
                var reservationId = $("#ContentPlaceHolder1_hfReservationId").val();
                if (paymentType == 'Refund') {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "/HotelManagement/frmReservationBillPayment.aspx/SearchMaximumRefundAmount",
                        data: "{'reservationId':'" + reservationId + "','editId':'" + editId + "'}",
                        dataType: "json",
                        success: function (data) {
                            $("#ContentPlaceHolder1_hfMaxRefundAmount").val(data.d);
                        },
                        error: function (result) {

                        }
                    });
                }
                if ($("#ContentPlaceHolder1_ddlReservationPaymentType").val() == 'Advance') {
                    document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                }
            });
            $("#ContentPlaceHolder1_txtLedgerAmount").blur(function () {
                if ($("#ContentPlaceHolder1_ddlReservationPaymentType").val() == 'Refund') {
                    
                    if (parseFloat($("#ContentPlaceHolder1_txtLedgerAmount").val()) > parseFloat($("#ContentPlaceHolder1_hfMaxRefundAmount").val())) {
                        toastr.warning("Maximum Refund Amount is " + parseFloat($("#ContentPlaceHolder1_hfMaxRefundAmount").val()));
                        $("#ContentPlaceHolder1_txtLedgerAmount").focus();
                        document.getElementById("ContentPlaceHolder1_btnSave").disabled = true;
                        return false;
                    }
                    else
                    {
                        document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                        return true;
                    }
                }
            });

        });

        function LedgerAmountShowHide() {
            var ddlCurrency = '<%=ddlCurrency.ClientID%>'
            var txtLedgerAmount = '<%=txtLedgerAmount.ClientID%>'
            var selectedIndex = parseFloat($('#' + ddlCurrency).prop("selectedIndex"));
            if (selectedIndex < 1) {
                $('#' + txtLedgerAmount).attr("disabled", false);
            }
            else {
                $('#' + txtLedgerAmount).attr("disabled", true);
            }
        }

        function OnLoadReservationBillInfoSucceed(result) {
            $("#gvGuestHouseService tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"5\" >No Data Found</td> </tr>";
                $("#gvGuestHouseService tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            var adp = _.findWhere(result.GridData, { PaymentType: 'Advance', IsPaymentTransfered: 0 });

            if (adp != null) {//result.PaymentType == 'Advance'
                $("#ContentPlaceHolder1_ddlPaymentType option[value=" + 'Refund' + "]").show();
            }
            else {
                $("#ContentPlaceHolder1_ddlPaymentType option[value=" + 'Refund' + "]").hide();
            }

            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#gvGuestHouseService tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                var paymentType = gridObject.PaymentType == 'NoShowCharge' ? 'No Show' : gridObject.PaymentType;

                tr += "<td align='left' style=\"width:20%;\">" + gridObject.PaymentDateStringFormat + "</td>";
                tr += "<td align='left' style=\"width:20%;\">" + paymentType + "</td>";
                tr += "<td align='left' style=\"width:20%\">" + gridObject.CurrencyAmount + "</td>";
                tr += "<td align='left' style=\"width:20%\">" + gridObject.CreatedByName + "</td>";
                if ($("#<%=hfIsUpdatePermission.ClientID %>").val() == "1") {
                    editLink = "<a href=\"javascript:void();\" onclick=\"javascript:return PerformFillFormAction('" + gridObject.PaymentId + "', '" + gridObject.PaymentType + "', '" + result.GridPageLinks.CurrentPageNumber + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" /> </a>";
                }
                if ($("#<%=hfIsDeletePermission.ClientID %>").val() == "1") {
                    deleteLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformDeleteAction('" + gridObject.PaymentId + "', '" + gridObject.PaymentType + "', '" + result.GridPageLinks.CurrentPageNumber + "');\"><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
                }
                previewLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return ViewAction('" + gridObject.PaymentId + "', '" + gridObject.PaymentType + "');\"><img alt=\"Preview\" src=\"../Images/ReportDocument.png\" /></a>";

                tr += "<td align='center' style=\"width:20%;\">" + editLink + deleteLink + previewLink + "</td>";

                tr += "</tr>"

                $("#gvGuestHouseService tbody ").append(tr);
                tr = "";

            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            return false;
        }

        function ViewAction(paymentId, paymentType) {

            var url = "/HotelManagement/Reports/frmReportReservationBillPayment.aspx?PaymentIdList=" + paymentId + "," + paymentType + "";
            //var url = "/HotelManagement/Reports/frmReportGuestPaymentInvoice.aspx?PaymentIdList=" + paymentId;
            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
        }
        function OnLoadReservationBillInfoFailed(error) {
            $("#<%=hfReservationId.ClientID%>").val("");
            $("#<%=hfPaymentId.ClientID%>").val("");
            $("#<%=txtSearchReservationCodeOrGuset.ClientID %>").val("");
        }
        function OnLoadReservationBillInfoReviewFailed(error) {
            $("#<%=hfReservationId.ClientID%>").val("");
            $("#<%=hfPaymentId.ClientID%>").val("");
            $("#<%=txtSearchReservationCodeOrGuset.ClientID %>").val("");
        }

        function OnLoadReservationBillInfoPreviewFailed(error) {
            $("#<%=hfReservationId.ClientID%>").val("");
            $("#<%=hfPaymentId.ClientID%>").val("");
            $("#<%=txtSearchReservationCodeOrGuset.ClientID %>").val("");
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
                        toastr.error(response.d);
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
        function PerformFillFormAction(actionId, paymentType) {
            editId = actionId;
            PageMethods.FillForm(actionId, paymentType, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {
            $("#<%=hfCurrencyType.ClientID %>").val(result.CurrencyType);
            MessagePanelHide();
            var txtLedgerAmount = '<%=txtLedgerAmount.ClientID%>'
            if ($("#<%=hfCurrencyType.ClientID %>").val() != "Local") {
                $('#ConversionPanel').show();
                $('#' + txtLedgerAmount).attr("disabled", true);
                $("#<%=txtConversionRate.ClientID %>").val(result.PaymentAmount / result.CurrencyAmount);
            }
            else {
                $('#' + txtLedgerAmount).attr("disabled", false);
                $('#ConversionPanel').hide();
                $("#<%=txtConversionRate.ClientID %>").val('');
            }
            $("#<%=ddlCurrency.ClientID %>").val(result.FieldId);
            $("#<%=txtCurrencyAmount.ClientID %>").val(result.CurrencyAmount);

            $("#<%=txtLedgerAmount.ClientID %>").val(result.PaymentAmount);
            $("#<%=txtRemarks.ClientID %>").val(result.Remarks);

            $("#<%=hfReservationId.ClientID %>").val(result.ReservationId);
            //$("#<%=txtSearchReservationCodeOrGuset.ClientID %>").val(result.ReservedCompany);

            $("#<%=hfPaymentId.ClientID %>").val(result.PaymentId);
            $("#<%=hfDealId.ClientID %>").val(result.DealId);
            if ($("#ContentPlaceHolder1_hfIsUpdatePermission").val() == "0")
                $("#<%=btnSave.ClientID %>").hide();
            else
                $("#<%=btnSave.ClientID %>").val("Update").show();
            $('#btnNewBIll').hide();
            $('#EntryPanel').show();

            $("#<%=ddlPayMode.ClientID %>").val(result.PaymentMode);

            $("#<%=txtCardNumber.ClientID %>").val(result.CardNumber);
            $("#<%=ddlCardType.ClientID %>").val(result.CardType);
            $("#<%=txtCardHolderName.ClientID %>").val(result.CardHolderName);
            $("#<%=txtExpireDate.ClientID %>").val(GetStringFromDateTime(result.ExpireDate));

            $("#<%=ddlBankId.ClientID %>").val(result.BankId);
            $("#<%=ddlMBankingBankId.ClientID %>").val(result.BankId);
            $('#txtBankId').val($("#<%=ddlBankId.ClientID %> option:selected").text());
            $('#txtMBankingBankId').val($("#<%=ddlMBankingBankId.ClientID %> option:selected").text());
            $("#<%=txtChecqueNumber.ClientID %>").val(result.ChecqueNumber);

            $("#<%=ddlBank.ClientID %>").val(result.BankId);
            $('#txtBank').val($("#<%=ddlBank.ClientID %> option:selected").text());

            //if (result.PaymentType == "Refund") {
            //    $("#ContentPlaceHolder1_ddlPaymentType option[value=" + 'Refund' + "]").show();
            //}
            //else {
            //    $("#ContentPlaceHolder1_ddlPaymentType option[value=" + 'Refund' + "]").hide();
            //}

            $("#<%=ddlReservationPaymentType.ClientID %>").val(result.PaymentType).trigger('change');

            if (result.PaymentMode == "Cash") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
                $('#BankPaymentAccountHeadDiv').hide();
            }
            else if (result.PaymentMode == "Card") {
                $('#CardPaymentAccountHeadDiv').show();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
            }
            else if (result.PaymentMode == "Cheque") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
            }
            else if (result.PaymentMode == "M-Banking") {
                $('#MBankingPaymentAccountHeadDiv').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
            }

            return false;
        }
        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
        }

        //For Delete-------------------------        
        function PerformDeleteAction(actionId, paymentType) {

            var answer = confirm("Do you want to delete this record?")
            if (answer) {
                PageMethods.DeleteData(actionId, paymentType, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            }
            return false;
        }
        function OnDeleteObjectSucceeded(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            ReloadGrid(0);
            PerformClearAction();
        }
        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#gvGuestHouseService tbody tr").length - 1;
            PageMethods.LoadReservationBillInfo($("#ContentPlaceHolder1_hfReservationId").val(), gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadReservationBillInfoSucceed, OnLoadReservationBillInfoFailed);
            return false;
        }

        function ReloadGrid(IsCurrentOrPreviousPage) {
            var currentPageNumber = $("#GridPagingContainer ul li[class='active']").text();

            if (currentPageNumber == "")
                currentPageNumber = 1;

            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }

        function PerformSaveAction() {
            
            if (!ValidateForm())
                return false;

                var paymentId = $("#<%=hfPaymentId.ClientID %>").val();
                var reservationId = $("#<%=hfReservationId.ClientID %>").val();
                var fieldId = $("#<%=ddlCurrency.ClientID %>").val();
                var currencyAmount = $("#<%=txtCurrencyAmount.ClientID %>").val();
                var conversionRate = $("#<%=txtConversionRate.ClientID %>").val();
                var paymentAmout = $("#<%=txtLedgerAmount.ClientID %>").val();
                var paymentMode = $("#<%=ddlPayMode.ClientID %>").val();
                var bankId = $("#<%=ddlBankName.ClientID %>").val();
                var checqueNumber = $("#<%=txtChecqueNumber.ClientID %>").val();
                var cardNumber = $("#<%=txtCardNumber.ClientID %>").val();
                var dealId = $("#<%=hfDealId.ClientID %>").val();
                var cardHolderName = $("#<%=txtCardHolderName.ClientID %>").val();
                var expireDate = $("#<%=txtExpireDate.ClientID %>").val();
                var cardType = $("#<%=ddlCardType.ClientID %>").val();
                var paymentType = $("#<%=ddlReservationPaymentType.ClientID %>").val();
                var ddlMBankingBankId = $("#<%=ddlMBankingBankId.ClientID %>").val();
                var ddlMBankingReceiveAccountsInfo = $("#<%=ddlMBankingReceiveAccountsInfo.ClientID %>").val();
                var remarks = $("#<%=txtRemarks.ClientID %>").val();
                var maxRefund = $("#<%=hfMaxRefundAmount.ClientID %>").val();
                var ddlGLCompany = "", ddlGLProject = "", txtSearchReservationCodeOrGuset = "", receiveAccountsInfo = "";
                var receiveAccountsInfoText = "", ddlIncomeSourceAccountsInfo = "", ddlIncomeSourceAccountsInfoText = "";

                ddlGLCompany = $("#<%=ddlGLCompany.ClientID %>").val();
                ddlGLProject = $("#<%=ddlGLProject.ClientID %>").val();
                txtSearchReservationCodeOrGuset = $("#<%=txtSearchReservationCodeOrGuset.ClientID %>").val();
                ddlIncomeSourceAccountsInfo = $("#<%=ddlIncomeSourceAccountsInfo.ClientID %>").val();
                ddlIncomeSourceAccountsInfoText = $("#<%=ddlIncomeSourceAccountsInfo.ClientID %> option:selected").text();

                if (paymentMode == "Cash") {
                    receiveAccountsInfo = $("#<%=ddlCashReceiveAccountsInfo.ClientID %>").val();
                    receiveAccountsInfoText = $("#<%=ddlCashReceiveAccountsInfo.ClientID %> option:selected").text();
                }
                else if (paymentMode == "Card") {
                    receiveAccountsInfo = $("#<%=ddlCardReceiveAccountsInfo.ClientID %>").val();
                    receiveAccountsInfoText = $("#<%=ddlCardReceiveAccountsInfo.ClientID %> option:selected").text();
                    bankId = $("#ContentPlaceHolder1_ddlBank").val();
                }
                else if (paymentMode == "Cheque") {
                    receiveAccountsInfo = $("#<%=ddlChequeReceiveAccountsInfo.ClientID %>").val();
                    receiveAccountsInfoText = $("#<%=ddlChequeReceiveAccountsInfo.ClientID %> option:selected").text();
                    bankId = $("#ContentPlaceHolder1_ddlBankId").val();
                }
                else if (paymentMode == "M-Banking") {
                    receiveAccountsInfo = $("#<%=ddlMBankingReceiveAccountsInfo.ClientID %>").val();
                    receiveAccountsInfoText = $("#<%=ddlMBankingReceiveAccountsInfo.ClientID %> option:selected").text();
                }

                var reservationBillPayment = {};
                reservationBillPayment.PaymentId = paymentId == "" ? "0" : paymentId;
                reservationBillPayment.ReservationId = reservationId == "" ? "0" : reservationId;
                reservationBillPayment.FieldId = fieldId == "" ? "0" : fieldId;

                if ($("#<%=hfCurrencyType.ClientID %>").val() == "Local") {
                    reservationBillPayment.CurrencyAmount = parseFloat(paymentAmout);
                    reservationBillPayment.PaymentAmount = reservationBillPayment.CurrencyAmount;
                }
                else {
                    reservationBillPayment.CurrencyAmount = parseFloat(currencyAmount == "" ? "0" : currencyAmount);
                    reservationBillPayment.PaymentAmount = parseFloat(paymentAmout);
                }

                reservationBillPayment.PaymentMode = paymentMode;
                reservationBillPayment.BankId = bankId == "" ? "0" : bankId;
                reservationBillPayment.ChecqueNumber = checqueNumber;
                reservationBillPayment.CardNumber = cardNumber;
                reservationBillPayment.DealId = dealId == "" ? "0" : dealId;
                reservationBillPayment.CardHolderName = cardHolderName;
                reservationBillPayment.CardType = cardType;
                reservationBillPayment.PaymentType = paymentType;
                reservationBillPayment.AccountsPostingHeadId = receiveAccountsInfo == "" ? "0" : receiveAccountsInfo;

                var format = innBoarDateFormat.replace('mm', 'MM');
                var iformat = format.replace('yy', 'yyyy');

                if (expireDate != "")
                    reservationBillPayment.ExpireDate = "";

                PageMethods.PerformReservationBillPaymentSaveAction(reservationBillPayment, ddlGLCompany, ddlGLProject, txtSearchReservationCodeOrGuset, receiveAccountsInfo, receiveAccountsInfoText, ddlIncomeSourceAccountsInfo, ddlIncomeSourceAccountsInfoText, ddlMBankingBankId, ddlMBankingReceiveAccountsInfo, remarks, OnSaveEmployeeIncrementSucceed, OnSaveEmployeeIncrementFailed);
                return false;
            
        }
        function OnSaveEmployeeIncrementSucceed(result) {
            
            if (result.IsSuccess == false) {
                toastr.warning(result.AlertMessage);
            }
            else {
                if ($("#<%=btnSave.ClientID %>").val() == "Save") {
                    toastr.success('Saved Operation Successfull.');
                }
                else {
                    toastr.success('Update Operation Successfull.');
                }


                ReloadGrid(1);
                PerformClearAction();
            }
        }
        function OnSaveEmployeeIncrementFailed(error) {
            toastr.error(error.get_message());
        }

        //For ClearForm-------------------------
        function PerformClearAction() {
            //$("#<%=txtSrcRoomNumber.ClientID %>").val('');
            $("#<%=txtLedgerAmount.ClientID %>").val('');
            $("#<%=hfPaymentId.ClientID %>").val("");
            $("#<%=hfDealId.ClientID %>").val('');
            //$("#<%=ddlCurrency.ClientID %>").val('45');
            $("#<%=ddlCurrency.ClientID %>").val('1');
            $("#<%=txtCurrencyAmount.ClientID %>").val('');
            $("#<%=txtCardNumber.ClientID %>").val('');
            $("#<%=ddlCardType.ClientID %>").val('');
            $("#<%=txtCardHolderName.ClientID %>").val('');
            $("#<%=txtExpireDate.ClientID %>").val('');
            $("#<%=ddlBank.ClientID %>").val('');
            $('#txtBank').val('');
            $('#txtBankId').val('');
            $('#txtMBankingBankId').val('');
            $("#<%=txtConversionRate.ClientID %>").val('');
            $("#<%=txtChecqueNumber.ClientID %>").val('');
            $('#ConversionPanel').hide();
            $("#<%=txtRemarks.ClientID %>").val('');
            $("#<%=ddlPayMode.ClientID %>").val('Cash');

            $('#CashReceiveAccountsInfo').show();
            $('#CardReceiveAccountsInfo').hide();
            $('#ChequeReceiveAccountsInfo').hide();
            $('#CardPaymentAccountHeadDiv').hide();
            $('#ChecquePaymentAccountHeadDiv').hide();
            $('#MBankingPaymentAccountHeadDiv').hide();
            $('#PaidByOtherRoomDiv').hide();
            $('#CompanyPaymentAccountHeadDiv').hide();
            if ($("#ContentPlaceHolder1_hfIsSavePermission").val() == "0")
                $("#<%=btnSave.ClientID %>").hide();
            else
                $("#<%=btnSave.ClientID %>").val("Save").show();
            editId = 0;

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
            
            var validated = true;
            var paymentType = $("#ContentPlaceHolder1_ddlReservationPaymentType").val();
            var reservationId = $("#ContentPlaceHolder1_hfReservationId").val();
            
            var maxRefund = $("#ContentPlaceHolder1_hfMaxRefundAmount").val();
            var paymentAmount = $("#ContentPlaceHolder1_txtLedgerAmount").val();
            

            if ($("#ContentPlaceHolder1_ddlCurrency").val() == "0") {
                
                toastr.warning("Please Select Currency Type.");
                validated = false;
            }
            if ($("#<%=hfCurrencyType.ClientID %>").val() != "Local") {
                if ($("#ContentPlaceHolder1_txtConversionRate").val() == "") {
                    toastr.warning("Please Give Conversion Rate.");
                    validated = false;
                }
            }

            if ($("#<%=hfReservationId.ClientID%>").val() == "") {
                toastr.warning("Please Provide Valid Reservation Number.");
                validated = false;
            }

            if ($.trim($("#<%=txtLedgerAmount.ClientID %>").val()) == "") {
                toastr.warning("Please Give Payment Amount.");
                validated = false;
            }

            if ($("#ContentPlaceHolder1_ddlPaymentType").val() == "Refund") {
                if ($("#ContentPlaceHolder1_ddlPayMode").val() != "Cash") {
                    toastr.warning("Refund Payment Must be Cash.");
                    validated = false;
                }
            }
            
            if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Card") {
                var ddlCardType = $('#<%=ddlCardType.ClientID%>').val();
                var bankId = $("#<%=ddlBank.ClientID %>").val();
                var cardNumber = $.trim($("#<%=txtCardNumber.ClientID %>").val());

                if (ddlCardType == "") {
                    toastr.warning("Please select card.");
                    validated = false;
                }
                else if (bankId == "") {
                    toastr.warning("Please select bank.");
                    validated = false;
                }
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cheque") {
                var txtBankId = $("#<%=ddlBankId.ClientID %>").val();
                var txtChecqueNumber = $.trim($("#<%=txtChecqueNumber.ClientID %>").val());

                if (txtBankId == "") {
                    toastr.warning("Please select bank.");
                    validated = false;
                }
                else if (txtChecqueNumber == "") {
                    toastr.warning("Please Provide Checque Number.");
                    validated = false;
                }
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "M-Banking") {
                var txtMBankingBankId = $("#<%=ddlMBankingBankId.ClientID %>").val();
                if (txtMBankingBankId == "") {
                    toastr.warning("Please select bank.");
                    validated = false;
                }
            }

            return validated;
        }

        function RefundValidation()
        {
            var reservationId = $("#ContentPlaceHolder1_hfReservationId").val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/HotelManagement/frmReservationBillPayment.aspx/SearchMaximumRefundAmount",
                data: "{'reservationId':'" + reservationId + "','editId':'" + editId + "'}",
                dataType: "json",
                success: function (data) {
                    var maxToBeRefundable = data.d;
                    if (paymentAmount > maxToBeRefundable) {
                        $("#<%=txtLedgerAmount.ClientID %>").val(maxToBeRefundable);
                        toastr.warning("Maximum Refund Amount is " + maxToBeRefundable);
                        $("#ContentPlaceHolder1_txtLedgerAmount").focus();
                        return false;
                    }
                    else {
                        return true;
                                
                    }
                    $("#ContentPlaceHolder1_hfMaxRefundAmount").val(data.d);
                },
                error: function (result) {
                    toastr.warning("Refund validation check error!");
                    return false;
                }
            });
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
                toastr.warning(messege);
                return false;
            }
            else {
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
        function SearchReservation() {
            $("#ReservationPopEntryPanel").show('slow');

            $("#ReservationPopup").dialog({
                autoOpen: true,
                modal: true,
                width: 935,
                closeOnEscape: true,
                resizable: false,
                title: "Reservation Information",
                show: 'slide'
            });

            return false;
        }
    </script>
    <asp:HiddenField ID="txtCardValidation" runat="server"></asp:HiddenField>
    <div id="btnNewBIll" class="btn-toolbar" style="display: none;">
        <button onclick="javascript: return EntryPanelVisibleTrue();" class="btn btn-primary">
            <i class="icon-plus"></i>New Payment</button>
        <div class="btn-group">
        </div>
    </div>
    <asp:HiddenField ID="hfCurrencyType" runat="server" />
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfPaymentId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfDealId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfReservationId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfResvIdForPrevw" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfMaxRefundAmount" runat="server" Value="0"></asp:HiddenField>

    <asp:HiddenField ID="hfIsSavePermission" runat="server" />
    <asp:HiddenField ID="hfIsUpdatePermission" runat="server" />
    <asp:HiddenField ID="hfIsDeletePermission" runat="server" />
    <asp:HiddenField ID="hfIsViewPermission" runat="server" />
    <div id="ReservationPopup" style="display: none;">
        <div id="Div2" class="alert alert-info" style="display: none;">
            <button type="button" class="close" data-dismiss="alert">
                ×</button>
            <asp:Label ID='Label17' Font-Bold="True" runat="server"></asp:Label>
        </div>
        <div id="ReservationPopEntryPanel" class="panel panel-default" style="width: 875px">
            <div class="panel-heading">
                Search Reservation
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <%-- <div class="col-md-2">
                            <asp:Label ID="lblResvGuestName" runat="server" class="control-label" Text="Guest Name"></asp:Label>
                        </div>--%>
                        <label for="Country" class="control-label col-md-2">
                            Guest Name</label>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtResvGuestName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <%--<div class="col-md-2">
                            <asp:Label ID="lblResvCompanyName" runat="server" class="control-label" Text="Company Name"></asp:Label>
                        </div>--%>
                        <label for="Country" class="control-label col-md-2">
                            Company Name</label>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtResvCompanyName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <%-- <div class="col-md-2">
                            <asp:Label ID="lblRsvCheckInDate" runat="server" class="control-label" Text="Check-In Date"></asp:Label>
                        </div>--%>
                        <label for="Country" class="control-label col-md-2">
                            Check-In Date</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtRsvCheckInDate" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                        </div>
                        <%--<div class="col-md-2">
                            <asp:Label ID="lblCheckOutDate" runat="server" class="control-label" Text="Check-Out Date"></asp:Label>
                        </div>--%>
                        <label for="Country" class="control-label col-md-2">
                            Check-Out Date</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtCheckOutDate" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <%-- <div class="col-md-2">
                            <asp:Label ID="lblReservationNo" runat="server" class="control-label" Text="Reservation No."></asp:Label>
                        </div>--%>
                        <label for="Country" class="control-label col-md-2">
                            Reservation No.</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtReservationNo" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <button type="button" id="btnReservationSearch" class="btn btn-primary btn-sm">
                                Search</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="ReservationPopSearchPanel" class="panel panel-default" style="width: 875px">
            <div class="panel-heading">
                Search Information
            </div>
            <div class="panel-body">
                <div id="ltlReservationInformation">
                </div>
            </div>
        </div>
    </div>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Reservation Payment Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <label for="SearchType" class="control-label col-md-2 required-field">
                        Reservation Number</label>
                    <div class="col-md-4">
                        <div class="input-group">
                            <asp:TextBox ID="txtSrcRoomNumber" Style="height: 27px;" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            <span class="input-group-addon" style="padding: 2px 2px;">
                                <asp:Button ID="btnSrcRoomNumber" Style="height: 20px; padding-top: unset" runat="server" Text="Search" Width="80" TabIndex="2"
                                    CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSrcRoomNumber_Click" />
                            </span>
                            <span class="input-group-addon" style="padding: 2px 2px;">
                                <asp:ImageButton ID="imgReservationSearch" Style="height: 19px" Width="25" runat="server"
                                    OnClientClick="javascript:return SearchReservation()"
                                    ImageUrl="~/Images/SearchItem.png" ToolTip="More Search" />
                            </span>
                        </div>
                    </div>
                    <label for="PaymentType" class="control-label col-md-2 required-field">
                        Reservation Number</label>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlReservationId" runat="server" CssClass="form-control" TabIndex="3"
                            Enabled="False">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlReservationPaymentType" CssClass="form-control" runat="server">
                            <asp:ListItem Text="Advance" Value="Advance"></asp:ListItem>
                            <asp:ListItem Text="Refund" Value="Refund"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" style="display: none">
                    <label for="SearchType" class="control-label col-md-2">
                        Search Type</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSearchType" TabIndex="1" CssClass="form-control" runat="server">
                            <asp:ListItem Value="RESERVATIONCODE" Text="Reservation Number"></asp:ListItem>
                            <asp:ListItem Value="GUESTNAME" Text="Guest Name"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <label for="PaymentType" class="control-label col-md-2">
                        Payment Type</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlPaymentType" CssClass="form-control" runat="server">
                            <asp:ListItem Text="Advance" Value="Advance"></asp:ListItem>
                            <asp:ListItem Text="No Show" Value="NoShowCharge"></asp:ListItem>
                            <asp:ListItem Text="Refund" Value="Refund"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="guestSearchSection" style="display: none">
                    <div class="form-group">
                        <label for="ReservationNumber" id="lblReservation" class="control-label col-md-2 required-field">
                            Reservation Number</label>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtSearchReservationCodeOrGuset" runat="server" placeholder="Enter Reservation Number or Guest Name..."
                                CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                </div>






                <div class="form-group">
                    <label for="SearchType" class="control-label col-md-2">
                        Guest Name</label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtGuestName" Enabled="false" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="SearchType" class="control-label col-md-2">
                        Guest Email</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtGuestEmail" Enabled="false" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                    </div>
                    <label for="PaymentType" class="control-label col-md-2">
                        Guest Phone</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtGuestPhone" Enabled="false" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                    </div>
                </div>







                <div class="form-group">
                    <label for="PaymentMode" class="control-label col-md-2 required-field">
                        Payment Mode</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlPayMode" runat="server" CssClass="form-control" TabIndex="3">
                            <asp:ListItem Value="Cash">Cash</asp:ListItem>
                            <asp:ListItem Value="Card">Card</asp:ListItem>
                            <asp:ListItem Value="Cheque">Cheque</asp:ListItem>
                            <asp:ListItem Value="M-Banking">M-Banking</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <label for="CurrencyType" class="control-label col-md-2 required-field">
                        Currency Type</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control" TabIndex="4">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" id="CompanyProjectPanel" style="display: none;">
                    <label for="Company" class="control-label col-md-2">
                        Company</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlGLCompany" TabIndex="5" CssClass="form-control" runat="server"
                            onchange="PopulateProjects();">
                        </asp:DropDownList>
                    </div>
                    <label for="Project" class="control-label col-md-2">
                        Project</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlGLProject" TabIndex="6" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" style="display: none;">
                    <label for="IncomePurpose" class="control-label col-md-2">
                        Income Purpose</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlIncomeSourceAccountsInfo" TabIndex="7" CssClass="form-control"
                            runat="server">
                        </asp:DropDownList>
                    </div>
                    <div id="IntegratedGeneralLedgerDiv" style="display: none;">
                        <label for="PaymentReceiveIn" id="lblPaymentAccountHead" class="control-label col-md-2">
                            Payment Receive In</label>
                        <div class="col-md-4">
                            <div id="CashReceiveAccountsInfo">
                                <asp:DropDownList ID="ddlCashReceiveAccountsInfo" TabIndex="8" CssClass="form-control"
                                    runat="server">
                                </asp:DropDownList>
                            </div>
                            <div id="CardReceiveAccountsInfo" style="display: none;">
                                <asp:DropDownList ID="ddlCardReceiveAccountsInfo" TabIndex="9" CssClass="form-control"
                                    runat="server">
                                </asp:DropDownList>
                            </div>
                            <div id="ChequeReceiveAccountsInfo" style="display: none;">
                                <asp:DropDownList ID="ddlChequeReceiveAccountsInfo" TabIndex="10" CssClass="form-control"
                                    runat="server">
                                </asp:DropDownList>
                            </div>
                            <div id="MBankingReceiveAccountsInfo" style="display: none;">
                                <asp:DropDownList ID="ddlMBankingReceiveAccountsInfo" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="ConversionPanel" class="form-group">
                    <label for="PaymentAmount" class="control-label col-md-2 required-field">
                        Payment Amount</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCurrencyAmount" runat="server" CssClass="form-control" TabIndex="11"></asp:TextBox>
                    </div>
                    <label for="ConversionRate" class="control-label col-md-2">
                        Conversion Rate</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtConversionRate" TabIndex="12" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div id="ChecquePaymentAccountHeadDiv" style="display: none;">
                    <div class="form-group">
                        <label for="ChequeNumber" class="control-label col-md-2 required-field">
                            Cheque Number</label>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtChecqueNumber" runat="server" CssClass="form-control" TabIndex="14"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="BankName" class="control-label col-md-2 required-field">
                            Bank Name</label>
                        <div class="col-md-10">
                            <input id="txtBankId" class="form-control" type="text" />
                            <div style="display: none;">
                                <asp:DropDownList ID="ddlBankId" runat="server" CssClass="form-control" AutoPostBack="false">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="CardPaymentAccountHeadDiv" style="display: none;">
                    <div class="form-group" style="display: none;">
                        <label for="BankName" class="control-label col-md-2 required-field">
                            Bank Name</label>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlBankName" TabIndex="15" runat="server" CssClass="form-control"
                                AutoPostBack="false">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="CardType" class="control-label col-md-2 required-field">
                            Card Type</label>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCardType" TabIndex="16" runat="server" CssClass="form-control">
                                <asp:ListItem Value="">---Please Select---</asp:ListItem>
                                <asp:ListItem Value="a">American Express</asp:ListItem>
                                <asp:ListItem Value="m">Master Card</asp:ListItem>
                                <asp:ListItem Value="v">Visa Card</asp:ListItem>
                                <asp:ListItem Value="d">Discover Card</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <label for="CardNumber" class="control-label col-md-2">
                            Card Number</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtCardNumber" TabIndex="17" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="BankName" class="control-label col-md-2 required-field">
                            Bank Name</label>
                        <div class="col-md-10">
                            <input id="txtBank" type="text" class="form-control" />
                            <div style="display: none;">
                                <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control" AutoPostBack="false">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div style="display: none;">
                        <div class="form-group">
                            <label for="ExpiryDate" class="control-label col-md-2">
                                Expiry Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtExpireDate" TabIndex="18" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <label for="CardHolderName" class="control-label col-md-2">
                                Card Holder Name</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCardHolderName" TabIndex="19" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="MBankingPaymentAccountHeadDiv" style="display: none;">
                    <div class="form-group">
                        <label for="BankName" class="control-label col-md-2 required-field">
                            Bank Name</label>
                        <div class="col-md-10">
                            <input id="txtMBankingBankId" type="text" class="form-control" />
                            <div style="display: none;">
                                <asp:DropDownList ID="ddlMBankingBankId" runat="server" CssClass="form-control" AutoPostBack="false">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="ActionPanel">
                    <div class="form-group">
                        <label for="PaymentAmount" id="lblReceiveAmount" class="control-label col-md-2 required-field">
                            Payment Amount</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtLedgerAmount" runat="server" CssClass="form-control quantitydecimal" TabIndex="20"> </asp:TextBox>
                            <asp:HiddenField ID="txtLedgerAmountHiddenField" runat="server"></asp:HiddenField>
                        </div>
                        <div class="col-md-2">
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label for="Remarks" class="control-label col-md-2">
                        Remarks</label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                            TabIndex="27"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="21" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript:return PerformSaveAction()" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="22" CssClass="btn btn-primary btn-sm"
                            OnClientClick="javascript: return PerformClearAction();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Bill Payment Details
        </div>
        <div class="panel-body">
            <table class="table table-bordered table-condensed table-responsive" id='gvGuestHouseService'
                width="100%">
                <colgroup>
                    <col style="width: 20%;" />
                    <col style="width: 20%;" />
                    <col style="width: 20%;" />
                    <col style="width: 20%;" />
                    <col style="width: 20%;" />
                </colgroup>
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                        <td>Date
                        </td>
                        <td>Payment Type
                        </td>
                        <td>Payment Amount
                        </td>
                        <td>Received By
                        </td>
                        <td style="text-align: center;">Action
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
    <script type="text/javascript">
        var isIntegrated = '<%=isIntegratedGeneralLedgerDiv%>';
        if (isIntegrated > -1) { IntegratedGeneralLedgerDivPanelShow(); } else {
            IntegratedGeneralLedgerDivPanelHide();
        }

        function AssignReservationNumber(resultInfo) {
            $("#ContentPlaceHolder1_txtSrcRoomNumber").val(resultInfo);
            $("#ReservationPopup").dialog("close");
            $('#ContentPlaceHolder1_btnSrcRoomNumber').trigger('click');
        }
    </script>
</asp:Content>
