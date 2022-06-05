<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="ContactTitle.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.ContactTitle" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var flag = 0;
        $(document).ready(function () {
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
                title: "Contact Title Information",
                show: 'slide'
            });

            return false;
        }
        function PerformClearAction() {
            $("#ContentPlaceHolder1_hfId").val("0")
            $("#ContentPlaceHolder1_txtSourceName").val("");
            $("#ContentPlaceHolder1_ddlTitleType").val("0");
            $("#ContentPlaceHolder1_ddlStatus").val("Active");
            $("#btnClear").show();
            $("#btnSave").val("Save & Close");
            $("#btnSaveNContinue").val("Save & Continue").show();
        }
        function SaveOrUpdateSource() {
            var IsUpdate = 0;
            var id = $("#ContentPlaceHolder1_hfId").val()
            var sourceName = $("#ContentPlaceHolder1_txtSourceName").val();
            if (id == 0) {
                IsUpdate = 0;
            }
            else {
                IsUpdate = 1;
            }
            if (sourceName == "") {
                toastr.warning("Enter Contact Title");
                $("#ContentPlaceHolder1_txtSourceName").focus();
                return false;
            }
            if ($("#ContentPlaceHolder1_ddlTitleType").val() == "0") {
                toastr.warning("Enter Title Type");
                $("#ContentPlaceHolder1_ddlTitleType").focus();
                return false;
            }

            PageMethods.DuplicateCheckDynamicaly("Title", sourceName, IsUpdate, id, DuplicateCheckDynamicalySucceed, DuplicateCheckDynamicalyFailed);
            return false;

        }
        function DuplicateCheckDynamicalySucceed(result) {
            id = $("#ContentPlaceHolder1_hfId").val()
            sourceName = $("#ContentPlaceHolder1_txtSourceName").val();
            transectionType = $("#ContentPlaceHolder1_ddlTitleType").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val() == "Active" ? true : false;
            if (result > 0) {
                toastr.warning("Duplicate Contact Title");
                $("#ContentPlaceHolder1_txtSourceName").focus();
                return false;
            }
            else {
                var ContactTitleBO = {
                    Id: id,
                    Title: sourceName,
                    TransectionType: transectionType,
                    Status: status,
                }
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../SalesAndMarketing/ContactTitle.aspx/SaveUpdateContactTitle',
                    data: JSON.stringify({ contactTitleBO: ContactTitleBO }),
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
            }

            $("#ContentPlaceHolder1_txtSourceName").focus();
        }
        function DuplicateCheckDynamicalyFailed(error) {

        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadGrid(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function LoadGrid(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#SourceTable tbody tr").length;
            var contactTitle = $("#ContentPlaceHolder1_txtSourceNameForSearch").val();
            var transectionType = $("#ContentPlaceHolder1_ddlSrcTransectionType").val();
            var status = $("#ContentPlaceHolder1_ddlStatusForSearch").val();

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/ContactTitle.aspx/LoadContactTitleSearch',
                data: "{'paramTitle':'" + contactTitle + "', 'paramTransectionType':'" + transectionType + "', 'paramStatus':'" + status + "', 'gridRecordsCount':'" + gridRecordsCount + "', 'pageNumber':'" + pageNumber + "', 'isCurrentOrPreviousPage':'" + IsCurrentOrPreviousPage + "'}",
                dataType: "json",
                success: function (data) {
                    LoadTable(data);
                },
                error: function (result) {
                    PerformClearAction();
                }
            });
            return false;
        }
        function LoadTable(data) {

            $("#SourceTable tbody").empty();
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

                tr += "<td style='width:50%;'>" + gridObject.Title + "</td>";
                tr += "<td style='width:30%;'>" + gridObject.TransectionType + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.ActiveStatus + "</td>";
                tr += "<td style='width:10%;'> <a onclick=\"javascript:return FillFormEdit(" + gridObject.Id + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'DeleteSource(" + gridObject.Id + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";

                tr += "<td style='display:none'>" + gridObject.Id + "</td>";


                tr += "</tr>";

                $("#SourceTable tbody").append(tr);

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
            $("#ContentPlaceHolder1_btnClear").hide();

            CommonHelper.SpinnerOpen();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/ContactTitle.aspx/FillForm',

                data: "{'Id':'" + Id + "'}",
                dataType: "json",
                success: function (data) {
                    if (!confirm("Do you want to edit - " + data.d.SourceName + "?")) {
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
                        title: "Update Contact Title - " + data.d.SourceName,
                        show: 'slide'
                    });
                    $("#btnSave").val("Update And Close");
                    $("#btnClear").hide();
                    $("#btnSaveNContinue").hide();
                    // $("#AddNewStatusContaiiner").dialog({ title: "Edit Source - " + data.d.SourceName + " " });
                    $("#ContentPlaceHolder1_hfId").val(data.d.Id)
                    $("#ContentPlaceHolder1_txtSourceName").val(data.d.Title);
                    $("#ContentPlaceHolder1_ddlTitleType").val(data.d.TransectionType);
                    var status = data.d.Status == true ? "Active" : "Inactive"
                    $("#ContentPlaceHolder1_ddlStatus").val(status);

                    CommonHelper.SpinnerClose();
                },
                error: function (result) {

                }
            });
            return false;
        }
        function DeleteSource(Id) {
            if (!confirm("Do you want to Delete?")) {
                return false;
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/ContactTitle.aspx/DeleteSource',
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
            $("#ContentPlaceHolder1_txtSourceNameForSearch").val("");
            return false;
        }
        function SaveAndClose() {
            flag = 1;
            SaveOrUpdateSource();
            return false;

        }
    </script>
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>
    <div class="panel panel-default">
        <div class="panel-heading">
            Contact Title Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Contact Title</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtSourceNameForSearch" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Title Type</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSrcTransectionType" runat="server" CssClass="form-control" TabIndex="4">
                            <asp:ListItem Value="0">--- All ---</asp:ListItem>
                            <asp:ListItem Value="Number">Phone</asp:ListItem>
                            <asp:ListItem Value="Email">Email</asp:ListItem>
                            <asp:ListItem Value="Fax">Fax</asp:ListItem>
                            <asp:ListItem Value="Website">Website</asp:ListItem>
                            <asp:ListItem Value="SocialMedia">Social Media</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Status</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlStatusForSearch" runat="server" CssClass="form-control" TabIndex="4">
                            <asp:ListItem Value="0">-- All --</asp:ListItem>
                            <asp:ListItem Value="Active">Active</asp:ListItem>
                            <asp:ListItem Value="Inactive">Inactive</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return GridPaging(1,1);" />
                        <asp:Button ID="btnClean" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return Clean();" />
                        <asp:Button ID="btnCreateNew" runat="server" Text="New Contact Title" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateNew();" />
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
            <div class="form-group" id="SourceTableContainer">
                <table class="table table-bordered table-condensed table-responsive" id="SourceTable"
                    style="width: 100%;">
                    <thead>
                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                            <th style="width: 50%;">Contact Title
                            </th>
                            <th style="width: 30%;">Title Type
                            </th>
                            <th style="width: 10%;">Status
                            </th>
                            <th style="width: 10%;">Action
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
                    <label class="control-label required-field">Contact Title</label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtSourceName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label">Title Type</label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlTitleType" runat="server" CssClass="form-control" TabIndex="4">
                        <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                        <asp:ListItem Value="Number">Phone</asp:ListItem>
                        <asp:ListItem Value="Email">Email</asp:ListItem>
                        <asp:ListItem Value="Fax">Fax</asp:ListItem>
                        <asp:ListItem Value="Website">Website</asp:ListItem>
                        <asp:ListItem Value="SocialMedia">Social Media</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <label class="control-label required-field">Status</label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" TabIndex="4">
                        <asp:ListItem Value="Active">Active</asp:ListItem>
                        <asp:ListItem Value="Inactive">Inactive</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                <div class="col-md-12">
                    <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="SaveAndClose()" value="Save & Close" />
                    <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return PerformClearAction();" />
                    <div style="display:none;">
                        <input id="btnSaveNContinue" type="button" value="Save & Continue" class="TransactionalButton btn btn-primary btn-sm"
                            onclick="javascript: return SaveOrUpdateSource();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
