<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="TermsNConditionsSetup.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.TermsNConditionsSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var ConditionForTable = "";
        var ConditionForList = [];
        var flag = 0;
        $(document).ready(function () {

            PopulateProjects();
            CommonHelper.ApplyIntigerValidation();
            $("[id=chkAll]").on("change", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#TblConditionFor tbody tr").find("td:eq(0)").find("input").prop("checked", true);
                }
                else {
                    $("#TblConditionFor tbody tr").find("td:eq(0) ").find("input").prop("checked", false);
                }
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
                title: "Terms And Conditions",
                show: 'slide'
            });

            return false;
        }
        function PopulateProjects() {
            $.ajax({
                type: "POST",
                url: "../HMCommon/TermsNConditionsSetup.aspx/GetAllConditionFor",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    PopulateConditionFor(response.d);

                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
        }
        function PopulateConditionFor(data) {
            $("#TblConditionFor tbody").empty();
            i = 0;

            $.each(data, function (count, gridObject) {

                var tr = "";

                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:10%;' align='center'><input id='" + gridObject.FieldId + "' type='checkbox'> </td>";
                tr += "<td style='width:90%;'>" + gridObject.FieldValue + "</td>";
                tr += "<td style='display:none'>" + gridObject.FieldId + "</td>";


                tr += "</tr>";

                $("#TblConditionFor tbody").append(tr);

                tr = "";
                i++;
            });
            return false;
        }
        function PerformClearAction() {
            $("#ContentPlaceHolder1_hfId").val("0")
            $("#ContentPlaceHolder1_txtTitle").val("");
            $("#ContentPlaceHolder1_txtDescription").val("");
            $("#<%=txtDisplaySequence.ClientID%>").val("");
            $("#btnClear").show();
            $("#btnSave").val("Save & Close");
            $("#btnSaveNContinue").val("Save & Continue").show();
            $("#TblConditionFor tbody tr").find("td:eq(0) ").find("input").prop("checked", false);
            $("#chkAll").prop("checked", false);
        }
        function SaveAndClose() {
            flag = 1;
            SaveOrUpdateTermsNConditions();
            return false;

        }
        function SaveOrUpdateTermsNConditions() {
            var IsUpdate = 0;
            var id = $("#ContentPlaceHolder1_hfId").val()
            var title = $("#<%=txtTitle.ClientID%>").val();
            if (id == 0) {
                var IsUpdate = 0;
            }
            else {
                IsUpdate = 1;
            } if (title == "") {
                toastr.warning("Enter Title");
                $("#<%=txtTitle.ClientID%>").focus();
                return false;
            }
            PageMethods.DuplicateCheckDynamicaly("Title", title, IsUpdate, id, DuplicateCheckDynamicalySucceed, DuplicateCheckDynamicalyFailed);
            return false;
        }
        function DuplicateCheckDynamicalySucceed(result) {
            id = $("#ContentPlaceHolder1_hfId").val()
            var title = $("#<%=txtTitle.ClientID%>").val();
            var description = $("#ContentPlaceHolder1_txtDescription").val();
            var displaySequence = $("#<%=txtDisplaySequence.ClientID%>").val();
            ConditionForList = [];
            $("#TblConditionFor tbody tr").each(function () {
                if ($(this).find("td:eq(0)").find("input").is(":checked")) {
                    var conditionFor = $.trim($(this).find("td:eq(2)").text());
                    ConditionForList.push(conditionFor);
                }
            });
            //$("#TblConditionFor tbody tr").find("td:eq(0)").find("input:checked").each(function (index, row) {
            //    var conditionFor = $.trim($(row).find("td:eq(1)").text());
            //    ConditionForList.push(conditionFor);
            //});
            $("#ContentPlaceHolder1_hfConditionForList").val(ConditionForList.toString());
            var ConditionForList = $("#ContentPlaceHolder1_hfConditionForList").val();
            if (result > 0) {
                toastr.warning("Duplicate Title");
                $("#<%=txtTitle.ClientID%>").focus();
                return false;
            }
            else {
                var TermsNConditionsMasterBO = {
                    Id: id,
                    Title: title,
                    Description: description,
                    DisplaySequence: displaySequence,
                }
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../HMCommon/TermsNConditionsSetup.aspx/SaveOrUpdateTermsNConditions',

                    data: JSON.stringify({ TermsNConditionsMasterBO: TermsNConditionsMasterBO, ConditionForList: ConditionForList }),
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

            $("#<%=txtTitle.ClientID%>").focus();
        }
        function DuplicateCheckDynamicalyFailed(error) {

        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadGrid(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function LoadGrid(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#TermsNConditionsTable tbody tr").length;
            var title = $("#ContentPlaceHolder1_txtTitleForSearch").val();
            var status = $("#ContentPlaceHolder1_ddlStatusForSearch").val() == "Active" ? true : false;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../HMCommon/TermsNConditionsSetup.aspx/LoadTermsNConditionsSearch',

                data: "{'Title':'" + title + "', 'gridRecordsCount':'" + gridRecordsCount + "', 'pageNumber':'" + pageNumber + "', 'isCurrentOrPreviousPage':'" + IsCurrentOrPreviousPage + "'}",
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

            $("#TermsNConditionsTable tbody").empty();
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

                tr += "<td style='width:15%;'>" + gridObject.Title + "</td>";
                tr += "<td style='width:75%;'>" + gridObject.Description + "</td>";

                tr += "<td style='width:10%;'> <a onclick=\"javascript:return FillFormEdit(" + gridObject.Id + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'DeleteAction(" + gridObject.Id + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";

                tr += "<td style='display:none'>" + gridObject.Id + "</td>";


                tr += "</tr>";

                $("#TermsNConditionsTable tbody").append(tr);

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
                url: '../HMCommon/TermsNConditionsSetup.aspx/FillForm',

                data: "{'Id':'" + Id + "'}",
                dataType: "json",
                success: function (data) {
                    if (!confirm("Do you want to edit - " + data.d.Title + "?")) {
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
                        title: "Update Terms And Conditions - " + data.d.Title,
                        show: 'slide'
                    });
                    $("#btnSave").val("Update And Close");
                    $("#btnClear").hide();
                    $("#btnSaveNContinue").hide();
                    // $("#AddNewStatusContaiiner").dialog({ title: "Edit Source - " + data.d.SourceName + " " });
                    $("#ContentPlaceHolder1_hfId").val(data.d.Id)
                    $("#<%=txtTitle.ClientID%>").val(data.d.Title);
                    $("#ContentPlaceHolder1_txtDescription").val(data.d.Description);
                    //var status = data.d.Status == true ? "Active" : "Inactive"
                    $("#<%=txtDisplaySequence.ClientID%>").val(data.d.DisplaySequence);
                    $("#TblConditionFor tbody tr").each(function () {
                        var conditionFor = $.trim($(this).find("td:eq(2)").text());
                        var a = data.d.Details.includes(parseFloat(conditionFor));
                        if (data.d.Details.includes(parseFloat(conditionFor))) {
                            $(this).find("td:eq(0)").find("input").prop("checked", true);
                        }
                    });
                    CommonHelper.SpinnerClose();
                },
                error: function (result) {

                }
            });
            return false;
        }
        function DeleteAction(id) {
            if (!confirm("Do you want to Delete?")) {
                return false;
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../HMCommon/TermsNConditionsSetup.aspx/DeleteAction',
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
    </script>

    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfConditionForList" Value="0" runat="server" />

    <div class="panel panel-default">
        <div class="panel-heading">
            Terms And Conditions
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Title</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtTitleForSearch" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return GridPaging(1,1);" />
                        <asp:Button ID="btnClean" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return Clean();" />
                        <asp:Button ID="btnCreateNew" runat="server" Text="New Terms And Conditions" CssClass="TransactionalButton btn btn-primary btn-sm"
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
            <div class="form-group" id="TermsNConditionsTableContainer">
                <table class="table table-bordered table-condensed table-responsive" id="TermsNConditionsTable"
                    style="width: 100%;">
                    <thead>
                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                            <th style="width: 15%;">Title
                            </th>
                            <th style="width: 75%;">Discription
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
                    <label class="control-label required-field">Titel</label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <label class="control-label required-field">Display Sequence</label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtDisplaySequence" runat="server" CssClass="quantity form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label required-field">Description</label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" TabIndex="6" Style="resize: none;"></asp:TextBox>
                </div>
            </div>
            <div class="form-group" id="ConditionForTableContainer">
                <div class="col-md-2">
                    <label class="control-label">Condition For</label>
                </div>
                <div class="col-md-10">
                    <div style="height: 300px; overflow-y: auto; width: 100%">
                        <table class="table table-bordered table-condensed table-responsive" id="TblConditionFor"
                            style="width: 100%;">
                            <thead>
                                <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                    <th style="width: 10%; text-align: center; align-content: center">
                                        <input id='chkAll' type='checkbox'>
                                    </th>
                                    <th style="width: 90%; text-align: left">Condition For
                                    </th>
                                    <th style="display: none">Id
                                    </th>

                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                <div class="col-md-12">
                    <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="SaveAndClose()" value="Save & Close" />
                    <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return PerformClearAction();" />
                    <input id="btnSaveNContinue" type="button" value="Save & Continue" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return SaveOrUpdateTermsNConditions();" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
