<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmGLConfiguration.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.frmGLConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var FiscalYearTable;
        var ProjectTable;
        var ProjectIdList = [];
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Accounts</a>";
            var formName = "<span class='divider'>/</span><li class='active'>GL Configuration</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            var txtStartDate = '<%=txtStartDate.ClientID%>'
            var txtEndDate = '<%=txtEndDate.ClientID%>'
            $('#' + txtStartDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtEndDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtEndDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtStartDate).datepicker("option", "maxDate", selectedDate);
                }
            });
        });

        var txtAccountHead = '<%=txtAccountHead.ClientID%>'

        $(document).ready(function () {
            var ddlAccMonthCompany = '<%=ddlAccMonthCompany.ClientID%>'
            var ddlAccMonthProject = '<%=ddlAccMonthProject.ClientID%>'


            var siddlAccMonthProject = parseFloat($('#' + ddlAccMonthCompany).prop("selectedIndex"));
            if (siddlAccMonthProject > 0) {
                $("#" + ddlAccMonthProject).attr("disabled", false);
            }
            else {
                $("#" + ddlAccMonthProject).attr("disabled", true);
            }
            $("#" + ddlAccMonthProject).empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');

            var ddlVCompany = '<%=ddlVCompany.ClientID%>'
            var ddlVProject = '<%=ddlVProject.ClientID%>'
            var ddlVoucherNumber = '<%=ddlVoucherNumber.ClientID%>'

            $("#" + ddlVProject).attr("disabled", true);
            $("#" + ddlVProject).empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');

            var ddlCompany = '<%=ddlCompany.ClientID%>'
            var ddlProject = '<%=ddlProject.ClientID%>'
            $("#" + ddlProject).attr("disabled", true);
            $("#" + ddlProject).empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');

            $("#" + ddlAccMonthCompany).change(function () {
                PopulateProjects(ddlAccMonthCompany, ddlAccMonthProject)
            });

            $("#" + ddlAccMonthProject).change(function () {
                var projectId = $("#<%=ddlAccMonthProject.ClientID %>").val();
                if (projectId > 0) {
                    PageMethods.CheckingFiscalYaerExist(projectId, OnCheckingFiscalYaerExistSucceeded, OnCheckingFiscalYaerExistFailed);
                }
                else {
                    $("#<%=txtFiscalYearName.ClientID %>").val("");
                    $("#<%=txtStartDate.ClientID %>").val("");
                    $("#<%=txtEndDate.ClientID %>").val("");
                    $("#<%=btnMonthSave.ClientID %>").val("Save");
                }
                return false;
            });


            $("#" + ddlVCompany).change(function () {
                PopulateProjects(ddlVCompany, ddlVProject)
            });

            $("#" + ddlVProject).change(function () {
                PopulateVoucher(ddlVProject, ddlVoucherNumber)
            });


            $("#" + ddlProject).change(function () {
                LoadGridInformation();
            });

            function OnCheckingFiscalYaerExistSucceeded(result) {
                if (result == "1") {

                    $.confirm({
                        title: 'Confirm!',
                        content: 'Do you want to save new Fiscal Year?',
                        buttons: {
                            confirm: function () {
                                $("#<%=txtFiscalYearName.ClientID %>").val("");
                                $("#<%=txtMonthSetupId.ClientID %>").val("");
                                $("#<%=txtStartDate.ClientID %>").val("");
                                $("#<%=txtEndDate.ClientID %>").val("");
                                $("#<%=btnMonthSave.ClientID %>").val("Save");
                            },
                            cancel: function () {
                                PopulateFiscalYearInfo(ddlAccMonthProject);
                            }
                        }
                    });
                }
                else {
                    $("#<%=txtFiscalYearName.ClientID %>").val("");
                    $("#<%=txtStartDate.ClientID %>").val("");
                    $("#<%=txtEndDate.ClientID %>").val("");
                    $("#<%=btnMonthSave.ClientID %>").val("Save");
                }

                return false;
            }
            function OnCheckingFiscalYaerExistFailed(error) {
                toastr.error(error.get_message());
            }

            function LoadGridInformation() {
                var projectId = $("#<%=ddlProject.ClientID %>").val();
                PageMethods.GenerateGridForMapping(projectId, OnLoadObjectSucceeded, OnLoadObjectFailed);
                return false;
            }
            function OnLoadObjectSucceeded(result) {
                $("#ltlAccountMaping").html(result);
                return false;
            }
            function OnLoadObjectFailed(error) {
                toastr.error(error.get_message());
            }

            function PerformDeleteAction(actionId) {

                $.confirm({
                    title: 'Confirm!',
                    content: 'Do you want to delete this record?',
                    buttons: {
                        confirm: function () {
                            PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
                        },
                        cancel: function () {
                        }
                    }
                });

                return false;
            }

            function OnDeleteObjectSucceeded(result) {
                LoadGridInformation();
            }

            function OnDeleteObjectFailed(error) {
                toastr.error(error.get_message());
            }

            $("#" + ddlCompany).change(function () {
                PopulateProjects(ddlCompany, ddlProject)
            });

            var projectId = $("#<%=ddlProject.ClientID %>").val();
            var accountType = $("#<%=ddlAccountType.ClientID %>").val();
            $('#' + projectId).change(function () {
                $("#<%=txtSaveValidation.ClientID %>").val('');
            });
            $('#' + accountType).change(function () {
                $("#<%=txtSaveValidation.ClientID %>").val('');
            });

            SearchText();
            var ddlNodeId = '<%=ddlHeadId.ClientID%>'
            $('#' + txtAccountHead).blur(function () {
                SearchTextForId();
            });

            FiscalYearTable = $("#tblFiscalYear").DataTable({

                data: [],
                columns: [
                    { title: "", data: "FiscalYearId", visible: false },
                    { title: "Name", data: "FiscalYearName", width: "60%" },
                    { title: "From Date", data: "FromDate", width: "15%" },
                    { title: "To Date", data: "ToDate", width: "15%" },
                    { title: "Action", "data": null, sWidth: '10%' },
                ],
                rowCallback: (row, data, displayNum, displayIndex, dataIndex) => {
                    var tableRow = "";

                    if (displayIndex % 2 == 0) {
                        $('td', row).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', row).css('background-color', '#FFFFFF');
                    }

                    tableRow += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return UpdateFiscalYear('" + data.FiscalYearId + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";

                    //tableRow += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'ShowQuotationInvoice(" + data.QuotationId + ")' title='Quotation Invoice' ><img style='width:16px;height:16px;' alt='Quotation Invoice' src='../Images/ReportDocument.png' /></a>";

                    $('td:eq(' + (row.children.length - 1) + ')', row).html(tableRow);
                },
                columnDefs: [
                    {
                        "targets": 2,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            return CommonHelper.DateFromStringToDisplay(data);
                        }
                    }, {
                        "targets": 3,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            return CommonHelper.DateFromStringToDisplay(data);
                        }
                    }],
                info: false,
                ordering: false,
                processing: false,
                paging: false,
                filter: false,
                language: {
                    emptyTable: "No Data Found"
                }
            });
            ProjectTable = $("#tblProject").DataTable({
                data: [],
                columns: [
                    { title: "<input id='chkAll' type='checkbox'></input>", data: null, width: "5%" },
                    { title: "", data: "ProjectId", visible: false },
                    { title: "Project Name", data: "Name", width: "95%" },
                ],
                rowCallback: (row, data, displayNum, displayIndex, dataIndex) => {
                    var tableRow = "";

                    if (displayIndex % 2 == 0) {
                        $('td', row).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', row).css('background-color', '#FFFFFF');
                    }

                },
                columnDefs: [
                    {
                        "targets": 0,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            if (ProjectIdList.includes(data.ProjectId)) {
                                return '<input type="checkbox" checked="checked" />'
                            }
                            else return '<input type="checkbox" />';
                        }
                    }],
                info: false,
                ordering: false,
                processing: false,
                paging: false,
                filter: false,
                language: {
                    emptyTable: "No Data Found"
                }
            });

            GetAllGLFiscalYearInfo();

            $("[id=chkAll]").on("change", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#tblProject tbody tr").find("td:eq(0)").find("input").prop("checked", true);
                }
                else {
                    $("#tblProject tbody tr").find("td:eq(0) ").find("input").prop("checked", false);
                }
            });

        });

        $(function () {
            $("#myTabs").tabs();
        });
        //---------------------------

        //

        function UpdateFiscalYear(fiscalYearId) {
            $.ajax({
                type: "POST",
                url: "/GeneralLedger/frmGLConfiguration.aspx/GetFiscalYearInfoByFiscalYearId",
                contentType: "application/json; charset=utf-8",
                data: '{fiscalYearId: ' + fiscalYearId + '}',
                dataType: "json",
                success: function (response) {
                    LoadFiscalYearInfo(response.d);
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
        }

        function LoadFiscalYearInfo(FiscalYear) {
            ProjectIdList = FiscalYear.GLFiscalYearProjects.map(i => { return i.ProjectId });
            $("#ContentPlaceHolder1_ddlAccMonthCompany").val(FiscalYear.CompanyId).trigger('change');
            $("#ContentPlaceHolder1_txtFiscalYearName").val(FiscalYear.FiscalYearName);
            $("#ContentPlaceHolder1_txtMonthSetupId").val(FiscalYear.FiscalYearId);
            $("#ContentPlaceHolder1_txtStartDate").val(CommonHelper.DateFromStringToDisplay(FiscalYear.FromDate, innBoarDateFormat));
            $("#ContentPlaceHolder1_txtEndDate").val(CommonHelper.DateFromStringToDisplay(FiscalYear.ToDate, innBoarDateFormat));
            $("#ContentPlaceHolder1_btnMonthSave").val("Update");
            $("#myTabs").tabs({ active: 0 });
        }
        function GetAllGLFiscalYearInfo() {
            return $.ajax({
                type: "POST",
                url: "/GeneralLedger/frmGLConfiguration.aspx/GetAllGLFiscalYearInfo",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    FiscalYearTable.clear();
                    FiscalYearTable.rows.add(response.d);
                    FiscalYearTable.draw();
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
        }
        function PopulateVoucher(ddlVProject, ddlVoucherNumber) {
            $.ajax({
                type: "POST",
                url: "/GeneralLedger/frmGLConfiguration.aspx/PopulateVoucher",
                data: '{projectId: ' + $("#" + ddlVProject).val() + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    OnVoucherPopulated(response, ddlVoucherNumber);
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
        }
        function OnVoucherPopulated(response, ddlVoucherNumber) {
            $("#" + ddlVoucherNumber).val(response.d.SetupValue);
            var txtVoucherSetupId = '<%=txtVoucherSetupId.ClientID%>'
            $("#" + txtVoucherSetupId).val("");
            $("#" + txtVoucherSetupId).val(response.d.SetupId);

            if (response.d.SetupId != "" || response.d.SetupId != 0) {

                var btnVoucherSave = '<%=btnVoucherSave.ClientID%>'
                $("#" + btnVoucherSave).val("Update");
            }

        }

        function PopulateFiscalYearInfo(Project) {
            $.ajax({
                type: "POST",
                url: "/GeneralLedger/frmGLConfiguration.aspx/PopulateGLFiscalYearInfo",
                data: '{projectId: ' + $("#" + Project).val() + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    OnFiscalYearPopulated(response);
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
        }
        function OnFiscalYearPopulated(response) {
            //alert(response.d.FiscalYearId);
            //$("#" + ddlAccMonth).val(response.d.SetupValue);
            var txtMonthSetupId = '<%=txtMonthSetupId.ClientID%>'
            $("#" + txtMonthSetupId).val("")
            $("#" + txtMonthSetupId).val(response.d.FiscalYearId)

            //alert(response.d.FromDate);
            var fromDate = GetDateTimeFromString(response.d.FromDate);
            var toDate = GetDateTimeFromString(response.d.ToDate);

            //fromDate = GetStringFromDateTime(fromDate);
            //toDate = GetStringFromDateTime(toDate);


            //if (response.d.FiscalYearId != "" || response.d.FiscalYearId != 0) {
            if (response.d.FiscalYearId > 0) {
                $("#<%=txtFiscalYearName.ClientID %>").val(response.d.FiscalYearName);
                $("#<%=txtStartDate.ClientID %>").val(response.d.FromDateForClientSideShow);
                $("#<%=txtEndDate.ClientID %>").val(response.d.TodateForClientSideShow);

                var btnMonthSave = '<%=btnMonthSave.ClientID%>'
                $("#" + btnMonthSave).val("Update");
            }
        }

        function PopulateProjects(Company, Project) {
            if ($("#" + Company).val() == "0") {
                $("#" + Project).empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                ProjectTable.clear().draw();
            }
            else {
                $("#" + Project).empty().append('<option selected="selected" value="0">Loading...</option>');
                $.ajax({
                    type: "POST",
                    url: "/GeneralLedger/frmGLConfiguration.aspx/GetGLProjectInfoByGLCompany",
                    data: '{companyId: ' + $("#" + Company).val() + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        OnProjectsPopulated(response, Project);
                    },
                    failure: function (response) {
                        toastr.error(response.d);
                    }
                });
            }
        }

        function OnProjectsPopulated(response, Project) {
            //$("#" + Project).attr("disabled", false);
            ProjectTable.clear();
            ProjectTable.rows.add(response.d);
            ProjectTable.draw();
            //PopulateControl(response.d, $("#" + Project), $("#<%=CommonDropDownHiddenField.ClientID %>").val());
        }

        function SearchTextForId() {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/GeneralLedger/frmGLConfiguration.aspx/FillForm",
                data: "{'searchText':'" + $('#' + txtAccountHead).val() + "'}",
                dataType: "json",
                success: function (data) {
                    var ddlHeadId = '<%=ddlHeadId.ClientID%>'
                    $('#' + ddlHeadId).val(data.d);
                },
                error: function (result) {
                    //alert("Error");
                }
            });
        }
        //---------------------------
        function SearchText() {
            $('.ThreeColumnTextBox').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "/GeneralLedger/frmGLConfiguration.aspx/GetAutoCompleteData",
                        data: "{'searchText':'" + $('#' + txtAccountHead).val() + "'}",
                        dataType: "json",
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                }
            });
        }

        $(document).ready(function () {

        });
        //For FillForm-------------------------

        function SaveValidation() {
            var projectId = $("#<%=ddlProject.ClientID %>").val();
            var accountType = $("#<%=ddlAccountType.ClientID %>").val();
            var txtSaveValidation = $("#<%=txtSaveValidation.ClientID %>").val();
            if (txtSaveValidation == '') {
                PageMethods.GetSaveValidation(projectId, accountType, OnValidationObjectSucceeded, OnValidationObjectFailed);
            }

            if (txtSaveValidation == "Update" || txtSaveValidation == "Save") {
                return true;
            }
            else {

                return false;
            }
        }
        function OnValidationObjectSucceeded(result) {

            if (result.NodeId > 0) {
                $.confirm({
                    title: 'Confirm!',
                    content: 'Do you want to Change Current Record?',
                    buttons: {
                        confirm: function () {
                            $("#<%=txtSaveValidation.ClientID %>").val("Update");
                            $("#<%=txtConfigurationId.ClientID %>").val(result.ConfigurationId);
                            $("#<%=btnSave.ClientID %>").click();
                        },
                        cancel: function () {
                            $("#<%=txtSaveValidation.ClientID %>").val("Cancel");
                        }
                    }
                });
            }
            else {
                $("#<%=txtSaveValidation.ClientID %>").val("Save");
                $("#<%=btnSave.ClientID %>").click();
            }
        }

        function OnValidationObjectFailed(error) {
            toastr.error(error.get_message());
        }


        //For Delete-------------------------        
        function PerformDeleteAction(actionId) {

            $.confirm({
                title: 'Confirm!',
                content: 'Do you want to delete this record?',
                buttons: {
                    confirm: function () {
                        PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
                    },
                    cancel: function () {
                    }
                }
            });

            return false;
        }


        function OnDeleteObjectSucceeded(result) {
            window.location = "frmGLConfiguration.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=ddlAccountType.ClientID %>").val(0);
            $("#<%=txtAccountHead.ClientID %>").val('');
            $("#<%=ddlProject.ClientID %>").val(0);
            $("#<%=txtConfigurationId.ClientID %>").val('');
            $("#<%=ddlCompany.ClientID %>").val(0);
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewAccountHead').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewAccountHead').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }
        function ValidateAndSaveDataToHiddenfield() {

            var companyName = $("#ContentPlaceHolder1_ddlAccMonthCompany").val();
            if (companyName == "0") {
                toastr.warning("Select Company.");
                $("#ContentPlaceHolder1_ddlAccMonthCompany").focus();
                return false;
            }

            var fiscalYearName = $("#ContentPlaceHolder1_txtFiscalYearName").val();
            if (!fiscalYearName) {
                toastr.warning("Enter Fiscal Year Name.");
                $("#ContentPlaceHolder1_txtFiscalYearName").focus();
                return false;
            }

            var startDate = $("#ContentPlaceHolder1_txtStartDate").val();
            if (!startDate) {
                toastr.warning("Enter Start Date.");
                $("#ContentPlaceHolder1_txtStartDate").focus();
                return false;
            }

            var toDate = $("#ContentPlaceHolder1_txtEndDate").val();
            if (!toDate) {
                toastr.warning("Enter To Date.");
                $("#ContentPlaceHolder1_txtEndDate").focus();
                return false;
            }

            var projectId;
            ProjectIdList = [];
            $("#tblProject tbody tr").find("td:eq(0)").find("input:checked").each(function (index, row) {

                projectId = ProjectTable.row($(this).parent().parent()).data().ProjectId;
                ProjectIdList.push(projectId);
            });
            if (ProjectIdList.length == 0) {
                toastr.warning("Select At least One Project.");
                return false;
            }

            $("#ContentPlaceHolder1_hfProjectIdList").val(ProjectIdList.toString());
            return true;
        }
    </script>
        <div id="AccFirstMonthPanel" class="panel panel-default" style="display: none">
            <div class="panel-heading">
                Fiscal Year Information
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <div class="col-md-4">
                                    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="txtMonthSetupId" runat="server"></asp:HiddenField>
                                    <asp:Label ID="lblAccMonthCompany" runat="server" class="control-label" Text="Company"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlAccMonthCompany" runat="server" CssClass="form-control"
                                        TabIndex="1">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-4">
                                    <asp:Label ID="lblFiscalYearName" runat="server" class="control-label" Text="Fiscal Year Name"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtFiscalYearName" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-4">
                                    <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-4">
                                    <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <div class="col-md-4">
                                    <asp:Label ID="lblAccMonthProject" runat="server" class="control-label" Text="Project"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:DropDownList Style="display: none;" ID="ddlAccMonthProject" runat="server" CssClass="form-control"
                                        OnSelectedIndexChanged="ddlAccMonthProject_SelectedIndexChanged" TabIndex="2">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hfProjectIdList" runat="server" />
                                    <div class="panel panel-default">
                                        <div class="panel-body">
                                            <table id="tblProject" class="table table-bordered table-responsive">
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnMonthSave" runat="server" Text="Save" CssClass="btn btn-primary btn-sm"
                                OnClientClick="return ValidateAndSaveDataToHiddenfield();" OnClick="btnMonthSave_Click" TabIndex="4" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="Div1" class="panel panel-default" style="display: none;">
            <div class="panel-heading">
                Accounts Approval Configuration Information
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label1" runat="server" class="control-label" Text="Checked By"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlAccountsCheckedBy" runat="server" CssClass="form-control"
                                TabIndex="1">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="Label2" runat="server" class="control-label" Text="Approved By"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlAccountsApprovedBy" runat="server" CssClass="form-control"
                                TabIndex="1">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnGLApproval" runat="server" Text="Save" CssClass="btn btn-primary btn-sm"
                                TabIndex="4" OnClick="btnGLApproval_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="Div2" class="panel panel-default" runat="server">
            <div class="panel-heading">
                GL Configuration
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                        </div>
                        <div class="col-md-8">
                            <div style="float: left;">
                                <asp:HiddenField ID="hfIsShowReferenceVoucherNumber" runat="server"></asp:HiddenField>
                                <asp:CheckBox ID="IsShowReferenceVoucherNumber" TabIndex="5" runat="Server" Text=""
                                    Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                &nbsp;&nbsp;Is Show Reference Voucher Number?
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnSaveGLConfig" runat="server" Text="Update" TabIndex="6" CssClass="btn btn-primary btn-sm"
                                OnClick="btnSaveGLConfig_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:Panel ID="pnlVoucherNumberPanel" runat="server">
            <div id="VoucherNumberPanel" class="panel panel-default">
                <div class="panel-heading">
                    Voucher Number Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="txtVoucherSetupId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblVCompany" runat="server" class="control-label" Text="Company"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlVCompany" runat="server" CssClass="form-control" TabIndex="5">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblVProject" runat="server" class="control-label" Text="Project"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlVProject" runat="server" CssClass="form-control" TabIndex="6">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblVoucherNumber" runat="server" class="control-label" Text="Number"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlVoucherNumber" runat="server" CssClass="form-control" TabIndex="7">
                                    <asp:ListItem Value="Auto">Auto Generated</asp:ListItem>
                                    <asp:ListItem Value="Manual">Manual</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnVoucherSave" runat="server" Text="Save" CssClass="btn btn-primary btn-sm"
                                    OnClick="btnVoucherSave_Click" TabIndex="8" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    <div id="tab-2" style="display:none">
        <div class="row">
            <div class="col-md-12">
                <table id="tblFiscalYear" class="table table-bordered table-responsive" style="width: 100%;">
                </table>
            </div>
        </div>
    </div>
    <div style="display: none;">
        <div id="EntryPanel" class="block">
            <a href="#page-stats" class="block-heading" data-toggle="collapse">Account Configuration</a>
            <div class="HMBodyContainer">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:HiddenField ID="txtSaveValidation" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="txtConfigurationId" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblCompany" runat="server" Text="Company"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblProject" runat="server" Text="Project"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlProject" runat="server" TabIndex="2">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="form-group" style="display: none">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlHeadId" CssClass="ThreeColumnDropDownList" runat="server"
                            TabIndex="3">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblAccountHead" runat="server" Text="Account Head"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtAccountHead" CssClass="ThreeColumnTextBox" runat="server" TabIndex="4"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblAccountType" runat="server" Text="Account Type"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlAccountType" runat="server" TabIndex="5">
                            <asp:ListItem Value="CP"> Cash Payment (CP)</asp:ListItem>
                            <asp:ListItem Value="BP">Bank Payment (BP)</asp:ListItem>
                            <asp:ListItem Value="CR"> Cash Receive (CR)</asp:ListItem>
                            <asp:ListItem Value="BR">Bank Receive (BR)</asp:ListItem>
                            <asp:ListItem Value="JV"> Journal Voucher (JV)</asp:ListItem>
                            <asp:ListItem Value="CV">Contra Voucher (CV)</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="HMContainerRowButton">
                    <%--Right Left--%>
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="TransactionalButton btn btn-primary"
                        TabIndex="6" />
                    <asp:Button ID="btnClear" runat="server" TabIndex="7" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                        OnClientClick="javascript: return PerformClearAction();" />
                    <asp:Button ID="btnCancel" runat="server" Text="Close" TabIndex="8" CssClass="TransactionalButton btn btn-primary"
                        OnClientClick="javascript: return EntryPanelVisibleFalse();" />
                </div>
            </div>
        </div>
        <div class="divClear">
        </div>
        <div id="SearchPanel" class="block">
            <div class="block-body collapse in">
                <div id="ltlAccountMaping">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
