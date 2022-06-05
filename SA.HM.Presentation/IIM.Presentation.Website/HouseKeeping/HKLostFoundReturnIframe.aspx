<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="HKLostFoundReturnIframe.aspx.cs" Inherits="HotelManagement.Presentation.Website.HouseKeeping.HKLostFoundReturnIframe" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtReturnDate.ClientID%>").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            var lostItemId = $.trim(CommonHelper.GetParameterByName("lid"));

            if (lostItemId != "") {
                FillForm(lostItemId);
                $("#<%=hfId.ClientID %>").val(lostItemId);
            }
        });
        function FillForm(Id) {
             PageMethods.FillForm(Id, OnFillFormSucceed, OnFailed);
            return false;
        }
        function OnFillFormSucceed(result) {
            if (result.HasItemReturned) {
                $("#<%=btnReturn.ClientID %>").val("Update Return");
                $("#<%=txtReturnDate.ClientID %>").val(CommonHelper.DateFromDateTimeToDisplay(result.ReturnDate, innBoarDateFormat));
                $("#<%=txtReturnDescription.ClientID %>").val(result.ReturnDescription);
                $("#<%=txtWhomToReturn.ClientID %>").val(result.WhomToReturn);
                ShowUploadedDocument($("#ContentPlaceHolder1_RandomDocId").val());
            }
            return false;
            
        }
        function PerformReturn() {
            var returnDate = $("#<%=txtReturnDate.ClientID %>").val();
            var returnDescription = $("#<%=txtReturnDescription.ClientID %>").val();
            var whomToReturn = $("#<%=txtWhomToReturn.ClientID %>").val();
            var id = $("#<%=hfId.ClientID %>").val();
            if (returnDate == "") {
                toastr.warning("Please Insert Return Date");
                $("#<%=txtReturnDate.ClientID %>").focus();
                return false;
            }
            else if (whomToReturn == "") {
                toastr.warning("Please Insert Whom To Return");
                $("#<%=txtWhomToReturn.ClientID %>").focus();
                return false;
            }

            var hfRandom = $("#<%=RandomDocId.ClientID %>").val();
            var deletedDocuments = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            if (deletedDocuments == "0") {
                deletedDocuments = "";
            }
            PageMethods.PerformReturnUpdate(id, returnDate, returnDescription, whomToReturn, hfRandom, deletedDocuments, OnReturnSucceed, OnFailed);
            return false;
        }
        function OnReturnSucceed(result) {
            if (result.IsSuccess == true) {

                if (typeof parent.CloseReturnDialog === "function")
                    parent.CloseReturnDialog();
                if (typeof parent.ShowAlert === "function")
                    parent.ShowAlert(result.AlertMessage);
                if (typeof parent.GridPaging === "function")
                    parent.GridPaging(1, 1);

                ClearReturn();

            }
            return false;
        }
        function ClearReturn() {
            $("#<%=txtReturnDate.ClientID %>").val("");
            $("#<%=txtReturnDescription.ClientID %>").val("");
            $("#<%=txtWhomToReturn.ClientID %>").val("");
            $("#<%=hfId.ClientID %>").val("0");
        }
        function OnFailed(error) {
            toastr.warning(error);
            return false;
        }
        //documents 

        function AttachFileReturn() {
            $("#documents").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Return Documents",
                show: 'slide'
            });
        }
        function ShowUploadedDocument(randomId) {
            var id = +$("#ContentPlaceHolder1_hfId").val();
            var deletedDoc = $("#<%=hfDeletedDoc.ClientID %>").val();
            PageMethods.GetUploadedDocByWebMethod(id, randomId, deletedDoc, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }
        function UploadComplete() {
            var randomId = +$("#<%=RandomDocId.ClientID %>").val();
            <%--var id = +$("#ContentPlaceHolder1_hfId").val();
            var deletedDoc = $("#<%=hfDeletedDoc.ClientID %>").val();--%>
            ShowUploadedDocument(randomId);
        }

        function OnLoadDocumentSucceeded(result) {
            var guestDoc = result;
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";

            guestDocumentTable += "<table id='DocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            guestDocumentTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }

                guestDocumentTable += "<td align='left' style='width: 50%'>" + guestDoc[row].Name + "</td>";

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
                guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteGuestDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                guestDocumentTable += "</td>";
                guestDocumentTable += "</tr>";
            }
            guestDocumentTable += "</table>";

            $("#DocumentInfo").html(guestDocumentTable);
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }

        function DeleteGuestDoc(docId, rowIndex) {
            if (confirm("Want to delete?")) {
                var deletedDoc = $("#<%=hfDeletedDoc.ClientID %>").val();

                if (deletedDoc != "")
                    deletedDoc += "," + docId;
                else
                    deletedDoc = docId;

                $("#<%=hfDeletedDoc.ClientID %>").val(deletedDoc);

                $("#trdoc" + rowIndex).remove();
            }
        }
    </script>
    <div id="documents" style="display: none;">
        <label for="Attachment" class="control-label col-md-2">
            Attachment</label>
        <div class="col-md-4">
            <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
                <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                    FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
            </asp:Panel>
        </div>
    </div>
    <%--<div id="ReturnDialogue" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>--%>
    <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="tempDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfParentDoc" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>
    <div id="ReturnDiv" class="panel panel-default">
        <div class="panel-heading">
            Return Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label runat="server" class="control-label required-field" Text="Return Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtReturnDate" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label runat="server" class="control-label required-field" Text="Whom to Return"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtWhomToReturn" runat="server" CssClass="form-control">
                        </asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label6" runat="server" class="control-label" Text="Return Description"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtReturnDescription" CssClass="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="Attachment" class="control-label col-md-2">Attachment</label>

                    <div class="col-md-10">
                        <input type="button" id="btnAttachmentReturn" class="TransactionalButton btn btn-primary btn-sm" value="Attach" onclick="AttachFileReturn()" />
                    </div>
                </div>
                <div id="DocumentInfo">
                </div>
                &nbsp;
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnReturn" runat="server" Text="Return" OnClientClick="javascript: return PerformReturn();"
                            CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />
                        <%--<asp:Button ID="btnClearReturn" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return ClearReturn();" TabIndex="6" />--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
