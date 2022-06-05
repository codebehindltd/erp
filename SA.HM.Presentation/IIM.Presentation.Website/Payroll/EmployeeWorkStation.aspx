﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="EmployeeWorkStation.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.EmployeeWorkStation" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var flag = 0;
        $(document).ready(function () {
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
                title: "Work Station Information",
                show: 'slide'
            });

            return false;
        }

        function SaveAndClose() {
            flag = 1;
            SaveOrUpdateWorkStation();
            return false;

        }
        function SaveOrUpdateWorkStation() {

            var id = $("#ContentPlaceHolder1_hfId").val()
            var WorkStationName = $("#ContentPlaceHolder1_txtWorkStationName").val();
            if (id == 0) {
                var IsUpdate = 0;
            }
            else {
                IsUpdate = 1;
            } if (WorkStationName == "") {
                toastr.warning("Enter Work Station Name");
                $("#ContentPlaceHolder1_txtWorkStationName").focus();
                return false;
            }
            PageMethods.DuplicateCheckDynamicaly("WorkStationName", WorkStationName, IsUpdate, id, DuplicateCheckDynamicalySucceed, DuplicateCheckDynamicalyFailed);
            return false;

        }
        function DuplicateCheckDynamicalySucceed(result) {
            id = $("#ContentPlaceHolder1_hfId").val()
            WorkStationName = $("#ContentPlaceHolder1_txtWorkStationName").val();
            description = $("#ContentPlaceHolder1_txtDescription").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val() == "Active" ? true : false;
            if (result > 0) {
                toastr.warning("Duplicate Work Station Name");
                $("#ContentPlaceHolder1_txtWorkStationName").focus();
                return false;
            }
            else {
                var EmpWorkStationBO = {
                    WorkStationId: id,
                    WorkStationName: WorkStationName,
                    Description: description,
                    Status: status,
                }
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../Payroll/EmployeeWorkStation.aspx/SaveUpdateWorkStationInformation',

                    data: JSON.stringify({ EmpWorkStationBO: EmpWorkStationBO }),
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

            $("#ContentPlaceHolder1_txtWorkStationName").focus();
        }
        function DuplicateCheckDynamicalyFailed(error) {

        }

        function PerformClearAction() {
            $("#ContentPlaceHolder1_hfId").val("0")
            $("#ContentPlaceHolder1_txtWorkStationName").val("");
            $("#ContentPlaceHolder1_txtDescription").val("");
            $("#ContentPlaceHolder1_ddlStatus").val("Active");
            $("#btnClear").show();
            $("#btnSave").val("Save & Close");
            $("#btnSaveNContinue").val("Save & Continue").show();
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadGrid(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function LoadGrid(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#WorkStationTable tbody tr").length;
            var WorkStationName = $("#ContentPlaceHolder1_txtWorkStationNameForSearch").val();
            var status = $("#ContentPlaceHolder1_ddlStatusForSearch").val() == "Active" ? true : false;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Payroll/EmployeeWorkStation.aspx/LoadWorkStationNameSearch',

                data: "{'WorkStationName':'" + WorkStationName + "','Status':'" + status + "', 'gridRecordsCount':'" + gridRecordsCount + "', 'pageNumber':'" + pageNumber + "', 'isCurrentOrPreviousPage':'" + IsCurrentOrPreviousPage + "'}",
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

            $("#WorkStationTable tbody").empty();
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

                tr += "<td style='width:75%;'>" + gridObject.WorkStationName + "</td>";

                tr += "<td style='width:25%;'> <a onclick=\"javascript:return FillFormEdit(" + gridObject.WorkStationId + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'DeleteWorkStation(" + gridObject.WorkStationId + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";

                tr += "<td style='display:none'>" + gridObject.WorkStationId + "</td>";


                tr += "</tr>";

                $("#WorkStationTable tbody").append(tr);

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
                url: '../Payroll/EmployeeWorkStation.aspx/FillForm',

                data: "{'Id':'" + Id + "'}",
                dataType: "json",
                success: function (data) {
                    if (!confirm("Do you want to edit - " + data.d.WorkStationName + "?")) {
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
                        title: "Update Work Station - " + data.d.WorkStationName,
                        show: 'slide'
                    });
                    $("#btnSave").val("Update And Close");
                    $("#btnClear").hide();
                    $("#btnSaveNContinue").hide();
                    $("#ContentPlaceHolder1_hfId").val(data.d.Id)
                    $("#ContentPlaceHolder1_txtWorkStationName").val(data.d.WorkStationName);
                    $("#ContentPlaceHolder1_txtDescription").val(data.d.Description);
                    var status = data.d.Status == true ? "Active" : "Inactive"
                    $("#ContentPlaceHolder1_ddlStatus").val(status);

                    CommonHelper.SpinnerClose();
                },
                error: function (result) {

                }
            });
            return false;
        }

        function DeleteWorkStation(Id) {
            if (!confirm("Do you want to Delete?")) {
                return false;
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Payroll/EmployeeWorkStation.aspx/DeleteWorkStation',
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
            $("#ContentPlaceHolder1_txtWorkStationNameForSearch").val("");
            return false;
        }

    </script>
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>

    <div class="panel panel-default">
        <div class="panel-heading">
            Work Station Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Work Station Name</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtWorkStationNameForSearch" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Status</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlStatusForSearch" runat="server" CssClass="form-control" TabIndex="4">
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
                        <asp:Button ID="btnCreateNew" runat="server" Text="New Work Station" CssClass="TransactionalButton btn btn-primary btn-sm"
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
            <div class="form-group" id="WorkStationTableContainer">
                <table class="table table-bordered table-condensed table-responsive" id="WorkStationTable"
                    style="width: 100%;">
                    <thead>
                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                            <th style="width: 75%;">Work Station Name
                            </th>
                            <th style="width: 25%;">Action
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
                    <label class="control-label required-field">Work Station Name</label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtWorkStationName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label">Description</label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" TabIndex="6" Style="resize: none;"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
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
                    <input id="btnSaveNContinue" type="button" value="Save & Continue" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return SaveOrUpdateWorkStation();" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
