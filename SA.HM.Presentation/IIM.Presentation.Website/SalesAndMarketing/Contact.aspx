<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.Contact" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var flag = 0;
        var contactId = "";
        var contactDetailsId = "0";
        var deleteDbItem = [], editDbItem = [], newlyAddedItem = [];
        $(document).ready(function () {
            $('#ContentPlaceHolder1_txtDOB').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    //$('#ContentPlaceHolder1_txtServiceFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtDateAnniv').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    //$('#ContentPlaceHolder1_txtServiceFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_ddlPhoneTitle").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlEmailTitle").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlSocialMediaTitle").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlWebsiteTitle").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_txtWorkCountry").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/Company.aspx/LoadCountryForAutoSearch',
                        data: JSON.stringify({ searchString: request.term }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.CountryName,
                                    value: m.CountryName,
                                    CountryId: m.CountryId
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
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_txtWorkCountry").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfWorkCountryId").val(ui.item.CountryId);
                    $("#ContentPlaceHolder1_txtWorkState").val("");
                    $("#ContentPlaceHolder1_hfWorkStateId").val("0");
                    $("#ContentPlaceHolder1_txtWorkCity").val("");
                    $("#ContentPlaceHolder1_hfWorkCityId").val("0");
                    $("#ContentPlaceHolder1_txtWorkLocation").val("");
                    $("#ContentPlaceHolder1_hfWorkLocationId").val("0");
                }
            });
            $("#ContentPlaceHolder1_txtPersonalCountry").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/Company.aspx/LoadCountryForAutoSearch',
                        data: JSON.stringify({ searchString: request.term }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.CountryName,
                                    value: m.CountryName,
                                    CountryId: m.CountryId
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
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_txtPersonalCountry").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfPersonalCountryId").val(ui.item.CountryId);
                    $("#ContentPlaceHolder1_txtPersonalState").val("");
                    $("#ContentPlaceHolder1_hfPersonalStateId").val("0");
                    $("#ContentPlaceHolder1_txtPersonalCity").val("");
                    $("#ContentPlaceHolder1_hfPersonalCityId").val("0");
                    $("#ContentPlaceHolder1_txtPersonalLocation").val("");
                    $("#ContentPlaceHolder1_hfPersonalLocationId").val("0");
                }
            });

            $("#ContentPlaceHolder1_txtWorkState").autocomplete({
                source: function (request, response) {
                    var workCountry = $("#ContentPlaceHolder1_hfWorkCountryId").val();
                    if (workCountry == 0) {
                        toastr.warning("Please Select Work Country");
                        $("#ContentPlaceHolder1_hfWorkCountryId").focus();
                        return false;
                    }
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/Company.aspx/LoadStateForAutoSearchByCountry',
                        data: JSON.stringify({ searchString: request.term, CountryId: workCountry }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.StateName,
                                    value: m.StateName,
                                    Id: m.Id
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
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_txtWorkState").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfWorkStateId").val(ui.item.Id);
                    $("#ContentPlaceHolder1_txtWorkCity").val("");
                    $("#ContentPlaceHolder1_hfWorkCityId").val("0");
                    $("#ContentPlaceHolder1_txtPersonalLocation").val("");
                    $("#ContentPlaceHolder1_hfPersonalLocationId").val("0");
                }
            });

            //multiple contact details start 
            $("#ContentPlaceHolder1_txtPersonalState").autocomplete({
                source: function (request, response) {
                    var personalCountry = $("#ContentPlaceHolder1_hfPersonalCountryId").val();
                    if (personalCountry == 0) {
                        toastr.warning("Please Select Personal Country");
                        $("#ContentPlaceHolder1_hfPersonalCountryId").focus();
                        return false;
                    }
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/Company.aspx/LoadStateForAutoSearchByCountry',
                        data: JSON.stringify({ searchString: request.term, CountryId: personalCountry }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.StateName,
                                    value: m.StateName,
                                    Id: m.Id
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
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_txtPersonalState").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfPersonalStateId").val(ui.item.Id);
                    $("#ContentPlaceHolder1_txtPersonalCity").val("");
                    $("#ContentPlaceHolder1_hfPersonalCityId").val("0");
                    $("#ContentPlaceHolder1_txtPersonalLocation").val("");
                    $("#ContentPlaceHolder1_hfPersonalLocationId").val("0");
                }
            });

            $("#ContentPlaceHolder1_txtWorkCity").autocomplete({
                source: function (request, response) {
                    var countryId = $("#ContentPlaceHolder1_hfWorkCountryId").val();
                    var stateId = $("#ContentPlaceHolder1_hfWorkStateId").val();
                    var stateString = $("#ContentPlaceHolder1_txtWorkState").val();

                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/Company.aspx/LoadCityForAutoSearchByState',
                        data: JSON.stringify({ searchString: request.term, CountryId: countryId, StateString: stateString, StateId: stateId }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.CityName,
                                    value: m.CityName,
                                    Id: m.CityId
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
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_txtWorkCity").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfWorkCityId").val(ui.item.Id);
                    $("#ContentPlaceHolder1_txtWorkLocation").val("");
                    $("#ContentPlaceHolder1_hfWorkLocationId").val("0");
                }
            });

            //multiple contact details start 
            $("#ContentPlaceHolder1_txtPersonalCity").autocomplete({
                source: function (request, response) {
                    var countryId = $("#ContentPlaceHolder1_hfPersonalCountryId").val();
                    var stateId = $("#ContentPlaceHolder1_hfPersonalStateId").val();
                    var stateString = $("#ContentPlaceHolder1_txtPersonalState").val();

                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/Company.aspx/LoadCityForAutoSearchByState',
                        data: JSON.stringify({ searchString: request.term, CountryId: countryId, StateString: stateString, StateId: stateId }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.CityName,
                                    value: m.CityName,
                                    Id: m.CityId
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
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_txtPersonalCity").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfPersonalCityId").val(ui.item.Id);
                    $("#ContentPlaceHolder1_txtPersonalLocation").val("");
                    $("#ContentPlaceHolder1_hfPersonalLocationId").val("0");
                }
            });

            //multiple contact details start 
            $("#ContentPlaceHolder1_txtWorkLocation").autocomplete({
                source: function (request, response) {
                    var countryId = $("#ContentPlaceHolder1_hfWorkCountryId").val();
                    var stateId = $("#ContentPlaceHolder1_hfWorkStateId").val();
                    var stateString = $("#ContentPlaceHolder1_txtWorkState").val();

                    var cityId = $("#ContentPlaceHolder1_hfWorkCityId").val();
                    var cityString = $("#ContentPlaceHolder1_txtWorkCity").val();

                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/Company.aspx/LoadLocationForAutoSearchByCity',
                        data: JSON.stringify({ searchString: request.term, CountryId: countryId, StateId: stateId, StateString: stateString, CityId: cityId, CityString: cityString }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.LocationName,
                                    value: m.LocationName,
                                    Id: m.LocationId
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
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_txtWorkLocation").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfWorkLocationId").val(ui.item.Id);
                }
            });

            //multiple contact details start 
            $("#ContentPlaceHolder1_txtPersonalLocation").autocomplete({
                source: function (request, response) {
                    var countryId = $("#ContentPlaceHolder1_hfPersonalCountryId").val();
                    var stateId = $("#ContentPlaceHolder1_hfPersonalStateId").val();
                    var stateString = $("#ContentPlaceHolder1_txtPersonalState").val();
                    var cityId = $("#ContentPlaceHolder1_hfPersonalCityId").val();
                    var cityString = $("#ContentPlaceHolder1_txtPersonalCity").val();

                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/Company.aspx/LoadLocationForAutoSearchByCity',
                        data: JSON.stringify({ searchString: request.term, CountryId: countryId, StateId: stateId, StateString: stateString, CityId: cityId, CityString: cityString }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.LocationName,
                                    value: m.LocationName,
                                    Id: m.LocationId
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
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_txtPersonalLocation").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfPersonalLocationId").val(ui.item.Id);
                }
            });

           <%-- $("#ContentPlaceHolder1_ddlCompanyId").change(function () {
                var companyId = $("#ContentPlaceHolder1_ddlCompanyId").val();
                if (companyId != 0) {
                    LoadCompanyLifeCycleStage(companyId);
                }
                else {
                    $("#<%=ddlLifeCycleStageId.ClientID %>").val("0");

                }

            });--%>

            var editId = $.trim(CommonHelper.GetParameterByName("editId"));
            var companyId = $.trim(CommonHelper.GetParameterByName("cid"));

            if (editId != "") {
                FillContactForm(editId);
            }
            if (companyId != "") {
                $("#ContentPlaceHolder1_hfPopUpCompany").val(companyId);
                CreateContactForCompany(companyId);
            }

            $("#ContentPlaceHolder1_ddlContactOwnerId").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            //$("#ContentPlaceHolder1_ddlSourceId").select2({
            //    tags: "true",
            //    //placeholder: "Select an option",
            //    allowClear: true,
            //    width: "99.75%"
            //});
            $("#btnAddNewSource").click(function () {
                CreateNewSource();
            });

            $("#btnAddNewCompany").click(function () {
                CreateNewCompany();
            });

            contactId = $("#<%=hfId.ClientID %>").val();
            contactDetailsId = $("#<%=hfcontactDetailsId.ClientID %>").val();
            $("#btnAddContact").click(function () {
                //var contactNumber = 0;
                var contactNumber = $("#<%=txtContactNumber.ClientID %>").val();
                //var contactLabel = $("#<%=txtContactLabel.ClientID %>").val();

                var phoneTitle = $("#<%=ddlPhoneTitle.ClientID %>").val();
                var contactLabel = $("#<%=ddlPhoneTitle.ClientID %> option:selected").text();

                var tableId = "ContactNumberGrid";
                if (contactNumber != "" && contactLabel != "") {
                    if (IsValidPhone(contactNumber) == false) {
                        toastr.warning("Invalid Phone Number");
                        $("#<%=txtContactNumber.ClientID %>").focus();
                        return false;
                    }

                    if (phoneTitle == "0") {
                        toastr.warning("Please Select Contact Number Title");
                        $("#<%=ddlPhoneTitle.ClientID %>").focus();
                        return false;
                    }

                    LoadContactTable(contactNumber, contactLabel, tableId, "txtContactNumber", "txtContactLabel", contactId, contactDetailsId);
                }
                else if (contactNumber == "") {
                    toastr.warning("Please add a number");
                    $("#<%=txtContactNumber.ClientID %>").focus();
                    return false;
                }
                else if (phoneTitle == "0") {
                    toastr.warning("Please Select Contact Number Title");
                    $("#<%=ddlPhoneTitle.ClientID %>").focus();
                        return false;
                    }
                <%--else {
                    toastr.warning("Please add a label");
                    $("#<%=txtContactLabel.ClientID %>").focus();
                    return false;
                }--%>
            });
            $("#btnAddEmail").click(function () {
                var email = $("#<%=txtEmailId.ClientID %>").val();
                //var emailLabel = $("#<%=txtEmailLabel.ClientID %>").val();

                var emailTitle = $("#<%=ddlEmailTitle.ClientID %>").val();
                var emailLabel = $("#<%=ddlEmailTitle.ClientID %> option:selected").text();

                var tableId = "EmailGrid";
                if (email != "" && emailLabel != "") {
                    if (CommonHelper.IsValidEmail(email) == false) {
                        toastr.warning("Invalid Email");
                        $("#<%=txtEmailId.ClientID %>").focus();
                        return false;
                    }

                    if (emailTitle == "0") {
                        toastr.warning("Please Select Email Title");
                        $("#<%=ddlPhoneTitle.ClientID %>").focus();
                        return false;
                    }

                    LoadContactTable(email, emailLabel, tableId, "txtEmailId", "txtEmailLabel", contactId, contactDetailsId);
                }
                else if (email == "") {
                    toastr.warning("Please add a email");
                    $("#<%=txtEmailId.ClientID %>").focus();
                    return false;
                }
                else if (emailTitle == "0") {
                    toastr.warning("Please Select Email Title");
                    $("#<%=ddlPhoneTitle.ClientID %>").focus();
                        return false;
                    }

                <%--else {
                    toastr.warning("Please add a label");
                    $("#<%=txtEmailLabel.ClientID %>").focus();
                    return false;
                }--%>

            });
            $("#btnAddSocialMedia").click(function () {
                var content = $("#<%=txtSocialMedia.ClientID %>").val();
                //var label = $("#<%=txtSocialMediaLabel.ClientID %>").val();

                var faxTitle = $("#<%=ddlSocialMediaTitle.ClientID %>").val();
                var label = $("#<%=ddlSocialMediaTitle.ClientID %> option:selected").text();

                var tableId = "SocialMediaGrid";
                if (content != "" && label != "") {
                    if (faxTitle == "0") {
                        toastr.warning("Please Select Social Media Title");
                        $("#<%=ddlPhoneTitle.ClientID %>").focus();
                        return false;
                    }
                    LoadContactTable(content, label, tableId, "txtSocialMedia", "txtSocialMediaLabel", contactId, contactDetailsId);
                }
                else if (content == "") {
                    toastr.warning("Please add a social media link");
                    $("#<%=txtSocialMedia.ClientID %>").focus();
                    return false;
                }
                else if (faxTitle == "0") {
                    toastr.warning("Please Select Social Media Title");
                    $("#<%=ddlPhoneTitle.ClientID %>").focus();
                        return false;
                    }
                <%--else {
                    toastr.warning("Please add a label");
                    $("#<%=txtSocialMediaLabel.ClientID %>").focus();
                    return false;
                }--%>
            });

            $("#btnWebsite").click(function () {
                var content = $("#<%=txtWebsite.ClientID %>").val();
                //var label = $("#<%=txtWebsiteLabel.ClientID %>").val();

                var websiteTitle = $("#<%=ddlWebsiteTitle.ClientID %>").val();
                var label = $("#<%=ddlWebsiteTitle.ClientID %> option:selected").text();

                var tableId = "WebsiteGrid";
                if (content != "" && label != "") {
                    if (websiteTitle == "0") {
                        toastr.warning("Please Select Website Title");
                        $("#<%=ddlPhoneTitle.ClientID %>").focus();
                    return false;
                }
                LoadContactTable(content, label, tableId, "txtWebsite", "txtWebsiteLabel", contactId, contactDetailsId);
            }
            else if (content == "") {
                toastr.warning("Please add a website link");
                $("#<%=txtWebsite.ClientID %>").focus();
                    return false;
                }
                else if (websiteTitle == "0") {
                    toastr.warning("Please Select Website Title");
                    $("#<%=ddlPhoneTitle.ClientID %>").focus();
                    return false;
                }

                <%--else {
                    toastr.warning("Please add a label");
                    $("#<%=txtWebsiteLabel.ClientID %>").focus();
                    return false;
                }--%>

            });


            $('#ContentPlaceHolder1_txtContactNumber').on('input', function (e) {
                var contactNumber = $("#<%=txtContactNumber.ClientID %>").val();
                var id = document.getElementById('<%=txtContactLabel.ClientID %>');
                LabelShowHide(id, contactNumber);
            });
            $('#ContentPlaceHolder1_txtEmailId').on('input', function (e) {
                var email = $("#<%=txtEmailId.ClientID %>").val();
                var id = document.getElementById('<%=txtEmailLabel.ClientID %>');
                LabelShowHide(id, email);
            });
            $('#ContentPlaceHolder1_txtWebsite').on('input', function (e) {
                var content = $("#<%=txtWebsite.ClientID %>").val();
                var id = document.getElementById('<%=txtWebsiteLabel.ClientID %>');
                LabelShowHide(id, content);
            });
            $('#ContentPlaceHolder1_txtSocialMedia').on('input', function (e) {
                var content = $("#<%=txtSocialMedia.ClientID %>").val();
                var id = document.getElementById('<%=txtSocialMediaLabel.ClientID %>');
                LabelShowHide(id, content);
            });

            <%--$("#ContentPlaceHolder1_txtSourceName").blur(function () {
                var sourceId = $("#<%=hfSourceId.ClientID %>").val();
                var sourceName = $("#<%=txtSourceName.ClientID %>").val();
                if (sourceId == "0" && sourceName != "") {
                    toastr.warning("Not A Valid Source. Please Add This Source");
                    return false;
                }
            });--%>

            $("#ContentPlaceHolder1_txtContactLabel").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/Contact.aspx/LoadLabelByAutoSearch',
                        //data: "{'searchTerm':'" + request.term + "'}",
                        data: JSON.stringify({ searchTerm: request.term, Type: "Number" }),
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Title,
                                    value: m.Title
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
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_txtContactLabel").val(ui.item.value);
                }
            });

            $("#ContentPlaceHolder1_txtEmailLabel").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/Contact.aspx/LoadLabelByAutoSearch',
                        data: JSON.stringify({ searchTerm: request.term, Type: "Email" }),
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Title,
                                    value: m.Title
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
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_txtEmailLabel").val(ui.item.value);
                }
            });
            $("#ContentPlaceHolder1_txtSocialMediaLabel").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/Contact.aspx/LoadLabelByAutoSearch',
                        data: JSON.stringify({ searchTerm: request.term, Type: "SocialMedia" }),
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Title,
                                    value: m.Title
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
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_txtSocialMediaLabel").val(ui.item.value);
                }
            });
            $("#ContentPlaceHolder1_txtWebsiteLabel").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/Contact.aspx/LoadLabelByAutoSearch',
                        data: JSON.stringify({ searchTerm: request.term, Type: "Website" }),
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Title,
                                    value: m.Title
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
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_txtWebsiteLabel").val(ui.item.value);
                }
            });

            $("#ContentPlaceHolder1_txtSourceName").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/SourceInformation.aspx/LoadSourceByAutoSearch',
                        data: "{'searchTerm':'" + request.term + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.SourceName,
                                    value: m.Id
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
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfSourceId").val(ui.item.value);
                }
            });

            $("#ContentPlaceHolder1_txtCompanyName").autocomplete({

                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/Contact.aspx/GetGuestCompanyInfoByCompanyName',
                        data: "{'companyName':'" + request.term + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    lifeCycleStageId: m.LifeCycleStageId,
                                    label: m.CompanyName,
                                    value: m.CompanyId
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
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfCompanyId").val(ui.item.value);
                    //$("#ContentPlaceHolder1_txtCompanyName").val(ui.item.label);
                    $("#ContentPlaceHolder1_ddlLifeCycleStageId").val(ui.item.lifeCycleStageId);
                }
            });

            //$("#ContentPlaceHolder1_ddlCompanyId").select2({
            //    tags: "true",
            //    //placeholder: "Select an option",
            //    allowClear: true,
            //    width: "99.75%"
            //});
        });
        function LabelAutoSearch(labelId) {
            $("#ContentPlaceHolder1_" + labelId).autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/Contact.aspx/LoadLabelByAutoSearch',
                        data: "{'searchTerm':'" + request.term + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.FieldValue,
                                    value: m.FieldValue
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
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_" + labelId).val(ui.item.value);
                }
            });
        }
        function IsValidPhone(inputtxt) {
            var regex = /^[+]*[(]{0,1}[0-9]{1,3}[)]{0,1}[-\s\./0-9]*$/;
            if (!regex.test(inputtxt)) {
                return false;
            } else {
                return true;
            }
        }
        function LabelShowHide(id, content) {
            if (content.length > 0) {
                //$("#divContactLabel").show();
                id.style.display = "block";
                //document.getElementById('txtContactLabel').style.visibility= true;
            }
            else {
                //$("#divContactLabel").hide();
                id.style.display = "none";
            }
        }
        function GridShowHide(tableId) {
            var length = $("#" + tableId + " tbody tr").length;
            if (length > 0) {
                $("#" + tableId).show();
            }
            else {
                $("#" + tableId).hide();
            }
        }
        function LoadContactTable(content, label, tableId, contentId, labelId, parentId, contactDetailsId) {

            var rowLength = $("#" + tableId + " tbody tr").length;

            var tr = "";

            if (rowLength % 2 == 0) {
                tr += "<tr style='background-color:#FFFFFF;'>";
            }
            else {
                tr += "<tr style='background-color:#E3EAEB;'>";
            }
            tr += "<td style='width:30%;'>" + label + "</td>"; //0
            tr += "<td style='width:60%;'>" + content + "</td>"; //1

            tr += "<td style='width:10%;'>";
            tr += "<a href='#' onclick= 'DeleteContactItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";

            tr += "<td style='display:none'>" + parentId + "</td>";    //3
            tr += "<td style='display:none'>" + contactDetailsId + "</td>"; //4

            tr += "</tr>";
            $("#" + tableId + " tbody").append(tr);
            //var tableId = "ContactNumberGrid";
            AfterContactAdd(tableId, contentId, labelId);

        }
        function AfterContactAdd(tableId, contentId, labelId) {
            if (contentId != "" && labelId != "") {
                $("#ContentPlaceHolder1_" + contentId).val("");
                $("#ContentPlaceHolder1_" + labelId).val("");

                var id = document.getElementById('ContentPlaceHolder1_' + labelId);
                var value = $("#ContentPlaceHolder1_" + contentId).val();
                LabelShowHide(id, value);
            }
            GridShowHide(tableId);
            return false;
        }
        function DeleteContactItem(deleteItem) {
            if (!confirm("Do you want to delete?")) {
                return;
            }
            var tr = $(deleteItem).parent().parent();
            var tableId = deleteItem.closest('table').id;

            contactDetailsId = $(tr).find("td:eq(4)").text();
            contactId = $("#ContentPlaceHolder1_hfId").val();

            if ((contactDetailsId != "0")) {
                deleteDbItem.push({
                    DetailsId: contactDetailsId,
                    ParentId: contactId
                });
            }

            $(deleteItem).parent().parent().remove();
            contactDetailsId = "0";
            GridShowHide(tableId);

            return false;

        }
        //function LoadEmailTable(content, label) {
        //    var rowLength = $("#EmailGrid tbody tr").length;

        //    var tr = "";

        //    if (rowLength % 2 == 0) {
        //        tr += "<tr style='background-color:#FFFFFF;'>";
        //    }
        //    else {
        //        tr += "<tr style='background-color:#E3EAEB;'>";
        //    }

        //    tr += "<td style='width:30%;'>" + label + "</td>";
        //    tr += "<td style='width:60%;'>" + content + "</td>";

        //    tr += "<td style='width:10%;'>";
        //    tr += "<a href='#' onclick= 'DeleteEmailItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";

        //    tr += "</tr>";
        //    $("#EmailGrid tbody").append(tr);
        //    AfterEmailAdd();

        //}
        <%--function AfterEmailAdd() {
            $("#<%=txtEmailId.ClientID %>").val("");
            $("#<%=txtEmailLabel.ClientID %>").val("");
            $("#divEmailLabel").hide();
            var length = $("#EmailGrid tbody tr").length;
            if (length > 0) {
                $("#EmailGrid").show();
            }
            else {
                $("#EmailGrid").hide();
            }
            return false;
        }--%>
        //function DeleteEmailItem(deleteItem) {
        //    if (!confirm("Do you want to delete?")) {
        //        return;
        //    }
        //    var tr = $(deleteItem).parent().parent();

        //    $(deleteItem).parent().parent().remove();
        //    var length = $("#EmailGrid tbody tr").length;
        //    if (length > 0) {
        //        $("#EmailGrid").show();
        //    }
        //    else {
        //        $("#EmailGrid").hide();
        //    }
        //}
        function CreateContactForCompany(companyId) {
            $("#companyDiv").show();
            $("#emailWork").show();
            $("#ContentPlaceHolder1_hfPopUpCompany").val(companyId);
            $("#ContentPlaceHolder1_chkIsSaveUnderCompany").attr('checked', true).prop('disabled', true);
            LoadCompanyLifeCycleStage(companyId);
            return false;
        }
        function LoadCompanyLifeCycleStage(companyId) {
            PageMethods.GetCompanyLifeCyleStage(companyId, OnLoadGetCompanyLifeCyleStageSucceed, OnLoadGetCompanyLifeCyleStageFailed);
            return false;
        }

        function OnLoadGetCompanyLifeCyleStageSucceed(result) {
            if (result != null) {
                $("#<%=hfCompanyId.ClientID %>").val(result.CompanyId);
                $("#<%=txtCompanyName.ClientID %>").val(result.CompanyName);
                $("#<%=ddlLifeCycleStageId.ClientID %>").val(result.LifeCycleStageId);
                $("#<%=ddlLifeCycleStageId.ClientID %>").prop('disabled', true);

                $("#<%=txtWorkAddress.ClientID %>").val(result.BillingAddress);
                $("#<%=hfWorkCountryId.ClientID %>").val(result.BillingCountryId);
                $("#<%=txtWorkCountry.ClientID %>").val(result.BillingCountry);
                $("#<%=hfWorkStateId.ClientID %>").val(result.BillingStateId);
                $("#<%=txtWorkState.ClientID %>").val(result.BillingState);
                $("#<%=hfWorkCityId.ClientID %>").val(result.BillingCityId);
                $("#<%=txtWorkCity.ClientID %>").val(result.BillingCity);           
                $("#<%=hfWorkLocationId.ClientID %>").val(result.BillingLocationId);
                $("#<%=txtWorkLocation.ClientID %>").val(result.BillingStreet);
                $("#<%=txtWorkPostCode.ClientID %>").val(result.BillingPostCode);
            }
            return false;
        }

        function OnLoadGetCompanyLifeCyleStageFailed(error) {

        }

        function CheckSaveUnderCompany() {
            if ($("#ContentPlaceHolder1_chkIsSaveUnderCompany").is(":checked")) {
                $("#companyDiv").show("slow");
                $("#emailWork").show("slow");
                $("#<%=ddlLifeCycleStageId.ClientID %>").prop('disabled', true);

            }
            else {
                $("#companyDiv").hide("slow");
                $("#emailWork").hide("slow");
                $("#<%=ddlLifeCycleStageId.ClientID %>").val($("#ContentPlaceHolder1_hfLifeCycleStageId").val()).trigger('change');
                $("#<%=ddlLifeCycleStageId.ClientID %>").prop('disabled', false);
            }
        }
        function AddNewContactItems(title, value, parentId, detailsId, transectionType, parentType) {
            newlyAddedItem.push({
                Title: title,
                Value: value,
                TransectionType: transectionType,
                DetailsId: parseInt(detailsId, 10),
                ParentId: parseInt(parentId, 10),
                ParentType: parentType
            });
        }
        function PerformSave() {
            var name = "", contactNo = "", companyId = 0, companyName = "", jobTitle = "", email = "", emailWork = "", id = 0;
            var contactOwnerId = 0, sourceId = 0, parentComId = 0, department = "", lifeCycleId = 0, contactType = "", ticketNo = "";
            var mobPerson = "", mobWork = "", phnPerson = "", phnWork = "", fb = "", whatsApp = "", skype = "", twitter = "", dob = "", doAnniv = "", pAddress = "", aAddress = "", lifeCycleStage = "";

            var workStreet = "", workPostCode = "", workCityId = 0, workStateId = 0, workCountryId = 0;
            var personalStreet = "", personalPostCode = "", personalCityId = 0, personalStateId = 0, personalCountryId = 0, workLocationId = 0, personalLocationId = 0;

            contactOwnerId = $("#<%=ddlContactOwnerId.ClientID %>").val();
            <%--sourceId = $("#<%=ddlSourceId.ClientID %>").val();--%>
            sourceId = $("#<%=hfSourceId.ClientID %>").val();

            parentComId = $("#<%=hfCompanyId.ClientID %>").val();

            id = $("#ContentPlaceHolder1_hfId").val();
            name = $("#ContentPlaceHolder1_txtName").val();
            jobTitle = $("#ContentPlaceHolder1_txtJobTitle").val();
            department = $("#<%=txtDepartment.ClientID %>").val();
            lifeCycleId = $("#<%=ddlLifeCycleStageId.ClientID %>").val();
            lifeCycleStage = $("#<%=ddlLifeCycleStageId.ClientID %>").find(":selected").text();
            contactType = $("#<%=ddlContactType.ClientID %>").val();
            ticketNo = $("#<%=txtTicketNo.ClientID %>").val();

            workStreet = $("#<%=txtWorkLocation.ClientID %>").val();
            workPostCode = $("#<%=txtWorkPostCode.ClientID %>").val();
            workCityId = $("#<%=hfWorkCityId.ClientID %>").val();
            workStateId = $("#<%=hfWorkStateId.ClientID %>").val();
            workCountryId = $("#<%=hfWorkCountryId.ClientID %>").val();
            personalStreet = $("#<%=txtPersonalLocation.ClientID %>").val();
            personalPostCode = $("#<%=txtPersonalPostCode.ClientID %>").val();
            personalCountryId = $("#<%=hfPersonalCountryId.ClientID %>").val();
            personalStateId = $("#<%=hfPersonalStateId.ClientID %>").val();
            personalCityId = $("#<%=hfPersonalCityId.ClientID %>").val();

            workLocationId = $("#<%=hfWorkLocationId.ClientID %>").val();
            personalLocationId = $("#<%=hfPersonalLocationId.ClientID %>").val();

            workAddress = $("#<%=txtWorkAddress.ClientID %>").val();
            personalAddress = $("#<%=txtPersonalAddress.ClientID %>").val();

            mobPerson = $("#<%=txtMobilePersonal.ClientID %>").val();
            mobWork = $("#<%=txtMobileWork.ClientID %>").val();
            phnPerson = $("#<%=txtPhonePersonal.ClientID %>").val();
            phnWork = $("#<%=txtPhoneWork.ClientID %>").val();

            email = $("#<%=txtEmail.ClientID %>").val();
            emailWork = $("#<%=txtEmailWork.ClientID %>").val();
            fb = $("#<%=txtFacebook.ClientID %>").val();
            whatsApp = $("#<%=txtWhatsapp.ClientID %>").val();
            skype = $("#<%=txtSkype.ClientID %>").val();
            twitter = $("#<%=txtTwitter.ClientID %>").val();

            dob = $("#<%=txtDOB.ClientID %>").val();
            doAnniv = $("#<%=txtDateAnniv.ClientID %>").val();
            if (dob != "") {
                dob = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(dob, innBoarDateFormat);
            }
            else {
                dob = "";
            }
            if (doAnniv != "") {
                doAnniv = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(doAnniv, innBoarDateFormat);

            }
            else {
                doAnniv = "";
            }
           <%-- pAddress = $("#<%=txtPersonalAddress.ClientID %>").val();
            wAddress = $("#<%=txtAddressWork.ClientID %>").val();--%>

            //contactNo = $("#ContentPlaceHolder1_txtPopUpContactNo").val();
            //contactOwnerId = $("#ContentPlaceHolder1_ddlPopUpContactOwner").val();
            contactOwner = $("#ContentPlaceHolder1_ddlContactOwnerId option:selected").text();
            companyId = $("#ContentPlaceHolder1_hfPopUpCompany").val();
            //companyName = $("#ContentPlaceHolder1_ddlCompanyId option:selected").text();

            //email = $("#ContentPlaceHolder1_txtEmail").val();

            //if (contactNo == "") {
            //    toastr.warning("Please Add Contact Number:");
            //    flag = 0;
            //    return;
            //}
            var isSaveCompany = $("#ContentPlaceHolder1_chkIsSaveUnderCompany").is(":checked");

            if (name == "") {
                flag = 0;
                toastr.warning("Please Add Contact Name:");
                $("#<%=txtName.ClientID %>").focus();
                return false;
            }
            else if (isSaveCompany && (parentComId == "0")) {
                toastr.warning("Please Add Company");
                flag = 0;
                return false;
            }
            else if (lifeCycleId == "0") {
                toastr.warning("Please add Life Cycle Stage");
                $("#<%=ddlLifeCycleStageId.ClientID %>").focus();
                flag = 0;
                return false;
            }
            <%--else if (email != "") {
                if (CommonHelper.IsValidEmail(email) == false) {
                    toastr.warning("Invalid Email");
                    $("#<%=txtEmail.ClientID %>").focus();
                    flag = 0;
                    return false;
                }
            }
            else if (emailWork != "") {
                if (CommonHelper.IsValidEmail(emailWork) == false) {
                    toastr.warning("Invalid Email");
                    $("#<%=txtEmailWork.ClientID %>").focus();
                    flag = 0;
                    return false;
                }
            }--%>
            else if (sourceId == "0") {
                toastr.warning("Invalid Source");
                $("#<%=txtSourceName.ClientID %>").focus();
                flag = 0;
                return false;
            }
           <%-- $("#<%=hfNewCompanyId.ClientID %>").val($("#<%=ddlCompanyId.ClientID %>").val());
            var newCompanyId = $("#<%=ddlCompanyId.ClientID %>").val();
            var previousComId = $("#<%=hfPreviousCompanyId.ClientID %>").val();--%>

            var hfRandom = $("#<%=RandomProductId.ClientID %>").val();
            var deletedDocuments = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();
            var contactInformationBO = {
                Id: id,
                Name: name,
                //ContactNo: contactNo,
                ContactOwnerId: contactOwnerId,
                SourceId: sourceId,
                ContactOwner: contactOwner,
                CompanyId: parentComId,
                CompanyName: companyName,
                JobTitle: jobTitle,
                Department: department,
                LifeCycleStage: lifeCycleStage,
                LifeCycleId: lifeCycleId,
                ContactType: contactType,
                TicketNo: ticketNo,
                Email: email,
                EmailWork: emailWork,
                MobilePersonal: mobPerson,
                MobileWork: mobWork,
                PhonePersonal: phnPerson,
                PhoneWork: phnWork,
                Facebook: fb,
                Skype: skype,
                Whatsapp: whatsApp,
                Twitter: twitter,
                WorkAddress: workAddress,
                WorkCountryId: workCountryId,
                WorkStateId: workStateId,
                WorkCityId: workCityId,
                WorkLocationId: workLocationId,
                WorkStreet: workStreet,
                WorkPostCode: workPostCode,
                PersonalAddress: personalAddress,
                PersonalCountryId: personalCountryId,
                PersonalStateId: personalStateId,
                PersonalCityId: personalCityId,
                PersonalLocationId: personalLocationId,
                PersonalStreet: personalStreet,
                PersonalPostCode: personalPostCode,
                DOB: dob,
                DateAnniversary: doAnniv
            }

            var check = CheckLifeCycleStageValidation(contactInformationBO);

            if (check) {
                if (id != 0) {
                    toastr.warning("Cannot Change Life Cycle Stage More Than one Stage.");
                    $("#<%=ddlLifeCycleStageId.ClientID %>").focus();
                    flag = 0;
                    return false;
                }
                else {
                    toastr.warning("Cannot Skip First Life Cycle Stage.");
                    $("#<%=ddlLifeCycleStageId.ClientID %>").focus();
                    flag = 0;
                    return false;
                }

            }

            var label = "", content = "", Id = "0", DetailsId = "0";
            var numberGrid = $("#ContactNumberGrid tbody tr").length;
            var emailGrid = $("#EmailGrid tbody tr").length;
            var socialGrid = $("#SocialMediaGrid tbody tr").length;
            var webGrid = $("#WebsiteGrid tbody tr").length;
            if (numberGrid > 0) {
                $("#ContactNumberGrid tbody tr").each(function (index, item) {
                    label = $.trim($(item).find("td:eq(0)").text());
                    content = $.trim($(item).find("td:eq(1)").text());
                    Id = $.trim($(item).find("td:eq(3)").text());
                    DetailsId = $.trim($(item).find("td:eq(4)").text());
                    if (DetailsId == "0") {
                        AddNewContactItems(label, content, Id, DetailsId, "Number", "Contact");
                    }
                });
            }
            if (emailGrid > 0) {
                $("#EmailGrid tbody tr").each(function (index, item) {
                    label = $.trim($(item).find("td:eq(0)").text());
                    content = $.trim($(item).find("td:eq(1)").text());
                    Id = $.trim($(item).find("td:eq(3)").text());
                    DetailsId = $.trim($(item).find("td:eq(4)").text());
                    if (DetailsId == "0") {
                        AddNewContactItems(label, content, Id, DetailsId, "Email", "Contact");
                    }
                });
            }
            if (socialGrid > 0) {
                $("#SocialMediaGrid tbody tr").each(function (index, item) {
                    label = $.trim($(item).find("td:eq(0)").text());
                    content = $.trim($(item).find("td:eq(1)").text());
                    Id = $.trim($(item).find("td:eq(3)").text());
                    DetailsId = $.trim($(item).find("td:eq(4)").text());
                    if (DetailsId == "0") {
                        AddNewContactItems(label, content, Id, DetailsId, "SocialMedia", "Contact");
                    }
                });
            }
            if (webGrid > 0) {
                $("#WebsiteGrid tbody tr").each(function (index, item) {
                    label = $.trim($(item).find("td:eq(0)").text());
                    content = $.trim($(item).find("td:eq(1)").text());
                    Id = $.trim($(item).find("td:eq(3)").text());
                    DetailsId = $.trim($(item).find("td:eq(4)").text());
                    if (DetailsId == "0") {
                        AddNewContactItems(label, content, Id, DetailsId, "Website", "Contact");
                    }
                });
            }
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/Contact.aspx/SaveUpdateContact',
                data: JSON.stringify({ contactInformationBO: contactInformationBO, hfRandom: hfRandom, deletedDocument: deletedDocuments, newlyAddedItem: newlyAddedItem, deleteDbItem: deleteDbItem }),
                dataType: "json",
                async: false,
                success: function (data) {
                    OnSaveContactSucceed(data.d);
                },
                error: function (result) {
                    OnSaveContactFailed(result.d);
                }
            });
            //PageMethods.SaveUpdateContact(contactInformationBO, hfRandom, deletedDocuments, OnSaveContactSucceed, OnSaveContactFailed);
            //return false;
        }

        function OnSaveContactSucceed(result) {
            var tempId = $("#ContentPlaceHolder1_hfPopUpCompany").val();

            if (result.IsSuccess) {
                parent.ShowAlert(result.AlertMessage);

                if (typeof parent.GridPaging === "function") {
                    parent.GridPaging(1, 1);
                    PerformClearAction();
                }

                if (typeof parent.GetContactsByCompanyId == "function")
                    parent.GetContactsByCompanyId();
                if (typeof parent.LoadLog == "function")
                    parent.LoadLog();
                //
                if (result.Data == null)
                    $("#ContentPlaceHolder1_btnSearch").trigger("click");
            }
            else {
                flag = 0;
                if (result.DataStr)
                    $("#ContentPlaceHolder1_ddlLifeCycleStageId").focus();
                parent.ShowAlert(result.AlertMessage);
            }
            //PerformClearAction();

        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }
        function OnSaveContactFailed(error) {
            CommonHelper.AlertMessage(error.AlertMessage);
            return false;
        }
        //function FillFormEdit(Id) {

        //    FillForm(Id);
        //    return false;
        //}
        function CreateNewSource() {
            //PerformClearAction();
            var name = $("#<%=txtSourceName.ClientID %>").val();
            if (name != "") {
                $("#<%=txtSourceName.ClientID %>").val("");
            }
            var iframeid = 'frmSourceInfo';
            var url = "../SalesAndMarketing/iFrameSourceInformation.aspx?name=" + name;
            document.getElementById(iframeid).src = url;

            $("#SourceDialogue").dialog({
                autoOpen: true,
                modal: true,
                width: "100%",
                height: 400,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Add New Source",
                show: 'slide'
            });
            //iframeid.contentWindow.focus();
            return false;
        }
        function CreateNewCompany() {
            //PerformClearAction();
            var name = $("#<%=txtCompanyName.ClientID %>").val();
            if (name != "") {
                $("#<%=txtCompanyName.ClientID %>").val("");
            }
            var iframeid = 'frmCompanyInfo';
            var url = "../SalesAndMarketing/Company.aspx";
            document.getElementById(iframeid).src = url;

            $("#CompanyDialogue").dialog({
                autoOpen: true,
                modal: true,
                width: "100%",
                height: 600,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Add New Company",
                show: 'slide'
            });
            //iframeid.contentWindow.focus();
            return false;
        }
        function FillContactForm(Id) {
            CommonHelper.SpinnerOpen();
            PageMethods.FillForm(Id, OnFillFormSucceed, OnFillFormFailed);
            return false;
        }
        function OnFillFormSucceed(result) {
            PerformClearAction();
            CommonHelper.SpinnerClose();
            var numbers = result[0].Numbers;
            var emails = result[0].Emails;
            var socialMedias = result[0].SocialMedias;
            var websites = result[0].Websites;
            var contactInfo = result[0].ContactInformation;

            $("#<%=hfLifeCycleStageId.ClientID %>").val(contactInfo.LifeCycleId);
            $("#ContentPlaceHolder1_btnContactSaveClose").val("Update And Close");
            $("#ContentPlaceHolder1_btnContactSaveContinue").hide();
            $("#ContentPlaceHolder1_btnContactClear").show();
            //AddNewContact();
            //$("#AddNewContactContaiiner").dialog({ title: "Edit Contact - " + result.Name + "" });
            $("#ContentPlaceHolder1_hfId").val(contactInfo.Id);
            $("#ContentPlaceHolder1_txtName").val(contactInfo.Name);
            //$("#ContentPlaceHolder1_txtPopUpContactNo").val(result.ContactNo);
            //$("#ContentPlaceHolder1_ddlPopUpContactOwner").val(result.ContactOwnerId);
            //$("#ContentPlaceHolder1_txtPopUpCompany").val(result.CompanyName);
            $("#ContentPlaceHolder1_txtJobTitle").val(contactInfo.JobTitle);
            //$("#ContentPlaceHolder1_txtMail").val(result.Email);

            if (contactInfo.CompanyId != 0) {
                $("#companyDiv").show();
                $("#emailWork").show();
                $("#<%=ddlLifeCycleStageId.ClientID %>").prop("disabled", true);
                $("#ContentPlaceHolder1_hfPopUpCompany").val(contactInfo.CompanyId);
                $("#ContentPlaceHolder1_chkIsSaveUnderCompany").prop("checked", true);
                $("#<%=hfPreviousCompanyId.ClientID %>").val(contactInfo.CompanyId);
            }
            else {
                $("#companyDiv").hide();
                $("#emailWork").hide();
                $("#ContentPlaceHolder1_chkIsSaveUnderCompany").prop("checked", false);
                $("#<%=ddlLifeCycleStageId.ClientID %>").prop("disabled", false);
            }

            $("#<%=txtEmail.ClientID %>").val(contactInfo.Email);
            $("#<%=txtEmailWork.ClientID %>").val(contactInfo.EmailWork);
            $("#<%=ddlContactOwnerId.ClientID %>").val(contactInfo.ContactOwnerId).trigger('change');

            $("#<%=hfCompanyId.ClientID %>").val(contactInfo.CompanyId);
            $("#<%=txtCompanyName.ClientID %>").val(contactInfo.CompanyName);

            $("#<%=hfSourceId.ClientID %>").val(contactInfo.SourceId);
            $("#<%=txtSourceName.ClientID %>").val(contactInfo.SourceName);
            $("#<%=txtDepartment.ClientID %>").val(contactInfo.Department);
            $("#<%=ddlLifeCycleStageId.ClientID %>").val(contactInfo.LifeCycleId);
            $("#<%=ddlContactType.ClientID %>").val(contactInfo.ContactType);
            $("#<%=txtTicketNo.ClientID %>").val(contactInfo.TicketNo);
            if (contactInfo.DOB != null) {
                $("#<%=txtDOB.ClientID %>").val(CommonHelper.DateFromDateTimeToDisplay(contactInfo.DOB, innBoarDateFormat));
            }
            if (contactInfo.DateAnniversary != null) {
                $("#<%=txtDateAnniv.ClientID %>").val(CommonHelper.DateFromDateTimeToDisplay(contactInfo.DateAnniversary, innBoarDateFormat));
            }

            $("#<%=txtWorkAddress.ClientID %>").val(contactInfo.WorkAddress);
            $("#<%=hfWorkLocationId.ClientID %>").val(contactInfo.WorkLocationId);
            $("#<%=txtWorkLocation.ClientID %>").val(contactInfo.WorkStreet);
            $("#<%=txtWorkPostCode.ClientID %>").val(contactInfo.WorkPostCode);
            $("#<%=hfWorkCityId.ClientID %>").val(contactInfo.WorkCityId);
            $("#<%=txtWorkCity.ClientID %>").val(contactInfo.WorkCity);
            $("#<%=hfWorkStateId.ClientID %>").val(contactInfo.WorkStateId);
            $("#<%=txtWorkState.ClientID %>").val(contactInfo.WorkState);
            $("#<%=hfWorkCountryId.ClientID %>").val(contactInfo.WorkCountryId);
            $("#<%=txtWorkCountry.ClientID %>").val(contactInfo.WorkCountry);

            $("#<%=txtPersonalAddress.ClientID %>").val(contactInfo.PersonalAddress);
            $("#<%=hfPersonalLocationId.ClientID %>").val(contactInfo.PersonalLocationId);
            $("#<%=txtPersonalLocation.ClientID %>").val(contactInfo.PersonalStreet);
            $("#<%=txtPersonalPostCode.ClientID %>").val(contactInfo.PersonalPostCode);
            $("#<%=hfPersonalCityId.ClientID %>").val(contactInfo.PersonalCityId);
            $("#<%=txtPersonalCity.ClientID %>").val(contactInfo.PersonalCity);
            $("#<%=hfPersonalStateId.ClientID %>").val(contactInfo.PersonalStateId);
            $("#<%=txtPersonalState.ClientID %>").val(contactInfo.PersonalState);
            $("#<%=hfPersonalCountryId.ClientID %>").val(contactInfo.PersonalCountryId);
            $("#<%=txtPersonalCountry.ClientID %>").val(contactInfo.PersonalCountry);

            $("#<%=txtMobilePersonal.ClientID %>").val(contactInfo.MobilePersonal);
            $("#<%=txtMobileWork.ClientID %>").val(contactInfo.MobileWork);
            $("#<%=txtPhonePersonal.ClientID %>").val(contactInfo.PhonePersonal);
            $("#<%=txtPhoneWork.ClientID %>").val(contactInfo.PhoneWork);

            $("#<%=txtFacebook.ClientID %>").val(contactInfo.Facebook);
            $("#<%=txtSkype.ClientID %>").val(contactInfo.Skype);
            $("#<%=txtWhatsapp.ClientID %>").val(contactInfo.Whatsapp);
            $("#<%=txtTwitter.ClientID %>").val(contactInfo.Twitter);

            if (numbers.length > 0) {
                for (var i = 0; i < numbers.length; i++) {
                    LoadContactTable(numbers[i].Value, numbers[i].Title, "ContactNumberGrid", "", "", numbers[i].ParentId, numbers[i].DetailsId);
                }
            }
            if (emails.length > 0) {
                for (var i = 0; i < emails.length; i++) {
                    LoadContactTable(emails[i].Value, emails[i].Title, "EmailGrid", "", "", emails[i].ParentId, emails[i].DetailsId);
                }
            }
            if (socialMedias.length > 0) {
                for (var i = 0; i < socialMedias.length; i++) {
                    LoadContactTable(socialMedias[i].Value, socialMedias[i].Title, "SocialMediaGrid", "", "", socialMedias[i].ParentId, socialMedias[i].DetailsId);
                }
            }
            if (websites.length > 0) {
                for (var i = 0; i < websites.length; i++) {
                    LoadContactTable(websites[i].Value, websites[i].Title, "WebsiteGrid", "", "", websites[i].ParentId, websites[i].DetailsId);
                }
            }

            UploadComplete();
            CommonHelper.SpinnerClose();
        }
        function OnFillFormFailed(error) {
            CommonHelper.AlertMessage(error.AlertMessage);
            CommonHelper.SpinnerClose();
            return false;
        }
        function CloseSourceDialog() {
            $('#SourceDialogue').dialog('close');
            return false;
        }
        function CloseCompanyDialog() {
            $('#CompanyDialogue').dialog('close');
            return false;
        }
        function ShowMsgDialog(msg) {
            CommonHelper.AlertMessage(msg);
            return false;
        }
        function SaveAndClose() {

            flag = 1;
            $.when(PerformSave()).done(function () {

                if (flag == 1) {
                    if (typeof parent.FillForm === "function") {
                        var id = $("#ContentPlaceHolder1_hfId").val();
                        parent.FillForm(id);
                    }
                    if (typeof parent.CloseContactDialog === "function")
                        parent.CloseContactDialog();
                    if (typeof parent.CloseDialog === "function")
                        parent.CloseDialog();
                    if ($("#btnSave").val() == "Update and Close") {
                        $("#btnSave").val("Save And Close");
                        $("#btnContactSaveContinue").show();
                        $("#btnCancel").show();
                    }
                    PerformClearAction();
                    //$('#AddNewContactContaiiner').dialog('close');
                }
            });
            return false;
        }
        function SaveAndContinue() {
            PerformSave();
            return false;
        }
        function CheckLifeCycleStageValidation(contactInformationBO) {
            var returnInfo = false;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/Contact.aspx/CheckLifeCycleStageValidation',
                data: JSON.stringify({ contactInformationBO: contactInformationBO }),
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d > 0) {
                        <%--flag = 0;
                        toastr.warning("Duplicate Company Name");
                        $("#<%=txtCompanyName.ClientID %>").focus();--%>
                        returnInfo = true;
                    }
                    else {
                        returnInfo = false;
                    }

                },
                error: function (result) {
                    toastr.error("Ajax call error.");
                    return false;
                }
            });
            return returnInfo;
        }

        function PerformClearAction() {
            $("#ContentPlaceHolder1_btnContactSaveClose").val("Save And Close");
            $("#ContentPlaceHolder1_btnContactSaveContinue").val("Save And Continue");
            $("#ContentPlaceHolder1_btnContactClear").show();
            $("#ContentPlaceHolder1_btnContactSaveContinue").show();

            $("#<%=hfLifeCycleStageId.ClientID %>").val("0");
            $("#ContentPlaceHolder1_txtName").val("");
            $("#ContentPlaceHolder1_txtJobTitle").val("");
            $("#ContentPlaceHolder1_hfPopUpCompany").val("0");
            $("#<%=hfCompanyId.ClientID %>").val("0");
            $("#<%=txtCompanyName.ClientID %>").val("");

            $("#<%=ddlContactType.ClientID %>").val("0");
            $("#<%=hfSourceId.ClientID %>").val("0");
            $("#<%=txtSourceName.ClientID %>").val("");
            $("#<%=ddlLifeCycleStageId.ClientID %>").val("0");
            $("#<%=txtEmail.ClientID %>").val("");
            $("#<%=txtEmailWork.ClientID %>").val("");
            $("#<%=txtDepartment.ClientID %>").val("");
            $("#<%=txtTicketNo.ClientID %>").val("");
            $("#<%=txtMobilePersonal.ClientID %>").val("");
            $("#<%=txtMobileWork.ClientID %>").val("");
            $("#<%=txtPhoneWork.ClientID %>").val("");
            $("#<%=txtPhonePersonal.ClientID %>").val("");
            $("#<%=txtFacebook.ClientID %>").val("");
            $("#<%=txtWhatsapp.ClientID %>").val("");
            $("#<%=txtSkype.ClientID %>").val("");
            $("#<%=txtTwitter.ClientID %>").val("");
            $("#<%=txtDOB.ClientID %>").val("");
            $("#<%=txtDateAnniv.ClientID %>").val("");

            $("#<%=txtWorkAddress.ClientID %>").val("");
            $("#<%=hfWorkLocationId.ClientID %>").val("0");
            $("#<%=txtWorkLocation.ClientID %>").val("");
            $("#<%=txtWorkPostCode.ClientID %>").val("");
            $("#<%=hfWorkCityId.ClientID %>").val("0");
            $("#<%=txtWorkCity.ClientID %>").val("");
            $("#<%=hfWorkStateId.ClientID %>").val("0");
            $("#<%=txtWorkState.ClientID %>").val("");
            $("#<%=hfWorkCountryId.ClientID %>").val("0");
            $("#<%=txtWorkCountry.ClientID %>").val("");

            $("#<%=txtPersonalAddress.ClientID %>").val("");
            $("#<%=hfPersonalLocationId.ClientID %>").val("0");
            $("#<%=txtPersonalLocation.ClientID %>").val("");
            $("#<%=txtPersonalPostCode.ClientID %>").val("");
            $("#<%=hfPersonalCityId.ClientID %>").val("0");
            $("#<%=txtPersonalCity.ClientID %>").val("");
            $("#<%=hfPersonalStateId.ClientID %>").val("0");
            $("#<%=txtPersonalState.ClientID %>").val("");
            $("#<%=hfPersonalCountryId.ClientID %>").val("0");
            $("#<%=txtPersonalCountry.ClientID %>").val("");

            if ($("#ContentPlaceHolder1_chkIsSaveUnderCompany").is(":checked")) {
                $("#<%=chkIsSaveUnderCompany.ClientID %>").prop("checked", false);
                CheckSaveUnderCompany();
            }
            deleteDbItem = []; editDbItem = []; newlyAddedItem = [];

            $("#ContactNumberGrid tbody tr").html("");
            $("#EmailGrid tbody tr").html("");
            $("#SocialMediaGrid tbody tr").html("");
            $("#WebsiteGrid tbody tr").html("");

            $("#ContentPlaceHolder1_hfId").val("0");
            $("#ContentPlaceHolder1_hfGuestDeletedDoc").val("");
            return false;
        }
        function UploadComplete() {
            var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var id = +$("#ContentPlaceHolder1_hfId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();

            PageMethods.LoadContactDocument(id, randomId, deletedDoc, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }

        function OnLoadDocumentSucceeded(result) {
            var guestDoc = result;
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";

            guestDocumentTable += "<table id='contactDocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            guestDocumentTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }

                guestDocumentTable += "<td align='left' style='width: 50%'>" + guestDoc[row].Name + "</td>";

                if (guestDoc[row].Path != "") {
                    if (guestDoc[row].Extention == ".jpg")
                        imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    else
                        imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
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

            // docc = guestDocumentTable;

            $("#ContactDocumentInfo").html(guestDocumentTable);
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }
        //function UploadComplete() {
        //    var id = $("#ContentPlaceHolder1_RandomProductId").val();
        //    ShowUploadedDocument(id);
        //}
        //function ShowUploadedDocument(id) {
        //    PageMethods.GetUploadedImageByWebMethod(id, "CompanyDoc", OnGetUploadedImageByWebMethodSucceeded, OnGetUploadedImageByWebMethodFailed);
        //    return false;
        //}
        //function OnGetUploadedImageByWebMethodSucceeded(result) {
        //    if (result != "") {
        //        $('#ContactDocumentInfo').show();
        //    }
        //    else {
        //        $('#ContactDocumentInfo').hide();
        //    }
        //    $('#CompanyDocumentInfo').html(result);
        //    return false;
        //}
        //function OnGetUploadedImageByWebMethodFailed(error) {
        //    toastr.error("Please Contact With Admin. Upload Failed.");
        //}
        function LoadDocUploader() {

            var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var path = "/SalesAndMarketing/Images/Contact/";
            var category = "ContactDocument";
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
        //function AttachFile() {
        //    $("#contactdocuments").dialog({
        //        autoOpen: true,
        //        modal: true,
        //        width: 900,
        //        closeOnEscape: true,
        //        resizable: false,
        //        title: "Contact Documents",
        //        show: 'slide'
        //    });
        //}
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
    <div id="SourceDialogue" style="display: none;">
        <iframe id="frmSourceInfo" name="IframeName" runat="server" height="100%" width="100%"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="CompanyDialogue" style="display: none;">
        <iframe id="frmCompanyInfo" name="IframeName" runat="server" height="100%" width="100%"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <%--<div id="contactdocuments" style="display: none;">
        <label for="Attachment" class="control-label col-md-2">
            Attachment</label>
        <div class="col-md-4">
            <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
                <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                    FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
            </asp:Panel>
        </div>
    </div>--%>
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfcontactDetailsId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfSourceId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfLifeCycleStageId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfPreviousCompanyId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfNewCompanyId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfGuestDeletedDoc" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfWorkCountryId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfWorkStateId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfWorkCityId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfWorkLocationId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfPersonalCountryId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfPersonalStateId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfPersonalCityId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfPersonalLocationId" runat="server" Value="0"></asp:HiddenField>
    <div id="AddNewContactContaiiner">
        <div id="AddPanel" class="panel panel-default">
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="control-label col-md-2 ">Account Manager</label>
                        <div class="col-sm-10">
                            <asp:DropDownList ID="ddlContactOwnerId" runat="server" CssClass="form-control" TabIndex="1">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-md-2 required-field">Source Name</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtSourceName" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-sm-1">
                            <input type="button" id="btnAddNewSource" class="TransactionalButton btn btn-primary btn-sm" value="+" title="Add New Source" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-md-2"></label>
                        <div class="col-md-4">
                            <asp:CheckBox ID="chkIsSaveUnderCompany" runat="server" onclick='CheckSaveUnderCompany()' />
                            &nbsp;
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Is Save Under Company"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group" id="companyDiv" style="display: none">
                        <label class="control-label col-md-2">Company Name</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtCompanyName" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-sm-1">
                            <input type="button" id="btnAddNewCompany" class="TransactionalButton btn btn-primary btn-sm" value="+" title="Add New Company" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-md-2 required-field">Name</label>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtName" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-md-2 ">Title</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtJobTitle" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <label class="control-label col-md-2 ">Department</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDepartment" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-md-2 required-field">Life Cycle Stage</label>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlLifeCycleStageId" runat="server" CssClass="form-control" TabIndex="1">
                            </asp:DropDownList>
                        </div>
                        <label class="control-label col-md-2">Contact Type</label>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlContactType" runat="server" CssClass="form-control" TabIndex="1">
                                <asp:ListItem Text="--- Please Select ---" Value="0" />
                                <asp:ListItem Text="Business" Value="Business" />
                                <asp:ListItem Text="Billing" Value="Billing" />
                                <asp:ListItem Text="Technical" Value="Technical" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group" style="display: none;">
                        <label class="control-label col-md-2 ">Ticket No.</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtTicketNo" runat="server" placeholder="TN00000001" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                    <div style="display: none">
                        <div class="form-group">
                            <label class="control-label col-md-2">Mobile (Personal)</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtMobilePersonal" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                            <label class="control-label col-md-2 ">Phone (Home)</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPhonePersonal" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-2 ">Mobile (Work)</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtMobileWork" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                            <label class="control-label col-md-2 ">Phone (Work)</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPhoneWork" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-2">Email (Personal)</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" id="emailWork" style="display: none;">
                            <label class="control-label col-md-2">Email (Work)</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtEmailWork" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-md-2">Contact Number</label>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtContactNumber" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                        <div class="col-md-2" id="divContactLabel">
                            <asp:DropDownList ID="ddlPhoneTitle" runat="server" CssClass="form-control" TabIndex="1">
                            </asp:DropDownList>
                            <div style="display: none;">
                                <asp:TextBox ID="txtContactLabel" runat="server" CssClass="form-control" placeholder="Label" TabIndex="2" Style="display: none;"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-1">
                            <input type="button" id="btnAddContact" class="TransactionalButton btn btn-primary btn-sm" value="+" title="Add Contact" />
                        </div>
                        <div class="col-md-4">
                            <table id="ContactNumberGrid" class="table table-bordered table-condensed table-responsive"
                                style="width: 100%; display: none">
                                <thead>
                                    <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                        <th style="width: 30%;">Label
                                        </th>
                                        <th style="width: 60%;">Number
                                        </th>
                                        <th style="width: 10%; text-align: center">Action
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-md-2">Email</label>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtEmailId" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                        <div class="col-md-2" id="divEmailLabel">
                            <asp:DropDownList ID="ddlEmailTitle" runat="server" CssClass="form-control" TabIndex="1">
                            </asp:DropDownList>
                            <div style="display: none;">
                                <asp:TextBox ID="txtEmailLabel" runat="server" CssClass="form-control" placeholder="Label" TabIndex="2" Style="display: none;"></asp:TextBox>
                                <
                            </div>
                        </div>
                        <div class="col-sm-1">
                            <input type="button" id="btnAddEmail" class="TransactionalButton btn btn-primary btn-sm" value="+" title="Add Email" />
                        </div>
                        <div class="col-md-4">
                            <table id="EmailGrid" class="table table-bordered table-condensed table-responsive"
                                style="width: 100%; display: none">
                                <thead>
                                    <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                        <th style="width: 30%;">Label
                                        </th>
                                        <th style="width: 60%;">Email
                                        </th>
                                        <th style="width: 10%; text-align: center">Action
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-md-2">Social Media</label>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtSocialMedia" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:DropDownList ID="ddlSocialMediaTitle" runat="server" CssClass="form-control" TabIndex="1">
                            </asp:DropDownList>
                            <div style="display: none;">
                                <asp:TextBox ID="txtSocialMediaLabel" runat="server" CssClass="form-control" placeholder="Label" TabIndex="2" Style="display: none;"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-1">
                            <input type="button" id="btnAddSocialMedia" class="TransactionalButton btn btn-primary btn-sm" value="+" title="Add Social Media" />
                        </div>
                        <div class="col-md-4">
                            <table id="SocialMediaGrid" class="table table-bordered table-condensed table-responsive"
                                style="width: 100%; display: none">
                                <thead>
                                    <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                        <th style="width: 30%;">Label
                                        </th>
                                        <th style="width: 60%;">Social Media
                                        </th>
                                        <th style="width: 10%; text-align: center">Action
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-md-2">Website</label>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtWebsite" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:DropDownList ID="ddlWebsiteTitle" runat="server" CssClass="form-control" TabIndex="1">
                            </asp:DropDownList>
                            <div style="display: none;">
                                <asp:TextBox ID="txtWebsiteLabel" runat="server" CssClass="form-control" placeholder="Label" TabIndex="2" Style="display: none;"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-1">
                            <input type="button" id="btnWebsite" class="TransactionalButton btn btn-primary btn-sm" value="+" title="Add Website" />
                        </div>
                        <div class="col-md-4">
                            <table id="WebsiteGrid" class="table table-bordered table-condensed table-responsive"
                                style="width: 100%; display: none">
                                <thead>
                                    <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                        <th style="width: 30%;">Label
                                        </th>
                                        <th style="width: 60%;">Website
                                        </th>
                                        <th style="width: 10%; text-align: center">Action
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div style="display: none">
                        <div class="form-group">
                            <label class="control-label col-md-2 ">Facebook</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFacebook" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                            <label class="control-label col-md-2 ">WhatsApp</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtWhatsapp" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-2 ">Skype</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSkype" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                            <label class="control-label col-md-2 ">Twitter</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTwitter" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-md-2 ">Date of Birth</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                        <label class="control-label col-md-2 ">Date of Aniversary</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDateAnniv" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <fieldset>
                            <legend>Work Address</legend>
                            <div class="form-group">
                                <label for="Address" class="control-label col-md-2">Address</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtWorkAddress" runat="server" CssClass="form-control" TextMode="MultiLine"
                                        TabIndex="9"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-md-2 ">Country</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtWorkCountry" runat="server" CssClass="form-control" TabIndex="1">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-md-2 ">State/ Province/ District</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtWorkState" runat="server" CssClass="form-control" TabIndex="1">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-md-2 ">City</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtWorkCity" runat="server" CssClass="form-control" TabIndex="1">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" runat="server" id="WorkAreaDiv">
                                <label class="control-label col-md-2 ">Area</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtWorkLocation" runat="server" CssClass="form-control"
                                        TabIndex="2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-md-2 ">Postal Code</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtWorkPostCode" runat="server" CssClass="form-control"
                                        TabIndex="2"></asp:TextBox>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <div class="form-group" id="DivPersonalAddress" runat="server">
                        <fieldset>
                            <legend>Home Address</legend>
                            <div class="form-group">
                                <label for="Address" class="control-label col-md-2">Address</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtPersonalAddress" runat="server" CssClass="form-control" TextMode="MultiLine"
                                        TabIndex="9"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-md-2 ">Country</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtPersonalCountry" runat="server" CssClass="form-control" TabIndex="1">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-md-2 ">State/ Province/ District</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtPersonalState" runat="server" CssClass="form-control" TabIndex="1">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-md-2 ">City</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtPersonalCity" runat="server" CssClass="form-control" TabIndex="1">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" runat="server" id="PersonalAreaDiv">
                                <label class="control-label col-md-2 ">Area</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtPersonalLocation" runat="server" CssClass="form-control"
                                        TabIndex="2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-md-2 ">Postal Code</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtPersonalPostCode" runat="server" CssClass="form-control"
                                        TabIndex="2"></asp:TextBox>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <div class="form-group">
                        <asp:HiddenField ID="RandomProductId" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="tempId" runat="server" Value="0"></asp:HiddenField>
                        <div>
                            <label class="control-label col-md-2">Attachment</label>
                        </div>
                        <div class="col-md-10">
                            <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Attach" onclick="LoadDocUploader()" />
                        </div>
                    </div>
                    <div id="ContactDocumentInfo">
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnContactSaveClose" runat="server" Text="Save And Close" OnClientClick="javascript:return SaveAndClose();"
                                CssClass="TransactionalButton btn btn-primary btn-sm" />
                            <asp:Button ID="btnContactSaveContinue" runat="server" Text="Save And Continue" OnClientClick="javascript:return SaveAndContinue();"
                                CssClass="TransactionalButton btn btn-primary btn-sm" />
                            <asp:Button ID="btnContactClear" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript: return PerformClearAction();" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
