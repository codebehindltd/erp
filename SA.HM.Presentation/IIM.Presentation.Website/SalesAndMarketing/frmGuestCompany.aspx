<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmGuestCompany.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.frmGuestCompany" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var txtDiscountPercent = '<%=txtDiscountPercent.ClientID%>'
            var lblMessage = '<%=lblMessage.ClientID%>'

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Sales & Marketing</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Company Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            var companyId = $("#<%=hfCompanyId.ClientID %>").val();
            if (companyId != "") {
                UploadComplete();
            }

            var criteria = $("#<%=ddlCriteria.ClientID %>").val();
            if (criteria == "0") {
                $("#SearchDateDiv").hide();
            }
            else {
                $("#SearchDateDiv").show();
            }

            $("#ContentPlaceHolder1_ddlCriteria").change(function () {
                var criteria = $("#<%=ddlCriteria.ClientID %>").val();
                if (criteria == "0") {
                    $("#SearchDateDiv").hide();
                }
                else {
                    $("#SearchDateDiv").show();
                }
            });

            $("#ContentPlaceHolder1_ddlSrcCompanyType").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlSrcLifeCycleStage").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlSrcOwnerId").select2({
                tags: "true",
                //placeholder: "Select an option",
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
                //placeholder: "Select an option",
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

            $('#ContentPlaceHolder1_txtSearchFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                // defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchToDate').datepicker("option", "minDate", selectedDate);
                }
            });
            //}).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtSearchToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtBillingCountry").blur(function () {
                if ($("#ContentPlaceHolder1_txtBillingCountry").val() == "") {
                    $("#ContentPlaceHolder1_hfBillingCountryId").val(0);
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
            $("#ContentPlaceHolder1_txtSCompanyName").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/frmGuestCompany.aspx/GetCompanyByAutoSearch',
                        data: JSON.stringify({ searchTerm: request.term }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
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
                    $("#ContentPlaceHolder1_hfSearchCompanyId").val(ui.item.value);
                }
            });            

            $("#ContentPlaceHolder1_txtBillingState").blur(function () {
                if ($("#ContentPlaceHolder1_txtBillingState").val() == "") {
                    $("#ContentPlaceHolder1_hfBillingStateId").val(0);
                }
            });

            $("#ContentPlaceHolder1_txtBillingState").autocomplete({
                source: function (request, response) {
                    var billingCountry = $("#ContentPlaceHolder1_hfBillingCountryId").val();
                    if (billingCountry == 0) {
                        toastr.warning("Please Select Country");
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

            $("#ContentPlaceHolder1_txtBillingCity").blur(function () {
                if ($("#ContentPlaceHolder1_txtBillingCity").val() == "") {
                    $("#ContentPlaceHolder1_hfBillingCityId").val(0);
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

            $("#ContentPlaceHolder1_txtBillingLocation").blur(function () {
                if ($("#ContentPlaceHolder1_txtBillingLocation").val() == "") {
                    $("#ContentPlaceHolder1_hfBillingLocationId").val(0);
                }
            });

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

            $("#addEmail").click(function () {
                var phone = $("#txtEmail").val();
                if (phone == "") {
                    toastr.warning("Please add one element first");
                    $("#txtEmail").focus();
                    return false;
                }
                else {
                    AddEmails();
                }

                return false;
            });
            $("#addWeb").click(function () {
                var phone = $("#txtaddedWeb").val();
                if (phone == "") {
                    toastr.warning("Please add one element first");
                    $("#txtaddedWeb").focus();
                    return false;
                }
                else {
                    AddWebs();
                }

                return false;
            });
            $("#addPhone").click(function () {
                var phone = $("#txtPhone").val();
                if (phone == "") {
                    toastr.warning("Please add one element first");
                    $("#txtPhone").focus();
                    return false;
                }
                else {
                    AddPhone();
                }

                return false;
            });
            $("#addFax").click(function () {
                var phone = $("#txtFax").val();
                if (phone == "") {
                    toastr.warning("Please add one element first");
                    $("#txtFax").focus();
                    return false;
                }
                else {
                    AddFax();
                }

                return false;
            });

            if (companyId != "") {
                Edit();
            }
            //LoadCompanyForSearch(1, 1);
            //$('#btnSearch').trigger('click');
            //__doPostBack('btnSearch','OnClick');
            <%--var clickButton = document.getElementById("<%=btnSearch.ClientID %>");
            clickButton.click();--%>

        });
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }
        function Edit() {
            var strEmail = $("#<%=hfEmail.ClientID %>").val();
            var tempStr = "";

            var hfemailRow = new Array();
            var hfemailCol = new Array();
            var k = 0;
            hfemailRow = strEmail.split("~").filter(x => x);
            for (var i = 0; i < hfemailRow.length; i++) {
                tempStr += hfemailRow[i].split(",").filter(x => x) + ",";
            }
            hfemailCol = tempStr.split(",").filter(x => x);
            var j = 1;

            if (companyId = ! "" && hfemailRow.length > 0) {
                for (var i = 0; i < hfemailRow.length; i++) {
                    if (i == 0) {
                        $("#txtEmailId").val(hfemailCol[i]);
                        $("#txtEmail").val(hfemailCol[i + 1]);
                    }
                    else {
                        AddEmails(hfemailCol[++j], hfemailCol[++j]);
                        //$("#txtaddedEmailId").val(hfemailCol[++j]);
                        //$("#txtaddedEmail").val(hfemailCol[++j]);
                    }
                    //j++;
                }
            }
        }
        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {

            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {
            UploadComplete();
            $("#<%=txtCompanyName.ClientID %>").val(result.CompanyName);
            $("#<%=txtCompanyAddress.ClientID %>").val(result.CompanyAddress);
            $("#<%=txtEmailAddress.ClientID %>").val(result.EmailAddress);
            $("#<%=txtWebAddress.ClientID %>").val(result.WebAddress);
            $("#<%=txtContactNumber.ClientID %>").val(result.ContactNumber);
            $("#<%=txtContactPerson.ClientID %>").val(result.ContactPerson);
            $("#<%=txtContactDesignation.ClientID %>").val(result.ContactDesignation);
            $("#<%=txtRemarks.ClientID %>").val(result.Remarks);
            $("#<%=txtDiscountPercent.ClientID %>").val(result.DiscountPercent);
            $("#<%=txtCompanyId.ClientID %>").val(result.CompanyId);
            $("#<%=txtNodeId.ClientID %>").val(result.NodeId);
            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewGuestCompany').hide("slow");
            $('#EntryPanel').show("slow");
        }

        function OnFillFormObjectFailed(error) {
            alert(error.get_message());
        }

        //For Delete-------------------------        
        function PerformDeleteAction(actionId) {
            var answer = confirm("Do you want to delete this record?")
            if (answer) {
                PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            }
            return false;
        }

        function OnDeleteObjectSucceeded(result) {
            window.location = "frmGuestCompany.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            alert(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=lblMessage.ClientID %>").text('');
            $("#<%=txtCompanyName.ClientID %>").val('');
            $("#<%=txtCompanyAddress.ClientID %>").val('');
            $("#<%=txtEmailAddress.ClientID %>").val('');
            $("#<%=txtWebAddress.ClientID %>").val('');
            $("#<%=txtContactNumber.ClientID %>").val('');
            $("#<%=txtContactDesignation.ClientID %>").val('');
            $("#<%=txtContactPerson.ClientID %>").val('');
            $("#<%=txtRemarks.ClientID %>").val('');
            $("#<%=txtDiscountPercent.ClientID %>").val('0');
            $("#<%=txtCompanyId.ClientID %>").val('');
            $("#<%=txtNodeId.ClientID %>").val('');
            $("#<%=txtTelephoneNumber.ClientID %>").val('');
            $("#<%=txtContactDesignation.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewGuestCompany').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewGuestCompany').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }
        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }
        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewGuestCompany').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewGuestCompany').hide("slow");
        }
        //$(function () {
        //    $("#myTabs").tabs();
        //});

        function LoadImageUploader() {
            $("#popUpImage").dialog({
                width: 650,
                height: 300,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "", // TODO add title
                show: 'slide'
            });
            return false;
        }

        function ConfirmEdit(companyName, id) {
            if (!confirm("Do you want to edit - " + companyName + "?")) {
                return false;
            }
            FillFormEdit(id, companyName);

        }

        function AddEmails(id, value) {
            var div = document.createElement('div');
            //var div = "";
            var count = jQuery("input[id='txtaddedEmail']").length;

            if (count > 0) {
                div.innerHTML = '<div class="col-md-10" style="padding: 0;"> <input style="margin-top: 5px" class="form-control" id="txtaddedEmail" name="DynamicEmail" type="text" value= "' + value + '"/> <input style="margin-top: 5px; display:none" class="form-control" id="txtaddedEmailId" type="text" value="' + id + '"/></div>' +
                    '<div class="col-md-1"> <input class="img-rounded" type="image" src="../Images/delete.png" style="padding-top:5px;margin-left: 5px; position:center; margin-top:5px;" onclick="RemoveEmailTextBox(this)" /></div>';
            } else {
                div.innerHTML = '<div class="col-md-10" style="padding: 0;"> <input style="margin-top: 5px" class="form-control" id="txtaddedEmail" name="DynamicEmail" type="text" value= "' + value + '" /><input style="margin-top: 5px; display:none" class="form-control" id="txtaddedEmailId"  type="text"value="' + id + '"/></div>' +
                    '<div class="col-md-1"> <input class="img-rounded" type="image" src="../Images/delete.png" style="padding-top:5px;margin-left: 5px; position:center; margin-top:5px;" onclick="RemoveEmailTextBox(this)" /></div>';
            }

            document.getElementById("TextBoxEmailContainer").appendChild(div);
            return false;
        }

        function RemoveEmailTextBox(div) {

            var parent = div.parentNode.parentNode.parentNode.childNodes[1].childNodes[0];
            var child1 = parent.childNodes[1]; //val
            var child2 = parent.childNodes[2]; //id

            //var subChild1 = child.childNodes[1].defaultValue; //value
            //var subChild2 = child.childNodes[2].defaultValue; //id
            var subChild1 = child1.defaultValue; //value
            var subChild2 = child2.defaultValue; //id
            var deleteIds = "";
            document.getElementById('ContentPlaceHolder1_hfEmailDelete').value += subChild2 + ",";
            //$("#ContentPlaceHolder1_hfEmailDelete").val();
            //var child2 = parent.childNodes[2];
            document.getElementById("TextBoxEmailContainer").removeChild(div.parentNode.parentNode);
            return false;
        }
        //function AddWebs() {
        //    var div = document.createElement('div');
        //    var count = jQuery("input[id='txtaddedWeb']").length;

        //    if (count > 0) {
        //        div.innerHTML = '<div class="col-md-10" style="padding: 0;"> <input style="margin-top: 5px" class="form-control" id="txtaddedWeb" name="DynamicWeb" type="text" value="" /></div>' +
        //            '<div class="col-md-1"> <input class="img-rounded" type="image" src="../Images/delete.png" style="padding-top:5px;margin-left: 5px; position:center; margin-top:5px;" onclick="RemoveWebTextBox(this)" /></div>';
        //    } else {
        //        div.innerHTML = '<div class="col-md-10" style="padding: 0;"> <input style="margin-top: 5px" class="form-control" id="txtaddedWeb" name="DynamicWeb" type="text" value="" /></div>' +
        //            '<div class="col-md-1"> <input class="img-rounded" type="image" src="../Images/delete.png" style="padding-top:5px;margin-left: 5px; position:center; margin-top:5px;" onclick="RemoveWebTextBox(this)" /></div>';
        //    }
        //    //<input style="margin-left: 5px; margin-top:5px" class = "btn btn-primary btn-sm " type="button" value="Remove" onclick = "RemoveWebTextBox(this)" />'
        //    //<input style="margin-left: 5px;" class = "btn btn-primary btn-sm" type="button" value="Remove" onclick = "RemoveWebTextBox(this)" /> 
        //    document.getElementById("TextBoxWebContainer").appendChild(div);
        //    return false;
        //}

        //function RemoveWebTextBox(div) {
        //    document.getElementById("TextBoxWebContainer").removeChild(div.parentNode.parentNode);
        //    return false;
        //}
        //function AddPhone() {
        //    var div = document.createElement('div');
        //    //var div = "";
        //    var count = jQuery("input[id='txtPhone']").length;

        //    if (count > 0) {
        //        div.innerHTML = '<div class="col-md-10" style="padding: 0;"> <input style="margin-top: 5px" class="form-control" id="txtPhone" name="DynamicPhone" type="text" value="" /></div>' +
        //            '<div class="col-md-1"> <input class="img-rounded" type="image" src="../Images/delete.png" style="padding-top:5px;margin-left: 5px; position:center; margin-top:5px;" onclick="RemovePhone(this)" /></div>';
        //    } else {
        //        div.innerHTML = '<div class="col-md-10" style="padding: 0;"> <input style="margin-top: 5px" class="form-control" id="txtPhone" name="DynamicPhone" type="text" value="" /></div>' +
        //            '<div class="col-md-1"> <input class="img-rounded" type="image" src="../Images/delete.png" style="padding-top:5px;margin-left: 5px; position:center; margin-top:5px;" onclick="RemovePhone(this)" /></div>';
        //    }

        //    document.getElementById("TextBoxPhoneContainer").appendChild(div);
        //    return false;
        //}
        //function RemovePhone(div) {
        //    document.getElementById("TextBoxPhoneContainer").removeChild(div.parentNode.parentNode);
        //    return false;
        //}
        //function AddFax() {
        //    var div = document.createElement('div');
        //    //var div = "";
        //    var count = jQuery("input[id='txtFax']").length;

        //    if (count > 0) {
        //        div.innerHTML = '<div class="col-md-10" style="padding: 0;"> <input style="margin-top: 5px" class="form-control" id="txtFax" name="DynamicFax" type="text" value="" /></div>' +
        //            '<div class="col-md-1"> <input class="img-rounded" type="image" src="../Images/delete.png" style="padding-top:5px;margin-left: 5px; position:center; margin-top:5px;" onclick="RemoveFax(this)" /></div>';
        //    } else {
        //        div.innerHTML = '<div class="col-md-10" style="padding: 0;"> <input style="margin-top: 5px" class="form-control" id="txtFax" name="DynamicFax" type="text" value="" /></div>' +
        //            '<div class="col-md-1"> <input class="img-rounded" type="image" src="../Images/delete.png" style="padding-top:5px;margin-left: 5px; position:center; margin-top:5px;" onclick="RemoveFax(this)" /></div>';
        //    }

        //    document.getElementById("TextBoxFaxContainer").appendChild(div);
        //    return false;
        //}
        //function RemoveFax(div) {
        //    document.getElementById("TextBoxFaxContainer").removeChild(div.parentNode.parentNode);
        //    return false;
        //}
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
        function ShowCompanyDocuments(companyId) {
            PageMethods.GetDocumentsByUserTypeAndUserId(companyId, OnLoadImagesSucceeded, OnLoadImagesFailed);
            return false;
        }
        function OnLoadImagesSucceeded(result) {
            $("#imageDiv").html(result);

            $("#companyDocuments").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Company Documents",
                show: 'slide'
            });

            return false;
        }
        function OnLoadImagesFailed(error) {
            toastr.error(error.get_message());
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

            // docc = guestDocumentTable;

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
        //function DeleteDoc(docId, rowIndex) {
        //    if (confirm("Want to delete?")) {
        //        $.ajax({
        //            type: "POST",
        //            contentType: "application/json; charset=utf-8",
        //            url: "../../../SalesAndMarketing/ContactInformation.aspx/DeleteCompanyDocument",
        //            dataType: "json",
        //            data: JSON.stringify({ documentId: docId }),
        //            async: false,
        //            success: (data) => {
        //                CommonHelper.AlertMessage(data.d.AlertMessage);
        //                $("#trdoc" + rowIndex).remove();

        //            },
        //            error: (error) => {
        //                toastr.error(error, "", { timeOut: 5000 });
        //            }
        //        });
        //    }
        //}

        // New Modify work//
        function LoadCompanyDetails(companyId) {
            var answer = confirm("Do you want to see details?")
            if (answer) {
                var url = "./CompanyInformation.aspx?id=" + companyId;
                window.location = url;
                return true;
            }            
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadCompanyForSearch(pageNumber, IsCurrentOrPreviousPage);
        }
        function LoadCompanyForSearch(pageNumber, IsCurrentOrPreviousPage) {
            //if ($("#ContentPlaceHolder1_ddlCriteria").val() != "0") {
            //var companyName = $("#ContentPlaceHolder1_txtSCompanyName").val();
            if ($("#ContentPlaceHolder1_txtSCompanyName").val() == "") {
                var companyName = "";
            } else {
                var companyName = $("#ContentPlaceHolder1_hfSearchCompanyId").val();
            }
           

            var companyType = $("#ContentPlaceHolder1_ddlSrcCompanyType").val();
            var contactNumber = $("#ContentPlaceHolder1_txtSContactNumber").val();
            var companyEmail = $("#ContentPlaceHolder1_txtSCompanyEmail").val();
            var companyNumber = "";

            var isCRMCompanyNumberEnable = $("#ContentPlaceHolder1_hfIsCRMCompanyNumberEnable").val()
            if (isCRMCompanyNumberEnable == "1") {
                companyNumber = $("#ContentPlaceHolder1_txtCompanyNumber").val();
            }

            var countryId = 0;
            var stateId = 0;
            var cityId = 0;
            var areaId = 0;

            if ($("#ContentPlaceHolder1_txtBillingCountry").val() != "") {
                countryId = $("#ContentPlaceHolder1_hfBillingCountryId").val();
            }

            if ($("#ContentPlaceHolder1_txtBillingState").val() != "") {
                stateId = $("#ContentPlaceHolder1_hfBillingStateId").val();
            }

            if ($("#ContentPlaceHolder1_txtBillingCity").val() != "") {
                cityId = $("#ContentPlaceHolder1_hfBillingCityId").val();
            }

            if ($("#ContentPlaceHolder1_txtBillingLocation").val() != "") {
                areaId = $("#ContentPlaceHolder1_hfBillingLocationId").val();
            }

            var lifeCycleStage = $("#ContentPlaceHolder1_ddlSrcLifeCycleStage").val()
            var companyOwnerId = $("#ContentPlaceHolder1_ddlSrcOwnerId").val()
            var dateSearchCriteria = $("#ContentPlaceHolder1_ddlCriteria").val()
            var fromDate = $("#ContentPlaceHolder1_txtSearchFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtSearchToDate").val();

            var gridRecordsCount = $("#tblGuestInfo tbody tr").length;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/frmGuestCompany.aspx/LoadCompanyForSearch',
                data: "{'companyName':'" + companyName.trim() + "', 'companyType':'" + companyType + "', 'contactNumber':'" + contactNumber.trim() + "', 'companyEmail':'" + companyEmail.trim() + "', 'countryId':'" + countryId + "', 'stateId':'" + stateId + "', 'cityId':'" + cityId + "', 'areaId':'" + areaId + "', 'lifeCycleStage':'" + lifeCycleStage + "', 'companyOwnerId':'" + companyOwnerId + "', 'dateSearchCriteria':'" + dateSearchCriteria + "', 'SearchFromDate':'" + fromDate + "', 'SearchToDate':'" + toDate + "', 'companyNumber':'" + companyNumber.trim() + "', 'gridRecordsCount':'" + gridRecordsCount + "', 'pageNumber':'" + pageNumber + "', 'isCurrentOrPreviousPage':'" + IsCurrentOrPreviousPage + "'}",
                dataType: "json",
                success: function (data) {
                    CommonHelper.SpinnerOpen();
                    LoadTable(data);
                },
                error: function (result) {
                    CommonHelper.AlertMessage(result.d.AlertMessage);
                }
            });
            return false;
            //}
        }
        function LoadTable(searchData) {
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
            if (searchData.d.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"7\" >No Data Found</td> </tr>";
                $("#tblGuestInfo tbody ").append(emptyTr);
                return false;
            }
            $.each(searchData.d.GridData, function (count, gridObject) {
                var isCRMCompanyNumberEnable = $("#ContentPlaceHolder1_hfIsCRMCompanyNumberEnable").val();
                var isCompanyHyperlinkEnableFromGrid = $("#ContentPlaceHolder1_hfIsCompanyHyperlinkEnableFromGrid").val();

                totalRow = $("#tblGuestInfo tbody tr").length;
                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                if (isCRMCompanyNumberEnable == "1") {
                    tr += "<td align='left' style='width: 8%;'>" + gridObject.CompanyNumber + "</td>";
                    if (isCompanyHyperlinkEnableFromGrid == "1") {
                        if (gridObject.IsDetailPanelEnableForCompany == 1)
                        {
                            tr += "<td align='left'  style='width: 20%;cursor:pointer' title='Company Information details' onClick='javascript:return LoadCompanyDetails(" + gridObject.CompanyId + ")'>" + gridObject.CompanyName + "</td>";
                        }
                        else {
                            tr += "<td align='left'>" + gridObject.CompanyName + "</td>";
                        }
                    }
                    else {
                        tr += "<td align='left'>" + gridObject.CompanyName + "</td>";
                    }
                }
                else {
                    $("#ContentPlaceHolder1_CompanyNumberColumnDiv").hide();
                    if (isCompanyHyperlinkEnableFromGrid == "1") {
                        if (gridObject.IsDetailPanelEnableForCompany == 1)
                        {
                            tr += "<td align='left'  style='width: 20%;cursor:pointer' title='Company Information details' onClick='javascript:return LoadCompanyDetails(" + gridObject.CompanyId + ")'>" + gridObject.CompanyName + "</td>";
                        }
                        else {
                            tr += "<td align='left'>" + gridObject.CompanyName + "</td>";
                        }
                    }
                    else {
                        tr += "<td align='left'>" + gridObject.CompanyName + "</td>";
                    }
                }

                if (gridObject.ParentCompany == "") {
                    tr += "<td align='left'</td>";
                }
                else {
                    if (isCompanyHyperlinkEnableFromGrid == "1") {
                        if (gridObject.IsDetailPanelEnableForParentCompany == 1) {
                            tr += "<td align='left'  style='width: 20%;cursor:pointer' title='Company Information details' onClick='javascript:return LoadCompanyDetails(" + gridObject.ParentCompanyId + ")'>" + gridObject.ParentCompany + "</td>";
                        }
                        else {
                            tr += "<td align='left'>" + gridObject.ParentCompany + "</td>";
                        }
                    }
                    else {
                        tr += "<td align='left'>" + gridObject.ParentCompany + "</td>";
                    }
                }

                //tr += "<td align='left' style='width: 10%;'>" + gridObject.ContactNumber + "</td>";
                tr += "<td align='left' style='width: 5%;'>" + gridObject.StateName + "</td>";
                tr += "<td align='left' style='width: 5%;'>" + gridObject.CityName + "</td>";
                tr += "<td align='left' style='width: 5%;'>" + gridObject.CountryName + "</td>";
                //tr += "<td align='left' style='width: 10%;'>" + gridObject.AssociateContacts + "</td>";
                tr += "<td align='left' style='width: 10%;'>" + gridObject.LifeCycleStage + "</td>";
                tr += "<td align='left' style='width: 10%;'>" + gridObject.AccountManager + "</td>";
                tr += "<td align='left' style='width: 5%;'>" + gridObject.CreatedDisplayDate + "</td>";
                tr += "<td style='width:15%;'> <a onclick=\"javascript:return FillFormEdit(" + gridObject.CompanyId + ",\'" + gridObject.CompanyName + '\');"' + "title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                tr += "&nbsp;&nbsp;<a href='#' title='Delete' onclick= 'DeleteCompany(" + gridObject.CompanyId + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                tr += '&nbsp;&nbsp;<a href="javascript:void();" onclick= "javascript:return ShowCompanyDocuments(' + gridObject.CompanyId + ');" title="Documents"><img style="width:16px;height:16px;" alt="Documents" src="../Images/document.png" /></a>';
                tr += "</td>";
                $("#tblGuestInfo tbody").append(tr);
                tr = "";

            });
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.NextButton);

            CommonHelper.ApplyIntigerValidation();
            return false;
        }
        function DeleteCompany(Id) {
            if (!confirm("Do you want to delete?")) {
                return;
            }
            $(Id).parent().parent().remove();
            PageMethods.DeleteCompany(Id, DeleteCompanySucceed, DeleteCompanyFailed);
            return false;
        }
        function DeleteCompanySucceed(result) {
            LoadCompanyForSearch(1, 1);
            CommonHelper.AlertMessage(result.AlertMessage);
            return false;
        }
        function DeleteCompanyFailed(error) {
            CommonHelper.AlertMessage(error.AlertMessage);
            return false;
        }
        function CreateNewCompany() {
            //PerformClearAction();
            var iframeid = 'frmPrint';
            var url = "./Company.aspx";
            parent.document.getElementById(iframeid).src = url;

            $("#CompanyDialogue").dialog({
                autoOpen: true,
                modal: true,
                width: 1100,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Create New Company",
                show: 'slide'
            });
            return false;
        }
        function CloseCompanyDialog() {
            $('#CompanyDialogue').dialog('close');
            return false;
        }
        function FillFormEdit(id, name) {
            if (!confirm("Do you want to edit ?")) {
                return false;
            }
            var iframeid = 'frmPrint';
            var url = "./Company.aspx?cid=" + id;
            parent.document.getElementById(iframeid).src = url;

            $("#CompanyDialogue").dialog({
                autoOpen: true,
                modal: true,
                width: 1100,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Update - " + name,
                show: 'slide'
            });

            return false;
        }
    </script>
    <div id="CompanyDialogue" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="CompanyDocumentInfo">
    </div>
    <div id="contactdocuments" style="display: none;">
        <label for="Attachment" class="control-label col-md-2">
            Attachment</label>
        <div class="col-md-4">
            <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
                <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                    FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
            </asp:Panel>
        </div>
    </div>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
        <input class=" img-rounded" type="image" src="../Images/delete.png" style="margin-left: 5px;" onclick="RemoveWebTextBox(this)" />
    </div>

    <asp:HiddenField ID="hfIsCRMCompanyNumberEnable" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsCompanyHyperlinkEnableFromGrid" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="RandomProductId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfGuestDeletedDoc" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="tempProductId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfEmail" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfEmailDelete" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtNodeId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtCompanyId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfSearchCompanyId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfIsHotelGuestCompanyRescitionForAllUsers" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBillingCountryId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBillingStateId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBillingCityId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBillingLocationId" runat="server" Value="0"></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style" style="display: none">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none; display: none"><a
                href="#tab-1">Company Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Company </a></li>
        </ul>
        <div id="tab-1" style="display: none">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Company Information
                </div>
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
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-2 ">Company Type</label>
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
                        <div class="form-group">
                            <label class="control-label col-md-2 ">Ownership</label>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlOwnership" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>
                            <label class="control-label col-md-2 required-field">Life Cycle Stage</label>
                            <div class="col-sm-10">
                                <asp:DropDownList ID="ddlLifeCycleStageId" runat="server" CssClass="form-control" TabIndex="1">
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
                                    <asp:TextBox ID="txtAnnualRevenue" runat="server" placeholder="$" CssClass="form-control" TabIndex="2"></asp:TextBox>
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
                                    <asp:TextBox ID="txtDiscountPercent" runat="server" CssClass="form-control" TabIndex="10">0</asp:TextBox>
                                </div>
                            </div>
                            <div id="DivCreditLimit" runat="server">
                                <label for="CreditLimit" class="control-label col-md-2">Credit Limit</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtCreditLimit" runat="server" CssClass="form-control" TabIndex="10">0</asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <fieldset>
                                <legend>Contact Information</legend>
                                <div class="form-group">
                                    <label class="control-label col-md-2 required-field">Phone</label>
                                    <%-- <div class="col-md-2">
                                        <label class="control-label">Phone</label>
                                    </div>--%>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"
                                            TabIndex="2"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-2 required-field">Email</label>
                                    <%--<div class="col-md-2">
                                        <label class="control-label">Email</label>
                                    </div>--%>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"
                                            TabIndex="2"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-2 ">Fax</label>
                                    <%--<div class="col-md-2">
                                        <label class="control-label">Fax</label>
                                    </div>--%>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="txtFax" runat="server" CssClass="form-control"
                                            TabIndex="2"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-2 ">Website</label>
                                    <%--<div class="col-md-2">
                                        <label class="control-label">Website</label>
                                    </div>--%>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="txtWebsite" runat="server" CssClass="form-control"
                                            TabIndex="2"></asp:TextBox>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                        <div class="form-group">
                            <fieldset>
                                <legend>Billing Address</legend>
                                <div class="form-group">
                                    <label class="control-label col-md-2 ">Street</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="txtBillingStreet" runat="server" CssClass="form-control"
                                            TabIndex="2"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-2 ">City</label>
                                    <div class="col-sm-10">
                                        <asp:DropDownList ID="ddlBillingCityId" runat="server" CssClass="form-control" TabIndex="1">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-2 ">State</label>
                                    <div class="col-sm-10">
                                        <asp:DropDownList ID="ddlBillingStateId" runat="server" CssClass="form-control" TabIndex="1">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-2 ">Country</label>
                                    <div class="col-sm-10">
                                        <asp:DropDownList ID="ddlBillingCountryId" runat="server" CssClass="form-control" TabIndex="1">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-2 ">Post Code</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtBillingPostCode" runat="server" CssClass="form-control"
                                            TabIndex="2"></asp:TextBox>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                        <div class="form-group" id="DivShippingAddress" runat="server">
                            <fieldset>
                                <legend>Shipping Address</legend>
                                <div class="form-group">
                                    <label class="control-label col-md-2 ">Street</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="txtShippingStreet" runat="server" CssClass="form-control"
                                            TabIndex="2"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-2 ">City</label>
                                    <div class="col-sm-10">
                                        <asp:DropDownList ID="ddlShippingCityId" runat="server" CssClass="form-control" TabIndex="1">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-2 ">State</label>
                                    <div class="col-sm-10">
                                        <asp:DropDownList ID="ddlShippingStateId" runat="server" CssClass="form-control" TabIndex="1">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-2 ">Country</label>
                                    <div class="col-sm-10">
                                        <asp:DropDownList ID="ddlShippingCountryId" runat="server" CssClass="form-control" TabIndex="1">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-md-2 ">Post Code</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtShippingPostCode" runat="server" CssClass="form-control"
                                            TabIndex="2"></asp:TextBox>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                        <div class="form-group">
                            <label for="Remarks" class="control-label col-md-2">Remarks</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="9"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Attachment" class="control-label col-md-2">Attachment</label>

                            <div class="col-md-10">
                                <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Attach" onclick="AttachFile()" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div id="ContactDocumentInfo">
                            </div>
                        </div>
                        <%--hided in new Dev--%>
                        <div style="display: none">
                            <div class="form-group">
                                <label for="CompanyAddress" class="control-label col-md-2">Address</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtCompanyAddress" runat="server" CssClass="form-control"
                                        TextMode="MultiLine" TabIndex="2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="EmailAddress" class="control-label col-md-2">Company Email</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="WebAddress" class="control-label col-md-2">Web Address</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtWebAddress" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="ContactPerson" class="control-label col-md-2 required-field">Contact Person</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="ContactNumber" class="control-label col-md-2 required-field">Contact Number</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtContactNumber" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                                </div>
                                <label for="TelephoneNumber" class="control-label col-md-2">Telephone Number</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtTelephoneNumber" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="ContactDesignation" class="control-label col-md-2">Designation</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtContactDesignation" runat="server" CssClass="form-control"
                                        TabIndex="8"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="Reference" class="control-label col-md-2">Account Manager</label>
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="ddlReferenceId" runat="server" CssClass="form-control" TabIndex="65">
                                    </asp:DropDownList>
                                </div>
                                <label for="Industry" class="control-label col-md-2">Business Type</label>
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="ddlIndustryId" runat="server" CssClass="form-control" TabIndex="65">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="Industry" class="control-label col-md-2">Teritory / Area</label>
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="ddlLocation" TabIndex="2" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">

                                <label for="SignupStatus" class="control-label col-md-2">Signup Status</label>
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="ddlSignupStatus" runat="server" CssClass="form-control"
                                        TabIndex="2">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <label class="control-label col-md-2">Number Of Employee</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNumberOfEmployee" CssClass="quantity form-control" runat="server"></asp:TextBox>
                                </div>


                                <label class="control-label col-md-2">Annual Revenue</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtAnnualRevenueOld" CssClass="quantitydecimal form-control" runat="server"></asp:TextBox>
                                </div>



                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--hided in new Dev--%>
            <%--<div class="childDivSection">
                <div id="GuestocumentsInformation" class="panel panel-default" style="height: 270px;">
                    <div class="panel-heading">
                        Company Documents Information
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                
                                
                                <label for="GuestDocument" class="control-label col-md-2">
                                    Company Document</label>
                                <div class="col-md-4">
                                    <asp:Panel ID="Panel1" runat="server" Style="text-align: center;">
                                        <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                                            FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>

            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary btn-sm"
                        TabIndex="11" />
                    <asp:Button ID="btnClear" TabIndex="12" runat="server" Text="Clear" CssClass="btn btn-primary btn-sm"
                        OnClientClick="javascript: return PerformClearAction();" />
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Company Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group" runat="server" id="CompanyNumberSrcDiv">
                            <label for="ContentPlaceHolder1_txtCompanyNumber" class="control-label col-md-2">Company Number</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtCompanyNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="CompanyName" class="control-label col-md-2">Company Name</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtSCompanyName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <label class="control-label col-md-2 ">Company Type</label>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlSrcCompanyType" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="ContactNumber" class="control-label col-md-2">Contact Number</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtSContactNumber" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                            </div>
                            <label for="CompanyEmail" class="control-label col-md-2">Company Email</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtSCompanyEmail" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-2 ">Country</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtBillingCountry" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:TextBox>
                            </div>
                            <label class="control-label col-md-2 ">State/ Province/ District</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtBillingState" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-2 ">City</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtBillingCity" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:TextBox>
                            </div>
                            <label class="control-label col-md-2 " runat="server" id="BillingAreaLabel">Area</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtBillingLocation" runat="server" CssClass="form-control"
                                    TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-2">Life Cycle Stage</label>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlSrcLifeCycleStage" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>
                            <label class="control-label col-md-2">Account Manager</label>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlSrcOwnerId" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="dvSearchDateTime" class="form-group">
                            <label class="control-label col-md-2">Criteria (Date)</label>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlCriteria" runat="server" CssClass="form-control" TabIndex="1">
                                    <asp:ListItem Value="0">--- Not Applicable ---</asp:ListItem>
                                    <asp:ListItem Value="1">Created Date</asp:ListItem>
                                    <asp:ListItem Value="2">Modified Date</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div id="SearchDateDiv">
                                <label class="control-label col-md-2">Search Date</label>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtSearchFromDate" placeholder="From Date" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtSearchToDate" placeholder="To Date" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="javascript: return LoadCompanyForSearch(1,1);"
                                    CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    TabIndex="6" OnClick="btnCancel_Click1" />
                                <asp:Button ID="btnAdd" runat="server" Text="Add New Company" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return CreateNewCompany();" TabIndex="6" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body" style="display: none">
                    <asp:GridView ID="gvGuestCompany" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                        ForeColor="#333333" PageSize="20" OnPageIndexChanging="gvGuestCompany_PageIndexChanging"
                        OnRowDataBound="gvGuestCompany_RowDataBound" OnRowCommand="gvGuestCompany_RowCommand"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("CompanyId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CreatedBy" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblCreatedBy" runat="server" Text='<%#Eval("CreatedBy") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:HyperLinkField ControlStyle-ForeColor="#333333" DataTextField="CompanyName" HeaderText="Company Name" ItemStyle-Width="50%"
                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" DataNavigateUrlFormatString="CompanyInformation.aspx?id={0}"
                                DataNavigateUrlFields="CompanyId" />
                            <asp:BoundField DataField="ContactNumber" HeaderText="Contact Number" ItemStyle-Width="35%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" OnClientClick='<%# string.Format("return ConfirmEdit(\"{0}\",\"{1}\");", Eval("CompanyName"), Eval("CompanyId")) %>' CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("CompanyId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("CompanyId") %>' ImageUrl="~/Images/delete.png" Text=""
                                        AlternateText="Delete" ToolTip="Delete" OnClientClick="return confirm('Do you want to Delete?');" />
                                    &nbsp;<asp:ImageButton ID="ImgShowDocuments" runat="server" CausesValidation="False" CommandName="CmdShowDocuments"
                                        CommandArgument='<%# bind("CompanyId") %>' ImageUrl="~/Images/document.png" Text=""
                                        AlternateText="Documents" ToolTip="Documents" OnClientClick='<%# Eval("CompanyId", "ShowCompanyDocuments(\"{0}\"); return false;") %>' />
                                </ItemTemplate>
                                <ControlStyle Font-Size="Small" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
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
                <div>
                    <div class="form-group" style="">
                        <table id="tblGuestInfo" style="width: 100%;" class="table table-bordered table-condensed table-responsive">
                            <thead>
                                <tr style="color: White; background-color: #44545E; font-weight: bold; text-align: left;">
                                    <th style="width: 8%;" id="CompanyNumberColumnDiv" runat="server">Company Number
                                    </th>
                                    <th style="width: 20%;">Company Name
                                    </th>
                                    <th style="width: 20%;">Parent Company
                                    </th>
                                    <%--<th style="width: 10%;">Company Contact
                                    </th>--%>
                                    <th style="width: 5%;">State/ Province/ District
                                    </th>
                                    <th style="width: 5%;">City
                                    </th>
                                    <th style="width: 5%;">Country
                                    </th>
                                    <%--<th style="width: 10%;">Associate Contacts
                                    </th>--%>
                                    <th style="width: 10%;">Life Cycle Stage
                                    </th>
                                    <th style="width: 10%;">Account Manager
                                    </th>
                                    <th style="width: 5%;">Created Date
                                    </th>
                                    <th style="width: 15%;">Action
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
                <%--<div class="childDivSection">
                    <div class="text-center" id="GridPagingContainer">
                        <ul class="pagination">
                            <asp:Literal ID="gridPaging" runat="server"></asp:Literal>
                        </ul>
                    </div>
                </div>--%>
            </div>
        </div>
    </div>
    <div id="companyDocuments" style="display: none;">
        <div id="imageDiv"></div>
    </div>
    <script type="text/javascript">
        var xNewAdd = '<%=isNewAddButtonEnable%>';
        if (xNewAdd > -1) {
            NewAddButtonPanelShow();
            if (parseInt(xNewAdd) == 2) {
                $('#btnNewGuestCompany').hide();
                $('#EntryPanel').show();
            }
        }
        else {
            NewAddButtonPanelHide();
        }
    </script>
</asp:Content>
