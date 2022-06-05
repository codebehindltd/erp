<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmSalesCall.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.frmSalesCall" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        var deleteSalesObj = [];
        var empIdList = [];
        var tmpEmpId = 0;
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Sales & Marketing</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Sales Call</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $('#ContentPlaceHolder1_txtProbableInitialHour').timepicker({
                showPeriod: is12HourFormat
            });
            $('#ContentPlaceHolder1_txtProbableFollowupHour').timepicker({
                showPeriod: is12HourFormat
            });
            CommonHelper.AutoSearchClientDataSource("txtEmpSearch", "ContentPlaceHolder1_ddlEmployee", "ContentPlaceHolder1_hfEmpSearch");
            CommonHelper.AutoSearchClientDataSource("txtCompanySearch", "ContentPlaceHolder1_ddlCompany", "ContentPlaceHolder1_hfCmpSearch");
            CommonHelper.AutoSearchClientDataSource("txtSCompanySearch", "ContentPlaceHolder1_ddlSCompany", "ContentPlaceHolder1_hfSCmpSearch");

            var txtFollowupName = '<%=txtFollowupName.ClientID%>'
            var txtPurposeName = '<%=txtPurposeName.ClientID%>'
            $('#' + txtFollowupName).attr('disabled', true);
            $('#' + txtPurposeName).attr('disabled', true);

            $("#SearchPanel").hide();
            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });

            $('#txtCompanySearch').blur(function () {
                var txtInitialDate = '<%=txtInitialDate.ClientID%>'
                var txtProbableInitialHour = '<%=txtProbableInitialHour.ClientID%>'
                if ($(this).val() != "") {
                    var cmpId = $("#ContentPlaceHolder1_hfCmpSearch").val();
                    if (cmpId != "") {
                        $("#NotEnlistCompanyInfo").hide();
                        $("#LastFollowUpDateDiv").show();
                        $("#CompanyInfo").show();
                        LoadCompanyInfo(cmpId);
                        LoadSiteByCompanyId(cmpId);
                    }
                    else {
                        $("#CompanyInfo").hide();
                        $("#NotEnlistCompanyInfo").show();
                        $("#LastFollowUpDateDiv").hide();
                        toastr.warning('Company is not enlisted');
                        $('#' + txtInitialDate).val('');
                        $('#' + txtProbableInitialHour).val('12:00');
                        return;
                    }
                }
            });

            $("#btnAddEmp").click(function () {
                var input = document.getElementById('txtEmpSearch');
                var empName = input.value;
                var empId = $("#ContentPlaceHolder1_hfEmpSearch").val();
                if (empId != "" && empName != "") {
                    if ($("#PaymentDetailsList tbody > tr").find("td:eq(1):contains('" + empId + "')").length == 0) {
                        AddNewPaymentInfoDetails(empName, empId, 0);
                    }
                    else {
                        toastr.warning('Duplicate Employee');
                        return;
                    }
                }
                else {
                    toastr.warning('Employee not found');
                    $("#txtEmpSearch").val('');
                    return;
                }
                $("#txtEmpSearch").val('');
                tmpEmpId = empId;
            });

            $("#gvSalesCallInformation").delegate("td > img.SalesCallDelete", "click", function () {
                var answer = confirm("Do you want to delete this record?")
                if (answer) {
                    var salesId = $.trim($(this).parent().parent().find("td:eq(5)").text());
                    var params = JSON.stringify({ salesCallId: salesId });

                    var $row = $(this).parent().parent();
                    $.ajax({
                        type: "POST",
                        url: "/SalesAndMarketing/frmSalesCall.aspx/DeleteSalesCallInfo",
                        data: params,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $row.remove();
                            CommonHelper.AlertMessage(data.d.AlertMessage);
                            $("#myTabs").tabs('load', 1);
                        },
                        error: function (error) {
                        }
                    });
                }
            });

            var txtInitialDate = '<%=txtInitialDate.ClientID%>'
            var txtFollowUpDate = '<%=txtFollowupDate.ClientID%>'
            var minCheckInDate = "";
            minCheckInDate = $("#<%=hfMinCheckInDate.ClientID %>").val();

            $("#ContentPlaceHolder1_txtInitialDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                minDate: minCheckInDate
            });
            $("#ContentPlaceHolder1_txtFollowupDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                minDate: minCheckInDate
            });
            $("#ContentPlaceHolder1_txtSFromInitialDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#ContentPlaceHolder1_txtSToInitialDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $("#ContentPlaceHolder1_txtSFromFollowupDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#ContentPlaceHolder1_txtSToFollowupDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            var ddlCity = '<%=ddlCity.ClientID%>'
            var ddlLocation = '<%=ddlLocation.ClientID%>'
            $("#" + ddlCity).change(function () {
                PopulateLocations(ddlCity, ddlLocation)
            });

            var ddlFollowupType = '<%=ddlFollowupType.ClientID%>'
            $('#' + ddlFollowupType).change(function () {
                var v = $("#<%=ddlFollowupType.ClientID %>").val();
                if (v == "149") {
                    $('#' + txtFollowupName).attr('disabled', false);
                }
                else {
                    $('#' + txtFollowupName).attr('disabled', true);
                    $("#<%=txtFollowupName.ClientID %>").val('');
                }
            });

            var ddlPurpose = '<%=ddlPurpose.ClientID%>'
            $('#' + ddlPurpose).change(function () {
                var v = $("#<%=ddlPurpose.ClientID %>").val();
                if (v == "152") {
                    $('#' + txtPurposeName).attr('disabled', false);
                }
                else {
                    $('#' + txtPurposeName).attr('disabled', true);
                    $("#<%=txtPurposeName.ClientID %>").val('');
                }
            });
        });

        var ff = [];

        function DeleteEmployee(anchor) {
            ff = anchor;
            var tr = $(anchor).parent().parent();

            var id = $.trim($(tr).find("td:eq(0)").text());
            var empId = $.trim($(tr).find("td:eq(1)").text());

            if (parseInt(id, 10) != 0) {
                deleteSalesObj.push({
                    SalesCallDetailId: id,
                    EmpId: empId
                });
            }

            $(tr).remove();
            return false;
        }

        function PopulateLocations(city, location) {
            if ($("#" + city).val() == "0") {
                $("#" + location).empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
            }
            else {
                $("#" + location).empty().append('<option selected="selected" value="0">Loading...</option>');
                $.ajax({
                    type: "POST",
                    url: "/SalesAndMarketing/frmSalesCall.aspx/PopulateLocations",
                    data: '{cityId: ' + $("#" + city).val() + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        OnLocationsPopulated(response, location);
                    },
                    failure: function (response) {
                        toastr.error(response.d);
                    }
                });
            }
        }

        function OnLocationsPopulated(response, Project) {
            $("#" + Project).attr("disabled", false);
            PopulateControl(response.d, $("#" + Project), $("#<%=CommonDropDownHiddenField.ClientID %>").val());
        }

        function AddNewPaymentInfoDetails(EmpName, EmpId, id) {
            var tr = "", totalRow = 0, deleteLink = "";
            totalRow = $("#PaymentDetailsList tbody tr").length;
            th = $("#PaymentDetailsList thead tr");

            deleteLink = "<a href=\"#\" onclick= 'DeleteEmployee(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";

            if ((totalRow % 2) == 0) {
                tr += "<tr style=\"background-color:#E3EAEB;\">";
            }
            else {
                tr += "<tr style=\"background-color:#FFFFFF;\">";
            }
            tr += "<td align='left' style=\"display:none;\">" + id + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + EmpId + "</td>";
            tr += "<td align='left' style=\"width:60%; text-align:Left;\">" + EmpName + "</td>";
            tr += "<td align='center' style=\"width:40%; cursor:pointer;\">" + deleteLink + "</td>";
            tr += "</tr>";

            $("#PaymentDetailsList tbody").append(tr);
        }


        function ValidationNPreprocess() {
            var saveObj = [];
            var id = 0, empId = 0, empName = '';

            var rowLength = $("#PaymentDetailsList tbody tr").length;

            $("#PaymentDetailsList tbody tr").each(function () {

                id = parseInt($.trim($(this).find("td:eq(0)").text(), 10));
                empId = parseInt($.trim($(this).find("td:eq(1)").text(), 10));
                empName = $.trim($(this).find("td:eq(2)").text());

                if (id == 0) {
                    saveObj.push({
                        SalesCallDetailId: id,
                        EmpId: empId,
                        EmpName: empName
                    });
                }
            });

            $("#<%=hfSaveObj.ClientID %>").val(JSON.stringify(saveObj));
            $("#<%=hfDeleteObj.ClientID %>").val(JSON.stringify(deleteSalesObj));
            $("#ContentPlaceHolder1_hfCompanySiteId").val($("#ContentPlaceHolder1_ddlCompanySite").val());

            if ($("#txtCompanySearch").val() == "") {
                toastr.warning('Please Provide Company Name.');
                return false;
            }
            else if ($("#<%=txtInitialDate.ClientID %>").val() == "") {
                toastr.warning('Please Provide Initial Date.');
                return false;
            }
            else if ($("#<%=txtFollowupDate.ClientID %>").val() == "") {
                toastr.warning('Please Provide Follow-Up Date.');
                return false;
            }
            else if (rowLength == 0 && saveObj.length == 0 && deleteSalesObj.length == 0) {
                toastr.warning('Please add at least one Participant.');
                return false;
            }
            else if ($("#<%=ddlLocation.ClientID %>").val() == "0") {
                toastr.warning('Please Select Location Name.');
                return false;
            }
            else if ($("#<%=ddlCity.ClientID %>").val() == "0") {
                toastr.warning('Please Select City Name.');
                return false;
            }
            else if ($("#<%=ddlFollowupType.ClientID %>").val() == "--- Please Select ---") {
                toastr.warning('Please Select Follow-Up Type.');
                return false;
            }
            else if ($("#<%=ddlPurpose.ClientID %>").val() == "--- Please Select ---") {
                toastr.warning('Please Select Purpose.');
                return false;
            }

    if ($("#<%=hfCmpSearch.ClientID %>").val() == "") {
                if ($("#txtNllblEmailAddress").val() == "") {
                    toastr.warning('Please provide Company Email.');
                    return false;
                }
                else if ($("#txtNlContactPerson").val() == "") {
                    toastr.warning('Please provide Contact Person.');
                    return false;
                }
                else if ($("#txtNlContactNumber").val() == "") {
                    toastr.warning('Please provide contact number.');
                    return false;
                }
                else if ($("#txtNlCompanyAddress").val() == "") {
                    toastr.warning('Please provide Company address.');
                    return false;
                }
            }
        }

        $(function () {
            $("#myTabs").tabs();
        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#gvSalesCallInformation tbody tr").length;
            var companyId = 0;

            var companyName = $("#txtSCompanySearch").val();
            if (companyName == '') {
                companyId = '';
            }
            else {
                companyId = $("#<%=ddlSCompany.ClientID %>").val();
            }
            var fromIniDate = $("#<%=txtSFromInitialDate.ClientID %>").val();
            var toIniDate = $("#<%=txtSToInitialDate.ClientID %>").val();
            var fromFolupDate = $("#<%=txtSFromFollowupDate.ClientID %>").val();
            var toFolupDate = $("#<%=txtSToFollowupDate.ClientID %>").val();
            var folupTypeId = $("#<%=ddlSFollowupType.ClientID %>").val();
            var purposeId = $("#<%=ddlSPurpose.ClientID %>").val();

            PageMethods.SearchSalesCallInfo(companyName, fromIniDate, toIniDate, fromFolupDate, toFolupDate, folupTypeId, purposeId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchSuccess, OnSearchFail);
            return false;
        }

        function OnSearchSuccess(result) {

            var format = innBoarDateFormat.replace('mm', 'MM');
            var format = format.replace('yy', 'yyyy');

            $("#gvSalesCallInformation tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvSalesCallInformation tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvSalesCallInformation tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:50%; cursor:pointer;\">" + gridObject.CompanyName + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.InitialDate.format(format) + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.FollowupDate.format(format) + "</td>";
                tr += "<td align='right' style=\"width:10%; cursor:pointer;\"><img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.SalesCallId + "')\" alt='Edit Information' border='0' /></td>";
                //tr += "<td align='right' style=\"width:10%; cursor:pointer;\"><img src='../Images/delete.png' class='SalesCallDelete'  alt='Delete Information' border='0' /></td>";
                //tr += "<td align='right' style=\"width:10%; display:none;\">" + gridObject.SalesCallId + "</td>";

                tr += "</tr>"

                $("#gvSalesCallInformation tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            return false;
        }
        function OnSearchFail(error) {
            toastr.error(error.get_message());
        }

        function PerformEditAction(salesCallId) {
            PageMethods.LoadDetailInformation(salesCallId, OnLoadDetailObjectSucceeded, OnLoadDetailObjectFailed);
            return false;
        }
        function OnLoadDetailObjectSucceeded(result) {
            if (!confirm("Do you want to edit - " + result.CompanyName + "?")) {
                return false;
            }
            var format = innBoarDateFormat.replace('mm', 'MM');
            var format = format.replace('yy', 'yyyy');

            $("#txtCompanySearch").val(result.CompanyName);
            $("#<%=txtInitialDate.ClientID %>").val(result.InitialDate.format(format));
            $("#<%=txtFollowupDate.ClientID %>").val(result.FollowupDate.format(format));
            $("#<%=hfInitialDate.ClientID %>").val(result.InitialDate.format(format));
            $("#<%=hfProbableInitialHour.ClientID %>").val((result.InitialTime.replace('AM', '')).replace('PM', ''));
            $("#<%=txtProbableInitialHour.ClientID %>").val((result.InitialTime.replace('AM', '')).replace('PM', ''));
            $("#<%=txtProbableFollowupHour.ClientID %>").val((result.FollowupTime.replace('AM', '')).replace('PM', ''));
            $("#<%=txtFollowupName.ClientID %>").val(result.FollowupType);
            $("#<%=txtPurposeName.ClientID %>").val(result.Purpose);
            $("#<%=txtRemarks.ClientID %>").val(result.Remarks);
            $("#<%=ddlLocation.ClientID %>").val(result.LocationId);
            $("#<%=ddlCity.ClientID %>").val(result.CityId);
            //$("#<%=ddlIndustry.ClientID %>").val(result.IndustryId);
            $("#<%=ddlFollowupType.ClientID %>").val(result.FollowupTypeId);
            $("#<%=ddlPurpose.ClientID %>").val(result.PurposeId);

            $("#<%=ddlCIType.ClientID %>").val(result.CITypeId);
            $("#<%=ddlActionPlan.ClientID %>").val(result.ActionPlanId);
            $("#<%=ddlOpportunityStatus.ClientID %>").val(result.OpportunityStatusId);

            $("#<%=hfSalesCallId.ClientID %>").val(result.SalesCallId);
            $("#<%=btnSave.ClientID %>").val("Update");
            $("#<%=hfCmpSearch.ClientID %>").val(result.CompanyId);
            $("#<%=hfCompanySiteId.ClientID %>").val(result.SiteId);

            LoadSiteByCompanyId(result.CompanyId);

            if (result.FollowupTypeId == "149") {
                $("#OthersFollowup").show();
            }
            else {
                $("#OthersFollowup").hide();
            }

            if (result.PurposeId == "152") {
                $("#OthersPurpose").show();
            }
            else {
                $("#OthersPurpose").hide();
            }

            //$("#myTabs").tabs('select', 0);
            $("#myTabs").tabs({ active: 0 });

            var salesCallLength = 0, row = 0;
            salesCallLength = result.SMCompanySalesCallDetailList.length;

            $("#PaymentDetailsList tbody tr").remove();

            for (row = 0; row < salesCallLength; row++) {
                AddNewPaymentInfoDetails(result.SMCompanySalesCallDetailList[row].EmpName, result.SMCompanySalesCallDetailList[row].EmpId, result.SMCompanySalesCallDetailList[row].SalesCallDetailId);
            }


            return false;
        }
        function OnLoadDetailObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function LoadCompanyInfo(companyId) {
            PageMethods.LoadCompanyInfo(companyId, OnLoadCompanySucceeded, OnLoadCompanyFailed);
            return false;
        }
        function OnLoadCompanySucceeded(result) {
            $("#<%=txtEmailAddress.ClientID %>").val(result.EmailAddress);
            $("#<%=txtWebAddress.ClientID %>").val(result.WebAddress);
            $("#<%=txtContactPerson.ClientID %>").val(result.ContactPerson);
            $("#<%=txtContactNumber.ClientID %>").val(result.ContactNumber);
            $("#<%=txtTelephoneNumber.ClientID %>").val(result.TelephoneNumber);
            $("#<%=txtAddress.ClientID %>").val(result.CompanyAddress);

            if ((result.FirstInitialDateString.length) < 1) {
                $("#<%=txtInitialDate.ClientID %>").attr('disabled', false);
                $("#<%=txtProbableInitialHour.ClientID %>").attr('disabled', false);
                $("#LastFollowUpDateDiv").hide();
            }
            else {
                $("#<%=txtInitialDate.ClientID %>").val(result.FirstInitialDateString);
                $("#<%=txtProbableInitialHour.ClientID %>").val(result.FirstInitialTimeString);
                $("#<%=hfInitialDate.ClientID %>").val(result.FirstInitialDateString);
                $("#<%=hfProbableInitialHour.ClientID %>").val(result.FirstInitialTimeString);
                $("#<%=txtLastFollowUpDate.ClientID %>").val(result.LastFollowUpDateString);
                $("#<%=txtLastFollowUpTime.ClientID %>").val(result.LastFollowUpTimeString);
                $("#LastFollowUpDateDiv").show();
                $("#txtEmpSearch").focus();
                $("#<%=txtInitialDate.ClientID %>").attr('disabled', true);
                $("#<%=txtProbableInitialHour.ClientID %>").attr('disabled', true);
            }

            if (result.IndustryId != 0) {
                $("#<%=ddlIndustry.ClientID %>").val(result.IndustryId);
            }
            else {
                $("#<%=ddlIndustry.ClientID %>").val('');
            }
            if (result.ReferenceId != 0) {
                $("#<%=ddlReference.ClientID %>").val(result.ReferenceId);
            }
            else {
                $("#<%=ddlReference.ClientID %>").val('');
            }
        }
        function OnLoadCompanyFailed(error) {
            toastr.error(error.get_message());
        }

        function LoadSiteByCompanyId(companyId) {
            PageMethods.LoadCompanySite(companyId, OnLoadSiteByCompanySucceeded, OnLoadSiteByCompanyFailed);
        }

        function OnLoadSiteByCompanySucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlCompanySite');

            control.empty();
            if (list != null) {
                if (list.length > 0) {

                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].SiteId + '">' + list[i].SiteName + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            control.val($("#ContentPlaceHolder1_hfCompanySiteId").val());
            return false;
        }
        function OnLoadSiteByCompanyFailed() { }

    </script>
    <asp:HiddenField ID="hfSaveObj" runat="server" />
    <asp:HiddenField ID="hfDeleteObj" runat="server" />
    <asp:HiddenField ID="hfEmpSearch" runat="server" />
    <asp:HiddenField ID="hfCmpSearch" runat="server" />
    <asp:HiddenField ID="hfSCmpSearch" runat="server" />
    <asp:HiddenField ID="hfMinCheckInDate" runat="server" />
    <asp:HiddenField ID="hfInitialDate" runat="server" />
    <asp:HiddenField ID="hfProbableInitialHour" runat="server" />
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanySiteId" runat="server" Value="0"></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Sales Call Info</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Sales Call</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">Sales Call Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="hfSalesCallId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblCompany" runat="server" class="control-label required-field" Text="Company Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <input id="txtCompanySearch" type="text" class="form-control" name="cmpName" />
                                <div style="display: none;">
                                    <asp:DropDownList ID="ddlCompany" TabIndex="1" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div id="CompanyInfo" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblEmailAddress" runat="server" class="control-label" Text="Company Email"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control" TabIndex="3"
                                        disabled></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblWebAddress" runat="server" class="control-label" Text="Web Address"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtWebAddress" runat="server" CssClass="form-control" TabIndex="4"
                                        disabled></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblContactPerson" runat="server" class="control-label" Text="Contact Person"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" TabIndex="5"
                                        disabled></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblContactNumber" runat="server" class="control-label" Text="Contact Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtContactNumber" runat="server" CssClass="form-control" TabIndex="6" disabled></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblTelephoneNumber" runat="server" class="control-label" Text="Telephone Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtTelephoneNumber" runat="server" CssClass="form-control" TabIndex="7" disabled></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblIndustry" runat="server" class="control-label" Text="Industry Type"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlIndustry" TabIndex="2" runat="server" CssClass="form-control"
                                        disabled>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblReference" runat="server" class="control-label" Text="Reference"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlReference" TabIndex="2" runat="server" CssClass="form-control"
                                        disabled>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblAddress" runat="server" class="control-label" Text="Address"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine"
                                        TabIndex="2" disabled></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div id="NotEnlistCompanyInfo" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblNlCompanyAddress" runat="server" class="control-label" Text="Company Address"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtNlCompanyAddress" runat="server" CssClass="form-control"
                                        TextMode="MultiLine" TabIndex="3"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblNllblEmailAddress" runat="server" class="control-label" Text="Company Email"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtNllblEmailAddress" runat="server" CssClass="form-control"
                                        TabIndex="3"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblNlWebAddress" runat="server" class="control-label" Text="Web Address"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtNlWebAddress" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblNlContactPerson" runat="server" class="control-label" Text="Contact Person"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtNlContactPerson" runat="server" CssClass="form-control"
                                        TabIndex="5"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblNlContactNumber" runat="server" class="control-label" Text="Contact Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNlContactNumber" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblNlTelephoneNumber" runat="server" class="control-label" Text="Telephone Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNlTelephoneNumber" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblReferenceId" runat="server" class="control-label" Text="Reference"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlReferenceId" runat="server" CssClass="form-control" TabIndex="65">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblIndustryId" runat="server" class="control-label" Text="Industry"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlIndustryId" runat="server" CssClass="form-control" TabIndex="65">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div id="Div1" class="panel panel-default">
                            <div class="panel-heading">Participants Information</div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblEmployee" runat="server" class="control-label" Text="Participant"></asp:Label>
                                        </div>
                                        <div class="col-md-8">
                                            <input id="txtEmpSearch" type="text" class="form-control" />
                                            <div style="display: none;">
                                                <asp:DropDownList ID="ddlEmployee" TabIndex="2" CssClass="form-control" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <input type="button" id="btnAddEmp" value="Add" class="TransactionalButton btn btn-primary btn-sm" />
                                        </div>
                                        <%--<div id="EmployeeGrid">
                        </div>--%>
                                    </div>
                                    <div class="form-group">
                                        <table id="PaymentDetailsList" class="table table-bordered table-condensed table-responsive" width="95%">
                                            <colgroup>
                                                <col style="width: 60%;" />
                                                <col style="width: 40%;" />
                                            </colgroup>
                                            <thead>
                                                <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                                    <th style="text-align: left">Participants Name
                                                    </th>
                                                    <th style="text-align: center">Action
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            </tbody>
                                        </table>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Company Site"></asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlCompanySite" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblInitialDate" runat="server" class="control-label required-field" Text="Initial Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtInitialDate" runat="server" CssClass="form-control" TabIndex="12"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblInitialTime" runat="server" class="control-label" Text="Probable Time"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtProbableInitialHour" placeholder="12" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                                <%--<asp:TextBox ID="txtProbableInitialMinute" placeholder="00" CssClass="CustomMinuteSize"
                                TabIndex="3" runat="server" disabled></asp:TextBox>
                            <asp:DropDownList ID="ddlProbableInitialAMPM" CssClass="CustomAMPMSize" runat="server"
                                TabIndex="4">
                                <asp:ListItem>AM</asp:ListItem>
                                <asp:ListItem>PM</asp:ListItem>
                            </asp:DropDownList>
                            (12:00AM)--%>
                            </div>
                        </div>
                        <div id="LastFollowUpDateDiv" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblLastFollowUpDate" runat="server" class="control-label" Text="Last Follow Up Date"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtLastFollowUpDate" runat="server" CssClass="form-control" TabIndex="7" disabled></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblLastFollowUpTime" runat="server" class="control-label" Text="Last Follow Up Time"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtLastFollowUpTime" runat="server" CssClass="form-control" TabIndex="7" disabled></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFollowupDate" runat="server" class="control-label required-field" Text="Follow Up Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFollowupDate" runat="server" CssClass="form-control" TabIndex="12"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblFollowupTime" runat="server" class="control-label" Text="Probable Time"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtProbableFollowupHour" placeholder="12" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                                <%-- <asp:TextBox ID="txtProbableFollowupMinute" placeholder="00" CssClass="CustomMinuteSize"
                                TabIndex="3" runat="server" disabled></asp:TextBox>
                            <asp:DropDownList ID="ddlProbableFollowupAMPM" CssClass="CustomAMPMSize" runat="server"
                                TabIndex="4">
                                <asp:ListItem>AM</asp:ListItem>
                                <asp:ListItem>PM</asp:ListItem>
                            </asp:DropDownList>
                            (12:00AM)--%>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCity" runat="server" class="control-label required-field" Text="City Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCity" TabIndex="2" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblLocation" runat="server" class="control-label required-field" Text="Teritory / Area"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlLocation" TabIndex="2" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFollowupType" runat="server" class="control-label required-field" Text="Follow-Up Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlFollowupType" TabIndex="2" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblpurpose" runat="server" class="control-label required-field" Text="Purpose"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPurpose" TabIndex="2" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>


                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="CI Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCIType" TabIndex="2" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Action Plan"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActionPlan" TabIndex="2" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Opportunity Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlOpportunityStatus" TabIndex="2" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                            </div>
                        </div>

                        <div class="form-group" style="display: none;">
                            <div id="OthersFollowup">
                                <div class="col-md-2">
                                    <asp:Label ID="lblFollowupName" runat="server" class="control-label" Text="Follow-Up Name"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtFollowupName" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                                </div>
                            </div>
                            <div id="OthersPurpose">
                                <div class="col-md-2">
                                    <asp:Label ID="lblPurposeName" runat="server" class="control-label" Text="Purpose Name"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPurposeName" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
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
                <div class="panel-heading">Search Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSCompany" runat="server" class="control-label" Text="Company Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <input id="txtSCompanySearch" type="text" class="form-control" />
                                <div style="display: none;">
                                    <asp:DropDownList ID="ddlSCompany" TabIndex="1" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSFromInitialDate" runat="server" class="control-label" Text="From Initial Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSFromInitialDate" runat="server" CssClass="form-control" TabIndex="12"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSToInitialDate" runat="server" class="control-label" Text="To Initial Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSToInitialDate" runat="server" CssClass="form-control" TabIndex="12"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSFromFollowupDate" runat="server" class="control-label" Text="From Followup Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSFromFollowupDate" runat="server" CssClass="form-control" TabIndex="12"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSToFollowupDate" runat="server" class="control-label" Text="To Followup Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSToFollowupDate" runat="server" CssClass="form-control" TabIndex="12"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSFollowupType" runat="server" class="control-label" Text="Followup Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSFollowupType" CssClass="form-control" TabIndex="2" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSPurpose" runat="server" class="control-label" Text="Purpose"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSPurpose" CssClass="form-control" TabIndex="2" runat="server">
                                </asp:DropDownList>
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
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">Search Information</div>
                <div class="panel-body">
                    <table id='gvSalesCallInformation' class="table table-bordered table-condensed table-responsive" width="100%">
                        <colgroup>
                            <col style="width: 50%;" />
                            <col style="width: 20%;" />
                            <col style="width: 20%;" />
                            <col style="width: 10%;" />
                            <%--<col style="width: 15%;" />--%>
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>Company Name
                                </td>
                                <td>Initial Date
                                </td>
                                <td>Followup Date
                                </td>
                                <td style="text-align: right;">Edit
                                </td>
                                <%--<td style="text-align: right;">
                                    Delete
                                </td>--%>
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
