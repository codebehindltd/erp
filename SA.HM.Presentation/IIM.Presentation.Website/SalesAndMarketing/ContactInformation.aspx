<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="ContactInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.ContactInformation" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        #tblPastCompanyInfo thead {
            display: none;
        }
    </style>
    <script type="text/javascript">
        var companyId = 0, contactId = 0;
        var DealInfo, PastCompanyInfo;
        $(document).ready(function () {
            if ($.trim(CommonHelper.GetParameterByName("conid")) != "")
                contactId = parseInt($.trim(CommonHelper.GetParameterByName("conid")), 10);
            else
                contactId = 0;

            DealInfo = $("#tblDeals").DataTable({
                data: [],
                columns: [
                    { title: "Deal Name", data: "Name", width: "25%" },
                    { title: "Amount", data: "Amount", width: "25%" },
                    { title: "Deal Stage", data: "Stage", width: "25%" },
                    { title: "Probability", data: "Complete", width: "30%" },
                    { title: "Action", data: null, width: "20%" },
                    { title: "", visible: false, data: "DealId" }
                ],
                rowCallback: (row, data, displayNum, displayIndex, dataIndex) => {

                    $('td:eq(' + (row.children.length - 1) + ')', row).html("&nbsp;&nbsp;<a href='javascript:void();' onclick= 'GoToDetails(" + data.DealId + ")' ><img style='cursor:pointer;' alt='Details' src='../Images/detailsInfo.png' /></a>");
                },
                autoWidth: false,
                info: false,
                ordering: false,
                processing: false,
                paging: false,
                filter: false,
                language: {
                    emptyTable: "No Data Found"
                }
            });
            PastCompanyInfo = $("#tblPastCompanyInfo").DataTable({
                data: [],
                columns: [
                    { data: null, width: "100%" },
                    { title: "", visible: false, data: "CompanyId" }
                ],
                columnDefs: [
                    {
                        "targets": 0,
                        "render": function (data, type, full, meta) {
                            let rowString;

                            rowString = full.JobTitle + ", ";
                            rowString += full.Department + " ";
                            rowString += " at <a href='javascript:void();' onclick= 'ShowCompanyDetail(" + full.CompanyId + ")' > " + full.CompanyName + "</a>";

                            return rowString;
                        }
                    }],
                autoWidth: false,
                info: false,
                ordering: false,
                processing: false,
                paging: false,
                filter: false,
                language: {
                    emptyTable: "No Data Found"
                }
            });

            if (contactId > 0)
                FillForm(contactId);
            LoadLog();
        });

        function GoToDetails(dealId) {
            var companyId = +$("#ContentPlaceHolder1_hfCompanyId").val();
            window.location.href = "./DealInformation.aspx?did=" + dealId + "&cid=" + companyId;
        }

        function FillForm(contactId) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../../../SalesAndMarketing/ContactInformation.aspx/FillForm",
                dataType: "json",
                data: JSON.stringify({ id: contactId }),
                async: false,
                success: (data) => {
                    LoadContactInformation(data.d);
                    LoadContactAttachments(data.d.Documents);
                },
                error: (error) => {
                    toastr.error(error, "", { timeOut: 5000 });
                }
            });
        }

        function LoadContactInformation(contact) {
            $("#ContentPlaceHolder1_hfContactId").val(contact.Id);
            $("#lblContactOwner").text(contact.ContactOwner);
            $("#<%=lblContactName.ClientID %>").text(contact.Name);
            $("#lblContactLifeCycleStage").text(contact.ContactLifeCycleStage);

            var details = "";
            details += contact.JobTitle != "" ? contact.JobTitle : "";
            details += contact.Department != "" ? (", " + contact.Department) : "";
            details += contact.CompanyName != "" ? (", " + " at " + contact.CompanyName) : "";

            $("#lblContactDetails").text(details);

            $("#ContentPlaceHolder1_lblContactEmail").text(contact.Email);
            $("#ContentPlaceHolder1_lblContactMobilePersonal").text(contact.MobilePersonal);
            $("#ContentPlaceHolder1_lblSocialMedia").text(contact.SocialMedia);
            $("#ContentPlaceHolder1_lblContactWebsite").text(contact.Website);
            $("#ContentPlaceHolder1_lblContactAdress").text(contact.PersonalAddress);

            if (contact.DOB != null)
                $("#ContentPlaceHolder1_lblContactDOB").text(moment(contact.DOB).format("DD MMM YYYY"));
            if (contact.DateAnniversary != null)
                $("#ContentPlaceHolder1_lblContactDateAnniversary").text(moment(contact.DateAnniversary).format("DD MMM YYYY"));
            //if (contact.LastActivityDateTime != null)
            //    $("#ContentPlaceHolder1_lblLastActivityDateTime").text(moment(contact.LastActivityDateTime).format("DD MMM YYYY") + " at " + moment(contact.LastActivityDateTime).format("hh:mm A"));

            if (contact.CompanyId > 0) {
                $("#CurrentCompany").show();
                $("#ContentPlaceHolder1_lblCompany").text(contact.CompanyName);
                $("#ContentPlaceHolder1_hfCompanyId").val(contact.CompanyId);
                $("#ContentPlaceHolder1_lblIndustry").text(contact.Industry);
                $("#ContentPlaceHolder1_lblPhone").text(contact.CompanyPhone);
                $("#ContentPlaceHolder1_lblEmail").text(contact.CompanyEmail);
                $("#ContentPlaceHolder1_lblWebsite").text(contact.CompanyWebsite);
                $("#ContentPlaceHolder1_lblLifeCycleStage").text(contact.CompanyLifeCycleStage);
                $("#ContentPlaceHolder1_lblAddress").text(contact.CompanyAddress);
            }
            else
                $("#CurrentCompany").hide();

            DealInfo.clear();
            DealInfo.rows.add(contact.Deals);
            DealInfo.draw();

            if (contact.Deals.length > 0)
                $("#btnAddDeal").val("Add Another Deal");
            else
                $("#btnAddDeal").val("Add Deal");

            PastCompanyInfo.clear();
            PastCompanyInfo.rows.add(contact.PastCompanys);
            PastCompanyInfo.draw();
            return false;
        }

        function LoadContactAttachments(result) {
            var guestDoc = result;

            if (result.length > 0)
                $("#btnAttachment").val("Add Another Attachment");
            else
                $("#btnAttachment").val("Add Attachment");

            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";

            guestDocumentTable += "<table id='contactDocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            guestDocumentTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";
            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }

                //guestDocumentTable += "<td align='left' style='width: 50%'>" + guestDoc[row].Name + "</td>";
                guestDocumentTable += "<td align='left' style='width: 30%'>";
                guestDocumentTable += "<a style='color:#333333;' target='_blank' href='" + guestDoc[row].Path + guestDoc[row].Name + "'>"
                guestDocumentTable += guestDoc[row].Name + "</a></td>";

                if (guestDoc[row].Path != "") {
                    if (guestDoc[row].Extention == ".jpg" || guestDoc[row].Extention == ".png")
                        imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    else
                        imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                guestDocumentTable += "<td align='left' style='width: 30%'>" + imagePath + "</td>";

                guestDocumentTable += "<td align='left' style='width: 20%'>";
                guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer;\" onClick=\"javascript:return DeleteDealDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Document' border='0' />";
                guestDocumentTable += "</td>";
                guestDocumentTable += "</tr>";
            }
            //for (row = 0; row < totalDoc; row++) {
            //    if (row % 2 == 0) {
            //        guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
            //    }
            //    else {
            //        guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
            //    }

            //    //guestDocumentTable += "<td align='left' style='width: 50%'>" + guestDoc[row].Name + "</td>";
            //    guestDocumentTable += "<td align='left' style='width: 50%'>";
            //    guestDocumentTable += "<a style='color:#333333;' target='_blank' href='" + guestDoc[row].Path + guestDoc[row].Name + "'>"
            //    guestDocumentTable += guestDoc[row].Name + "</a></td>";

            //    if (guestDoc[row].Path != "") {
            //        if (guestDoc[row].Extention == ".jpg")
            //            imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
            //        else
            //            imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
            //    }
            //    else
            //        imagePath = "";

            //    guestDocumentTable += "<td align='left' style='width: 30%'>" + imagePath + "</td>";

            //    guestDocumentTable += "<td align='left' style='width: 20%'>";
            //    guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer;\" onClick=\"javascript:return DeleteDealDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Document' border='0' />";
            //    guestDocumentTable += "</td>";
            //    guestDocumentTable += "</tr>";
            //}
            guestDocumentTable += "</table>";

            // docc = guestDocumentTable;

            $("#DealDocumentInfo").html(guestDocumentTable);
        }
        function LoadLog() {
            if ($.trim(CommonHelper.GetParameterByName("conid")) != "")
                contactId = parseInt($.trim(CommonHelper.GetParameterByName("conid")), 10);
            if ($.trim(CommonHelper.GetParameterByName("cid")) != "")
                companyId = parseInt($.trim(CommonHelper.GetParameterByName("cid")), 10);
            if (contactId != 0) {
                var iframeid = 'logDetails';
                var url = "./LogActivity.aspx?conid=" + contactId;
                parent.document.getElementById(iframeid).src = url;
            }
        }

        function LoadContactAllDeal() {
            var contactId = +$("#ContentPlaceHolder1_hfContactId").val();
            if (contactId > 0)
                FillForm(contactId);
        }
        function GoToCompanyLink()
        {
            var companyId = +$("#ContentPlaceHolder1_hfCompanyId").val();
            if (companyId > 0)
            {
                var url = "./CompanyInformation.aspx?id=" + companyId;
                window.location = url;
                return true;
            }
        }

        function LogEntry() {
            var logId = $("#hfLogId").val();

            if ($.trim(CommonHelper.GetParameterByName("id")) != "")
                companyId = parseInt($.trim(CommonHelper.GetParameterByName("id")), 10);
            if ($.trim(CommonHelper.GetParameterByName("cid")) != "")
                companyId = parseInt($.trim(CommonHelper.GetParameterByName("cid")), 10);
            var iframeid = 'logDoc';
            var url = "./SalesCallEntry.aspx?id=" + logId + "&cid=" + companyId + "&ctid=" + contactId;
            parent.document.getElementById(iframeid).src = url;

            $("#LogEntryPage").dialog({
                autoOpen: true,
                modal: true,
                width: 1200,
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

            $("#hfLogId").val("0");
        }

        function DialogCloseAfterUpdate() {
            $("#LogEntryPage").dialog("close");
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
            var name = $("#<%=lblContactName.ClientID %>").text();
            var contactId = +$("#ContentPlaceHolder1_hfContactId").val();

            var iframeid = 'frmPrint';
            var url = "./Contact.aspx?editId=" + contactId;
            parent.document.getElementById(iframeid).src = url;

            $("#CreateDealDialog").dialog({
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
        function LoadDocUploader() {

            var randomId = +$("#ContentPlaceHolder1_hfContactId").val();
            var path = "/SalesAndMarketing/Images/Contact/";
            var category = "ContactDocument";
            var iframeid = 'frmPrintDoc';
            //var url = "/HMCommon/FileUploadTest.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            var url = "/Common/FileUpload.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
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
            $("#contactdocuments").dialog({
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
            var id = +$("#ContentPlaceHolder1_hfContactId").val();

            if (id > 0) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "../../../SalesAndMarketing/ContactInformation.aspx/LoadContactDocument",
                    dataType: "json",
                    data: JSON.stringify({ id: id }),
                    async: false,
                    success: (data) => {
                        LoadContactAttachments(data.d);
                    },
                    error: (error) => {
                        toastr.error(error.d.get_message());
                    }
                });
            }
        }

        function DeleteDealDoc(docId, rowIndex) {
            if (confirm("Want to delete?")) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "../../../SalesAndMarketing/ContactInformation.aspx/DeleteContactDocument",
                    dataType: "json",
                    data: JSON.stringify({ documentId: docId }),
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

        function CreateNewDeal() {
            var companyId = +$("#ContentPlaceHolder1_hfCompanyId").val();
            var contactId = +$("#ContentPlaceHolder1_hfContactId").val();

            var iframeid = 'frmPrint';
            var url = "./Deal.aspx?cid=" + companyId + "&conid=" + contactId;
            parent.document.getElementById(iframeid).src = url;

            $("#CreateDealDialog").dialog({
                autoOpen: true,
                modal: true,
                width: 1300,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Create New Deal",
                show: 'slide'
            });
        }

        function ShowCompanyDetail(companyId) {
            PageMethods.GetPreviousCompanyInfoById(companyId, OnGetCompanySuccess, OnGetCompanyFail);
            return false;
        }

        function OnGetCompanySuccess(result) {
            $("#lblPreviousCompanyName").text(result.CompanyName);
            $("#lblPreviousCompanyIndustry").text(result.IndustryName);
            $("#lblPreviousCompanyPhone").text(result.ContactNumber);
            $("#lblPreviousCompanyEmail").text(result.EmailAddress);
            $("#lblPreviousCompanyWebsite").text(result.WebAddress);

            var address = "";
            if (result.ShippingStreet != "")
                address = result.ShippingStreet + " , " + result.ShippingState + " , " + result.ShippingCity + " , Postal Code: " + result.ShippingPostCode + " , " + result.ShippingCountry;

            $("#lblPreviousCompanyAddress").text(address);
            $("#lblPreviousCompanyLifeCycleStage").text(result.LifeCycleStage);

            $("#PreviousCompanyDialog").dialog({
                autoOpen: true,
                modal: true,
                width: 600,
                height: 300,
                minWidth: 550,
                minHeight: 300,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Company Information",
                show: 'slide'
            });
        }
        function OnGetCompanyFail(error) {
            toastr.error(error.get_message());
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }
        function CloseDialog() {
            $("#CreateDealDialog").dialog('close');
            return false;
        }
        function CloseLog() {
            $("#LogEntryPage").dialog('close');
            return false;
        }
    </script>

    <input id="hfLogId" type="hidden" value="0" />
    <asp:HiddenField ID="hfContactId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0" />
    <div style="display: none;">
        <input id="btnLoadLog" type="button" value="Call Me" onclick="LoadLog()" />
    </div>
    <div id="CreateDealDialog" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="PreviousCompanyDialog" style="display: none;">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-4">
                    <label class="control-label">Company Name</label>
                </div>
                <div class="col-md-8">
                    <span style="font-weight: bold; border: none">:&nbsp;</span>
                    <label id="lblPreviousCompanyName" class="control-label"></label>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-4">
                    <label class="control-label">Industry</label>
                </div>
                <div class="col-md-8">
                    <span style="font-weight: bold; border: none">:&nbsp;</span>
                    <label id="lblPreviousCompanyIndustry" class="control-label"></label>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-4">
                    <label class="control-label">Phone</label>
                </div>
                <div class="col-md-1">
                    <label class="control-label">:</label>
                </div>
                <div class="col-md-7">
                    <label id="lblPreviousCompanyPhone" class="control-label"></label>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-4">
                    <label class="control-label">Email</label>
                </div>
                <div class="col-md-1">
                    <label class="control-label">:</label>
                </div>
                <div class="col-md-7">
                    <label id="lblPreviousCompanyEmail" class="control-label"></label>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-4">
                    <label class="control-label">WebSite</label>
                </div>
                <div class="col-md-1">
                    <label class="control-label">:</label>
                </div>
                <div class="col-md-7">
                    <label id="lblPreviousCompanyWebsite" class="control-label"></label>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-4">
                    <label class="control-label">Address</label>
                </div>
                <div class="col-md-1">
                    <label class="control-label">:</label>
                </div>
                <div class="col-md-7">
                    <label id="lblPreviousCompanyAddress" class="control-label"></label>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-4">
                    <label class="control-label">Life Cycle Stage</label>
                </div>
                <div class="col-md-1">
                    <label class="control-label">:</label>
                </div>
                <div class="col-md-7">
                    <label id="lblPreviousCompanyLifeCycleStage" class="control-label"></label>
                </div>
            </div>
        </div>
    </div>
    <%--<div id="contactdocuments" style="display: none;">
        <label for="Attachment" class="control-label col-md-2">
            Attachment</label>
        <div class="col-md-4">
            <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
                <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                    FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
            </asp:Panel>
        </div>
    </div>--%>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrintDoc" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div class="form-horizontal">
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-md-10" style="font-size: 14px; font-weight: bold;">
                                <asp:Label ID="lblContactName" runat="server" Style="font-weight: bold;"></asp:Label>
                            </div>
                            <div class="text-right" style="padding: 0px 20px 0px 20px;">
                                <a href="javascript:void()" id="goEditAnchor" onclick="GoEdit()" style="color: white">Edit</a>&nbsp;|&nbsp;
                                <a href="javascript:void()" id="goBackAnchor" onclick="GoBack()" style="color: white">Go Back</a>
                            </div>
                        </div>
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <label class="control-label">Account Manager</label>
                                </div>
                                <div class="col-md-10">
                                    <span style="font-weight: bold; border: none">:&nbsp;</span>
                                    <label id="lblContactOwner" class="control-label"></label>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <label class="control-label">Life Cycle Stage</label>
                                </div>
                                <div class="col-md-10">
                                    <span style="font-weight: bold; border: none">:&nbsp;</span>
                                    <label id="lblContactLifeCycleStage" class="control-label"></label>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-12">
                                    <label id="lblContactDetails" class="control-label"></label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-5">
                <div id="CurrentCompany" class="panel panel-default">
                    <div class="panel-heading">
                        Current Company Information
                    </div>
                    <div class="panel-body">
                        <table id="tblComapanyinfo" class="table table-hover" style="border: none;" border="0">
                            <tbody>
                                <tr>
                                    <td class="col-md-3" style="border: none">
                                        <label>Company Name</label>
                                    </td>
                                    <td class="col-md-9" style="border: none">
                                        <span style="font-weight: bold; border: none">:&nbsp;</span>
                                        <a id="lblCompanyHP" href="javascript:void()" id="lblCompanyHPLogentryanchor" onclick="GoToCompanyLink()"><asp:Label ID="lblCompany" runat="server"></asp:Label></a>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="col-md-3" style="border: none">
                                        <label>Industry</label>
                                    </td>
                                    <td class="col-md-9" style="border: none">
                                        <span style="font-weight: bold; border: none">:&nbsp;</span>
                                        <asp:Label ID="lblIndustry" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="col-md-3" style="border: none">
                                        <label>Phone</label>
                                    </td>
                                    <td class="col-md-9" style="border: none">
                                        <span style="font-weight: bold; border: none">:&nbsp;</span>
                                        <asp:Label ID="lblPhone" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="col-md-3" style="border: none">
                                        <label>Email</label>
                                    </td>
                                    <td class="col-md-9" style="border: none">
                                        <span style="font-weight: bold; border: none">:&nbsp;</span>
                                        <asp:Label ID="lblEmail" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="col-md-3" style="border: none">
                                        <label>WebSite</label>
                                    </td>
                                    <td class="col-md-9" style="border: none">
                                        <span style="font-weight: bold; border: none">:&nbsp;</span>
                                        <asp:Label ID="lblWebsite" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="col-md-3" style="border: none">
                                        <label>Address</label>
                                    </td>
                                    <td class="col-md-9" style="border: none">
                                        <span style="font-weight: bold; border: none">:&nbsp;</span>
                                        <asp:Label ID="lblAddress" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="col-md-3" style="border: none">
                                        <label>Life Cycle Stage</label>
                                    </td>
                                    <td class="col-md-9" style="border: none">
                                        <span style="font-weight: bold; border: none">:&nbsp;</span>
                                        <asp:Label ID="lblLifeCycleStage" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Personal Information
                    </div>
                    <div class="panel-body">
                        <table id="tblPersonalinfo" class="table table-hover" style="border: none;" border="0">
                            <tbody>
                                <tr>
                                    <td class="col-md-3" style="border: none">
                                        <label>Email</label>
                                    </td>
                                    <td class="col-md-9" style="border: none">
                                        <span style="font-weight: bold; border: none">:&nbsp;</span>
                                        <asp:Label ID="lblContactEmail" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="col-md-3" style="border: none">
                                        <label>Mobile</label>
                                    </td>
                                    <td class="col-md-9" style="border: none">
                                        <span style="font-weight: bold; border: none">:&nbsp;</span>
                                        <asp:Label ID="lblContactMobilePersonal" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="col-md-3" style="border: none">
                                        <label>Social Media</label>
                                    </td>
                                    <td class="col-md-9" style="border: none">
                                        <span style="font-weight: bold; border: none">:&nbsp;</span>
                                        <asp:Label ID="lblSocialMedia" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="col-md-3" style="border: none">
                                        <label>Website</label>
                                    </td>
                                    <td class="col-md-9" style="border: none">
                                        <span style="font-weight: bold; border: none">:&nbsp;</span>
                                        <asp:Label ID="lblContactWebsite" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="col-md-3" style="border: none">
                                        <label>Address</label>
                                    </td>
                                    <td class="col-md-9" style="border: none">
                                        <span style="font-weight: bold; border: none">:&nbsp;</span>
                                        <asp:Label ID="lblContactAdress" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="col-md-3" style="border: none">
                                        <label>Date of Birth</label>
                                    </td>
                                    <td class="col-md-9" style="border: none">
                                        <span style="font-weight: bold; border: none">:&nbsp;</span>
                                        <asp:Label ID="lblContactDOB" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="col-md-3" style="border: none">
                                        <label>Anniversary Date</label>
                                    </td>
                                    <td class="col-md-9" style="border: none">
                                        <span style="font-weight: bold; border: none">:&nbsp;</span>
                                        <asp:Label ID="lblContactDateAnniversary" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Past Company Information
                    </div>
                    <div class="panel-body">
                        <table id="tblPastCompanyInfo" class="table table-responsive"></table>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Deal Information
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <table style="table-layout: fixed; width: 100%;" id="tblDeals" class="table table-bordered table-responsive">
                                </table>
                            </div>
                        </div>
                        <div class="row" style="text-align: right">
                            <div class="col-md-12">
                                <input type="button" id="btnAddDeal" class="TransactionalButton btn btn-primary btn-sm" value="Add a Deal" onclick="CreateNewDeal()" />
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
                        <div class="form-group">
                            <div class="col-md-12">
                                <iframe id="logDetails" name="logDetails" width="100%" height="700" style="overflow: hidden; border: none;"></iframe>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="LogEntryPage" style="display: none;">
            <iframe id="logDoc" name="logDoc" width="100%" height="650" frameborder="0" style="overflow: hidden;"></iframe>
        </div>
</asp:Content>
