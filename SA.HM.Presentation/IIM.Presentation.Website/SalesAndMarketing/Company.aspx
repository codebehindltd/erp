<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="Company.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.Company" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var flag = 0;
        var Id = "";
        var contactDetailsId = "0";
        var deleteDbItem = [], editDbItem = [], newlyAddedItem = [];
        $(document).ready(function () {
            LoadParentCompanyAfterSave();

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
            $("#ContentPlaceHolder1_ddlFaxTitle").select2({
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
            $("#ContentPlaceHolder1_ddlCompanyOwner").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlParentCompany").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlCompanyType").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlIndustry").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlOwnership").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlBillingCityId").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlBillingStateId").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlBillingCountryId").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlShippingCityId").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlShippingStateId").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlShippingCountryId").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            var txtDiscountPercent = '<%=txtDiscountPercent.ClientID%>'
            $('#' + txtDiscountPercent).blur(function () {
                var discountAmount = $('#' + txtDiscountPercent).val();
                if (discountAmount >= 0) {
                    $('#' + lblMessage).text("");
                }
                else {
                    $('#' + lblMessage).text("Vat amount is not in correct format");
                }
            });
            CommonHelper.ApplyDecimalValidation();
            CommonHelper.ApplyIntigerValidation();

            var companyId = $.trim(CommonHelper.GetParameterByName("cid"));
            if (companyId != "") {
                if (companyId > 0) {
                    FillForm(companyId);
                }
            }

            //multiple contact details start 
            Id = $("#<%=hfCompanyId.ClientID %>").val();
            contactDetailsId = $("#<%=hfcontactDetailsId.ClientID %>").val();

            $("#btnAddPhone").click(function () {
                var content = $("#<%=txtPhone.ClientID %>").val();
                var phoneTitle = $("#<%=ddlPhoneTitle.ClientID %>").val();
                var label = $("#<%=ddlPhoneTitle.ClientID %> option:selected").text();
                var tableId = "PhoneGrid";

                if (content != "" && label != "") {
                    var validPhone = IsValidPhone(content);
                    if (!validPhone) {
                        toastr.warning("Invalid Phone Number");
                        $("#<%=txtPhone.ClientID %>").focus();
                        return false;
                    }
                    if (phoneTitle == "0") {
                        toastr.warning("Please Select Phone Title");
                        $("#<%=ddlPhoneTitle.ClientID %>").focus();
                        return false;
                    }

                    LoadContactTable(content, label, tableId, "txtPhone", "txtPhoneLabel", Id, contactDetailsId);
                }
                else if (content == "") {
                    toastr.warning("Please add a number");
                    $("#<%=txtPhone.ClientID %>").focus();
                    return false;
                }
                else if (phoneTitle == "0") {
                    toastr.warning("Please Select Phone Title");
                    $("#<%=ddlPhoneTitle.ClientID %>").focus();
                    return false;
                }
            });

            $("#btnAddEmail").click(function () {
                var content = $("#<%=txtEmail.ClientID %>").val();
                var emailTitle = $("#<%=ddlEmailTitle.ClientID %>").val();
                var label = $("#<%=ddlEmailTitle.ClientID %> option:selected").text();

                var tableId = "EmailGrid";
                if (content != "" && label != "") {
                    if (CommonHelper.IsValidEmail(content) == false) {
                        toastr.warning("Invalid Email");
                        $("#<%=txtEmail.ClientID %>").focus();
                        return false;
                    }

                    if (emailTitle == "0") {
                        toastr.warning("Please Select Email Title");
                        $("#<%=ddlPhoneTitle.ClientID %>").focus();
                        return false;
                    }

                    LoadContactTable(content, label, tableId, "txtEmail", "txtEmailLabel", Id, contactDetailsId);
                }
                else if (email == "") {
                    toastr.warning("Please add a email");
                    $("#<%=txtEmail.ClientID %>").focus();
                    return false;
                }
                else if (emailTitle == "0") {
                    toastr.warning("Please Select Email Title");
                    $("#<%=ddlPhoneTitle.ClientID %>").focus();
                    return false;
                }
            });
            $("#btnAddFax").click(function () {
                var content = $("#<%=txtFax.ClientID %>").val();
                var faxTitle = $("#<%=ddlFaxTitle.ClientID %>").val();
                var label = $("#<%=ddlFaxTitle.ClientID %> option:selected").text();

                var tableId = "FaxGrid";
                if (content != "" && label != "") {
                    if (faxTitle == "0") {
                        toastr.warning("Please Select Fax Title");
                        $("#<%=ddlPhoneTitle.ClientID %>").focus();
                        return false;
                    }

                    LoadContactTable(content, label, tableId, "txtFax", "txtFaxLabel", Id, contactDetailsId);
                }
                else if (content == "") {
                    toastr.warning("Please add a social media link");
                    $("#<%=txtFax.ClientID %>").focus();
                    return false;
                }
                else if (faxTitle == "0") {
                    toastr.warning("Please Select Fax Title");
                    $("#<%=ddlPhoneTitle.ClientID %>").focus();
                    return false;
                }
            });

            $("#btnWebsite").click(function () {
                var content = $("#<%=txtWebsite.ClientID %>").val();
                var websiteTitle = $("#<%=ddlWebsiteTitle.ClientID %>").val();
                var label = $("#<%=ddlWebsiteTitle.ClientID %> option:selected").text();

                var tableId = "WebsiteGrid";
                if (content != "" && label != "") {
                    if (websiteTitle == "0") {
                        toastr.warning("Please Select Website Title");
                        $("#<%=ddlPhoneTitle.ClientID %>").focus();
                        return false;
                    }

                    LoadContactTable(content, label, tableId, "txtWebsite", "txtWebsiteLabel", Id, contactDetailsId);
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
            });

            $('#ContentPlaceHolder1_txtPhone').on('input', function (e) {
                var contactNumber = $("#<%=txtPhone.ClientID %>").val();
                var id = document.getElementById('<%=txtPhoneLabel.ClientID %>');
                LabelShowHide(id, contactNumber);
            });
            $('#ContentPlaceHolder1_txtEmail').on('input', function (e) {
                var email = $("#<%=txtEmail.ClientID %>").val();
                var id = document.getElementById('<%=txtEmailLabel.ClientID %>');
                LabelShowHide(id, email);
            });
            $('#ContentPlaceHolder1_txtWebsite').on('input', function (e) {
                var content = $("#<%=txtWebsite.ClientID %>").val();
                var id = document.getElementById('<%=txtWebsiteLabel.ClientID %>');
                LabelShowHide(id, content);
            });
            $('#ContentPlaceHolder1_txtFax').on('input', function (e) {
                var content = $("#<%=txtFax.ClientID %>").val();
                var id = document.getElementById('<%=txtFaxLabel.ClientID %>');
                LabelShowHide(id, content);
            });

            $("#ContentPlaceHolder1_txtPhoneLabel").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/Contact.aspx/LoadLabelByAutoSearch',
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
            $("#ContentPlaceHolder1_txtFaxLabel").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/Contact.aspx/LoadLabelByAutoSearch',
                        data: JSON.stringify({ searchTerm: request.term, Type: "Fax" }),
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
            $("#ContentPlaceHolder1_txtBillingCountry").autocomplete({
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
                    $("#ContentPlaceHolder1_txtBillingCountry").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfBillingCountryId").val(ui.item.CountryId);
                    $("#ContentPlaceHolder1_txtBillingState").val("");
                    $("#ContentPlaceHolder1_hfBillingStateId").val("0");
                    $("#ContentPlaceHolder1_txtBillingCity").val("");
                    $("#ContentPlaceHolder1_hfBillingCityId").val("0");
                    $("#ContentPlaceHolder1_txtBillingLocation").val("");
                    $("#ContentPlaceHolder1_hfBillingLocationId").val("0");
                }
            });
            $("#ContentPlaceHolder1_txtShippingCountry").autocomplete({
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
                    $("#ContentPlaceHolder1_txtShippingCountry").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfShippingCountryId").val(ui.item.CountryId);
                    $("#ContentPlaceHolder1_txtShippingState").val("");
                    $("#ContentPlaceHolder1_hfShippingStateId").val("0");
                    $("#ContentPlaceHolder1_txtShippingCity").val("");
                    $("#ContentPlaceHolder1_hfShippingCityId").val("0");
                    $("#ContentPlaceHolder1_txtShippingLocation").val("");
                    $("#ContentPlaceHolder1_hfShippingLocationId").val("0");
                }
            });

            $("#ContentPlaceHolder1_txtBillingState").autocomplete({
                source: function (request, response) {
                    var billingCountry = $("#ContentPlaceHolder1_hfBillingCountryId").val();
                    if (billingCountry == 0) {
                        toastr.warning("Please Select Billing Country");
                        $("#ContentPlaceHolder1_hfBillingCountryId").focus();
                        return false;
                    }
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/Company.aspx/LoadStateForAutoSearchByCountry',
                        data: JSON.stringify({ searchString: request.term, CountryId: billingCountry }),
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
                    $("#ContentPlaceHolder1_txtBillingState").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfBillingStateId").val(ui.item.Id);
                    $("#ContentPlaceHolder1_txtBillingCity").val("");
                    $("#ContentPlaceHolder1_hfBillingCityId").val("0");
                    $("#ContentPlaceHolder1_txtShippingLocation").val("");
                    $("#ContentPlaceHolder1_hfShippingLocationId").val("0");
                }
            });

            //multiple contact details start 
            $("#ContentPlaceHolder1_txtShippingState").autocomplete({
                source: function (request, response) {
                    var ShippingCountry = $("#ContentPlaceHolder1_hfShippingCountryId").val();
                    if (ShippingCountry == 0) {
                        toastr.warning("Please Select Shipping Country");
                        $("#ContentPlaceHolder1_hfShippingCountryId").focus();
                        return false;
                    }
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/Company.aspx/LoadStateForAutoSearchByCountry',
                        data: JSON.stringify({ searchString: request.term, CountryId: ShippingCountry }),
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
                    $("#ContentPlaceHolder1_txtShippingState").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfShippingStateId").val(ui.item.Id);
                    $("#ContentPlaceHolder1_txtShippingCity").val("");
                    $("#ContentPlaceHolder1_hfShippingCityId").val("0");
                    $("#ContentPlaceHolder1_txtShippingLocation").val("");
                    $("#ContentPlaceHolder1_hfShippingLocationId").val("0");
                }
            });

            $("#ContentPlaceHolder1_txtBillingCity").autocomplete({
                source: function (request, response) {
                    var countryId = $("#ContentPlaceHolder1_hfBillingCountryId").val();
                    var stateId = $("#ContentPlaceHolder1_hfBillingStateId").val();
                    var stateString = $("#ContentPlaceHolder1_txtBillingState").val();

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
                    $("#ContentPlaceHolder1_txtBillingCity").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfBillingCityId").val(ui.item.Id);
                    $("#ContentPlaceHolder1_txtBillingLocation").val("");
                    $("#ContentPlaceHolder1_hfBillingLocationId").val("0");
                }
            });

            //multiple contact details start 
            $("#ContentPlaceHolder1_txtShippingCity").autocomplete({
                source: function (request, response) {
                    var countryId = $("#ContentPlaceHolder1_hfShippingCountryId").val();
                    var stateId = $("#ContentPlaceHolder1_hfShippingStateId").val();
                    var stateString = $("#ContentPlaceHolder1_txtShippingState").val();

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
                    $("#ContentPlaceHolder1_txtShippingCity").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfShippingCityId").val(ui.item.Id);
                    $("#ContentPlaceHolder1_txtShippingLocation").val("");
                    $("#ContentPlaceHolder1_hfShippingLocationId").val("0");
                }
            });

            //multiple contact details start 
            $("#ContentPlaceHolder1_txtBillingLocation").autocomplete({
                source: function (request, response) {
                    var countryId = $("#ContentPlaceHolder1_hfBillingCountryId").val();
                    var stateId = $("#ContentPlaceHolder1_hfBillingStateId").val();
                    var stateString = $("#ContentPlaceHolder1_txtBillingState").val();

                    var cityId = $("#ContentPlaceHolder1_hfBillingCityId").val();
                    var cityString = $("#ContentPlaceHolder1_txtBillingCity").val();

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
                    $("#ContentPlaceHolder1_txtBillingLocation").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfBillingLocationId").val(ui.item.Id);
                }
            });

            //multiple contact details start 
            $("#ContentPlaceHolder1_txtShippingLocation").autocomplete({
                source: function (request, response) {
                    var countryId = $("#ContentPlaceHolder1_hfShippingCountryId").val();
                    var stateId = $("#ContentPlaceHolder1_hfShippingStateId").val();
                    var stateString = $("#ContentPlaceHolder1_txtShippingState").val();
                    var cityId = $("#ContentPlaceHolder1_hfShippingCityId").val();
                    var cityString = $("#ContentPlaceHolder1_txtShippingCity").val();

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
                    $("#ContentPlaceHolder1_txtShippingLocation").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfShippingLocationId").val(ui.item.Id);
                }
            });

            $("[id=ContentPlaceHolder1_gvCompany_ChkCreate]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#ContentPlaceHolder1_gvCompany tbody tr").find("td:eq(0)").find("input").prop("checked", true);
                }
                else {
                    $("#ContentPlaceHolder1_gvCompany tbody tr").find("td:eq(0)").find("input").prop("checked", false);
                }
            });
        });

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
            contactId = $("#ContentPlaceHolder1_hfCompanyId").val();
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
        // multi contact details add END
        function SaveAndClose() {
            flag = 1;
            //DuplicateCheckDynamicaly();
            $.when(PerformSave()).done(function () {
                if (flag == 1) {
                    if (typeof parent.FillForm === "function") {
                        var id = $("#ContentPlaceHolder1_hfCompanyId").val();
                        parent.FillForm(id);
                    }
                    if (typeof parent.CloseCompanyDialog === "function")
                        parent.CloseCompanyDialog();
                    if (typeof parent.CloseDialog === "function")
                        parent.CloseDialog();
                    if ($("#btnSave").val() == "Update and Close") {
                        $("#btnSave").val("Save And Close");
                        $("#btnSaveContinue").show();
                        $("#btnCancel").show();
                    }
                    PerformClearAction();
                    //$('#AddNewContactContaiiner').dialog('close');
                }
            });
            return false;
        }
        function SaveAndContinue() {
            //DuplicateCheckDynamicaly();
            PerformSave();
            return false;
        }
        function DuplicateCheckDynamicaly() {
            var comName = "";
            comName = $("#<%=txtCompanyName.ClientID %>").val();
            if (comName == "") {
                flag = 0;
                toastr.warning("Please Add Company Name");
                $("#<%=txtCompanyName.ClientID %>").focus();
                return false;
            }
            var id = $("#<%=hfCompanyId.ClientID %>").val();
            var IsUpdate = 0;
            if (id != 0) {
                IsUpdate = 1;
            }
            PageMethods.DuplicateCheckDynamicaly("CompanyName", comName, IsUpdate, id, IsDuplicateSucceed, IsDuplicateFailed);
            return false;
        }
        function PerformSave() {
            var comOwnerId = 0, parentComId = 0, comTypeId = 0, ownershipId = 0, industryId = 0, lifeCycleStId = 0, annualRev = 0, noOfEmp = 0, discount = 0, creditLim = 0;
            var billStreet = "", billPost = "", billCityId = 0, billStateId = 0, billCountryId = 0;
            var shipStreet = "", shipPost = "", shipCityId = 0, shipStateId = 0, shipContryId = 0, billingLocationId = 0, shippingLocationId = 0;
            var comName = "", phone = "", email = "", lifeCycleStage = "", companyOwner = "", billingAddress = "", shippingAddress = "";
            comName = $("#<%=txtCompanyName.ClientID %>").val();
            if (comName == "") {
                flag = 0;
                toastr.warning("Please Add Company Name");
                $("#<%=txtCompanyName.ClientID %>").focus();
                return false;
            }
            phone = $("#<%=txtPhone.ClientID %>").val();
            email = $("#<%=txtEmail.ClientID %>").val();
            if (email != "") {
               <%-- if (!CommonHelper.IsValidEmail(email)) {
                    flag = 0;
                    toastr.warning("Please Insert Valid Email");
                    $("#<%=txtEmail.ClientID %>").focus();
                    return false;
                }--%>
            }
            comOwnerId = $("#<%=ddlCompanyOwner.ClientID %>").val();
            parentComId = $("#<%=ddlParentCompany.ClientID %>").val();
            if (parentComId == 0) {
                parentComId = -1;
            }
            companyOwner = $("#<%=ddlCompanyOwner.ClientID %>").find(":selected").text();
            
            ownershipId = $("#<%=ddlOwnership.ClientID %>").val();
            industryId = $("#<%=ddlIndustry.ClientID %>").val();
            lifeCycleStId = $("#<%=ddlLifeCycleStageId.ClientID %>").val();
            lifeCycleStage = $("#<%=ddlLifeCycleStageId.ClientID %>").find(":selected").text();
            if (lifeCycleStId == 0) {
                flag = 0;
                toastr.warning("Please Select Life Cycle Stage");
                $("#<%=ddlLifeCycleStageId.ClientID %>").focus();
                return false;
            }

            comTypeId = $("#<%=ddlCompanyType.ClientID %>").val();
            if (comTypeId == 0) {
                flag = 0;
                toastr.warning("Please Select Company Type");
                $("#<%=ddlCompanyType.ClientID %>").focus();
                return false;
            }

            var ticketNo = $("#<%=txtTicketNo.ClientID %>").val();
            var annualRev = $("#<%=txtAnnualRevenue.ClientID %>").val();
            var noOfEmp = $("#<%=txtNoOfEmployee.ClientID %>").val();
            var discount = $("#<%=txtDiscountPercent.ClientID %>").val();
            var creditLim = $("#<%=txtCreditLimit.ClientID %>").val();
            var fax = $("#<%=txtFax.ClientID %>").val();
            var web = $("#<%=txtWebsite.ClientID %>").val();
            var remarks = $("#<%=txtRemarks.ClientID %>").val();
            var branchCode = $("#<%=txtBranchCode.ClientID %>").val();
            billStreet = $("#<%=txtBillingLocation.ClientID %>").val();
            billPost = $("#<%=txtBillingPostCode.ClientID %>").val();
            billCityId = $("#<%=hfBillingCityId.ClientID %>").val();
            billStateId = $("#<%=hfBillingStateId.ClientID %>").val();
            billCountryId = $("#<%=hfBillingCountryId.ClientID %>").val();
            shipStreet = $("#<%=txtShippingLocation.ClientID %>").val();
            shipPost = $("#<%=txtShippingPostCode.ClientID %>").val();
            shipContryId = $("#<%=hfShippingCountryId.ClientID %>").val();
            shipStateId = $("#<%=hfShippingStateId.ClientID %>").val();
            shipCityId = $("#<%=hfShippingCityId.ClientID %>").val();
            billingLocationId = $("#<%=hfBillingLocationId.ClientID %>").val();
            shippingLocationId = $("#<%=hfShippingLocationId.ClientID %>").val();
            billingAddress = $("#<%=txtBillingAddress.ClientID %>").val();
            shippingAddress = $("#<%=txtShippingAddress.ClientID %>").val();

            var id = $("#<%=hfCompanyId.ClientID %>").val();
            var hfRandom = $("#<%=RandomProductId.ClientID %>").val();
            var deletedDocuments = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();
            var IsUpdate = 0;
            if (id != 0) {
                IsUpdate = 1;
            }
            var isDuplicate = CheckDuplicate(comName, IsUpdate, id);
            if (isDuplicate) {
                flag = 0;
                toastr.warning("Duplicate Company Name");
                $("#<%=txtCompanyName.ClientID %>").focus();
                return false;
            }
            if (annualRev == "") {
                annualRev = 0;
            } if (discount == "") {
                discount = 0;
            } if (creditLim == "") {
                creditLim = 0;
            }

            var companyGrid = $("#ContentPlaceHolder1_gvCompany tbody tr").length;
            var crmCompanyIds = [];
            if (companyGrid > 0) {

                crmCompanyIds = $("input:checkbox:checked", "#ContentPlaceHolder1_gvCompany").map(function () {
                    return parseInt($(this).val());
                }).get();

            }

            var compayBO = {
                CompanyId: id,
                AncestorId: parentComId,
                CompanyOwnerName: companyOwner,
                CompanyOwnerId: comOwnerId,
                OwnershipId: ownershipId,
                CompanyName: comName,
                CompanyType: comTypeId,
                IndustryId: industryId,
                TicketNumber: ticketNo,
                DiscountPercent: discount,
                CreditLimit: creditLim,
                AnnualRevenue: annualRev,
                NumberOfEmployee: noOfEmp,
                LifeCycleStage: lifeCycleStage,
                LifeCycleStageId: lifeCycleStId,
                EmailAddress: email,
                ContactNumber: phone,
                WebAddress: web,
                Fax: fax,
                Remarks: remarks,
                BranchCode: branchCode,
                BillingLocationId: billingLocationId,
                BillingStreet: billStreet,
                BillingPostCode: billPost,
                BillingCountryId: billCountryId,
                BillingCityId: billCityId,
                BillingStateId: billStateId,
                ShippingLocationId: shippingLocationId,
                ShippingStreet: shipStreet,
                ShippingPostCode: shipPost,
                ShippingCountryId: shipContryId,
                ShippingCityId: shipCityId,
                ShippingStateId: shipStateId,
                BillingAddress: billingAddress,
                ShippingAddress: shippingAddress
            }
            var check = CheckLifeCycleStageValidation(compayBO);
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
            var label = "", content = "", parentId = "0", DetailsId = "0";
            var numberGrid = $("#PhoneGrid tbody tr").length;
            var emailGrid = $("#EmailGrid tbody tr").length;
            var fax = $("#FaxGrid tbody tr").length;
            var webGrid = $("#WebsiteGrid tbody tr").length;
            
            if (numberGrid > 0) {
                $("#PhoneGrid tbody tr").each(function (index, item) {
                    label = $.trim($(item).find("td:eq(0)").text());
                    content = $.trim($(item).find("td:eq(1)").text());
                    parentId = $.trim($(item).find("td:eq(3)").text());
                    DetailsId = $.trim($(item).find("td:eq(4)").text());
                    if (DetailsId == "0") {
                        AddNewContactItems(label, content, parentId, DetailsId, "Number", "Company");
                    }
                });
            }
            if (emailGrid > 0) {
                $("#EmailGrid tbody tr").each(function (index, item) {
                    label = $.trim($(item).find("td:eq(0)").text());
                    content = $.trim($(item).find("td:eq(1)").text());
                    parentId = $.trim($(item).find("td:eq(3)").text());
                    DetailsId = $.trim($(item).find("td:eq(4)").text());
                    if (DetailsId == "0") {
                        AddNewContactItems(label, content, parentId, DetailsId, "Email", "Company");
                    }
                });
            }
            if (fax > 0) {
                $("#FaxGrid tbody tr").each(function (index, item) {
                    label = $.trim($(item).find("td:eq(0)").text());
                    content = $.trim($(item).find("td:eq(1)").text());
                    parentId = $.trim($(item).find("td:eq(3)").text());
                    DetailsId = $.trim($(item).find("td:eq(4)").text());
                    if (DetailsId == "0") {
                        AddNewContactItems(label, content, parentId, DetailsId, "Fax", "Company");
                    }
                });
            }
            if (webGrid > 0) {
                $("#WebsiteGrid tbody tr").each(function (index, item) {
                    label = $.trim($(item).find("td:eq(0)").text());
                    content = $.trim($(item).find("td:eq(1)").text());
                    parentId = $.trim($(item).find("td:eq(3)").text());
                    DetailsId = $.trim($(item).find("td:eq(4)").text());
                    if (DetailsId == "0") {
                        AddNewContactItems(label, content, parentId, DetailsId, "Website", "Company");
                    }
                });
            }
            
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/Company.aspx/SaveUpdateCompany',
                data: JSON.stringify({ CompanyBO: compayBO, CRMCompanyIds: crmCompanyIds, hfRandom: hfRandom, deletedDocument: deletedDocuments, newlyAddedItem: newlyAddedItem, deleteDbItem: deleteDbItem }),
                dataType: "json",
                async: false,
                success: function (data) {
                    OnSaveCompanySucceed(data.d);
                },
                error: function (result) {
                    OnSaveCompanyFailed(result.d);
                }
            });
            //PageMethods.SaveUpdateCompany(compayBO, hfRandom, deletedDocuments, OnSaveCompanySucceed, OnSaveCompanyFailed);
            //return false;
        }
        function IsDuplicateSucceed(result) {
            if (result > 0) {
                flag = 0;
                toastr.warning("Duplicate Company Name");
                $("#<%=txtCompanyName.ClientID %>").focus();
                return false;
            }
            else {
                PerformSave();
            }
        }
        function CheckDuplicate(comName, IsUpdate, id) {
            var returnInfo = false;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/Company.aspx/DuplicateCheckDynamicaly',
                data: "{'fieldName':'" + "CompanyName" + "', 'fieldValue':'" + comName.trim() + "','isUpdate':'" + IsUpdate + "', 'pkId':'" + id + "'}",
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
                    CommonHelper.AlertMessage(result.d.AlertMessage);
                }
            });
            return returnInfo;
        }
        function IsDuplicateFailed(error) {
            toastr.warning("Error Occured");
            return false;
        }
        function PerformSaveUpdate() {
        }
        function OnSaveCompanySucceed(result) {
            //PerformClearAction();
            if (result.IsSuccess) {
                parent.ShowAlert(result.AlertMessage);
                if (typeof parent.GridPaging === "function")
                    parent.GridPaging(1, 1);
                PerformClearAction();
                LoadParentCompanyAfterSave();
            }
            else {
                if (result.DataStr)
                    $("#ContentPlaceHolder1_ddlLifeCycleStageId").focus();
                flag = 0;
                parent.ShowAlert(result.AlertMessage);
            }
            return false;
        }
        function OnSaveCompanyFailed(error) {
            CommonHelper.AlertMessage(error.AlertMessage);
            return false;
        }
        function CheckLifeCycleStageValidation(contactInformationBO) {
            var returnInfo = false;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/Company.aspx/CheckLifeCycleStageValidation',
                data: JSON.stringify({ CompanyBO: contactInformationBO }),
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
        function IsValidPhone(inputtxt) {
            var regex = /^[+]*[(]{0,1}[0-9]{1,3}[)]{0,1}[-\s\./0-9]*$/;
            if (!regex.test(inputtxt)) {
                return false;
            } else {
                return true;
            }
        }
        function LoadParentCompanyAfterSave() {
            $.ajax({
                type: "POST",
                url: "Company.aspx/LoadParentCompanyAfterSave",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    PopulateControlWithValueNTextField(response.d, $("#ContentPlaceHolder1_ddlParentCompany"), $("#<%=CommonDropDownHiddenField.ClientID %>").val(), "CompanyName", "CompanyId");
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
            }
            function FillForm(id) {
                CommonHelper.SpinnerOpen();
                PageMethods.FillForm(id, OnFillFormSucceed, OnFillFormFailed);
                return false;
            }
            function OnFillFormSucceed(result) {
                PerformClearAction();
                CommonHelper.SpinnerClose();
                $("#ContentPlaceHolder1_btnSaveClose").val("Update And Close");
                $("#ContentPlaceHolder1_btnSaveContinue").hide();
                $("#ContentPlaceHolder1_btnClear").show();
                var numbers = result[0].Numbers;
                var emails = result[0].Emails;
                var fax = result[0].Fax;
                var websites = result[0].Websites;
                var companyInfo = result[0].GuestCompany;
                

                $("#<%=txtCompanyName.ClientID %>").val(companyInfo.CompanyName);

                if (companyInfo.CompanyOwnerId == 0 || companyInfo.CompanyOwnerId == 1) {
                    $("#<%=ddlCompanyOwner.ClientID %>").val("0").trigger('change');
                }
                else {
                    $("#<%=ddlCompanyOwner.ClientID %>").val(companyInfo.CompanyOwnerId).trigger('change');
                }
                if (companyInfo.AncestorId < 0) {
                    $("#<%=ddlParentCompany.ClientID %>").val("0").trigger('change');
                }
                else if (companyInfo.AncestorId == companyInfo.CompanyId) {
                    $("#<%=ddlParentCompany.ClientID %>").val("0").trigger('change');
                }
                else {
                    $("#<%=ddlParentCompany.ClientID %>").val(companyInfo.AncestorId).trigger('change');
                }

            $("#<%=ddlCompanyType.ClientID %>").val(companyInfo.CompanyType).trigger('change');
                $("#<%=ddlOwnership.ClientID %>").val(companyInfo.OwnershipId).trigger('change');
                $("#<%=ddlIndustry.ClientID %>").val(companyInfo.IndustryId).trigger('change');
                $("#<%=ddlLifeCycleStageId.ClientID %>").val(companyInfo.LifeCycleStageId).trigger('change');
                $("#<%=txtTicketNo.ClientID %>").val(companyInfo.TicketNumber);
                $("#<%=txtAnnualRevenue.ClientID %>").val(companyInfo.AnnualRevenue);
                $("#<%=txtNoOfEmployee.ClientID %>").val(companyInfo.NumberOfEmployee);
                $("#<%=txtDiscountPercent.ClientID %>").val(companyInfo.DiscountPercent);
                $("#<%=txtCreditLimit.ClientID %>").val(companyInfo.CreditLimit);
                $("#<%=txtRemarks.ClientID %>").val(companyInfo.Remarks);
                $("#<%=txtBranchCode.ClientID %>").val(companyInfo.BranchCode);
                $("#<%=hfBillingLocationId.ClientID %>").val(companyInfo.BillingLocationId);
                $("#<%=txtBillingLocation.ClientID %>").val(companyInfo.BillingStreet);
                $("#<%=txtBillingPostCode.ClientID %>").val(companyInfo.BillingPostCode);
                $("#<%=txtBillingPostCode.ClientID %>").val(companyInfo.BillingPostCode);
                $("#<%=hfBillingCityId.ClientID %>").val(companyInfo.BillingCityId);
                $("#<%=txtBillingCity.ClientID %>").val(companyInfo.BillingCity);
                $("#<%=hfBillingStateId.ClientID %>").val(companyInfo.BillingStateId);
                $("#<%=txtBillingState.ClientID %>").val(companyInfo.BillingState);
                $("#<%=hfBillingCountryId.ClientID %>").val(companyInfo.BillingCountryId);
                $("#<%=txtBillingCountry.ClientID %>").val(companyInfo.BillingCountry);
                $("#<%=hfShippingLocationId.ClientID %>").val(companyInfo.ShippingLocationId);
                $("#<%=txtShippingLocation.ClientID %>").val(companyInfo.ShippingStreet);
                $("#<%=txtShippingPostCode.ClientID %>").val(companyInfo.ShippingPostCode);
                $("#<%=hfShippingCityId.ClientID %>").val(companyInfo.ShippingCityId);
                $("#<%=txtShippingCity.ClientID %>").val(companyInfo.ShippingCity);
                $("#<%=hfShippingStateId.ClientID %>").val(companyInfo.ShippingStateId);
                $("#<%=txtShippingState.ClientID %>").val(companyInfo.ShippingState);
                $("#<%=hfShippingCountryId.ClientID %>").val(companyInfo.ShippingCountryId);
                $("#<%=txtShippingCountry.ClientID %>").val(companyInfo.ShippingCountry);
                $("#<%=txtBillingAddress.ClientID %>").val(companyInfo.BillingAddress);
                $("#<%=txtShippingAddress.ClientID %>").val(companyInfo.ShippingAddress);

                $("#<%=hfCompanyId.ClientID %>").val(companyInfo.CompanyId);
                if (numbers.length > 0) {
                    for (var i = 0; i < numbers.length; i++) {
                        LoadContactTable(numbers[i].Value, numbers[i].Title, "PhoneGrid", "", "", numbers[i].ParentId, numbers[i].DetailsId);
                    }
                }
                if (emails.length > 0) {
                    for (var i = 0; i < emails.length; i++) {
                        LoadContactTable(emails[i].Value, emails[i].Title, "EmailGrid", "", "", emails[i].ParentId, emails[i].DetailsId);
                    }
                }
                if (fax.length > 0) {
                    for (var i = 0; i < fax.length; i++) {
                        LoadContactTable(fax[i].Value, fax[i].Title, "FaxGrid", "", "", fax[i].ParentId, fax[i].DetailsId);
                    }
                }
                if (websites.length > 0) {
                    for (var i = 0; i < websites.length; i++) {
                        LoadContactTable(websites[i].Value, websites[i].Title, "WebsiteGrid", "", "", websites[i].ParentId, websites[i].DetailsId);
                    }
                }

                var CRMCompanyIds = result[0].CRMCompanyIds;
                console.log(CRMCompanyIds);

                $("input:checkbox", "#ContentPlaceHolder1_gvCompany").map(function () {
                    var checkBoxValue = $(this).val();
                    var checkBox = this;
                    
                    CRMCompanyIds.map(function (companyId) {
                        console.log("Array Id: " + companyId);
                        if (parseInt(companyId) == parseInt(checkBoxValue)) {
                            $(checkBox).prop('checked', true);
                        }
                    });
                    //return parseInt($(this).val());
                });

                UploadComplete();
            }
            function OnFillFormFailed(error) {
                toastr.warning("Error Happened");
            }
            function PerformClearAction() {
                $("#ContentPlaceHolder1_btnSaveClose").val("Save And Close");
                $("#ContentPlaceHolder1_btnSaveContinue").val("Save And Continue");
                $("#ContentPlaceHolder1_btnClear").show();
                $("#ContentPlaceHolder1_btnSaveContinue").show();
                $("#<%=txtCompanyName.ClientID %>").val("");
                $("#<%=txtPhone.ClientID %>").val("");
                $("#<%=txtEmail.ClientID %>").val("");
                $("#<%=ddlParentCompany.ClientID %>").val("0").trigger('change');
                $("#<%=ddlCompanyType.ClientID %>").val("0").trigger('change');
                $("#<%=ddlOwnership.ClientID %>").val("0").trigger('change');
                $("#<%=ddlIndustry.ClientID %>").val("0").trigger('change');
                $("#<%=ddlLifeCycleStageId.ClientID %>").val("0").trigger('change');
                $("#<%=txtTicketNo.ClientID %>").val("");
                $("#<%=txtAnnualRevenue.ClientID %>").val("");
                $("#<%=txtNoOfEmployee.ClientID %>").val("");
                $("#<%=txtDiscountPercent.ClientID %>").val("");
                $("#<%=txtCreditLimit.ClientID %>").val("");
                $("#<%=txtFax.ClientID %>").val("");
                $("#<%=txtWebsite.ClientID %>").val("");
                $("#<%=txtRemarks.ClientID %>").val("");
                $("#<%=txtBranchCode.ClientID %>").val("");
                $("#<%=txtBillingLocation.ClientID %>").val("");
                $("#<%=txtBillingPostCode.ClientID %>").val("");
                $("#<%=hfBillingCityId.ClientID %>").val("0");
                $("#<%=txtBillingCity.ClientID %>").val("");
                $("#<%=hfBillingStateId.ClientID %>").val("0");
                $("#<%=txtBillingState.ClientID %>").val("");
                $("#<%=hfBillingCountryId.ClientID %>").val("0");
                $("#<%=txtBillingCountry.ClientID %>").val("");
                $("#<%=txtShippingLocation.ClientID %>").val("");
                $("#<%=txtShippingPostCode.ClientID %>").val("");
                $("#<%=hfShippingCityId.ClientID %>").val("0");
                $("#<%=txtShippingCity.ClientID %>").val("");
                $("#<%=hfShippingStateId.ClientID %>").val("0");
                $("#<%=txtShippingState.ClientID %>").val("");
                $("#<%=hfShippingCountryId.ClientID %>").val("0");
                $("#<%=txtShippingCountry.ClientID %>").val("");
                $("#<%=hfCompanyId.ClientID %>").val("0");
                $("#<%=hfGuestDeletedDoc.ClientID %>").val("");
                $("#<%=hfBillingLocationId.ClientID %>").val("0");
                $("#<%=hfShippingLocationId.ClientID %>").val("0");
                $("#<%=txtBillingAddress.ClientID %>").val("");
                $("#<%=txtShippingAddress.ClientID %>").val("");

                $("#PhoneGrid tbody tr").remove();
                $("#EmailGrid tbody tr").remove();
                $("#FaxGrid tbody tr").remove();
                $("#WebsiteGrid tbody tr").remove();
                $("#PhoneGrid").hide();
                $("#EmailGrid").hide();
                $("#FaxGrid").hide();
                $("#WebsiteGrid").hide();
                $("#<%=ddlCompanyOwner.ClientID %>").val("0").trigger('change');
                return false;
            }
            function LoadDocUploader() {
                var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
                var path = "/SalesAndMarketing/Images/Company/";
                var category = "CompanyDocument";
                var iframeid = 'frmPrint';
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
            function AttachFile() {
                $("#contactdocuments").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 900,
                    closeOnEscape: true,
                    resizable: false,
                    title: "Company Documents",
                    show: 'slide'
                });
            }
            function UploadComplete() {
                var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
                var id = +$("#ContentPlaceHolder1_hfCompanyId").val();
                var deletedDoc = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();
                PageMethods.LoadCompanyDocument(id, randomId, deletedDoc, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
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
                    guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                    guestDocumentTable += "</td>";
                    guestDocumentTable += "</tr>";
                }
                guestDocumentTable += "</table>";
                $("#ContactDocumentInfo").html(guestDocumentTable);
            }
            function OnLoadDocumentFailed(error) {
                toastr.error(error.get_message());
            }
            function DeleteDoc(docId, rowIndex) {
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
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="RandomProductId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfGuestDeletedDoc" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfcontactDetailsId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBillingCountryId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBillingStateId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBillingCityId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBillingLocationId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfShippingCountryId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfShippingStateId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfShippingCityId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfShippingLocationId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsGLCompanyWiseCRMCompanyDifferent" runat="server" Value="0"></asp:HiddenField>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-md-2">Account Manager</label>
                    <div class="col-sm-10">
                        <asp:DropDownList ID="ddlCompanyOwner" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-md-2">Parent Company</label>
                    <div class="col-sm-10">
                        <asp:DropDownList ID="ddlParentCompany" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label for="CompanyName" class="control-label col-md-2 required-field">Company Name</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-md-2 required-field">Life Cycle Stage</label>
                    <div class="col-sm-4">
                        <asp:DropDownList ID="ddlLifeCycleStageId" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                    <label class="control-label col-md-2 ">Ownership</label>
                    <div class="col-sm-4">
                        <asp:DropDownList ID="ddlOwnership" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-md-2 required-field">Company Type</label>
                    <div class="col-sm-4">
                        <asp:DropDownList ID="ddlCompanyType" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                    <label class="control-label col-md-2 ">Industry</label>
                    <div class="col-sm-4">
                        <asp:DropDownList ID="ddlIndustry" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" style="display: none;">
                    <label class="control-label col-md-2 ">Ticket Number</label>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtTicketNo" runat="server" placeholder="TN00000001" CssClass="form-control" TabIndex="2"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div id="DivAnnualRevenue" runat="server">
                        <label class="control-label col-md-2 ">Annual Revenue</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtAnnualRevenue" runat="server" placeholder="$" CssClass="form-control quantitydecimal" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                    <div id="DivNoOfEmployee" runat="server">
                        <label class="control-label col-md-2 ">No. of Employee</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtNoOfEmployee" runat="server" CssClass="form-control quantitydecimal" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div id="DivDiscountPercentage" runat="server">
                        <label for="DiscountPercent" class="control-label col-md-2">Discount (%)</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtDiscountPercent" runat="server" CssClass="form-control quantitydecimal" TabIndex="10">0</asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div id="DivCreditLimit" runat="server">
                        <label for="CreditLimit" class="control-label col-md-2">Credit Limit</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtCreditLimit" runat="server" CssClass="form-control quantitydecimal" TabIndex="10">0</asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <fieldset>
                        <legend>Contact Information</legend>
                        <div class="form-group">
                            <label class="control-label col-md-2">Phone</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"
                                    TabIndex="2"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlPhoneTitle" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                                <div style="display: none;">
                                    <asp:TextBox ID="txtPhoneLabel" runat="server" CssClass="form-control" placeholder="Label" TabIndex="2" Style="display: none;"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-1">
                                <input type="button" id="btnAddPhone" class="TransactionalButton btn btn-primary btn-sm" value="+" title="Add Phone" />
                            </div>
                            <div class="col-md-4">
                                <table id="PhoneGrid" class="table table-bordered table-condensed table-responsive"
                                    style="width: 100%; display: none">
                                    <thead>
                                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                            <th style="width: 30%;">Label
                                            </th>
                                            <th style="width: 60%;">Phone
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
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"
                                    TabIndex="2"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlEmailTitle" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                                <div style="display: none;">
                                    <asp:TextBox ID="txtEmailLabel" runat="server" CssClass="form-control" placeholder="Label" TabIndex="2" Style="display: none;"></asp:TextBox>
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
                            <label class="control-label col-md-2 ">Fax</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtFax" runat="server" CssClass="form-control"
                                    TabIndex="2"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlFaxTitle" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                                <div style="display: none;">
                                    <asp:TextBox ID="txtFaxLabel" runat="server" CssClass="form-control" placeholder="Label" TabIndex="2" Style="display: none;"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-1">
                                <input type="button" id="btnAddFax" class="TransactionalButton btn btn-primary btn-sm" value="+" title="Add Fax" />
                            </div>
                            <div class="col-md-4">
                                <table id="FaxGrid" class="table table-bordered table-condensed table-responsive"
                                    style="width: 100%; display: none">
                                    <thead>
                                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                            <th style="width: 30%;">Label
                                            </th>
                                            <th style="width: 60%;">Fax
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
                    </fieldset>
                </div>
                <div class="form-group">
                    <fieldset>
                        <legend>Billing Address</legend>
                        <div class="form-group">
                            <label for="Address" class="control-label col-md-2">Address</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtBillingAddress" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="9"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-2 ">Country</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtBillingCountry" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-2 ">State/ Province/ District</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtBillingState" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-2 ">City</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtBillingCity" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" runat="server" id="BillingAreaDiv">
                            <label class="control-label col-md-2 ">Area</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtBillingLocation" runat="server" CssClass="form-control"
                                    TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-2 ">Postal Code</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtBillingPostCode" runat="server" CssClass="form-control"
                                    TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                    </fieldset>
                </div>
                <div class="form-group" id="DivCRMCompany" runat="server">
                    <fieldset>
                        <legend>Company</legend>
                        <div class="panel-body">
                            <asp:GridView ID="gvCompany" Width="100%" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                ForeColor="#333333" PageSize="500000" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCompanyId" runat="server" Text='<%#Eval("CompanyId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Create/Update" ItemStyle-Width="05%">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="ChkCreate" CssClass="ChkCreate" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%--<asp:CheckBox ID="chkIsSavePermission" CssClass="Chk_Create" runat="server" />--%>
                                            <input type="checkbox" id="chkIsSavePermission" class="Chk_Create" runat="server" value='<%#Eval("CompanyId") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Company" ItemStyle-Width="85%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvCompany" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblRecordNotFound" runat="server" Text="Record Not Found."></asp:Label>
                                </EmptyDataTemplate>
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#44545E" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#7C6F57" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </div>
                    </fieldset>
                </div>
                <div class="form-group" id="DivShippingAddress" runat="server">
                    <fieldset>
                        <legend>Shipping Address</legend>
                        <div class="form-group">
                            <label for="Address" class="control-label col-md-2">Address</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtShippingAddress" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="9"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-2 ">Country</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtShippingCountry" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-2 ">State/ Province/ District</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtShippingState" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-2 ">City</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtShippingCity" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" runat="server" id="ShippingAreaDiv">
                            <label class="control-label col-md-2 ">Area</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtShippingLocation" runat="server" CssClass="form-control"
                                    TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-2 ">Postal Code</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtShippingPostCode" runat="server" CssClass="form-control"
                                    TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                    </fieldset>
                </div>
                <div class="form-group">
                    <div id="BranchCodeDiv" runat="server">
                        <label for="BranchCode" class="control-label col-md-2">Branch Code</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtBranchCode" runat="server" CssClass="form-control" TabIndex="31"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label for="Remarks" class="control-label col-md-2">Remarks</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                            TabIndex="32"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="Attachment" class="control-label col-md-2">Attachment</label>
                    <div class="col-md-10">
                        <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Attach" onclick="LoadDocUploader()" />
                    </div>
                </div>
                <div class="form-group">
                    <div id="ContactDocumentInfo">
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSaveClose" runat="server" Text="Save And Close" OnClientClick="javascript:return SaveAndClose();"
                            CssClass="TransactionalButton btn btn-primary btn-sm" />
                        <asp:Button ID="btnSaveContinue" runat="server" Text="Save And Continue" OnClientClick="javascript:return SaveAndContinue();"
                            CssClass="TransactionalButton btn btn-primary btn-sm" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return PerformClearAction();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
