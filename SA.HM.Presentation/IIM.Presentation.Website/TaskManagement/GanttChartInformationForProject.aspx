<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="GanttChartInformationForProject.aspx.cs" Inherits="HotelManagement.Presentation.Website.TaskManagement.GanttChartInformationForProject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/Scripts/jsgantt.js"></script>
    <link rel="stylesheet" href="/Content/jsgantt.css" />
    <script type="text/javascript" src="/Scripts/subjx.js"></script>
    <link rel="stylesheet" href="/Content/subjx.css" />
    <%--<script language="javascript" src="graphics.js"></script>--%>
    <style type="text/css">
        /*<!--
        .style1 {
            color: #0000FF;
        }

        .roundedCorner {
            display: block;
        }

            .roundedCorner * {
                display: block;
                height: 1px;
                overflow: hidden;
                font-size: .01em;
                background: #0061ce;
            }

        .roundedCorner1 {
            margin-left: 3px;
            margin-right: 3px;
            padding-left: 1px;
            padding-right: 1px;
            border-left: 1px solid #91bbe9;
            border-right: 1px solid #91bbe9;
            background: #3f88da;
        }

        .roundedCorner2 {
            margin-left: 1px;
            margin-right: 1px;
            padding-right: 1px;
            padding-left: 1px;
            border-left: 1px solid #e5effa;
            border-right: 1px solid #e5effa;
            background: #307fd7;
        }

        .roundedCorner3 {
            margin-left: 1px;
            margin-right: 1px;
            border-left: 1px solid #307fd7;
            border-right: 1px solid #307fd7;
        }

        .roundedCorner4 {
            border-left: 1px solid #91bbe9;
            border-right: 1px solid #91bbe9;
        }

        .roundedCorner5 {
            border-left: 1px solid #3f88da;
            border-right: 1px solid #3f88da;
        }

        .roundedCornerfg {
            background: #0061ce;
        }*/


        -->
    </style>
    <div class="form-group">
        <div class="col-md-12 form-group">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="form-group">
                        <label id="lblGanttChart" class="control-label">Gantt Chart</label>
                        <div id="DivGoBack" style="float: right; display: none">
                        </div>
                    </div>
                </div>
                <div class="panel-body form-group">
                    <div class="form-group" id="companyProjectDiv">
                        <div class="col-md-2">
                            <asp:Label ID="lblGLCompany" runat="server" class="control-label" Text="Company"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlGLCompany" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblGLProject" runat="server" class="control-label" Text="Project"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlGLProject" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <br />
                        <br />
                    </div>
                    <div id="ShowChart" style="display: none">
                        <div class="form-group">
                            <div class="col-md-4">
                                <asp:Label ID="lblProjectName" runat="server" class="control-label" Text="qqq"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="lblProjectComplete" runat="server" class="control-label" Text="rrr"></asp:Label>
                            </div>
                            <br />
                        </div>
                        <div class="form-group">
                            <div id="chart_div1" class="col-md-12" style="overflow-x: scroll"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="position: relative" class="gantt" id="GanttChartDIV"></div>
    <div id="ParentGranttDiv" class="col-md-12"></div>

    <script>
        var g;
        var projectId;
        var TaskIdFromGanttChart;
        $(document).ready(function () {
            var itsPostBack = <%= Page.IsPostBack ? "true" : "false" %>;
            if (!itsPostBack) {
                //;
                var previousAddress = document.referrer + "";

                $('#ContentPlaceHolder1_hfRefPreviousPage').val(previousAddress);
                var str = "<input type='button' id='btnGoBack' class='TransactionalButton btn btn-primary' value='Go Back' onclick=\"javascript:return GoBack('" + previousAddress + "')\" />";
                $('#DivGoBack').html(str);
            }
            else {
                //;
                var previousAddress = $('#ContentPlaceHolder1_hfRefPreviousPage').val();
                var str = "<input type='button' id='btnGoBack' class='TransactionalButton btn btn-primary' value='Go Back' onclick=\"javascript:return GoBack('" + previousAddress + "')\" />";
                $('#DivGoBack').html(str);
            }

            $("#ContentPlaceHolder1_ddlGLProject").change(function () {
                var projectId = parseInt($("#ContentPlaceHolder1_ddlGLProject").val().trim());
                drawChart(projectId);
            });

            $("#ContentPlaceHolder1_ddlGLCompany").change(function () {

                var CompanyId = parseInt($("#ContentPlaceHolder1_ddlGLCompany").val().trim());
                PageMethods.LoadProjectByCompanyId(CompanyId, OnLoadProjectByCompanyIdSucceed, OnLoadProjectByCompanyIdFailed);
                return false;
            });

            $("#ContentPlaceHolder1_ddlGLCompany").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlGLProject").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            var project = $.trim(CommonHelper.GetParameterByName("pid"));

            if (project != "") {
                drawChart(parseFloat(project));
                $("#companyProjectDiv").hide();

            }
            $("#TaskDialig").dialog(opt).dialog("open");
        });
        JSGantt.taskLink = function (pRef, pWidth, pHeight) {

            TaskIdFromGanttChart = pRef;
            PageMethods.GetTaskId(parseInt(pRef), OnSuccessLoading, OnFailLoading)
            return false;
            //var iframeid = 'formTask';
            //var url = "../TaskManagement/AssignTaskIFrame.aspx?tid=" + pRef;
            //document.getElementById(iframeid).src = url;


        }
        function OnSuccessLoading(result) {
            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '98%',
                height: 620,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Task: " + result.TaskName,
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
        function OnFailLoading(error) {

        }
        function EditThisTask() {
            var iframeid = 'formTask';
            var url = "../TaskManagement/AssignTaskIFrame.aspx?tid=" + TaskIdFromGanttChart;
            document.getElementById(iframeid).src = url;

            $("#TaskDialig").dialog({
                autoOpen: true,
                modal: true,
                width: '98%',
                height: 615,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Edit Task",
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
        function ViewDetails() {
            var iframeid = 'formTask';
            var url = "../TaskManagement/TaskDetails.aspx?tid=" + TaskIdFromGanttChart;
            document.getElementById(iframeid).src = url;

            $("#TaskDialig").dialog({
                autoOpen: true,
                modal: true,
                width: '98%',
                height: 615,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Feedback",
                show: 'slide'
            });

            return false;
        }
        function AddNewTask() {
            var project = $.trim(CommonHelper.GetParameterByName("pid"));
            var iframeid = 'formTask';
            var url = "../TaskManagement/AssignTaskIFrame.aspx??tid=" + TaskIdFromGanttChart + "&pid=" + project;
            document.getElementById(iframeid).src = url;

            $("#TaskDialig").dialog({
                autoOpen: true,
                modal: true,
                width: '98%',
                height: 615,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Add New Task",
                show: 'slide'
            });

            return false;
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }
        function CloseDialog() {
            $("#TaskDialig").dialog('close');
            return false;
        }
        function GridPaging(a, b) {
            debugger;
            var project = $.trim(CommonHelper.GetParameterByName("pid"));
            drawChart(parseFloat(project));
        }
        function GoBack(previousAddress) {
            if (confirm("Do you want to go back?")) {
                window.location = previousAddress;
            }
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

        function OnLoadProjectByCompanyIdFailed(xhr, err) {
            toastr.error(xhr.responseText);
        }

        // here's all the html code neccessary to display the chart object

        // Future idea would be to allow XML file name to be passed in and chart tasks built from file.

        function parseJsonDate(jsonDate) {

            var fullDate = new Date(parseInt(jsonDate.substr(6)));
            //var twoDigitMonth = (fullDate.getMonth() + 1) + ""; if (twoDigitMonth.length == 1) twoDigitMonth = "0" + twoDigitMonth;
            var twoDigitMonth = (fullDate.getMonth() + 1) + ""; if (twoDigitMonth.length == 1) twoDigitMonth = "0" + twoDigitMonth;

            var twoDigitDate = fullDate.getDate() + ""; if (twoDigitDate.length == 1) twoDigitDate = "0" + twoDigitDate;
            var currentDate = twoDigitMonth + "/" + twoDigitDate + "/" + fullDate.getFullYear();

            return currentDate;
        };

        function drawChart(projectId) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../../../TaskManagement/GanttChartInformationForProject.aspx/LoadTaskByProjectId",
                dataType: "json",
                data: JSON.stringify({ projetcId: projectId }),
                async: false,
                success: (data) => {
                    OnTaskLoadingSucceed(data.d);

                },
                error: (error) => {
                    OnTaskLoadingFailed(error);
                }
            });
            return false;
        }

        function OnTaskLoadingSucceed(result) {
            //debugger;
            if (result.length > 0) {
                $('#ShowChart').show();
                $('#ContentPlaceHolder1_lblProjectName').text('Project Name : ' + result[0].ProjectName);
                $('#ContentPlaceHolder1_lblProjectComplete').text('Project Complete : ' + result[0].ProjectComplete + '%');
            }
            else {
                $('#ShowChart').hide();
                $('#ContentPlaceHolder1_lblProjectName').text('');
                $('#ContentPlaceHolder1_lblProjectComplete').text('');
            }

            g = new JSGantt.GanttChart('g', document.getElementById('chart_div1'), 'day');
            g.setShowRes(1); // Show/Hide Responsible (0/1)
            g.setShowDur(1); // Show/Hide Duration (0/1)
            g.setShowComp(1); // Show/Hide % Complete(0/1)
            //g.setCaptionType('Resource');  // Set to Show Caption (None,Caption,Resource,Duration,Complete)
            //var gr = new Graphics();

            if (g) {

                // Parameters             (pID, pName,                  pStart,      pEnd,        pColor,   pLink,          pMile, pRes,  pComp, pGroup, pParent, pOpen, pDepend, pCaption)

                // You can also use the XML file parser JSGantt.parseXML('project.xml',g)

                for (i = 0; i < result.length; i++) {
                    var id = result[i].Id;
                    var taskName = result[i].TaskName;
                    var startDate = parseJsonDate(result[i].TaskDate);
                    var endDate = parseJsonDate(result[i].EstimatedDoneDate);
                    var complete = result[i].Complete == null ? 0 : result[i].Complete;
                    var name = result[i].EmployeeName;
                    var ParentTaskId = result[i].ParentTaskId;
                    var hasChild = result[i].HasChild;
                    var dependentTaskId = result[i].DependentTaskId;
                    g.AddTaskItem(new JSGantt.TaskItem(id, taskName, startDate, endDate, '4682b4', '', 0, name, complete, hasChild, ParentTaskId, 1, dependentTaskId));

                }

                //g.AddTaskItem(new JSGantt.TaskItem(id, taskName, '', '', 'ff0000', '', 0, 'Brian', 0, 1, 0, 1));
                //g.AddTaskItem(new JSGantt.TaskItem(11, 'Chart Object', '7/20/2008', '7/20/2008', 'ff00ff', '', 1, 'Shlomy', 100, 0, 1, 1));
                //g.AddTaskItem(new JSGantt.TaskItem(12, 'Task Objects', '', '', '00ff00', '', 0, 'Shlomy', 40, 1, 1, 1));
                //g.AddTaskItem(new JSGantt.TaskItem(121, 'Constructor Proc', '7/21/2008', '8/9/2008', '00ffff', '', 0, 'Brian T.', 60, 0, 12, 1));
                //g.AddTaskItem(new JSGantt.TaskItem(122, 'Task Variables', '8/6/2008', '8/11/2008', 'ff0000', '', 0, 'Brian', 60, 0, 12, 1, 121));
                //g.AddTaskItem(new JSGantt.TaskItem(123, 'Task by Minute/Hour', '8/6/2008', '8/11/2008 12:00', 'ffff00', '', 0, 'Ilan', 60, 0, 12, 1, 121));
                //g.AddTaskItem(new JSGantt.TaskItem(124, 'Task Functions', '8/9/2008', '8/29/2008', 'ff0000', '', 0, 'Anyone', 60, 0, 12, 1, 0, 'This is another caption'));
                //g.AddTaskItem(new JSGantt.TaskItem(2, 'Create HTML Shell', '8/24/2008', '8/25/2008', 'ffff00', '', 0, 'Brian', 20, 0, 0, 1, 122));
                //g.AddTaskItem(new JSGantt.TaskItem(3, 'Code Javascript', '', '', 'ff0000', '', 0, 'Brian', 0, 1, 0, 1));
                //g.AddTaskItem(new JSGantt.TaskItem(31, 'Define Variables', '7/25/2008', '8/17/2008', 'ff00ff', '', 0, 'Brian', 30, 0, 3, 1, '', 'Caption 1'));
                //g.AddTaskItem(new JSGantt.TaskItem(32, 'Calculate Chart Size', '8/15/2008', '8/24/2008', '00ff00', '', 0, 'Shlomy', 40, 0, 3, 1));
                //g.AddTaskItem(new JSGantt.TaskItem(33, 'Draw Taks Items', '', '', '00ff00', '', 0, 'Someone', 40, 1, 3, 1));
                //g.AddTaskItem(new JSGantt.TaskItem(332, 'Task Label Table', '8/6/2008', '8/11/2008', '0000ff', '', 0, 'Brian', 60, 0, 33, 1));
                //g.AddTaskItem(new JSGantt.TaskItem(333, 'Task Scrolling Grid', '8/9/2008', '8/20/2008', '0000ff', '', 0, 'Brian', 60, 0, 33, 1));
                //g.AddTaskItem(new JSGantt.TaskItem(34, 'Draw Task Bars', '', '', '990000', '', 0, 'Anybody', 60, 1, 3, 0));
                //g.AddTaskItem(new JSGantt.TaskItem(341, 'Loop each Task', '8/26/2008', '9/11/2008', 'ff0000', '', 0, 'Brian', 60, 0, 34, 1));
                //g.AddTaskItem(new JSGantt.TaskItem(342, 'Calculate Start/Stop', '9/12/2008', '10/18/2008', 'ff6666', '', 0, 'Brian', 60, 0, 34, 1));
                //g.AddTaskItem(new JSGantt.TaskItem(343, 'Draw Task Div', '10/13/2008', '10/17/2008', 'ff0000', '', 0, 'Brian', 60, 0, 34, 1));
                //g.AddTaskItem(new JSGantt.TaskItem(344, 'Draw Completion Div', '10/17/2008', '11/04/2008', 'ff0000', '', 0, 'Brian', 60, 0, 34, 1, "342,343"));
                //g.AddTaskItem(new JSGantt.TaskItem(35, 'Make Updates', '12/17/2008', '2/04/2009', 'f600f6', '', 0, 'Brian', 30, 0, 3, 1));
                g.Draw();
                g.DrawDependencies();
                //g.changeFormat("day", g);

            }

            else {
                alert("No task defined");

            }
        }

        function OnTaskLoadingFailed(error) {
            toastr.error(error.get_message());
            return false;
        }
        function Close() {
            $("#SalesNoteDialog").dialog('close');
            return false;
        }

    </script>
    <asp:HiddenField ID="hfRefPreviousPage" runat="server"></asp:HiddenField>
    <div id="SalesNoteDialog" style="display: none;">
        <%--<iframe id="formTask" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>--%>
        <div class="row" style="padding-bottom: 0; padding-top: 10px;">
            <div class="form-group">
                <div class="col-md-12" style="padding-bottom: 0; padding-top: 20px;">
                    <input id="ViewDetails" type="button" value="Task Feedback" style="width:100px;" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return ViewDetails();" />
                    <input id="EditThisTask" type="button" value="Edit This Task" style="width:100px;" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return EditThisTask();" />
                    <input id="AddNewTask" type="button" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return AddNewTask();" value="Add New Task" style="width:100px;" />
                </div>
                <div class="col-md-12" style="padding-bottom: 0; padding-top: 5px;">
                    <input id="Close" type="button" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return Close();" style="width:100px;" value="Close" />
                </div>
            </div>
        </div>
    </div>
    <div id="TaskDialig" style="display: none;">
        <iframe id="formTask" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>

    </div>
</asp:Content>
