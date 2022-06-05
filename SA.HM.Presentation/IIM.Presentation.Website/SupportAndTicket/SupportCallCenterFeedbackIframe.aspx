<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="SupportCallCenterFeedbackIframe.aspx.cs" Inherits="HotelManagement.Presentation.Website.SupportAndTicket.SupportCallCenterFeedbackIframe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var isClose;
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;

        $(document).ready(function () {
            $('#ContentPlaceHolder1_txtFeedbackDate').keypress(function (event) {
                event.preventDefault();
                return false;
            });

            $('#ContentPlaceHolder1_txtFeedbackDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: 0,
                dateFormat: innBoarDateFormat,
                minDate: 0
            }).datepicker("setDate", 0);

            var supportCallId = $.trim(CommonHelper.GetParameterByName("sc"));

            if (supportCallId != "") {


                PerformEdit(supportCallId);
            }
            else {
                Clear();
            }
        });

        function PerformEdit(id) {
            //debugger;
            PageMethods.GetSupportFeedbackById(id, OnSuccessLoading, OnFailLoading);

            return false;
        }

        function OnSuccessLoading(result) {
            FillForm(result);
            return false;
        }

        function FillForm(Result) {
            //debugger;
            Clear();


            $("#ContentPlaceHolder1_hfSupportId").val(Result.Id);



            if (Result.SupportStatus != null && Result.SupportStatus != "")
                $("#ContentPlaceHolder1_ddlSupportStatus").val(Result.SupportStatus).trigger('change');
            if (Result.FeedbackDate != null)
                $("#ContentPlaceHolder1_txtFeedbackDate").val(GetStringFromDateTime(Result.FeedbackDate));


            $("#ContentPlaceHolder1_txtFeedback").val(Result.Feedback);

            $("#btnSave").val('Update').show();

            //$("#btnSave").val('Update').show();


            UploadComplete();
        }

        function Clear() {


            $("#ContentPlaceHolder1_ddlSupportStatus").val("Done").trigger('change');


            $("#ContentPlaceHolder1_txtFeedback").val("0");
            $("#ContentPlaceHolder1_hfSupportId").val("0");

            //$("#btnSave").val('Save');
            if (IsCanSave) {
                $("#btnSave").val('Save').show();
            }
            else {
                $("#btnSave").hide();
            }
            isClose = false;
            //$("#btnClear").show();
            return false;
        }


        // Documents Div
        function UploadComplete() {
            var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var id = +$("#ContentPlaceHolder1_hfSupportId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();

            PageMethods.LoadContactDocument(id, randomId, deletedDoc, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
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

                guestDocumentTable += "<td align='left' style='width: 50%; cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + guestDoc[row].Name + "</td>";

                if (guestDoc[row].Path != "") {
                    if (guestDoc[row].Extention == ".jpg" || guestDoc[row].Extention == ".png" || guestDoc[row].Extention == ".PNG" || guestDoc[row].Extention == ".JPG")
                        imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    else
                        imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                guestDocumentTable += "<td align='left' style='width: 30%;cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + imagePath + "</td>";

                guestDocumentTable += "<td align='left' style='width: 20%'>";
                guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteGuestDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                guestDocumentTable += "</td>";
                guestDocumentTable += "</tr>";
            }
            guestDocumentTable += "</table>";

            // docc = guestDocumentTable;

            $("#DocumentInfo").html(guestDocumentTable);
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
        }


        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }

        function CloseDialog() {
            $("#DocumentDialouge").dialog('close');
            return false;
        }


        function AttachFile() {

            var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var path = "/SupportAndTicket/Images/SupportAndTicket/";
            var category = "SupportAndTicketFeedbackDoc";
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

            //$("#documentsDiv").dialog({
            //    autoOpen: true,
            //    modal: true,
            //    width: 900,
            //    closeOnEscape: true,
            //    resizable: false,
            //    title: "Add Documents",
            //    show: 'slide'
            //});
        }

        function SaveNClose() {
            //debugger;
            isClose = true;
            //SaveOrUpdateTask();
            $.when(SaveOrUpdateSupportFeedback()).done(function () {
                if (isClose) {

                    if (typeof parent.CloseDialog === "function") {
                        parent.CloseDialog();
                    }

                }
            });
            return false;
        }

        function SaveOrUpdateSupportFeedback() {
            var id = +$("#ContentPlaceHolder1_hfSupportId").val();


            var feedbackDate = $("#ContentPlaceHolder1_txtFeedbackDate").val();;

            if (feedbackDate == "") {
                isClose = false;
                toastr.warning("Select Feedback Date");
                $("#ContentPlaceHolder1_txtFeedbackDate").focus();
                return false;
            }

            var supportStatus = $("#ContentPlaceHolder1_ddlSupportStatus").val();
            if (supportStatus == "") {
                isClose = false;
                toastr.warning("Select Support Status.");
                $("#ContentPlaceHolder1_ddlSupportStatus").focus();
                return false;
            }



            var feedback = $("#ContentPlaceHolder1_txtFeedback").val();

            var Support = {
                Id: id,
                FeedbackDate: CommonHelper.DateFormatToMMDDYYYY(feedbackDate, '/'),
                SupportStatus: supportStatus,
                Feedback: feedback

            }


            var hfRandom = $("#<%=RandomProductId.ClientID %>").val();
            var deletedDocuments = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();




            PageMethods.SaveOrUpdateSupportFeedback(Support, hfRandom, deletedDocuments, OnSuccessSaveOrUpdate, OnFailSaveOrUpdate);
            return false;


        }

        function OnFailLoading(error) {
            toastr.error(error.get_message());
            return false;
        }

        function OnSuccessSaveOrUpdate(result) {
            //debugger;
            if (result.IsSuccess) {
                if (result.IsSuccess) {
                    parent.ShowAlert(result.AlertMessage);
                    parent.SearchInformation(1, 1);
                }
                //if (typeof parent.GridPaging === "function")
                //    parent.GridPaging(1, 1);
                Clear();
            }
        }

        function OnFailSaveOrUpdate(error) {
            isClose = false;
            toastr.error(error.get_message());
            return false;
        }

    </script>



    <asp:HiddenField ID="hfSupportId" Value="0" runat="server" />

    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />

    <div>
        <div style="padding: 10px 30px 10px 30px">

            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2 ">
                        <label class="control-label required-field">Feedback Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFeedbackDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Support Status</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList runat="server" ID="ddlSupportStatus" CssClass="form-control">

                            <asp:ListItem Text="Done" Value="Done"></asp:ListItem>
                            <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                            <asp:ListItem Text="Decline" Value="Decline"></asp:ListItem>

                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Feedback</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtFeedback" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">

                    <asp:HiddenField ID="RandomProductId" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hfGuestDeletedDoc" runat="server" Value=""></asp:HiddenField>
                    <div class="col-md-2">
                        <label class="control-label">Attachment</label>
                    </div>
                    <div class="col-md-10">
                        <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Attach" onclick="AttachFile()" />
                    </div>
                </div>

                <div id="DocumentInfo">
                </div>

                <div id="DocumentDialouge" style="display: none;">
                    <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
                        clientidmode="static" scrolling="yes"></iframe>
                </div>

                <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                    <div class="col-md-12">
                        <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                            onclick="SaveNClose()" value="Save" />
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
