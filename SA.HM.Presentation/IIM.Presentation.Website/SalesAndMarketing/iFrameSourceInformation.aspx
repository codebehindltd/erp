<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="iFrameSourceInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.iFrameSourceInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var flag = 0; var Name = "";
        $(document).ready(function () {
            Name = $.trim(CommonHelper.GetParameterByName("name"));
            if (Name != "") {
                $("#ContentPlaceHolder1_txtSourceName").val(Name);
            }
        });
        function PerformClearAction() {
            //$("#ContentPlaceHolder1_hfId").val("0")
            $("#ContentPlaceHolder1_txtSourceName").val("");
            $("#ContentPlaceHolder1_txtDescription").val("");
            $("#ContentPlaceHolder1_ddlStatus").val("Active");
            $("#btnClear").show();
            $("#btnSave").val("Save & Close");
            $("#btnSaveNContinue").val("Save & Continue").show();
        }
        function SaveOrUpdateSource() {

            var id = 0;
            var sourceName = $("#ContentPlaceHolder1_txtSourceName").val();
            if (id == 0) {
                var IsUpdate = 0;
            }
            else {
                IsUpdate = 1;
            } if (sourceName == "") {
                toastr.warning("Enter Source Name");
                $("#ContentPlaceHolder1_txtSourceName").focus();
                return false;
            }
            PageMethods.DuplicateCheckDynamicaly("SourceName", sourceName, IsUpdate, id, DuplicateCheckDynamicalySucceed, DuplicateCheckDynamicalyFailed);
            return false;

        }
        function DuplicateCheckDynamicalySucceed(result) {
            id = 0;
            sourceName = $("#ContentPlaceHolder1_txtSourceName").val();
            description = $("#ContentPlaceHolder1_txtDescription").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val() == "Active" ? true : false;
            if (result > 0) {
                toastr.warning("Duplicate Source Name");
                $("#ContentPlaceHolder1_txtSourceName").focus();
                return false;
            }
            else {
                var SMSourceInformationBO = {
                    Id: id,
                    SourceName: sourceName,
                    Description: description,
                    Status: status,
                }
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../SalesAndMarketing/SourceInformation.aspx/SaveUpdateSourceInformation',

                    data: JSON.stringify({ sMSourceInformationBO: SMSourceInformationBO }),
                    dataType: "json",
                    success: function (data) {
                        //GridPaging(1, 1);
                        PerformClearAction();
                        //CommonHelper.AlertMessage(data.d.AlertMessage);
                        if (flag == 1) {
                            //$('#CreateNewDialog').dialog('close');
                            if (typeof parent.CloseSourceDialog === "function")
                                parent.CloseSourceDialog();
                            if (typeof parent.ShowMsgDialog === "function")
                                parent.ShowMsgDialog(data.d.AlertMessage);

                        }
                        flag = 0;
                    },
                    error: function (result) {

                    }
                });
            }

            $("#ContentPlaceHolder1_txtSourceName").focus();
        }
        function DuplicateCheckDynamicalyFailed(error) {

        }
        function SaveAndClose() {
            flag = 1;
            SaveOrUpdateSource();
            return false;

        }
    </script>
    <div id="CreateNewDialog" class="panel panel-default">
        <div id="AddPanel">
            <div class="panel-body">

                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Source Name</label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtSourceName" runat="server" CssClass="form-control"></asp:TextBox>
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
                                onclick="javascript: return SaveOrUpdateSource();" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
