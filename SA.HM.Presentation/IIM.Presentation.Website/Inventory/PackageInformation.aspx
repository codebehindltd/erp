<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="PackageInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.PackageInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var ProjectTable;
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;
        $(document).ready(function () {

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            ProjectTable = $("#tblPackage").DataTable({
                data: [],
                columns: [
                    { title: "Name", "data": "PackageName", sWidth: '30%' },
                    { title: "Status", "data": "IsActive", sWidth: '15%' },
                    { title: "Action", "data": null, sWidth: '15%' },
                    { title: "", "data": "ServicePackageId", visible: false }
                ],

                columnDefs: [
                    {
                        "targets": 1,
                        "classname": "text-center",
                        "render": function (data, type, full, meta) {
                            return (data == true ? "Active" : "Inactive");
                        }
                    }],

                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var row = '';
                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }
                    if (IsCanEdit)
                        row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return EditPackage(" + aData.ServicePackageId + ",'" + aData.PackageName + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";
                    if (IsCanDelete)
                        row += "&nbsp;&nbsp;<a href='javascript:void();' onclick= \"DeletePackage(" + aData.ServicePackageId + ",'" + aData.PackageName + "');\"> <img alt='Delete' src='../Images/delete.png' title='Delete' /></a>";

                    $('td:eq(' + (nRow.children.length - 1) + ')', nRow).html(row);
                },
                pageLength: UserInfoFromDB.GridViewPageSize,
                filter: false,
                info: false,
                ordering: false,
                processing: true,
                retrieve: true,
                bAutoWidth: false,
                bLengthChange: false,
                bInfo: false,
                bPaginate: false,
                language: {
                    emptyTable: "No Data Found"
                },

            });
            if (IsCanSave)
                $("#btnSave").show();
            else
                $("#btnSave").hide();
            GridPaging(1, 1);
        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#tblPackage tbody tr").length;
            var name = $("#ContentPlaceHolder1_txtSearchName").val();
            var isActive = $("#ContentPlaceHolder1_ddlSearchStatus").val() == "1";
            PageMethods.SearchPackage(name, isActive, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchucceed, OnSearchFailed);
            return false;
        }

        function OnSearchucceed(result) {
            $("#GridPagingContainer ul").empty();

            ProjectTable.clear();
            ProjectTable.rows.add(result.GridData);
            ProjectTable.draw();

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
            return false;
        }

        function OnSearchFailed(error) {
            toastr.error(error.get_message());
            return false;
        }

        function CreateNew() {
            Clear();
            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '75%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Create Package",
                show: 'slide'
            });
            return false;
        }

        function Save() {
            var name = $("#ContentPlaceHolder1_txtName").val();
            var isActive = $("#ContentPlaceHolder1_ddlStatus").val() == "1";
            if (!name) {
                toastr.warning("Enter Package Name.");
                $("#ContentPlaceHolder1_txtName").focus();
                return false;
            }
            var servicePackageId = $("#ContentPlaceHolder1_hfServicePackageId").val();
            var ServicePackage = {
                ServicePackageId: servicePackageId,
                PackageName: name,
                IsActive: isActive
            }
            PageMethods.SaveOrUpdate(ServicePackage, OnSaveOrUpdateSucceed, OnSaveOrUpdateFailed);
            return false;
        }

        function OnSaveOrUpdateSucceed(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            if (result.IsSuccess) {
                GridPaging(1, 1);
                Clear();
            }
            return false;
        }

        function OnSaveOrUpdateFailed(error) {
            toastr.error(error.get_message());
        }

        function EditPackage(id, packageName) {
            if (!confirm(`Do you want to edit - ${packageName}?`)) {
                return false;
            }
            PageMethods.GetPackageInformationById(id, OnGetPackageSucceed, OnGetPackageFailed);
            return false;
        }

        function DeletePackage(id, packageName) {
            if (!confirm(`Do you want to delete - ${packageName}?`)) {
                return false;
            }
            PageMethods.DeletePackage(id, OnSaveOrUpdateSucceed, OnSaveOrUpdateFailed);
            return false;
        }

        function OnGetPackageSucceed(result) {
            Clear();
            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '75%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: `Update Package - ${result.PackageName} `,
                show: 'slide'
            });

            $("#ContentPlaceHolder1_hfServicePackageId").val(result.ServicePackageId);
            $("#ContentPlaceHolder1_txtName").val(result.PackageName);
            $("#ContentPlaceHolder1_ddlStatus").val((result.IsActive == true ? "1" : "0"));
            if (IsCanEdit)
                $("#btnSave").val("Update").show();
            else
                $("#btnSave").hide();
        }

        function OnGetPackageFailed(error) {
            toastr.error(error.get_message());
        }

        function Clear() {
            $("#ContentPlaceHolder1_hfServicePackageId").val("0");
            $("#ContentPlaceHolder1_txtName").val("");
            $("#ContentPlaceHolder1_ddlStatus").val("1");
            if (IsCanSave)
                $("#btnSave").val("Save").show();
            else
                $("#btnSave").hide();
        }

    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfServicePackageId" Value="0" runat="server" />
    <div class="panel panel-default">
        <div class="panel-heading">
            Package Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Name</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Status</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSearchStatus" runat="server" CssClass="form-control">
                            <asp:ListItem Value="1">Active</asp:ListItem>
                            <asp:ListItem Value="0">Inactive</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return GridPaging(1,1);" />
                        <asp:Button ID="btnCreateNew" runat="server" TabIndex="4" Text="New Package" CssClass="TransactionalButton btn btn-primary btn-sm"
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
            <table id="tblPackage" class="table table-bordered table-condensed table-responsive">
            </table>
            <div class="childDivSection">
                <div class="text-center" id="GridPagingContainer">
                    <ul class="pagination">
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div id="CreateNewDialog" style="display: none; overflow: unset">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label required-field">Name</label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label">Status</label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                        <asp:ListItem Value="1">Active</asp:ListItem>
                        <asp:ListItem Value="0">Inactive</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row" style="padding-bottom: 0; padding-top: 10px;">
            <div class="col-md-12">
                <input id="btnSave" type="button" value="Save" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="javascript: return Save();" />
                <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="javascript: return Clear();" />
            </div>
        </div>
    </div>
</asp:Content>
