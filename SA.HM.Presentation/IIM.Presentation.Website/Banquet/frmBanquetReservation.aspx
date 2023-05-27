<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmBanquetReservation.aspx.cs" EnableEventValidation="false" Inherits="HotelManagement.Presentation.Website.Banquet.frmBanquetReservation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .mycheckbox input[type="checkbox"] {
            margin-right: 10px;
            padding-top: 5px;
        }
    </style>
    <script type="text/javascript">
        var ItemList = new Array();
        var AddedItemList = new Array();
        var minCheckInDate = "";
        var deleteObj = [];
        var editObj = [];
        var categoryWiseDiscount = new Array();
        var categoryWiseDiscountDeleted = new Array();
        var savedCategoryWiseDiscount = new Array();

        CommonHelper.ApplyDecimalValidation();

        var oo = {};

        function GetPrice() {
            var itemId = $("#ContentPlaceHolder1_ddlItemId").val();
            var item = _.findWhere(ItemList, { ItemId: parseInt(itemId, 10) });

            if (item != null) {
                $("#ContentPlaceHolder1_txtUnitPrice").val(item.UnitPriceLocal);

                if (item.IsItemEditable == true) {
                    $("#ContentPlaceHolder1_txtUnitPrice").attr('disabled', false);
                }
                else {
                    $("#ContentPlaceHolder1_txtUnitPrice").attr('disabled', true);
                }
            }
        }
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;

        $(document).ready(function () {

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfIsUpdatePermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Banquet Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Banquet Reservation</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            //$("#ContentPlaceHolder1_lblGrandTotal").text("Grand Total");

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                var am = JSON.parse($("#InnboardMessageHiddenField").val());
                //$("#InnboardMessageHiddenField").val("");

                if (am.RederictUrl != "") {
                    window.location = am.RederictUrl; //after update redirect to main url
                }
            }
            var eventType = $("#ContentPlaceHolder1_hfEventType").val();
            var glCompanyId = $("#ContentPlaceHolder1_hfGLCompanyId").val();
            var glProjectId = $("#ContentPlaceHolder1_hfGLProjectId").val();
            if (eventType != "0") {
                DivShowHideForEventType(eventType);
            }
            if (glCompanyId != "0") {
                PopulateProjects(glCompanyId);
            }
            if (glProjectId != "0") {
                $("#<%=ddlGLProject.ClientID %>").val(glProjectId);
            }
            var single = $("#ContentPlaceHolder1_hfIsSingle").val();
            if (single == "1") {
                $('#CompanyProjectDiv').hide();

            }
            else if (!IsCanView) {
                $('#CompanyProjectDiv').hide();
            }
            else {
                $('#CompanyProjectDiv').show();

            }

            $('#ContentPlaceHolder1_ddlParticipantFromClient').select2();

            var salesCallParticipantFromClient = new Array();
            var participantFromOffice = new Array();

            $("#ContentPlaceHolder1_ddlParticipantFromClient :selected").each(function () {
                salesCallParticipantFromClient.push({
                    ContactId: $(this).val(),
                    Contact: $(this).text()
                });
            });

            $("#ContentPlaceHolder1_ddlParticipantFromClient").val(salesCallParticipantFromClient).trigger('change');
            CommonHelper.ApplyDecimalValidation();
            CommonHelper.AutoSearchClientDataSource("txtCompanyId", "ContentPlaceHolder1_ddlCompanyId", "ContentPlaceHolder1_ddlCompanyId");
            CommonHelper.AutoSearchClientDataSource("txtItemId", "ContentPlaceHolder1_ddlItemId", "ContentPlaceHolder1_ddlItemId");

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

            $('#ContentPlaceHolder1_txtProbableArrivalHour').timepicker({
                showMinutes: false,
                showPeriod: is12HourFormat,

            });
            $('#ContentPlaceHolder1_txtProbableDepartureHour').timepicker({
                showMinutes: false,
                showPeriod: is12HourFormat,
            });

            $('#ContentPlaceHolder1_txtItemArrivalTime').timepicker({
                showMinutes: false,
                showPeriod: is12HourFormat

            });

            if ($('#ContentPlaceHolder1_hfClassificationDiscountAlreadySave').val() != "") {
                categoryWiseDiscount = JSON.parse($('#ContentPlaceHolder1_hfClassificationDiscountAlreadySave').val());
                savedCategoryWiseDiscount = JSON.parse($('#ContentPlaceHolder1_hfClassificationDiscountAlreadySave').val());
            }
            $('#ContentPlaceHolder1_txtProbableArrivalHour').change(function () {
                var startTime = $("#ContentPlaceHolder1_txtProbableArrivalHour").val();
                var d = new Date(),
                    parts = startTime.match(/(\d+)\ (\w+)/),
                    strathours = /am/i.test(parts[2]) ? parseInt(parts[1], 10) : parseInt(parts[1], 10) + 12;
                d.setHours(strathours);

                var endTime = $('#ContentPlaceHolder1_txtProbableDepartureHour').val();
                var e = new Date(),
                    parts = endTime.match(/(\d+)\ (\w+)/),
                    endhours = /am/i.test(parts[2]) ? parseInt(parts[1], 10) : parseInt(parts[1], 10) + 12;
                e.setHours(endhours);
                if (Date.parse(d) >= Date.parse(e)) {
                    //strathours = strathours == 24 ? 1 : strathours + 1;
                    if (strathours == 24)
                        strathours = 1;
                    else if (strathours == 23)
                        strathours = 12;
                    else
                        strathours = strathours + 1;
                    $('#ContentPlaceHolder1_txtProbableDepartureHour').val(moment(strathours, ["HH"]).format("hh A"));
                    var x = $('#ContentPlaceHolder1_txtProbableDepartureHour').val();
                }

            });
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
            var txtArriveDate = '<%=txtArriveDate.ClientID%>'
            var txtSearchArraiveDate = '<%=txtSearchArraiveDate.ClientID%>'
            var txtSearchDepartureDate = '<%=txtSearchDepartureDate.ClientID%>'

            $("#ContentPlaceHolder1_txtArriveDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                minDate: minCheckInDate
            });

            $("#ContentPlaceHolder1_txtSearchArraiveDate").datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtSearchDepartureDate").datepicker("option", "minDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_txtSearchDepartureDate").datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtSearchArraiveDate").datepicker("option", "maxDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $("#<%=ddlDiscountType.ClientID %>").change(function () {

                if ($("#ContentPlaceHolder1_txtDiscountAmount").val() == "") { return; }

                TotalRoomRateVatServiceChargeCalculation();
            });
            $("#<%=ddlEventTypeId.ClientID %>").change(function () {
                var type = $("#<%=ddlEventTypeId.ClientID %>").val();
                $("#<%=hfEventType.ClientID %>").val(type);
                DivShowHideForEventType(type);
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

            $('#ContentPlaceHolder1_txtBanquetRate').blur(function () {
                if ($(this).val() == "") {
                    toastr.warning('Entered Hall Rate is not in correct format.');
                    return;
                }

                $("#ContentPlaceHolder1_hfBanquetRate").val($('#ContentPlaceHolder1_txtBanquetRate').val());
                CalculateTotalPaiedNDueAmount();
            });

            var txtArriveDate = '<%=txtArriveDate.ClientID%>'
            $('#' + txtArriveDate).change(function () {
                DuplicateDataChecking();
            });
            var txtProbableArrivalHour = '<%=txtProbableArrivalHour.ClientID%>'
            $('#' + txtProbableArrivalHour).change(function () {
                DuplicateDataChecking();
            });
            var txtProbableDepartureHour = '<%=txtProbableDepartureHour.ClientID%>'
            $('#' + txtProbableDepartureHour).change(function () {
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
                $('#txtItemId').val('');

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
            $("#ContentPlaceHolder1_ddlEmployeeId").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#btnAddDetailItem").click(function () {
                var add = false;
                var reservationId = $("#ContentPlaceHolder1_txtReservationId").val();
                if (reservationId == "") {
                    reservationId = 0;
                }
                var x = document.getElementById("ContentPlaceHolder1_chkIscomplementary").checked;

                if ($("#<%=ddlBanquetId.ClientID %>").val() == "0") {
                    toastr.warning("Please Select Hall Name.");
                    return;
                }
                else if ($("#<%=ddlItemType.ClientID %>").val() == "0") {
                    toastr.warning("Please Select Category Name.");
                    return;
                }
                else if ($("#<%=ddlItemId.ClientID %>").val() == "0") {
                    toastr.warning("Please Select Item Name.");
                    return;
                }

                if ($("#<%=txtUnitPrice.ClientID %>").val() == "0") {
                    if (x == true) {
                        add = true;
                    }
                    else {
                        toastr.warning("Please Provide Unit Price.");
                        return;
                    }
                }
                if ($("#<%=txtUnitPrice.ClientID %>").val() != "0") {
                    var decimalOnly = /^\s*-?[1-9]\d*(\.\d{1,2})?\s*$/;
                    if (decimalOnly.test($("#<%=txtUnitPrice.ClientID %>").val())) {
                        add = true;
                    }
                    else {
                        toastr.warning("Wrong input for Unit Price.");
                        return;
                    }
                }
                if ($("#<%=txtItemUnit.ClientID %>").val() == "") {
                    toastr.warning("Please Provide Item Quantity.");
                    return;
                }
                if ($("#<%=txtItemUnit.ClientID %>").val() != "") {
                    <%--var numbersOnly = /^\d+$/;
                    if (numbersOnly.test($("#<%=txtItemUnit.ClientID %>").val())) {
                        add = true;
                    }
                    else {
                        toastr.warning("Wrong input for Item Unit.");
                        return;
                    }--%>
                    add = true;
                }
                else {
                    toastr.warning("Please provide Item Unit.");
                    return;
                }
                if ($("#<%=txtItemArrivalTime.ClientID %>").val() == "") {
                    $("#<%=txtItemArrivalTime.ClientID %>").focus();
                    toastr.warning("Please Provide Item Delivery time.");
                    add = false;
                    return;
                }
                else {
                    var arrivalTime = $("#ContentPlaceHolder1_txtItemArrivalTime").val();
                    var d = new Date(),
                        parts = arrivalTime.match(/(\d+)\ (\w+)/),
                        strathours = /am/i.test(parts[2]) ? parseInt(parts[1], 10) : parseInt(parts[1], 10) + 12;
                    d.setHours(strathours);

                    var endTime = $('#ContentPlaceHolder1_txtProbableDepartureHour').val();
                    var e = new Date(),
                        parts = endTime.match(/(\d+)\ (\w+)/),
                        endhours = /am/i.test(parts[2]) ? parseInt(parts[1], 10) : parseInt(parts[1], 10) + 12;
                    e.setHours(endhours);
                    if (Date.parse(d) >= Date.parse(e)) {
                        toastr.warning("Please provide item delivery time before End time.");
                        $("#ContentPlaceHolder1_txtItemArrivalTime").focus();
                        return false;
                    }
                }
                if (add == true) {
                    AddItemDetailsInfo(0, reservationId);
                    $("#txtItemId").val("");
                    $("#txtItemId").focus();
                }
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

            var reserveMode = $("#<%=ddlReservationMode.ClientID %>").val();
            if (reserveMode == "Company") {
                $("#ReserveModeDetails").show("slow");
                ShowCompany();
                var chkIsLitedCompany = '<%=chkIsLitedCompany.ClientID%>'
                if ($("#" + chkIsLitedCompany).is(":checked")) {
                    $('#ReservedCompany').hide();
                    $('#ListedCompany').show();
                }
                else {
                    $('#ListedCompany').hide();
                    $('#ReservedCompany').show();
                }
            }
            else if (reserveMode == "Personal") {
                $("#ReserveModeDetails").show("slow");
                ShowPersonal();
            }
            else {
                $("#ReserveModeDetails").hide("slow");
            }

            var ddlReserveMode = '<%=ddlReservationMode.ClientID%>'
            $("#" + ddlReserveMode).change(function () {
                var reserveMode = $("#<%=ddlReservationMode.ClientID %>").val();
                if (reserveMode == "Company") {
                    $("#ReserveModeDetails").show("slow");
                    ShowCompany();
                    $("#chkIsLitedCompanyDiv").show();

                    var chkIsLitedCompany = '<%=chkIsLitedCompany.ClientID%>'
                    if ($("#" + chkIsLitedCompany).is(":checked")) {
                        $('#ReservedCompany').hide();
                        $('#ListedCompany').show();
                    }
                    else {
                        $('#ListedCompany').hide();
                        $('#ReservedCompany').show();
                    }
                }
                else if (reserveMode == "Personal") {
                    $("#ReserveModeDetails").show("slow");
                    ShowPersonal();
                    $("#chkIsLitedCompanyDiv").hide();
                }
                else {
                    $("#ReserveModeDetails").hide("slow");
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
            $("#ContentPlaceHolder1_ddlGLCompany").change(function () {
                var glCompanyId = $("#<%=ddlGLCompany.ClientID %>").val();
                if (glCompanyId != 0) {
                    PopulateProjects(glCompanyId);
                }
            });
            $("#ContentPlaceHolder1_ddlGLProject").change(function () {
                var id = $("#<%=ddlGLProject.ClientID %>").val();
                if (id != 0) {
                    $("#<%=hfGLProjectId.ClientID %>").val(id);
                }
            });

            $("#txtCompanyId").blur(function () {
                var companyId = $("#<%=ddlCompanyId.ClientID %>").val();
                if (companyId > 0) {
                    PageMethods.GetAffiliatedCompany(companyId, GetAffiliatedCompanyObjectSucceeded, GetAffiliatedCompanyObjectFailed);
                    return false;
                }
                else if (companyId == 0) {
                    toastr.warning("Please select a Company Name.");
                    return false;
                }
                else {
                    toastr.warning("Please provide an enlisted company.");
                    return false;
                }
            });

            //var employeeIdArray = "2,3,5, 6,7";
            var employeeIdArray = $("#ContentPlaceHolder1_hfparticipantFromOfficeValue").val();
            var arrayArea = employeeIdArray.split(',');
            $("#ContentPlaceHolder1_ddltxtParticipantFromOffice").select2({
                multiple: true,
            });
            $('#ContentPlaceHolder1_ddltxtParticipantFromOffice').val(arrayArea).trigger('change');

        });
        function PopulateProjects(companyId) {
            $.ajax({
                type: "POST",
                url: "frmBanquetReservation.aspx/GetGLProjectByGLCompanyId",
                data: JSON.stringify({ companyId: companyId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    PopulateControlWithValueNTextField(response.d, $("#ContentPlaceHolder1_ddlGLProject"), $("#<%=CommonDropDownHiddenField.ClientID %>").val(), "Name", "ProjectId");
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
            }
            function DivShowHideForEventType(type) {
                if (type == "Internal") {
                    $(".DivHideForEventType").hide();
                    $(".DivShowForEventType").show();
                    $("#MeetingAgendaDiv").show();
                    $("#NumberOfPersonAdultLabelDiv").hide();
                    $("#NumberOfPersonAdultControlDiv").hide();
                    $("#ParticipantFromOfficeDiv").show();
                    $("#<%=lblRemarks.ClientID %>").text("Description");
                    $("#<%=lblNumberOfPersonAdult.ClientID %>").text("Number Of Person");
                }
                else {
                    $("#MeetingAgendaDiv").hide();                    
                    $(".DivHideForEventType").show();
                    $(".DivShowForEventType").hide();
                    $("#ParticipantFromOfficeDiv").hide();
                    $("#NumberOfPersonAdultLabelDiv").show();
                    $("#NumberOfPersonAdultControlDiv").show();
                    $("#<%=lblNumberOfPersonAdult.ClientID %>").text("Number Of Adult");
                }
            }
            function ShowPersonal() {
                $("#<%=lblName.ClientID %>").text('Contact Person');
                $("#ContactPersonDiv").hide();
                $("#<%=chkIsLitedCompany.ClientID %>").attr("Disabled", true);
                $("#<%=chkIsLitedCompany.ClientID %>").prop("checked", false);
                $("#ListedCompany").hide();
                $("#ReservedCompany").show();
                $("#txtCompanyId").val("");
            }
            function ShowCompany() {
                $("#ContactPersonDiv").show();
                $("#<%=lblName.ClientID %>").text('Company Name');
                $("#<%=chkIsLitedCompany.ClientID %>").attr("Disabled", false);
                $("#ListedCompany").show();
                $("#ReservedCompany").hide();
            }
            function GetAffiliatedCompanyObjectSucceeded(result) {
                $("#<%=txtAddress.ClientID %>").val(result.CompanyAddress)
            $("#<%=txtContactPerson.ClientID %>").val(result.ContactPerson)
            $("#<%=txtContactEmail.ClientID %>").val(result.EmailAddressWithoutLabel)
            $("#<%=txtPhoneNumber.ClientID %>").val(result.TelephoneNumber)
            $("#<%=txtContactPhone.ClientID %>").val(result.ContactNumberWithoutLabel)
            return false;
        }
        function GetAffiliatedCompanyObjectFailed(error) {
            toastr.error(error.get_message());
        }

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
            var editId = $('#ContentPlaceHolder1_hfEditedId').val() == "" ? "0" : $('#ContentPlaceHolder1_hfEditedId').val();
            var banquetId = $.trim($("#<%=ddlBanquetId.ClientID %>").val());
            var startDate = $.trim($("#<%=txtArriveDate.ClientID %>").val());
            var arriveTime = $.trim($("#<%=txtProbableArrivalHour.ClientID %>").val());
            var departTime = $.trim($("#<%=txtProbableDepartureHour.ClientID %>").val());
            PageMethods.GetBanquetReservationInfoForDuplicateChecking(editId, banquetId, startDate, arriveTime, departTime, OnGetBanquetReservationInfoForDuplicateCheckingSucceeded, OnGetBanquetReservationInfoForDuplicateCheckingFailed);
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
        }

        function AddItemDetailsInfo(id, reservationId) {
            var itemTypeId = $("#<%=ddlItemType.ClientID %>").val();
            var itemId = $("#<%=ddlItemId.ClientID %>").val();
            var unitPrice = $("#<%=txtUnitPrice.ClientID %>").val();
            var itemUnit = $("#<%=txtItemUnit.ClientID %>").val();
            var isComplementary = $("#ContentPlaceHolder1_chkIscomplementary").is(":checked");
            var itemArrivalTime = $("#<%=txtItemArrivalTime.ClientID %>").val();
            var itemDescription = $("#<%=txtItemDescription.ClientID %>").val();

            var selectedItem = {};
            var isItemId = CommonHelper.IsInt(itemId);
            if (isItemId == false) {
                toastr.warning("This Item is not available in this Catagory. Please add item first.");
                $("#ContentPlaceHolder1_ddlItemType").focus();
                return false;
            }
            if (ItemList.length == 0) {
                toastr.warning("There is no Item in this Catagory. Please add item first.");
                $("#ContentPlaceHolder1_ddlItemType").focus();
                return false;
            }

            if (itemId != "0") {
                selectedItem = _.findWhere(ItemList, { ItemId: parseInt(itemId, 10) });
            }

            if ($('#' + unitPrice).val() != 0) {

                var addedItem = _.findWhere(AddedItemList, { ItemId: parseInt(itemId, 10), ItemTypeId: parseInt(itemTypeId, 10) });

                if (addedItem != null) {
                    toastr.info("Same Item Is Added Already. Duplicate Item Cannot Added.");
                    return false;
                }

                AddedItemList.push({
                    ItemId: parseInt(itemId, 10),
                    ItemTypeId: parseInt(itemTypeId, 10),
                    UnitPrice: parseFloat(unitPrice, 10),
                    ItemUnit: parseFloat(itemUnit, 10),
                    IsComplementary: isComplementary,
                    ItemArrivalTime: itemArrivalTime,
                    ItemDescription: itemDescription
                });

                var calculatedAmount = 0.00; //calculat complementary
                if ($("#ContentPlaceHolder1_chkIscomplementary").is(":checked") == false) {
                    calculatedAmount = parseFloat(itemUnit) * parseFloat(unitPrice);
                }

                if ($("#ltlTableWiseItemInformation > table").length > 0 && reservationId == 0) {
                    AddNewRow(id, reservationId, itemTypeId, itemId, unitPrice, itemUnit, calculatedAmount, isComplementary, itemArrivalTime, itemDescription);//flase
                    return false;
                }
                else if ($("#ltlTableWiseItemInformation > table").length > 0 && reservationId > 0) {
                    AddNewRow(id, reservationId, itemTypeId, itemId, unitPrice, itemUnit, calculatedAmount, isComplementary, itemArrivalTime, itemDescription);
                    return false;
                }

                var table = "", deleteLink = "", editLink = "", arrivalTime = "";
                arrivalTime = itemArrivalTime;
                deleteLink = "<a href=\"#\" onclick= 'DeleteDetailInfo(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
                table += "<table id='RecipeItemInformation' class='table table-bordered table-condensed table-hover' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                table += "<th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th align='left' scope='col' style='width: 20%;'>Item Name</th><th align='left' scope='col' style='width: 15%;'>Unit Price</th><th align='left' scope='col' style='width: 15%;'>Unit</th><th align='left' scope='col' style='width: 15%;'>Amount</th><th align='left' scope='col' style='width: 10%;'>Arrival Time</th><th align='left' scope='col' style='width: 15%;'>Description</th><th align='center' scope='col' style='width: 10%;'>Action</th></tr></thead>";

                table += "<tbody>";
                table += "<tr>";

                if (selectedItem.IsItemEditable) {
                    table += "<td style=\"width:20%;\">";
                    table += "<input type=\"text\" id=\"itmname" + itemId + "\"class=\"form-control\" value=\"" + $("#<%=ddlItemId.ClientID %> option:selected").text() + "\"/>";
                    table += "</td>";
                }
                else {
                    table += "<td align='left' style=\"width:20%; text-align:Left;\">" + $("#<%=ddlItemId.ClientID %> option:selected").text() + "</td>";
                }

                if (selectedItem.IsItemEditable) {
                    table += "<td style=\"width:15%;\">";
                    table += "<input type=\"text\" id=\"itmprice" + itemId + "\"class=\"form-control quantitydecimal\" value=\"" + unitPrice + "\" onblur=\"PriceQuantityChange(this, 'price')\" />";
                    table += "</td>";
                }
                else {
                    table += "<td align='left' style=\"width:15%; text-align:Left;\">" + unitPrice + "</td>";
                }

                table += "<td style=\"width:15%;\">";
                table += "<input type=\"text\" id=\"itmqtn" + itemId + "\"class=\"form-control quantitydecimal\" value=\"" + itemUnit + "\" onblur=\"PriceQuantityChange(this, 'quantity')\"/>";
                table += "</td>";

                table += "<td align='left' style=\"width:15%; text-align:Left;\">" + calculatedAmount + "</td>";
                table += "<td align='left' style=\"width:10%; cursor:pointer;\">" + arrivalTime + "</td>";

                table += "<td align='left' style=\"display:none;\">" + id + "</td>";
                table += "<td align='left' style=\"display:none;\">" + reservationId + "</td>";
                table += "<td align='left' style=\"display:none;\">" + itemTypeId + "</td>";
                table += "<td align='left' style=\"display:none;\">" + $("#<%=ddlItemType.ClientID %> option:selected").text() + "</td>";
                table += "<td align='left' style=\"display:none;\">" + itemId + "</td>";

                table += "<td align='left' style=\"display:none;\">" + selectedItem.ItemWiseDiscountType + "</td>";
                table += "<td align='left' style=\"display:none;\">" + selectedItem.ItemWiseIndividualDiscount + "</td>";
                table += "<td align='left' style=\"display:none;\">" + selectedItem.ServiceCharge + "</td>";
                table += "<td align='left' style=\"display:none;\">" + selectedItem.CitySDCharge + "</td>";
                table += "<td align='left' style=\"display:none;\">" + selectedItem.VatAmount + "</td>";
                table += "<td align='left' style=\"display:none;\">" + selectedItem.AdditionalChargeType + "</td>";
                table += "<td align='left' style=\"display:none;\">" + selectedItem.AdditionalCharge + "</td>";
                table += "<td align='left' style=\"display:none;\">" + isComplementary + "</td>"; //17
                table += "<td align='left' style=\"width:15%\">" + itemDescription + "</td>";//18
                table += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";
                table += "</tr>";
                table += "</tbody>";
                table += "</table>";

                $("#ltlTableWiseItemInformation").html(table);

                CalculateTotalPaiedNDueAmount();

                $("#<%=ddlItemId.ClientID %>").val('0');
                $("#<%=txtUnitPrice.ClientID %>").val('');
                $("#<%=txtItemUnit.ClientID %>").val('');
                $("#<%=txtItemDescription.ClientID %>").val('');
                $("#<%=txtItemArrivalTime.ClientID %>").val('');
                $("#ContentPlaceHolder1_chkIscomplementary").prop('checked', false);

                CommonHelper.ApplyDecimalValidation();
            }
        }
        function AddNewRow(id, reservationId, itemTypeId, itemId, unitPrice, itemUnit, calculatedAmount, isComplementary, itemArrivalTime, itemDescription) {
            var tr = "", totalRow = 0, deleteLink = "", editLink = "";
            totalRow = $("#RecipeItemInformation tbody tr").length;

            var selectedItem = {};

            if (itemId != "0") {
                selectedItem = _.findWhere(ItemList, { ItemId: parseInt(itemId, 10) });
            }

            deleteLink = "<a href=\"#\" onclick= 'DeleteDetailInfo(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
            tr += "<tr>";

            if (selectedItem.IsItemEditable) {
                tr += "<td style=\"width:45%;\">";
                tr += "<input type=\"text\" id=\"itmname" + itemId + "\"class=\"form-control\" value=\"" + $("#<%=ddlItemId.ClientID %> option:selected").text() + "\"/>";
                tr += "</td>";
            }
            else {
                tr += "<td align='left' style=\"width:30%; text-align:Left;\">" + $("#<%=ddlItemId.ClientID %> option:selected").text() + "</td>";
            }

            if (selectedItem.IsItemEditable) {
                tr += "<td style=\"width:15%;\">";
                tr += "<input type=\"text\" id=\"itmprice" + itemId + "\"class=\"form-control quantitydecimal\" value=\"" + unitPrice + "\" onblur=\"PriceQuantityChange(this, 'price')\"/>";
                tr += "</td>";
            }
            else {
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + unitPrice + "</td>";
            }

            tr += "<td style=\"width:15%;\">";
            tr += "<input type=\"text\" id=\"itmqtn" + itemId + "\"class=\"form-control quantitydecimal\" value=\"" + itemUnit + "\" onblur=\"PriceQuantityChange(this, 'quantity')\"/>";
            tr += "</td>";

            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + calculatedAmount + "</td>";
            tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + itemArrivalTime + "</td>";

            tr += "<td align='left' style=\"display:none;\">" + id + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + reservationId + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + itemTypeId + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + $("#<%=ddlItemType.ClientID %> option:selected").text() + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + itemId + "</td>";

            tr += "<td align='left' style=\"display:none;\">" + selectedItem.ItemWiseDiscountType + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + selectedItem.ItemWiseIndividualDiscount + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + selectedItem.ServiceCharge + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + selectedItem.CitySDCharge + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + selectedItem.VatAmount + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + selectedItem.AdditionalChargeType + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + selectedItem.AdditionalCharge + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + isComplementary + "</td>";
            tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + itemDescription + "</td>";
            tr += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";
            tr += "</tr>";

            $("#RecipeItemInformation tbody").append(tr);
            CalculateTotalPaiedNDueAmount();
            $("#<%=ddlItemId.ClientID %>").val('0');
            $("#<%=txtUnitPrice.ClientID %>").val('');
            $("#<%=txtItemUnit.ClientID %>").val('');
            $("#<%=txtItemDescription.ClientID %>").val('');
            $("#<%=txtItemArrivalTime.ClientID %>").val('');
            $("#ContentPlaceHolder1_chkIscomplementary").prop('checked', false);

            CommonHelper.ApplyDecimalValidation();
        }
        //price calculation in table
        function PriceQuantityChange(control, actionType) {
            var tr = $(control).parent().parent();
            var unitPrice = 0.00, itemUnit = 0.00, totalPrice = 0.00;
            var isComplementary = "";

            if ($(tr).find("td:eq(1)").find("input").is(":input")) {
                unitPrice = parseFloat($(tr).find("td:eq(1)").find("input").val(), 10);
            }
            else {
                unitPrice = parseFloat($(tr).find("td:eq(1)").text(), 10);
            }

            if ($(tr).find("td:eq(2)").find("input").is(":input")) {
                itemUnit = parseFloat($(tr).find("td:eq(2)").find("input").val(), 10);
            }
            else {
                itemUnit = parseFloat($(tr).find("td:eq(2)").text(), 10);
            }
            isComplementary = $(tr).find("td:eq(17)").text();


            if ((isComplementary == "true") || (isComplementary == "True")) {
                totalPrice = 0.00;
                //toastr.info(isComplementary);
            }
            else {
                //toastr.warning(isComplementary);
                totalPrice = itemUnit * unitPrice;
            }

            $(tr).find("td:eq(3)").text(totalPrice);

            CalculateTotalPaiedNDueAmount();
        }

        function CalculateTotalAddedPayment() {
            var calculateAmount = 0;
            $("#RecipeItemInformation tbody tr").each(function () {
                calculateAmount += parseFloat($.trim($(this).find("td:eq(3)").text(), 10));
            });

            return calculateAmount;
        }

        function CalculateTotalPaiedNDueAmount() {
            var calculateAmount = 0, hfBanquetRate = 0;
            $("#RecipeItemInformation tbody tr").each(function () {
                calculateAmount += parseFloat($.trim($(this).find("td:eq(3)").text(), 10));
            });

            hfBanquetRate = parseFloat($.trim($("#<%= hfBanquetRate.ClientID %>").val()));
            $("#TotalPaid").text("Total Amount: " + calculateAmount);
            var totalCalculatedAmount = calculateAmount + hfBanquetRate;

            var txtTotalAmount = '<%=txtTotalAmount.ClientID%>'
            var hfTotalAmount = '<%=hfTotalAmount.ClientID%>'
            var txtDiscountedAmount = '<%=txtDiscountedAmount.ClientID%>'
            var hfDiscountedAmount = '<%=hfDiscountedAmount.ClientID%>'

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
            var startDate = $.trim($("#<%=txtArriveDate.ClientID %>").val());
            var occasionType = $.trim($("#<%=ddlOccessionTypeId.ClientID %>").val());
            var seatingName = $.trim($("#<%=ddlSeatingId.ClientID %>").val());
            <%--var noOfAdult = $.trim($("#<%=txtNumberOfPersonAdult.ClientID %>").val());--%>
            var arriveTime = $.trim($("#<%=txtProbableArrivalHour.ClientID %>").val());
            var departTime = $.trim($("#<%=txtProbableDepartureHour.ClientID %>").val());
            var ddlReservationMode = $.trim($("#<%=ddlReservationMode.ClientID %>").val());

            var participantFromOfficeValue = $("#<%=ddltxtParticipantFromOffice.ClientID %>").val();
            var participantFromOfficeValueStr = participantFromOfficeValue.join(",");
            $("#<%=hfparticipantFromOfficeValue.ClientID %>").val(participantFromOfficeValueStr);

            var hfDuplicateReservarionValidation = '<%=hfDuplicateReservarionValidation.ClientID%>'
            if ($('#' + hfDuplicateReservarionValidation).val() == "1") {
                toastr.warning('Already Reservation exist in your given time period.');
                return false;
            }
            var now = moment();
            var beginningTime = moment(arriveTime, 'h:mma');
            var endTime = moment(departTime, 'h:mma');

            var isValidEndTime = beginningTime.isAfter(endTime);
            var isSameTime = beginningTime.isSame(endTime);
            var isValidStart = beginningTime.isBefore(now);
            //var today = moment(new Date());

            var today2 = $("#ContentPlaceHolder1_hfToday").val();
            var nowDate = CommonHelper.DateFormatToYYYYMMDD(today2, '/');
            var reserveDate = CommonHelper.DateFormatToYYYYMMDD(startDate, '/');
            var isValidDate = moment(reserveDate).isSame(nowDate);
            //var isValidStartTime = endTime.isAfter(beginningTime);
            //new fields
            var IsCompanySingle = $("#ContentPlaceHolder1_hfIsSingle").val();
            var GLCompanyId = $.trim($("#<%=ddlGLCompany.ClientID %>").val());
            var GLProjectId = $.trim($("#<%=ddlGLProject.ClientID %>").val());
            var eventType = $.trim($("#<%=ddlEventTypeId.ClientID %>").val());
            var eventTitle = $("#<%=txtEventTitle.ClientID %>").val();
            var meetingAgenda = $("#<%=txtMeetingAgenda.ClientID %>").val();
            var officeparticipantno = $("#<%=hfparticipantFromOfficeValue.ClientID %>").val();
            if (IsCompanySingle == "0") {
                if (GLCompanyId == "0") {
                    toastr.warning("Please select a Company.");
                    $("#<%=ddlGLCompany.ClientID %>").focus();
                    return false;
                }
                else if (GLProjectId == "0") {
                    toastr.warning("Please select a Project.");
                    $("#<%=ddlGLProject.ClientID %>").focus();
                    return false;
                }
        }
        if (eventType == "0") {
            toastr.warning("Please select a Event Type.");
            $("#<%=ddlEventTypeId.ClientID %>").focus();
            return false;
        }
        if (isValidEndTime) {
            toastr.warning("Please insert valid time.");
            $("#ContentPlaceHolder1_txtProbableDepartureHour").focus();
            return false;
        }
        else if (isSameTime) {
            toastr.warning("Same time inserted. Please insert valid time.");
            $("#ContentPlaceHolder1_txtProbableDepartureHour").focus();
            return false;
        }
        else if ((isValidStart) && (isValidDate)) {
            toastr.warning("Your Arrival Time Has Passed. Please insert valid time.");
            $("#ContentPlaceHolder1_txtProbableArrivalHour").focus();
            return false;
        }

        var chkIsLitedCompany = '<%=chkIsLitedCompany.ClientID%>'
        if (banquetName == "0") {
            toastr.warning("Please Provide Hall Name.");
            $("#ContentPlaceHolder1_ddlBanquetId").focus();

            return false;
        }
        else if (startDate == "") {
            toastr.warning("Please Provide Party Start Date.");
            $("#ContentPlaceHolder1_txtArriveDate").focus();

            return false;
        }
        else if (occasionType == "0") {
            toastr.warning("Please Select Occasion Type.");
            $("#ContentPlaceHolder1_ddlOccessionTypeId").focus();

            return false;
        }
        else if (seatingName == "0" && eventType == "Rental") {
            toastr.warning("Please Select Layout Type.");
            $("#ContentPlaceHolder1_ddlSeatingId").focus();
            return false;
        }
        else if (ddlReservationMode == "0" && eventType == "Rental") {
            toastr.warning("Please Select Reservation Mode.");
            $("#ContentPlaceHolder1_ddlReservationMode").focus();
            return false;
        }
        else if (eventTitle == "" && eventType == "Internal") {
            toastr.warning("Please Insert Event Title.");
            $("#ContentPlaceHolder1_txtEventTitle").focus();
            return false;
        }
        else if (meetingAgenda == "" && eventType == "Internal") {
            toastr.warning("Please Insert Meeting Agenda.");
            $("#ContentPlaceHolder1_txtMeetingAgenda").focus();
            return false;
        }
        else if (officeparticipantno == "" && eventType == "Internal") {
            toastr.warning("Please select Participant From Office.");
            $("#ContentPlaceHolder1_ddltxtParticipantFromOffice").focus();
            return false;
        }
        else if (ddlReservationMode == "Personal") {
            if ($("#ContentPlaceHolder1_txtName").val() == "") {
                toastr.warning("Please provide a contact name");
                $("#ContentPlaceHolder1_txtName").focus();
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtContactPhone").val() == "") {
                toastr.warning("Please provide a contact number");
                $("#ContentPlaceHolder1_txtContactPhone").focus();
                return false;
            }
        }
        else if (ddlReservationMode == "Company") {
            var chkIsLitedCompany = '<%=chkIsLitedCompany.ClientID%>'
            if ($("#" + chkIsLitedCompany).is(":checked")) {
                var companyId = $("#<%=ddlCompanyId.ClientID %>").val();
                var isCompanyId = CommonHelper.IsInt(companyId);
                if (companyId == 0) {
                    toastr.warning("Please select a Company Name.");
                    return false;
                }
                else if (isCompanyId == false) {
                    toastr.warning("Please provide an enlisted company.");
                    return false;
                }
                if ($("#ContentPlaceHolder1_txtContactPhone").val() == "") {
                    toastr.warning("Please provide a contact number");
                    $("#ContentPlaceHolder1_txtContactPhone").focus();
                    return false;
                }
            }
        }
    if ($("#<%=txtContactEmail.ClientID %>").val() != "") {
                var status = CommonHelper.IsValidEmail($("#<%=txtContactEmail.ClientID %>").val());
        if (!status) {
            toastr.warning("Email Address is not valid.");
            $("#<%=txtContactEmail.ClientID %>").focus();
            return false;
        }
    }
    if ($("#<%=txtPhoneNumber.ClientID %>").val() != "") {
                var status = CommonHelper.IsValidPhone($("#<%=txtPhoneNumber.ClientID %>").val());
        if (!status) {
            toastr.warning("Please Provide Valid Phone Number");
            $("#<%=txtPhoneNumber.ClientID %>").focus();
            return false;
        }
    }
    if ($("#<%=txtContactPhone.ClientID %>").val() != "") {
                var status = CommonHelper.IsValidPhone($("#<%=txtContactPhone.ClientID %>").val());
        if (!status) {
            toastr.warning("Please Provide Valid Mobile Number.");
            $("#<%=txtContactPhone.ClientID %>").focus();
            return false;
        }
    }
    if ($("#<%=ddlBanquetId.ClientID %>").val() == "0") {
                toastr.warning("Please Select Banquet Name");
                $("#<%=ddlBanquetId.ClientID %>").focus();
                return false;
            }
            if ($("#<%=ddlOccessionTypeId.ClientID %>").val() == "0") {
                toastr.warning("Please Select Occasion Type.");
                $("#<%=ddlOccessionTypeId.ClientID %>").focus();
                return false;
            }
            if ($("#<%=ddlSeatingId.ClientID %>").val() == "0" && eventType == "Rental") {
                toastr.warning("Please Select Layout Type.");
                $("#<%=ddlSeatingId.ClientID %>").focus();
                return false;
            }
            if ($("#<%=txtArriveDate.ClientID %>").val() == "") {
                toastr.warning("Please enter Party Start Date.");
                $("#<%=txtArriveDate.ClientID %>").focus();
                return false;
            }
            if (arriveTime.length == 1) {
                arriveTime = "0" + arriveTime;
            }
            if (departTime.length == 1) {
                departTime = "0" + departTime;
            }

            var saveObj = new Array();
            var editObj = new Array();
            var detailId = 0, reservationId = 0, itemTypeId = 0, itemType = "", itemId = 0, itemName = "", itemUnit = 0, unitPrice = 0, totalPrice = 0;
            var isComplementary = "";
            var isCom = false;
            var itemArrivalTime = "";
            var itemDescription = "";

            var rowLength = $("#ltlTableWiseItemInformation > table tbody tr").length;

            $("#ltlTableWiseItemInformation > table tbody tr").each(function () {

                if ($(this).find("td:eq(0)").find("input").is(":input")) {
                    itemName = $(this).find("td:eq(0)").find("input").val();
                }
                else {
                    itemName = $(this).find("td:eq(0)").text();
                }

                if ($(this).find("td:eq(1)").find("input").is(":input")) {
                    unitPrice = parseFloat($(this).find("td:eq(1)").find("input").val(), 10);
                }
                else {
                    unitPrice = parseFloat($(this).find("td:eq(1)").text(), 10);
                }

                if ($(this).find("td:eq(2)").find("input").is(":input")) {
                    itemUnit = parseFloat($(this).find("td:eq(2)").find("input").val(), 10);
                }
                else {
                    itemUnit = parseFloat($(this).find("td:eq(2)").text(), 10);
                }

                totalPrice = parseFloat($.trim($(this).find("td:eq(3)").text(), 10));
                itemArrivalTime = ($(this).find("td:eq(4)").text());

                detailId = parseInt($.trim($(this).find("td:eq(5)").text(), 10));
                reservationId = parseInt($.trim($(this).find("td:eq(6)").text(), 10));
                itemTypeId = parseInt($.trim($(this).find("td:eq(7)").text(), 10));
                itemType = $(this).find("td:eq(8)").text();
                itemId = parseInt($.trim($(this).find("td:eq(9)").text(), 10));
                isComplementary = $(this).find("td:eq(17)").text();
                itemDescription = $(this).find("td:eq(18)").text();

                if ((isComplementary == "true") || (isComplementary == "True")) {
                    isCom = true;
                }
                else {
                    isCom = false;
                }

                if (detailId == 0) {

                    saveObj.push({
                        ItemName: itemName,
                        ItemUnit: itemUnit,
                        UnitPrice: unitPrice,
                        TotalPrice: totalPrice,
                        Id: detailId,
                        ReservationId: reservationId,
                        ItemTypeId: itemTypeId,
                        ItemType: itemType,
                        ItemId: itemId,
                        IsComplementary: isCom,
                        ItemArrivalTime: itemArrivalTime,
                        ItemDescription: itemDescription
                    });
                }
                else if (detailId > 0) {
                    editObj.push({
                        ItemName: itemName,
                        ItemUnit: itemUnit,
                        UnitPrice: unitPrice,
                        TotalPrice: totalPrice,
                        Id: detailId,
                        ReservationId: reservationId,
                        ItemTypeId: itemTypeId,
                        ItemType: itemType,
                        ItemId: itemId,
                        IsComplementary: isCom,
                        ItemArrivalTime: itemArrivalTime,
                        ItemDescription: itemDescription
                    });
                }
            });
            
            $("#<%=hfSaveObj.ClientID %>").val(JSON.stringify(saveObj));
            $("#<%=hfEditObj.ClientID %>").val(JSON.stringify(editObj));
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

            var detailId = $.trim($(tr).find("td:eq(5)").text());
            var reservationId = $.trim($(tr).find("td:eq(6)").text());
            var itemId = $.trim($(tr).find("td:eq(9)").text());
            var itemTypeId = $.trim($(tr).find("td:eq(7)").text());

            if (parseInt(detailId, 10) != 0) {
                deleteObj.push({
                    Id: detailId,
                    ReservationId: reservationId
                });
            }

            var obgDetails = _.findWhere(AddedItemList, { ItemId: parseInt(itemId, 10), ItemTypeId: parseInt(itemTypeId, 10) });
            var indexObj = _.indexOf(AddedItemList, obgDetails);
            AddedItemList.splice(indexObj, 1);

            $(tr).remove();
            CalculateTotalPaiedNDueAmount();
            return false;
        }

        function EditDetailInfo(detailId, reservationId) {
            var itemTypeId = $("#<%=ddlItemType.ClientID %>").val();
            var itemId = $("#<%=ddlItemId.ClientID %>").val();
            var unitPrice = $("#<%=txtUnitPrice.ClientID %>").val();
            var itemUnit = $("#<%=txtItemUnit.ClientID %>").val();
            var itemArrivalTime = $("#<%=txtItemArrivalTime.ClientID %>").val();
            var itemDescription = $("#<%=txtItemDescription.ClientID %>").val();

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
                tr += "<td align='left' style=\"width:30%; text-align:Left;\">" + $("#<%=ddlItemId.ClientID %> option:selected").text() + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + unitPrice + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itemUnit + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + calculatedAmount + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itemArrivalTime + "</td>";
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

            if ($("#ContentPlaceHolder1_hfReservationId").val() == "") {
                $("#ContentPlaceHolder1_txtBanquetRate").val(result[0].UnitPrice);
                $("#ContentPlaceHolder1_hfBanquetRate").val(result[0].UnitPrice);
            }

            $("#ContentPlaceHolder1_hfIsDiscountApplicableOnRackRate").val(result[1].IsDiscountApplicableOnRackRate == true ? "1" : "0");
            $("#ContentPlaceHolder1_hfIsBanquetBillInclusive").val(result[1].IsVatSChargeInclusive);
            $("#ContentPlaceHolder1_hfIsRatePlusPlus").val(result[1].IsRatePlusPlus);
            $("#ContentPlaceHolder1_hfIsVatOnSD").val(result[1].IsVatOnSDCharge == true ? "1" : "0");
            $("#ContentPlaceHolder1_hfIsSDChargeEnable").val(result[1].IsCitySDChargeEnable == true ? "1" : "0");
            $("#ContentPlaceHolder1_hfIsServiceChargeEnable").val(result[1].IsServiceChargeEnable == true ? "1" : "0");
            $("#ContentPlaceHolder1_hfIsVatEnable").val(result[1].IsVatEnable == true ? "1" : "0");
            $("#ContentPlaceHolder1_hfIsAdditionalChargeEnable").val(result[1].IsAdditionalChargeEnable == true ? "1" : "0");
            $("#ContentPlaceHolder1_hfAdditionalChargeType").val(result[1].AdditionalChargeType);
            $("#ContentPlaceHolder1_hfAdditionalCharge").val(result[1].AdditionalCharge);
            $("#ContentPlaceHolder1_hfBanquetServiceCharge").val(result[1].ServiceCharge);
            $("#ContentPlaceHolder1_hfBanquetVatAmount").val(result[1].VatAmount);
            $("#ContentPlaceHolder1_hfSDCharge").val(result[1].CitySDCharge);
            $("#ContentPlaceHolder1_hfCostCenterId").val(result[0].CostCenterId);
            ShowHideCharges();
            var txtTotalAmount = '<%=txtTotalAmount.ClientID%>'
            $('#' + txtTotalAmount).attr("disabled", false);

            if (result[1].IsVatSChargeInclusive == 0) {
                $("#ContentPlaceHolder1_lblGrandTotal").text("Grand Total");
            }
            else {
                $("#ContentPlaceHolder1_lblGrandTotal").text("Service Rate");
            }

            if ($('#' + txtTotalAmount).val() == "") {
                $('#' + txtTotalAmount).val(result[1].UnitPriceLocal);
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
            //Response.Redirect("~/Banquet/frmBanquetReservation.aspx");
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
            ItemList = result;
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

        function TotalRoomRateVatServiceChargeCalculation() {
            var checkValue = CheckDiscountValidationForFixedRPercentage($("#ContentPlaceHolder1_ddlDiscountType"), $("#ContentPlaceHolder1_txtDiscountAmount"), $("#ContentPlaceHolder1_txtTotalAmount"), "Discount Amount");
            if (checkValue == false) { return false; }

            var isInclusiveBill = 0, vatAmount = 0.00, serviceChargeAmount = 0.00;
            var citySDCharge = 0.00, additionalCharge = 0.00, isRatePlusPlus = 0;
            var isInvoiceCitySDChargeEnable = 1, isInvoiceAdditionalChargeEnable = 1, isServiceChargeEnable = 1, isVatEnable = 1;
            var additionalChargeType = $("#ContentPlaceHolder1_hfAdditionalChargeType").val();
            var isVatOnSD = 0, isDiscountApplicableOnRackRate = 0;

            if ($("#ContentPlaceHolder1_hfIsRatePlusPlus").val() != "") { isRatePlusPlus = parseInt($("#ContentPlaceHolder1_hfIsRatePlusPlus").val(), 10); }

            if ($("#ContentPlaceHolder1_hfIsDiscountApplicableOnRackRate").val() != "") { isDiscountApplicableOnRackRate = parseInt($("#ContentPlaceHolder1_hfIsDiscountApplicableOnRackRate").val(), 10); }

            if ($("#ContentPlaceHolder1_hfIsVatOnSD").val() != "") { isVatOnSD = parseInt($("#ContentPlaceHolder1_hfIsVatOnSD").val(), 10); }

            if ($("#ContentPlaceHolder1_hfIsBanquetBillInclusive").val() != "") { isInclusiveBill = parseInt($("#ContentPlaceHolder1_hfIsBanquetBillInclusive").val(), 10); }

            if ($("#ContentPlaceHolder1_hfBanquetVatAmount").val() != "")
                vatAmount = parseFloat($("#ContentPlaceHolder1_hfBanquetVatAmount").val());

            if ($("#ContentPlaceHolder1_hfBanquetServiceCharge").val() != "")
                serviceChargeAmount = parseFloat($("#ContentPlaceHolder1_hfBanquetServiceCharge").val());

            if ($("#ContentPlaceHolder1_hfSDCharge").val() != "")
                citySDCharge = parseFloat($("#ContentPlaceHolder1_hfSDCharge").val());

            if ($("#ContentPlaceHolder1_hfAdditionalCharge").val() != "")
                additionalCharge = parseFloat($("#ContentPlaceHolder1_hfAdditionalCharge").val());

            if ($('#ContentPlaceHolder1_cbServiceCharge').is(':checked')) {
                isServiceChargeEnable = 1;
            }
            else {
                isServiceChargeEnable = 0;
                serviceChargeAmount = 0.00;
            }

            if ($('#ContentPlaceHolder1_cbVatAmount').is(':checked')) {
                isVatEnable = 1;
            }
            else {
                isVatEnable = 0;
                vatAmount = 0.00;
            }

            if ($("#ContentPlaceHolder1_cbSDCharge").is(":checked")) {
                isInvoiceCitySDChargeEnable = 1;
            }
            else {
                isInvoiceCitySDChargeEnable = 0;
                citySDCharge = 0.00;
            }

            if ($("#ContentPlaceHolder1_cbAdditionalCharge").is(":checked")) {
                isInvoiceAdditionalChargeEnable = 1;
            }
            else {
                isInvoiceAdditionalChargeEnable = 0;
                additionalCharge = 0.00;
            }

            var transactionalAmount = 0.00, discountType = 'Percentage', discount = 0.00;

            discountType = $("#ContentPlaceHolder1_ddlDiscountType").val();
            discount = $("#ContentPlaceHolder1_txtDiscountAmount").val() == "" ? 0.00 : parseFloat($("#ContentPlaceHolder1_txtDiscountAmount").val());

            if ($.trim($("#ContentPlaceHolder1_txtTotalAmount").val()) != "")
                transactionalAmount = parseFloat($("#ContentPlaceHolder1_txtTotalAmount").val());

            var InnboardRateCalculationInfo = CommonHelper.GetRackRateServiceChargeVatInformation(transactionalAmount, serviceChargeAmount, citySDCharge,
                vatAmount, additionalCharge, additionalChargeType, isInclusiveBill,
                isRatePlusPlus, isVatOnSD, isVatEnable, isServiceChargeEnable,
                isInvoiceCitySDChargeEnable, isInvoiceAdditionalChargeEnable,
                isDiscountApplicableOnRackRate, discountType, discount
            );

            oo = InnboardRateCalculationInfo;

            OnLoadRackRateServiceChargeVatInformationSucceeded(InnboardRateCalculationInfo);
        }

        function OnLoadRackRateServiceChargeVatInformationSucceeded(result) {
            var isInclusiveBill = 0;
            if ($("#ContentPlaceHolder1_hfIsBanquetBillInclusive").val() != "") { isInclusiveBill = parseInt($("#ContentPlaceHolder1_hfIsBanquetBillInclusive").val(), 10); }

            if (result.RackRate > 0) {

                if (isInclusiveBill == 1) {
                    $("#ContentPlaceHolder1_txtDiscountedAmount").val(result.DiscountedAmount);
                }
                else {
                    $("#ContentPlaceHolder1_txtDiscountedAmount").val(result.RackRate);
                }

                $("#ContentPlaceHolder1_txtServiceRate").val(result.RackRate);
                $("#ContentPlaceHolder1_txtGrandTotal").val(result.GrandTotal);
                $("#ContentPlaceHolder1_txtServiceCharge").val(result.ServiceCharge);
                $("#ContentPlaceHolder1_txtVatAmount").val(result.VatAmount);
                $("#ContentPlaceHolder1_txtSDCharge").val(result.SDCityCharge);
                $("#ContentPlaceHolder1_txtAdditionalCharge").val(result.AdditionalCharge);
                $("#ContentPlaceHolder1_hfDiscountedAmount").val(result.DiscountedAmount);
                $("#ContentPlaceHolder1_hfServiceRate").val(result.RackRate);
                $("#ContentPlaceHolder1_hfServiceCharge").val(result.ServiceCharge);
                $("#ContentPlaceHolder1_hfVatAmount").val(result.VatAmount);
                $("#ContentPlaceHolder1_hfSDChargeAmount").val(result.SDCityCharge);
                $("#ContentPlaceHolder1_hfAdditionalChargeAmount").val(result.AdditionalCharge);
                $("#ContentPlaceHolder1_hfGrandTotal").val(result.GrandTotal);
            }
            else {
                $("#ContentPlaceHolder1_txtDiscountedAmount").val('0');
                $("#ContentPlaceHolder1_txtServiceRate").val('0');
                $("#ContentPlaceHolder1_txtGrandTotal").val('0');
                $("#ContentPlaceHolder1_txtServiceCharge").val('0');
                $("#ContentPlaceHolder1_txtVatAmount").val('0');
                $("#ContentPlaceHolder1_txtSDCharge").val('0');
                $("#ContentPlaceHolder1_txtAdditionalCharge").val('0');
                $("#ContentPlaceHolder1_hfDiscountedAmount").val("0");
                $("#ContentPlaceHolder1_hfServiceRate").val('0');
                $("#ContentPlaceHolder1_hfServiceCharge").val('0');
                $("#ContentPlaceHolder1_hfVatAmount").val('0');
                $("#ContentPlaceHolder1_hfSDChargeAmount").val('0');
                $("#ContentPlaceHolder1_hfAdditionalChargeAmount").val('0');
                $("#ContentPlaceHolder1_hfGrandTotal").val('0');
            }

            return false;
        }

        function OnLoadRackRateServiceChargeVatInformationFailed(error) {
            //alert(error.get_message());
        }

        var reservationId = 0;
        function PerformCencelAction(id) {
            if (confirm("Want to cancel reservation ?")) {
                reservationId = id;
                $("#CancelDiv").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 900,
                    height: 250,
                    closeOnEscape: true,
                    resizable: false,
                    fluid: true,
                    title: "Reservation Cancel",
                    show: 'slide'
                });
            }
            return false;
        }
        function CancelReservation() {
            var reason = $("#<%=txtReason.ClientID %>").val();
            if (reason == "") {
                toastr.warning("Please provide reason for cancelation.");
                $("#<%=txtReason.ClientID %>").focus();
                return false;
            }
            PageMethods.CancelReservation(reservationId, reason, OnSucceedCancelation, OnFailed);
            return false;
        }
        function OnSucceedCancelation(result) {
            if (result.IsSuccess) {
                toastr.success(result.AlertMessage.Message);
                reservationId = 0;
                $("#<%=txtReason.ClientID %>").val("");
                $("#CancelDiv").dialog("close");
                $("#btnSearch").click();
            }
        }
        function OnFailed(error) {

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
            var issBanquetReservationRestictionForAllUser = $("#<%=hfIsBanquetReservationRestictionForAllUser.ClientID %>").val();

            PageMethods.SearchReservationAndLoad(name, reserveNo, banquetId, email, phone, arriveDate, departDate, issBanquetReservationRestictionForAllUser, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }

        function OnLoadObjectSucceeded(result) {

            $("#gvReservation tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"5\" >No Data Found</td> </tr>";
                $("#gvReservation tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";
            var isUpdatePermission = ($("#<%=hfIsUpdatePermission.ClientID %>").val() == '1');

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvReservation tbody tr").length;
                //totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:10%; cursor:pointer;\">" + gridObject.ReservationNumber + "</td>";
                tr += "<td align='left' style=\"width:10%; cursor:pointer;\">" + gridObject.EventType + "</td>";
                tr += "<td align='left' style=\"width:35%; cursor:pointer;\">" + gridObject.Name + "</td>";
                tr += "<td align='left' style=\"width:25%; cursor:pointer;\">" + gridObject.ContactEmail + "</td>";

                if (gridObject.IsBillSettlement == true) {
                    tr += "<td align='left' style=\"width:10%; cursor:pointer;\">" + 'Settled' + "</td>";
                }
                else {
                    tr += "<td align='left' style=\"width:10%; cursor:pointer;\">" + gridObject.Status + "</td>";
                }

                tr += "<td align='center' style=\"width:10%; cursor:pointer;\">";

                if (gridObject.IsBillSettlement == false) {

                    if (gridObject.ActiveStatus == true && isUpdatePermission) {
                        tr += "&nbsp&nbsp<img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.Id + "')\" alt='Edit Information' text='Edit' border='0' />";
                    }

                    if (gridObject.ActiveStatus == false && IsCanDelete)
                        tr += "&nbsp&nbsp<img src='../Images/approved.png' onClick= \"javascript:return ActiveReservation('" + gridObject.Id + "')\" alt='Active Reservation' text='Active' border='0' />";
                    if (gridObject.Status != "Canceled" && isUpdatePermission) {
                        tr += "&nbsp&nbsp<img src='../Images/delete.png' onClick= \"javascript:return PerformCencelAction('" + gridObject.Id + "')\" alt='Cancel Information' text='Cancel' border='0' />";
                    }
                }

                if (gridObject.EventType != "Internal") {
                    tr += "&nbsp&nbsp<img src='../Images/ReportDocument.png' onClick= \"javascript:return PerformBillPreview('" + gridObject.Id + "')\" tooltip='Bill Preview' alt='Bill Preview' text='Report' border='0' />";
                    tr += "&nbsp&nbsp<img src='../Images/ReportDocument.png' onClick= \"javascript:return PerformPartySheetPreview('" + gridObject.Id + "')\" tooltip='Party Sheet' alt='Party Sheet' text='Report' border='0' />";
                }
                tr += "</td>";
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
            var isPreview = true;
            var url = "/Banquet/Reports/frmReportReservationConLatter.aspx?ReservationId=" + reservationId + "&isPreview=" + isPreview;
            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=790,height=780,left=300,top=50,resizable=yes");
        }
        function PerformPartySheetPreview(reservationId) {
            var isPreview = true;
            var url = "/Banquet/Reports/frmReportReservationConLatter.aspx?ps=1&ReservationId=" + reservationId + "&isPreview=" + isPreview;
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

        function ActiveReservation(id) {
            if (confirm("Want to active reservation ?")) {
                PageMethods.ActiveReservation(id, OnSuccessActivate, OnFailedActivate);
            }
            return false;
        }

        function OnSuccessActivate(result) {

            if (result.IsSuccess) {
                toastr.success(result.AlertMessage.Message);
                $("#btnSearch").click();
            }

            else {
                if (result.IsReservationCheckInDateValidation)
                    toastr.error(result.AlertMessage.Message);
                else
                    toastr.warning(result.AlertMessage.Message);
            }
        }

        function OnFailedActivate(error) {
            toastr.error(error.get_message());
        }
        function ShowHideCharges() {

            if ($("#ContentPlaceHolder1_hfIsServiceChargeEnable").val() == "0") {
                $('#ServiceChargeLabel').hide();
                $('#ServiceChargeControl').hide();
            }
            else {
                $('#ServiceChargeLabel').show();
                $('#ServiceChargeControl').show();
            }

            if ($("#ContentPlaceHolder1_hfIsSDChargeEnable").val() == "0") {
                $('#CityChargeLabel').hide();
                $('#CityChargeControl').hide();
            }
            else {
                $('#CityChargeLabel').show();
                $('#CityChargeControl').show();
            }

            if ($("#ContentPlaceHolder1_hfIsVatEnable").val() == "0") {
                $('#VatAmountLabel').hide();
                $('#VatAmountControl').hide();
            }
            else {
                $('#VatAmountLabel').show();
                $('#VatAmountControl').show();
            }

            if ($("#ContentPlaceHolder1_hfIsAdditionalChargeEnable").val() == "0") {
                $('#AdditionalChargeLabel').hide();
                $('#AdditionalChargeControl').hide();
            }
            else {
                $('#AdditionalChargeLabel').show();
                $('#AdditionalChargeControl').show();
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
    <asp:HiddenField ID="hfIsVatEnable" runat="server" />
    <asp:HiddenField ID="hfIsServiceChargeEnable" runat="server" />
    <asp:HiddenField ID="hfIsSDChargeEnable" runat="server" />
    <asp:HiddenField ID="hfIsAdditionalChargeEnable" runat="server" />
    <asp:HiddenField ID="hfAdditionalChargeType" runat="server" />
    <asp:HiddenField ID="hfIsVatOnSD" runat="server" />
    <asp:HiddenField ID="hfIsRatePlusPlus" runat="server" />
    <asp:HiddenField ID="hfIsDiscountApplicableOnRackRate" runat="server" />
    <asp:HiddenField ID="hfIsBanquetBillInclusive" runat="server" />
    <asp:HiddenField ID="hfBanquetVatAmount" runat="server" />
    <asp:HiddenField ID="hfBanquetServiceCharge" runat="server" />
    <asp:HiddenField ID="hfAdditionalCharge" runat="server" />
    <asp:HiddenField ID="hfSDCharge" runat="server" />
    <asp:HiddenField ID="hfClassificationDiscountSave" runat="server" />
    <asp:HiddenField ID="hfClassificationDiscountDelete" runat="server" />
    <asp:HiddenField ID="hfClassificationDiscountAlreadySave" runat="server" />
    <asp:HiddenField ID="hfIsBanquetBillAmountWillRound" runat="server" />
    <asp:HiddenField ID="hfDuplicateReservarionValidation" runat="server" />
    <asp:HiddenField ID="hfReservationId" runat="server" />
    <asp:HiddenField ID="hfBanquetId" runat="server" />
    <asp:HiddenField ID="hfToday" runat="server" />
    <asp:HiddenField ID="hfEditedId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsBanquetReservationRestictionForAllUser" runat="server" Value="0" />
    <asp:HiddenField ID="hfparticipantFromOfficeValue" runat="server" Value="" />
    <asp:HiddenField ID="hfddlEmployeeId" runat="server" Value="" />
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
    <div id="CancelDiv" style="display: none;">
        <div class="panel panel-default">
            <div class="panel-body">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label7" runat="server" class="control-label required-field"
                            Text="Reason"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" TextMode="MultiLine" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                &nbsp;
                <div class="row">
                    <div class="col-md-offset-2 col-md-12 ">
                        <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary btn-sm"
                            TabIndex="32" OnClientClick="CancelReservation()" />
                    </div>
                </div>
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
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Hall Reservation Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group" id="CompanyProjectDiv">
                            <div class="col-md-2">
                                <asp:HiddenField ID="hfIsSingle" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hfCompanyAll" runat="server" />
                                <asp:HiddenField ID="hfGLCompanyId" runat="server" Value="0" />
                                <asp:HiddenField ID="hfGLProjectId" runat="server" Value="0" />
                                <asp:HiddenField ID="CommonDropDownHiddenFieldForPleaseSelect" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="LiabilitiesAmountHiddenField" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="AssetsAmountHiddenField" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblGLCompany" runat="server" class="control-label required-field"
                                    Text="Company"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlGLCompany" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblGLProject" runat="server" class="control-label required-field"
                                    Text="Project"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlGLProject" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblArriveDate" runat="server" class="control-label required-field"
                                    Text="Event Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtArriveDate" runat="server" CssClass="form-control" TabIndex="12"></asp:TextBox>
                            </div>
                            <div class="col-md-2" style="padding-right: 0">
                                <asp:Label ID="lblProbableArrivalTime" runat="server" class="control-label required-field" Text="Start Time"></asp:Label>
                            </div>
                            <div class="col-md-1">
                                <asp:TextBox ID="txtProbableArrivalHour" placeholder="12" CssClass="form-control"
                                    runat="server" TabIndex="2"></asp:TextBox>
                            </div>
                            <div class="col-md-offset-1 col-md-1" style="padding-right: 0">
                                <asp:Label ID="lblProbableDepartureTime" runat="server" class="control-label required-field" Text="End Time"></asp:Label>
                            </div>
                            <div class="col-md-1">
                                <asp:TextBox ID="txtProbableDepartureHour" placeholder="12" CssClass="form-control"
                                    runat="server" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field"
                                    Text="Event Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:HiddenField ID="hfEventType" runat="server" Value="0"></asp:HiddenField>
                                <asp:DropDownList ID="ddlEventTypeId" CssClass="form-control" runat="server"
                                    TabIndex="14">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblOccessionTypeId" runat="server" class="control-label required-field"
                                    Text="Occasion Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlOccessionTypeId" CssClass="form-control" runat="server"
                                    TabIndex="14">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group DivShowForEventType">
                            <div class="col-md-2">
                                <asp:Label ID="Label6" runat="server" class="control-label required-field"
                                    Text="Event Title"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtEventTitle" runat="server" CssClass="form-control" TabIndex="15"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblBanquetId" runat="server" class="control-label required-field"
                                    Text="Hall Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlBanquetId" runat="server" TabIndex="15" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group DivHideForEventType" style="display: none">
                            <div class="col-md-2">
                                <asp:Label ID="lblBanquetRate" runat="server" class="control-label"
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
                            <div class="col-md-2" style="display: none" id="NumberOfPersonAdultLabelDiv">
                                <asp:Label ID="lblNumberOfPersonAdult" runat="server" class="control-label required-field"
                                    Text="Number Of Adult"></asp:Label>
                            </div>
                            <div class="col-md-4" style="display: none" id="NumberOfPersonAdultControlDiv">
                                <asp:TextBox ID="txtNumberOfPersonAdult" runat="server" CssClass="form-control" TabIndex="17"></asp:TextBox>
                            </div>
                            <div class="DivHideForEventType" style="display: none">
                                <div class="col-md-2">
                                    <asp:Label ID="lblNumberOfPersonChild" runat="server" class="control-label" Text="Number Of Child"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNumberOfPersonChild" runat="server" CssClass="form-control" TabIndex="18"></asp:TextBox>
                                </div>
                            </div>
                            <div class="DivShowForEventType">
                                <div class="col-md-2">
                                    <asp:Label ID="Label3" runat="server" class="control-label" Text="Employee Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlEmployeeId" runat="server" CssClass="form-control" TabIndex="11">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" id="MeetingAgendaDiv" style="display: none;">
                            <div class="col-md-2">
                                <label class="control-label required-field">Meeting Agenda</label>
                            </div>
                            <div class="col-md-10">
                                <textarea id="txtMeetingAgenda" class="form-control" rows="5" cols="30" runat="server"></textarea>
                            </div>
                        </div>
                        <div class="form-group" id="ParticipantFromOfficeDiv" style="display: none;">
                            <div class="col-md-2">
                                <label class="control-label required-field">Participant From Office</label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddltxtParticipantFromOffice" TabIndex="1" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="DivHideForEventType" style="display: none">
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
                                <div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblRefferenceId" runat="server" class="control-label" Text="Reference Name"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlRefferenceId" runat="server" CssClass="form-control" TabIndex="11">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="ReserveModeDetails" style="display: none">
                            <div id="ListedCompanyInfo" class="form-group">
                                <div id="ContactLabel" class="col-md-2" style="text-align: left;">
                                    <asp:Label ID="lblName" runat="server" class="control-label required-field" Text="Listed Company"></asp:Label>
                                </div>
                                <div id="ContactControl" class="col-md-10">
                                    <div class="input-group col-md-12">
                                        <span id="chkIsLitedCompanyDiv" class="input-group-addon">
                                            <asp:CheckBox ID="chkIsLitedCompany" runat="server" Text="" onclick="javascript: return ToggleFieldVisibleForListedCompany(this);"
                                                TabIndex="8" />
                                        </span>
                                        <div id="ListedCompany" style="display: none; width: 100%;">
                                            <input id="txtCompanyId" type="text" class="form-control" />
                                            <div style="display: none;">
                                                <asp:DropDownList ID="ddlCompanyId" runat="server" CssClass="form-control" AutoPostBack="false">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div id="ReservedCompany" style="width: 100%;">
                                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                        </div>
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
                                    <asp:Label ID="lblContactPhone" runat="server" class="control-label required-field" Text="Mobile Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtContactPhone" runat="server" CssClass="form-control" TabIndex="10"></asp:TextBox>
                                </div>
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
                        <div class="childDivSection">
                            <div id="RoomDetailsInformation" class="panel panel-default">
                                <div class="panel-heading">
                                    Item Detail Information
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
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
                                                <input id="txtItemId" type="text" class="form-control" onblur="GetPrice()" />
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
                                                <asp:Label ID="Label5" runat="server" class="control-label required-field" Text="Delivery Time"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtItemArrivalTime" runat="server" CssClass="form-control" TabIndex="24"></asp:TextBox>
                                            </div>
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
                                                <asp:Label ID="Label4" runat="server" class="control-label" Text="Description"></asp:Label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:TextBox ID="txtItemDescription" runat="server" CssClass="form-control" TabIndex="23" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-10">
                                                <input type="button" id="btnAddDetailItem" value="Add" class="TransactionalButton btn btn-primary btn-sm" />
                                                <asp:Label ID="lblHiddenId" runat="server" Text='' Visible="False"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="panel-body">
                                            <div id="ltlTableWiseItemInformation" runat="server" clientidmode="Static">
                                            </div>
                                        </div>
                                        <div id="TotalPaid" class="totalAmout">
                                        </div>
                                        <div id="DueTotal" class="totalAmout">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="19"></asp:TextBox>
                            </div>
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
                        <div class="DivHideForEventType">
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
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDiscountAmount" runat="server" TabIndex="21" CssClass="form-control quantitydecimal"
                                        onblur="TotalRoomRateVatServiceChargeCalculation()"></asp:TextBox>
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
                                <div class="col-md-2">
                                    <asp:Label ID="Label1" runat="server" class="control-label" Text="Service Rate"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:HiddenField ID="hfServiceRate" runat="server" />
                                    <asp:TextBox ID="txtServiceRate" runat="server" TabIndex="26" CssClass="form-control"
                                        ReadOnly="True"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <label id="ServiceChargeLabel" class="control-label">Service Charge</label>
                                </div>
                                <div class="col-md-4">
                                    <div id="ServiceChargeControl" class="input-group">
                                        <asp:TextBox ID="txtServiceCharge" runat="server" TabIndex="22" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="hfServiceCharge" runat="server"></asp:HiddenField>
                                        <span class="input-group-addon">
                                            <asp:CheckBox ID="cbServiceCharge" runat="server" Text="" onclick="javascript: return TotalRoomRateVatServiceChargeCalculation();"
                                                TabIndex="8" Checked="True" />
                                        </span>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <label id="CityChargeLabel" class="control-label">SD Charge</label>
                                </div>
                                <div class="col-md-4">
                                    <div id="CityChargeControl" class="input-group">
                                        <asp:TextBox ID="txtSDCharge" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="hfSDChargeAmount" runat="server"></asp:HiddenField>
                                        <span class="input-group-addon">
                                            <asp:CheckBox ID="cbSDCharge" runat="server" onclick="javascript: return TotalRoomRateVatServiceChargeCalculation();"
                                                Checked="True" />
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <label id="VatAmountLabel" class="control-label">Vat Amount</label>
                                </div>
                                <div class="col-md-4">
                                    <div id="VatAmountControl" class="input-group">
                                        <asp:TextBox ID="txtVatAmount" runat="server" TabIndex="23" CssClass="form-control"
                                            Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="hfVatAmount" runat="server"></asp:HiddenField>
                                        <span class="input-group-addon">
                                            <asp:CheckBox ID="cbVatAmount" runat="server" onclick="javascript: return TotalRoomRateVatServiceChargeCalculation();" Checked="True" />
                                        </span>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <label id="AdditionalChargeLabel" class="control-label">Additional Charge</label>
                                </div>
                                <div class="col-md-4">
                                    <div id="AdditionalChargeControl" class="input-group">
                                        <asp:TextBox ID="txtAdditionalCharge" runat="server" CssClass="form-control"
                                            Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="hfAdditionalChargeAmount" runat="server"></asp:HiddenField>
                                        <span class="input-group-addon">
                                            <asp:CheckBox ID="cbAdditionalCharge" runat="server" onclick="javascript: return TotalRoomRateVatServiceChargeCalculation();" Checked="True" />
                                        </span>
                                    </div>
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
                                    <asp:HiddenField ID="hfGrandTotal" runat="server"></asp:HiddenField>
                                </div>
                            </div>
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
                    Hall Reservation Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                <asp:Label ID="lblSearchName" runat="server" class="control-label" Text="Reserved By"></asp:Label>
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
                                <asp:Label ID="lblSearchBanquetName" runat="server" class="control-label" Text="Hall Name"></asp:Label>
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
                <asp:HiddenField runat="server" ID="hfIsUpdatePermission" />
                <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
                <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
                <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
                <div id="SearchPanel" class="panel panel-default">
                    <div class="panel-heading">
                        Search Information
                    </div>
                    <div class="panel-body">
                        <table id='gvReservation' class="table table-bordered table-condensed table-responsive"
                            width="100%">
                            <colgroup>
                                <col style="width: 10%;" />
                                <col style="width: 10%;" />
                                <col style="width: 35%;" />                                
                                <col style="width: 25%;" />
                                <col style="width: 10%;" />
                                <col style="width: 10%;" />
                            </colgroup>
                            <thead>
                                <tr style="color: White; background-color: #44545E; font-weight: bold;">                                    
                                    <td>Reservation #
                                    </td>
                                    <td>Type
                                    </td>
                                    <td>Company/ Person
                                    </td>
                                    <td>Email Address
                                    </td>
                                    <td>Status
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
                $('#ListedCompany').hide();
                $('#ReservedCompany').show();
                $("#ContactPersonDiv").show();
                $("#chkIsLitedCompanyDiv").show();
                $("#<%=lblName.ClientID %>").text('Company Name');
                $("#<%=chkIsLitedCompany.ClientID %>").attr("Disabled", false);
            }
            else {
                $("#<%=lblName.ClientID %>").text('Contact Person');
                $("#ContactPersonDiv").hide();
                $("#chkIsLitedCompanyDiv").hide();
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
