<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmAirlineTicketInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.AirTicketing.frmAirlineTicketInfo" %>

<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="companyProjectUserControlSrc" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">

        var queryReceiveOrderId = "";
        var AddedSerialCount = 0;

        var CurrencyList = new Array();
        var PurchaseOrderList = new Array();
        var LCList = new Array();

        var ItemSelected = null;
        var ReceiveOrderItem = new Array();
        var ReceiveOrderItemDeleted = new Array();
        var ReceiveOrderItemFromPurchase = new Array();
        var NewAddedSerial = new Array();
        var AddedSerialzableProduct = new Array();
        var DeletedSerialzableProduct = new Array();
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        var deletedAirlineInfoList = [];

        $(document).ready(function () {
            deletedAirlineInfoList = [];
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            $("#ContentPlaceHolder1_ddlAirlineName").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlProject").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlPaymentInstructionBank").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlTransactionType").change(function () {
                $("#ContentPlaceHolder1_ddlPayMode option[value='5']").remove();
                $("#ContentPlaceHolder1_ddlPayMode option[value='6']").remove();
                $("#ContentPlaceHolder1_ddlPayMode option[value='7']").remove();
                if ($("#ContentPlaceHolder1_ddlTransactionType").val() == "CorporateCompany") {
                    $("#ReferenceForCorporateCompany").show();
                    $("#ReferenceForWalkIn").hide();
                    $("#ReferenceForRoomGuest").hide();
                    $("#ContentPlaceHolder1_ddlPayMode").append("<option value='5'>Company</option>");
                }
                else if ($("#ContentPlaceHolder1_ddlTransactionType").val() == "WalkInCustomer") {
                    $("#ReferenceForWalkIn").show();
                    $("#ReferenceForCorporateCompany").hide();
                    $("#ReferenceForRoomGuest").hide();
                    $("#ContentPlaceHolder1_ddlPayMode option[value='5']").remove();
                }
                else if ($("#ContentPlaceHolder1_ddlTransactionType").val() == "RoomGuest") {
                    $("#ReferenceForRoomGuest").show();
                    $("#ReferenceForCorporateCompany").hide();
                    $("#ReferenceForWalkIn").hide();
                }
            });

            $("#ContentPlaceHolder1_txtCompany").autocomplete({
                minLength: 3,
                selectFirst: true,
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../AirTicketing/frmAirlineTicketInfo.aspx/GetCompanyInfoForAirTicket",
                        data: JSON.stringify({ searchTerm: request.term }),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.CompanyName,
                                    value: m.CompanyId,

                                };
                            });
                            response(searchData);
                        },
                        failed: function (result) {

                        }
                    });
                },
                focus: function (event, ui) {
                    event.preventDefault();
                },
                select: function (event, ui) {
                    event.preventDefault();
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfCompanyId").val(ui.item.value);
                }
            });

            $("#ContentPlaceHolder1_txtReferenceNameForWalkIn").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../AirTicketing/frmAirlineTicketInfo.aspx/GetGuestReferenceInfo",
                        data: JSON.stringify({ searchTerm: request.term }),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.ReferenceId,
                                };
                            });
                            response(searchData);
                        },
                        failed: function (result) {

                        }
                    });
                },
                focus: function (event, ui) {
                    event.preventDefault();
                },
                select: function (event, ui) {
                    event.preventDefault();
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfReferenceIdForWalkIn").val(ui.item.value);
                }
            });

            $("#ContentPlaceHolder1_txtReferenceForCompany").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../AirTicketing/frmAirlineTicketInfo.aspx/GetGuestReferenceInfoForCompany",
                        data: JSON.stringify({ companyId: $("#ContentPlaceHolder1_hfCompanyId").val(), searchTerm: request.term }),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.Id
                                }
                            });
                            response(searchData);
                        },
                        failed: function (result) {
                        }
                    });
                },
                focus: function (event, ui) {
                    event.preventDefault();
                },
                select: function (event, ui) {
                    event.preventDefault();
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfReferenceIdForCompany").val(ui.item.value);
                }
            });

            $("#ContentPlaceHolder1_txtbankName").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../AirTicketing/frmAirlineTicketInfo.aspx/GetBankInfoForAutoComplete",
                        data: JSON.stringify({ searchTerm: request.term }),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.BankName,
                                    value: m.BankId
                                }
                            });
                            response(searchData);
                        },
                        failed: function (result) {

                        }
                    });
                },
                focus: function (event, ui) {
                    event.preventDefault();
                },
                select: function (event, ui) {
                    event.preventDefault();
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfbankId").val(ui.item.value);
                }
            });

            $("#ContentPlaceHolder1_txtBankNameForMBanking").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../AirTicketing/frmAirlineTicketInfo.aspx/GetBankInfoForAutoComplete",
                        data: JSON.stringify({ searchTerm: request.term }),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.BankName,
                                    value: m.BankId
                                }
                            });
                            response(searchData);
                        },
                        failed: function (result) {

                        }
                    });
                },
                focus: function (event, ui) {
                    event.preventDefault();
                },
                select: function (event, ui) {
                    event.preventDefault();
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfbankId").val(ui.item.value);
                }
            });

            $("#ContentPlaceHolder1_txtBankNameForCheque").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../AirTicketing/frmAirlineTicketInfo.aspx/GetBankInfoForAutoComplete",
                        data: JSON.stringify({ searchTerm: request.term }),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.BankName,
                                    value: m.BankId
                                }
                            });
                            response(searchData);
                        },
                        failed: function (result) {

                        }
                    });
                },
                focus: function (event, ui) {
                    event.preventDefault();
                },
                select: function (event, ui) {
                    event.preventDefault();
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfbankId").val(ui.item.value);
                }
            });

            $("#ContentPlaceHolder1_txtIssueDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#ContentPlaceHolder1_txtFlightDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            
            $("#ContentPlaceHolder1_txtChequeDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#ContentPlaceHolder1_txtReturnDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
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

            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            var lblPaymentAccountHead = '<%=lblPaymentAccountHead.ClientID%>'

            if ($('#' + ddlPayMode).val() == "1") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#' + lblPaymentAccountHead).show();
                $('#ComPaymentDiv').hide();
                $('#RefundDiv').hide();
                $('#PrintPreviewDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "3") {
                $('#MBankingPaymentAccountHeadDiv').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#' + lblPaymentAccountHead).show();
                $('#ComPaymentDiv').hide();
                $('#RefundDiv').hide();
                $('#PrintPreviewDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "2") {
                $('#CardPaymentAccountHeadDiv').show();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#' + lblPaymentAccountHead).hide();
                $('#ComPaymentDiv').hide();
                $('#RefundDiv').hide();
                $('#PrintPreviewDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "4") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#' + lblPaymentAccountHead).hide();
                $('#ComPaymentDiv').hide();
                $('#RefundDiv').hide();
                $('#PrintPreviewDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "5") {
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').show();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#lblPaymentAccountHead').hide();
                $('#' + lblPaymentAccountHead).show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#ComPaymentDiv').show();
                $('#PrintPreviewDiv').hide();
                $('#RefundDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "6") {
                $('#PaidByOtherRoomDiv').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#' + lblPaymentAccountHead).show();
                $('#ComPaymentDiv').hide();
                $('#PrintPreviewDiv').show();
                $('#RefundDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "7") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#' + lblPaymentAccountHead).hide();
                $('#ComPaymentDiv').hide();
                $('#PrintPreviewDiv').show();
                $('#RefundDiv').show();
            }


            $(function () {
                var ddlPayMode = '<%=ddlPayMode.ClientID%>'
                var lblReceiveLeadgerAmount = '<%=lblReceiveLeadgerAmount.ClientID%>'
                var lblPaymentAccountHead = '<%=lblPaymentAccountHead.ClientID%>'
                var ddlCurrency = '<%=ddlCurrency.ClientID%>'

                $('#' + ddlPayMode).change(function () {
                    $('#' + lblReceiveLeadgerAmount).text("Receive Amount");
                    if ($('#' + ddlPayMode).val() == "1") {
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#MBankingPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').show();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).show();
                        $('#ComPaymentDiv').hide();
                        $('#RefundDiv').hide();
                        $('#PrintPreviewDiv').show();
                    }
                    else if ($('#' + ddlPayMode).val() == "3") {
                        $('#MBankingPaymentAccountHeadDiv').show();
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).show();
                        $('#ComPaymentDiv').hide();
                        $('#RefundDiv').hide();
                        $('#PrintPreviewDiv').show();
                    }
                    else if ($('#' + ddlPayMode).val() == "2") {
                        $('#CardPaymentAccountHeadDiv').show();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#MBankingPaymentAccountHeadDiv').hide();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).hide();
                        $('#ComPaymentDiv').hide();
                        $('#RefundDiv').hide();
                        $('#PrintPreviewDiv').show();
                    }
                    else if ($('#' + ddlPayMode).val() == "4") {
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').show();
                        $('#MBankingPaymentAccountHeadDiv').hide();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).hide();
                        $('#ComPaymentDiv').hide();
                        $('#RefundDiv').hide();
                        $('#PrintPreviewDiv').show();
                    }
                    else if ($('#' + ddlPayMode).val() == "5") {
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').show();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#MBankingPaymentAccountHeadDiv').hide();
                        $('#lblPaymentAccountHead').hide();
                        $('#' + lblPaymentAccountHead).show();
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#ComPaymentDiv').show();
                        $('#PrintPreviewDiv').hide();
                        $('#RefundDiv').hide();
                    }
                    else if ($('#' + ddlPayMode).val() == "6") {
                        $('#PaidByOtherRoomDiv').show();
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#MBankingPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).show();
                        $('#ComPaymentDiv').hide();
                        $('#PrintPreviewDiv').show();
                        $('#RefundDiv').hide();

                    }
                    else if ($('#' + ddlPayMode).val() == "7") {
                        $('#' + lblReceiveLeadgerAmount).text("Paid Out");
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#MBankingPaymentAccountHeadDiv').hide();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).hide();
                        $('#ComPaymentDiv').hide();
                        $('#PrintPreviewDiv').show();
                        $('#RefundDiv').show();
                    }
                });
            });

            $("#myTabs").tabs();
        });

        function AddItemForAirTicket() {
            if ($("#ContentPlaceHolder1_ddlTransactionType").val() == "0") {
                toastr.warning("Please Select Transaction Type.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtClientName").val() == "") {
                toastr.warning("Please Give Client Name.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtIssueDate").val() == "") {
                toastr.warning("Please Select Issue Date.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlTicketType").val() == "0") {
                toastr.warning("Please Select Ticket Type.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlAirlineName").val() == "0") {
                toastr.warning("Please Select Airline Name.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtFlightDate").val() == "") {
                toastr.warning("Please Select Flight Date.");
                return false;
            }
                //else if ($("#ContentPlaceHolder1_txtReturnDate").val() == "") {
                //    toastr.warning("Please Select Return Date.");
                //    return false;
                //}
            else if ($("#ContentPlaceHolder1_txtTicketNumber").val() == "") {
                toastr.warning("Please Give Ticket Number.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtPNR").val() == "") {
                toastr.warning("Please Give PNR.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtTicketValue").val() == "") {
                toastr.warning("Please Give Ticket Value.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtInvoiceAmount").val() == "") {
                toastr.warning("Please Give Invoice Amount.");
                return false;
            }
                //else if ($("#ContentPlaceHolder1_txtAirlineAmount").val() == "") {
                //    toastr.warning("Please Give a Number or Zero to Airline Amount.");
                //    return false;
                //}
            else if ($("#ContentPlaceHolder1_txtRoute").val() == "") {
                toastr.warning("Please Give Route.");
                return false;
            }

            AddItemForAirTicketTable();
        }

        function AddItemForAirTicketTable() {
            var clientName = $("#ContentPlaceHolder1_txtClientName").val();
            var mobileNumber = $("#ContentPlaceHolder1_txtMobileNo").val();
            var email = $("#ContentPlaceHolder1_txtEmail").val();
            var address = $("#ContentPlaceHolder1_txtAddress").val();
            var issueDate = $("#ContentPlaceHolder1_txtIssueDate").val();
            var ticketTypeId = $("#ContentPlaceHolder1_ddlTicketType option:selected").val();
            var ticketType = $("#ContentPlaceHolder1_ddlTicketType option:selected").text();
            var airlineId = $("#ContentPlaceHolder1_ddlAirlineName option:selected").val();
            var airlineName = $("#ContentPlaceHolder1_ddlAirlineName option:selected").text();
            var flightDate = $("#ContentPlaceHolder1_txtFlightDate").val();
            var returnDate = $("#ContentPlaceHolder1_txtReturnDate").val();
            var ticketNumber = $("#ContentPlaceHolder1_txtTicketNumber").val();
            var pnrNumber = $("#ContentPlaceHolder1_txtPNR").val();
            var ticketValue = $("#ContentPlaceHolder1_txtTicketValue").val();
            var invoiceAmount = $("#ContentPlaceHolder1_txtInvoiceAmount").val();
            var airlineAmount = $("#ContentPlaceHolder1_txtAirlineAmount").val();
            var routePath = $("#ContentPlaceHolder1_txtRoute").val();
            var remarks = $("#ContentPlaceHolder1_txtRemarks").val();

            if (airlineAmount == "") {
                airlineAmount = parseFloat(0);
            }
            
            var lastAirlineTicketId = $("#ContentPlaceHolder1_hfLastAirlineTicketId").val();
            lastAirlineTicketId = parseInt(lastAirlineTicketId);
            var lastTr = $("#TicketInformationTbl").find("tr").last();
            var lastId = $(lastTr).find("td").eq(0).html();
            var newId;
            if (lastId == undefined && lastAirlineTicketId == 0) {
                lastId = 0;
                newId = 1;
            }
            else {
                lastId = parseInt(lastAirlineTicketId);
                newId = lastId + 1;
            }

            if ($("#ContentPlaceHolder1_hfIsAirlineInfoEdit").val() != 1) {
                var tr = "";

                tr += "<tr>";
                tr += "<td style='display:none;'>" + newId + "</td>";
                tr += "<td style='width:20%;'>" + clientName + "</td>";
                tr += "<td style='width:20%;'>" + airlineName + "</td>";
                tr += "<td style='width:15%;'>" + issueDate + "</td>";
                tr += "<td style='width:15%;'>" + flightDate + "</td>";
                tr += "<td style='width:15%;'>" + invoiceAmount + "</td>";
                tr += "<td style='width:15%;'>" +
                    "<a href='javascript:void()' onclick= 'DeleteAirlineInfoItem(this)' ><img alt='Delete' src='../Images/delete.png' title='Delete' /></a>";
                tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return EditAirlineInfoItem(this)\" alt='Edit'  title='Edit' border='0' />";
                tr += "</td>";

                tr += "<td style='display:none;'>" + mobileNumber + "</td>";
                tr += "<td style='display:none;'>" + email + "</td>";
                tr += "<td style='display:none;'>" + address + "</td>";
                tr += "<td style='display:none;'>" + ticketTypeId + "</td>";
                tr += "<td style='display:none;'>" + ticketType + "</td>";
                tr += "<td style='display:none;'>" + airlineId + "</td>";
                tr += "<td style='display:none;'>" + returnDate + "</td>";
                tr += "<td style='display:none;'>" + ticketNumber + "</td>";
                tr += "<td style='display:none;'>" + pnrNumber + "</td>";
                tr += "<td style='display:none;'>" + airlineAmount + "</td>";
                tr += "<td style='display:none;'>" + routePath + "</td>";
                tr += "<td style='display:none;'>" + remarks + "</td>";
                tr += "<td style='display:none;'>" + ticketValue + "</td>";

                tr += "</tr>";

                $("#TicketInformationTbl tbody").append(tr);
                $("#ContentPlaceHolder1_hfLastAirlineTicketId").val(newId);

                var totalAmount = 0;
                $("#TicketInformationTbl tr").each(function () {
                    var amount = $(this).find("td").eq(5).html();
                    if (amount == undefined) {
                        amount = 0;
                    }
                    totalAmount = parseFloat(totalAmount) + parseFloat(amount);
                });
                totalAmount = totalAmount.toFixed(2);
                $("#ContentPlaceHolder1_txtTotalInvoiceAmount").val(totalAmount);
                $("#ContentPlaceHolder1_hftotalForTicketInfos").val(totalAmount);

                tr = "";

                ClearAfterAirlineInfoAdded();
            }
            else {
                $("#TicketInformationTbl tr").each(function () {
                    var currentAirlineId = $(this).find("td").eq(0).html();
                    if ($("#ContentPlaceHolder1_hfIsAirlineInfoEdit").val() == 1) {
                        var clickedId = $("#ContentPlaceHolder1_hfClickedAirlineId").val();
                        if (currentAirlineId == clickedId) {
                            $(this).find("td").eq(1).html(clientName);
                            $(this).find("td").eq(2).html(airlineName);
                            $(this).find("td").eq(3).html(issueDate);
                            $(this).find("td").eq(4).html(flightDate);
                            $(this).find("td").eq(5).html(invoiceAmount);
                            $(this).find("td").eq(7).html(mobileNumber);
                            $(this).find("td").eq(8).html(email);
                            $(this).find("td").eq(9).html(address);
                            $(this).find("td").eq(10).html(ticketTypeId);
                            $(this).find("td").eq(11).html(ticketType);
                            $(this).find("td").eq(12).html(airlineId);
                            $(this).find("td").eq(13).html(returnDate);
                            $(this).find("td").eq(14).html(ticketNumber);
                            $(this).find("td").eq(15).html(pnrNumber);
                            $(this).find("td").eq(16).html(airlineAmount);
                            $(this).find("td").eq(17).html(routePath);
                            $(this).find("td").eq(18).html(remarks);
                            $(this).find("td").eq(19).html(ticketValue);

                            var totalAmount = 0;
                            $("#TicketInformationTbl tr").each(function () {
                                var amount = $(this).find("td").eq(5).html();
                                if (amount == undefined) {
                                    amount = 0;
                                }
                                totalAmount = parseFloat(totalAmount) + parseFloat(amount);
                            });
                            totalAmount = totalAmount.toFixed(2);
                            $("#ContentPlaceHolder1_txtTotalInvoiceAmount").val(totalAmount);
                            $("#ContentPlaceHolder1_hftotalForTicketInfos").val(totalAmount);

                            tr = "";

                            ClearAfterAirlineInfoAdded();
                        }
                    }
                });
            }
        }

        function IsAirlineInfoExists(serialId) {
            var IsDuplicate = false;
            $("#TicketInformationTbl tr").each(function (index) {

                if (index !== 0 && !IsDuplicate) {
                    var serialIdValueInTable = $(this).find("td").eq(0).html();

                    serialIdValueInTable = parseInt(serialIdValueInTable);
                    serialId = parseInt(serialId);
                    var IsSerialIdFound;
                    if (serialIdValueInTable == serialId) {
                        IsSerialIdFound = true;
                    }
                    else {
                        IsSerialIdFound = false;
                    }

                    if (IsSerialIdFound) {
                        if ($("#ContentPlaceHolder1_hfIsAirlineInfoEdit").val() == 1) {
                            toastr.success('Payment Information Updated Successfully.');
                            IsDuplicate = true;
                        }
                        else {
                            toastr.warning('Payment Mode Already Added.');
                            IsDuplicate = true;
                            return true;
                        }
                    }
                }
            });
            return IsDuplicate;
        }

        function ClearAfterAirlineInfoAdded() {
            $("#btnAdd").val("Add");
            $("#ContentPlaceHolder1_txtClientName").val("");
            $("#ContentPlaceHolder1_txtMobileNo").val("");
            $("#ContentPlaceHolder1_txtEmail").val("");
            $("#ContentPlaceHolder1_txtAddress").val("");
            $("#ContentPlaceHolder1_txtIssueDate").val("");
            $("#ContentPlaceHolder1_ddlTicketType").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlAirlineName").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlAirlineName").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtFlightDate").val("");
            $("#ContentPlaceHolder1_txtReturnDate").val("");
            $("#ContentPlaceHolder1_txtTicketNumber").val("");
            $("#ContentPlaceHolder1_txtPNR").val("");
            $("#ContentPlaceHolder1_txtTicketValue").val("");
            $("#ContentPlaceHolder1_txtInvoiceAmount").val("");
            $("#ContentPlaceHolder1_txtAirlineAmount").val("");
            $("#ContentPlaceHolder1_txtRoute").val("");
            $("#ContentPlaceHolder1_txtRemarks").val("");
            $("#ContentPlaceHolder1_hfIsAirlineInfoEdit").val(0);
            $("#ContentPlaceHolder1_hfClickedAirlineId").val(0);
            return false;
        }

        function AddItemForPaymentInfo() {
            if ($("#ContentPlaceHolder1_ddlPayMode").val() == "0") {
                toastr.warning("Please Select Payment Mode");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlCurrency option:selected").val() == "0") {
                toastr.warning("Please Select Currency Type.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtReceiveLeadgerAmount").val() == "") {
                toastr.warning("Please Give Receive Amount.");
                return false;
            }
            AddItemForPaymentInfoTable();
        }

        function AddItemForPaymentInfoTable() {
            var paymentModeId = $("#ContentPlaceHolder1_ddlPayMode option:selected").val();
            var paymentModeName = $("#ContentPlaceHolder1_ddlPayMode option:selected").text();
            var currencyTypeId = $("#ContentPlaceHolder1_ddlCurrency option:selected").val();
            var currencyType = $("#ContentPlaceHolder1_ddlCurrency option:selected").text();
            var receiveAmount = $("#ContentPlaceHolder1_txtReceiveLeadgerAmount").val();
            var cardType = "", cardTypeId = 0, cardNumber = "", bankName = "", chequeNumber = "", chequeDate = "";
            var bankId = $("#ContentPlaceHolder1_hfbankId").val();
            if (paymentModeName == "Card") {
                cardTypeId = $("#ContentPlaceHolder1_ddlCardType option:selected").val();
                cardType = $("#ContentPlaceHolder1_ddlCardType option:selected").text();
                cardNumber = $("#ContentPlaceHolder1_txtCardNumber").val();
                bankName = $("#ContentPlaceHolder1_txtbankName").val();
            }
            else if (paymentModeName == "M-Banking") {
                bankName = $("#ContentPlaceHolder1_txtBankNameForMBanking").val();
            }
            else if (paymentModeName == "Cheque") {
                chequeNumber = $("#ContentPlaceHolder1_txtChecqueNumber").val();
                bankName = $("#ContentPlaceHolder1_txtBankNameForCheque").val();
                chequeDate = $("#ContentPlaceHolder1_txtChequeDate").val();
                if (chequeNumber == "") {
                    toastr.warning("Please Give Cheque Number.");
                    return false;
                }
                else if (chequeDate == "") {
                    toastr.warning("Please Select Cheque Date.");
                    return false;
                }
                else if (bankName == "") {
                    toastr.warning("Please Give Bank Name.");
                    return false;
                }
            }
            if (!IsPaymentHeadExists(paymentModeId)) {
                if ($("#ContentPlaceHolder1_hfEditPayment").val() == 0) {
                    var tr = "";

                    tr += "<tr>";
                    tr += "<td style='width:35%;'>" + paymentModeName + "</td>";
                    tr += "<td style='width:25%;'>" + bankName + "</td>";
                    tr += "<td style='width:25%;'>" + receiveAmount + "</td>";
                    tr += "<td style=\"width:15%;\">";                    
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'DeletePaymentInfoItem(this)' ><img alt='Delete' src='../Images/delete.png' title='Delete' /></a>";
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return EditPaymentInfoItem('" + paymentModeId + "','" + paymentModeName + "','" + bankId + "','" + bankName + "','" + receiveAmount + "','" + currencyTypeId + "','" + currencyType + "','" + cardTypeId + "','" + cardType + "','" + cardNumber + "','" + chequeNumber + "','" + chequeDate + "')\" alt='Edit'  title='Edit' border='0' />";
                    tr += "</td>";

                    tr += "<td style='display:none;'>" + paymentModeId + "</td>";
                    tr += "<td style='display:none;'>" + currencyTypeId + "</td>";
                    tr += "<td style='display:none;'>" + currencyType + "</td>";
                    tr += "<td style='display:none;'>" + cardType + "</td>";
                    tr += "<td style='display:none;'>" + cardTypeId + "</td>";
                    tr += "<td style='display:none;'>" + cardNumber + "</td>";
                    tr += "<td style='display:none;'>" + bankId + "</td>";
                    tr += "<td style='display:none;'>" + chequeNumber + "</td>";
                    tr += "<td style='display:none;'>" + chequeDate + "</td>";

                    tr += "</tr>";

                    $("#PaymentInformationTbl tbody").prepend(tr);

                    var totalAmount = 0;
                    $("#PaymentInformationTbl tr").each(function () {
                        var amount = $(this).find("td").eq(2).html();
                        if (amount == undefined) {
                            amount = 0;
                        }
                        totalAmount = parseFloat(totalAmount) + parseFloat(amount);
                    });
                    totalAmount = totalAmount.toFixed(2);
                    $("#ContentPlaceHolder1_txtTotalPaymentAmount").val(totalAmount);
                    $("#ContentPlaceHolder1_hftotalForPaymentInfos").val(totalAmount);

                    tr = "";

                    ClearAfterPaymentInfoAdded();
                }
            }
            else {
                $("#PaymentInformationTbl tr").each(function () {
                    var currentPaymentModeId = $(this).find("td").eq(4).html();
                    if ($("#ContentPlaceHolder1_hfEditPayment").val() == 1) {
                        if (currentPaymentModeId == paymentModeId) {
                            $(this).find("td").eq(0).html(paymentModeName);
                            $(this).find("td").eq(1).html(bankName);
                            $(this).find("td").eq(2).html(receiveAmount);
                            $(this).find("td").eq(4).html(paymentModeId);
                            $(this).find("td").eq(5).html(currencyTypeId);
                            $(this).find("td").eq(6).html(currencyType);
                            $(this).find("td").eq(7).html(cardType);
                            $(this).find("td").eq(8).html(cardTypeId);
                            $(this).find("td").eq(9).html(cardNumber);
                            $(this).find("td").eq(10).html(bankId);
                            $(this).find("td").eq(11).html(chequeNumber);
                            $(this).find("td").eq(12).html(chequeDate);
                        }
                    }
                });
                var totalAmount = 0;
                $("#PaymentInformationTbl tr").each(function () {
                    var amount = $(this).find("td").eq(2).html();
                    if (amount == undefined) {
                        amount = 0;
                    }
                    totalAmount = parseFloat(totalAmount) + parseFloat(amount);
                });
                totalAmount = totalAmount.toFixed(2);
                $("#ContentPlaceHolder1_txtTotalPaymentAmount").val(totalAmount);
                $("#ContentPlaceHolder1_hftotalForPaymentInfos").val(totalAmount);
                if ($("#ContentPlaceHolder1_hfEditPayment").val() == 1) {
                    ClearAfterPaymentInfoAdded();
                }
            }
        }

        function ClearAfterPaymentInfoAdded() {
            $("#ContentPlaceHolder1_ddlPayMode").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlCurrency").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtReceiveLeadgerAmount").val("");
            $("#ContentPlaceHolder1_ddlCardType").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtCardNumber").val("");
            $("#ContentPlaceHolder1_txtbankName").val("");
            $("#ContentPlaceHolder1_txtBankNameForMBanking").val("");
            $("#ContentPlaceHolder1_txtBankNameForCheque").val("");
            $("#ContentPlaceHolder1_txtChecqueNumber").val("");
            $("#ContentPlaceHolder1_hfBankId").val(0);
            $("#ContentPlaceHolder1_hfEditPayment").val(0);
            $("#btnAddDetailGuestPayment").val("Add");
            $('#ContentPlaceHolder1_ddlPayMode').attr('disabled', false);
            $("#ContentPlaceHolder1_txtChequeDate").val("");
        }        

        function IsPaymentHeadExists(paymentHeadId) {
            var IsDuplicate = false;
            $("#PaymentInformationTbl tr").each(function (index) {

                if (index !== 0 && !IsDuplicate) {
                    var paymentHeadIdValueInTable = $(this).find("td").eq(4).html();

                    var IsPaymentHeadIdFound = paymentHeadIdValueInTable.indexOf(paymentHeadId) > -1;
                    if (IsPaymentHeadIdFound) {
                        if ($("#ContentPlaceHolder1_hfEditPayment").val() == 1) {
                            toastr.success('Payment Information Updated Successfully.');
                            IsDuplicate = true;
                        }
                        else {
                            toastr.warning('Payment Mode Already Added.');
                            IsDuplicate = true;
                            return true;
                        }
                    }
                }
            });
            return IsDuplicate;
        }

        function ValidationBeforeSave() {
            var rowCountAT = $('#TicketInformationTbl tbody tr').length;
            if (rowCountAT == 0) {
                toastr.warning('Add at least one Airline Ticket Information.');
                $("#ContentPlaceHolder1_txtClientName").focus();
                return false;
            }

            var rowCountPayment = $('#PaymentInformationTbl tbody tr').length;
            if (rowCountPayment == 0) {
                toastr.warning('Add at least one Payment Information.');
                $("#ContentPlaceHolder1_ddlPayMode").focus();
                return false;
            }

            if ($("#ContentPlaceHolder1_ddlProject").val() == "0") {
                toastr.warning("Please Select Project.");
                return false;
            }

            var transactionType = "", companyName = "", companyId = "0", referenceName = "", registrationNumber = "",
                clientName = "", mobileNumber = "", email = "", address = "", issueDate = "", ticketTypeId = "",
                ticketType = "", airlineName = "", airlineId = "0", flightDate = "", returnDate = "",
                ticketNumber = "", pnrNumber = "", ticketValue = 0, invoiceAmount = 0, airlineAmount = 0, routePath = "", remarks = "", paymentModeId = "",
                paymentModeName = "", currencyTypeId = "", currencyType = "", receiveAmount = 0, cardTypeId = "", cardType = "", cardNumber = "", bankId = "0",
                bankName = "", chequeNumber = "", chequeDate = "";

            //var quantity = "0", finishedProductDetailsId = "0";
            //var isEdit = "0", finishProductId = "0";

            //var quantityRM = "0", finishedProductDetailsIdRM = "0";
            //var isEditRM = "0", finishProductIdRM = "0";

            //var accountHeadId = "0", amount = "0", description = "", accoutHeadDetailsId = "0";
            //var isEditOE = "0", finishProductIdOE = "0";

            var ticketId = $("#ContentPlaceHolder1_hfTicketMasterId").val();
            transactionType = $("#ContentPlaceHolder1_ddlTransactionType option:selected").val();
            projectId = $("#ContentPlaceHolder1_ddlProject").val();
            paymentInstructionBankId = $("#ContentPlaceHolder1_ddlPaymentInstructionBank").val();

            if (transactionType == "CorporateCompany") {
                companyId = $("#ContentPlaceHolder1_hfCompanyId").val();
                companyName = $("#ContentPlaceHolder1_txtCompany").val();
                referenceId = $("#ContentPlaceHolder1_hfReferenceIdForCompany").val();
                referenceName = $("#ContentPlaceHolder1_txtReferenceForCompany").val();
            }
            else if (transactionType == "WalkInCustomer") {
                companyName = $("#ContentPlaceHolder1_txtCompanyWalkInGuest").val();
                referenceId = $("#ContentPlaceHolder1_hfReferenceIdForWalkIn").val();
                referenceName = $("#ContentPlaceHolder1_txtReferenceNameForWalkIn").val();
            }

            if (transactionType == "RoomGuest") {
                registrationNumber = $("#ContentPlaceHolder1_txtRegistrationNumber").val();
            }

            var totalForTicketInfos = $("#ContentPlaceHolder1_hftotalForTicketInfos").val();
            totalForTicketInfos = parseFloat(totalForTicketInfos);
            totalForTicketInfos = totalForTicketInfos.toFixed(2);
            var AirTicketMasterInfo = {
                TicketId: ticketId,
                TransactionType: transactionType,
                CompanyId: companyId,
                CompanyName: companyName,
                ReferenceId: referenceId,
                ReferenceName: referenceName,
                RegistrationNumber: registrationNumber,
                InvoiceAmount: totalForTicketInfos,
                ProjectId: projectId,
                PaymentInstructionBankId: paymentInstructionBankId
            }


            var AddedSingleTicketInfo = [], EditedSingleTicketInfo = [];

            $("#TicketInformationTbl tbody tr").each(function (index, item) {
                var id = $.trim($(item).find("td:eq(0)").text());
                if (id == 'NaN') {
                    id = 0;
                }
                clientName = $.trim($(item).find("td:eq(1)").text());
                airlineName = $(item).find("td:eq(2)").text();
                issueDate = $.trim($(item).find("td:eq(3)").text());
                if (issueDate != '') {
                    issueDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(issueDate, innBoarDateFormat);
                }
                flightDate = $.trim($(item).find("td:eq(4)").text());
                if (flightDate != '') {
                    flightDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(flightDate, innBoarDateFormat);
                }
                invoiceAmount = $.trim($(item).find("td:eq(5)").text());

                mobileNumber = $.trim($(item).find("td:eq(7)").text());
                email = $.trim($(item).find("td:eq(8)").text());
                address = $.trim($(item).find("td:eq(9)").text());
                ticketTypeId = $.trim($(item).find("td:eq(10)").text());
                ticketType = $.trim($(item).find("td:eq(11)").text());
                airlineId = $.trim($(item).find("td:eq(12)").text());
                returnDate = $.trim($(item).find("td:eq(13)").text());
                if (returnDate != '') {
                    returnDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(returnDate, innBoarDateFormat);
                }
                ticketNumber = $.trim($(item).find("td:eq(14)").text());
                pnrNumber = $.trim($(item).find("td:eq(15)").text());
                airlineAmount = $.trim($(item).find("td:eq(16)").text());
                routePath = $.trim($(item).find("td:eq(17)").text());
                remarks = $.trim($(item).find("td:eq(18)").text());
                ticketValue = $.trim($(item).find("td:eq(19)").text());

                airlineAmount = airlineAmount != "" ? parseFloat(airlineAmount) : 0.00;
                var isPreviousDataExists = $("#ContentPlaceHolder1_hfIsPreviousDataExists").val();
                AddedSingleTicketInfo.push({
                    Id: id,
                    AirlineName: airlineName,
                    IssueDate: issueDate,
                    FlightDate: flightDate,
                    InvoiceAmount: invoiceAmount,
                    ClientName: clientName,
                    MobileNumber: mobileNumber,
                    Email: email,
                    Address: address,
                    TicketTypeId: ticketTypeId,
                    TicketType: ticketType,
                    AirlineId: airlineId,
                    ReturnDate: returnDate,
                    TicketNumber: ticketNumber,
                    PnrNumber: pnrNumber,
                    TicketValue: ticketValue,
                    AirlineAmount: airlineAmount,
                    RoutePath: routePath,
                    Remarks: remarks,
                    IsPreviousDataExists: isPreviousDataExists
                });
            });

            var AddedPaymentInfo = [], EditedPaymentInfo = [];

            $("#PaymentInformationTbl tbody tr").each(function (index, item) {

                paymentModeName = $.trim($(item).find("td:eq(0)").text());
                bankName = $.trim($(item).find("td:eq(1)").text());
                receiveAmount = $.trim($(item).find("td:eq(2)").text());
                paymentModeId = $.trim($(item).find("td:eq(4)").text());
                currencyTypeId = $.trim($(item).find("td:eq(5)").text());
                currencyType = $.trim($(item).find("td:eq(6)").text());
                cardType = $.trim($(item).find("td:eq(7)").text());
                cardTypeId = $.trim($(item).find("td:eq(8)").text());
                cardNumber = $.trim($(item).find("td:eq(9)").text());
                bankId = $.trim($(item).find("td:eq(10)").text());
                chequeNumber = $.trim($(item).find("td:eq(11)").text());
                chequeDate = $.trim($(item).find("td:eq(12)").text());
                if (chequeDate != '') {
                    chequeDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(chequeDate, innBoarDateFormat);
                }

                AddedPaymentInfo.push({
                    PaymentMode: paymentModeName,
                    BankName: bankName,
                    ReceiveAmount: receiveAmount,
                    PaymentModeId: paymentModeId,
                    CurrencyTypeId: currencyTypeId,
                    CurrencyType: currencyType,
                    CardType: cardType,
                    CardTypeId: cardTypeId,
                    CardNumber: cardNumber,
                    BankId: bankId,
                    ChequeNumber: chequeNumber,
                    ChequeDate: chequeDate
                });

            });
            var totalForTicketInfos = $("#ContentPlaceHolder1_hftotalForTicketInfos").val();
            var totalForPaymentInfos = $("#ContentPlaceHolder1_hftotalForPaymentInfos").val();
            totalForTicketInfos = parseFloat(totalForTicketInfos);
            totalForPaymentInfos = parseFloat(totalForPaymentInfos);
            totalForTicketInfos = totalForTicketInfos.toFixed(2);
            totalForPaymentInfos = totalForPaymentInfos.toFixed(2);
            if (totalForTicketInfos != totalForPaymentInfos) {
                toastr.warning("Total Ticket price & Total Payment are not same.");
                $("#ContentPlaceHolder1_txtPMAmount").focus();
                return false;
            }
            
            var randomDocId = $("#ContentPlaceHolder1_RandomDocId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            CommonHelper.SpinnerOpen();
            PageMethods.SaveAirlineTicketInfo(AirTicketMasterInfo, AddedSingleTicketInfo, deletedAirlineInfoList, AddedPaymentInfo, deletedPaymentInfoList, parseInt(randomDocId), deletedDoc, OnSaveAirlineTicketInfoSucceeded, OnSaveAirlineTicketInfoFailed);

            return false;
        }
        function OnSaveAirlineTicketInfoSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
                $("#ContentPlaceHolder1_RandomDocId").val(result.Data);
                deletedPaymentInfoList = [];
                deletedAirlineInfoList = [];
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            CommonHelper.SpinnerClose();
            return false;
        }
        function OnSaveAirlineTicketInfoFailed(error) {
            toastr.error(error.get_message());
            CommonHelper.SpinnerClose();
        }

        deletedAirlineInfoList = [];
        function DeleteAirlineInfoItem(control) {
            if (!confirm("Do you want to delete item?")) { return false; }

            var tr = $(control).parent().parent();
            let airlineInfoId = $(tr).find("td").eq(0).html();
            deletedAirlineInfoList.push(parseInt(airlineInfoId, 10));
            $(tr).remove();
            var totalAmount = 0;
            $("#TicketInformationTbl tr").each(function () {
                var amount = $(this).find("td").eq(5).html();
                if (amount == undefined) {
                    amount = 0;
                }
                totalAmount = parseFloat(totalAmount) + parseFloat(amount);
            });
            totalAmount = totalAmount.toFixed(2);
            $("#ContentPlaceHolder1_txtTotalInvoiceAmount").val(totalAmount);
            $("#ContentPlaceHolder1_hftotalForTicketInfos").val(totalAmount);
        }

        function EditAirlineInfoItem(control) {
            if (!confirm("Do You Want To Edit Item?")) {
                return false;
            }
            $("#btnAdd").val("Update");
            $("#ContentPlaceHolder1_hfIsAirlineInfoEdit").val(1);
            var tr = $(control).parent().parent();
            var clickedId = $(tr).find("td").eq(0).html();
            $("#ContentPlaceHolder1_hfClickedAirlineId").val(clickedId);
            var clientName = $(tr).find("td").eq(1).html();
            var airlineName = $(tr).find("td").eq(2).html();
            var issueDate = $(tr).find("td").eq(3).html();
            var flightDate = $(tr).find("td").eq(4).html();
            var invoiceAmount = $(tr).find("td").eq(5).html();
            var mobileNumber = $(tr).find("td").eq(7).html();
            var email = $(tr).find("td").eq(8).html();
            var address = $(tr).find("td").eq(9).html();
            var ticketTypeId = $(tr).find("td").eq(10).html();
            var ticketType = $.trim((tr).find("td").eq(11).html());
            if (ticketType == "International") {
                ticketTypeId = 1;
            }
            else if (ticketType == "Domestic") {
                ticketTypeId = 2;
            }
            var airlineId = $(tr).find("td").eq(12).html();
            var returnDate = $(tr).find("td").eq(13).html();
            var ticketNumber = $(tr).find("td").eq(14).html();
            var pnrNumber = $(tr).find("td").eq(15).html();
            var airlineAmount = $(tr).find("td").eq(16).html();
            var routePath = $(tr).find("td").eq(17).html();
            var remarks = $(tr).find("td").eq(18).html();
            var ticketValue = $(tr).find("td").eq(19).html();
            $("#ContentPlaceHolder1_txtClientName").val(clientName);
            $("#ContentPlaceHolder1_txtMobileNo").val(mobileNumber);
            $("#ContentPlaceHolder1_txtEmail").val(email);
            $("#ContentPlaceHolder1_txtAddress").val(address);
            $("#ContentPlaceHolder1_txtIssueDate").val(issueDate);
            $("#ContentPlaceHolder1_ddlTicketType").val(ticketTypeId).trigger('change');
            $("#ContentPlaceHolder1_ddlAirlineName").val(airlineId).trigger('change');
            $("#ContentPlaceHolder1_txtFlightDate").val(flightDate);
            $("#ContentPlaceHolder1_txtReturnDate").val(returnDate);
            $("#ContentPlaceHolder1_txtTicketNumber").val(ticketNumber);
            $("#ContentPlaceHolder1_txtPNR").val(pnrNumber);
            $("#ContentPlaceHolder1_txtTicketValue").val(ticketValue);
            $("#ContentPlaceHolder1_txtAirlineAmount").val(airlineAmount);
            $("#ContentPlaceHolder1_txtInvoiceAmount").val(invoiceAmount);
            $("#ContentPlaceHolder1_txtRoute").val(routePath);
            $("#ContentPlaceHolder1_txtRemarks").val(remarks);
        }

        function EditPaymentInfoItem(paymentModeId, paymentMode, bankId, bankName, receiveAmount, currencyTypeId, currencyType, cardTypeId, cardType, cardNumber, chequeNumber, chequeDate) {
            if (!confirm("Do you want to edit item?")) {
                return false;
            }
            $('#ContentPlaceHolder1_ddlPayMode').attr('disabled', true);
            $("#btnAddDetailGuestPayment").val("Update");
            $("#ContentPlaceHolder1_hfEditPayment").val(1);
            $("#ContentPlaceHolder1_ddlPayMode").val(paymentModeId).trigger('change');
            $("#ContentPlaceHolder1_ddlCurrency").val(currencyTypeId).trigger('change');
            $("#ContentPlaceHolder1_txtReceiveLeadgerAmount").val(receiveAmount);
            $("#ContentPlaceHolder1_ddlCardType").val(cardTypeId).trigger('change');
            $("#ContentPlaceHolder1_txtCardNumber").val(cardNumber);
            $("#ContentPlaceHolder1_txtbankName").val(bankName);
            $("#ContentPlaceHolder1_txtBankNameForMBanking").val(bankName);
            $("#ContentPlaceHolder1_txtBankNameForCheque").val(bankName);
            $("#ContentPlaceHolder1_hfbankId").val(bankId);
            $("#ContentPlaceHolder1_txtChecqueNumber").val(chequeNumber);
            $("#ContentPlaceHolder1_txtChequeDate").val(GetStringFromDateTime(chequeDate));
        }
        var deletedPaymentInfoList = [];
        function DeletePaymentInfoItem(control) {
            if (!confirm("Do you want to delete item?")) { return false; }

            var tr = $(control).parent().parent();
            let paymentModeId = $(tr).find("td").eq(4).html();
            deletedPaymentInfoList.push(parseInt(paymentModeId, 10));
            $(tr).remove();

            var totalAmount = 0;
            $("#PaymentInformationTbl tr").each(function () {
                var amount = $(this).find("td").eq(2).html();
                if (amount == undefined) {
                    amount = 0;
                }
                totalAmount = parseFloat(totalAmount) + parseFloat(amount);
            });
            totalAmount = totalAmount.toFixed(2);
            $("#ContentPlaceHolder1_txtTotalPaymentAmount").val(totalAmount);
            $("#ContentPlaceHolder1_hftotalForPaymentInfos").val(totalAmount);
        }

        function PerformClearAction() {
            $("#ContentPlaceHolder1_ddlTransactionType").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlProject").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlPaymentInstructionBank").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtCompany").val("");
            $("#ContentPlaceHolder1_txtReferenceForCompany").val("");
            $("#ContentPlaceHolder1_txtCompanyWalkInGuest").val("");
            $("#ContentPlaceHolder1_txtReferenceNameForWalkIn").val("");
            $("#ContentPlaceHolder1_txtRegistrationNumber").val("");
            $("#TicketInformationTbl tbody").html("");
            $("#PaymentInformationTbl tbody").html("");
            $("#ContentPlaceHolder1_hfCompanyId").val(0);
            $("#ContentPlaceHolder1_hfCompanySearchId").val(0);
            $("#ContentPlaceHolder1_hfReferenceIdForCompany").val(0);
            $("#ContentPlaceHolder1_hfReferenceIdForWalkIn").val(0);
            $("#ContentPlaceHolder1_hfbankId").val(0);
            $("#ContentPlaceHolder1_hfRegistrationNumber").val(0);
            $("#ContentPlaceHolder1_hfTicketMasterId").val(0);
            $("#ContentPlaceHolder1_hftotalForPaymentInfos").val(0);
            $("#ContentPlaceHolder1_hftotalForTicketInfos").val(0);
            $("#DocumentInfo").html("");
            $("#ContentPlaceHolder1_txtTotalInvoiceAmount").val("");
            $("#ContentPlaceHolder1_txtTotalPaymentAmount").val("");
            $("#ContentPlaceHolder1_hfLastAirlineTicketId").val(0);
            $("#ContentPlaceHolder1_hfIsPreviousDataExists").val(0);
            $("#btnSave").val("Save");
            ClearAfterPaymentInfoAdded();
        }
        function PerformClearActionWithConfirmation() {

            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            PerformClearAction();
        }

        function SearchTicketInformation(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#TicketInformationGrid tbody tr").length;
            var fromDate = null, toDate = null, invoiceNumber = "", companyName = "", referenceName = "", ticketNumber = "", pnrNumber = "";
            fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            toDate = $("#ContentPlaceHolder1_txtToDate").val();
            invoiceNumber = $("#ContentPlaceHolder1_txtInvoiceNumber").val();
            companyName = $("#ContentPlaceHolder1_txtCompanyName").val();
            referenceName = $("#ContentPlaceHolder1_txtRefName").val();
            ticketNumber = $("#ContentPlaceHolder1_txtTicketNumberSearch").val();
            pnrNumber = $("#ContentPlaceHolder1_txtPNRSearch").val();

            if (fromDate != "")
                fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');

            if (toDate != "")
                toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            $("#GridPagingContainer ul").html("");
            $("#TicketInformationGrid tbody").html("");

            if (pageNumber < 0)
                pageNumber = 1;

            PageMethods.SearchTicketInformation(fromDate, toDate, invoiceNumber, companyName, referenceName, ticketNumber, pnrNumber,
                gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchTicketInformationSucceed, OnSearchTicketInformationFailed);

            return false;
        }
        function PerformBillPreviewAction(billId) {
            var url = "";
            var popup_window = "Print Preview";
            url = "/AirTicketing/Reports/frmATBillInfo.aspx?billID=" + billId;
            window.open(url, popup_window, "width=750,height=680,left=300,top=50,resizable=yes");
        }
        function OnSearchTicketInformationSucceed(result) {
            var tr = "";
            $.each(result.GridData, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style='width:10%;'>" + gridObject.BillNumber + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.TransactionType + "</td>";
                tr += "<td style='width:30%;'>" + gridObject.CompanyName + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.InvoiceAmount + "</td>";

                tr += "<td style='width:10%;'>" + gridObject.Status + "</td>";

                tr += "<td style=\"text-align: center; width:10%; cursor:pointer;\">";

                tr += '&nbsp;&nbsp;<a href="javascript:void();" onclick= "javascript:return ShowDocuments(' + gridObject.TicketId + ');" title="Documents"><img style="width:16px;height:16px;" alt="Documents" src="../Images/document.png" /></a>';

                if (gridObject.IsCanEdit && IsCanEdit) {
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return TicketInfoEditWithConfirmation(" + gridObject.TicketId + ")\" alt='Edit'  title='Edit' border='0' />";
                }

                if (gridObject.IsCanDelete && IsCanDelete) {
                    tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return TicketInformationDelete(" + gridObject.TicketId + ")\" alt='Delete'  title='Delete' border='0' />";
                }

                if (gridObject.IsCanChecked && IsCanSave) {
                    tr += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return TicketInformationCheckWithConfirmation(" + gridObject.TicketId + ")\" alt='Check'  title='Check' border='0' />";
                }
                if (gridObject.IsCanApproved && IsCanSave) {
                    tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return TicketInformationApprovalWithConfirmation(" + gridObject.TicketId + ")\" alt='Approve'  title='Approve' border='0' />";
                }

                tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return PerformBillPreviewAction(" + gridObject.TicketId + ")\" alt='Invoice' title='Invoice' border='0' />";

                //tr += "&nbsp;&nbsp;<img src='../Images/note.png'  onClick= \"javascript:return ShowDealDocuments('" + gridObject.ReceivedId + "')\" alt='Invoice' title='Receive Order Info' border='0' />";
                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.TicketId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.CostCenterId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.TransactionId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.ReferenceId + "</td>";

                tr += "</tr>";

                $("#TicketInformationGrid tbody").append(tr);

                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnSearchTicketInformationFailed() {

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
                title: "Airline Ticket Documents",
                show: 'slide'
            });

            return false;
        }

        function OnLoadDocumentByIdFailed(error) {
            toastr.error(error.get_message());
        }

        function TicketInfoEdit(TicketId) {
            $("#ContentPlaceHolder1_hfTicketMasterId").val("0");
            PageMethods.TicketInfoEdit(TicketId, OnTicketInfoEditSucceed, OnTicketInfoEditFailed);
            return false;
        }
        function TicketInfoEditWithConfirmation(TicketId) {
            if (!confirm("Do you Want To Edit?")) {
                return false;
            }
            TicketInfoEdit(TicketId);
        }
        function OnTicketInfoEditSucceed(result) {
            if (IsCanEdit) {
                $('#btnSave').show();
            } else {
                $('#btnSave').hide();
            }
            if (result.length > 0) {
                $("#ContentPlaceHolder1_hfIsPreviousDataExists").val(1);
            }
            LoadLastAirlineTicketId();
            $("#btnSave").val("Update");
            $("#ContentPlaceHolder1_hfTicketMasterId").val(result.ATMasterInfo.TicketId);
            $("#TicketInformationTbl tbody").html("");
            $("#PaymentInformationTbl tbody").html("");
            AddedSerialzableProduct = new Array();
            DeletedSerialzableProduct = new Array();
            AddedSerialCount = 0;

            $("#ContentPlaceHolder1_ddlTransactionType").val(result.ATMasterInfo.TransactionType);
            $("#ContentPlaceHolder1_ddlProject").val(result.ATMasterInfo.ProjectId).trigger('change');
            $("#ContentPlaceHolder1_ddlPaymentInstructionBank").val(result.ATMasterInfo.PaymentInstructionBankId).trigger('change');
            if (result.ATMasterInfo.TransactionType == "CorporateCompany") {
                $("#ContentPlaceHolder1_txtCompany").val(result.ATMasterInfo.CompanyName);
                $("#ContentPlaceHolder1_txtReferenceForCompany").val(result.ATMasterInfo.ReferenceName);
                $("#ReferenceForCorporateCompany").show();
                $("#ReferenceForWalkIn").hide();
                $("#ReferenceForRoomGuest").hide();

            }
            else if (result.ATMasterInfo.TransactionType == "WalkInCustomer") {
                $("#ContentPlaceHolder1_txtCompanyWalkInGuest").val(result.ATMasterInfo.CompanyName);
                $("#ContentPlaceHolder1_txtReferenceNameForWalkIn").val(result.ATMasterInfo.ReferenceName);
                $("#ReferenceForWalkIn").show();
                $("#ReferenceForCorporateCompany").hide();
                $("#ReferenceForRoomGuest").hide();
            }
            else if (result.ATMasterInfo.TransactionType == "RoomGuest") {
                $("#ContentPlaceHolder1_txtRegistrationNumber").val(result.ATMasterInfo.RegistrationNumber);
                $("#ReferenceForRoomGuest").show();
                $("#ReferenceForCorporateCompany").hide();
                $("#ReferenceForWalkIn").hide();
            }

            SingleTicketInformationEdit(result);

            PaymentMethodInformationEdit(result.ATPaymentInfo);
            ShowUploadedDocument($("#ContentPlaceHolder1_RandomDocId").val());
        }
        function OnTicketInfoEditFailed() { }
        function LoadLastAirlineTicketId() {
            PageMethods.GetLastAirlineTicketId(OnGetLastAirlineTicketIdSucceeded, OnGetLastAirlineTicketIdFailed);
            return false;
        }
        function OnGetLastAirlineTicketIdSucceeded(result) {
            $("#ContentPlaceHolder1_hfLastAirlineTicketId").val(result.LastId);
        }
        function OnGetLastAirlineTicketIdFailed(error) {
            alert(error.get_message());
        }
        function PaymentMethodInformationEdit(result) {
            $.each(result, function (count, obj) {

                var tr = "";

                tr += "<tr>";
                tr += "<td style='width:35%;'>" + obj.PaymentMode + "</td>";
                tr += "<td style='width:25%;'>" + obj.BankName + "</td>";
                tr += "<td style='width:25%;'>" + obj.ReceiveAmount + "</td>";
                tr += "<td style=\"width:15%;\">";
                tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'DeletePaymentInfoItem(this)' ><img alt='Delete' src='../Images/delete.png' title='Delete' /></a>";
                tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return EditPaymentInfoItem('" + obj.PaymentModeId + "','" + obj.PaymentMode + "','" + obj.BankId + "','" + obj.BankName + "','" + obj.ReceiveAmount + "','" + obj.CurrencyTypeId + "','" + obj.CurrencyType + "','" + obj.CardTypeId + "','" + obj.CardType + "','" + obj.CardNumber + "','" + obj.ChequeNumber + "','" + obj.ChequeDate + "')\" alt='Edit'  title='Edit' border='0' />";
                tr += "</td>";

                tr += "<td style='display:none;'>" + obj.PaymentModeId + "</td>";
                tr += "<td style='display:none;'>" + obj.CurrencyTypeId + "</td>";
                tr += "<td style='display:none;'>" + obj.CurrencyType + "</td>";
                tr += "<td style='display:none;'>" + obj.CardType + "</td>";
                tr += "<td style='display:none;'>" + obj.CardTypeId + "</td>";
                tr += "<td style='display:none;'>" + obj.CardNumber + "</td>";
                tr += "<td style='display:none;'>" + obj.BankId + "</td>";
                tr += "<td style='display:none;'>" + obj.ChequeNumber + "</td>";
                tr += "<td style='display:none;'>" + GetStringFromDateTime(obj.ChequeDate) + "</td>";

                tr += "</tr>";

                $("#PaymentInformationTbl tbody").prepend(tr);
                var totalAmount = 0;
                $("#PaymentInformationTbl tr").each(function () {
                    var amount = $(this).find("td").eq(2).html();
                    if (amount == undefined) {
                        amount = 0;
                    }
                    totalAmount = parseFloat(totalAmount) + parseFloat(amount);
                });
                totalAmount = totalAmount.toFixed(2);
                $("#ContentPlaceHolder1_txtTotalPaymentAmount").val(totalAmount);
                $("#ContentPlaceHolder1_hftotalForPaymentInfos").val(totalAmount);

                tr = "";
            });
        }

        function SingleTicketInformationEdit(result) {
            $.each(result.ATInformationDetails, function (count, obj) {
                var tr = "";

                tr += "<tr>";
                tr += "<td style='display:none;'>" + obj.Id + "</td>";
                tr += "<td style='width:20%;'>" + obj.ClientName + "</td>";
                tr += "<td style='width:20%;'>" + obj.AirlineName + "</td>";
                tr += "<td style='width:15%;'>" + GetStringFromDateTime(obj.IssueDate) + "</td>";
                tr += "<td style='width:15%;'>" + GetStringFromDateTime(obj.FlightDate) + "</td>";
                tr += "<td style='width:15%;'>" + obj.InvoiceAmount + "</td>";
                tr += "<td style='width:15%;'>" +
                    "<a href='javascript:void()' onclick= 'DeleteAirlineInfoItem(this)' ><img alt='Delete' src='../Images/delete.png' title='Delete' /></a>";
                tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return EditAirlineInfoItem(this)\" alt='Edit'  title='Edit' border='0' />";
                tr += "</td>";

                tr += "<td style='display:none;'>" + obj.MobileNumber + "</td>";
                tr += "<td style='display:none;'>" + obj.Email + "</td>";
                tr += "<td style='display:none;'>" + obj.Address + "</td>";
                tr += "<td style='display:none;'>" + obj.TicketTypeId + "</td>";
                tr += "<td style='display:none;'>" + obj.TicketType + "</td>";
                tr += "<td style='display:none;'>" + obj.AirlineId + "</td>";
                tr += "<td style='display:none;'>" + GetStringFromDateTime(obj.ReturnDate) + "</td>";
                tr += "<td style='display:none;'>" + obj.TicketNumber + "</td>";
                tr += "<td style='display:none;'>" + obj.PnrNumber + "</td>";
                tr += "<td style='display:none;'>" + obj.AirlineAmount + "</td>";
                tr += "<td style='display:none;'>" + obj.RoutePath + "</td>";
                tr += "<td style='display:none;'>" + obj.Remarks + "</td>";
                tr += "<td style='display:none;'>" + obj.TicketValue + "</td>";

                tr += "</tr>";

                $("#TicketInformationTbl tbody").append(tr);

                var totalAmount = 0;
                $("#TicketInformationTbl tr").each(function () {
                    var amount = $(this).find("td").eq(5).html();
                    if (amount == undefined) {
                        amount = 0;
                    }
                    totalAmount = parseFloat(totalAmount) + parseFloat(amount);
                });
                totalAmount = totalAmount.toFixed(2);
                $("#ContentPlaceHolder1_txtTotalInvoiceAmount").val(totalAmount);
                $("#ContentPlaceHolder1_hftotalForTicketInfos").val(totalAmount);

                tr = "";
            });

            $("#myTabs").tabs({ active: 0 });
        }

        function TicketInformationDelete(TicketId) {

            if (!confirm("Do you Want To Delete?")) {
                return false;
            }

            PageMethods.TicketInformationDelete(TicketId, OnTicketInformationDeleteSucceed, OnTicketInformationDeleteFailed);
        }

        function OnTicketInformationDeleteSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchTicketInformation(1, 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            return false;
        }
        function OnTicketInformationDeleteFailed() {
            toastr.error(error.get_message());
        }

        function TicketInformationCheck(ticketId) {
            PageMethods.TicketInformationCheck(ticketId, OnTicketInformationCheckSucceed, OnTicketInformationCheckFailed);
        }
        function OnTicketInformationCheckSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchTicketInformation(1, 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            return false;
        }
        function OnTicketInformationCheckFailed() {
            toastr.error(error.get_message());
        }

        function TicketInformationApproval(TicketId) {

            PageMethods.TicketInformationApproval(TicketId, OnApprovalSucceed, OnApprovalFailed);
        }
        function TicketInformationApprovalWithConfirmation(TicketId) {

            if (!confirm("Do you Want To Approve?")) {
                return false;
            }
            TicketInformationApproval(TicketId);
        }
        function TicketInformationCheckWithConfirmation(TicketId) {
            if (!confirm("Do you Want To Check?")) {
                return false;
            }
            TicketInformationCheck(TicketId);

        }
        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchTicketInformation(1, 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            return false;
        }
        function OnApprovalFailed() {
            toastr.error(error.get_message());
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchTicketInformation(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }

        function GoToAirlineTicketInfoPage() {
            window.location = "/AirTicketing/frmAirlineTicketInfo.aspx";
            return false;
        }
        function TicketUnapprovalPanel(result) {
            $("#<%=txtApprovalTransactionNo.ClientID%>").val("");

            $("#AdminApprovalDiv").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Ticket Unapproval Information",
                show: 'slide'
            });

            return false;
        }

        function CloseDialogTicketUnapprovalPanel() {
            $("#AdminApprovalDiv").dialog('close');
            return false;
        }
        function AdminApprovalProcess() {
            var r = confirm("Do you want to continue Voucher Unapproval?");
            if (r == true) {
                var transactionNo = $("#<%=txtApprovalTransactionNo.ClientID%>").val();
                
                var status = "Pending";

                if (transactionNo == '') {
                    toastr.warning("Please Enter Ticket Number.");
                    $("#<%=txtApprovalTransactionNo.ClientID%>").focus();
                    return false;
                }

                PageMethods.AdminApprovalStatus(transactionNo, status, OnAdminApprovalProcessSucceed, OnAdminApprovalProcessFailed);
            }

            return false;
        }

        function OnAdminApprovalProcessSucceed(result) {
            toastr.success("Ticket Unapprove Successfull.");
            return false;
        }

        function OnAdminApprovalProcessFailed(error) {
            toastr.error(error.get_message());
            return false;
        }
        function LoadDocUploader() {
            var randomId = +$("#ContentPlaceHolder1_RandomDocId").val();
            var path = "/AirTicketing/Image/";
            var category = "AirlineTicketInfo";
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
        function UploadComplete() {
            var randomId = $("#ContentPlaceHolder1_RandomDocId").val();
            ShowUploadedDocument(randomId);
        }
        function ShowUploadedDocument(randomId) {
            var id = $("#ContentPlaceHolder1_hfTicketMasterId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            PageMethods.GetUploadedDocByWebMethod(randomId, id, deletedDoc, OnGetUploadedDocByWebMethodSucceeded, OnGetUploadedDocByWebMethodFailed);
            return false;
        }
        function OnGetUploadedDocByWebMethodSucceeded(result) {
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            DocTable = "";

            DocTable += "<table id='DocTableList' style='width:100%' class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            DocTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    DocTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    DocTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }
                DocTable += "<td align='left' style='width: 50%;cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + "','" + result[row].Name + "');\">" + result[row].Name + "</td>";

                if (result[row].Path != "") {
                    imagePath = "<img src='" + result[row].Path + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                DocTable += "<td align='left' style='width: 30%'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + "','" + result[row].Name + "');\">" + imagePath + "</td>";

                DocTable += "<td align='left' style='width: 20%'>";
                DocTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteDoc('" + result[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                DocTable += "</td>";
                DocTable += "</tr>";
            }
            DocTable += "</table>";

            docc = DocTable;

            $("#DocumentInfo").html(DocTable);

            return false;
        }
        function DeleteDoc(docId, rowIndex) {
            var deletedDoc = $("#<%=hfDeletedDoc.ClientID %>").val();

            if (deletedDoc != "")
                deletedDoc += "," + docId;
            else
                deletedDoc = docId;

            $("#<%=hfDeletedDoc.ClientID %>").val(deletedDoc);

            $("#trdoc" + rowIndex).remove();
        }
        function OnGetUploadedDocByWebMethodFailed(error) {
            alert(error.get_message());
        }
        function ShowDocument(path, name) {
            var iframeid = 'fileIframe';
            document.getElementById(iframeid).src = path;
            $("#ShowDocumentDiv").dialog({
                autoOpen: true,
                modal: true,
                width: "82%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Document - " + name,
                show: 'slide'
            });
            return false;
        }
    </script>
    <asp:HiddenField ID="hfIsPreviousDataExists" runat="server" Value="0" />
    <asp:HiddenField ID="hfLastAirlineTicketId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsAirlineInfoEdit" runat="server" Value="0" />
    <asp:HiddenField ID="hfClickedAirlineId" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />
    <asp:HiddenField ID="hfPaymentId" runat="server" Value="0" />
    <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanySearchId" runat="server" Value="0" />
    <asp:HiddenField ID="hfReferenceIdForCompany" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfReferenceIdForWalkIn" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfbankId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfRegistrationNumber" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfTicketMasterId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfEditPayment" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfStopAddingExistingPayment" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hftotalForPaymentInfos" runat="server" Value="0" />
    <asp:HiddenField ID="hftotalForTicketInfos" runat="server" Value="0" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <div id="voucherDocuments" style="display: none;">
        <div id="imageDiv"></div>
    </div>
    <div id="ShowDocumentDiv" style="display: none;">
        <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="AdminApprovalDiv" class="panel panel-default" style="display: none;">
        <div class="panel-body">
            <div class="form-horizontal">                
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Ticket Number</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtApprovalTransactionNo" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="Button2" runat="server" Text="Unapprove" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return AdminApprovalProcess();" />
                    <asp:Button ID="Button3" runat="server" Text="Close" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return CloseDialogTicketUnapprovalPanel();" />
                </div>
            </div>
        </div>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Airline Ticket Information</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Airline Ticket</a></li>
        </ul>
        <div id="tab-1">
            <div class="panel panel-default">
                <div class="panel-heading">New Airline Ticket Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblTransactionType" runat="server" class="control-label required-field" Text="Transaction Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlTransactionType" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Corporate Company" Value="CorporateCompany"></asp:ListItem>
                                    <asp:ListItem Text="Walk-In Customer" Value="WalkInCustomer"></asp:ListItem>
                                    <%--<asp:ListItem Text="Room Guest" Value="RoomGuest"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblProject" runat="server" class="control-label required-field" Text="Project"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="ReferenceForRoomGuest" style="display: none">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblRegistrationNumber" runat="server" class="control-label required-field" Text="Registration Number"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtRegistrationNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div id="ReferenceForCorporateCompany" style="display: none">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblCompany" runat="server" class="control-label required-field" Text="Company"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtCompany" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblReferenceForCompany" runat="server" class="control-label required-field" Text="Reference Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtReferenceForCompany" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div id="ReferenceForWalkIn" style="display: none">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblCompanyWalkInGuest" runat="server" class="control-label required-field" Text="Company"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtCompanyWalkInGuest" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblReferenceName" runat="server" class="control-label required-field" Text="Reference Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtReferenceNameForWalkIn" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">Airline Ticket Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblClientName" runat="server" class="control-label required-field" Text="Client Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtClientName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblMobileNo" runat="server" class="control-label" Text="Mobile Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblEmail" runat="server" class="control-label" Text="Email"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAddress" runat="server" class="control-label" Text="Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblIssueDate" runat="server" class="control-label required-field" Text="Issue Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtIssueDate" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblTicketType" runat="server" class="control-label required-field" Text="Ticket Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlTicketType" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="International" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Domestic" Value="2"></asp:ListItem>
                                    <%--<asp:ListItem Text="Visa" Value="3"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAirlineName" runat="server" class="control-label required-field" Text="Airline Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlAirlineName" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFlightDate" runat="server" class="control-label required-field" Text="Flight Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFlightDate" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblReturnDate" runat="server" class="control-label" Text="Return Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReturnDate" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblTicketNumber" runat="server" class="control-label required-field" Text="Ticket Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTicketNumber" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPNR" runat="server" class="control-label required-field" Text="PNR"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPNR" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblTicketValue" runat="server" class="control-label required-field" Text="Ticket Value"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTicketValue" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblAirlineAmount" runat="server" class="control-label" Text="Airline Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAirlineAmount" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblInvoiceAmount" runat="server" class="control-label required-field" Text="Invoice Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtInvoiceAmount" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRoute" runat="server" class="control-label required-field" Text="Route"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRoute" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Description"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" style="padding-top: 10px;">
                            <div class="col-md-12">
                                <input id="btnAdd" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" onclick="AddItemForAirTicket()" />
                                <input id="btnCancelOrder" type="button" value="Cancel" onclick="ClearAfterAirlineInfoAdded()"
                                    class="TransactionalButton btn btn-primary btn-sm" />
                            </div>
                        </div>
                        <div id="Ticket Information" style="overflow-y: scroll;">
                            <table id="TicketInformationTbl" class="table table-bordered table-condensed table-hover">
                                <thead>
                                    <tr>
                                        <th style="width: 20%;">Client Name</th>
                                        <th style="width: 20%;">Airline Name</th>
                                        <th style="width: 15%;">Issue Date</th>
                                        <th style="width: 15%;">Flight Date</th>
                                        <th style="width: 15%;">Invoice Amount</th>
                                        <th style="width: 15%;">Action</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                                <tfoot></tfoot>
                            </table>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblTotalInvoiceAmount" runat="server" class="control-label" Text="Total Invoice Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtTotalInvoiceAmount" ReadOnly="true" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="PaymentDetailsInformation" class="childDivSection">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                Guest Payment Information
                            </div>
                            <div class="panel-body childDivSectionDivBlockBody">
                                <div class="form-horizontal">

                                    <%--<div class="form-group" id="GrandTotalPaymentDetailsDiv">
                                        <div class="col-md-2">
                                            <asp:Label ID="Label7" runat="server" class="control-label required-field" Text="Grand Total"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtGrandTotalInfo" runat="server" CssClass="form-control"
                                                Enabled="false"> </asp:TextBox>
                                        </div>
                                    </div>--%>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblPayMode" runat="server" class="control-label required-field" Text="Payment Mode"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlPayMode" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                                <asp:ListItem Value="1">Cash</asp:ListItem>
                                                <asp:ListItem Value="2">Card</asp:ListItem>
                                                <asp:ListItem Value="3">M-Banking</asp:ListItem>
                                                <asp:ListItem Value="4">Cheque</asp:ListItem>
                                                <asp:ListItem Value="5">Company</asp:ListItem>
                                                <asp:ListItem Value="6">Guest Room</asp:ListItem>
                                                <asp:ListItem Value="7">Refund</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Label ID="lblCurrencyType" runat="server" class="control-label required-field"
                                                Text="Currency Type"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlCurrency" CssClass="form-control" runat="server">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblDisplayConvertionRate" runat="server" Text=""></asp:Label>
                                            <asp:HiddenField ID="ddlCurrencyHiddenField" runat="server"></asp:HiddenField>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblReceiveLeadgerAmount" runat="server" class="control-label required-field"
                                                Text="Receive Amount"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtReceiveLeadgerAmount" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                                        </div>
                                        <div id="ConversionRateDivInformation" style="display: none;">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblConversionRate" runat="server" class="control-label required-field"
                                                    Text="Conversion Rate"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtConversionRate" runat="server" CssClass="form-control" Text=""></asp:TextBox>
                                                <asp:HiddenField ID="txtConversionRateHiddenField" runat="server"></asp:HiddenField>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group" style="display: none;">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblPaymentAccountHead" runat="server" class="control-label" Text="Account Head"></asp:Label>
                                        </div>
                                        <div class="col-md-10">
                                            <div id="CashPaymentAccountHeadDiv">
                                                <asp:DropDownList ID="ddlCashReceiveAccountsInfo" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                            <div id="BankPaymentAccountHeadDiv" style="display: none;">
                                                <asp:DropDownList ID="ddlCardReceiveAccountsInfo" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                            <div id="CompanyPaymentAccountHeadDiv" style="display: none;">
                                                <asp:DropDownList ID="ddlCompanyPaymentAccountHead" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                            <div id="MBankingReceiveAccountsInfo" style="display: none;">
                                                <asp:DropDownList ID="ddlMBankingReceiveAccountsInfo" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="PaidByOtherRoomDiv" style="display: none">
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="Label6" runat="server" class="control-label required-field" Text="Room Number"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlPaidByRegistrationId" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="ChecquePaymentAccountHeadDiv" style="display: none;">
                                        <div class="form-group" style="display: none;">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblChecquePaymentAccountHeadId" runat="server" class="control-label required-field"
                                                    Text="Company Name"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlChecquePaymentAccountHeadId" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblChecqueNumber" runat="server" class="control-label required-field"
                                                    Text="Cheque Number"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtChecqueNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="lblChequeDate" runat="server" class="control-label required-field" Text="Cheque Date"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtChequeDate" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblBankNameForCheque" runat="server" class="control-label required-field" Text="Bank Name"></asp:Label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:TextBox ID="txtBankNameForCheque" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="CardPaymentAccountHeadDiv" style="display: none;">
                                        <div class="form-group" style="display: none;">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblCardPaymentAccountHeadId" runat="server" class="control-label"
                                                    Text="Accounts Posting Head"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlCardPaymentAccountHeadId" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Card Type"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlCardType" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                                    <asp:ListItem Value="1">American Express</asp:ListItem>
                                                    <asp:ListItem Value="2">Master Card</asp:ListItem>
                                                    <asp:ListItem Value="3">Visa Card</asp:ListItem>
                                                    <asp:ListItem Value="4">Discover Card</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="lblCardNumber" runat="server" class="control-label" Text="Card Number"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtCardNumber" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblBankId" runat="server" class="control-label required-field" Text="Bank Name"></asp:Label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:TextBox ID="txtbankName" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div style="display: none;">
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                    <asp:Label ID="Label4" runat="server" class="control-label" Text="Expiry Date"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtExpireDate" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Label ID="Label5" runat="server" class="control-label" Text="Card Holder Name"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtCardHolderName" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="MBankingPaymentAccountHeadDiv" style="display: none;">
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblBankNameForMBanking" runat="server" class="control-label required-field" Text="Bank Name"></asp:Label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:TextBox ID="txtBankNameForMBanking" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="RefundDiv">
                                        <div class="form-group" style="display: none;">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblRefundAccountHead" runat="server" class="control-label" Text="Account Head"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlRefundAccountHead" CssClass="form-control" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group" style="padding-left: 10px;">
                                        <%--Right Left--%>
                                        <input id="btnAddDetailGuestPayment" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" onclick="AddItemForPaymentInfo()" />
                                        <input id="btnCancelPayment" type="button" value="Cancel" onclick="ClearAfterPaymentInfoAdded()"
                                            class="TransactionalButton btn btn-primary btn-sm" />

                                        <asp:Label ID="lblHiddenIdDetailGuestPayment" runat="server" Text='' Visible="False"></asp:Label>
                                    </div>
                                    <div id="PaymentInformation" style="overflow-y: scroll;">
                                        <table id="PaymentInformationTbl" class="table table-bordered table-condensed table-hover">
                                            <thead>
                                                <tr>
                                                    <th style="width: 35%;">Payment Mode</th>
                                                    <th style="width: 25%;">Payment Head</th>
                                                    <th style="width: 25%;">Payment Amount</th>
                                                    <th style="width: 15%;">Action</th>
                                                </tr>
                                            </thead>
                                            <tbody></tbody>
                                            <tfoot></tfoot>
                                        </table>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblTotalPaymentAmount" runat="server" class="control-label" Text="Total Payment Amount"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtTotalPaymentAmount" ReadOnly="true" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="GuestPaymentDetailGrid" class="childDivSection">
                                    </div>
                                    <div id="TotalPaid" class="totalAmout">
                                    </div>
                                    <div id="dueTotal" class="totalAmout">
                                    </div>
                                </div>
                            </div>
                            <div>
                                <asp:Label ID="AlartMessege" runat="server" Style="color: Red;" Text='Grand Total and Guest Payment Amount is not Equal.'
                                    CssClass="totalAmout" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div id="PaymentInstructionInformation" class="childDivSection">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                Payment Instruction Information
                            </div>
                            <div class="panel-body childDivSectionDivBlockBody">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblPaymentInstructionBank" runat="server" class="control-label" Text="Bank Name"></asp:Label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:DropDownList ID="ddlPaymentInstructionBank" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="AttachmentDiv" class="childDivSection">
                        <div class="panel panel-default">
                            <div class="panel-body childDivSectionDivBlockBody">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <label class="control-label">Attachment</label>
                                        </div>
                                        <div class="col-md-4">
                                            <input id="btnImageUp" type="button" onclick="javascript: return LoadDocUploader();"
                                                class="TransactionalButton btn btn-primary btn-sm" value="Documents..." />
                                        </div>
                                    </div>
                                    <div id="DocumentInfo">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="DocumentDialouge" style="display: none;">
                        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
                            clientidmode="static" scrolling="yes"></iframe>
                    </div>
                    <div class="form-group" style="padding-top: 10px;">
                        <div class="col-md-12">
                            <input id="btnSave" type="button" value="Save" onclick="ValidationBeforeSave()"
                                class="TransactionalButton btn btn-primary btn-sm" />
                            <input id="btnCancelTicket" type="button" value="Cancel" onclick="PerformClearActionWithConfirmation()"
                                class="TransactionalButton btn btn-primary btn-sm" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Airline Ticket Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="Date"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtFromDate" Placeholder="From" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtToDate" Placeholder="To" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblInvoiceNumber" runat="server" class="control-label" Text="Invoice No."></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtInvoiceNumber" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCompanyName" runat="server" class="control-label" Text="Company Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtCompanyName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRefName" runat="server" class="control-label" Text="Reference Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRefName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblTicketNumberSearch" runat="server" class="control-label" Text="Ticket Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTicketNumberSearch" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPNRSearch" runat="server" class="control-label" Text="PNR"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPNRSearch" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-large" value="Search" onclick="SearchTicketInformation(1, 1)" />
                                <input type="button" id="btnSearchCancel" class="TransactionalButton btn btn-primary btn-large" value="Clear" onclick="ClearSearch()" />
                                <asp:Button ID="btnAdminApproval" runat="server" Text="Ticket Unapproval" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return TicketUnapprovalPanel();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="Div1" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <table id="TicketInformationGrid" class="table table-bordered table-condensed table-responsive" width="100%">
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <th style="width: 10%;">Invoice No.
                                </th>
                                <th style="width: 20%;">Transaction Type
                                </th>
                                <th style="width: 30%;">Transaction For
                                </th>
                                <th style="width: 20%;">Invoice Amount
                                </th>
                                <th style="width: 10%;">Status
                                </th>
                                <th style="width: 10%;">Action
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
    </div>
</asp:Content>
