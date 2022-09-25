<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmBillSearch.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmBillSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var billPaymmentDetails = [];
        var vcv = [];

        if ($.trim($("#<%=txtBillId.ClientID %>").val()) != "") {
            GridPaging(1, 1);
            $("#<%=txtBillId.ClientID %>").val("");
            //toastr.info($("#<%=txtBillId.ClientID %>").val());
        }

        //Bread Crumbs Information-------------
        $(document).ready(function () {
            CommonHelper.AutoSearchClientDataSource("txtBankId", "ContentPlaceHolder1_ddlBankId", "ContentPlaceHolder1_ddlBankId");

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

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
                ignoreReadonly: true,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSrcFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });

        });

        $(document).ready(function () {
            $('#ContentPlaceHolder1_gvVoucherInfo_ChkApproved').click(function () {
                if ($('#ContentPlaceHolder1_gvVoucherInfo_ChkApproved').is(':checked')) {
                    CheckAllCheckBox()
                }
                else {
                    UnCheckAllCheckBox();
                }
            });

            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });
            $("#SearchPanel").hide('slow');

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

                if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Other Room") {
                    var duplicateCheck = false;
                    $('#GuestPaymentDetailGrid table tbody tr').each(function () {
                        var v = $.trim($(this).find("td:eq(0)").text());
                        if ((v).indexOf("Guest Room") >= 0)
                            duplicateCheck = true;
                    });

                    if (duplicateCheck == true) {
                        toastr.warning('Multiple Room Payment Restiction.');
                        return;
                    }
                }

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
            var ddlRegistrationId = 0;
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

function GetTotalPaidAmount() {
    PageMethods.PerformGetTotalPaidAmountByWebMethod(OnPerformGetTotalPaidAmountSucceeded, PerformGetTotalPaidAmountFailed)
    return false;
}

function PerformGetTotalPaidAmountFailed(error) {
    toastr.error(error.get_message());
}
function OnPerformGetTotalPaidAmountSucceeded(result) {
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
                        toastr.info(response.d);
                    }
                });
            }
        }

        function OnProjectsPopulated(response) {
            var ddlGLProject = '<%=ddlGLProject.ClientID%>'
            $("#" + ddlGLProject).attr("disabled", false);
            PopulateControl(response.d, $("#<%=ddlGLProject.ClientID %>"), $("#<%=CommonDropDownHiddenField.ClientID %>").val());
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

        //--------------------------Payment Method Edit Ending------------------------------------

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#tblRstBillSearch tbody tr").length;
            var srcFromDate = $("#<%=txtSrcFromDate.ClientID %>").val();
            var srcToDate = $("#<%=txtSrcToDate.ClientID %>").val();
            var billNo = $("#<%=txtBillNumber.ClientID %>").val();
            var customerInfo = $("#<%=txtCustomerInfo.ClientID %>").val();
            var remarks = $("#<%=txtSrcRemarks.ClientID %>").val();
            var costCenterId = $("#<%=ddlCostCenter.ClientID %>").val();
            PageMethods.SearchBillAndLoadGridInformation(srcFromDate, srcToDate, billNo, customerInfo, remarks, costCenterId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSuccess, OnFail);
            return false;
        }
        function OnSuccess(result) {
            
            $("#tblRstBillSearch tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#tblRstBillSearch tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#tblRstBillSearch tbody tr").length;

                if (totalRow % 2 == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:10%; cursor:pointer;\">" + gridObject.BillNumber + "</td>";
                tr += "<td align='left' style=\"width:60%; cursor:pointer;\">" + gridObject.CustomerName + "</td>";
                //tr += "<td align='left' style=\"width:30%; cursor:pointer;\">" + gridObject.Remarks + "</td>";
                tr += "<td align='left' style=\"width:10%; cursor:pointer;\">" + gridObject.GrandTotal + "</td>";

                if (gridObject.IsDayClosed == 0) {
                    var editBill = "<img  src='../Images/edit.png' onClick= \"javascript:return PerformEditBill('" + gridObject.BillId + "')\" alt='Edit Bill' Title='Edit Bill'  border='0'/>";
                    editBill = "";
                    var resettlementBill = "";

                    if (gridObject.CostCenterType == "Billing") {
                        resettlementBill = "&nbsp;&nbsp;<img id='preview' src='../Images/billresettlement.png' onClick= \"javascript:return BillReSettlementForBilling('" + gridObject.BillId + "','" + gridObject.CostCenterId + "')\" alt='Re Sattlement' Title='Bill Re-Settlement' border='0'/>";
                    }
                    else
                        if (gridObject.CostCenterType != "RetailPos") {
                            resettlementBill = "&nbsp;&nbsp;<img id='preview' src='../Images/billresettlement.png' onClick= \"javascript:return BillReSettlement('" + gridObject.BillId + "')\" alt='Re Sattlement' Title='Bill Re-Settlement' border='0'/>";
                        }

                    if (gridObject.BillStatus == "Active") {

                        if (gridObject.CheckOutDate == "") {

                            tr += "<td id='Cancel' align='centre' style=\"width:10%; cursor:pointer;\">";

                            if ($("#<%=hfIsUpdatePermission.ClientID %>").val() == "1") {
                                tr += editBill;
                                tr += resettlementBill;
                            }

                            if ($("#<%=hfIsDeletePermission.ClientID %>").val() == "1") {
                                tr += "&nbsp;&nbsp;<img  src='../Images/delete.png' onClick= \"javascript:return AddRemarks(" + gridObject.BillId + "," + gridObject.CostCenterId + ")\" alt='Cancel Bill' Title='Void'  border='0'/>";
                            }

                            tr += "&nbsp;&nbsp;<img id='preview' src='../Images/ReportDocument.png' onClick= \"javascript:return PerformBillPreviewAction(" + gridObject.BillId + ", '" + gridObject.CostCenterType + "')\" alt='Preview Bill' Title='Preview Bill' border='0'/>";

                            if ($("#<%=hfImageApprovedBySignature.ClientID %>").val() != "0") {
                                tr += "&nbsp;&nbsp;<img id='preview' src='../Images/ReportDocument.png' onClick= \"javascript:return PerformBillPreviewActionWithSignature(" + gridObject.BillId + ", '" + gridObject.CostCenterType + "')\" alt='Preview Bill with Signature' Title='Preview Bill with Signature' border='0'/>";
                            }
                            tr += "&nbsp;&nbsp;<img id='preview' src='../Images/detailsInfo.png' onClick= \"javascript:return LoadOthersDocumentUploader(" + gridObject.BillId + ") \" alt='Image' Title='Preview' border='0'/>";
                            tr += "</td>";
                        }
                        else {
                            tr += "<td id='Cancel' align='centre' style=\"width:10%; cursor:pointer;\">";
                            tr += "&nbsp;&nbsp;<img id='preview' src='../Images/ReportDocument.png' onClick= \"javascript:return PerformBillPreviewAction(" + gridObject.BillId + ", '" + gridObject.CostCenterType + "')\" alt='Preview Bill' Title='Preview Bill' border='0'/>";

                            if ($("#<%=hfImageApprovedBySignature.ClientID %>").val() != "0") {
                                tr += "&nbsp;&nbsp;<img id='preview' src='../Images/ReportDocument.png' onClick= \"javascript:return PerformBillPreviewActionWithSignature(" + gridObject.BillId + ", '" + gridObject.CostCenterType + "')\" alt='Preview Bill with Signature' Title='Preview Bill with Signature' border='0'/>";
                            }
                            tr += "&nbsp;&nbsp;<img id='preview' src='../Images/detailsInfo.png' onClick= \"javascript:return LoadOthersDocumentUploader(" + gridObject.BillId + ") \" alt='Image' Title='Preview' border='0'/>";
                            tr += "</td>";
                        }
                    }
                    else {
                        if (gridObject.CheckOutDate == "") {
                            tr += "<td align='centre' style=\"width:10%; cursor:pointer;\">";
                            if ($("#<%=hfIsUpdatePermission.ClientID %>").val() == "1") {
                                tr += editBill;
                                tr += resettlementBill
                            }
                            if ($("#<%=hfIsDeletePermission.ClientID %>").val() == "1") {
                                tr += "&nbsp;&nbsp;<img id='Active' src='../Images/select.png' onClick= \"javascript:return ActiveBillStatus(" + gridObject.BillId + "," + gridObject.CostCenterId + ")\" alt='Active Bill' Title='Active' border='0'/>";
                            }

                            tr += "&nbsp;&nbsp;<img id='preview' src='../Images/ReportDocument.png' onClick= \"javascript:return PerformBillPreviewAction(" + gridObject.BillId + ", '" + gridObject.CostCenterType + "')\" alt='Preview Bill' Title='Preview' border='0'/>";
                            tr += "&nbsp;&nbsp;<img id='preview' src='../Images/detailsInfo.png' onClick= \"javascript:return LoadOthersDocumentUploader(" + gridObject.BillId + ") \" alt='Image' Title='Preview' border='0'/>";

                            tr += "</td>";
                        }
                        else {
                            tr += "<td align='centre' style=\"width:10%; cursor:pointer;\">";
                            tr += "</td>";
                        }
                    }
                }
                else {
                    tr += "<td id='Cancel' align='centre' style=\"width:10%; cursor:pointer;\"><img id='preview' style='margin-right: 10px' src='../Images/ReportDocument.png' onClick= \"javascript:return PerformBillPreviewAction(" + gridObject.BillId + ", '" + gridObject.CostCenterType + "')\" alt='Preview Bill' Title='Preview' border='0'/>";

                    tr += "&nbsp;&nbsp;<img id='preview' src='../Images/detailsInfo.png' onClick= \"javascript:return LoadOthersDocumentUploader(" + gridObject.BillId + ") \" alt='Image' Title='Preview' border='0'/>";

                }

                tr += "</tr>"

                $("#tblRstBillSearch tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
            
            return false;
        }

        function LoadOthersDocumentUploader(billId) {
            console.log('working');
            
            $("#ContentPlaceHolder1_txtBillId").val(billId);
            var path = "/POS/Images/Documents/";
            var category = "Employee Other Documents";
            var iframeid = 'frmPrint';
            //var url = "/HMCommon/FileUploadTest.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            var url = "/Common/FileUpload.aspx?Path=" + path + "&OwnerId=" + billId + "&Category=" + category;
            console.log(url);
            document.getElementById(iframeid).src = url;

            ShowUploadedOthersDocument(billId);

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
            return false;
        }

        function UploadComplete() {
            console.log("Upload Completed Now");
            var id = $("#ContentPlaceHolder1_txtBillId").val();
            if (id != 0) {
                //ShowUploadedDocument(id);
                //ShowUploadedSignature(id);
                ShowUploadedOthersDocument(id);
            }
        }

        function ShowUploadedOthersDocument(id) {            
            PageMethods.GetUploadedDocumentsByWebMethod(id, "Employee Other Documents", OnGetUploadedOthersDocumentByWebMethodSucceeded, OnGetUploadedOthersDocumentByWebMethodFailed);
            return false;
        }

        function ShowDocument(path, name) {
            console.log("show document");
            var iframeid = 'fileIframe';
            document.getElementById(iframeid).src = path;
            $("#ShowDocumentDiv").dialog({
                autoOpen: true,
                modal: true,
                width: "82%",
                height: 1000,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Document - " + name,
                show: 'slide'
            });
            return false;
        }
        function OnGetUploadedOthersDocumentByWebMethodSucceeded(result) {
            console.log("table");
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
                DocTable += "<td align='left' style='width: 50%;cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + result[row].Name + "</td>";

                if (result[row].Path != "") {
                    if (result[row].Extention == ".jpg" || result[row].Extention == ".png") {
                        imagePath = "<img src='" + result[row].Path + result[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    }
                    else {
                        imagePath = "<img src='" + result[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    }
                }
                else
                    imagePath = "";

                DocTable += "<td align='left' style='width: 30%'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + imagePath + "</td>";

                DocTable += "<td align='left' style='width: 20%'>";
                DocTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteDoc('" + result[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                DocTable += "</td>";
                DocTable += "</tr>";
            }
            DocTable += "</table>";

            docc = DocTable;

            $("#ContentPlaceHolder1_DocumentInfo").html(DocTable);
            $('#OthersDocDiv').html(result);
            return false;
        }

        function DeleteDoc(docId, rowIndex) {
            if (confirm("Do you want to delete?")) {
                <%--var deletedDoc = $("#<%=hfGuestDeletedDoc.ClientID %>").val();

                if (deletedDoc != "")
                    deletedDoc += "," + docId;
                else
                    deletedDoc = docId;

                $("#<%=hfGuestDeletedDoc.ClientID %>").val(deletedDoc);--%>
                PageMethods.DeleteDoc(docId, OnDeleteDocSucceeded, OnDeleteDocFailed);

                $("#trdoc" + rowIndex).remove();

                return false;

            }
        }

        function OnDeleteDocSucceeded(result) {
            if (result == true) {
                toastr.success("Delete operation successfully done.");
            }
            else {
                toastr.error("Delete operation unsuccessfull.");
            }
        }

        function OnDeleteDocFailed(error) {
        }


        function OnGetUploadedOthersDocumentByWebMethodFailed(error) {
            toastr.error(error.get_message());
        }

        function OnFail(error) {
            toastr.error(error.get_message());
        }

        function PerformBillPreviewAction(billId, costCenterType, withSignature) {
            var url = "";
            var popup_window = "Print Preview";

            if (costCenterType == "RetailPos") {
                url = "/POS/Reports/frmReportBillForRetailPos.aspx?billID=" + billId;
            }
            else {
                url = "/POS/Reports/frmReportBillInfo.aspx?billID=" + billId;
            }

            window.open(url, popup_window, "width=750,height=680,left=300,top=50,resizable=yes");
        }

        function PerformBillPreviewActionWithSignature(billId, costCenterType) {
            var url = "";
            var popup_window = "Print Preview";

            if (costCenterType == "RetailPos") {
                url = "/POS/Reports/frmReportBillForRetailPos.aspx?US=Y&billID=" + billId;
            }
            else {
                url = "/POS/Reports/frmReportBillInfo.aspx?US=Y&billID=" + billId;
            }

            window.open(url, popup_window, "width=750,height=680,left=300,top=50,resizable=yes");
        }

        function CancelBillStatus() {
            var billId = $("#<%=txtHiddenBillId.ClientID %>").val();
            var costcenterId = $("#<%=hfCostcenterId.ClientID %>").val();

            var remarks = $("#<%=txtRemarks.ClientID %>").val();
            billStatus = "Void";
            if (remarks == "") {
                toastr.warning("Please provide reason.");
            }
            else {
                PageMethods.UpdateRestaurantBill(billStatus, remarks, billId, costcenterId, OnUpdateSuccess, OnUpdateFailure);
            }
        }

        function ActiveBillStatus(billId, costcenterId) {
            var isActive = confirm('Do you want to active this bill?');
            if (isActive) {
                var billStatus = "Active";
                PageMethods.UpdateRestaurantBill(billStatus, "", billId, costcenterId, OnUpdateSuccess, OnUpdateFailure);
            }
        }

        function OnUpdateSuccess(result) {
            var possiblePath = "frmBillSearch.aspx";
            window.location = possiblePath;
        }

        function OnUpdateFailure(error) {
        }

        function AddNewItem(value) {
            SetSelectedItem(value);
            $("#<%=txtReportId.ClientID %>").val(value);
            $("#TouchKeypad").dialog({
                width: 412,
                height: 210,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "", ////TODO add title
                show: 'slide'
            });
            return false;
        }
        function SetSelectedItem(dealId) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/GeneralLedger/frmVoucherSearch.aspx/SetSelected",
                data: "{'DealId':'" + dealId + "'}",
                dataType: "json",
                success: OnSetSelected,
                error: function (result) {
                    //alert("Error");
                }
            });
        }
        function OnSetSelected(response) {
            $("#<%=ddlChangeStatus.ClientID %>").val(response.d);
        }

        function PerformDeleteAction(actionId) {

            var answer = confirm("Do you want to delete this record?")
            if (answer) {

                PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            }
            return false;
        }

        function OnDeleteObjectSucceeded(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
        }

        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function PerformClearAction() {
            $("#<%=txtSrcFromDate.ClientID %>").val('');
            $("#<%=txtSrcToDate.ClientID %>").val('');
            MessagePanelHide();
            return false;
        }

        function CheckAllCheckBox() {
            $('.chkBox_Approve input[type=checkbox]').each(function () {
                $(this).prop("checked", true);
            });
        }

        function UnCheckAllCheckBox() {
            $('.chkBox_Approve input[type=checkbox]').each(function () {
                $(this).prop("checked", false);
            });
        }

        function AddRemarks(hiddenBillId, costCenterId) {
            if (!confirm("Do you want to delete?")) {
                return false;
            }
            $("#<%=txtHiddenBillId.ClientID %>").val(hiddenBillId);
            $("#<%=hfCostcenterId.ClientID %>").val(costCenterId);
            $("#<%=txtRemarks.ClientID %>").val();

            $("#RemarksDiv").dialog({
                autoOpen: true,
                modal: true,
                width: 725,
                closeOnEscape: true,
                resizable: false,
                title: "Bill Void Information",
                show: 'slide'
            });

            return false;
        }

        function PerformEditBill(billId) {

            $("#<%=txtBillId.ClientID %>").val(billId);
            PageMethods.GetRestaurantBillPayment(billId, OnLoadEditDataSucceeded, OnLoadEditDataFailed);
        }

        function OnLoadEditDataSucceeded(result) {
            billPaymmentDetails = result;

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
                strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformGuestPaymentDetailDelete(\"" + result[rowCounter].TransactionType + "\", " + result[rowCounter].PaymentId + ")' alt='Delete Information' border='0' />";
                strTable += "</td></tr>";
            }
            strTable += "</tbody></table>";

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
                title: "", ////TODO add title
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

        function PerformPaymentPosting() {

            var totalPayment = 0.00, grandTotal = 0.00;

            $("#ReservationDetailGrid tbody tr").each(function () {
                totalPayment += parseFloat($(this).find("td:eq(2)").text());
            });

            grandTotal = parseFloat($("#txtGrandTotal").text());

            if (totalPayment > grandTotal) { toastr.info("Payment Amount Cannot Greater Than Grand Total."); return false; }
            if (totalPayment < grandTotal) { toastr.info("Payment Amount Cannot Less Than Grand Total."); return false; }

            var txtBillId = $("#<%=txtBillId.ClientID %>").val();
            var hfDeletedPaymentForPaymentId = $("#<%=hfDeletedPaymentForPaymentId.ClientID %>").val();
            var hfDeletedPaymentForTransferedPaymentId = $("#<%=hfDeletedPaymentForTransferedPaymentId.ClientID %>").val();
            var hfDeletedPaymentForAchievementPaymentId = $("#<%=hfDeletedPaymentForAchievementPaymentId.ClientID %>").val();

            PageMethods.PerformPaymentPostingByWebMethod(txtBillId, hfDeletedPaymentForPaymentId, hfDeletedPaymentForTransferedPaymentId, hfDeletedPaymentForAchievementPaymentId, OnPaymentPostingSuccess, OnPaymentPostingFailure);

        }

        function OnPaymentPostingSuccess(result) {
            if (result == "1") {
                toastr.success("Save Operation Successfull.");
                $("#<%=txtBillId.ClientID %>").val("");
                $("#<%=hfDeletedPaymentForPaymentId.ClientID %>").val("");
                $("#<%=hfDeletedPaymentForTransferedPaymentId.ClientID %>").val("");
                $("#<%=hfDeletedPaymentForAchievementPaymentId.ClientID %>").val("");
                GridPaging(1, 1);

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

        function BillReSettlement(billId) {
            if (!confirm("Do you want to Bill Re-Settlement?")) {
                return false;
            }
            PageMethods.BillResettlement(billId, OnBillResettlementSucceeded, OnBillResettlementFailed);
        }
        function BillReSettlementForBilling(billId, CostCenterId) {
            if (!confirm("Do you want to Bill Re-Settlement?")) {
                return false;
            }
            window.location = "/POS/BillingResettlement.aspx?cid=" +CostCenterId +"&billingBillID=" + billId;
        }
        function OnBillResettlementSucceeded(result) {
            if (result.IsSuccess == true) {
                window.location = result.RedirectUrl;
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnBillResettlementFailed() { }


    </script>
    <asp:HiddenField ID="hfIsSavePermission" runat="server" />
    <asp:HiddenField ID="hfIsUpdatePermission" runat="server" />
    <asp:HiddenField ID="hfImageApprovedBySignature" runat="server" />
    <asp:HiddenField ID="hfIsDeletePermission" runat="server" />
    <asp:HiddenField ID="hfIsViewPermission" runat="server" />
    <asp:HiddenField ID="txtReportId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtHiddenBillId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCostcenterId" runat="server"></asp:HiddenField>

    <div id="TouchKeypad" style="display: none;">
        <div id="Div1" class="panel panel-default">
            <div class="panel-heading">
                Change Voucher Status
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblChangeStatus" runat="server" class="control-label" Text="Change Status"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlChangeStatus" runat="server" CssClass="form-control" TabIndex="1">
                                <asp:ListItem>Pending</asp:ListItem>
                                <asp:ListItem>Checked</asp:ListItem>
                                <asp:ListItem>Approved</asp:ListItem>
                                <asp:ListItem>Cancel</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Bill Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="Search Date"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:HiddenField ID="txtDealId" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hfIsDayClosed" runat="server"></asp:HiddenField>
                        <asp:TextBox ID="txtSrcFromDate" CssClass="form-control" placeholder="From Date" runat="server" TabIndex="3"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtSrcToDate" CssClass="form-control" placeholder="To Date" runat="server" TabIndex="3"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblBillNumber" runat="server" class="control-label" Text="Bill Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtBillNumber" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblCostCenter" runat="server" class="control-label" Text="Cost Center"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlCostCenter" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>                    
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label8" runat="server" class="control-label" Text="Customer Information"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtCustomerInfo" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label10" runat="server" class="control-label" Text="Remarks"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtSrcRemarks" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <button type="button" id="btnSearch" class="btn btn-primary btn-sm">
                            Search</button>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary btn-sm"
                            OnClientClick="javascript: return PerformClearAction();" TabIndex="7" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default" style="display: none;">
        <div class="panel-heading">
            Search Information
        </div>
        <div class="panel-body">
            <table id='tblRstBillSearch' class="table table-bordered table-condensed table-responsive">
                <colgroup>
                    <col style="width: 10%;" />
                    <col style="width: 60%;" />
                    <col style="width: 10%;" />
                    <col style="width: 20%;" />
                </colgroup>
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                        <td>Bill Number
                        </td>
                        <td>Customer Name
                        </td>
                        <td>Bill Total
                        </td>
                        <td>Action
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
    <div id="RemarksDiv" style="display: none;" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label7" runat="server" class="control-label required-field" Text="Reason"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                            TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <div class='NumericItemDiv'>
                            <button class="btn btn-primary btn-sm" type="button" onclick='javascript:return CancelBillStatus()'>
                                Ok</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="txtBillId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtBillIdForInvoicePreview" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsRestaurantIntegrateWithFrontOffice" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtCardValidation" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDeletedPaymentForPaymentId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDeletedPaymentForTransferedPaymentId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDeletedPaymentForAchievementPaymentId" runat="server"></asp:HiddenField>
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
                                <asp:ListItem Value="Other Room">Guest Room</asp:ListItem>
                                <asp:ListItem>Employee</asp:ListItem>
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
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Card Type"></asp:Label>
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
                                    <asp:Label ID="Label2" runat="server" class="control-label" Text="Expiry Date"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtExpireDate" TabIndex="10" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="Label3" runat="server" class="control-label" Text="Card Holder Name"></asp:Label>
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
                                <asp:DropDownList ID="ddlRoomId" TabIndex="12" runat="server" CssClass="form-control"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlRoomId_SelectedIndexChanged">
                                </asp:DropDownList>
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
                            <asp:Label ID="Label4" runat="server" class="control-label" Text="Account Head"></asp:Label>
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
                        <asp:Label ID="Label6" runat="server" class="control-label" Text="Grand Total"></asp:Label>
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
                <asp:Button ID="btnPaymentPosting" runat="server" ClientIDMode="Static" Text="Save"
                    CssClass="btn btn-primary btn-sm" OnClientClick="javascript:return ValidateForm();"
                    TabIndex="23" OnClick="btnPaymentPosting_Click" />
                <button class="btn btn-primary btn-sm" type="button" onclick='javascript:return PerformPaymentPosting()'>
                    Save Payment</button>
            </div>
        </div>
    </div>

     <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100px" runat="server"
            clientidmode="static" scrolling="yes">

        </iframe>
         <div class="block-body collapse in" style="width: 100%; height: 150px;">
                        <div class="form-group">
                            <div id="DocumentInfo" runat="server" class="col-md-12">
                            </div>
                        </div>
                    </div>
    </div>
    <div id="popUpSignature" style="display: none;">
        <iframe id="Iframe1" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="popUpDocument" style="display: none;">
        <iframe id="Iframe2" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>

    <div id="ShowDocumentDiv" style="display: none;">
        <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
</asp:Content>
