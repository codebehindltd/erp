<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="NewSetupIframe.aspx.cs" Inherits="HotelManagement.Presentation.Website.SupportAndTicket.NewSetupIframe" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var isClose = 0;
        $(document).ready(function () {
            var setupType = $.trim(CommonHelper.GetParameterByName("sType"));
            var id = $.trim(CommonHelper.GetParameterByName("sid"));
            $("#ContentPlaceHolder1_ddlSetupType").change(function () {
                if ($(this).val() == 'SupportPriority') {
                    $("#PriorityLabelDiv").show();
                    $("#SupportStage").hide();
                    $("#SupportSetup").show();

                }
                else if ($(this).val() == 'SupportStage') {
                    $("#SupportStage").show();
                    $("#SupportSetup").hide();
                }
                else {
                    $("#PriorityLabelDiv").hide();
                    $("#SupportStage").hide();
                    $("#SupportSetup").show();
                }
            });
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
            var id = $("#ContentPlaceHolder1_hfId").val();
            var setupType = $("#ContentPlaceHolder1_ddlSetupType").val();
            if (setupType == "0") {
                toastr.warning("Please Select Setup Type");
                $("#ContentPlaceHolder1_ddlSetupType").focus();
                isClose = 0;
                return false;
            }
            if (result > 0) {
                toastr.warning("Duplicate Support Case Name");
                $("#ContentPlaceHolder1_txtName").focus();
                isClose = 0;
                return false;
            }
            var PriorityLabel = null;
            if (setupType == 'SupportStage')
                PriorityLabel = $("#ContentPlaceHolder1_txtDisplaySequence").val();
            else
                PriorityLabel = $("#ContentPlaceHolder1_txtPriorityLabel").val();
            if (id == 0) {
                var IsUpdate = 0;
            }
            else {
                IsUpdate = 1;
            }
            if (result > 0) {
                toastr.warning("Duplicate Support Case Name");
                $("#ContentPlaceHolder1_txtName").focus();
                isClose = 0;
                return false;
            }
            else if ((setupType == "SupportPriority" || setupType == 'SupportStage') && PriorityLabel == '') {
                if ($("#ContentPlaceHolder1_ddlSetupType").val() == 'SupportStage') {
                    toastr.warning("Please Add Display Sequence");
                }
                else if ($("#ContentPlaceHolder1_ddlSetupType").val() == 'SupportPriority') {
                    toastr.warning("Please Add Priority Level");
                }

                $("#ContentPlaceHolder1_txtPriorityLabel").focus();
                isClose = 0;
                return false;

            }
            else if (PriorityLabel == '') {
                DuplicateCheckSequenceSucceed(0);
            }
            else {
                if (setupType == 'SupportStage')
                    PriorityLabel = $("#ContentPlaceHolder1_txtDisplaySequence").val();
                else
                    PriorityLabel = $("#ContentPlaceHolder1_txtPriorityLabel").val();
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../SupportAndTicket/NewSetupIframe.aspx/DuplicateCheckDynamicaly',
                    data: JSON.stringify({ fieldName1: "PriorityLabel", fieldValue1: PriorityLabel, fieldName2: "SetupType", fieldValue2: setupType, isUpdate: IsUpdate, id: id }),
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        DuplicateCheckSequenceSucceed(data.d);
                    },
                    error: function (result) {
                        isClose = 0;
                        DuplicateCheckSequenceFailed(result)
                    }
                });
                return false;
                //PageMethods.DuplicateCheckDynamicaly("PriorityLabel", PriorityLabel, "SetupType", setupType, IsUpdate, id, DuplicateCheckSequenceSucceed, DuplicateCheckSequenceFailed);
                //return false;
            }
            function DuplicateCheckSequenceSucceed(result) {
                var PriorityLabel = null;
                var IsDeclineStage = null;
                var IsCloseStage = null;
                if (result > 0) {
                    if ($("#ContentPlaceHolder1_ddlSetupType").val() == 'SupportStage') {
                        toastr.warning("Duplicate Display Sequence");
                    }
                    else if ($("#ContentPlaceHolder1_ddlSetupType").val() == 'SupportPriority') {
                        toastr.warning("Duplicate Priority Level");
                    }

                    $("#ContentPlaceHolder1_txtPriorityLabel").focus();
                    isClose = 0;
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
                if (SetupType == 'SupportStage') {
                    Name = $("#ContentPlaceHolder1_txtSupportStage").val();
                    Description = $("#ContentPlaceHolder1_txtStageDescription").val();
                }
                else {
                    Name = $("#ContentPlaceHolder1_txtName").val();
                    Description = $("#ContentPlaceHolder1_txtDescription").val();
                }
                if (Name == "") {
                    isClose = 0;
                    toastr.warning("Enter a Name");
                    $("#ContentPlaceHolder1_txtName").focus();
                    return false;
                }

                var Status = $("#ContentPlaceHolder1_ddlStatus").val();
                Status = Status == '1' ? true : false;

                PriorityLabel = $("#ContentPlaceHolder1_txtPriorityLabel").val();
                if (PriorityLabel != '' || PriorityLabel != null) {
                    if (SetupType == 'SupportStage')
                        PriorityLabel = $("#ContentPlaceHolder1_txtDisplaySequence").val();
                    else
                        PriorityLabel = $("#ContentPlaceHolder1_txtPriorityLabel").val();
                }
                else {
                    PriorityLabel = null;
                }
                if (SetupType == 'SupportStage') {
                    IsDeclineStage = $("#ContentPlaceHolder1_cbDeclineStage").is(":checked");
                    IsCloseStage = $("#ContentPlaceHolder1_cbCloseStage").is(":checked");
                    Status = true;
                }

                var STSupportNCaseSetupInfoBO = {
                    Id: Id,
                    Name: Name,
                    Description: Description,
                    SetupType: SetupType,
                    Status: Status,
                    PriorityLabel: PriorityLabel,
                    IsCloseStage: IsCloseStage,
                    IsDeclineStage: IsDeclineStage
                }

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../SupportAndTicket/NewSetupIframe.aspx/SaveOrUpdateSupportNCaseSetupInfo',
                    data: JSON.stringify({ SupportNCaseSetupInfo: STSupportNCaseSetupInfoBO }),
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
        }
        function DuplicateCheckSequenceFailed(error) {
            isClose = 0;
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
        }
        function PerformClearAction() {
            $("#ContentPlaceHolder1_txtName").focus();
            $("#ContentPlaceHolder1_hfId").val('0');
            $("#ContentPlaceHolder1_txtName").val('');
            $("#ContentPlaceHolder1_txtDescription").val('');
            $("#ContentPlaceHolder1_ddlSetupType").val('0');
            $("#ContentPlaceHolder1_ddlStatus").val('1');
            $("#ContentPlaceHolder1_txtPriorityLabel").val('');

            $("#ContentPlaceHolder1_txtSupportStage").val('');
            $("#ContentPlaceHolder1_txtDisplaySequence").val('');
            $("#ContentPlaceHolder1_txtStageDescription").val('');
            //$("#ContentPlaceHolder1_cbDeclineStage").attr("checked", false);
            //$("#ContentPlaceHolder1_cbCloseStage").attr("checked", false);

            if ($("#ContentPlaceHolder1_ddlSetupType").val() == 'SupportPriority') {
                $("#PriorityLabelDiv").show();
                $("#SupportStage").hide();
                $("#SupportSetup").show();

            }
            if ($("#ContentPlaceHolder1_ddlSetupType").val() == 'SupportStage') {
                $("#SupportStage").show();
                $("#SupportSetup").hide();
            }
            else {
                $("#PriorityLabelDiv").hide();
                $("#SupportStage").hide();
                $("#SupportSetup").show();
            }
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
            if (data.SetupType == 'SupportPriority')
                $("#ContentPlaceHolder1_txtPriorityLabel").val(data.PriorityLabel);
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
                        <asp:ListItem Value="Case">Case</asp:ListItem>
                        <asp:ListItem Value="SupportStage">Support Stage</asp:ListItem>
                        <asp:ListItem Value="SupportType">Support Type</asp:ListItem>
                        <asp:ListItem Value="SupportCategory">Support Category</asp:ListItem>
                        <asp:ListItem Value="SupportPriority">Support Priority</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div id="SupportSetup">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Name</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
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
                <div class="form-group" id="PriorityLabelDiv" style="display: none">
                    <div class="col-md-2">
                        <label class="control-label required-field">Priority Level</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPriorityLabel" runat="server" CssClass="quantity form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div id="SupportStage" style="display: none">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Support Stage</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtSupportStage" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Display Sequence</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtDisplaySequence" runat="server" CssClass="quantity form-control" TabIndex="5"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Description</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtStageDescription" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" TabIndex="6" Style="resize: none;"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2"></div>
                    <div class="col-md-8">
                        <div class="col-md-12">
                            <asp:CheckBox ID="cbDeclineStage" runat="server" />
                            It's Decline Stage
                        </div>

                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2"></div>
                    <div class="col-md-8">
                        <div class="col-md-12">
                            <asp:CheckBox ID="cbCloseStage" runat="server" />
                            It's Close Stage
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
    </div>
</asp:Content>
