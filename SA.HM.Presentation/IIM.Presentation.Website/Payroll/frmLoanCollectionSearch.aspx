<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmLoanCollectionSearch.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmLoanCollectionSearch" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="EmployeeForLoanSearch" Src="~/HMCommon/UserControl/EmployeeSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Loan Collection Search</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#ContentPlaceHolder1_btnSearch").click(function () {
                GridPaging(1, 1);
                return false;
            });

        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var employeeId = "", loanType = "";

            employeeId = $("#ContentPlaceHolder1_employeeForLoanSearch_hfEmployeeId").val();
            loanType = $("#ContentPlaceHolder1_ddlSLoanType").val();

            if (employeeId == "0")
                employeeId = "";

            var gridRecordsCount = $("#tblLoanCollectionSearch tbody tr").length;

            PageMethods.LoadEmployeeLoanCollectionInfo(employeeId, loanType, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoanLoadSucceed, OnLoanLoadFailed);
            return false;
        }

        function ReloadGrid(IsCurrentOrPreviousPage) {
            var currentPageNumber = $("#GridPagingContainer ul li[class='active']").text();

            if (currentPageNumber == "")
                currentPageNumber = 1;

            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }

        function OnLoanLoadSucceed(result) {

            $("#tblLoanCollectionSearch tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"7\" >No Data Found</td> </tr>";
                $("#tblLoanCollectionSearch tbody").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "", holdupLink = "",
                approvalLink = "", collectLink = "", takenPeriodDisplay = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#tblLoanCollectionSearch tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                if (gridObject.LoanTakenForPeriod > 1) {
                    takenPeriodDisplay = gridObject.LoanTakenForPeriod + " " + gridObject.LoanTakenForMonthOrYear + "s";
                }
                else {
                    takenPeriodDisplay = gridObject.LoanTakenForPeriod + " " + gridObject.LoanTakenForMonthOrYear
                }

                tr += "<td align='left' style=\"width:15%;\">" + gridObject.EmployeeName + "</td>";
                tr += "<td align='left' style=\"width:11%;\">" + gridObject.LoanType + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + takenPeriodDisplay + "</td>";
                tr += "<td align='left' style=\"width:11%;\">" + gridObject.LoanAmount + "</td>";
                tr += "<td align='left' style=\"width:11%;\">" + gridObject.DueAmount + "</td>";
                tr += "<td align='left' style=\"width:11%;\">" + (gridObject.CollectionDate == null ? '' : GetStringFromDateTime(gridObject.CollectionDate)) + "</td>";
                tr += "<td align='left' style=\"width:11%;\">" + (gridObject.InstallmentNumber == null ? '' : gridObject.InstallmentNumber) + "</td>";
                tr += "<td align='left' style=\"width:11%;\">" + (gridObject.CollectionAmount == null ? '' : gridObject.CollectionAmount) + "</td>";

                if (gridObject.ApprovedStatus != "Approved") {
                    editLink = "<a href=\"javascript:void(0);\"  title=\"Edit\" onclick=\"javascript:return PerformFillFormAction(" + gridObject.CollectionId + "," + gridObject.LoanId + "," + gridObject.EmpId + ", " + result.GridPageLinks.CurrentPageNumber + ");\"> <img alt=\"Edit\" src=\"../Images/edit.png\" /> </a>";
                    deleteLink = "&nbsp;<a href=\"javascript:void(0);\"  title=\"Delete\" onclick=\"javascript:return PerformDeleteAction(" + gridObject.CollectionId + "," + gridObject.LoanId + "," + gridObject.EmpId + ", " + result.GridPageLinks.CurrentPageNumber + ");\"><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
                    approvalLink = "&nbsp;<a href=\"javascript:void(0);\"  title=\"Approved\" onclick=\"javascript:return LoanCollectionApproved(" + gridObject.CollectionId + "," + gridObject.LoanId + ",'" + gridObject.ApprovedStatus + "'," + result.GridPageLinks.CurrentPageNumber + ");\"> <img alt=\"Approval\" src=\"../Images/approved.png\" /> </a>";
                }

                tr += "<td align='center' style=\"width:20%;\">" + editLink + deleteLink + approvalLink + "</td>";

                tr += "</tr>"

                $("#tblLoanCollectionSearch tbody").append(tr);
                tr = "";

            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
        }
        function OnLoanLoadFailed(error) {

        }

        function PerformClearAction() {

            $("#frmHotelManagement")[0].reset();
            $("#<%=hfLoanId.ClientID %>").val("");
            $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val("0");
            $("#ContentPlaceHolder1_employeeSearch_hfEmployeeName").val("");
            $("#ContentPlaceHolder1_approvedByEmployee_hfEmployeeId").val("0");
            $("#ContentPlaceHolder1_approvedByEmployee_hfEmployeeName").val("");
            $("#ContentPlaceHolder1_hfIsCurrentOrPreviousPage").val("0");

            return false;
        }

        function PerformFillFormAction(collectionId, loanId, empId, currentPageNumber) {
            $("#<%=hfLoanCollectionId.ClientID %>").val(collectionId);
            $("#<%=hfLoanId.ClientID %>").val(loanId);
            $("#<%=hfEmployeeId.ClientID %>").val(empId);

            PageMethods.GetLastLoanCollectionInfo(loanId, OnLoadLoanCollectionSucceed, OnLoadLoanCollectionFailed);

            $("#collectionDialog").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Edit Loan Collection",
                show: 'slide'
            });

            return false;
        }

        function OnLoadLoanCollectionSucceed(result) {

            $("#ContentPlaceHolder1_txtInstallmentNumber").val(result.InstallmentNumber);
            $("#ContentPlaceHolder1_txtPerInstallLoanAmount").val(result.CollectedLoanAmount);
            $("#ContentPlaceHolder1_txtPerInstallInterestAmount").val(result.CollectedInterestAmount);
            $("#ContentPlaceHolder1_txtCollectAmount").val(result.CollectedLoanAmount + result.CollectedInterestAmount);
            $("#ContentPlaceHolder1_txtLoanAmount").val(result.LoanAmount);
            $("#ContentPlaceHolder1_txtDueAmount").val(result.DueAmount);
            $("#ContentPlaceHolder1_txtOverDueAmount").val(result.OverdueAmount);
            $("#ContentPlaceHolder1_txtCollectDate").val(GetStringFromDateTime(result.CollectionDate));
        }
        function OnLoadLoanCollectionFailed(error)
        { }

        var vc = [];
        function SaveLoanCollection() {

            var collectionId = $("#<%=hfLoanCollectionId.ClientID %>").val();
            var loanId = $("#<%=hfLoanId.ClientID %>").val();
            var empId = $("#<%=hfEmployeeId.ClientID %>").val();

            var installmentNumber = $("#<%=txtInstallmentNumber.ClientID %>").val();
            var collectionDate = $("#<%=txtCollectDate.ClientID %>").val();
            var collectedLoanAmount = $("#<%=txtPerInstallLoanAmount.ClientID %>").val();
            var collectedinterestAmount = $("#<%=txtPerInstallInterestAmount.ClientID %>").val();

            var loanCollection = {
                CollectionId: collectionId,
                LoanId: loanId,
                EmpId: empId,
                InstallmentNumber: installmentNumber,
                CollectionDate: collectionDate,
                CollectedLoanAmount: collectedLoanAmount,
                CollectedInterestAmount: collectedinterestAmount
            };

            PageMethods.UpdateLoanCollection(loanCollection, OnUpdateLoanCollectionSucceed, OnUpdateLoanCollectionFailed);

            return false;
        }

        function OnUpdateLoanCollectionSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $("#collectionDialog").dialog("close");
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnUpdateLoanCollectionFailed(error) { }

        function LoanCollectionApproved(collectionId, loanId, approvedStatus, currentPageNumber) {
            $("#ContentPlaceHolder1_hfIsCurrentOrPreviousPage").val(currentPageNumber);
            PageMethods.ApprovedLoanCollection(collectionId, loanId, approvedStatus, OnApprovedLoanCollectionSucceed, OnApprovedLoanCollectionFailed);
        }
        function OnApprovedLoanCollectionSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                ReloadGrid($("#ContentPlaceHolder1_hfIsCurrentOrPreviousPage").val());
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnApprovedLoanCollectionFailed(error) { }

        function PerformDeleteAction(collectionId, loanId, empId, currentPageNumber) {
            $("#ContentPlaceHolder1_hfIsCurrentOrPreviousPage").val(currentPageNumber);
            PageMethods.DeleteLoanCollection(collectionId, loanId, OnDeleteLoanCollectionSucceed, OnDeleteLoanCollectionFailed);
        }
        function OnDeleteLoanCollectionSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                ReloadGrid($("#ContentPlaceHolder1_hfIsCurrentOrPreviousPage").val());
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnDeleteLoanCollectionFailed(error) { }

        function CloseDialog() {
            $("#collectionDialog").dialog("close");
            return false;
        }

    </script>
    <asp:HiddenField ID="hfLoanCollectionId" runat="server" />
    <asp:HiddenField ID="hfLoanId" runat="server" Value="" />
    <asp:HiddenField ID="hfEmployeeId" runat="server" Value="" />
    <asp:HiddenField ID="hfIsCurrentOrPreviousPage" runat="server" Value="" />
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="collectionDialog" style="display: none;">
        <div id="collectionContent">
        <div class="panel-body">
        <div class="form-horizontal"> 
            <fieldset>
                <legend>Loan Collection Info</legend>
                <div class="form-group">
                    <div class="col-md-2" style="width: 160px;">
                        <asp:Label ID="lblInstallmentNumber" runat="server" Text="Installment Number"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="txtInstallmentNumber" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" Text="Loan Amount"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="txtLoanAmount" ReadOnly="true" runat="server"></asp:TextBox>
                    </div>
                </div>                
                <div class="form-group">
                    <div class="col-md-2" style="width: 160px;">
                        <asp:Label ID="lblPerInstallAmount" runat="server" Text="Installment Loan Amount"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="txtPerInstallLoanAmount" ReadOnly="true" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblDueAmount" runat="server" Text="Due Amount"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="txtDueAmount" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>               
                <div class="form-group">
                    <div class="col-md-2" style="width: 160px;">
                        <asp:Label ID="Label2" runat="server" Text="Installment Interest Amount"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="txtPerInstallInterestAmount" ReadOnly="true" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblOverDueAmount" runat="server" Text="Over Due Amount"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="txtOverDueAmount" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>                
                <div class="form-group">
                    <div class="col-md-2" style="width: 160px;">
                        <asp:Label ID="lblCollectAmount" runat="server" Text="Total Collection Amount"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="txtCollectAmount" runat="server" TabIndex="3"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblCollectDate" runat="server" Text="Collection Date"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="txtCollectDate" runat="server" CssClass="datepicker" TabIndex="2"></asp:TextBox>
                    </div>
                </div>               
                <div class="row">
 <div class="col-md-10">
                    <asp:Button ID="btnSave" runat="server" OnClientClick="javascript:return SaveLoanCollection()"
                        Text="Save" CssClass="TransactionalButton btn btn-primary" />
                    <asp:Button ID="btnCancel" runat="server" Text="Close" OnClientClick="javascript:return CloseDialog()"
                        CssClass="TransactionalButton btn btn-primary" />
                </div>  
                </div>             
            </fieldset>
            </div>
            </div>
        </div>
    </div>
    <div>
        <div class="panel panel-default">           
            <div class="panel-heading">Loan Search</div>
            <div class="panel-body">
        <div class="form-horizontal">               
                <UserControl:EmployeeForLoanSearch ID="employeeForLoanSearch" runat="server" />               
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSLoanType" runat="server" class="control-label" Text="Loan Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSLoanType" runat="server" CssClass="form-control">
                            <asp:ListItem Text="---Please Select---" Value=""></asp:ListItem>
                            <asp:ListItem Text="Company Loan" Value="CompanyLoan"></asp:ListItem>
                            <asp:ListItem Text="PF Loan" Value="PFLoan"> </asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblSLoanStatus" runat="server" class="control-label" Text="Loan Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSLoanStatus" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Regular" Value="REGULAR"></asp:ListItem>
                            <asp:ListItem Text="Holdup Loan" Value="HOLDUP"> </asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>               
                <div class="row">
 <div class="col-md-12">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary" />
                </div>  
                </div>              
            </div>
            </div>
        </div>       
        <div id="SearchPanel" class="panel panel-default">           
            <div class="panel-heading">Search Information</div>
            <div class="panel-body">
                <table id='tblLoanCollectionSearch' class="table table-bordered table-condensed table-responsive" width="100%">
                    <colgroup>
                        <col style="width: 15%;" />
                        <col style="width: 11%;" />
                        <col style="width: 11%;" />
                        <col style="width: 11%;" />
                        <col style="width: 11%;" />
                        <col style="width: 11%;" />
                        <col style="width: 11%;" />
                        <col style="width: 11%;" />
                        <col style="width: 8%;" />
                    </colgroup>
                    <thead>
                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                            <td>Employee
                            </td>
                            <td>Loan Type
                            </td>
                            <td>Loan Period
                            </td>
                            <td>Loan Amount
                            </td>
                            <td>Due Amount
                            </td>
                            <td>Collection Date
                            </td>
                            <td>Installment Number
                            </td>
                            <td>Collection Amount
                            </td>
                            <td>Action
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
    </div>
    <script type="text/javascript">
        var x = '<%=isMessageBoxEnable%>';
        if (x > -1) {
            MessagePanelShow();
            if (x == 2) {
                $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
            }
        }
        else {
            MessagePanelHide();
        }

    </script>
</asp:Content>
