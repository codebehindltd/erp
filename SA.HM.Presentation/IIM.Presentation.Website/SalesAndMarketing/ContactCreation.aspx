<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="ContactCreation.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.ContactCreation" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var flag = 0; var isAdminUser = false;;
        $(document).ready(function () {
            var adminUser = $("#<%=hfIsAdminUser.ClientID %>").val();
            if (adminUser == "True" || adminUser == "1") {
                isAdminUser = true;
                $("#accountManHead").show();
            }
            else {
                isAdminUser = false;
                $("#accountManHead").hide();
            }

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

            $("#ContentPlaceHolder1_ddlContactSource").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlContactType").select2({
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

            $("#ContentPlaceHolder1_txtBillingCountry").blur(function () {
                if ($("#ContentPlaceHolder1_txtBillingCountry").val() == "") {
                    $("#ContentPlaceHolder1_hfBillingCountryId").val(0);
                }
            });
            
            $("#ContentPlaceHolder1_txtContactName").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/ContactCreation.aspx/LoadLabelByAutoSearch',
                        //data: "{'searchTerm':'" + request.term + "'}",
                        data: JSON.stringify({ searchTerm: request.term }),
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
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
                    $("#ContentPlaceHolder1_hfContactId").val(ui.item.value);
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

            $("#ContentPlaceHolder1_txtBillingState").blur(function () {
                if ($("#ContentPlaceHolder1_txtBillingState").val() == "") {
                    $("#ContentPlaceHolder1_hfBillingStateId").val(0);
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

            //LoadContactForSearch(1, 1);

            $("#ContentPlaceHolder1_txtCompany").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/ContactCreation.aspx/CompanySearch',
                        data: JSON.stringify({ searchTerm: request.term }),
                        dataType: "json",
                        success: function (data) {


                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.CompanyName,
                                    value: m.CompanyId,

                                };
                            });
                            response(searchData);
                        },
                        error: function (result) {
                            CommonHelper.AlertMessage(result.AlertMessage);
                            return false;
                        }
                    });
                },
                focus: function (event, ui) {

                    event.preventDefault();

                },
                select: function (event, ui) {

                    event.preventDefault();
                    ItemSelected = ui.item;

                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfCompany").val(ui.item.value);

                }
            });

            $("#ContentPlaceHolder1_ddlContactOwner").select2({
                tags: false,
                allowClear: true,
                width: "99.75%",
            });



            //$("#ContentPlaceHolder1_txtPopUpCompany").autocomplete({
            //    source: function (request, response) {
            //        $.ajax({
            //            type: "POST",
            //            contentType: "application/json; charset=utf-8",
            //            url: '../SalesAndMarketing/ContactCreation.aspx/CompanySearch',
            //            data: JSON.stringify({ searchTerm: request.term }),
            //            dataType: "json",
            //            success: function (data) {
            //                var searchData = data.error ? [] : $.map(data.d, function (m) {
            //                    return {
            //                        label: m.CompanyName,
            //                        value: m.CompanyId,

            //                    };
            //                });
            //                response(searchData);
            //            },
            //            error: function (result) {
            //                CommonHelper.AlertMessage(result.AlertMessage);
            //                return false;
            //            }
            //        });
            //    },
            //    focus: function (event, ui) {

            //        event.preventDefault();
            //    },
            //    select: function (event, ui) {

            //        event.preventDefault();
            //        ItemSelected = ui.item;

            //        $(this).val(ui.item.label);
            //        $("#ContentPlaceHolder1_hfPopUpCompany").val(ui.item.value);

            //    }
            //});

        });

        //function AddNewContact() {
        //    $("#ContactDialogue").dialog({
        //        autoOpen: true,
        //        modal: true,
        //        width: '68%',
        //        maxWidth: '90%',
        //        closeOnEscape: true, 
        //        resizable: false,
        //        height: 'auto',
        //        fluid: true,
        //        title: "Add New Contact",
        //        show: 'slide',
        //        hide: 'fold',
        //        close: function (event, ui) {
        //            $("#ContentPlaceHolder1_btnSaveClose").val("Save And Close");
        //            $("#ContentPlaceHolder1_btnSaveContinue").val("Save And Continue");
        //            $("#ContentPlaceHolder1_btnClear").show();
        //            $("#ContentPlaceHolder1_btnSaveContinue").show();
        //            $("#ContentPlaceHolder1_hfId").val("0");
        //            $("#ContentPlaceHolder1_txtName").val("");
        //            //$("#ContentPlaceHolder1_txtPopUpContactNo").val("");
        //            //$("#ContentPlaceHolder1_ddlPopUpContactOwner").val("0");
        //            //$("#ContentPlaceHolder1_txtPopUpCompany").val("");
        //            $("#ContentPlaceHolder1_txtJobTitle").val("");
        //            //$("#ContentPlaceHolder1_txtMail").val("");
        //            $("#ContentPlaceHolder1_hfPopUpCompany").val("0");

        //        }
        //    });
        //    CommonHelper.SpinnerClose();
        //    return false;
        //}
        function CloseContactDialog() {
            $('#ContactDialogue').dialog('close');
            return false;
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadContactForSearch(pageNumber, IsCurrentOrPreviousPage);
        }
        function LoadContactForSearch(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#ContactTable tbody tr").length;
            //var contactName = contactName = $("#ContentPlaceHolder1_txtContactName").val();            
            //var contactName = contactName = $("#ContentPlaceHolder1_hfContactId").val();            
            //var contactOwnerId = $("#ContentPlaceHolder1_ddlContactOwner option:selected").val();
            if ($("#ContentPlaceHolder1_txtContactName").val() == "") {
                var contactName = "";
            } else {
                var contactName = contactName = $("#ContentPlaceHolder1_hfContactId").val();  
            }
            var companyId = $("#ContentPlaceHolder1_hfCompany").val();
            var contactNo = $("#ContentPlaceHolder1_txtSrcContactNumber").val();
            var contactEmail = $("#ContentPlaceHolder1_txtSrcContactEmail").val();
            var contactNumber = $("#ContentPlaceHolder1_txtContactNumber").val();

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
            var contactSource = $("#ContentPlaceHolder1_ddlContactSource").val();
            var lifeCycleStage = $("#ContentPlaceHolder1_ddlSrcLifeCycleStage").val();
            var contactOwnerId = $("#ContentPlaceHolder1_ddlContactOwner").val();
            var dateSearchCriteria = $("#ContentPlaceHolder1_ddlCriteria").val();
            var fromDate = $("#ContentPlaceHolder1_txtSearchFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtSearchToDate").val();
            var contactType = $("#ContentPlaceHolder1_ddlContactType").val();

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/ContactCreation.aspx/LoadContactForSearch',
                data: "{'ContactName':'" + contactName.trim() + "', 'CompanyId':'" + companyId + "', 'ContactNo':'" + contactNo.trim() + "', 'ContactEmail':'" + contactEmail.trim() + "', 'countryId':'" + countryId + "', 'stateId':'" + stateId + "', 'cityId':'" + cityId + "', 'areaId':'" + areaId + "', 'lifeCycleStage':'" + lifeCycleStage + "', 'contactSource':'" + contactSource + "', 'contactOwnerId':'" + contactOwnerId + "', 'dateSearchCriteria':'" + dateSearchCriteria + "', 'SearchFromDate':'" + fromDate + "', 'SearchToDate':'" + toDate + "', 'ContactType':'" + contactType.trim() + "', 'contactNumber':'" + contactNumber.trim() + "', 'gridRecordsCount':'" + gridRecordsCount + "', 'pageNumber':'" + pageNumber + "', 'isCurrentOrPreviousPage':'" + IsCurrentOrPreviousPage + "'}",
                dataType: "json",
                success: function (data) {
                    LoadTable(data);
                },
                error: function (result) {
                    CommonHelper.AlertMessage(result.d.AlertMessage);
                }
            });
            return false;
        }

        function LoadCompanyDetails(companyId) {
            var answer = confirm("Do you want to see details?")
            if (answer) {
                var url = "./CompanyInformation.aspx?id=" + companyId;
                window.location = url;
                return true;
            }
        }

        function LoadTable(searchData) {
            var rowLength = $("#ContactTable tbody tr").length;
            var dataLength = searchData.length;
            $("#ContactTable tbody").empty();
            $("#GridPagingContainer ul").empty();
            i = 0;

            var isCompanyHyperlinkEnableFromGrid = $("#ContentPlaceHolder1_hfIsCompanyHyperlinkEnableFromGrid").val();
            var isContactHyperlinkEnableFromGrid = $("#ContentPlaceHolder1_hfIsContactHyperlinkEnableFromGrid").val();

            if (searchData.d.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"8\" >No Data Found</td> </tr>";
                $("#ContactTable tbody ").append(emptyTr);
                return false;
            }

            $.each(searchData.d.GridData, function (count, gridObject) {
                var tr = "";

                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }

                //tr += "<td style='width:15%;'>" + gridObject.ContactNumber + "</td>";

                if (isContactHyperlinkEnableFromGrid == "1") {
                    if (gridObject.IsDetailPanelEnableForContact == 1) {
                        tr += "<td style='width:20%;cursor:pointer;'><a style='color:#333;' onclick=\"javascript:return LoadContactDetails(" + gridObject.Id + "," + gridObject.CompanyId + ");\">" + gridObject.Name + "</td></a>";
                    }
                    else {
                        tr += "<td style='width:20%;' align='left'>" + gridObject.Name + "</td>";
                    }
                }
                else {
                    tr += "<td style='width:20%;' align='left'>" + gridObject.Name + "</td>";
                }

                tr += "<td style='width:10%;'>" + gridObject.JobTitle + "</td>";
                //tr += "<td style='width:15%;'>" + gridObject.CompanyName + "</td>";

                if (gridObject.CompanyName == "") {
                    tr += "<td align='left'</td>";
                }
                else {
                    if (isCompanyHyperlinkEnableFromGrid == "1") {
                        if (gridObject.IsDetailPanelEnableForParentCompany == 1) {
                            tr += "<td align='left'  style='width: 15%;cursor:pointer' title='Company Information details' onClick='javascript:return LoadCompanyDetails(" + gridObject.CompanyId + ")'>" + gridObject.CompanyName + "</td>";
                        }
                        else {
                            tr += "<td align='left'>" + gridObject.CompanyName + "</td>";
                        }
                    }
                    else {
                        tr += "<td align='left'>" + gridObject.CompanyName + "</td>";
                    }
                }

                //tr += "<td style='width:10%;'>" + gridObject.MobilePersonal + "</td>";
                //tr += "<td style='width:10%;'>" + gridObject.Email + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.ContactType + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.LifeCycleStage + "</td>";

                tr += "<td style='width:10%;'>" + gridObject.AccountManager + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.CreatedDisplayDate + "</td>";

                //if (isAdminUser) {
                //    tr += "<td style='width:15%;'>" + gridObject.ContactOwner + "</td>";
                //}

                tr += "<td style='width:10%;'> <a onclick=\"javascript:return FillFormEdit(" + gridObject.Id + ",\'" + gridObject.Name + '\');"' + "title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                tr += "&nbsp;&nbsp;<a href='#' onclick= 'DeleteContact(" + gridObject.Id + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                tr += '&nbsp;&nbsp;<a href="javascript:void();" onclick= "javascript:return ShowContactDocuments(' + gridObject.Id + ');" title="Documents"><img style="width:16px;height:16px;" alt="Documents" src="../Images/document.png" /></a>';
                tr += "</td>";

                tr += "<td style='display:none'>" + gridObject.Id + "</td>";
                tr += "<td style='display:none'>" + gridObject.CompanyId + "</td>";
                tr += "<td style='display:none'>" + gridObject.ContactOwnerId + "</td>";

                tr += "</tr>";

                $("#ContactTable tbody").append(tr);

                tr = "";
                i++;
            });
            
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.NextButton);
            return false;
        }
        function FillFormEdit(id, name) {

            if (!confirm("Do you want to edit ?")) {
                return false;
            }
            var iframeid = 'frmPrint';
            var url = "./Contact.aspx?editId=" + id;
            parent.document.getElementById(iframeid).src = url;

            $("#ContactDialogue").dialog({
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

        function LoadContactDetails(id, companyId) {
            var answer = confirm("Do you want to see details?")
            if (answer) {
                var url = "./ContactInformation.aspx?conid=" + id + "&cid=" + companyId;
                window.location = url;
                return true;
            }            
        }
        function allowOnlyNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (!(charCode > 31 && (charCode < 48 || charCode > 57)) || charCode == 43) {
                return true;
            }
            else {
                toastr.info("Number Only");
                return false;
            }
        }
        function CloseDialog() {
            $("#ContactDialogue").dialog('close');
            return false;
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }
        function DeleteContact(Id) {
            if (!confirm("Do you want to delete?")) {
                return;
            }
            $(Id).parent().parent().remove();
            PageMethods.DeleteContact(Id, DeleteContactSucceed, DeleteContactFailed);
            return false;
        }
        function DeleteContactSucceed(result) {
            LoadContactForSearch(1, 1);
            CommonHelper.AlertMessage(result.AlertMessage);
            return false;
        }
        function DeleteContactFailed(error) {
            CommonHelper.AlertMessage(error.AlertMessage);
            return false;
        }

        function CreateNewContact() {
            //PerformClearAction();
            var iframeid = 'frmPrint';
            var url = "./Contact.aspx";
            parent.document.getElementById(iframeid).src = url;

            $("#ContactDialogue").dialog({
                autoOpen: true,
                modal: true,
                width: 1100,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Create New Contact",
                show: 'slide'
            });
            return false;
        }
        // Documents show
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
                title: "Contact Documents",
                show: 'slide'
            });

            return false;
        }
        function OnLoadImagesFailed(error) {
            toastr.error(error.get_message());
        }
        function ShowContactDocuments(id) {
            PageMethods.LoadContactDocument(id, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }
        function OnLoadDocumentSucceeded(result) {
            $("#imageDiv").html(result);

            $("#companyDocuments").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Contact Documents",
                show: 'slide'
            });

            return false;
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }

    </script>
    <asp:HiddenField ID="hfPopUpCompany" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsAdminUser" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBillingCountryId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBillingStateId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBillingCityId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBillingLocationId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsCompanyHyperlinkEnableFromGrid" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsContactHyperlinkEnableFromGrid" runat="server"></asp:HiddenField>

    <asp:HiddenField ID="hfCompanyId" Value="" runat="server" />
    <asp:HiddenField ID="hfContactId" Value="" runat="server" />
    <div id="ContactDialogue" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="companyDocuments" style="display: none;">
        <div id="imageDiv"></div>
    </div>
    <div>
        <div id="InfoPanel" class="panel panel-default">
            <div class="panel-heading">
                Contact Information
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="control-label col-md-2 ">Contact Name</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtContactName" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <label class="control-label col-md-2 ">Company Name</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtCompany" runat="server" CssClass="form-control">
                            </asp:TextBox>
                            <asp:HiddenField ID="hfCompany" runat="server" Value="0"></asp:HiddenField>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-md-2 ">Contact Number</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSrcContactNumber" CssClass="form-control" runat="server" onkeypress="return allowOnlyNumber(event);"></asp:TextBox>
                        </div>
                        <label class="control-label col-md-2 ">Contact Email</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSrcContactEmail" CssClass="form-control" runat="server"></asp:TextBox>
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
                        <label class="control-label col-md-2" runat="server" id="BillingAreaLabel">Area</label>
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
                        <label class="control-label col-md-2 ">Account Manager</label>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlContactOwner" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-md-2">Source</label>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlContactSource" runat="server" CssClass="form-control" TabIndex="1">
                            </asp:DropDownList>
                        </div>
                        <label class="control-label col-md-2">Contact Type</label>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlContactType" runat="server" CssClass="form-control" TabIndex="1">
                                <asp:ListItem Text="--- All ---" Value="0" />
                                <asp:ListItem Text="Business" Value="Business" />
                                <asp:ListItem Text="Billing" Value="Billing" />
                                <asp:ListItem Text="Technical" Value="Technical" />
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
                    <div class="form-group" style="display: none;">
                        <div class="col-md-2">
                            <asp:Label ID="Label1" runat="server" class="control-label" Text="Contact Number"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtContactNumber" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="javascript: return LoadContactForSearch(1,1);"
                                CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary btn-sm"
                                TabIndex="6" OnClick="btnCancel_Click" />
                            <asp:Button ID="btnAdd" runat="server" Text="Add New Contact" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript: return CreateNewContact();" TabIndex="6" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <div class="form-group" id="ContactTableContainer" style="overflow: scroll;">
                        <table class="table table-bordered table-condensed table-responsive" id="ContactTable"
                            style="width: 100%;">
                            <thead>
                                <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                    <%--<th style="width: 15%;">Contact Number
                                    </th>--%>
                                    <th style="width: 15%;">Contact Name
                                    </th>
                                    <th style="width: 10%;">Contact Title
                                    </th>
                                    <th style="width: 15%;">Associate Company
                                    </th>
                                    <%--<th style="width: 10%;">Contact Number
                                    </th>
                                    <th style="width: 10%;">Contact Email
                                    </th>--%>
                                    <th style="width: 10%;">Contact Type
                                    </th>
                                    <th style="width: 10%;">Life Cycle Stage
                                    </th>         
                                    <th style="width: 10%;">Account Manager
                                    </th>
                                    <th style="width: 10%;">Created Date
                                    </th>                           
                                    <%--<th style="width: 15%;" id="accountManHead">Account Manager
                                    </th>--%>
                                    <th style="width: 10%;">Action
                                    </th>
                                    <th style="display: none">Id
                                    </th>
                                    <th style="display: none">CompanyId
                                    </th>
                                    <th style="display: none">ContactOwnerId
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
    </div>
</asp:Content>
