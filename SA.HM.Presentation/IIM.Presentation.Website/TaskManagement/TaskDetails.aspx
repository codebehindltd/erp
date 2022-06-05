<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="TaskDetails.aspx.cs" Inherits="HotelManagement.Presentation.Website.TaskManagement.TaskDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">


        var taskId, empId;

        $(document).ready(function () {

            //sid=" + dealStageId + "&cid=" + companyId + "&oid=" + ownerId + "&dname=" + dealName + "&conId=" + contactId + "&dty=" + dateType + "&fd=" + fromDate + "&td=" + toDate;
            if ($.trim(CommonHelper.GetParameterByName("tid")) != "")
                taskId = parseInt($.trim(CommonHelper.GetParameterByName("tid")), 10);
            else
                taskId = 0;

            PerformProjectDetails(taskId);
            empId = parseInt($.trim(CommonHelper.GetParameterByName("eid")), 10);

            $('#ContentPlaceHolder1_txtImpDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                }
            }).datepicker("setDate", DayOpenDate);
            $('#ContentPlaceHolder1_txtImpTime').timepicker({
                showPeriod: is12HourFormat
            });

        });


        function ChangeReminder(element) {

            if ($(element).prop("checked") == true) {
                var id = +$("#ContentPlaceHolder1_ddlAssignTo").val();
                if (id != 0) {
                    //HasEmailAddress(id);
                    $("#dvEmail").show();
                }
                else
                    $("#dvEmail").show();
            }
            else
                $("#dvEmail").hide();
            return false;
        }


        function PerformProjectDetails(id) {

            taskId = id;
            PageMethods.GetTaskId(id, OnSuccessLoading, OnFailLoading);
            return false;
        }

        function OnSuccessLoading(result) {

            FillForm(result);
            return false;
        }

        function Clear() {

            //$("#ContentPlaceHolder1_hfTaskId").val('0');
            $("#ContentPlaceHolder1_txtTaskName").val('');
            $("#ContentPlaceHolder1_ddlTaskType").val('Sales').trigger('change');
            $("#ContentPlaceHolder1_txtEndTime").val('');
            $("#ContentPlaceHolder1_txtCallToAction").val('');
            $("#ContentPlaceHolder1_ddlProjectName").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlSaleNo").val("0").trigger('change');

            $("#ContentPlaceHolder1_txtDueDate").val(DayOpenDate);
            $("#ContentPlaceHolder1_txtDueTime").val('');
            $("#ContentPlaceHolder1_txtEstimatedDoneHour").val("");
            $("#ContentPlaceHolder1_txtDescription").val('');
            $("#DocumentInfo").html("");
            $("#ContentPlaceHolder1_ddlAssignTo").val(null).trigger('change');
            // $("#ContentPlaceHolder1_ddlSearchAssignTo option:selected").removeAttr("selected").trigger('change');
            $("#ContentPlaceHolder1_IsEnableEmailReminder").prop("checked", false);
            ChangeReminder($("#ContentPlaceHolder1_IsEnableEmailReminder"));
            $("#ContentPlaceHolder1_txtEmailReminderDate").val(DayOpenDate);
            $("#ContentPlaceHolder1_txtEmailReminderTime").val('');
            $("#ContentPlaceHolder1_ddlReminderType").val("Once");


            return false;
        }

        function FillForm(Result) {
            Clear();
            //$("#CreateNewDialog").dialog({
            //    autoOpen: true,
            //    modal: true,
            //    width: 900,
            //    closeOnEscape: true,
            //    resizable: false,
            //    height: 'auto',
            //    fluid: true,
            //    title: "Task - " + Result.Task.TaskName,
            //    show: 'slide'
            //});

            $("#CreateNewDialog").show();

            $("#ContentPlaceHolder1_hfTaskId").val(Result.Task.Id);
            $("#ContentPlaceHolder1_txtTaskName").val(Result.Task.TaskName);
            $("#ContentPlaceHolder1_ddlTaskType").val(Result.Task.TaskType);
            if (Result.Task.TaskType == "Project") {
                $("#ContentPlaceHolder1_ddlProjectName").val(Result.Task.SourceNameId).trigger('change');
            }
            else if (Result.Task.TaskType == "Sales") {
                $("#ContentPlaceHolder1_ddlSaleNo").val(Result.Task.SourceNameId).trigger('change');
            }
            if (Result.Task.TaskType == "") 
            $("#ContentPlaceHolder1_ddlTaskStage").val(Result.Task.TaskStage);

            $("#ContentPlaceHolder1_txtDueDate").val(GetStringFromDateTime(Result.Task.TaskDate));
            $("#ContentPlaceHolder1_txtDueTime").val(moment(Result.Task.StartTime).format("h:mm A"));
            $("#ContentPlaceHolder1_txtEstimatedDoneDate").val(GetStringFromDateTime(Result.Task.EstimatedDoneDate));
            $("#ContentPlaceHolder1_txtEstimatedDoneHour").val(Result.Task.EstimatedDoneHour);
            $("#ContentPlaceHolder1_txtEndTime").val(moment(Result.Task.EndTime).format("h:mm A"));
            $("#ContentPlaceHolder1_txtDescription").val(Result.Task.Description);
            $("#ContentPlaceHolder1_ddlAssignTo").val(Result.EmployeeList).trigger('change');

            if (Result.Task.EmailReminderType != "") {

                $("#ContentPlaceHolder1_ddlReminderType").val(Result.Task.EmailReminderType).trigger('change');
                $("#ContentPlaceHolder1_IsEnableEmailReminder").prop("checked", true);
                ChangeReminder($("#ContentPlaceHolder1_IsEnableEmailReminder"));
                if (Result.Task.EmailReminderType == "Once") {
                    $("#ContentPlaceHolder1_txtEmailReminderDate").val(GetStringFromDateTime(Result.Task.EmailReminderDate));
                    $("#ContentPlaceHolder1_txtEmailReminderTime").val(moment(Result.Task.EmailReminderTime).format("h:mm A"));
                }

            }
            else
                $("#ContentPlaceHolder1_IsEnableEmailReminder").prop("checked", false);

            var SupportId = Result.Task.SourceNameId == null ? 0 : Result.Task.SourceNameId;

            ShowUploadedDocument(taskId, SupportId);
            var randomId = $("#ContentPlaceHolder1_RandomDocId").val();
            ShowUploadedDocumentForFeedback(randomId);
            return false;
        }

        function ShowUploadedDocument(id, SupportId) {
            PageMethods.GetUploadedDocByWebMethod(id, SupportId, OnGetUploadedDocByWebMethodSucceeded, OnGetUploadedDocByWebMethodFailed);
            return false;
        }

        function OnGetUploadedDocByWebMethodSucceeded(result) {
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            DocTable = "";
            if (totalDoc > 0) {
                DocTable += "<table id='DocTableList' style='width:100%' class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                DocTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th></tr>";

            }

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    DocTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    DocTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }

                DocTable += "<td align='left' style='width: 50%' onclick= \"ShowDocument('" + result[row].Path + "','" + result[row].Name + "');\">" + result[row].Name + "</td>";

                //if (result[row].Path != "") {
                //    imagePath = "<img src='" + result[row].Path + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                //}
                //else
                //    imagePath = "";

                if (result[row].Path != "") {
                    if (result[row].Extention == ".jpg" || result[row].Extention == ".png")
                        imagePath = "<img src='" + result[row].Path + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    else
                        imagePath = "<img src='" + result[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                DocTable += "<td align='left' style='width: 50%'onclick= \"ShowDocument('" + result[row].Path + "','" + result[row].Name + "');\">" + imagePath + "</td>";


                DocTable += "</tr>";
            }
            DocTable += "</table>";

            docc = DocTable;

            $("#DocumentInfo").html(DocTable);

            return false;
        }

        function OnGetUploadedDocByWebMethodFailed(error) {
            alert(error.get_message());
        }

        function OnFailLoading(error) {
            toastr.error(error.get_message());
            return false;
        }

        function SaveFeedback() {
            var stage = $("#ContentPlaceHolder1_ddlTaskStage").val();
            debugger;
            if (stage == "" || stage == "0") {
                toastr.warning("Select Task Stage");
                $("#ContentPlaceHolder1_ddlTaskStage").focus();
                return false;
            }
            var feedback = $("#ContentPlaceHolder1_txtTaskFeedback").val();
            var status = $("#ContentPlaceHolder1_ddlImpStatus").val();
            if (status == "") {
                isClose = false;
                toastr.warning("Enter Implementation Status");
                $("#ContentPlaceHolder1_ddlImpStatus").focus();
                return false;
            }
            status = status == 0 ? false : true;
            var date = $("#ContentPlaceHolder1_txtImpDate").val();
            var time = $("#ContentPlaceHolder1_txtImpTime").val();
            if (date == "") {
                isClose = false;
                toastr.warning("Enter Implementation Date");
                $("#ContentPlaceHolder1_txtImpDate").focus();
                return false;
            }
            date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
            var time = $("#ContentPlaceHolder1_txtImpTime").val();

            if (time == "") {
                isClose = false;
                toastr.warning("Enter Implementation Time");
                $("#ContentPlaceHolder1_txtImpTime").focus();
                return false;
            }
            time = moment(time, ["h:mm A"]).format("HH:mm");

            if (feedback == "") {
                toastr.warning("Please Add Task Feedback");
                $("#ContentPlaceHolder1_txtTaskFeedback").focus();
                return false;
            }
            var randomDocId = $("#ContentPlaceHolder1_RandomDocId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../TaskManagement/TaskDetails.aspx/SaveTaskFeedback',

                data: JSON.stringify({ stage: stage, feedback: feedback, taskId: taskId, empId: empId, ImpStatus: status, date: date, time: time, randomDocId: parseInt(randomDocId), deletedDoc: deletedDoc }),
                dataType: "json",
                success: function (data) {
                    if (data.d.IsSuccess) {
                        $("#ContentPlaceHolder1_RandomDocId").val(data.d.Data);
                        parent.ShowAlert(data.d.AlertMessage);
                    }
                    parent.CloseTaskDialog();
                    if (typeof parent.ReloadTaskDialog === "function") {
                        parent.ReloadTaskDialog();
                    }
                },
                error: function (result) {

                }
            });
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

        function LoadDocUploader() {

            var randomId = +$("#ContentPlaceHolder1_RandomDocId").val();
            var path = "/TaskManagement/Images/TaskAssign/";
            var category = "TaskFeedbackDocuments";
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
            ShowUploadedDocumentForFeedback(randomId);
        }
        function ShowUploadedDocumentForFeedback(randomId) {
            var id = $("#ContentPlaceHolder1_hfTaskId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            PageMethods.GetUploadedDocForFeedbackByWebMethod(randomId, id, deletedDoc, OnGetUploadedDocForFeedbackByWebMethodSucceeded, OnGetUploadedDocForFeedbackByWebMethodFailed);
            return false;
        }
        function OnGetUploadedDocForFeedbackByWebMethodSucceeded(result) {
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

            $("#DocumentInfoForFeedback").html(DocTable);

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
        function OnGetUploadedDocForFeedbackByWebMethodFailed(error) {
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
    <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />
    <asp:HiddenField ID="tempDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfParentDoc" runat="server"></asp:HiddenField>

    <asp:HiddenField ID="hfTaskId" Value="0" runat="server" />
    <div id="ShowDocumentDiv" style="display: none;">
        <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="CreateNewDialog" style="display: none; overflow: unset;">
        <div class="panel panel-default">
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label ">Task Type</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlTaskType" runat="server" CssClass="form-control" Enabled="false">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group" id="DivProjectName" style="display: none">
                        <div class="col-md-2">
                            <label class="control-label ">Project Name</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlProjectName" runat="server" CssClass="form-control" Enabled="false">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group" id="DivSaleNo" style="display: none">
                        <div class="col-md-2">
                            <label class="control-label ">Sale No</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlSaleNo" runat="server" CssClass="form-control" Enabled="false">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label ">Task Name</label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtTaskName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label ">Task Date</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDueDate" runat="server" CssClass="form-control" TabIndex="1" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label class="control-label ">Estimated Done Date</label>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtEstimatedDoneDate" runat="server" CssClass="form-control" TabIndex="1" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label ">Start Time</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDueTime" runat="server" CssClass="form-control" TabIndex="1" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label class="control-label ">Estimated Done Hour</label>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtEstimatedDoneHour" runat="server" CssClass="quantitydecimal form-control" TabIndex="1" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label ">End Time</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtEndTime" runat="server" CssClass="form-control" TabIndex="1" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblAssignTo" runat="server" class="control-label" Text="Assign To"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlAssignTo" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;" Enabled="false"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Attachment</label>
                        </div>

                    </div>
                    <div id="DocumentInfo">
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Description</label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox TextMode="MultiLine" ID="TextBox1" runat="server" CssClass="form-control" TabIndex="1" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <div style="display: none;">
                        <div class="form-group">
                            <div class="col-md-8">
                                <asp:CheckBox ID="IsEnableEmailReminder" TabIndex="4" runat="Server" Text="Enable email reminder?" Font-Bold="true"
                                    Checked="false" CssClass="mycheckbox" TextAlign="right" onclick="ChangeReminder(this)" Enabled="false" />
                            </div>
                        </div>
                        <div id="dvEmail" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <label class="control-label">Reminder Type</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlReminderType" runat="server" CssClass="form-control" Enabled="false">
                                        <asp:ListItem Value="Once">Once</asp:ListItem>
                                        <asp:ListItem Value="Daily">Daily</asp:ListItem>
                                        <asp:ListItem Value="Weekly">Weekly</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div id="dvReminderDateNTime" class="form-group">
                                <div class="col-md-2">
                                    <label class="control-label">Email Reminder Date</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtEmailReminderDate" runat="server" CssClass="form-control" TabIndex="1" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <label class="control-label">Email Reminder Time</label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtEmailReminderTime" runat="server" CssClass="form-control" TabIndex="1" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <label class="control-label">Call To Action</label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox TextMode="MultiLine" ID="txtCallToAction" runat="server" CssClass="form-control" TabIndex="1" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Implementation Status</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlImpStatus" runat="server" CssClass="form-control">
                                <asp:ListItem Value="">---Please Select---</asp:ListItem>
                                <asp:ListItem Value="0">Pending</asp:ListItem>
                                <asp:ListItem Value="1">Done</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Implementation Date</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtImpDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label required-field">Implementation Time</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtImpTime" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Attachment</label>
                    </div>
                    <div class="col-md-4">
                        <input id="btnImageUp" type="button" onclick="javascript: return LoadDocUploader();"
                            class="TransactionalButton btn btn-primary btn-sm" value="Task Assignment Doc..." />
                    </div>
                </div>
                <div id="DocumentInfoForFeedback">
                </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Task Feedback</label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtTaskFeedback" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" TabIndex="6" Style="resize: none;"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                         <div class="col-md-2">
                            <label class="control-label required-field">Task Stage</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlTaskStage" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                        <div class="col-md-12">
                            <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                                onclick="SaveFeedback()" value="Save Feedback" />
                            <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                                onclick="javascript: return PerformClearAction();" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
</asp:Content>
