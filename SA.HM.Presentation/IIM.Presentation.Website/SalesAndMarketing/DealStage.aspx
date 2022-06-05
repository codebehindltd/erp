<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="DealStage.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.DealStage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var isContinueSave;
        var StageTable;
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;
        $(document).ready(function () {

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;

            isContinueSave = false;

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            CommonHelper.ApplyDecimalValidation();
            StageTable = $("#tblSatge").DataTable({
                data: [],
                columns: [
                    { title: "Deal Stage", "data": "DealStage", sWidth: '20%' },
                    { title: "Forcast Type", "data": "ForcastType", sWidth: '20%' },
                    { title: "Forcast Category", "data": "ForcastCategory", sWidth: '20%' },
                    { title: "Complete", "data": "Complete", sWidth: '15%' },
                    { title: "Display Sequence", "data": "DisplaySequence", sWidth: '15%' },
                    { title: "Action", "data": null, sWidth: '10%' },
                    { title: "", "data": "Id", visible: false }
                ],

                columnDefs: [
                    {
                        //"targets": 3,
                        //"className": "text-center",
                        //"render": function (data, type, full, meta) {
                        //    return (data == true ? "Active" : "Inactive");
                        // }
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
                        row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return EditStage('" + aData.Id + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";
                    if (IsCanDelete)
                        row += "&nbsp;&nbsp;<a href='javascript:void();' onclick= \"DeleteStage('" + aData.Id + "');\"> <img alt='Delete' src='../Images/delete.png' title='Delete' /></a>";

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
            $('#ContentPlaceHolder1_cbSiteSurvey').change(function () {
                if ($(this).is(":checked")) {
                    $(this).attr("disabled", false);
                    $('#ContentPlaceHolder1_cbQuotationReveiw').prop("checked", false).attr("disabled", true);
                    $('#ContentPlaceHolder1_cbCloseWon').prop("checked", false).attr("disabled", true);
                    $('#ContentPlaceHolder1_cbCloseLost').prop("checked", false).attr("disabled", true);
                }
                else {
                    $('#ContentPlaceHolder1_cbQuotationReveiw').attr("disabled", false);
                    $('#ContentPlaceHolder1_cbCloseWon').attr("disabled", false);
                    $('#ContentPlaceHolder1_cbCloseLost').attr("disabled", false);

                }
            });
            $('#ContentPlaceHolder1_cbQuotationReveiw').change(function () {
                if ($(this).is(":checked")) {
                    $(this).attr("disabled", false);
                    $('#ContentPlaceHolder1_cbSiteSurvey').prop("checked", false).attr("disabled", true);
                    $('#ContentPlaceHolder1_cbCloseWon').prop("checked", false).attr("disabled", true);
                    $('#ContentPlaceHolder1_cbCloseLost').prop("checked", false).attr("disabled", true);
                }
                else {
                    $('#ContentPlaceHolder1_cbCloseWon').attr("disabled", false);
                    $('#ContentPlaceHolder1_cbCloseLost').attr("disabled", false);
                    $('#ContentPlaceHolder1_cbSiteSurvey').attr("disabled", false);
                }
            });
            $('#ContentPlaceHolder1_cbCloseWon').change(function () {
                if ($(this).is(":checked")) {
                    $(this).attr("disabled", false);
                    $('#ContentPlaceHolder1_cbQuotationReveiw').prop("checked", false).attr("disabled", true);
                    $('#ContentPlaceHolder1_cbCloseLost').prop("checked", false).attr("disabled", true);
                    $('#ContentPlaceHolder1_cbSiteSurvey').prop("checked", false).attr("disabled", true);
                }
                else {
                    $('#ContentPlaceHolder1_cbQuotationReveiw').attr("disabled", false);
                    $('#ContentPlaceHolder1_cbCloseLost').attr("disabled", false);
                    $('#ContentPlaceHolder1_cbSiteSurvey').attr("disabled", false);
                }
            });
            $('#ContentPlaceHolder1_cbCloseLost').change(function () {
                if ($(this).is(":checked")) {
                    $(this).attr("disabled", false);
                    $('#ContentPlaceHolder1_cbQuotationReveiw').prop("checked", false).attr("disabled", true);
                    $('#ContentPlaceHolder1_cbCloseWon').prop("checked", false).attr("disabled", true);
                    $('#ContentPlaceHolder1_cbSiteSurvey').prop("checked", false).attr("disabled", true);
                }
                else {
                    $('#ContentPlaceHolder1_cbQuotationReveiw').attr("disabled", false);
                    $('#ContentPlaceHolder1_cbCloseWon').attr("disabled", false);
                    $('#ContentPlaceHolder1_cbSiteSurvey').attr("disabled", false);
                }
            });

            GridPaging(1, 1);
        });
        function CreateNew() {
            PerformClearAction();
            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '95%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Create Deal Stage",
                show: 'slide'
            });
            //$("#ContentPlaceHolder1_ddlForcastType").val("Deal");
            return false;
        }

        function SaveOrUpdateStage() {
            var DealStage = $("#ContentPlaceHolder1_txtDealStage").val();
            if (DealStage == "") {
                toastr.warning("Enter Deal Stage");
                $("#ContentPlaceHolder1_txtDealStage").focus();
                return false;
            }
            var Complete = $("#ContentPlaceHolder1_txtComplete").val();
            if (Complete == "") {
                toastr.warning("Enter % Complete");
                $("#ContentPlaceHolder1_txtComplete").focus();
                return false;
            }
            if (parseFloat(Complete) > 100) {
                toastr.warning("Completed Percent Cann't be More Than 100");
                $("#ContentPlaceHolder1_txtComplete").focus();
                return false;
            }
            var sequence = $("#ContentPlaceHolder1_txtDisplaySequence").val();
            if (sequence == "") {
                toastr.warning("Enter Display Sequence");
                $("#ContentPlaceHolder1_txtDisplaySequence").focus();
                return false;
            }

            var id = $("#ContentPlaceHolder1_hfStageId").val();

            if (id == 0) {
                var IsUpdate = 0;
            }
            else {
                IsUpdate = 1;
            }
            PageMethods.DuplicateCheckDynamicaly("DealStage", DealStage, IsUpdate, id, DuplicateCheckDynamicalySucceed, DuplicateCheckDynamicalyFailed);
            return false;

        }
        function DuplicateCheckDynamicalySucceed(result) {
            id = $("#ContentPlaceHolder1_hfStageId").val();

            if (id == 0) {
                var IsUpdate = 0;
            }
            else {
                IsUpdate = 1;
            }

            if (result > 0) {
                toastr.warning("Duplicate Deal Stage");
                $("#ContentPlaceHolder1_txtDealStage").focus();
                return false;
            }
            else {
                sequence = +$("#ContentPlaceHolder1_txtDisplaySequence").val();
                PageMethods.DuplicateCheckDynamicaly("DisplaySequence", sequence, IsUpdate, id, DuplicateCheckSequenceSucceed, DuplicateCheckSequenceFailed);
                return false;

            }
            function DuplicateCheckSequenceSucceed(result) {
                if (result > 0) {
                    toastr.warning("Duplicate Display Sequence");
                    $("#ContentPlaceHolder1_txtDisplaySequence").focus();
                    return false;
                }
                id = $("#ContentPlaceHolder1_hfStageId").val();
                DealStage = $("#ContentPlaceHolder1_txtDealStage").val();
                Complete = $("#ContentPlaceHolder1_txtComplete").val();
                sequence = +$("#ContentPlaceHolder1_txtDisplaySequence").val();
                ForcastType = $("#ContentPlaceHolder1_ddlForcastType").val();

                ForcastCategory = $("#ContentPlaceHolder1_ddlForcastCategory").val();
                Description = $("#ContentPlaceHolder1_txtDescription").val();
                if (CheckDuplicateForcastType(ForcastType, id)) {
                    toastr.warning("" + ForcastType + " already exists");
                    $("#ContentPlaceHolder1_ddlForcastType").focus();
                    return false;
                }

                IsSiteSurvey = $("#ContentPlaceHolder1_cbSiteSurvey").is(":checked");
                IsQuotationReveiw = $("#ContentPlaceHolder1_cbQuotationReveiw").is(":checked");
                IsCloseWon = $("#ContentPlaceHolder1_cbCloseWon").is(":checked");
                IsCloseLost = $("#ContentPlaceHolder1_cbCloseLost").is(":checked");

                var DealStage = {
                    Id: id,
                    DealStage: DealStage,
                    Complete: Complete,
                    ForcastType: ForcastType,
                    ForcastCategory: ForcastCategory,
                    DisplaySequence: sequence,
                    Description: Description,
                    IsSiteSurvey: IsSiteSurvey,
                    IsQuotationReveiw: IsQuotationReveiw,
                    IsCloseWon: IsCloseWon,
                    IsCloseLost: IsCloseLost
                }
                PageMethods.SaveOrUpdateStage(DealStage, OnSuccessSaveOrUpdate, OnFailSaveOrUpdate);
            }
        }
        function DuplicateCheckSequenceFailed(error) {

        }

        function DuplicateCheckDynamicalyFailed(error) {

        }
        function OnSuccessSaveOrUpdate(result) {

            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                if (!isContinueSave) {
                    $("#CreateNewDialog").dialog("close");

                }
                GridPaging(1, 1);
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            isContinueSave = false;
            return false;
        }

        function OnFailSaveOrUpdate(error) {
            toastr.error(error.get_message());
        }

        function EditStage(id) {
            PerformClearAction();
            FillForm(id);
            return false;
        }
        function FillForm(id) {
            PageMethods.GetStageById(id, OnSuccessLoad, OnFailLoad)
            return false;
        }

        function OnSuccessLoad(result) {
            if (!confirm("Do you want to edit - " + result.DealStage + "?")) {
                return false;
            }
            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '95%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Update Deal Stage - " + result.DealStage,
                show: 'slide'
            });
            $("#ContentPlaceHolder1_hfStageId").val(result.Id);
            $("#ContentPlaceHolder1_txtDealStage").val(result.DealStage);
            $("#ContentPlaceHolder1_txtComplete").val(result.Complete);
            $("#ContentPlaceHolder1_ddlForcastType").val(result.ForcastType);
            $("#ContentPlaceHolder1_ddlForcastCategory").val(result.ForcastCategory);
            $("#ContentPlaceHolder1_txtDisplaySequence").val(result.DisplaySequence);
            $("#ContentPlaceHolder1_txtDescription").val(result.Description);

            if (result.IsCloseLost)
                $('#ContentPlaceHolder1_cbCloseLost').prop("checked", true).trigger('change');
            if (result.IsCloseWon)
                $('#ContentPlaceHolder1_cbCloseWon').prop("checked", true).trigger('change');
            if (result.IsQuotationReveiw)
                $('#ContentPlaceHolder1_cbQuotationReveiw').prop("checked", true).trigger('change');
            if (result.IsSiteSurvey)
                $('#ContentPlaceHolder1_cbSiteSurvey').prop("checked", true).trigger('change');


            $("#btnSave").val("Update & Close");
            $("#btnSaveNContinue").hide();
            $("#btnClear").hide();
            return false;
        }

        function OnFailLoad(error) {
            toastr.error(error.get_message());
        }

        function PerformClearAction() {
            $("#ContentPlaceHolder1_hfStageId").val("0");
            $("#ContentPlaceHolder1_txtDealStage").val("");
            $("#ContentPlaceHolder1_txtComplete").val("");
            $("#ContentPlaceHolder1_ddlForcastType").val("Open");
            $("#ContentPlaceHolder1_ddlForcastCategory").val("Pipeline");
            $("#ContentPlaceHolder1_txtDisplaySequence").val("");
            $("#ContentPlaceHolder1_txtDescription").val("");
            $('#ContentPlaceHolder1_cbCloseLost').prop("checked", false).attr("disabled", false);
            $('#ContentPlaceHolder1_cbCloseWon').prop("checked", false).attr("disabled", false);
            $('#ContentPlaceHolder1_cbQuotationReveiw').prop("checked", false).attr("disabled", false);
            $('#ContentPlaceHolder1_cbSiteSurvey').prop("checked", false).attr("disabled", false);
            $("#btnSave").val("Save & Close");
            $("#btnSaveNContinue").show();
            $("#btnClear").show();
            return false;
        }
        function SaveNContinue() {
            isContinueSave = true;
            SaveOrUpdateStage();
            return false;
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadGrid(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function LoadGrid(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#SourceTable tbody tr").length;
            var name = $("#ContentPlaceHolder1_txtSearchName").val();
            PageMethods.SearchStage(name, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchucceed, OnSearchFailed);
            return false;
        }
        //function SearchStage() {
        //    var name = $("#ContentPlaceHolder1_txtSearchName").val();

        //    PageMethods.SearchStage(name, OnSearchucceed, OnSearchFailed);
        //    return false;
        //}

        function OnSearchucceed(result) {
            $("#GridPagingContainer ul").empty();
            StageTable.clear();
            StageTable.rows.add(result.GridData);
            StageTable.draw();
            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
            return false;
        }

        function OnSearchFailed(error) {
            toastr.error(error.get_message());
            return false;
        }

        function DeleteStage(id) {
            if (confirm("Want to delete?")) {
                PageMethods.DeleteStage(id, OnSuccessDelete, OnFailedDelete);
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
        function CheckDuplicateForcastType(forcastType, id) {
            if (forcastType == "Closed Won" || forcastType == "Closed Lost") {
                var duplicateType = false;
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../SalesAndMarketing/DealStage.aspx/DuplicateTypeCheck',

                    data: JSON.stringify({ forcastType: forcastType, id: id }),
                    async: false,
                    dataType: "json",
                    success: function (data) {
                        duplicateType = data.d;
                    },
                    error: function (result) {

                    }
                });
                return duplicateType;
            }
            else
                return false;

        }
    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfStageId" Value="0" runat="server" />
    <div class="panel panel-default">
        <div class="panel-heading">
            Deal Stage Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Deal Stage</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return GridPaging(1,1);" />
                        <asp:Button ID="btnCreateNew" runat="server" TabIndex="4" Text="New Deal Stage" CssClass="TransactionalButton btn btn-primary btn-sm"
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

            <table id="tblSatge" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
            </table>
            <div class="childDivSection">
                <div class="text-center" id="GridPagingContainer">
                    <ul class="pagination">
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div id="CreateNewDialog" style="display: none; overflow: unset" class="panel panel-default">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label required-field">Deal Stage</label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtDealStage" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label required-field">% Complete</label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtComplete" runat="server" CssClass="quantity form-control" TabIndex="2"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <label class="control-label required-field">Forcast Type</label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlForcastType" runat="server" CssClass="form-control" TabIndex="3">
                        <asp:ListItem Value="Open">Open</asp:ListItem>
                        <asp:ListItem Value="Closed Won">Closed Won </asp:ListItem>
                        <asp:ListItem Value="Closed Lost">Closed Lost </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label required-field">Forcast Category</label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlForcastCategory" runat="server" CssClass="form-control" TabIndex="4">
                        <asp:ListItem Value="Pipeline">Pipeline</asp:ListItem>
                        <asp:ListItem Value="Closed">Closed</asp:ListItem>
                        <asp:ListItem Value="Lost">Lost </asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <label class="control-label required-field">Display Sequence</label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtDisplaySequence" runat="server" CssClass="quantity form-control" TabIndex="5"></asp:TextBox>
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
                <div class="col-md-2"></div>
                <div class="col-md-8">
                    <div class="col-md-12">
                        <asp:CheckBox ID="cbSiteSurvey" runat="server" />
                        It's site survey Stage
                    </div>

                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2"></div>
                <div class="col-md-8">
                    <div class="col-md-12">
                        <asp:CheckBox ID="cbQuotationReveiw" runat="server" />
                        It's Quotation/Quotation Reveiw Stage
                    </div>

                </div>
            </div>
            <%--<div class="form-group">
                <div class="col-md-2"></div>
                <div class="col-md-8">
                    <div class="col-md-12">
                        <asp:CheckBox ID="cbCloseWon" runat="server" />
                        It's Close Won Stage
                    </div>
                </div>
            </div>--%>
            <%-- <div class="form-group">
                <div class="col-md-2"></div>
                <div class="col-md-8">
                    <div class="col-md-12">
                        <asp:CheckBox ID="cbCloseLost" runat="server" />
                        It's Close Lost Stage
                    </div>
                </div>
            </div>--%>
            <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                <div class="col-md-12">
                    <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="SaveOrUpdateStage()" value="Save & Close" />
                    <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return PerformClearAction();" />
                    <input id="btnSaveNContinue" type="button" value="Save & Continue" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return SaveNContinue();" />
                </div>
            </div>
        </div>


    </div>
</asp:Content>
