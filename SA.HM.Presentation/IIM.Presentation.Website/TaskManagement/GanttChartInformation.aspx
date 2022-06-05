<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="GanttChartInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.TaskManagement.GanttChartInformation" %>

<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>


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
    <div class="form-horizontal">
        <div class="panel panel-default">
            <div class="panel-heading">
                <div class="form-group">
                    <label id="lblGanttChart" class="control-label">&nbsp; &nbsp; Gantt Chart</label>
                    <div id="DivGoBack" style="float: right; display: none">
                    </div>
                </div>
            </div>
            <div class="panel-body form-group">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Task Type</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTaskType" runat="server" CssClass="form-control">
                            <%--<asp:ListItem Value="Project">Project</asp:ListItem>
                            <asp:ListItem Value="PreSales">Pre Sales</asp:ListItem>
                            <asp:ListItem Value="Billing">Billing</asp:ListItem>
                            <asp:ListItem Value="Employee">Employee</asp:ListItem>--%>
                        </asp:DropDownList>
                    </div>
                </div>

                <div id="companyProjectDiv">

                    <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />

                    <%--<div class="col-md-2">
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
                        <br />--%>
                </div>

                <div class="form-group" id="DivSaleNo" style="display: none">
                    <div class="col-md-2">
                        <label class="control-label required-field">Sale No</label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlSaleNo" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>


                <div class="form-group" id="DivBillNo" style="display: none">
                    <div class="col-md-2">
                        <label class="control-label required-field">Bill No</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtBillNo" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group" id="DivEmployeeDiv" style="display: none">
                    <div class="col-md-2">
                        <label class="control-label required-field">Employee Name</label>
                    </div>
                    <div class="col-md-10">
                        <input type="text" class="form-control" id="txtEmployeeNameSearch" />
                    </div>
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
    <div style="position: relative" class="gantt" id="GanttChartDIV"></div>
    <div id="ParentGranttDiv" class="col-md-12"></div>
    <script>
        var g;
        var projectId;
        $(document).ready(function () {
            $("#ContentPlaceHolder1_companyProjectUserControl_hfDropdownFirstValue").val("select");

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

            $("#ContentPlaceHolder1_txtBillNo").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../TaskManagement/AssignTaskIFrame.aspx/GetBillNoByText',
                        //data: "{'searchTerm':'" + request.term + "'}",
                        data: JSON.stringify({ searchTerm: request.term }),
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.BillNumber,
                                    value: m.BillId
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
                    debugger;
                    $(this).val(ui.item.label);
                    drawChart(ui.item.value, 'Billing');
                    //$("#ContentPlaceHolder1_hfBillId").val(ui.item.value);
                }
            });

            $("#txtEmployeeNameSearch").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../../Common/WebMethodPage.aspx/SearchEmployeeByName",
                        data: "{'employeeName':'" + request.term + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    EmpId: m.EmpId,
                                    EmployeeName: m.EmployeeName,
                                    Department: m.Department,
                                    Designation: m.Designation,
                                    label: m.EmployeeName,
                                    value: m.EmpId
                                };
                            });
                            response(searchData);
                        },
                        error: function (xhr, err) {
                            toastr.error(xhr.responseText);
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

                    drawChart(ui.item.value, 'Employee');
                }
            });

            $("#ContentPlaceHolder1_ddlSaleNo").select2({
                tags: false,
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlTaskType").change(function () {
                debugger;
                if ($(this).val() == "PreSales") {
                    $("#companyProjectDiv").hide();
                    $("#DivBillNo").hide();
                    $("#DivEmployeeDiv").hide();
                    $("#DivSaleNo").show();
                    $('#ShowChart').hide();
                    $("#ContentPlaceHolder1_ddlSaleNo").val("0").trigger('change');
                }
                else if ($(this).val() == "Project") {
                    $("#companyProjectDiv").show();
                    $("#DivBillNo").hide();
                    $("#DivSaleNo").hide();
                    $("#DivEmployeeDiv").hide();
                    $('#ShowChart').hide();
                    $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val("0").trigger('change');

                }
                else if ($(this).val() == "Billing") {
                    $("#companyProjectDiv").hide();
                    $("#DivBillNo").show();
                    $("#DivSaleNo").hide();
                    $("#DivEmployeeDiv").hide();
                    $('#ShowChart').hide();
                    $("#ContentPlaceHolder1_txtBillNo").val("");

                }
                else if ($(this).val() == "Employee") {
                    $("#companyProjectDiv").hide();
                    $("#DivBillNo").hide();
                    $("#DivSaleNo").hide();
                    $("#DivEmployeeDiv").show();
                    $('#ShowChart').hide();
                    $("#txtEmployeeNameSearch").val("");

                }

            });

            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").change(function () {
                var projectId = parseInt($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val().trim());
                drawChart(projectId, 'Project');
            });

            $("#ContentPlaceHolder1_ddlSaleNo").change(function () {
                var Id = parseInt($("#ContentPlaceHolder1_ddlSaleNo").val().trim());
                drawChart(Id, 'PreSales');
            });



            var project = $.trim(CommonHelper.GetParameterByName("pid"));
            if (project != "") {
                //drawChart(project);
                $("#companyProjectDiv").hide();
            }
        });

        function GoBack(previousAddress) {
            if (confirm("Do you want to go back?")) {
                window.location = previousAddress;
            }
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

        function drawChart(Id, Type) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../../../TaskManagement/GanttChartInformation.aspx/LoadTaskByProjectId",
                dataType: "json",
                data: JSON.stringify({ Id: Id, Type: Type }),
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
            if (result.length > 0) {
                debugger;
                $('#ShowChart').show();
                $('#ContentPlaceHolder1_lblProjectName').text('Name : ' + result[0].SourceName);
                $('#ContentPlaceHolder1_lblProjectComplete').text('Complete : ' + result[0].ProjectComplete + '%');
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
            debugger;
            var type = $("#ContentPlaceHolder1_ddlTaskType").val();
            if (type == "Employee") {
                $('#ContentPlaceHolder1_lblProjectName').text('Name : ' + $("#txtEmployeeNameSearch").val());
                $('#ContentPlaceHolder1_lblProjectComplete').text('');
            }
            else if (type == "Billing") {
                $('#ContentPlaceHolder1_lblProjectComplete').text('');
            }

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
                    var name;
                    if (type == "Employee") {
                        name = result[i].SourceName;
                    }
                    else {
                        name = result[i].EmployeeName;
                    }


                    var ParentTaskId = result[i].ParentTaskId;
                    var hasChild = result[i].HasChild;
                    var dependentTaskId = result[i].DependentTaskId;
                    g.AddTaskItem(new JSGantt.TaskItem(id, taskName, startDate, endDate, 'ffff00', '', 0, name, complete, hasChild, ParentTaskId, 1, dependentTaskId));

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

            }

            else {
                alert("No task defined");

            }
        }

        function OnTaskLoadingFailed(error) {
            toastr.error(error.get_message());
            return false;
        }

    </script>
    <asp:HiddenField ID="hfRefPreviousPage" runat="server"></asp:HiddenField>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#ContentPlaceHolder1_companyProjectUserControl_hfIsSingle").val() != "1") {

                if ($("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val() == "0") {
                    $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val($("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val()).trigger("change");
                }
            }
        });
    </script>
</asp:Content>
