<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="AccountManagerNew.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.AccountManagerNew" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var flag = 0;
        $(document).ready(function () {
            $("#ContentPlaceHolder1_txtAccountManager").autocomplete({
                source: function (request, response) {

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/AccountManagerNew.aspx/GetEmployeeInfoForAcountManager',
                        data: "{'searchString':'" + request.term + "'}",
                        dataType: "json",
                        async: false,
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.DisplayName,
                                    value: m.EmpId,
                                    EmpCode: m.EmpCode,
                                    EmpId: m.EmpId,
                                    DisplayName: m.DisplayName,

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
                    $("#ContentPlaceHolder1_hfAccountManager").val(ui.item.value);
                    $(this).val(ui.item.label);
                }
            });
            $("#ContentPlaceHolder1_txtSupervisonName").autocomplete({
                source: function (request, response) {

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/AccountManagerNew.aspx/GetEmployeeInfoForSupervison',
                        data: "{'searchString':'" + request.term + "'}",
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.DisplayName,
                                    value: m.AccountManagerId,
                                    EmpId: m.EmpId,
                                    DisplayName: m.DisplayName,

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
                    $("#ContentPlaceHolder1_hfSupervisonName").val(ui.item.value);
                    $(this).val(ui.item.label);
                }
            });

            GridPaging(1, 1);
        });
        function CreateNew() {
            //PerformClearAction();
            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '75%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "AccountManager Information",
                show: 'slide'
            });

            return false;
        }
        function SaveAndClose() {
            flag = 1;
            SaveOrUpdateAccountManager();
            return false;

        }
        function SaveOrUpdateAccountManager() {
            var id = $("#ContentPlaceHolder1_hfId").val();
            var empId = $("#ContentPlaceHolder1_hfAccountManager").val();
            var ancestorId = '-1';
            if ($("#ContentPlaceHolder1_hfSupervisonName").val() == '0' || $("#ContentPlaceHolder1_txtSupervisonName").val() == '') {
                var ancestorId = '-1';
            }
            else {
                var ancestorId = $("#ContentPlaceHolder1_hfSupervisonName").val();
            }
            var type = 'CRM';
            if (empId == "0" || $("#ContentPlaceHolder1_txtAccountManager").val() == '') {
                toastr.warning("Add an employee as an Account Manager");
                $("#ContentPlaceHolder1_txtAccountManager").focus();
                return false;
            }
            var AccountManagerBO = {
                AccountManagerId: id,
                EmpId: empId,
                AncestorId: ancestorId,
                Type: type
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/AccountManagerNew.aspx/SaveAccountManagerInformation',

                data: JSON.stringify({ AccountManagerBO: AccountManagerBO }),
                dataType: "json",
                success: function (data) {
                    GridPaging(1, 1);
                    PerformClearAction();
                    CommonHelper.AlertMessage(data.d.AlertMessage);
                    if (flag == 1) {
                        $('#CreateNewDialog').dialog('close');
                    }
                    flag = 0;
                },
                error: function (result) {

                }
            });
            return false;
        }
        function PerformClearAction() {
            $("#ContentPlaceHolder1_hfId").val('0');
            $("#ContentPlaceHolder1_hfAccountManager").val('0');
            $("#ContentPlaceHolder1_hfSupervisonName").val('0');
            $("#ContentPlaceHolder1_txtAccountManager").val('');
            $("#ContentPlaceHolder1_txtSupervisonName").val('');
            return false;
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadGrid(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function LoadGrid(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#AccountManagerTable tbody tr").length;
            var AccountManager = $("#ContentPlaceHolder1_txtAccountManagerSrc").val();
            var SupervisonName = $("#ContentPlaceHolder1_txtSupervisonNameSrc").val();

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/AccountManagerNew.aspx/LoadAccountManagerSearch',

                data: "{'AccountManagerName':'" + AccountManager + "','SupervisonName':'" + SupervisonName + "', 'gridRecordsCount':'" + gridRecordsCount + "', 'pageNumber':'" + pageNumber + "', 'isCurrentOrPreviousPage':'" + IsCurrentOrPreviousPage + "'}",
                dataType: "json",
                success: function (data) {
                    LoadTable(data);
                },
                error: function (result) {
                    //PerformClearAction();
                }
            });
            return false;
        }
        function LoadTable(data) {

            $("#AccountManagerTable tbody").empty();
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

                tr += "<td style='width:40%;'>" + gridObject.AccountManager + "</td>";
                tr += "<td style='width:40%;'>" + gridObject.SupervisonName + "</td>";

                tr += "<td style='width:20%;'> <a onclick=\"javascript:return FillFormEdit(" + gridObject.AccountManagerId + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'DeleteAccountManager(" + gridObject.AccountManagerId + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";

                tr += "<td style='display:none'>" + gridObject.AccountManagerId + "</td>";


                tr += "</tr>";

                $("#AccountManagerTable tbody").append(tr);

                tr = "";
                i++;
            });
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.NextButton);
            return false;
        }
        function Clean() {
            $("#ContentPlaceHolder1_txtAccountManagerSrc").val('');
            $("#ContentPlaceHolder1_txtSupervisonNameSrc").val('');
            return false;
        }
        function DeleteAccountManager(Id) {
            if (!confirm("Do You Want To Delete?"))
                return false;

            PageMethods.DeleteAccountManager(Id, OnDeleteAccountManagerSucceded, OnDeleteAccountManagerFailed);
        }
        function OnDeleteAccountManagerSucceded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                GridPaging(1, 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnDeleteAccountManagerFailed() {

        }

        function FillFormEdit(Id) {
            $("#ContentPlaceHolder1_btnClear").hide();

            CommonHelper.SpinnerOpen();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/AccountManagerNew.aspx/FillForm',

                data: "{'Id':'" + Id + "'}",
                dataType: "json",
                success: function (data) {
                    if (!confirm("Do you want to edit - " + data.d.AccountManager + "?")) {
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
                        title: "Update Account Manager - " + data.d.AccountManager,
                        show: 'slide'
                    });
                    $("#btnSave").val("Update And Close");
                    $("#btnClear").hide();
                    $("#btnSaveNContinue").hide();
                    // $("#AddNewStatusContaiiner").dialog({ title: "Edit Source - " + data.d.SourceName + " " });
                    $("#ContentPlaceHolder1_hfId").val(data.d.AccountManagerId)
                    $("#ContentPlaceHolder1_hfAccountManager").val(data.d.EmpId)
                    $("#ContentPlaceHolder1_hfSupervisonName").val(data.d.AncestorId);
                    $("#ContentPlaceHolder1_txtAccountManager").val(data.d.AccountManager);
                    $("#ContentPlaceHolder1_txtSupervisonName").val(data.d.SupervisonName);

                    CommonHelper.SpinnerClose();
                },
                error: function (result) {

                }
            });
            return false;
        }
        function LoadTreeView() {
            var iframeid = 'frmPrint';
            var url = "./AccountManagerTreeView.aspx";
            document.getElementById(iframeid).src = url;
            $("#AccountManagerTreeView").dialog({
                autoOpen: true,
                modal: true,
                width: "90%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                title: "Account Manager",
                show: 'slide'
            });
            return false;
        }
    </script>
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>
    <div id="AccountManagerTreeView" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Account Manager Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Account Manager</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtAccountManagerSrc" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Supervison Name</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtSupervisonNameSrc" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return GridPaging(1,1);" />
                        <asp:Button ID="btnClean" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return Clean();" />
                        <asp:Button ID="btnCreateNew" runat="server" Text="New Account Manager" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateNew();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Account Manager Information
            <a style="float: right; padding: 0px;" href='javascript:void();' onclick='javascript:return LoadTreeView()' title='Account Manager'>
                <img style='width: 22px; height: 20px;' alt='Search Quotation' src='/Images/management.png' /></a>

        </div>
        <div class="panel-body">
            <div class="form-group" id="AccountManagerTableContainer">
                <table class="table table-bordered table-condensed table-responsive" id="AccountManagerTable"
                    style="width: 100%;">
                    <thead>
                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                            <th style="width: 40%;">Account Manager
                            </th>
                            <th style="width: 40%;">Supervison Name
                            </th>
                            <th style="width: 20%;">Action
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
                <asp:HiddenField ID="hfAccountManager" runat="server" Value="0" />
                <div class="col-md-2">
                    <label class="control-label required-field">Account Manager</label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtAccountManager" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <asp:HiddenField ID="hfSupervisonName" runat="server" Value="0" />
                <div class="col-md-2">
                    <label class="control-label">Supervison Name</label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtSupervisonName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                <div class="col-md-12">
                    <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="SaveAndClose()" value="Save & Close" />
                    <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return PerformClearAction();" />
                    <input id="btnSaveNContinue" type="button" value="Save & Continue" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return SaveOrUpdateAccountManager();" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
