<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="VMVehicleType.aspx.cs" Inherits="HotelManagement.Presentation.Website.VehicleManagement.VMVehicleType" %>

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
                title: "Vehicle Type Information",
                show: 'slide'
            });

            return false;
        }
        function PerformClearAction() {
            $("#ContentPlaceHolder1_hfId").val("0")
            $("#ContentPlaceHolder1_txtTypeName").val("");
            $("#ContentPlaceHolder1_txtDescription").val("");
            $("#ContentPlaceHolder1_ddlStatus").val("Active");
            $("#btnClear").show();
            $("#btnSave").val("Save & Close");
            $("#btnSaveNContinue").val("Save & Continue").show();
        }
        function SaveOrUpdateVehicleType() {

            var id = $("#ContentPlaceHolder1_hfId").val()
            var typeName = $("#ContentPlaceHolder1_txtTypeName").val();
            if (id == 0) {
                var IsUpdate = 0;
            }
            else {
                IsUpdate = 1;
            } if (typeName == "") {
                toastr.warning("Enter Type Name");
                $("#ContentPlaceHolder1_txtTypeName").focus();
                return false;
            }
            PageMethods.DuplicateCheckDynamicaly("TypeName", typeName, IsUpdate, id, DuplicateCheckDynamicalySucceed, DuplicateCheckDynamicalyFailed);
            return false;

        }
        function DuplicateCheckDynamicalySucceed(result) {
            id = $("#ContentPlaceHolder1_hfId").val()
            typeName = $("#ContentPlaceHolder1_txtTypeName").val();
            description = $("#ContentPlaceHolder1_txtDescription").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val() == "Active" ? true : false;
            if (result > 0) {
                toastr.warning("Duplicate Type Name");
                $("#ContentPlaceHolder1_txtTypeName").focus();
                return false;
            }
            else {
                var VMVehicleTypeBO = {
                    Id: id,
                    TypeName: typeName,
                    Description: description,
                    Status: status,
                }
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: './VMVehicleType.aspx/SaveUpdateVehicleTypeInformation',

                    data: JSON.stringify({ vMVehicleTypeBO: VMVehicleTypeBO }),
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

            $("#ContentPlaceHolder1_txtTypeName").focus();
        }
        function DuplicateCheckDynamicalyFailed(error) {

        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadGrid(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function LoadGrid(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#VehicleTypeTable tbody tr").length;
            var typeName = $("#ContentPlaceHolder1_txtTypeNameForSearch").val();
            var status = $("#ContentPlaceHolder1_ddlStatusForSearch").val() == "Active" ? true : false;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './VMVehicleType.aspx/LoadTypeNameSearch',

                data: "{'TypeName':'" + typeName + "','Status':'" + status + "', 'gridRecordsCount':'" + gridRecordsCount + "', 'pageNumber':'" + pageNumber + "', 'isCurrentOrPreviousPage':'" + IsCurrentOrPreviousPage + "'}",
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

            $("#VehicleTypeTable tbody").empty();
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

                tr += "<td style='width:75%;'>" + gridObject.TypeName + "</td>";

                tr += "<td style='width:25%;'> <a onclick=\"javascript:return FillFormEdit(" + gridObject.Id + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'DeleteVehicleType(" + gridObject.Id + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";

                tr += "<td style='display:none'>" + gridObject.Id + "</td>";


                tr += "</tr>";

                $("#VehicleTypeTable tbody").append(tr);

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
                url: './VMVehicleType.aspx/FillForm',

                data: "{'Id':'" + Id + "'}",
                dataType: "json",
                success: function (data) {
                    if (!confirm("Do you want to edit - " + data.d.TypeName + "?")) {
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
                        title: "Update Vehicle Type - "+ data.d.TypeName ,
                        show: 'slide'
                    });
                    $("#btnSave").val("Update And Close");
                    $("#btnClear").hide();
                    $("#btnSaveNContinue").hide();
                   // $("#AddNewStatusContaiiner").dialog({ title: "Edit Vehicle Type - " + data.d.TypeName + " " });
                    $("#ContentPlaceHolder1_hfId").val(data.d.Id)
                    $("#ContentPlaceHolder1_txtTypeName").val(data.d.TypeName);
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
        function DeleteVehicleType(Id) {
            if (!confirm("Do you want to Delete?")) {
                return false;
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './VMVehicleType.aspx/DeleteVehicleType',
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
            $("#ContentPlaceHolder1_txtTypeNameForSearch").val("");
            return false;
        }
        function SaveAndClose() {
            flag = 1;
            SaveOrUpdateVehicleType();
            return false;

        }
    </script>
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>

    <div class="panel panel-default">
        <div class="panel-heading">
            Vehicle Type Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Type Name</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtTypeNameForSearch" runat="server" CssClass="form-control"></asp:TextBox>
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
                        <asp:Button ID="btnCreateNew" runat="server" Text="New Vehicle Type" CssClass="TransactionalButton btn btn-primary btn-sm"
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
            <div class="form-group" id="VehicleTypeTableContainer">
                <table class="table table-bordered table-condensed table-responsive" id="VehicleTypeTable"
                    style="width: 100%;">
                    <thead>
                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                            <th style="width: 75%;">Type Name
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
                    <label class="control-label required-field">Type Name</label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtTypeName" runat="server" CssClass="form-control"></asp:TextBox>
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
                        onclick="javascript: return SaveOrUpdateVehicleType();" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
