<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmGuestCheckIn.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmGuestCheckIn" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var MandaoryFieldsList = "";
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Pax In</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            if ($("#ContentPlaceHolder1_hfMandatoryFields").val() != "") {
                MandaoryFieldsList = JSON.parse($("#ContentPlaceHolder1_hfMandatoryFields").val());
            }

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_txtPaxInRate").keypress(function (e) {
                //if the letter is not digit then display error and don't type anything
                // “.” CHECK DOT, AND ONLY ONE.
                if ((e.which != 46 || $("#ContentPlaceHolder1_txtPaxInRate").val().indexOf('.') != -1) && (e.which < 48 || e.which > 57)) {
                    //display error message
                    toastr.warning("Numbers Only");
                    return false;
                }
            });

            $('#ContentPlaceHolder1_txtGuestDOB').datepicker({
                changeMonth: true,
                changeYear: true,
                yearRange: "-70:+30",
                dateFormat: innBoarDateFormat
            });
            $('#ContentPlaceHolder1_txtSrcDateOfBirth').datepicker({
                changeMonth: true,
                changeYear: true,
                yearRange: "-70:+30",
                dateFormat: innBoarDateFormat
            });
            $('#ContentPlaceHolder1_txtVIssueDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtVExpireDate').datepicker("option", "minDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtVExpireDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtVIssueDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtPIssueDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtPExpireDate').datepicker("option", "minDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtPExpireDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtPIssueDate').datepicker("option", "maxDate", selectedDate);
                }
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

            $("#PopSearchPanel").hide();
            $("#PopTabPanel").hide();
            $("#ExtraSearch").hide();

            $("#<%=ddlExtraBedCharge.ClientID %>").change(function () {
                if ($("#ContentPlaceHolder1_ddlExtraBedCharge").val() == "Yes") {
                    $("#PaxInRateDiv").show();
                }
                else {
                    $("#PaxInRateDiv").hide();
                }
            });

            $("#btnPopSearch").click(function () {
                $("#PopSearchPanel").show('slow');
                $("#PopTabPanel").hide('slow');
                LoadGridInformation();
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
            $("#btnAddPerson").click(function () {
                $("#PopEntryPanel").show();
                AddNewItem();
            });

            $("#btnSearchSuccess").click(function () {
                $("#<%=txtGuestName.ClientID %>").val($("#<%=hiddenGuestName.ClientID %>").val());
                $("#PopSearchPanel").hide();
                $("#PopTabPanel").hide();

                $("#btnSearchSuccess").val("1");
                LoadDataOnParentForm();
                $("#TouchKeypad").dialog("close");
            });
            $("#btnSearchCancel").click(function () {
                $("#<%=hiddenGuestName.ClientID %>").val('');
                $("#<%=hiddenGuestId.ClientID %>").text('');
                $("#PopSearchPanel").hide();
                $("#PopTabPanel").hide();
                $("#TouchKeypad").dialog("close");
            });

            $("#<%=ddlTitle.ClientID %>").change(function () {
                AutoGenderLoadInfo();
            });

            $("#txtGuestCountrySearch").blur(function () {
                var countryId = $("#<%=ddlGuestCountry.ClientID %>").val();
                PageMethods.GetNationality(countryId, OnCountrySucceeded, OnCountryFailed);
            });

            $("#ContentPlaceHolder1_ddlTitle").blur(function () {
                var title = $("#<%=ddlTitle.ClientID %>").val();
                var titleText = $("#<%=ddlTitle.ClientID %> option:selected").text();

                if (title == "N/A") {
                    titleText = "";
                }
                var firstName = $("#<%=txtFirstName.ClientID %>").val();
                var lastName = $("#<%=txtLastName.ClientID %>").val();
                if (title != "0") {
                    $("#<%=txtGuestName.ClientID %>").val(titleText + " " + firstName + " " + lastName);
                }
                else $("#<%=txtGuestName.ClientID %>").val(firstName + " " + lastName);
            });

            $("#ContentPlaceHolder1_ddlTitle").select({
                tags: "true",
                //placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_txtFirstName").blur(function () {
                var title = $("#<%=ddlTitle.ClientID %>").val();
                var titleText = $("#<%=ddlTitle.ClientID %> option:selected").text();
                if (title == "N/A") {
                    titleText = "";
                }
                var firstName = $("#<%=txtFirstName.ClientID %>").val();
                var lastName = $("#<%=txtLastName.ClientID %>").val();
                if (title != "0") {
                    $("#<%=txtGuestName.ClientID %>").val(titleText + " " + firstName + " " + lastName);
                }
                else $("#<%=txtGuestName.ClientID %>").val(firstName + " " + lastName);
            });

            $("#ContentPlaceHolder1_txtLastName").blur(function () {
                var title = $("#<%=ddlTitle.ClientID %>").val();
                var titleText = $("#<%=ddlTitle.ClientID %> option:selected").text();
                if (title == "N/A") {
                    titleText = "";
                }
                var firstName = $("#<%=txtFirstName.ClientID %>").val();
                var lastName = $("#<%=txtLastName.ClientID %>").val();
                if (title != "0") {
                    $("#<%=txtGuestName.ClientID %>").val(titleText + " " + firstName + " " + lastName);
                }
                else $("#<%=txtGuestName.ClientID %>").val(firstName + " " + lastName);
            });

            CommonHelper.AutoSearchClientDataSource("txtGuestCountrySearch", "ContentPlaceHolder1_ddlGuestCountry", "ContentPlaceHolder1_ddlGuestCountry");
        });

        function OnCountrySucceeded(result) {
            $("#ContentPlaceHolder1_txtGuestNationality").val(result);

            var country = $("#<%=ddlGuestCountry.ClientID %>").val();
            if (country == 19) {
                for (var key in MandaoryFieldsList) {
                    var id = MandaoryFieldsList[key].FieldId;
                    if (!($(id).length)) {
                        id = "#ContentPlaceHolder1_" + id;
                    }
                    var tr = $(id).parent().parent();
                    $(tr).find("label").addClass("required-field");
                    if (id == "#ContentPlaceHolder1_txtNationalId") {
                        $('#lblVisaNumber').removeClass("required-field");
                    }
                    else if (id == "#ContentPlaceHolder1_txtPassportNumber") {
                        $('#lblPIssuePlace').removeClass("required-field");
                    }

                }
            }
        }
        function OnCountryFailed() { }

        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }
        $(function () {
            $("#PopMyTabs").tabs();
        });

        function AutoGenderLoadInfo() {
            var titleSex = $('#<%=ddlTitle.ClientID%>').val();

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

        function AddNewItem() {
            //popup(1, 'TouchKeypad', '', 935, 500);
            $("#TouchKeypad").dialog({
                width: 935,
                height: 500,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Search Guest",
                show: 'slide'
            });
            return false;
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
            var RoomNumber = $("#<%=txtSrcCheckInRoomNumber.ClientID %>").val();

            PageMethods.SearchGuestAndLoadGridInformation(companyName, DateOfBirth, EmailAddress, FromDate, ToDate, GuestName, MobileNumber, NationalId, PassportNumber, RegistrationNumber, RoomNumber, OnLoadGridViewObjectSucceeded, OnLoadGridViewObjectFailed);
            return false;
        }
        function OnLoadGridViewObjectSucceeded(result) {
            $("#ltlTableSearchGuest").html(result);

            return false;
        }
        function OnLoadGridViewObjectFailed(error) {
            toastr.error(error);
        }

        function SelectGuestInformation(GuestId) {
            $("#PopSearchPanel").hide('slow');
            $("#PopTabPanel").show('slow');
            PageMethods.LoadDetailInformation(GuestId, OnLoadDetailObjectSucceeded, OnLoadDetailObjectFailed);
            LoadGuestImage(GuestId)
            LoadGuestHistory(GuestId)
            ShowUploadedDocument($("#ContentPlaceHolder1_RandomDocId").val());
            return false;
        }
        function OnLoadDetailObjectSucceeded(result) {

            if (result.GuestDOB) {
                var date1 = new Date(result.GuestDOB);
                $("#<%=lblDGuestDOB.ClientID %>").text(GetStringFromDateTime(result.GuestDOB));
            }
            if (result.PIssueDate) {
                var date2 = new Date(result.PIssueDate);
                $("#<%=lblDPIssueDate.ClientID %>").text(GetStringFromDateTime(result.PIssueDate));
            }
            if (result.PExpireDate) {
                var date3 = new Date(result.PExpireDate);
                $("#<%=lblDPExpireDate.ClientID %>").text(GetStringFromDateTime(result.PExpireDate));
            }
            if (result.VIssueDate) {
                var date4 = new Date(result.VIssueDate);
                $("#<%=lblDVIssueDate.ClientID %>").text(GetStringFromDateTime(result.VIssueDate));
            }
            if (result.VExpireDate) {
                var date5 = new Date(result.VExpireDate);
                $("#<%=lblDVExpireDate.ClientID %>").text(GetStringFromDateTime(result.VExpireDate));
            }

            $("#<%=hiddenGuestName.ClientID %>").val(result.GuestName);
            $("#<%=hiddenGuestId.ClientID %>").val(result.GuestId);
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
            $("#<%=lblDGuestAuthentication.ClientID %>").text(result.GuestAuthentication);
            $("#<%=lblDNationalId.ClientID %>").text(result.NationalId);
            $("#<%=lblDPassportNumber.ClientID %>").text(result.PassportNumber);

            $("#<%=lblDPIssuePlace.ClientID %>").text(result.PIssuePlace);

            $("#<%=lblDVisaNumber.ClientID %>").text(result.VisaNumber);

            $("#<%=lblDCountryName.ClientID %>").text(result.CountryName);

            return false;
        }
        function OnLoadDetailObjectFailed(error) {
            toastr.error(error);
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
            toastr.error(error);
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
            toastr.error(error);
        }

        function LoadDataOnParentForm() {
            var guestId = $("#<%=hiddenGuestId.ClientID %>").val();
            //popup(-1);
            $("#TouchKeypad").dialog("close");
            PageMethods.LoadDetailInformation(guestId, OnLoadParentFromDetailObjectSucceeded, OnLoadParentFromDetailObjectFailed);
            return false;
        }
        function OnLoadParentFromDetailObjectSucceeded(result) {

            $("#ContentPlaceHolder1_chkIsReturnedGuest").attr("checked", true);

            if ($("#hfSearchDetailsFireOrNot").val() != "0") {
                $("#hfSearchDetailsFireOrNot").val("1");
            }
            else {
                $("#hfSearchDetailsFireOrNot").val("0");
            }

            if (result.GuestDOB) {

                $("#<%=txtGuestDOB.ClientID %>").val(GetStringFromDateTime(result.GuestDOB));
            }
            if (result.PIssueDate) {
                $("#<%=txtPIssueDate.ClientID %>").val(GetStringFromDateTime(result.PIssueDate));
            }
            if (result.PExpireDate) {
                $("#<%=txtPExpireDate.ClientID %>").val(GetStringFromDateTime(result.PExpireDate));
            }
            if (result.VIssueDate) {
                $("#<%=txtVIssueDate.ClientID %>").val(GetStringFromDateTime(result.VIssueDate));
            }
            if (result.VExpireDate) {
                $("#<%=txtVExpireDate.ClientID %>").val(GetStringFromDateTime(result.VExpireDate));
            }
            $("#<%=ddlTitle.ClientID %>").val(result.Title);
            $("#<%=txtFirstName.ClientID %>").val(result.FirstName);
            $("#<%=txtLastName.ClientID %>").val(result.LastName);

            $("#<%=hiddenGuestName.ClientID %>").val(result.GuestName);
            $("#<%=hiddenGuestId.ClientID %>").val(result.GuestId);
            $("#<%=txtGuestName.ClientID %>").val(result.GuestName);
            $("#<%=ddlGuestSex.ClientID %>").val(result.GuestSex);
            $("#<%=txtGuestEmail.ClientID %>").val(result.GuestEmail);
            $("#<%=txtGuestPhone.ClientID %>").val(result.GuestPhone);
            $("#<%=txtCompanyName.ClientID %>").val(result.GuestAddress1);
            $("#<%=txtGuestAddress.ClientID %>").val(result.GuestAddress2);
            $("#<%=ddlProfessionId.ClientID %>").val(result.ProfessionId);
            $("#<%=txtGuestCity.ClientID %>").val(result.GuestCity);
            $("#<%=txtGuestZipCode.ClientID %>").val(result.GuestZipCode);
            $("#<%=txtGuestNationality.ClientID %>").val(result.GuestNationality);
            $("#<%=txtGuestDrivinlgLicense.ClientID %>").val(result.GuestDrivinlgLicense);

            $("#<%=txtNationalId.ClientID %>").val(result.NationalId);
            $("#<%=txtPassportNumber.ClientID %>").val(result.PassportNumber);

            $("#<%=txtPIssuePlace.ClientID %>").text(result.PIssuePlace);

            $("#<%=txtVisaNumber.ClientID %>").val(result.VisaNumber);

            $("#<%=ddlGuestCountry.ClientID %>").val(result.GuestCountryId);

            $("#txtGuestCountrySearch").val(result.CountryName);
            return false;
        }
        function OnLoadParentFromDetailObjectFailed(error) {
            $("#hfSearchDetailsFireOrNot").val("0");
            toastr.error(error);
        }
        function PerformValidationForSave() {
            var txtGuestName = $("#<%=txtGuestName.ClientID %>").val();
            var txtTitle = $("#<%=ddlTitle.ClientID %>").val();
            var txtFirstName = $("#<%=txtFirstName.ClientID %>").val();
            var txtPaxInRate = $("#<%=txtPaxInRate.ClientID %>").val();
            var ddlExtraBedCharge = $("#<%=ddlExtraBedCharge.ClientID %>").val();
            var isValid = true;

            var country = $("#<%=ddlGuestCountry.ClientID %>").val();
            if (country == 19) {


                var validationEmailAddress = _.findWhere(MandaoryFieldsList, { FieldId: "txtGuestEmail" });
                if (validationEmailAddress != null) {
                    var emailAddress = $("#ContentPlaceHolder1_txtGuestEmail").val();
                    if (emailAddress == "") {
                        toastr.warning("Please Enter a Email Address");
                        $("#ContentPlaceHolder1_txtGuestEmail").focus();
                        return false;
                    }
                }
                var txtGuestEmail = $("#ContentPlaceHolder1_txtGuestEmail").val();
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

                var validationPassportNumber = _.findWhere(MandaoryFieldsList, { FieldId: "txtPassportNumber" });
                if (validationPassportNumber != null) {
                    var PassportNumberValue = $("#ContentPlaceHolder1_txtPassportNumber").val();
                    if (PassportNumberValue == "") {
                        toastr.warning("Please Enter a Passport Number");
                        $("#ContentPlaceHolder1_txtPassportNumber").focus();
                        return false;
                    }
                }

                var validationGuestDrivinlgLicense = _.findWhere(MandaoryFieldsList, { FieldId: "txtGuestDrivinlgLicense" });
                if (validationGuestDrivinlgLicense != null) {
                    var GuestDrivinlgLicenseValue = $("#ContentPlaceHolder1_txtGuestDrivinlgLicense").val();
                    if (GuestDrivinlgLicenseValue == "") {
                        toastr.warning("Please Enter a Drivinlg License Number");
                        $("#ContentPlaceHolder1_txtGuestDrivinlgLicense").focus();
                        return false;
                    }
                }

                var validationNationalId = _.findWhere(MandaoryFieldsList, { FieldId: "txtNationalId" });
                if (validationNationalId != null) {
                    var NationalIdValue = $("#ContentPlaceHolder1_txtNationalId").val();
                    if (NationalIdValue == "") {
                        toastr.warning("Please Enter a National Id");
                        $("#ContentPlaceHolder1_txtNationalId").focus();
                        return false;
                    }
                }
            }

            if (txtTitle == "0") {
                toastr.warning('Please Select Title.');
                $("#ddlTitle").focus();
                isValid = false;
            }
            else if (txtFirstName == "") {
                toastr.warning('Please Provide First Name.');
                $("#txtFirstName").focus();
                isValid = false;
            }
            else if (ddlExtraBedCharge == "Yes") {
                if (txtPaxInRate == "") {
                    toastr.warning('Please Provide Pax In Rate.');
                    $("#<%=txtPaxInRate.ClientID %>").focus();
                    isValid = false;
                }
            }
            else {
                isValid = true;
            }
            return isValid;
        }
        //Document Related Functions 
        function LoadDocUploader() {

            var randomId = +$("#ContentPlaceHolder1_RandomDocId").val();
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

        }
        function CloseDialog() {
            $("#DocumentDialouge").dialog('close');
            return false;
        }
        function UploadComplete() {
            var randomId = $("#ContentPlaceHolder1_RandomDocId").val();
            ShowUploadedDocument(randomId);
        }
        function ShowUploadedDocument(randomId) {
            var id = $("#ContentPlaceHolder1_hfRegistrationId").val();
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
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />
    <asp:HiddenField ID="tempDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfParentDoc" runat="server"></asp:HiddenField>

    <asp:HiddenField ID="hfRegistrationId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfRegistrationNumber" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCheckInDate" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="RandomGuestId" runat="server" />
    <asp:HiddenField ID="hiddenGuestId" runat="server" />
    <asp:HiddenField ID="hfMandatoryFields" runat="server" />
    <div id="ShowDocumentDiv" style="display: none;">
        <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <label for="RoomNumber" class="control-label col-md-2 required-field">
                        Room Number</label>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtSrcRoomNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnSrcRoomNumber" runat="server" Text="Search" TabIndex="2" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClick="btnSrcRoomNumber_Click" />
                    </div>
                    <label for="RegistrationNumber" class="control-label col-md-2 required-field">
                        Registration Number</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRegistrationId" runat="server" CssClass="form-control" TabIndex="3"
                            Enabled="False">
                        </asp:DropDownList>
                    </div>
                </div>
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
                    <label for="Name" class="control-label col-md-2 required-field">
                        Full Name</label>
                    <div class="col-md-8">
                        <asp:TextBox ID="txtGuestName" CssClass="form-control" runat="server" TabIndex="38" ReadOnly></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <button type="button" id="btnAddPerson" class="btn btn-primary btn-sm">
                            Search Guest</button>
                    </div>
                </div>
                <div class="form-group">
                    <label for="GuestDOB" class="control-label col-md-2">
                        Date Of Birth</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtGuestDOB" runat="server" CssClass="form-control" TabIndex="39"></asp:TextBox>
                    </div>
                    <label for="GuestSex" class="control-label col-md-2 required-field">
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
                    <label for="CompanyName" class="control-label col-md-2">
                        Company Name</label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" TabIndex="41"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="GuestAddress" class="control-label col-md-2">
                        Address</label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtGuestAddress" runat="server" CssClass="form-control" TabIndex="42"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="GuestEmail" class="control-label col-md-2">
                        Email Address</label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtGuestEmail" runat="server" TabIndex="43" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="Profession" class="control-label col-md-2">
                        Profession</label>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlProfessionId" runat="server" TabIndex="40" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label for="GuestPhone" class="control-label col-md-2">
                        Phone Number</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtGuestPhone" runat="server" CssClass="form-control" TabIndex="44"></asp:TextBox>
                    </div>
                    <label for="GuestCity" class="control-label col-md-2">
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
                            <asp:DropDownList ID="ddlGuestCountry" runat="server" TabIndex="47">
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
                    <label for="DrivinlgLicense" class="control-label col-md-2">
                        Driving License</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtGuestDrivinlgLicense" runat="server" CssClass="form-control"
                            TabIndex="49"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="NationalId" class="control-label col-md-2">
                        National Id</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtNationalId" runat="server" CssClass="form-control" TabIndex="50"></asp:TextBox>
                    </div>
                    <label for="VisaNumber" class="control-label col-md-2" id="lblVisaNumber">
                        Visa Number</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtVisaNumber" runat="server" CssClass="form-control" TabIndex="51"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="VIssueDate" class="control-label col-md-2">
                        Visa Issue Date</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtVIssueDate" runat="server" CssClass="form-control" TabIndex="52"></asp:TextBox>
                    </div>
                    <label for="VExpireDate" class="control-label col-md-2">
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
                    <label for="PIssuePlace" class="control-label col-md-2" id="lblPIssuePlace">
                        Pass. Issue Place</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPIssuePlace" runat="server" CssClass="form-control" TabIndex="55"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="PIssueDate" class="control-label col-md-2">
                        Pass. Issue Date</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPIssueDate" runat="server" CssClass="form-control" TabIndex="56"></asp:TextBox>
                    </div>
                    <label for="PExpireDate" class="control-label col-md-2">
                        Pass. Expiry Date</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPExpireDate" runat="server" CssClass="form-control" TabIndex="57"></asp:TextBox>
                    </div>
                </div>
                <div id="GuestocumentsInformation" class="panel panel-default" style="height: 270px;">
                    <div class="panel-heading">
                        Guest Documents Information
                    </div>
                    <div class="panel-body">
                        <div class="col-md-2">
                            <label class="control-label">Attachment</label>
                        </div>
                        <div class="col-md-4">
                            <input id="btnImageUp" type="button" onclick="javascript: return LoadDocUploader();"
                                class="TransactionalButton btn btn-primary btn-sm" value="Assignment Doc..." />
                        </div>
                    </div>
                    <div id="DocumentInfo">
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label for="ExtraBedCharge" class="control-label col-md-2 required-field">
                    Extra Bed Charge</label>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlExtraBedCharge" runat="server" CssClass="form-control" TabIndex="40">
                        <asp:ListItem>Yes</asp:ListItem>
                        <asp:ListItem>No</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div id="PaxInRateDiv" class="form-group">
                <label for="PaxInRate" class="control-label col-md-2 required-field">
                    Pax In Rate</label>
                <div class="col-md-2">
                    <asp:TextBox ID="txtPaxInRate" runat="server" CssClass="form-control quantitydecimal" TabIndex="56"></asp:TextBox>
                </div>
                <asp:Label ID="lblCurrencyType" CssClass="control-label" runat="server" Text=""></asp:Label>
            </div>
            <div id="GuestDocumentInfo">
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                        TabIndex="99" OnClick="btnSave_Click" PostBackUrl="~/HotelManagement/frmRoomRegistration.aspx"
                        OnClientClick="javascript: return PerformValidationForSave();" />
                    <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="btn btn-primary btn-sm"
                        TabIndex="100" OnClick="btnCancel_Click" />
                </div>
            </div>
        </div>
    </div>
    </div>
    <!-- Pop Up Guest Search -->
    <div id="TouchKeypad" style="display: none;">
        <div id="PopMessageBox" class="alert alert-info" style="display: none;">
            <button type="button" class="close" data-dismiss="alert">
                ×</button>
            <asp:Label ID='lblPopMessageBox' Font-Bold="True" runat="server"></asp:Label>
        </div>
        <div id="PopEntryPanel" class="panel panel-default" style="width: 911px">
            <div class="panel-heading">
                Guest Information
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                            <asp:HiddenField ID="hiddenGuestName" runat="server" />
                            <asp:Label ID="lblSrcGuestName" runat="server" class="control-label" Text="Guest Name"></asp:Label>
                        </div>
                        <div class="col-md-9">
                            <asp:TextBox ID="txtSrcGuestName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <div style="float: right; margin-left: 150px">
                                <img id="imgCollapse" width="40px" src="/HotelManagement/Image/expand_alt.png" />
                            </div>
                        </div>
                    </div>
                    <div id="ExtraSearch">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label6" runat="server" class="control-label" Text="Company Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSrcCompanyName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Room No."></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcCheckInRoomNumber" runat="server" CssClass="form-control"
                                    TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSrcRegistrationNumber" runat="server" class="control-label" Text="Registration No."></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcRegistrationNumber" runat="server" CssClass="form-control"
                                    TabIndex="4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSrcFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcFromDate" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSrcToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcToDate" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSrcEmailAddress" runat="server" class="control-label" Text="Email Address"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcEmailAddress" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSrcMobileNumber" runat="server" class="control-label" Text="Mobile Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcMobileNumber" runat="server" CssClass="form-control" TabIndex="8"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSrcNationalId" runat="server" class="control-label" Text="National ID"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcNationalId" runat="server" CssClass="form-control" TabIndex="9"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSrcDateOfBirth" runat="server" class="control-label" Text="Date of Birth"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcDateOfBirth" runat="server" CssClass="form-control" TabIndex="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSrcPassportNumber" runat="server" class="control-label" Text="Passport Number"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSrcPassportNumber" runat="server" CssClass="form-control" TabIndex="11"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <button type="button" tabindex="12" id="btnPopSearch" class="btn btn-primary btn-sm">
                                Search</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="PopSearchPanel" class="block" style="width: 911px">
            <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Information
            </a>
            <div class="block-body collapse in">
                <div id="ltlTableSearchGuest">
                </div>
            </div>
        </div>
        <div id="PopTabPanel" style="width: 935px">
            <div id="PopMyTabs">
                <ul id="PoptabPage" class="ui-style">
                    <li id="PopA" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                        <a href="#Poptab-1">Guest Information</a></li>
                    <li id="PopB" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                        <a href="#Poptab-2">Guest Document</a></li>
                    <li id="PopC" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                        <a href="#Poptab-3">Guest History</a></li>
                </ul>
                <div id="Poptab-1">
                    <div id="GuestInfo" class="panel panel-default" style="font-weight: bold">
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
                                            <asp:Label ID="lblLGuestAuthentication" runat="server" Text="Authentication"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestAuthentication" runat="server"></asp:Label>
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
                                        <td class="span2"></td>
                                        <td class="span4"></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="Poptab-2">
                    <div id="imageDiv">
                    </div>
                </div>
                <div id="Poptab-3">
                    <div class="HMBodyContainer">
                        <div id="guestHistoryDiv">
                        </div>
                    </div>
                </div>
                <button type="button" id="btnSearchSuccess" class="btn btn-primary btn-sm">
                    OK</button>
                <button type="button" id="btnSearchCancel" class="btn btn-primary btn-sm">
                    Cancel</button>
                <button type="button" id="btnPrintDocument" class="btn btn-primary btn-sm" style="display: none;">
                    Print Preview</button>
            </div>
        </div>
        <div class='divClear'>
        </div>
    </div>
    <!-- End Pop Up Guest Search -->
    <script type="text/javascript">
        if ($("#ContentPlaceHolder1_ddlExtraBedCharge").val() == "Yes") {
            $("#PaxInRateDiv").show();
        }
        else {
            $("#PaxInRateDiv").hide();
        }
    </script>
</asp:Content>
