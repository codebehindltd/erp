<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmVoucherEntrySearch.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.frmVoucherEntrySearch"
    EnableEventValidation="false" %>

<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControlVoucherApproval" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Accounts</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Search Voucher</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            var single = $("#ContentPlaceHolder1_hfIsSingle").val();
            if (single == "1") {
                $('#CompanyProjectPanel').hide();
            }
            else {
                $('#CompanyProjectPanel').show();
            }

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
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
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });


            $("#ContentPlaceHolder1_companyProjectUserControl_lblGLCompany").parent().addClass('text-right');
            $("#ContentPlaceHolder1_companyProjectUserControl_lblGLProject").parent().addClass('text-right');

        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            debugger;
            CommonHelper.SpinnerOpen();
            var gridRecordsCount = $("#VoucherSearchGrid tbody tr").length;

            var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
            var voucherNo = $("#ContentPlaceHolder1_txtVoucherNumber").val();
            var fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();
            var voucherType = $("#ContentPlaceHolder1_ddlVoucherType").val();
            var voucherStatus = $("#ContentPlaceHolder1_ddlVoucherTypeStatus").val();
            var referenceNo = $("#ContentPlaceHolder1_txtReferenceNumber").val();
            var referenceVoucherNo = $("#ContentPlaceHolder1_txtReferenceVoucherNumber").val();
            var narration = $("#ContentPlaceHolder1_txtSrcNarration").val();

            $("#ContentPlaceHolder1_hfPageNumber").val(pageNumber);

            if (companyId == "0") {
                companyId = 0;
                projectId = 0;
            }

            PageMethods.GetVoucherBySearchCriteria(companyId, projectId, voucherType, voucherStatus, voucherNo, fromDate, toDate, referenceNo, referenceVoucherNo, narration, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {

            $("#VoucherSearchGrid tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"7\" >No Data Found</td> </tr>";
                $("#VoucherSearchGrid tbody ").append(emptyTr);
                CommonHelper.SpinnerClose();
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                tr += "<tr>";

                if (gridObject.GLStatus == 'Submit' || gridObject.GLStatus == 'Pending') {
                    tr += "<td style=\"text-align:center; width:5%;\">  <input type='checkbox' value='" + gridObject.LedgerMasterId + "' /> </td>";
                }
                else {
                    tr += "<td style=\"width:5%;\"></td>";
                }

                //tr += "<td style=\"width:15%;\">" + new Date(gridObject.VoucherDate).format(innBoarDateFormat) + "</td>";
                tr += "<td style=\"width:10%;\">" + gridObject.VoucherDateString + "</td>";
                tr += "<td style=\"width:12%;\">" + gridObject.VoucherNo + "</td>";
                tr += "<td style=\"width:40%;\">" + gridObject.Narration + "</td>";
                tr += "<td style=\"width:15%;\">" + gridObject.VoucherTotalAmount + "</td>";
                tr += "<td style=\"width:10%;\">" + gridObject.GLStatus + "</td>";
                tr += "<td style=\"text-align: center; width:8%; cursor:pointer;\">";
                if (gridObject.IsCanCheck) {
                    tr += "<img src='../Images/checked.png' onClick= \"javascript:return ApprovedIndividualVoucher('" + gridObject.LedgerMasterId + "', this)\" alt='Check'  title='Check' border='0' />";
                }
                else if (gridObject.IsCanApprove) {
                    tr += "<img src='../Images/approved.png' onClick= \"javascript:return ApprovedIndividualVoucher('" + gridObject.LedgerMasterId + "', this)\" alt='Approved'  title='Approved' border='0' />";
                }
                tr += "&nbsp;&nbsp;<img src='../Images/detailsInfo.png'  onClick= \"javascript:return VoucherDetails('" + gridObject.LedgerMasterId + "')\" alt='Details' title='Details' border='0' />";
                tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return VoucherPreviewOption('" + gridObject.LedgerMasterId + "')\" alt='Voucher' title='Voucher' border='0' />";
                tr += '&nbsp;&nbsp;<a href="javascript:void();" onclick= "javascript:return ShowDocuments(' + gridObject.LedgerMasterId + ');" title="Documents"><img style="width:16px;height:16px;" alt="Documents" src="../Images/document.png" /></a>';
                tr += "</td>";



                tr += "<td align='right' style=\"display:none;\">" + gridObject.LedgerMasterId + "</td>";
                tr += "<td align='right' style=\"display:none;\">" + gridObject.CreatedBy + "</td>";

                tr += "</tr>"

                $("#VoucherSearchGrid tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnLoadObjectFailed(error) {
            toastr.warning("Error");
            CommonHelper.SpinnerClose();
        }

        function ShowDocuments(id) {
            console.log("Working");
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
                title: "Voucher Documents",
                show: 'slide'
            });

            return false;
        }

        function OnLoadDocumentByIdFailed(error) {
            toastr.error(error.get_message());
        }


        function VoucherDetails(ledgerMasterId) {
            CommonHelper.SpinnerOpen();
            PageMethods.GetVoucherDetailsForDisplay(ledgerMasterId, OnSucceedDetailsLoadSave, OnFailedDetailsLoadSave);
        }
        function OnSucceedDetailsLoadSave(result) {

            $("#VoucherDetailsDisplayGrid tbody").html("");

            $("#lblCompany").text(result.LedgerMaster.CompanyName);
            $("#lblProject").text(result.LedgerMaster.ProjectName);
            $("#lblDonor").text(result.LedgerMaster.DonorName);
            $("#lblPayerOrPayee").text(result.LedgerMaster.PayerOrPayee);
            $("#lblReferenceNumber").text(result.LedgerMaster.ReferenceNumber);
            $("#lblVoucherDate").text(GetStringFromDateTime(result.LedgerMaster.VoucherDate));
            $("#lblVoucherType").text(result.LedgerMaster.VoucherTypeName);
            $("#lblCurrencyType").text(result.LedgerMaster.CurrencyName);
            $("#lblConvertionRate").text(result.LedgerMaster.ConvertionRate);

            var tr = "";

            $.each(result.LedgerMasterDetails, function (index, obj) {
                tr += "<tr>";
                tr += "<td style='width:25%;'>" + obj.NodeHead + "</td>";
                tr += "<td class='text-right' style='width:10%;'>" + obj.DRAmount + "</td>";
                tr += "<td class='text-right' style='width:10%;'>" + obj.CRAmount + "</td>";
                tr += "<td style='width:15%;'>" + (obj.ChequeNumber == null ? '' : obj.ChequeNumber) + "</td>";
                tr += "<td style='width:40%;'>" + obj.NodeNarration + "</td>";
                tr += "</tr>";
            });

            $("#TotalDR").text(result.TotalDrAmount);
            $("#TotalCR").text(result.TotalCrAmount);

            $("#VoucherDetailsDisplayGrid tbody").append(tr);

            $("#VoucherDetailsGrid").dialog({
                autoOpen: true,
                modal: true,
                width: '1000px',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Voucher Details",
                show: 'slide'
            });

            CommonHelper.SpinnerClose();
        }
        function OnFailedDetailsLoadSave(result) {
            toastr.warning("Data Error.");
            CommonHelper.SpinnerClose();
        }

        function CheckAllVoucher() {
            if ($("#voucherCheck").is(":checked")) {
                $("#VoucherSearchGrid tbody tr").find("td:eq(0)").find("input").prop("checked", true);
            }
            else {
                $("#VoucherSearchGrid tbody tr").find("td:eq(0)").find("input").prop("checked", false);
            }
        }

        function ApprovedIndividualVoucher(ledgerMasterId, selectedVoucher) {

            var VocuherApproval = new Array();
            var tr = $(selectedVoucher).parent().parent();

            VocuherApproval.push({
                LedgerMasterId: ledgerMasterId,
                GLStatus: $(tr).find("td:eq(5)").text(),
                CreatedBy: $(tr).find("td:eq(8)").text()
            });

            PageMethods.VoucherApproval(VocuherApproval, OnSucceedVoucherApproval, OnSucceedVoucherApprovalFailed);
            return false;
        }

        function ApprovedVoucher() {

            if ($("#VoucherSearchGrid tbody tr").find("td:eq(0)").find("input:checked").length == 0) {
                toastr.info("Please Select Voucher For Approved.");
                return false;
            }

            var VocuherApproval = new Array();

            $("#VoucherSearchGrid tbody tr").each(function () {

                if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {

                    VocuherApproval.push({
                        LedgerMasterId: $(this).find("td:eq(0)").find("input").val(),
                        GLStatus: $(this).find("td:eq(5)").text(),
                        CreatedBy: $(this).find("td:eq(8)").text()
                    });
                }

            });

            PageMethods.VoucherApproval(VocuherApproval, OnSucceedVoucherApproval, OnSucceedVoucherApprovalFailed);
            return false;
        }
        function OnSucceedVoucherApproval(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                if ($("#ContentPlaceHolder1_hfPageNumber").val() == "") {
                    GridPaging($("#GridPagingContainer").find("li.active").index(), 1);
                }
                else {
                    var pageNumber = $("#ContentPlaceHolder1_hfPageNumber").val();
                    GridPaging(pageNumber, 1);
                }

            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnSucceedVoucherApprovalFailed(result) {
            toastr.error("Approval Failed. Try Again.");
        }

        function VoucherPreviewOption(ledgerMasterId) {
            var iframeid = 'printDoc';
            var url = "Reports/frmReportVoucherInfo.aspx?DealId=" + ledgerMasterId;
            parent.document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 730,
                minHeight: 555,
                width: 'auto',
                closeOnEscape: false,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Voucher Preview",
                show: 'slide'
                //,close: ClosePrintDialog
            });

            setTimeout(function () { ScrollToBottom(); }, 1000);
        }

        function AdminApprovalProcess() {
            var r = confirm("Do you want to continue Voucher Unapproval?");
            if (r == true) {
                var transactionNo = $("#<%=txtApprovalTransactionNo.ClientID%>").val();
                var companyIdUC = $("#ContentPlaceHolder1_CompanyProjectUserControlVoucherApproval_ddlGLCompany").val();
                var projectIdUC = $("#ContentPlaceHolder1_CompanyProjectUserControlVoucherApproval_ddlGLProject").val();
                $("#ContentPlaceHolder1_CompanyProjectUserControlVoucherApproval_ddlGLProject").change(function () {
                    projectIdUC = $("#ContentPlaceHolder1_CompanyProjectUserControlVoucherApproval_ddlGLProject").val();
                });
                console.log(companyIdUC);
                console.log(projectIdUC);
                var status = "Pending";


                if (companyIdUC == 0) {
                    toastr.warning("Please Select a Company");
                    $("#ContentPlaceHolder1_CompanyProjectUserControlVoucherApproval_ddlGLCompany").focus();
                    return false;
                }

                if (projectIdUC == 0) {
                    toastr.warning("Please Select a Project");
                    $("#ContentPlaceHolder1_CompanyProjectUserControlVoucherApproval_ddlGLProject").focus();
                    return false;
                }

                if (transactionNo == '') {
                    toastr.warning("Please Enter Voucher Number.");
                    $("#<%=txtApprovalTransactionNo.ClientID%>").focus();
                    return false;
                }

                PageMethods.AdminApprovalStatus(companyIdUC, projectIdUC, transactionNo, status, OnAdminApprovalProcessSucceed, OnAdminApprovalProcessFailed);
            }

            return false;
        }

        function OnAdminApprovalProcessSucceed(result) {
            toastr.success("Voucher Unapprove Successfull.");
            return false;
        }

        function OnAdminApprovalProcessFailed(error) {
            toastr.error(error.get_message());
            return false;
        }

        function VoucherUnapprovalPanel(result) {
            $("#<%=txtApprovalTransactionNo.ClientID%>").val("");

            $("#AdminApprovalDiv").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Voucher Unapproval Information",
                show: 'slide'
            });

            return false;
        }

        function CloseDialogVoucherUnapprovalPanel() {
            $("#AdminApprovalDiv").dialog('close');
            return false;
        }
    </script>
    <asp:HiddenField ID="hfPageNumber" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsSingle" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfProjectId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <div id="voucherDocuments" style="display: none;">
        <div id="imageDiv"></div>
    </div>
    <div id="displayBill" style="display: none;">
        <iframe id="printDoc" name="printDoc" width="730" height="650" frameborder="0" style="overflow: hidden;"></iframe>
        <div id="bottomPrint">
        </div>
    </div>
    <div id="AdminApprovalDiv" class="panel panel-default" style="display: none;">
        <div class="panel-body">
            <div class="form-horizontal">
                <div id="dvSearchAdminApprovalStatus" class="form-group">
                    <div class="col-md-12">
                        <UserControl:CompanyProjectUserControlVoucherApproval ID="CompanyProjectUserControlVoucherApproval" runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Voucher Number</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtApprovalTransactionNo" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="Button2" runat="server" TabIndex="3" Text="Unapprove" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return AdminApprovalProcess();" />
                    <asp:Button ID="Button3" runat="server" TabIndex="4" Text="Close" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return CloseDialogVoucherUnapprovalPanel();" />
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Voucher Search
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />
                <div class="form-group">
                    <label for="" class="control-label col-md-2">
                        Voucher Date</label>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtFromDate" CssClass="form-control" placeholder="From Date" runat="server" TabIndex="1"></asp:TextBox>
                    </div>

                    <div class="col-md-2">
                        <asp:TextBox ID="txtToDate" CssClass="form-control" placeholder="To Date" runat="server"></asp:TextBox>
                    </div>
                    <label for="" class="control-label col-md-2">
                        Voucher Status</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlVoucherTypeStatus" runat="server" CssClass="form-control" TabIndex="5">
                            <asp:ListItem Value="">--- All Status ---</asp:ListItem>
                            <asp:ListItem Value="Pending">Pending </asp:ListItem>
                            <asp:ListItem Value="Checked">Checked</asp:ListItem>
                            <asp:ListItem Value="Approved">Approved</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-md-2">
                        Voucher Number</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtVoucherNumber" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                    </div>
                    <label for="" class="control-label col-md-2">
                        Voucher Type</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlVoucherType" runat="server" CssClass="form-control" TabIndex="5">
                            <asp:ListItem Value="">--- All Type Voucher ---</asp:ListItem>
                            <asp:ListItem Value="CP">Cash Payment (CP) </asp:ListItem>
                            <asp:ListItem Value="BP">Bank Payment (BP)</asp:ListItem>
                            <asp:ListItem Value="CR">Cash Receive (CR)</asp:ListItem>
                            <asp:ListItem Value="BR">Bank Receive (BR)</asp:ListItem>
                            <asp:ListItem Value="JV">Journal Voucher (JV)</asp:ListItem>
                            <asp:ListItem Value="CV">Contra Voucher (CV)</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-md-2">
                        Reference Number</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtReferenceNumber" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                    </div>
                    <label class="control-label col-md-2">
                        Reference Voucher</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtReferenceVoucherNumber" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-md-2">
                        Narration</label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtSrcNarration" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <input type="button" value="Search Voucher" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="GridPaging(1, 1)" tabindex="4" />
                    <asp:Button ID="btnAdminApproval" runat="server" TabIndex="4" Text="Voucher Unapproval" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return VoucherUnapprovalPanel();" />
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-12">
                    <table id="VoucherSearchGrid" class="table table-hover table-bordered table-condensed table-responsive">
                        <thead>
                            <tr>
                                <td style="text-align: center; width: 5%;">
                                    <%--<input type="checkbox" value="" id="voucherCheck" onclick="CheckAllVoucher()" />--%>
                                </td>
                                <td style="width: 10%;">Voucher Date
                                </td>
                                <td style="width: 12%;">Voucher No
                                </td>
                                <td style="width: 40%;">Narration
                                </td>
                                <td style="width: 15%;">Voucher Total
                                </td>
                                <td style="width: 10%;">Status
                                </td>
                                <td style="width: 10%;">Action
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <div class="text-center" id="GridPagingContainer">
                        <ul class="pagination">
                        </ul>
                    </div>
                </div>
            </div>
            <div class="row" style="display: none;">
                <div class="col-md-12">
                    <input type="button" id="btnApprovedVoucher" class="TransactionalButton btn btn-primary btn-sm"
                        value="Approved Voucher" onclick="ApprovedVoucher()" tabindex="5" />
                </div>
            </div>
        </div>
    </div>
    <div id="VoucherDetailsGrid" style="display: none;">
        <div class="form-horizontal">
            <fieldset>
                <legend>Company/Project</legend>
                <div class="form-group">
                    <label for="" class="control-label col-md-2">
                        Company</label>
                    <div class="col-md-4">
                        <label id="lblCompany" class="form-control">
                        </label>
                    </div>
                    <label for="" class="control-label col-md-2">
                        Project</label>
                    <div class="col-md-4">
                        <label id="lblProject" class="form-control">
                        </label>
                    </div>
                </div>
                <div class="form-group">
                    <label for="" class="control-label col-md-2">
                        Donor</label>
                    <div class="col-md-4">
                        <label id="lblDonor" class="form-control">
                        </label>
                    </div>
                </div>
                <div class="form-group">
                    <label for="" class="control-label col-md-2">
                        Payer Or Payee</label>
                    <div class="col-md-10">
                        <label id="lblPayerOrPayee" class="form-control">
                        </label>
                    </div>
                </div>
                <div class="form-group">
                    <label for="" class="control-label col-md-2">
                        Reference Number</label>
                    <div class="col-md-4">
                        <label id="lblReferenceNumber" class="form-control">
                        </label>
                    </div>
                </div>
            </fieldset>
            <fieldset>
                <legend>Voucher Info</legend>
                <div class="form-group">
                    <label class="control-label col-md-2">
                        Voucher Date</label>
                    <div class="col-md-4">
                        <label id="lblVoucherDate" class="form-control">
                        </label>
                    </div>
                    <label for="" class="control-label col-md-2">
                        Voucher Type</label>
                    <div class="col-md-4">
                        <label id="lblVoucherType" class="form-control">
                        </label>
                    </div>
                </div>
                <div class="form-group">
                    <label for="" class="control-label col-md-2">
                        Currency Type</label>
                    <div class="col-md-4">
                        <div class="row">
                            <div class="col-md-6">
                                <label id="lblCurrencyType" class="form-control">
                                </label>
                            </div>
                            <div class="col-md-6 col-padding-left-none">
                                <div class="input-group">
                                    <span class="input-group-addon">C.Rate</span>
                                    <label id="lblConvertionRate" class="form-control">
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
            <table id="VoucherDetailsDisplayGrid" class="table table-bordered table-hover table-condensed table-responsive">
                <thead>
                    <tr>
                        <td style="width: 25%;">
                            <label for="" class="control-label">
                                Account Name</label>
                        </td>
                        <td style="width: 10%;">
                            <label for="" class="control-label">
                                Dr. Amount</label>
                        </td>
                        <td style="width: 10%;">
                            <label for="" class="control-label">
                                Cr. Amount</label>
                        </td>
                        <td style="width: 15%;">
                            <label for="" class="control-label">
                                Cheque Number</label>
                        </td>
                        <td style="width: 40%;">
                            <label for="" class="control-label">
                                Narration</label>
                        </td>
                    </tr>
                </thead>
                <tbody>
                </tbody>
                <tfoot>
                    <tr>
                        <th style="width: 25%; text-align: right;">
                            <label class="control-label text-right">
                                Total
                            </label>
                        </th>
                        <th style="width: 10%; text-align: right;">
                            <label class="control-label text-right" id="TotalDR">
                            </label>
                        </th>
                        <th style="width: 10%; text-align: right;">
                            <label class="control-label text-right" id="TotalCR">
                            </label>
                        </th>
                        <th style="width: 15%;"></th>
                        <th style="width: 40%;"></th>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {

            var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val();
            debugger;
            if (companyId == "0") {

                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val($("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val()).trigger("change");

            }
        });

    </script>
</asp:Content>
