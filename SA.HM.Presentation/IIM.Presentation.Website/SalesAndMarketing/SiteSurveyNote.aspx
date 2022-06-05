<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="SiteSurveyNote.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.SiteSurveyNote" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>

        $(document).ready(function () {

            var siteSurveyNoteId = $.trim(CommonHelper.GetParameterByName("editId"));
            var CompanyId = $.trim(CommonHelper.GetParameterByName("cid"));
            var DealId = $.trim(CommonHelper.GetParameterByName("did"));
            var ContactId = $.trim(CommonHelper.GetParameterByName("conid"));

            $("#ContentPlaceHolder1_ddlCompany").change(function () {
                var selectedCompany = $('#ContentPlaceHolder1_ddlCompany').val();
                LoadDeal($(this).val(), 0);
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../SalesAndMarketing/SiteSurveyNote.aspx/LoadCompanyAddress',
                    async: false,
                    data: JSON.stringify({ Id: selectedCompany }),
                    dataType: "json",
                    success: function (data) {
                        $('#ContentPlaceHolder1_lblBillingStreet').html(data.d.BillingStreet);
                        $('#ContentPlaceHolder1_lblBillingCityId').html(data.d.BillingCity);
                        $('#ContentPlaceHolder1_lblBillingStateId').html(data.d.BillingState);
                        $('#ContentPlaceHolder1_lblBillingCountryId').html(data.d.BillingCountry);
                        $('#ContentPlaceHolder1_lblBillingPostCode').html(data.d.BillingPostCode);
                    },
                    error: function (result) {
                        CommonHelper.AlertMessage(result.d.AlertMessage);
                    }
                });

                return false;
            });
            $("#ContentPlaceHolder1_ddlContact").change(function () {
                LoadDeal(0, $("#ContentPlaceHolder1_ddlContact").val());
            });
            if (CompanyId != "0" && CompanyId != "") {
                $("ComapnyDiv").show();
                $("ContactDiv").hide();
                $('#ContentPlaceHolder1_cbCompanyOrContact').prop("checked", true).trigger('change').attr("disabled", true);
                $("#ContentPlaceHolder1_ddlCompany").val(CompanyId).trigger('change').attr("disabled", true);
                if (DealId != "0") {
                    LoadDeal(CompanyId, DealId);
                    $("#ContentPlaceHolder1_ddlDealName").val(DealId).attr("disabled", true);
                }
            }

            if (ContactId != "0" && ContactId != "") {
                $("ComapnyDiv").hide();
                $("ContactDiv").show();
                $('#ContentPlaceHolder1_cbCompanyOrContact').prop("checked", false).trigger('change').attr("disabled", true);
                $("#ContentPlaceHolder1_ddlContact").val(ContactId).trigger('change').attr("disabled", true);
                if (DealId != "0") {
                    $("#ContentPlaceHolder1_ddlDealName").val(DealId).trigger('change').attr("disabled", true);
                }
            }

            $('#ImagePanel').hide();

            if ($("#ContentPlaceHolder1_cbCompanyOrContact").is(":checked")) {
                $("#CompanyDiv").show()
                $("#ContactDiv").hide()
            }
            else {
                $("#CompanyDiv").hide()
                $("#ContactDiv").show()
            }
            $("#ContentPlaceHolder1_cbCompanyOrContact").change(function () {
                if ($(this).is(":checked")) {
                    $("#CompanyDiv").show()
                    $("#ContactDiv").hide()

                }
                else {
                    $("#CompanyDiv").hide()
                    $("#ContactDiv").show()
                }
            });
            $("#ContentPlaceHolder1_NoSiteSurvey").change(function () {
                if ($(this).is(":checked")) {
                    $("#SiteSurveyDiv").hide()

                }
                else {
                    $("#SiteSurveyDiv").show()
                }
            });
            //
            //if ($("#InnboardMessageHiddenField").val() != "") {
            //    CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
            //    $("#InnboardMessageHiddenField").val("");
            //}
            //$("#ContentPlaceHolder1_ddlCompany").change(function () {
            //    if ($(this).val() != "0") {
            //        LoadDeal($(this).val(), 0);
            //    }

            //});
            $("#ContentPlaceHolder1_ddlContact").change(function () {
                if ($(this).val() != "0") {
                    LoadDeal(0, $(this).val());
                }

            });
            $('#ContentPlaceHolder1_ddlDealName').change(function () {
                $('#ContentPlaceHolder1_hfSelectedDealId').val($('#ContentPlaceHolder1_ddlDealName').val());

            });

            if (siteSurveyNoteId != "") {
                FillForm(siteSurveyNoteId);
            }
            $("#ContentPlaceHolder1_cbCompanyOrContact").focus();
        });
        function FillForm(siteSurveyNoteId) {
            PageMethods.GetSiteSurveyNoteById(siteSurveyNoteId, GetSiteSurveyNoteSucceeded, GetSiteSurveyNoteFailed);
        }
        function GetSiteSurveyNoteSucceeded(result) {
            $("#ContentPlaceHolder1_btnSave").val("Update");

            if (result.IsSiteSurveyUnderCompany == true) {
                $("#ContentPlaceHolder1_cbCompanyOrContact").attr("checked", true).attr('disabled', true).trigger('change');
                $("#ContentPlaceHolder1_ddlCompany").val(result.CompanyId).trigger('change').attr('disabled', true);
            }
            else {
                $("#ContentPlaceHolder1_cbCompanyOrContact").attr("checked", false).attr('disabled', true).trigger('change');
                $("#ContentPlaceHolder1_ddlContact").val(result.ContactId).trigger('change').attr('disabled', true);;
            }
            $("#ContentPlaceHolder1_ddlDealName").val(result.DealId);

            if (result.IsDealNeedSiteSurvey) {
                $("#ContentPlaceHolder1_NoSiteSurvey").attr("checked", true).attr('disabled', false).trigger('change');
            }
            $("#ContentPlaceHolder1_txtDescription").val(result.Description);
            $("#ContentPlaceHolder1_ddlSurveyFor").val(result.SegmentId);
            $("#ContentPlaceHolder1_hfId").val(result.Id);
            //$("#ContentPlaceHolder1_RandomDocId").val(result.Id);

            ShowUploadedDocument($("#ContentPlaceHolder1_RandomDocId").val());
            return false;
        }
        function GetSiteSurveyNoteFailed(error) {
            CommonHelper.AlertMessage(error.AlertMessage);
        }
        function LoadDeal(companyId, contactId) {
            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/SiteSurveyNote.aspx/GetDealByCompanyId',
                async: false,
                data: JSON.stringify({ companyId: companyId, contactId: contactId }),
                dataType: "json",
                success: function (data) {
                    var control = $('#ContentPlaceHolder1_ddlDealName');
                    control.empty();
                    if (data.d != null) {
                        if (data.d.length > 0) {
                            if (data.d.length > 1)
                                control.empty().append('<option value="0">---Please Select---</option>');
                            for (i = 0; i < data.d.length; i++) {
                                control.append('<option title="' + data.d[i].Name + '" value="' + data.d[i].Id + '">' + data.d[i].Name + '</option>');
                            }
                        }
                        else {
                            control.empty().append('<option selected="selected" value="0">---Please Select---</option>');
                        }
                    }
                    if (data.d.length == 1 && $("#ContentPlaceHolder1_hfDealId").val() == "0")
                        control.val($("#ContentPlaceHolder1_ddlDealName option:first").val()).trigger('change');
                    else
                        control.val($("#ContentPlaceHolder1_hfDealId").val());

                    $("#ContentPlaceHolder1_hfSelectedDealId").val(control.val());

                    return false;
                },
                error: function (result) {
                    CommonHelper.AlertMessage(result.d.AlertMessage);
                }
            });
            return false;
        }


        function LoadDocUploader() {
            $("#popUpImage").dialog({
                width: 650,
                height: 300,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "", // TODO add title
                show: 'slide'
            });
            return false;
        }
        function UploadComplete() {
            var randomId = $("#ContentPlaceHolder1_RandomDocId").val();
            ShowUploadedDocument(randomId);
        }

        function ShowUploadedDocument(randomId) {
            var id = $("#ContentPlaceHolder1_hfId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            PageMethods.GetUploadedDocByWebMethod(randomId, id, deletedDoc, OnGetUploadedDocByWebMethodSucceeded, OnGetUploadedDocByWebMethodFailed);
            return false;
        }

        function OnGetUploadedDocByWebMethodSucceeded(result) {
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            var DocTable = "";

            DocTable += "<table id='DocTableList' style='width:100%' class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            DocTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    DocTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    DocTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }

                DocTable += "<td align='left' style='width: 50%'>" + result[row].Name + "</td>";

                if (result[row].Path != "") {
                    imagePath = "<img src='" + result[row].Path + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                DocTable += "<td align='left' style='width: 30%'>" + imagePath + "</td>";

                DocTable += "<td align='left' style='width: 20%'>";
                DocTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteDoc('" + result[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                DocTable += "</td>";
                DocTable += "</tr>";
            }
            DocTable += "</table>";

            docc = DocTable;

            $("#DocumentInfo").html(DocTable);

            return false;
        }
        function DeleteDoc(docId, rowIndex) {
            var deletedDoc = $("#<%=hfDeletedDoc.ClientID %>").val();

            if (deletedDoc != "")
                deletedDoc += "," + docId;
            else
                deletedDoc = docId;

            $("#<%=hfDeletedDoc.ClientID %>").val(deletedDoc);

            $("#trdoc" + rowIndex).remove();
        }
        function OnGetUploadedDocByWebMethodFailed(error) {
            alert(error.get_message());
        }
        function PerformClearWithConfirmation() {
            if (!confirm("Do you want to Clear?")) {
                return false;
            }
            PerformClear();
        }
        function PerformClear() {

            $("#ContentPlaceHolder1_ddlCompany").val("0").trigger('Change').attr('disabled', false);;
            $("#ContentPlaceHolder1_NoSiteSurvey").prop("checked", false).trigger('Change');
            $("#ContentPlaceHolder1_txtDescription").val("");
            $("#ContentPlaceHolder1_ddlSurveyFor").val("0");
            $("#ContentPlaceHolder1_hfDealId").val("0");
            $("#ContentPlaceHolder1_hfSelectedDealId").val("0");
            $("#ContentPlaceHolder1_hfId").val("0");
            $("#ContentPlaceHolder1_hfDeletedDoc").val("0");
            $("#ContentPlaceHolder1_hfDeletedDoc").val("0");
            $("#ContentPlaceHolder1_btnSave").val("Save");
            $("#ContentPlaceHolder1_cbCompanyOrContact").attr("checked", false).attr('disabled', false).trigger('change');
            $("#ContentPlaceHolder1_ddlContact").val("0").trigger('change').attr('disabled', false);

            DocTable = "";
            return false;
        }
        function PerformSave() {
            var id = $("#ContentPlaceHolder1_hfId").val();
            var companyId = $("#ContentPlaceHolder1_ddlCompany").val();
            var contactId = $("#ContentPlaceHolder1_ddlContact").val();
            var dealId = $("#ContentPlaceHolder1_ddlDealName").val();
            var isDealNeedSiteSurvey = !$("#ContentPlaceHolder1_NoSiteSurvey").is(":checked");
            var isSiteSurveyUnderCompany = $("#ContentPlaceHolder1_cbCompanyOrContact").is(":checked");
            var description = $("#ContentPlaceHolder1_txtDescription").val();
            var segmentId = $("#ContentPlaceHolder1_ddlSurveyFor").val();
            if (isSiteSurveyUnderCompany == true && companyId == "0") {
                toastr.warning("Please Select a Company");
                $("#ContentPlaceHolder1_ddlCompany").focus();
                return false;
            }
            if (isSiteSurveyUnderCompany == false && contactId == "0") {
                toastr.warning("Please Select a Contact");
                $("#ContentPlaceHolder1_ddlContact").focus();
                return false;
            }
            if (dealId == "0") {
                toastr.warning("Please Select a Deal");
                $("#ContentPlaceHolder1_ddlDealName").focus();
                return false;
            }
            if (isDealNeedSiteSurvey && (segmentId == "0")) {
                toastr.warning("Please Select a Site Survey For");
                $("#ContentPlaceHolder1_ddlSurveyFor").focus();
                return false;
            }
            if (isSiteSurveyUnderCompany == true) {
                contactId = 0;
            }
            else {
                companyId = 0;
            }
            var randomDocId = $("#ContentPlaceHolder1_RandomDocId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            var sMSiteSurveyNoteBO = {
                Id: id,
                CompanyId: companyId,
                DealId: dealId,
                IsDealNeedSiteSurvey: isDealNeedSiteSurvey,
                Description: description,
                SegmentId: segmentId,
                IsSiteSurveyUnderCompany: isSiteSurveyUnderCompany,
                ContactId: contactId
            }

            PageMethods.SaveSiteSurveyNote(sMSiteSurveyNoteBO, parseInt(randomDocId), deletedDoc, OnSaveSiteSurveyNoteSucced, OnSaveSiteSurveyNoteFailed)
            return false;
        }
        function OnSaveSiteSurveyNoteSucced(result) {
            UploadComplete();
            PerformClear();
            $("#ContentPlaceHolder1_RandomDocId").val(result.data);
            parent.ShowAlert(result.AlertMessage);
            parent.CloseDialog();
            if (typeof parent.GridPaging === "function")
                parent.GridPaging(1, 1);
            return false;
        }
        function OnSaveSiteSurveyNoteFailed(error) {
            CommonHelper.AlertMessage(error.AlertMessage);
        }

    </script>
    <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="tempDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfParentDoc" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />
    <asp:HiddenField ID="hfDealId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSelectedDealId" runat="server" Value="0" />
    
    <div class="panel panel-default">
        <div class="panel-heading">
            Site Survey Note
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2"></div>
                    <div class="col-md-5">
                        <div class="col-md-1">
                            <asp:CheckBox ID="cbCompanyOrContact" runat="server" CssClass="mycheckbox" />
                        </div>
                        <div class="col-md-11">
                            <label class="control-label required-field">Is Site Survey Under Company?</label>
                        </div>
                    </div>
                </div>
                <div id="CompanyDiv" style="display: none">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Company Name</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <fieldset>
                            <legend>Company Address</legend>
                            <div class="form-group">
                                <label class="control-label col-md-2 ">Street</label>
                                <div class="col-sm-10">
                                    <asp:Label ID="lblBillingStreet" runat="server" CssClass="form-control"
                                        ></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-md-2 ">City</label>
                                <div class="col-sm-10">
                                    <asp:Label ID="lblBillingCityId" runat="server" CssClass="form-control" >
                                    </asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-md-2 ">State</label>
                                <div class="col-sm-10">
                                    <asp:Label ID="lblBillingStateId" runat="server" CssClass="form-control" >
                                    </asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-md-2 ">Country</label>
                                <div class="col-sm-10">
                                    <asp:Label ID="lblBillingCountryId" runat="server" CssClass="form-control" >
                                    </asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-md-2 ">Post Code</label>
                                <div class="col-sm-4">
                                    <asp:Label ID="lblBillingPostCode" runat="server" CssClass="form-control"
                                        ></asp:Label>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div id="ContactDiv" style="display: none">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Contact</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlContact" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Deal Name</label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlDealName" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2"></div>
                    <div class="col-md-5">
                        <div class="col-md-1">
                            <asp:CheckBox ID="NoSiteSurvey" runat="server" CssClass="mycheckbox" />
                        </div>
                        <div class="col-md-11">
                            <label class="control-label required-field">This deal need no site survey</label>
                        </div>
                    </div>
                </div>
                <div id="SiteSurveyDiv">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Description</label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="4" CssClass="quantity form-control"  Style="resize: none;"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Survey For</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlSurveyFor" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Attachment</label>
                        </div>
                        <div class="col-md-4">
                            <input id="btnImageUp"  type="button" onclick="javascript: return LoadDocUploader();"
                                class="TransactionalButton btn btn-primary btn-sm" value="Site Survey Doc..." />
                        </div>
                    </div>
                    <div id="DocumentInfo">
                    </div>
                </div>
                <div class="row" id="SubmitButtonDiv" style="padding-top: 10px;">
                    <div class="col-md-12">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm" OnClientClick="return PerformSave();" />
                        <asp:Button ID="btnCancel" OnClientClick="return PerformClear();" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="popUpImage" style="display: none">
        <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
            <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
        </asp:Panel>
    </div>

</asp:Content>
