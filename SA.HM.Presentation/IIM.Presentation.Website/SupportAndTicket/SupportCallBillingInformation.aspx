<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="SupportCallBillingInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.SupportAndTicket.SupportCallBillingInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var ClientSelected = null;
        var SupportCallBillingInformationTable;
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;

        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;

            SupportCallBillingInformationTable = $("#tblSupportCallBillingInformation").DataTable({
                data: [],
                columns: [
                    { title: "", "data": "Id", visible: false },
                    { title: "Ticket Number", "data": "CaseNumber", sWidth: '8%' },
                    { title: "Date", "data": "CreatedDate", sWidth: '8%' },
                    { title: "Case", "data": "CaseName", sWidth: '25%' },
                    { title: "Client", "data": "CompanyName", sWidth: '25%' },
                    { title: "Ticket Status", "data": "SupportStatus", sWidth: '5%' },
                    { title: "Imp. Status", "data": "TaskStatus", sWidth: '5%' },
                    { title: "Bill Status", "data": "BillStatus", sWidth: '5%' },
                    { title: "Pass Day", "data": "PassDay", sWidth: '4%' },
                    { title: "Action", "data": null, sWidth: '15%' }
                ],
                columnDefs: [

                    {
                        "targets": 2,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            //debugger;
                            return CommonHelper.DateFromDateTimeToDisplay(data, innBoarDateFormat);
                        }
                    }],
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

                    if (IsCanEdit && aData.BillStatus == 'Pending') {
                        if (isAllOptionDilabled == "0") {
                            row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return GenerateSupportBill('" + aData.Id + "', '" + aData.CaseNumber + "');\"> <img alt=\"Generate\" src=\"../Images/cashAdjustment.png\" title='Generate' /> </a>";
                        }
                    }

                    if (IsCanEdit && aData.BillStatus != 'Pending') {
                        if (isAllOptionDilabled == "0") {
                            row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformBillPreviewAction('" + aData.Id + "');\"> <img alt=\"Preview Bill\" src=\"../Images/ReportDocument.png\" title='Preview Bill' /> </a>";
                            row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformBillPreviewActionWithSignature('" + aData.Id + "');\"> <img alt=\"Preview Bill with Signature\" src=\"../Images/ReportDocument.png\" title='Preview Bill with Signature' /> </a>";
                        }
                    }

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
                        url: '../SupportAndTicket/SupportCallBillingInformation.aspx/ClientSearch',
                        data: JSON.stringify({ searchTerm: request.term }),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.CompanyName,
                                    value: m.CompanyId,
                                    CompanyId: m.CompanyId
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
        });

        function PerformBillPreviewAction(billId) {
            var url = "";
            var popup_window = "Print Preview";
            url = "/SupportAndTicket/Reports/frmSTBillInfo.aspx?billID=" + billId;
            window.open(url, popup_window, "width=750,height=680,left=300,top=50,resizable=yes");
        }

        function PerformBillPreviewActionWithSignature(billId) {
            var url = "";
            var popup_window = "Print Preview";
            url = "/SupportAndTicket/Reports/frmSTBillInfo.aspx?US=Y&billID=" + billId;
            window.open(url, popup_window, "width=750,height=680,left=300,top=50,resizable=yes");
        }

        function OnSuccessGenerateSupportBill(result) {
            if (result.IsSuccess) {
                if (result.IsSuccess) {
                    ShowAlert(result.AlertMessage);
                    SearchInformation(1, 1);
                }
            }
        }

        function OnFailGenerateSupportBill(error) {
            isClose = false;
            toastr.error(error.get_message());
            return false;
        }

        function SearchInformation(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = SupportCallBillingInformationTable.data().length;
            var clientName = $("#ContentPlaceHolder1_txtClientName").val();
            var clientId = $("#ContentPlaceHolder1_hfClientId").val();
            var caseId = $("#ContentPlaceHolder1_ddlCase").val();
            var caseNumber = $("#ContentPlaceHolder1_txtCaseNumber").val();
            var fromDate = $("#ContentPlaceHolder1_txtSearchFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtSearchToDate").val();
            var billingStatus = $("#ContentPlaceHolder1_ddlBillingStatus").val();

            if (clientName == "") {
                clientId = 0;
            }

            PageMethods.GetSupportCallBillingInformationForGridPaging(clientId, caseId, caseNumber, fromDate, toDate, billingStatus, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSupportCallInformationLoadingSucceed, OnSupportCallInformationLoadingFailed);
            return false;
        }

        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }

        function OnSupportCallInformationLoadingSucceed(result) {
            SupportCallBillingInformationTable.clear();
            SupportCallBillingInformationTable.rows.add(result.GridData);
            SupportCallBillingInformationTable.draw();

            $("#GridPagingContainer ul").html("");
            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
            return false;
        }

        function OnSupportCallInformationLoadingFailed(error) {
            toastr.error(error.get_message());
            return false;
        }

        function GenerateSupportBill(Id, CaseNumber) {
            if (!confirm("Want to generate bill?")) {
                return false;
            }
            //debugger;

            var iframeid = 'frmPrint';
            var url = "./SupportCallDetailsIframe.aspx?sc=" + Id;
            document.getElementById(iframeid).src = url;
            $("#SupportCallDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 700,
                closeOnEscape: false,
                resizable: false,
                title: "Generate Support Bill : " + CaseNumber,
                show: 'slide'
            });
            return false;
        }

        function CloseDialog() {
            $("#SupportCallDialouge").dialog('close');
            return false;
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
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <div id="voucherDocuments" style="display: none;">
        <div id="imageDiv" style="overflow: auto; height: 500px;"></div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Billing Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <asp:HiddenField ID="hfClientId" runat="server" Value="0" />
                <div id="SupportCallDialouge" style="display: none;">
                    <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
                        clientidmode="static" scrolling="yes"></iframe>
                </div>
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
                        <label class="control-label">Billing Status</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList runat="server" ID="ddlBillingStatus" CssClass="form-control">
                            <asp:ListItem Text="--- All ---" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                            <asp:ListItem Text="Generate" Value="Generate"></asp:ListItem>
                            <asp:ListItem Text="Full Payment" Value="Full Payment"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return SearchInformation(1,1);" />
                </div>
            </div>
            <div>
                <table id="tblSupportCallBillingInformation" class="table table-bordered table-condensed table-responsive">
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
</asp:Content>
