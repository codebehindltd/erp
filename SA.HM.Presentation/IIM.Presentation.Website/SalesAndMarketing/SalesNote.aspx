<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="SalesNote.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.SalesNote" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(() => {

            QuotationTable = $("#tblQuotation").DataTable({
                data: [],
                columns: [
                    { title: "Quotation Number", data: "QuotationNo", width: "20%" },
                    { title: "Deal Name", data: "DealName", width: "20%" },
                    { title: "Company", data: "CompanyName", width: "20%" },
                    { title: "Proposal Date", data: "ProposalDate", width: "20%" },
                    { title: "Action", data: null, width: "20%" },
                    { title: "", data: "QuotationId", visible: false }
                ],
                rowCallback: (row, data, displayNum, displayIndex, dataIndex) => {

                    var tableRow = "";

                    if (displayIndex % 2 == 0) {
                        $('td', row).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', row).css('background-color', '#FFFFFF');
                    }
                    //if (!data.IsSalesNoteFinal)

                    if ('<%=IsTechnicalUser%>' == 'True')
                        tableRow += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return UpdateSalesNote('" + data.QuotationId + "');\"> <img alt=\"Sales Note Entry\" src=\"../Images/note.png\" title='Sales Note Entry' /> </a>";
                    if (data.IsSalesNoteFinal &&'<%=IsInventoryUser%>' == 'True' && data.HasItem)
                        tableRow += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return SalesOut('" + data.QuotationId + "');\"> <img alt=\"Sales Out\" src=\"../Images/detailsInfo.png\" title='Sales Out' /> </a>";

                    tableRow += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'ShowQuotationInvoice(" + data.QuotationId + ")' title='Quotation Invoice' ><img style='width:16px;height:16px;' alt='Quotation Invoice' src='../Images/ReportDocument.png' /></a>";

                    $('td:eq(' + (row.children.length - 1) + ')', row).html(tableRow);
                },
                info: false,
                ordering: false,
                processing: false,
                paging: false,
                filter: false,
                language: {
                    emptyTable: "No Data Found"
                }

            });

            $("#ContentPlaceHolder1_txtFromDate").datepicker({
                dateFormat: innBoarDateFormat,
                defaultDate: DayOpenDate,
                changeMonth: true,
                changeYear: true
            });

            $("#ContentPlaceHolder1_txtToDate").datepicker({
                dateFormat: innBoarDateFormat,
                defaultDate: DayOpenDate,
                changeMonth: true,
                changeYear: true
            });

            $("#ContentPlaceHolder1_ddlCompany").select2({
                tags: false,
                allowClear: true,
                width: "99.75%"
            });

            SearchQuotation();

        });

        var GridPaging = (pageNumber, isCurrentOrPreviousPage) => {

            let gridRecordsCount = QuotationTable.data().length;

            let quotationNumber = $("#ContentPlaceHolder1_txtQuotationNumber").val().trim();
            let companyId = $("#ContentPlaceHolder1_ddlCompany").val();
            let fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            let toDate = $("#ContentPlaceHolder1_txtToDate").val() ;

            if (fromDate == "" && toDate != "") {
                fromDate = CommonHelper.DateFormatToMMDDYYYY(DayOpenDate, '/');
            }
            if (fromDate != "" && toDate == "") {
                toDate = CommonHelper.DateFormatToMMDDYYYY(DayOpenDate, '/');
            }

            fromDate = fromDate != "" ? CommonHelper.DateFormatToMMDDYYYY(fromDate, '/') : null;
            toDate = toDate != "" ? CommonHelper.DateFormatToMMDDYYYY(toDate, '/') : null;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "./SalesNote.aspx/SearchQuotation",
                dataType: "json",
                data: JSON.stringify({ quotationNumber: quotationNumber, companyId: companyId, fromDate: fromDate, toDate: toDate, gridRecordsCount: gridRecordsCount, pageNumber: pageNumber, isCurrentOrPreviousPage: isCurrentOrPreviousPage }),
                async: false,
                success: (data) => {
                    GenerateQuotationTable(data);
                },
                error: (error) => {
                    toastr.error(error, "", { timeOut: 5000 });
                }
            });
            return false;
        }

        var SearchQuotation = () => {

            GridPaging(1, 1);
        }

        var GenerateQuotationTable = (result) => {

            result.d.GridData.map(row => { row.ProposalDate = CommonHelper.DateFromStringToDisplay(row.ProposalDate, innBoarDateFormat); });

            QuotationTable.clear();
            QuotationTable.rows.add(result.d.GridData);
            QuotationTable.draw();

            $("#GridPagingContainer ul").html("");

            $("#GridPagingContainer ul").append(result.d.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.d.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.d.GridPageLinks.NextButton);

            return true;
        }

        var UpdateSalesNote = (quotationId) => {
            var iframeid = 'frmPrint';
            var url = "./SalesNoteEntry.aspx?qid=" + quotationId;
            parent.document.getElementById(iframeid).src = url;
            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: 1100,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Sales Note",
                show: 'slide'

            });
        }
        var SearchQuotationForSalesNote = () => {
            GridPaging(1, 1);
        }
        var ShowQuotationInvoice = (quotationId) => {
            var url = "/SalesAndMarketing/Reports/SMQuotationDetailsReport.aspx?id=" + quotationId;
            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=800,height=680,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
        }

        var SalesOut = (QuotationId) => {
            window.location.href = "../Inventory/ItemTransfer.aspx?isfrmSNote=1&qid=" + QuotationId;
        }

    </script>
    <div id="SalesNoteDialog" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Search Quotation
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Quotaion Number</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtQuotationNumber" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Company</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCompany" runat="server" class="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Proposal Date From</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Proposal Date To</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" runat="server" class="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <input type="button" class="btn btn-primary btn-sm" value="Search" onclick="SearchQuotation()" />
                        <input type="button" class="btn btn-primary btn-sm" value="Clear" />
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-heading">
            Search Information
        </div>
        <div class="panel-body">
            <table id="tblQuotation" class="table table-bordered table-condensed table-responsive">
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
