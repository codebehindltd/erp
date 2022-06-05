<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="DealImplementationFeedbackIFrame.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.DealImplementationFeedbackIFrame" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var flag = 0;
        var EnginnerList = [];
        var AllFlag = 0;
        $(document).ready(function () {
            var dealId = 0;
            dealId = $.trim(CommonHelper.GetParameterByName("did"));
            $('#ContentPlaceHolder1_hfDealId').val(dealId);
            $('#ContentPlaceHolder1_txtImpDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    //$('#ContentPlaceHolder1_txtServiceFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_ddlEngineers').select2();
            $("#ContentPlaceHolder1_ddlEngineers").change(function () {

                var len = $('#ContentPlaceHolder1_ddlEngineers option:selected').length;
                if (len > 1) {
                    var list = $('#ContentPlaceHolder1_ddlEngineers').val();
                    if (AllFlag == 0) {
                        $("#ContentPlaceHolder1_ddlEngineers").val(list[1]).trigger("change");
                        AllFlag = 1;
                    }
                    else if (jQuery.inArray("0", list) > -1) {
                        $("#ContentPlaceHolder1_ddlEngineers").val("0").trigger("change");
                        AllFlag = 0;
                    }
                }
            });
            FillForm(dealId);
        });
        function FillForm(dealId) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/DealImplementationFeedbackIFrame.aspx/GetImpFeedbackInfoByDealId',
                data: "{'dealId':'" + dealId + "'}",
                dataType: "json",
                async: false,
                success: function (data) {
                    OnFillFormSucceed(data.d);

                },
                error: function (result) {
                    OnFillFormFailed(result)
                }
            });
            return false;
        }
        function OnFillFormSucceed(result) {
            if (result.length > 0) {
                var infos = result[0].Infos;
                var engineers = result[0].Engineers;
                if (result[0].Engineers.length > 0) {
                    EnginnerList = result[0].Engineers.map((r) => r.ImpEngineerId);
                    $("#<%=btnSaveClose.ClientID %>").val("Update Feedback");
                }
                else {
                    $("#<%=btnSaveClose.ClientID %>").val("Save Feedback");
                }
                if (EnginnerList.length > 0)
                    $("#ContentPlaceHolder1_ddlEngineers").val(EnginnerList).trigger('change');
                $("#ContentPlaceHolder1_txtImpFeedback").val(result[0].Infos.ImplementationFeedback);
                $("#ContentPlaceHolder1_ddlStatus").val(result[0].Infos.ImplementationStatus);
                //$("#ContentPlaceHolder1_txtImpDate").val(CommonHelper.DateFromDateTimeToDisplay(result[0].Infos.ImplementationDate, innBoarDateFormat));
                if (result[0].Infos.ImplementationDate != null) {
                    $("#<%=txtImpDate.ClientID %>").val(CommonHelper.DateFromStringToDisplay(result[0].Infos.ImplementationDate, innBoarDateFormat));
                }
                //$("#ContentPlaceHolder1_RandomDocId").val(infos.Id);
                UploadComplete();
            }
            else {

            }
        }
        function OnFillFormFailed(error) {
            toastr.error(error.get_message());
            return false;
        }
        function SaveAndClose() {
            flag = 1;
            SaveDealFeedback();
            if (flag == 1) {
                PerformClearAction();
                if (typeof parent.CloseDialog === "function") {
                    parent.CloseDialog();
                }
            }
            return false;
        }
        function SaveDealFeedback() {
            var engineers = $("#ContentPlaceHolder1_ddlEngineers").val();
            var impDate = $("#ContentPlaceHolder1_txtImpDate").val();
            var impFeedback = $("#ContentPlaceHolder1_txtImpFeedback").val();
            // txtImpDate txtImpFeedback
            if (engineers.length == 0) {
                toastr.warning("Select Engineer");
                $("#ContentPlaceHolder1_ddlEngineers").focus();
                flag = 0;
                return false;
            }
            else if (impDate == "") {
                toastr.warning("Select Date");
                $("#ContentPlaceHolder1_txtImpDate").focus();
                flag = 0;
                return false;
            }

            impDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY($("#ContentPlaceHolder1_txtImpDate").val(), innBoarDateFormat);
            var list = new Array();

            engineers.forEach((r) => {
                list.push({
                    ImpEngineerId: r
                });
            });
            var hfRandom = $("#<%=RandomDocId.ClientID %>").val();
            var hfDealId = $("#<%=hfDealId.ClientID %>").val();
            var deletedDocuments = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            var implementationStatus = $("#ContentPlaceHolder1_ddlStatus").val();
            PageMethods.SaveUpdateDealFeedback(list, hfRandom, hfDealId, impFeedback, impDate, deletedDocuments, implementationStatus, OnSaveSucceed, OnSaveFailed);
            return false;
        }
        function OnSaveSucceed(result) {
            PerformClearAction();
            if (result.IsSuccess) {
                if (result.IsSuccess) {
                    parent.ShowAlert(result.AlertMessage);
                }
                PerformClearAction();
                //if (result.Data == null)
                $("#ContentPlaceHolder1_RandomDealId").val(result.Data);
            }

            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            PerformClearAction();
        }
        function OnSaveFailed(error) {
            CommonHelper.AlertMessage(error.AlertMessage);
            return false;
        }
        function PerformClearAction() {
            $("#<%=hfDealId.ClientID %>").val("0").trigger('change');
            $("#<%=txtImpDate.ClientID %>").val("").trigger('change');
            $("#<%=txtImpFeedback.ClientID %>").val("").trigger('change');
            EnginnerList = [];
            $("#ContentPlaceHolder1_ddlEngineers").val(EnginnerList).trigger('change');
            $("#ContactDocumentInfo").html('');
            flag = 0;
            return false;
        }
        //function AttachFile() {
        //    $("#implementationDocuments").dialog({
        //        autoOpen: true,
        //        modal: true,
        //        width: 900,
        //        closeOnEscape: true,
        //        resizable: false,
        //        title: "Deal Implementation Feedback Documents",
        //        show: 'slide'
        //    });
        //}
        //function UploadComplete() {
        //    var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
        //    var id = +$("#ContentPlaceHolder1_hfDealId").val();

        //    PageMethods.LoadImpFeedbackDocument(id, randomId, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
        //    return false;
        //}
        //function OnLoadDocumentSucceeded(result) {

        //    var guestDoc = result;
        //    var totalDoc = result.length;
        //    var row = 0;
        //    var imagePath = "";
        //    var guestDocumentTable = "";

        //    guestDocumentTable += "<table id='contactDocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
        //    guestDocumentTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";
        //    if (totalDoc > 0) {
        //        for (row = 0; row < totalDoc; row++) {
        //            if (row % 2 == 0) {
        //                guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
        //            }
        //            else {
        //                guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
        //            }

        //            guestDocumentTable += "<td align='left' style='width: 50%'>" + guestDoc[row].Name + "</td>";

        //            if (guestDoc[row].Path != "") {
        //                if (guestDoc[row].Extention == ".jpg")
        //                    imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
        //                else
        //                    imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
        //            }
        //            else
        //                imagePath = "";

        //            guestDocumentTable += "<td align='left' style='width: 30%'>" + imagePath + "</td>";

        //            guestDocumentTable += "<td align='left' style='width: 20%'>";
        //            guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteGuestDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
        //            guestDocumentTable += "</td>";
        //            guestDocumentTable += "</tr>";
        //        }
        //        guestDocumentTable += "</table>";

        //        // docc = guestDocumentTable;

        //        $("#ContactDocumentInfo").html(guestDocumentTable);
        //    }

        //}

        <%--function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }
        function DeleteGuestDoc(docId, rowIndex) {
            if (confirm("Want to delete?")) {
                var deletedDoc = $("#<%=hfGuestDeletedDoc.ClientID %>").val();

                if (deletedDoc != "")
                    deletedDoc += "," + docId;
                else
                    deletedDoc = docId;

                $("#<%=hfGuestDeletedDoc.ClientID %>").val(deletedDoc);

                $("#trdoc" + rowIndex).remove();
            }
        }--%>
        function LoadDocUploader() {

            var randomId = +$("#ContentPlaceHolder1_RandomDocId").val();
            var path = "/SalesAndMarketing/Images/Deal/";
            var category = "SalesDealFeedbackDocuments";
            var iframeid = 'frmPrint';
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

        function CloseDialog() {
            $("#DocumentDialouge").dialog('close');
            return false;
        }
        function UploadComplete() {
            var randomId = $("#ContentPlaceHolder1_RandomDocId").val();
            ShowUploadedDocument(randomId);
        }

        function ShowUploadedDocument(randomId) {
            var id = $("#ContentPlaceHolder1_hfDealId").val();
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
    </script>

    <asp:HiddenField ID="hfDealId" runat="server"></asp:HiddenField>
   <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="tempDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfParentDoc" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />

    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="ShowDocumentDiv" style="display: none;">
        <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="SalesNoteDialog">
        <%--<div id="implementationDocuments" style="display: none;">
            <label for="Attachment" class="control-label col-md-2">
                Attachment</label>
            <div class="col-md-4">
                <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
                    <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                        FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
                </asp:Panel>
            </div>
        </div>--%>
        <div class="form-horizontal">
            <div class="panel panel-default">
                <div class=" panel-body">
                    <div class="form-group">
                        <label class="control-label col-md-2 ">Engineer Name</label>
                        <div class="col-sm-10">
                            <asp:DropDownList ID="ddlEngineers" runat="server" CssClass="form-control" Style="width: 100%;" multiple="multiple" name="states[]">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-md-2 ">Implement Date</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtImpDate" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-md-2 ">Status</label>
                        <div class="col-sm-10">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                <asp:ListItem Value="Ongoing">Ongoing</asp:ListItem>
                                <asp:ListItem Value="Completed">Completed</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-md-2 ">Implement Feedback</label>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtImpFeedback" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <%-- <div class="form-group">
                        <label class="control-label col-md-2 ">Attachment</label>
                        <div class="col-md-10">
                            <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Attach" onclick="AttachFile()" />
                        </div>
                    </div>

                    <div id="ContactDocumentInfo">
                    </div>--%>
                    <div class="form-group">
                            <label class="control-label col-md-2">Attachment</label>
                        <div class="col-md-4">
                            <input id="btnImageUp" type="button" onclick="javascript: return LoadDocUploader();"
                                class="TransactionalButton btn btn-primary btn-sm" value="Deal Implementation Doc..." />
                        </div>
                    </div>
                    <div id="DocumentInfo">
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button ID="btnSaveClose" runat="server" Text="Save Feedback" OnClientClick="javascript:return SaveAndClose();"
                                CssClass="TransactionalButton btn btn-primary btn-sm" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
