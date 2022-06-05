<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmBanquetReservationNew.aspx.cs" EnableEventValidation="false" Inherits="HotelManagement.Presentation.Website.Banquet.frmBanquetReservationNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .mycheckbox input[type="checkbox"] {
            margin-right: 10px;
            padding-top: 5px;
        }
    </style>
    <script type="text/javascript">

        var minCheckInDate = "", minCheckOutDate = "";
        var DayDuration = new Array();
        var deleteObj = [];
        var editObj = [];
        var categoryWiseDiscount = new Array();
        var categoryWiseDiscountDeleted = new Array();
        var savedCategoryWiseDiscount = new Array();

        $(document).ready(function () {

            setTimeout(CalculateDiscountAmount, 3000);

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Banquet Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Banquet Reservation</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            $("#ContentPlaceHolder1_lblGrandTotal").text("Grand Total");
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            minCheckInDate = $("#<%=hfMinCheckInDate.ClientID %>").val();

            CommonHelper.ApplyDecimalValidation();
            CommonHelper.AutoSearchClientDataSource("txtCompanyId", "ContentPlaceHolder1_ddlCompanyId", "ContentPlaceHolder1_ddlCompanyId");
            CommonHelper.AutoSearchClientDataSource("txtItemId", "ContentPlaceHolder1_ddlItemId", "ContentPlaceHolder1_ddlItemId");

            $("#ContentPlaceHolder1_txtReservationDateFrom").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                minDate: minCheckInDate,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtReservationDateTo").datepicker("option", "minDate", selectedDate);
                    $("#ContentPlaceHolder1_txtPartyDate").datepicker("option", "minDate", selectedDate);
                    $("#ContentPlaceHolder1_txtPartyDateForItem").datepicker("option", "minDate", selectedDate);

                    var strDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtReservationDateFrom").val(), '/');
                    minCheckOutDate = GetStringFromDateTime(CommonHelper.DaysAdd(strDate, 1));

                    $("#ContentPlaceHolder1_txtReservationDateTo").datepicker("option", {
                        minDate: minCheckOutDate
                    });

                    if ($("#ContentPlaceHolder1_txtReservationDateTo").val() != "") {
                        var d = CommonHelper.DateDifferenceInDays(CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtReservationDateFrom").val(), '/'), CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtReservationDateTo").val(), '/'));
                        $("#txtReservationDuration").val(d + 1);
                    }
                    else
                        $("#txtReservationDuration").val("");
                }
            });

            $("#ContentPlaceHolder1_txtReservationDateTo").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                minDate: minCheckOutDate,
                onClose: function (selectedDate) {

                    DayDuration = new Array();

                    $("#ContentPlaceHolder1_txtPartyDate").datepicker("option", "maxDate", selectedDate);
                    $("#ContentPlaceHolder1_txtPartyDateForItem").datepicker("option", "maxDate", selectedDate);
                    $("#ContentPlaceHolder1_txtReservationDateFrom").datepicker("option", "maxDate", selectedDate);

                    if ($("#ContentPlaceHolder1_txtReservationDateTo").val() != "") {

                        var strartDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtReservationDateFrom").val(), '/');
                        var dateDuration = $("#ContentPlaceHolder1_txtReservationDateFrom").val();

                        var i = 0;
                        var d = CommonHelper.DateDifferenceInDays(CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtReservationDateFrom").val(), '/'), CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtReservationDateTo").val(), '/'));
                        d = d + 1;

                        $("#txtReservationDuration").val(d);

                        for (i = 0; i < d; i++) {

                            DayDuration.push({
                                PartyDay: dateDuration,
                                DayCaption: 'Day ' + (i + 1) + ' (' + dateDuration + ')'
                            });

                            dateDuration = GetStringFromDateTime(CommonHelper.DaysAdd(strartDate, (i + 1)));
                        }
                        DayDurationFeed();
                    }
                    else
                        $("#txtReservationDuration").val("");
                }
            });

            $("#ContentPlaceHolder1_txtPartyDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#<%=txtProbableArrivalHour.ClientID %>").keypress(function (e) {
                //if the letter is not digit then display error and don't type anything
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    //display error message
                    toastr.warning("Digits Only");
                    $("#<%=txtProbableArrivalHour.ClientID %>").focus();
                    return false;
                }
            });

            $("#<%=txtProbableDepartureHour.ClientID %>").keypress(function (e) {
                //if the letter is not digit then display error and don't type anything
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    //display error message
                    toastr.warning("Digits Only");
                    $("#<%=txtProbableDepartureHour.ClientID %>").focus();
                    return false;
                }
            });

            $('#ContentPlaceHolder1_txtProbableArrivalHour').timepicker({ showMinutes: false, showPeriod: is12HourFormat });
            $('#ContentPlaceHolder1_txtProbableDepartureHour').timepicker({ showMinutes: false, showPeriod: is12HourFormat });

            if ($('#ContentPlaceHolder1_hfClassificationDiscountAlreadySave').val() != "") {
                categoryWiseDiscount = JSON.parse($('#ContentPlaceHolder1_hfClassificationDiscountAlreadySave').val());
                savedCategoryWiseDiscount = JSON.parse($('#ContentPlaceHolder1_hfClassificationDiscountAlreadySave').val());
            }

            var hfReservationId = $("#<%=hfReservationId.ClientID %>").val();
            if (hfReservationId > 0) {
                var banquetId = $("#<%=hfBanquetId.ClientID %>").val();
                PageMethods.GetBanquetInfoByCriteria(banquetId, LoadBanquetInfoSucceeded, LoadBanquetInfoFailed);
            }

            minCheckInDate = $("#<%=hfMinCheckInDate.ClientID %>").val();

            var ddlBanquetId = '<%=ddlBanquetId.ClientID%>'
            var ddlItemType = '<%=ddlItemType.ClientID%>'
            var itemType = $('#' + ddlItemType).val();
            var ddlItemId = '<%=ddlItemId.ClientID%>'
            var txtItemUnit = '<%=txtItemUnit.ClientID%>'
            var txtUnitPrice = '<%=txtUnitPrice.ClientID%>'
            var txtArriveDate = '<%=txtPartyDate.ClientID%>'
            var txtSearchArraiveDate = '<%=txtSearchArraiveDate.ClientID%>'
            var txtSearchDepartureDate = '<%=txtSearchDepartureDate.ClientID%>'

            $("#ContentPlaceHolder1_txtSearchArraiveDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtSearchDepartureDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtSearchDepartureDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtSearchArraiveDate").datepicker("option", "maxDate", selectedDate);
                }
            });

            $("#<%=ddlDiscountType.ClientID %>").change(function () {
                CalculateDiscountAmount();
            });

            $("#<%=txtNumberOfPersonAdult.ClientID %>").keypress(function (e) {
                //if the letter is not digit then display error and don't type anything
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    //display error message
                    toastr.warning("Digits Only");
                    $("#<%=txtNumberOfPersonAdult.ClientID %>").focus();
                    return false;
                }
            });

            $("#<%=txtNumberOfPersonChild.ClientID %>").keypress(function (e) {
                //if the letter is not digit then display error and don't type anything
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    //display error message
                    toastr.warning("Digits Only");
                    $("#<%=txtNumberOfPersonChild.ClientID %>").focus();
                    return false;
                }
            });

            var txtBanquetRate = '<%=txtBanquetRate.ClientID%>'
            $('#' + txtBanquetRate).blur(function () {
                if (isNaN($('#' + txtBanquetRate).val())) {
                    toastr.warning('Entered Banquet Rate is not in correct format.');
                    return;
                }

                $("#<%= hfBanquetRate.ClientID %>").val($('#' + txtBanquetRate).val());
                CalculateTotalPaiedNDueAmount();
            });

            var txtArriveDate = '<%=txtPartyDate.ClientID%>'
            $('#' + txtArriveDate).blur(function () {
                DuplicateDataChecking();
            });
            var txtProbableArrivalHour = '<%=txtProbableArrivalHour.ClientID%>'
            $('#' + txtProbableArrivalHour).blur(function () {
                DuplicateDataChecking();
            });
            var txtProbableDepartureHour = '<%=txtProbableDepartureHour.ClientID%>'
            $('#' + txtProbableDepartureHour).blur(function () {
                DuplicateDataChecking();
            });

            var chkIscomplementary = '<%=chkIscomplementary.ClientID%>'
            $('#' + chkIscomplementary).change(function () {
            });

            $('#' + txtUnitPrice).blur(function () {
                if ($('#' + chkIscomplementary).is(':checked')) {
                    $('#' + txtUnitPrice).val("0")
                }
            });

            if ($('#' + chkIscomplementary).is(':checked')) {
                $('#' + txtUnitPrice).val("0")
            }

            var txtItemUnit = '<%=txtItemUnit.ClientID%>'

            $('#' + ddlItemType).change(function () {
                var itemType = $('#' + ddlItemType).val();
                var txtHiddenItemId = '<%=txtHiddenItemId.ClientID%>'
                $('#' + txtHiddenItemId).val("");
                $('#' + txtItemUnit).val('');
                $('#' + txtUnitPrice).val('');
                LoadProductItem(itemType);

            });

            $('#' + ddlItemId).change(function () {
                var ddlItemId = '<%=ddlItemId.ClientID%>'
                var ddlItemId = $('#' + ddlItemId).val();
                var txtHiddenItemId = '<%=txtHiddenItemId.ClientID%>'
                $('#' + txtHiddenItemId).val(ddlItemId);
                LoadProductData(ddlItemId);
            });

            $('#' + ddlBanquetId).change(function () {
                DuplicateDataChecking();
            });

            $("#<%=txtContactEmail.ClientID %>").blur(function () {
                var contactEmailValue = $("#<%=txtContactEmail.ClientID %>").val().length;
                if (contactEmailValue > 0) {
                    var isCEmailValid = IsEmail($("#<%=txtContactEmail.ClientID %>").val());
                    if (isCEmailValid == true) {
                    }
                    else {
                        toastr.info("Email is not in correct format.");
                    }
                }
            });

            var ddlReserveMode = '<%=ddlReservationMode.ClientID%>'
            $("#" + ddlReserveMode).change(function () {
                var reserveMode = $("#<%=ddlReservationMode.ClientID %>").val();

                if (reserveMode == "Personal") {
                    $("#<%=lblName.ClientID %>").text('Contact Person');
                    $("#ContactPersonDiv").hide();
                    $("#<%=chkIsLitedCompany.ClientID %>").attr("Disabled", true);
                    $("#ListedCompany").hide();
                    $("#ReservedCompany").show();
                }
                else {
                    $("#ContactPersonDiv").show();
                    $("#<%=lblName.ClientID %>").text('Company Name');
                    $("#<%=chkIsLitedCompany.ClientID %>").attr("Disabled", false);
                    $("#ListedCompany").show();
                    $("#ReservedCompany").hide();
                }
            });

            $("#SearchPanel").hide();
            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });

            $("#btnCategoryWiseDiscountOK").click(function () {

                categoryWiseDiscount = new Array();

                $("#CategorizedDiscount tbody tr").each(function () {
                    if ($(this).find("td:eq(2)").find("input").is(':checked')) {
                        categoryWiseDiscount.push(
                        {
                            DiscountId: parseInt($.trim($(this).find("td:eq(1)").text()), 10),
                            CategoryId: parseInt($.trim($(this).find("td:eq(0)").text()), 10),
                            Category: $.trim($(this).find("td:eq(3)").text())
                        });
                    }
                    else {
                        if (parseInt($.trim($(this).find("td:eq(1)").text()), 10) != 0) {
                            categoryWiseDiscountDeleted.push(
                            {
                                DiscountId: parseInt($.trim($(this).find("td:eq(1)").text()), 10),
                                CategoryId: parseInt($.trim($(this).find("td:eq(0)").text()), 10),
                                Category: $.trim($(this).find("td:eq(3)").text())
                            });
                        }
                    }
                });

                CalculateDiscountAmount();
                $("#CategoryWiseDiscountContainer").dialog("close");
            });

            $("#btnCategoryWiseDiscountCancel").click(function () {
                $("#CategoryWiseDiscountContainer").dialog("close");
            });

            $("#ContentPlaceHolder1_ddlCompanyId").change(function () {
                var companyId = $("#<%=ddlCompanyId.ClientID %>").val();
                if (companyId != 0) {
                    PageMethods.GetAffiliatedCompany(companyId, GetAffiliatedCompanyObjectSucceeded, GetAffiliatedCompanyObjectFailed);
                    return false;
                }
            });

            $("#txtCompanyId").blur(function () {
                var companyId = $("#<%=ddlCompanyId.ClientID %>").val();
                if (companyId > 0) {
                    PageMethods.GetAffiliatedCompany(companyId, GetAffiliatedCompanyObjectSucceeded, GetAffiliatedCompanyObjectFailed);
                    return false;
                }
                else if (companyId == 0) {
                    toastr.warning("Please select a company.");
                    return false;
                }
                else {
                    toastr.warning("Please provide an enlisted company.");
                    return false;
                }
            });
        });

        function GetAffiliatedCompanyObjectSucceeded(result) {
            $("#<%=txtAddress.ClientID %>").val(result.CompanyAddress)
            $("#<%=txtContactPerson.ClientID %>").val(result.ContactPerson)
            $("#<%=txtContactEmail.ClientID %>").val(result.EmailAddress)
            $("#<%=txtPhoneNumber.ClientID %>").val(result.TelephoneNumber)
            $("#<%=txtContactPhone.ClientID %>").val(result.ContactNumber)
            return false;
        }
        function GetAffiliatedCompanyObjectFailed(error) {
            toastr.error(error.get_message());
        }

        //        function ToggleFieldVisibleForListedCompany(ctrl) {
        //            if ($(ctrl).is(':checked')) {
        //                $('#ReservedCompany').hide();
        //                $('#ListedCompany').show();
        //            }
        //            else {
        //                $('#ListedCompany').hide();
        //                $('#ReservedCompany').show();
        //            }
        //        }
        function ToggleFieldVisibleForListedCompany(ctrl) {

            if ($(ctrl).is(':checked')) {
                $('#ReservedCompany').hide();
                $('#ListedCompany').show();
            }
            else {
                $('#ListedCompany').hide();
                $('#ReservedCompany').show();
            }
        }

        function DuplicateDataChecking() {
            var banquetId = $.trim($("#<%=ddlBanquetId.ClientID %>").val());
            var startDate = $.trim($("#<%=txtPartyDate.ClientID %>").val());
            var arriveTime = $.trim($("#<%=txtProbableArrivalHour.ClientID %>").val());
            var departTime = $.trim($("#<%=txtProbableDepartureHour.ClientID %>").val());
            PageMethods.GetBanquetReservationInfoForDuplicateChecking(banquetId, startDate, arriveTime, departTime, OnGetBanquetReservationInfoForDuplicateCheckingSucceeded, OnGetBanquetReservationInfoForDuplicateCheckingFailed);
            return false;
        }

        function UpdateDiscountAmount() {
            var txtDiscountAmount = $('#<%=txtDiscountAmount.ClientID%>').val();
            var hfTotalAmount = '<%=hfTotalAmount.ClientID%>'
            var hfDiscountedAmount = '<%=hfDiscountedAmount.ClientID%>'
            var txtDiscountedAmount = '<%=txtDiscountedAmount.ClientID%>'
            var ddlDiscountType = '<%=ddlDiscountType.ClientID%>'

            var discountType = $('#' + ddlDiscountType).val();
            var unitPrice = 0.0, roomRate = 0.0;

            if ($('#' + hfTotalAmount).val() != "")
                unitPrice = parseFloat($('#' + hfTotalAmount).val());

            if (txtDiscountAmount == "") {
                txtDiscountAmount = 0;
            }

            var discount = 0.0;
            if (discountType == "Fixed") {
                discount = parseFloat(txtDiscountAmount);
                unitPrice = unitPrice - discount;
            }
            else {
                discount = parseFloat(txtDiscountAmount);
                unitPrice = unitPrice - ((unitPrice * discount) / 100);
            }

            $('#' + hfDiscountedAmount).val(unitPrice.toFixed(2));
            $('#' + txtDiscountedAmount).val(unitPrice.toFixed(2));
            TotalRoomRateVatServiceChargeCalculation();
        }

        function AddItemDetailsInfo(id, reservationId) {

            var itemTypeId = $("#<%=ddlItemType.ClientID %>").val();
            var itemId = $("#<%=ddlItemId.ClientID %>").val();
            var unitPrice = $("#<%=txtUnitPrice.ClientID %>").val();
            var itemUnit = $("#<%=txtItemUnit.ClientID %>").val();

            if ($('#' + unitPrice).val() != 0) {
                var calculatedAmount = 0.00;
                if ($("#ContentPlaceHolder1_chkIscomplementary").is(":checked") == false) {
                    calculatedAmount = parseFloat(itemUnit) * parseFloat(unitPrice);
                }

                if ($("#ltlTableWiseItemInformation > table").length > 0 && reservationId == 0) {
                    AddNewRow(id, reservationId, itemTypeId, itemId, unitPrice, itemUnit, calculatedAmount);
                    return false;
                }
                else if ($("#ltlTableWiseItemInformation > table").length > 0 && reservationId > 0) {
                    AddNewRow(id, reservationId, itemTypeId, itemId, unitPrice, itemUnit, calculatedAmount);
                    return false;
                }

                var table = "", deleteLink = "", editLink = "";

                deleteLink = "<a href=\"#\" onclick= 'DeleteDetailInfo(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
                //editLink = "<a href=\"#\" onclick= 'EditReservationDetail(this)' ><img alt=\"Edit\" src=\"../Images/edit.png\" /></a>";
                table += "<table id='RecipeItemInformation' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                table += "<th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th align='left' scope='col' style='width: 45%;'>Item Name</th><th align='left' scope='col' style='width: 15%;'>Unit Price</th><th align='left' scope='col' style='width: 15%;'>Unit</th><th align='left' scope='col' style='width: 15%;'>Amount</th><th align='center' scope='col' style='width: 10%;'>Action</th></tr></thead>";

                table += "<tbody>";
                table += "<tr style=\"background-color:#E3EAEB;\">";

                table += "<td align='left' style=\"display:none;\">" + id + "</td>";
                table += "<td align='left' style=\"display:none;\">" + reservationId + "</td>";
                table += "<td align='left' style=\"display:none;\">" + itemTypeId + "</td>";
                table += "<td align='left' style=\"display:none;\">" + $("#<%=ddlItemType.ClientID %> option:selected").text() + "</td>";
                table += "<td align='left' style=\"display:none;\">" + itemId + "</td>";
                table += "<td align='left' style=\"width:45%; text-align:Left;\">" + $("#<%=ddlItemId.ClientID %> option:selected").text() + "</td>";
                table += "<td align='left' style=\"width:15%; text-align:Left;\">" + unitPrice + "</td>";
                table += "<td align='left' style=\"width:15%; text-align:Left;\">" + itemUnit + "</td>";
                table += "<td align='left' style=\"width:15%; text-align:Left;\">" + calculatedAmount + "</td>";
                table += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

                table += "</tr>";
                table += "</tbody>";
                table += "</table>";

                $("#ltlTableWiseItemInformation").html(table);
                CalculateTotalPaiedNDueAmount();
                $("#<%=ddlItemId.ClientID %>").val('0');
                $("#<%=txtUnitPrice.ClientID %>").val('');
                $("#<%=txtItemUnit.ClientID %>").val('');
                $("#ContentPlaceHolder1_chkIscomplementary").prop('checked', false);
            }
        }
        function AddNewRow(id, reservationId, itemTypeId, itemId, unitPrice, itemUnit, calculatedAmount) {
            var tr = "", totalRow = 0, deleteLink = "", editLink = "";
            totalRow = $("#RecipeItemInformation tbody tr").length;

            deleteLink = "<a href=\"#\" onclick= 'DeleteDetailInfo(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
            //editLink = "<a href=\"#\" onclick= 'EditReservationDetail(this)' ><img alt=\"Edit\" src=\"../Images/edit.png\" /></a>";

            if ((totalRow % 2) == 0) {
                tr += "<tr style=\"background-color:#E3EAEB;\">";
            }
            else {
                tr += "<tr style=\"background-color:#FFFFFF;\">";
            }
            tr += "<td align='left' style=\"display:none;\">" + id + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + reservationId + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + itemTypeId + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + $("#<%=ddlItemType.ClientID %> option:selected").text() + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + itemId + "</td>";
            tr += "<td align='left' style=\"width:45%; text-align:Left;\">" + $("#<%=ddlItemId.ClientID %> option:selected").text() + "</td>";
            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + unitPrice + "</td>";
            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itemUnit + "</td>";
            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + calculatedAmount + "</td>";
            tr += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

            tr += "</tr>";

            $("#RecipeItemInformation tbody").append(tr);
            CalculateTotalPaiedNDueAmount();
            $("#<%=ddlItemId.ClientID %>").val('0');
            $("#<%=txtUnitPrice.ClientID %>").val('');
            $("#<%=txtItemUnit.ClientID %>").val('');
            $("#ContentPlaceHolder1_chkIscomplementary").prop('checked', false);
        }

        function CalculateTotalAddedPayment() {
            var calculateAmount = 0;
            $("#RecipeItemInformation tbody tr").each(function () {
                calculateAmount += parseFloat($.trim($(this).find("td:eq(8)").text(), 10));
            });

            return calculateAmount;
        }

        function CalculateTotalPaiedNDueAmount() {
            var calculateAmount = 0, hfBanquetRate = 0, dueAmount = 0, refundAmount = 0;
            calculateAmount = CalculateTotalAddedPayment();
            refundAmount = 0;

            hfBanquetRate = parseFloat($.trim($("#<%= hfBanquetRate.ClientID %>").val()));

            $("#TotalPaid").text("Total Amount: " + calculateAmount);
            var floatdueAmount = parseFloat(dueAmount) + parseFloat(refundAmount);

            var txtTotalAmount = '<%=txtTotalAmount.ClientID%>'
            var hfTotalAmount = '<%=hfTotalAmount.ClientID%>'
            var txtDiscountedAmount = '<%=txtDiscountedAmount.ClientID%>'
            var hfDiscountedAmount = '<%=hfDiscountedAmount.ClientID%>'
            var totalCalculatedAmount = calculateAmount + hfBanquetRate;
            $('#' + txtTotalAmount).val(totalCalculatedAmount);
            $('#' + hfTotalAmount).val(totalCalculatedAmount);
            $('#' + txtDiscountedAmount).val(totalCalculatedAmount);
            $('#' + hfDiscountedAmount).val(totalCalculatedAmount);
            TotalRoomRateVatServiceChargeCalculation();
        }

        function ValidationNPreprocess() {
            var name = $.trim($("#<%=txtName.ClientID %>").val());
            var email = $.trim($("#<%=txtContactEmail.ClientID %>").val());
            var banquetName = $.trim($("#<%=ddlBanquetId.ClientID %>").val());
            var startDate = $.trim($("#<%=txtPartyDate.ClientID %>").val());
            var occasionType = $.trim($("#<%=ddlOccessionTypeId.ClientID %>").val());
            var seatingName = $.trim($("#<%=ddlSeatingId.ClientID %>").val());
            var noOfAdult = $.trim($("#<%=txtNumberOfPersonAdult.ClientID %>").val());
            var arriveTime = $.trim($("#<%=txtProbableArrivalHour.ClientID %>").val());
            var departTime = $.trim($("#<%=txtProbableDepartureHour.ClientID %>").val());
            var ddlReservationMode = $.trim($("#<%=ddlReservationMode.ClientID %>").val());

            var hfDuplicateReservarionValidation = '<%=hfDuplicateReservarionValidation.ClientID%>'
            if ($('#' + hfDuplicateReservarionValidation).val() == "1") {
                toastr.warning('Already Reservation exist in your given time period.');
                return false;
            }

            var chkIsLitedCompany = '<%=chkIsLitedCompany.ClientID%>'
            if (banquetName == "0") {
                toastr.warning("Please Provide Banquet Name.");
                return false;
            }
            else if (startDate == "") {
                toastr.warning("Please Provide Party Start Date.");
                return false;
            }
            else if (occasionType == "0") {
                toastr.warning("Please Select Occasion Type.");
                return false;
            }
            else if (seatingName == "0") {
                toastr.warning("Please Select Seating Name.");
                return false;
            }
            else if (noOfAdult == "") {
                toastr.warning("Please Provide number of Adult.");
                return false;
            }
            else if (ddlReservationMode == "0") {
                toastr.warning("Please Select Reservation Mode.");
                return false;
            }

            if (noOfAdult != "") {
                var numbersOnly = /^\d+$/;
                if (numbersOnly.test(noOfAdult)) {
                }
                else {
                    toastr.warning("Provide correct number of Adult.");
                    return false;
                }
            }
            if (arriveTime.length == 1) {
                arriveTime = "0" + arriveTime;
            }
            if (departTime.length == 1) {
                departTime = "0" + departTime;
            }
            if (arriveTime >= departTime) {
                toastr.warning('Party Start time can not be greater or equal to End time.');
                return false;
            }

            var saveObj = [];
            var detailId = 0, reservationId = 0, itemTypeId = 0, itemType = "", itemId = 0, itemName = "", itemUnit = 0, unitPrice = 0, totalPrice = 0;

            var rowLength = $("#ltlTableWiseItemInformation > table tbody tr").length;

            $("#ltlTableWiseItemInformation > table tbody tr").each(function () {
                detailId = parseInt($.trim($(this).find("td:eq(0)").text(), 10));
                reservationId = parseInt($.trim($(this).find("td:eq(1)").text(), 10));
                itemTypeId = parseInt($.trim($(this).find("td:eq(2)").text(), 10));
                itemType = $(this).find("td:eq(3)").text();
                itemId = parseInt($.trim($(this).find("td:eq(4)").text(), 10));
                itemName = $(this).find("td:eq(5)").text();
                unitPrice = parseFloat($.trim($(this).find("td:eq(6)").text(), 10));
                itemUnit = parseFloat($.trim($(this).find("td:eq(7)").text(), 10));
                totalPrice = parseFloat($.trim($(this).find("td:eq(8)").text(), 10));

                if (detailId == 0) {

                    saveObj.push({
                        DetailId: detailId,
                        ReservationId: reservationId,
                        ItemTypeId: itemTypeId,
                        ItemType: itemType,
                        ItemId: itemId,
                        ItemName: itemName,
                        ItemUnit: itemUnit,
                        UnitPrice: unitPrice,
                        TotalPrice: totalPrice
                    });
                }
            });

            $("#<%=hfSaveObj.ClientID %>").val(JSON.stringify(saveObj));
            $("#<%=hfDeleteObj.ClientID %>").val(JSON.stringify(deleteObj));
            $("#ContentPlaceHolder1_hfClassificationDiscountSave").val(JSON.stringify(categoryWiseDiscount));
            $("#ContentPlaceHolder1_hfClassificationDiscountDelete").val(JSON.stringify(categoryWiseDiscountDeleted));
        }
        function OnGetBanquetReservationInfoForDuplicateCheckingSucceeded(result) {
            if (result == "1") {
                $("#ContentPlaceHolder1_hfDuplicateReservarionValidation").val("1");
                toastr.warning('Already Reservation exist in your given time period.');
                return false;
            }
            else {
                var ddlBanquetId = '<%=ddlBanquetId.ClientID%>'
                $("#ContentPlaceHolder1_hfDuplicateReservarionValidation").val("0");
                PageMethods.GetBanquetInfoByCriteria($('#' + ddlBanquetId).val(), LoadBanquetInfoSucceeded, LoadBanquetInfoFailed);
            }

            return false;
        }
        function OnGetBanquetReservationInfoForDuplicateCheckingFailed(error) {
            toastr.error(error.get_message());
        }
        function DeleteDetailInfo(anchor) {
            ff = anchor;
            var tr = $(anchor).parent().parent();

            var detailId = $.trim($(tr).find("td:eq(0)").text());
            var reservationId = $.trim($(tr).find("td:eq(1)").text());

            if (parseInt(detailId, 10) != 0) {
                deleteObj.push({
                    DetailId: detailId,
                    ReservationId: reservationId
                });
            }

            $(tr).remove();
            CalculateTotalPaiedNDueAmount();
            return false;
        }

        function EditDetailInfo(detailId, reservationId) {

            var itemTypeId = $("#<%=ddlItemType.ClientID %>").val();
            var itemId = $("#<%=ddlItemId.ClientID %>").val();
            var unitPrice = $("#<%=txtUnitPrice.ClientID %>").val();
            var itemUnit = $("#<%=txtItemUnit.ClientID %>").val();

            if ($('#' + unitPrice).val() != 0) {

                var calculatedAmount = 0.00;

                if ($("#ContentPlaceHolder1_chkIscomplementary").is(":checked") == false) {
                    calculatedAmount = parseFloat(itemUnit) * parseFloat(unitPrice);
                }

                var tr = "", totalRow = 0, deleteLink = "", editLink = "";
                totalRow = $("#RecipeItemInformation tbody tr").length;

                deleteLink = "<a href=\"#\" onclick= 'DeleteDetailInfo(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
                editLink = "<a href=\"#\" onclick= 'EditReservationDetail(this)' ><img alt=\"Edit\" src=\"../Images/edit.png\" /></a>";

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                tr += "<td align='left' style=\"display:none;\">" + detailId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + reservationId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itemTypeId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + $("#<%=ddlItemType.ClientID %> option:selected").text() + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itemId + "</td>";
                tr += "<td align='left' style=\"width:45%; text-align:Left;\">" + $("#<%=ddlItemId.ClientID %> option:selected").text() + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + unitPrice + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itemUnit + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + calculatedAmount + "</td>";
                tr += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

                tr += "</tr>";

                $("#RecipeItemInformation tbody").append(tr);
            }
            return false;
        }

        function EditReservationDetail(anchor) {

            var tr = $(anchor).parent().parent();

            var detailId = $.trim($(tr).find("td:eq(0)").text());
            var reservationId = $.trim($(tr).find("td:eq(1)").text());

            $(tr).remove();
            PageMethods.GetReservationDetailInfo(detailId, reservationId, OnDetailSucceeded, OnDetailFailed);
            return false;
        }
        function OnDetailSucceeded(result) {
            $("#<%=ddlItemType.ClientID %>").val(result.ItemTypeId);
            $("#<%=ddlItemId.ClientID %>").val(result.ItemId);
            $("#<%=txtUnitPrice.ClientID %>").val(result.UnitPrice);
            $("#<%=txtItemUnit.ClientID %>").val(result.ItemUnit);
            $("#<%=hfDetailId.ClientID %>").val(result.DetailId);
        }
        function OnDetailFailed(error) {
            toastr.error(error.get_message());
        }

        function IsEmail(email) {
            var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            return regex.test(email);
        }

        function LoadProductData(ddlItemId) {
            var costCenterId = $("#<%=hfCostCenterId.ClientID %>").val();
            var categoryId = $("#<%=ddlItemType.ClientID %>").val();
            PageMethods.GetProductDataByCriteria(categoryId, costCenterId, ddlItemId, LoadProductDataSucceeded, LoadProductDataFailed);
            return false;
        }
        function LoadProductDataSucceeded(result) {
            var txtUnitPrice = '<%=txtUnitPrice.ClientID%>'
            var txtHiddenProductId = '<%=txtHiddenProductId.ClientID%>'
            $('#' + txtHiddenProductId).val(result.ItemId);
            $('#' + txtUnitPrice).val(result.UnitPriceLocal);
            return false;
        }

        function LoadProductDataFailed(error) {
            toastr.error(error.get_message());
        }

        function LoadBanquetInfoSucceeded(result) {
            var txtBanquetRate = '<%=txtBanquetRate.ClientID%>'
            var hfBanquetRate = '<%=hfBanquetRate.ClientID%>'

            $('#' + txtBanquetRate).val(result.UnitPrice);
            $('#' + hfBanquetRate).val(result.UnitPrice);

            $("#<%=hfIsBanquetBillInclusive.ClientID %>").val(result.IsVatSChargeInclusive);
            $("#<%=hfBanquetServiceCharge.ClientID %>").val(result.ServiceCharge);
            $("#<%=hfBanquetVatAmount.ClientID %>").val(result.VatAmount);
            $("#<%=hfCostCenterId.ClientID %>").val(result.CostCenterId);

            var txtTotalAmount = '<%=txtTotalAmount.ClientID%>'
            $('#' + txtTotalAmount).attr("disabled", false);

            if (result.IsVatSChargeInclusive == 0) {
                $("#ContentPlaceHolder1_lblGrandTotal").text("Grand Total");
            }
            else {
                $("#ContentPlaceHolder1_lblGrandTotal").text("Service Rate");
            }

            if ($('#' + txtTotalAmount).val() == "") {
                $('#' + txtTotalAmount).val(result.UnitPriceLocal);
            }
            CalculateTotalPaiedNDueAmount();
            var costCenterId = $("#<%=hfCostCenterId.ClientID %>").val();
            PageMethods.LoadCategory(costCenterId, OnLoadCategorySucceeded, OnLoadCategoryFailed);
            return false;
        }

        function LoadBanquetInfoFailed(error) {
            toastr.error(error.get_message());
        }

        function OnLoadCategorySucceeded(result) {
            var list = result;
            var ddlCategoryId = '<%=ddlItemType.ClientID%>';
            var control = $('#' + ddlCategoryId);
            control.empty();

            if (list != null) {
                if (list.length > 0) {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].MatrixInfo + '" value="' + list[i].CategoryId + '">' + list[i].MatrixInfo + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                }
            }
            return false;
        }

        function OnLoadCategoryFailed() {
            toastr.error(error.get_message());
        }

        function LoadProductItem(itemType) {
            var costCenterId = $("#<%=hfCostCenterId.ClientID %>").val();
            PageMethods.GetInvItemByCategoryNCostCenter(costCenterId, itemType, OnFillServiceSucceeded, OnFillServiceFailed);
            return false;
        }
        function OnFillServiceSucceeded(result) {
            var list = result;
            var ddlItemId = '<%=ddlItemId.ClientID%>';
            var control = $('#' + ddlItemId);
            control.empty();

            if (list != null) {
                if (list.length > 0) {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].ItemId + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                }
            }
            return false;
        }

        function OnFillServiceFailed(error) {
            toastr.error(error.get_message());
        }


        $(function () {
            $("#myTabs").tabs();
        });

        function ShowUploadedDocument(id) {
            var ddlSeatingId = '<%=ddlSeatingId.ClientID%>';
            var ddlSeatingId = $('#' + ddlSeatingId).val();
            //alert(ddlSeatingId);
            if (ddlSeatingId == 0) {
                toastr.info('Please Select a Seating Plan');
                return false;
            }
            else {
                PageMethods.GetUploadedImageByWebMethod(ddlSeatingId, "BanquetSeatingPlan", OnGetUploadedImageByWebMethodSucceeded, OnGetUploadedImageByWebMethodFailed);
            }
            return false;
        }

        function OnGetUploadedImageByWebMethodSucceeded(result) {
            $("#SeatingPlanDialog").dialog({
                autoOpen: true,
                modal: true,
                //width: 450,
                closeOnEscape: true,
                resizable: false,
                title: "Seating Plan",
                show: 'slide'
            });
            if (result != "") {
                $('#ImagePanel').show();
            }
            else {
                $('#ImagePanel').hide();
            }
            $('#SigDiv').html(result);
            return false;
        }
        function OnGetUploadedImageByWebMethodFailed(error) {
            toastr.error(error.get_message());
        }

        function ToggleTotalRoomRateVatServiceChargeCalculationForServiceCharge(ctrl) {
            TotalRoomRateVatServiceChargeCalculation();
        }

        function ToggleTotalRoomRateVatServiceChargeCalculationForVat(ctrl) {
            TotalRoomRateVatServiceChargeCalculation();
        }

        function TotalRoomRateVatServiceChargeCalculation() {
            var txtRoomRate = '<%=hfDiscountedAmount.ClientID%>'
            var cbServiceCharge = '<%=cbServiceCharge.ClientID%>'
            var cbVatAmount = '<%=cbVatAmount.ClientID%>'

            var cbServiceChargeVal = "1";
            if ($('#' + cbServiceCharge).is(':checked')) {
                cbServiceChargeVal = "1";
            }
            else {
                cbServiceChargeVal = "0";
            }

            var cbVatAmountVal = "1";
            if ($('#' + cbVatAmount).is(':checked')) {
                cbVatAmountVal = "1";
            }
            else {
                cbVatAmountVal = "0";
            }

            var txtRoomRateVal = parseFloat($('#' + txtRoomRate).val());

            var InclusiveBill = 0, Vat = 0.00, ServiceCharge = 0.00;

            if ($("#<%=hfIsBanquetBillInclusive.ClientID %>").val() != "")
            { InclusiveBill = parseInt($("#<%=hfIsBanquetBillInclusive.ClientID %>").val(), 10); }

            if ($("#<%=hfBanquetVatAmount.ClientID %>").val() != "")
                Vat = parseFloat($("#<%=hfBanquetVatAmount.ClientID %>").val());

            if ($("#<%=hfBanquetServiceCharge.ClientID %>").val() != "")
                ServiceCharge = parseFloat($("#<%=hfBanquetServiceCharge.ClientID %>").val());

            //var InnboardRateCalculationInfo = CommonHelper.GetRackRateServiceChargeVatInformation(InclusiveBill, Vat, ServiceCharge, txtRoomRateVal, parseInt(cbServiceChargeVal, 10), parseInt(cbVatAmountVal, 10));
            var InnboardRateCalculationInfo = CommonHelper.GetRackRateServiceChargeVatInformation(parseInt(InclusiveBill, 10), parseFloat(Vat), parseFloat(ServiceCharge), 0,
                                                            parseFloat(txtRoomRateVal), parseInt(cbServiceChargeVal, 10), 0, parseInt(cbVatAmountVal, 10));
            OnLoadRackRateServiceChargeVatInformationSucceeded(InnboardRateCalculationInfo);
        }

        function OnLoadRackRateServiceChargeVatInformationSucceeded(result) {
            if (result.RackRate > 0) {
                $("#<%=txtGrandTotal.ClientID %>").val(result.RackRate);
                $("#<%=txtServiceCharge.ClientID %>").val(result.ServiceCharge);
                $("#<%=hfServiceCharge.ClientID %>").val(result.ServiceCharge);
                $("#<%=txtVatAmount.ClientID %>").val(result.VatAmount);
                $("#<%=hfVatAmount.ClientID %>").val(result.VatAmount);

            }
            else {
                $("#<%=txtGrandTotal.ClientID %>").val('0');
                $("#<%=txtServiceCharge.ClientID %>").val('0');
                $("#<%=hfServiceCharge.ClientID %>").val('0');
                $("#<%=txtVatAmount.ClientID %>").val('0');
                $("#<%=hfVatAmount.ClientID %>").val('0');
            }

            return false;
        }

        function OnLoadRackRateServiceChargeVatInformationFailed(error) {
            //alert(error.get_message());
        }

        function CalculateDiscountAmount() {
            var checkValue = CheckDiscountValidationForFixedRPercentage($("#ContentPlaceHolder1_ddlDiscountType"), $("#ContentPlaceHolder1_txtDiscountAmount"), $("#ContentPlaceHolder1_txtTotalAmount"), "Discount Amount");
            if (checkValue == false) { return false; }

            var checkDiscount = $("#ContentPlaceHolder1_txtDiscountAmount").val();

            if ($.trim(checkDiscount) == "")
                checkDiscount = "0";

            //if ($("#ContentPlaceHolder1_ddlDiscountType").val() == "Percentage") {
            //    if (parseFloat(checkDiscount) > 100.00) {
            //        toastr.info("Discount Percentage Cannot Be Greater Than (>) 100.");
            //        $("#ContentPlaceHolder1_txtDiscountAmount").val("0");
            //    }
            //}
            //else if ($("#ContentPlaceHolder1_ddlDiscountType").val() == "Fixed") {
            //    if (parseFloat(checkDiscount) > parseFloat($("#ContentPlaceHolder1_txtTotalAmount").val())) {
            //        toastr.info("Discount Amount Cannot Be Greater Than (>) Sales Amount.");
            //        $("#ContentPlaceHolder1_txtDiscountAmount").val("0");
            //    }
            //}

            var IsInclusiveBill = 0, vatAmount = 0.00, serviceChargeAmount = 0.00;

            if ($("#ContentPlaceHolder1_hfIsBanquetBillInclusive").val() != "")
            { IsInclusiveBill = parseInt($("#ContentPlaceHolder1_hfIsBanquetBillInclusive").val(), 10); }

            if ($("#ContentPlaceHolder1_hfBanquetVatAmount").val() != "")
                vatAmount = parseFloat($("#ContentPlaceHolder1_hfBanquetVatAmount").val());

            if ($("#ContentPlaceHolder1_hfBanquetServiceCharge").val() != "")
                serviceChargeAmount = parseFloat($("#ContentPlaceHolder1_hfBanquetServiceCharge").val());


            var IsServiceChargeEnable = 1;
            if ($('#ContentPlaceHolder1_cbServiceCharge').is(':checked')) {
                IsServiceChargeEnable = 1;
            }
            else {
                IsServiceChargeEnable = 0;
            }

            var IsVatEnable = 1;
            if ($('#ContentPlaceHolder1_cbVatAmount').is(':checked')) {
                IsVatEnable = 1;
            }
            else {
                IsVatEnable = 0;
            }

            var discountType = "", discount = 0.00;

            discountType = $("#ContentPlaceHolder1_ddlDiscountType").val();
            discount = $("#ContentPlaceHolder1_txtDiscountAmount").val() == "" ? 0.00 : parseFloat($("#ContentPlaceHolder1_txtDiscountAmount").val());

            var itemTableId = "RecipeItemInformation";

            var BillPaymentDetails = CommonHelper.GetBanquetVatNSChargeNDiscountNComplementary(itemTableId, discountType, discount,
                                                                                               vatAmount, serviceChargeAmount, IsServiceChargeEnable,
                                                                                               IsVatEnable, IsInclusiveBill);
            if (IsInclusiveBill == 0) {
                $("#ContentPlaceHolder1_lblGrandTotal").text("Grand Total");
                if ($("#ContentPlaceHolder1_hfIsBanquetBillAmountWillRound").val() == "1") {
                    $("#ContentPlaceHolder1_txtGrandTotal").val(Math.round(BillPaymentDetails.GrandTotal));
                }
                else {
                    $("#ContentPlaceHolder1_txtGrandTotal").val(BillPaymentDetails.GrandTotal);
                }
            }
            else {
                $("#ContentPlaceHolder1_lblGrandTotal").text("Service Rate");
                $("#ContentPlaceHolder1_txtGrandTotal").val(BillPaymentDetails.ServiceRate);
            }

            $("#ContentPlaceHolder1_txtDiscountedAmount").val(BillPaymentDetails.DiscountedAmount);
            $("#ContentPlaceHolder1_txtServiceCharge").val(BillPaymentDetails.ServiceCharge);
            $("#ContentPlaceHolder1_txtVatAmount").val(BillPaymentDetails.VatAmount);

            $("#ContentPlaceHolder1_hfServiceCharge").val(BillPaymentDetails.ServiceCharge);
            $("#ContentPlaceHolder1_hfVatAmount").val(BillPaymentDetails.VatAmount);
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#gvReservation tbody tr").length;
            var name = $("#<%=txtSearchName.ClientID %>").val();
            var reserveNo = $("#<%=txtSearchReservationNumber.ClientID %>").val();
            var banquetId = $("#<%=ddlSearchBanquetName.ClientID %>").val();
            var email = $("#<%=txtSearchEmail.ClientID %>").val();
            var phone = $("#<%=txtSearchPhone.ClientID %>").val();
            var arriveDate = $("#<%=txtSearchArraiveDate.ClientID %>").val();
            var departDate = $("#<%=txtSearchDepartureDate.ClientID %>").val();

            PageMethods.SearchReservationAndLoad(name, reserveNo, banquetId, email, phone, arriveDate, departDate, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {

            $("#gvReservation tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvReservation tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvReservation tbody tr").length;
                //totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:35%; cursor:pointer;\">" + gridObject.Name + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.ReservationNumber + "</td>";
                tr += "<td align='left' style=\"width:25%; cursor:pointer;\">" + gridObject.ContactEmail + "</td>";

                if (gridObject.IsBillSettlement == false && gridObject.ActiveStatus == true) {
                    tr += "<td align='left' style=\"width:10%; cursor:pointer;\">" + gridObject.Status + "</td>";
                    tr += "<td align='right' style=\"width:5%; cursor:pointer;\"><img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.ReservationId + "')\" alt='Edit Information' border='0' /></td>";
                }
                else {
                    if (gridObject.IsBillSettlement == true) {
                        tr += "<td align='left' style=\"width:10%; cursor:pointer;\">" + 'Settled' + "</td>";
                    }
                    else {
                        tr += "<td align='left' style=\"width:10%; cursor:pointer;\">" + gridObject.Status + "</td>";
                    }

                    tr += "<td align='right' disabled='disabled' style=\"width:5%; cursor:pointer;\"> </td>";
                }
                tr += "<td align='right' style=\"width:5%; cursor:pointer;\"><img src='../Images/ReportDocument.png' onClick= \"javascript:return PerformBillPreview('" + gridObject.ReservationId + "')\" alt='Delete Information' border='0' /></td>";

                tr += "</tr>"

                $("#gvReservation tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
            return false;
        }
        function OnLoadObjectFailed(error) {
            toastr.error(error.get_message());
        }
        function PerformEditAction(reservationId) {
            var possiblePath = "frmBanquetReservation.aspx?editId=" + reservationId;
            window.location = possiblePath;
        }
        function PerformBillPreview(reservationId) {
            var url = "/Banquet/Reports/frmReportReservationConLatter.aspx?ReservationId=" + reservationId;
            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=790,height=780,left=300,top=50,resizable=yes");
        }

        function LoadCategorizedDiscountInfo() {
            var categoryId = 100001;
            var discountCategory = new Array();

            var vRequisities = _.findWhere(categoryWiseDiscount, { CategoryId: categoryId });

            discountCategory.push(
            {
                DiscountId: vRequisities == null ? 0 : vRequisities.DiscountId,
                CategoryId: categoryId,
                Category: "Banquet Hall"
            });

            $("#RecipeItemInformation tbody tr").each(function () {

                categoryId = parseInt($.trim($(this).find("td:eq(2)").text(), 10));

                var vRequisities = _.findWhere(discountCategory, { CategoryId: categoryId });
                var vRequisitiesSaved = _.findWhere(categoryWiseDiscount, { CategoryId: categoryId });

                if (vRequisities == null) {

                    if (categoryId == 100000) {
                        discountCategory.push(
                        {
                            DiscountId: vRequisitiesSaved == null ? 0 : vRequisitiesSaved.DiscountId,
                            CategoryId: categoryId,
                            Category: "Requisites"
                        });
                    }
                    else {
                        discountCategory.push(
                        {
                            DiscountId: vRequisitiesSaved == null ? 0 : vRequisitiesSaved.DiscountId,
                            CategoryId: categoryId,
                            Category: "Restaurant Item"
                        });
                    }
                }
            });

            var table = "", checkBoxChecked = "";
            table += "<table cellspacing='0' cellpadding='4' id='CategorizedDiscount' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            table += "<th style='display:none'></th><th align='left' scope='col' style='width: 8%;'><input type='checkbox'></th><th align='left' scope='col' style='width: 70%;'>Category</th></tr></thead>";

            table += "<tbody>";

            for (var i = 0; i < discountCategory.length; i++) {

                var vRequisities = _.findWhere(categoryWiseDiscount, { CategoryId: discountCategory[i].CategoryId });

                if (vRequisities != null) {
                    checkBoxChecked = "checked = 'checked'";
                }
                else {
                    checkBoxChecked = "";
                }

                table += "<tr style=\"background-color:#E3EAEB;\">";
                table += "<td align='left' style=\"display:none;\">" + discountCategory[i].CategoryId + "</td>";
                table += "<td align='left' style=\"display:none;\">" + discountCategory[i].DiscountId + "</td>";
                table += "<td align='left' style=\"width:8%; text-align:Left;\"> <input type='checkbox' " + checkBoxChecked + "> </td>";
                table += "<td align='left' style=\"width:70%; text-align:Left;\">" + discountCategory[i].Category + "</td>";
                table += "</tr>";
            }

            table += "</tbody>";
            table += "</table>";

            $("#CategoryWiseDiscountDiv").html(table);

            $("#CategoryWiseDiscountContainer").dialog({
                autoOpen: true,
                modal: true,
                width: 450,
                closeOnEscape: true,
                resizable: false,
                title: "Category Wise Discount",
                show: 'slide'
            });
            return;
        }

        function AddReservationDetails() {
            var reservationDate, startTime, endTime, occasitionType, banquetName, hallRate, layoutType, numberOfAdult, numberOfChild;
            var occasitionTypeId, banquetId, layoutTypeId, reservationDetailsId, reservationId, reservationParentId;

            reservationDate = $("#ContentPlaceHolder1_txtPartyDate").val();
            startTime = $("#ContentPlaceHolder1_txtProbableArrivalHour").val();
            endTime = $("#ContentPlaceHolder1_txtProbableDepartureHour").val();
            occasitionType = $("#ContentPlaceHolder1_ddlOccessionTypeId option:selected").text();
            banquetName = $("#ContentPlaceHolder1_ddlBanquetId option:selected").text();
            hallRate = $("#ContentPlaceHolder1_txtBanquetRate").val();
            layoutType = $("#ContentPlaceHolder1_ddlSeatingId option:selected").text();
            numberOfAdult = $("#ContentPlaceHolder1_txtNumberOfPersonAdult").val();
            numberOfChild = $("#ContentPlaceHolder1_txtNumberOfPersonChild").val();

            occasitionTypeId = $("#ContentPlaceHolder1_ddlOccessionTypeId").val();
            banquetId = $("#ContentPlaceHolder1_ddlBanquetId").val();
            layoutTypeId = $("#ContentPlaceHolder1_ddlSeatingId").val();

            var tr = "";

            tr += "<tr>";
            tr += "<td style='width: 8%; vertical-align: middle;'>" + reservationDate + "</td>";
            tr += "<td style='width: 5%; vertical-align: middle;'>" + startTime + "</td>";
            tr += "<td style='width: 5%; vertical-align: middle;'>" + endTime + "</td>";
            tr += "<td style='width: 22%; vertical-align: middle;'>" + occasitionType + "</td>";
            tr += "<td style='width: 18%; vertical-align: middle;'>" + banquetName + "</td>";
            tr += "<td style='width: 18%; vertical-align: middle;'>" + layoutType + "</td>";
            tr += "<td style='width: 5%; vertical-align: middle;'>" + numberOfAdult + "</td>";
            tr += "<td style='width: 5%; vertical-align: middle;'>" + numberOfChild + "</td>";
            tr += "<td style='width: 8%; vertical-align: middle;'>" + hallRate + "</td>";

            tr += "<td style='width: 6%; vertical-align: middle;'>";
            tr += "<img src='../Images/edit.png' style='cursor:pointer;' onClick= \"javascript:return EditReservationDetails('" + reservationId + "')\" alt='Edit' border='0' />";
            tr += "&nbsp;&nbsp;&nbsp;";
            tr += "<img src='../Images/delete.png' style='cursor:pointer;' onClick= \"javascript:return DeleteReservationDetails('" + reservationId + "')\" alt='Delete' border='0' />";
            tr += "</td>";

            tr += "<td style='display: none;'>" + occasitionTypeId + "</td>";
            tr += "<td style='display: none;'>" + banquetId + "</td>";
            tr += "<td style='display: none;'>" + layoutTypeId + "</td>";
            tr += "</tr>"

            $("#ReservationDetailsTable > tbody").append(tr);

            AddItemDetailsForReservation();
        }

        function AddItemForReservation() {

            var categoryId, itemId, unitPrice, itemUnit, itemName, categoryName, calculatedAmount = 0.00;
            var id, reservationId;

            categoryId = $("#ContentPlaceHolder1_ddlItemType").val();
            itemId = $("#ContentPlaceHolder1_ddlItemId").val();
            unitPrice = $("#ContentPlaceHolder1_txtUnitPrice").val();
            itemUnit = $("#ContentPlaceHolder1_txtItemUnit").val();

            if (unitPrice == "" || unitPrice == "0") {
                toastr.info("Please Give Item Price.");
                return false;
            }

            itemName = $("#ContentPlaceHolder1_ddlItemId option:selected").text();
            categoryName = $("#ContentPlaceHolder1_ddlItemType option:selected").text();

            if ($("#ContentPlaceHolder1_chkIscomplementary").is(":checked") == false) {
                calculatedAmount = parseFloat(itemUnit) * parseFloat(unitPrice);
            }

            var tr = "", deleteLink = "", editLink = "";

            deleteLink = "<a href=\"#\" onclick= 'DeleteDetailInfo(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
            //editLink = "<a href=\"#\" onclick= 'EditReservationDetail(this)' ><img alt=\"Edit\" src=\"../Images/edit.png\" /></a>";

            tr += "<tr>";

            tr += "<td align='left' style=\"width:45%; text-align:Left;\">" + itemName + "</td>";
            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + unitPrice + "</td>";
            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itemUnit + "</td>";
            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + calculatedAmount + "</td>";
            tr += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

            tr += "<td align='left' style=\"display:none;\">" + id + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + reservationId + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + categoryId + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + categoryName + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + itemId + "</td>";

            tr += "</tr>";

            //CalculateTotalPaiedNDueAmount();
            $("#ItemInformation > tbody").append(tr);

            $("#ContentPlaceHolder1_ddlItemId").val('0');
            $("#ContentPlaceHolder1_txtUnitPrice").val('');
            $("#ContentPlaceHolder1_txtItemUnit").val('');
        }

        function AddItemDetailsForReservation() {

            var categoryId, itemId, unitPrice, itemUnit, itemName, categoryName, calculatedAmount = 0.00;
            var id, reservationId, partyDate;
            var tr = "", deleteLink = "", editLink = "";

            partyDate = $("#ContentPlaceHolder1_txtPartyDate").val();

            $("#ItemInformation tbody tr").each(function () {
                deleteLink = "<a href=\"#\" onclick= 'DeleteDetailInfo(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
                //editLink = "<a href=\"#\" onclick= 'EditReservationDetail(this)' ><img alt=\"Edit\" src=\"../Images/edit.png\" /></a>";

                tr += "<tr>";

                tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + partyDate + "</td>";
                tr += "<td align='left' style=\"width:35%; text-align:Left;\">" + $(this).find("td:eq(0)").text() + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + $(this).find("td:eq(1)").text() + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + $(this).find("td:eq(2)").text() + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + $(this).find("td:eq(3)").text() + "</td>";
                tr += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

                tr += "<td align='left' style=\"display:none;\">" + $(this).find("td:eq(5)").text() + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + $(this).find("td:eq(6)").text() + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + $(this).find("td:eq(7)").text() + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + $(this).find("td:eq(8)").text() + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + $(this).find("td:eq(9)").text() + "</td>";

                tr += "</tr>";

                //CalculateTotalPaiedNDueAmount();
                $("#ItemDetailsInformation > tbody").append(tr);
                tr = "";
            });
            
            if (unitPrice == "" || unitPrice == "0") {
                toastr.info("Please Give Item Price.");
                return false;
            }

            itemName = $("#ContentPlaceHolder1_ddlItemId option:selected").text();
            categoryName = $("#ContentPlaceHolder1_ddlItemType option:selected").text();

            if ($("#ContentPlaceHolder1_chkIscomplementary").is(":checked") == false) {
                calculatedAmount = parseFloat(itemUnit) * parseFloat(unitPrice);
            }

            

           

            $("#ContentPlaceHolder1_ddlItemId").val('0');
            $("#ContentPlaceHolder1_txtUnitPrice").val('');
            $("#ContentPlaceHolder1_txtItemUnit").val('');
        }

        function DayDurationFeed() {
            var duarionCount = DayDuration.length;
            var row = 0;

            var DetailsDay = $("#ContentPlaceHolder1_ddlDetailsDay");
            var ItemDetailsDay = $("#ContentPlaceHolder1_ddlItemDetailsDay");

            DetailsDay.empty();
            ItemDetailsDay.empty();

            if (duarionCount > 0) {

                DetailsDay.removeAttr("disabled");
                DetailsDay.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');

                ItemDetailsDay.removeAttr("disabled");
                ItemDetailsDay.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');

                for (row = 0; row < duarionCount; row++) {
                    DetailsDay.append('<option title="' + DayDuration[row].DayCaption + '" value="' + DayDuration[row].PartyDay + '">' + DayDuration[row].DayCaption + '</option>');
                    ItemDetailsDay.append('<option title="' + DayDuration[row].DayCaption + '" value="' + DayDuration[row].PartyDay + '">' + DayDuration[row].DayCaption + '</option>');
                }
            }
        }

    </script>
    <asp:HiddenField ID="hfCostCenterId" runat="server" />
    <asp:HiddenField ID="hfMinCheckInDate" runat="server" />
    <asp:HiddenField ID="hfSaveObj" runat="server" />
    <asp:HiddenField ID="hfEditObj" runat="server" />
    <asp:HiddenField ID="hfDeleteObj" runat="server" />
    <asp:HiddenField ID="txtReservationId" runat="server" />
    <asp:HiddenField ID="txtHiddenItemId" runat="server" />
    <asp:HiddenField ID="hfDetailId" runat="server" />
    <asp:HiddenField ID="hfIsBanquetBillInclusive" runat="server" />
    <asp:HiddenField ID="hfBanquetVatAmount" runat="server" />
    <asp:HiddenField ID="hfBanquetServiceCharge" runat="server" />
    <asp:HiddenField ID="hfClassificationDiscountSave" runat="server" />
    <asp:HiddenField ID="hfClassificationDiscountDelete" runat="server" />
    <asp:HiddenField ID="hfClassificationDiscountAlreadySave" runat="server" />
    <asp:HiddenField ID="hfIsBanquetBillAmountWillRound" runat="server" />
    <asp:HiddenField ID="hfDuplicateReservarionValidation" runat="server" />
    <asp:HiddenField ID="hfReservationId" runat="server" />
    <asp:HiddenField ID="hfBanquetId" runat="server" />
    <div id="SeatingPlanDialog" style="display: none;">
        <div class="">
            <div class="form-group" id="ImagePanel">
                <div id="SigDiv" style="width: 250px; height: 260px;">
                </div>
            </div>
            <div class="divClear">
            </div>
        </div>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Reservation Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Reservation</a></li>
        </ul>
        <div id="tab-1">

            <div class="panel panel-default">
                <div class="panel-heading">Reservation Basic</div>
                <div class="panel-body">
                    <div class="form-horizontal">

                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label required-field"
                                    Text="Date From"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReservationDateFrom" runat="server" CssClass="form-control" TabIndex="12"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field"
                                    Text="Date To"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-9">
                                        <asp:TextBox ID="txtReservationDateTo" runat="server" CssClass="form-control" TabIndex="12"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3 col-padding-left-none">
                                        <asp:TextBox ID="txtReservationDuration" CssClass="form-control" runat="server" ReadOnly="true"
                                            ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblReservationMode" runat="server" class="control-label required-field" Text="Reservation Mode"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlReservationMode" runat="server" CssClass="form-control"
                                    TabIndex="11">
                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="Company">Company</asp:ListItem>
                                    <asp:ListItem Value="Personal">Personal</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblRefferenceId" runat="server" class="control-label" Text="Reference Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRefferenceId" runat="server" CssClass="form-control" TabIndex="11">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="ListedCompanyInfo" class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblName" runat="server" class="control-label" Text="Listed Company"></asp:Label>
                                <asp:CheckBox ID="chkIsLitedCompany" runat="server" Text="" onclick="javascript: return ToggleFieldVisibleForListedCompany(this);"
                                    TabIndex="8" />
                            </div>
                            <div class="col-md-10">
                                <div id="ListedCompany" style="display: none;">
                                    <input id="txtCompanyId" type="text" class="form-control" />
                                    <div style="display: none;">
                                        <asp:DropDownList ID="ddlCompanyId" runat="server" CssClass="form-control" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div id="ReservedCompany">
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCityName" runat="server" class="control-label" Text="Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TabIndex="2"
                                    TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div id="ContactPersonDiv" class="form-group" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="lblContactPerson" runat="server" class="control-label" Text="Contact Person"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" TabIndex="8"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblContactEmail" runat="server" class="control-label" Text="Contact Email"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtContactEmail" runat="server" CssClass="form-control" TabIndex="9"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPhoneNumber" runat="server" class="control-label" Text="Phone Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblContactPhone" runat="server" class="control-label" Text="Mobile Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtContactPhone" runat="server" CssClass="form-control" TabIndex="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAddress" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="19"></asp:TextBox>
                            </div>
                        </div>

                    </div>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-heading">Date Wise Reservation Details</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <fieldset>
                            <legend>Reservation Details</legend>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblArriveDate" runat="server" class="control-label required-field"
                                        Text="Party Date"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPartyDate" runat="server" CssClass="form-control" TabIndex="12"></asp:TextBox>
                                </div>

                                <div class="col-md-1" style="padding-right: 0">
                                    <asp:Label ID="lblProbableArrivalTime" runat="server" class="control-label" Text="Start Time"></asp:Label>
                                </div>
                                <div class="col-md-1">
                                    <asp:TextBox ID="txtProbableArrivalHour" placeholder="12" CssClass="form-control"
                                        runat="server" TabIndex="2"></asp:TextBox>
                                </div>
                                <div class="col-md-1" style="padding-right: 0">
                                    <asp:Label ID="lblProbableDepartureTime" runat="server" class="control-label" Text="End Time"></asp:Label>
                                </div>
                                <div class="col-md-1">
                                    <asp:TextBox ID="txtProbableDepartureHour" placeholder="12" CssClass="form-control"
                                        runat="server" TabIndex="2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblOccessionTypeId" runat="server" class="control-label required-field"
                                        Text="Occasion Type"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlOccessionTypeId" CssClass="form-control" runat="server"
                                        TabIndex="14">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblBanquetId" runat="server" class="control-label required-field"
                                        Text="Banquet Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlBanquetId" runat="server" TabIndex="15" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblBanquetRate" runat="server" class="control-label required-field"
                                        Text="Hall Rate"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:HiddenField ID="hfBanquetRate" runat="server" />
                                    <asp:TextBox ID="txtBanquetRate" runat="server" CssClass="form-control quantitydecimal" TabIndex="29"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblSeatingId" runat="server" class="control-label required-field"
                                        Text="Layout Type"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlSeatingId" runat="server" CssClass="form-control" TabIndex="16">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-1" style="padding-left: 0">
                                    <span style="margin-left: 3px;">
                                        <img src='../Images/service.png' title='Seat Plan' style="cursor: pointer;" onclick='javascript:return ShowUploadedDocument()'
                                            alt='Add Service' border='0' /></span>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblNumberOfPersonAdult" runat="server" class="control-label required-field"
                                        Text="Number Of Adult"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNumberOfPersonAdult" runat="server" CssClass="form-control" TabIndex="17"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblNumberOfPersonChild" runat="server" class="control-label" Text="Number Of Child"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNumberOfPersonChild" runat="server" CssClass="form-control" TabIndex="18"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <div class="col-md-2">
                                    <asp:Label ID="CountryId" runat="server" class="control-label required-field" Text="Country"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlCountryId" runat="server" CssClass="form-control" TabIndex="4">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Same As"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlDetailsDay" runat="server" CssClass="form-control" TabIndex="4">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </fieldset>

                        <fieldset>
                            <legend>Item Details</legend>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server" />
                                    <asp:Label ID="lblItemType" runat="server" class="control-label required-field" Text="Category Name"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlItemType" runat="server" CssClass="form-control" TabIndex="20">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblItemId" runat="server" class="control-label required-field" Text="Item Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <input id="txtItemId" type="text" class="form-control" />
                                    <div style="display: none;">
                                        <asp:HiddenField ID="txtHiddenProductId" runat="server" />
                                        <asp:DropDownList ID="ddlItemId" runat="server" CssClass="form-control" TabIndex="21">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblUnitPrice" runat="server" class="control-label required-field"
                                        Text="Unit Price"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control quantitydecimal" TabIndex="22"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblItemUnit" runat="server" class="control-label required-field" Text="Quantity"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtItemUnit" runat="server" CssClass="form-control quantitydecimal" TabIndex="23"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                </div>
                                <div class="col-md-4">
                                    <div style="float: left">
                                        <asp:CheckBox ID="chkIscomplementary" TabIndex="24" runat="Server" Text=" Is it a complementary Item?"
                                            Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Same As"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlItemDetailsDay" runat="server" CssClass="form-control" TabIndex="4">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6">
                                    <input type="button" id="btnAddDetailItem" value="Add Item / Requisities"
                                        class="TransactionalButton btn btn-primary btn-sm" onclick="AddItemForReservation()" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-12">
                                    <table id="ItemInformation" class="table table-bordered table-condensed table-hover table-responsive">
                                        <thead>
                                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                                <th style="width: 45%;">Item Name</th>
                                                <th style="width: 15%;">Unit Price</th>
                                                <th style="width: 15%;">Unit</th>
                                                <th style="width: 15%;">Amount</th>
                                                <th style="width: 10%;">Action</th>
                                                <th style="display: none"></th>
                                                <th style="display: none"></th>
                                                <th style="display: none"></th>
                                                <th style="display: none"></th>
                                                <th style="display: none"></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div id="TotalPaid" class="totalAmout">
                            </div>
                            <div id="DueTotal" class="totalAmout">
                            </div>
                        </fieldset>

                        <div class="form-group">
                            <div class="col-md-12">
                                <input type="button" id="btnAddReservationDetails" value="Add Details"
                                    class="TransactionalButton btn btn-primary btn-sm" onclick="AddReservationDetails()" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-heading">
                    Reservation Details
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-12">
                                <table id="ReservationDetailsTable" class="table table-bordered table-condensed table-hover table-responsive">
                                    <thead>
                                        <tr style="height: 50px;">
                                            <th style="width: 8%; vertical-align: middle;">Date</th>
                                            <th style="width: 5%; vertical-align: middle;">Start Time</th>
                                            <th style="width: 5%; vertical-align: middle;">End Time</th>
                                            <th style="width: 22%; vertical-align: middle;">Occation Type</th>
                                            <th style="width: 18%; vertical-align: middle;">Banquet Name</th>
                                            <th style="width: 18%; vertical-align: middle;">Layout Type</th>
                                            <th style="width: 5%; vertical-align: middle;">Number Of Adult</th>
                                            <th style="width: 5%; vertical-align: middle;">Number Of Child</th>
                                            <th style="width: 8%; vertical-align: middle;">Hall Rate</th>

                                            <th style="width: 6%; vertical-align: middle;">Action</th>

                                            <th style="display: none;">OccationTypeId</th>
                                            <th style="display: none;">BanquetId</th>
                                            <th style="display: none;">LayoutTypeId</th>

                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>

                                <table id="ItemDetailsInformation" class="table table-bordered table-condensed table-hover table-responsive">
                                    <thead>
                                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                            <th style="width: 10%; vertical-align: middle;">Date</th>
                                            <th style="width: 35%;">Item Name</th>
                                            <th style="width: 15%;">Unit Price</th>
                                            <th style="width: 15%;">Unit</th>
                                            <th style="width: 15%;">Amount</th>
                                            <th style="width: 10%;">Action</th>
                                            <th style="display: none"></th>
                                            <th style="display: none"></th>
                                            <th style="display: none"></th>
                                            <th style="display: none"></th>
                                            <th style="display: none"></th>
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

            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Reservation Payment
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="childDivSection">
                        </div>

                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblTotalAmount" runat="server" class="control-label" Text="Total Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:HiddenField ID="hfTotalAmount" runat="server" />
                                <asp:TextBox ID="txtTotalAmount" runat="server" TabIndex="26" CssClass="form-control"
                                    ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDiscountType" runat="server" class="control-label" Text="Discount Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlDiscountType" runat="server" CssClass="form-control" TabIndex="20">
                                    <asp:ListItem>Fixed</asp:ListItem>
                                    <asp:ListItem>Percentage</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblDiscountAmount" runat="server" class="control-label" Text="Discount Amount"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtDiscountAmount" runat="server" TabIndex="21" CssClass="form-control quantitydecimal"
                                    onblur="CalculateDiscountAmount()"></asp:TextBox>
                            </div>
                            <div class="col-md-1" style="padding-left: 0">
                                <span style="margin-left: 3px;">
                                    <img id="" src='../Images/service.png' title='Category Wise Discount' style="cursor: pointer;"
                                        onclick='javascript:return LoadCategorizedDiscountInfo()' alt='Category Wise Discount'
                                        border='0' /></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDiscountedAmount" runat="server" class="control-label" Text="After Discount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:HiddenField ID="hfDiscountedAmount" runat="server" />
                                <asp:TextBox ID="txtDiscountedAmount" runat="server" TabIndex="26" CssClass="form-control"
                                    ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                        <asp:Panel ID="pnlRackRateServiceChargeVatInformation" runat="server">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblServiceCharge" runat="server" class="control-label required-field"
                                        Text="Service Charge"></asp:Label>
                                </div>
                                <div class="col-md-3" style="padding-right: 0">
                                    <asp:TextBox ID="txtServiceCharge" runat="server" TabIndex="22" CssClass="form-control"
                                        Enabled="false"></asp:TextBox>
                                    <asp:HiddenField ID="hfServiceCharge" runat="server"></asp:HiddenField>
                                </div>
                                <div class="col-md-1">
                                    <asp:CheckBox ID="cbServiceCharge" runat="server" Text="" CssClass="customChkBox"
                                        onclick="javascript: return CalculateDiscountAmount();" TabIndex="8" Checked="True" />
                                </div>
                                <div class="col-md-2" style="padding-right: 0">
                                    <asp:Label ID="lblVatAmount" runat="server" class="control-label required-field"
                                        Text="Vat Amount"></asp:Label>
                                </div>
                                <div class="col-md-3" style="padding-right: 0">
                                    <asp:TextBox ID="txtVatAmount" runat="server" TabIndex="23" CssClass="form-control"
                                        Enabled="false"></asp:TextBox>
                                    <asp:HiddenField ID="hfVatAmount" runat="server"></asp:HiddenField>
                                </div>
                                <div class="col-md-1">
                                    <asp:CheckBox ID="cbVatAmount" runat="server" Text="" CssClass="customChkBox" onclick="javascript: return CalculateDiscountAmount();"
                                        TabIndex="8" Checked="True" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblGrandTotal" runat="server" class="control-label required-field"
                                        Text="Grand Total"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtGrandTotal" runat="server" TabIndex="22" CssClass="form-control"
                                        Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="form-group">
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                                <div style="float: left">
                                    <asp:CheckBox ID="chkIsReturnedGuest" TabIndex="30" runat="Server" Text="Is previously visited guest?"
                                        Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    TabIndex="31" OnClick="btnSave_Click" OnClientClick="javascript:return ValidationNPreprocess();" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    TabIndex="32" OnClick="btnCancel_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Banquet Reservation Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                <asp:Label ID="lblSearchName" runat="server" class="control-label" Text="Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSearchName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSearchReservationNumber" runat="server" class="control-label" Text="Reservation Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchReservationNumber" runat="server" CssClass="form-control"
                                    TabIndex="2"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSearchBanquetName" runat="server" class="control-label" Text="Banquet Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSearchBanquetName" runat="server" CssClass="form-control"
                                    TabIndex="3">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSearchEmail" runat="server" class="control-label" Text="Email"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchEmail" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSearchPhone" runat="server" class="control-label" Text="Contact Phone"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchPhone" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSearchArraiveDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchArraiveDate" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSearchDepartureDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchDepartureDate" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <button type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm">
                                    Search</button>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="SearchPanel" class="panel panel-default">
                    <div class="panel-heading">
                        Search Information
                    </div>
                    <div class="panel-body">
                        <table id='gvReservation' class="table table-bordered table-condensed table-responsive"
                            width="100%">
                            <colgroup>
                                <col style="width: 35%;" />
                                <col style="width: 20%;" />
                                <col style="width: 25%;" />
                                <col style="width: 10%;" />
                                <col style="width: 5%;" />
                                <col style="width: 5%;" />
                            </colgroup>
                            <thead>
                                <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                    <td>Company/ Person
                                    </td>
                                    <td>Reservation Number
                                    </td>
                                    <td>Email Address
                                    </td>
                                    <td>Status
                                    </td>
                                    <td style="text-align: right;">Action
                                    </td>
                                    <td></td>
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
    </div>
    <div id="CategoryWiseDiscountContainer" style="width: 450px; display: none;">
        <div id="CategoryWiseDiscountDiv">
        </div>
        <div id="Div4" style="padding-left: 5px; width: 100%; margin-top: 5px;">
            <div style="float: left; padding-bottom: 15px;">
                <input type="button" id="btnCategoryWiseDiscountOK" class="TransactionalButton btn btn-primary btn-sm"
                    style="width: 80px;" value="Ok" />
                <input type="button" id="btnCategoryWiseDiscountCancel" class="TransactionalButton btn btn-primary btn-sm"
                    style="width: 80px;" value="Cancel" />
            </div>
            <div id="ItemWiseSpecialRemarksContainer" class="alert alert-info" style="display: none;">
                <button type="button" class="close" data-dismiss="alert">
                    ×</button>
                <asp:Label ID='ItemWiseSpecialRemarksMessage' Font-Bold="True" runat="server" ClientIDMode="Static"></asp:Label>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var reserveMode = $("#<%=ddlReservationMode.ClientID %>").val();

            if (reserveMode == "Company") {
                $("#ContactPersonDiv").show();
                $("#<%=lblName.ClientID %>").text('Company Name');
                $("#<%=chkIsLitedCompany.ClientID %>").attr("Disabled", false);
            }
            else {
                $("#<%=lblName.ClientID %>").text('Contact Person');
                $("#ContactPersonDiv").hide();
                $("#<%=chkIsLitedCompany.ClientID %>").attr("Disabled", true);
            }

            var chkIsLitedCompany = '<%=chkIsLitedCompany.ClientID%>'
            if ($("#" + chkIsLitedCompany).is(":checked")) {
                $('#ReservedCompany').hide();
                $('#ListedCompany').show();
            }
            else {
                $('#ListedCompany').hide();
                $('#ReservedCompany').show();
            }

            $("#txtCompanyId").val($("#<%=txtName.ClientID %>").val());
        });

    </script>
</asp:Content>
