<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="InvItemAttributeSetupIframe.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.InvItemAttributeSetupIframe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var isClose = 0;
        $(document).ready(function () {
            var setupType = $.trim(CommonHelper.GetParameterByName("sType"));
            var id = $.trim(CommonHelper.GetParameterByName("sid"));
            if (id != "") {
                PerformEdit(id);
            }
            CommonHelper.ApplyIntigerValidation();

            $('#ContentPlaceHolder1_cbDeclineStage').change(function () {
                if ($(this).is(":checked")) {
                    $(this).attr("disabled", false);
                    $('#ContentPlaceHolder1_cbCloseStage').prop("checked", false).attr("disabled", true);
                }
                else {
                    $('#ContentPlaceHolder1_cbCloseStage').attr("disabled", false);

                }
            });
            $('#ContentPlaceHolder1_cbCloseStage').change(function () {
                if ($(this).is(":checked")) {
                    $(this).attr("disabled", false);
                    $('#ContentPlaceHolder1_cbDeclineStage').prop("checked", false).attr("disabled", true);
                }
                else {
                    $('#ContentPlaceHolder1_cbDeclineStage').attr("disabled", false);
                }
            });

        });
        function SaveOrUpdateSetup() {
            var Id = $("#ContentPlaceHolder1_hfId").val();
            var Name = '';
            var setupType = $("#ContentPlaceHolder1_ddlSetupType").val();
            if (setupType == "0") {
                toastr.warning("Please Select Setup Type");
                $("#ContentPlaceHolder1_ddlSetupType").focus();
                isClose = 0;
                return false;
            }

            if (setupType == 'SupportStage')
                Name = $("#ContentPlaceHolder1_txtSupportStage").val();
            else
                Name = $("#ContentPlaceHolder1_txtName").val();
            if (Id == 0) {
                var IsUpdate = 0;
            }
            else {
                IsUpdate = 1;
            }
            PageMethods.DuplicateCheckDynamicaly("Name", Name, "SetupType", setupType, IsUpdate, Id, DuplicateCheckDynamicalySucceed, DuplicateCheckDynamicalyFailed);
            return false;
        }
        function DuplicateCheckDynamicalySucceed(result) {

            var PriorityLabel = null;
            var IsDeclineStage = null;
            var IsCloseStage = null;

            if (result > 0) {
                isClose = 0;
                toastr.warning("Duplicate Attribute");                
                return false;
            }
            var Id = $("#ContentPlaceHolder1_hfId").val();
            var Name = '';

            var Description = '';
            var SetupType = $("#ContentPlaceHolder1_ddlSetupType").val();
            if (SetupType == "0") {
                isClose = 0;
                toastr.warning("Select Setup Type");
                $("#ContentPlaceHolder1_ddlSetupType").focus();
                return false;
            }

            Name = $("#ContentPlaceHolder1_txtName").val();
            Description = $("#ContentPlaceHolder1_txtDescription").val();
            if (Name == "") {
                isClose = 0;
                toastr.warning("Enter a Name");
                $("#ContentPlaceHolder1_txtName").focus();
                return false;
            }

            var Status = $("#ContentPlaceHolder1_ddlStatus").val();
            Status = Status == '1' ? true : false;

            var InvItemAttributeBO = {
                Id: Id,
                Name: Name,
                Description: Description,
                SetupType: SetupType,
                Status: Status
            }

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/InvItemAttributeSetupIframe.aspx/SaveInvItemAttributeSetup',
                data: JSON.stringify({ InvItemAttributeBO: InvItemAttributeBO }),
                dataType: "json",
                success: function (data) {
                    OnSuccessSaveOrUpdate(data.d);
                },
                error: function (result) {
                    isClose = 0;
                }
            });
            return false;
        }
        function DuplicateCheckDynamicalyFailed(error) {
            isClose = 0;
        }
        function OnSuccessSaveOrUpdate(result) {
            if (result.IsSuccess) {
                if (result.IsSuccess) {
                    parent.ShowAlert(result.AlertMessage);
                }
                parent.GridPaging(1, 1);
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
                isClose = 0;
            }
            //isContinueSave = false;
            return false;
        }
        function OnFailSaveOrUpdate(error) {
            toastr.error(error.get_message());
        }
        function SaveAndClose() {
            isClose = 1;
            //SaveOrUpdateTask();
            $.when(SaveOrUpdateSetup()).done(function () {

                if (isClose == 1) {
                    if (typeof parent.CloseDialog === "function") {
                        parent.CloseDialog();
                    }
                    if ($("#btnSave").val() == "Update and Close") {
                        $("#btnSave").val("Save And Close");
                        $("#btnSaveNContinue").show();
                        $("#btnClear").show();
                    }
                }
            });
            return false;
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
            return false;
        }
        function PerformClearAction() {
            $("#ContentPlaceHolder1_txtName").focus();
            $("#ContentPlaceHolder1_hfId").val('0');
            $("#ContentPlaceHolder1_txtName").val('');
            $("#ContentPlaceHolder1_txtDescription").val('');
            $("#ContentPlaceHolder1_ddlSetupType").val('0');
            $("#ContentPlaceHolder1_ddlStatus").val('1');
            return false;
        }
        function PerformEdit(id) {
            PageMethods.GetSetupById(id, OnSuccessLoading, OnFailLoading)
            return false;
        }
        function OnSuccessLoading(result) {
            FillForm(result);
            return false;
        }
        function OnFailLoading(error) {
            return false;
        }
        function FillForm(data) {
            $("#ContentPlaceHolder1_hfId").val(data.Id);
            $("#ContentPlaceHolder1_txtName").val(data.Name);
            $("#ContentPlaceHolder1_txtDescription").val(data.Description);
            $("#ContentPlaceHolder1_ddlSetupType").val(data.SetupType).trigger('change');
            var Status = data.Status == true ? 1 : 0;
            $("#ContentPlaceHolder1_ddlStatus").val(Status);
            $("#btnClear").hide();
            $("#btnSaveNContinue").hide();
            $("#btnSave").val('Update');
            return false;
        }
    </script>
    <asp:HiddenField ID="hfId" runat="server" Value="0" />
    <div style="padding: 10px 30px 10px 30px">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label required-field">Setup Type</label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlSetupType" CssClass="form-control" runat="server" Style="width: 100%;">
                        <asp:ListItem Value="0">---Please Select---</asp:ListItem>
                        <asp:ListItem Value="Color">Color</asp:ListItem>
                        <asp:ListItem Value="Size">Size</asp:ListItem>
                        <asp:ListItem Value="Style">Style</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div id="SupportSetup">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Name</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" AutoComplete="off"></asp:TextBox>
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
                            <asp:ListItem Value="1">Active</asp:ListItem>
                            <asp:ListItem Value="0">Inactive</asp:ListItem>
                        </asp:DropDownList>
                    </div>
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
                    onclick="javascript: return SaveOrUpdateSetup();" />
            </div>
        </div>
    </div>
</asp:Content>
