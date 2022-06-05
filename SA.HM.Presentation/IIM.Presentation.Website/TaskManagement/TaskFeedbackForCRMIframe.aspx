<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="TaskFeedbackForCRMIframe.aspx.cs" Inherits="HotelManagement.Presentation.Website.TaskManagement.TaskFeedbackForCRMIframe" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var taskFor, taskType, isClose = 1;
        $(document).ready(function () {
            taskId = $.trim(CommonHelper.GetParameterByName("tid"));
            employeeId = $.trim(CommonHelper.GetParameterByName("eid"));
            taskFor = $.trim(CommonHelper.GetParameterByName("tFor"));
            taskType = $.trim(CommonHelper.GetParameterByName("tType"));
            debugger;
            if (taskType == "Meeting") {
                $("#divNonMeeting").hide();
                $("#divMeeting").show();
            } else {
                $("#divNonMeeting").show();
                $("#divMeeting").hide();
            }
            $('#ContentPlaceHolder1_txtStartTime').timepicker({
                showPeriod: is12HourFormat
            });

            $('#ContentPlaceHolder1_txtStartDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtFinishTime').timepicker({
                showPeriod: is12HourFormat
            });

            $('#ContentPlaceHolder1_txtFinishDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);
            $("#ContentPlaceHolder1_ddlParticipantFromClient").select2({
            });
            $("#ContentPlaceHolder1_ddlParticipantFromOffice").select2({
            });
        });
        function Clear() {
            $("#ContentPlaceHolder1_ddlImplementationStatus").val("Not Started");
            $("#ContentPlaceHolder1_ddlTaskStage").val("0");
            $("#ContentPlaceHolder1_txtStartDate").val(DayOpenDate);
            $("#ContentPlaceHolder1_txtFinishDate").val(DayOpenDate);
            $("#ContentPlaceHolder1_txtStartTime").val("");
            $("#ContentPlaceHolder1_txtFinishTime").val("");
            $("#ContentPlaceHolder1_txtTaskFeedback").val("");
            $("#ContentPlaceHolder1_txtMeetingAgenda").val("");
            $("#ContentPlaceHolder1_txtMeetingLocation").val("");
            $("#ContentPlaceHolder1_txtMeetingDiscussion").val("");
            $("#ContentPlaceHolder1_txtCallToAction").val("");
            $("#ContentPlaceHolder1_ddlParticipantFromClient").val(null).trigger('change');
            $("#ContentPlaceHolder1_ddlParticipantFromOffice").val(null).trigger('change');
            return false;
        }
        function SaveNClose() {
            var TaskId = taskId;
            var EmployeeId = employeeId;
            var Id, MeetingAgenda, MeetingLocation, MeetingDiscussion, CallToAction, ImplementationStatus, TaskStage, StartDate, FinishDate, StartTime, FinishTime, TaskFeedback;
            var ParticipantFromClient = "", ParticipantFromOffice = "";
            var clientId = new Array();
            var clientLIst = "";
            var empList = "";
            Id = $("#ContentPlaceHolder1_Id").val();
            ImplementationStatus = $("#ContentPlaceHolder1_ddlImplementationStatus").val();
            TaskStage = $("#ContentPlaceHolder1_ddlTaskStage").val();
            if (TaskStage == "0") {
                toastr.warning("Please Select Task Done(%)");
                $("#ContentPlaceHolder1_ddlTaskStage").focus();
                isClose = 0;
                return false;
            }
            StartDate = $("#ContentPlaceHolder1_txtStartDate").val();
            if (StartDate == "") {
                toastr.warning("Please Add Start Date");
                $("#ContentPlaceHolder1_txtStartDate").focus();
                isClose = 0;
                return false;
            }
            else {
                StartDate = CommonHelper.DateFormatToMMDDYYYY(StartDate, '/');
            }

            FinishDate = $("#ContentPlaceHolder1_txtFinishDate").val();
            if (FinishDate == "") {
                toastr.warning("Please Add Finish Date");
                $("#ContentPlaceHolder1_txtFinishDate").focus();
                isClose = 0;
                return false;
            }
            else {
                FinishDate = CommonHelper.DateFormatToMMDDYYYY(FinishDate, '/');
            }

            StartTime = $("#ContentPlaceHolder1_txtStartTime").val();
            if (StartTime == "") {
                toastr.warning("Please Add Start Time");
                $("#ContentPlaceHolder1_txtStartTime").focus();
                isClose = 0;
                return false;
            }
            else {
                StartTime = moment(StartTime, ["h:mm A"]).format("HH:mm");
            }

            FinishTime = $("#ContentPlaceHolder1_txtFinishTime").val();
            if (FinishTime == "") {
                toastr.warning("Please Add Start Time");
                $("#ContentPlaceHolder1_txtFinishTime").focus();
                isClose = 0;
                return false;
            }
            else {
                FinishTime = moment(FinishTime, ["h:mm A"]).format("HH:mm");
            }
            if (taskType == "Meeting") {
                MeetingAgenda = $("#ContentPlaceHolder1_txtMeetingAgenda").val();
                if (MeetingAgenda == "") {
                    toastr.warning("Please Add Meeting Agenda");
                    $("#ContentPlaceHolder1_txtMeetingAgenda").focus();
                    isClose = 0;
                    return false;
                }
                MeetingLocation = $("#ContentPlaceHolder1_txtMeetingLocation").val();
                if (MeetingLocation == "") {
                    toastr.warning("Please Add  Meeting Location");
                    $("#ContentPlaceHolder1_txtMeetingLocation").focus();
                    isClose = 0;
                    return false;
                }
                MeetingDiscussion = $("#ContentPlaceHolder1_txtMeetingDiscussion").val();
                if (MeetingDiscussion == "") {
                    toastr.warning("Please Add  Meeting Discussion");
                    $("#ContentPlaceHolder1_txtMeetingDiscussion").focus();
                    isClose = 0;
                    return false;
                }
                CallToAction = $("#ContentPlaceHolder1_txtCallToAction").val();
                if (CallToAction == "") {
                    toastr.warning("Please Add a Call to Action");
                    $("#ContentPlaceHolder1_txtCallToAction").focus();
                    isClose = 0;
                    return false;
                }
                clientId = new Array();
                clientId = $("#ContentPlaceHolder1_ddlParticipantFromClient").val();
                clientLIst = $('#ContentPlaceHolder1_hfSelectedEmpId').val(clientId).val();
                if (clientLIst == "") {
                    toastr.warning("Please Add a Participant From Client ");
                    $("#ContentPlaceHolder1_ddlParticipantFromClient").focus();
                    isClose = 0;
                    return false;
                }

                clientId = new Array();
                clientId = $("#ContentPlaceHolder1_ddlParticipantFromOffice").val();
                empList = $('#ContentPlaceHolder1_hfSelectedEmpId').val(clientId).val();
                if (empList == "") {
                    toastr.warning("Please Add a Participant From Office");
                    $("#ContentPlaceHolder1_ddlParticipantFromOffice").focus();
                    isClose = 0;
                    return false;
                }

            }
            else {
                TaskFeedback = $("#ContentPlaceHolder1_txtTaskFeedback").val();
                if (TaskFeedback == "") {
                    toastr.warning("Please Add Task Feedback");
                    $("#ContentPlaceHolder1_txtTaskFeedback").focus();
                    isClose = 0;
                    return false;
                }
            }
            var SMTaskFeedback = {
                Id: Id,
                TaskId: TaskId,
                EmployeeId: EmployeeId,
                ImplementationStatus: ImplementationStatus,
                TaskStage: TaskStage,
                TaskFeedback: TaskFeedback,
                StartDate: StartDate,
                StartTime: StartTime,
                FinishDate: FinishDate,
                FinishTime: FinishTime,
                MeetingAgenda: MeetingAgenda,
                MeetingLocation: MeetingLocation,
                MeetingDiscussion: MeetingDiscussion,
                CallToAction: CallToAction
            }
            var randomDocId = $("#ContentPlaceHolder1_RandomDocId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../TaskManagement/TaskFeedbackForCRMIframe.aspx/SaveTaskFeedback",
                dataType: "json",
                data: JSON.stringify({ SMTaskFeedback: SMTaskFeedback, participantFromCompany: empList, participantFromClient: clientLIst, randomDocId: randomDocId, deletedDoc: deletedDoc }),
                async: false,
                success: (data) => {
                    OnSaveSalesLogSucceed(data.d);
                },
                error: (error) => {
                    isClose = 0;
                    toastr.error(error, "", { timeOut: 5000 });
                }
            });
            return false;
        }
        function SaveNCovertAsLog() {
            PageMethods.GetTaskId(taskId, OnSuccessLoading, OnFailLoading);
            return false;
        }
        function OnSuccessLoading(result) {
            isClose = 0;
            SaveNClose();
            var logId = result.Task.LogId ? result.Task.LogId : 0;
            var logId = result.Task.LogId ? result.Task.LogId : 0;
            var companyId = result.Task.CompanyId ? result.Task.CompanyId : 0;
            var contactId = result.Task.ContactId ? result.Task.ContactId : 0;
            var dealId = result.Task.DealId ? result.Task.DealId : 0;
            var taskType = result.Task.TaskType;
            var StartDate = $("#ContentPlaceHolder1_txtStartDate").val();
            var StartTime = $("#ContentPlaceHolder1_txtStartTime").val();
            var TaskFeedback = $("#ContentPlaceHolder1_txtTaskFeedback").val();
            var MeetingAgenda = $("#ContentPlaceHolder1_txtMeetingAgenda").val();
            var MeetingLocation = $("#ContentPlaceHolder1_txtMeetingLocation").val();
            var MeetingDiscussion = $("#ContentPlaceHolder1_txtMeetingDiscussion").val();
            var CallToAction = $("#ContentPlaceHolder1_txtCallToAction").val();

            var iframeid = 'frmPrint';
            var url = "../SalesAndMarketing/SalesCallEntry.aspx?id=" + logId + "&cid=" + companyId + "&ctid=" + contactId + "&from=task &date=" + StartDate + "&tType=" + taskType + "&did=" + dealId +
                "&time=" + StartTime + "&tfb=" + TaskFeedback + "&ta=" + MeetingAgenda + "&tl=" + MeetingLocation + "&td=" + MeetingDiscussion + "&tcta=" + CallToAction ;
            document.getElementById(iframeid).src = url;
            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "95%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                title: "Log Entry",
                show: 'slide'
            });
            return false;
        }
        function OnFailLoading(error) {
            return false;
        }

        function OnSaveSalesLogSucceed(data) {
            debugger;
            $("#ContentPlaceHolder1_RandomDocId").val(data.Data);
            if (data.IsSuccess) {
                if (typeof parent.ShowAlert === "function") {
                    parent.ShowAlert(data.AlertMessage);
                }
                if ((typeof parent.CloseTaskDialog === "function") && (isClose == 1)) {
                    parent.CloseTaskDialog();
                    Clear();
                }
            }
            return false;
        }
        function DialogCloseAfterUpdate() {
            Clear();
            $("#SalesNoteDialog").dialog('close');
            if (typeof parent.CloseTaskDialog === "function") {
                parent.CloseTaskDialog();

            }
            if (typeof parent.ShowAlertMessege === "function") {
                parent.ShowAlertMessege();
            }
            return false;
        }


        function LoadDocUploader() {

            var randomId = +$("#ContentPlaceHolder1_RandomDocId").val();
            var path = "/TaskManagement/Images/TaskFeedback/";
            var category = "TaskFeedbackDocuments";
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
            return false;
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
            var id = $("#ContentPlaceHolder1_Id").val();
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
    <div>

        <div id="SalesNoteDialog" style="display: none;">
            <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
                clientidmode="static" scrolling="yes"></iframe>
        </div>
        <div id="ShowDocumentDiv" style="display: none;">
            <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
                clientidmode="static" scrolling="yes"></iframe>
        </div>

        <asp:HiddenField runat="server" ID="Id" Value="0" />
        <asp:HiddenField runat="server" ID="hfSelectedEmpId" Value="" />
        <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="tempDocId" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfParentDoc" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />

        <div style="padding: 10px 30px 10px 30px">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Implementation Status</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlImplementationStatus" runat="server" CssClass="form-control">
                            <asp:ListItem Value="Not Started" Text="Not Started"></asp:ListItem>
                            <asp:ListItem Value="In Progress" Text="In Progress"></asp:ListItem>
                            <asp:ListItem Value="Completed" Text="Completed"></asp:ListItem>
                            <asp:ListItem Value="Deferred" Text="Deferred"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Task Done(%)</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTaskStage" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Start Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Start Time</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStartTime" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>

                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Finish Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFinishDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Finish Time</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFinishTime" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                </div>
                <div id="divNonMeeting">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Task Feedback</label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox TextMode="MultiLine" ID="txtTaskFeedback" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div id="divMeeting">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Meeting Agenda</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox TextMode="MultiLine" ID="txtMeetingAgenda" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label required-field">Meeting Location</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox TextMode="MultiLine" ID="txtMeetingLocation" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Meeting Discussion</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox TextMode="MultiLine" ID="txtMeetingDiscussion" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label required-field">Call To Action</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox TextMode="MultiLine" ID="txtCallToAction" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Participant From Client</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlParticipantFromClient" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Participant From Team</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlParticipantFromOffice" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Attachment</label>
                    </div>
                    <div class="col-md-4">
                        <input id="btnImageUp" type="button" onclick="javascript: return LoadDocUploader();"
                            class="TransactionalButton btn btn-primary btn-sm" value="Task Feedback Doc..." />
                    </div>
                </div>
                <div id="DocumentInfo">
                </div>
            </div>
            <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                <div class="col-md-12">
                    <input id="btnSaveFeedback" type="button" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="SaveNClose()" value="Save & Close" />
                    <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return Clear();" />
                    <input id="btnSaveNConvertAsLog" type="button" value="Save Feedback & Convert as Log" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return SaveNCovertAsLog();" />
                </div>
            </div>
        </div>
    </div>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrintDoc" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
</asp:Content>
