<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="CustomNotice.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.CustomNotice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            GridPaging(1, 1);

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
        function CreateNewNotice() {
            var iframeid = 'frmPrint';
            var url = "./CustomNoticeIframe.aspx";
            parent.document.getElementById(iframeid).src = url;

            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '100%',
                height: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "New Notice",
                show: 'slide'
            });
        }
        function CloseDialog() {
            $("#SalesNoteDialog").dialog('close');
            return false;
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }
        function PerformClearAction() {
            $("#ContentPlaceHolder1_txtNoticeName").val('');
            $("#ContentPlaceHolder1_txtFromDate").val('');
            $("#ContentPlaceHolder1_txtToDate").val('');

            return false;
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var id = 0, noticeName = "", contactId = 0, companyId = 0, dealStageId = 0;

            var gridRecordsCount = $("#NoticeGrid tbody tr").length;

            noticeName = $("#ContentPlaceHolder1_txtNoticeName").val();

            var fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();

            if (fromDate != "") {
                fromDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtFromDate").val(), '/');
            }
            if (toDate != "") {
                toDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtToDate").val(), '/');
            }

            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../HMCommon/CustomNotice.aspx/LoadGridPaging',
                data: JSON.stringify({ name: noticeName, fromDate: fromDate, toDate: toDate, gridRecordsCount: gridRecordsCount, pageNumber: pageNumber, IsCurrentOrPreviousPage: IsCurrentOrPreviousPage }),
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

            var rowLength = $("#NoticeGrid tbody tr").length;
            var dataLength = searchData.d.GridData.length;
            $("#NoticeGrid tbody").empty();
            $("#GridPagingContainer ul").empty();
            i = 0;

            if (searchData.d.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"8\" >No Data Found</td> </tr>";
                $("#NoticeGrid tbody ").append(emptyTr);
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
                tr += "<td style='width:20%;'>" + gridObject.NoticeName + "</td>";
                tr += "<td style='width:10%;'>" + (gridObject.CreatedDate != null ? CommonHelper.DateFromStringToDisplay(gridObject.CreatedDate, innBoarDateFormat) : "") + "</td>";
                //tr += "<td style='width:10%;'>" + CommonHelper.DateFromStringToDisplay(gridObject.CreatedDate, innBoarDateFormat) + "</td>";
                tr += "<td style='width:15%;cursor:pointer;'>";
                if (IsCanEdit) {
                    tr += '<a href="javascript:void();" onclick="javascript:return FillFormEdit(' + gridObject.Id + ",\'" + gridObject.NoticeName + '\');"' + "title='Edit' ><img src='../Images/edit.png' alt='Edit'></a>";
                }
                if (IsCanDelete) {
                    tr += '&nbsp;&nbsp;<a href="javascript:void();" onclick= "javascript:return DeleteDeal(' + gridObject.Id + ",\'" + gridObject.NoticeName + '\');" ><img alt="Delete" src="../Images/delete.png" /></a>';
                }
                if (IsCanView) {
                    tr += '&nbsp;&nbsp;<a href="javascript:void();" onclick= "javascript:return ShowInvoice(' + gridObject.Id + ');" title="Documents"><img style="width:16px;height:16px;" alt="Documents" src="../Images/document.png" /></a>';
                }
                tr += "</td>";
                tr += "</tr>";

                $("#NoticeGrid tbody").append(tr);

                tr = "";
                i++;
            });

            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.NextButton);
            return false;
        }

        function ShowInvoice(Id) {
            //var url = "/HMCommon/Reports/CustomeNoticeReport.aspx?nId=" + Id;
            //var popup_window = "Notice Preview";
            //window.open(url, popup_window, "width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../HMCommon/CustomNotice.aspx/LoadNotice",
                dataType: "json",
                data: JSON.stringify({ Id: Id }),
                async: false,
                success: (data) => {
                    $("#SalesNoteDialog").dialog({
                        autoOpen: true,
                        modal: true,
                        width: '75%',
                        height: 580,
                        closeOnEscape: true,
                        resizable: false,
                        fluid: true,
                        title: "Notice",
                        show: 'slide'
                    });
                    $("#frmPrint").attr("src", data.d[0].Path + data.d[0].Name);
                    //window.open(data.d[0].Path + data.d[0].Name);
                },
                error: (error) => {
                    toastr.error(error.d.get_message());
                }
            });
            return false;
        }
        function FillFormEdit(id, name) {
            if (!confirm("Do you want to edit - " + name + "?")) {
                return false;
            }
            var iframeid = 'frmPrint';
            var url = "./CustomNoticeIframe.aspx?nId=" + id;
            parent.document.getElementById(iframeid).src = url;

            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "100%",
                height: 600,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Update - " + name,
                show: 'slide'
            });

            return false;
        }

        function DeleteDeal(id, name) {
            if (confirm("Want to delete " + name + " ?")) {
                $.ajax({

                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../HMCommon/CustomNotice.aspx/DeleteNotice',
                    data: JSON.stringify({ Id: id }),
                    dataType: "json",
                    success: function (data) {
                        GridPaging(1, 1);
                        ShowAlert(data.d.AlertMessage);

                    },
                    error: function (result) {

                    }
                });
            }
            return false;
        }

    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />

    <div id="SalesNoteDialog" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static"></iframe>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Custom Notice
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblNoticeName" runat="server" class="control-label" Text="Notice Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtNoticeName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm" value="Search" onclick="javascript: return GridPaging(1, 1);" />

                        <input type="button" id="btnSearchCancel" class="TransactionalButton btn btn-primary btn-sm" value="Clear" onclick="javascript: return PerformClearAction();" />
                        <input type="button" id="btnNewNotice" class="TransactionalButton btn btn-primary btn-sm" value="New Notice" onclick="javascript: return CreateNewNotice();" />
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
            <table id="NoticeGrid" class="table table-bordered table-condensed table-responsive" width="100%">
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                        <th style="display: none">Id
                        </th>
                        <th style="width: 20%;">Notice Name
                        </th>
                        <th style="width: 20%;">Date
                        </th>
                        <th style="width: 15%;">Action
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
