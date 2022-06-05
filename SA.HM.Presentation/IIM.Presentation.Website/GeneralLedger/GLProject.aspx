<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="GLProject.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.GLProject" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            IsCanSave = $("#<%=hfSavePermission.ClientID %>").val() == '1' ? true : false;
            IsCanEdit = $("#<%=hfEditPermission.ClientID %>").val() == '1' ? true : false;
            IsCanDelete = $("#<%=hfDeletePermission.ClientID %>").val() == '1' ? true : false;
            IsCanView = $("#<%=hfViewPermission.ClientID %>").val() == '1' ? true : false;
            GridPaging(1, 1);
            $("#<%=ddlSCompanyName.ClientID%>").select2();
            if (!IsCanSave) {
                $("#ContentPlaceHolder1_btnCreateNew").hide();
            }
            else {
                $("#ContentPlaceHolder1_btnCreateNew").show();
            }
        });
        function CreateNew() {
            var iframeid = 'frmPrint';
            var url = "./NewGLProjectIframe.aspx";
            document.getElementById(iframeid).src = url;
            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                title: "New Project",
                show: 'slide'
            });
            return false;
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadGrid(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function LoadGrid(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#ProjectTable tbody tr").length;
            var CompanyId = $("#<%=ddlSCompanyName.ClientID%>").val();
            var Code = $("#<%=txtSCompanyCode.ClientID%>").val();
            var Name = $("#<%=txtSProjectName.ClientID%>").val();
            var ShortName = $("#<%=txtSShortName.ClientID%>").val();

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './GLProject.aspx/LoadProject',

                data: "{'ProjectName':'" + Name + "','ShortName':'" + ShortName + "','CompanyCode':'" + Code + "','CompanyId':'" + CompanyId + "', 'gridRecordsCount':'" + gridRecordsCount + "', 'pageNumber':'" + pageNumber + "', 'isCurrentOrPreviousPage':'" + IsCurrentOrPreviousPage + "'}",
                dataType: "json",
                success: function (data) {
                    LoadTable(data);
                },
                error: function (result) {
                    Clear();
                }
            });
            return false;
        }
        function LoadTable(data) {

            $("#ProjectTable tbody").empty();
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

                tr += "<td style='width:45%;'>" + gridObject.Name + "</td>";
                tr += "<td style='width:30%;'>" + gridObject.Code + "</td>";
                tr += "<td style='width:25%;'>";
                if (IsCanView)
                    tr += "&nbsp;&nbsp;<a onclick=\"javascript:return LoadProject(" + gridObject.ProjectId + "," + gridObject.CompanyId + ");\" title='Project' href='javascript:void();'><img src='../Images/detailsInfo.png' alt='Project'></a>"
                if (IsCanEdit)
                    tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'FillFormEdit(" + gridObject.ProjectId + ")'><img alt='Edit' src='../Images/edit.png' /></a>"
                if (IsCanDelete)
                    tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'DeleteProject(" + gridObject.ProjectId + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                if (IsCanView)
                    tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= \"ShowDocument('" + gridObject.ProjectId + "');\"> <img alt='Document' src='/Images/document.png' title='document' /></a>";
                tr += "<td style='display:none'>" + gridObject.ProjectId + "</td>";


                tr += "</tr>";

                $("#ProjectTable tbody").append(tr);

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
            if (!confirm("Want to edit ?")) {
                return false;
            }
            var iframeid = 'frmPrint';
            var url = "./NewGLProjectIframe.aspx?pid=" + Id;
            document.getElementById(iframeid).src = url;
            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "82%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Project Information",
                show: 'slide'
            });
        }
        function LoadProject(ProjectId, CompanyId) {
            window.location.href = "../GeneralLedger/ProjectInformation.aspx?pid=" + ProjectId + "&cid=" + CompanyId;
            return false;
        }
        function LoadProjectManagement() {
            window.location.href = "./ProjectStageManagement.aspx";
            return false;
        }
        function DeleteProject(id) {
            if (confirm("Want to delete?")) {
                PageMethods.DeleteProject(id, OnSuccessDelete, OnFailedDelete);
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
        function ShowDocument(id) {
            PageMethods.LoadProjectDocument(id, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }
        function OnLoadDocumentSucceeded(result) {
            $("#imageDiv").html(result);

            $("#ProjectDocuments").dialog({
                autoOpen: true,
                modal: true,
                width: "70%",
                height: 300,
                closeOnEscape: true,
                resizable: false,
                title: "Project Documents",
                show: 'slide'
            });

            return false;
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }
        function Clear() {
            $("#<%=txtSCompanyCode.ClientID%>").val("");
            $("#<%=txtSProjectName.ClientID%>").val("");
            $("#<%=txtSShortName.ClientID%>").val("");
        }
        function CloseDialog() {
            $("#SalesNoteDialog").dialog('close');
            return false;
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }
    </script>
    <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="tempDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfParentDoc" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfSelectedEmpId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSelectedEmpIdForSearch" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />

    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <div id="ProjectDocuments" style="display: none;">
        <div id="imageDiv"></div>
    </div>
    <div id="SalesNoteDialog" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static"></iframe>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Project Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSProjectName" runat="server" class="control-label " Text="Project Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtSProjectName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div id="srcAccountCompanyInfo" class="form-group" runat="server">
                    <div class="col-md-2">
                        <asp:Label ID="lblSCompanyName" runat="server" class="control-label " Text="Accounts Company"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlSCompanyName" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSCompanyCode" runat="server" class="control-label " Text="Project Code"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSCompanyCode" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblSShortName" runat="server" class="control-label" Text="Short Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSShortName" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return GridPaging(1,1);" />
                        <asp:Button ID="btnCreateNew" runat="server" Text="Create New" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateNew();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information

            <a style="float: right; padding: 2px;" href='javascript:void();' onclick='javascript:return LoadProjectManagement()' title='Project Management'>
                <img style='width: 22px; height: 20px;' alt='Search Quotation' src='../Images/management.png' /></a>

        </div>
        <div class="panel-body">
            <div class="form-group" id="ProjectTableContainer">
                <table class="table table-bordered table-condensed table-responsive" id="ProjectTable"
                    style="width: 100%;">
                    <thead>
                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                            <th style="width: 45%;">Project Name
                            </th>
                            <th style="width: 30%;">Project Code
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
</asp:Content>
