<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="FormWiseFieldSetup.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.FormWiseFieldSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>

        $(document).ready(function () {
            $("#myTabs").tabs();

            $("#ContentPlaceHolder1_ddlFormName").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
        });

        function SaveFormWiseField() {

            var Id = "0", pageId = "0", fieldId = "", fieldName = "";

            id = $("#ContentPlaceHolder1_hfId").val();
            pageId = $("#ContentPlaceHolder1_ddlFormName").val();
            fieldId = $("#ContentPlaceHolder1_txtFieldId").val();
            fieldName = $("#ContentPlaceHolder1_txtFieldName").val();

            if (pageId == "0") {
                toastr.info("Please Select Page Name");
                return false;
            }
            else if (fieldName == "") {
                toastr.info("Please Fill Field Name");
                return false;
            }
            else if (fieldId == "") {
                toastr.info("Please Fill Field Id");
                return false;
            }

            var FormWiseField = {
                Id: id,
                PageId: pageId,
                FieldName: fieldName,
                FieldId:  fieldId
            }

            PageMethods.SaveFormWiseField(FormWiseField, OnSaveFormWiseFieldSucceed, OnSaveFormWiseFieldFailed);
            return false;
        }
        function SearchFormWiseField(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#FormWiseFieldGrid tbody tr").length;
            var pageId = "0";

            pageId = $("#ContentPlaceHolder1_ddlSearchFormName").val();
            if (pageId == "0") {
                toastr.info("Please Select Page Name");
                return false;
            }

            $("#GridPagingContainer ul").html("");
            $("#FormWiseFieldGrid tbody tr").remove();
            $("#FormWiseFieldGrid tbody").html("");

            if (pageNumber < 0)
                pageNumber = 1;

            PageMethods.SearchFormWiseField(pageId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchFormWiseFieldSucceed, OnSearchFormWiseFieldOrderFailed);

            return false;
        }
        function OnSaveFormWiseFieldSucceed(result) {
            $("#<%=btnSave.ClientID %>").val("Save");
            CommonHelper.AlertMessage(result.AlertMessage);
            ClearAction();
        }
        function OnSaveFormWiseFieldFailed() {
        }
        function OnSearchFormWiseFieldSucceed(result) {
            var tr = "";
            $.each(result.GridData, function (count, gridObject) {
                tr += "<tr>";

                tr += "<td style='width:40%;'>" + gridObject.FieldId + "</td>";
                tr += "<td style='width:40%;'>" + gridObject.FieldName + "</td>";
                tr += "<td style=\"text-align: center; width:15%; cursor:pointer;\">";
                tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return FormWiseFieldEdit(" + gridObject.Id + ")\" alt='Edit'  title='Edit' border='0' />";
                tr += "</td>";
                tr += "</tr>";

                $("#FormWiseFieldGrid tbody").append(tr);
                tr = "";


            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
            return false;
        }
        function FormWiseFieldEdit(Id) {
            PageMethods.EditFormWiseField(Id, OnEditFormWiseFieldSucceed, OnEditFormWiseFieldFailed);
            return false;
        }
        function OnEditFormWiseFieldSucceed(result) {
            $("#ContentPlaceHolder1_btnSave").val("Update");
            $("#myTabs").tabs({ active: 0 });
            $("#ContentPlaceHolder1_hfId").val(result.Id);
            $("#ContentPlaceHolder1_ddlFormName").val(result.PageId);
            $("#ContentPlaceHolder1_txtFieldName").val(result.FieldName);
            $("#ContentPlaceHolder1_txtFieldId").val(result.FieldId);

        }
        function OnEditFormWiseFieldFailed() {

        }
        function OnSearchFormWiseFieldOrderFailed(error) {
            CommonHelper.SpinnerClose();
            toastr.error(error.get_message());
        }
        function ClearAction() {
            $("#ContentPlaceHolder1_hfId").val("0");
            $("#ContentPlaceHolder1_ddlFormName").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtFieldName").val("");
            $("#ContentPlaceHolder1_txtFieldId").val("");
            $("#ContentPlaceHolder1_btnSave").val("Save");
            return false;
        }
        function ClearSearch() {
            $("#ContentPlaceHolder1_ddlSearchFormName").val("0").trigger('change');
            return false;
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchFormWiseField(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }

    </script>

    <asp:HiddenField ID="hfId" runat="server" Value="0" />

    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Entry Tab</a></li>

            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Tab</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Form Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-sm-2">
                                <asp:Label ID="lblFormName" runat="server" class="control-label required-field" Text="Form Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlFormName" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFieldName" runat="server" class="control-label required-field" Text="Field Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtFieldName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFieldId" runat="server" class="control-label required-field" Text="Field Id"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtFieldId" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" TabIndex="4" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript:return SaveFormWiseField()" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="5" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return ClearAction();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Form Wise Field Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSearchFormName" runat="server" class="control-label" Text="Form Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSearchFormName" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-large" value="Search" onclick="SearchFormWiseField(1, 1)" />
                                <input type="button" id="btnSearchCancel" class="TransactionalButton btn btn-primary btn-large" value="Clear" onclick="ClearSearch()" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="divSearchResult" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">

                    <table id="FormWiseFieldGrid" class="table table-bordered table-condensed table-responsive" width="100%">
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <th style="width: 40%;">Field Name
                                </th>
                                <th style="width: 40%;">Field Information
                                </th>
                                <th style="width: 20%;">Action
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

