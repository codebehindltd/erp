<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmCommonMessageDetails.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.frmCommonMessageDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var UserInformation = [];

        $(document).ready(function () {

            $("#SearchPanel").hide();

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Company Information</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Message Compose</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $(".InnboardMessageHiddenField").val("");
            }

            GridPaging(1, 1);

        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#gvInboxMessage tbody tr").length;

            PageMethods.LoadMessageInbox(gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnMessageInboxLoadSucceed, OnMessageInboxLoadFailed);
            return false;
        }

        function OnMessageInboxLoadSucceed(result) {

            $("#gvInboxMessage tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            $("#gvInboxMessage tbody").append(result.GridBody);

            $("#ContentPlaceHolder1_hfIsCurrentOrPreviousPage").val(result.GridPageLinks.CurrentPageNumber);

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);


        }
        function OnMessageInboxLoadFailed() {
        }

        function ReloadGrid(IsCurrentOrPreviousPage) {
            var currentPageNumber = $("#GridPagingContainer ul li[class='active']").text();

            if (currentPageNumber == "")
                currentPageNumber = 1;

            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }

        function LoadMessageDetails(mid, mdId) {
            MessageDetailsLoading(mid, mdId);
        }

        function MessageDetailsLoading(mid, mdId) {
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../HMCommon/frmCommonMessageDetails.aspx/LoadMessageDetails',
                data: "{'msgId':'" + mid + "','msgDetailsId':'" + mdId + "'}",
                dataType: "json",
                success: function (data) {

                    if (data.d.IsSuccess == true) {

                        $("#ContentPlaceHolder1_MessageSubject").text(data.d.Arr[0]);
                        $("#ContentPlaceHolder1_MessageBody").text(data.d.Arr[1]);

                        $("#EntryPanel").show();
                        $("#SearchPanel").hide();

                        if (data.d.Arr[2] != "") {
                            $("#MessageBriefDescription").html(data.d.Arr[2]);
                            $("#lblMessageCount").text(data.d.Arr[3]);
                            $("#ContentPlaceHolder1_lblMessageCount").text(data.d.Arr[3]);

                            ReloadGrid($("#ContentPlaceHolder1_hfIsCurrentOrPreviousPage").val());
                        }
                    }
                    else {
                        CommonHelper.AlertMessage(data.d.AlertMessage);
                        $("#EntryPanel").hide();
                        $("#SearchPanel").show();
                    }
                },
                error: function (result) {
                    //alert("Error");
                }
            });
        }

        function LoadInbox() {
            $("#EntryPanel").hide();
            $("#SearchPanel").show();

            $("#ContentPlaceHolder1_MessageSubject").text("");
            $("#ContentPlaceHolder1_MessageBody").text("");
        }

    </script>
    <asp:HiddenField ID="hfIsCurrentOrPreviousPage" runat="server" Value="" />
    <%--<div id="MessageInboxShow" style="">
        <div class="panel panel-default">
            <div class="panel-heading">
                <div class="navbar" style="z-index: 0;">
                    <div class="navbar-inner">
                        <ul class="nav pull-left">
                             <li id="message-menu" class="dropdown"><a href="javascript:void();" onclick="javascript:return LoadInbox();"
                                style="border-left: none;" class="dropdown-toggle text-white" data-toggle="dropdown"><i class="icon-envelope"></i>&nbsp;Inbox&nbsp;&nbsp;<span id="MessageCountBadge" runat="server" class="badge">
                                    <asp:Label ID="lblMessageCount" runat="server" Text="0" Font-Bold="true"></asp:Label></span>
                            </a></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>--%>

    <div id="MessageInboxShow" style="">
            <nav class="navbar navbar-default">
                <div class="container-fluid">
                    <div class="navbar-static-top">
                        <ul class="nav navbar-nav navbar-left">
                            <%--<li id="message-menu" class="dropdown"><a href="javascript:void(0)" class="dropdown-toggle"
                                data-toggle="dropdown"><i class="icon-envelope"></i>&nbsp;Mail Box&nbsp;&nbsp;<span
                                    id="Span1" class="badge" runat="server">
                                    <asp:Label ID="Label1" runat="server" Text="0" Font-Bold="true"></asp:Label></span>
                            </a>
                                <ul class="dropdown-menu" runat="server" id="MessageBriefDescription" style="overflow-x: scroll; width: 560px;">
                                </ul>
                            </li>--%>

                            <li id="message-menu" ><a href="javascript:void();" onclick="javascript:return LoadInbox();"
                                style="border-left: none;" class="dropdown-toggle " data-toggle="dropdown"><i class="icon-envelope"></i>&nbsp;Inbox&nbsp;&nbsp;<span id="MessageCountBadge" runat="server" class="badge">
                                    <asp:Label ID="lblMessageCount" runat="server" Text="0" Font-Bold="true"></asp:Label></span>
                            </a></li>
                        </ul>
                    </div>
                </div>
            </nav>
    </div>
    <div id="EntryPanel" class="block panel panel-default" >



        <div class="panel-heading">
            Message Details
            
        </div>
        <div class=" panel-body">

            <div class="row-fluid">
                <h2 class="span12" id="MessageSubject" runat="server" style="border-bottom: 1px solid #e5e5e5; padding: 5px;"></h2>
            </div>
            <div class="row-fluid" style="height: 55vh; overflow-y: auto;">
                <p class="span12" id="MessageBody" runat="server" style="padding: 5px; height: 50vh;">
                </p>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Send Message Information
            
        </div>
        <div class=" panel-body">
            <table cellspacing='0' class="table" cellpadding='4' id='gvInboxMessage' width="100%">
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold; width: 100%;">
                        <th style="width: 23%;">Sender Name
                        </th>
                        <th style="width: 62%;">Subject
                        </th>
                        <th style="width: 15%;">Date & Time
                        </th>
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
</asp:Content>
