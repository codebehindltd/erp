<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="AssignTask.aspx.cs" Inherits="HotelManagement.Presentation.Website.TaskManagment.AssignTask" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var TaskTable, isClose;
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;
        var DocTable = "";
        $(document).ready(function () {
            CommonHelper.ApplyDecimalValidation();
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            $("#ContentPlaceHolder1_ddlTaskType").change(function () {
                if ($(this).val() == "Sales") {
                    $("#DivSaleNo").show();
                    $("#DivProjectName").hide();
                }
                else if ($(this).val() == "Project") {
                    $("#DivSaleNo").hide();
                    $("#DivProjectName").show();
                }
                else if ($(this).val() == "Internal") {
                    $("#DivSaleNo").hide();
                    $("#DivProjectName").hide();
                }

            });
            $('#ContentPlaceHolder1_txtSearchFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                // defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchToDate').datepicker("option", "minDate", selectedDate);
                }
            }).datepicker();

            $('#ContentPlaceHolder1_txtSearchToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchFromDate').datepicker("option", "maxDate", selectedDate);
                }
            }).datepicker();
            $('#ContentPlaceHolder1_txtDueDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);
            $('#ContentPlaceHolder1_txtEstimatedDoneDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);
            $('#ContentPlaceHolder1_txtDueTime').timepicker({
                showPeriod: is12HourFormat
            });
            $('#ContentPlaceHolder1_txtEndTime').timepicker({
                showPeriod: is12HourFormat
            });
            $('#ContentPlaceHolder1_txtEmailReminderDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);
            $('#ContentPlaceHolder1_txtEmailReminderTime').timepicker({
                showPeriod: is12HourFormat
            });
            $("#ContentPlaceHolder1_ddlAssignTo").select2({
                dropdownParent: $("#CreateNewDialog")
            });
            $("#ContentPlaceHolder1_ddlProjectName").select2({
                tags: false,
                allowClear: true,
                width: "99.75%",
                dropdownParent: $("#CreateNewDialog")
            });
            $("#ContentPlaceHolder1_ddlSaleNo").select2({
                tags: false,
                allowClear: true,
                width: "99.75%",
                dropdownParent: $("#CreateNewDialog")
            });
            $("#ContentPlaceHolder1_ddlSearchAssignTo").select2();
            $("#ContentPlaceHolder1_ddlAssignTo").change(function () {

                //var len = $('#ContentPlaceHolder1_ddlAssignTo option:selected').length;
                //if (len > 1) {
                //    var list = $('#ContentPlaceHolder1_ddlAssignTo').val();
                //    if (jQuery.inArray("0", list) > -1) {
                //        $("#ContentPlaceHolder1_ddlAssignTo").val("0");
                //    }
                //}
                ChangeReminder($("#ContentPlaceHolder1_IsEnableEmailReminder"));
            });
            TaskTable = $("#tblTask").DataTable({
                data: [],
                columns: [
                    { title: "Task Name", "data": "TaskName", sWidth: '8%' },
                    { title: "Type", "data": "TaskType", sWidth: '8%' },
                    { title: "Company", "data": "CompanyName", sWidth: '8%' },
                    { title: "Contact", "data": "ContactName", sWidth: '8%' },
                    { title: "Deal", "data": "DealName", sWidth: '8%' },
                    { title: "Perticipent From Client", "data": "PerticipentFromClient", sWidth: '10%' },
                    { title: "Assigned By", "data": "AccountManagerName", sWidth: '8%' },
                    { title: "Assigned To", "data": "EmployeeNameList", sWidth: '10%' },
                    { title: "Priority", "data": "TaskPriority", sWidth: '8%' },
                    { title: "Due Date & Time", "data": "DueDateTime", sWidth: '8%' },
                    { title: "TaskStatus", "data": "TaskStatus", sWidth: '8%' },
                    { title: "Action", "data": null, sWidth: '8%' },
                    { title: "", "data": "Id", visible: false }
                ],
                columnDefs: [
                    //{
                    //    "targets": 6,
                    //    "className": "text-center",
                    //    "render": function (data, type, full, meta) {
                    //        var status;
                    //        if (data == true)
                    //            img = 'Done';
                    //        else
                    //            img = 'Pending';
                    //        return img;
                    //    }
                    //},                    
                    {
                        "targets": 8,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            var status;
                            if (data == 1)
                                img = 'High';
                            else if (data == 2)
                                img = 'Medium';
                            else if (data == 3)
                                img = 'Low';
                            return img;
                        }
                    }],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var row = '';
                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }
                    if (IsCanEdit && !aData.IsCompleted)
                        row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformEdit('" + aData.Id + "','" + aData.TaskName + "');\"> <img alt=\"Edit\" src=\"/Images/edit.png\" title='Edit' /> </a>";
                    if (IsCanDelete && !aData.IsCompleted)
                        row += "&nbsp;&nbsp;<a href='javascript:void();' onclick= \"DeleteTask('" + aData.Id + "');\"> <img alt='Delete' src='/Images/delete.png' title='Delete' /></a>";
                    //if (!aData.IsCompleted)
                    //    row += "&nbsp;&nbsp;<a href='javascript:void();' onclick= \"UpdateTaskIsCompleted('" + aData.Id + "',true);\"> <img alt='Mark As Complete' src='../Images/completerequisition.png' title='Mark As Complete' /></a>";
                    //else
                    //    row += "&nbsp;&nbsp;<a href='javascript:void();' onclick= \"UpdateTaskIsCompleted('" + aData.Id + "',false);\"> <img alt='Mark As InComplete' src='../Images/approved.png' title='Mark As InComplete' /></a>";

                    row += "&nbsp;&nbsp;<a href='javascript:void();' onclick= \"ShowDocument('" + aData.Id + "',false);\"> <img alt='Document' src='/Images/document.png' title='document' /></a>";
                    row += "&nbsp;&nbsp;<a href='javascript:void();' onclick= \"TaskFeedback('" + aData.Id + "','" + aData.TaskName + "','" + aData.TaskFor + "','" + aData.TaskType + "','" + aData.EmpId + "');\"> <img alt='Task Feedback' src='/Images/detailsInfo.png' title='Task Feedback' /></a>";

                    //row += "&nbsp;&nbsp;<a href='#' onclick= \"GetDiscountDetails('" + aData.Id + "');\"> <img alt='Delete' src='../Images/detailsInfo.png' /></a>";

                    $('td:eq(' + (nRow.children.length - 1) + ')', nRow).html(row);
                },
                pageLength: UserInfoFromDB.GridViewPageSize,
                filter: false,
                info: false,
                ordering: false,
                processing: true,
                retrieve: true,
                bAutoWidth: false,
                bLengthChange: false,
                bInfo: false,
                bPaginate: false,
                language: {
                    emptyTable: "No Data Found"
                },

            });


            $("#ContentPlaceHolder1_ddlReminderType").change(function () {

                if ($(this).val() == "Once")
                    $("#dvReminderDateNTime").show();
                else
                    $("#dvReminderDateNTime").hide();
            });

            //$("#ContentPlaceHolder1_ddlAssignTo").change(function () {
            //    ChangeReminder($("#ContentPlaceHolder1_IsEnableEmailReminder"));
            //});
            $("#ContentPlaceHolder1_ddlFilterBy").change(function () {
                if ($(this).val() == "Custom" || $(this).val() == "Overdue" || $(this).val() == "Completed")
                    $("#dvSearchDateTime").show();
                else
                    $("#dvSearchDateTime").hide();
            });
            $("#btnClear").show();
            //SearchTask(1, 1);
        });
        function TaskFeedback(taskId, taskName, taskFor, taskType, empId) {
            var iframeid = 'frmTask';
            if (taskFor == 'CRM') {
                var url = "../TaskManagement/TaskFeedbackForCRMIframe.aspx?tid=" + taskId + "&eid=" + empId + "&tType=" + taskType + "&tFor=" + taskFor;
            }
            else {
                var url = "../TaskManagement/TaskDetails.aspx?tid=" + taskId + "&eid=" + empId;
            }
            document.getElementById(iframeid).src = url;

            $("#TaskDialogue").dialog({
                autoOpen: true,
                modal: true,
                width: "95%",
                height: 800,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: taskName,
                show: 'slide'
            });

            return false;
        }
        function CloseTaskDialog() {
            $("#TaskDialogue").dialog('close');
            return false;
        }

        function ShowDocument(id) {
            PageMethods.LoadTaskDocument(id, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }
        function OnLoadDocumentSucceeded(result) {
            $("#imageDiv").html(result);

            $("#taskDocuments").dialog({
                autoOpen: true,
                modal: true,
                width: "70%",
                height: 300,
                closeOnEscape: true,
                resizable: false,
                title: "Task Assign Documents",
                show: 'slide'
            });

            return false;
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }
        function PerformEdit(Id, name) {
            if (!confirm("Want to edit - " + name + "?")) {
                return false;
            }
            var iframeid = 'frmPrint';
            var url = "./AssignTaskIFrame.aspx?tid=" + Id;
            document.getElementById(iframeid).src = url;
            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "90%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Task Assign",
                show: 'slide'
            });
        }
        function CloseDialog() {
            $("#SalesNoteDialog").dialog('close');
            return false;
        }
        function CreateNew() {
            var iframeid = 'frmPrint';
            var url = "./AssignTaskIFrame.aspx?tid=" + "";
            document.getElementById(iframeid).src = url;
            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "90%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                title: "Task Assign",
                show: 'slide'
            });
            return false;
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchTask(pageNumber, IsCurrentOrPreviousPage);
        }
        function SearchTask(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = TaskTable.data().length;

            var taskName = $("#ContentPlaceHolder1_txtTaskNameForSearch").val();
            var fromDate = $("#ContentPlaceHolder1_txtSearchFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtSearchToDate").val();
            if (fromDate != "")
                fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');
            //fromDate = new Date();
            if (toDate != "")
                toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');
            // toDate = new Date();
            //if (filterBy == "Custom" || filterBy == "Overdue" || filterBy == "Completed") {



            var assignToId = $("#ContentPlaceHolder1_ddlSearchAssignTo").val();
            var assignToId = $("#ContentPlaceHolder1_hfSelectedEmpIdForSearch").val(assignToId).val();

            PageMethods.GetTaskListBySearchCriteria(taskName, fromDate, toDate, assignToId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnTaskLoadingSucceed, OnTaskLoadingFailed);
            return false;
        }

        function OnTaskLoadingSucceed(result) {

            TaskTable.clear();
            TaskTable.rows.add(result.GridData);
            TaskTable.draw();

            $("#GridPagingContainer ul").html("");
            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
            return false;
        }

        function OnTaskLoadingFailed(error) {
            toastr.error(error.get_message());
            return false;
        }


        //function PerformEdit(id) {
        //    PageMethods.GetTaskId(id, OnSuccessLoading, OnFailLoading)
        //    return false;
        //}


        function UpdateTaskIsCompleted(id, isCompleted) {
            if (confirm("Are you Sure?")) {
                PageMethods.UpdateTaskIsCompleted(id, isCompleted, OnSuccessUpdate, OnFailedUpdate);
            }

            return false;
        }

        function OnSuccessUpdate(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                GridPaging(1, 1);
            }
        }

        function OnFailedUpdate(error) {
            toastr.error(error.get_message());
            return false;
        }
        function DeleteTask(id) {
            if (confirm("Want to delete?")) {
                PageMethods.DeleteTask(id, OnSuccessDelete, OnFailedDelete);
            }
            return false;
        }

        function OnSuccessDelete(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                GridPaging(1, 1);
            }
            return false;
        }

        function OnFailedDelete(error) {
            toastr.error(error.get_message());
            return false;
        }

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

        function HasEmailAddress(id) {

            PageMethods.HasEmailAddress(id, OnSuccessChecking, OnFailChecking);
            return false;
        }

        function OnSuccessChecking(result) {

            if (result)
                $("#dvEmail").show();
            else {
                $("#dvEmail").hide();
                $("#ContentPlaceHolder1_IsEnableEmailReminder").prop("checked", false);
                toastr.warning("Assign To has no email. Please update email address.");
                return false;
            }
            return false;
        }

        function OnFailChecking(error) {
            toastr.error(error.get_message());
            return false;
        }

        function SaveNClose() {
            isClose = true;
            SaveOrUpdateTask();

            return false;
        }

        function ShowUploadedDocument(randomId) {
            var id = $("#ContentPlaceHolder1_hfTaskId").val();
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

        function LoadTaskManagement() {
            //var taskName = $("#ContentPlaceHolder1_txtTaskNameForSearch").val();
            //var fromDate = $("#ContentPlaceHolder1_txtSearchFromDate").val();
            //var toDate = $("#ContentPlaceHolder1_txtSearchToDate").val();
            //if (fromDate == "")
            //    fromDate = new Date();
            //if (toDate == "")
            //    toDate = new Date();

            //fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');
            //toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            //var assignToId = $("#ContentPlaceHolder1_ddlSearchAssignTo").val();
            //var assignToId = $("#ContentPlaceHolder1_hfSelectedEmpIdForSearch").val(assignToId).val();

            //window.location.href = "./TaskManagement.aspx?&taskname=" + taskName + "&frmDate=" + fromDate + "&toDate=" + toDate + "&asigned=" + assignToId ;
            window.location.href = "./TaskManagement.aspx";
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }
    </script>

    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />

    <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="tempDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfParentDoc" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfSelectedEmpId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSelectedEmpIdForSearch" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />
    <div id="taskDocuments" style="display: none;">
        <div id="imageDiv"></div>
    </div>
    <div id="SalesNoteDialog" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>

    <div id="TaskDialogue" style="display: none;">
        <iframe id="frmTask" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes" style="height: 620px;"></iframe>
    </div>

    <div class="panel panel-default">
        <div class="panel-heading">
            Task Assignment
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label ">Task Name</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtTaskNameForSearch" runat="server" CssClass="form-control">                            
                        </asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Assign To</label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlSearchAssignTo" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;"></asp:DropDownList>
                    </div>
                </div>
                <div id="dvSearchDateTime" class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">From Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchFromDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">To Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchToDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return SearchTask(1,1);" />
                        <asp:Button ID="btnCreateNew" runat="server" Text="Create New" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateNew();" />
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-heading">
            Task
            <a style="float: right; padding: 0px;" href='javascript:void();' onclick='javascript:return LoadTaskManagement()' title='Task Management'>
                <img style='width: 22px; height: 20px;' alt='Search Quotation' src='/Images/management.png' /></a>

        </div>
        <div class="panel-body">
            <table id="tblTask" class="table table-bordered table-condensed table-responsive">
            </table>
            <div class="childDivSection">
                <div class="text-center" id="GridPagingContainer">
                    <ul class="pagination">
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfTaskId" Value="0" runat="server" />
    <div id="popUpImage" style="display: none">
        <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
            <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
        </asp:Panel>
    </div>
</asp:Content>
