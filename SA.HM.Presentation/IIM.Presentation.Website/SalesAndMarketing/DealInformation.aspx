<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="DealInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.DealInformation" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var dealId = "", companyId = "", contactId = "";
        var DealContacts;
        var Contacts = [];
        $(document).ready(function () {
            if ($.trim(CommonHelper.GetParameterByName("did")) != "")
                dealId = parseInt($.trim(CommonHelper.GetParameterByName("did")), 10);
            else
                dealId = 0;

            if ($.trim(CommonHelper.GetParameterByName("cid")) != "")
                companyId = parseInt($.trim(CommonHelper.GetParameterByName("cid")), 10);
            else
                companyId = 0;

            if ($.trim(CommonHelper.GetParameterByName("conid")) != "")
                contactId = parseInt($.trim(CommonHelper.GetParameterByName("conid")), 10);
            else
                contactId = 0;
            $('#ContentPlaceHolder1_ddlContacts').select2();

            DealContacts = $("#tblDealContacts").DataTable({
                data: [],
                columns: [
                    { title: "Name", data: "Name", width: "40%" },
                    { title: "Title", data: "Title", width: "40%" },
                    { title: "Action", data: null, width: "20%" }
                ],
                columnDefs: [
                    {
                        "targets": 0,
                        "render": function (data, type, full, meta) {
                            return "<a href='javascript:void();'style='color:#333333;' onclick= 'GoToDetails(" + full.ContactId + ")' >" + full.Name + "</a>";
                        }
                    }],
                rowCallback: (row, data, displayNum, displayIndex, dataIndex) => {
                    $('td:eq(' + (row.children.length - 1) + ')', row).html("&nbsp;&nbsp;<a href='javascript:void();' onclick= 'DeleteContact(this)' ><img style='cursor:pointer;' alt='Delete' src='../Images/delete.png' /></a>");
                },
                info: false,
                ordering: false,
                processing: false,
                paging: false,
                filter: false,
                language: {
                    emptyTable: "No Data Found"
                }
            });

            if (dealId > 0) {
                FillForm(dealId);
            }
            LoadLog();

        });
        function GoToCompanyLink() {
            var companyId = +$("#ContentPlaceHolder1_hfCompanylId").val();
            if (companyId > 0) {
                var url = "./CompanyInformation.aspx?id=" + companyId;
                window.location = url;
                return true;
            }
        }
        function GoToDetails(id) {
            window.location.href = "./ContactInformation.aspx?conid=" + id;
        }

        function FillForm(id) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../../../SalesAndMarketing/DealInformation.aspx/FillForm",
                dataType: "json",
                data: JSON.stringify({ id: id }),
                async: false,
                success: (data) => {
                    LoadDealInformation(data.d);
                    LoadDealAttachments(id);
                },
                error: (error) => {
                    toastr.error(error, "", { timeOut: 5000 });
                }
            });
        }

        function LoadDealInformation(deal) {
            $("#ContentPlaceHolder1_hfCompanylId").val(deal.CompanyId);
            $("#ContentPlaceHolder1_lblDealName").text(deal.Name);
            $("#ContentPlaceHolder1_lblDealOwner").text(deal.Owner);
            $("#ContentPlaceHolder1_lblDealStage").text(deal.Stage);
            $("#ContentPlaceHolder1_lblDealAmount").text(deal.Amount);
            if (deal.CompanyId == 0) {
                if (deal.Contacts.length > 0)
                    $("#btnAddContact").hide();
                else
                    $("#btnAddContact").show();
            }
            else {
                $("#CompanyDiv").show();
                $("#btnAddContact").show();
                $("#ContentPlaceHolder1_lblCompanyName").text(deal.Company);
                $("#ContentPlaceHolder1_lblIndustry").text(deal.Industry);
                $("#ContentPlaceHolder1_lblPhone").text(deal.Phone);
                $("#ContentPlaceHolder1_lblEmail").text(deal.Email);
                $("#ContentPlaceHolder1_lblWebsite").text(deal.Website);
                $("#ContentPlaceHolder1_lblLifeCycleStage").text(deal.LifeCycleStage);
            }

            var address = "";
            if (deal.ShippingStreet != "")
                address = deal.ShippingStreet + " , " + deal.ShippingState + " , " + deal.ShippingCity + " , Postal Code: " + deal.ShippingPostCode + " , " + deal.ShippingCountry;

            $("#ContentPlaceHolder1_lblAddress").text(address);
            $("#ContentPlaceHolder1_lblProbability").text(deal.Complete);
            if (deal.IsCloseWon)
                $("#ImplementationFeedbackNStatus").show();
            else
                $("#ImplementationFeedbackNStatus").hide();
            $("#ContentPlaceHolder1_lblImplementationFeedbackNStatus").text(deal.ImplementationStatus + (deal.ImplementationFeedback != "" ? " [" + deal.ImplementationFeedback + "]" : ""));

            //$("#ContentPlaceHolder1_lblLastActivityDateTime").text(moment(deal.LastActivityDateTime).format("DD MMM YYYY") + " at " + moment(deal.LastActivityDateTime).format("hh:mm A"));
            Contacts = deal.Contacts;
            DealContacts.clear();
            DealContacts.rows.add(deal.Contacts);
            DealContacts.draw();

            if (deal.Contacts.length > 0)
                $("#btnAddContact").val("Add Another Contact");
            else
                $("#btnAddContact").val("Add Contact");

            //LoadContactsDropdown(deal.CompanyContacts);
            return false;
        }

        function LoadContactsDropdown(results) {
            $("#ContentPlaceHolder1_ddlContacts").empty();
            results = results.filter(i => !Contacts.some(j => j.ContactId == i.Id));
            var i = 0, fieldLength = results.length;

            if (fieldLength > 0) {
                for (i = 0; i < fieldLength; i++) {
                    $('<option value="' + results[i].Id + '">' + results[i].Name + '</option>').appendTo('#ContentPlaceHolder1_ddlContacts');
                }
            }
            else {
                $("<option value='0'>--No Contact Found--</option>").appendTo("#ContentPlaceHolder1_ddlContacts");
            }
            if (fieldLength == 1)
                $("#ContentPlaceHolder1_ddlContacts").val($("#ContentPlaceHolder1_ddlContacts option:first").val()).trigger('change');
        }

        function DeleteContact(item) {
            if (confirm("Want to delete?")) {
                var row = $(item).parents('tr');

                var id = DealContacts.row(row).data().Id;
                DealContacts.row(row).remove().draw();
                PageMethods.DeleteContact(id, OnDeleteSuccess, OnDeleteFailed);
            }
            return false;
        }

        function OnDeleteSuccess(result) {
            if (result.IsSuccess) {
                var id = +$("#ContentPlaceHolder1_hfDealId").val();
                FillForm(id);
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }

        function OnDeleteFailed(error) {
            toastr.error(error, "", { timeOut: 5000 });
        }

        function LoadLog() {
            if (dealId != 0) {
                var iframeid = 'logDetails';
                var url = "./LogActivity.aspx?did=" + dealId;
                parent.document.getElementById(iframeid).src = url;
            }
        }
        function LogEntry() {
            var logId = $("#hfLogId").val();
            debugger;
            var iframeid = 'logDoc';
            var url = "./SalesCallEntry.aspx?id=" + logId + "&did=" + dealId + "&cid=" + companyId + "&ctid=" + contactId;
            parent.document.getElementById(iframeid).src = url;

            $("#LogEntryPage").dialog({
                autoOpen: true,
                modal: true,
                width: 1300,
                height: 700,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Log Entry",
                show: 'slide',
                open: function (event, ui) {
                    $('#LogEntryPage').css('overflow', 'hidden');
                }
            });
        }
        function DialogCloseAfterUpdate() {
            $("#LogEntryPage").dialog("close");
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }
        function GoBack() {
            var isFirefox = typeof InstallTrigger !== 'undefined';
            if (isFirefox) {
                window.history.go(-2);
            }
            else
                window.history.back();
            return true;
        }

        function GoEdit() {
            var name = $("#<%=lblDealName.ClientID %>").text();
            //var dealId = +$("#ContentPlaceHolder1_hfContactId").val();

            var iframeid = 'frmPrint';
            var url = "./Deal.aspx?did=" + dealId;
            parent.document.getElementById(iframeid).src = url;

            $("#CreateContactDialog").dialog({
                autoOpen: true,
                modal: true,
                width: 1300,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Update - " + name,
                show: 'slide'
            });
        }

        function LoadDealAttachments(id) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../../../SalesAndMarketing/Deal.aspx/LoadDealDocument",
                dataType: "json",
                data: JSON.stringify({ id: id, randomId: 0, deletedDoc: "" }),
                async: false,
                success: (data) => {
                    OnLoadDocumentSucceeded(data.d);
                },
                error: (error) => {
                    toastr.error(error.d.get_message());
                }
            });
        }

        function OnLoadDocumentSucceeded(result) {
            var guestDoc = result;

            if (result.length > 0)
                $("#btnAttachment").val("Add Another Attachment");
            else
                $("#btnAttachment").val("Add Attachment");

            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";

            guestDocumentTable += "<table id='dealDocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            guestDocumentTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }

                guestDocumentTable += "<td align='left' style='width: 50%'>";
                guestDocumentTable += "<a style='color:#333333;' target='_blank' href='" + guestDoc[row].Path + guestDoc[row].Name + "'>"
                guestDocumentTable += guestDoc[row].Name + "</a></td>";

                if (guestDoc[row].Path != "") {

                    imagePath = "<a target='_blank' href='" + guestDoc[row].Path + guestDoc[row].Name + "'>"

                    if (guestDoc[row].Extention == ".jpg")
                        imagePath += "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    else
                        imagePath += "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";

                    imagePath += "</a>";
                }
                else
                    imagePath = "";

                guestDocumentTable += "<td align='left' style='width: 30%'>" + imagePath + "</td>";
                guestDocumentTable += "<td align='left' style='width: 20%'>";
                guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer;\" onClick=\"javascript:return DeleteDealDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                guestDocumentTable += "</td>";
                guestDocumentTable += "</tr>";
            }
            guestDocumentTable += "</table>";

            $("#DealDocumentInfo").html(guestDocumentTable);
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }
        function LoadDocUploader() {

            //var randomId = +$("#ContentPlaceHolder1_RandomDealId").val();
            var path = "/SalesAndMarketing/Images/Deal/";
            var category = "SalesDealDocuments";
            var iframeid = 'frmPrintDoc';
            //var url = "/HMCommon/FileUploadTest.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            var url = "/Common/FileUpload.aspx?Path=" + path + "&OwnerId=" + dealId + "&Category=" + category;
            document.getElementById(iframeid).src = url;

            $("#DocumentDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 300,
                closeOnEscape: false,
                resizable: false,
                title: "Documents Upload",
                show: 'slide'
            });

        }
        function AttachFile() {
            $("#dealdocuments").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Deal Documents",
                show: 'slide'
            });
        }

        function UploadComplete() {
            var id = +$("#ContentPlaceHolder1_hfDealId").val();
            LoadDealAttachments(id);
        }

        function AddContact() {

            var companyId = +$("#ContentPlaceHolder1_hfCompanylId").val();

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../../../SalesAndMarketing/DealInformation.aspx/GetCompanyContacts",
                dataType: "json",
                data: JSON.stringify({ companyId: companyId }),
                async: false,
                success: (data) => {
                    LoadContactsDropdown(data.d);

                },
                error: (error) => {
                    toastr.error(error, "", { timeOut: 5000 });
                }
            });

            $("#ContactDialog").dialog({
                autoOpen: true,
                modal: true,
                width: 750,
                minWidth: 550,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Add Contact",
                show: 'slide'
            });
        }

        function DeleteDealDoc(docId, rowIndex) {
            if (confirm("Want to delete?")) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "../../../SalesAndMarketing/Deal.aspx/DeleteDealDocument",
                    dataType: "json",
                    data: JSON.stringify({ deletedDocumentId: docId }),
                    async: false,
                    success: (data) => {
                        CommonHelper.AlertMessage(data.d.AlertMessage);
                        $("#trdoc" + rowIndex).remove();

                    },
                    error: (error) => {
                        toastr.error(error, "", { timeOut: 5000 });
                    }
                });
            }
        }

        function CreateNewContact() {
            $("#ContactDialog").dialog('close');

            var companyId = +$("#ContentPlaceHolder1_hfCompanylId").val();
            var iframeid = 'frmPrint';
            var url = "./Contact.aspx?cid=" + companyId;
            parent.document.getElementById(iframeid).src = url;

            $("#CreateContactDialog").dialog({
                autoOpen: true,
                modal: true,
                width: 1300,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Create New",
                show: 'slide'
            });
        }

        function SaveAddedContacts() {
            var id = +$("#ContentPlaceHolder1_hfDealId").val();
            var contactIds = $("#ContentPlaceHolder1_ddlContacts").val();

            if (contactIds.length == 0) {
                toastr.warning("Select Contact");
                $("#ContentPlaceHolder1_ddlContacts").focus();
                return false;
            }

            var contacts = new Array();

            contactIds.forEach((r) => {
                contacts.push({
                    DealId: id,
                    ContactId: r
                });
            });

            PageMethods.AddDealContact(contacts, OnSuccessSaveContact, OnFailSaveContact)
            return false;
        }

        function OnSuccessSaveContact(result) {
            if (result.IsSuccess) {
                $("#ContactDialog").dialog('close');
                var id = +$("#ContentPlaceHolder1_hfDealId").val();
                FillForm(id);
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnFailSaveContact(error) {
            toastr.error(error.get_message());
        }
        function CloseDialog() {
            $("#CreateContactDialog").dialog('close');
            return false;
        }
        function CloseLog() {
            $("#LogEntryPage").dialog('close');
            return false;
        }
    </script>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrintDoc" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
	
    <div id="CreateContactDialog" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="ContactDialog" style="display: none; overflow-x: hidden">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label required-field">Contacts</label>
                </div>
                <div class="col-md-10">
                    <asp:DropDownList ID="ddlContacts" runat="server" CssClass="form-control" multiple="multiple" name="states[]" Style="width: 100%;">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12" style="text-align: right;">
                    <input type="button" class="TransactionalButton btn btn-primary btn-sm" value="Add Contacts" onclick="SaveAddedContacts()" />
                    <input type="button" id="btnNewContacts" class="TransactionalButton btn btn-primary btn-sm" value="Create New Contact" onclick="CreateNewContact()" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfDealId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCompanylId" runat="server" Value="0" />
    <div id="dealdocuments" style="display: none;">
        <label for="Attachment" class="control-label col-md-2">
            Attachment</label>
        <div class="col-md-4">
            <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
                <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                    FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
            </asp:Panel>
        </div>
    </div>
    <input id="hfLogId" type="hidden" value="0" />
    <div style="display: none;">
        <input id="btnLoadLog" type="button" value="Call Me" onclick="LoadLog()" />
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-md-10" style="font-size: 14px; font-weight: bold;">
                            <asp:Label CssClass="control-label" ID="lblDealName" runat="server"></asp:Label>
                        </div>
                        <div class="text-right" style="padding: 0px 20px 0px 20px;">
                            <a href="javascript:void()" id="goEditAnchor" onclick="GoEdit()" style="color: white">Edit</a>&nbsp;|&nbsp;
                            <%--                            <input type="button" class="TransactionalButton btn btn-primary btn-sm" id="goBackAnchor"  style="color: white" value="Go Back"/>--%>
                            <a href="javascript:void();" id="goBackAnchor" onclick="javascript:return GoBack();" style="color: white">Go Back</a>
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label">Account Manager</label>
                            </div>
                            <div class="col-md-4">
                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                <asp:Label CssClass="control-label" ID="lblDealOwner" runat="server"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <label class="control-label">Deal Amount</label>
                            </div>
                            <div class="col-md-4">
                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                <asp:Label CssClass="control-label" ID="lblDealAmount" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label">Deal Stage</label>
                            </div>
                            <div class="col-md-4">
                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                <asp:Label CssClass="control-label" ID="lblDealStage" runat="server"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <label class="control-label">Probability (%)</label>
                            </div>
                            <div class="col-md-4">
                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                <asp:Label CssClass="control-label" ID="lblProbability" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group" id="ImplementationFeedbackNStatus">
                            <div class="col-md-2">
                                <label class="control-label">Implementation Status</label>
                            </div>
                            <div class="col-md-10">
                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                <asp:Label CssClass="control-label" ID="lblImplementationFeedbackNStatus" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-5">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Contacts Information
                </div>
                <div class="panel-body">
                    <table id="tblDealContacts" class="table table-bordered table-responsive" style="width: 100%;">
                    </table>
                    <div class="row" style="text-align: right">
                        <div class="col-md-12">
                            <input type="button" id="btnAddContact" class="TransactionalButton btn btn-primary btn-sm" value="Add Contact" onclick="AddContact()" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="CompanyDiv" class="panel panel-default">
                <div class="panel-heading">
                    Company Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-4">
                                <label class="control-label">Company Name</label>
                            </div>
                            <div class="col-md-8">
                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                <%--<asp:Label CssClass="control-label" ID="lblCompanyName" runat="server"></asp:Label>--%>
                                <a id="lblCompanyHP" href="javascript:void()" id="lblCompanyHPLogentryanchor" onclick="GoToCompanyLink()"><asp:Label ID="lblCompanyName" runat="server"></asp:Label></a>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4">
                                <label class="control-label">Industry</label>
                            </div>
                            <div class="col-md-8">
                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                <asp:Label CssClass="control-label" ID="lblIndustry" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4">
                                <label class="control-label">Phone</label>
                            </div>
                            <div class="col-md-8">
                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                <asp:Label CssClass="control-label" ID="lblPhone" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4">
                                <label class="control-label">Email</label>
                            </div>
                            <div class="col-md-8">
                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                <asp:Label CssClass="control-label" ID="lblEmail" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4">
                                <label class="control-label">WebSite</label>
                            </div>
                            <div class="col-md-8">
                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                <asp:Label CssClass="control-label" ID="lblWebsite" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4">
                                <label class="control-label">Address</label>
                            </div>
                            <div class="col-md-8">
                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                <asp:Label CssClass="control-label" ID="lblAddress" runat="server" Style="text-align: left;"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4">
                                <label class="control-label">Life Cycle Stage</label>
                            </div>
                            <div class="col-md-8">
                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                <asp:Label CssClass="control-label" ID="lblLifeCycleStage" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">
                    Attachments
                </div>
                <div class="panel-body">
                    <div id="DealDocumentInfo" style="overflow-x: scroll;">
                    </div>
                    <div class="row" style="text-align: right">
                        <div class="col-md-12">
                            <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Add Attachment" onclick="LoadDocUploader()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-7">            
            <div class="panel panel-default">
                <div class="panel-heading">
                        <div class="row">
                            <div class="col-md-10">
                                Log Activity
                            </div>
                            <div class="text-right" style="padding: 0px 20px 0px 20px;">
                                <a id="btnLogEntry" href="javascript:void()" id="logentryanchor" onclick="LogEntry()" style="color: white">Log Entry</a>
                            </div>
                        </div>
                    </div>
                <div class="panel-body">
                    <iframe id="logDetails" name="logDetails" width="100%" height="700" style="overflow: hidden; border: none;"></iframe>

                </div>
            </div>
        </div>
    </div>
    <div id="LogEntryPage" style="display: none;">
        <iframe id="logDoc" name="logDoc" width="100%" height="650" frameborder="0" style="overflow: hidden;"></iframe>
    </div>
</asp:Content>
