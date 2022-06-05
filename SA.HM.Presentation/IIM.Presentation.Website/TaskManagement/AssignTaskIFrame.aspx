<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="AssignTaskIFrame.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.AssignTaskIFrame" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var parentTaskIdSelect = 0;
        var dependentTaskIdSelect = 0;
        var TaskTable, isClose;
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;
        var DocTable = "";
        var taskMinDate = "";
        var taskMaxDate = "";
        var projectId;
        var UserInformation = [];
        var taskId;
        var logId, companyId, contactId, dealId, company, contact, deal;
        $(document).ready(function () {
            projectId = $.trim(CommonHelper.GetParameterByName("pid"));
            taskId = $.trim(CommonHelper.GetParameterByName("tid"));
            logId = $.trim(CommonHelper.GetParameterByName("lid"));
            companyId = $.trim(CommonHelper.GetParameterByName("cid"));
            contactId = $.trim(CommonHelper.GetParameterByName("cntid"));
            dealId = $.trim(CommonHelper.GetParameterByName("did"));
            company = $.trim(CommonHelper.GetParameterByName("cpny"));
            contact = $.trim(CommonHelper.GetParameterByName("cntct"));
            deal = $.trim(CommonHelper.GetParameterByName("dl"));
            var taskType = $.trim(CommonHelper.GetParameterByName("tType"));
            CommonHelper.ApplyDecimalValidation();
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            $("#ContentPlaceHolder1_ddlTaskFor").change(function () {
                if ($(this).val() == "PreSales") {
                    $("#DivSaleNo").show();
                    $("#DivProjectName").hide();
                    $("#DivParentTask").hide();
                    $("#DivBillNo").hide();
                    $("#divProject_Sale_Bill_ParentTask").show();
                    $("#divAssignToTableDesign").show();
                    $("#divMultipleAssignTo").hide();
                    $("#divForLog").hide();
                }
                else if ($(this).val() == "Project") {
                    $("#DivSaleNo").hide();
                    $("#DivProjectName").show();
                    $("#DivParentTask").show();
                    $("#DivBillNo").hide();
                    $("#divProject_Sale_Bill_ParentTask").show();
                    $("#divAssignToTableDesign").show();
                    $("#divMultipleAssignTo").hide();
                    $("#divForLog").hide();
                }
                else if ($(this).val() == "Internal") {
                    $("#DivSaleNo").hide();
                    $("#DivProjectName").hide();
                    $("#DivParentTask").hide();
                    $("#DivBillNo").hide();
                    $("#divProject_Sale_Bill_ParentTask").show();
                    $("#divAssignToTableDesign").show();
                    $("#divMultipleAssignTo").hide();
                    $("#divForLog").hide();
                }
                else if ($(this).val() == "Billing") {
                    $("#DivSaleNo").hide();
                    $("#DivProjectName").hide();
                    $("#DivParentTask").show();
                    $("#DivBillNo").show();
                    $("#divProject_Sale_Bill_ParentTask").show();
                    $("#divAssignToTableDesign").show();
                    $("#divMultipleAssignTo").hide();
                    $("#divForLog").hide();
                } else if ($(this).val() == "CRM") {
                    $("#divProject_Sale_Bill_ParentTask").hide();
                    $("#divAssignToTableDesign").hide();
                    $("#divMultipleAssignTo").show();
                    $("#divForLog").show();
                } else {
                    $("#DivSaleNo").hide();
                    $("#DivProjectName").hide();
                    $("#DivParentTask").hide();
                    $("#DivBillNo").hide();
                    $("#divProject_Sale_Bill_ParentTask").show();
                    $("#divAssignToTableDesign").show();
                    $("#divMultipleAssignTo").hide();
                    $("#divForLog").hide();
                }

            });
            $("#ContentPlaceHolder1_ddlTaskBy").change(function () {
                if ($(this).val() == "Company Wise") {
                    $("#company").show();
                    $("#contact").hide();
                    $("#ContentPlaceHolder1_lblCompany").text("Company");
                }
                else if ($(this).val() == "Contact Wise") {
                    $("#company").hide();
                    $("#contact").show();
                    $("#ContentPlaceHolder1_lblCompany").text("Contact");
                }

            });

            $("#ContentPlaceHolder1_txtBillNo").autocomplete({
                source: function (request, response) {

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../TaskManagement/AssignTaskIFrame.aspx/GetBillNoByText',
                        //data: "{'searchTerm':'" + request.term + "'}",
                        data: JSON.stringify({ searchTerm: request.term }),
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.BillNumber,
                                    value: m.BillId
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
                    //debugger;
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfBillId").val(ui.item.value);
                    PageMethods.LoadTaskByTaskTypeNTaskId('Billing', ui.item.value, OnTaskLoadingSucceed, OnTaskLoadingFailed);
                    return false;
                }
            });

            $("#ContentPlaceHolder1_ddlProjectName").change(function () {
                var projectId = $(this).val();
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../TaskManagement/AssignTaskIFrame.aspx/LoadTaskByTaskTypeNTaskId',

                    data: JSON.stringify({ taskType: 'Project', taskId: projectId }),
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        OnTaskLoadingSucceed(data.d);
                    },
                    error: function (result) {
                        OnTaskLoadingFailed(result.d);
                    }
                });
                //PageMethods.LoadTaskByTaskTypeNTaskId('Project', projectId, OnTaskLoadingSucceed, OnTaskLoadingFailed);
                return false;
            });

            $("#ContentPlaceHolder1_ddlParentTaskName").change(function () {
                var parentTaskId = $("#ContentPlaceHolder1_ddlParentTaskName").val() == null ? 0 : $("#ContentPlaceHolder1_ddlParentTaskName").val();
                PageMethods.GetTaskId(parentTaskId, OnSuccessParentTaskLoading, OnFailParentTaskLoading);
                //PageMethods.LoadTaskByProjectId(projectId, OnTaskLoadingSucceed, OnTaskLoadingFailed);
                return false;
            });

            $('#ContentPlaceHolder1_txtDueDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtEstimatedDoneDate").datepicker("option", "minDate", selectedDate);

                    var strDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtDueDate").val(), '/');
                    minEndDate = GetStringFromDateTime(CommonHelper.DaysAdd(strDate, 0));

                    $("#ContentPlaceHolder1_txtEstimatedDoneDate").datepicker("option", {
                        minDate: minEndDate
                    });
                }
            }).datepicker("setDate", DayOpenDate);
            $('#ContentPlaceHolder1_txtEstimatedDoneDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtDueDate").datepicker("option", "maxDate", selectedDate);

                }
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

            $('#ContentPlaceHolder1_txtReminderFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtReminderToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);
            $('#ContentPlaceHolder1_txtEmailReminderTime').timepicker({
                showPeriod: is12HourFormat
            });
            $("#ContentPlaceHolder1_ddlAssignTo").select2({
            });
            $("#ContentPlaceHolder1_ddlAssignDepartment").select2({
            });
            $("#ContentPlaceHolder1_ddltxtParticipantFromClient").select2({
            });
            $("#ContentPlaceHolder1_ddlProjectName").select2({
                tags: false,
                allowClear: true,
                width: "99.75%",
            });
            $("#ContentPlaceHolder1_ddlSaleNo").select2({
                tags: false,
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlParentTaskName").select2({
                tags: false,
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlDependentTaskName").select2({
                tags: false,
                allowClear: true,
                width: "99.75%"
            });

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

            if (taskId != "" && projectId != "") {
                AddNewTaskUnderTaskId(taskId, projectId);
            }
            else if (taskId != "" && projectId == "") {
                if (taskType == "Internal") {
                    LoadRepairNMaintenance(taskId);
                }
                else
                    PerformEdit(taskId);
            }
            else if (projectId != "") {

                LoadProject(projectId);
            }
            else {
                //Clear();
            }
            if (logId != "" && logId != "0") {
                var Client = new Array();
                $("#divProject_Sale_Bill_ParentTask").hide();
                $("#divAssignToTableDesign").hide();
                $("#divMultipleAssignTo").show();
                $("#divForLog").show();
                $("#ContentPlaceHolder1_ddlTaskFor").val('CRM').prop("disabled", true);;
                if (companyId != '0' && companyId != '') {
                    $("#ContentPlaceHolder1_hfCompanyId").val(companyId);
                    $("#ContentPlaceHolder1_ddlCompany").val(company).prop("disabled", true);
                    $("#ContentPlaceHolder1_ddlTaskBy").val("Company Wise").trigger("change").prop("disabled", true);
                }
                else if (contactId != '0' && contactId != '') {
                    $("#ContentPlaceHolder1_hfContactId").val(contactId);
                    $("#ContentPlaceHolder1_ddlContact").val(contact).prop("disabled", true);
                    $("#ContentPlaceHolder1_ddlTaskBy").val("Contact Wise").trigger("change").prop("disabled", true);
                    Client.push(contactId);
                    $("#ContentPlaceHolder1_ddltxtParticipantFromClient").val(Client).trigger('change');
                }
                if (dealId != '0' && dealId != '') {
                    $("#ContentPlaceHolder1_hfDealId").val(dealId);
                    $("#ContentPlaceHolder1_ddlDeal").val(deal).prop("disabled", true);
                }
                $("#ContentPlaceHolder1_ddlTaskStage").val("1");
                $("#ContentPlaceHolder1_ddlTaskType").val("To do").trigger('change');
                if (taskId == "" || taskId == "0") {
                    $("#<%=txtTaskName.ClientID%>").val("Follow Up");
                }
                if (contact == "") {
                    $("#contact").hide();
                }
                if (company == "") {
                    $("#company").hide();
                    if (contact == "") {
                        $("#lblCompanyContact").hide();
                    }
                }
                if (deal == "") {
                    $("#deal").hide();
                }

            }
            $("#ContentPlaceHolder1_ddlAssignType").change(function () {
                if (logId != "" && logId != "0") {
                    if ($(this).val() == "Individual") {
                        $("#divMultipleAssignTo").show();
                        $("#divAssignDepartment").hide();
                        $("#divAssignEmployee").show();
                    } else if ($(this).val() == "Department") {
                        $("#divMultipleAssignTo").show();
                        $("#divAssignDepartment").show();
                        $("#divAssignEmployee").hide();
                    }
                    else {
                        $("#divMultipleAssignTo").hide();
                    }
                }
                else {
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
                }
            });
            $("#ContentPlaceHolder1_ddlReminder").change(function () {
                if ($(this).val() == 'custom') {
                    $("#divReminderDateTime").show();
                }
                else {
                    $("#divReminderDateTime").hide();
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
            //$("#ContentPlaceHolder1_ddlTaskFor").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlCompany").autocomplete({
                source: function (request, response) {

                    var accountManager = $("#ContentPlaceHolder1_ddlAccountManager").val();
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../TaskManagement/AssignTaskIFrame.aspx/GetCompanyByAutoSearchAndAccountManagerId',
                        data: JSON.stringify({ searchTerm: request.term, accountManagerId: accountManager }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.CompanyName,
                                    value: m.CompanyId
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
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfCompanyId").val(ui.item.value);
                    LoadContactByCompanyId(ui.item.value);
                }
            });
            $("#ContentPlaceHolder1_ddlContact").autocomplete({
                source: function (request, response) {
                    //debugger;
                    var accountManager = $("#ContentPlaceHolder1_ddlAccountManager").val();
                    var company = "0";
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/SalesCallEntry.aspx/GetContactByAccountManagerNCompany',
                        //data: "{'searchTerm':'" + request.term + "'}",
                        data: JSON.stringify({ searchTerm: request.term, accountManagerId: accountManager, companyId: company }),
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.Id,
                                    WorkCountry: m.WorkCountry,
                                    WorkState: m.WorkState,
                                    WorkCity: m.WorkCity,
                                    WorkLocation: m.WorkLocation,
                                    MobileWork: m.MobileWork
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
                    // manually update the textbox pand hidden field
                    $("#txtContactDetails").text('');
                    $(this).val(ui.item.label);
                    $("#ContactDetailsDiv").show();
                    $("#txtContactDetails").text('Country: ' + ui.item.WorkCountry + '; State: ' + ui.item.WorkState + '; State: ' + ui.item.WorkCity + ' (' + ui.item.MobileWork + ')');
                    $("#ContentPlaceHolder1_hfContactId").val(ui.item.value);
                }
            });
            $("#ContentPlaceHolder1_ddlDeal").autocomplete({
                source: function (request, response) {
                    //debugger;
                    var accountManager = $("#ContentPlaceHolder1_ddlAccountManager").val();
                    var company = $("#ContentPlaceHolder1_hfCompanyId").val();
                    var contact = $("#ContentPlaceHolder1_hfContactId").val();
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/SalesCallEntry.aspx/GetDealByCompanyIdContactIdNAccountManager',
                        data: JSON.stringify({ searchText: request.term, contactId: contact, companyId: company, accountManagerId: accountManager }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.Id
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
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfDealId").val(ui.item.value);
                }
            });

        });
        function LoadContactByCompanyId(Id) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../SalesAndMarketing/SalesCallEntry.aspx/GetContactInformationByCompanyId",
                dataType: "json",
                data: JSON.stringify({ id: Id }),
                async: false,
                success: (data) => {
                    OnLoadContactSucceed(data.d);
                },
                error: (error) => {
                    toastr.error(error, "", { timeOut: 5000 });
                }
            });
            //PageMethods.GetContactInformationByCompanyId(Id, OnLoadContactSucceed, OnLoadContactFailed);
            return false;
        }
        function OnLoadContactSucceed(result) {
            var list = result;
            var CallContacts = '<%=ddltxtParticipantFromClient.ClientID%>';
            
            var control = $('#' + CallContacts);
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.removeAttr("disabled");

                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].Id + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    <%--control.empty().append('<option value="-1">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    control2.empty().append('<option value="-1">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');--%>
                }
            }
            else {
                control.empty().append('<option value="-1">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
            }
            return false;

        }
        function OnLoadContactFailed(error) {
            toastr.error(error);
            return false;
        }

        function AddNewTaskUnderTaskId(taskId, ProjectId) {

            //Clear();

            $("#ContentPlaceHolder1_ddlTaskFor").val('Project').trigger('change').prop("disabled", true);;
            $("#ContentPlaceHolder1_ddlProjectName").val(ProjectId).trigger('change').prop("disabled", true);
            $("#ContentPlaceHolder1_ddlParentTaskName").val(taskId).trigger('change').prop("disabled", true);;

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
        function GetEmpByDepartment(id) {
            debugger;
            var empArray = [];
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../DocumentManagement/AddNewDocumentIFrame.aspx/LoadEmployeeByGroup',
                data: "{'groupId':'" + id + "'}",
                dataType: "json",
                async: false,
                success: function (data) {

                    var rowLength = data.d.length;

                    var tr = "", i = 0;

                    for (i = 0; i < rowLength; i++) {
                        debugger;
                        empArray.push(data.d[i].EmpId);
                    }
                }
            });
            return empArray;
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

        function OnTaskLoadingSucceed(result) {
            typesList = [];
            $("#ContentPlaceHolder1_ddlParentTaskName").empty();
            $("#ContentPlaceHolder1_ddlDependentTaskName").empty();

            var i = 0, fieldLength = result.length;

            if (fieldLength > 0) {
                typesList = result;
                $('<option value="' + 0 + '">' + '--- Please Select ---' + '</option>').appendTo('#ContentPlaceHolder1_ddlParentTaskName');
                $('<option value="' + 0 + '">' + '--- Please Select ---' + '</option>').appendTo('#ContentPlaceHolder1_ddlDependentTaskName');
                for (i = 0; i < fieldLength; i++) {
                    $('<option value="' + result[i].Id + '">' + result[i].TaskName + '</option>').appendTo('#ContentPlaceHolder1_ddlParentTaskName');
                    $('<option value="' + result[i].Id + '">' + result[i].TaskName + '</option>').appendTo('#ContentPlaceHolder1_ddlDependentTaskName');
                }

                $("#ContentPlaceHolder1_ddlParentTaskName").val(parentTaskIdSelect).trigger('change');
                $("#ContentPlaceHolder1_ddlDependentTaskName").val(dependentTaskIdSelect);
                parentTaskIdSelect = 0;
                dependentTaskIdSelect = 0;
            }
            else {
                $("<option value='0'>--No Tasks Found--</option>").appendTo("#ContentPlaceHolder1_ddlParentTaskName");
                $("<option value='0'>--No Tasks Found--</option>").appendTo("#ContentPlaceHolder1_ddlDependentTaskName");

            }
            return false;
        }
        function OnTaskLoadingFailed(error) {
            toastr.error(error.get_message());
            return false;
        }

        function CalculateHour(startDate, startTime, endDate, endTime) {

            //var hour = CommonHelper.CalculateHourByDateTime(startDate, startTime, endDate, endTime);
            //alert(hour);
            if (startDate == "" || startTime == "" || endDate == "" || endTime == "") {
                return false;
            }
            var start = new Date(startDate + ' ' + startTime);
            var end = new Date(endDate + ' ' + endTime);
            var difference = end - start;
            var hour = difference / 3600000;
            return hour;
        }
        function SaveOrUpdateTask() {
            var EmpDepartment = 0;
            var taskType, taskFor;
            var id = +$("#ContentPlaceHolder1_hfTaskId").val();
            taskFor = $("#ContentPlaceHolder1_ddlTaskFor").val();
            if (taskFor == "0") {
                isClose = false;
                toastr.warning("Enter Task For");
                $("#ContentPlaceHolder1_ddlTaskFor").focus();
                return false;
            }
            var taskName = $("#ContentPlaceHolder1_txtTaskName").val();
            if (taskName == "") {
                isClose = false;
                toastr.warning("Enter Task Name");
                $("#ContentPlaceHolder1_txtTaskName").focus();
                return false;
            }
            var taskType = $("#ContentPlaceHolder1_ddlTaskType").val();
            var taskDate = $("#ContentPlaceHolder1_txtDueDate").val();

            if (taskDate == "") {
                isClose = false;
                toastr.warning("Enter Task Date");
                $("#ContentPlaceHolder1_txtDueDate").focus();
                return false;
            }
            taskDate = CommonHelper.DateFormatToMMDDYYYY(taskDate, '/');
            //var startTime = $("#ContentPlaceHolder1_txtDueTime").val();

            //if (startTime == "") {
            //    isClose = false;
            //    toastr.warning("Enter Due Time");
            //    $("#ContentPlaceHolder1_txtDueTime").focus();
            //    return false;
            //}
            //startTime = moment(startTime, ["h:mm A"]).format("HH:mm");
            var description = $("#ContentPlaceHolder1_txtDescription").val();

            var taskStage = $("#ContentPlaceHolder1_ddlTaskStage").val();

            var accountManagerId = $("#ContentPlaceHolder1_ddlAccountManager").val();
            var taskPriority = $("#ContentPlaceHolder1_ddlAccountManager").val();
            var companyId = $("#ContentPlaceHolder1_hfCompanyId").val();
            var contactId = $("#ContentPlaceHolder1_hfContactId").val();
            var dealId = $("#ContentPlaceHolder1_hfDealId").val();
            var reminderDateFrom = $("#ContentPlaceHolder1_txtReminderFromDate").val();
            var reminderDateTo = $("#ContentPlaceHolder1_txtReminderToDate").val();

            if (taskStage == "0") {
                isClose = false;
                toastr.warning("Select Task Stage");
                $("#ContentPlaceHolder1_ddlTaskStage").focus();
                return false;
            }
            var sourceName;
            if (taskFor == "Project") {
                sourceName = $("#ContentPlaceHolder1_ddlProjectName").val();
                if (sourceName == "0") {
                    isClose = false;
                    toastr.warning("Select a Project");
                    $("#ContentPlaceHolder1_ddlProjectName").focus();
                    return false;
                }
            }
            else if (taskFor == "PreSales") {
                sourceName = $("#ContentPlaceHolder1_ddlSaleNo").val();
                if (sourceName == "0") {
                    isClose = false;
                    toastr.warning("Select Sale No");
                    $("#ContentPlaceHolder1_ddlSaleNo").focus();
                    return false;
                }
            }
            else if (taskFor == "Billing") {
                sourceName = $("#ContentPlaceHolder1_hfBillId").val();
                if (sourceName == "") {
                    isClose = false;
                    toastr.warning("Select Bill No");
                    $("#ContentPlaceHolder1_txtBillNo").focus();
                    return false;
                }
            }
            else if (taskFor == "CRM") {
                sourceName = parseInt(logId);
            }

            //var estimatedDoneHour = $("#ContentPlaceHolder1_txtEstimatedDoneHour").val();
            //if (estimatedDoneHour == "") {
            //    //isClose = false;
            //    //toastr.warning("Add Estimated Done Hour");
            //    //$("#ContentPlaceHolder1_txtEstimatedDoneHour").focus();
            //    //return false;
            //    estimatedDoneHour = '0';
            //}
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
            //var endTime = $("#ContentPlaceHolder1_txtEndTime").val();
            //if (endTime == "") {
            //    isClose = false;
            //    toastr.warning("Select End Time");
            //    $("#ContentPlaceHolder1_txtEndTime").focus();
            //    return false;
            //}
            //else {
            //    var endTime = moment(endTime, ["h:mm A"]).format("HH:mm");
            //}
            if ($("#ContentPlaceHolder1_ddlReminder").val() != 'custom') {
                reminderDateTo = estimatedDoneDate;
                var today = new Date();
                reminderDateFrom = AddOrSubDateFromDate(reminderDateTo, $("#ContentPlaceHolder1_ddlReminder").val(), '-')
            }
            else {
                reminderDateTo = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtReminderToDate").val(), '/');
                reminderDateFrom = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtReminderFromDate").val(), '/');
            }
            var AssignType = $("#<%=ddlAssignType.ClientID%>").val();
            var EmpId = new Array();
            if (logId != "0" && logId != "") {
                if (AssignType == "Individual")
                    EmpId = $("#ContentPlaceHolder1_ddlAssignTo").val();
                else if (AssignType == 'Department')
                    EmpId = $("#ContentPlaceHolder1_ddlAssignDepartment").val();
                else
                    EmpId = 0;
            }
            else if (taskFor == "CRM") {
                if (AssignType == "Individual")
                    EmpId = $("#ContentPlaceHolder1_ddlAssignTo").val();
                else if (AssignType == 'Department')
                    EmpId = $("#ContentPlaceHolder1_ddlAssignDepartment").val();
                else
                    EmpId = 0;
                sourceName = 0;
            }
            else {
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
            }
            var EmpList = $('#ContentPlaceHolder1_hfSelectedEmpId').val(EmpId).val();

            if (jQuery.inArray("0", EmpId) > -1 || EmpList == "") {
                isClose = false;
                toastr.warning("Select Employee");
                $("#ContentPlaceHolder1_ddlAssignTo").focus();
                return false;
            }

            var EmpList = $('#ContentPlaceHolder1_hfSelectedEmpId').val(EmpId).val();
            if (jQuery.inArray("0", EmpId) > -1 || EmpList == "") {
                isClose = false;
                toastr.warning("Select Assign To");
                $("#ContentPlaceHolder1_ddlAssignTo").focus();
                return false;
            }
            var emailDate = null, emailTime = null;

            var reminderType = null;


            if ($("#ContentPlaceHolder1_IsEnableEmailReminder").prop("checked") == true) {

                reminderType = $("#ContentPlaceHolder1_ddlReminderType").val();

                if (reminderType == "Once") {
                    emailDate = $("#ContentPlaceHolder1_txtEmailReminderDate").val();

                    if (emailDate == "") {
                        isClose = false;
                        toastr.warning("Enter Email Reminder Date");
                        $("#ContentPlaceHolder1_txtEmailReminderDate").focus();
                        return false;
                    }
                    emailDate = CommonHelper.DateFormatToMMDDYYYY(emailDate, '/');
                    emailTime = $("#ContentPlaceHolder1_txtEmailReminderTime").val();

                    if (emailTime == "") {
                        isClose = false;
                        toastr.warning("Enter Email Reminder Time");
                        $("#ContentPlaceHolder1_txtEmailReminderTime").focus();
                        return false;
                    }
                    emailTime = moment(emailTime, ["h:mm A"]).format("HH:mm");
                }
                var callToAction = $("#ContentPlaceHolder1_txtCallToAction").val();

            }

            var parentTaskId = $("#ContentPlaceHolder1_ddlParentTaskName").val() == null ? 0 : $("#ContentPlaceHolder1_ddlParentTaskName").val();
            var dependentTaskId = $("#ContentPlaceHolder1_ddlDependentTaskName").val() == null ? 0 : $("#ContentPlaceHolder1_ddlDependentTaskName").val();
            var task = {
                Id: id,
                TaskName: taskName,
                TaskDate: taskDate,
                //StartTime: startTime,
                Description: description,
                TaskType: taskType,
                TaskStage: taskStage,
                ParentTaskId: parentTaskId,
                DependentTaskId: dependentTaskId,
                SourceNameId: sourceName,
                EstimatedDoneDate: estimatedDoneDate,
                //EstimatedDoneHour: estimatedDoneHour,
                //EndTime: endTime,
                EmailReminderType: reminderType,
                EmailReminderDate: emailDate,
                EmailReminderTime: emailTime,
                CallToAction: callToAction,
                AssignType: AssignType,
                EmpDepartment: EmpDepartment,
                TaskFor: taskFor,
                AccountManagerId: accountManagerId,
                TaskPriority: taskPriority,
                CompanyId: companyId,
                ContactId: contactId,
                DealId: dealId,
                ReminderDateFrom: reminderDateFrom,
                ReminderDateTo: reminderDateTo,
                AssignType: AssignType
            }
            var randomDocId = $("#ContentPlaceHolder1_RandomDocId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();

            var clientId = new Array();
            var clientLIst = "";
            clientId = $("#ContentPlaceHolder1_ddltxtParticipantFromClient").val();
            clientLIst = $('#ContentPlaceHolder1_hfSelectedEmpId').val(clientId).val();
            PageMethods.SaveOrUpdateTask(task, EmpList, clientLIst, parseInt(randomDocId), deletedDoc, OnSuccessSaveOrUpdate, OnFailSaveOrUpdate);
            return false;
        }
        function AddOrSubDateFromDate(date, days, operator) {

            var tt = date;

            var date = new Date(tt);
            var newdate = new Date(date);
            if (operator == '+')
                newdate.setDate(newdate.getDate() + days);
            else if (operator == '-')
                newdate.setDate(newdate.getDate() - days);
            var dd = newdate.getDate();
            var mm = newdate.getMonth() + 1;
            var y = newdate.getFullYear();

            var someFormattedDate = mm + '/' + dd + '/' + y;
            return someFormattedDate;
        }
        function OnSuccessSaveOrUpdate(result) {
            if (result.IsSuccess) {
                if (result.IsSuccess) {
                    parent.ShowAlert(result.AlertMessage);
                    if (typeof parent.parent.ShowAlert === "function") {
                        parent.parent.ShowAlert(result.AlertMessage);
                    }
                }
                if (typeof parent.GridPaging === "function") {
                    parent.GridPaging(1, 1);
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
        function LoadContactByCompanyId(Id) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../TaskManagement/AssignTaskIFrame.aspx/GetContactInformationByCompanyId",
                dataType: "json",
                data: JSON.stringify({ id: Id }),
                async: false,
                success: (data) => {
                    OnLoadContactSucceed(data.d);
                },
                error: (error) => {
                    toastr.error(error, "", { timeOut: 5000 });
                }
            });
            //PageMethods.GetContactInformationByCompanyId(Id, OnLoadContactSucceed, OnLoadContactFailed);
            return false;
        }
        function OnLoadContactSucceed(result) {
            var list = result;
            var clientContacts = '<%=ddltxtParticipantFromClient.ClientID%>';
            var control3 = $('#' + clientContacts);
            control3.empty();
            if (list != null) {
                if (list.length > 0) {
                    control3.removeAttr("disabled");

                    for (i = 0; i < list.length; i++) {
                        control3.append('<option title="' + list[i].Name + '" value="' + list[i].Id + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    <%--control.empty().append('<option value="-1">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    control2.empty().append('<option value="-1">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');--%>
                }
            }
            else {
                control3.empty().append('<option value="-1">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
            }
            return false;

        }
        function OnLoadContactFailed(error) {
            toastr.error(error);
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

        function OnSuccessParentTaskLoading(result) {

            if (result.Task.EstimatedDoneDate == null) {
                taskMinDate = "";
                taskMaxDate = "";
            }
            else {
                taskMinDate = (GetStringFromDateTime(result.Task.TaskDate));
                taskMaxDate = (GetStringFromDateTime(result.Task.EstimatedDoneDate));
            }
            $("#ContentPlaceHolder1_txtEstimatedDoneDate").datepicker("option", {
                minDate: taskMinDate
            });
            $("#ContentPlaceHolder1_txtDueDate").datepicker("option", "maxDate", taskMaxDate);
            $("#ContentPlaceHolder1_txtDueDate").datepicker("option", {
                minDate: taskMinDate
            });

            $("#ContentPlaceHolder1_txtEstimatedDoneDate").datepicker("option", "maxDate", taskMaxDate);

            if ($('#ContentPlaceHolder1_hfTaskId').val() == '0') {
                $('#ContentPlaceHolder1_txtDueDate').datepicker({
                }).datepicker("setDate", taskMinDate);
                $('#ContentPlaceHolder1_txtEstimatedDoneDate').datepicker({
                }).datepicker("setDate", taskMaxDate);
            }
            return false;
        }
        function OnFailParentTaskLoading(error) {
            toastr.error(error.get_message());
            return false;
        }

        function FillForm(Result) {
            Clear();
            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Edit Task - " + Result.Task.TaskName,
                show: 'slide'
            });
            parentTaskIdSelect = Result.Task.ParentTaskId;
            dependentTaskIdSelect = Result.Task.DependentTaskId;
            $("#ContentPlaceHolder1_hfTaskId").val(Result.Task.Id);
            $("#ContentPlaceHolder1_txtTaskName").val(Result.Task.TaskName);
            $("#ContentPlaceHolder1_ddlTaskFor").val(Result.Task.TaskFor).trigger('change');
            if (Result.Task.TaskFor == "Project") {
                $("#ContentPlaceHolder1_ddlProjectName").val(Result.Task.SourceNameId).trigger('change');
            }
            else if (Result.Task.TaskFor == "PreSales") {
                $("#ContentPlaceHolder1_ddlSaleNo").val(Result.Task.SourceNameId).trigger('change');
            }
            else if (Result.Task.TaskFor == "Billing") {
                $("#ContentPlaceHolder1_hfBillId").val(Result.Task.SourceNameId);
                $("#ContentPlaceHolder1_txtBillNo").val(Result.Task.SourceName);
                PageMethods.LoadTaskByTaskTypeNTaskId('Billing', Result.Task.SourceNameId, OnTaskLoadingSucceed, OnTaskLoadingFailed);

            } else if (Result.Task.TaskFor == "CRM") {
                logId = Result.Task.SourceNameId;
                $("#divProject_Sale_Bill_ParentTask").hide();
                $("#divAssignToTableDesign").hide();
                $("#divMultipleAssignTo").show();
                $("#divForLog").show();
                $("#ContentPlaceHolder1_ddlTaskFor").val('CRM').prop("disabled", true);;
                if (Result.Task.CompanyId != '0' && Result.Task.CompanyId != ''&& Result.Task.CompanyId != null) {
                    $("#ContentPlaceHolder1_hfCompanyId").val(Result.Task.CompanyId);
                    $("#ContentPlaceHolder1_ddlCompany").val(Result.Task.CompanyName).prop("disabled", true);
                    $("#ContentPlaceHolder1_ddlTaskBy").val("Company Wise").trigger("change").prop("disabled", true);
                    LoadContactByCompanyId(Result.Task.CompanyId);
                }
                else if (Result.Task.ContactId != '0' && Result.Task.ContactId != ''&& Result.Task.ContactId != null ) {
                    $("#ContentPlaceHolder1_hfContactId").val(Result.Task.ContactId);
                    $("#ContentPlaceHolder1_ddlContact").val(Result.Task.ContactName).prop("disabled", true);
                    $("#ContentPlaceHolder1_ddlTaskBy").val("Contact Wise").trigger("change").prop("disabled", true);
                    LoadContactByCompanyId("0");
                }
                $("#ContentPlaceHolder1_ddltxtParticipantFromClient").val(Result.ContactLIst).trigger('change');

                if (Result.Task.DealId != '0' && Result.Task.DealId != '') {
                    $("#ContentPlaceHolder1_hfDealId").val(Result.Task.DealId);
                    $("#ContentPlaceHolder1_ddlDeal").val(Result.Task.DealName).prop("disabled", true);
                }
                $("#ContentPlaceHolder1_ddlTaskType").val(Result.Task.TaskType).trigger('change');
                $("#<%=txtTaskName.ClientID%>").val(Result.Task.TaskName);
                if (Result.Task.ContacName == "") {
                    $("#contact").hide();
                }
                if (Result.Task.CompanyName == "") {
                    $("#company").hide();
                    if (contact == "") {
                        $("#lblCompanyContact").hide();
                    }
                }
                if (Result.Task.DealName == "") {
                    $("#deal").hide();
                }
                var result = new Array();
                result = Result.EmployeeList.map(a => a.EmpId);
                if (Result.Task.AssignType == 'Individual') {
                    $("#ContentPlaceHolder1_ddlAssignType").val("Individual").trigger('change')
                    $("#ContentPlaceHolder1_ddlAssignTo").val(result).trigger('change');;
                }
                else if (Result.Task.AssignType == 'Department') {
                    $("#ContentPlaceHolder1_ddlAssignType").val("Department").trigger('change')
                    $("#ContentPlaceHolder1_ddlAssignDepartment").val(result).trigger('change');
                }
                else {
                    $("#ContentPlaceHolder1_ddlAssignType").val("All").trigger('change')
                }
                $("#ContentPlaceHolder1_ddlReminder").val("custom").trigger('change');
                $("#ContentPlaceHolder1_ddlTaskPriority").val(Result.Task.TaskPriority).trigger('change');
                $("#ContentPlaceHolder1_txtReminderFromDate").val(GetStringFromDateTime(Result.Task.ReminderDateFrom));
                $("#ContentPlaceHolder1_txtReminderToDate").val(GetStringFromDateTime(Result.Task.ReminderDateTo));

                $("#dvEmail").hide();

            }
            $("#ContentPlaceHolder1_ddlTaskStage").val(Result.Task.TaskStage);


            $("#ContentPlaceHolder1_txtDueDate").val(GetStringFromDateTime(Result.Task.TaskDate));
            //$("#ContentPlaceHolder1_txtDueTime").val(moment(Result.Task.StartTime).format("h:mm A"));
            $("#ContentPlaceHolder1_txtEstimatedDoneDate").val(GetStringFromDateTime(Result.Task.EstimatedDoneDate));
            //$("#ContentPlaceHolder1_txtEstimatedDoneHour").val(Result.Task.EstimatedDoneHour);
            //$("#ContentPlaceHolder1_txtEndTime").val(moment(Result.Task.EndTime).format("h:mm A"));
            $("#ContentPlaceHolder1_txtDescription").val(Result.Task.Description);
            //$("#ContentPlaceHolder1_ddlAssignTo").val(Result.EmployeeList).trigger('change');

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
            if (IsCanEdit)
                $("#btnSave").val('Update & Close').show();
            else
                $("#btnSave").hide();
            $("#btnClear").hide();
            $("#btnSaveNContinue").hide();
            ShowUploadedDocument($("#ContentPlaceHolder1_RandomDocId").val());
            return false;
        }

        function OnFailLoading(error) {
            toastr.error(error.get_message());
            return false;
        }
        function UpdateTaskIsCompleted(id, isCompleted) {
            if (confirm("Are you Sure?")) {
                PageMethods.UpdateTaskIsCompleted(id, isCompleted, OnSuccessUpdate, OnFailedUpdate);
            }

            return false;
        }

        function OnSuccessUpdate(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                Clear();
                $("#ContentPlaceHolder1_btnSearch").trigger("click");
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
                Clear();
                $("#ContentPlaceHolder1_btnSearch").trigger("click");
            }
            return false;
        }

        function OnFailedDelete(error) {
            toastr.error(error.get_message());
            return false;
        }

        function Clear() {
            if (logId != "" || logId != "0") {
                $("#ContentPlaceHolder1_txtTaskName").val('Follow Up');
                $("#ContentPlaceHolder1_ddlTaskFor").val('CRM').trigger('change');
                $("#ContentPlaceHolder1_ddlTaskType").val('To Do').trigger('change');
                $("#ContentPlaceHolder1_ddlAssignTo").val(null).trigger('change');
                $("#ContentPlaceHolder1_ddlAssignDepartment").val(null).trigger('change');
            }
            else {
                $("#ContentPlaceHolder1_txtTaskName").val('');
                $("#ContentPlaceHolder1_ddlTaskFor").val('PreSales').trigger('change');
            }

            $("#ContentPlaceHolder1_ddlTaskPriority").val('0');
            $("#ContentPlaceHolder1_ddlAccountManager").val('0');
            $("#ContentPlaceHolder1_ddlCompany").val('');
            $("#ContentPlaceHolder1_hfCompanyId").val('0');
            $("#ContentPlaceHolder1_hfContactId").val('0');
            $("#ContentPlaceHolder1_ddlContact").val('');
            $("#ContentPlaceHolder1_hfDealId").val('0');
            $("#ContentPlaceHolder1_ddlDeal").val('');
            $("#ContentPlaceHolder1_ddlAssignTo").val(null).trigger('change');;
            $("#ContentPlaceHolder1_ddltxtParticipantFromClient").val(null).trigger('change');;

            $("#AllAssignContainer").hide();
            $("#GroupAssignContainer").hide();
            $("#IndividualAssignUser tbody").html("");
            //$("#ContentPlaceHolder1_hfTaskId").val('0');
            $("#ContentPlaceHolder1_txtEndTime").val('');
            
            $("#ContentPlaceHolder1_hfBillId").val('');
            $("#ContentPlaceHolder1_txtBillNo").val('');
            $("#ContentPlaceHolder1_txtCallToAction").val('');
            $("#ContentPlaceHolder1_ddlProjectName").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlSaleNo").val("0").trigger('change');

            $("#ContentPlaceHolder1_txtDueDate").val(DayOpenDate);
            $("#ContentPlaceHolder1_txtDueTime").val('');
            $("#ContentPlaceHolder1_txtEstimatedDoneHour").val("");
            $("#ContentPlaceHolder1_ddlTaskStage").val("0");
            $("#ContentPlaceHolder1_txtDescription").val('');
            $("#DocumentInfo").html("");
            $("#ContentPlaceHolder1_ddlAssignTo").val(null).trigger('change');
            // $("#ContentPlaceHolder1_ddlSearchAssignTo option:selected").removeAttr("selected").trigger('change');
            $("#ContentPlaceHolder1_IsEnableEmailReminder").prop("checked", false);
            ChangeReminder($("#ContentPlaceHolder1_IsEnableEmailReminder"));
            $("#ContentPlaceHolder1_txtEmailReminderDate").val(DayOpenDate);
            $("#ContentPlaceHolder1_txtEstimatedDoneDate").val(DayOpenDate);
            $("#ContentPlaceHolder1_txtDueDate").val(DayOpenDate);
            $("#ContentPlaceHolder1_txtEmailReminderTime").val('');
            $("#ContentPlaceHolder1_ddlReminderType").val("Once");
            if (IsCanSave) {
                $("#btnSave").val('Save & Close').show();
                $("#btnSaveNContinue").show();
            }
            else {
                $("#btnSaveNContinue").show();
                $("#btnSave").hide();
            }
            isClose = false;
            $("#btnClear").show();
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
            //SaveOrUpdateTask();
            $.when(SaveOrUpdateTask()).done(function () {
                if (isClose) {
                    debugger;
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
        function LoadRepairNMaintenance(id) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../TaskManagement/AssignTaskIFrame.aspx/GetRepairNMaintenanceById',

                data: "{ 'id':'" + id + "'}",
                dataType: "json",
                success: function (data) {
                    $(<%=ddlTaskFor.ClientID%>).val("Internal").prop("disabled", true);
                    var itemName = data.d.MaintenanceType == "Fixed Asset" ? data.d.FixedItemName : data.d.ItemName;
                    $(<%=txtTaskName.ClientID%>).val(itemName).prop("disabled", true);

                },
                error: function (result) {

                }
            });
            return false;
        }
        function LoadProject(Id) {
            Clear();

            $("#ContentPlaceHolder1_ddlTaskFor").val('Project').trigger('change').prop("disabled", true);;
            $("#ContentPlaceHolder1_ddlProjectName").val(Id).trigger('change').prop("disabled", true);
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
    </script>

    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    
    <asp:HiddenField ID="CommonDropDownHiddenField" Value="0" runat="server" />
    <asp:HiddenField ID="hfBillId" runat="server" Value="0" />

    <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="tempDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfParentDoc" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfSelectedEmpId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSelectedEmpIdForSearch" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />
    <asp:HiddenField ID="hfTaskId" Value="0" runat="server" />
    <asp:HiddenField ID="hfCompanyId" Value="0" runat="server" />
    <asp:HiddenField ID="hfContactId" Value="0" runat="server" />
    <asp:HiddenField ID="hfDealId" Value="0" runat="server" />
    <div id="ShowDocumentDiv" style="display: none;">
        <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div>
        <div style="padding: 10px 30px 10px 30px">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Task For</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTaskFor" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Task Stage</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTaskStage" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" id="DivTaskType">
                    <div class="col-md-2">
                        <label class="control-label required-field">Task Type</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTaskType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0" Text="---Please Select---"></asp:ListItem>
                            <asp:ListItem Value="To do" Text="To do"></asp:ListItem>
                            <asp:ListItem Value="Call" Text="Call"></asp:ListItem>
                            <asp:ListItem Value="Message" Text="Message"></asp:ListItem>
                            <asp:ListItem Value="Email" Text="Email"></asp:ListItem>
                            <asp:ListItem Value="Meeting" Text="Meeting"></asp:ListItem>
                            <asp:ListItem Value="Site Survey" Text="Site Survey"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="divProject_Sale_Bill_ParentTask">
                    <div class="form-group" id="DivProjectName" style="display: none">
                        <div class="col-md-2">
                            <label class="control-label required-field">Project Name</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlProjectName" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group" id="DivSaleNo" style="display: none">
                        <div class="col-md-2">
                            <label class="control-label required-field">Sale No</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlSaleNo" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="form-group" id="DivBillNo" style="display: none">
                        <div class="col-md-2">
                            <label class="control-label required-field">Bill No</label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtBillNo" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group" id="DivParentTask" style="display: none">
                        <div class="col-md-2">
                            <label class="control-label ">Parent Task Name</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlParentTaskName" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-2">
                            <label class="control-label ">Dependent Task Name</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlDependentTaskName" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>

                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Task Name</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtTaskName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Task Priority</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTaskPriority" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0" Text="---Please Select---"></asp:ListItem>
                            <asp:ListItem Value="1" Text="High"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Medium"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Low"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="divForLog" style="display: none">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Task By</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlTaskBy" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Company Wise" Value="Company Wise"></asp:ListItem>
                                <asp:ListItem Text="Contact Wise" Value="Contact Wise"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label required-field">Account Manager</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlAccountManager" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2" id="lblCompanyContact">
                            <asp:Label class="control-label required-field" ID="lblCompany" runat="server" Text="Company"></asp:Label>
                        </div>
                        <div class="col-md-4" id="company">
                            <asp:TextBox ID="ddlCompany" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-md-4" style="display: none" id="contact">
                            <asp:TextBox ID="ddlContact" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                        <div id="deal">
                            <div class="col-md-2">
                                <label class="control-label required-field">Deal</label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="ddlDeal" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Participant From Client</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddltxtParticipantFromClient" TabIndex="1" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Start Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtDueDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">End Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEstimatedDoneDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" style="display: none">
                    <div class="col-md-2">
                        <label class="control-label required-field">Start Time</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtDueTime" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label class="control-label">Estimated Done Hour</label>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="txtEstimatedDoneHour" runat="server" CssClass="quantitydecimal form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" style="display: none">
                    <div class="col-md-2">
                        <label class="control-label required-field">End Time</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEndTime" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
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
                <div id="divAssignToTableDesign">
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
                            <input id="btnAddUser" type="button" value="Add User" tabindex="8" class="TransactionalButton btn btn-primary btn-sm" />
                            <input id="btnCancelUser" type="button" value="Cancel User" tabindex="9" class="TransactionalButton btn btn-primary btn-sm" />
                        </div>
                    </div>
                    <div class="IndividualAssignContainer" style="width: 100%;">
                        <div class="form-group">
                            <table id="IndividualAssignUser" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
                                <thead>
                                    <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                                        <th style="width: 50%;">Employee Name
                                        </th>
                                        <th style="width: 20%;">Employee Code
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
                </div>
                <div class="form-group" style="display: none" id="divMultipleAssignTo">
                    <div class="col-md-2">
                        <asp:Label ID="lblAssignTo" runat="server" class="control-label required-field" Text="Assign To"></asp:Label>
                    </div>
                    <div class="col-md-10" id="divAssignEmployee">
                        <asp:DropDownList ID="ddlAssignTo" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;"></asp:DropDownList>
                    </div>
                    <div class="col-md-10" id="divAssignDepartment" style="display: none">
                        <asp:DropDownList ID="ddlAssignDepartment" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Add Reminder</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReminder" runat="server" CssClass="form-control"
                            TabIndex="1">
                            <asp:ListItem Value="0" Text="Remind Same Day"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Remind 1 Day Before"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Remind 3 Days Before"></asp:ListItem>
                            <asp:ListItem Value="7" Text="Remind 1 Week Before"></asp:ListItem>
                            <asp:ListItem Value="custom" Text="Custom"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div id="divReminderDateTime" class="form-group" style="display: none">
                        <div class="col-md-2">
                            <label class="control-label">Date</label>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtReminderFromDate" placeholder="From Date" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtReminderToDate" placeholder="To Date" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
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
                <div id="DocumentInfo">
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Description</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox TextMode="MultiLine" ID="txtDescription" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" style="display: none;">
                    <div class="col-md-8">
                        <asp:CheckBox ID="IsEnableEmailReminder" TabIndex="4" runat="Server" Text="Enable email reminder?" Font-Bold="true"
                            Checked="false" CssClass="mycheckbox" TextAlign="right" onclick="ChangeReminder(this)" />
                    </div>
                </div>
                <div id="dvEmail" style="display: none;">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Reminder Type</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlReminderType" runat="server" CssClass="form-control">
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
                            <asp:TextBox ID="txtEmailReminderDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label">Email Reminder Time</label>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtEmailReminderTime" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Call To Action</label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox TextMode="MultiLine" ID="txtCallToAction" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                </div>

            </div>
            <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                <div class="col-md-12">
                    <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="SaveNClose()" value="Save & Close" />
                    <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return Clear();" />
                    <input id="btnSaveNContinue" type="button" value="Save & Continue" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return SaveOrUpdateTask();" />
                </div>
            </div>
        </div>
    </div>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
</asp:Content>
