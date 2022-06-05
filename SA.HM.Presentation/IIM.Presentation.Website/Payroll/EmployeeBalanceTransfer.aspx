<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="EmployeeBalanceTransfer.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.EmployeeBalanceTransfer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var flag = 0;
        $(document).ready(function () {
            CommonHelper.ApplyDecimalValidation();
            $("#ContentPlaceHolder1_ddlTransferFrom").select2({
                tags: false,
                allowClear: true,
                width: "99.75%",
                dropdownParent: $("#CreateNewDialog")
            });
            $("#ContentPlaceHolder1_ddlTransferTo").select2({
                tags: false,
                allowClear: true,
                width: "99.75%",
                dropdownParent: $("#CreateNewDialog")
            });
            $("#ContentPlaceHolder1_ddlTransferFromSearch").select2({
                tags: false,
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlTransferToSearch").select2({
                tags: false,
                allowClear: true,
                width: "99.75%"
            });
            GridPaging(1, 1);
        });
        function CreateNew() {
            PerformClearAction();
            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '75%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Balance Transfer Information",
                show: 'slide'
            });
            $("#ContentPlaceHolder1_ddlTransferFrom").focus();
            return false;
        }
        function SaveAndClose() {
            flag = 1;
            SaveOrUpdateTransfer();
            return 0;
        }
        function SaveOrUpdateTransfer() {
            var id = $("#<%=hfId.ClientID%>").val();
            var transferFrom = $("#<%=ddlTransferFrom.ClientID%>").val();
            if (transferFrom == "0" || transferFrom == "") {
                toastr.warning("Select Transfer From");
                $("#<%=ddlTransferFrom.ClientID%>").focus();
                return false;
            }

            var transferTo = $("#<%=ddlTransferTo.ClientID%>").val();
            if (transferTo == "0" || transferTo == "") {
                toastr.warning("Select Transfer To");
                $("#<%=ddlTransferTo.ClientID%>").focus();
                return false;
            }

            var description = $("#<%=txtDescription.ClientID%>").val();
            var transferAmount = $("#<%=txtTransferAmount.ClientID%>").val();
            if (transferAmount == '') {
                toastr.warning("Enter Transfer Amount");
                $("#<%=txtTransferAmount.ClientID%>").focus();
                return false;
            }
            if (description == '') {
                toastr.warning("Enter Description");
                $("#<%=txtDescription.ClientID%>").focus();
                return false;
            }
            if (transferFrom ==  transferTo) {
                toastr.warning("Cann't Transfer to Same Person");
                $("#<%=ddlTransferFrom.ClientID%>").focus();
                $("#<%=ddlTransferTo.ClientID%>").focus();
                
                return false;
            }
            var PayrollEmployeeBalanceTransferBO = {
                Id: id,
                TransferFrom: transferFrom,
                TransferTo: transferTo,
                TransferAmount: transferAmount,
                Description: description
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Payroll/EmployeeBalanceTransfer.aspx/SaveUpdateBalanceTransferInformation',

                data: JSON.stringify({ PayrollEmployeeBalanceTransferBO: PayrollEmployeeBalanceTransferBO }),
                dataType: "json",
                success: function (data) {
                    GridPaging(1, 1);
                    PerformClearAction();
                    CommonHelper.AlertMessage(data.d.AlertMessage);
                    if (flag == 1) {
                        $('#CreateNewDialog').dialog('close');
                    }
                    flag = 1;
                },
                error: function (result) {
                    CommonHelper.AlertMessage(result.d.AlertMessage);
                }
            });
            return false;
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadGrid(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function LoadGrid(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#BalanceTransferTable tbody tr").length;
            var transferFrom = $("#<%=ddlTransferFromSearch.ClientID%>").val();
            var transferTo = $("#<%=ddlTransferToSearch.ClientID%>").val();

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Payroll/EmployeeBalanceTransfer.aspx/LoadBalanceTransferSearch',

                data: "{'TransferFrom':'" + transferFrom + "','TransferTo':'" + transferTo + "', 'gridRecordsCount':'" + gridRecordsCount + "', 'pageNumber':'" + pageNumber + "', 'isCurrentOrPreviousPage':'" + IsCurrentOrPreviousPage + "'}",
                dataType: "json",
                success: function (data) {
                    LoadTable(data);
                },
                error: function (result) {
                    CommonHelper.AlertMessage(result.d.AlertMessage);
                    PerformClearAction();
                }
            });
            return false;
        }
        function LoadTable(data) {

            $("#BalanceTransferTable tbody").empty();
            $("#GridPagingContainer ul").empty();
            i = 0;

            $.each(data.d.GridData, function (count, gridObject) {

                var tr = "";

                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:15%;'>" + gridObject.TransferFromEmp + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.TransferToEmp + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.TransferAmount + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.Status + "</td>";
                tr += "<td style='width:30%;'>" + gridObject.Description + "</td>";

                tr += "<td style=\"text-align: center; width:15%; cursor:pointer;\">";
                if (gridObject.IsCanEdit) {
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return FillFormEdit(" + gridObject.Id + ");\" alt='Edit'  title='Edit' border='0' />";
                }
                if (gridObject.IsCanDelete) {
                    tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'PerformDeleteAction(" + gridObject.Id + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                }
                if (gridObject.IsCanChecked) {
                    tr += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return PerformApproveActionWithConfirmation('" + 'Checked' + "'," + gridObject.Id + ")\" alt='Checked'  title='Checked' border='0' />";

                }
                if (gridObject.IsCanApproved) {
                    tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return PerformApproveActionWithConfirmation('" + 'Approved' + "', " + gridObject.Id + ")\" alt='Approved'  title='Approved' border='0' />";
                }

                tr += "<td style='display:none'>" + gridObject.Id + "</td>";

                tr += "</tr>";

                $("#BalanceTransferTable tbody").append(tr);

                tr = "";
                i++;
            });
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.NextButton);
            return false;
        }
        function FillFormEdit(Id) {
            FillForm(Id);
            return false;
        }
        function FillForm(Id) {
            $("#ContentPlaceHolder1_btnClean").hide();

            CommonHelper.SpinnerOpen();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Payroll/EmployeeBalanceTransfer.aspx/FillForm',

                data: "{'Id':'" + Id + "'}",
                dataType: "json",
                success: function (data) {
                    if (!confirm("Do you want to edit  Balance Transfer?")) {
                        return false;
                    }
                    $("#CreateNewDialog").dialog({
                        autoOpen: true,
                        modal: true,
                        width: '75%',
                        closeOnEscape: true,
                        resizable: false,
                        height: 'auto',
                        fluid: true,
                        title: "Update Balance Transfer",
                        show: 'slide'
                    });
                    $("#btnSave").val("Update And Close");
                    $("#btnClear").hide();
                    $("#btnSaveNContinue").hide();
                    // $("#AddNewStatusContaiiner").dialog({ title: "Edit Source - " + data.d.SourceName + " " });
                    $("#ContentPlaceHolder1_hfId").val(data.d.Id)
                    $("#ContentPlaceHolder1_ddlTransferFrom").val(data.d.TransferFrom).trigger('change');
                    $("#ContentPlaceHolder1_ddlTransferTo").val(data.d.TransferTo).trigger('change');
                    $("#ContentPlaceHolder1_txtTransferAmount").val(data.d.TransferAmount);
                    $("#ContentPlaceHolder1_txtDescription").val(data.d.Description);
                    
                    CommonHelper.SpinnerClose();
                },
                error: function (result) {

                }
            });
            return false;
        }
        function PerformClearAction() {
            $("#<%=hfId.ClientID%>").val("0")
            $("#<%=ddlTransferFrom.ClientID%>").val("0").trigger('change');
            $("#<%=ddlTransferTo.ClientID%>").val("0").trigger('change');
            $("#<%=txtDescription.ClientID%>").val("");
            $("#<%=txtTransferAmount.ClientID%>").val("");
            $("#btnClear").show();
            $("#btnSave").val("Save & Close");
            $("#btnSaveNContinue").val("Save & Continue").show();
        }
        function PerformApproveActionWithConfirmation(ApprovedStatus, Id) {
            if (!confirm('Do you want to ' + (ApprovedStatus == 'Checked' ? 'check' : 'approve') + " ?")) {
                return false;
            }
            ApproveAction(ApprovedStatus, Id);
        }
        function ApproveAction(ApprovedStatus, Id) {

            PageMethods.ApproveAction(Id, ApprovedStatus, OnApprovalSucceed, OnApprovalFailed);
        }

        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);

                GridPaging($("#GridPagingContainer").find("li.active").index(), 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnApprovalFailed() {

        }
        function PerformDeleteAction(Id) {
            if (!confirm("Do you want to Delete?")) {
                return false;
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Payroll/EmployeeBalanceTransfer.aspx/PerformDeleteAction',
                data: "{'Id':'" + Id + "'}",
                dataType: "json",
                success: function (data) {
                    CommonHelper.AlertMessage(data.d.AlertMessage);
                    GridPaging(1, 1);
                },
                error: function (result) {

                }
            });
            return false;
        }
        function Clean() {
            $("#<%=ddlTransferFromSearch.ClientID%>").val("0").trigger('change');
            $("#<%=ddlTransferToSearch.ClientID%>").val("0").trigger('change');

            return false;
        }
    </script>
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>

    <div class="panel panel-default">
        <div class="panel-heading">
            Balance Transfer Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Transfer From</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTransferFromSearch" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Transfer To</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTransferToSearch" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return GridPaging(1,1);" />
                        <asp:Button ID="btnClean" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return Clean();" />
                        <asp:Button ID="btnCreateNew" runat="server" Text="New Balance Transfer" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateNew();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Balance Transfer Information
        </div>
        <div class="panel-body">
            <div class="form-group" id="BalanceTransferTableContainer" style="overflow: scroll;">
                <table class="table table-bordered table-condensed table-responsive" id="BalanceTransferTable"
                    style="width: 100%;">
                    <thead>
                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                            <th style="width: 15%;">Transfer From
                            </th>
                            <th style="width: 15%;">Transfer To
                            </th>
                            <th style="width: 15%;">Transfer Amount
                            </th>
                            <th style="width: 10%;">Approval Status
                            </th>
                            <th style="width: 30%;">Description
                            </th>
                            <th style="width: 15%;">Action
                            </th>
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
    <div id="CreateNewDialog" style="display: none; overflow: unset">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label required-field">Transfer From</label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlTransferFrom" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <label class="control-label required-field">Transfer To</label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlTransferTo" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label required-field">Transfer Amount</label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtTransferAmount" runat="server" CssClass="quantitydecimal form-control "></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label required-field">Description</label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" Style="resize: none;"></asp:TextBox>
                </div>
            </div>
            <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                <div class="col-md-12">
                    <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="SaveAndClose()" value="Save & Close" />
                    <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return PerformClearAction();" />
                    <input id="btnSaveNContinue" type="button" value="Save & Continue" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return SaveOrUpdateTransfer();" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
