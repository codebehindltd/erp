<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmNutrientRequiredValues.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.frmNutrientRequiredValues" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">

        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;

        $(document).ready(function () {

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            
            $("#ContentPlaceHolder1_ddlItemName").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlNutrient").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#myTabs").tabs();
        });       

        function AddNutrientRequiredValue() {
            if ($("#ContentPlaceHolder1_ddlNutrient").val() == "0") {
                toastr.warning("Please Select Nutrient.");
                $("#ContentPlaceHolder1_ddlNutrient").focus();
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtRequiredValue").val() == "") {
                toastr.warning("Please Give Required Value.");
                $("#ContentPlaceHolder1_txtRequiredValue").focus();
                return false;
            }
            AddNutrientRequiredValueTable();
            //ClearAfterNutrientRequiredValueAdded();
        }

        function AddNutrientRequiredValueTable() {
            var nutrientName = $("#ContentPlaceHolder1_ddlNutrient option:selected").text();
            var nutrientId = $("#ContentPlaceHolder1_ddlNutrient option:selected").val();
            var requiredValue = $("#ContentPlaceHolder1_txtRequiredValue").val();

            if (!IsNutrientRequiredValueExists(nutrientId)) {
                debugger;
                if ($("#ContentPlaceHolder1_hfEditNutrientRequiredValue").val() == 0) {
                    var tr = "";

                    tr += "<tr>";
                    tr += "<td style='width:40%;'>" + nutrientName + "</td>";
                    tr += "<td style='width:40%;'>" + requiredValue + "</td>";
                    tr += "<td style=\"width:15%;\">";
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return EditNutrientRequiredValueItem('" + nutrientId + "','" + requiredValue + "')\" alt='Edit'  title='Edit' border='0' />";
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'DeleteNutrientRequiredValueItem(this)' ><img alt='Delete' src='../Images/delete.png' title='Delete' /></a>";
                    tr += "</td>";

                    tr += "<td style='display:none;'>" + nutrientId + "</td>";

                    tr += "</tr>";

                    $("#NutrientRequiredValuesTbl tbody").prepend(tr);
                    tr = "";
                    ClearAfterNutrientRequiredValueAdded();
                }
            }
            else {
                $("#NutrientRequiredValuesTbl tr").each(function () {
                    var currentNutrientId = $(this).find("td").eq(3).html();
                    if ($("#ContentPlaceHolder1_hfEditNutrientRequiredValue").val() == 1) {
                        if (currentNutrientId == nutrientId) {
                            $(this).find("td").eq(0).html(nutrientName);
                            $(this).find("td").eq(1).html(requiredValue);
                            $(this).find("td").eq(3).html(nutrientId);
                        }
                    }
                });
                if ($("#ContentPlaceHolder1_hfEditNutrientRequiredValue").val() == 1) {
                    ClearAfterNutrientRequiredValueAdded();
                }
            }
        }

        function ClearAfterNutrientRequiredValueAdded() {
            $("#ContentPlaceHolder1_ddlNutrient").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtRequiredValue").val("");
            $("#ContentPlaceHolder1_hfEditNutrientRequiredValue").val(0);
        }

        function IsNutrientRequiredValueExists(nutrientId) {
            var IsDuplicate = false;
            $("#NutrientRequiredValuesTbl tr").each(function (index) {
                
                if (index !== 0 && !IsDuplicate) {
                    var nutrientIdValueInTable = $(this).find("td").eq(3).html();
                    console.log(nutrientIdValueInTable);

                    var IsNutrientIdFound = nutrientIdValueInTable.indexOf(nutrientId) > -1;
                    if (IsNutrientIdFound) {
                        if ($("#ContentPlaceHolder1_hfEditNutrientRequiredValue").val() == 1) {
                            toastr.success('Nutrient Required Value Updated Successfully.');
                            IsDuplicate = true;
                        }
                        else {
                            toastr.warning('Nutrient Required Value Already Added.');
                            IsDuplicate = true;
                            return true;
                        }
                    }
                }
            });
            return IsDuplicate;
        }

        function ValidationBeforeSave() {
            var rowCountAT = $('#NutrientRequiredValuesTbl tbody tr').length;
            if (rowCountAT == 0) {
                toastr.warning('Add at least one Nutrient Required Value.');
                $("#ContentPlaceHolder1_ddlNutrient").focus();
                return false;
            }


            var transactionType = "", companyName = "", companyId = "0", referenceName = "", registrationNumber = "",
                clientName = "", mobileNumber = "", email = "", address = "", issueDate = "", ticketTypeId = "",
                ticketType = "", airlineName = "", airlineId = "0", flightDate = "", returnDate = "",
                ticketNumber = "", pnrNumber = "", ticketValue = 0, invoiceAmount = 0, airlineAmount = 0, routePath = "", remarks = "", paymentModeId = "",
                paymentModeName = "", currencyTypeId = "", currencyType = "", receiveAmount = 0, cardTypeId = "", cardType = "", cardNumber = "", bankId = "0", bankName = "", chequeNumber = "";

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
                clientName = $.trim($(item).find("td:eq(0)").text());
                airlineName = $(item).find("td:eq(1)").text();
                issueDate = $.trim($(item).find("td:eq(2)").text());
                if (issueDate != '') {
                    issueDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(issueDate, innBoarDateFormat);
                }
                flightDate = $.trim($(item).find("td:eq(3)").text());
                if (flightDate != '') {
                    flightDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(flightDate, innBoarDateFormat);
                }
                invoiceAmount = $.trim($(item).find("td:eq(4)").text());

                mobileNumber = $.trim($(item).find("td:eq(6)").text());
                email = $.trim($(item).find("td:eq(7)").text());
                address = $.trim($(item).find("td:eq(8)").text());
                ticketTypeId = $.trim($(item).find("td:eq(9)").text());
                ticketType = $.trim($(item).find("td:eq(10)").text());
                airlineId = $.trim($(item).find("td:eq(11)").text());
                returnDate = $.trim($(item).find("td:eq(12)").text());
                if (returnDate != '') {
                    returnDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(returnDate, innBoarDateFormat);
                }
                ticketNumber = $.trim($(item).find("td:eq(13)").text());
                pnrNumber = $.trim($(item).find("td:eq(14)").text());
                airlineAmount = $.trim($(item).find("td:eq(15)").text());
                routePath = $.trim($(item).find("td:eq(16)").text());
                remarks = $.trim($(item).find("td:eq(17)").text());
                ticketValue = $.trim($(item).find("td:eq(18)").text());

                airlineAmount = airlineAmount != "" ? parseFloat(airlineAmount) : 0.00;

                AddedSingleTicketInfo.push({
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
                    Remarks: remarks
                });
            });

            var AddedPaymentInfo = [], EditedPaymentInfo = [];

            $("#NutrientRequiredValuesTbl tbody tr").each(function (index, item) {

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
                    ChequeNumber: chequeNumber
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

            PageMethods.SaveAirlineTicketInfo(AirTicketMasterInfo, AddedSingleTicketInfo, AddedPaymentInfo, deletedNutrientRequiredValueList, OnSaveAirlineTicketInfoSucceeded, OnSaveAirlineTicketInfoFailed);

            return false;
        }
        function OnSaveAirlineTicketInfoSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
                deletedNutrientRequiredValueList = [];
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            return false;
        }
        function OnSaveAirlineTicketInfoFailed(error) {
            toastr.error(error.get_message());
        }
                
        function EditNutrientRequiredValueItem(nutrientId, requiredValue) {
            if (!confirm("Do you want to edit item?")) {
                return false;
            }
            $("#ContentPlaceHolder1_hfEditNutrientRequiredValue").val(1);
            $("#ContentPlaceHolder1_ddlNutrient").val(nutrientId).trigger('change');
            $("#ContentPlaceHolder1_txtRequiredValue").val(requiredValue);
        }
        var deletedNutrientRequiredValueList = [];
        function DeleteNutrientRequiredValueItem(control) {
            if (!confirm("Do you want to delete this item?")) { return false; }

            var tr = $(control).parent().parent();
            let nutrientId = $(tr).find("td").eq(4).html();
            deletedNutrientRequiredValueList.push(parseInt(nutrientId, 10));
            $(tr).remove();
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
            $("#NutrientRequiredValuesTbl tbody").html("");
            $("#ContentPlaceHolder1_hfCompanyId").val(0);
            $("#ContentPlaceHolder1_hfCompanySearchId").val(0);
            $("#ContentPlaceHolder1_hfReferenceIdForCompany").val(0);
            $("#ContentPlaceHolder1_hfReferenceIdForWalkIn").val(0);
            $("#ContentPlaceHolder1_hfbankId").val(0);
            $("#ContentPlaceHolder1_hfRegistrationNumber").val(0);
            $("#ContentPlaceHolder1_hfTicketMasterId").val(0);
            $("#ContentPlaceHolder1_hftotalForPaymentInfos").val(0);
            $("#ContentPlaceHolder1_hftotalForTicketInfos").val(0);

            $("#ContentPlaceHolder1_txtTotalInvoiceAmount").val("");
            $("#ContentPlaceHolder1_txtTotalPaymentAmount").val("");
            $("#btnSave").val("Save");
            ClearAfterNutrientRequiredValueAdded();
        }
        function PerformClearActionWithConfirmation() {

            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            PerformClearAction();
        }

        function SearchTicketInformation(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#TicketInformationGrid tbody tr").length;
            var fromDate = null, toDate = null, invoiceNumber = "", companyName = "", referenceName = "";
            fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            toDate = $("#ContentPlaceHolder1_txtToDate").val();
            invoiceNumber = $("#ContentPlaceHolder1_txtInvoiceNumber").val();
            companyName = $("#ContentPlaceHolder1_txtCompanyName").val();
            referenceName = $("#ContentPlaceHolder1_txtRefName").val();

            if (fromDate != "")
                fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');

            if (toDate != "")
                toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            $("#GridPagingContainer ul").html("");
            $("#TicketInformationGrid tbody").html("");

            if (pageNumber < 0)
                pageNumber = 1;

            PageMethods.SearchTicketInformation(fromDate, toDate, invoiceNumber, companyName, referenceName,
                gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchTicketInformationSucceed, OnSearchTicketInformationFailed);

            return false;
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

        function PaymentMethodInformationEdit(result) {
            $.each(result, function (count, obj) {

                var tr = "";

                tr += "<tr>";
                tr += "<td style='width:35%;'>" + obj.PaymentMode + "</td>";
                tr += "<td style='width:25%;'>" + obj.BankName + "</td>";
                tr += "<td style='width:25%;'>" + obj.ReceiveAmount + "</td>";
                tr += "<td style=\"width:15%;\">";
                tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return EditPaymentInfoItem('" + obj.PaymentModeId + "','" + obj.PaymentMode + "','" + obj.BankId + "','" + obj.BankName + "','" + obj.ReceiveAmount + "','" + obj.CurrencyTypeId + "','" + obj.CurrencyType + "','" + obj.CardTypeId + "','" + obj.CardType + "','" + obj.CardNumber + "','" + obj.ChequeNumber + "')\" alt='Edit'  title='Edit' border='0' />";
                tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'DeleteNutrientRequiredValueItem(this)' ><img alt='Delete' src='../Images/delete.png' title='Delete' /></a>";
                tr += "</td>";

                tr += "<td style='display:none;'>" + obj.PaymentModeId + "</td>";
                tr += "<td style='display:none;'>" + obj.CurrencyTypeId + "</td>";
                tr += "<td style='display:none;'>" + obj.CurrencyType + "</td>";
                tr += "<td style='display:none;'>" + obj.CardType + "</td>";
                tr += "<td style='display:none;'>" + obj.CardTypeId + "</td>";
                tr += "<td style='display:none;'>" + obj.CardNumber + "</td>";
                tr += "<td style='display:none;'>" + obj.BankId + "</td>";
                tr += "<td style='display:none;'>" + obj.ChequeNumber + "</td>";

                tr += "</tr>";

                $("#NutrientRequiredValuesTbl tbody").prepend(tr);
                var totalAmount = 0;
                $("#NutrientRequiredValuesTbl tr").each(function () {
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

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchTicketInformation(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }

        function GoToAirlineTicketInfoPage() {
            window.location = "/AirTicketing/frmAirlineTicketInfo.aspx";
            return false;
        }
    </script>
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanySearchId" runat="server" Value="0" />
    <asp:HiddenField ID="hfReferenceIdForCompany" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfReferenceIdForWalkIn" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfbankId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfRegistrationNumber" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfTicketMasterId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfEditNutrientRequiredValue" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfStopAddingExistingPayment" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hftotalForPaymentInfos" runat="server" Value="0" />
    <asp:HiddenField ID="hftotalForTicketInfos" runat="server" Value="0" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Nutrient Required Values</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Nutrient Required Values</a></li>
        </ul>
        <div id="tab-1">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblItemName" runat="server" class="control-label required-field" Text="Item Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlItemName" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div id="NutrientRequiredValue" class="childDivSection">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Nutrient Required Value
                    </div>
                    <div class="panel-body childDivSectionDivBlockBody">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblNutrient" runat="server" class="control-label required-field" Text="Nutrient"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlNutrient" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblRequiredValue" runat="server" class="control-label required-field" Text="Required Value"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtRequiredValue" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="padding-left: 10px;">
                                <input id="btnAddNutrientRequiredValue" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" onclick="AddNutrientRequiredValue()" />
                                <input id="btnCancelPayment" type="button" value="Cancel" onclick="ClearAfterNutrientRequiredValueAdded()" class="TransactionalButton btn btn-primary btn-sm" />
                            </div>
                            <div id="NutrientRequiredValues" style="overflow-y: scroll;">
                                <table id="NutrientRequiredValuesTbl" class="table table-bordered table-condensed table-hover">
                                    <thead>
                                        <tr>
                                            <th style="width: 40%;">Nutrient</th>
                                            <th style="width: 40%;">Required Value</th>
                                            <th style="width: 20%;">Action</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                    <tfoot></tfoot>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
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
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Nutrient Required Values
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblNutrientSearch" runat="server" class="control-label" Text="Nutrient"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlNutrientSearch" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-large" value="Search" onclick="SearchTicketInformation(1, 1)" />
                                <input type="button" id="btnSearchCancel" class="TransactionalButton btn btn-primary btn-large" value="Clear" onclick="ClearSearch()" />
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
                    <table id="NutrientRequiredValuesGrid" class="table table-bordered table-condensed table-responsive">
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
