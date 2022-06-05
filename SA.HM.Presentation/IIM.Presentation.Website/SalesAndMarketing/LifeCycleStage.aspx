<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="LifeCycleStage.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.LifeCycleStage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var flag;
        var duplicateType = false;

        $(document).ready(function () {
            CommonHelper.ApplyIntigerValidation();
            falg = 0;
            $("#ContentPlaceHolder1_ddlIsRelatedToDeal").change(function () {
                if (this.value == "Yes") {
                    $("#ContentPlaceHolder1_ddlDealType").show();
                    $("#ContentPlaceHolder1_ddlForcastType").show();
                }
                else {
                    $("#ContentPlaceHolder1_ddlDealType").val("0").hide();
                    $("#ContentPlaceHolder1_ddlForcastType").val("0").hide();
                }
            });
            LoadGrid(1, 1);
            $("#ContentPlaceHolder1_txtLifeCycleStage").focus();
        });
        function AddNewStatus() {
            PerformClearAction();
            $("#AddNewLifeCycleContaiiner").dialog({
                autoOpen: true,
                modal: true,
                width: '95%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Create Life Cycle Stage",
                hide: 'fold',
                show: 'slide',
                close: function (event, ui) {


                }
            });
            CommonHelper.SpinnerClose();
            return false;
        }
        function PerformSave() {

            var id = $("#ContentPlaceHolder1_hfId").val()
            var lifeCycleStage = $("#ContentPlaceHolder1_txtLifeCycleStage").val();
            if (id == 0) {
                var IsUpdate = 0;
            }
            else {
                IsUpdate = 1;
            }
            var isRelatedToDeal = $("#ContentPlaceHolder1_ddlIsRelatedToDeal").val() == "Yes" ? true : false;
            var dealType = $("#ContentPlaceHolder1_ddlDealType").val();
            var forcastType = $("#ContentPlaceHolder1_ddlForcastType").val();
            if (lifeCycleStage == "") {
                toastr.warning("Enter life Cycle Stage");
                $("#ContentPlaceHolder1_txtLifeCycleStage").focus();
                return false;
            }
            if (isRelatedToDeal == true) {
                if (dealType == "0") {
                    toastr.warning("Please Select Deal Type");
                    $("#ContentPlaceHolder1_ddlDealType").focus();
                    return false;
                }
                else if (forcastType == "0") {
                    toastr.warning("Please Select Forcast Type");
                    $("#ContentPlaceHolder1_txtLifeCycleStage").focus();
                    return false;
                }
            }
            PageMethods.DuplicateCheckDynamicaly("LifeCycleStage", lifeCycleStage, IsUpdate, id, DuplicateCheckDynamicalySucceed, DuplicateCheckDynamicalyFailed);
            return false;

        }
        function DuplicateCheckDynamicalySucceed(result) {

            if (result > 0) {
                toastr.warning("Duplicate Life Cycle Stage");
                $("#ContentPlaceHolder1_txtTypeName").focus();
                return false;
            }
            else {
                id = $("#ContentPlaceHolder1_hfId").val()
                if (id == 0) {
                    var IsUpdate = 0;
                }
                else {
                    IsUpdate = 1;
                }
                displaySequence = $("#ContentPlaceHolder1_txtDisplaySequence").val();
                PageMethods.DuplicateCheckDynamicaly("DisplaySequence", displaySequence, IsUpdate, id, DuplicateCheckSequenceSucceed, DuplicateCheckSequenceFailed);
                return false;
            }
        }
        function DuplicateCheckSequenceSucceed(result) {
            if (result > 0) {
                toastr.warning("Duplicate Display Sequence");
                $("#ContentPlaceHolder1_txtDisplaySequence").focus();
                return false;
            }
            else {
                id = $("#ContentPlaceHolder1_hfId").val()
                lifeCycleStage = $("#ContentPlaceHolder1_txtLifeCycleStage").val();
                displaySequence = $("#ContentPlaceHolder1_txtDisplaySequence").val();
                description = $("#ContentPlaceHolder1_txtDescription").val();
                isRelatedToDeal = $("#ContentPlaceHolder1_ddlIsRelatedToDeal").val() == "Yes" ? true : false;
                dealType = $("#ContentPlaceHolder1_ddlDealType").val();
                forcastType = $("#ContentPlaceHolder1_ddlForcastType").val();

                var SMLifeCycleStageBO = {
                    Id: id,
                    LifeCycleStage: lifeCycleStage,
                    DisplaySequence: displaySequence,
                    Description: description,
                    IsRelatedToDeal: isRelatedToDeal,
                    DealType: dealType,
                    ForcastType: forcastType,
                }

                DuplicateTypeCheck(SMLifeCycleStageBO);

                if (duplicateType == true) {

                    toastr.warning("Deal Stage already exists for " + dealType + " and " + forcastType + "");
                    $("#ContentPlaceHolder1_ddlDealType").focus();
                    $("#ContentPlaceHolder1_ddlForcastType").focus();
                    duplicateType = false;
                    return false;
                }
                //PageMethods.DuplicateTypeCheck(SMLifeCycleStageBO, DuplicateTypeCheckSucceed, DuplicateTypeCheckFailed);
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../SalesAndMarketing/LifeCycleStage.aspx/SaveUpdateDealStatus',

                    data: JSON.stringify({ SMLifeCycleStageBO: SMLifeCycleStageBO }),
                    async: false,
                    dataType: "json",
                    success: function (data) {
                        LoadGrid(1, 1);
                        PerformClearAction();
                        CommonHelper.AlertMessage(data.d.AlertMessage);
                        $("#ContentPlaceHolder1_txtLifeCycleStage").focus();
                        if (flag == 1) {
                            $('#AddNewLifeCycleContaiiner').dialog('close');
                        }
                        flag = 0;
                    },
                    error: function (result) {

                    }
                });
            }
        }
        function DuplicateCheckSequenceFailed(error) {

        }
        function DuplicateTypeCheck(SMLifeCycleStageBO) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/LifeCycleStage.aspx/DuplicateTypeCheck',

                data: JSON.stringify({ sMLifeCycleStage: SMLifeCycleStageBO }),
                async: false,
                dataType: "json",
                success: function (data) {
                    duplicateType = data.d;

                },
                error: function (result) {

                }
            });
        }


        function DuplicateCheckDynamicalyFailed(error) {
            CommonHelper.AlertMessage(error.AlertMessage);
            return false;
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadGrid(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function LoadGrid(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#StatusTable tbody tr").length;
            var lifeCycleStage = $("#ContentPlaceHolder1_txtLifeCycleStageForSearch").val();

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/LifeCycleStage.aspx/LoadCompanyStatusSearch',

                data: "{'lifeCycleStage':'" + lifeCycleStage + "', 'gridRecordsCount':'" + gridRecordsCount + "', 'pageNumber':'" + pageNumber + "', 'isCurrentOrPreviousPage':'" + IsCurrentOrPreviousPage + "'}",
                dataType: "json",
                success: function (data) {
                    LoadTable(data);
                },
                error: function (result) {
                    PerformClearAction();
                    CommonHelper.AlertMessage(result.d.AlertMessage);
                }
            });
            return false;
        }
        function LoadTable(data) {

            $("#StatusTable tbody").empty();
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

                tr += "<td style='width:20%;'>" + gridObject.LifeCycleStage + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.DealType + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.ForcastType + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.DisplaySequence + "</td>";

                tr += "<td style='width:20%;'> <a onclick=\"javascript:return FillFormEdit(" + gridObject.Id + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'DeleteStatus(" + gridObject.Id + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";

                tr += "<td style='display:none'>" + gridObject.Id + "</td>";


                tr += "</tr>";

                $("#StatusTable tbody").append(tr);

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
                url: '../SalesAndMarketing/LifeCycleStage.aspx/FillForm',

                data: "{'Id':'" + Id + "'}",
                dataType: "json",
                success: function (data) {
                    if (!confirm("Do you want to edit - " + data.d.LifeCycleStage + "?")) {
                        return false;
                    }
                    AddNewStatus();
                    $("#ContentPlaceHolder1_btnSaveClose").val("Update And Close");
                    $("#ContentPlaceHolder1_btnClear").hide();
                    $("#ContentPlaceHolder1_btnSaveContinue").hide();
                    $("#AddNewLifeCycleContaiiner").dialog({ title: "Upadte Life Cycle Stage - " + data.d.LifeCycleStage + " " });
                    $("#ContentPlaceHolder1_hfId").val(data.d.Id)
                    $("#ContentPlaceHolder1_txtLifeCycleStage").val(data.d.LifeCycleStage);
                    $("#ContentPlaceHolder1_txtDisplaySequence").val(data.d.DisplaySequence);
                    $("#ContentPlaceHolder1_txtDescription").val(data.d.Description);
                    var IsRelatedToDeal = data.d.IsRelatedToDeal == true ? "Yes" : "No"
                    $("#ContentPlaceHolder1_ddlIsRelatedToDeal").val(IsRelatedToDeal).trigger('change');
                    var DealType = data.d.DealType == null ? "0" : data.d.DealType
                    $("#ContentPlaceHolder1_ddlDealType").val(DealType);
                    var ForcastType = data.d.ForcastType == null ? "0" : data.d.ForcastType
                    $("#ContentPlaceHolder1_ddlForcastType").val(ForcastType);


                    CommonHelper.SpinnerClose();
                },
                error: function (result) {
                    CommonHelper.AlertMessage(result.d.AlertMessage);
                }
            });
            return false;

        }
        function DeleteStatus(Id) {
            if (!confirm("Do you want to Delete?")) {
                return false;
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/LifeCycleStage.aspx/DeleteStatus',

                data: "{'Id':'" + Id + "'}",
                dataType: "json",
                success: function (data) {
                    CommonHelper.AlertMessage(data.d.AlertMessage);
                    LoadGrid(1, 1);
                },
                error: function (result) {
                    CommonHelper.AlertMessage(result.d.AlertMessage);
                }
            });
            return false;
        }
        function PerformClearAction() {

            $("#ContentPlaceHolder1_btnSaveClose").val("Save And Close");
            $("#ContentPlaceHolder1_btnSaveContinue").val("Save And Continue");
            $("#ContentPlaceHolder1_btnClear").show();
            $("#ContentPlaceHolder1_btnSaveContinue").show();

            $("#ContentPlaceHolder1_hfId").val("0")
            $("#ContentPlaceHolder1_txtLifeCycleStage").val("");
            $("#ContentPlaceHolder1_txtDisplaySequence").val("");
            $("#ContentPlaceHolder1_txtDescription").val("");
            $("#ContentPlaceHolder1_ddlIsRelatedToDeal").val("No").trigger('change');
            $("#ContentPlaceHolder1_ddlDealType").val("0");
            $("#ContentPlaceHolder1_ddlForcastType").val("0");
            return false;
        }
        function PerformCancleAction() {
            $("#ContentPlaceHolder1_txtLifeCycleStageForSearch").val("");
            return false;
        }
        function SaveAndClose() {
            flag = 1;
            PerformSave();
            return false;
        }
        function SaveAndContinue() {
            PerformSave();
        }
    </script>
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>

    <div id="AddNewLifeCycleContaiiner" style="display: none;">
        <div id="AddPanel" class="panel panel-default">
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblLifeCycleStage" runat="server" class="control-label required-field" Text="Life Cycle Stage"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtLifeCycleStage" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblDisplaySequence" runat="server" class="control-label" Text="Display Sequence"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDisplaySequence" CssClass="quantity form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblDescription" runat="server" class="control-label " Text="Description"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtDescription" CssClass="form-control" TextMode="MultiLine" Rows="4" runat="server" Style="resize: none;"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="lblDealRelation" runat="server" class="control-label" Style="float: left" Text="Is this stage related to Deal?"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlIsRelatedToDeal" runat="server" CssClass="form-control" TabIndex="4">
                            <asp:ListItem Value="No">No</asp:ListItem>
                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlDealType" runat="server" CssClass="form-control" Style="display: none; text-align: center;">
                            <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                            <asp:ListItem Value="New Business">New Business</asp:ListItem>
                            <asp:ListItem Value="Existing Business">Existing Business</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlForcastType" runat="server" CssClass="form-control" Style="display: none; text-align: center;">
                            <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                            <asp:ListItem Value="Open">Open</asp:ListItem>
                            <asp:ListItem Value="Closed Won">Closed Won</asp:ListItem>
                            <asp:ListItem Value="Closed Lost">Closed Lost</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSaveClose" runat="server" Text="Save And Close" OnClientClick="javascript:return SaveAndClose();"
                            CssClass="TransactionalButton btn btn-primary btn-sm" />
                        <asp:Button ID="btnSaveContinue" runat="server" Text="Save And Continue" OnClientClick="javascript:return SaveAndContinue();"
                            CssClass="TransactionalButton btn btn-primary btn-sm" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return PerformClearAction();" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="InfoPanel" class="panel panel-default">
        <div class="panel-heading">
            Life Cycle Stage Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Life Cycle Stage</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtLifeCycleStageForSearch" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>

                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="javascript: return LoadGrid(1,1);"
                            CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />
                        <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return PerformCancleAction();" TabIndex="6" />
                        <asp:Button ID="btnAdd" runat="server" Text="New Life Cycle Stage" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return AddNewStatus();" TabIndex="6" />
                    </div>
                </div>
            </div>
        </div>
        <div id="SearchPanel" class="panel panel-default">
            <div class="panel-heading">
                Search Information
            </div>
            <div class="panel-body">
                <div class="form-group" id="StatusTableContainer" style="overflow: scroll;">
                    <table class="table table-bordered table-condensed table-responsive" id="StatusTable"
                        style="width: 100%;">
                        <thead>
                            <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                <th style="width: 20%;">Life Cycle Stage
                                </th>
                                <th style="width: 20%;">Deal Type
                                </th>
                                <th style="width: 20%;">Forcast Type
                                </th>
                                <th style="width: 20%;">Display Sequence
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
    </div>
</asp:Content>
