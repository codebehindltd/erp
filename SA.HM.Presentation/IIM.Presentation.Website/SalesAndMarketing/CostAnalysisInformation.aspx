<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="CostAnalysisInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.CostAnalysisInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {

            $("#ContentPlaceHolder1_txtSearchFromDate").datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {

                    var strDate = CommonHelper.DateFormatToMMDDYYYY($(this).val(), '/');
                    minEndDate = GetStringFromDateTime(CommonHelper.DaysAdd(strDate, 1));

                    $("#ContentPlaceHolder1_txtSearchToDate").datepicker("option", {
                        minDate: minEndDate
                    });
                }
            }).datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_txtSearchToDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                minDate: DayOpenDate,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtSearchFromDate").datepicker("option", "maxDate", selectedDate);

                }
            }).datepicker("setDate", DayOpenDate);

            GridPaging(1, 1);
        });
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#tblCostAnalysis tbody tr").length;
            var name, fromDate, toDate;

            name = $("#ContentPlaceHolder1_txtSearchName").val();
            fromDate = $("#ContentPlaceHolder1_txtSearchFromDate").val();
            toDate = $("#ContentPlaceHolder1_txtSearchToDate").val();

            PageMethods.GetCostAnalysisWithPagination(name, fromDate, toDate, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchSuccess, OnSearchFail);
            return false;
        }

        function OnSearchSuccess(result) {

            var format = innBoarDateFormat.replace('mm', 'MM');
            var format = format.replace('yy', 'yyyy');

            $("#tblCostAnalysis tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"6\" >No Data Found</td> </tr>";
                $("#tblCostAnalysis tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#tblCostAnalysis tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:70%; \">" + gridObject.Name + "</td>";
                tr += "<td align='left' style=\"width:15%; \">" + GetStringFromDateTime(gridObject.CreatedDate) + "</td>";

                tr += "<td>";

                tr += "<img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction(" + gridObject.Id + ",'" + gridObject.Name + "')\" alt='Update Cost Analysis' border='0' />";

                tr += "</td>";
                tr += "</tr>";

                $("#tblCostAnalysis tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            return false;
        }

        function OnSearchFail(error) {
            toastr.error(error.get_message());
        }

        function CreateNewCostAnalysis() {

            var iframeid = 'frmPrint';
            var url = "./CostAnalysis.aspx";
            parent.document.getElementById(iframeid).src = url;

            $("#CreateCostAnalysis").dialog({
                autoOpen: true,
                modal: true,
                width: 1300,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Create New",
                show: 'slide'
            });
            return false;
        }

        function PerformEditAction(id, name) {
            if (!confirm("Do you want to edit - " + name + "?")) {
                return false;
            }

            var iframeid = 'frmPrint';
            var url = "./CostAnalysis.aspx?caid=" + id;
            parent.document.getElementById(iframeid).src = url;

            $("#CreateCostAnalysis").dialog({
                autoOpen: true,
                modal: true,
                width: 1200,
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: `Update - ${name}`,
                show: 'slide'
            });

            return false;
        }

        function CloseDialog() {
            $("#CreateCostAnalysis").dialog('close');
            return false;
        }

        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }

        function Clear() {
            $("#ContentPlaceHolder1_txtSearchName").val("");
            $("#ContentPlaceHolder1_txtSearchFromDate").val(DayOpenDate);
            $("#ContentPlaceHolder1_txtSearchToDate").val(DayOpenDate);
        }
    </script>
    <div id="CreateCostAnalysis" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Cost Analysis
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Name</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">From Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">To Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchToDate" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <input type="button" class="TransactionalButton btn btn-primary btn-sm" value="Search" onclick="GridPaging(1, 1)" />
                        <input type="button" class="btn btn-primary btn-sm" value="Clear" onclick="Clear()" />
                        <input type="button" class="TransactionalButton btn btn-primary btn-sm" value="Create New" onclick="CreateNewCostAnalysis()" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">Search Information</div>
        <div class="panel-body">
            <table id='tblCostAnalysis' class="table table-bordered table-condensed table-responsive">
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                        <td style="width: 70%;">Name
                        </td>
                        <td style="width: 15%;">Date
                        </td>
                        <td style="width: 15%;">Action
                        </td>
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
