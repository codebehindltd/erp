<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="PageWiseActivityLogDetailsSetup.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.PageWiseActivityLogDetailsSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ContentPlaceHolder1_ddlFormSetupName").change(function () {
                var formId = $("#ContentPlaceHolder1_ddlFormSetupName").val().trim();

                if (formId == "0") {
                    toastr.info("Please Select a User Form");
                    $("#FormWiseMenuAssign body tr").html("");
                    return false;
                }
                $("#FormWiseMenuAssign thead tr").find("th:eq(1)").find("input").prop("checked", false);

                PageMethods.GetFieldsByFormId(formId, OnLoadGetFieldsSucceed, OnFailed);
                return false;

            });

            $("#chkFields").change(function () {
                if ($(this).is(":checked")) {
                    $("#FormWiseMenuAssign tbody tr").find("td:eq(1)").find("input").prop("checked", true);
                }
                else {
                    $("#FormWiseMenuAssign tbody tr").find("td:eq(1)").find("input").prop("checked", false);
                }
            });
            $("#myTabs").tabs();
            $("#ContentPlaceHolder1_ddlFormName").select2({
                tags: "true",
                allowClear: true,
                width: "99.75%"
            });
        });
        // // Save
        function SaveFormWiseField() {

            var id = "0", pageId = "", fieldId = "", fieldName = "";

            id = $("#ContentPlaceHolder1_hfId").val();
            pageId = $("#<%=ddlFormName.ClientID %>").val();
            //pageId = $("#ContentPlaceHolder1_ddlFormName").trigger().val();
            //fieldId = $("#ContentPlaceHolder1_txtFieldId").val();
            fieldName = $("#ContentPlaceHolder1_txtFieldName").val();

            if (pageId == "0") {
                toastr.info("Please Select Page Name");
                return false;
            }
            else if (fieldName == "") {
                toastr.info("Please Fill Field Name");
                return false;
            }
            //else if (fieldId == "") {
            //    toastr.info("Please Fill Field Id");
            //    return false;
            //}
            var IsUpdate = 0;
            if (id != 0) {
                IsUpdate = 1;
            }
            <%--var isDuplicate = CheckDuplicate(fieldName, IsUpdate, id);
            if (isDuplicate) {
                toastr.warning("Duplicate Field Name for this Page");
                $("#<%=txtFieldName.ClientID %>").focus();
                return false;
            }--%>

            var FormWiseField = {
                Id: id,
                PageIdStr: pageId,
                FieldName: fieldName
                //FieldId: fieldId
            }
            $("#<%=hfPageId.ClientID %>").val(pageId);
            PageMethods.SaveFormWiseField(FormWiseField, OnSaveFormWiseFieldSucceed, OnFailed);
            return false;
        }
        function OnSaveFormWiseFieldSucceed(result) {
            $("#<%=btnSaveField.ClientID %>").val("Save");
            CommonHelper.AlertMessage(result.AlertMessage);
            setTimeout("location.reload();", 6000);
            $("#<%=ddlFormName.ClientID %>").val($("#<%=hfPageId.ClientID %>").val()).trigger('change');
            ClearAction();
        }
        // // Update
        function FormWiseFieldEdit(Id) {
            PageMethods.EditFormWiseField(Id, OnEditFormWiseFieldSucceed, OnFailed);
            return false;
        }
        function OnEditFormWiseFieldSucceed(result) {
            $("#ContentPlaceHolder1_btnSaveField").val("Update");
            $("#myTabs").tabs({ active: 0 });
            $("#ContentPlaceHolder1_hfId").val(result.Id);
            $("#<%=ddlFormName.ClientID %>").val(result.PageIdStr).trigger('change');
            //$("#ContentPlaceHolder1_ddlFormName").trigger(result.PageId);
            $("#ContentPlaceHolder1_txtFieldName").val(result.FieldName);
            //$("#ContentPlaceHolder1_txtFieldId").val(result.FieldId);
            
        }
        function FormWiseFieldDelete(Id) {
            PageMethods.DeleteFormWiseField(Id, OnDeleteFormWiseFieldSucceed, OnFailed);
            return false;
        }
        function OnDeleteFormWiseFieldSucceed(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            $("#myTabs").tabs({ active: 2 });
            $("#btnSearch").click();
        }
        function OnLoadGetFieldsSucceed(results) {
            $("#FormWiseMenuAssign tbody").html("");
            var isSelected = "";
            var i = 0, fieldLength = results.length;
            var tr = "";

            for (i = 0; i < fieldLength; i++) {

                if (i % 2 == 0) {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }
                else {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }

                tr += "<td style=\"width: 50%;\">" +
                    results[i].FieldName +
                    "</td>";//0
                tr += "<td style=\"width: 50%; text-align:center;\">" +
                    "<input type='checkbox' " + (results[i].IsSaveActivity == true ? "checked='checked'" : "") + "/>" +
                    "</td>";//1

                tr += "<td style=\"display:none;\">" + results[i].IsSaveActivity + "</td>";//2
                tr += "<td style=\"display:none;\">" + results[i].Id + "</td>";//3
                tr += "<td style=\"display:none;\">" + results[i].PageIdStr + "</td>";//4

                tr += "</tr>";

                $("#FormWiseMenuAssign tbody").append(tr);
                tr = "";
            }

        }
        function UpdateSelectedFields() {

            var formId = $("#ContentPlaceHolder1_ddlFormSetupName").val().trim();

            if (formId == "0") {
                toastr.info("Please Select a User Form");
                return false;
            }
            var PageId = $("#ContentPlaceHolder1_ddlFormSetupName").val();

            var FormWiseFieldSetupList = new Array();
            $("#FormWiseMenuAssign tbody tr").each(function () {

               var isChecked = $(this).find("td:eq(1)").find("input").is(":checked");
               var isCheckedPrevious = ($(this).find("td:eq(2)").text());

                if (("" + isChecked) != isCheckedPrevious) {

                    FormWiseFieldSetupList.push({
                        Id: ($(this).find("td:eq(3)").text()),
                        IsSaveActivity: isChecked
                    });

                }

            });
            if (FormWiseFieldSetupList.length > 0) {
                PageMethods.UpdateSelectedFields(FormWiseFieldSetupList, UpdateSelectedFieldsSucceed, OnFailed);
            }
            return false;
        }
        function UpdateSelectedFieldsSucceed(result) {

            $("#FormWiseMenuAssign thead tr").find("th:eq(1)").find("input").prop("checked", false);

            if (result.AlertMessage.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function SearchFormWiseField(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#FormWiseFieldGrid tbody tr").length;
            var pageId = "";

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

            PageMethods.SearchFormWiseField(pageId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchFormWiseFieldSucceed, OnFailed);

            return false;
        }
        function OnSearchFormWiseFieldSucceed(result) {
            var tr = "";
            $("#FormWiseFieldGrid tbody").html("");
            $.each(result.GridData, function (count, gridObject) {
                tr += "<tr>";

                //tr += "<td style='width:40%;'>" + gridObject.FieldId + "</td>";
                tr += "<td style='width:50%;'>" + gridObject.FieldName + "</td>";
                tr += "<td style=\"text-align: center; width:15%; cursor:pointer;\">";
                tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return FormWiseFieldEdit(" + gridObject.Id + ")\" alt='Edit'  title='Edit' border='0' />";
                tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return FormWiseFieldDelete(" + gridObject.Id + ")\" alt='Edit'  title='Edit' border='0' />";
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
        function CheckDuplicate(name, IsUpdate, id) {
            var returnInfo = false;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../HMCommon/PageWiseActivityLogDetailsSetup.aspx/DuplicateCheckDynamicaly',
                data: "{'fieldName':'" + "FieldName" + "', 'fieldValue':'" + name.trim() + "','isUpdate':'" + IsUpdate + "', 'pkId':'" + id + "'}",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d > 0) {
                        <%--flag = 0;
                        toastr.warning("Duplicate Company Name");
                        $("#<%=txtCompanyName.ClientID %>").focus();--%>
                        returnInfo = true;
                    }
                    else {
                        returnInfo = false;
                    }

                },
                error: function (data) {
                    toastr.warning("Error in Json")
                }
            });
            return returnInfo;
        }
        function OnFailed(error) {
            toastr.warning(error);
            return false;
        }
        function ClearAction() {
            $("#ContentPlaceHolder1_hfId").val("0");
            $("#<%=hfPageId.ClientID %>").val("0");
            //$("#ContentPlaceHolder1_ddlFormName").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtFieldName").val("");
            //$("#ContentPlaceHolder1_txtFieldId").val("");
            $("#ContentPlaceHolder1_btnSaveField").val("Save");
            return false;
        }
    </script>
    <asp:HiddenField ID="hfId" runat="server" Value="0" />
    <asp:HiddenField ID="hfPageId" runat="server" Value="0" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Entry Tab</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Setup Tab</a></li>
            <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-3">Search Tab</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Form Wise Field Selection
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
                        <%--<div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFieldId" runat="server" class="control-label required-field" Text="Field Id"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtFieldId" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                        </div>--%>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSaveField" runat="server" TabIndex="4" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
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
            <div id="FieldSetupPanel" class="panel panel-default">
                <div class="panel-heading">
                    Fields Setup
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Form Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlFormSetupName" runat="server" CssClass="form-control"
                                    TabIndex="3">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <br />
                        <div class="form-group">
                            <table id="FormWiseMenuAssign" style="width: 100%;" class="table table-bordered table-condensed table-responsive">
                                <thead>
                                    <tr style="color: White; background-color: #44545E; font-weight: bold; text-align: left;">
                                        <th style="width: 50%; vertical-align: middle">Field Name 
                                        </th>
                                        <th style="width: 50%; text-align: center;">Select 
                                    <br />
                                            <input type="checkbox" id="chkFields" />
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="2"
                                    CssClass="TransactionalButton btn btn-primary btn-sm" OnClientClick="javascript:return UpdateSelectedFields()" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-3">
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
                                <th style="width: 50%;">Field Name
                                </th>
<%--                                <th style="width: 40%;">Field Information
                                </th>--%>
                                <th style="width: 50%;">Action
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
