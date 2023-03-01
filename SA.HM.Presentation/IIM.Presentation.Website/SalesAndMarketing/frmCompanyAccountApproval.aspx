<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmCompanyAccountApproval.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.frmCompanyAccountApproval" %>
<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ContentPlaceHolder1_ddlLegalAction").change(function () {
                var legalAction = $("#ContentPlaceHolder1_ddlLegalAction").val();
                if (legalAction == 0) {
                    $("#legalActionDiv").hide();
                }
                else {
                    $("#legalActionDiv").show();
                }
            });
            $("#ContentPlaceHolder1_txtCreditLimitExpireDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            
            $("#ContentPlaceHolder1_txtShortCreditLimitDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
                        
            $("#ContentPlaceHolder1_txtTransactionDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#ContentPlaceHolder1_txtGuestCompany").autocomplete({
                minLength: 3,
                selectFirst: true,
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../SalesAndMarketing/frmCompanyAccountApproval.aspx/GetCompanyInfoForAccountApproval",
                        data: JSON.stringify({ searchTerm: request.term }),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.CompanyName,
                                    value: m.CompanyId,

                                };
                            });
                            response(searchData);
                        },
                        failed: function (result) {

                        }
                    });
                },
                focus: function (event, ui) {
                    event.preventDefault();
                },
                select: function (event, ui) {
                    event.preventDefault();
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfGuestCompanyId").val(ui.item.value);
                    let guestCompanyId = ui.item.value;
                    LoadCompanyInformationForFillForm(guestCompanyId);
                }
            });
        });
        function LoadCompanyInformationForFillForm(companyId) {
            PageMethods.GetCompanyInformationForFillForm(companyId, OnGetCompanyInformationForFillFormSucceeded, OnGetCompanyInformationForFillFormFailed);
            return false;
        }
        function OnGetCompanyInformationForFillFormSucceeded(result) {
            var creditLimit = result.CreditLimit > 0 ? result.CreditLimit : "";
            var creditLimitExpire = result.CreditLimitExpire == null ? "" : GetStringFromDateTime(result.CreditLimitExpire);
            var shortCreditLimit = result.ShortCreditLimit > 0 ? result.ShortCreditLimit : "";
            var shortCreditLimitExpire = result.ShortCreditLimitExpire == null ? "" : GetStringFromDateTime(result.ShortCreditLimitExpire);
            var transportFareFactory = result.TransportFareFactory > 0 ? result.TransportFareFactory : "";
            var transportFareDepo = result.TransportFareDepo > 0 ? result.TransportFareDepo : "";
            var salesCommission = result.SalesCommission > 0 ? result.SalesCommission : "";

            var legalAction = result.LegalAction == null ? 0 : result.LegalAction;
            var transactionDate = result.TransactionDate == null ? "" : GetStringFromDateTime(result.TransactionDate);
            var detailDescription = result.DetailDescription;
            var callToAction = result.CallToAction;


            $("#ContentPlaceHolder1_txtCreditLimit").val(creditLimit);
            $("#ContentPlaceHolder1_txtCreditLimitExpireDate").val(creditLimitExpire);
            $("#ContentPlaceHolder1_txtShortCreditLimit").val(shortCreditLimit);
            $("#ContentPlaceHolder1_txtShortCreditLimitDate").val(shortCreditLimitExpire);
            $("#ContentPlaceHolder1_txtTransportFareFactory").val(transportFareFactory);
            $("#ContentPlaceHolder1_txtTransportFareDepo").val(transportFareDepo);
            $("#ContentPlaceHolder1_txtSalesCommission").val(salesCommission);

            $("#ContentPlaceHolder1_ddlLegalAction").val(parseInt(legalAction)).trigger('change');

            $("#ContentPlaceHolder1_txtTransactionDate").val(transactionDate);
            $("#ContentPlaceHolder1_txtDetailDescription").val(detailDescription);
            $("#ContentPlaceHolder1_txtCallToAction").val(callToAction);
            ShowUploadedDocument($("#ContentPlaceHolder1_RandomDocId").val());
        }
        function OnGetCompanyInformationForFillFormFailed(error) {
            alert(error.get_message());
        }
        function LoadDocUploader() {
            var randomId = +$("#ContentPlaceHolder1_RandomDocId").val();
            var path = "/SalesAndMarketing/Images/";
            var category = "CompanyAccountApprovalInfo";
            var iframeid = 'frmPrint';
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
        function UploadComplete() {
            var randomId = $("#ContentPlaceHolder1_RandomDocId").val();
            ShowUploadedDocument(randomId);
        }
        function ShowUploadedDocument(randomId) {
            var id = $("#ContentPlaceHolder1_hfGuestCompanyId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            PageMethods.GetUploadedDocByWebMethod(randomId, id, deletedDoc, OnGetUploadedDocByWebMethodSucceeded, OnGetUploadedDocByWebMethodFailed);
            return false;
        }
        function OnGetUploadedDocByWebMethodSucceeded(result) {
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            DocTable = "";

            DocTable += "<table id='DocTableList' style='width:100%' class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            DocTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    DocTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    DocTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }
                DocTable += "<td align='left' style='width: 50%;cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + "','" + result[row].Name + "');\">" + result[row].Name + "</td>";

                if (result[row].Path != "") {
                    imagePath = "<img src='" + result[row].Path + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                DocTable += "<td align='left' style='width: 30%'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + "','" + result[row].Name + "');\">" + imagePath + "</td>";

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
        function ShowDocument(path, name) {
            var iframeid = 'fileIframe';
            document.getElementById(iframeid).src = path;
            $("#ShowDocumentDiv").dialog({
                autoOpen: true,
                modal: true,
                width: "82%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Document - " + name,
                show: 'slide'
            });
            return false;
        }

        function ValidationBeforeSave() {
            var companyId = $("#ContentPlaceHolder1_hfGuestCompanyId").val();
            var companyName = $("#ContentPlaceHolder1_txtGuestCompany").val();
            var creditLimit = $("#ContentPlaceHolder1_txtCreditLimit").val();
            var creditLimitExpire = $("#ContentPlaceHolder1_txtCreditLimitExpireDate").val();
            var shortCrLimit = $("#ContentPlaceHolder1_txtShortCreditLimit").val();
            var shortCrLimitExpire = $("#ContentPlaceHolder1_txtShortCreditLimitDate").val();
            var transportFareFactory = $("#ContentPlaceHolder1_txtTransportFareFactory").val();
            var transportFareDepo = $("#ContentPlaceHolder1_txtTransportFareDepo").val();
            var salesCommission = $("#ContentPlaceHolder1_txtSalesCommission").val();
            var transactionDate = "", detailDescription = "", callToAction = "";
            if (companyName == "") {
                toastr.warning("Please Enter Company.");
                $("#ContentPlaceHolder1_txtGuestCompany").focus();
                return false;
            }
            else if(creditLimit == ""){
                toastr.warning("Please Give Credit Limit.");
                $("#ContentPlaceHolder1_txtCreditLimit").focus();
                return false;
            }
            else if (creditLimitExpire == "") {
                toastr.warning("Please Give Credit Limit Expire.");
                $("#ContentPlaceHolder1_txtCreditLimitExpireDate").focus();
                return false;
            }
            else if (shortCrLimit == "") {
                toastr.warning("Please Give Short Credit Limit.");
                $("#ContentPlaceHolder1_txtShortCreditLimit").focus();
                return false;
            }
            else if (shortCrLimitExpire == "") {
                toastr.warning("Please Give Short Credit Limit Expire.");
                $("#ContentPlaceHolder1_txtShortCreditLimitDate").focus();
                return false;
            }
            else if (transportFareFactory == "") {
                toastr.warning("Please Give Transport Fare (Factory).");
                $("#ContentPlaceHolder1_txtTransportFareFactory").focus();
                return false;
            }
            else if (transportFareDepo == "") {
                toastr.warning("Please Give Transport Fare (Depo).");
                $("#ContentPlaceHolder1_txtTransportFareDepo").focus();
                return false;
            }
            else if (salesCommission == "") {
                toastr.warning("Please Give Sales Commission.");
                $("#ContentPlaceHolder1_txtSalesCommission").focus();
                return false;
            }

            var legalAction = $("#ContentPlaceHolder1_ddlLegalAction").val();
            var LegalActions = {};
            if (legalAction == 1) {
                transactionDate = $("#ContentPlaceHolder1_txtTransactionDate").val();
                detailDescription = $("#ContentPlaceHolder1_txtDetailDescription").val();
                callToAction = $("#ContentPlaceHolder1_txtCallToAction").val();
                if (transactionDate == "") {
                    toastr.warning("Please Give Transaction Date.");
                    $("#ContentPlaceHolder1_txtTransactionDate").focus();
                    return false;
                }
                else if (detailDescription == "") {
                    toastr.warning("Please Give Detail Description.");
                    $("#ContentPlaceHolder1_txtDetailDescription").focus();
                    return false;
                }
                else if (callToAction == "") {
                    toastr.warning("Please Give Call To Action.");
                    $("#ContentPlaceHolder1_txtCallToAction").focus();
                    return false;
                }
                if (transactionDate != '') {
                    transactionDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(transactionDate, innBoarDateFormat);
                }
                LegalActions = {
                    CompanyId: companyId,
                    TransactionDate: transactionDate,
                    DetailDescription: detailDescription,
                    CallToAction: callToAction
                }
            }
            if (creditLimitExpire != '') {
                creditLimitExpire = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(creditLimitExpire, innBoarDateFormat);
            }
            if (shortCrLimitExpire != '') {
                shortCrLimitExpire = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(shortCrLimitExpire, innBoarDateFormat);
            }


            var BenefitList = {
                CompanyId: companyId,
                CompanyName: companyName,
                CreditLimit: creditLimit,
                CreditLimitExpire: creditLimitExpire,
                ShortCreditLimit: shortCrLimit,
                ShortCreditLimitExpire: shortCrLimitExpire,
                TransportFareFactory: transportFareFactory,
                TransportFareDepo: transportFareDepo,
                SalesCommission: salesCommission,
                LegalAction: legalAction
            }
            
            
            var randomDocId = $("#ContentPlaceHolder1_RandomDocId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            CommonHelper.SpinnerOpen();
            PageMethods.SaveCompanyAccountApprovalInfo(BenefitList, LegalActions, parseInt(randomDocId), deletedDoc, OnSaveCompanyAccountApprovalInfoSucceeded, OnSaveCompanyAccountApprovalInfoFailed);
            return false;
        }
        function OnSaveCompanyAccountApprovalInfoSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
                $("#ContentPlaceHolder1_RandomDocId").val(result.Data);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            CommonHelper.SpinnerClose();
            return false;
        }
        function OnSaveCompanyAccountApprovalInfoFailed(error) {
            toastr.error(error.get_message());
            CommonHelper.SpinnerClose();
        }
        function PerformClearActionWithConfirmation() {
            if (!confirm("Do You Want To Clear?")) {
                return false;
            }
            PerformClearAction();
        }
        function PerformClearAction() {
            $("#ContentPlaceHolder1_txtGuestCompany").val("");
            $("#ContentPlaceHolder1_txtCreditLimit").val("");
            $("#ContentPlaceHolder1_txtCreditLimitExpireDate").val("");
            $("#ContentPlaceHolder1_txtShortCreditLimit").val("");
            $("#ContentPlaceHolder1_txtShortCreditLimitDate").val("");
            $("#ContentPlaceHolder1_txtTransportFareFactory").val("");
            $("#ContentPlaceHolder1_txtTransportFareDepo").val("");
            $("#ContentPlaceHolder1_txtSalesCommission").val("");

            $("#ContentPlaceHolder1_ddlLegalAction").val("0").trigger('change');

            $("#ContentPlaceHolder1_txtTransactionDate").val("");
            $("#ContentPlaceHolder1_txtDetailDescription").val("");
            $("#ContentPlaceHolder1_txtCallToAction").val("");
            $("#DocumentInfo").html("");
        }
    </script>
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0" />
    <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfGuestCompanyId" runat="server" Value="0"></asp:HiddenField>
    <div id="ShowDocumentDiv" style="display: none;">
        <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Company Account Approval
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblGuestCompany" runat="server" class="control-label required-field" Text="Company"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtGuestCompany" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">
                    Benefits
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCreditLimit" runat="server" class="control-label required-field" Text="Credit Limit"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCreditLimit" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblCreditLimitExpireDate" runat="server" class="control-label required-field" Text="Credit Limit Expire Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCreditLimitExpireDate" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblShortCreditLimit" runat="server" class="control-label required-field" Text="Short Credit Limit"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtShortCreditLimit" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblShortCreditLimitDate" runat="server" class="control-label required-field" Text="Short Credit Limit Expire Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtShortCreditLimitDate" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblTransportFareFactory" runat="server" class="control-label required-field" Text="Transport Fare (Factory)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTransportFareFactory" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblTransportFareDepo" runat="server" class="control-label required-field" Text="Transport Fare (Depo)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTransportFareDepo" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSalesCommission" runat="server" class="control-label required-field" Text="Sales Commission"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSalesCommission" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">
                    Legal Action
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblLegalAction" runat="server" class="control-label required-field" Text="Legal Action"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlLegalAction" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="0">No Legal Action</asp:ListItem>
                                    <asp:ListItem Value="1">Under Process For Legal Action</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="legalActionDiv" style="display: none">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblTransactionDate" runat="server" class="control-label required-field" Text="Transaction Date"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtTransactionDate" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblDetailDescription" runat="server" class="control-label required-field" Text="Detail Description"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtDetailDescription" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblCallToAction" runat="server" class="control-label required-field" Text="Call To Action"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtCallToAction" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="AttachmentDiv" class="childDivSection">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Document Management
                    </div>
                    <div class="panel-body childDivSectionDivBlockBody">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <label class="control-label">Attachment</label>
                                </div>
                                <div class="col-md-4">
                                    <input id="btnImageUp" type="button" onclick="javascript: return LoadDocUploader();"
                                        class="TransactionalButton btn btn-primary btn-sm" value="Documents..." />
                                </div>
                            </div>
                            <div id="DocumentInfo">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="DocumentDialouge" style="display: none;">
                <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
                    clientidmode="static" scrolling="yes"></iframe>
            </div>
            <div class="form-group" style="padding-top: 10px;">
                <div class="col-md-12">
                    <input id="btnSave" type="button" value="Update" onclick="ValidationBeforeSave()"
                        class="TransactionalButton btn btn-primary btn-sm" />
                    <input id="btnCancelTicket" type="button" value="Cancel" onclick="PerformClearActionWithConfirmation()"
                        class="TransactionalButton btn btn-primary btn-sm" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>