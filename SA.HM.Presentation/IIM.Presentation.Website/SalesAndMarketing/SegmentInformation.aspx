<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="SegmentInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.SegmentInformation" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var flag = 0;
        $(document).ready(function () {
            $("#ContentPlaceHolder1_txtSegmentName").focus();
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
                title: "Segment Information",
                show: 'slide'
            });
            $("#ContentPlaceHolder1_txtSegmentName").focus();
            return false;
        }
        function PerformClearAction() {
            $("#ContentPlaceHolder1_hfId").val("0")
            $("#ContentPlaceHolder1_txtSegmentName").val("");
            $("#ContentPlaceHolder1_txtDescription").val("");
            $("#ContentPlaceHolder1_ddlStatus").val("Active");
            $("#btnClear").show();
            $("#btnSave").val("Save & Close");
            $("#btnSaveNContinue").val("Save & Continue").show();
        }
        function SaveOrUpdateSegment() {

            var id = $("#ContentPlaceHolder1_hfId").val()
            var segmentName = $("#ContentPlaceHolder1_txtSegmentName").val();
            if (id == 0) {
                var IsUpdate = 0;
            }
            else {
                IsUpdate = 1;
            }
            if (segmentName == "") {
                toastr.warning("Enter Segment Name");
                $("#ContentPlaceHolder1_txtSegmentName").focus();
                return false;
            }
            PageMethods.DuplicateCheckDynamicaly("SegmentName", segmentName, IsUpdate, id, DuplicateCheckDynamicalySucceed, DuplicateCheckDynamicalyFailed);
            return false;

        }
        function DuplicateCheckDynamicalySucceed(result) {
            id = $("#ContentPlaceHolder1_hfId").val()
            segmentName = $("#ContentPlaceHolder1_txtSegmentName").val();
            description = $("#ContentPlaceHolder1_txtDescription").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val() == "Active" ? true : false;
            if (result > 0) {
                toastr.warning("Duplicate Segment Name");
                $("#ContentPlaceHolder1_txtSegmentName").focus();
                return false;
            }
            else {
                var SMSegmentInformationBO = {
                    Id: id,
                    SegmentName: segmentName,
                    Description: description,
                    Status: status,
                }
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../SalesAndMarketing/SegmentInformation.aspx/SaveUpdateSegmentInformation',

                    data: JSON.stringify({ sMSegmentInformationBO: SMSegmentInformationBO }),
                    dataType: "json",
                    success: function (data) {
                        GridPaging(1, 1);
                        PerformClearAction();
                        CommonHelper.AlertMessage(data.d.AlertMessage);
                        if (flag == 1) {
                            $('#CreateNewDialog').dialog('close');
                        }
                        flag = 1;
                    },
                    error: function (result) {
                        CommonHelper.AlertMessage(result.d.AlertMessage);
                    }
                });
            }
        }
        function DuplicateCheckDynamicalyFailed(error) {
            CommonHelper.AlertMessage(error.AlertMessage);
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadGrid(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function LoadGrid(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#SegmentTable tbody tr").length;
            var segmentName = $("#ContentPlaceHolder1_txtSegmentNameForSearch").val();
            var status = $("#ContentPlaceHolder1_ddlStatusForSearch").val() == "Active" ? true : false;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/SegmentInformation.aspx/LoadSegmentNameSearch',

                data: "{'SegmentName':'" + segmentName + "','Status':'" + status + "', 'gridRecordsCount':'" + gridRecordsCount + "', 'pageNumber':'" + pageNumber + "', 'isCurrentOrPreviousPage':'" + IsCurrentOrPreviousPage + "'}",
                dataType: "json",
                success: function (data) {
                    LoadTable(data);
                },
                error: function (result) {
                    CommonHelper.AlertMessage(result.d.AlertMessage);
                    PerformClearAction();
                }
            });
            return false;
        }
        function LoadTable(data) {

            $("#SegmentTable tbody").empty();
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

                tr += "<td style='width:75%;'>" + gridObject.SegmentName + "</td>";

                tr += "<td style='width:25%;'> <a onclick=\"javascript:return FillFormEdit(" + gridObject.Id + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'DeleteSegment(" + gridObject.Id + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";

                tr += "<td style='display:none'>" + gridObject.Id + "</td>";


                tr += "</tr>";

                $("#SegmentTable tbody").append(tr);

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
                url: '../SalesAndMarketing/SegmentInformation.aspx/FillForm',

                data: "{'Id':'" + Id + "'}",
                dataType: "json",
                success: function (data) {
                    if (!confirm("Do you want to edit - " + data.d.SegmentName + "?")) {
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
                        title: "Update Segment - " + data.d.SegmentName,
                        show: 'slide'
                    });
                    $("#btnSave").val("Update And Close");
                    $("#btnClear").hide();
                    $("#btnSaveNContinue").hide();
                    //$("#AddNewStatusContaiiner").dialog('option', 'title', "Update"); //+ data.d.SegmentName + " " });
                    $("#ContentPlaceHolder1_hfId").val(data.d.Id)
                    $("#ContentPlaceHolder1_txtSegmentName").val(data.d.SegmentName);
                    $("#ContentPlaceHolder1_txtDescription").val(data.d.Description);
                    var status = data.d.Status == true ? "Active" : "Inactive"
                    $("#ContentPlaceHolder1_ddlStatus").val(status);

                    CommonHelper.SpinnerClose();
                },
                error: function (result) {
                    CommonHelper.AlertMessage(result.d.AlertMessage);
                }
            });
            return false;
        }
        function DeleteSegment(Id) {
            if (!confirm("Do you want to Delete?")) {
                return false;
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/SegmentInformation.aspx/DeleteSegment',

                data: "{'Id':'" + Id + "'}",
                dataType: "json",
                success: function (data) {
                    CommonHelper.AlertMessage(data.d.AlertMessage);
                    GridPaging(1, 1);
                },
                error: function (result) {
                    CommonHelper.AlertMessage(result.d.AlertMessage);
                }
            });
            return false;
        }
        function Clean() {
            $("#ContentPlaceHolder1_txtSegmentNameForSearch").val("");
            return false;
        }
        function SaveAndClose() {
            flag = 1;
            SaveOrUpdateSegment();
            return 0;
        }
    </script>
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>

    <div class="panel panel-default">
        <div class="panel-heading">
            Segment Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Segment Name</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSegmentNameForSearch" runat="server" CssClass="form-control"></asp:TextBox>
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
                        <asp:Button ID="btnCreateNew" runat="server" Text="New Segment" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateNew();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Segment Information
        </div>
        <div class="panel-body">
            <div class="form-group" id="SegmentTableContainer" style="overflow: scroll;">
                <table class="table table-bordered table-condensed table-responsive" id="SegmentTable"
                    style="width: 100%;">
                    <thead>
                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                            <th style="width: 75%;">Segment Name
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
                    <label class="control-label required-field">Segment Name</label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtSegmentName" runat="server" CssClass="form-control"></asp:TextBox>
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
                        onclick="javascript: return SaveOrUpdateSegment();" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
