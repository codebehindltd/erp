<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="ServiceBandwidthFrequency.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.ServiceBandwidthFrequency" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var FreequencyTable;
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;
        $(document).ready(function () {

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            CommonHelper.ApplyIntigerValidation();
            FreequencyTable = $("#tblFreequency").DataTable({
                data: [],
                columns: [
                    { title: "Frequency", "data": "Frequency", sWidth: '70%' },
                    { title: "Action", "data": null, sWidth: '30%' },
                    { title: "", "data": "Id", visible: false }
                ],

                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var row = '';
                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }
                    if (IsCanEdit)
                        row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return EditFreequency(" + aData.Id + ",'" + aData.Frequency + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";
                    if (IsCanDelete)
                        row += "&nbsp;&nbsp;<a href='javascript:void();' onclick= \"DeleteFreequency(" + aData.Id + ",'" + aData.Frequency + "');\"> <img alt='Delete' src='../Images/delete.png' title='Delete' /></a>";

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
            PerformClearAction();
            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '75%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Create Freequency",
                show: 'slide'
            });
            if (IsCanSave)
                $("#btnSave").val("Save").show();
            else
                $("#btnSave").hide();
            return false;
        }

        function SaveFreequency() {
            var freequency = Math.trunc($("#ContentPlaceHolder1_txtFreequency").val());
            if (freequency <= 0) {
                toastr.warning("Enter Freequency");
                $("#ContentPlaceHolder1_txtFreequency").focus();
                return false;
            }

            var id = $("#ContentPlaceHolder1_hfId").val();

            PageMethods.SaveOrUpdateFreeQuency(freequency, id, OnSuccessSaveOrUpdate, OnFailSaveOrUpdate);
            return false;
        }
        function OnSuccessSaveOrUpdate(result) {

            if (result.IsSuccess) {
                $("#CreateNewDialog").dialog('close');
                CommonHelper.AlertMessage(result.AlertMessage);
                GridPaging(1, 1);
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            return false;
        }

        function OnFailSaveOrUpdate(error) {
            toastr.error(error.get_message());
        }

        function EditFreequency(id ,Freequency) {
            if (!confirm("Do you want to edit - " + Freequency + "?")) {
                return false;
            }
            PerformClearAction();
            FillForm(id);
            return false;
        }
        function FillForm(id) {
            PageMethods.GetFreequencyId(id, OnSuccessLoad, OnFailLoad)
            return false;
        }

        function OnSuccessLoad(result) {

            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '75%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Update Freequency - " + result.Freequency,
                show: 'slide'
            });
            $("#ContentPlaceHolder1_hfId").val(result.Id);
            $("#ContentPlaceHolder1_txtFreequency").val(result.Frequency);
            if (IsCanEdit)
                $("#btnSave").val('Update').show();
            else
                $("#btnSave").hide();
            return false;
        }

        function OnFailLoad(error) {
            toastr.error(error.get_message());
        }

        function PerformClearAction() {
            $("#ContentPlaceHolder1_hfId").val("0");
            $("#ContentPlaceHolder1_txtFreequency").val("");
            if (IsCanSave)
                $("#btnSave").val("Save").show();
            else
                $("#btnSave").hide();
            return false;
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadGrid(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function LoadGrid(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#tblFreequency tbody tr").length;
            var freequency = $("#ContentPlaceHolder1_txtSearchFreequency").val();
            PageMethods.SearchFreequency(freequency, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchucceed, OnSearchFailed);
            return false;
        }
        function OnSearchucceed(result) {
            $("#GridPagingContainer ul").empty();
            FreequencyTable.clear();
            FreequencyTable.rows.add(result.GridData);
            FreequencyTable.draw();
            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
            return false;
        }

        function OnSearchFailed(error) {
            toastr.error(error.get_message());
            return false;
        }

        function DeleteFreequency(id,Freequency) {
            if (confirm("Want to delete "+ Freequency +"?")) {
                PageMethods.DeleteFreequency(id, OnSuccessDelete, OnFailedDelete);
            }
            return false;
        }

        function OnSuccessDelete(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                GridPaging(1, 1);
                Clear();
            }
            return false;
        }

        function OnFailedDelete(error) {
            toastr.error(error.get_message());
            return false;
        }

    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfId" Value="0" runat="server" />
    <div class="panel panel-default">
        <div class="panel-heading">
            Frequency Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Freequency</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchFreequency" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return GridPaging(1,1);" />
                        <asp:Button ID="btnCreateNew" runat="server" TabIndex="4" Text="New Freequency" CssClass="TransactionalButton btn btn-primary btn-sm"
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

            <table id="tblFreequency" class="table table-bordered table-condensed table-responsive">
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
                    <label class="control-label required-field">Freequency</label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtFreequency" runat="server" CssClass="form-control quantity" TabIndex="1"></asp:TextBox>
                </div>
            </div>
        </div>

        <div class="row" style="padding-bottom: 0; padding-top: 10px;">
            <div class="col-md-12">
                <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="SaveFreequency()" value="Save" />
                <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="javascript: return PerformClearAction();" />
            </div>
        </div>
    </div>
</asp:Content>
