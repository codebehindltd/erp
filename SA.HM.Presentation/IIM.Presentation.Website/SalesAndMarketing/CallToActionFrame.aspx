<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="CallToActionFrame.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.CallToActionFrame" %>

<%@ Register TagPrefix="UserControl" TagName="CallToActionUserControl" Src="~/HMCommon/UserControl/CallToActionUserControl.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script>
        var CallToActionDetails = new Array();
        var ReminderDay = new Array();
        var CallToActionDetailsDeleted = new Array();
        var tempId = 0;
        var fromCallToAction = '';
        var isClose;
        $(document).ready(function () {
            // i = 0;
            debugger;
            fromCallToAction = $.trim(CommonHelper.GetParameterByName("fca"));
            companyId = CommonHelper.GetParameterByName("cid");
            contactId = CommonHelper.GetParameterByName("ctid");
            id = CommonHelper.GetParameterByName("id");
            $("#ContentPlaceHolder1_CallToActionUserControl_ddlParticipantFromClient").empty();
            if (companyId != "0" && companyId != "") {
                LoadContactByCompanyId(companyId);

            }
            if ((contactId != "" || contactId != "0") && companyId == "0") {
                LoadContactByCompanyId("0");
            }

            if (id != '0' && id != '') {
                LoadCallToActionForEdit(id);
            }
            
            $("#ContentPlaceHolder1_txtCompany").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/CallToActionFrame.aspx/GetCompanyByAutoSearch',
                        data: JSON.stringify({ searchTerm: request.term }),
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
                    //$("#ContentPlaceHolder1_ddlActivityBy").attr("disabled", true);
                }
            });
            $("#ContentPlaceHolder1_txtContact").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/CallToActionFrame.aspx/LoadLabelByAutoSearch',
                        //data: "{'searchTerm':'" + request.term + "'}",
                        data: JSON.stringify({ searchTerm: request.term }),
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
                    $("#ContentPlaceHolder1_hfContactId").val(ui.item.value);
                }
            });
            $("#ContentPlaceHolder1_ddlActivityBy").change(function () {
                var activityBy = $("#ContentPlaceHolder1_ddlActivityBy").val();
                if (activityBy == "Contact Wise") {
                    $("#CompanyDiv").hide();
                    $("#ContactDiv").show();
                    LoadContactByCompanyId("0");
                }
                else if (activityBy == "Company Wise") {
                    $("#CompanyDiv").show();
                    $("#ContactDiv").hide();
                    $("#ContentPlaceHolder1_txtCompany").val("");
                    $("#ContentPlaceHolder1_hfCompanyId").val("0");
                    $("#ContentPlaceHolder1_CallToActionUserControl_ddlParticipantFromClient").empty();
                }
            });

            $('#ContentPlaceHolder1_CallToActionUserControl_cbRemindSameDay').change(function () {
                if ($(this).is(":checked")) {
                    ReminderDay.push(0);
                }
                else {
                    const index = ReminderDay.indexOf(0);
                    if (index > -1) {
                        ReminderDay.splice(index, 1);
                    }

                }
            });
            $('#ContentPlaceHolder1_CallToActionUserControl_cbRemind1DayBefore').change(function () {
                if ($(this).is(":checked")) {
                    ReminderDay.push(1);
                }
                else {
                    const index = ReminderDay.indexOf(1);
                    if (index > -1) {
                        ReminderDay.splice(index, 1);
                    }

                }
            });
            $('#ContentPlaceHolder1_CallToActionUserControl_cbRemind3DaysBefore').change(function () {
                if ($(this).is(":checked")) {
                    ReminderDay.push(3);
                }
                else {
                    const index = ReminderDay.indexOf(3);
                    if (index > -1) {
                        ReminderDay.splice(index, 1);
                    }

                }
            });
            $('#ContentPlaceHolder1_CallToActionUserControl_cbRemind1WeekBefore').change(function () {
                if ($(this).is(":checked")) {
                    ReminderDay.push(7);
                }
                else {
                    const index = ReminderDay.indexOf(7);
                    if (index > -1) {
                        ReminderDay.splice(index, 1);
                    }

                }
            });
            if (fromCallToAction != "Menu Links") {
                $("#ActionButtons").hide();
                $("#divForMenulinks").hide();
            }
        });
        function CreateNew() {
            //PerformClearAction();
            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '95%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Call To Action",
                show: 'slide'
            });

            return false;
        }
        function LoadContactByCompanyId(Id) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../SalesAndMarketing/CallToActionFrame.aspx/GetContactInformationByCompanyId",
                dataType: "json",
                data: JSON.stringify({ companyId: Id }),
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
            var CallContacts = 'ContentPlaceHolder1_CallToActionUserControl_ddlParticipantFromClient';
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
        function AddItem() {
            var taskName, type, date, time, time, taskAssignedEmployee, perticipentFromClient, perticipentFromOffice, ReminderDayList, otherAction, companyId, contactId, disscription, callToActionId;
            var taskAssignedEmployeeName, perticipentFromClientName, perticipentFromOfficeName;
            type = $("#ContentPlaceHolder1_CallToActionUserControl_ddlType").val();
            callToActionId = $("#ContentPlaceHolder1_hfCallToActionId").val();
            date = $("#txtDate").val();
            time = $("#txtTime").val();

            otherAction = $("#txtMeetingDiscussion").val();
            description = $("#txtDescription").val();
            taskName = $("#ContentPlaceHolder1_txtTaskName").val();
            perticipentFromClient = $("#ContentPlaceHolder1_CallToActionUserControl_ddlParticipantFromClient").val();
            perticipentFromClient = $("#ContentPlaceHolder1_hfPerticipentFromClient").val(perticipentFromClient).val();
            perticipentFromOffice = $("#ContentPlaceHolder1_CallToActionUserControl_ddlParticipantFromOffice").val();
            perticipentFromOffice = $("#ContentPlaceHolder1_hfPerticipentFromOffice").val(perticipentFromOffice).val();
            taskAssignedEmployee = $("#ContentPlaceHolder1_CallToActionUserControl_ddlAssignToEmployee").val();
            taskAssignedEmployee = $("#ContentPlaceHolder1_hfTaskAssignedEmployee").val(taskAssignedEmployee).val();

            taskAssignedEmployeeName = $("#ContentPlaceHolder1_CallToActionUserControl_ddlAssignToEmployee option:selected").toArray().map(item => item.text).join();
            taskAssignedEmployeeName = $("#ContentPlaceHolder1_hfTaskAssignedEmployee").val(taskAssignedEmployeeName).val();

            perticipentFromClientName = $("#ContentPlaceHolder1_CallToActionUserControl_ddlParticipantFromClient option:selected").toArray().map(item => item.text).join();
            perticipentFromClientName = $("#ContentPlaceHolder1_hfPerticipentFromClient").val(perticipentFromClientName).val();

            perticipentFromOfficeName = $("#ContentPlaceHolder1_CallToActionUserControl_ddlParticipantFromOffice option:selected").toArray().map(item => item.text).join();
            perticipentFromOfficeName = $("#ContentPlaceHolder1_hfPerticipentFromOffice").val(perticipentFromOfficeName).val();

            ReminderDayList = $("#ContentPlaceHolder1_hfReminderDaysList").val(ReminderDay).val();
            companyId = $("#ContentPlaceHolder1_hfCompanyId").val();
            contactId = $("#ContentPlaceHolder1_hfContactId").val();

            if (date == "") {
                toastr.warning("Please Select a Date.");
                $("#txtDate").focus();
                return false;
            }
            if (time == "") {
                toastr.warning("Please Select a Time.");
                $("#txtTime").focus();
                return false;
            }
            if (type == "") {
                toastr.warning("Please Select a Type.");
                $("#ContentPlaceHolder1_CallToActionUserControl_ddlType").focus();
                return false;
            }
            if (perticipentFromClient == "") {
                toastr.warning("Please Select Perticipent From Client.");
                $("#ContentPlaceHolder1_CallToActionUserControl_ddlType").focus();
                return false;
            }
            if (perticipentFromOffice == "") {
                toastr.warning("Please Select Perticipent From Office.");
                $("#ContentPlaceHolder1_CallToActionUserControl_ddlType").focus();
                return false;
            }
            if (taskAssignedEmployee == "") {
                toastr.warning("Please Select Employee For Task Assign.");
                $("#ContentPlaceHolder1_CallToActionUserControl_ddlType").focus();
                return false;
            }
            var tr = "";
            tr += "<tr>";
            tr += "<td style='width:6%;'>" + type + "</td>";
            tr += "<td style='width:8%;'>" + taskName + "</td>";
            tr += "<td style='width:6%;'>" + date + "</td>";
            tr += "<td style='width:5%;'>" + time + "</td>";
            tr += "<td style='width:12%;'>" + perticipentFromOfficeName + "</td>";
            tr += "<td style='width:12%;'>" + perticipentFromClientName + "</td>";
            tr += "<td style='width:12%;'>" + taskAssignedEmployeeName + "</td>";
            tr += "<td style='width:15%;'>" + otherAction + "</td>";
            tr += "<td style='width:15%;'>" + description + "</td>";
            tr += "<td style='width:9%;'>";
            tr += "<a href='javascript:void()' onclick= 'DeleteCallToActionDetails(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
            tr += "<a href='javascript:void()' onclick= 'EditCallToActionDetails(this)' ><img alt='Delete' src='../Images/edit.png' /></a>";
            tr += "</td>";
            tr += "<td style='display:none;'>" + + "</td>";
            tr += "<td style='display:none;'>" + + "</td>";
            //tr += "<td style='display:none;'>" + ItemSelected.StockBy + "</td>";
            tr += "<td style='display:none;'>" + tempId + "</td>";
            tr += "</tr>";

            $("#SupportItemTbl tbody").prepend(tr);
            tr = "";
            //CommonHelper.ApplyDecimalValidation();

            CallToActionDetails.push({
                Id: $("#ContentPlaceHolder1_hfCallToActionDetailsId").val(),
                TaskName: taskName,
                TempId: tempId,
                CallToActionId: callToActionId,
                CompanyId: companyId,
                ContactId: contactId,
                Type: type,
                Date: CommonHelper.DateFormatToMMDDYYYY(date, '/'),
                Time: moment(time, ["h:mm A"]).format("HH:mm"),
                OtherActivities: otherAction,
                Description: description,
                PerticipentFromOffice: perticipentFromOffice,
                PerticipentFromClient: perticipentFromClient,
                TaskAssignedEmployee: taskAssignedEmployee,
                ReminderDayList: ReminderDayList,
                CompanyName: $("#ContentPlaceHolder1_txtCompany").val(),
                ContactName: $("#ContentPlaceHolder1_txtContact").val()
            });
            tempId++;
            ClearAfterAdd();
        }
        function DeleteCallToActionDetails(control) {
            if (!confirm("Do you want to delete?")) { return false; }

            var tr = $(control).parent().parent();

            var index = parseInt($.trim($(tr).find("td:eq(12)").text()), 10);
            var id = parseInt($.trim($(tr).find("td:eq(10)").text()), 10);

            //var ItemOBj = CallToActionDetails.indexOf(index);
            var ItemOBj = _.findWhere(CallToActionDetails, { TempId: index });
            if (parseInt(id, 10) > 0)
                CallToActionDetailsDeleted.push(JSON.parse(JSON.stringify(ItemOBj)));

            //CallToActionDetails.splice(index, 1);
            CallToActionDetails = CallToActionDetails.filter(function (obj) {
                return obj.TempId != index;
            });
            $(tr).remove();
            return false;
        }
        function ClearAfterAdd() {
            ReminderDay = new Array();
            $('#ContentPlaceHolder1_CallToActionUserControl_cbRemindSameDay').prop("checked", false);
            $('#ContentPlaceHolder1_CallToActionUserControl_cbRemind1DayBefore').prop("checked", false);
            $('#ContentPlaceHolder1_CallToActionUserControl_cbRemind3DaysBefore').prop("checked", false);
            $('#ContentPlaceHolder1_CallToActionUserControl_cbRemind1WeekBefore').prop("checked", false);
            $("#ContentPlaceHolder1_CallToActionUserControl_ddlType").val("Call");
            $("#ContentPlaceHolder1_CallToActionUserControl_ddlParticipantFromOffice").val(null).trigger('change');
            $("#ContentPlaceHolder1_CallToActionUserControl_ddlParticipantFromClient").val(null).trigger('change');
            $("#ContentPlaceHolder1_CallToActionUserControl_ddlAssignToEmployee").val(null).trigger('change');
            //$("#ContentPlaceHolder1_hfCallToActionId").val();
            $("#txtDate").val("");
            $("#txtTime").val("");
            $("#txtMeetingDiscussion").val("");
            $("#txtDescription").val("");
            $("#ContentPlaceHolder1_txtCompany").val("");
            $("#ContentPlaceHolder1_hfCompanyId").val("0");
            $("#ContentPlaceHolder1_txtContact").val("");
            $("#ContentPlaceHolder1_hfContactId").val("0");
            $("#ContentPlaceHolder1_hfCallToActionDetailsId").val("0");
            return false;
        }
        function SaveOrUpdateCallToAction(masterId, Company = 0, Contact = 0) {
            var activityBy="", companyId, contactId;
            if (fromCallToAction != 'Menu Links') {
                for (var i = 0; i < CallToActionDetails.length; i++) {
                    CallToActionDetails[i].CompanyId = Company;
                    CallToActionDetails[i].ContactId = Contact;
                }
            }

            //var masterId = "0";
            var CallToAction = {
                Id: parseInt($("#ContentPlaceHolder1_hfCallToActionId").val()),
                ActivityBy: activityBy,
                FromCallToAction: fromCallToAction,
                MasterId: masterId
            }

            PageMethods.SaveOrUpdateCallToAction(CallToAction, CallToActionDetails, CallToActionDetailsDeleted, OnSuccessSaveOrUpdate, OnFailSaveOrUpdate);
            return false;
        }
        function OnSuccessSaveOrUpdate(result) {
            if (typeof parent.parent.CloseDialog === "function") {
                parent.parent.ShowAlert(result.AlertMessage);
            }           
            PerformClearAction();
        }
        function OnFailSaveOrUpdate(error) {

        }
        function PerformClearAction() {
            $("#SupportItemTbl tbody").prepend("");
            $("#ContentPlaceHolder1_hfCallToActionId").val("0");
            return false;
        }

        function LoadCallToActionForEdit(id) {
            PageMethods.GetCallToActionById(id, OnGetSuccess, OnGetFail);
            return false;
        }
        function OnGetSuccess(data) {
            
            $("#ContentPlaceHolder1_hfCallToActionId").val(data.CallToAction.Id);
            if (fromCallToAction == 'Menu Links') {
                if (data.CallToAction.CompanyId != 0 && data.CallToAction.CompanyId != '') {
                    $("#ContentPlaceHolder1_ddlActivityBy").val('Company Wise').trigger('change');
                    $("#ContentPlaceHolder1_hfCompanyId").val(data.CallToAction.CompanyId);
                    $("#ContentPlaceHolder1_txtCompany").val(data.CallToAction.CompanyName);
                    LoadContactByCompanyId(data.CallToAction.CompanyId);
                }
                else {
                    debugger;
                    $("#ContentPlaceHolder1_ddlActivityBy").val('Company Wise').trigger('change');
                    $("#ContentPlaceHolder1_hfContactId").val(data.CallToAction.ContactId);
                    $("#ContentPlaceHolder1_txtContact").val(data.CallToAction.ContactName);
                    LoadContactByCompanyId("0");
                }
            }
            else {
                if (data.CallToAction.CompanyId != 0 && data.CallToAction.CompanyId != '') {
                    $("#ContentPlaceHolder1_hfCompanyId").val(data.CallToAction.CompanyId);
                }
                else {
                    $("#ContentPlaceHolder1_hfContactId").val(data.CallToAction.ContactId);
                }
            }
            if (data.CallToActionDetailList.length > 0) {
                for (var i = 0; i < data.CallToActionDetailList.length; i++) {
                    var tr = "";
                    tr += "<tr>";
                    tr += "<td style='width:6%;'>" + data.CallToActionDetailList[i].Type + "</td>";
                    tr += "<td style='width:8%;'>" + data.CallToActionDetailList[i].TaskName + "</td>";
                    tr += "<td style='width:6%;'>" + moment(data.CallToActionDetailList[i].Date).format("MM/DD/YYYY") + "</td>";
                    tr += "<td style='width:5%;'>" + moment(data.CallToActionDetailList[i].Time).format("hh:mm A") + "</td>";
                    tr += "<td style='width:12%;'>" + data.CallToActionDetailList[i].PerticipentFromOfficeName + "</td>";
                    tr += "<td style='width:12%;'>" + data.CallToActionDetailList[i].PerticipentFromClientName + "</td>";
                    tr += "<td style='width:12%;'>" + data.CallToActionDetailList[i].TaskAssignedEmployeeName + "</td>";
                    tr += "<td style='width:15%;'>" + data.CallToActionDetailList[i].OtherActivities + "</td>";
                    tr += "<td style='width:15%;'>" + data.CallToActionDetailList[i].Description + "</td>";
                    tr += "<td style='width:9%;'>";
                    tr += "<a href='javascript:void()' onclick= 'DeleteCallToActionDetails(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                    tr += "<a href='javascript:void()' onclick= 'EditCallToActionDetails(this)' ><img alt='Delete' src='../Images/edit.png' /></a>";
                    tr += "</td>";
                    tr += "<td style='display:none;'>" + data.CallToActionDetailList[i].Id + "</td>";
                    tr += "<td style='display:none;'>" + + "</td>";
                    //tr += "<td style='display:none;'>" + ItemSelected.StockBy + "</td>";
                    tr += "<td style='display:none;'>" + tempId + "</td>";
                    tr += "</tr>";

                    $("#SupportItemTbl tbody").prepend(tr);
                    tr = "";
                    //CommonHelper.ApplyDecimalValidation();

                    CallToActionDetails.push({
                        Id: data.CallToActionDetailList[i].Id,
                        TempId: tempId,
                        TaskName: data.CallToActionDetailList[i].TaskName,
                        CallToActionId: data.CallToActionDetailList[i].CallToActionId,
                        Type: data.CallToActionDetailList[i].Type,
                        Date: moment(data.CallToActionDetailList[i].Date).format("MM/DD/YYYY"),
                        Time: moment(data.CallToActionDetailList[i].Time, ["h:mm A"]).format("HH:mm"),
                        OtherActivities: data.CallToActionDetailList[i].OtherActivities,
                        Description: data.CallToActionDetailList[i].Description,
                        PerticipentFromOffice: data.CallToActionDetailList[i].PerticipentFromOffice,
                        PerticipentFromClient: data.CallToActionDetailList[i].PerticipentFromClient,
                        TaskAssignedEmployee: data.CallToActionDetailList[i].TaskAssignedEmployee,
                        ReminderDayList: data.CallToActionDetailList[i].ReminderDayList,
                        CompanyName: data.CallToActionDetailList[i].CompanyName,
                        CompanyId: data.CallToActionDetailList[i].CompanyId,
                        ContactName: data.CallToActionDetailList[i].ContactName,
                        ContactId: data.CallToActionDetailList[i].ContactId
                    });
                    tempId++;
                    ClearAfterAdd();
                }
            }
        }
        function OnGetFail(error) {

        }
        function SaveAndClose() {
            isClose = true;
            //SaveOrUpdateTask();
            $.when(SaveOrUpdateCallToAction()).done(function () {
                if (isClose) {

                    if (typeof parent.CloseDialog === "function") {
                        parent.CloseDialog();
                    }
                    if (typeof parent.GridPaging === "function") {
                        parent.GridPaging(1, 1);
                    }
                }
            });
            return false;
        }

        function EditCallToActionDetails(control) {
            if (!confirm("Do you want to Edit?")) { return false; }
            var taskAssignedEmployee, perticipentFromClient, perticipentFromOffice;
            var tr = $(control).parent().parent();

            var index = parseInt($.trim($(tr).find("td:eq(12)").text()), 10);
            var id = parseInt($.trim($(tr).find("td:eq(10)").text()), 10);

            //var ItemOBj = CallToActionDetails.indexOf(index);
            var ItemOBj = _.findWhere(CallToActionDetails, { TempId: index });
            //ReminderDay = ItemOBj.ReminderDayList.split(",");
            //var taskAssignedEmployee = ItemOBj.TaskAssignedEmployee.split(",");
            //var perticipentFromClient = ItemOBj.PerticipentFromClient.split(",");
            //var perticipentFromOffice = ItemOBj.PerticipentFromOffice.split(",");
            var perticipentFromOffice = new Array();
            $.each(ItemOBj.PerticipentFromOffice.split(","), function () {
                perticipentFromOffice.push($.trim(this));
            });

            var perticipentFromClient = new Array();
            $.each(ItemOBj.PerticipentFromClient.split(","), function () {
                perticipentFromClient.push($.trim(this));
            });

            var taskAssignedEmployee = new Array();
            $.each(ItemOBj.TaskAssignedEmployee.split(","), function () {
                taskAssignedEmployee.push($.trim(this));
            });

            $.each(ItemOBj.ReminderDayList.split(","), function () {
                ReminderDay.push($.trim(this));
            });


            
            //for (var i = 0; i < ItemOBj.ReminderList.length; i++) {
            //    ReminderDay.push(data.ReminderList[i]);
            //}

            if (ReminderDay.includes("0"))
                $('#ContentPlaceHolder1_CallToActionUserControl_cbRemindSameDay').prop("checked", true);
            if (ReminderDay.includes('1'))
                $('#ContentPlaceHolder1_CallToActionUserControl_cbRemind1DayBefore').prop("checked", true);
            if (ReminderDay.includes('3'))
                $('#ContentPlaceHolder1_CallToActionUserControl_cbRemind3DaysBefore').prop("checked", true);
            if (ReminderDay.includes('7'))
                $('#ContentPlaceHolder1_CallToActionUserControl_cbRemind1WeekBefore').prop("checked", true);

            $("#ContentPlaceHolder1_CallToActionUserControl_ddlType").val(ItemOBj.Type);

            $("#ContentPlaceHolder1_hfCallToActionId").val();
            $("#txtDate").val(moment(ItemOBj.Date).format("DD/MM/YYYY"));
            $("#txtTime").val(Conv24ToAMPM(ItemOBj.Time));
            $("#txtMeetingDiscussion").val(ItemOBj.OtherActivities);
            $("#txtDescription").val(ItemOBj.Description);
            $("#ContentPlaceHolder1_Id").val(ItemOBj.Id);
            $("#ContentPlaceHolder1_hfCallToActionId").val(ItemOBj.CallToActionId);
            $("#ContentPlaceHolder1_txtTaskName").val(ItemOBj.TaskName);
            $("#ContentPlaceHolder1_hfCallToActionDetailsId").val(ItemOBj.Id);
            
            if (ItemOBj.CompanyId == 0 && ItemOBj.ContactId == 0) {
                $("#divForMenulinks").hide();
            }
            if (ItemOBj.CompanyId != 0) {
                $("#ContentPlaceHolder1_ddlActivityBy").val("Company Wise").trigger('change');
                LoadContactByCompanyId(ItemOBj.CompanyId)
            }
            if (ItemOBj.ContactId != 0) {
                $("#ContentPlaceHolder1_ddlActivityBy").val("Contact Wise").trigger('change');
                LoadContactByCompanyId("0");
            }
            $("#ContentPlaceHolder1_txtCompany").val(ItemOBj.CompanyName);
            $("#ContentPlaceHolder1_hfCompanyId").val(ItemOBj.CompanyId);
            $("#ContentPlaceHolder1_txtContact").val(ItemOBj.ComtactName);
            $("#ContentPlaceHolder1_hfContactId").val(ItemOBj.ContactId);

            $("#ContentPlaceHolder1_CallToActionUserControl_ddlParticipantFromOffice").val(perticipentFromOffice).trigger('change');
            $("#ContentPlaceHolder1_CallToActionUserControl_ddlParticipantFromClient").val(perticipentFromClient).trigger('change');
            $("#ContentPlaceHolder1_CallToActionUserControl_ddlAssignToEmployee").val(taskAssignedEmployee).trigger('change');
            //return false;
            //CallToActionDetails.remove(ItemO);
            CallToActionDetails = CallToActionDetails.filter(function (obj) {
                return obj.TempId != index;
            });
            
            $(tr).remove();
            return false;
        }
        Conv24ToAMPM = function (time24) {
            var ts = time24;
            var H = +ts.substr(0, 2);
            var h = (H % 12) || 12;
            h = (h < 10) ? ("0" + h) : h;  // leading 0 at the left for 1 digit hours
            var ampm = H < 12 ? " AM" : " PM";
            ts = h + ts.substr(2, 3) + ampm;
            return ts;
        };
    </script>
    <%--<UserControl:CallToActionUserControl ID="CallToActionUserControl" runat="server" />--%>
    <asp:HiddenField ID="CommonDropDownHiddenField" Value="0" runat="server" />
    <asp:HiddenField ID="hfPerticipentFromClient" Value="" runat="server" />
    <asp:HiddenField ID="hfPerticipentFromOffice" Value="" runat="server" />
    <asp:HiddenField ID="hfTaskAssignedEmployee" Value="" runat="server" />
    <asp:HiddenField ID="hfReminderDaysList" Value="" runat="server" />
    <asp:HiddenField ID="hfCallToActionId" Value="0" runat="server" />
    <asp:HiddenField ID="hfCallToActionDetailsId" Value="0" runat="server" />
    <asp:HiddenField ID="hfCompanyId" Value="0" runat="server" />
    <asp:HiddenField ID="hfContactId" Value="0" runat="server" />

    <div style="padding: 10px 30px 10px 30px">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label required-field">Task Name</label>
                </div>
                <div class="col-sm-4">
                    <asp:TextBox ID="txtTaskName" runat="server" CssClass="form-control" AutoComplete="Off"></asp:TextBox>
                </div>
            </div>
            <div id="divForMenulinks">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Activity By</label>
                    </div>
                    <div class="col-sm-4">
                        <asp:DropDownList ID="ddlActivityBy" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Company Wise" Value="Company Wise"></asp:ListItem>
                            <asp:ListItem Text="Contact Wise" Value="Contact Wise"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group">
                    <div id="CompanyDiv">
                        <div class="col-md-2">
                            <label class="control-label required-field">Company</label>
                        </div>
                        <div class="col-sm-10">
                            <asp:TextBox ID="txtCompany" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div id="ContactDiv" style="display: none;">
                        <div class="col-md-2">
                            <label class="control-label required-field">Contact</label>
                        </div>
                        <div class="col-sm-10">
                            <asp:TextBox ID="txtContact" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <UserControl:CallToActionUserControl ID="CallToActionUserControl" runat="server" />
            <div class="form-group" style="padding-top: 10px;">
                <div class="col-md-12">
                    <input id="btnAdd" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" onclick="AddItem()" />
                    <input id="btnCancelOrder" type="button" value="Cancel" onclick="ClearSupportItemContainer()"
                        class="TransactionalButton btn btn-primary btn-sm" />
                </div>
            </div>
        </div>
        <div style="height: auto; overflow-y: scroll;">
            <table id="SupportItemTbl" class="table table-bordered table-condensed table-hover">
                <thead>
                    <tr>
                        <th style="width: 6%;">Task Type</th>
                        <th style="width: 8%;">Task Name</th>
                        <th style="width: 6%;">Date</th>
                        <th style="width: 5%;">Time</th>
                        <th style="width: 12%;">Perticipent From Office</th>
                        <th style="width: 12%;">Perticipent From Office</th>
                        <th style="width: 12%;">Assigned Employee</th>
                        <th style="width: 15%;">Other Action</th>
                        <th style="width: 15%;">Discription</th>
                        <th style="width: 9%;">Action</th>
                    </tr>
                </thead>
                <tbody></tbody>
            </table>
        </div>
        <div class="row" id="ActionButtons" style="padding-bottom: 0; padding-top: 10px;">
            <div class="col-md-12">
                <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="SaveAndClose()" value="Save & Close" />
                <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="javascript: return PerformClearAction();" />
                <input id="btnSaveNContinue" type="button" value="Save & Continue" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="javascript: return SaveOrUpdateCallToAction(0);" />
            </div>
        </div>
    </div>
</asp:Content>
