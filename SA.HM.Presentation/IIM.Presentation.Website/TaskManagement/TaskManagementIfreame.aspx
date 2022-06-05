<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="TaskManagementIfreame.aspx.cs" Inherits="HotelManagement.Presentation.Website.TaskManagement.TaskManagementIfreame" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .taskStage {
            height: auto;
            width: 200px;
            padding: 10px;
            float: left;
        }

        .item {
            margin: 0px;
            /*width: 200px;*/
            float: left;
            padding: 5px;
            cursor: move;
        }

            .item:hover {
                background-color: lightgrey;
            }

        a:hover {
            color: cornflowerblue;
        }

        .header {
            padding: 10px;
            height: auto;
            width: 200px;
            float: left;
        }
    </style>
    <script src="../Scripts/jquery-1.4.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.13.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.13.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var fromDate = "";
        var toDate = "";
        var panelWidth = 0;
        $(document).ready(function () {
            debugger;
            //if ($.trim(CommonHelper.GetParameterByName("fd")) != "")
            //    fromDate = $.trim(CommonHelper.GetParameterByName("fd"));
            //else
            //    fromDate = "";
            //if ($.trim(CommonHelper.GetParameterByName("td")) != "")
            //    toDate = $.trim(CommonHelper.GetParameterByName("td"));
            //else
            //    toDate = "";
            LoadTaskAndStage();

            $('.taskStage').sortable({
                connectWith: '.taskStage',
                dropOnEmpty: false,
                //containment: 
            });
            $('.taskStage').droppable({ drop: Drop });

            $("#container .taskStage .panel-heading").sortable({
                drop: false
            });

            $(".taskStage").sortable({
                drop: true
            });

            $(".panel-heading").sortable("cancel");

             document.getElementById('InfoPanel').style.width = panelWidth + "px";
        });
        function Drop(event, ui) {
            debugger;
            var draggableId = ui.draggable.attr("id");
            var droppableId = $(this).attr("id");

            var taskId = draggableId.slice(4, draggableId.length);
            var taskStageId = droppableId.slice(5, droppableId.length);

            var noTask = $(this).find('.noTask');
            if (noTask.length > 0) {
                $(this).find('.noTask').remove();
            }
            PageMethods.UpdateTaskStage(taskStageId, taskId, OnSucceed, OnFailed);

        }
        function OnSucceed(result) {
            debugger;
            if (result.IsSuccess) {

                prevStageId = result.Id;
                if ($("#stage" + prevStageId).find('.item').length <= 0) {
                    // do something
                    var data = "";
                    data += '<div class="noTask" style="width:200px; height: 30px;">';
                    //data += '<label></label>';
                    data += '</div>';
                    document.getElementById("stage" + prevStageId).innerHTML += data;
                    //$("#stage" + prevStageId).height("60px");
                }
                parent.ShowAlert(result.AlertMessage);
            }
            else {
                parent.ShowAlert(result.AlertMessage);
            }
        }

        function LoadTaskAndStage() {
            debugger;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                async: false,
                url: '../TaskManagement/TaskManagementIfreame.aspx/LoadTaskAndStage',
                //data: JSON.stringify({ ownerId: ownerId, dealName: dealName, dealStageId: dealStageId, companyId: companyId, dateType: dateType, fromDate: fromDate, toDate: toDate }),
                dataType: "json",
                success: function (data) {
                    CommonHelper.SpinnerOpen();
                    LoadDashboard(data);
                },
                error: function (result) {
                    toastr.error(result);
                }
            });
            return false;
            //CommonHelper.SpinnerOpen();
            //PageMethods.LoadTaskAndStage(OnLoadTaskAndStageSucceed, OnFailed);
        }
        //function OnLoadTaskAndStageSucceed(result) {
        //    CommonHelper.SpinnerClose();
        //    LoadDashboard(result);
        //    return false;
        //}
        function LoadDashboard(data) {
            debugger;
            var tasks = data.d[0].Tasks;
            var stages = data.d[0].Stages;
            var innerDiv = "";
            panelWidth = (stages.length * 250)+20;
            document.getElementById('container').setAttribute("style", "width: " + panelWidth + "px");
            if (stages.length > 0) {
                for (var i = 0; i < stages.length; i++) {
                    {
                        var iDiv = "";
                        iDiv += '<div class="panel panel-default taskStagePanel" style="width:250px; float: left;" >';
                        iDiv += '<div class="panel-heading">' + stages[i].TaskStage + '</div>'; // header of task stage
                        iDiv += '<div class="taskStage" id="stage' + stages[i].Id + '">';
                        var stageWisetasks = _.where(tasks, { TaskStage: stages[i].Id });
                        if (stageWisetasks.length > 0) {
                            data = "";
                            for (var j = 0; j < stageWisetasks.length; j++) {
                                innerDiv = "";
                                var taskName = stageWisetasks[j].TaskName.replace(/\s/g, "");
                                var taskDate = moment(stageWisetasks[j].TaskDate).format("DD MMM YYYY");
                                var closeDate = moment(stageWisetasks[j].EstimatedDoneDate).format("DD MMM YYYY");
                                innerDiv += '<div class="item" style="width:230px;border:solid;border-color:lightgray; border-width: thin;" id="task' + stageWisetasks[j].Id + '">';

                                innerDiv += '<p>'
                                //innerDiv += '<label style="color:blue;width:150px>"' + stageWisetasks[j].TaskName + '</label><br />';
                                innerDiv += '<label style=" font-weight:bold; color:blue;">' + stageWisetasks[j].TaskName + '</label><br />';
                                if (stageWisetasks[j].TaskType == "Project") {
                                    innerDiv += '<label style="font-weight: normal;">' + stageWisetasks[j].TaskType + " ( " + stageWisetasks[j].ProjectName + " ) " + '</label><br />';
                                }
                                else {
                                    innerDiv += '<label style="font-weight: normal;">' + stageWisetasks[j].TaskType + '</label><br/>';
                                }
                                innerDiv += '<label style="font-weight: normal;"> Start Date: ' + taskDate + '</label><br />';
                                innerDiv += '<label style="font-weight: normal;">End Date: ' + closeDate + '</label><br />';
                                innerDiv += '<label style="font-weight: normal;"> Assigned To : ' + stageWisetasks[j].EmployeeName + '</label>';
                                innerDiv += '</p>';
                                innerDiv += '</div>';
                                data += innerDiv;
                            }
                        }
                        else {
                            data = "";
                            data += '<div class="noTask" style="width:200px; height: 30px;">';
                            //data += '<label></label>';
                            data += '</div>';
                        }
                        data += '<div class="fakaTask" style="width:230px; height: 30px; float: left;">';
                        data += '<label></label>';
                        data += '</div>';
                        iDiv += data;
                        iDiv += '</div>';
                        iDiv += '</div>';
                        document.getElementById('container').innerHTML += iDiv;
                        data = "";
                    }

                }
            }
        }
        function OnFailed(error) {
            parent.ShowAlert(error);
            return false;
        }
    </script>

    <div id="InfoPanel" class="panel panel-default">
        <div class="panel-heading">
            Task Management 
        </div>
        <div class="panel-body">
            <div style="width: 100%;">
                <div class="form-group" id="container" style="width: 2200px; height: 100%; border-color: lightgray">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
