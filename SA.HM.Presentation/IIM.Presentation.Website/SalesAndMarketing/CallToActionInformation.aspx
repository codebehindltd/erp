<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="CallToActionInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.CallToActionInformation" %>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var CallToActionDetails = new Array();
        var ReminderDay = new Array();
        var CallToActionDetailsDeleted = new Array();
        var i = 0;
        $(document).ready(function () {
            $("#ContentPlaceHolder1_txtFromDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#ContentPlaceHolder1_txtToDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            GridPaging(1, 1);
        });
        function CreateNew() {
            var iframeid = 'frmPrint';
            var url = "../SalesAndMarketing/CallToActionFrame.aspx?fca=Menu Links";
            document.getElementById(iframeid).src = url;
            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "96%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                title: "Call To Action",
                show: 'slide'
            });
            return false;
        }
        function FillFormEdit(id) {
            var iframeid = 'frmPrint';
            var url = "../SalesAndMarketing/CallToActionDetailsEditIFrame.aspx?id=" + id;
            document.getElementById(iframeid).src = url;
            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "96%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                title: "Call To Action",
                show: 'slide'
            });
            return false;
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadGrid(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }

        function LoadGrid(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#SourceTable tbody tr").length;
            var callToActionFor = $("#ContentPlaceHolder1_txtReminderFor").val();
            var fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            if (fromDate != "") {
                fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');
            }
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();
            if (toDate != "") {
                toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');
            }
            var company = $("#ContentPlaceHolder1_txtCompanyForSrc").val();
            var contact = $("#ContentPlaceHolder1_txtContactForSrc").val();

            PageMethods.SearchCallToAction(callToActionFor, fromDate, toDate, company, contact, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchucceed, OnSearchFailed);
            return false;
        }

        function OnSearchucceed(data) {
            LoadTable(data)
        }
        function OnSearchFailed(error) {

        }
        function LoadTable(data) {

            $("#SourceTable tbody").empty();
            $("#GridPagingContainer ul").empty();
            i = 0;

            $.each(data.GridData, function (count, gridObject) {

                var tr = "";

                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:6%;'>" + gridObject.Type + "</td>";
                tr += "<td style='width:8%;'>" + gridObject.TaskName + "</td>";
                tr += "<td style='width:8%;'>" + moment(gridObject.Date).format("DD/MM/YYYY") + "</td>";
                tr += "<td style='width:8%;'>" + moment(gridObject.Time).format("hh:mm A") + "</td>";
                tr += "<td style='width:8%;'>" + gridObject.CompanyName + "</td>";
                tr += "<td style='width:8%;'>" + gridObject.ContactName + "</td>";
                tr += "<td style='width:17%;'>" + gridObject.PerticipentFromOfficeName + "</td>";
                tr += "<td style='width:17%;'>" + gridObject.PerticipentFromClientName + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.TaskAssignedEmployeeName + "</td>";

                tr += "<td style='width:5%;'> <a onclick=\"javascript:return FillFormEdit(" + gridObject.Id + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                //tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'DeleteSource(" + gridObject.Id + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";

                tr += "<td style='display:none'>" + gridObject.Id + "</td>";
                tr += "<td style='display:none'>" + gridObject.MasterId + "</td>";
                tr += "<td style='display:none'>" + gridObject.CompanyId + "</td>";
                tr += "<td style='display:none'>" + gridObject.ContactId + "</td>";


                tr += "</tr>";

                $("#SourceTable tbody").append(tr);

                tr = "";
                i++;
            });
            $("#GridPagingContainer ul").append(data.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(data.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(data.GridPageLinks.NextButton);
            return false;
        }
        function CloseDialog() {
            $("#SalesNoteDialog").dialog('close');
            return false;
        }
    </script>
    <%--<UserControl:CallToActionUserControl ID="CallToActionUserControl" runat="server" />--%>
    <asp:HiddenField ID="CommonDropDownHiddenField" Value="0" runat="server" />
    <asp:HiddenField ID="hfPerticipentFromClient" Value="" runat="server" />
    <asp:HiddenField ID="hfPerticipentFromOffice" Value="" runat="server" />
    <asp:HiddenField ID="hfReminderDaysList" Value="" runat="server" />
    <asp:HiddenField ID="hfCallToActionId" Value="0" runat="server" />
    <asp:HiddenField ID="hfCompanyId" Value="0" runat="server" />
    <asp:HiddenField ID="hfContactId" Value="0" runat="server" />
    <div class="panel panel-default">
        <div class="panel-heading">
            Call To Action Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Reminder For</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtReminderFor" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">From Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">To Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Company</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCompanyForSrc" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Contact</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtContactForSrc" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return GridPaging(1,1);" />
                        <asp:Button ID="btnClean" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return Clean();" />
                        <asp:Button ID="btnCreateNew" runat="server" Text="New Call To Action" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateNew();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information
        </div>
        <div class="panel-body">
            <div class="form-group" id="CallToActionContainer">
                <table class="table table-bordered table-condensed table-responsive" id="SourceTable"
                    style="width: 100%;">
                    <thead>
                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                            <th style="width: 6%;">Task Type</th>
                            <th style="width: 8%;">Task Name</th>
                            <th style="width: 8%;">Date</th>
                            <th style="width: 8%;">Time</th>
                            <th style="width: 8%;">Company</th>
                            <th style="width: 8%;">Contact</th>
                            <th style="width: 17%;">Perticipent From Office</th>
                            <th style="width: 17%;">Perticipent From Office</th>
                            <th style="width: 15%;">Assigned Employee</th>
                            <th style="width: 5%;">Action</th>
                            <th style="display: none">Id
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
    </div>
    <div id="SalesNoteDialog" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>

</asp:Content>
