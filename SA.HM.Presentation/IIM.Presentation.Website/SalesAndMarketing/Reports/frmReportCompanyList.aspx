<%@ Page Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmReportCompanyList.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.Reports.frmReportCompanyList" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
           
        $(document).ready(function () {


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
                //UploadComplete();
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
                        url: '/SalesAndMarketing/Reports/frmReportCompanyList.aspx/LoadCountryForAutoSearch',
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
                        url: '/SalesAndMarketing/Reports/frmReportCompanyList.aspx/GetCompanyByAutoSearch',
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
                        url: '/SalesAndMarketing/Reports/frmReportCompanyList.aspx/LoadStateForAutoSearchByCountry',
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
                        url: '/SalesAndMarketing/Reports/frmReportCompanyList.aspx/LoadCityForAutoSearchByState',
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

           

            CommonHelper.ApplyDecimalValidation();
            CommonHelper.ApplyIntigerValidation();


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
                console.log(companyName);

                $("#ContentPlaceHolder1_hfGuestCompanyName").val(companyName);

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
                            if (gridObject.IsDetailPanelEnableForCompany == 1) {
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
                            if (gridObject.IsDetailPanelEnableForCompany == 1) {
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
        });

        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }
        function LoadCompanyNameForReport(pageIndex) {
            if ($("#ContentPlaceHolder1_txtSCompanyName").val() == "") {
                var companyName = "";
            } else {
                var companyName = $("#ContentPlaceHolder1_hfSearchCompanyId").val();
            }

            $("#ContentPlaceHolder1_hfGuestCompanyName").val(companyName);
            $("#ContentPlaceHolder1_hfPageIndex").val(pageIndex);
        }
    </script>
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfProjectId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyName" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfProjectName" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfGuestCompanyName" runat="server" Value=""></asp:HiddenField>


    <asp:HiddenField ID="hfIsCRMCompanyNumberEnable" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsCompanyHyperlinkEnableFromGrid" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="RandomProductId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfGuestDeletedDoc" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="tempProductId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfEmail" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfEmailDelete" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtNodeId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtCompanyId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfSearchCompanyId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfPageIndex" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsHotelGuestCompanyRescitionForAllUsers" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBillingCountryId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBillingStateId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBillingCityId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBillingLocationId" runat="server" Value="0"></asp:HiddenField>



    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
           Company List</div>
        <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group" runat="server" id="CompanyNumberSrcDiv">
                            <label for="ContentPlaceHolder1_txtCompanyNumber" class="control-label col-md-2">Company Number</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtCompanyNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" style="display: none">
                            <label class="control-label col-md-2">Account Manager</label>
                            <div class="col-sm-10">
                                <asp:DropDownList ID="ddlCompanyOwner" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
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
                    </div>
                </div>
        </div>
    <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="btn btn-primary" OnClientClick="javascript: return LoadCompanyNameForReport(1);"
                            OnClick="btnGenerate_Click" />
                    </div>
                </div>
    <%--<div style="display: none;">
        <asp:Button ID="btnPrintReportFromClient" runat="server" Text="Button" OnClick="btnPrintReportFromClient_Click"
            ClientIDMode="Static" />
    </div>--%>
    <div style="display: none;">
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000; top: 2000;"
            clientidmode="static"></iframe>
    </div>
    <div id="ReportPanel" class="panel panel-default" style="display: none;">
        <div class="panel-heading">
            Company List</div>
        <div class="panel-body">
            <rsweb:ReportViewer ID="rvTransaction" runat="server" ShowFindControls="false" ShowWaitControlCancelLink="false"
                    PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" Font-Names="Verdana"
                    Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="830px" Height="820px">
                </rsweb:ReportViewer>
            
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if (CommonHelper.BrowserType().mozilla || CommonHelper.BrowserType().chrome) {

                var barControlId = CommonHelper.GetReportViewerControlId($("#<%=rvTransaction.ClientID %>"));

                var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                var innerTable = '<table title="Print" onclick="PrintDocumentFunc(\'' + barControlId + '\'); return false;" id="ff_print" style="cursor: default;">' + innerTbody + '</table>'
                var outerDiv = '<div style="display: inline-block; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + '</td></tr></tbody></table></div>';

                $("#" + barControlId + " > div").append(outerDiv);
            }
        });
        function PrintDocumentFunc(ss) {
            $('#btnPrintReportFromClient').trigger('click');
            return true;
        }
        var x = '<%=_CompanyListReportShow%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
   </asp:Content>






