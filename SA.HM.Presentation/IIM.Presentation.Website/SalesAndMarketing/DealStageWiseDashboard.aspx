<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="DealStageWiseDashboard.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.DealStageWiseDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .dealStage {
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


    <script type="text/javascript">
        var dealStageDiv = 0;
        var prevStageId = 0;
        var dealStageId = "", companyId = "", contactId = "", ownerId = "", dealName = "";
        var dateType = "";
        var fromDate = "";
        var toDate = "";
        $(document).ready(function () {

            //sid=" + dealStageId + "&cid=" + companyId + "&oid=" + ownerId + "&dname=" + dealName + "&conId=" + contactId + "&dty=" + dateType + "&fd=" + fromDate + "&td=" + toDate;
            if ($.trim(CommonHelper.GetParameterByName("cid")) != "")
                companyId = parseInt($.trim(CommonHelper.GetParameterByName("cid")), 10);
            else
                companyId = "";

            if ($.trim(CommonHelper.GetParameterByName("sid")) != "")
                dealStageId = parseInt($.trim(CommonHelper.GetParameterByName("sid")), 10);
            else
                dealStageId = "";

            if ($.trim(CommonHelper.GetParameterByName("oid")) != "")
                ownerId = parseInt($.trim(CommonHelper.GetParameterByName("oid")), 10);
            else
                ownerId = "";
            if ($.trim(CommonHelper.GetParameterByName("dname")) != "")
                dealName = $.trim(CommonHelper.GetParameterByName("dname"));
            else
                dealName = "";
            if ($.trim(CommonHelper.GetParameterByName("dty")) != "")
                dateType = $.trim(CommonHelper.GetParameterByName("dty"));
            else
                dateType = "";
            if ($.trim(CommonHelper.GetParameterByName("fd")) != "")
                fromDate = $.trim(CommonHelper.GetParameterByName("fd"));
            else
                fromDate = "";
            if ($.trim(CommonHelper.GetParameterByName("td")) != "")
                toDate = $.trim(CommonHelper.GetParameterByName("td"));
            else
                toDate = "";


            LoadDealAndStage(companyId, dealStageId, dealName, ownerId, dateType, fromDate, toDate);
            $('.dealStage').sortable({
                connectWith: '.dealStage',
                dropOnEmpty: false,
                //containment: 
            });

            $('.dealStage').droppable({ drop: Drop });
            $("#container .dealStage .panel-heading").sortable({
                drop: false
            });

            $(".dealStage").sortable({
                drop: true
            });

            $(".panel-heading").sortable("cancel");

            document.getElementById('InfoPanel').style.width = dealStageDiv + "px";
        });
        function Dragged(event, ui) {

            var dealStageId = $(this).attr("id");
        }
        function Drop(event, ui) {
            debugger;
            var draggableId = ui.draggable.attr("id");
            var droppableId = $(this).attr("id");

            var dealId = draggableId.slice(4, draggableId.length);
            var dealStageId = droppableId.slice(5, droppableId.length);

            var noDeal = $(this).find('.noDeal');
            if (noDeal.length > 0) {
                $(this).find('.noDeal').remove();
            }
            PageMethods.UpdateDealStage(dealStageId, dealId, OnSucceed, OnFailed);

        }
        function OnSucceed(result) {
            debugger;
            if (result.IsSuccess) {

                prevStageId = result.Id;
                if ($("#stage" + prevStageId).find('.item').length <= 0) {
                    // do something
                    var data = "";
                    data += '<div class="noDeal" style="width:200px; height: 30px;">';
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
        function OnFailed(error) {
            parent.ShowAlert(error);
            //CommonHelper.AlertMessage(error.AlertMessage);
            return false;
        }
        function LoadDealAndStage(companyId, dealStageId, dealName, ownerId, dateType, fromDate, toDate) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                async: false,
                url: '../SalesAndMarketing/DealStageWiseDashboard.aspx/LoadDealAndStage',
                data: JSON.stringify({ ownerId: ownerId, dealName: dealName, dealStageId: dealStageId, companyId: companyId, dateType: dateType, fromDate: fromDate, toDate: toDate }),
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
        }
        function LoadDashboard(data) {
            //debugger;
            CommonHelper.SpinnerClose();
            var deals = data.d[0].Deals;
            var stages = data.d[0].DealStages;
            var row = deals.length;
            var col = stages.length;
            var data = "";
            var innerDiv = "";
            var width = Math.floor(12 / col);
            dealStageDiv = (200 * stages.length) + 20;
            document.getElementById('container').setAttribute("style", "width: " + dealStageDiv + "px");

            if (stages.length > 0) {//if stages available
                var divWidth = Math.floor(100 / stages.length);

                for (var i = 0; i < stages.length; i++) {
                    var iDiv = "";// deal stage div
                    var dealStage = stages[i].DealStage.replace(/\s/g, "");

                    //dealStageIds = stages.length;
                    iDiv += '<div class="panel panel-default dealStagePanel" style="width:200px; float: left;" >';
                    iDiv += '<div class="panel-heading">' + stages[i].DealStage + '</div>'; // header of deal stage
                    iDiv += '<div class="dealStage" id="stage' + stages[i].Id + '">';
                    var stageWiseDeals = _.where(deals, { StageId: stages[i].Id });


                    if (stageWiseDeals.length > 0) {//deals within this deal stage
                        data = "";
                        for (var j = 0; j < stageWiseDeals.length; j++) {
                            innerDiv = "";
                            var dealName = stageWiseDeals[j].Name.replace(/\s/g, "");
                            var closeDate = moment(stageWiseDeals[j].CloseDate).format("DD MMM YYYY");
                            innerDiv += '<div class="item" style="width:180px;border:solid;border-color:lightgray; border-width: thin;" id="deal' + stageWiseDeals[j].Id + '">';
                            //innerDiv += '<div id="' + stageWiseDeals[j].Id + '">';
                            //"<a title='Deal Details' href='javascript:void();'style='color:#333333;' onclick= 'GoToDetails(" + gridObject.Id + "," + gridObject.CompanyId + ")' >" + gridObject.Name + "</a>
                            innerDiv += '<p>'
                            innerDiv += '<label>' + " <a title='Deal Details' href='javascript:void();'style='color:blue;width:150px' onclick= 'javascript:return GoToDetails(" + stageWiseDeals[j].Id + "," + stageWiseDeals[j].CompanyId + ")' >" + stageWiseDeals[j].Name + "</a>" + '</label><br />';
                            innerDiv += '<label style="font-weight: normal;">' + stageWiseDeals[j].Owner + '</label><br/>';
                            if (stageWiseDeals[j].ContactName != "") {
                                innerDiv += '<label style="font-weight: normal;">' + stageWiseDeals[j].ContactName + '</label><br />';
                            }
                            else {
                                innerDiv += '<label style="font-weight: normal;">' + stageWiseDeals[j].Company + '</label><br />';
                            }

                            if (stageWiseDeals[j].Amount > 0) {
                                innerDiv += '<label style="font-weight: normal;">' + stageWiseDeals[j].Amount + ' Tk.</label><br />';
                            }
                            innerDiv += '<label style="font-weight: normal;">' + closeDate + '</label>';
                            innerDiv += '</p>';
                            innerDiv += '</div>';
                            data += innerDiv;
                        }
                    }
                    else {
                        data = "";
                        data += '<div class="noDeal" style="width:200px; height: 30px;">';
                        //data += '<label></label>';
                        data += '</div>';
                    }
                    data += '<div class="fakaDeal" style="width:200px; height: 10px; float: left;">';
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
        function GoToDetails(dealId, companyId, contactId) {
            parent.GoToDetails(dealId, companyId, contactId);
            return false;
            //window.location.href = "./DealInformation.aspx?did=" + dealId + "&cid=" + companyId;
        }
    </script>
    <script src="../Scripts/jquery-1.4.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.13.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.13.min.js" type="text/javascript"></script>
    <div id="InfoPanel" class="panel panel-default">
        <div class="panel-heading">
            Deal Management 
        </div>
        <div class="panel-body">
            <div style="width: 100%;">
                <div class="form-group" id="container" style="width: 2200px; height: 100%; border-color: lightgray">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
