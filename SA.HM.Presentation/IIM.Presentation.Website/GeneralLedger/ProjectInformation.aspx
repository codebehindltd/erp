<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="ProjectInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.ProjectInformation" EnableEventValidation="false" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/Scripts/jsgantt.js"></script>
    <link rel="stylesheet" href="/Content/jsgantt.css" />
    <%--<script language="javascript" src="graphics.js"></script>--%>
    <script>
        var projectId = "", companyId = "", contactId = "";
        var DealContacts;
        var TaskTable;
        var g;
        $(document).ready(function () {

            $("#myTabs").tabs();
            
            TaskTable = $("#tblTask").DataTable({
                data: [],
                columns: [
                    { title: "", "data": "Id", visible: false },
                    { title: "Task Name", "data": "TaskName", sWidth: '70%' },
                    { title: "Start Date", "data": "TaskDate", sWidth: '10%' },
                    { title: "Estimated Done Date", "data": "EstimatedDoneDate", sWidth: '10%' },
                    { title: "Action", "data": null, sWidth: '10%' }
                ],
                columnDefs: [

                    {
                        "targets": 1,
                        "className": "left",
                        "render": function (data, type, full, meta) {
                            return "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'PerformProjectDetails(this)' >" + data + "</a>";
                        }
                    },

                    {
                        "targets": 2,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            return CommonHelper.DateFromStringToDisplay(data, innBoarDateFormat);
                        }
                    },
                    {
                        "targets": 3,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            return CommonHelper.DateFromStringToDisplay(data, innBoarDateFormat);
                        }
                    }],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var row = '';
                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }
                    row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return EditTask('" + aData.Id + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";
                    $('td:eq(' + (nRow.children.length - 1) + ')', nRow).html(row);
                },
                pageLength: UserInfoFromDB.GridViewPageSize,
                filter: false,
                info: false,
                ordering: false,
                processing: true,
                retrieve: true,
                bAutoWidth: false,
                bLengthChange: false,
                bInfo: false,
                bPaginate: false,
                language: {
                    emptyTable: "No Data Found"
                },

            });

            $('#ContentPlaceHolder1_txtReportStartDate').datepicker({

                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtReportEndDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtReportEndDate').datepicker({

                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtReportStartDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            var ddlSearchType = '<%=ddlSearchType.ClientID%>'
            var searchType = $('#' + ddlSearchType).val();

            if (searchType == 1) {
                $('#FiscalYearPanel').show();
                $('#DateRangePanel').hide();
                PopulateFiscalYear();
            }
            else if (searchType == 2) {
                $('#DateRangePanel').show();
                $('#FiscalYearPanel').hide();
            }
            
            var ddlSearchType = '<%=ddlSearchType.ClientID%>'
            $('#' + ddlSearchType).change(function () {

                var searchType = $('#' + ddlSearchType).val();
                if (searchType == 0) {
                    toastr.warning('Please Select Search Type');
                    $('#FiscalYearPanel').hide();
                    $('#DateRangePanel').hide();
                }
                else if (searchType == 1) {
                    $('#FiscalYearPanel').show();
                    $('#DateRangePanel').hide();
                    PopulateFiscalYear();
                }
                else if (searchType == 2) {
                    $('#DateRangePanel').show();
                    $('#FiscalYearPanel').hide();
                }
            });
            var itsPostBack = <%= Page.IsPostBack ? "true" : "false" %>;
            
            projectId = parseInt($.trim(CommonHelper.GetParameterByName("pid")), 10);
            ChangeReportHeader($("#ContentPlaceHolder1_ddlReportType"));
            
            if (projectId > 0)
            {
                LoadGanttChart(projectId)
                LoadStockInfoReport(projectId);
                SearchTask(1, 1);
            }
                
            $("#SalesNoteDialog").dialog(opt).dialog("open");
            
        });
        $(function () {
            $("#myTabs").tabs({
                activate: function (event, ui) {
                    LoadGanttChart(projectId);
                }
            });
        });
        
        function EditTask(id) {
            var iframeid = 'formTask';
            var url = "../TaskManagement/AssignTaskIFrame.aspx?tid=" + id +"&pid=" + projectId;
            document.getElementById(iframeid).src = url;

            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: 1100,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: name,
                show: 'slide',
                Open: function (event, ui) {
                    $("#frmTask").show();
                    $("#frmTask").attr("src", url);
                },
                Close: function () {
                    $(this).dialog('close');
                }
            });

            return false;
        }
        function PopulateFiscalYear() {
            $('#<%=ddlFiscalYear.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            var projectId = parseInt($.trim(CommonHelper.GetParameterByName("pid")), 10);
            $.ajax({
                type: "POST",
                url: "./frmGLProject.aspx/PopulateFiscalYear",
                data: '{projectId: ' + projectId + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: OnFiscalYearPopulated,
                failure: function (response) {
                    toastr.error(response.d);
                }
            });

        }

        function OnFiscalYearPopulated(response) {
            PopulateControl(response.d, $("#<%=ddlFiscalYear.ClientID %>"), $("#<%=CommonDropDownHiddenFieldForPleaseSelect.ClientID %>").val());
            if ($("#ContentPlaceHolder1_hfFiscalYearId").val() != "0")
                $("#<%=ddlFiscalYear.ClientID %>").val($("#ContentPlaceHolder1_hfFiscalYearId").val());
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchTask(pageNumber, IsCurrentOrPreviousPage);
        }

        function PerformProjectDetails(item) {

            var row = $(item).parents('tr');
            var id = TaskTable.row(row).data().Id;
            taskId = id;
            var name = TaskTable.row(row).data().TaskName;

            var iframeid = 'frmTask';
            var url = "../TaskManagement/TaskDetails.aspx?tid=" + id;
            document.getElementById(iframeid).src = url;

            $("#TaskDialogue").dialog({
                autoOpen: true,
                modal: true,
                width: 1100,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: name,
                show: 'slide'
            });

            return false;

        }


        function SearchTask(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = TaskTable.data().length;
            var projectId = parseInt($.trim(CommonHelper.GetParameterByName("pid")), 10);

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../../../GeneralLedger/ProjectInformation.aspx/LoadTaskByProjectId",
                dataType: "json",
                data: JSON.stringify({ projetcId: projectId, gridRecordsCount: gridRecordsCount, pageNumber: pageNumber, isCurrentOrPreviousPage: IsCurrentOrPreviousPage }),
                async: false,
                success: (data) => {
                    OnTaskLoadSucceed(data.d);
                },
                error: (error) => {
                    OnTaskLoadingFailed(error);
                }
            });
            return false;
        }

        function OnTaskLoadSucceed(result) {

            TaskTable.clear();
            TaskTable.rows.add(result);
            TaskTable.draw();

            return false;
        }

        function OnTaskLoadingFailed(error) {
            toastr.error(error.get_message());
            return false;
        }

        function AttachFile() {
            var randomId = projectId;
            var path = "/GeneralLedger/File/GLProject/";
            var category = "GLProjectDocument";
            var iframeid = 'Iframe1';
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
        function UploadComplete() {
            var id = +$("#ContentPlaceHolder1_hfProjectId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            PageMethods.LoadProjectDocument(id, deletedDoc, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }

        function OnLoadDocumentSucceeded(result) {
            var guestDoc = result;
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";

            if (totalDoc > 0) {
                guestDocumentTable += "<table id='dealDocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
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
                    guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteProjetcDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                    guestDocumentTable += "</td>";
                    guestDocumentTable += "</tr>";
                }
                guestDocumentTable += "</table>";

                $("#ContentPlaceHolder1_ProjectDocumentInfo").html(guestDocumentTable);
            }
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }

        function DeleteProjetcDoc(docId, rowIndex) {
            if (confirm("Want to delete?")) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: ".../../../GeneralLedger/ProjectInformation.aspx/DeleteProjectDocument",
                    dataType: "json",
                    data: JSON.stringify({ deletedDocumentId: docId }),
                    async: false,
                    success: (data) => {
                        CommonHelper.AlertMessage(data.d.AlertMessage);
                        $("#trdoc" + rowIndex).remove();

                    },
                    error: (error) => {
                        toastr.error(error, "", { timeOut: 5000 });
                    }
                });
            }
        }

        function SelectFiscalYear(control) {
            $("#ContentPlaceHolder1_hfFiscalYearId").val($(control).val());
        }

        function OpenNotesDetailReport(nodeId, particulars, notes) {

            var startDate = '', endDate = '', fiscalYearId = '';
            var companyId = 0, projectId = 0, donorId = 0, withOrWithoutOpening = '';

            NotesNNodeHead = "Notes(" + notes + ") details Of Account - " + particulars;
            withOrWithoutOpening = $("#ContentPlaceHolder1_hfWithOrWithoutOpening").val();

            if ($("#ContentPlaceHolder1_ddlSearchType").val() == "1") {
                fiscalYearId = $("#ContentPlaceHolder1_ddlFiscalYear").val();
            }
            else if ($("#ContentPlaceHolder1_ddlSearchType").val() == "2") {

                if ($("#ContentPlaceHolder1_txtReportStartDate").val() != "") {
                    startDate = $("#ContentPlaceHolder1_txtReportStartDate").val();
                }

                if ($("#ContentPlaceHolder1_txtReportEndDate").val() != "") {
                    endDate = $("#ContentPlaceHolder1_txtReportEndDate").val();
                }
            }

            if (CommonHelper.GetParameterByName("cid") != "") {
                companyId = CommonHelper.GetParameterByName("cid");
            }

            if (CommonHelper.GetParameterByName("pid") != "") {
                projectId = CommonHelper.GetParameterByName("pid");
            }

            //if ($("#ContentPlaceHolder1_ddlDonor").val() != "") {
            //    donorId = $("#ContentPlaceHolder1_ddlDonor").val();
            //}

            var iframeid = 'printDoc';
            var url = "/GeneralLedger/Reports/frmReportNotesDetails.aspx?nd=" + nodeId + "&sd=" + startDate + "&ed=" + endDate + "&fy=" + fiscalYearId
                + "&cp=" + companyId + "&pj=" + projectId + "&dr=" + donorId + "&wop=" + withOrWithoutOpening;
            parent.document.getElementById(iframeid).src = url;

            $("#DisplayNotesDetails").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 880,
                minHeight: 555,
                width: 'auto',
                closeOnEscape: false,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: NotesNNodeHead,
                show: 'slide'
            });

            return false;
        }
        function ChangeReportHeader(control) {
            var value = $(control).val();
            var selectedOptionText = $(control).find("option:selected").text();
            if (value == "0")
                $('#ReportHeader').html("Report:: Financial Report");
            else
                $('#ReportHeader').html(`Report:: ${selectedOptionText}`);
        }
        function Validate() {
            var ddlSearchType = '<%=ddlSearchType.ClientID%>'
            var searchType = $('#' + ddlSearchType).val();

            if (searchType == 1) {
                if ($("#ContentPlaceHolder1_ddlFiscalYear").val() == "0") {
                    $("#ContentPlaceHolder1_ddlFiscalYear").focus();
                    toastr.warning("Please Select Fiscal Year.");
                    return false;
                }
            }
        }
        function AddNewTask() {
            
            var iframeid = 'formTask';
            var url = "../TaskManagement/AssignTaskIFrame.aspx?pid=" + projectId;
            document.getElementById(iframeid).src = url;
            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "90%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                title: "Task Assign",
                show: 'slide'
            });
            return false;
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }
        function CloseDialog() {
            $("#SalesNoteDialog").dialog('close');
            $("#TaskDialogue").dialog('close');
            return false;
        }

        function GoBack() {
            
            if (confirm("Do you want to go back?")) {

                //var isFirefox = typeof InstallTrigger !== 'undefined';
                //if (isFirefox) {
                //    window.history.go(-2);
                //}
                //else {
                //    window.history.back(-1);
                //}
                window.location.href = "./GLProject.aspx";
            }
            return false;
        }

        function OnLoadProjectByCompanyIdSucceed(result) {

            typesList = [];
            $("#ContentPlaceHolder1_ddlGLProject").empty();
            var i = 0, fieldLength = result.length;

            if (fieldLength > 0) {
                typesList = result;
                $('<option value="' + 0 + '">' + '--- Please Select ---' + '</option>').appendTo('#ContentPlaceHolder1_ddlGLProject');
                for (i = 0; i < fieldLength; i++) {
                    $('<option value="' + result[i].ProjectId + '">' + result[i].Name + '</option>').appendTo('#ContentPlaceHolder1_ddlGLProject');
                }
                //$("#ContentPlaceHolder1_ddlGLProject").attr("Enabled", true);
            }
            else {
                $("<option value='0'>--No Projects Found--</option>").appendTo("#ContentPlaceHolder1_ddlGLProject");
                var company = $("#ContentPlaceHolder1_ddlGLCompany").val();
                $('#ShowChart').hide();
                $('#ContentPlaceHolder1_lblProjectName').text('');
                $('#ContentPlaceHolder1_lblProjectComplete').text('');
                //$("#ContentPlaceHolder1_ddlGLProject").attr("Enabled", false);
                if (company == 0) {
                    toastr.warning("Please select a company.");
                    return false;
                }

            }

            return false;

        }
        function LoadGanttChart(pid) {

            var iframeid = 'GanttIframe';
            var url = "../TaskManagement/GanttChartInformationForProject.aspx?pid=" + pid;
            document.getElementById(iframeid).src = url;

            return false;
        }
        function LoadStockInfoReport(pid) {

            var iframeid = 'StockInfoIframe';
            var url = "../GeneralLedger/Reports/frmReportLocationWiseStockForProject.aspx?pid=" + pid;
            document.getElementById(iframeid).src = url;

            return false;
        }

    </script>
    <div id="DisplayNotesDetails" style="display: none;">
        <iframe id="printDoc" name="printDoc" width="880" height="650" frameborder="0" style="overflow: hidden;"></iframe>
        <div id="bottomPrint">
        </div>
    </div>
    <div style="display: none;">
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000; top: 2000;"
            clientidmode="static"></iframe>
    </div>
    <asp:HiddenField ID="CommonDropDownHiddenFieldForPleaseSelect" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="Iframe1" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="SalesNoteDialog" style="display: none;">
        <iframe id="formTask" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <asp:HiddenField ID="hfProjectId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCompanylId" runat="server" Value="0" />
    <asp:HiddenField ID="hfFiscalYearId" runat="server" Value="0" />

    <asp:HiddenField ID="hfRefPreviousPage" runat="server"></asp:HiddenField>
    <asp:TextBox ID="txtUrl" runat="server" Visible="False"></asp:TextBox>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-md-10" style="font-size: 14px; font-weight: bold;">
                            <asp:Label CssClass="control-label" ID="lblProjectName" runat="server"></asp:Label>
                        </div>
                        <div class="text-right" style="padding: 0px 20px 0px 20px;">
                            <a href="javascript:void();" id="goBackAnchor" onclick="javascript:return GoBack();" style="color: white">Go Back</a>
                        </div>
                    </div>
                </div>
                <div id="myTabs">
                    <ul id="tabPage" class="ui-style">
                        <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                            href="#tab-1">Details</a></li>
                        <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                            href="#tab-2">Customer Info</a></li>
                        <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                            href="#tab-3">Financial Reports</a></li>
                        <li id="D" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                            href="#tab-4">Task</a></li>
                        <li id="F" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                            href="#tab-6">Gantt Chart</a></li>
                        <li id="G" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                            href="#tab-7">Stock Info</a></li>
                        <li id="E" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                            href="#tab-5">Documents</a></li>
                    </ul>
                    <div id="tab-1">
                        <div id="DetailsPanel" class="panel panel-default">
                            <div class="panel-heading">
                                Details Information
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div id="AccountCompanyInfo" class="form-group" runat="server" style="display: none;">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblCompanyId" runat="server" class="control-label required-field" Text="Company Name"></asp:Label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="txtGLCompany" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblCode" runat="server" class="control-label required-field" Text="Project Code"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtCode" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Label ID="lblShortName" runat="server" class="control-label" Text="Short Name"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtShortName" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="Label1" runat="server" class="control-label" Text="Start Date"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtStartDate" Enabled="false" CssClass="datepicker form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Label ID="Label2" runat="server" class="control-label" Text="End Date"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtEndDate" Enabled="false" CssClass="datepicker form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Project Stage"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtProjectStage" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Label ID="Label4" runat="server" class="control-label" Text="Project Amount"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtProjectAmount" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblDescription" runat="server" class="control-label" Text="Description"></asp:Label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="txtDescription" Enabled="false" Height="300px" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="tab-2">
                        <div id="CustomerPanel" class="panel panel-default">
                            <div class="panel-heading">
                                Customer Information
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="col-md-6">
                                        <div class="panel panel-default">
                                            <div class=" panel-heading">
                                                Company Detail
                                            </div>
                                            <div class="panel-body">
                                                <table id="tblComapanyinfo" class="table table-hover" style="border: none;" border="0">
                                                    <tbody>
                                                        <tr>
                                                            <td class="col-md-3" style="border: none">
                                                                <label>Company Name</label>
                                                            </td>
                                                            <td class="col-md-9" style="border: none">
                                                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                                <label runat="server" id="lblCompanyName"></label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="col-md-3" style="border: none">
                                                                <label>Company Type</label>
                                                            </td>
                                                            <td class="col-md-9" style="border: none">
                                                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                                <asp:Label ID="lblCompanyType" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="col-md-3" style="border: none">
                                                                <label>Life Cycle Stage</label>
                                                            </td>
                                                            <td class="col-md-9" style="border: none">
                                                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                                <asp:Label ID="lblLifeCycleStage" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="col-md-3" style="border: none">
                                                                <label>Industry</label>
                                                            </td>
                                                            <td class="col-md-9" style="border: none">
                                                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                                <asp:Label ID="lblIndustry" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="col-md-3" style="border: none">
                                                                <label>Ownership</label>
                                                            </td>
                                                            <td class="col-md-9" style="border: none">
                                                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                                <asp:Label ID="lblOwnership" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="DivAnnualRevenue" runat="server">
                                                            <td class="col-md-3" style="border: none">
                                                                <label>Annual Revenue</label>
                                                            </td>
                                                            <td class="col-md-9" style="border: none">
                                                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                                <asp:Label ID="lblAnnualRevenue" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="DivNoOfEmployee" runat="server">
                                                            <td class="col-md-3" style="border: none">
                                                                <label>Number Of Employee</label>
                                                            </td>
                                                            <td class="col-md-9" style="border: none">
                                                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                                <asp:Label ID="lblNumberOfEmployee" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="col-md-3" style="border: none">
                                                                <label>Phone</label>
                                                            </td>
                                                            <td class="col-md-9" style="border: none">
                                                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                                <asp:Label ID="lblPhone" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="col-md-3" style="border: none">
                                                                <label>Email</label>
                                                            </td>
                                                            <td class="col-md-9" style="border: none">
                                                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                                <asp:Label ID="lblCompanyEmail" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="col-md-3" style="border: none">
                                                                <label>Fax</label>
                                                            </td>
                                                            <td class="col-md-9" style="border: none">
                                                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                                <asp:Label ID="lblFax" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="col-md-3" style="border: none">
                                                                <label>Website</label>
                                                            </td>
                                                            <td class="col-md-9" style="border: none">
                                                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                                <asp:Label ID="lblWebAddress" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                Billing Address
                                            </div>
                                            <div class="panel-body">
                                                <table class="table table-responsive">
                                                    <tr>
                                                        <td class="col-md-3" style="border: none">
                                                            <label>Street</label>
                                                        </td>
                                                        <td class="col-md-9" style="border: none">
                                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                            <asp:Label ID="lblBillingStreet" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="col-md-3" style="border: none">
                                                            <label>City</label>
                                                        </td>
                                                        <td class="col-md-9" style="border: none">
                                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                            <asp:Label ID="lblBillingCity" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="col-md-3" style="border: none">
                                                            <label>State</label>
                                                        </td>
                                                        <td class="col-md-9" style="border: none">
                                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                            <asp:Label ID="lblBillingState" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="col-md-3" style="border: none">
                                                            <label>Country</label>
                                                        </td>
                                                        <td class="col-md-9" style="border: none">
                                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                            <asp:Label ID="lblBillingCountry" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="col-md-3" style="border: none">
                                                            <label>Post Code</label>
                                                        </td>
                                                        <td class="col-md-9" style="border: none">
                                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                            <asp:Label ID="lblBillingPostCode" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="panel panel-default" id="DivShippingAddress" runat="server">
                                            <div class="panel-heading">
                                                Shipping Address
                                            </div>
                                            <div class="panel-body">
                                                <table class="table table-responsive">
                                                    <tr>
                                                        <td class="col-md-3" style="border: none">
                                                            <label>Street</label>
                                                        </td>
                                                        <td class="col-md-9" style="border: none">
                                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                            <asp:Label ID="lblShippingStreet" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="col-md-3" style="border: none">
                                                            <label>City</label>
                                                        </td>
                                                        <td class="col-md-9" style="border: none">
                                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                            <asp:Label ID="lblShippingCity" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="col-md-3" style="border: none">
                                                            <label>State</label>
                                                        </td>
                                                        <td class="col-md-9" style="border: none">
                                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                            <asp:Label ID="lblShippingState" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="col-md-3" style="border: none">
                                                            <label>Country</label>
                                                        </td>
                                                        <td class="col-md-9" style="border: none">
                                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                            <asp:Label ID="lblShippingCountry" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="col-md-3" style="border: none">
                                                            <label>Post Code</label>
                                                        </td>
                                                        <td class="col-md-9" style="border: none">
                                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                            <asp:Label ID="lblShippingPostCode" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="tab-3">
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label5" runat="server" class="control-label required-field"
                                            Text="Report Type"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control" TabIndex="2" onchange="ChangeReportHeader(this)">
                                            <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                            <asp:ListItem Value="TransactionList">Transaction List</asp:ListItem>
                                            <asp:ListItem Value="TrialBalance">Trial Balance</asp:ListItem>
                                            <asp:ListItem Value="CashFlow">Cash Flow</asp:ListItem>
                                            <asp:ListItem Value="Profit&Loss">Profit & Loss</asp:ListItem>
                                            <asp:ListItem Value="BalanceSheet">Balance Sheet</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div id="SearchTypePanel">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblSearchType" runat="server" class="control-label required-field"
                                                Text="Search Type"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlSearchType" runat="server" CssClass="form-control" TabIndex="2">
                                                <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                                <asp:ListItem Value="1">Fiscal Year</asp:ListItem>
                                                <asp:ListItem Value="2">Date Range</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div id="FiscalYearPanel" class="form-group" style="display: none;">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblFiscalYear" runat="server" class="control-label required-field"
                                            Text="Fiscal Year"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control" TabIndex="2" onchange="SelectFiscalYear(this)">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div id="DateRangePanel" class="form-group" style="display: none;">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtReportStartDate" CssClass="form-control" runat="server"></asp:TextBox><input
                                            type="hidden" id="hidFromDate" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtReportEndDate" CssClass="form-control" runat="server"></asp:TextBox><input
                                            type="hidden" id="hidToDate" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="btn btn-primary btn-sm" OnClick="btnGenerate_Click" OnClientClick="return Validate()" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel panel-default">
                            <div id="ReportHeader" class="panel-heading">
                                Report:: Financial Report
                            </div>
                            <div class="panel-body">
                                <asp:Panel ID="Panel2" runat="server" ScrollBars="Both" Height="700px">
                                    <div>
                                        <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvFiniancialReport"
                                            PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                                            Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                                            WaitMessageFont-Size="14pt" Height="820px">
                                        </rsweb:ReportViewer>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                    <div id="tab-4">
                        <div id="TaskPanel" class="panel panel-default">
                            <div class="panel-heading">
                                Task Information

                              <%--  <a style="float: right; padding: 2px;" href='javascript:void();' onclick='javascript:return LoadProjectManagement()' title='Project Management'>
                            New Task</a>--%>
                                <asp:Button ID="btnTask" runat="server" Style="float: right; padding: 2px;" Text="Add New Task" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return AddNewTask(1,1);" />
                            </div>
                            <div class="panel-body">
                                <div id="TaskDialogue" style="display: none;">
                                    <iframe id="frmTask" name="IframeName" width="100%" height="100%" runat="server"
                                        clientidmode="static" scrolling="yes" style="height: 620px; overflow-y: scroll"></iframe>
                                </div>
                                <div class="form-horizontal">
                                    <table id="tblTask" class="table table-bordered table-condensed table-responsive">
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
                    <div id="tab-5">
                        <div id="DocumentsPanel" class="panel panel-default">
                            <div class="panel-heading">
                                Documents Information
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <asp:HiddenField ID="RandomProjectId" runat="server"></asp:HiddenField>
                                        <div class="col-md-2">
                                            <label class="control-label">Attachment</label>
                                        </div>
                                        <div class="col-md-10">
                                            <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Attach" onclick="AttachFile()" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div runat="server" id="ProjectDocumentInfo" class="col-md-12">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="tab-6">
                        <div id="GanttChartTab">
                            <iframe id="GanttIframe" name="IframeName" width="100%" height="100%" runat="server"
                                clientidmode="static" scrolling="yes" style="height: 620px; overflow-y: scroll"></iframe>
                        </div>
                    </div>
                    <div id="tab-7">
                        <div id="StockInfoTab">
                            <iframe id="StockInfoIframe" name="IframeName" width="100%" height="100%" runat="server"
                                clientidmode="static" scrolling="yes" style="height: 620px; overflow-y: scroll"></iframe>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
