<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="SupportCallImplementationIframe.aspx.cs" Inherits="HotelManagement.Presentation.Website.SupportAndTicket.SupportCallImplementationIframe" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var UserInformation = [];
        var supportCallId;
        var taskId;
        var CaseNumber;
        var EmpId = [];
        $(document).ready(function () {
            supportCallId = $.trim(CommonHelper.GetParameterByName("sid"));
            taskId = $.trim(CommonHelper.GetParameterByName("tid"));
            CaseNumber = $.trim(CommonHelper.GetParameterByName("cno"));
            $('#ContentPlaceHolder1_txtEstimatedDoneDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);
            $("#ContentPlaceHolder1_ddlAssignType").change(function () {

                $("#IndividualMailUser tbody").html("");
                $("#GrouplyMailUserGrid tbody").html("");

                if ($(this).val() == "Individual") {
                    $(".GroupAssignContainer").hide();
                    $(".IndividualAssignContainer").show();
                    $(".AllAssignContainer").hide();
                }
                else if ($(this).val() == "Department") {
                    $(".GroupAssignContainer").show();
                    $(".IndividualAssignContainer").hide();
                    $(".AllAssignContainer").hide();
                }
                else if ($(this).val() == "All") {
                    $(".AllAssignContainer").show();
                    $(".IndividualAssignContainer").hide();
                    $(".GroupAssignContainer").hide();
                    GetAllEmployee();
                }
            });
            $("#btnAddUser").click(function () {
                var a = 0;
                if (UserInformation.length == 0) {
                    toastr.warning("Please select an user.");
                    return false;
                }
                $("#IndividualAssignUser tbody tr").each(function () {
                    if (($.trim($(this).find("td:eq(4)").text()) == UserInformation[0].EmpId)
                    ) {
                        $("#txtEmployeeName").focus();
                        toastr.warning("This Employee is already added");
                        a = 1;
                        return false;
                    }
                });
                if (a == 0)
                    AddIndividualEmployee();
                else {

                    var length = UserInformation.length;
                    UserInformation[0] = UserInformation[length - 1];
                }

                return false;
            });

            $("#btnCancelUser").click(function () {
                $("#txtEmployeeName").val("");
                UserInformation = [];
            });
            $("#txtEmployeeName").autocomplete({
                source: function (request, response) {

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../DocumentManagement/AddNewDocumentIFrame.aspx/GetEmployeeInformationAutoSearch',
                        data: "{'searchString':'" + request.term + "'}",
                        dataType: "json",
                        async: false,
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.DisplayName,
                                    value: m.EmpId,
                                    Department: m.Department,
                                    EmpCode: m.EmpCode,
                                    EmpId: m.EmpId,
                                    DisplayName: m.DisplayName,

                                };
                            });
                            response(searchData);
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                },
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field

                    UserInformation[0] = ui.item;
                    $(this).val(ui.item.label);
                }
            });
            $("#ContentPlaceHolder1_ddlEmpDepartment").change(function () {
                $("#ckhAll").prop("checked", false);
                $("#GrouplyAssignUserGrid tbody").html("");
                EmployeeLoadByGroup($(this).val());
            });

            if (taskId != "0") {
                PerformEdit(taskId)
            }
            LoadSupportCall(supportCallId);
            ShowCallCenterUploadedDocument(supportCallId);
            ShowFeedbackUploadedDocument(supportCallId);
        });
        function LoadSupportCall(id) {
            PageMethods.GetSupportById(id, OnSuccessSupportCallLoading, OnFailLoading);

            return false;
        }
        function OnSuccessSupportCallLoading(Result) {
            $("#ContentPlaceHolder1_ddlCaseOwner").val(Result.CaseOwnerId).trigger('change').prop('disabled', true);
            $("#ContentPlaceHolder1_ddlSupportCategory").val(Result.SupportCategoryId).trigger('change').prop('disabled', true);
            $("#ContentPlaceHolder1_ddlSupportSource").val(Result.SupportSource).trigger('change').prop('disabled', true);
            $("#ContentPlaceHolder1_ddlCase").val(Result.CaseId).trigger('change').prop('disabled', true);
            $("#ContentPlaceHolder1_ddlSupportStage").val(Result.SupportStageId).trigger('change').prop('disabled', true);
            $("#ContentPlaceHolder1_txtClientName").val(Result.CompanyNameWithCode).prop('disabled', true);
            $("#ContentPlaceHolder1_txtOtherSource").val(Result.SupportSourceOtherDetails).prop('disabled', true);
            $("#ContentPlaceHolder1_txtCaseDeltails").val(Result.CaseDetails).prop('disabled', true);
            $("#ContentPlaceHolder1_txtItemDetails").val(Result.ItemOrServiceDetails).prop('disabled', true);

            //if (Result.TaskId > 0) {
            //    taskId = $.trim(Result.TaskId);
            //}

            var tr = "";
            for (var i = 0; i < Result.STSupportDetails.length; i++) {

                if (Result.STSupportDetails[i].Type == "Support") {
                    tr += "<tr>";
                    tr += "<td style='width:50%;'>" + Result.STSupportDetails[i].ItemName + "</td>";
                    tr += "<td style='width:30%;'>" + Result.STSupportDetails[i].HeadName + "</td>";
                    tr += "<td style='width:20%;'>";
                    tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                    tr += "</td>";
                    tr += "<td style='display:none;'>" + Result.STSupportDetails[i].ItemId + "</td>";
                    tr += "<td style='display:none;'>" + Result.STSupportDetails[i].CategoryId + "</td>";
                    tr += "<td style='display:none;'>" + Result.STSupportDetails[i].StockBy + "</td>";
                    tr += "<td style='display:none;'>" + Result.STSupportDetails[i].STSupportDetailsId + "</td>";
                    tr += "</tr>";
                }
            }

            $("#SupportItemTbl tbody").prepend(tr);
            $("#ContentPlaceHolder1_txtInternalNotes").val(Result.InternalNotesDetails).prop('disabled', true);

            //if (Result.SupportTypeId != null)
            $("#ContentPlaceHolder1_ddlSupportType").val(Result.SupportTypeId).trigger('change').prop('disabled', true);
            //if (Result.SupportPriorityId != null)
            $("#ContentPlaceHolder1_ddlSupportPriority").val(Result.SupportPriorityId).trigger('change').prop('disabled', true);
            //if (Result.SupportForwardToId != null)
            $("#ContentPlaceHolder1_ddlSupportForwardTo").val(Result.SupportForwardToId).trigger('change').prop('disabled', true);
            //if (Result.BillConfirmation != null && Result.BillConfirmation != "")
            $("#ContentPlaceHolder1_ddlBillConfirmation").val(Result.BillConfirmation).trigger('change').prop('disabled', true);
            //if (Result.SupportDeadline != null)
            $("#ContentPlaceHolder1_txtDeadLineDate").val(GetStringFromDateTime(Result.SupportDeadline)).prop('disabled', true);

            tr = "";
            if (Result.STSupportDetails.length > 0)
                $("#SupportDetailsDiv").show();
            for (var i = 0; i < Result.STSupportDetails.length; i++) {

                if (Result.STSupportDetails[i].Type == "SupportDetails") {
                    tr += "<tr>";
                    tr += "<td style='width:40%;'>" + Result.STSupportDetails[i].ItemName + "</td>";
                    tr += "<td style='width:25%;'>" + Result.STSupportDetails[i].HeadName + "</td>";
                    //tr += "<td style='width:25%;'>" + Result.STSupportDetails[i].UnitPrice + "</td>";
                    tr += "<td style='width:25%;'>" + Result.STSupportDetails[i].UnitQuantity + "</td>";

                    //tr += "<td style='width:25%;'>" +
                    //    "<input type='text' value='" + Result.STSupportDetails[i].UnitPrice + "' id='pp" + Result.STSupportDetails[i].ItemName + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoq(this)'  />" +
                    //    "</td>";

                    //tr += "<td style='width:20%;'>";
                    tr += "</td>";
                    tr += "<td style='display:none;'>" + Result.STSupportDetails[i].ItemId + "</td>";
                    tr += "<td style='display:none;'>" + Result.STSupportDetails[i].CategoryId + "</td>";
                    tr += "<td style='display:none;'>" + Result.STSupportDetails[i].StockBy + "</td>";
                    tr += "<td style='display:none;'>" + Result.STSupportDetails[i].STSupportDetailsId + "</td>";
                    tr += "</tr>";

                    CommonHelper.ApplyDecimalValidation();
                }
            }

            $("#SupportItemTblForSupport tbody").prepend(tr);

            CommonHelper.ApplyDecimalValidation();

            return false;
        }
        function PerformEdit(id) {
            PageMethods.GetTaskId(id, OnSuccessLoading, OnFailLoading)
            return false;
        }

        function OnSuccessLoading(result) {
            FillForm(result);
            return false;
        }
        function FillForm(Result) {
            Clear();
            $("#ContentPlaceHolder1_hfTaskId").val(Result.Task.Id);
            $("#ContentPlaceHolder1_txtEstimatedDoneDate").val(GetStringFromDateTime(Result.Task.EstimatedDoneDate));

            //for (i = 0; i < Result.EmployeeList.length; i++) {
            //    EmpId.push(Result.EmployeeList[i].EmpId);
            //}
            if (Result.Task.AssignType == 'Individual') {
                IndividualTableLoadOnFillForm(Result.EmployeeList)
            }
            else if (Result.Task.AssignType == 'Department') {
                $("#<%=ddlAssignType.ClientID%>").val(Result.Task.AssignType).trigger('change');
                $("#<%=ddlEmpDepartment.ClientID%>").val(Result.Task.EmpDepartment).trigger('change');
                $("#GrouplyAssignUserGrid tbody tr").each(function () {
                    for (i = 0; i < Result.EmployeeList.length; i++) {
                        if (parseFloat($.trim($(this).find("td:eq(4)").text())) == Result.EmployeeList[i].EmpId) {
                            $(this).find("td:eq(0)").find("input").prop("checked", true);
                        }
                    }
                });
            }
            else if (Result.Task.AssignType == 'All') {
                $("#<%=ddlAssignType.ClientID%>").val(Result.Task.AssignType).trigger('change');
                    $("#AllAssignUserGrid tbody tr").each(function () {
                        for (i = 0; i < Result.EmployeeList.length; i++) {
                            if (parseFloat($.trim($(this).find("td:eq(4)").text())) == Result.EmployeeList[i].EmpId) {
                                $(this).find("td:eq(0)").find("input").prop("checked", true);
                            }
                        }
                    });
                }

        $("#btnSave").val('Update & Assign').show();
        $("#btnClear").hide();
        ShowUploadedDocument($("#ContentPlaceHolder1_RandomDocId").val());

        return false;
    }

    function OnFailLoading(error) {
        toastr.error(error.get_message());
        return false;
    }
    function GetAllEmployee() {
        $("#AllAssignUserGrid tbody").html("");
        return $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: '../DocumentManagement/AddNewDocumentIFrame.aspx/LoadAllEmployee',
            dataType: "json",
            async: false,
            success: function (data) {

                var rowLength = data.d.length;

                var tr = "", i = 0;

                for (i = 0; i < rowLength; i++) {

                    if (i % 2 == 0) {
                        tr += "<tr style='background-color:#FFFFFF;'>";
                    }
                    else {
                        tr += "<tr style='background-color:#E3EAEB;'>";
                    }

                    tr += "<td style='width:7%; text-align:center;'>";
                    tr += "<input type='checkbox' value='' />";//0
                    tr += "</td>";
                    tr += "<td style='width:53%;'>" + data.d[i].DisplayName + "</td>";//1
                    tr += "<td style='width:20%;'>" + data.d[i].EmpCode + "</td>";//2
                    tr += "<td style='width:20%;'>" + data.d[i].Department + "</td>";//3

                    tr += "<td style='display:none'>" + data.d[i].EmpId + "</td>";//4
                    tr += "</tr>";

                    $("#AllAssignUserGrid tbody").append(tr);
                    tr = "";
                }

                return false;

            },
            error: function (result) {
                //alert("Error");
            }
        });
    }
    function EmployeeLoadByGroup(groupId) {
        EmployeeLoading(groupId);
        return false;
    }
    function EmployeeLoading(groupId) {
        return $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: '../DocumentManagement/AddNewDocumentIFrame.aspx/LoadEmployeeByGroup',
            data: "{'groupId':'" + groupId + "'}",
            dataType: "json",
            async: false,
            success: function (data) {

                var rowLength = data.d.length;

                var tr = "", i = 0;

                for (i = 0; i < rowLength; i++) {

                    if (i % 2 == 0) {
                        tr += "<tr style='background-color:#FFFFFF;'>";
                    }
                    else {
                        tr += "<tr style='background-color:#E3EAEB;'>";
                    }

                    tr += "<td style='width:7%; text-align:center;'>";
                    tr += "<input type='checkbox' value='' />";//0
                    tr += "</td>";
                    tr += "<td style='width:53%;'>" + data.d[i].DisplayName + "</td>";//1
                    tr += "<td style='width:20%;'>" + data.d[i].EmpCode + "</td>";//2
                    tr += "<td style='width:20%;'>" + data.d[i].Department + "</td>";//3

                    tr += "<td style='display:none'>" + data.d[i].EmpId + "</td>";//4
                    tr += "</tr>";

                    $("#GrouplyAssignUserGrid tbody").append(tr);
                    tr = "";
                }

                return false;

            },
            error: function (result) {
                //alert("Error");
            }
        });
    }
    function AddIndividualEmployee() {

        var rowLength = $("#IndividualAssignUser tbody tr").length;

        var tr = "";

        if (rowLength % 2 == 0) {
            tr += "<tr style='background-color:#FFFFFF;'>";
        }
        else {
            tr += "<tr style='background-color:#E3EAEB;'>";
        }

        tr += "<td style='width:50%;'>" + UserInformation[0].DisplayName + "</td>";
        tr += "<td style='width:20%;'>" + UserInformation[0].EmpCode + "</td>";
        tr += "<td style='width:20%;'>" + UserInformation[0].Department + "</td>";

        tr += "<td style='width:10%;'>";
        tr += "<a href='javascript:void();' onclick= 'javascript:return DeleteUser(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
        tr += "</td>";

        tr += "<td style='display:none'>" + UserInformation[0].EmpId + "</td>";


        tr += "</tr>";

        $("#IndividualAssignUser tbody").append(tr);
        $("#txtEmployeeName").val("");


        UserInformation = [];
    }
    function DeleteUser(deleteUser) {
        $(deleteUser).parent().parent().remove();
    }
    function CheckAllGroupUser(topCheckBox) {
        if ($(topCheckBox).is(":checked") == true) {
            $("#GrouplyAssignUserGrid tbody tr").find("td:eq(0)").find("input").prop("checked", true);
        }
        else {
            $("#GrouplyAssignUserGrid tbody tr").find("td:eq(0)").find("input").prop("checked", false);
        }
    }
    function CheckAllUser(topCheckBox) {
        if ($(topCheckBox).is(":checked") == true) {
            $("#AllAssignUserGrid tbody tr").find("td:eq(0)").find("input").prop("checked", true);
        }
        else {
            $("#AllAssignUserGrid tbody tr").find("td:eq(0)").find("input").prop("checked", false);
        }
    }
    function IndividualTableLoadOnFillForm(data) {
        $("#IndividualAssignUser tbody").html("");
        var rowLength = data.length;

        var tr = "", i = 0;

        for (i = 0; i < rowLength; i++) {
            if (i % 2 == 0) {
                tr += "<tr style='background-color:#FFFFFF;'>";
            }
            else {
                tr += "<tr style='background-color:#E3EAEB;'>";
            }

            tr += "<td style='width:50%;'>" + data[i].DisplayName + "</td>";
            tr += "<td style='width:20%;'>" + data[i].EmpCode + "</td>";
            tr += "<td style='width:20%;'>" + data[i].Department + "</td>";

            tr += "<td style='width:10%;'>";
            tr += "<a href='javascript:void();' onclick= 'javascript:return DeleteUser(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
            tr += "</td>";

            tr += "<td style='display:none'>" + data[i].EmpId + "</td>";


            tr += "</tr>";

            $("#IndividualAssignUser tbody").append(tr);
            tr = "";
        }
    }
    function SaveNClose() {
        isClose = true;
        //SaveOrUpdateTask();
        $.when(SaveOrUpdateTask()).done(function () {
            if (isClose) {

                if (typeof parent.CloseDialog === "function") {
                    parent.CloseDialog();
                }
                if ($("#btnSave").val() == "Update and Close") {
                    $("#btnSave").val("Save And Close");
                    $("#btnSaveNContinue").show();
                    $("#btnClear").show();
                }
            }
        });
        return false;
    }
    function SaveOrUpdateTask() {
        var EmpDepartment = 0;
        var id = +$("#ContentPlaceHolder1_hfTaskId").val();
        var caseName = $("#<%=ddlCase.ClientID %> option:selected").text();
            var taskName = CaseNumber + " - " + caseName;
            var estimatedDoneDate = $("#ContentPlaceHolder1_txtEstimatedDoneDate").val();
            if (estimatedDoneDate == "") {
                isClose = false;
                toastr.warning("Select Estimated Done Date");
                $("#ContentPlaceHolder1_txtEstimatedDoneDate").focus();
                return false;
            }
            else {
                estimatedDoneDate = CommonHelper.DateFormatToMMDDYYYY(estimatedDoneDate, '/');
            }

            var AssignType = $("#<%=ddlAssignType.ClientID%>").val();

            if (AssignType == "Individual") {
                $("#IndividualAssignUser tbody tr").each(function () {
                    EmpId.push($.trim($(this).find("td:eq(4)").text()));
                });
            }
            else if (AssignType == 'Department') {
                EmpDepartment = $("#<%=ddlEmpDepartment.ClientID%>").val();
                $("#GrouplyAssignUserGrid tbody tr").each(function () {
                    if ($(this).find("td:eq(0)").find("input").is(":checked")) {
                        EmpId.push($.trim($(this).find("td:eq(4)").text()));
                    }
                });
            }
            else if (AssignType == 'All') {
                $("#AllAssignUserGrid tbody tr").each(function () {
                    if ($(this).find("td:eq(0)").find("input").is(":checked")) {
                        EmpId.push($.trim($(this).find("td:eq(4)").text()));
                    }
                });
            }
        var EmpList = $('#ContentPlaceHolder1_hfSelectedEmpId').val(EmpId).val();

        if (jQuery.inArray("0", EmpId) > -1 || EmpList == "") {
            isClose = false;
            toastr.warning("Select Employee");
            $("#ContentPlaceHolder1_ddlAssignTo").focus();
            return false;
        }
            //var EmpId = $('#ContentPlaceHolder1_ddlAssignTo').val();
        var EmpList = $('#ContentPlaceHolder1_hfSelectedEmpId').val(EmpId).val();
        if (jQuery.inArray("0", EmpId) > -1 || EmpList == "") {
            isClose = false;
            toastr.warning("Select Assign To");
            $("#ContentPlaceHolder1_ddlAssignTo").focus();
            return false;
        }
        var task = {
            Id: id,
            TaskName: taskName,
            TaskType: "SupportNTicket",
            SourceNameId: parseInt(supportCallId),
            EstimatedDoneDate: estimatedDoneDate,
            AssignType: AssignType,
            EmpDepartment: EmpDepartment
        }
        var randomDocId = $("#ContentPlaceHolder1_RandomDocId").val();
        var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();

        PageMethods.SaveOrUpdateTask(task, EmpList, parseInt(randomDocId), deletedDoc, OnSuccessSaveOrUpdate, OnFailSaveOrUpdate);
        return false;
    }

    function OnSuccessSaveOrUpdate(result) {
        if (result.IsSuccess) {
            if (result.IsSuccess) {
                parent.ShowAlert(result.AlertMessage);
            }
            if (typeof parent.CloseDialog === "function") {
                parent.CloseDialog();
            }
        }

        $("#ContentPlaceHolder1_RandomDocId").val(result.Data);
        Clear();
        projectId = "";
    }

    function OnFailSaveOrUpdate(error) {
        isClose = false;
        toastr.error(error.get_message());
        return false;
    }
    function Clear() {
        $("#AllAssignContainer").hide();
        $("#GroupAssignContainer").hide();
        $("#IndividualAssignUser tbody").html("");
        $("#ContentPlaceHolder1_txtEstimatedDoneDate").val("");
    }

    //Document Related Functions 
    function LoadDocUploader() {

        var randomId = +$("#ContentPlaceHolder1_RandomDocId").val();
        var path = "/TaskManagement/Images/TaskAssign/";
        var category = "TaskAssignDocuments";
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
            DocTable += "<td align='left' style='width: 50%;cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + "','" + result[row].Name + "');\">" + result[row].Name + "</td>";

            if (result[row].Path != "") {

                if (result[row].Extention == ".jpg" || result[row].Extention == ".png")
                    imagePath = "<img src='" + result[row].Path + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                else
                    imagePath = "<img src='" + result[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
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

    function ShowCallCenterUploadedDocument(supportCallId) {
        var id = $("#ContentPlaceHolder1_hfTaskId").val();
        PageMethods.GetCallCenterUploadedDocByWebMethod(id, supportCallId, OnGetCallCenterUploadedDocByWebMethodSucceeded, OnGetCallCenterUploadedDocByWebMethodFailed);
        return false;
    }

    function OnGetCallCenterUploadedDocByWebMethodSucceeded(result) {
        var totalDoc = result.length;
        var row = 0;
        var imagePath = "";
        DocTable = "";
        if (totalDoc > 0) {
            DocTable += "<table id='DocTableListCallCenter' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
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

        $("#DocumentInfo_CallCenter").html(DocTable);

        return false;
    }

    function OnGetCallCenterUploadedDocByWebMethodFailed(error) {
        alert(error.get_message());
    }

    function ShowFeedbackUploadedDocument(supportCallId) {
        var id = $("#ContentPlaceHolder1_hfTaskId").val();
        PageMethods.GetFeedbackUploadedDocByWebMethod(taskId, supportCallId, OnGetFeedbackUploadedDocByWebMethodSucceeded, OnGetFeedbackUploadedDocByWebMethodFailed);
        return false;
    }

    function OnGetFeedbackUploadedDocByWebMethodSucceeded(result) {
        var totalDoc = result.length;
        var row = 0;
        var imagePath = "";
        DocTable = "";
        if (totalDoc > 0) {
            DocTable += "<table id='DocTableListCallCenter' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
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

        $("#DocumentInfo_Feedback").html(DocTable);

        return false;
    }

    function OnGetFeedbackUploadedDocByWebMethodFailed(error) {
        alert(error.get_message());
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
        <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />
        <asp:HiddenField ID="tempDocId" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfParentDoc" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfSelectedEmpId" runat="server" Value="0" />
        <asp:HiddenField ID="hfTaskId" Value="0" runat="server" />
        <asp:HiddenField ID="hfClientId" runat="server" Value="0" />
        <div id="ShowDocumentDiv" style="display: none;">
            <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
                clientidmode="static" scrolling="yes"></iframe>
        </div>
        <div style="padding: 10px 30px 10px 30px">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Ticket Owner</label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList runat="server" ID="ddlCaseOwner" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Client Name</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtClientName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Support Category</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList runat="server" ID="ddlSupportCategory" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Support Source</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList runat="server" ID="ddlSupportSource" CssClass="form-control">
                            <asp:ListItem Text="Phone" Value="Phone"></asp:ListItem>
                            <asp:ListItem Text="Mail" Value="Mail"></asp:ListItem>
                            <asp:ListItem Text="SMS" Value="SMS"></asp:ListItem>
                            <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div id="OtherSourceDiv" style="display: none;">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Other Source</label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtOtherSource" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Case</label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList runat="server" ID="ddlCase" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Case Deltails</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtCaseDeltails" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="panel panel-default" style="display: none;">
                    <div class="panel-heading">Item/Service Information</div>
                    <div style="height: 250px; overflow-y: scroll;">
                        <table id="SupportItemTbl" class="table table-bordered table-condensed table-hover">
                            <thead>
                                <tr>
                                    <th style="width: 50%;">Item</th>
                                    <th style="width: 30%;">Stock Unit</th>
                                    <th style="width: 20%;">Action</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
                <div class="form-group" style="display: none;">
                    <div class="col-md-2">
                        <label class="control-label">Item/Service Details</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtItemDetails" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Support Stage</label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList runat="server" ID="ddlSupportStage" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="SupportDetailsDiv" style="display: none">
                    <div class="panel panel-default">
                        <div class="panel-heading">Item/Service Information</div>
                        <div class="panel-body">
                            <div style="height: 250px; overflow-y: scroll;">
                                <table id="SupportItemTblForSupport" class="table table-bordered table-condensed table-hover">
                                    <thead>
                                        <tr>
                                            <th style="width: 40%;">Item</th>
                                            <th style="width: 25%;">Stock Unit</th>
                                            <%--<th style="width: 25%;">Unit Price</th>--%>
                                            <th style="width: 25%;">Unit Quantity</th>
                                            <%-- <th style="width: 10%;">Action</th>--%>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Internal Notes</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtInternalNotes" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Support Type</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList runat="server" ID="ddlSupportType" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2 ">
                        <label class="control-label required-field">Support Deadline</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtDeadLineDate" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Support Priority</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList runat="server" ID="ddlSupportPriority" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Bill Confirmation</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList runat="server" ID="ddlBillConfirmation" CssClass="form-control">
                            <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
                            <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                            <asp:ListItem Text="TBA" Value="TBA"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" style="display: none;">
                    <div class="col-md-2">
                        <label class="control-label required-field">Support Forward To</label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList runat="server" ID="ddlSupportForwardTo" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Estimated Done Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEstimatedDoneDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblMessageMode" runat="server" class="control-label" Text="Employee Type"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlAssignType" runat="server" CssClass="form-control"
                            TabIndex="1">
                            <asp:ListItem Value="Individual" Text="Individual Employee"></asp:ListItem>
                            <asp:ListItem Value="Department" Text="Employee Department"></asp:ListItem>
                            <asp:ListItem Value="All" Text="All"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="GroupAssignContainer" style="display: none">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Employee Department"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlEmpDepartment" runat="server" CssClass="form-control"
                                TabIndex="2">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="IndividualAssignContainer">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Employee Name"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtEmployeeName" runat="server" TabIndex="5" CssClass="form-control"
                                ClientIDMode="Static"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group" style="padding: 5px 0 5px 0;">
                        <input id="btnAddUser" type="button" value="Add Employee" tabindex="8" class="TransactionalButton btn btn-primary btn-sm" />
                        <input id="btnCancelUser" type="button" value="Cancel Employee" tabindex="9" class="TransactionalButton btn btn-primary btn-sm" />
                    </div>
                </div>
                <div class="IndividualAssignContainer" style="width: 100%;">
                    <div class="form-group">
                        <table id="IndividualAssignUser" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
                            <thead>
                                <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                                    <th style="width: 50%;">Name
                                    </th>
                                    <th style="width: 20%;">Code
                                    </th>
                                    <th style="width: 20%;">Department
                                    </th>
                                    <th style="width: 10%;">Action
                                    </th>
                                    <th style="display: none;">Employee Id
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="GroupAssignContainer" style="width: 100%; height: 300px; overflow-y: scroll; display: none">
                    <div class="form-group">
                        <table id="GrouplyAssignUserGrid" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
                            <thead>
                                <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                                    <th style="width: 7%; text-align: center;">
                                        <input type='checkbox' value='' id="ckhAll" onchange="CheckAllGroupUser(this)" style="padding: 0; margin: 3px;" />
                                    </th>
                                    <th style="width: 53%;">Employee Name
                                    </th>
                                    <th style="width: 20%;">Employee Code
                                    </th>
                                    <th style="width: 20%;">Department
                                    </th>
                                    <th style="display: none;">Employee Id
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="AllAssignContainer" style="width: 100%; height: 300px; overflow-y: scroll; display: none">
                    <div class="form-group">
                        <table id="AllAssignUserGrid" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
                            <thead>
                                <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                                    <th style="width: 7%; text-align: center;">
                                        <input type='checkbox' value='' onchange="CheckAllUser(this)" style="padding: 0; margin: 3px;" />
                                    </th>
                                    <th style="width: 53%;">Employee Name
                                    </th>
                                    <th style="width: 20%;">Employee Code
                                    </th>
                                    <th style="width: 20%;">Department
                                    </th>
                                    <th style="display: none;">Employee Id
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-12">
                        <label class="control-label">Attachment (From Call Center)</label>
                    </div>

                </div>
                <div id="DocumentInfo_CallCenter">
                </div>
                <div class="form-group">
                    <div class="col-md-12">
                        <label class="control-label">Attachment (Feedback)</label>
                    </div>

                </div>
                <div id="DocumentInfo_Feedback">
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
                <div id="DocumentInfo">
                </div>
            </div>
            <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                <div class="col-md-12">
                    <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="SaveNClose()" value="Assign" />
                    <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return Clear();" />
                </div>
            </div>
        </div>
    </div>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
</asp:Content>
