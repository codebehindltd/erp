<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="GatePassInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.Maintenance.GatePassInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var cancelItem = [], approvedItem = [];
        var str = "";

        $(document).ready(function () {

            str = Cookies.get('frmtoDate');

            if (str != undefined) {
                var str = Cookies.get('frmtoDate');
                str = str.split(',');
                $("#ContentPlaceHolder1_txtFromDate").val(str[0]);
                $("#ContentPlaceHolder1_txtToDate").val(str[1]);
                $("#ContentPlaceHolder1_ddlStatus").val(str[2]);
                Cookies.remove('frmtoDate');
                $("#ContentPlaceHolder1_btnSearch").trigger("click");
            }

            $("#ContentPlaceHolder1_txtFromDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            });
            $("#ContentPlaceHolder1_txtToDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#btnApprove").click(function () {

                CommonHelper.SpinnerOpen();

                var gatePassId = "0", gatePassDetailsId = "0";
                var quantity = "", approvedQuantity = "";

                cancelItem = []; approvedItem = [];

                gatePassId = $("#ContentPlaceHolder1_hfGatePassId").val();

                $("#tbGatePassDetailsGrid tbody tr").each(function (index, item) {

                    if ($(item).find("td:eq(0)").find("input").is(':checkbox')) {

                        quantity = $.trim($(item).find("td:eq(3)").text());
                        approvedQuantity = $.trim($(item).find("td:eq(4)").find("input").val());
                        gatePassDetailsId = $.trim($(item).find("td:eq(5)").text());

                        if ($(item).find("td:eq(0)").find("input").is(":checked") && !$(item).find("td:eq(0)").find("input").is(":disabled")) {

                            if (approvedQuantity == "" || approvedQuantity == "0") {
                                approvedQuantity = quantity;
                            }

                            approvedItem.push({
                                GatePassItemId: parseInt(gatePassDetailsId, 10),
                                GatePassId: parseInt(gatePassId, 10),
                                Quantity: parseFloat(quantity),
                                ApprovedQuantity: parseFloat(approvedQuantity)

                            });
                        }
                        else if (!$(item).find("td:eq(0)").find("input").is(":disabled")) {
                            cancelItem.push({
                                GatePassItemId: parseInt(gatePassDetailsId, 10),
                                GatePassId: parseInt(gatePassId, 10),
                                ApprovedQuantity: parseFloat(0),
                                Status: "Cancel"
                            });
                        }
                    }
                });


                PageMethods.CheckOrApprovedGatePass(gatePassId, approvedItem, cancelItem, OnCheckOrApprovedGatePassSucceed, OnCheckOrApprovedGatePassFailed);

                return false;
            });
        });

        function OnCheckOrApprovedGatePassSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $("#DetailsGatePassDialog").dialog("close");

                $("#ContentPlaceHolder1_hfGatePassId").val("0");
                pendingItem = [];
                approvedItem = [];

                //PerformClearAction();
                $("#ContentPlaceHolder1_btnSearch").trigger("click");
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            CommonHelper.SpinnerClose();
        }
        function OnCheckOrApprovedGatePassFailed(error) {
            CommonHelper.SpinnerClose();
        }

        function SearchGatePassList(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#tbGatePassDetails tbody tr").length;
            var fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();
            var status = $("#ContentPlaceHolder1_ddlStatus").val();

            PageMethods.SearchGatePassList(fromDate, toDate, status, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnsuccessLoading, OnfailLoading);
            return false;
        }

        function OnsuccessLoading(result) {
            $("#tbGatePassDetails tbody tr").remove();
            $("#GridPagingContainer ul").html("");
            var tr = "", totalRow = 0, editLink = "", deleteLink = "", invoiceLink = "", approvalLink = "";
            var editPermission = true, deletePermission = true, approvalPermission = true;

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"7\" >No Data Found</td> </tr>";
                $("#tbGatePassDetails tbody ").append(emptyTr);
                return false;
            }

            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#tbGatePassDetails tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:10%;\">" + gridObject.GatePassNumber + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + GetStringFromDateTime(gridObject.GatePassDate) + "</td>";
                tr += "<td align='left' style=\"width:20%\">" + gridObject.CreatedByPerson + "</td>";
                tr += "<td align='left' style=\"width:10%\">" + gridObject.Status + "</td>";

                if (gridObject.IsCanEdit)
                    editLink = "<a href=\"javascript:void();\" onclick=\"javascript:return PerformEdit('" + gridObject.GatePassId + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" /> </a>";

                if (gridObject.IsCanDelete)
                    deleteLink = "<a href=\"javascript:void();\" onclick=\"javascript:return PerformCancelAction('" + gridObject.GatePassId + "');\"><img alt=\"Cancel\" src=\"../Images/delete.png\" /></a>";

                if (gridObject.IsCanChecked || gridObject.IsCanApproved)
                    approvalLink = "<a href=\"javascript:void();\" onclick=\"javascript:return CheckOrApproveGatePass('" + gridObject.GatePassId + "');\"><img alt=\"Approval\" src=\"../Images/detailsInfo.png\" /></a>";

                invoiceLink = "<a href=\"javascript:void();\" onclick=\"javascript:return ViewInvoice('" + gridObject.GatePassId + "," + gridObject.SupplierId + "');\"><img alt=\"Invoice\" src=\"../Images/ReportDocument.png\" /></a>";

                tr += "<td align='center' style=\"width:10%;\">" + editLink + deleteLink + approvalLink + invoiceLink + "</td>";

                tr += "</tr>";

                $("#tbGatePassDetails tbody").append(tr);
                tr = "";
                editLink = "";
                deleteLink = "";
                approvalLink = "";
            });
            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

        }
        function OnfailLoading() {

        }

        function PerformEdit(GatePassId) {
            var temp = $("#ContentPlaceHolder1_txtFromDate").val() + ',' +
                        $("#ContentPlaceHolder1_txtToDate").val() + ',' +
                        $("#ContentPlaceHolder1_ddlStatus").val();

            Cookies.set('frmtoDate', temp, { expires: 1 });
            var url = "/Maintenance/frmGatePass.aspx?GpId=" + GatePassId;
            window.location = url;
        }
        function CheckOrApproveGatePass(GatePassId) {
            $("#ContentPlaceHolder1_hfGatePassId").val(GatePassId);
            PageMethods.GetGatePassDetailsForApproval(GatePassId, OnFillGatePassDetailsSucceed, OnFillGatePassDetailsFailed);
            return false;
        }

        function OnFillGatePassDetailsSucceed(result) {

            $("#DetailsGatePassGridContainer").html(result);

            $("#DetailsGatePassDialog").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 700,
                maxWidth: 900,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Gate Pass Details",
                show: 'slide'
            });
            CommonHelper.SpinnerClose();
        }
        function OnFillGatePassDetailsFailed(error) {
            toastr.error(error.get_message());
        }

        function PerformCancelAction(GatePassId) {
            var doCancel = confirm("Do you want to cancel the reqistion?");
            if (doCancel)
                PageMethods.CancelGatePass(GatePassId, OnsuccessCancel, OnfailCancel);
            return false;

        }
        function OnsuccessCancel(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $("#ContentPlaceHolder1_hfGatePassId").val("0");
                $("#ContentPlaceHolder1_btnSearch").trigger("click");
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            CommonHelper.SpinnerClose();
        }
        function OnfailCancel(error) {
            toastr.error(error.get_message());
        }

        function ViewInvoice(QueryString) {
            var arr = QueryString.split(',');
            var GatePassId = arr[0];
            var SupplierId = arr[1];
            var url = "/Maintenance/Reports/frmReportGatePassInvoice.aspx?GpId=" + GatePassId + "&SupId=" + SupplierId;
            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=800,height=680,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");

        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchGatePassList(pageNumber, IsCurrentOrPreviousPage);
        }

    </script>
    <div id="DetailsGatePassDialog" style="display: none;">
        <div id="DetailsGatePassGridContainer">
        </div>
        <div class="HMContainerRowButton" style="padding-bottom: 0; padding-top: 10px;">
            <input id="btnApprove" type="button" class="TransactionalButton btn btn-primary btn-sm"
                value="Check GatePass" />
        </div>
    </div>
    <asp:HiddenField ID="hfGatePassId" runat="server" Value="0" />
    <div class="panel panel-default">
        <div class="panel-heading">
            Gate Pass Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label CssClass="control-label" runat="server" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control" ID="txtFromDate" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label CssClass="control-label" runat="server" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control" ID="txtToDate" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label CssClass="control-label" runat="server" Text="Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList CssClass="form-control" ID="ddlStatus" runat="server">
                            <asp:ListItem Value="Pending">Pending</asp:ListItem>
                            <asp:ListItem Value="Checked">Checked</asp:ListItem>
                            <asp:ListItem Value="Partially Checked">Partially Checked</asp:ListItem>
                            <asp:ListItem Value="Approved">Approved</asp:ListItem>
                            <asp:ListItem Value="Partially Approved">Partially Approved</asp:ListItem>
                            <asp:ListItem Value="Cancel">Cancel</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="return SearchGatePassList(1,1);"
                            CssClass="TransactionalButton btn btn-primary btn-sm" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return PerformClearAction();" />
                    </div>
                </div>
            </div>
            <div>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Search Information
                    </div>
                    <div class="panel-body">
                        <table class="table table-bordered table-condensed table-responsive" id='tbGatePassDetails'
                            width="100%">
                            <colgroup>
                                <col style="width: 10%;" />
                                <col style="width: 10%;" />
                                <col style="width: 20%;" />
                                <col style="width: 10%;" />
                                <col style="width: 10%;" />
                            </colgroup>
                            <thead>
                                <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                    <td>Gate Pass No
                                    </td>
                                    <td>Date
                                    </td>
                                    <td>Created By
                                    </td>
                                    <td>Status
                                    </td>
                                    <td style="text-align: center;">Action
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
                <div class="row">
                    <asp:Button Style="float: right" ID="btnNewGatePass" runat="server" Text="New Gate Pass" CssClass="TransactionalButton btn btn-primary btn-sm" OnClick="btnNewGatePass_Click" />
                </div>
            </div>

        </div>
    </div>
</asp:Content>
