<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="CompanyInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.CompanyInformation" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var companyId = 0;
        $(document).ready(function () {
            if ($.trim(CommonHelper.GetParameterByName("id")) != "")
                companyId = parseInt($.trim(CommonHelper.GetParameterByName("id")), 10);
            if (companyId != 0) {
                FillForm(companyId);
                LoadLog();
                //LogEntry();
            }
            $('#ContentPlaceHolder1_ddlContacts').select2();
            $("#tblContactInfo").delegate("td > img.ContactDelete", "click", function () {
                var answer = confirm("Do you want to delete this it?")
                if (answer) {
                    CommonHelper.SpinnerOpen();
                    var contactId = $.trim($(this).parent().parent().find("td:eq(0)").text());
                    var params = JSON.stringify({ deleteId: contactId, companyId: companyId });
                    var $row = $(this).parent().parent();
                    $.ajax({
                        type: "POST",
                        url: "/SalesAndMarketing/CompanyInformation.aspx/DeleteContact",
                        data: params,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            CommonHelper.AlertMessage(data.d.AlertMessage);
                            $row.remove();
                            CommonHelper.SpinnerClose();
                        },
                        error: function (error) {
                        }
                    });
                }
            });
            //$("#goBackAnchor").click(function () {
            //    window.history.back();
            //});
        });
        function FillForm(companyId) {
            LoadCompanyInfo();
            GetDealInfoByCompanyId();
            GetContactsByCompanyId();
            UploadComplete();
        }
        function LoadCompanyInfo() {
            PageMethods.GetCompanyInfoById(companyId, OnGetCompanyInfoByIdSucceed, OnGetCompanyInfoByIdFaild);
            return false;
        }
        function OnGetCompanyInfoByIdSucceed(result) {
            $("#<%=lblAccountManager.ClientID %>").text(result.CompanyOwnerName);
            $("#<%=lblCompanyName.ClientID %>").text(result.CompanyName);
            $("#<%=lblCompanyType.ClientID %>").text(result.TypeName);
            $("#<%=lblLifeCycleStage.ClientID %>").text(result.LifeCycleStage);
            $("#<%=lblIndustry.ClientID %>").text(result.IndustryName);
            $("#<%=lblOwnership.ClientID %>").text(result.OwnershipName);
            $("#<%=lblAnnualRevenue.ClientID %>").text(result.AnnualRevenue);
            $("#<%=lblNumberOfEmployee.ClientID %>").text(result.NumberOfEmployee);
            $("#<%=lblPhone.ClientID %>").text(result.ContactNumber);
            $("#<%=lblCompanyEmail.ClientID %>").text(result.EmailAddress);
            $("#<%=lblFax.ClientID %>").text(result.Fax);
            $("#<%=lblWebAddress.ClientID %>").text(result.WebAddress);
            $("#<%=lblBillingCity.ClientID %>").text(result.BillingCity);
            $("#<%=lblBillingCountry.ClientID %>").text(result.BillingCountry);
            $("#<%=lblBillingPostCode.ClientID %>").text(result.BillingPostCode);
            $("#<%=lblBillingState.ClientID %>").text(result.BillingState);
            $("#<%=lblBillingStreet.ClientID %>").text(result.BillingStreet);
            $("#<%=lblShippingCity.ClientID %>").text(result.ShippingCity);
            $("#<%=lblShippingCountry.ClientID %>").text(result.ShippingCountry);
            $("#<%=lblShippingPostCode.ClientID %>").text(result.ShippingPostCode);
            $("#<%=lblShippingState.ClientID %>").text(result.ShippingState);
            $("#<%=lblShippingStreet.ClientID %>").text(result.ShippingStreet);
            $("#<%=lblBillingAddress.ClientID %>").text(result.BillingAddress);
            $("#<%=lblShippingAddress.ClientID %>").text(result.ShippingAddress);
            return false;
        }
        function OnGetCompanyInfoByIdFaild(error) {
            toastr.error(error.get_message());
        }
        // GetContactsByCompanyId GetDealInfoByCompanyId
        function GetContactsByCompanyId() {
            PageMethods.GetContactsByCompanyId(companyId, OnGetContactsByCompanyIdSucceed, OnGetContactsByCompanyIdFailed);
            return false;
        }
        function OnGetContactsByCompanyIdSucceed(result) {
            $("#tblContactInfo tbody tr").remove();
            var tr = "", totalRow = 0, detailLink = "";
            //debugger;
            if (result.length <= 0) {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"3\" >No Data Found</td> </tr>";
                $("#tblContactInfo tbody ").append(emptyTr);
                return false;
            }
            else {
                $("#ContentPlaceHolder1_btnAddContact").val("Add Another Contact");
                for (var i = 0; i < result.length; i++) {
                    totalRow = $("#tblContactInfo tbody tr").length;
                    if ((totalRow % 2) == 0) {
                        tr += "<tr style=\"background-color:#E3EAEB;\">";
                    }
                    else {
                        tr += "<tr style=\"background-color:#FFFFFF;\">";
                    }
                    tr += "<td align='left' style=\"display:none;\">" + result[i].Id + "</td>";
                    //tr += "<td align='left' style=\"width:50%;\">" + result[i].Name + "</td>";
                    tr += "<td align='left' style=\"width:50%;\"><a href='javascript:void();' onClick= \"javascript:return ShowContactDetails('" + result[i].Id + "' )\">" + result[i].Name + "</a></td>";
                    tr += "<td align='left' style=\"width:25%;\">" + result[i].JobTitle + "</td>";
                    //tr += "<td align='center' style=\"width:25%; cursor:pointer;\"><img src='../Images/detailsInfo.png' onClick= \"javascript:return ShowContactDetails('" + result[i].Id + "' )\" alt='Details' title='Details' border='0' />";
                    tr += "<td align='center' style=\"width:25%; cursor:pointer;\">";
                    tr += " <img src='../Images/delete.png' class= 'ContactDelete'  alt='Delete Information' border='0' /> </td>";
                    tr += "</tr>";
                    $("#tblContactInfo tbody").append(tr);
                    tr = "";
                }
            }
            return false;
        }
        function OnGetContactsByCompanyIdFailed(error) {
            toastr.error(error.get_message());
        }
        function GetDealInfoByCompanyId() {
            PageMethods.GetDealInfoByCompanyId(companyId, OnGetDealInfoByCompanyIdSucceed, OnGetDealInfoByCompanyIdFailed);
            return false;
        }
        function OnGetDealInfoByCompanyIdSucceed(result) {
            $("#tblDealInfo tbody tr").remove();
            var tr = "", totalRow = 0, detailLink = "";
            if (result.length <= 0) {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#tblDealInfo tbody ").append(emptyTr);
                return false;
            }
            else {
                $("#ContentPlaceHolder1_btnAddDeal").val("Add Another Deal");
                for (var i = 0; i < result.length; i++) {
                    totalRow = $("#tblDealInfo tbody tr").length;
                    if ((totalRow % 2) == 0) {
                        tr += "<tr style=\"background-color:#E3EAEB;\">";
                    }
                    else {
                        tr += "<tr style=\"background-color:#FFFFFF;\">";
                    }
                    //tr += "<td align='left' style=\"width:25%;\">" + result[i].Name + "</td>";
                    tr += "<td align='left' style=\"width:25%;\"><a href='javascript:void();' onClick= \"javascript:return ShowDealDetails('" + result[i].Id + "' )\">" + result[i].Name + "</a></td>";
                    tr += "<td align='left' style=\"width:20%;\">" + result[i].Amount + "</td>";
                    tr += "<td align='left' style=\"width:25%;\">" + result[i].Stage + "</td>";
                    tr += "<td align='left' style=\"width:20%;\">" + result[i].ProbabilityStage + "</td>";
                    //tr += "<td align='center' style=\"width:10%; cursor:pointer;\"><img src='../Images/detailsInfo.png' onClick= \"javascript:return ShowDealDetails('" + result[i].Id + "' )\" alt='Details' title='Details' border='0' /> </td>";
                    tr += "</tr>";
                    $("#tblDealInfo tbody").append(tr);
                    tr = "";
                }
            }
            return false;
        }
        function OnGetDealInfoByCompanyIdFailed(error) {
            toastr.error(error.get_message());
        }
        function ShowContactDetails(contactId) {
            var url = "./ContactInformation.aspx?conid=" + contactId + "&cid=" + companyId;
            window.location = url;
        }
        function ShowDealDetails(dealId) {
            window.location.href = "./DealInformation.aspx?did=" + dealId + "&cid=" + companyId;
        }
        function PerformClearAction() {
            $("#<%=lblAccountManager.ClientID %>").text("");
            $("#<%=lblCompanyName.ClientID %>").text("");
            $("#<%=lblCompanyType.ClientID %>").text("");
            $("#<%=lblLifeCycleStage.ClientID %>").text("");
            $("#<%=lblIndustry.ClientID %>").text("");
            $("#<%=lblOwnership.ClientID %>").text("");
            $("#<%=lblAnnualRevenue.ClientID %>").text("");
            $("#<%=lblNumberOfEmployee.ClientID %>").text("");
            $("#<%=lblPhone.ClientID %>").text("");
            $("#<%=lblCompanyEmail.ClientID %>").text("");
            $("#<%=lblFax.ClientID %>").text("");
            $("#<%=lblWebAddress.ClientID %>").text("");
            $("#<%=lblBillingCity.ClientID %>").text("");
            $("#<%=lblBillingCountry.ClientID %>").text("");
            $("#<%=lblBillingPostCode.ClientID %>").text("");
            $("#<%=lblBillingState.ClientID %>").text("");
            $("#<%=lblBillingStreet.ClientID %>").text("");
            $("#<%=lblShippingCity.ClientID %>").text("");
            $("#<%=lblShippingCountry.ClientID %>").text("");
            $("#<%=lblShippingPostCode.ClientID %>").text("");
            $("#<%=lblShippingState.ClientID %>").text("");
            $("#<%=lblShippingStreet.ClientID %>").text("");
            return false;
        }
        function LoadLog() {
            if (companyId != 0) {
                var iframeid = 'logDetails';
                var url = "./LogActivity.aspx?cid=" + companyId;
                parent.document.getElementById(iframeid).src = url;
            }
        }
        function LogEntry() {
            var logId = $("#hfLogId").val();
            var iframeid = 'logDoc';
            var url = "./SalesCallEntry.aspx?id=" + logId + "&cid=" + companyId;
            parent.document.getElementById(iframeid).src = url;
            debugger;
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
            $("#hfLogId").val("0");
        }
        function CloseLog() {
            $("#LogEntryPage").dialog('close');
            return false;
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
            var iframeid = 'frmDeal';
            var url = "./Company.aspx?cid=" + companyId;
            parent.document.getElementById(iframeid).src = url;
            var name = $("#<%=lblCompanyName.ClientID %>").text();
            $("#DealDialogue").dialog({
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
            return false;
        }
        function CreateContactForCompany(companyId) {
            var iframeid = 'frmPrint';
            var url = "./Contact.aspx?cid=" + id;
            parent.document.getElementById(iframeid).src = url;
            $("#ContactDialogue").dialog({
                autoOpen: true,
                modal: true,
                width: 1100,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Add New Contact",
                show: 'slide'
            });
            return false;
        }
        function AddContact() {
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
            LoadContactsDropdown();
            return false;
        }
        function LoadContactsDropdown() {
            PageMethods.LoadContactsDropdown(OnLoadContactsDropdownSucceed, OnLoadContactsDropdownFailed);
            return false;
        }
        function OnLoadContactsDropdownSucceed(result) {
            $("#ContentPlaceHolder1_ddlContacts").empty();
            // debugger;
            var i = 0, fieldLength = result.length;
            if (fieldLength > 0) {
                for (i = 0; i < fieldLength; i++) {
                    $('<option value="' + result[i].Id + '">' + result[i].Name + '</option>').appendTo('#ContentPlaceHolder1_ddlContacts');
                }
            }
            else {
                $("<option value='0'>--No Contact Found--</option>").appendTo("#ContentPlaceHolder1_ddlContacts");
            }
            if (fieldLength == 1)
                $("#ContentPlaceHolder1_ddlContacts").val($("#ContentPlaceHolder1_ddlContacts option:first").val()).trigger('change');
        }
        function OnLoadContactsDropdownFailed(error) {
            toastr.error(error.get_message());
        }
        function SaveAddedContacts() {
            var contactIds = $("#ContentPlaceHolder1_ddlContacts").val();
            if (contactIds.length == 0) {
                toastr.warning("Select Contact");
                $("#ContentPlaceHolder1_ddlContacts").focus();
                return false;
            }
            var contacts = new Array();
            contactIds.forEach((r) => {
                contacts.push({
                    Id: r
                });
            });
            PageMethods.SaveAddedContacts(contacts, companyId, OnSuccessSaveContact, OnFailSaveContact)
            return false;
        }
        function OnSuccessSaveContact(result) {
            if (result.IsSuccess) {
                FillForm(companyId);
                $("#ContactDialog").dialog('close');
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnFailSaveContact(error) {
            toastr.error(error.get_message());
        }
        function CreateNewContact() {
            $("#ContactDialog").dialog('close');
            var iframeid = 'frmCotact';
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
                title: "New Contact Information",
                show: 'slide'
            });
        }
        function CreateAnotherDeal() {
            var iframeid = 'frmDeal';
            var url = "./Deal.aspx?cid=" + companyId;
            parent.document.getElementById(iframeid).src = url;
            $("#DealDialogue").dialog({
                autoOpen: true,
                modal: true,
                width: 1300,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "New Deal Information",
                show: 'slide'
            });
            return false;
        }
        function LoadDocUploader() {
            //var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var path = "/SalesAndMarketing/Images/Company/";
            var category = "CompanyDocument";
            var iframeid = 'frmPrintDoc';
            //var url = "/HMCommon/FileUploadTest.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            var url = "/Common/FileUpload.aspx?Path=" + path + "&OwnerId=" + companyId + "&Category=" + category;
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
            $("#companydocuments").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Company Documents",
                show: 'slide'
            });
        }
        function UploadComplete() {
            var id = companyId;
            PageMethods.LoadCompanyDocument(id, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }
        function OnLoadDocumentSucceeded(result) {
            var guestDoc = result;
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";
            if (totalDoc > 0) {
                $("#btnAttachment").val("Add Another Document");
            }
            else {
                $("#btnAttachment").val("Add Document");
            }
            guestDocumentTable += "<table id='companytDocList'style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            guestDocumentTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";
            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }
                //guestDocumentTable += "<td align='left' style='width: 50%'>" + guestDoc[row].Name + "</td>";
                guestDocumentTable += "<td align='left' style='width: 50%'>";
                guestDocumentTable += "<a style='color:#333333;' target='_blank' href='" + guestDoc[row].Path + guestDoc[row].Name + "'>"
                guestDocumentTable += guestDoc[row].Name + "</a></td>";
                if (guestDoc[row].Path != "") {
                    if (guestDoc[row].Extention == ".jpg")
                        imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    else
                        imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";
                guestDocumentTable += "<td align='left' style='width: 30%'>" + imagePath + "</td>";
                guestDocumentTable += "<td align='left' style='width: 20%'>";
                guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer;\" onClick=\"javascript:return DeleteDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Document' border='0' />";
                guestDocumentTable += "</td>";
                guestDocumentTable += "</tr>";
                $("#docTable tbody").append(guestDocumentTable);
            }
            //for (row = 0; row < totalDoc; row++) {
            //    if (row % 2 == 0) {
            //        guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
            //    }
            //    else {
            //        guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
            //    }
            //    guestDocumentTable += "<td align='left' style='width: 50%'>" + guestDoc[row].Name + "</td>";
            //    if (guestDoc[row].Path != "") {
            //        if (guestDoc[row].Extention == ".jpg")
            //            imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
            //        else
            //            imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
            //    }
            //    else
            //        imagePath = "";
            //    guestDocumentTable += "<td align='left' style='width: 30%'>";
            //    guestDocumentTable += "<a style='color:#333333;' target='_blank' href='" + guestDoc[row].Path + guestDoc[row].Name + "'>"
            //    guestDocumentTable += guestDoc[row].Name + "</a></td>";
            //    guestDocumentTable += "<td align='left' style='width: 20%'>";
            //    guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer;\" onClick=\"javascript:return DeleteDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Document' border='0' />";
            //    guestDocumentTable += "</td>";
            //    guestDocumentTable += "</tr>";
            //}
            guestDocumentTable += "</table>";
            $("#companyDocumentInfo").html(guestDocumentTable);
        }
        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }
        function DeleteDoc(docId, rowIndex) {
            if (confirm("Want to delete?")) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "../../../SalesAndMarketing/CompanyInformation.aspx/DeleteCompanyDocument",
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
        function CloseContactDialog() {
            $("#CreateContactDialog").dialog('close');
            return false;
        }
        function CloseDialog() {
            $("#DealDialogue").dialog('close');
            return false;
        }
    </script>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrintDoc" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>

    <%--<div id="companydocuments" style="display: none;">
        <label for="Attachment" class="control-label col-md-2">
            Attachment</label>
        <div class="col-md-4">
            <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
                <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                    FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
            </asp:Panel>
        </div>
    </div>--%>
    <input id="hfLogId" type="hidden" value="0" />
    <div id="ContactDialogue" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="CreateContactDialog" style="display: none;">
        <iframe id="frmCotact" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="DealDialogue" style="display: none;">
        <iframe id="frmDeal" name="IframeName" width="100%" height="100%" runat="server"
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
                    <input type="button" class="TransactionalButton btn btn-primary btn-sm" value="Add Contact" onclick="SaveAddedContacts()" />
                    <input type="button" id="btnNewContacts" class="TransactionalButton btn btn-primary btn-sm" value="Create New Contact" onclick="CreateNewContact()" />
                </div>
            </div>
        </div>
    </div>
    <div style="display: none;">
        <input id="btnLoadLog" type="button" value="Call Me" onclick="LoadLog()" />
    </div>
    <div class="form-horizontal">
        <div class="panel panel-default">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-md-10" style="font-size: 14px; font-weight: bold;">
                        <asp:Label ID="lblCompanyName" runat="server" Style="font-weight: bold;"></asp:Label>
                    </div>
                    <div class="text-right" style="padding: 0px 20px 0px 20px;">
                        <a href="javascript:void()" id="goEditAnchor" onclick="GoEdit()" style="color: white">Edit</a>&nbsp;|&nbsp;
                        <a href="javascript:void();" id="goBackAnchor" onclick="javascript:return GoBack();" style="color: white">Go Back</a>
                    </div>
                </div>
            </div>
            <div class="panel-body">
                &nbsp;
                <div class="form-group">
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class=" panel-heading">
                                Company Detail
                            </div>
                            <div class="panel-body">
                                <table id="tblComapanyinfo" class="table table-hover" style="border: none;" border="0">
                                    <tbody>
                                        <tr>
                                            <td class="col-md-3" style="border: none">
                                                <label>Account Manager</label>
                                            </td>
                                            <td class="col-md-9" style="border: none">
                                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                <asp:Label ID="lblAccountManager" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-md-3" style="border: none">
                                                <label>Company Type</label>
                                            </td>
                                            <td class="col-md-9" style="border: none">
                                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                <asp:Label ID="lblCompanyType" runat="server"></asp:Label>
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
                                                <label>Ownership</label>
                                            </td>
                                            <td class="col-md-9" style="border: none">
                                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                <asp:Label ID="lblOwnership" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="DivAnnualRevenue" runat="server">
                                            <td class="col-md-3" style="border: none">
                                                <label>Annual Revenue</label>
                                            </td>
                                            <td class="col-md-9" style="border: none">
                                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                <asp:Label ID="lblAnnualRevenue" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="DivNoOfEmployee" runat="server">
                                            <td class="col-md-3" style="border: none">
                                                <label>Number Of Employee</label>
                                            </td>
                                            <td class="col-md-9" style="border: none">
                                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                <asp:Label ID="lblNumberOfEmployee" runat="server"></asp:Label>
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
                                                <asp:Label ID="lblCompanyEmail" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-md-3" style="border: none">
                                                <label>Fax</label>
                                            </td>
                                            <td class="col-md-9" style="border: none">
                                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                <asp:Label ID="lblFax" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-md-3" style="border: none">
                                                <label>Website</label>
                                            </td>
                                            <td class="col-md-9" style="border: none">
                                                <span style="font-weight: bold; border: none">:&nbsp;</span>
                                                <asp:Label ID="lblWebAddress" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                Billing Address
                            </div>
                            <div class="panel-body">
                                <table class="table table-responsive">
                                    <tr>
                                        <td class="col-md-3" style="border: none">
                                            <label>Address</label>
                                        </td>
                                        <td class="col-md-9" style="border: none">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label ID="lblBillingAddress" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="col-md-3" style="border: none">
                                            <label>Country</label>
                                        </td>
                                        <td class="col-md-9" style="border: none">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label ID="lblBillingCountry" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="col-md-3" style="border: none">
                                            <label>State/ Province/ District</label>
                                        </td>
                                        <td class="col-md-9" style="border: none">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label ID="lblBillingState" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="col-md-3" style="border: none">
                                            <label>City</label>
                                        </td>
                                        <td class="col-md-9" style="border: none">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label ID="lblBillingCity" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="col-md-3" style="border: none">
                                            <label>Area</label>
                                        </td>
                                        <td class="col-md-9" style="border: none">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label ID="lblBillingStreet" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="col-md-3" style="border: none">
                                            <label>Post Code</label>
                                        </td>
                                        <td class="col-md-9" style="border: none">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label ID="lblBillingPostCode" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="panel panel-default" id="DivShippingAddress" runat="server">
                            <div class="panel-heading">
                                Shipping Address
                            </div>
                            <div class="panel-body">
                                <table class="table table-responsive">
                                    <tr>
                                        <td class="col-md-3" style="border: none">
                                            <label>Address</label>
                                        </td>
                                        <td class="col-md-9" style="border: none">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label ID="lblShippingAddress" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="col-md-3" style="border: none">
                                            <label>Country</label>
                                        </td>
                                        <td class="col-md-9" style="border: none">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label ID="lblShippingCountry" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="col-md-3" style="border: none">
                                            <label>State/ Province/ District</label>
                                        </td>
                                        <td class="col-md-9" style="border: none">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label ID="lblShippingState" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="col-md-3" style="border: none">
                                            <label>City</label>
                                        </td>
                                        <td class="col-md-9" style="border: none">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label ID="lblShippingCity" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="col-md-3" style="border: none">
                                            <label>Area</label>
                                        </td>
                                        <td class="col-md-9" style="border: none">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label ID="lblShippingStreet" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="col-md-3" style="border: none">
                                            <label>Post Code</label>
                                        </td>
                                        <td class="col-md-9" style="border: none">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label ID="lblShippingPostCode" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                Deal Information
                            </div>
                            <div class="panel-body">
                                <table id="tblDealInfo" class="table table-bordered table-condensed table-responsive" style="overflow-x: scroll">
                                    <thead>
                                        <tr>
                                            <th style="width: 35%">Deal Name</th>
                                            <th style="width: 20%">Amount</th>
                                            <th style="width: 25%">Deal Stage</th>
                                            <th style="width: 20%">Probability</th>
                                            <%--<th style="width: 20%">Action</th>--%>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                                <div class="col-md-12" style="text-align: right">
                                    <asp:Button ID="btnAddDeal" runat="server" Text="Add Deal" CssClass="TransactionalButton btn btn-primary btn-sm"
                                        OnClientClick="javascript: return CreateAnotherDeal();" TabIndex="6" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                Contacts Information
                            </div>
                            <div class="panel-body">
                                <table id="tblContactInfo" class="table table-bordered table-condensed table-responsive">
                                    <thead>
                                        <tr>
                                            <th style="width: 50%">Name</th>
                                            <th style="width: 25%">Title</th>
                                            <th style="width: 25%">Action</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                                <div class="row">
                                    <div class="col-md-12" style="text-align: right">
                                        <asp:Button ID="btnAddContact" runat="server" Text="Add Contact" CssClass="TransactionalButton btn btn-primary btn-sm"
                                            OnClientClick="javascript: return AddContact();" TabIndex="6" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <%--<div class="col-md-6" style="display: none">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                            </div>
                            <div class="panel-body">
                            </div>
                        </div>
                        <fieldset>
                            <legend>Ticket Information</legend>
                        </fieldset>
                    </div>--%>
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                Attachment
                            </div>
                            <div class="panel-body">
                                <div class="form-group">
                                    <div></div>
                                    <div id="companyDocumentInfo" style="overflow-x: scroll;">
                                    </div>
                                </div>

                                <div class="row" style="text-align: right">
                                    <div class="col-md-12">
                                        <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Add Attachment" onclick="LoadDocUploader()" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
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
    <div id="LogEntryPage" style="display: none;">
        <iframe id="logDoc" name="logDoc" width="100%" height="650" frameborder="0" style="overflow: hidden;"></iframe>
    </div>
</asp:Content>
