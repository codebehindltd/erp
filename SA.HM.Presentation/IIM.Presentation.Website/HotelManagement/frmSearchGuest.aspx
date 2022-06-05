<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmSearchGuest.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmSearchGuest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            $("#ExtraSearch").hide();
            $("#SearchPanel").hide();
            $("#GuestDetaails").hide();
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Guest Search</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            CommonHelper.AutoSearchClientDataSource("txtBankId", "ContentPlaceHolder1_ddlBankId", "ContentPlaceHolder1_ddlBankId");

            var txtSrcDateOfBirth = '<%=txtSrcDateOfBirth.ClientID%>'
            var txtSrcFromDate = '<%=txtSrcFromDate.ClientID%>'
            var txtSrcToDate = '<%=txtSrcToDate.ClientID%>'
            $('#imgCollapse').click(function () {
                var imageSrc = $('#imgCollapse').attr("src");
                if (imageSrc == '/HotelManagement/Image/expand_alt.png') {
                    $("#ExtraSearch").show('slow');
                    $('#imgCollapse').attr("src", '/HotelManagement/Image/collapse_alt.png');
                }
                else {
                    $("#ExtraSearch").hide('slow');
                    $('#imgCollapse').attr("src", '/HotelManagement/Image/expand_alt.png');
                }
            })

            $('#ContentPlaceHolder1_txtSrcFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSrcToDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtSrcToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSrcFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtSrcDateOfBirth').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $(function () {
                $("#myTabs").tabs();
            });
            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                //LoadGridInformation();
                GridPaging(1, 1);
            });

            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            $('#BankDiv').hide();
            $('#' + ddlPayMode).change(function () {
                if ($('#' + ddlPayMode).val() == "Cash") {
                    $('#BankDiv').hide();
                    $('#EmployeeInfoDiv').hide();
                    $('#RoomNumberDiv').hide();
                    $('#AccountsPostingDiv').hide();
                    $('#CashPaymentAccountHeadDiv').show();
                    $('#BankPaymentAccountHeadDiv').hide();
                    $('#CompanyInfoDiv').hide();
                }
                else if ($('#' + ddlPayMode).val() == "Card") {
                    $('#RoomNumberDiv').hide();
                    $('#BankDiv').show();
                    $('#EmployeeInfoDiv').hide();
                    $('#AccountsPostingDiv').hide();
                    $('#CashPaymentAccountHeadDiv').hide();
                    $('#BankPaymentAccountHeadDiv').show();
                    $('#CompanyInfoDiv').hide();
                }
                else if ($('#' + ddlPayMode).val() == "Other Room") {
                    $('#BankDiv').hide();
                    $('#RoomNumberDiv').show();
                    $('#EmployeeInfoDiv').hide();
                    $('#AccountsPostingDiv').hide();
                    $('#CompanyInfoDiv').hide();
                }
                else if ($('#' + ddlPayMode).val() == "Employee") {
                    $('#BankDiv').hide();
                    $('#RoomNumberDiv').hide();
                    $('#EmployeeInfoDiv').show();
                    $('#AccountsPostingDiv').hide();
                    $('#CompanyInfoDiv').hide();
                }
                else if ($('#' + ddlPayMode).val() == "Company") {
                    $('#BankDiv').hide();
                    $('#RoomNumberDiv').hide();
                    $('#EmployeeInfoDiv').hide();
                    $('#AccountsPostingDiv').hide();
                    $('#CompanyInfoDiv').show();
                }
                else if ($('#' + ddlPayMode).val() == "Include With Room") {
                    $('#BankDiv').hide();
                    $('#RoomNumberDiv').hide();
                    $('#EmployeeInfoDiv').hide();
                    $('#AccountsPostingDiv').hide();
                    $('#CompanyInfoDiv').hide();
                }
            });

            $("#btnAddDetailGuestPayment").click(function () {
                var enteredAmount = $.trim($("#<%=txtPaymentAmount.ClientID %>").val());

                if (enteredAmount == "") {
                    toastr.warning('Entered Amount is not in correct format.');
                    return;
                }
                else if (enteredAmount == "0") {
                    toastr.warning('Entered Amount is not in correct format.');
                    return;
                }
                else if (CommonHelper.IsDecimal(enteredAmount) == false) {
                    toastr.warning('Entered Amount is not in correct format.');
                    return;
                }
                else {
                    SaveGuestPaymentDetailsInformationByWebMethod();
                }
            });
        });


        function SaveGuestPaymentDetailsInformationByWebMethod() {
            var Amount = $("#<%=txtPaymentAmount.ClientID %>").val();
            var floatAmout = parseFloat(Amount);
            if (floatAmout < 0) {
                toastr.warning('Receive Amount is not in correct format.');
                return;
            }

            var isEdit = false;
            if ($('#btnAddDetailGuestPayment').val() == "Edit") {
                $("#<%=lblHiddenIdDetailGuestPayment.ClientID %>").val("5");
                isEdit = true;
            }
            else {
                $("#<%=lblHiddenIdDetailGuestPayment.ClientID %>").val("0");
            }
            var ddlRegistrationId = $("#<%=hfRegistrationId.ClientID %>").val();
            var ddlPayMode = $("#<%=ddlPayMode.ClientID %>").val();
            var txtReceiveLeadgerAmount = $("#<%=txtPaymentAmount.ClientID %>").val();
            var ddlCashReceiveAccountsInfo = $("#<%=ddlCashReceiveAccountsInfo.ClientID %>").val();

            var txtCardNumber = $("#<%=txtCardNumber.ClientID %>").val();
            var ddlCardType = $("#<%=ddlCardType.ClientID %>").val();
            var txtExpireDate = $("#<%=txtExpireDate.ClientID %>").val();
            var txtCardHolderName = $("#<%=txtCardHolderName.ClientID %>").val();

            var txtChecqueNumber = $("#<%=txtChecqueNumber.ClientID %>").val();
            var ddlBankId = $("#<%=ddlBankId.ClientID %>").val();
            var ddlChecquePaymentAccountHeadId = $("#<%=ddlChecquePaymentAccountHeadId.ClientID %>").val();
            var ddlCardPaymentAccountHeadId = $("#<%=ddlCardPaymentAccountHeadId.ClientID %>").val();

            var ddlCompanyPaymentAccountHead = $("#<%=ddlCompanyPaymentAccountHead.ClientID %>").val();
            var ddlPaidByRegistrationId = 0;

            var RefundAccountHead = $("#<%=ddlRefundAccountHead.ClientID %>").val();
            var ddlEmpId = $("#<%=ddlEmpId.ClientID %>").val();
            var ddlEmployeePaymentAccountHead = $("#<%=ddlEmployeePaymentAccountHead.ClientID %>").val();

            var paymentDescription = "";
            var ddlPaidByRegistrationId = 0;

            if (ddlPayMode == "Cash") {
                paymentDescription = "";
            }
            else if (ddlPayMode == "Card") {
                var ddlCardTypeText = $("#<%=ddlCardType.ClientID %> option:selected").text();
                paymentDescription = ddlCardTypeText;
            }
            else if (ddlPayMode == "Other Room") {
                ddlPaidByRegistrationId = $("#<%=ddlRoomNumberId.ClientID %>").val();
                var ddlPaidByRegistrationIdText = $("#<%=ddlRoomNumberId.ClientID %> option:selected").text();
                paymentDescription = "Room# " + ddlPaidByRegistrationIdText;
            }
            else if (ddlPayMode == "Refund") {
                paymentDescription = "";
            }
            else if (ddlPayMode == "Company") {
                var ddlPaidByRegistrationId = $("#<%=ddlCompanyName.ClientID %>").val();
                var ddlPaidByRegistrationIdText = $("#<%=ddlCompanyName.ClientID %> option:selected").text();
                ddlCompanyPaymentAccountHead = $("#<%=ddlCompanyName.ClientID %>").val();
                paymentDescription = "Company: " + ddlPaidByRegistrationIdText;
            }
            else if (ddlPayMode == "Employee") {
                var ddlPaidByRegistrationId = $("#<%=ddlEmpId.ClientID %>").val();
                var ddlPaidByRegistrationIdText = $("#<%=ddlEmpId.ClientID %> option:selected").text();
                paymentDescription = "Employee: " + ddlPaidByRegistrationIdText;
            }

    if (ddlPayMode == "Card") {
        if (ddlCardType == "0") {
            toastr.warning('Please Select Card Type.');
            return;
        }
        else if (ddlBankId == "0") {
            toastr.warning('Please Select Bank Name.');
            return;
        }
    }

    if (ddlPayMode == "Other Room") {
        if (ddlPaidByRegistrationId == "0") {
            toastr.warning('Please Select Guest Payment Room Number.');
            return;
        }
    }
    if (ddlPayMode == "Employee") {
        if (ddlPaidByRegistrationId == "0") {
            toastr.warning('Please Select Employee Name.');
            return;
        }
    }
    if (ddlPayMode == "Company") {
        if (ddlPaidByRegistrationId == "0") {
            toastr.warning('Please Select Company Name.');
            return;
        }
    }

    $('#btnAddDetailGuestPayment').val("Add");
    PageMethods.PerformSaveGuestPaymentDetailsInformationByWebMethod(isEdit, paymentDescription, ddlPayMode, ddlBankId, txtReceiveLeadgerAmount, ddlRegistrationId, ddlCashReceiveAccountsInfo, txtCardNumber, ddlCardType, txtExpireDate, txtCardHolderName, txtChecqueNumber, ddlChecquePaymentAccountHeadId, ddlCardPaymentAccountHeadId, ddlCompanyPaymentAccountHead, ddlPaidByRegistrationId, RefundAccountHead, ddlEmpId, ddlEmployeePaymentAccountHead, OnPerformSaveGuestPaymentDetailsInformationSucceeded, OnPerformSaveGuestPaymentDetailsInformationFailed)
    return false;
}
function OnPerformSaveGuestPaymentDetailsInformationFailed(error) {
    toastr.error(error.get_message());
}
function OnPerformSaveGuestPaymentDetailsInformationSucceeded(result) {

    var descriptionNId = result.split('#');
    var strTable = "";
    var rowCounter = $("#ReservationDetailGrid tbody tr").length;
    var paymentMethod = $("#ContentPlaceHolder1_ddlPayMode option:selected").text();

    if (rowCounter % 2 == 0) {
        strTable += "<tr id='trPay" + "Payment" + descriptionNId[0] + "' style='background-color:#E3EAEB;'><td align='left' style='width: 40%;'>" + paymentMethod + "</td>";
    }
    else {
        strTable += "<tr id='trPay" + "Payment" + descriptionNId[0] + "' style='background-color:White;'><td align='left' style='width: 40%;'>" + paymentMethod + "</td>";
    }
    strTable += "<td align='left' style='width: 40%;'>" + descriptionNId[1] + "</td>";
    strTable += "<td align='left' style='width: 20%;'>" + $("#<%=txtPaymentAmount.ClientID %>").val() + "</td>";
    strTable += "<td align='center' style='width: 15%;'>";
    strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformGuestPaymentDetailDelete(\"Payment\"," + descriptionNId[0] + ")' alt='Delete Information' border='0' />";
    strTable += "</td></tr>";

    $("#ReservationDetailGrid tbody:last").append(strTable);
    var total = CalculateTotalPaid();
    GrandTotalCheck(total);
    ClearDetailsPart();
}

function PerformPaymentPosting() {
    var totalPayment = 0.00, grandTotal = 0.00;

    $("#ReservationDetailGrid tbody tr").each(function () {
        totalPayment += parseFloat($(this).find("td:eq(2)").text());
    });

    grandTotal = parseFloat($("#txtGrandTotal").text());
    if (totalPayment > grandTotal) { toastr.info("Payment Amount Cannot Greater Than Grand Total."); return false; }
    if (totalPayment < grandTotal) { toastr.info("Payment Amount Cannot Less Than Grand Total."); return false; }
    var txtBillId = $("#<%=hfRegistrationId.ClientID %>").val();
            var hfDeletedPaymentForPaymentId = $("#<%=hfDeletedPaymentForPaymentId.ClientID %>").val();
            var hfDeletedPaymentForTransferedPaymentId = $("#<%=hfDeletedPaymentForTransferedPaymentId.ClientID %>").val();
            var hfDeletedPaymentForAchievementPaymentId = $("#<%=hfDeletedPaymentForAchievementPaymentId.ClientID %>").val();
            PageMethods.PerformPaymentPostingByWebMethod(txtBillId, hfDeletedPaymentForPaymentId, hfDeletedPaymentForTransferedPaymentId, hfDeletedPaymentForAchievementPaymentId, OnPaymentPostingSuccess, OnPaymentPostingFailure);
        }

        function OnPaymentPostingSuccess(result) {
            if (result == "1") {
                toastr.success("Save Operation Successfull.");
                $("#<%=hfRegistrationId.ClientID %>").val("");
                $("#<%=hfDeletedPaymentForPaymentId.ClientID %>").val("");
                $("#<%=hfDeletedPaymentForTransferedPaymentId.ClientID %>").val("");
                $("#<%=hfDeletedPaymentForAchievementPaymentId.ClientID %>").val("");
                //GridPaging(1, 1);

                $("#<%=ddlPayMode.ClientID %>").val("Cash");
                $("#<%=txtPaymentAmount.ClientID %>").val("");
                $("#<%=ddlCardType.ClientID %>").val("0");
                $("#<%=ddlBankId.ClientID %>").val("0");
                $('#txtBankId').val("");
                $("#<%=txtCardNumber.ClientID %>").val("");
                $("#<%=ddlRoomNumberId.ClientID %>").val("0");
                $("#<%=ddlEmpId.ClientID %>").val("0");
                $("#<%=ddlCompanyName.ClientID %>").val("0");
            }
            else {
                toastr.error("Save Operation Un-Successfull.");
            }
        }

        function OnPaymentPostingFailure(error) {
        }

        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=txtSrcCompanyName.ClientID %>").val('');
            $("#<%=txtSrcDateOfBirth.ClientID %>").val('');
            $("#<%=txtSrcEmailAddress.ClientID %>").val('');
            $("#<%=txtSrcFromDate.ClientID %>").val('');
            $("#<%=txtSrcToDate.ClientID %>").val('');
            $("#<%=txtSrcGuestName.ClientID %>").val('');
            $("#<%=txtSrcMobileNumber.ClientID %>").val('');
            $("#<%=txtSrcNationalId.ClientID %>").val('');
            $("#<%=txtSrcPassportNumber.ClientID %>").val('');
            $("#<%=txtSrcRegistrationNumber.ClientID %>").val('');
            $("#<%=txtSrcRoomNumber.ClientID %>").val('');
            $("#SearchPanel").hide();
            $("#GuestDetaails").hide();
            $("#ExtraSearch").hide('slow');
            $('#imgCollapse').attr("src", '/HotelManagement/Image/expand_alt.png');
        }

        function LoadGridInformation() {
            var companyName = $("#<%=txtSrcCompanyName.ClientID %>").val();
            var DateOfBirth = $("#<%=txtSrcDateOfBirth.ClientID %>").val();
            var EmailAddress = $("#<%=txtSrcEmailAddress.ClientID %>").val();
            var FromDate = $("#<%=txtSrcFromDate.ClientID %>").val();
            var ToDate = $("#<%=txtSrcToDate.ClientID %>").val();
            var GuestName = $("#<%=txtSrcGuestName.ClientID %>").val();
            var MobileNumber = $("#<%=txtSrcMobileNumber.ClientID %>").val();
            var NationalId = $("#<%=txtSrcNationalId.ClientID %>").val();
            var PassportNumber = $("#<%=txtSrcPassportNumber.ClientID %>").val();
            var RegistrationNumber = $("#<%=txtSrcRegistrationNumber.ClientID %>").val();
            var RoomNumber = $("#<%=txtSrcRoomNumber.ClientID %>").val();

            PageMethods.SearchGuestAndLoadGridInformation(companyName, DateOfBirth, EmailAddress, FromDate, ToDate, GuestName, MobileNumber, NationalId, PassportNumber, RegistrationNumber, RoomNumber, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var companyName = $("#<%=txtSrcCompanyName.ClientID %>").val();
            var DateOfBirth = $("#<%=txtSrcDateOfBirth.ClientID %>").val();
            var EmailAddress = $("#<%=txtSrcEmailAddress.ClientID %>").val();
            var FromDate = $("#<%=txtSrcFromDate.ClientID %>").val();
            var ToDate = $("#<%=txtSrcToDate.ClientID %>").val();
            var GuestName = $("#<%=txtSrcGuestName.ClientID %>").val();
            var MobileNumber = $("#<%=txtSrcMobileNumber.ClientID %>").val();
            var NationalId = $("#<%=txtSrcNationalId.ClientID %>").val();
            var PassportNumber = $("#<%=txtSrcPassportNumber.ClientID %>").val();
            var RegistrationNumber = $("#<%=txtSrcRegistrationNumber.ClientID %>").val();
            var ReservationNumber = $("#<%=txtSrcReservationNumber.ClientID %>").val();
            var RoomNumber = $("#<%=txtSrcRoomNumber.ClientID %>").val();
            var gridRecordsCount = $("#tblGuestInfo tbody tr").length;
            CommonHelper.SpinnerOpen();
            PageMethods.SearchGuestAndLoadGridInformationPaging(companyName, DateOfBirth, EmailAddress, FromDate, ToDate, GuestName, MobileNumber, NationalId, PassportNumber, RegistrationNumber, ReservationNumber, RoomNumber, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectPagingSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {
            $("#ltlTableWiseItemInformation").html(result);
            return false;
        }
        function OnLoadObjectPagingSucceeded(result) {
            CommonHelper.SpinnerClose();

            var gridRecordsCount = $("#tblGuestInfo tbody tr").length;
            if (gridRecordsCount < 0) {
                $("#tblGuestInfo tbody").html("");
            }
            else {
                $("#tblGuestInfo tbody tr").remove();
            }
            $("#GridPagingContainer ul").html("");
            var tr = "", totalRow = 0;
            var i = 0;
            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#tblGuestInfo tbody ").append(emptyTr);
                return false;
            }
            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#tblGuestInfo tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                //strTable += "<tr style='background-color:White;'><td align='left' style='width: 25%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + dr.GuestId + ")'>" + dr.GuestName + "</td>";
                //strTable += "<td align='left' style='width: 25%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + dr.GuestId + ")'>" + dr.CountryName + "</td>";
                //strTable += "<td align='left' style='width: 20%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + dr.GuestId + ")'>" + dr.CountryName + "</td>";
                //strTable += "<td align='left' style='width: 20%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + dr.GuestId + ")'>" + dr.GuestPhone + "</td>";
                //strTable += "<td align='left' style='width: 20%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + dr.GuestId + ")'>" + dr.GuestEmail + "</td>";
                //strTable += "<td align='left' style='width: 15%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + dr.GuestId + ")'>" + dr.RoomNumber + "</td>";
                tr += "<td align='left' style='width: 25%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + gridObject.GuestId + ")'>" + gridObject.GuestName + "</td>";
                tr += "<td align='left' style='width: 20%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + gridObject.GuestId + ")'>" + gridObject.CountryName + "</td>";
                tr += "<td align='left' style='width: 20%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + gridObject.GuestId + ")'>" + gridObject.GuestPhone + "</td>";
                tr += "<td align='left' style='width: 20%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + gridObject.GuestId + ")'>" + gridObject.GuestEmail + "</td>";
                //tr += "<td align='left' style='width: 15%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + gridObject.GuestId + ")'>" + gridObject.RoomNumber + "</td>";
                tr += "</td></tr>";

                $("#tblGuestInfo tbody").append(tr);
                tr = "";
            });
            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            CommonHelper.ApplyIntigerValidation();
            return false;
        }
        function OnLoadObjectFailed(error) {
            alert(error.get_message());
        }
        function ReloadGrid(IsCurrentOrPreviousPage) {
            var currentPageNumber = $("#GridPagingContainer ul li[class='active']").text();
            if (currentPageNumber == "")
                currentPageNumber = 1;

            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }
        function SelectGuestInformation(GuestId) {
            PageMethods.LoadDetailInformation(GuestId, OnLoadDetailObjectSucceeded, OnLoadDetailObjectFailed);
            LoadGuestImage(GuestId);
            LoadGuestHistory(GuestId)
            return false;
        }

        function LoadGuestHistory(guestId) {
            PageMethods.GetGuestRegistrationHistoryGuestId(guestId, OnLoadGuestHistorySucceeded, OnLoadGuestHistoryFailed);
            return false;
        }
        function OnLoadGuestHistorySucceeded(result) {
            $("#guestHistoryDiv").html(result);
            return false;
        }
        function OnLoadGuestHistoryFailed(error) {
            alert(error.get_message());
        }

        function LoadGuestImage(guestId) {
            PageMethods.GetDocumentsByUserTypeAndUserId(guestId, OnLoadImagesSucceeded, OnLoadImagesFailed);
            return false;
        }
        function OnLoadImagesSucceeded(result) {
            $("#imageDiv").html(result);
            return false;
        }
        function OnLoadImagesFailed(error) {
            alert(error.get_message());
        }
        function OnLoadDetailObjectSucceeded(result) {
            if (result.GuestDOB) {
                $("#<%=lblDGuestDOB.ClientID %>").text(GetStringFromDateTime(result.GuestDOB));
            }
            if (result.PIssueDate) {

                $("#<%=lblDPIssueDate.ClientID %>").text(GetStringFromDateTime(result.PIssueDate));
            }
            if (result.PExpireDate) {

                $("#<%=lblDPExpireDate.ClientID %>").text(GetStringFromDateTime(result.PExpireDate));
            }
            if (result.VIssueDate) {

                $("#<%=lblDVIssueDate.ClientID %>").text(GetStringFromDateTime(result.VIssueDate));
            }
            if (result.VExpireDate) {

                $("#<%=lblDVExpireDate.ClientID %>").text(GetStringFromDateTime(result.VExpireDate));
            }

            $("#<%=lblDGuestName.ClientID %>").text(result.GuestName);
            $("#<%=lblDGuestSex.ClientID %>").text(result.GuestSex);
            $("#<%=lblDGuestEmail.ClientID %>").text(result.GuestEmail);
            $("#<%=lblDGuestPhone.ClientID %>").text(result.GuestPhone);
            $("#<%=lblDGuestAddress1.ClientID %>").text(result.GuestAddress1);
            $("#<%=lblDGuestAddress2.ClientID %>").text(result.GuestAddress2);
            $("#<%=lblDGuestCity.ClientID %>").text(result.GuestCity);
            $("#<%=lblDGuestZipCode.ClientID %>").text(result.GuestZipCode);
            $("#<%=lblDGuestNationality.ClientID %>").text(result.GuestNationality);
            $("#<%=lblDGuestDrivinlgLicense.ClientID %>").text(result.GuestDrivinlgLicense);
            $("#<%=lblDGuestProfession.ClientID %>").text(result.ProfessionName);
            $("#<%=lblDNationalId.ClientID %>").text(result.NationalId);
            $("#<%=lblDPassportNumber.ClientID %>").text(result.PassportNumber);
            $("#<%=lblDPIssuePlace.ClientID %>").text(result.PIssuePlace);
            $("#<%=lblDVisaNumber.ClientID %>").text(result.VisaNumber);
            $("#<%=lblDCountryName.ClientID %>").text(result.CountryName);

            if (result.TotalStayedNight != null)
                $("#<%=lblNumberOfNights.ClientID %>").text(result.TotalStayedNight);

            $("#GuestDetaails").dialog({
                width: 900,
                height: 550,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                title: "Guest Info",
                show: 'slide'
            });

            return false;
        }
        function OnLoadDetailObjectFailed(error) {
            alert(error.get_message());
        }

        function PerformGuestBillInfoShow(RegistrationId, ConvertionRate) {
            PageMethods.SetSessionValueForGuestBill(RegistrationId, ConvertionRate, OnSessionValueForGuestBillSucceeded, OnSessionValueForGuestBillFailed);
            return false;
        }

        function OnSessionValueForGuestBillSucceeded(result) {
            var RegistrationId = result;
            var url = "/HotelManagement/Reports/frmReportGuestBillInfo.aspx?GuestBillInfo=" + RegistrationId;
            var popup_window = "Bill Preview";
            window.open(url, popup_window, "width=830,height=680,left=300,top=50,resizable=yes");
        }
        function OnSessionValueForGuestBillFailed(error) {
            alert(error.get_message());
        }

        function PerformCancelCheckOut(registrationID, guestId) {

            if (confirm("Do you want to cancel Check Out?")) {
                PageMethods.PerformCancelCheckOut(registrationID, guestId, OnCancelCheckOutSucceed, OnCancelCheckOutFailed);
            }
        }
        function OnCancelCheckOutSucceed(result) {
            if (result[0].IsCanceled == "1") {
                SelectGuestInformation(result[0].GuestId);
            }
        }
        function OnCancelCheckOutFailed(error) {
            alert(error.get_message());
        }

        function PerformCheckedOutPaymentModification(billId) {
            $("#<%=hfRegistrationId.ClientID %>").val(billId);
            PageMethods.GetGuestBillPayment(billId, OnLoadEditDataSucceeded, OnLoadEditDataFailed);
        }

        function OnLoadEditDataSucceeded(result) {
            billPaymmentDetails = result;
            var isCompanyCheckOutEnable = 0;
            var strTable = "";
            var rowCounter = 0, billCount = 0, grandTotal = 0;
            billCount = result.length;

            strTable += "<table style='width:100%' class='table table-bordered table-condensed table-responsive' id='ReservationDetailGrid'> <thead> <tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='left' scope='col'>Payment Mode</th><th align='left' scope='col'>Description</th><th align='left' scope='col'>Amount</th><th align='center' scope='col'>Action</th></tr></thead><tbody>";

            for (rowCounter = 0; rowCounter < billCount; rowCounter++) {
                if (result[rowCounter].PaymentType != "Refund") {
                    grandTotal = grandTotal + result[rowCounter].PaymentAmount;
                }
                else {
                    grandTotal = grandTotal - result[rowCounter].PaymentAmount;
                }

                if (rowCounter % 2 == 0) {
                    strTable += "<tr id='trPay" + result[rowCounter].TransactionType + "" + result[rowCounter].PaymentId + "' style='background-color:#E3EAEB;'><td align='left' style='width: 40%;'>" + result[rowCounter].PaymentMode + "</td>";
                }
                else {
                    strTable += "<tr id='trPay" + result[rowCounter].TransactionType + "" + result[rowCounter].PaymentId + "' style='background-color:White;'><td align='left' style='width: 40%;'>" + result[rowCounter].PaymentMode + "</td>";
                }
                strTable += "<td align='left' style='width: 40%;'>" + result[rowCounter].PaymentDescription + "</td>";
                strTable += "<td align='left' style='width: 20%;'>" + result[rowCounter].PaymentAmount + "</td>";
                strTable += "<td align='center' style='width: 15%;'>";
                strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformGuestPaymentDetailDelete(\"" + result[rowCounter].PaymentType + "\", " + result[rowCounter].PaymentId + ")' alt='Delete Information' border='0' />";
                strTable += "</td></tr>";

                if (result[rowCounter].CompanyId == "1") {
                    isCompanyCheckOutEnable = 1;
                }
            }
            strTable += "</tbody></table>";

            if (isCompanyCheckOutEnable == 0) {
                $("#ContentPlaceHolder1_ddlPayMode").find('option').filter(function () {
                    return this.value === 'Company' && $(this).text() === 'Company';
                }).remove();
            }
            else {
                $("#<%=ddlPayMode.ClientID %>").find('option').filter(function () {
                    return this.value === 'Company' && $(this).text() === 'Company';
                }).remove();

                $("#<%=ddlPayMode.ClientID %>").append('<option value="Company">Company</option>')
            }

            $("#GuestPaymentDetailGrid").html(strTable);
            $("#txtGrandTotal").text(grandTotal);
            $("#TotalPaid").text("Total Amount: " + grandTotal);
            $("#dueTotal").text("Due Amount: 0");
            $('#ContentPlaceHolder1_AlartMessege').hide();

            $("#GuestBillPaymentDialog").dialog({
                width: 900,
                height: 500,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Guest Payment Information",
                show: 'slide'
            });
        }

        function OnLoadEditDataFailed(error) {

        }
        function PerformGuestPaymentDetailDelete(transactionType, paymentId) {
            var paymentDeleted = _.findWhere(billPaymmentDetails, { PaymentId: paymentId });
            var paymentIds = $("#<%=hfDeletedPaymentForPaymentId.ClientID %>").val();
            var transferedIds = $("#<%=hfDeletedPaymentForTransferedPaymentId.ClientID %>").val();
            var achievementIds = $("#<%=hfDeletedPaymentForAchievementPaymentId.ClientID %>").val();

            if (paymentDeleted != null) {
                billPaymmentDetails = _.without(billPaymmentDetails, paymentDeleted);

                if (transactionType == "Payment") {
                    if (paymentIds != "") {
                        paymentIds += "," + paymentDeleted.PaymentId;
                    }
                    else {
                        paymentIds = paymentDeleted.PaymentId;
                    }
                }
                else if (transactionType == "Company") {
                    if (paymentIds != "") {
                        paymentIds += "," + paymentDeleted.PaymentId;
                    }
                    else {
                        paymentIds = paymentDeleted.PaymentId;
                    }
                }
                else if (transactionType == "Transfered") {
                    if (transferedIds != "") {
                        transferedIds += "," + paymentDeleted.PaymentId;
                    }
                    else {
                        transferedIds = paymentDeleted.PaymentId;
                    }
                }
                else if (transactionType == "Achievement") {
                    if (achievementIds != "") {
                        achievementIds += "," + paymentDeleted.PaymentId;
                    }
                    else {
                        achievementIds = paymentDeleted.PaymentId;
                    }
                }

                $("#<%=hfDeletedPaymentForPaymentId.ClientID %>").val(paymentIds);
                $("#<%=hfDeletedPaymentForTransferedPaymentId.ClientID %>").val(transferedIds);
                $("#<%=hfDeletedPaymentForAchievementPaymentId.ClientID %>").val(achievementIds);
                PageMethods.PerformDeleteGuestPaymentByWebMethod(transactionType, paymentId, OnPerformDeleteGuestPaymentDetailsSucceeded, OnPerformDeleteGuestPaymentDetailsFailed);
                return false;

                $("#trPay" + transactionType + paymentId).remove();
                var total = CalculateTotalPaid();
                GrandTotalCheck(total);
            }
            else {
                PageMethods.PerformDeleteGuestPaymentByWebMethod(transactionType, paymentId, OnPerformDeleteGuestPaymentDetailsSucceeded, OnPerformDeleteGuestPaymentDetailsFailed);
                return false;
            }
        }

        function OnPerformDeleteGuestPaymentDetailsSucceeded(result) {
            $("#trPay" + result).remove();
            var total = CalculateTotalPaid();
            GrandTotalCheck(total);
            return false;
        }
        function OnPerformDeleteGuestPaymentDetailsFailed(error) {
        }

        function ClearDetailsPart() {
            $("#<%=txtPaymentAmount.ClientID %>").val('');
            $("#<%=txtCardNumber.ClientID %>").val('');
            $("#<%=ddlCardType.ClientID %>").val('a');
            $("#<%=txtExpireDate.ClientID %>").val('');
            $("#<%=txtCardHolderName.ClientID %>").val('');
            var ddlPayMode = '<%=ddlPayMode.ClientID%>'

            if ($('#' + ddlPayMode).val() == "Cash") {
                $('#BankDiv').hide();
                $('#EmployeeInfoDiv').hide();
                $('#RoomNumberDiv').hide();
                $('#AccountsPostingDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CompanyInfoDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $('#RoomNumberDiv').hide();
                $('#BankDiv').show();
                $('#EmployeeInfoDiv').hide();
                $('#AccountsPostingDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').show();
                $('#CompanyInfoDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Other Room") {
                $('#BankDiv').hide();
                $('#EmployeeInfoDiv').hide();
                $('#RoomNumberDiv').show();
                $('#AccountsPostingDiv').hide();
                $('#CompanyInfoDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Employee") {
                $('#BankDiv').hide();
                $('#RoomNumberDiv').hide();
                $('#EmployeeInfoDiv').show();
                $('#AccountsPostingDiv').hide();
                $('#CompanyInfoDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $('#BankDiv').hide();
                $('#RoomNumberDiv').hide();
                $('#EmployeeInfoDiv').hide();
                $('#AccountsPostingDiv').hide();
                $('#CompanyInfoDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "Include With Room") {
                $('#BankDiv').hide();
                $('#EmployeeInfoDiv').hide();
                $('#RoomNumberDiv').hide();
                $('#AccountsPostingDiv').hide();
                $('#CompanyInfoDiv').hide();
            }
        }
        function ValidateForm() {
            if (GrandTotalCheck(CalculateTotalPaid()) == true) {
                return true;
            }
            else {
                return false;
            }
        }
        function GrandTotalCheck(paidAmount) {
            var txtGrandTotal = $("#txtGrandTotal").text();
            var GrandTotal = parseFloat(txtGrandTotal);
            var PaidTotal = parseFloat(paidAmount);

            var status = false;

            if (PaidTotal != GrandTotal) {
                $('#ContentPlaceHolder1_btnPaymentPosting').attr('disabled', true);
                $('#ContentPlaceHolder1_AlartMessege').show();
                status = false;
            }
            else {
                $('#ContentPlaceHolder1_btnPaymentPosting').attr('disabled', false);
                $('#ContentPlaceHolder1_AlartMessege').hide();
                status = true;
            }

            return status;
        }
        function CalculateTotalPaid() {
            var total = 0, due = 0;
            $("#GuestPaymentDetailGrid tbody tr").each(function () {
                total += parseFloat($(this).find("td:eq(2)").text());
            });

            var grandTotal = parseFloat($("#txtGrandTotal").text());
            due = grandTotal - total;

            $("#TotalPaid").text("Total Amount: " + total);
            $("#dueTotal").text("Due Amount: " + due);

            return total;
        }
    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <asp:HiddenField ID="hfRegistrationId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDeletedPaymentForPaymentId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDeletedPaymentForTransferedPaymentId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDeletedPaymentForAchievementPaymentId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsSavePermission" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsDeletePermission" runat="server"></asp:HiddenField>
    <div id="GuestBillPaymentDialog" class="panel panel-default" style="display: none;">
        <div id="GuestPaymentInformationDiv" runat="server">
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblPayMode" runat="server" class="control-label" Text="Payment Mode"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlPayMode" runat="server" TabIndex="6" CssClass="form-control">
                                <asp:ListItem>Cash</asp:ListItem>
                                <asp:ListItem>Card</asp:ListItem>
                                <%--<asp:ListItem>Cheque</asp:ListItem>
                                <asp:ListItem>Company</asp:ListItem>
                                <asp:ListItem>Refund</asp:ListItem>
                                <asp:ListItem>Include With Room</asp:ListItem>--%>
                                <%--<asp:ListItem Value="Other Room">Guest Room</asp:ListItem>
                                <asp:ListItem>Employee</asp:ListItem>--%>
                                <asp:ListItem>Company</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblPaymentAmount" runat="server" class="control-label" Text="Payment Amount"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtPaymentAmount" TabIndex="9" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div id="BankDiv">
                        <div style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblBankName" runat="server" class="control-label" Text="Bank Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlBankName" TabIndex="7" runat="server" CssClass="form-control"
                                        AutoPostBack="false">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Card Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCardType" TabIndex="8" runat="server" CssClass="form-control">
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
                                <asp:TextBox ID="txtCardNumber" TabIndex="9" CssClass="form-control" runat="server"></asp:TextBox>
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
                                    <asp:TextBox ID="txtExpireDate" TabIndex="10" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="Label4" runat="server" class="control-label" Text="Card Holder Name"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCardHolderName" TabIndex="11" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="RoomNumberDiv" style="display: none;">
                        <div class="form-group" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="lblRoomNumber" runat="server" class="control-label" Text="Room Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <%--<asp:DropDownList ID="ddlRoomId" TabIndex="12" runat="server" CssClass="form-control"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlRoomId_SelectedIndexChanged">
                                </asp:DropDownList>--%>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSrcRoomNumber" runat="server" class="control-label" Text="Room Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRoomNumberId" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div id="EmployeeInfoDiv" style="display: none;">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="IsRestaurantIntegrateWithPayrollVal" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblEmployeeName" runat="server" class="control-label" Text="Employee Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlEmpId" TabIndex="12" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div id="CompanyInfoDiv" style="display: none;">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblCompanyName" runat="server" class="control-label" Text="Company Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCompanyName" TabIndex="12" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div id="ChecquePaymentAccountHeadDiv" style="display: none;">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblChecquePaymentAccountHeadId" runat="server" class="control-label"
                                    Text="Accounts Posting Head"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlChecquePaymentAccountHeadId" runat="server" CssClass="form-control"
                                    TabIndex="6">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="lblChecqueNumber" runat="server" class="control-label" Text="Cheque Number"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtChecqueNumber" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
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
                                <asp:DropDownList ID="ddlCardPaymentAccountHeadId" runat="server" CssClass="form-control"
                                    TabIndex="6">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div id="EmployeePaymentAccountHeadDiv" style="display: none;">
                        <div class="form-group" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="Label5" runat="server" class="control-label" Text="Accounts Posting Head"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlEmployeePaymentAccountHead" runat="server" CssClass="form-control"
                                    TabIndex="6">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-group" style="display: none;">
                        <div class="col-md-2">
                            <asp:Label ID="Label6" runat="server" class="control-label" Text="Account Head"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <div id="ddlCashReceiveAccountsInfoDiv">
                                <asp:DropDownList ID="ddlCashReceiveAccountsInfo" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div id="ddlCardReceiveAccountsInfoDiv" style="display: none;">
                                <asp:DropDownList ID="ddlCardReceiveAccountsInfo" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div id="CompanyPaymentAccountHeadDiv" style="display: none;">
                                <asp:DropDownList ID="ddlCompanyPaymentAccountHead" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div id="RefundDiv" style="display: none;">
                        <div class="form-group">
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
                        <input type="button" tabindex="37" id="btnAddDetailGuestPayment" value="Add" class="btn btn-primary btn-sm" />
                        <asp:Label ID="lblHiddenIdDetailGuestPayment" runat="server" class="control-label"
                            Text='' Visible="False"></asp:Label>
                    </div>
                </div>
                <div class="form-group" style="padding-left: 10px; margin-top: 10px; margin-bottom: 2px; background-color: #eeeeee; border: 1px solid #cccccc;">
                    <div class="col-md-2">
                        <asp:Label ID="Label7" runat="server" class="control-label" Text="Grand Total"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <label id="txtGrandTotal" class="control-label">
                        </label>
                    </div>
                </div>
                <div id="GuestPaymentDetailGrid" class="childDivSection">
                </div>
                <div id="TotalPaid" class="totalAmout">
                </div>
                <div id="dueTotal" class="totalAmout">
                </div>
                <div>
                    <asp:Label ID="AlartMessege" runat="server" Style="color: Red;" Text='Grand Total and Guest Payment Amount is not Equal.'
                        CssClass="totalAmout" Visible="true"></asp:Label>
                </div>
            </div>
            <div class="childDivSection">
                <div id="AccountsPostingDiv" class="panel panel-default" style="display: none">
                    <div class="panel-heading">
                        Accounts Posting Information
                    </div>
                    <div class="panel-body">
                        <div id="ActionPanel">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblPaymentAccountHead" runat="server" class="control-label" Text="Account Head"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <div id="CashPaymentAccountHeadDiv">
                                            <asp:DropDownList ID="ddlCashPaymentAccountHead" TabIndex="17" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                        <div id="BankPaymentAccountHeadDiv" style="display: none;">
                                            <asp:DropDownList ID="ddlBankPaymentAccountHead" TabIndex="18" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblReceiveAmount" runat="server" class="control-label" Text="Receive Amount"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtReceiveAmount" runat="server" Enabled="false" CssClass="form-control"
                                            TabIndex="19"> </asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblConvertionRate" runat="server" class="control-label" Text="Convertion Rate"
                                            Visible="false"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtConvertionRate" runat="server" Visible="false" CssClass="form-control"
                                            TabIndex="20"> </asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblGLCompany" runat="server" class="control-label" Text="Company"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlGLCompany" TabIndex="21" CssClass="form-control" runat="server"
                                            onchange="PopulateProjects();">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblGLProject" runat="server" class="control-label" Text="Project"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlGLProject" runat="server" CssClass="form-control" TabIndex="22">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <%--<asp:Button ID="btnPaymentPosting" runat="server" ClientIDMode="Static" Text="Save"
                    CssClass="btn btn-primary btn-sm" OnClientClick="javascript:return ValidateForm();"
                    TabIndex="23" OnClick="btnPaymentPosting_Click" />--%>
                <button class="btn btn-primary btn-sm" type="button" onclick='javascript:return PerformPaymentPosting()'>
                    Save Payment</button>
            </div>
        </div>
    </div>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Guest Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <label for="GuestName" class="control-label col-md-2">
                        Guest Name</label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtSrcGuestName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="CompanyName" class="control-label col-md-2">
                        Company Name</label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtSrcCompanyName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="EmailAddress" class="control-label col-md-2">
                        Email Address</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSrcEmailAddress" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                    </div>
                    <label for="MobileNumber" class="control-label col-md-2">
                        Mobile Number</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSrcMobileNumber" runat="server" CssClass="form-control" TabIndex="8"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="NationalID" class="control-label col-md-2">
                        National ID</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSrcNationalId" runat="server" CssClass="form-control" TabIndex="9"></asp:TextBox>
                    </div>
                    <label for="DateOfBirth" class="control-label col-md-2">
                        Date of Birth</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSrcDateOfBirth" runat="server" CssClass="form-control"
                            TabIndex="10"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="PassportNumber" class="control-label col-md-2">
                        Passport Number</label>
                    <div class="col-md-9">
                        <asp:TextBox ID="txtSrcPassportNumber" runat="server" CssClass="form-control" TabIndex="11"></asp:TextBox>
                    </div>
                    <div class="col-md-1">
                        <div style="float: right; margin-left: 150px">
                            <img id="imgCollapse" width="40px" src="/HotelManagement/Image/expand_alt.png" />
                        </div>
                    </div>
                </div>
                <div id="ExtraSearch">
                    <div class="form-group">
                        <label for="RoomNumber" class="control-label col-md-2">
                            Room Number</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSrcRoomNumber" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                        </div>
                        <label for="RegistrationNo" class="control-label col-md-2">
                            Registration No.</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSrcRegistrationNumber" runat="server" CssClass="form-control"
                                TabIndex="4"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="FromDate" class="control-label col-md-2">
                            Arrive Date</label>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtSrcFromDate" runat="server" CssClass="form-control"
                                TabIndex="5"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtSrcToDate" runat="server" CssClass="form-control"
                                TabIndex="6"></asp:TextBox>
                        </div>
                        <label for="RegistrationNo" class="control-label col-md-2">
                            Reservation No.</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSrcReservationNumber" runat="server" CssClass="form-control"
                                TabIndex="4"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <button type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm" tabindex="12">
                            Search</button>
                        <button type="button" id="btnClear" tabindex="12" onclick="PerformClearAction()"
                            class="btn btn-primary btn-sm">
                            Clear</button>
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
            <div class="form-group" style="">
                <table id="tblGuestInfo" style="width: 100%;" class="table table-bordered table-condensed table-responsive">
                    <thead>
                        <tr style="color: White; background-color: #44545E; font-weight: bold; text-align: left;">
                            <th style="width: 25%;">Guest Name
                            </th>
                            <th style="width: 20%;">Country Name
                            </th>
                            <th style="width: 20%;">Phone
                            </th>
                            <th style="width: 20%;">Email
                            </th>
                            <%--<th style="width: 15%;">Room Number
                            </th>--%>
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
            <div id="ltlTableWiseItemInformation">
            </div>
        </div>
    </div>
    <div style="height: 45px">
    </div>
    <div id="GuestDetaails" style="padding: 0.5em;">
        <div id="TabPanel" style="margin-top: 35px;">
            <div id="myTabs">
                <ul id="tabPage" class="ui-style">
                    <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                        href="#tab-1">Guest Information</a></li>
                    <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                        href="#tab-2">Guest Documents</a></li>
                    <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                        href="#tab-3">Guest History</a></li>
                </ul>
                <div id="tab-1" style="padding: 0;">
                    <div id="GuestInfo" class="panel panel-default" style="font-weight: bold; margin: 0.5em;">
                        <div class="panel-heading">
                            Guest Information
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <table class="table table-striped table-bordered table-condensed table-hover">
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestName" runat="server" Text="Guest Name"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestName" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestDOB" runat="server" Text="Date of Birth"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestDOB" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestSex" runat="server" Text="Gender"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestSex" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestEmail" runat="server" Text="Email"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestEmail" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestPhone" runat="server" Text="Phone Number"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestPhone" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestAddress1" runat="server" Text="Company Name"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestAddress1" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestAddress2" runat="server" Text="Guest Address"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestAddress2" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestCity" runat="server" Text="City "></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestCity" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestZipCode" runat="server" Text="Zip Code"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestZipCode" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestNationality" runat="server" Text="Guest Nationality"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestNationality" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestDrivinlgLicense" runat="server" Text="Driving License No"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestDrivinlgLicense" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestProfession" runat="server" Text="Profession"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestProfession" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLNationalId" runat="server" Text="National ID"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDNationalId" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLPassportNumber" runat="server" Text="Passport Number"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDPassportNumber" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLPIssueDate" runat="server" Text="Pasport Issue Date"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDPIssueDate" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLPIssuePlace" runat="server" Text="Passport Issue Place"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDPIssuePlace" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLPExpireDate" runat="server" Text="Passport Expiry Date"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDPExpireDate" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLVisaNumber" runat="server" Text="Visa Number"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDVisaNumber" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLVIssueDate" runat="server" Text="Visa Issue Date"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDVIssueDate" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLVExpireDate" runat="server" Text="Visa Expiry Date"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDVExpireDate" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLCountryName" runat="server" Text="Country Name"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDCountryName" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="Label1" runat="server" Text="Number of Nights"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblNumberOfNights" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="tab-2" style="padding: 0;">
                    <div id="imageDiv">
                    </div>
                </div>
                <div id="tab-3" style="padding: 0;">
                    <div class="panel-body">
                        <div id="guestHistoryDiv">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
