<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="TemplateInfoIframe.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.TemplateInfoIframe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/ckeditor/ckeditor.js"></script>
    <script type="text/javascript">
        var flag = 0;
        var templateId = "";
        var templateDetailsId = "0";
        var deleteDbItem = [], editDbItem = [], newlyAddedItem = [];
        $(document).ready(function () {
            CKEDITOR.replace('txtBody');
            var editId = $.trim(CommonHelper.GetParameterByName("editId"));
            if (editId != "") {
                FillForm(editId);
            }
            $("#<%=ddlTemplateFor.ClientID %>").change(function () {
                var segement = $("#<%=ddlTemplateFor.ClientID %>").val();
                LoadTemplateFor(segement);  
                
            });
            var ddlItem = '<%=ddlReplacedBy.ClientID%>';
            var control = $('#' + ddlItem);
            control.empty();
            control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');


            $("#btnAddDetails").click(function () {
                var bodyText = $("#<%=txtBodyText.ClientID %>").val();
                var replaceBy = $("#<%=ddlReplacedBy.ClientID %>").val();
                var replaceByText = $("#<%=ddlReplacedBy.ClientID %>").text();
                var templateId = $("#<%=hfId.ClientID %>").val();
                var tableName = $("#<%=ddlTemplateFor.ClientID %>").val();
                
                var tableId = "DetailsGrid";
                if (bodyText != "" && replaceBy != "0") {
                    LoadDetailsTable(bodyText, replaceBy, tableId, templateId, "0", tableName);
                }
                else if (bodyText == "") {
                    toastr.warning("Please add body text.");
                    $("#<%=txtBodyText.ClientID %>").focus();
                   return false;
               }
               else {
                   toastr.warning("Please select field.");
                   $("#<%=ddlReplacedBy.ClientID %>").focus();
                    return false;
                }

            });
            $("#<%=ddlType.ClientID %>").change(function () {
                var id = $("#<%=ddlType.ClientID %>").val();
                ChangeType(id);

            });
            $("#ContentPlaceHolder1_ddlReplacedBy").select2({
                tags: "true",
                //placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            //CKEDITOR.config.removePlugins = 'newpage,save,flash,iframe,about,print';
        });
        function ChangeType(type) {
            if (type == "Email") {
                $("#divSubject").show();
                $("#divCKbody").show();
                $("#divSMS").hide();
            }
            else if (type == "Letter") {
                $("#divSubject").hide();
                $("#divCKbody").show();
                $("#divSMS").hide();
            }
            else if (type == "SMS") {
                $("#divSubject").hide();
                $("#divCKbody").hide();
                $("#divSMS").show();
            }
            else {
                $("#divSubject").hide();
                $("#divSMS").hide();
            }
        }
        function LoadTemplateFor(segement) {
            if (segement != "0") {
                if (segement != "CommonDataForTemplate") {
                    LoadFields(segement);
                }
                else {
                    LoadCommonFields(segement);
                }
                
                $("#replaceByDiv").show();
            }
            else {
                $("#replaceByDiv").hide();
            }
        }
        function LoadDetailsTable(bodyText, replaceBy, tableId, templateId, tempDetailsId, tableName) {

            var rowLength = $("#" + tableId + " tbody tr").length;

            var tr = "";

            if (rowLength % 2 == 0) {
                tr += "<tr style='background-color:#FFFFFF;'>";
            }
            else {
                tr += "<tr style='background-color:#E3EAEB;'>";
            }
            tr += "<td style='width:40%;'>" + bodyText + "</td>"; //0
            tr += "<td style='width:50%;'>" + replaceBy + "</td>"; //1

            tr += "<td style='width:10%;'>";
            tr += "<a href='#' onclick= 'DeleteDetailsItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";

            tr += "<td style='display:none'>" + templateId + "</td>";    //3
            tr += "<td style='display:none'>" + tempDetailsId + "</td>"; //4
            tr += "<td style='display:none'>" + tableName + "</td>"; //5

            tr += "</tr>";
            $("#" + tableId + " tbody").append(tr);

            GridShowHide(tableId);

        }
        function GridShowHide(tableId) {
            var length = $("#" + tableId + " tbody tr").length;
            if (length > 0) {
                $("#" + tableId).show();
            }
            else {
                $("#" + tableId).hide();
            }
            $("#<%=txtBodyText.ClientID %>").val('');
            $("#<%=ddlReplacedBy.ClientID %>").val("0").trigger('change');
        }
        function DeleteDetailsItem(deleteItem) {
            if (!confirm("Do you want to delete?")) {
                return;
            }
            var tr = $(deleteItem).parent().parent();
            var tableId = deleteItem.closest('table').id;

            var DetailsId = $(tr).find("td:eq(4)").text();
            var Id = $("#ContentPlaceHolder1_hfId").val();

            if ((DetailsId != "0")) {
                deleteDbItem.push({
                    Id: DetailsId,
                    TemplateId: Id
                });
            }

            $(deleteItem).parent().parent().remove();
            contactDetailsId = "0";


            return false;
        }
        function SaveAndContinue() {
            PerformSave();
            return false;
        }
        function SaveAndClose() {

            flag = 1;
            $.when(PerformSave()).done(function () {

                if (flag == 1) {
                    //if (typeof parent.FillForm === "function") {
                    //    var id = $("#ContentPlaceHolder1_hfId").val();
                    //    parent.FillForm(id);
                    //}
                    //if (typeof parent.CloseContactDialog === "function")
                    //    parent.CloseContactDialog();
                    if (typeof parent.CloseDialog === "function")
                        parent.CloseDialog();
                    if ($("#btnSave").val() == "Update and Close") {
                        $("#btnSave").val("Save And Close");
                        $("#btnSaveContinue").show();
                        $("#btnCancel").show();
                    }
                    PerformClearAction();
                    //$('#AddNewContactContaiiner').dialog('close');
                }
            });
            return false;
        }
        function FillForm(Id) {

            CommonHelper.SpinnerOpen();

            PageMethods.FillForm(Id, OnFillFormSucceed, OnFillFormFailed);

            return false;
        }
        function OnFillFormSucceed(result) {

            PerformClearAction();
            CommonHelper.SpinnerClose();
            var details = result[0].Details;
            var templeteInfo = result[0].TemplateInformation;

            $("#ContentPlaceHolder1_btnSaveClose").val("Update And Close");
            $("#ContentPlaceHolder1_btnSaveContinue").hide();
            $("#ContentPlaceHolder1_btnClear").show();

            $("#<%=hfId.ClientID %>").val(templeteInfo.Id);
            $("#<%=txtName.ClientID %>").val(templeteInfo.Name);
            $("#<%=txtSubject.ClientID %>").val(templeteInfo.Subject);
            
            if (templeteInfo.Type == "SMS") {
                $("#<%=txtBodySMS.ClientID %>").val(templeteInfo.Body);
            }
            else {
                CKEDITOR.instances.txtBody.setData(templeteInfo.Body);
            }
            <%--$("#<%=txtBody.ClientID %>").val(templeteInfo.Body);--%>
            $("#<%=ddlType.ClientID %>").val(templeteInfo.Type).trigger('change');
            $("#<%=ddlTemplateFor.ClientID %>").val(templeteInfo.TemplateFor);
            LoadTemplateFor(templeteInfo.TemplateFor);
            if (details.length > 0) {
                for (var i = 0; i < details.length; i++) {
                    LoadDetailsTable(details[i].BodyText, details[i].ReplacedBy, "DetailsGrid", details[i].TemplateId, details[i].Id, details[i].TableName);
                }
            }

            CommonHelper.SpinnerClose();
        }
        function OnFillFormFailed(error) {
            CommonHelper.AlertMessage(error.AlertMessage);
            CommonHelper.SpinnerClose();
            return false;
        }
        function PerformSave() {
            var name = "", contactNo = "", companyId = 0, subject = "", body = "", emailWork = "", id = 0;
            var typeId = "", templateForId = "";


            id = $("#<%=hfId.ClientID %>").val();
            name = $("#<%=txtName.ClientID %>").val();
            subject = $("#<%=txtSubject.ClientID %>").val();
            
            <%--body = $("#<%=txtBody.ClientID %>").val();--%>
            typeId = $("#<%=ddlType.ClientID %>").val();
            templateForId = $("#<%=ddlTemplateFor.ClientID %>").val();

            if (typeId == "SMS") {
                body = $("#<%=txtBodySMS.ClientID %>").val();
            } else {
                body = CKEDITOR.instances.txtBody.getData();     
            }
            if (name == "") {
                flag = 0;
                toastr.warning("Please Add A Name:");
                $("#<%=txtName.ClientID %>").focus();
                return false;
            }
            else if (typeId == "0") {
                toastr.warning("Please Select Type");
                $("#<%=ddlType.ClientID %>").focus();
                flag = 0;
                return false;
            }
            else if (templateForId == "0") {
                toastr.warning("Please Select Template For");
                $("#<%=ddlTemplateFor.ClientID %>").focus();
                flag = 0;
                return false;
            }
            else if (body == "") {
                toastr.warning("Please add body");
                <%--$("#<%=txtBody.ClientID %>").focus();--%>
                flag = 0;
                return false;
            }

            var templateInformationBO = {
                Id: id,
                Name: name,
                Type: typeId,
                TemplateFor: templateForId,
                Subject: subject,
                Body: body
                
            }

            var bodyText = "", replaceBy = "", Id = "0", DetailsId = "0", tableName = "";
            var details = $("#DetailsGrid tbody tr").length;

            if (details > 0) {
                $("#DetailsGrid tbody tr").each(function (index, item) {
                    bodyText = $.trim($(item).find("td:eq(0)").text());
                    replaceBy = $.trim($(item).find("td:eq(1)").text());
                    Id = $.trim($(item).find("td:eq(3)").text());
                    DetailsId = $.trim($(item).find("td:eq(4)").text());
                    tableName = $.trim($(item).find("td:eq(5)").text());
                    if (DetailsId == "0") {
                        
                        newlyAddedItem.push({
                            BodyText: bodyText,
                            ReplacedBy: replaceBy,
                            DetailsId: parseInt(DetailsId, 10),
                            TemplateId: parseInt(Id, 10),
                            TableName: tableName
                        });
                    }
                });
            }

            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './TemplateInfoIframe.aspx/SaveUpdate',
                data: JSON.stringify({ bO: templateInformationBO, newlyAddedItem: newlyAddedItem, deleteDbItem: deleteDbItem }),
                dataType: "json",
                async: false,
                success: function (data) {
                    OnSaveSucceed(data.d);
                },
                error: function (result) {
                    OnSaveFailed(result.d);
                }
            });

        }
        function OnSaveSucceed(result) {

            if (result.IsSuccess) {
                parent.ShowAlert(result.AlertMessage);

                if (typeof parent.GridPaging === "function") {
                    parent.GridPaging(1, 1);
                    PerformClearAction();
                }
                
            }
            else {
                flag = 0;
                parent.ShowAlert(result.AlertMessage);
            }
            //PerformClearAction();

        }
        function OnSaveFailed(error) {
            CommonHelper.AlertMessage(error.AlertMessage);
            return false;
        }
        function PerformClearAction() {
            $("#ContentPlaceHolder1_btnSaveClose").val("Save And Close");
            $("#ContentPlaceHolder1_btnSaveContinue").val("Save And Continue");
            $("#ContentPlaceHolder1_btnClear").show();
            $("#ContentPlaceHolder1_btnSaveContinue").show();

            deleteDbItem = []; editDbItem = []; newlyAddedItem = [];

            $("#<%=hfId.ClientID %>").val("0");
            $("#<%=txtName.ClientID %>").val("");
            $("#<%=txtSubject.ClientID %>").val("");
            CKEDITOR.instances.txtBody.setData('');
            $("#<%=txtBodySMS.ClientID %>").val("");
            <%--$("#<%=txtBody.ClientID %>").val("");--%>
            $("#<%=ddlType.ClientID %>").val("0");
            $("#<%=ddlTemplateFor.ClientID %>").val("0");

            $("#DetailsGrid tbody tr").html("");
            return false;
        }
        function LoadFields(segement) {
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './TemplateInfoIframe.aspx/GetFieldsFromTable',
                data: JSON.stringify({ segment: segement }),
                dataType: "json",
                async: false,
                success: function (data) {
                    SucceedGetFieldsFromTable(data.d);
                },
                error: function (result) {
                    FailedGetFieldsFromTable(result.d);
                }
            });
            //PageMethods.GetFieldsFromTable(segement, SucceedGetFieldsFromTable, FailedGetFieldsFromTable);
            return false;
        }
        function SucceedGetFieldsFromTable(result) {
            if (result[0].RawColumns.length > 0) {
                //document.getElementById("ContentPlaceHolder1_ddlTemplateFor").disabled = true;
                var list = result[0].RawColumns;
                var text = result[0].Rearranged;
                var ddlItem = '<%=ddlReplacedBy.ClientID%>';
                var control = $('#' + ddlItem);
                control.empty();
                
                if (list != null) {
                    if (list.length > 0) {
                        control.removeAttr("disabled");
                        control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                        //for (i = 0; i < list.length; i++) {
                        //    control.append('<option title="' + list[i].ColumnName + '" value="' + list[i].ColumnName + '">' + list[i].ColumnName + '</option>');
                        //}
                        $.each(result[0].RawColumns, function (i, item) {
                            control.append('<option title="' + result[0].Rearranged[i].ColumnName + '" value="' + item.ColumnName + '">' + result[0].Rearranged[i].ColumnName + '</option>');
                        });
                    }
                    else {
                        control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    }
                }
                else {
                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
                return false;
            }

        }
        function FailedGetFieldsFromTable(error) {

        }
        function LoadCommonFields(segement) {
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './TemplateInfoIframe.aspx/GetCommonFields',
                data: JSON.stringify({ segment: segement }),
                dataType: "json",
                async: false,
                success: function (data) {
                    SucceedLoadCommonFields(data.d);
                },
                error: function (result) {
                    FailedLoadCommonFields(result.d);
                }
            });
            //PageMethods.GetFieldsFromTable(segement, SucceedGetFieldsFromTable, FailedGetFieldsFromTable);
            return false;
        }
        function SucceedLoadCommonFields(result) {
            if (result.length > 0) {
                //document.getElementById("ContentPlaceHolder1_ddlTemplateFor").disabled = true;
                var list = result;
                
                var ddlItem = '<%=ddlReplacedBy.ClientID%>';
                var control = $('#' + ddlItem);
                control.empty();
                
                if (list != null) {
                    if (list.length > 0) {
                        control.removeAttr("disabled");
                        control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                        //for (i = 0; i < list.length; i++) {
                        //    control.append('<option title="' + list[i].ColumnName + '" value="' + list[i].ColumnName + '">' + list[i].ColumnName + '</option>');
                        //}
                        $.each(list, function (i, item) {
                            control.append('<option title="' + item.FieldValue + '" value="' + item.Description + '">' + item.FieldValue + '</option>');
                        });
                    }
                    else {
                        control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    }
                }
                else {
                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
                return false;
            }

        }
        function FailedLoadCommonFields(error) {

        }
    </script>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfDetailsId" runat="server" Value="0"></asp:HiddenField>
    <div id="AddPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">

                <div class="form-group">
                    <label class="control-label col-md-2 required-field">Name</label>
                    <div class="col-sm-10">

                        <asp:TextBox ID="txtName" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-md-2 ">Type</label>

                    <div class="col-sm-4">
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                    
                </div>
                <div class="form-group" id="divSubject">
                    <label class="control-label col-md-2 required-field">Subject</label>
                    <div class="col-sm-10">

                        <asp:TextBox ID="txtSubject" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" id="divCKbody">
                    <label class="control-label col-md-2 required-field">Body</label>
                    <div class="col-sm-10">
                        <textarea id="txtBody" name="txtBody"></textarea>
                        <%--<asp:TextBox ID="txtBody" CssClass="form-control" runat="server" TextMode="MultiLine" Wrap="true"></asp:TextBox>--%>
                    </div>
                </div>
                <div class="form-group" id="divSMS" style="display:none">
                    <label class="control-label col-md-2 required-field">Body</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txtBodySMS" CssClass="form-control" runat="server" TextMode="MultiLine" Wrap="true"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div id="AddPanelDetails" class="panel panel-default">
            <div class="panel-heading">
                Body Details
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="control-label col-md-2 ">Template For</label>

                    <div class="col-sm-4">
                        <asp:DropDownList ID="ddlTemplateFor" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-md-2 required-field">Body Text</label>
                        <div class="col-sm-10">

                            <asp:TextBox ID="txtBodyText" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group" id="replaceByDiv" style="display:none">
                        <label class="control-label col-md-2 ">Replace By</label>

                        <div class="col-sm-10">
                            <asp:DropDownList ID="ddlReplacedBy" runat="server" CssClass="form-control" TabIndex="1">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <input type="button" id="btnAddDetails" class="TransactionalButton btn btn-primary btn-sm" value="Add" title="Add Details" />
                        <input type="button" id="btnClearDetails" class="TransactionalButton btn btn-primary btn-sm" value="Clear" title="Add Details" />
                    </div>
                    &nbsp;
                    <div class="form-group">
                        <table id="DetailsGrid" class="table table-bordered table-condensed table-responsive"
                            style="width: 100%; display: none">
                            <thead>
                                <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                    <th style="width: 40%;">Body Text
                                    </th>
                                    <th style="width: 50%;">Replace By
                                    </th>
                                    <th style="width: 10%; text-align: center">Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
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

</asp:Content>
