<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="SupportStageSetupIframe.aspx.cs" Inherits="HotelManagement.Presentation.Website.SupportAndTicket.SupportStageSetupIframe" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var sClose = false;
        $(document).ready(function () {
            var id = $.trim(CommonHelper.GetParameterByName("sid"));

            if (id != "") {
                PerformEdit(id);
            }
            CommonHelper.ApplyIntigerValidation();
        });
        function SaveOrUpdateStage() {
            var Id = $("#ContentPlaceHolder1_hfId").val();
            var Name = $("#ContentPlaceHolder1_txtSupportStage").val();
            if (Id == 0) {
                var IsUpdate = 0;
            }
            else {
                IsUpdate = 1;
            }
            PageMethods.DuplicateCheckDynamicaly("Name", Name, IsUpdate, Id, DuplicateCheckDynamicalySucceed, DuplicateCheckDynamicalyFailed);
            return false;

        }
        function DuplicateCheckDynamicalySucceed(result) {
            var id = $("#ContentPlaceHolder1_hfId").val();

            if (id == 0) {
                var IsUpdate = 0;
            }
            else {
                IsUpdate = 1;
            }

            if (result > 0) {
                toastr.warning("Duplicate Support Case Name");
                $("#ContentPlaceHolder1_txtSupportStage").focus();
                isClose = false;
                return false;
            }
            else {
                var PriorityLabel = +$("#ContentPlaceHolder1_txtDisplaySequence").val();
                PageMethods.DuplicateCheckDynamicaly("PriorityLabel", PriorityLabel, IsUpdate, id, DuplicateCheckSequenceSucceed, DuplicateCheckSequenceFailed);
                return false;

            }
            function DuplicateCheckSequenceSucceed(result) {
                if (result > 0) {
                    toastr.warning("Duplicate Priority Level");
                    $("#ContentPlaceHolder1_txtPriorityLabel").focus();
                    isClose = false;
                    return false;
                }
                var Id = $("#ContentPlaceHolder1_hfId").val();
                var Name = $("#ContentPlaceHolder1_txtSupportStage").val();
                if (Name == "") {
                    isClose = false;
                    toastr.warning("Enter a Support Stage");
                    $("#ContentPlaceHolder1_txtSupportStage").focus();
                    return false;
                }
                var Description = $("#ContentPlaceHolder1_txtDescription").val();
                var SetupType = "SupportStage";
                var Status = 1;
                Status = Status == 1 ? true : false;
                var PriorityLabel = $("#ContentPlaceHolder1_txtDisplaySequence").val();
                var IsDeclineStage = $("#ContentPlaceHolder1_cbDeclineStage").is(":checked");
                var IsCloseStage = $("#ContentPlaceHolder1_cbCloseStage").is(":checked");
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
                    url: '../SupportAndTicket/SupportStageSetupIframe.aspx/SaveOrUpdateSupportNCaseSetupInfo',

                    data: JSON.stringify({ SupportNCaseSetupInfo: STSupportNCaseSetupInfoBO }),
                    dataType: "json",
                    success: function (data) {
                        OnSuccessSaveOrUpdate(data.d);
                    },
                    error: function (result) {
                        isClose = false;
                    }
                });
                return false;
            }
        }
        function DuplicateCheckSequenceFailed(error) {
            isClose = false;
        }

        function DuplicateCheckDynamicalyFailed(error) {
            isClose = false;
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
                isClose = false;
            }
            //isContinueSave = false;
            return false;
        }

        function OnFailSaveOrUpdate(error) {
            toastr.error(error.get_message());
        }
        function SaveAndClose() {
            isClose = true;
            $.when(SaveOrUpdateStage()).done(function () {
                if (isClose) {

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
            $("#ContentPlaceHolder1_txtSupportStage").val(data.Name);
            $("#ContentPlaceHolder1_txtDescription").val(data.Description);            
            $("#ContentPlaceHolder1_txtDisplaySequence").val(data.PriorityLabel);
            if (data.IsDeclineStage)
                $('#ContentPlaceHolder1_cbDeclineStage').prop("checked", true).trigger('change');
            if (data.IsCloseStage)
                $('#ContentPlaceHolder1_cbCloseStage').prop("checked", true).trigger('change');
            $("#btnClear").hide();
            $("#btnSaveNContinue").hide();
            $("#btnSave").val('Update');
            return false;
        }

        function PerformClearAction() {
            $("#ContentPlaceHolder1_hfId").val('0');
            $("#ContentPlaceHolder1_txtSupportStage").val('');
            $("#ContentPlaceHolder1_txtDescription").val('');
            $("#ContentPlaceHolder1_txtDisplaySequence").val('');
            $('#ContentPlaceHolder1_cbDeclineStage').prop("checked", false).trigger('change');
            $('#ContentPlaceHolder1_cbCloseStage').prop("checked", false).trigger('change');
        }
    </script>
    <asp:HiddenField ID="hfId" runat="server" Value="0" />
    <div class="form-horizontal">
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
                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" TabIndex="6" Style="resize: none;"></asp:TextBox>
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

        <div class="row" style="padding-bottom: 0; padding-top: 10px;">
            <div class="col-md-12">
                <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="SaveAndClose()" value="Save & Close" />
                <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="javascript: return PerformClearAction();" />
                <input id="btnSaveNContinue" type="button" value="Save & Continue" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="javascript: return SaveOrUpdateStage();" />
            </div>
        </div>
    </div>
</asp:Content>
