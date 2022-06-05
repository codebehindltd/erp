<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="ProjectStageManagementIframe.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.ProjectStageManagementIframe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <style>
        .projectStage {
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
            LoadProjectAndStage();

            $('.projectStage').sortable({
                connectWith: '.projectStage',
                dropOnEmpty: false,
                //containment: 
            });
            $('.projectStage').droppable({ drop: Drop });

            $("#container .projectStage .panel-heading").sortable({
                drop: false
            });

            $(".projectStage").sortable({
                drop: true
            });

            $(".panel-heading").sortable("cancel");
            document.getElementById('InfoPanel').style.width = panelWidth + "px";
        });
        function Drop(event, ui) {
            debugger;
            var draggableId = ui.draggable.attr("id");
            var droppableId = $(this).attr("id");

            var projectId = draggableId.slice(7, draggableId.length);
            var projectStageId = droppableId.slice(5, droppableId.length);

            var noProject = $(this).find('.noProject');
            if (noProject.length > 0) {
                $(this).find('.noProject').remove();
            }
            PageMethods.UpdateProjectStage(projectStageId, projectId, OnSucceed, OnFailed);

        }
        function OnSucceed(result) {
            debugger;
            if (result.IsSuccess) {

                prevStageId = result.Id;
                if ($("#stage" + prevStageId).find('.item').length <= 0) {
                    // do something
                    var data = "";
                    data += '<div class="noProject" style="width:200px; height: 30px;">';
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

        function LoadProjectAndStage() {
            debugger;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                async: false,
                url: '../GeneralLedger/ProjectStageManagementIframe.aspx/LoadProjectAndStage',
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
            //PageMethods.LoadProjectAndStage(OnLoadProjectAndStageSucceed, OnFailed);
        }
        //function OnLoadProjectAndStageSucceed(result) {
        //    CommonHelper.SpinnerClose();
        //    LoadDashboard(result);
        //    return false;
        //}
        function LoadDashboard(data) {
            debugger;
            var projects = data.d[0].Projects;
            var stages = data.d[0].Stages;
            var innerDiv = "";
            panelWidth = (stages.length * 250)+20;
            document.getElementById('container').setAttribute("style", "width: " + panelWidth + "px");
            if (stages.length > 0) {
                for (var i = 0; i < stages.length; i++) {
                    {
                        var iDiv = "";
                        iDiv += '<div class="panel panel-default projectStagePanel" style="width:250px; float: left;" >';
                        iDiv += '<div class="panel-heading">' + stages[i].ProjectStage + '</div>'; // header of project stage
                        iDiv += '<div class="projectStage" id="stage' + stages[i].Id + '">';
                        var stageWiseprojects = _.where(projects, { StageId: stages[i].Id });
                        if (stageWiseprojects.length > 0) {
                            data = "";
                            for (var j = 0; j < stageWiseprojects.length; j++) {
                                innerDiv = "";
                                //var projectName = stageWiseprojects[j].Name.replace(/\s/g, "");
                                var projectDate = "";
                                var closeDate = "";
                                if (stageWiseprojects[j].StartDate != null) {
                                    projectDate = moment(stageWiseprojects[j].StartDate).format("DD MMM YYYY");
                                }
                                if (stageWiseprojects[j].EndDate != null) {
                                    closeDate = moment(stageWiseprojects[j].EndDate).format("DD MMM YYYY");
                                }
                                innerDiv += '<div class="item" style="width:230px;border:solid;border-color:lightgray; border-width: thin;" id="project' + stageWiseprojects[j].ProjectId + '">';

                                innerDiv += '<p>'
                                //innerDiv += '<label style="color:blue;width:150px>"' + stageWiseprojects[j].ProjectName + '</label><br />';
                                innerDiv += '<label style="font-weight: bold; color:blue;">' + stageWiseprojects[j].Name + '</label><br />';
                                
                                innerDiv += '<label style="font-weight: normal;"> Start Date: ' + projectDate + '</label><br />';
                                innerDiv += '<label style="font-weight: normal;">End Date: ' + closeDate + '</label><br />';
                                innerDiv += '<label style="font-weight: normal;">Project Amount : ' + stageWiseprojects[j].ProjectAmount + '</label>';
                                innerDiv += '</p>';
                                innerDiv += '</div>';
                                data += innerDiv;
                            }
                        }
                        else {
                            data = "";
                            data += '<div class="noProject" style="width:200px; height: 30px;">';
                            //data += '<label></label>';
                            data += '</div>';
                        }
                        data += '<div class="fakaProject" style="width:230px; height: 30px; float: left;">';
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
            Project Management 
        </div>
        <div class="panel-body">
            <div style="width: 100%;">
                <div class="form-group" id="container" style="width: 2200px; height: 100%; border-color: lightgray">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
