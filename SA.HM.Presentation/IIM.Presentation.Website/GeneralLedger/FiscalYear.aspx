<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="FiscalYear.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.FiscalYear" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var ProjectTable;
        var ProjectIdList = [];
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Accounts</a>";
            var formName = "<span class='divider'>/</span><li class='active'>GL Configuration</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            ProjectTable = $("#tblProject").DataTable({
                data: [],
                columns: [
                    { title: "<input id='chkAll' type='checkbox'></input>", data: null, width: "5%" },
                    { title: "", data: "CompanyId", visible: false },
                    { title: "Company Name", data: "GLCompany", width: "47.5%" },
                    { title: "", data: "ProjectId", visible: false },
                    { title: "Project Name", data: "Name", width: "47.5%" },
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
            PopulateProjects();
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
        function PopulateProjects() {
            $.ajax({
                type: "POST",
                url: "../GeneralLedger/FiscalYear.aspx/GetAllGLProjects",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    OnProjectsPopulated(response);
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
        }
        function OnProjectsPopulated(response) {
            //$("#" + Project).attr("disabled", false);
            ProjectTable.clear();
            ProjectTable.rows.add(response.d);
            ProjectTable.draw();
            return false;
        }
        function ValidateAndSaveDataToHiddenfield() {

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
        function GetAllGLFiscalYearInfo() {
            return $.ajax({
                type: "POST",
                url: "/GeneralLedger/FiscalYear.aspx/GetAllGLFiscalYearInfo",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
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
        function UpdateFiscalYear(fiscalYearId) {
            $.ajax({
                type: "POST",
                url: "/GeneralLedger/FiscalYear.aspx/GetFiscalYearInfoByFiscalYearId",
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
            //var table = $("#tblProject tbody");
            for (var i = 0; i < ProjectTable.data().length; i++) {
                projectId = $.trim(ProjectTable.cell(i, 3).nodes().to$().text());
                for (var i = 0; i < ProjectTable.data().length; i++) {
                    var projectId = $.trim(ProjectTable.cell(i, 3).nodes().to$().text());
                    for (var j = 0; j < ProjectIdList.length; j++) {
                        if (projectId == ProjectIdList[j]) {
                            ProjectTable.cell(i, 0).nodes().to$().find("input").prop("checked", true);
                        }
                    }
                }
                $("#ContentPlaceHolder1_txtFiscalYearName").val(FiscalYear.FiscalYearName);
                $("#ContentPlaceHolder1_txtMonthSetupId").val(FiscalYear.FiscalYearId);
                $("#ContentPlaceHolder1_txtStartDate").val(CommonHelper.DateFromStringToDisplay(FiscalYear.FromDate, innBoarDateFormat));
                $("#ContentPlaceHolder1_txtEndDate").val(CommonHelper.DateFromStringToDisplay(FiscalYear.ToDate, innBoarDateFormat));
                $("#ContentPlaceHolder1_btnMonthSave").val("Update");
                $("#myTabs").tabs({ active: 0 });
            }
        }
        $(function () {
            $("#myTabs").tabs();
        });
    </script>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfProjectIdList" runat="server" />
    <asp:HiddenField ID="txtMonthSetupId" runat="server"></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Fiscal Year Information</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Fiscal Year</a></li>
        </ul>
        <div id="tab-1">
            <div id="AccFirstMonthPanel" class="panel panel-default">
                <div class="panel-heading">
                    Fiscal Year Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblFiscalYearName" runat="server" class="control-label" Text="Fiscal Year Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtFiscalYearName" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                        <table id="tblProject" class="table table-bordered table-responsive">
                        </table>
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
        <div id="tab-2">
            <div class="row">
                <div class="col-md-12">
                    <table id="tblFiscalYear" class="table table-bordered table-responsive" style="width: 100%;">
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
