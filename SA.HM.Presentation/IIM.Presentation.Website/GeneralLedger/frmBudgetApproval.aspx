<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmBudgetApproval.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.frmBudgetApproval"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Accounts</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Search Budget</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

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
        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            CommonHelper.SpinnerOpen();
            var gridRecordsCount = $("#BudgetSearchGrid tbody tr").length;
            var glCompanyId = $("#ContentPlaceHolder1_ddlCompany").val();
            var glProjectId = $("#ContentPlaceHolder1_ddlProject").val();
            var fiscalYearId = $("#ContentPlaceHolder1_ddlFiscalYear").val();
            var approvedStatus = $("#ContentPlaceHolder1_ddlApprovedStatus").val();

            PageMethods.GetBudgetBySearchCriteria(glCompanyId, glProjectId, fiscalYearId, approvedStatus, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {

            $("#BudgetSearchGrid tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"6\" >No Data Found</td> </tr>";
                $("#BudgetSearchGrid tbody ").append(emptyTr);
                CommonHelper.SpinnerClose();
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style=\"width:15%;\">" + gridObject.FiscalYearName + "</td>";
                tr += "<td style=\"width:17%;\">" + gridObject.ApprovedStatus + "</td>";

                if (gridObject.ApprovedStatus == 'Submit' || gridObject.ApprovedStatus == 'Pending') {
                    tr += "<td style=\"text-align: center; width:8%; cursor:pointer;\"><img src='../Images/approved.png' onClick= \"javascript:return ApprovedBudget('" + gridObject.BudgetId + "', this)\" alt='Approved'  title='Approved' border='0' />";
                    tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return BudgetPreviewOption(" + gridObject.BudgetId + ",'" + gridObject.ApprovedStatus + "')\" alt='Voucher' title='Voucher' border='0' />";
                    tr += "</td>";
                }
                else {
                    tr += "<td style=\"text-align: center; width:8%; cursor:pointer;\">";
                    tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return BudgetPreviewOption(" + gridObject.BudgetId + ",'" + gridObject.ApprovedStatus + "')\" alt='Voucher' title='Voucher' border='0' />";
                    tr += "</td>";
                }

                tr += "<td align='right' style=\"display:none;\">" + gridObject.BudgetId + "</td>";
                tr += "<td align='right' style=\"display:none;\">" + gridObject.CreatedBy + "</td>";

                tr += "</tr>"

                $("#BudgetSearchGrid tbody ").append(tr);
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
                $("#BudgetSearchGrid tbody tr").find("td:eq(0)").find("input").prop("checked", true);
            }
            else {
                $("#BudgetSearchGrid tbody tr").find("td:eq(0)").find("input").prop("checked", false);
            }
        }

        function ApprovedBudget(budgetId, selectedBudget) {

            var BudgetApproval = new Array();
            var tr = $(selectedBudget).parent().parent();

            BudgetApproval = {
                BudgetId: budgetId,
                CreatedBy: $(tr).find("td:eq(4)").text()
            };

            PageMethods.ApprovalBudget(BudgetApproval, OnSucceedBudgetApproval, OnSucceedBudgetApprovalFailed);
            return false;
        }

        function OnSucceedBudgetApproval(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                GridPaging($("#GridPagingContainer").find("li.active").index(), 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnSucceedBudgetApprovalFailed(result) {
            toastr.error("Approval Failed. Try Again.");
        }

        function BudgetPreviewOption(budgetId, approvedStatus) {

            var fid = $("#ContentPlaceHolder1_ddlFiscalYear").val();
            var iframeid = 'printDoc';
            var url = "Reports/frmReportBudgetShow.aspx?bid=" + budgetId + "&fid=" + fid + "&as=" + approvedStatus;
            parent.document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 1050,
                height: 555,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Budget Preview",
                show: 'slide'
                //,close: ClosePrintDialog
            });
        }
        
        function PopulateProjects(control) {
            $("#SaveContent").hide();
            $("#balanceTable").html("");
            let companyId = $(control).val();

            $("#ContentPlaceHolder1_ddlProject").empty().append('<option selected="selected" value="0">Loading...</option>');

            $.ajax({
                type: "POST",
                url: "./frmBudget.aspx/GetGLProjectByGLCompanyId",
                data: JSON.stringify({ companyId: companyId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    PopulateControlWithValueNTextField(response.d, $("#ContentPlaceHolder1_ddlProject"), $("#<%=CommonDropDownHiddenField.ClientID %>").val(), "Name", "ProjectId");
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
        }
        function PopulateFiscalYear(control) {
            let projectId = $(control).val();
            $("#SaveContent").hide();
            $("#balanceTable").html("");
            $('#<%=ddlFiscalYear.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');

            $.ajax({
                type: "POST",
                url: "./frmBudget.aspx/PopulateFiscalYear",
                data: JSON.stringify({ projectId: projectId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    PopulateControlWithValueNTextField(result.d, $("#<%=ddlFiscalYear.ClientID %>"), $("#<%=CommonDropDownHiddenField.ClientID %>").val(), "FiscalYearName", "FiscalYearId");
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
        }
    </script>
     <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <div id="displayBill" style="display: none; background-color: #ffffff;">
        <iframe id="printDoc" name="printDoc" width="2000" height="650" frameborder="0" style="overflow: hidden; background-color: #fff;"></iframe>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Budget Search
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Company</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control" TabIndex="2" onchange="PopulateProjects(this)">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Project</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control" TabIndex="3" onchange="PopulateFiscalYear(this)">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFiscalYear" runat="server" class="control-label required-field"
                            Text="Fiscal Year"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control" TabIndex="2">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label required-field"
                            Text="Approved Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlApprovedStatus" runat="server" CssClass="form-control" TabIndex="2">
                            <asp:ListItem Text="---Please Select---" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                            <asp:ListItem Text="Submit" Value="Submit"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <input type="button" value="Search Budget" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="GridPaging(1, 1)" tabindex="4" />
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-12">
                    <table id="BudgetSearchGrid" class="table table-hover table-bordered table-condensed table-responsive">
                        <thead>
                            <tr>
                                <td style="width: 55%;">Fiscal Year
                                </td>
                                <td style="width: 30%;">Approved Status
                                </td>
                                <td style="width: 15%;">Action
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
            <div class="row">
                <div class="col-md-12">
                    <input type="button" id="btnApprovedVoucher" class="TransactionalButton btn btn-primary btn-sm"
                        value="Approved Budget" onclick="ApprovedVoucher()" tabindex="5" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
