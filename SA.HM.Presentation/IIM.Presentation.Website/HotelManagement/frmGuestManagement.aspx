<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmGuestManagement.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmGuestManagement" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var vv = [];
        var guestList = new Array();
        var SelectdPreferenceId = "";
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Company Information</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Bank Name</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#GuestBlockDiv").hide();
            $("#GuestSearchResult").hide();

            $("#ContentPlaceHolder1_ddlLinkRoomAmend").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlLinkName").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $('#ContentPlaceHolder1_txtDoBSrc').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                maxDate: 0,
                yearRange: "-100:+0"
            });
            $("#ContentPlaceHolder1_ddlType").change(function () {
                var type = $("#ContentPlaceHolder1_ddlType option:selected").val();
                if ((type == "2")) {
                    $("#txtStopChargePostingRoomNumber").val("");
                    $("#divLinkRoomSelection").show("slow");
                    $("#inputDiv").hide("slow");
                    $(".roomIsRegistered").hide("slow");
                    $('input[type="checkbox"]').prop('checked', false);
                    LoadLinkRooms();
                }
                else if ((type == "1") || (type == "0")) {
                    $("#divLinkRoomSelection").hide("slow");
                    $("#roomLinkedStopCharge").hide("slow");
                    $('input[type="checkbox"]').prop('checked', false);
                    $("#inputDiv").show("slow");
                    //$("#txtStopChargePostingRoomNumber").prop('disabled', false);
                }

            });
            $("#ContentPlaceHolder1_ddlTypeAmend").change(function () {
                var typeAmend = $("#ContentPlaceHolder1_ddlTypeAmend option:selected").val();
                //inputAmendDiv divAmendLinkRoomSelection
                if (typeAmend == "2") {
                    $("#txtAmendStayRoomNumber").val("");
                    $("#divAmendLinkRoomSelection").show("slow");
                    $("#inputAmendDiv").hide("slow");
                    $(".roomIsRegistered").hide("slow");
                    LoadLinkRoomsAmend();
                }
                else if ((typeAmend == "1") || (typeAmend == "0")) {
                    $("#inputAmendDiv").show("slow");
                    $("#divAmendLinkRoomSelection").hide("slow");
                    $("#roomLinkedAmendStay").hide("slow");

                }
            });

            $("#ContentPlaceHolder1_ddlNoShowPostingRoomType").change(function () {
                var noShowPostingInfo = $("#ContentPlaceHolder1_ddlNoShowPostingRoomType option:selected").val();
                if (noShowPostingInfo == "2") {
                    $("#txtNoShowPostingInfoRoomNumber").val("");
                    $("#divNoShowPostingInfoLinkRoomSelection").show("slow");
                    $("#inputNoShowPostingInfoDiv").hide("slow");
                    $(".roomIsRegistered").hide("slow");
                    LoadLinkRoomsNoShowPostingInfo();
                }
                else if ((noShowPostingInfo == "1") || (noShowPostingInfo == "0")) {
                    $("#inputNoShowPostingInfoDiv").show("slow");
                    $("#divNoShowPostingInfoLinkRoomSelection").hide("slow");
                    $("#roomLinkedNoShowPostingInfo").hide("slow");

                }
            });




            //---Guest search -----
            $("#btnGuestSearch").on('click', function (event) {
                $("#GuestSearchResult").show('slow');
                GridPaging(1, 1);
                //LoadGuestSearch();
            });

            $("#ContentPlaceHolder1_ddlTitle").select({
                tags: "true",
                //placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#<%=ddlTitle.ClientID %>").change(function () {
                AutoGenderLoadInfo();
            });

            // multiple guest swap
            $("#ContentPlaceHolder1_ddlPaxFrom").change(function () {
                var selectedId = $("#ContentPlaceHolder1_ddlPaxFrom option:selected").val();
                if (selectedId != "All") {
                    $("#ContentPlaceHolder1_hfRoomSwapFromGuestId").val(selectedId);
                    GetFromGuestInfoById(selectedId);
                }
                else {
                    GetAllFromGuestByRoomNumber();
                }

            });
            $("#ContentPlaceHolder1_ddlPaxTo").change(function () {
                var selectedId = $("#ContentPlaceHolder1_ddlPaxTo option:selected").val();
                if (selectedId != "All") {
                    $("#ContentPlaceHolder1_hfRoomSwapToGuestId").val(selectedId);
                    GetToGuestInfoById(selectedId);
                }
                else {
                    GetAllToGuestByRoomNumber();
                }

            });
            //---Delete linked rooms-------------ContentPlaceHolder1_hfSelectedMasterRoomPK
            $("#TblLinkedRoom").delegate("td > img.RoomDelete", "click", function () {
                var answer = confirm("Do you want to delete this record?")

                if (answer) {

                    //vv = $(this);
                    CommonHelper.ExactMatch();
                    var tr = $(this).parent().parent();

                    var masterId = $("#ContentPlaceHolder1_hfSelectedMasterId").val();
                    var regId = $.trim($(tr).find("td:eq(3)").text());
                    var pk = $.trim($(tr).find("td:eq(4)").text());
                    var params = JSON.stringify({ deleteId: regId });

                    if (pk == "0") {
                        //var removeRow = $("#TblLinkedRoom tbody tr").find("td:eq(4):textEquals('" + pk + "')").parent();
                        //$(removeRow).remove();
                        $(tr).remove();
                    }
                    else {
                        $.ajax({
                            type: "POST",
                            url: "/HotelManagement/frmGuestManagement.aspx/DeleteRoomData",
                            data: params,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                CommonHelper.AlertMessage(data.d.AlertMessage);
                                $(tr).remove();
                                //$("#myTabs").tabs('load', 4);
                                CheckLinkedRooms(masterId);
                            },
                            error: function (error) {

                            }
                        });
                    }

                }
            });//end

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            CommonHelper.AutoSearchClientDataSource("txtGuestCountrySearch", "ContentPlaceHolder1_ddlGuestCountry", "ContentPlaceHolder1_ddlGuestCountry");

            $("#txtGuestCountrySearch").blur(function () {
                var countryId = $("#<%=ddlGuestCountry.ClientID %>").val();
                PageMethods.GetNationality(countryId, OnCountrySucceeded, OnCountryFailed);
            });



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

            var txtGuestDOB = '<%=txtGuestDOB.ClientID%>'
            $('#ContentPlaceHolder1_txtGuestDOB').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                maxDate: 0,
                yearRange: "-100:+0"
            });

            var txtSrcDateOfBirth = '<%=txtSrcDateOfBirth.ClientID%>'
            $('#ContentPlaceHolder1_txtSrcDateOfBirth').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                maxDate: 0,
                yearRange: "-100:+0"
            });

            var txtVIssueDate = '<%=txtVIssueDate.ClientID%>'
            $('#ContentPlaceHolder1_txtVIssueDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtVExpireDate').datepicker("option", "minDate", selectedDate);
                }
            });
            var txtVExpireDate = '<%=txtVExpireDate.ClientID%>'
            $('#ContentPlaceHolder1_txtVExpireDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtVIssueDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            var txtPIssueDate = '<%=txtPIssueDate.ClientID%>'
            $('#ContentPlaceHolder1_txtPIssueDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtPExpireDate').datepicker("option", "minDate", selectedDate);
                }
            });
            var txtPExpireDate = '<%=txtPExpireDate.ClientID%>'
            $('#ContentPlaceHolder1_txtPExpireDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtPIssueDate').datepicker("option", "maxDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_ddlTitle").blur(function () {
                var title = $("#<%=ddlTitle.ClientID %>").val();
                var titleText = $("#<%=ddlTitle.ClientID %> option:selected").text();

                if (title == "MrNMrs.") {
                    title = "Mr. & Mrs.";
                }
                else if (title == "N/A") {
                    title = "0";
                }
                //else {
                //    title = titleText;
                //}
                var firstName = $("#<%=txtFirstName.ClientID %>").val().trim();
                var lastName = $("#<%=txtLastName.ClientID %>").val().trim();
                if (title != "0") {
                    $("#<%=txtGuestName.ClientID %>").val(titleText + " " + firstName + " " + lastName);
                }
                else $("#<%=txtGuestName.ClientID %>").val(firstName + " " + lastName);
            });

            $("#ContentPlaceHolder1_txtFirstName").blur(function () {
                var title = $("#<%=ddlTitle.ClientID %>").val();
                var titleText = $("#<%=ddlTitle.ClientID %> option:selected").text();

                if (title == "MrNMrs.") {
                    title = "Mr. & Mrs.";
                }
                else if (title == "N/A") {
                    title = "0";
                }
                //else {
                //    title = titleText;
                //}
                var firstName = $("#<%=txtFirstName.ClientID %>").val().trim();
                var lastName = $("#<%=txtLastName.ClientID %>").val().trim();
                if (title != "0") {
                    $("#<%=txtGuestName.ClientID %>").val(titleText + " " + firstName + " " + lastName);
                }
                else $("#<%=txtGuestName.ClientID %>").val(firstName + " " + lastName);
            });

            $("#ContentPlaceHolder1_txtLastName").blur(function () {
                var title = $("#<%=ddlTitle.ClientID %>").val();
                var titleText = $("#<%=ddlTitle.ClientID %> option:selected").text();

                if (title == "MrNMrs.") {
                    title = "Mr. & Mrs.";
                }
                else if (title == "N/A") {
                    title = "0";
                }
                //else {
                //    title = titleText;
                //}
                var firstName = $("#<%=txtFirstName.ClientID %>").val().trim();
                var lastName = $("#<%=txtLastName.ClientID %>").val().trim();
                if (title != "0") {
                    $("#<%=txtGuestName.ClientID %>").val(titleText + " " + firstName + " " + lastName);
                }
                else $("#<%=txtGuestName.ClientID %>").val(firstName + " " + lastName);
            });

            $('#btnPopUp').click(function () {
                masterSelection();
            });

            $('#btnSave').click(function () {
                MasterRoomSave();
            });

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

            $("#btnSearch").click(function () {
                //btnOk
                LoadGridInformation();
            });

            var txtDepartureDate = '<%=txtAmendStayDExpectedDepartureDate.ClientID%>'


            $('#ContentPlaceHolder1_txtAmendStayDExpectedDepartureDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                //minDate: $("#ContentPlaceHolder1_lblAmendStayDArrivalDate").text()
                minDate: 0
            });
            $('#ContentPlaceHolder1_txtExpectedDepDateLinked').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                minDate: 0
            });

            $('.checkBlock').on('change', function () {
                $('.checkBlock').not(this).prop('checked', false);
            });



            $('#btnUpdateBlock').click(function () {
                UpdateGuestInfoForBlock();

            });
            CheckButtonPermission();


        });

        function CheckAll() {
            if ($("#chkAllOutlet").is(":checked")) {
                $("#ExpressCheckInDetailsGrid tbody tr").find("td:eq(1)").find("input").prop("checked", true);
            }
            else {
                $("#ExpressCheckInDetailsGrid tbody tr").find("td:eq(1)").find("input").prop("checked", false);
            }
        }
        function CheckAllRooms() {
            if ($("#chkAllRooms").is(":checked")) {
                $("#LikedRoomsInfoGrid tbody tr").find("td:eq(1)").find("input").prop("checked", true);
            }
            else {
                $("#LikedRoomsInfoGrid tbody tr").find("td:eq(1)").find("input").prop("checked", false);
            }
        }
        function CheckAllRoomsStop() {
            if ($("#chkAllRoomsStop").is(":checked")) {
                $("#LikedRoomsInfoGridStop tbody tr").find("td:eq(1)").find("input").prop("checked", true);
            }
            else {
                $("#LikedRoomsInfoGridStop tbody tr").find("td:eq(1)").find("input").prop("checked", false);
            }
        }
        function CheckAllFromGuestsSwap() {
            if ($("#chkAllFromGuestSwap").is(":checked")) {
                $("#FromGuestSwapGrid tbody tr").find("td:eq(1)").find("input").prop("checked", true);
            }
            else {
                $("#FromGuestSwapGrid tbody tr").find("td:eq(1)").find("input").prop("checked", false);
            }
        }
        function CheckAllToGuestsSwap() {
            if ($("#chkAllToGuestSwap").is(":checked")) {
                $("#ToGuestSwapGrid tbody tr").find("td:eq(1)").find("input").prop("checked", true);
            }
            else {
                $("#ToGuestSwapGrid tbody tr").find("td:eq(1)").find("input").prop("checked", false);
            }
        }
        //---- GuestBlock --------


        function UpdateGuestInfoForBlock() {
            var isBlock = $("#chkYesBlock").is(":checked");
            //var isNotBlock = $("#chkNotBlock").is(":checked");
            var isGuestBlock = '';
            //toastr.info(isBlock);

            if (isBlock == true) {
                isGuestBlock = true;
            }
            else {
                isGuestBlock = false;
            }

            var description = $("#ContentPlaceHolder1_txtGuestDescription").val();

            if ((description == "") && (isBlock == true)) {
                toastr.warning("Please provide reason for Blocking.");
                $("#ContentPlaceHolder1_txtGuestDescription").focus();
                return false;
            }
            var guestId = $("#ContentPlaceHolder1_hfGuestId").val();



            PageMethods.UpdateGuestBlockInfo(guestId, isGuestBlock, description, OnBlockUpdateSucceed, OnBlockUpdateFailed);

            return false;
        }
        function OnBlockUpdateSucceed(result) {
            if (result.IsSuccess) {
                $("#GuestBlockDiv").dialog('close');
                CommonHelper.AlertMessage(result.AlertMessage);
                ClearGuestBlock();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnBlockUpdateFailed() {

        }//end

        function AutoGenderLoadInfo() {
            var titleSex = $('#<%=ddlTitle.ClientID%>').val();
            //toastr.info(titleSex);

            if (titleSex == "Mr.") {
                $('#<%=ddlGuestSex.ClientID%>').val("Male");
            }
            else if (titleSex == "0") {
                $('#<%=ddlGuestSex.ClientID%>').val("0");
            }
            else if (titleSex == "MrNMrs.") {
                $('#<%=ddlGuestSex.ClientID%>').val("0");
            }
            else if (titleSex == "Dr.") {
                $('#<%=ddlGuestSex.ClientID%>').val("0");
            }
            else if (titleSex == "N/A") {
                $('#<%=ddlGuestSex.ClientID%>').val("0");
            }
            else {
                $('#<%=ddlGuestSex.ClientID%>').val("Female");
            }

        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#tblGuestInfo tbody tr").length;

            var guestName = $("#<%= txtGuestNameSrc.ClientID %>").val();
            var companyName = $("#<%= txtCompanyNameSrc.ClientID %>").val();
            var email = $("#<%= txtEmailSrc.ClientID %>").val();
            var mobileNumber = $("#<%= txtMobileNumberSrc.ClientID %>").val();
            var nId = $("#<%= txtNIDsrc.ClientID %>").val();
            //var doB = $("#<%= txtDoBSrc.ClientID %>").val();

            var doB = "";
            if ($("#<%= txtDoBSrc.ClientID %>").val() != "") {
                doB = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtDoBSrc").val(), '/');
            }

            var passport = $("#<%= txtPassportsrc.ClientID %>").val();
            CommonHelper.SpinnerOpen();

            PageMethods.SearchAndLoadGuestInfo(guestName, companyName, email, mobileNumber, nId, doB, passport, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadGuestSearchSucceed, OnLoadGuestSearchFailed);
            return false;
        }
        function OnLoadGuestSearchSucceed(result) {

            CommonHelper.SpinnerClose();

            var gridRecordsCount = $("#tblGuestInfo tbody tr").length;
            if (gridRecordsCount < 0) {
                $("#tblGuestInfo tbody").html("");
            }
            else {
                $("#tblGuestInfo tbody tr").remove();
            }
            //$("#tblGuestInfo tbody").html("");
            //$("#tblGuestInfo tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            //guestList = result;
            //[0].GuestInfo[0].GuestId

            var tr = "", totalRow = 0;
            //var countList = result[0].GuestInfo.length;
            var i = 0;
            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"7\" >No Data Found</td> </tr>";
                $("#tblGuestInfo tbody ").append(emptyTr);
                return false;
            }
            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#tblGuestInfo tbody tr").length;
                var isBlock = gridObject.GuestBlock == true ? "Yes" : "No";
                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                tr += "<td align='left' style='width: 15%;cursor:pointer' onClick='javascript:return SelectGuest(" + gridObject.GuestId + ")'>" + gridObject.GuestName + "</td>";
                tr += "<td align='left' style='width: 15%;cursor:pointer' onClick='javascript:return SelectGuest(" + gridObject.GuestId + ")'>" + gridObject.CompanyName + "</td>";
                tr += "<td align='left' style='width: 15%;cursor:pointer' onClick='javascript:return SelectGuest(" + gridObject.GuestId + ")'>" + gridObject.GuestEmail + "</td>";
                tr += "<td align='left' style='width: 15%;cursor:pointer' onClick='javascript:return SelectGuest(" + gridObject.GuestId + ")'>" + gridObject.GuestPhone + "</td>";
                tr += "<td align='left' style='width: 15%;cursor:pointer' onClick='javascript:return SelectGuest(" + gridObject.GuestId + ")'>" + gridObject.NationalId + "</td>";
                tr += "<td align='left' style='width: 10%;cursor:pointer' onClick='javascript:return SelectGuest(" + gridObject.GuestId + ")'>" + gridObject.GuestDOBShow + "</td>";
                tr += "<td align='left' style='width: 15%;cursor:pointer' onClick='javascript:return SelectGuest(" + gridObject.GuestId + ")'>" + isBlock + "</td>";
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
        function OnLoadGuestSearchFailed(error) {
            alert(error.get_message());
        }
        function ReloadGrid(IsCurrentOrPreviousPage) {
            var currentPageNumber = $("#GridPagingContainer ul li[class='active']").text();
            if (currentPageNumber == "")
                currentPageNumber = 1;

            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }
        function SelectGuest(GuestId) {
            $("#ContentPlaceHolder1_hfGuestId").val(GuestId);

            $("#GuestBlockDiv").dialog({
                width: 600,
                height: 250,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                title: "Guest Information",
                show: 'slide'
            });

            PageMethods.GetGuestFromHotelGuestInfo(GuestId, OnLoadGuestFromHotelGuestInfoSucceed, OnLoadGuestFromHotelGuestInfoFaild);
            return false;
        }
        function OnLoadGuestFromHotelGuestInfoSucceed(result) {
            var isBlock = result.GuestBlock;

            if (isBlock == "1") {
                $("#chkYesBlock").prop("checked", true);
                // $("#chkNotBlock").prop("checked", false);
            }
            else if (isBlock == "0") {
                // $("#chkNotBlock").prop("checked", true);
                $("#chkYesBlock").prop("checked", false);
            }
            else {
                //$("#chkNotBlock").prop("checked", false);
                $("#chkYesBlock").prop("checked", false);
            }

            $("#ContentPlaceHolder1_BlGuestName").val(result.GuestName);
            $("#ContentPlaceHolder1_txtGuestDescription").val(result.Description);

            return false;

        }
        function OnLoadGuestFromHotelGuestInfoFaild(error) {
            alert(error.get_message());
        }
        function ClearGuestBlock() {
            //$("#tblGuestInfo").find("tr:not(:first)").remove();

            //$("#GridPagingContainer").hide();
            //$("#GuestSearchResult").hide();
            ReloadGrid(1);
            $("#ContentPlaceHolder1_txtGuestNameSrc").val("");
            $("#ContentPlaceHolder1_txtCompanyNameSrc").val("");
            $("#ContentPlaceHolder1_txtEmailSrc").val("");
            $("#ContentPlaceHolder1_txtMobileNumberSrc").val("");
            $("#ContentPlaceHolder1_txtDoBSrc").val("");
            $("#ContentPlaceHolder1_txtPassportsrc").val("");
            $("#ContentPlaceHolder1_hfGuestId").val("");
        }
        $(function () {
            $("#myTabs").tabs();
        });
        function OnCountrySucceeded(result) {
            $("#ContentPlaceHolder1_txtGuestNationality").val(result);
        }
        function OnCountryFailed() { }

        function SelectGuestForDetailInformation(GuestId) {
            PageMethods.LoadDetailInformation(GuestId, OnLoadDetailObjectSucceeded, OnLoadDetailObjectFailed);
            //LoadGuestImage(GuestId);
            //LoadGuestHistory(GuestId)
            return false;
        }
        function OnLoadDetailObjectSucceeded(result) {
            var guestInfo = result.GuestInfo;
            var guestDoc = result.GuestDoc;
            $("#<%=hiddenGuestId.ClientID %>").val(guestInfo.GuestId);

            $("#<%=ddlTitle.ClientID %>").val(guestInfo.Title);
            $("#<%=txtFirstName.ClientID %>").val(guestInfo.FirstName);
            $("#<%=txtLastName.ClientID %>").val(guestInfo.LastName);
            $("#<%=txtGuestName.ClientID %>").val(guestInfo.GuestName);
            var date = new Date(guestInfo.GuestDOB);
            var shortDate = "";
            if (!guestInfo.GuestDOB) {
                shortDate = "";
            }
            else {
                shortDate = GetStringFromDateTime(date);
            }
            $("#<%=txtGuestDOB.ClientID %>").val(shortDate);
            $("#<%=ddlGuestSex.ClientID %>").val(guestInfo.GuestSex);
            $("#<%=txtGuestAddress1.ClientID %>").val(guestInfo.GuestAddress1);
            $("#<%=txtGuestAddress2.ClientID %>").val(guestInfo.GuestAddress2);
            $("#<%=txtGuestEmail.ClientID %>").val(guestInfo.GuestEmail);
            $("#<%=txtGuestPhone.ClientID %>").val(guestInfo.GuestPhone);
            $("#<%=ddlProfessionId.ClientID %>").val(guestInfo.ProfessionId);
            $("#<%=txtGuestCity.ClientID %>").val(guestInfo.GuestCity);
            $("#<%=txtGuestZipCode.ClientID %>").val(guestInfo.GuestZipCode);
            $("#txtGuestCountrySearch").val(guestInfo.CountryName);
            $("#<%=ddlGuestCountry.ClientID %>").val(guestInfo.GuestCountryId);
            $("#<%=txtGuestNationality.ClientID %>").val(guestInfo.GuestNationality);
            $("#<%=txtGuestDrivinlgLicense.ClientID %>").val(guestInfo.GuestDrivinlgLicense);
            $("#<%=txtNationalId.ClientID %>").val(guestInfo.NationalId);
            $("#<%=txtVisaNumber.ClientID %>").val(guestInfo.VisaNumber);
            $("#ContentPlaceHolder1_hfGuestPreferenceId").val(result.GuestPreferenceId);
            $("#<%=lblGstPreference.ClientID %>").text(result.GuestPreference);

            var dateVIssue = new Date(guestInfo.VIssueDate);
            var shortDateVIssue = "";
            if (!guestInfo.VIssueDate) {
                shortDateVIssue = "";
            }
            else {
                shortDateVIssue = GetStringFromDateTime(dateVIssue);
            }

            $("#<%=txtVIssueDate.ClientID %>").val(shortDateVIssue);

            var dateVExpire = new Date(guestInfo.VExpireDate);
            var shortDateVExpire = "";
            if (!guestInfo.VExpireDate) {
                shortDateVExpire = "";
            }
            else {
                shortDateVExpire = GetStringFromDateTime(dateVExpire);
            }

            $("#<%=txtVExpireDate.ClientID %>").val(shortDateVExpire);

            var datePIssue = new Date(guestInfo.PIssueDate);
            var shortDatePIssue = "";
            if (!guestInfo.PIssueDate) {
                shortDatePIssue = "";
            }
            else {
                shortDatePIssue = GetStringFromDateTime(datePIssue);
            }

            $("#<%=txtPIssueDate.ClientID %>").val(shortDatePIssue);

            var datePExpire = new Date(guestInfo.PExpireDate);
            var shortDatePExpire = "";
            if (!guestInfo.PExpireDate) {
                shortDatePExpire = "";
            }
            else {
                shortDatePExpire = GetStringFromDateTime(datePExpire);
            }
            $("#<%=txtPExpireDate.ClientID %>").val(shortDatePExpire);

            $("#<%=txtPassportNumber.ClientID %>").val(guestInfo.PassportNumber);

            UploadComplete();

            var totalDoc = guestDoc.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";

            guestDocumentTable += "<table id='guestDocList' style='width:100%' class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            guestDocumentTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }

                guestDocumentTable += "<td align='left' style='width: 50%'>" + guestDoc[row].Name + "</td>";

                if (result.GuestDoc[row].Path != "") {
                    imagePath = "<img src='" + guestDoc[row].Path + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
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

            docc = guestDocumentTable;

            $("#GuestDocumentInfo").html(guestDocumentTable);

            $("#GuestInfoUpdateDiv").show();
            $("#GuestEntryPanel").hide();
            $("#GuestSearchPanel").hide();
            return false;
        }
        function OnLoadDetailObjectFailed(error) {
            alert(error.get_message());
        }

        function LoadGridInformation() {
            var companyName = $("#<%=txtSrcCompanyName.ClientID %>").val();
            var DateOfBirth = $("#<%=txtSrcDateOfBirth.ClientID %>").val();
            //var DateOfBirth = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtSrcDateOfBirth").val(), '/');
            var EmailAddress = $("#<%=txtSrcEmailAddress.ClientID %>").val();
            var FromDate = $("#<%=txtSrcFromDate.ClientID %>").val();
            //var FromDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtSrcFromDate").val(), '/');
            var ToDate = $("#<%=txtSrcToDate.ClientID %>").val();
            //var ToDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtSrcToDate").val(), '/');
            var GuestName = $("#<%=txtSrcGuestName.ClientID %>").val();
            var MobileNumber = $("#<%=txtSrcMobileNumber.ClientID %>").val();
            var NationalId = $("#<%=txtSrcNationalId.ClientID %>").val();
            var PassportNumber = $("#<%=txtSrcPassportNumber.ClientID %>").val();
            var RegistrationNumber = $("#<%=txtSrcRegistrationNumber.ClientID %>").val();
            var RoomNumber = $("#<%=txtSrcRoomNumber.ClientID %>").val();
            $("#GuestSearchPanel").show();
            PageMethods.SearchGuestAndLoadGridInformation(companyName, DateOfBirth, EmailAddress, FromDate, ToDate, GuestName, MobileNumber, NationalId, PassportNumber, RegistrationNumber, RoomNumber, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {
            $("#ltlTableWiseItemInformation").html(result);
            return false;
        }
        function OnLoadObjectFailed(error) {
            alert(error.get_message());
        }

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
            $("#GuestSearchPanel").hide();
        }

        function PerformBackToSearchGuestAction() {
            $("#GuestEntryPanel").show();
            $("#GuestInfoUpdateDiv").hide();
            $("#GuestSearchPanel").show();
        }

        // multiple guest 
        function GetMultipleGuest() {
            ClearGuestSwap();
            var roomNumber = $.trim($("#txtRoomSwapRoomNumber").val());
            var newRoomNumber = $.trim($("#txtRoomSwapAlterRoomNumber").val());

            if (roomNumber == "") {
                toastr.warning("Please Provide Room Number."); return false;
            }
            else if (newRoomNumber == "") {
                toastr.warning("Please Provide To Room Number For Change."); return false;
            }
            else if (newRoomNumber == roomNumber) {
                toastr.warning("For Same Rooms, Guests Cannot Be Swapped.");
                return false;
            }

            $("#ContentPlaceHolder1_hfFromRoom").val(roomNumber);
            $("#ContentPlaceHolder1_hfToRoom").val(newRoomNumber);

            //PageMethods.GetMultipleGuestFromRoom(roomNumber, OnGetFromGuestSucceed, OnGetFromGuestFailed);
            PageMethods.GetAllFromGuestByRoomNumber(roomNumber, OnGetAllFromGuestByRoomNumberSucceed, OnGetAllFromGuestByRoomNumberFailed);
            PageMethods.GetAllToGuestByRoomNumber(newRoomNumber, OnGetAllToGuestByRoomNumberSucceed, OnGetAllToGuestByRoomNumberFailed);
            //PageMethods.GetMultipleGuestToRoom(newRoomNumber, OnGetToGuestSucceed, OnGetToGuestFailed);
        }

        function OnGetFromGuestSucceed(result) {
            var list = result;
            var control = $("#<%=ddlPaxFrom.ClientID %>");
            var newRoomNumber = $.trim($("#txtRoomSwapAlterRoomNumber").val());
            var fromGuestId = list[0].Value;
            $("#ContentPlaceHolder1_hfRoomSwapFromGuestId").val(fromGuestId);
            if (list.length > 1) {
                $("#fromRoomPaxDiv").show();
                control.empty().append('<option selected="selected" value="0">' + "Please Select" + '</option>');
                $.each(list, function () {
                    control.append($("<option></option>").val(this['Value']).html(this['Text']));
                });
                control.append('<option value="All">' + "Select All" + '</option>');
            }
            else {
                $("#fromRoomPaxDiv").show();
                $.each(list, function () {
                    control.append($("<option></option>").val(this['Value']).html(this['Text']));
                });
                SearchRoomInfoFrom($("#ContentPlaceHolder1_hfFromRoom").val());
                //OnGetRegistrationInformationForChangeByRoomNumberSucceeded(result);
                //control.empty().append('<option selected="selected" value="0">Not available</option>');
            }

        }
        function OnGetFromGuestFailed() {
            $("#ContentPlaceHolder1_hfFromRoom").val("");
        }

        function OnGetToGuestSucceed(result) {
            var list = result;
            var control = $("#<%=ddlPaxTo.ClientID %>");
            var toGuestId = list[0].Value;
            $("#ContentPlaceHolder1_hfRoomSwapToGuestId").val(toGuestId);
            if (list.length > 1) {
                $("#toRoomPaxDiv").show();
                control.empty().append('<option selected="selected" value="0">' + "Please Select" + '</option>');
                $.each(list, function () {
                    control.append($("<option></option>").val(this['Value']).html(this['Text']));
                });
                control.append('<option value="All">' + "Select All" + '</option>');
            }
            else {
                $("#toRoomPaxDiv").show();
                $.each(list, function () {
                    control.append($("<option></option>").val(this['Value']).html(this['Text']));
                });
                SearchRoomInfoTo($("#ContentPlaceHolder1_hfToRoom").val());
            }
        }
        function OnGetToGuestFailed() {
            $("#ContentPlaceHolder1_hfToRoom").val("");
        }
        //Multi Guest Swap
        function GetAllFromGuestByRoomNumber() {
            var roomNumber = $("#ContentPlaceHolder1_hfFromRoom").val();
            var newRoomNumber = $("#ContentPlaceHolder1_hfToRoom").val();

            PageMethods.GetAllFromGuestByRoomNumber(roomNumber, OnGetAllFromGuestByRoomNumberSucceed, OnGetAllFromGuestByRoomNumberFailed);
        }
        function OnGetAllFromGuestByRoomNumberSucceed(result) {
            if (result[0].length > 0) {
                $(".roomIsRegistered").hide("slow");

                $("#multiGuestSwapDiv").show("slow");
                //$("#fromGuestSwapDiv").show("slow");
                $("#ltlFromGuestSwap").html(result[0]);
            }
            else {

            }
        }
        function OnGetAllFromGuestByRoomNumberFailed(error) {

        }
        function GetAllToGuestByRoomNumber() {
            var roomNumber = $("#ContentPlaceHolder1_hfFromRoom").val();
            var newRoomNumber = $("#ContentPlaceHolder1_hfToRoom").val();

            PageMethods.GetAllToGuestByRoomNumber(newRoomNumber, OnGetAllToGuestByRoomNumberSucceed, OnGetAllToGuestByRoomNumberFailed);
        }
        function OnGetAllToGuestByRoomNumberSucceed(result) {
            if (result[0].length > 0) {
                $(".roomIsRegistered").hide("slow");

                $("#multiGuestSwapDiv").show("slow");
                //$("#toGuestSwapDiv").show("slow");
                $("#ltlToGuestSwap").html(result[0]);
            }
            else {

            }
        }
        function OnGetAllToGuestByRoomNumberFailed(error) {

        }
        function GetFromGuestInfoById(guestId) {
            var roomNumber = $("#ContentPlaceHolder1_hfFromRoom").val();
            var newRoomNumber = $("#ContentPlaceHolder1_hfToRoom").val();

            PageMethods.GetRoomSwapInformationForRoomChangeByRoomNumber_GuestId(roomNumber, guestId, OnGetFromGuestInfoByIdSucceeded, OnGetFromGuestInfoByIdFailed);
        }
        function OnGetFromGuestInfoByIdSucceeded(result) {
            if (result[0].RoomId != 0) {
                $(".roomIsRegistered").show();

            }
            else { $(".roomIsRegistered").hide(); }

            //if ((result[0].RoomNumber != "") && (result[1].RoomNumber != "")) {
            //    GetMultipleGuest();
            //}

            ClearRoomDetails();

            $("#ContentPlaceHolder1_hfCostcenterId").val(result[0].CostCenterId);
            $("#ContentPlaceHolder1_hfRoomId").val(result[0].RoomId);
            $("#ContentPlaceHolder1_hfKotId").val(result[0].KotId);

            if (result[0].KotId != 0) {
                $("#RoomRestaurantOrder").css("background", "#D63CC4");
                $(".roomChangeRoomSwapContainer").show();
            }

            else {
                $("#RoomRestaurantOrder").css("background", "");
                $(".roomChangeRoomSwapContainer").hide();
            }

            if ($("#roomInfoRoomSwapContainer").hasClass('col-md-12')) {
                $("#roomInfoRoomSwapContainer").removeClass("col-md-12");
                $("#roomInfoRoomSwapContainer").addClass("col-md-6");
            }

            $("#<%=lblRoomSwapRoomNumber.ClientID %>").text(result[0].RoomNumber);
            $("#<%=lblRoomSwapRoomType.ClientID %>").text(result[0].RoomType);
            $("#<%=lblRoomSwapDGuestName.ClientID %>").text(result[0].GuestName);
            $("#<%=lblRoomSwapDGuestSex.ClientID %>").text(result[0].GuestSex);
            //$("#<%=lblRoomSwapDGuestDOB.ClientID %>").text(GetStringFromDateTime(result[0].GuestDOB));


            if (GetStringFromDateTime(result[0].GuestDOB) == "01/01/1970") {
                $("#<%=lblRoomSwapDGuestDOB.ClientID %>").text("");
            }
            else {
                $("#<%=lblRoomSwapDGuestDOB.ClientID %>").text(GetStringFromDateTime(result[0].GuestDOB));
            }

            $("#<%=lblRoomSwapDGuestEmail.ClientID %>").text(result[0].GuestEmail);
            $("#<%=lblRoomSwapDGuestPhone.ClientID %>").text(result[0].GuestPhone);
            $("#<%=lblRoomSwapDGuestAddress2.ClientID %>").text(result[0].GuestAddress2);
            $("#<%=lblRoomSwapDGuestNationality.ClientID %>").text(result[0].GuestNationality);
            $("#<%=lblRoomSwapDCountryName.ClientID %>").text(result[0].CountryName);
            $("#<%=lblRoomSwapDArrivalDate.ClientID %>").text(GetStringFromDateTime(result[0].ArriveDate));
            $("#<%=lblRoomSwapDExpectedDepartureDate.ClientID %>").text(GetStringFromDateTime(result[0].ExpectedCheckOutDate));

            $("#ContentPlaceHolder1_hfRoomSwapFromRegistrationId").val(result[0].RegistrationId);

            //$("#roomChangeRoomSwapContainerTable").hide();

            return false;
        }
        function OnGetFromGuestInfoByIdFailed(error) {
            CommonHelper.SpinnerClose();
        }
        function GetToGuestInfoById(guestId) {
            var roomNumber = $("#ContentPlaceHolder1_hfFromRoom").val();
            var newRoomNumber = $("#ContentPlaceHolder1_hfToRoom").val();

            PageMethods.GetRoomSwapInformationForRoomChangeByRoomNumber_GuestId(newRoomNumber, guestId, OnGetToGuestInfoByIdSucceeded, OnGetToGuestInfoByIdFailed);
        }

        function OnGetToGuestInfoByIdSucceeded(result) {
            if (result[0].RoomId != 0) {
                $(".roomIsRegistered").show();

            }
            else { $(".roomIsRegistered").hide(); }

            //ClearRoomDetails();
            $("#roomChangeRoomSwapContainerTable").show();
            $("#ContentPlaceHolder1_hfCostcenterId").val(result[0].CostCenterId);
            $("#ContentPlaceHolder1_hfRoomId").val(result[0].RoomId);
            $("#ContentPlaceHolder1_hfKotId").val(result[0].KotId);

            if (result[0].KotId != 0) {
                $("#RoomRestaurantOrder").css("background", "#D63CC4");
                $(".roomChangeRoomSwapContainer").show();
            }

            else {
                $("#RoomRestaurantOrder").css("background", "");
                $(".roomChangeRoomSwapContainer").hide();
            }

            if ($("#roomInfoRoomSwapContainer").hasClass('col-md-12')) {
                $("#roomInfoRoomSwapContainer").removeClass("col-md-12");
                $("#roomInfoRoomSwapContainer").addClass("col-md-6");
            }
            $("#ContentPlaceHolder1_hfRoomSwapToRegistrationId").val(result[0].RegistrationId);

            $("#<%=lblRoomSwapRoomNumberToChange.ClientID %>").text(result[0].RoomNumber);
            $("#<%=lblRoomSwapRoomTypeToChange.ClientID %>").text(result[0].RoomType);
            $("#<%=lblRoomSwapDGuestNameToChange.ClientID %>").text(result[0].GuestName);
            $("#<%=lblRoomSwapDGuestSexToChange.ClientID %>").text(result[0].GuestSex);
            //$("#<%=lblRoomSwapDGuestDOBToChange.ClientID %>").text(GetStringFromDateTime(result[0].GuestDOB));

            if (GetStringFromDateTime(result[0].GuestDOB) == "01/01/1970") {
                $("#<%=lblRoomSwapDGuestDOBToChange.ClientID %>").text("");
            }
            else {
                $("#<%=lblRoomSwapDGuestDOBToChange.ClientID %>").text(GetStringFromDateTime(result[0].GuestDOB));
            }

            $("#<%=lblRoomSwapDGuestEmailToChange.ClientID %>").text(result[0].GuestEmail);
            $("#<%=lblRoomSwapDGuestPhoneToChange.ClientID %>").text(result[0].GuestPhone);
            $("#<%=lblRoomSwapDGuestAddress2ToChange.ClientID %>").text(result[0].GuestAddress2);
            $("#<%=lblRoomSwapDGuestNationalityToChange.ClientID %>").text(result[0].GuestNationality);
            $("#<%=lblRoomSwapDCountryNameToChange.ClientID %>").text(result[0].CountryName);
            $("#<%=lblRoomSwapDArrivalDateToChange.ClientID %>").text(GetStringFromDateTime(result[0].ArriveDate));
            $("#<%=lblRoomSwapDExpectedDepartureDateToChange.ClientID %>").text(GetStringFromDateTime(result[0].ExpectedCheckOutDate));
            return false;
        }

        function OnGetToGuestInfoByIdFailed(error) {
            CommonHelper.SpinnerClose();
        }

        function SearchRoomInfoFrom(roomNumber) {
            CommonHelper.SpinnerOpen();
            PageMethods.GetRoomSwapInformationByRoomNumber(roomNumber, OnGetRoomSwapInformationByRoomNumberSucceed, OnGetRoomSwapInformationByRoomNumberFailed);
            return false;
        }
        function OnGetRoomSwapInformationByRoomNumberSucceed(result) {
            if (result[0].RoomId != 0) {
                $(".roomIsRegistered").show();

            }
            else { $(".roomIsRegistered").hide(); }

            //if ((result[0].RoomNumber != "") && (result[1].RoomNumber != "")) {
            //    GetMultipleGuest();
            //}

            ClearRoomDetails();

            $("#ContentPlaceHolder1_hfCostcenterId").val(result[0].CostCenterId);
            $("#ContentPlaceHolder1_hfRoomId").val(result[0].RoomId);
            $("#ContentPlaceHolder1_hfKotId").val(result[0].KotId);

            if (result[0].KotId != 0) {
                $("#RoomRestaurantOrder").css("background", "#D63CC4");
                $(".roomChangeRoomSwapContainer").show();
            }

            else {
                $("#RoomRestaurantOrder").css("background", "");
                $(".roomChangeRoomSwapContainer").hide();
            }

            if ($("#roomInfoRoomSwapContainer").hasClass('col-md-12')) {
                $("#roomInfoRoomSwapContainer").removeClass("col-md-12");
                $("#roomInfoRoomSwapContainer").addClass("col-md-6");
            }

            $("#<%=lblRoomSwapRoomNumber.ClientID %>").text(result[0].RoomNumber);
            $("#<%=lblRoomSwapRoomType.ClientID %>").text(result[0].RoomType);
            $("#<%=lblRoomSwapDGuestName.ClientID %>").text(result[0].GuestName);
            $("#<%=lblRoomSwapDGuestSex.ClientID %>").text(result[0].GuestSex);
            //$("#<%=lblRoomSwapDGuestDOB.ClientID %>").text(GetStringFromDateTime(result[0].GuestDOB));


            if (GetStringFromDateTime(result[0].GuestDOB) == "01/01/1970") {
                $("#<%=lblRoomSwapDGuestDOB.ClientID %>").text("");
            }
            else {
                $("#<%=lblRoomSwapDGuestDOB.ClientID %>").text(GetStringFromDateTime(result[0].GuestDOB));
            }

            $("#<%=lblRoomSwapDGuestEmail.ClientID %>").text(result[0].GuestEmail);
            $("#<%=lblRoomSwapDGuestPhone.ClientID %>").text(result[0].GuestPhone);
            $("#<%=lblRoomSwapDGuestAddress2.ClientID %>").text(result[0].GuestAddress2);
            $("#<%=lblRoomSwapDGuestNationality.ClientID %>").text(result[0].GuestNationality);
            $("#<%=lblRoomSwapDCountryName.ClientID %>").text(result[0].CountryName);
            $("#<%=lblRoomSwapDArrivalDate.ClientID %>").text(GetStringFromDateTime(result[0].ArriveDate));
            $("#<%=lblRoomSwapDExpectedDepartureDate.ClientID %>").text(GetStringFromDateTime(result[0].ExpectedCheckOutDate));

            $("#ContentPlaceHolder1_hfRoomSwapFromRegistrationId").val(result[0].RegistrationId);
            CommonHelper.SpinnerClose();

            //$("#roomChangeRoomSwapContainerTable").hide();

            return false;
        }
        function OnGetRoomSwapInformationByRoomNumberFailed() {
            CommonHelper.SpinnerClose();
        }
        function SearchRoomInfoTo(roomNumber) {
            CommonHelper.SpinnerOpen();
            PageMethods.GetRoomSwapInformationByRoomNumber(roomNumber, OnGetRoomSwapInformationByToRoomNumberSucceed, OnGetRoomSwapInformationByToRoomNumberFailed);
            return false;
        }
        function OnGetRoomSwapInformationByToRoomNumberSucceed(result) {

            if (result[0].RoomId != 0) {
                $(".roomIsRegistered").show();

            }
            else { $(".roomIsRegistered").hide(); }

            //ClearRoomDetails();
            $("#roomChangeRoomSwapContainerTable").show();
            $("#ContentPlaceHolder1_hfCostcenterId").val(result[0].CostCenterId);
            $("#ContentPlaceHolder1_hfRoomId").val(result[0].RoomId);
            $("#ContentPlaceHolder1_hfKotId").val(result[0].KotId);

            if (result[0].KotId != 0) {
                $("#RoomRestaurantOrder").css("background", "#D63CC4");
                $(".roomChangeRoomSwapContainer").show();
            }

            else {
                $("#RoomRestaurantOrder").css("background", "");
                $(".roomChangeRoomSwapContainer").hide();
            }

            if ($("#roomInfoRoomSwapContainer").hasClass('col-md-12')) {
                $("#roomInfoRoomSwapContainer").removeClass("col-md-12");
                $("#roomInfoRoomSwapContainer").addClass("col-md-6");
            }
            $("#ContentPlaceHolder1_hfRoomSwapToRegistrationId").val(result[0].RegistrationId);

            $("#<%=lblRoomSwapRoomNumberToChange.ClientID %>").text(result[0].RoomNumber);
            $("#<%=lblRoomSwapRoomTypeToChange.ClientID %>").text(result[0].RoomType);
            $("#<%=lblRoomSwapDGuestNameToChange.ClientID %>").text(result[0].GuestName);
            $("#<%=lblRoomSwapDGuestSexToChange.ClientID %>").text(result[0].GuestSex);
            //$("#<%=lblRoomSwapDGuestDOBToChange.ClientID %>").text(GetStringFromDateTime(result[0].GuestDOB));

            if (GetStringFromDateTime(result[0].GuestDOB) == "01/01/1970") {
                $("#<%=lblRoomSwapDGuestDOBToChange.ClientID %>").text("");
            }
            else {
                $("#<%=lblRoomSwapDGuestDOBToChange.ClientID %>").text(GetStringFromDateTime(result[0].GuestDOB));
            }

            $("#<%=lblRoomSwapDGuestEmailToChange.ClientID %>").text(result[0].GuestEmail);
            $("#<%=lblRoomSwapDGuestPhoneToChange.ClientID %>").text(result[0].GuestPhone);
            $("#<%=lblRoomSwapDGuestAddress2ToChange.ClientID %>").text(result[0].GuestAddress2);
            $("#<%=lblRoomSwapDGuestNationalityToChange.ClientID %>").text(result[0].GuestNationality);
            $("#<%=lblRoomSwapDCountryNameToChange.ClientID %>").text(result[0].CountryName);
            $("#<%=lblRoomSwapDArrivalDateToChange.ClientID %>").text(GetStringFromDateTime(result[0].ArriveDate));
            $("#<%=lblRoomSwapDExpectedDepartureDateToChange.ClientID %>").text(GetStringFromDateTime(result[0].ExpectedCheckOutDate));
            CommonHelper.SpinnerClose();
            return false;
        }
        function OnGetRoomSwapInformationByToRoomNumberFailed() {
            CommonHelper.SpinnerClose();
        }
        function SearchRoomInfo() {
            var roomNumber = $.trim($("#txtRoomSwapRoomNumber").val());
            var newRoomNumber = $.trim($("#txtRoomSwapAlterRoomNumber").val());
            //var roomNumber = $("#ContentPlaceHolder1_hfFromRoom").val();
            //var newRoomNumber = $("#ContentPlaceHolder1_hfToRoom").val();

            //if (roomNumber == "") {
            //    toastr.warning("Please Give From Room Number."); return false;
            //}
            //else if (newRoomNumber == "") {
            //    toastr.warning("Please Give To Room Number For Change."); return false;
            //}
            //else if (newRoomNumber == roomNumber) {
            //    toastr.warning("Same Room Cannot Be Changed.");
            //    return false;
            //}

            CommonHelper.SpinnerOpen();
            PageMethods.GetRoomSwapInformationForRoomChangeByRoomNumber(0, roomNumber, newRoomNumber, OnGetRegistrationInformationForChangeByRoomNumberSucceeded, OnGetRegistrationInformationForSingleGuestByRoomNumberFailed);
            return false;
        }

        function OnGetRegistrationInformationForChangeByRoomNumberSucceeded(result) {
            CommonHelper.SpinnerClose();
            if (result[0].RegistrationId <= 0) {
                toastr.warning("Please provide valid From room number.");
                $(".roomIsRegistered").hide();
                ClearRoomDetails();
                $("#txtRoomSwapRoomNumber").focus();
            }
            else if (result[1].RegistrationId <= 0) {
                toastr.warning("Please provide valid To room number.");
                $(".roomIsRegistered").hide();
                ClearRoomDetails();
                $("#txtRoomSwapAlterRoomNumber").focus();
            }
            else {
                if (result[0].RoomId != 0) {
                    $(".roomIsRegistered").show();

                }
                else { $(".roomIsRegistered").hide(); }

                ClearRoomDetails();

                $("#ContentPlaceHolder1_hfCostcenterId").val(result[0].CostCenterId);
                $("#ContentPlaceHolder1_hfRoomId").val(result[0].RoomId);
                $("#ContentPlaceHolder1_hfKotId").val(result[0].KotId);

                if (result[0].KotId != 0) {
                    $("#RoomRestaurantOrder").css("background", "#D63CC4");
                    $(".roomChangeRoomSwapContainer").show();
                }

                else {
                    $("#RoomRestaurantOrder").css("background", "");
                    $(".roomChangeRoomSwapContainer").hide();
                }

                if ($("#roomInfoRoomSwapContainer").hasClass('col-md-12')) {
                    $("#roomInfoRoomSwapContainer").removeClass("col-md-12");
                    $("#roomInfoRoomSwapContainer").addClass("col-md-6");
                }

                $("#<%=lblRoomSwapRoomNumber.ClientID %>").text(result[0].RoomNumber);
                $("#<%=lblRoomSwapRoomType.ClientID %>").text(result[0].RoomType);
                $("#<%=lblRoomSwapDGuestName.ClientID %>").text(result[0].GuestName);
                $("#<%=lblRoomSwapDGuestSex.ClientID %>").text(result[0].GuestSex);
                //$("#<%=lblRoomSwapDGuestDOB.ClientID %>").text(GetStringFromDateTime(result[0].GuestDOB));


                if (GetStringFromDateTime(result[0].GuestDOB) == "01/01/1970") {
                    $("#<%=lblRoomSwapDGuestDOB.ClientID %>").text("");
                }
                else {
                    $("#<%=lblRoomSwapDGuestDOB.ClientID %>").text(GetStringFromDateTime(result[0].GuestDOB));
                }

                $("#<%=lblRoomSwapDGuestEmail.ClientID %>").text(result[0].GuestEmail);
                $("#<%=lblRoomSwapDGuestPhone.ClientID %>").text(result[0].GuestPhone);
                $("#<%=lblRoomSwapDGuestAddress2.ClientID %>").text(result[0].GuestAddress2);
                $("#<%=lblRoomSwapDGuestNationality.ClientID %>").text(result[0].GuestNationality);
                $("#<%=lblRoomSwapDCountryName.ClientID %>").text(result[0].CountryName);
                $("#<%=lblRoomSwapDArrivalDate.ClientID %>").text(GetStringFromDateTime(result[0].ArriveDate));
                $("#<%=lblRoomSwapDExpectedDepartureDate.ClientID %>").text(GetStringFromDateTime(result[0].ExpectedCheckOutDate));

                $("#ContentPlaceHolder1_hfRoomSwapFromRegistrationId").val(result[0].RegistrationId);
                //----------------------
                $("#ContentPlaceHolder1_hfRoomSwapToRegistrationId").val(result[1].RegistrationId);

                $("#<%=lblRoomSwapRoomNumberToChange.ClientID %>").text(result[1].RoomNumber);
                $("#<%=lblRoomSwapRoomTypeToChange.ClientID %>").text(result[1].RoomType);
                $("#<%=lblRoomSwapDGuestNameToChange.ClientID %>").text(result[1].GuestName);
                $("#<%=lblRoomSwapDGuestSexToChange.ClientID %>").text(result[1].GuestSex);
                //$("#<%=lblRoomSwapDGuestDOBToChange.ClientID %>").text(GetStringFromDateTime(result[1].GuestDOB));

                if (GetStringFromDateTime(result[1].GuestDOB) == "01/01/1970") {
                    $("#<%=lblRoomSwapDGuestDOBToChange.ClientID %>").text("");
                }
                else {
                    $("#<%=lblRoomSwapDGuestDOBToChange.ClientID %>").text(GetStringFromDateTime(result[1].GuestDOB));
                }

                $("#<%=lblRoomSwapDGuestEmailToChange.ClientID %>").text(result[1].GuestEmail);
                $("#<%=lblRoomSwapDGuestPhoneToChange.ClientID %>").text(result[1].GuestPhone);
                $("#<%=lblRoomSwapDGuestAddress2ToChange.ClientID %>").text(result[1].GuestAddress2);
                $("#<%=lblRoomSwapDGuestNationalityToChange.ClientID %>").text(result[1].GuestNationality);
                $("#<%=lblRoomSwapDCountryNameToChange.ClientID %>").text(result[1].CountryName);
                $("#<%=lblRoomSwapDArrivalDateToChange.ClientID %>").text(GetStringFromDateTime(result[1].ArriveDate));
                $("#<%=lblRoomSwapDExpectedDepartureDateToChange.ClientID %>").text(GetStringFromDateTime(result[1].ExpectedCheckOutDate));
            }
            return false;
        }
        function OnGetRegistrationInformationForSingleGuestByRoomNumberFailed(error) {
            CommonHelper.SpinnerClose();
        }
        function ClearRoomDetails() {

            $("#<%=lblRoomSwapDGuestName.ClientID %>").text("");
            $("#<%=lblRoomSwapDGuestSex.ClientID %>").text("");
            $("#<%=lblRoomSwapDGuestEmail.ClientID %>").text("");
            $("#<%=lblRoomSwapDGuestPhone.ClientID %>").text("");
            $("#<%=lblRoomSwapDGuestAddress2.ClientID %>").text("");
            $("#<%=lblRoomSwapDGuestNationality.ClientID %>").text("");

            $("#txtRoomSwapRoomNumber").text("");
            $("#ContentPlaceHolder1_lblRoomSwapRoomNumber").text("");
            $("#ContentPlaceHolder1_lblRoomSwapRoomType").text("");
        }

        function ClearGuestSwap() {

            $("#fromRoomPaxDiv").hide;
            $("#toRoomPaxDiv").hide;
            $("#ContentPlaceHolder1_ddlPaxFrom").empty();
            $("#ContentPlaceHolder1_ddlPaxTo").empty();


            $("#<%=lblRoomSwapRoomNumber.ClientID %>").text("");
            $("#<%=lblRoomSwapRoomType.ClientID %>").text("");
            $("#<%=lblRoomSwapDGuestName.ClientID %>").text("");
            $("#<%=lblRoomSwapDGuestSex.ClientID %>").text("");
            //$("#<%=lblRoomSwapDGuestDOB.ClientID %>").text(GetStringFromDateTime(result[0].GuestDOB));
            $("#<%=lblRoomSwapDGuestDOB.ClientID %>").text("");



            $("#<%=lblRoomSwapDGuestEmail.ClientID %>").text("");
            $("#<%=lblRoomSwapDGuestPhone.ClientID %>").text("");
            $("#<%=lblRoomSwapDGuestAddress2.ClientID %>").text("");
            $("#<%=lblRoomSwapDGuestNationality.ClientID %>").text("");
            $("#<%=lblRoomSwapDCountryName.ClientID %>").text("");
            $("#<%=lblRoomSwapDArrivalDate.ClientID %>").text("");
            $("#<%=lblRoomSwapDExpectedDepartureDate.ClientID %>").text("");

            $("#ContentPlaceHolder1_hfRoomSwapFromRegistrationId").val("");
            $("#ContentPlaceHolder1_hfRoomSwapFromGuestId").val("");
            $("#ContentPlaceHolder1_hfRoomSwapToGuestId").val("");

            $("#ContentPlaceHolder1_hfRoomSwapToRegistrationId").val("");

            $("#<%=lblRoomSwapRoomNumberToChange.ClientID %>").text("");
            $("#<%=lblRoomSwapRoomTypeToChange.ClientID %>").text("");
            $("#<%=lblRoomSwapDGuestNameToChange.ClientID %>").text("");
            $("#<%=lblRoomSwapDGuestSexToChange.ClientID %>").text("");
            //$("#<%=lblRoomSwapDGuestDOBToChange.ClientID %>").text(GetStringFromDateTime(result[1].GuestDOB));
            $("#<%=lblRoomSwapDGuestDOBToChange.ClientID %>").text("");


            $("#<%=lblRoomSwapDGuestEmailToChange.ClientID %>").text("");
            $("#<%=lblRoomSwapDGuestPhoneToChange.ClientID %>").text("");
            $("#<%=lblRoomSwapDGuestAddress2ToChange.ClientID %>").text("");
            $("#<%=lblRoomSwapDGuestNationalityToChange.ClientID %>").text("");
            $("#<%=lblRoomSwapDCountryNameToChange.ClientID %>").text("");
            $("#<%=lblRoomSwapDArrivalDateToChange.ClientID %>").text("");
            $("#<%=lblRoomSwapDExpectedDepartureDateToChange.ClientID %>").text("");
        }
        function RoomSwapInfo() {
            var fromRegistrationId = $("#ContentPlaceHolder1_hfRoomSwapFromRegistrationId").val();
            var fromGuestId = $("#ContentPlaceHolder1_hfRoomSwapFromGuestId").val();
            //----------------------
            var toRegistrationId = $("#ContentPlaceHolder1_hfRoomSwapToRegistrationId").val();
            var toGuestId = $("#ContentPlaceHolder1_hfRoomSwapToGuestId").val();

            if ((fromRegistrationId == "")) {
                toastr.warning("Please select a guest");
                $("#ContentPlaceHolder1_ddlPaxFrom").focus();
            }
            else if (toRegistrationId == "") {
                toastr.warning("Please select a guest");
                $("#ContentPlaceHolder1_ddlPaxTo").focus();
            }
            CommonHelper.SpinnerOpen();
            PageMethods.SaveUpdateRoomSwapInformation(fromRegistrationId, toRegistrationId, fromGuestId, toGuestId, OnRoomSwapInformationSucceeded, OnGetRegistrationInformationForSingleGuestByRoomNumberFailed);
            return false;
        }
        function OnRoomSwapInformationSucceeded(result) {
            CommonHelper.SpinnerClose();
            //SearchRoomInfo();
            toastr.success(result);
            $(".roomIsRegistered").hide();

            $("#fromRoomPaxDiv").hide();

            $("#toRoomPaxDiv").hide();
            $("#txtRoomSwapRoomNumber").val("");
            $("#txtRoomSwapAlterRoomNumber").val("");

        }
        function RoomSwapInfoMulti() {
            var fromGuestId = "", fromRegId = "", FromGuestIdList = [];
            var toGuestId = "", toRegId = "", ToGuestIdList = [];
            var rowCountFrom = $("#FromGuestSwapGrid tbody tr").length;
            var rowCountTo = $("#ToGuestSwapGrid tbody tr").length;

            $("#FromGuestSwapGrid tbody tr").each(function (index, item) {
                var chkBox = $(item).find("td:eq(1)").find('input');
                fromRegId = $(item).find("td:eq(3)").text();
                fromGuestId = $(item).find("td:eq(4)").text();

                if ($(chkBox).is(":checked")) {

                    FromGuestIdList.push({
                        GuestId: fromGuestId
                    });
                }
                //else {
                //    toastr.warning("No guest is selected in From Room Number.");
                //    return false;
                //}
            });
            var fromCount = FromGuestIdList.length;
            $("#ToGuestSwapGrid tbody tr").each(function (index, item) {
                var chkBox = $(item).find("td:eq(1)").find('input');
                toRegId = $(item).find("td:eq(3)").text();
                toGuestId = $(item).find("td:eq(4)").text();

                if ($(chkBox).is(":checked")) {
                    ToGuestIdList.push({
                        GuestId: toGuestId
                    });
                }
                else {
                    // NotSelectedRegIds.push({
                    //    RegistrationId: registrationId
                    //});
                }
            });
            var toCount = ToGuestIdList.length;

            if ((rowCountFrom <= 1) && (toCount < 1)) {
                toastr.warning("One Guest Cannot Be Swapped.");
                return false;
            }
            if ((rowCountTo <= 1) && (fromCount < 1)) {
                toastr.warning("One Guest Cannot Be Swapped.");
                return false;
            }
            //debugger;
            CommonHelper.SpinnerOpen();
            PageMethods.SaveUpdateRoomSwapInformationMulti(fromRegId, toRegId, FromGuestIdList, ToGuestIdList, OnSaveUpdateRoomSwapInformationMultiSucceeded, OnSaveUpdateRoomSwapInformationMultiFailed);
            return false;
        }
        function OnSaveUpdateRoomSwapInformationMultiSucceeded(result) {
            CommonHelper.SpinnerClose();
            toastr.success(result);
            $("#multiGuestSwapDiv").hide("slow");
            ClearGuestSwap();
            GetMultipleGuest();
            //$("#txtRoomSwapRoomNumber").val("");
            //$("#txtRoomSwapAlterRoomNumber").val("");

        }
        function OnSaveUpdateRoomSwapInformationMultiFailed() {

        }

        // //-------------------Amend Stay Information -------------------------------------------------------------------------
        function SearchAmendStayRoomInfo() {
            var roomNumber = $.trim($("#txtAmendStayRoomNumber").val());
            var typeAmend = $("#ContentPlaceHolder1_ddlTypeAmend option:selected").val();
            var linkSelected = $("#ContentPlaceHolder1_ddlLinkRoomAmend option:selected").val();

            if (typeAmend == "0") {
                toastr.warning("Please Slelect a Room Type First");
                $("#ContentPlaceHolder1_ddlTypeAmend").focus();
                return false;
            }
            else if ((typeAmend == "1") && (roomNumber == "")) {
                toastr.warning("Please Give From Room Number.");
                $("#txtAmendStayRoomNumber").focus();
                return false;
            }
            else if ((typeAmend == "2") && (linkSelected == "0")) {
                toastr.warning("Please select a Link Name.");
                $("#ContentPlaceHolder1_ddlLinkRoomAmend").focus();
                return false;
            }

            if ((typeAmend == "1") && (roomNumber != "")) {
                CommonHelper.SpinnerOpen();
                PageMethods.GetRoomAmendStayByRoomNumber(roomNumber, OnGetRoomAmendStayByRoomNumberSucceeded, OnGetCommonFailed);
                return false;
            }
            else if ((typeAmend == "2") && (linkSelected != "0")) {
                PageMethods.GetRoomAmendStayForLinkedRooms(linkSelected, OnGetRoomAmendStayForLinkedRoomsSucceeded, OnGetRoomAmendStayForLinkedRoomsFailed);
                return false;
            }

        }

        function OnGetRoomAmendStayByRoomNumberSucceeded(result) {
            //toastr.info(result[0].RegistrationId);
            CommonHelper.SpinnerClose();
            //toastr.warning(result.count);
            //if (result[0].RegistrationId == 0) {

            //}
            $("#roomLinkedAmendStay").hide("slow");
            if (result[0].RegistrationId > 0) {
                ClearRoomAmendStayDetails();
                if (result[0].RoomId != 0) {
                    $(".roomIsRegistered").show("slow");
                }
                else { $(".roomIsRegistered").hide(); }


                if (result[0].KotId != 0) {
                    $(".roomChangeAmendStayContainer").show();
                }
                else {
                    $(".roomChangeAmendStayContainer").hide();
                }

                if ($("#roomInfoAmendStayContainer").hasClass('col-md-12')) {
                    $("#roomInfoAmendStayContainer").removeClass("col-md-12");
                    $("#roomInfoAmendStayContainer").addClass("col-md-6");
                }

                $("#<%=lblAmendStayRoomNumber.ClientID %>").text(result[0].RoomNumber);
                $("#<%=lblAmendStayRoomType.ClientID %>").text(result[0].RoomType);
                $("#<%=lblAmendStayDGuestName.ClientID %>").text(result[0].GuestName);
                $("#<%=lblAmendStayDGuestSex.ClientID %>").text(result[0].GuestSex);

                if (GetStringFromDateTime(result[0].GuestDOB) == "01/01/1970") {
                    $("#<%=lblAmendStayDGuestDOB.ClientID %>").text("");
                }
                else {
                    $("#<%=lblAmendStayDGuestDOB.ClientID %>").text(GetStringFromDateTime(result[0].GuestDOB));
                }

                $("#<%=lblAmendStayDGuestEmail.ClientID %>").text(result[0].GuestEmail);
                $("#<%=lblAmendStayDGuestPhone.ClientID %>").text(result[0].GuestPhone);
                $("#<%=lblAmendStayDGuestAddress2.ClientID %>").text(result[0].GuestAddress2);
                $("#<%=lblAmendStayDGuestNationality.ClientID %>").text(result[0].GuestNationality);
                $("#<%=lblAmendStayDCountryName.ClientID %>").text(result[0].CountryName);
                $("#<%=lblAmendStayDArrivalDate.ClientID %>").text(GetStringFromDateTime(result[0].ArriveDate));
                $("#<%=txtAmendStayDExpectedDepartureDate.ClientID %>").val(GetStringFromDateTime(result[0].ExpectedCheckOutDate));
                $("#<%=txtAmendStayDisplayCheckInDate.ClientID %>").val(GetStringFromDateTime(result[0].ArriveDate));
                $("#ContentPlaceHolder1_hfAmendStayFromRegistrationId").val(result[0].RegistrationId);
                $('#ContentPlaceHolder1_txtAmendStayDExpectedDepartureDate').datepicker("option", "minDate", 0);
            }
            else {
                toastr.warning("Please provide a valid room number.");
                $(".roomIsRegistered").hide();
                ClearRoomAmendStayDetails();

            }
            return false;
        }
        function OnGetCommonFailed(error) {
            toastr.error(error.get_message());
            CommonHelper.SpinnerClose();
            return false;
        }
        function OnGetRoomAmendStayForLinkedRoomsSucceeded(result) {
            if (result[0].length > 0) {
                $("#roomLinkedAmendStay").show("slow");
                $(".roomIsRegistered").hide("slow");
                ClearRoomAmendStayDetails();
                $("#ltlLinkedRoomsAmendStay").html(result[0]);
                $("#ContentPlaceHolder1_txtExpectedDepDateLinked").datepicker().datepicker("setDate", new Date());
            }
            else {
                $("#roomLinkedAmendStay").hide();

                toastr.warning("Please provide a valid link room number.");
            }
        }
        function OnGetRoomAmendStayForLinkedRoomsFailed() {

        }





        // //-------------------No Show Posting Information -------------------------------------------------------------------------
        function SearchNoShowPostingRoomInfo() {
            var roomNumber = $.trim($("#txtNoShowPostingRoomNumber").val());
            var typeAmend = $("#ContentPlaceHolder1_ddlNoShowPostingRoomType option:selected").val();
            var linkSelected = $("#ContentPlaceHolder1_ddlLinkRoomNoShowPostingInfo option:selected").val();

            if (typeAmend == "0") {
                toastr.warning("Please Slelect a Room Type First");
                $("#ContentPlaceHolder1_ddlNoShowPostingRoomType").focus();
                return false;
            }
            else if ((typeAmend == "1") && (roomNumber == "")) {
                toastr.warning("Please Give From Room Number.");
                $("#txtNoShowPostingRoomNumber").focus();
                return false;
            }
            else if ((typeAmend == "2") && (linkSelected == "0")) {
                toastr.warning("Please select a Link Name.");
                $("#ContentPlaceHolder1_ddlLinkRoomNoShowPostingInfo").focus();
                return false;
            }

            if ((typeAmend == "1") && (roomNumber != "")) {
                CommonHelper.SpinnerOpen();
                //PageMethods.GetRoomNoShowPostingByRoomNumber(roomNumber, OnGetRoomNoShowPostingByRoomNumberSucceeded, OnGetCommonFailed);
                return false;
            }
            else if ((typeAmend == "2") && (linkSelected != "0")) {
                PageMethods.GetRoomNoShowPostingForLinkedRooms(linkSelected, OnGetRoomNoShowPostingForLinkedRoomsSucceeded, OnGetRoomNoShowPostingForLinkedRoomsFailed);
                return false;
            }

        }

        function OnGetRoomNoShowPostingByRoomNumberSucceeded(result) {
            //toastr.info(result[0].RegistrationId);
            CommonHelper.SpinnerClose();
            //toastr.warning(result.count);
            //if (result[0].RegistrationId == 0) {

            //}
            $("#roomLinkedAmendStay").hide("slow");
            if (result[0].RegistrationId > 0) {
                ClearRoomAmendStayDetails();
                if (result[0].RoomId != 0) {
                    $(".roomIsRegistered").show("slow");
                }
                else { $(".roomIsRegistered").hide(); }


                if (result[0].KotId != 0) {
                    $(".roomChangeAmendStayContainer").show();
                }
                else {
                    $(".roomChangeAmendStayContainer").hide();
                }

                if ($("#roomInfoAmendStayContainer").hasClass('col-md-12')) {
                    $("#roomInfoAmendStayContainer").removeClass("col-md-12");
                    $("#roomInfoAmendStayContainer").addClass("col-md-6");
                }

                $("#<%=lblAmendStayRoomNumber.ClientID %>").text(result[0].RoomNumber);
                $("#<%=lblAmendStayRoomType.ClientID %>").text(result[0].RoomType);
                $("#<%=lblAmendStayDGuestName.ClientID %>").text(result[0].GuestName);
                $("#<%=lblAmendStayDGuestSex.ClientID %>").text(result[0].GuestSex);

                if (GetStringFromDateTime(result[0].GuestDOB) == "01/01/1970") {
                    $("#<%=lblAmendStayDGuestDOB.ClientID %>").text("");
                }
                else {
                    $("#<%=lblAmendStayDGuestDOB.ClientID %>").text(GetStringFromDateTime(result[0].GuestDOB));
                }

                $("#<%=lblAmendStayDGuestEmail.ClientID %>").text(result[0].GuestEmail);
                $("#<%=lblAmendStayDGuestPhone.ClientID %>").text(result[0].GuestPhone);
                $("#<%=lblAmendStayDGuestAddress2.ClientID %>").text(result[0].GuestAddress2);
                $("#<%=lblAmendStayDGuestNationality.ClientID %>").text(result[0].GuestNationality);
                $("#<%=lblAmendStayDCountryName.ClientID %>").text(result[0].CountryName);
                $("#<%=lblAmendStayDArrivalDate.ClientID %>").text(GetStringFromDateTime(result[0].ArriveDate));
                $("#<%=txtAmendStayDExpectedDepartureDate.ClientID %>").val(GetStringFromDateTime(result[0].ExpectedCheckOutDate));
                $("#<%=txtAmendStayDisplayCheckInDate.ClientID %>").val(GetStringFromDateTime(result[0].ArriveDate));
                $("#ContentPlaceHolder1_hfAmendStayFromRegistrationId").val(result[0].RegistrationId);
                $('#ContentPlaceHolder1_txtAmendStayDExpectedDepartureDate').datepicker("option", "minDate", 0);
            }
            else {
                toastr.warning("Please provide a valid room number.");
                $(".roomIsRegistered").hide();
                ClearRoomNoShowPostingDetails();

            }
            return false;
        }
        function OnGetCommonFailed(error) {
            toastr.error(error.get_message());
            CommonHelper.SpinnerClose();
            return false;
        }
        function OnGetRoomNoShowPostingForLinkedRoomsSucceeded(result) {
            if (result[0].length > 0) {
                $("#roomLinkedAmendStay").show("slow");
                $(".roomIsRegistered").hide("slow");
                ClearRoomNoShowPostingDetails();
                $("#ltlLinkedRoomsAmendStay").html(result[0]);
                $("#ContentPlaceHolder1_txtExpectedDepDateLinked").datepicker().datepicker("setDate", new Date());
            }
            else {
                $("#roomLinkedAmendStay").hide();

                toastr.warning("Please provide a valid link room number.");
            }
        }
        function OnGetRoomNoShowPostingForLinkedRoomsFailed() {

        }



        function ClearRoomAmendStayDetails() {
            $("#<%=lblAmendStayDGuestName.ClientID %>").text("");
            $("#<%=lblAmendStayDGuestSex.ClientID %>").text("");
            $("#<%=lblAmendStayDGuestEmail.ClientID %>").text("");
            $("#<%=lblAmendStayDGuestPhone.ClientID %>").text("");
            $("#<%=lblAmendStayDGuestAddress2.ClientID %>").text("");
            $("#<%=lblAmendStayDGuestNationality.ClientID %>").text("");
            $(".roomIsRegistered").hide();
            $("#txtAmendStayRoomNumber").val("");
            $("#ContentPlaceHolder1_lblAmendStayRoomNumber").text("");
            $("#ContentPlaceHolder1_lblAmendStayRoomType").text("");
        }
        function ClearRoomAmendStayDetailsLinked() {
            $("#roomLinkedAmendStay").hide("slow");
            $("#ContentPlaceHolder1_ddlTypeAmend").val("2");
            $("#ContentPlaceHolder1_ddlLinkRoomAmend").val("0").trigger('change');
        }

        function RoomAmendStayInfo() {
            var roomNumber = $.trim($("#ContentPlaceHolder1_txtAmendStayDExpectedDepartureDate").val());

            if (roomNumber == "") {
                $("#ContentPlaceHolder1_txtAmendStayDExpectedDepartureDate").focus();
                toastr.warning("Please Give Expected Departure Date."); return false;
            }
            var registrationId = $("#ContentPlaceHolder1_hfAmendStayFromRegistrationId").val();
            var expectedCheckOutDate = $("#ContentPlaceHolder1_txtAmendStayDExpectedDepartureDate").val();
            var today = $("#ContentPlaceHolder1_hfToday").val();
            CommonHelper.SpinnerOpen();
            PageMethods.RoomAmendStayInformation(registrationId, expectedCheckOutDate, OnRoomAmendStayInformationSucceeded, OnGetCommonFailed);
            return false;
        }
        function OnRoomAmendStayInformationSucceeded(result) {
            CommonHelper.SpinnerClose();
            ClearRoomAmendStayDetails();
            if (result == "Room Amend Stay Successfull.") {
                toastr.success(result);
            }
            else {
                toastr.warning(result);
            }
        }
        function RoomAmendStayInfoLink() {
            var expDate = $('#ContentPlaceHolder1_txtExpectedDepDateLinked').val();
            var RoomStopChargePostingRegIds = [];
            var NotSelectedRegIds = [];
            if (expDate == "") {
                $('#ContentPlaceHolder1_txtExpectedDepDateLinked').focus();
                toastr.warning("Please Give Expected Departure Date."); return false;
            }
            var IsPermitForSave = confirm("Do You Want To Update Stay?");
            if (IsPermitForSave == true) {

                var registrationId = "";
                $("#LikedRoomsInfoGrid tbody tr").each(function (index, item) {
                    var chkBox = $(item).find("td:eq(1)").find('input');
                    registrationId = $(item).find("td:eq(4)").text();

                    if ($(chkBox).is(":checked")) {
                        RoomStopChargePostingRegIds.push({
                            RegistrationId: registrationId
                        });
                    }
                    else {
                        NotSelectedRegIds.push({
                            RegistrationId: registrationId
                        });
                    }
                });
                if (RoomStopChargePostingRegIds.length <= 0) {
                    toastr.warning("Please Select a room for Expected Departure."); return false;
                }

                CommonHelper.SpinnerOpen();
                PageMethods.RoomAmendStayInformationLinked(RoomStopChargePostingRegIds, expDate, OnRoomAmendStayInformationLinkedSucceeded, OnGetCommonFailed);
                return false;
            }
        }
        function OnRoomAmendStayInformationLinkedSucceeded(result) {
            CommonHelper.SpinnerClose();
            //SearchAmendStayRoomInfo();
            toastr.success(result);
            ClearRoomAmendStayDetails();
            ClearRoomAmendStayDetailsLinked();
        }

        function LoadLinkRoomsAmend() {
            PageMethods.GetMasterLinkRoomForddl(OnLoadGetMasterLinkRoomForAmendSucceeded, OnLoadGetMasterLinkRoomForAmendFailed);
            return false;
        }

        function OnLoadGetMasterLinkRoomForAmendSucceeded(result) {
            var list = result;
            var control = $("#<%=ddlLinkRoomAmend.ClientID %>");
            if (list.length > 0) {
                control.empty().append('<option selected="selected" value="0">' + "Please Select" + '</option>');
                $.each(list, function () {
                    control.append($("<option></option>").val(this['Value']).html(this['Text']));
                });
            }
            else {
                $("#inputAmendDiv").show("slow");
                $("#divAmendLinkRoomSelection").hide("slow");
                toastr.warning("No Link Room Found.");
                return false;
            }
        }
        function OnLoadGetMasterLinkRoomForAmendFailed() {

        }

        function LoadLinkRoomsNoShowPostingInfo() {
            PageMethods.GetMasterLinkRoomForddl(OnLoadGetMasterLinkRoomForNoShowPostingInfoSucceeded, OnLoadGetMasterLinkRoomForNoShowPostingInfoFailed);
            return false;
        }

        function OnLoadGetMasterLinkRoomForNoShowPostingInfoSucceeded(result) {
            var list = result;
            var control = $("#<%=ddlLinkRoomNoShowPostingInfo.ClientID %>");
            if (list.length > 0) {
                control.empty().append('<option selected="selected" value="0">' + "Please Select" + '</option>');
                $.each(list, function () {
                    control.append($("<option></option>").val(this['Value']).html(this['Text']));
                });
            }
            else {
                $("#inputNoShowPostingInfoDiv").show("slow");
                $("#divNoShowPostingInfoLinkRoomSelection").hide("slow");
                toastr.warning("No Link Room Found.");
                return false;
            }
        }
        function OnLoadGetMasterLinkRoomForNoShowPostingInfoFailed() {

        }

        // //-------------------Stop Charge Posting Information -------------------------------------------------------------------------
        function LoadLinkRooms() {
            PageMethods.GetMasterLinkRoomForddl(OnLoadGetMasterLinkRoomForddlSucceeded, OnLoadGetMasterLinkRoomForddlFailed);
            return false;
        }
        function OnLoadGetMasterLinkRoomForddlSucceeded(result) {
            var list = result;
            var control = $("#<%=ddlLinkName.ClientID %>");
            if (list.length > 0) {
                //$("#fromRoomPaxDiv").show();
                control.empty().append('<option selected="selected" value="0">' + "Please Select" + '</option>');
                $.each(list, function () {
                    control.append($("<option></option>").val(this['Value']).html(this['Text']));
                });
                //$("#txtStopChargePostingRoomNumber").prop('disabled', true);
            }
            else {
                $("#divLinkRoomSelection").hide("slow");
                $("#roomLinkedStopCharge").hide("slow");
                $("#inputDiv").show("slow");
                toastr.warning("No Link Room Found.");
                return false;
            }
        }
        function OnLoadGetMasterLinkRoomForddlFailed() {

        }
        function BillPendingReleaseInfo() {
            var roomNumber = $.trim($("#txtBillPendingRoomNumber").val());
            if (roomNumber == "") {
                toastr.warning("Please Provide Room Number."); return false;
            }

            if ((roomNumber != "")) {
                CommonHelper.SpinnerOpen();
                PageMethods.BillPendingReleaseProcess(roomNumber, OnBillPendingReleaseInfoSucceeded, OnGetCommonFailed);
                return false;
            }
        }
        function OnBillPendingReleaseInfoSucceeded(result) {
            CommonHelper.SpinnerClose();
            toastr.success("Bill Pending Released Successful.");
        }
        function RoomBillPostingProcessInfo() {
            debugger;
            var roomBillPostingProcessType = $("#ContentPlaceHolder1_ddlRoomBillPostingProcessType option:selected").val();
            if (roomBillPostingProcessType == "0") {
                toastr.warning("Please Provide Posting Type."); return false;
            }
            else {
                var roomNumber = $.trim($("#txtRoomBillPostingRoomNumber").val());
                if (roomNumber == "") {
                    toastr.warning("Please Provide Room Number."); return false;
                }

                if (window.confirm("Do you want to Process Room Bill Posting?")) {
                    if ((roomNumber != "")) {
                        CommonHelper.SpinnerOpen();
                        PageMethods.RoomBillPostingProcess(roomBillPostingProcessType, roomNumber, OnRoomBillPostingProcessSucceeded, OnGetCommonFailed);
                        return false;
                    }
                }
            }
        }
        function OnRoomBillPostingProcessSucceeded(result) {
            CommonHelper.SpinnerClose();
            toastr.success("Bill Pending Released Successful.");
        }
        function SearchStopChargePostingRoomInfo() {
            var roomNumber = $.trim($("#txtStopChargePostingRoomNumber").val());
            var type = $("#ContentPlaceHolder1_ddlType option:selected").val();
            var linkDdl = $("#ContentPlaceHolder1_ddlLinkName option:selected").val();

            if ((type == "0") && (roomNumber == "")) {
                toastr.warning("Please Select Room Type.");
                $("#ContentPlaceHolder1_ddlType").focus();
                return false;
            }
            else if ((type == "1") && (roomNumber == "")) {
                toastr.warning("Please Provide Room Number."); return false;
            }
            if ((linkDdl == "0") && (type == "2")) {
                toastr.warning("Please Select Link Rooms.");
                $("#ContentPlaceHolder1_ddlLinkName").focus();
                return false;
            }

            if ((roomNumber != "") && (type == "1")) {
                CommonHelper.SpinnerOpen();
                PageMethods.GetRoomStopChargePostingByRoomNumber(roomNumber, OnGetRoomStopChargePostingByRoomNumberSucceeded, OnGetCommonFailed);
                return false;
            }
            else if ((linkDdl != "0") && (type == "2")) {
                PageMethods.GetRoomStopChargePostingForLinkedRooms(linkDdl, OnGetRoomStopChargePostingForLinkedRoomsSucceeded, OnGetRoomStopChargePostingForLinkedRoomsFailed);
                return false;
            }
        }

        function OnGetRoomStopChargePostingByRoomNumberSucceeded(result) {
            CommonHelper.SpinnerClose();
            if (result[0].RegistrationId > 0) {
                if (result[0].RoomId != 0) {
                    $(".roomIsRegistered").show();
                    $("#roomLinkedStopCharge").hide();
                }
                else {
                    $(".roomIsRegistered").hide();
                }

                ClearRoomStopChargePostingDetails();
                if (result[0].KotId != 0) {
                    $(".roomChangeStopChargePostingContainer").show();
                }
                else {
                    $(".roomChangeStopChargePostingContainer").hide();
                }

                if ($("#roomInfoStopChargePostingContainer").hasClass('col-md-12')) {
                    $("#roomInfoStopChargePostingContainer").removeClass("col-md-12");
                    $("#roomInfoStopChargePostingContainer").addClass("col-md-6");
                }

                $("#<%=lblStopChargePostingRoomNumber.ClientID %>").text(result[0].RoomNumber);
                $("#<%=lblStopChargePostingRoomType.ClientID %>").text(result[0].RoomType);
                $("#<%=lblStopChargePostingDGuestName.ClientID %>").text(result[0].GuestName);
                $("#<%=lblStopChargePostingDGuestSex.ClientID %>").text(result[0].GuestSex);
                $("#<%=lblStopChargePostingDGuestDOB.ClientID %>").text(GetStringFromDateTime(result[0].GuestDOB));

                if (GetStringFromDateTime(result[0].GuestDOB) == "01/01/1970") {
                    $("#<%=lblStopChargePostingDGuestDOB.ClientID %>").text("");
                }
                else {
                    $("#<%=lblStopChargePostingDGuestDOB.ClientID %>").text(GetStringFromDateTime(result[0].GuestDOB));
                }


            <%-- toastr.info(result[0].GuestDOB.length);
            if (result[0].GuestDOB) {
                $("#<%=lblStopChargePostingDGuestDOB.ClientID %>").text(GetStringFromDateTime(result[0].GuestDOB));
            }--%>

                $("#<%=lblStopChargePostingDGuestEmail.ClientID %>").text(result[0].GuestEmail);
                $("#<%=lblStopChargePostingDGuestPhone.ClientID %>").text(result[0].GuestPhone);
                $("#<%=lblStopChargePostingDGuestAddress2.ClientID %>").text(result[0].GuestAddress2);
                $("#<%=lblStopChargePostingDGuestNationality.ClientID %>").text(result[0].GuestNationality);
                $("#<%=lblStopChargePostingDCountryName.ClientID %>").text(result[0].CountryName);
                $("#<%=lblStopChargePostingDArrivalDate.ClientID %>").text(GetStringFromDateTime(result[0].ArriveDate));
                $("#<%=lblStopChargePostingDExpectedDepartureDate.ClientID %>").text(GetStringFromDateTime(result[0].ExpectedCheckOutDate));
                $("#<%=txtStopChargePostingDisplayCheckInDate.ClientID %>").val(GetStringFromDateTime(result[0].ArriveDate));

                $("#ContentPlaceHolder1_hfStopChargePostingFromRegistrationId").val(result[0].RegistrationId);
                //----------------------
                $("#ltlCalenderControl").html(result[1]);
            }
            else {
                $(".roomIsRegistered").hide();
                ClearRoomStopChargePostingDetails();
                toastr.warning("Please provide a valid room number.");
            }

            //toastr.info(result[1]);

            <%--$("#ContentPlaceHolder1_hfAmendStayToRegistrationId").val(result[1].RegistrationId);

            $("#<%=lblAmendStayRoomNumberToChange.ClientID %>").text(result[1].RoomNumber);
            $("#<%=lblAmendStayRoomTypeToChange.ClientID %>").text(result[1].RoomType);
            $("#<%=lblAmendStayDGuestNameToChange.ClientID %>").text(result[1].GuestName);
            $("#<%=lblAmendStayDGuestSexToChange.ClientID %>").text(result[1].GuestSex);
            $("#<%=lblAmendStayDGuestEmailToChange.ClientID %>").text(result[1].GuestEmail);
            $("#<%=lblAmendStayDGuestPhoneToChange.ClientID %>").text(result[1].GuestPhone);
            $("#<%=lblAmendStayDGuestAddress2ToChange.ClientID %>").text(result[1].GuestAddress2);
            $("#<%=lblAmendStayDGuestNationalityToChange.ClientID %>").text(result[1].GuestNationality);
            $("#<%=lblAmendStayDCountryNameToChange.ClientID %>").text(result[1].CountryName);
            $("#<%=lblAmendStayDArrivalDateToChange.ClientID %>").text(GetStringFromDateTime(result[1].ArriveDate));
            $("#<%=lblAmendStayDExpectedDepartureDateToChange.ClientID %>").text(GetStringFromDateTime(result[1].ExpectedCheckOutDate));--%>

            //$("#ContentPlaceHolder1_txtAmendStayDExpectedDepartureDate").focus();


            return false;
        }
        function OnGetCommonFailed(error) {
            CommonHelper.SpinnerClose();
        }

        function OnGetRoomStopChargePostingForLinkedRoomsSucceeded(result) {
            if (result[0].length > 0) {
                $("#roomLinkedStopCharge").show();
                $(".roomIsRegistered").hide();
                //ClearRoomStopChargePostingDetails();
                //$("#roomInfoStopChargePostingContainer").hide();

                $("#ltlLinkedRooms").html(result[0]);
                $("#ltlCostCeneterTable").html(result[1]);
            }
            else {
                $("#roomLinkedStopCharge").hide();

                toastr.warning("Please provide a valid link room number.");
            }
            //ClearRoomStopChargePostingDetails();
        }
        function OnGetRoomStopChargePostingForLinkedRoomsFailed() {

        }
        function ClearRoomStopChargePostingDetails() {
            $("#ContentPlaceHolder1_ddlType").val("1");
            $("#txtStopChargePostingRoomNumber").val("");

            $("#ContentPlaceHolder1_lblStopChargePostingRoomNumber").text("");
            $("#ContentPlaceHolder1_lblStopChargePostingDArrivalDate").text("");
            $("#ContentPlaceHolder1_lblStopChargePostingRoomType").text("");
            $("#ContentPlaceHolder1_lblStopChargePostingDArrivalDate").text("");
            $("#ContentPlaceHolder1_lblStopChargePostingDExpectedDepartureDate").text("");
            $("#ContentPlaceHolder1_lblStopChargePostingDGuestName").text("");
            $("#ContentPlaceHolder1_lblStopChargePostingDCountryName").text("");
            $("#ContentPlaceHolder1_lblStopChargePostingDGuestSex").text("");
            $("#ContentPlaceHolder1_lblStopChargePostingDGuestDOB").text("");
            $("#ContentPlaceHolder1_lblStopChargePostingDGuestEmail").text("");
            $("#ContentPlaceHolder1_lblStopChargePostingDGuestPhone").text("");
            $("#ContentPlaceHolder1_lblStopChargePostingDGuestAddress2").text("");
            $("#ContentPlaceHolder1_lblStopChargePostingDGuestNationality").text("");

            $("#ExpressCheckInDetailsGrid tbody tr").each(function (index, item) {
                var chkBox = $(item).find("td:eq(1)").find('input');
                $(chkBox).prop('checked', false);
            });
        }
        function ClearStopCHargePostingDivs() {
            $(".roomIsRegistered").hide();
            $("#txtStopChargePostingRoomNumber").val("");
            $("#ContentPlaceHolder1_ddlType").val("2");
            $("#ContentPlaceHolder1_ddlLinkName").val("0").trigger('change');
        }

        function RoomStopChargePostingInfo() {
            var IsPermitForSave = confirm("Do You Want To Update Stop Charge Posting?");
            if (IsPermitForSave == true) {
                CommonHelper.SpinnerOpen();
                var registrationId = "", costCenterId = "";
                registrationId = $("#ContentPlaceHolder1_hfStopChargePostingFromRegistrationId").val();

                var RoomStopChargePostingDetails = [];

                $("#ExpressCheckInDetailsGrid tbody tr").each(function (index, item) {
                    var chkBox = $(item).find("td:eq(1)").find('input');
                    costCenterId = $(item).find("td:eq(3)").text();

                    if ($(chkBox).is(":checked")) {
                        RoomStopChargePostingDetails.push({
                            RegistrationId: registrationId,
                            CostCenterId: costCenterId
                        });
                    }
                });

                PageMethods.SaveUpdateStopChargePosting(registrationId, RoomStopChargePostingDetails, OnSaveUpdateStopChargePostingSucceeded, OnGetCommonFailed);
                return false;
            } else {
                CommonHelper.SpinnerClose();
                return false;

            }
        }
        function OnSaveUpdateStopChargePostingSucceeded(result) {
            CommonHelper.SpinnerClose();
            //SearchAmendStayRoomInfo();
            toastr.success(result);
            ClearRoomStopChargePostingDetails();
        }
        function RoomStopChargePostingInfoLink() {
            var IsPermitForSave = confirm("Do You Want To Update Stop Charge Posting?");
            if (IsPermitForSave == true) {
                CommonHelper.SpinnerOpen();
                var registrationId = "", costCenterId = "";
                //registrationId = $("#ContentPlaceHolder1_hfStopChargePostingFromRegistrationId").val();

                var RoomStopChargePostingDetails = [];
                var RoomStopChargePostingRegIds = [];
                var NotSelectedRegIds = [];

                $("#ExpressCheckInDetailsGrid tbody tr").each(function (index, item) {
                    var chkBox = $(item).find("td:eq(1)").find('input');
                    costCenterId = $(item).find("td:eq(3)").text();

                    if ($(chkBox).is(":checked")) {
                        RoomStopChargePostingDetails.push({
                            //RegistrationId: registrationId,
                            CostCenterId: costCenterId
                        });
                    }
                });
                $("#LikedRoomsInfoGridStop tbody tr").each(function (index, item) {
                    var chkBox = $(item).find("td:eq(1)").find('input');
                    registrationId = $(item).find("td:eq(4)").text();

                    if ($(chkBox).is(":checked")) {
                        RoomStopChargePostingRegIds.push({
                            RegistrationId: registrationId
                        });
                    }
                    else {
                        NotSelectedRegIds.push({
                            RegistrationId: registrationId
                        });
                    }
                });
                //$("#LikedRoomsInfoGrid tbody tr").each(function (index, item) {
                //    var chkBox = $(item).find("td:eq(1)").find('input');
                //    registrationId = $(item).find("td:eq(4)").text();

                //    if ($(chkBox).is(":checked")) {
                //        NotSelectedRegIds.push({

                //            RegistrationId: registrationId
                //        });
                //    }
                //    else {

                //    }
                //});
                //debugger;
                if (RoomStopChargePostingRegIds.length <= 0) {
                    toastr.warning("Please Select Room.");
                    return false;
                }
                if (RoomStopChargePostingDetails.length <= 0) {
                    toastr.warning("Please Select outlet Name.");
                    return false;
                }

                PageMethods.SaveUpdateStopChargePostingLink(RoomStopChargePostingRegIds, RoomStopChargePostingDetails, NotSelectedRegIds, OnSaveUpdateStopChargePostingLinkSucceeded, OnGetCommonFailed);
                return false;
            } else {
                CommonHelper.SpinnerClose();
                return false;

            }
        }
        function OnSaveUpdateStopChargePostingLinkSucceeded(result) {
            CommonHelper.SpinnerClose();
            //SearchAmendStayRoomInfo();
            toastr.success(result);
            ClearRoomStopChargePostingDetails();
            ClearStopCHargePostingDivs();
        }
        // Guest Preference
        function LoadGuestPreference() {
            LoadGuestPreferenceInfo();
            //popup(1, 'DivGuestReference', '', 600, 525);
            $("#DivGuestPreference").dialog({
                width: 600,
                height: 525,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Guest Preference",
                show: 'slide'
            });
            return false;
        }

        function LoadGuestPreferenceInfo() {
            PageMethods.LoadGuestPreferenceInfo(OnLoadGuestPreferenceSucceeded, OnLoadGuestPreferenceFailed);
            return false;
        }
        function OnLoadGuestPreferenceSucceeded(result) {
            $("#ltlGuestPreference").html(result);

            var PreferenceIdList = "";

            PreferenceIdList = $("#ContentPlaceHolder1_hfGuestPreferenceId").val();

            if (SelectdPreferenceId != "") {
                var PreferenceArray = SelectdPreferenceId.split(",");
                if (PreferenceArray.length > 0) {
                    for (var i = 0; i < PreferenceArray.length; i++) {
                        var preferenceId = "#" + PreferenceArray[i].trim();
                        $(preferenceId).attr("checked", true);
                    }
                }
                SelectdPreferenceId = "";
            }

            if (PreferenceIdList != "") {
                var SavedPreferenceArray = PreferenceIdList.split(",");
                if (SavedPreferenceArray.length > 0) {
                    for (var i = 0; i < SavedPreferenceArray.length; i++) {
                        var preferenceId = "#" + SavedPreferenceArray[i].trim();
                        $(preferenceId).attr("checked", true);
                    }
                }
            }
            return false;
        }
        function OnLoadGuestPreferenceFailed() {
        }
        function GetCheckedGuestPreference() {
            $("#GuestPreferenceDiv").show();
            var SelectdPreferenceName = "";

            $('#GuestPreferenceInformation tbody tr').each(function () {
                var chkBox = $(this).find("td:eq(1)").find("input");

                if ($(chkBox).is(":checked") == true) {
                    if (SelectdPreferenceId != "") {
                        SelectdPreferenceId += ',' + $(chkBox).attr('value');
                        SelectdPreferenceName += ', ' + $(chkBox).attr('name');
                    }
                    else {
                        SelectdPreferenceId = $(chkBox).attr('value');
                        SelectdPreferenceName = $(chkBox).attr('name');
                    }
                }
            });
            $("#<%=lblGstPreference.ClientID %>").text(SelectdPreferenceName);
            $("#DivGuestPreference").dialog("close");
        }

        function ClosePreferenceDialog() {
            $("#DivGuestPreference").dialog("close");
        }

        function SaveGuestInformation() {
            //Guest Detail            
            var txtTitle = $("#<%=ddlTitle.ClientID %>").val();
            var titleText = $("#<%=ddlTitle.ClientID %> option:selected").text();

            if (txtTitle == "MrNMrs.") {
                txtTitle = "Mr. & Mrs.";
            }
            else if (txtTitle == "N/A") {
                title = "";
            }
            var txtFirstName = $("#<%=txtFirstName.ClientID %>").val().trim();
            var txtLastName = $("#<%=txtLastName.ClientID %>").val().trim();
            var txtGuestName = $("#<%=txtGuestName.ClientID %>").val().trim();
            txtGuestName = txtGuestName.trim();
            var txtGuestEmail = $("#<%=txtGuestEmail.ClientID %>").val();
            var hiddenGuestId = $("#<%=hiddenGuestId.ClientID %>").val();
            var txtGuestDrivinlgLicense = $("#<%=txtGuestDrivinlgLicense.ClientID %>").val();
            var txtGuestDOB = $("#<%=txtGuestDOB.ClientID %>").val();
            var txtGuestAddress1 = $("#<%=txtGuestAddress1.ClientID %>").val();
            var txtGuestAddress2 = $("#<%=txtGuestAddress2.ClientID %>").val();
            var ddlProfessionId = $("#<%=ddlProfessionId.ClientID %>").val();
            var txtGuestCity = $("#<%=txtGuestCity.ClientID %>").val();
            var txtGuestNationality = $("#<%=txtGuestNationality.ClientID %>").val();
            var txtNumberOfPersonAdult = 0;
            var txtGuestPhone = $("#<%=txtGuestPhone.ClientID %>").val();

            var ddlGuestSex = $("#<%=ddlGuestSex.ClientID %>").val();
            if (ddlGuestSex == "0") {
                ddlGuestSex = "Male";
            }
            var txtGuestZipCode = $("#<%=txtGuestZipCode.ClientID %>").val();
            var txtNationalId = $("#<%=txtNationalId.ClientID %>").val();
            var txtPassportNumber = $("#<%=txtPassportNumber.ClientID %>").val();
            var txtPExpireDate = $("#<%=txtPExpireDate.ClientID %>").val();
            var txtPIssueDate = $("#<%=txtPIssueDate.ClientID %>").val();
            var txtPIssuePlace = '';
            var txtVExpireDate = $("#<%=txtVExpireDate.ClientID %>").val();
            var txtVisaNumber = $("#<%=txtVisaNumber.ClientID %>").val();
            var txtVIssueDate = $("#<%=txtVIssueDate.ClientID %>").val();

            if (txtGuestName == "") {
                if (txtGuestName == "") {
                    toastr.warning("Please provide Guest Name.");
                    $("#<%=txtGuestName.ClientID %>").focus();
                }
                return;
            }

            var ddlGuestCountry = $("#<%=ddlGuestCountry.ClientID %>").val();
            var txtGuestCountrySearch = $.trim($("#txtGuestCountrySearch").val());
            var defaultCountry = $.trim($("#<%= hfDefaultCountryId.ClientID %>").val());
            var enteredCountry = $.trim($("#<%=ddlGuestCountry.ClientID %>").find('option:selected').text());

            if (enteredCountry.toString() != txtGuestCountrySearch.toString()) {
                toastr.warning('Please Enter Valid Country Name.');
                $("#txtGuestCountrySearch").focus();
                return;
            }

            if (txtTitle == "0") {
                toastr.warning('Please Select Title.');
                $("#ddlTitle").focus();
                return;
            }

            if (txtGuestCountrySearch == "") {
                toastr.warning('Please Enter Country Name.');
                $("#txtGuestCountrySearch").focus();
                return;
            }

            if (defaultCountry != ddlGuestCountry) {
                if ($.trim($("#ContentPlaceHolder1_txtPassportNumber").val()) == "") {
                    toastr.warning('Please Enter Passport Number.');
                    $("#ContentPlaceHolder1_txtPassportNumber").focus();
                    return;
                }
            }

            if (txtGuestEmail != "") {
                var mailformat = new RegExp('^[a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,15})$', 'i');
                if (txtGuestEmail.match(mailformat)) {
                }
                else {
                    toastr.warning("You have entered an invalid email address!");
                    document.getElementById("txtGuestEmail>").focus();
                    return;
                }
            }

            if (ddlGuestSex == "0") {
                toastr.warning('Please Select Gender');
                return;
            }

            // Document Detail
            var isEdit = "";
            var RandomOwnerId = 0;
            var IntOwner = 0;
            var IsEditAfterRegistration = 0;
            var deletedGuestId = "";

            var guestDeletedDoc = $("#<%=hfGuestDeletedDoc.ClientID %>").val();
            PageMethods.SaveGuestInformation(hiddenGuestId, txtTitle, txtFirstName, txtLastName, txtGuestName, txtGuestEmail, hiddenGuestId, txtGuestDrivinlgLicense, txtGuestDOB, txtGuestAddress1, txtGuestAddress2, ddlProfessionId, txtGuestCity, ddlGuestCountry, txtGuestNationality, txtGuestPhone, ddlGuestSex, txtGuestZipCode, txtNationalId, txtPassportNumber, txtPExpireDate, txtPIssueDate, txtPIssuePlace, txtVExpireDate, txtVisaNumber, txtVIssueDate, guestDeletedDoc, deletedGuestId, SelectdPreferenceId, OnLoadDetailGridInformationSucceeded, OnLoadDetailGridInformationFailed);
            return false;
        }

        function OnLoadDetailGridInformationSucceeded(result) {
            CommonHelper.SpinnerClose();
            toastr.success(result);
            SelectdPreferenceId = "";
            clearUserDetailsControl();
            return false;
        }
        function OnLoadDetailGridInformationFailed(error) {
            if (error.toString == "2")
                toastr.warning('Provide Valid Email');
            toastr.error(error.get_message());
        }

        //---------- Linked Room Information------------------------//
        function AddByRoomNumber() {
            var roomNumber = $("#ContentPlaceHolder1_txtRoomNumberLinked").val();
            if (roomNumber == "") {
                toastr.warning("Please Provide Room Number.");
                $("#ContentPlaceHolder1_txtRoomNumberSearch").focus();
                return false;
            }
            PageMethods.GetRegistrationInfoByRoomNumber(roomNumber, onLoadRegistrationInfoSucceed, onLoadRegistrationInfoFailed);
        }
        function onLoadRegistrationInfoSucceed(result) {

            //$("#TblLinkedRoom tbody").html("");
            var rowLength = $("#TblLinkedRoom tbody tr").length;
            var tr = "";

            var alreadyExist = result[0].AlreadyExistRoom;
            var newAdded = result[0].NewAddedRoom;
            var tblRoomNumber = "";

            if (alreadyExist.length > 0) {
                toastr.warning("This item has already added. Please add another");
                $("#ContentPlaceHolder1_txtRoomNumberLinked").val("");
                return false;
            }
            else if (newAdded[0].RegistrationId == "") {
                toastr.warning("This Room is not registered. Please add another");
                return false;
            }
            else {
                //if (rowLength % 2 == 0) {
                //    tr += "<tr style='background-color:#FFFFFF;'>";
                //}
                //else {
                //    tr += "<tr style='background-color:#E3EAEB;'>";
                //}

                $("#TblLinkedRoom tbody tr").each(function () {
                    tblRoomNumber = $.trim($(this).find("td:eq(0)").text());
                });

                if (newAdded[0].RoomNumber == tblRoomNumber) {
                    toastr.warning("This item has already added.");
                    return false;
                }
                else {
                    tr += "<tr>";
                    tr += "<td style=\"width: 20%;\">" + newAdded[0].RoomNumber + "</td>";//0
                    tr += "<td style=\"width: 20%;\">" + newAdded[0].RegistrationNumber + "</td>";//1

                    tr += "<td style=\"width: 45%;\">" + newAdded[0].GuestName + "</td>";//2
                    tr += "<td style=\"display:none;\">" + newAdded[0].RegistrationId + "</td>";
                    tr += "<td style=\"display:none;\">0 </td>";
                    tr += "<td align='center' style=\"width:15%; cursor:pointer;\"><img src='../Images/delete.png' class= 'RoomDelete'  alt='Delete Room' border='0' /></td>";

                    tr += "</tr>";

                    $("#TblLinkedRoom tbody").append(tr);
                }


            }
            $("#ContentPlaceHolder1_txtRoomNumberLinked").val("");
            //CommonHelper.ApplyIntigerValidation();

        }
        function onLoadRegistrationInfoFailed(error) {
            toastr.error(error.get_message());
            return false;
        }

        //------ Search Room ----------------
        function SearchByRoomNumber() {
            var roomNumber = $("#ContentPlaceHolder1_txtRoomNumberSearch").val();
            if (roomNumber == "") {
                toastr.warning("Please Provide Link Room Number.");
                $("#ContentPlaceHolder1_txtRoomNumberSearch").focus();
                return false;
            }
            PageMethods.GetLinkedRoomByRoomNumber(roomNumber, OnLoadSearchRoomSucceed, OnLoadSearchRoomFailed);
        }
        function OnLoadSearchRoomSucceed(result) {
            if (result.length == 0) {
                toastr.info("No Link Room found. Please add first.");
                clearLinkedRoomInfo();
                return false;
            }
            else {
                var masterRoomInfo = result[0].MasterRoomInfo;// carries guest name, regNum, roomNum,
                var masterRoom = result[0].MasterRoom; //carries link name n description
                var detailRooms = result[0].DetailRooms;
                var tr = "";
                var tblRoomNumber = "";

                var rowLength = $("#TblLinkedRoom tbody tr").length;

                $("#ContentPlaceHolder1_txtRoomNumberLinked").val("");

                if (rowLength > 0) {
                    clearLinkedRoomInfo();

                }

                if (detailRooms.length <= 0) {
                    toastr.warning("No Room Found. Please enter a valid room number.");
                    return false;
                }
                else {
                    for (var i = 0; i < detailRooms.length; i++) {

                        //if (rowLength % 2 == 0) {
                        //tr += "<tr style='background-color:#ffffff;'>";
                        //}
                        //else {
                        //tr += "<tr style='background-color:#e3eaeb;'>";
                        //}
                        tr += "<tr>";
                        tr += "<td style=\"width: 20%;\">" + detailRooms[i].RoomNumber + "</td>";
                        tr += "<td style=\"width: 20%;\">" + detailRooms[i].RegistrationNumber + "</td>";
                        tr += "<td style=\"width: 45%;\">" + detailRooms[i].GuestName + "</td>";
                        tr += "<td style=\"display:none;\">" + detailRooms[i].RegistrationId + "</td>";
                        tr += "<td style=\"display:none;\">" + detailRooms[i].Id + "</td>";
                        tr += "<td style=\"display:none;\">" + detailRooms[i].MasterId + "</td>";
                        tr += "<td align='center' style=\"width:15%; cursor:pointer;\"><img src='../Images/delete.png' class= 'RoomDelete'  alt='Delete Room' border='0' /></td>";

                        tr += "</tr>";

                        $("#TblLinkedRoom tbody").append(tr);
                        tr = "";
                    }
                    $("#ContentPlaceHolder1_txtMasterRoom").val(masterRoomInfo[0].RoomNumber);

                    $("#ContentPlaceHolder1_txtRegistrationNumber").val(masterRoomInfo[0].RegistrationNumber);
                    $("#ContentPlaceHolder1_txtGuestNametbl").val(masterRoomInfo[0].GuestName);
                    $("#ContentPlaceHolder1_txtGroupName").val(masterRoom[0].LinkName);
                    $("#ContentPlaceHolder1_txtDescription").val(masterRoom[0].Description);

                    //keeping in hidden fields
                    $("#ContentPlaceHolder1_hfSelectedMasterRoomRegId").val(masterRoom[0].RegistrationId);
                    $("#ContentPlaceHolder1_hfSelectedMasterId").val(masterRoom[0].Id);
                    $("#ContentPlaceHolder1_hfSelectedMasterRoomRegNum").val(masterRoomInfo[0].RegistrationNumber);
                    $("#ContentPlaceHolder1_hfSelectedMasterRoomGuest").val(masterRoomInfo[0].GuestName);
                    $("#ContentPlaceHolder1_hfSelectedMasterRoomNumber").val(masterRoomInfo[0].RoomNumber);
                    $("#ContentPlaceHolder1_hfSelectedMasterRoomPK").val(masterRoomInfo[0].Id);

                }
            }


        }
        function OnLoadSearchRoomFailed(error) {
            toastr.error(error.get_message());
            return false;
        }

        //------- Master pop up and selection---------------
        function masterSelection() {
            var rowLength = $("#TblLinkedRoom tbody tr").length;

            if (rowLength <= 0) {
                toastr.warning("Please Add Room.");
                return false;
            }
            var roomNumberArr = new Array();
            var tblRoomNumber = "0";
            var tblRegNumber = "0";
            var tblGuestName = "";
            var tblRegId = "0";
            var tblPK = "0";
            var tblMasterId = "0";

            var tempArr = new Array();
            //var regNumberArr = new Array();
            //var guestNameArr = new Array();

            var ddlMasterRoom = '<%=ddlMasterRoom.ClientID%>';
            var control = $('#' + ddlMasterRoom);
            control.empty();

            $("#TblLinkedRoom tbody tr").each(function () {
                tblRoomNumber = $.trim($(this).find("td:eq(0)").text());
                tblRegNumber = $.trim($(this).find("td:eq(1)").text());
                tblGuestName = $.trim($(this).find("td:eq(2)").text());
                tblRegId = $.trim($(this).find("td:eq(3)").text());
                tblPK = $.trim($(this).find("td:eq(4)").text());
                tblMasterId = $.trim($(this).find("td:eq(5)").text());

                roomNumberArr.push({
                    RoomNumber: tblRoomNumber,
                    RegistrationNumber: tblRegNumber,
                    GuestName: tblGuestName,
                    RegistrationId: tblRegId,
                    PK: tblPK,
                    MasterId: tblMasterId
                });

                if (tblRegId != null) {
                    if (tblRoomNumber != "") {
                        control.append('<option title="' + tblRoomNumber + '" value="' + tblRegId + '">' + tblRoomNumber + '</option>');
                    }
                }


            });
            var tempRegId = $("#ContentPlaceHolder1_hfSelectedMasterRoomRegId").val();
            var tempRegNum = $("#ContentPlaceHolder1_hfSelectedMasterRoomRegNum").val();
            var tempGuest = $("#ContentPlaceHolder1_hfSelectedMasterRoomGuest").val();
            var tempRoom = $("#ContentPlaceHolder1_hfSelectedMasterRoomNumber").val();
            var tempPK = $("#ContentPlaceHolder1_hfSelectedMasterRoomPK").val();
            var tempMasterId = $("#ContentPlaceHolder1_hfSelectedMasterId").val();

            tempArr.push({
                RoomNumber: tempRoom,
                RegistrationNumber: tempRegNum,
                GuestName: tempGuest,
                RegistrationId: tempRegId,
                PK: tempPK,
                MasterId: tempMasterId
            });

            if (tempArr[0].RegistrationId != "") {
                control.append('<option title="' + tempArr[0].RoomNumber + '" value="' + tempArr[0].RegistrationId + '">' + tempArr[0].RoomNumber + '</option>');
            }
            //add the master room in dropdown

            if (tempRegId != "") {
                $("#<%=ddlMasterRoom.ClientID %>").val(tempRegId);
            }

            //$("#MasterRoomSelectPopUp").show('slow');

            $("#MasterRoomSelectPopUp").dialog({
                width: 400,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Master Room Selection"
            });
        }

        function SelectMasterRoom() {

            CommonHelper.ExactMatch();

            var selectedMasterRoomText = $("#<%=ddlMasterRoom.ClientID %> option:selected").title;
            var selectedMasterRegId = $("#<%=ddlMasterRoom.ClientID %> option:selected").val();

            var tblRoomNumber = "";
            var tblRegNumber = "0";
            var tblGuestName = "0";
            var tblRegId = "0";
            var tblPK = "0";
            var tblMasterId = "0";
            var tr = "";

            var tableData = new Array();
            var tempArr = new Array();
            var result = new Array();

            var tempRegId = $("#ContentPlaceHolder1_hfSelectedMasterRoomRegId").val();
            var tempRegNum = $("#ContentPlaceHolder1_hfSelectedMasterRoomRegNum").val();
            var tempGuest = $("#ContentPlaceHolder1_hfSelectedMasterRoomGuest").val();
            var tempRoom = $("#ContentPlaceHolder1_hfSelectedMasterRoomNumber").val();
            var tempPK = $("#ContentPlaceHolder1_hfSelectedMasterRoomPK").val();
            var tempMasterId = $("#ContentPlaceHolder1_hfSelectedMasterId").val();

            tempArr.push({
                RoomNumber: tempRoom,
                RegistrationNumber: tempRegNum,
                GuestName: tempGuest,
                RegistrationId: tempRegId,
                PK: tempPK,
                MasterId: tempMasterId
            });//previous master room



            $("#TblLinkedRoom tbody tr").each(function () {
                tblRoomNumber = $.trim($(this).find("td:eq(0)").text());
                tblRegNumber = $.trim($(this).find("td:eq(1)").text());
                tblGuestName = $.trim($(this).find("td:eq(2)").text());
                tblRegId = $.trim($(this).find("td:eq(3)").text());
                tblPK = $.trim($(this).find("td:eq(4)").text());

                tableData.push({
                    RoomNumber: tblRoomNumber,
                    RegistrationNumber: tblRegNumber,
                    GuestName: tblGuestName,
                    RegistrationId: tblRegId,
                    PK: tblPK
                });
            });// all table data

            //remove from table if exists there 
            var removeRow = $("#TblLinkedRoom tbody tr").find("td:eq(3):textEquals('" + selectedMasterRegId + "')").parent();
            $(removeRow).remove();




            //add here
            if (tempArr[0].RegistrationId != "") {
                tr += "<tr>";
                tr += "<td style=\"width: 20%;\">" + tempArr[0].RoomNumber + "</td>";//0
                tr += "<td style=\"width: 20%;\">" + tempArr[0].RegistrationNumber + "</td>";//1

                tr += "<td style=\"width: 45%;\">" + tempArr[0].GuestName + "</td>";//2
                tr += "<td style=\"display:none;\">" + tempArr[0].RegistrationId + "</td>";
                tr += "<td style=\"display:none;\">" + tempArr[0].PK + "</td>";
                tr += "<td align='center' style=\"width:15%; cursor:pointer;\"><img src='../Images/delete.png' class= 'RoomDelete'  alt='Delete Room' border='0' /></td>";

                tr += "</tr>";
                $("#TblLinkedRoom tbody").append(tr);
            }

            var selectedMaster = _.findWhere(tableData, { RegistrationId: selectedMasterRegId });

            $("#ContentPlaceHolder1_txtMasterRoom").val(selectedMaster.RoomNumber);
            $("#ContentPlaceHolder1_txtRegistrationNumber").val(selectedMaster.RegistrationNumber);
            $("#ContentPlaceHolder1_txtGuestNametbl").val(selectedMaster.GuestName);
            $("#ContentPlaceHolder1_hfSelectedMasterRoomRegId").val(selectedMasterRegId);
            $("#ContentPlaceHolder1_hfSelectedMasterRoomPK").val(selectedMaster.PK);

            var hfRegId = $("#ContentPlaceHolder1_hfSelectedMasterRoomRegId").val();
            var hfRegNum = $("#ContentPlaceHolder1_txtRegistrationNumber").val();
            var hfRoomNo = $("#ContentPlaceHolder1_txtMasterRoom").val();
            var hfGuestName = $("#ContentPlaceHolder1_txtGuestNametbl").val();
            var hfPK = $("#ContentPlaceHolder1_hfSelectedMasterRoomPK").val();



            $("#ContentPlaceHolder1_hfSelectedMasterRoomRegId").val(hfRegId);
            $("#ContentPlaceHolder1_hfSelectedMasterRoomRegNum").val(hfRegNum);
            $("#ContentPlaceHolder1_hfSelectedMasterRoomGuest").val(hfGuestName);
            $("#ContentPlaceHolder1_hfSelectedMasterRoomNumber").val(hfRoomNo);
            $("#ContentPlaceHolder1_hfSelectedMasterRoomPK").val(hfPK);



            $("#MasterRoomSelectPopUp").dialog("close");
        }

        function MasterRoomSave() {

            var tblRoomNumber = "";
            var tblRegNumber = "0";
            var tblGuestName = "0";
            var tblRegId = "0";
            var tblPK = "0";

            var masterRoomRegId = $("#ContentPlaceHolder1_hfSelectedMasterRoomRegId").val();
            var groupName = $("#ContentPlaceHolder1_txtGroupName").val();
            var description = $("#ContentPlaceHolder1_txtDescription").val();
            var pk = $("#ContentPlaceHolder1_hfSelectedMasterRoomPK").val()

            var tableData = new Array();
            var masterData = new Array();

            $("#TblLinkedRoom tbody tr").each(function () {
                tblRoomNumber = $.trim($(this).find("td:eq(0)").text());
                tblRegNumber = $.trim($(this).find("td:eq(1)").text());
                tblGuestName = $.trim($(this).find("td:eq(2)").text());
                tblRegId = $.trim($(this).find("td:eq(3)").text());
                tblPK = $.trim($(this).find("td:eq(4)").text());;

                tableData.push({
                    RoomNumber: tblRoomNumber,
                    RegistrationNumber: tblRegNumber,
                    GuestName: tblGuestName,
                    RegistrationId: tblRegId,
                    Id: tblPK
                });
            });

            masterData.push({
                RegistrationId: masterRoomRegId,
                LinkName: groupName,
                Description: description,
                Id: pk
            });

            if (masterRoomRegId == "") {
                toastr.warning("Please select a master room");
                return false;
            }
            else if (groupName == "") {
                toastr.warning("Please enter a Group Name");
                return false;
            }
            else if ((tableData.length <= 0) && (masterData.length <= 0)) {
                toastr.warning("Please add some rooms and select a master room");
                return false;
            }
            else {
                PageMethods.SaveMasterAndDetailRooms(tableData, masterData, masterRoomRegId, groupName, description, OnSaveMasterandDetailRoomsSucceed, OnSaveMasterandDetailRoomsFailed);
                return false;
            }

        }

        function OnSaveMasterandDetailRoomsSucceed(result) {
            if (result.IsSuccess) {
                //$("#myTabs").tabs('load', 5);
                CommonHelper.AlertMessage(result.AlertMessage);
                clearLinkedRoomInfo();
            }
            else {

                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnSaveMasterandDetailRoomsFailed(error) {
            toastr.error(error.get_message());
        }

        function MasterRoomSelectPopUpClose() {
            $("#MasterRoomSelectPopUp").dialog("close");
        }

        function CheckLinkedRooms(masterId) {
            PageMethods.CheckLinkedRooms(masterId, OnCheckLinkedRoomsSucceed, OnCheckLinkedRoomsFailed);

        }
        function OnCheckLinkedRoomsSucceed(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            clearLinkedRoomInfo();
        }
        function OnCheckLinkedRoomsFailed(error) {
            //toastr.error(error.get_message());
            clearLinkedRoomInfo();
        }

        function clearLinkedRoomInfo() {
            //$('#TblLinkedRoom').find('tr').remove();
            $("#TblLinkedRoom").find("tr:not(:first)").remove();
            //$("#TblLinkedRoom").find("tr:gt(0)").remove();
            $("#<%=txtGroupName.ClientID %>").val('');
            $("#<%=txtMasterRoom.ClientID %>").val('');
            $("#<%=txtRegistrationNumber.ClientID %>").val('');
            $("#<%=txtGuestNametbl.ClientID %>").val('');
            $("#<%=txtDescription.ClientID %>").val('');
            $("#ContentPlaceHolder1_txtRoomNumberSearch").val("");
            $("#ContentPlaceHolder1_hfSelectedMasterRoomRegId").val("");
            $("#ContentPlaceHolder1_hfSelectedMasterRoomRegNum").val("");
            $("#ContentPlaceHolder1_hfSelectedMasterRoomGuest").val("");
            $("#ContentPlaceHolder1_hfSelectedMasterRoomNumber").val("");
            $("#ContentPlaceHolder1_hfSelectedMasterRoomPK").val("");
            $("#ContentPlaceHolder1_hfSelectedMasterId").val("")
        }
        function clearUserDetailsControl() {
            $("#<%=txtFirstName.ClientID %>").val('');
            $("#<%=txtLastName.ClientID %>").val('');
            $("#<%=txtGuestName.ClientID %>").val('');
            $("#<%=ddlTitle.ClientID %>").val('0');
            $("#<%=txtGuestEmail.ClientID %>").val('');
            $("#<%=hiddenGuestId.ClientID %>").val('0');
            $("#DocumentInfo").html('');
            $("#<%=txtGuestDrivinlgLicense.ClientID %>").val('');
            $("#<%=txtGuestDOB.ClientID %>").val('');
            $("#<%=txtGuestAddress1.ClientID %>").val('');
            $("#<%=txtGuestAddress2.ClientID %>").val('');
            $("#<%=ddlProfessionId.ClientID %>").val('0');
            $("#<%=txtGuestCity.ClientID %>").val('');
            $("#<%=ddlGuestCountry.ClientID %>").val('');
            $("#<%=txtGuestNationality.ClientID %>").val('');
            $("#<%=txtGuestPhone.ClientID %>").val('');
            $("#<%=ddlGuestSex.ClientID %>").val('0');
            $("#<%=txtGuestZipCode.ClientID %>").val('');
            $("#<%=txtNationalId.ClientID %>").val('');
            $("#<%=txtPassportNumber.ClientID %>").val('');
            $("#<%=txtPExpireDate.ClientID %>").val('');
            $("#<%=txtPIssueDate.ClientID %>").val('');
            $("#<%=txtVExpireDate.ClientID %>").val('');
            $("#<%=txtVisaNumber.ClientID %>").val('');
            $("#<%=txtVIssueDate.ClientID %>").val('');
            $("#<%=lblGstPreference.ClientID %>").text('');
            $("#GuestPreferenceDiv").hide();
            $("#txtGuestCountrySearch").val("");
            $("#hfSearchDetailsFireOrNot").val("0");
            $("#<%= hfGuestDeletedDoc.ClientID %>").val("");
            $("#GuestDocumentInfo").html('');
            var HiddenCompanyId = $("#<%=HiddenCompanyId.ClientID %>").val();
        }

        function CheckButtonPermission() {
            if ($("#ContentPlaceHolder1_hfIsUpdatePermission").val() == "0") {
                $("#btnAddDetailGuest").hide();
                $("#btnRoomSwap").hide();
                $("#btnAmendStay").hide();
                $("#btnStopCharge").hide();
                $("#btnStopChargeLink").hide();
                $("#btnSave").hide();
                $("#btnUpdateBlock").hide();

            }
            else {
                $("#btnAddDetailGuest").show();
                $("#btnRoomSwap").show();
                $("#btnAmendStay").show();
                $("#btnStopCharge").show();
                $("#btnStopChargeLink").show();
                $("#btnSave").show();
                $("#btnUpdateBlock").show();
            }
        }

        function UploadComplete() {
            var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var id = +$("#ContentPlaceHolder1_hiddenGuestId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();

            PageMethods.LoadDocument(id, randomId, deletedDoc, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }
        function OnLoadDocumentSucceeded(result) {

            var guestDoc = result;
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";

            guestDocumentTable += "<table id='DocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            guestDocumentTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }

                guestDocumentTable += "<td align='left' style='width: 50%; cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + guestDoc[row].Name + "</td>";

                if (guestDoc[row].Path != "") {
                    if (guestDoc[row].Extention == ".jpg" || guestDoc[row].Extention == ".png" || guestDoc[row].Extention == ".PNG" || guestDoc[row].Extention == ".JPG")
                        imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    else
                        imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                guestDocumentTable += "<td align='left' style='width: 30%'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + imagePath + "</td>";

                guestDocumentTable += "<td align='left' style='width: 20%'>";
                guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteGuestDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                guestDocumentTable += "</td>";
                guestDocumentTable += "</tr>";
            }
            guestDocumentTable += "</table>";

            // docc = guestDocumentTable;

            $("#DocumentInfo").html(guestDocumentTable);
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }

        function AttachFile() {

            var randomId = +$("#<%=hiddenGuestId.ClientID %>").val();
            var path = "/HotelManagement/Image/";
            var category = "Guest";
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


            //$("#documentsDiv").dialog({
            //    autoOpen: true,
            //    modal: true,
            //    width: 700,
            //    closeOnEscape: true,
            //    resizable: false,
            //    title: "Add Documents",
            //    show: 'slide'
            //});
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
        function CloseDialog() {
            $("#DocumentDialouge").dialog('close');
            return false;
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
    </script>
    <asp:HiddenField ID="hiddenGuestId" runat="server" />
    <asp:HiddenField ID="hfGuestDeletedDoc" runat="server" />
    <asp:HiddenField ID="hfDefaultCountryId" runat="server" />
    <asp:HiddenField ID="HiddenCompanyId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsSavePermission" runat="server" />
    <asp:HiddenField ID="hfIsUpdatePermission" runat="server" />
    <asp:HiddenField ID="hfIsDeletePermission" runat="server" />
    <asp:HiddenField ID="hfFromRoom" runat="server" />
    <asp:HiddenField ID="hfToRoom" runat="server" />
    <asp:HiddenField ID="hfToday" runat="server" />
    <asp:HiddenField ID="hfQueryStringRoomNumber" runat="server" />
    <%----Master room pop up----------------%>
    <div id="MasterRoomSelectPopUp" style="display: none">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-4">
                    <asp:Label ID="lbl" runat="server" class="control-label" Text="Selected Rooms"></asp:Label>
                </div>
                <div class="col-md-6">
                    <asp:DropDownList ID="ddlMasterRoom" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="form-horizontal" style="margin-top: 20px">
            <div class="form-group">
                <div class="col-md-offset-4 col-md-2">
                    <input id="btnOkay" type="button" value="Ok" class="btn btn-primary" onclick="SelectMasterRoom()" />
                </div>
                <div class="col-md-2">
                    <input id="btnBack" type="button" value="Back" class="btn btn-primary" onclick="MasterRoomSelectPopUpClose()" />
                </div>
            </div>
        </div>
    </div>
    <!--Guest Preference PopUp -->
    <div id="DivGuestPreference" style="display: none;">
        <div id="Div1">
            <div id="ltlGuestPreference">
            </div>
        </div>
    </div>
    <!--End Guest Preference PopUp -->
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Update Guest Info</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Guest Swap</a></li>
            <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-3">Amend Stay</a></li>
            <li id="D" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-4">Stop Posting</a></li>
            <li id="E" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-5">Linked Room</a></li>
            <li id="F" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab_6">Guest Block</a></li>
            <li id="G" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab_7">Bill Pending</a></li>
            <li id="H" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab_8">Room Bill Posting</a></li>
        </ul>
        <div id="tab-1">
            <div id="ChangeGuestInfoDiv" class="panel panel-default">
                <div id="GuestEntryPanel" class="panel panel-default">
                    <div class="panel-heading">
                        Guest Information
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
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
                                    From Date</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtSrcFromDate" runat="server" CssClass="form-control"
                                        TabIndex="5"></asp:TextBox>
                                </div>
                                <label for="ToDate" class="control-label col-md-2">
                                    To Date</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtSrcToDate" runat="server" CssClass="form-control"
                                        TabIndex="6"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="GuestName" class="control-label col-md-2">
                                    Guest Name</label>
                                <div class="col-md-9">
                                    <asp:TextBox ID="txtSrcGuestName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                </div>
                                <div class="col-md-1">
                                    <div style="float: right; margin-left: 150px">
                                        <img id="imgCollapse" width="40px" src="/HotelManagement/Image/expand_alt.png" />
                                    </div>
                                </div>
                            </div>
                            <div id="ExtraSearch" style="display: none;">
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
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtSrcPassportNumber" runat="server" CssClass="form-control" TabIndex="11"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <button type="button" id="btnSearch" class="btn btn-primary btn-sm" tabindex="12">
                                        Search</button>
                                    <button type="button" id="btnClear" tabindex="12" onclick="PerformClearAction()"
                                        class="btn btn-primary btn-sm">
                                        Clear</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="GuestSearchPanel" class="panel panel-default" style="display: none;">
                    <div class="panel-heading">
                        Search Information
                    </div>
                    <div class="panel-body">
                        <div id="ltlTableWiseItemInformation">
                        </div>
                    </div>
                </div>
                <div id="GuestInfoUpdateDiv" style="display: none;">
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-12">
                                    <div id="SearchPanel" class="panel panel-default">
                                        <div class="panel-heading">
                                            Update Guest Information
                                        </div>
                                        <div class="panel-body">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                    <label for="Title" class="control-label required-field col-md-2">
                                                        Title</label>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ddlTitle" runat="server" CssClass="form-control" TabIndex="2">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="FirstName" class="control-label col-md-2 required-field">
                                                        First Name</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtFirstName" CssClass="form-control" runat="server"></asp:TextBox>
                                                    </div>
                                                    <label for="LastName" class="control-label col-md-2">
                                                        Last Name</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtLastName" CssClass="form-control" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="FullName" class="control-label col-md-2 required-field">
                                                        Full Name</label>
                                                    <div class="col-md-10">
                                                        <asp:TextBox ID="txtGuestName" CssClass="form-control" runat="server" TabIndex="38"
                                                            ReadOnly></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="DateOfBirth" class="control-label col-md-2">
                                                        Date Of Birth</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtGuestDOB" runat="server" CssClass="form-control" TabIndex="39"></asp:TextBox>
                                                    </div>
                                                    <label for="Gender" class="control-label col-md-2 required-field">
                                                        Gender</label>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ddlGuestSex" runat="server" CssClass="form-control" TabIndex="40">
                                                            <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                                            <asp:ListItem>Male</asp:ListItem>
                                                            <asp:ListItem>Female</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="Company Name" class="control-label col-md-2">
                                                        Company Name</label>
                                                    <div class="col-md-10">
                                                        <asp:TextBox ID="txtGuestAddress1" runat="server" CssClass="form-control" TabIndex="41"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="Address" class="control-label col-md-2">
                                                        Address</label>
                                                    <div class="col-md-10">
                                                        <asp:TextBox ID="txtGuestAddress2" runat="server" CssClass="form-control" TabIndex="42"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="EmailAddress" class="control-label col-md-2">
                                                        Email Address</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtGuestEmail" runat="server" TabIndex="43" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <label for="Profession" class="control-label col-md-2">
                                                        Profession</label>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ddlProfessionId" runat="server" TabIndex="40" CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="PhoneNumber" class="control-label col-md-2">
                                                        Phone Number</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtGuestPhone" runat="server" CssClass="form-control" TabIndex="44"></asp:TextBox>
                                                    </div>
                                                    <label for="City" class="control-label col-md-2">
                                                        City</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtGuestCity" runat="server" CssClass="form-control" TabIndex="45"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="ZipCode" class="control-label col-md-2">
                                                        Zip Code</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtGuestZipCode" runat="server" CssClass="form-control" TabIndex="46"></asp:TextBox>
                                                    </div>
                                                    <label for="Country" class="control-label col-md-2 required-field">
                                                        Country</label>
                                                    <div class="col-md-4">
                                                        <input id="txtGuestCountrySearch" type="text" class="form-control" />
                                                        <div style="display: none;">
                                                            <asp:DropDownList ID="ddlGuestCountry" runat="server" CssClass="form-control" TabIndex="47">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="Nationality" class="control-label col-md-2">
                                                        Nationality</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtGuestNationality" runat="server" CssClass="form-control" TabIndex="48"></asp:TextBox>
                                                    </div>
                                                    <label for="DrivingLicense" class="control-label col-md-2">
                                                        Driving License</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtGuestDrivinlgLicense" runat="server" CssClass="form-control"
                                                            TabIndex="49"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="NationalID" class="control-label col-md-2">
                                                        National ID</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtNationalId" runat="server" CssClass="form-control" TabIndex="50"></asp:TextBox>
                                                    </div>
                                                    <label for="VisaNumber" class="control-label col-md-2">
                                                        Visa Number</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtVisaNumber" runat="server" CssClass="form-control" TabIndex="51"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="VisaIssueDate" class="control-label col-md-2">
                                                        Visa Issue Date</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtVIssueDate" runat="server" CssClass="form-control" TabIndex="52"></asp:TextBox>
                                                    </div>
                                                    <label for="VisaExpiryDate" class="control-label col-md-2">
                                                        Visa Expiry Date</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtVExpireDate" runat="server" CssClass="form-control" TabIndex="53"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="PassportNumber" class="control-label col-md-2">
                                                        Passport Number</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtPassportNumber" runat="server" CssClass="form-control" TabIndex="54"></asp:TextBox>
                                                    </div>
                                                    <label for="PassIssueDate" class="control-label col-md-2">
                                                        Pass. Issue Date</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtPIssueDate" runat="server" CssClass="form-control" TabIndex="56"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="PassExpiryDate" class="control-label col-md-2">
                                                        Pass. Expiry Date</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtPExpireDate" runat="server" CssClass="form-control" TabIndex="57"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div id="DocumentDialouge" style="display: none;">
                                                    <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
                                                        clientidmode="static" scrolling="yes"></iframe>
                                                </div>
                                                <div id="ShowDocumentDiv" style="display: none;">
                                                    <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
                                                        clientidmode="static" scrolling="yes"></iframe>
                                                </div>

                                                <div class="form-group">
                                                    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>
                                                    <asp:HiddenField ID="RandomProductId" Value="0" runat="server"></asp:HiddenField>
                                                    <asp:HiddenField ID="tempId" runat="server" Value="0"></asp:HiddenField>
                                                    <asp:HiddenField ID="HiddenField1" runat="server" Value=""></asp:HiddenField>
                                                    <div class="col-md-2">
                                                        <label class="control-label col-md-2">Attachment</label>
                                                    </div>
                                                    <div class="col-md-10">
                                                        <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Attach" onclick="AttachFile()" />
                                                    </div>
                                                </div>
                                                <div id="DocumentInfo">
                                                </div>
                                                <div class="form-group" id="GuestPreferenceDiv" style="display: none">
                                                    <label for="GuestPreferences" class="control-label col-md-2">
                                                        Guest Preferences</label>
                                                    <div class="col-md-10">
                                                        <asp:Label ID="lblGstPreference" runat="server" class="control-label"></asp:Label>
                                                    </div>
                                                </div>
                                                <div id="TemporaryHideDiv" style="display: none;">
                                                    <div class="form-group">
                                                        <div class="col-md-2">
                                                            <input type="button" tabindex="18" id="btnGuestReferences" value="Preferences" class="btn btn-primary btn-sm"
                                                                onclick="javascript: return LoadGuestPreference()" />
                                                        </div>
                                                    </div>
                                                    <div class="childDivSection">
                                                        <div id="GuestocumentsInformation" class="panel panel-default" style="height: 270px;">
                                                            <div class="panel-heading">
                                                                Guest Documents Information
                                                            </div>
                                                            <div class="panel-body">
                                                                <div class="form-horizontal">
                                                                    <div class="form-group">
                                                                        <label for="GuestDocument" class="control-label col-md-2">
                                                                            Guest Document</label>
                                                                        <div class="col-md-4">
                                                                            <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
                                                                                <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                                                                                    FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
                                                                            </asp:Panel>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div id="GuestDocumentInfo">
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <input id="btnAddDetailGuest" type="button" value="Update Guest Info" onclick="SaveGuestInformation()" class="btn btn-primary btn-sm" />
                                                        <input id="btnBackToSearchGuest" type="button" value="Back" onclick="PerformBackToSearchGuestAction()" class="btn btn-primary btn-sm" />
                                                        <asp:Label ID="lblHiddenId" runat="server" Text='' Visible="False"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="divSection">
                                                    <div style="float: right; padding-right: 30px">
                                                        <input id="btnNext2" type="button" tabindex="59" value="Next" class="btn btn-primary btn-sm"
                                                            style="display: none;" />
                                                    </div>
                                                    <div style="float: right; padding-right: 10px">
                                                        <input id="btnPrev1" type="button" value="Prev" tabindex="60" class="btn btn-primary btn-sm"
                                                            style="display: none;" />
                                                    </div>
                                                </div>
                                                <div id="ltlGuestDetailGrid" style="padding-top: 10px;">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="RoomSwapInfoDiv" class="panel panel-default">
                <div class="panel-heading">
                    Guest Swap Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-12">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon">
                                                    <label class="control-label required-field" style="padding-top: 0px;">Room Number</label></span></span>
                                                        <input type="text" id="txtRoomSwapRoomNumber" class="form-control" />
                                            </div>

                                        </div>
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon">
                                                    <label class="control-label required-field" style="padding-top: 0px;">To Room Number</label></span>
                                                <input type="text" id="txtRoomSwapAlterRoomNumber" class="form-control" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <input type="button" value="Search" class="TransactionalButton btn btn-primary" onclick="GetMultipleGuest()" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="" id="fromRoomPaxDiv" style="display: none">
                                            <div class="col-md-1" style="text-align: center;">
                                                <asp:Label ID="Label51" runat="server" class="control-label" Text="Guest"></asp:Label>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ddlPaxFrom" runat="server" TabIndex="40" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="" id="toRoomPaxDiv" style="display: none">
                                            <div class="col-md-1" style="text-align: center;">
                                                <asp:Label ID="Label52" runat="server" class="control-label" Text="Guest"></asp:Label>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ddlPaxTo" runat="server" TabIndex="40" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="form-horizontal" style="display: none;"">
                                    <div class="form-group">
                                        <div class="col-md-6" id="roomInfoRoomSwapContainer">
                                            <table class="table table-striped table-bordered table-condensed table-hover">
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label1" runat="server" Text="Room Number"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapRoomNumber" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label2" runat="server" Text="Room Type"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapRoomType" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="lblLArrivalDate" runat="server" Text="Arrival Date"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapDArrivalDate"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="lblLExpectedDepartureDate"
                                                            runat="server" Text="Expected Departure Date"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapDExpectedDepartureDate" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label
                                                            ID="lblLGuestName" runat="server" Text="Name"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapDGuestName" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="lblLCountryName" runat="server"
                                                            Text="Country Name"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapDCountryName"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="lblLGuestSex" runat="server"
                                                            Text="Gender"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapDGuestSex"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="lblLGuestDOB" runat="server" Text="Date of Birth"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapDGuestDOB" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="lblLGuestEmail"
                                                            runat="server" Text="Email"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapDGuestEmail"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label
                                                            ID="lblLGuestPhone" runat="server" Text="Phone Number"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapDGuestPhone" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="lblLGuestAddress2" runat="server"
                                                            Text="Address"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapDGuestAddress2"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="lblLGuestNationality" runat="server" Text="Nationality"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapDGuestNationality" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="col-md-6" id="roomChangeRoomSwapContainerTable">
                                            <table class="table table-striped table-bordered table-condensed table-hover">
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label3" runat="server" Text="Room Number"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapRoomNumberToChange" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label4" runat="server" Text="Room Type"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapRoomTypeToChange" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label5" runat="server" Text="Arrival Date"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapDArrivalDateToChange"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label6"
                                                            runat="server" Text="Expected Departure Date"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapDExpectedDepartureDateToChange" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label
                                                            ID="Label7" runat="server" Text="Name"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapDGuestNameToChange" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label8" runat="server"
                                                            Text="Country Name"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapDCountryNameToChange"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label9" runat="server"
                                                            Text="Gender"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapDGuestSexToChange"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label10" runat="server" Text="Date of Birth"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapDGuestDOBToChange" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label11"
                                                            runat="server" Text="Email"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapDGuestEmailToChange"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label
                                                            ID="Label12" runat="server" Text="Phone Number"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapDGuestPhoneToChange" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label13" runat="server"
                                                            Text="Address"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapDGuestAddress2ToChange"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label14" runat="server" Text="Nationality"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblRoomSwapDGuestNationalityToChange" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <asp:HiddenField ID="hfRoomSwapFromRegistrationId" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="hfRoomSwapToRegistrationId" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="hfRoomSwapToGuestId" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="hfRoomSwapFromGuestId" runat="server"></asp:HiddenField>
                                        
                                    </div>
                                    <div class="form-group">
                                        <input id="btnRoomSwap" type="button" value="Guest Swap" class="btn btn-primary" onclick="RoomSwapInfo()" />
                                    </div>
                                </div>
                                &nbsp
                                <div class="form-horizontal" id="multiGuestSwapDiv" style="display: none;">
                                    <div class="form-group">
                                        <div class="col-md-6" id="fromGuestSwapDiv" style=" float:left">
                                            <div id="ltlFromGuestSwap">
                                            </div>
                                        </div>
                                        <div class="col-md-6" id="toGuestSwapDiv" style=" float:right">
                                            <div id="ltlToGuestSwap">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class ="col-md-2">
                                            <input id="btnRoomSwapMulti" type="button" value="Guest Swap" class="btn btn-primary" onclick="RoomSwapInfoMulti()" />
                                        </div>
                                        
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-3">
            <div id="AmendStayInfoDiv" class="panel panel-default">
                <div class="panel-heading">
                    Amend Stay Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-12">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label for="Type" class="control-label col-md-2">
                                                 Room Type</label>
                                                <div class="col-md-3">
                                                    <asp:DropDownList ID="ddlTypeAmend" runat="server" CssClass="form-control" TabIndex="2">
                                                        <%--<asp:ListItem Value="0">--- Please Select ---</asp:ListItem>--%>
                                                        <asp:ListItem Value="1">Individual Room</asp:ListItem>
                                                        <asp:ListItem Value="2">Link Room</asp:ListItem>
                                    
                                                    </asp:DropDownList>
                                                </div>
                                        <div id="divAmendLinkRoomSelection" style="display:none">
                                                        <label for="Type" class="control-label required-field col-md-2">
                                                                Link Rooms</label>
                                                                <div class="col-md-3">
                                                            <asp:DropDownList ID="ddlLinkRoomAmend" runat="server" TabIndex="10" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                        <div id="inputAmendDiv">
                                            <div class="col-md-3">
                                            <div class="input-group">
                                                <span class="input-group-addon">
                                                    <label class="control-label required-field" style="padding-top: 0px;">Room Number</label></span></span>
                                                        <input type="text" id="txtAmendStayRoomNumber" class="form-control" />
                                            </div>
                                        </div>
                                        </div>
                                        
                                        <div class="col-md-2">
                                            <input type="button" value="Search" class="TransactionalButton btn btn-primary" onclick="SearchAmendStayRoomInfo()" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-horizontal">
                                    <div class="form-group roomIsRegistered" style="display: none;">
                                        <div class="col-md-6" id="roomInfoAmendStayContainer">
                                            <table class="table table-striped table-bordered table-condensed table-hover">
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label15" runat="server" Text="Room Number"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayRoomNumber" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label16" runat="server" Text="Room Type"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayRoomType" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label17" runat="server" Text="Arrival Date"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayDArrivalDate"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label18"
                                                            runat="server" Text="Expected Departure Date"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:TextBox ID="txtAmendStayDisplayCheckInDate" runat="server" CssClass="form-control" TabIndex="5"
                                                            Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtAmendStayDExpectedDepartureDate" CssClass="form-control" runat="server" TabIndex="6"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label
                                                            ID="Label19" runat="server" Text="Name"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayDGuestName" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label20" runat="server"
                                                            Text="Country Name"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayDCountryName"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label21" runat="server"
                                                            Text="Gender"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayDGuestSex"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label22" runat="server" Text="Date of Birth"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayDGuestDOB" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label23"
                                                            runat="server" Text="Email"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayDGuestEmail"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label
                                                            ID="Label24" runat="server" Text="Phone Number"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayDGuestPhone" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label25" runat="server"
                                                            Text="Address"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayDGuestAddress2"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label26" runat="server" Text="Nationality"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayDGuestNationality" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="col-md-6" id="roomChangeAmendStayContainerTable" style="display: none;">
                                            <table class="table table-striped table-bordered table-condensed table-hover">
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label27" runat="server" Text="Room Number"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayRoomNumberToChange" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label28" runat="server" Text="Room Type"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayRoomTypeToChange" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label29" runat="server" Text="Arrival Date"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayDArrivalDateToChange"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label30"
                                                            runat="server" Text="Expected Departure Date"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayDExpectedDepartureDateToChange" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label
                                                            ID="Label31" runat="server" Text="Name"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayDGuestNameToChange" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label32" runat="server"
                                                            Text="Country Name"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayDCountryNameToChange"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label33" runat="server"
                                                            Text="Gender"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayDGuestSexToChange"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label34" runat="server" Text="Date of Birth"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayDGuestDOBToChange" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label35"
                                                            runat="server" Text="Email"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayDGuestEmailToChange"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label
                                                            ID="Label36" runat="server" Text="Phone Number"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayDGuestPhoneToChange" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label37" runat="server"
                                                            Text="Address"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayDGuestAddress2ToChange"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label38" runat="server" Text="Nationality"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblAmendStayDGuestNationalityToChange" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <asp:HiddenField ID="hfAmendStayFromRegistrationId" runat="server"></asp:HiddenField>
                                        <input id="btnAmendStay" type="button" value="Update Stay" class="btn btn-primary" onclick="RoomAmendStayInfo()" />
                                    </div>
                                </div>                                
                                <div class="form-horizontal" id="roomLinkedAmendStay" style="display: none;">
                                    <div class="form-group">
                                        <div class="col-md-6" id="roomInfoLinkedRoomDivAmendStay">
                                            <div id="ltlLinkedRoomsAmendStay">
                                            </div>
                                        </div>
                                        <div class ="col-md-2">
                                            <asp:Label ID="Label53" runat="server"
                                                            Text="Expected Deperature"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtExpectedDepDateLinked" CssClass="form-control" runat="server" TabIndex="6"></asp:TextBox>
                                        </div>
                                    </div>
                                    <input id="btnLinkedAmendStay" type="button" value="Update Stay" class="btn btn-primary" onclick="RoomAmendStayInfoLink()" />
                                </div>
                                
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-4">
            <div id="StopChargePostingInfoDiv" class="panel panel-default">
                <div class="panel-heading">
                    Stop Charge Posting Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-12">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label for="Type" class="control-label col-md-2">
                                                 Room Type</label>
                                                <div class="col-md-3">
                                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" TabIndex="2">
                                                        <asp:ListItem Value="1">Individual Room</asp:ListItem>
                                                        <asp:ListItem Value="2">Link Room</asp:ListItem>                                    
                                                    </asp:DropDownList>
                                                </div>
                                                    <div id="divLinkRoomSelection" style="display:none">
                                                        <label for="Type" class="control-label required-field col-md-2">
                                                                Link Rooms</label>
                                                                <div class="col-md-3">
                                                            <asp:DropDownList ID="ddlLinkName" runat="server" TabIndex="10" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div id="inputDiv">
                                                        <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">
                                                                <label class="control-label required-field" style="padding-top: 0px;">Room Number</label></span></span>
                                                                    <input type="text" id="txtStopChargePostingRoomNumber" class="form-control" />
                                                        </div>
                                                    </div>
                                                    </div>                                                    
                                                    <div class="col-md-2">
                                                        <input type="button" value="Search" class="TransactionalButton btn btn-primary" onclick="SearchStopChargePostingRoomInfo()" />
                                                    </div>
                                            </div>                                    
                                </div>
                                <div class="form-horizontal">
                                    <div class="form-group roomIsRegistered" style="display: none;">                                        
                                        <div class="col-md-6" id="roomInfoStopChargePostingContainer">
                                            <table class="table table-striped table-bordered table-condensed table-hover">
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label39" runat="server" Text="Room Number"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblStopChargePostingRoomNumber" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label40" runat="server" Text="Room Type"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblStopChargePostingRoomType" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label41" runat="server" Text="Arrival Date"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblStopChargePostingDArrivalDate"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label42"
                                                            runat="server" Text="Expected Departure Date"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:TextBox ID="txtStopChargePostingDisplayCheckInDate" runat="server" CssClass="form-control" TabIndex="5"
                                                            Visible="false"></asp:TextBox>
                                                        <asp:Label ID="lblStopChargePostingDExpectedDepartureDate"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label
                                                            ID="Label43" runat="server" Text="Name"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblStopChargePostingDGuestName" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label44" runat="server"
                                                            Text="Country Name"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblStopChargePostingDCountryName"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label45" runat="server"
                                                            Text="Gender"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblStopChargePostingDGuestSex"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label46" runat="server" Text="Date of Birth"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblStopChargePostingDGuestDOB" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label47"
                                                            runat="server" Text="Email"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblStopChargePostingDGuestEmail"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label
                                                            ID="Label48" runat="server" Text="Phone Number"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblStopChargePostingDGuestPhone" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label49" runat="server"
                                                            Text="Address"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblStopChargePostingDGuestAddress2"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label50" runat="server" Text="Nationality"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="lblStopChargePostingDGuestNationality" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="col-md-6" id="roomChangeStopChargePostingContainerTable">
                                            <div id="ltlCalenderControl">
                                            </div>
                                        </div>
                                        <asp:HiddenField ID="hfStopChargePostingFromRegistrationId" runat="server"></asp:HiddenField>
                                        <input id="btnStopCharge" type="button" value="Update Stop Charge Posting" class="btn btn-primary" onclick="RoomStopChargePostingInfo()" />
                                    </div>                                    
                                </div>
                                <div class="form-horizontal">
                                    <div class="form-group" id="roomLinkedStopCharge" style="display: none;">
                                        <div class="col-md-6" id="roomInfoLinkedRoomDiv">
                                            <div id="ltlLinkedRooms">
                                            </div>
                                        </div>
                                        <div class="col-md-6" id="roomInfoLinkedRoomDivCostCeneterTable">
                                            <div id="ltlCostCeneterTable">
                                            </div>
                                        </div>
                                        <input id="btnStopChargeLink" type="button" value="Update Stop Charge Posting" class="btn btn-primary" onclick="RoomStopChargePostingInfoLink()" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-5">
            <div id="DivRoomLinkedInfo" class="">
                <asp:HiddenField ID="hfSelectedMasterRoomRegId" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hfSelectedMasterId" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hfSelectedMasterRoomRegNum" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hfSelectedMasterRoomGuest" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hfSelectedMasterRoomNumber" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hfSelectedMasterRoomPK" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hfTempRoomIdf" runat="server"></asp:HiddenField>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="panel panel-default">
                            <div class="panel-heading">Room Linked Information</div>
                            <div class="panel-body" id="pnlRoomSearch">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblLinkRoomNumber" runat="server" class="control-label required-field" Text="Link Room Number"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtRoomNumberSearch" runat="server" TabIndex="1" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <input id="btnSearchLinkRoom" type="button" value="Search" class="TransactionalButton btn btn-primary" onclick="SearchByRoomNumber()" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-body" id="">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblGroupName" runat="server" class="control-label" Text="Link / Group Name"></asp:Label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="txtGroupName" runat="server" TabIndex="4" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="panel panel-default" id="" style="padding-top: 20px;">
                                        <div class="form-group">
                                            <div class="col-md-2" style="padding-right: 20px; text-align: center">
                                                <asp:Label ID="lblRoomNumberLinked" runat="server" class="control-label required-field" Text="Room Number"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtRoomNumberLinked" runat="server" TabIndex="5" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <input id="btnAdd" type="button" value="Add" class="TransactionalButton btn btn-primary" onclick="AddByRoomNumber()" />                                                
                                            </div>
                                        </div>
                                        <div class="panel-body" id="pnlTable">
                                            <div class="form-group" style="overflow: scroll;">
                                                <table id='TblLinkedRoom' class="table table-bordered table-condensed table-responsive" style="width:100%" >
                                                    <colgroup>
                                                        <col style="width: 20%;" />
                                                        <col style="width: 20%;" />
                                                        <col style="width: 45%;" />
                                                        <col style="width: 15%;" />

                                                    </colgroup>
                                                    <thead>
                                                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                                            <td style="text-align: center;">Room
                                                            </td>
                                                            <td style="text-align: center;">Registration No.
                                                            </td>
                                                            <td style="text-align: center;">Guest Name
                                                            </td>
                                                            <td style="text-align: center;">Delete
                                                            </td>
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
                            <div class="form-horizontal">
                                <div class="panel-body">
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblMasterRoom" runat="server" class="control-label required-field" Text="Master Room"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtMasterRoom" disabled="disabled" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-1">
                                            <input id="btnPopUp" type="button" value="..." class="btn btn-primary" />
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Label ID="lblRegistrationNumber" runat="server" class="control-label required-field" Text="Registration Number"></asp:Label>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtRegistrationNumber" disabled="disabled" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblGuestName" runat="server" class="control-label" Text="Guest Name"></asp:Label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="txtGuestNametbl" disabled="disabled" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblDescription" runat="server" class="control-label" Text="Description"></asp:Label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TabIndex="7" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-4 col-md-offset-2">
                                            <input id="btnSave" type="button" value="Save" class="TransactionalButton btn btn-primary" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab_6">
            <div id="divGuestBlock">
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="panel panel-default">
                            <div class="panel-heading">Guest Search</div>
                            <div class="panel-body" id="pnlGuestSearch">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label for="GuestName" class="control-label col-md-2">
                                            Guest Name</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtGuestNameSrc" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                        </div>
                                        <label for="CompanyName" class="control-label col-md-2">
                                            Company Name</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtCompanyNameSrc" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="EmailAddress" class="control-label col-md-2">
                                            Email Address</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtEmailSrc" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                                        </div>
                                        <label for="MobileNumber" class="control-label col-md-2">
                                            Mobile Number</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtMobileNumberSrc" runat="server" CssClass="form-control" TabIndex="8"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="NationalID" class="control-label col-md-2">
                                            National ID</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtNIDsrc" runat="server" CssClass="form-control" TabIndex="9"></asp:TextBox>
                                        </div>
                                        <label for="DateOfBirth" class="control-label col-md-2">
                                            Date of Birth</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtDoBSrc" runat="server" CssClass="form-control"
                                                TabIndex="10"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="PassportNumber" class="control-label col-md-2">
                                            Passport Number</label>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="txtPassportsrc" runat="server" CssClass="form-control" TabIndex="11"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-12 col-md-offset-2">
                                            <button type="button" id="btnGuestSearch" class="TransactionalButton btn btn-primary btn-sm" tabindex="12">
                                                Search</button>
                                            <button type="button" id="btnGuestClear" tabindex="12"
                                                class="btn btn-primary btn-sm" onclick="ClearGuestBlock()">
                                                Clear</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:HiddenField ID="hfGuestId" runat="server" Value="0"></asp:HiddenField>
                        <div id="GuestSearchResult" class="panel panel-default">
                            <div class="panel-heading">Search Information</div>
                            <div class="panel-body">
                                <div class="form-group">
                                    <table id="tblGuestInfo" style="width: 100%;" class="table table-bordered table-condensed table-responsive">
                                        <thead>
                                            <tr style="color: White; background-color: #44545E; font-weight: bold; text-align: left;">
                                                <%--<th style="width: 6%; text-align: center;">
                                    Select<br />
                                    <input type="checkbox" id="chkAll" />
                                </th>--%>
                                                <th style="width: 15%;">Guest Name
                                                </th>
                                                <th style="width: 15%;">Company Name
                                                </th>
                                                <th style="width: 15%;">Email
                                                </th>
                                                <th style="width: 15%;">Phone
                                                </th>
                                                <th style="width: 15%;">NID
                                                </th>
                                                <th style="width: 10%;">DOB
                                                </th>
                                                <th style="width: 15%;">Block
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
                                <div id="ltlGuestInfo">
                                </div>
                            </div>
                        </div>
                        <div id="GuestBlockDiv" class="panel panel-default">
                            <%--<div class="panel-heading">Guest Information</div>--%>
                            <div class="form-horizontal">
                                <div class="panel-body">
                                    <div class="form-group">
                                        <div class=" col-md-3">
                                            <asp:Label runat="server" class="control-label" Text="Guest Name"></asp:Label>
                                        </div>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="BlGuestName" runat="server" CssClass="form-control" TabIndex="2" ReadOnly="true" TextMode="SingleLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class=" col-md-3">
                                            <asp:Label runat="server" class="control-label" Text="Guest Block"></asp:Label>
                                        </div>

                                        <div class="col-md-2">
                                            <input class="checkBlock" type="checkbox" value="1" id="chkYesBlock" />
                                            Yes
                                        </div>
                                        <%--<div class="col-md-1">
                                            <input class="checkBlock" type="checkbox" value="0" id="chkNotBlock" /> No
                                        </div>--%>
                                    </div>
                                    <div class="form-group">

                                        <div class=" col-md-3">
                                            <asp:Label runat="server" class="control-label required-field" Text="Description"></asp:Label>
                                        </div>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="txtGuestDescription" runat="server" CssClass="form-control" TabIndex="2" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-6 col-md-offset-3">
                                            <button type="button" id="btnUpdateBlock" class="btn btn-primary btn-sm" tabindex="3">
                                                Update</button>
                                        </div>
                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab_7">
            <div id="BillPendingInfoDiv" class="panel panel-default">
                <div class="panel-heading">
                    Bill Pending Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">                                        
                            <div>
                                <div class="col-md-3">
                                    <div class="input-group">
                                        <span class="input-group-addon">
                                            <label class="control-label required-field" style="padding-top: 0px;">Room Number</label>
                                        </span>
                                        <input type="text" id="txtBillPendingRoomNumber" class="form-control" />
                                    </div>
                                </div>
                            </div>                                                    
                            <div class="col-md-2">
                                <input type="button" value="Bill Pending Release" class="TransactionalButton btn btn-primary" onclick="BillPendingReleaseInfo()" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab_8">
            <div id="NoShowPostingInfoDiv" class="panel panel-default">
                <div class="panel-heading">
                    Room Bill Posting Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-12">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label for="Type" class="control-label col-md-2">
                                                Room Type</label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ddlNoShowPostingRoomType" runat="server" CssClass="form-control" TabIndex="2">
                                                    <asp:ListItem Value="1">Individual Room</asp:ListItem>
                                                    <%--<asp:ListItem Value="2">Link Room</asp:ListItem>--%>
                                                </asp:DropDownList>
                                            </div>
                                            <div id="divNoShowPostingInfoLinkRoomSelection" style="display:none">
                                                <label for="Type" class="control-label required-field col-md-2">
                                                        Link Rooms</label>
                                                <div class="col-md-3">
                                                    <asp:DropDownList ID="ddlLinkRoomNoShowPostingInfo" runat="server" TabIndex="10" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                                </div>
                                            <div id="inputNoShowPostingInfoDiv">
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <span class="input-group-addon">
                                                            <label class="control-label required-field" style="padding-top: 0px;">Room Number</label>
                                                        </span>
                                                        <input type="text" id="txtRoomBillPostingRoomNumber" class="form-control" />
                                                    </div>
                                                </div>
                                            </div> 
                                    </div>
                                    <div class="form-group">
                                        <label for="Type" class="control-label col-md-2">
                                                Posting Type</label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ddlRoomBillPostingProcessType" runat="server" CssClass="form-control" TabIndex="2">
                                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                                    <asp:ListItem Value="1">No Show Posting</asp:ListItem>
                                                    <asp:ListItem Value="2">Advance Room Bill Posting</asp:ListItem>                                    
                                                </asp:DropDownList>
                                            </div>
                                    </div>
                                    <div class="form-group">
                                        <input type="btnRoomBillPosting" value="Room Bill Posting" class="TransactionalButton btn btn-primary" onclick="RoomBillPostingProcessInfo()" />
                                    </div>
                                </div>
                                <div class="form-horizontal">
                                    <div class="form-group roomIsRegistered" style="display: none;">
                                        <div class="col-md-6" id="roomNoShowPostingInfoContainer">
                                            <table class="table table-striped table-bordered table-condensed table-hover">
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label54" runat="server" Text="Room Number"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label55" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label56" runat="server" Text="Room Type"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label57" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label58" runat="server" Text="Arrival Date"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label59"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label60"
                                                            runat="server" Text="Expected Departure Date"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" TabIndex="5"
                                                            Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="TextBox2" CssClass="form-control" runat="server" TabIndex="6"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label
                                                            ID="Label61" runat="server" Text="Name"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label62" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label63" runat="server"
                                                            Text="Country Name"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label64"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label65" runat="server"
                                                            Text="Gender"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label66"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label67" runat="server" Text="Date of Birth"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label68" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label69"
                                                            runat="server" Text="Email"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label70"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label
                                                            ID="Label71" runat="server" Text="Phone Number"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label72" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label73" runat="server"
                                                            Text="Address"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label74"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label75" runat="server" Text="Nationality"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label76" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="col-md-6" id="roomChangeNoShowPostingInfoContainerTable" style="display: none;">
                                            <table class="table table-striped table-bordered table-condensed table-hover">
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label77" runat="server" Text="Room Number"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label78" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label79" runat="server" Text="Room Type"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label80" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label81" runat="server" Text="Arrival Date"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label82"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label83"
                                                            runat="server" Text="Expected Departure Date"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label84" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label
                                                            ID="Label85" runat="server" Text="Name"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label86" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label87" runat="server"
                                                            Text="Country Name"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label88"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label89" runat="server"
                                                            Text="Gender"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label90"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label91" runat="server" Text="Date of Birth"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label92" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label93"
                                                            runat="server" Text="Email"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label94"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label
                                                            ID="Label95" runat="server" Text="Phone Number"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label96" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label97" runat="server"
                                                            Text="Address"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label98"
                                                            runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-md-2">
                                                        <asp:Label ID="Label99" runat="server" Text="Nationality"></asp:Label>
                                                    </td>
                                                    <td class="col-md-4">
                                                        <asp:Label ID="Label100" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <asp:HiddenField ID="HiddenField2" runat="server"></asp:HiddenField>
                                        <input id="btnNoShowPostingInfo" type="button" value="Update Stay" class="btn btn-primary" onclick="RoomAmendStayInfo()" />
                                    </div>
                                </div>                                
                                <div class="form-horizontal" id="roomLinkedNoShowPostingInfo" style="display: none;">
                                    <div class="form-group">
                                        <div class="col-md-6" id="roomInfoLinkedRoomDivNoShowPostingInfo">
                                            <div id="ltlLinkedRoomsNoShowPostingInfo">
                                            </div>
                                        </div>
                                        <div class ="col-md-2">
                                            <asp:Label ID="Label101" runat="server"
                                                            Text="Expected Deperature"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="TextBox3" CssClass="form-control" runat="server" TabIndex="6"></asp:TextBox>
                                        </div>
                                    </div>
                                    <input id="btnLinkedNoShowPostingInfo" type="button" value="Update Stay" class="btn btn-primary" onclick="RoomAmendStayInfoLink()" />
                                </div>
                                
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="RoomInstructionInfoDiv" class="panel panel-default" style="display: none;">
        <div class="panel-heading">
            Room Instruction Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-12">
                        Room Instruction Information
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="divClear">
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#ContentPlaceHolder1_hfQueryStringRoomNumber").val() != "") {
                $("#txtRoomSwapRoomNumber").val($("#ContentPlaceHolder1_hfQueryStringRoomNumber").val());
                $("#txtAmendStayRoomNumber").val($("#ContentPlaceHolder1_hfQueryStringRoomNumber").val());
                $("#txtStopChargePostingRoomNumber").val($("#ContentPlaceHolder1_hfQueryStringRoomNumber").val());
            }
        });
    </script>
</asp:Content>
