<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="SupportCallImplementation.aspx.cs" Inherits="HotelManagement.Presentation.Website.SupportAndTicket.SupportCallImplementation" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            isContinueSave = false;
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            CommonHelper.ApplyDecimalValidation();
            SupportCall = $("#tblSupportCall").DataTable({
                data: [],
                columns: [
                    { title: "Ticket Number", "data": "CaseNumber", sWidth: '8%' },
                    { title: "Date", "data": "CreatedDate", sWidth: '6%' },
                    { title: "Case", "data": "CaseName", sWidth: '20%' },
                    { title: "Client", "data": "CompanyName", sWidth: '15%' },
                    { title: "Ticket Status", "data": "SupportStatus", sWidth: '5%' },
                    { title: "Bill Status", "data": "BillStatus", sWidth: '5%' },
                    { title: "Imp. Status", "data": "TaskStatus", sWidth: '5%' },
                    { title: "Created By", "data": "CreatedByName", sWidth: '15%' },
                    { title: "Pass Day", "data": "PassDay", sWidth: '5%' },
                    { title: "Action", "data": null, sWidth: '15%' },
                    { title: "", "data": "Id", visible: false },
                    { title: "", "data": "TaskId", visible: false }
                ],
                columnDefs: [
                    {
                        "targets": 1,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    {
                        "targets": 5,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            return (data == "" ? "Pending" : data);
                        }
                    }
                ],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var row = '';
                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }

                    var isAllOptionDilabled = "0";
                    if (aData.SupportStatus == "Decline") {
                        isAllOptionDilabled = "1";
                    }

                    if (aData.SupportStatus != "Done") {
                        if (isAllOptionDilabled == "0") {
                            row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformImplementation('" + aData.Id + "','" + aData.TaskId + "','" + aData.CaseNumber + "');\"> <img alt=\"Implementation Team Assign\" src=\"../Images/member.png\" title='Implementation Team Assign' width='20' height='20'/> </a>";
                        }
                    }

                    row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformTicketView('" + aData.Id + "');\"> <img alt=\"Ticket\" src=\"../Images/ReportDocument.png\" title='Ticket' /> </a>";
                    row += '&nbsp;&nbsp;<a href="javascript:void();" onclick= "javascript:return ShowDocuments(' + aData.Id + ');" title="Documents"><img style="width:16px;height:16px;" alt="Documents" src="../Images/document.png" /></a>';

                    $('td:eq(' + (nRow.children.length - 1) + ')', nRow).html(row);
                },
                pageLength: UserInfoFromDB.GridViewPageSize,
                filter: false,
                info: false,
                ordering: false,
                processing: true,
                retrieve: true,
                bAutoWidth: false,
                bLengthChange: false,
                bInfo: false,
                bPaginate: false,
                language: {
                    emptyTable: "No Data Found"
                },
            });
            $("#ContentPlaceHolder1_txtClientName").autocomplete({
                source: function (request, response) {


                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SupportAndTicket/SupportCallInformation.aspx/ClientSearch',
                        data: JSON.stringify({ searchTerm: request.term }),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.CompanyName,
                                    value: m.CompanyId,
                                    Lvl: m.Lvl,
                                    Hierarchy: m.Hierarchy
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

                    ClientSelected = ui.item;

                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfClientId").val(ui.item.value);
                }
            });
            $("#ContentPlaceHolder1_ddlCase").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $('#ContentPlaceHolder1_txtSearchFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                // defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchToDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtSearchToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            //GridPaging(1, 1);
        });
        function PerformImplementation(Id, TaskId, CaseNumber) {
            if (!confirm("Want to Assign Implementation Team Member for - " + CaseNumber + "?")) {
                return false;
            }
            var iframeid = 'frmPrint';
            var url = "./SupportCallImplementationIframe.aspx?sid=" + Id + "&tid=" + TaskId + "&cno=" + CaseNumber;
            document.getElementById(iframeid).src = url;
            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "90%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Implementation Team Member Assign",
                show: 'slide'
            });
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchSupportCall(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function SearchSupportCall(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = SupportCall.data().length;
            var clientName = $("#ContentPlaceHolder1_txtClientName").val();
            var clientId = $("#ContentPlaceHolder1_hfClientId").val();
            var caseId = $("#ContentPlaceHolder1_ddlCase").val();
            var caseNumber = $("#ContentPlaceHolder1_txtCaseNumber").val();
            var fromDate = $("#ContentPlaceHolder1_txtSearchFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtSearchToDate").val();
            if (fromDate != "")
                fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');

            if (toDate != "")
                toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            if (clientName == "") {
                clientId = 0;
            }

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SupportAndTicket/SupportCallImplementation.aspx/GetSupportCallBySearchCriteria',

                data: JSON.stringify({ clientId: clientId, caseId: caseId, caseNumber: caseNumber, fromDate: fromDate, toDate: toDate, status: '', gridRecordsCount: gridRecordsCount, pageNumber: pageNumber, isCurrentOrPreviousPage: IsCurrentOrPreviousPage }),
                dataType: "json",
                async: false,
                success: function (data) {
                    OnTaskLoadingSucceed(data.d);
                },
                error: function (result) {
                    OnTaskLoadingFailed(result.d);
                }
            });
            return false;
        }

        function OnTaskLoadingSucceed(result) {

            SupportCall.clear();
            SupportCall.rows.add(result.GridData);
            SupportCall.draw();

            $("#GridPagingContainer ul").html("");
            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
            return false;
        }

        function OnTaskLoadingFailed(error) {
            toastr.error(error.get_message());
            return false;
        }
        function CloseDialog() {
            $("#SalesNoteDialog").dialog('close');
            return false;
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
            return false;
        }
        function PerformTicketView(Id) {
            var type = "STicket";
            var url = "/SupportAndTicket/Reports/frmSupportAndTicketReport.aspx?TId=" + Id + "," + type;
            var popup_window = "Support Ticket";
            window.open(url, popup_window, "width=760,height=780,left=300,top=50,resizable=yes");
        }
        function ShowDocuments(id) {
            PageMethods.LoadVoucherDocumentById(id, OnLoadDocumentByIdSucceeded, OnLoadDocumentByIdFailed);
            return false;
        }
        function OnLoadDocumentByIdSucceeded(result) {
            $("#imageDiv").html(result);

            $("#voucherDocuments").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                minHeight: 400,
                closeOnEscape: true,
                resizable: false,
                title: "Support Documents",
                show: 'slide'
            });

            return false;
        }

        function OnLoadDocumentByIdFailed(error) {
            toastr.error(error.get_message());
        }
    </script>
    <asp:HiddenField ID="hfClientId" runat="server" Value="0" />
    <div id="voucherDocuments" style="display: none;">
        <div id="imageDiv" style="overflow: auto; height: 500px;"></div>
    </div>
    <div id="SalesNoteDialog" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Ticket Assignment
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Client Name</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtClientName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Case</label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList runat="server" ID="ddlCase" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="dvSearchDateTime" class="form-group">
                    <div class="col-md-2 ">
                        <label class="control-label ">From Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchFromDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-2 ">
                        <label class="control-label">To Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchToDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Ticket Number</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCaseNumber" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="col-md-2">
                        <label class="control-label">Status</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control" autocomplete="off" TabIndex="6"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return GridPaging(1,1);" />
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

            <table id="tblSupportCall" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
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
