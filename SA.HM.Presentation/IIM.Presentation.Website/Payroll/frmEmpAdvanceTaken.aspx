<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpAdvanceTaken.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpAdvanceTaken" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="EmployeeForLoanSearch" Src="~/HMCommon/UserControl/EmployeeSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        var vsc = [];
        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Advance Taken</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#SearchPanel").hide();
            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });

            $("#gvAdvanceTaken").delegate("td > img.AdvanceTakenDelete", "click", function () {
                var answer = confirm("Do you want to delete this record?")
                if (answer) {
                    var advanceId = $.trim($(this).parent().parent().find("td:eq(4)").text());
                    var params = JSON.stringify({ sEmpId: advanceId });

                    var $row = $(this).parent().parent();
                    $.ajax({
                        type: "POST",
                        url: "/Payroll/frmEmpAdvanceTaken.aspx/DeleteAdvanceTakenById",
                        data: params,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            CommonHelper.AlertMessage(data.d.AlertMessage);
                            $row.remove();
                            $("#myTabs").tabs('load', 1);
                        },
                        error: function (error) {
                        }
                    });
                }
            });

            $("#ContentPlaceHolder1_txtAdvanceDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
        });
        $(function () {
            $("#myTabs").tabs();
        });

        if ($(IsCanSave)) {
            $('#btnEmpAdvanceTakenSave').show();
        } else {
            $('#btnEmpAdvanceTakenSave').hide();
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#gvAdvanceTaken tbody tr").length;

            var employeeId = $("#ContentPlaceHolder1_employeeForLoanSearch_hfEmployeeId").val();

            if (employeeId == "0")
                employeeId = "";

            if (employeeId == "") {
                toastr.info("Please Provide Employee Information");
                return false;
            }
            else {
                PageMethods.SearchAdvTknAndLoadGridInformation(employeeId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
                return false;
            }
        }
        function OnLoadObjectSucceeded(result) {
            $("#gvAdvanceTaken tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvAdvanceTaken tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvAdvanceTaken tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                var date = GetStringFromDateTime(gridObject.AdvanceDate)

                tr += "<td align='left' style=\"width:50%; cursor:pointer;\">" + date + "</td>";
                tr += "<td align='left' style=\"width:35%; cursor:pointer;\">" + gridObject.AdvanceAmount + "</td>";

                editLink = "<a href=\"javascript:void();\"  title=\"Edit\" onclick=\"javascript:return PerformEditAction('" + gridObject.AdvanceId + "', '" + result.GridPageLinks.CurrentPageNumber + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" /> </a>";
                cancelLink = "&nbsp;<a href=\"javascript:void();\"  title=\"Cancel\" onclick=\"javascript:return CancelAdvanceTaken(" + gridObject.AdvanceId + "," + gridObject.EmpId + "," + result.GridPageLinks.CurrentPageNumber + ");\"> <img alt=\"Cancel\" src=\"../Images/cancel.png\" /> </a>";
                approvedLink = "&nbsp;<a href=\"javascript:void();\"  title=\"Approved\" onclick=\"javascript:return ApproveAdvanceTaken(" + gridObject.AdvanceId + "," + gridObject.EmpId + "," + result.GridPageLinks.CurrentPageNumber + ");\"> <img alt=\"Approval\" src=\"../Images/approved.png\" /> </a>";

                if ((gridObject.ApprovedStatus != "Approved") && (gridObject.ApprovedStatus != "Cancel")) {
                    tr += "<td align='center' style=\"width:15%;\">";
                    if (IsCanEdit) {
                        tr += editLink;
                    }
                    if (IsCanDelete) {
                        tr += cancelLink;
                    }
                    if (IsCanSave) {
                        tr += approvedLink;
                    }
                    tr += "</td>";
                }
                else if (gridObject.ApprovedStatus == "Approved" & IsCanSave) {
                    tr += "<td align='center' style=\"width:15%;\">" + 'Approved' + "</td>";
                }
                else if (IsCanEdit) {
                    tr += "<td align='center' style=\"width:15%;\">" + editLink + "</td>";
                }

                tr += "</tr>"

                $("#gvAdvanceTaken tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            return false;
        }
        function OnLoadObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function PerformEditAction(AdvanceId, currentPageNumber) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }
            $("#ContentPlaceHolder1_hfIsCurrentOrPreviousPage").val(currentPageNumber);
            PageMethods.FillForm(AdvanceId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
        }

        function OnFillFormObjectSucceeded(result) {
            if ($(IsCanEdit)) {
                $('#btnEmpAdvanceTakenSave').show();
            } else {
                $('#btnEmpAdvanceTakenSave').hide();
            }
            $("#<%=btnEmpAdvanceTakenSave.ClientID %>").val("Update");

            vsc = result;

            $("#<%=hfAdvanceTakenId.ClientID %>").val(result.AdvanceId);
            $("#ContentPlaceHolder1_employeeSearch_txtSearchEmployee").val(result.EmpCode);
            $("#ContentPlaceHolder1_employeeSearch_txtEmployeeName").val(result.EmpName);
            $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val(result.EmpId);
            $("#ContentPlaceHolder1_employeeSearch_hfEmployeeName").val(result.EmpName);
            $("#ContentPlaceHolder1_txtAdvanceDate").val(GetStringFromDateTime(result.AdvanceDate));
            $("#ContentPlaceHolder1_txtAdvanceAmount").val(result.AdvanceAmount);
            $("#ContentPlaceHolder1_ddlPayMonth").val(result.PayMonth);
            $("#ContentPlaceHolder1_chkIsDeductFromSalary").attr('checked', true);
            $("#ContentPlaceHolder1_txtRemarks").val(result.Remarks);
            $('#EntryPanel').show();
            $("#myTabs").tabs({ active: 0 });

            return false;
        }
        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function CheckValidation() {
            var advanceAmount = $("#ContentPlaceHolder1_txtAdvanceAmount").val();

            if (CommonHelper.IsDecimal(advanceAmount) == false) {
                toastr.info("Please Give Valid Amount");
                return false;
            }
        }

        function WorkAfterSearchEmployee() { }

        function CancelAdvanceTaken(advanceId, empId, currentPageNumber) {
            if (confirm("Do you want to cancel?")) {
                $("#ContentPlaceHolder1_hfIsCurrentOrPreviousPage").val(currentPageNumber);
                PageMethods.AdvanceTakenCancel(advanceId, empId, OnCancelAdvanceTakenSucceed, OnCancelAdvanceTakenFailed);
            }
        }
        function OnCancelAdvanceTakenSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                ReloadGrid($("#ContentPlaceHolder1_hfIsCurrentOrPreviousPage").val());
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            return false;
        }
        function OnCancelAdvanceTakenFailed() { }

        function ApproveAdvanceTaken(advanceId, empId, currentPageNumber) {
            if (!confirm("Do you want to approve?")) {
                return false;
            }
            $("#ContentPlaceHolder1_hfIsCurrentOrPreviousPage").val(currentPageNumber);
            PageMethods.AdvanceTakenApproval(advanceId, empId, OnApprovedAdvanceTakenSucceed, OnApprovedAdvanceTakenFailed);
        }
        function OnApprovedAdvanceTakenSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                ReloadGrid($("#ContentPlaceHolder1_hfIsCurrentOrPreviousPage").val());
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            return false;
        }
        function OnApprovedAdvanceTakenFailed() { }

        function ReloadGrid(IsCurrentOrPreviousPage) {
            var currentPageNumber = $("#GridPagingContainer ul li[class='active']").text();

            if (currentPageNumber == "")
                currentPageNumber = 1;

            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }

    </script>
    <asp:HiddenField ID="hfAdvanceTakenId" runat="server" Value="" />
    <asp:HiddenField ID="hfIsCurrentOrPreviousPage" runat="server" Value="" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Advance Taken Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Advance Taken</a></li>
        </ul>
        <div id="tab-1">
            <div id="TaxDeductionEntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Employee Advance Taken Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <UserControl:EmployeeSearch ID="employeeSearch" runat="server" />
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAdvanceDate" runat="server" class="control-label required-field" Text="Advance Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAdvanceDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblAdvanceAmount" runat="server" class="control-label required-field" Text="Advance Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtAdvanceAmount" CssClass="form-control" TabIndex="2">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPayMonth" runat="server" class="control-label required-field" Text="Pay Month"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPayMonth" runat="server" CssClass="form-control"
                                    TabIndex="3">
                                    <asp:ListItem Value="1">January</asp:ListItem>
                                    <asp:ListItem Value="2">February</asp:ListItem>
                                    <asp:ListItem Value="3">March</asp:ListItem>
                                    <asp:ListItem Value="4">April</asp:ListItem>
                                    <asp:ListItem Value="5">May</asp:ListItem>
                                    <asp:ListItem Value="6">June</asp:ListItem>
                                    <asp:ListItem Value="7">July</asp:ListItem>
                                    <asp:ListItem Value="8">August</asp:ListItem>
                                    <asp:ListItem Value="9">September</asp:ListItem>
                                    <asp:ListItem Value="10">October</asp:ListItem>
                                    <asp:ListItem Value="11">November</asp:ListItem>
                                    <asp:ListItem Value="12">December</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <asp:CheckBox ID="chkIsDeductFromSalary" Checked="true" Enabled="false" runat="server" Text="" CssClass="customChkBox"
                                    TabIndex="4" />
                                <asp:Label ID="lblIsDeductFromSalary" runat="server" class="control-label required-field" Text="Deduct From Salary"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="7"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnEmpAdvanceTakenSave" runat="server" Text="Save" TabIndex="8" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnEmpAdvanceTakenSave_Click" OnClientClick="javascript:return CheckValidation();" />
                                <asp:Button ID="btnEmpAdvanceTakenClear" OnClientClick="return confirm('Do you want to clear?');" runat="server" Text="Clear" TabIndex="9"
                                    CssClass="TransactionalButton btn btn-primary" OnClick="btnEmpAdvanceTakenClear_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Search Advance
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <UserControl:EmployeeForLoanSearch ID="employeeForLoanSearch" runat="server" />
                        <div class="row">
                            <div class="col-md-12">
                                <button type="button" id="btnSearch" class="TransactionalButton btn btn-primary">
                                    Search</button>
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
                    <table id='gvAdvanceTaken' class="table table-bordered table-condensed table-responsive"
                        width="100%">
                        <colgroup>
                            <col style="width: 50%;" />
                            <col style="width: 35%;" />
                            <col style="width: 15%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>Advance Date
                                </td>
                                <td>Advance Amount
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
        </div>
    </div>
</asp:Content>
