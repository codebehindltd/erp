<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="TemplateInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.TemplateInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var flag = 0; var isAdminUser = false;
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            LoadSearch(1, 1);
        });
        function FillFormEdit(id, name) {

            if (!confirm("Do you want to edit ?")) {
                return false;
            }
            var iframeid = 'frmPrint';
            var url = "./TemplateInfoIframe.aspx?editId=" + id;
            parent.document.getElementById(iframeid).src = url;

            $("#DialogueDiv").dialog({
                autoOpen: true,
                modal: true,
                width: 1200,
                height: 700,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Update - " + name,
                show: 'slide'
            });

            return false;
        }
        function CreateNew() {
            //PerformClearAction();
            var iframeid = 'frmPrint';
            var url = "./TemplateInfoIframe.aspx";
            parent.document.getElementById(iframeid).src = url;

            $("#DialogueDiv").dialog({
                autoOpen: true,
                modal: true,
                width: 1200,
                height: 700,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Create New Templete",
                show: 'slide'
            });
            return false;
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadSearch(pageNumber, IsCurrentOrPreviousPage);
        }
        function LoadSearch(pageNumber, IsCurrentOrPreviousPage) {
            var name = "", typeId = "", templateId = "", subject = "";
            var gridRecordsCount = $("#ContactTable tbody tr").length;
            name = $("#<%=txtName.ClientID %>").val();
            subject = $("#<%=txtSubject.ClientID %>").val();
            typeId = $("#<%=ddlType.ClientID %>").val();
            templateId = $("#<%=ddlTemplateFor.ClientID %>").val();
            if (typeId == "0") {
                typeId = "";
            }
            if (templateId == "0") {
                templateId = "";
            }
            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './TemplateInformation.aspx/LoadSearch',
                data: JSON.stringify({ name: name, typeId: typeId, templateForId: templateId, subject: subject, gridRecordsCount: gridRecordsCount, pageNumber: pageNumber, isCurrentOrPreviousPage: IsCurrentOrPreviousPage }),

                dataType: "json",
                success: function (data) {

                    LoadTable(data);
                },
                error: function (result) {
                    CommonHelper.AlertMessage(result.d.AlertMessage);
                }
            });
            return false;
        }
        function LoadTable(searchData) {

            var rowLength = $("#ContactTable tbody tr").length;
            var dataLength = searchData.length;
            $("#ContactTable tbody").empty();
            $("#GridPagingContainer ul").empty();
            i = 0;

            if (searchData.d.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"6\" >No Data Found</td> </tr>";
                $("#ContactTable tbody ").append(emptyTr);
                return false;
            }

            $.each(searchData.d.GridData, function (count, gridObject) {

                var tr = "";

                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }
                tr += "<td style='width:15%;'>" + gridObject.Name + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.Type + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.TemplateFor + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.Subject + "</td>";

                tr += "<td style='width:10%;'> <a onclick=\"javascript:return FillFormEdit(" + gridObject.Id + ",\'" + gridObject.Name + '\');"' + "title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                tr += "&nbsp;&nbsp;<a href='#' onclick= 'DeleteItem(" + gridObject.Id + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";


                tr += "<td style='display:none'>" + gridObject.Id + "</td>";
                tr += "</tr>";

                $("#ContactTable tbody").append(tr);

                tr = "";
                i++;
            });
            //PerformCancleAction();
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.NextButton);
            return false;
        }
        function DeleteItem(Id) {
            if (!confirm("Do you want to delete?")) {
                return;
            }
            $(Id).parent().parent().remove();
            PageMethods.DeleteData(Id, DeleteSucceed, DeleteFailed);
            return false;
        }
        function DeleteSucceed(result) {
            LoadContactForSearch(1, 1);
            CommonHelper.AlertMessage(result.AlertMessage);
            return false;
        }
        function DeleteFailed(error) {
            CommonHelper.AlertMessage(error.AlertMessage);
            return false;
        }
        function CloseDialog() {
            $("#DialogueDiv").dialog('close');
            return false;
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }
    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <div id="DialogueDiv" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="AddPanel" class="panel panel-default">
        <div class="panel-heading">
            Template Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">

                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Name</label>
                    </div>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtName" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Subject</label>
                    </div>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtSubject" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>

                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Type</label>
                    </div>
                    <div class="col-sm-4">
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Template For</label>
                    </div>
                    <div class="col-sm-4">
                        <asp:DropDownList ID="ddlTemplateFor" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="javascript: return LoadSearch(1,1);"
                            CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />
                        <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return PerformCancleAction();" TabIndex="6" />
                        <asp:Button ID="btnAdd" runat="server" Text="Add New Templete" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateNew();" TabIndex="6" />
                    </div>
                </div>
            </div>
        </div>
        <div id="SearchPanel" class="panel panel-default">
            <div class="panel-heading">
                Search Information
            </div>
            <div class="panel-body">
                <div class="form-group" id="ContactTableContainer" style="overflow: scroll;">
                    <table class="table table-bordered table-condensed table-responsive" id="ContactTable"
                        style="width: 100%;">
                        <thead>
                            <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                <th style="width: 20%;">Name
                                </th>
                                <th style="width: 20%;">Template Type
                                </th>
                                <th style="width: 20%;">Template For
                                </th>
                                <th style="width: 20%;">Subject
                                </th>
                                <th style="width: 10%;">Action
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
