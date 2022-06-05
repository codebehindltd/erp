<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="InvItemAttributeSetup.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.InvItemAttributeSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            ItemAttributeSetupTable = $("#tblSetup").DataTable({
                data: [],
                columns: [
                    { title: "Name", "data": "Name", sWidth: '20%' },
                    { title: "Setup Type", "data": "SetupType", sWidth: '15%' },
                    { title: "Description", "data": "Description", sWidth: '35%' },
                    { title: "Status", "data": "Status", sWidth: '15%' },
                    { title: "Action", "data": null, sWidth: '15%' },
                    { title: "", "data": "Id", visible: false },
                    { title: "", "data": "SetupType", visible: false }
                ],
                columnDefs: [
                    {
                        "targets": 2,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            var status;
                            if (data == 0)
                                img = '';
                            else
                                img = data;
                            return img;
                        }
                    },
                    {
                        "targets": 3,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            var status;
                            if (data == true)
                                img = 'Active';
                            else
                                img = 'Inactive';
                            return img;
                        }
                    }
                ],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var row = '';
                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }
                    //if (IsCanEdit )
                    row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformEdit('" + aData.Id + "','" + aData.SetupType + "');\"> <img alt=\"Edit\" src=\"/Images/edit.png\" title='Edit' /> </a>";
                    //if (IsCanDelete )
                    //row += "&nbsp;&nbsp;<a href='javascript:void();' onclick= \"DeleteSetup('" + aData.Id + "');\"> <img alt='Delete' src='/Images/delete.png' title='Delete' /></a>";

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
            GridPaging(1, 1);
        });
        function CreateNew() {
            var setupType = $("#ContentPlaceHolder1_ddlSetupType").val();
            var iframeid = 'frmPrint';
            var url = "./InvItemAttributeSetupIframe.aspx?sType=" + setupType;
            document.getElementById(iframeid).src = url;

            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "90%",
                height: 350,
                closeOnEscape: false,
                resizable: false,
                title: "New Item Attribute",
                show: 'slide'
            });
            return false;
        }
        function CloseDialog() {
            $("#SalesNoteDialog").dialog('close');
            return false;
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchSupportNCase(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function SearchSupportNCase(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = ItemAttributeSetupTable.data().length;
            var name = $("#ContentPlaceHolder1_txtName").val();
            var setupType = $("#ContentPlaceHolder1_ddlSetupType").val();
            var status = $("#ContentPlaceHolder1_ddlStatus").val();

            PageMethods.GetInvItemAttributeSetupBySearchCriteria(name, setupType, status, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSupportNCaseLoadingSucceed, OnSupportNCaseLoadingFailed);
            return false;
        }

        function OnSupportNCaseLoadingSucceed(result) {
            ItemAttributeSetupTable.clear();
            ItemAttributeSetupTable.rows.add(result.GridData);
            ItemAttributeSetupTable.draw();

            $("#GridPagingContainer ul").html("");
            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
            return false;
        }

        function OnSupportNCaseLoadingFailed(error) {
            toastr.error(error.get_message());
            return false;
        }
        function PerformEdit(id) {
            if (!confirm("Do you want to edit ?")) {
                return false;
            }

            var iframeid = 'frmPrint';
            var url = "./InvItemAttributeSetupIframe.aspx?sid=" + id;
            document.getElementById(iframeid).src = url;

            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "90%",
                height: 350,
                closeOnEscape: false,
                resizable: false,
                title: "Item Attribute",
                show: 'slide'
            });
            return false;
        }
        function DeleteSetup(id) {
            if (confirm("Do you want to delete?")) {
                PageMethods.DeleteSetup(id, OnSuccessDelete, OnFailedDelete);
            }
            return false;
        }

        function OnSuccessDelete(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                GridPaging(1, 1);
            }
            return false;
        }

        function OnFailedDelete(error) {
            toastr.error(error.get_message());
            return false;
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }
        function PerformClearAction() {
            $("#ContentPlaceHolder1_txtName").val('');
            $("#ContentPlaceHolder1_ddlSetupType").val('');
            $("#ContentPlaceHolder1_ddlStatus").val('');
        }
    </script>
    <div id="SalesNoteDialog" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Item Attribute Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label ">Name</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" AutoComplete="off">                            
                        </asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Setup Type</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSetupType" CssClass="form-control" runat="server" Style="width: 100%;">
                            <asp:ListItem Value="">--- All ---</asp:ListItem>
                            <asp:ListItem Value="Color">Color</asp:ListItem>
                            <asp:ListItem Value="Size">Size</asp:ListItem>
                            <asp:ListItem Value="Style">Style</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Status</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlStatus" CssClass="form-control" runat="server" Style="width: 100%;">
                            <asp:ListItem Value="">--- All ---</asp:ListItem>
                            <asp:ListItem Value="1">Active</asp:ListItem>
                            <asp:ListItem Value="0">InActive</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return GridPaging(1,1);" />
                        <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                            onclick="javascript: return PerformClearAction();" />
                        <asp:Button ID="btnCreateNew" runat="server" Text="New Attribute Setup" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateNew();" />
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-body">
            <table id="tblSetup" class="table table-bordered table-condensed table-responsive">
            </table>
            <div class="childDivSection">
                <div class="text-center" id="GridPagingContainer">
                    <ul class="pagination">
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
