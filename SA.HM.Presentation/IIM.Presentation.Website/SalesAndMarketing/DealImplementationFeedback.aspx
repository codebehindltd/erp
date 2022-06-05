<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="DealImplementationFeedback.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.DealImplementationFeedback" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var flag = 0;
        var EnginnerList = [];
        $(document).ready(function () {
            GridPaging(1, 1);
            var dealId = 0;

            //************************Script For Deal Search Strats From Here*****************************//
            $("#ContentPlaceHolder1_ddlSearchCompany").select2({
                tags: false,
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlSearchDealOwner").select2({
                tags: false,
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlSearchDealStage").select2({
                tags: false,
                allowClear: true,
                width: "99.75%"
            });

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });
        });


        function AttachFile() {
            $("#implementationDocuments").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Deal Implementation Feedback Documents",
                show: 'slide'
            });
        }
        function CloseDialog() {
            $("#DealImplementationDialog").dialog('close');
            return false;
        }
        //***********************Script For Deal Search Strats From Here*****************************//
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var id = 0, dealName = "", contactId = 0, companyId = 0, dealStageId = 0;

            var gridRecordsCount = $("#DealsGrid tbody tr").length;
            var dealNumber = $("#ContentPlaceHolder1_txtSearchDealNumber").val();
            dealName = $("#ContentPlaceHolder1_txtSearchDealName").val();
            dealStatus = $("#ContentPlaceHolder1_ddlSearchDealStatus").val();
            companyId = $("#ContentPlaceHolder1_ddlSearchCompany").val();
            var dateType = $("#ContentPlaceHolder1_ddlDateType").val();
            var fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();

            if (fromDate != "") {
                fromDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtFromDate").val(), '/');
            }
            if (toDate != "") {
                fromDate = CommonHelper.DateFormatToMMDDYYYY($("#ContnentPlaceHolder1_txtToDate").val(), '/');
            }

            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/DealImplementationFeedback.aspx/LoadGridPaging',
                data: JSON.stringify({ dealNumber: dealNumber, name: dealName, companyId: companyId, dateType: dateType, fromDate: fromDate, toDate: toDate, gridRecordsCount: gridRecordsCount, pageNumber: pageNumber, IsCurrentOrPreviousPage: IsCurrentOrPreviousPage }),
                dataType: "json",
                success: function (data) {

                    LoadTable(data);
                },
                error: function (result) {
                }
            });
            return false;
        }

        function LoadTable(searchData) {

            var rowLength = $("#DealsGrid tbody tr").length;
            var dataLength = searchData.length;
            $("#DealsGrid tbody").empty();
            $("#GridPagingContainer ul").empty();
            i = 0;

            if (searchData.d.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"7\" >No Data Found</td> </tr>";
                $("#DealsGrid tbody ").append(emptyTr);
                return false;
            }

            $.each(searchData.d.GridData, function (count, gridObject) {
                var tr = "";

                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='display:none'>" + gridObject.Id + "</td>";
                tr += "<td style='width:20%'>" + gridObject.Name + "</td>";
                tr += "<td style='width:20%;'>" + (gridObject.CompanyId != 0 ? gridObject.Company : gridObject.ContactName) + "</td>";
                tr += "<td style='width:10%;'>" + CommonHelper.DateFromStringToDisplay(gridObject.StartDate, innBoarDateFormat) + "</td>";
                tr += "<td style='width:10%;'>" + (gridObject.CloseDate != null ? CommonHelper.DateFromStringToDisplay(gridObject.CloseDate, innBoarDateFormat) : "") + "</td>";
                tr += "<td style='width:15%;cursor:pointer;'>";

                //if (gridObject.IsCloseWon)
                tr += '&nbsp;&nbsp;<a href="javascript:void();" onclick= "javascript:return ShowDealImpFeedback(' + gridObject.Id + ",\'" + gridObject.Name + '\');" title="Deal Implementation Feedback"><img style="width:16px;height:16px;" alt="Documents" src="../Images/detailsInfo.png" /></a>';
                tr += "</td>";
                tr += "<td style='display:none'>" + gridObject.NumberId + "</td>";
                tr += "<td style='display:none'>" + gridObject.CompanyId + "</td>";
                tr += "</tr>";

                $("#DealsGrid tbody").append(tr);

                tr = "";
                i++;
            });

            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.NextButton);
            return false;
        }

        function ShowDealImpFeedback(id, name) {
            var iframeid = 'frmPrint';
            var url = "./DealImplementationFeedbackIFrame.aspx?did=" + id;
            parent.document.getElementById(iframeid).src = url;

            $("#DealImplementationDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "100%",
                height: 400,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Deal Implementation Feedback ( " + name + " )",
                show: 'slide'
            });
            return false;
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }
        function PerformClearAction() {
            $("#ContentPlaceHolder1_txtSearchDealNumber").val("");
            $("#ContentPlaceHolder1_txtSearchDealName").val("");
            $("#ContentPlaceHolder1_ddlSearchDealStatus").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlSearchCompany").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtFromDate").val("");
            $("#ContentPlaceHolder1_txtToDate").val("");
        }
    </script>
    <div id="DealImplementationDialog" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Deal Information
            
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblDealNumber" runat="server" class="control-label" Text="Deal Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchDealNumber" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblDealName1" runat="server" class="control-label" Text="Deal Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchDealName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblCompany" runat="server" class="control-label" Text="Company"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSearchCompany" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Date Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlDateType" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Start Date" Value="StartDate" />
                            <asp:ListItem Text="End Date" Value="EndDate" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label required-field" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label required-field" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" style="display: none;">
                    <div class="col-md-2">
                        <asp:Label ID="lblContact" runat="server" class="control-label" Text="Contact Person"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlContact" runat="server" CssClass="form-control" TabIndex="7">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm" value="Search" onclick="javascript: return GridPaging(1, 1);" />

                        <input type="button" id="btnSearchCancel" class="TransactionalButton btn btn-primary btn-sm" value="Clear" onclick="javascript: return PerformClearAction();" />

                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Search Information
        </div>
        <div class="panel-body">
            <table id="DealsGrid" class="table table-bordered table-condensed table-responsive" width="100%">
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                        <th style="display: none">Id
                        </th>
                        <th style="width: 25%;">Deal Name
                        </th>
                        <th style="width: 30%;">Company / Contact person
                        </th>
                        <th style="width: 15%;">Start Date
                        </th>
                        <th style="width: 15%;">End Date
                        </th>
                        <th style="width: 15%;">Action
                        </th>
                        <th style="display: none">CompanyId
                        </th>
                        <th style="display: none">ContactId
                        </th>
                        <th style="display: none">DealStageID
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
