<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="SalesCallEntry.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.SalesCallEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var salesCallId = "", companyId = "", actionType = "", dealId = "", contactId = "", from = "";
        var previousClientPerticipent; var deletedClientPerticipent = new Array();
        var isClose = 1;
        $(document).ready(function () {
            companyId = CommonHelper.GetParameterByName("cid");
            salesCallId = CommonHelper.GetParameterByName("id");
            actionType = CommonHelper.GetParameterByName("t");
            dealId = CommonHelper.GetParameterByName("did");
            contactId = CommonHelper.GetParameterByName("ctid");
            from = CommonHelper.GetParameterByName("from");
            var date = CommonHelper.GetParameterByName("date");
            var time = CommonHelper.GetParameterByName("time");
            var taskFeedback = CommonHelper.GetParameterByName("tfb");
            var meetingAgenda = CommonHelper.GetParameterByName("ta");
            var meetingLocation = CommonHelper.GetParameterByName("tl");
            var meetingDiscussion = CommonHelper.GetParameterByName("td");
            var callToAction = CommonHelper.GetParameterByName("tcta");
            var taskType = CommonHelper.GetParameterByName("tType");


            $("#MeetingLogArea").hide();
            $("#EmailLogArea").hide();
            $("#CallLogArea").show();
            $('#ContentPlaceHolder1_cbAddReminder').prop("checked", false);

            
            if ((companyId != "" && companyId != "0" && companyId != "undefined") && (dealId == "" || dealId == "0")) {
                LoadContactByCompanyId(companyId);
                $("#ContentPlaceHolder1_hfCompanyId").val(companyId);

            }
            else {
                LoadContactByCompanyId("0");
            }
            if ((contactId != "" && contactId != "0" && contactId != "undefined") && (companyId == "0" || companyId == "")) {
                LoadContactByCompanyId("0");
                $("#ContentPlaceHolder1_hfContactId").val(contactId);
            }
            if ((salesCallId != "" && salesCallId != "0" && salesCallId != "undefined") && actionType == "") {
                debugger;
                GetLogId(salesCallId);
            }
            else if ((salesCallId != "" && salesCallId != "0" && salesCallId != "undefined") && actionType == "d") {
                DeleteLog(salesCallId);
            }
            if (dealId != "" && dealId != "0" && dealId != "undefined") {
                if ((companyId != "0" && companyId != "")) {
                    LoadContactByCompanyId(companyId);
                    $("#ContentPlaceHolder1_hfCompanyId").val(companyId);
                }
                else {
                    LoadContactByCompanyId("0");
                }
                $("#ContentPlaceHolder1_hfDealId").val(dealId);

            }
            $('#ContentPlaceHolder1_ddlParticipantFromClient').select2();

            $('#ContentPlaceHolder1_ddltxtParticipantFromOffice').select2();
            $('#ContentPlaceHolder1_ddlEmailParticipantFromOffice').select2();
            $('#ContentPlaceHolder1_ddlCallParticipantFromOffice').select2();

            $('#ContentPlaceHolder1_ddlEmailContacts').select2();
            $('#ContentPlaceHolder1_ddlCallContacts').select2();

            $('#txtLogTime').timepicker({
                showPeriod: is12HourFormat
            });

            $('#txtMeetingLogTime').timepicker({
                showPeriod: is12HourFormat
            });

            $('#txtCallTime').timepicker({
                showPeriod: is12HourFormat
            });

            $('#txtEmailTime').timepicker({
                showPeriod: is12HourFormat
            });

            $("#txtLogDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#txtMeetingDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#txtCallDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#txtEmailDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#ContentPlaceHolder1_ddlCompanyOwner").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            //if ($("#ContentPlaceHolder1_hfIsAdminUser").val() == "1") {
            //    $("#LogDateDiv").show();
            //}
            //else {
            //    $("#LogDateDiv").hide();
            //}


            $("#ContentPlaceHolder1_ddlLogType").change(function () {
                if ($(this).val() == "Log a call") {
                    $("#CallBodyLabel").text("Call Body");
                    $("#CallDateLabel").text("Call Date");
                    $("#CallTimeLabel").text("Call Time");
                    $("#MeetingLogArea").hide();
                    $("#EmailLogArea").hide();
                    $("#CallLogArea").show();
                    $("#MeetingTypeDiv").hide();
                    $("#MeetingTypeControlDiv").hide();
                    $("#MessageTypeDiv").hide();
                    $("#MessageTypeControlDiv").hide();
                    $("#CallLogStatusDiv").show();
                    $("#CallLogStatusControlDiv").show();
                    $("#MessageLogStatusDiv").hide();
                    $("#MessageLogStatusControlDiv").hide();
                }
                else if ($(this).val() == "Log a message") {
                    $("#CallBodyLabel").text("Message Body");
                    $("#CallDateLabel").text("Message Date");
                    $("#CallTimeLabel").text("Message Time");
                    $("#MeetingLogArea").hide();
                    $("#EmailLogArea").hide();
                    $("#CallLogArea").show();
                    $("#MeetingTypeDiv").hide();
                    $("#MeetingTypeControlDiv").hide();
                    $("#MessageTypeDiv").show();
                    $("#MessageTypeControlDiv").show();
                    $("#CallLogStatusDiv").hide();
                    $("#CallLogStatusControlDiv").hide();
                    $("#MessageLogStatusDiv").show();
                    $("#MessageLogStatusControlDiv").show();
                }
                else if ($(this).val() == "Log an email") {
                    $("#MeetingLogArea").hide();
                    $("#EmailLogArea").show();
                    $("#CallLogArea").hide();
                    $("#MeetingTypeDiv").hide();
                    $("#MeetingTypeControlDiv").hide();
                    $("#MessageTypeDiv").hide();
                    $("#MessageTypeControlDiv").hide();
                    $("#CallLogStatusDiv").hide();
                    $("#CallLogStatusControlDiv").hide();
                    $("#MessageLogStatusDiv").hide();
                    $("#MessageLogStatusControlDiv").hide();
                }
                else if ($(this).val() == "Log a meeting") {
                    $("#MeetingLogArea").show();
                    $("#EmailLogArea").hide();
                    $("#CallLogArea").hide();
                    $("#MeetingTypeDiv").show();
                    $("#MeetingTypeControlDiv").show();
                    $("#MessageTypeDiv").hide();
                    $("#MessageTypeControlDiv").hide();
                    $("#CallLogStatusDiv").hide();
                    $("#CallLogStatusControlDiv").hide();
                    $("#MessageLogStatusDiv").hide();
                    $("#MessageLogStatusControlDiv").hide();
                }
            });
            if (from == 'MenuLinks') {
                $('#AccountManagerDiv').show();
            }
            else if ($.trim(from) == "task") {
                if ($.trim(taskType) == "Call") {
                    $("#ContentPlaceHolder1_ddlLogType").val("Log a call").trigger('change');
                    $("#txtCallDate").val(date);//GetStringFromDateTime(log.MeetingDate));
                    $("#txtCallTime").val(time);
                    $("#txtCallBody").val(taskFeedback);
                }
                else if ($.trim(taskType) == "Message") {
                    $("#ContentPlaceHolder1_ddlLogType").val("Log a message").trigger('change');
                    $("#txtCallDate").val(date);//GetStringFromDateTime(log.MeetingDate));
                    $("#txtCallTime").val(time);
                    $("#txtCallBody").val(taskFeedback);
                }
                else if ($.trim(taskType) == "Email") {
                    $("#ContentPlaceHolder1_ddlLogType").val("Log an email").trigger('change');
                    $("#txtEmailDate").val(date)//GetStringFromDateTime(log.MeetingDate));
                    $("#txtEmailTime").val(time);
                    $("#txtEmailBody").val(taskFeedback);
                }
                else if ($.trim(taskType) == "Meeting") {
                    $("#ContentPlaceHolder1_ddlLogType").val("Log a meeting").trigger('change');

                    $("#txtMeetingDate").val(date);
                    $("#txtMeetingLogTime").val(time);
                    $("#txtMeetingLocation").val(meetingLocation);
                    $("#txtMeetingAgenda").val(meetingAgenda);
                    $("#txtMeetingDiscussion").val(meetingDiscussion);
                    $("#txtCallToAction").val(callToAction);
                }
                $("#ContentPlaceHolder1_btnSaveNCreateTask").hide();
                $("#ContentPlaceHolder1_btnSaveNContinue").hide();
                $("#ContentPlaceHolder1_btnClean").hide();
            }
            $("#ContentPlaceHolder1_txtCompany").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/SalesCallEntry.aspx/GetCompanyByAutoSearch',
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
                    LoadContactByCompany(ui.item.value);
                }
            });
            $("#ContentPlaceHolder1_txtContact").autocomplete({
                source: function (request, response) {
                    //debugger;
                    var accountManager = $("#ContentPlaceHolder1_ddlCompanyOwner").val();
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
            $("#ContentPlaceHolder1_txtDeal").autocomplete({
                source: function (request, response) {
                    //debugger;
                    var accountManager = $("#ContentPlaceHolder1_ddlCompanyOwner").val();
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
            $("#ContentPlaceHolder1_ddlActivityBy").change(function () {
                var activityBy = $("#ContentPlaceHolder1_ddlActivityBy").val();
                if (activityBy == "Contact Wise") {
                    $("#CompanyDiv").hide();
                    $("#ContactDiv").show();

                    LoadContactByCompany("0");
                }
                else if (activityBy == "Company Wise") {
                    $("#CompanyDiv").show();
                    $("#ContactDiv").hide();
                    $("#ContentPlaceHolder1_txtCompany").val("");
                    $("#ContentPlaceHolder1_hfCompanyId").val("0");
                    $("#ContentPlaceHolder1_ddlCallContacts").empty();
                    $("#ContentPlaceHolder1_ddlEmailContacts").empty();
                    $("#ContentPlaceHolder1_ddlParticipantFromClient").empty();
                }
            });

        });

        function SaveNClose() {
            debugger;
            var id = 0, socialMediaId = 0, logType, meetingType, messengerId, meetingLocation, participantFromParty, meetingAgenda,
                decission, meetingAfterAction, emailType, callStatus, logBody, createdtime, createdDate;

            var salesCallParticipantFromClient = new Array();
            var participantFromOffice = new Array();

            if (companyId == "") {
                companyId = "0";
            }

            logType = $("#ContentPlaceHolder1_ddlLogType").val();
            socialMediaId = $("#ContentPlaceHolder1_ddlMessageType").val();
            meetingType = $("#ContentPlaceHolder1_ddlMeetingType").val();

            createdDate = $("#txtLogDate").val();
            createdtime = $("#txtLogTime").val();

            var eventDate = "";
            var eventTime = "";
            var messagengerId = "";
            var accountManagerId = "0";
            var isAdminUser = $("#ContentPlaceHolder1_hfIsAdminUser").val();

            //if ($("#ContentPlaceHolder1_hfIsAdminUser").val() == "1") {
            //    if (createdDate == "") {
            //        toastr.warning("Please Enter Log Date.");
            //        return false;
            //    }
            //    else if (createdtime == "") {
            //        toastr.warning("Please Enter Log Time.");
            //        return false;
            //    }
            //}

            if (createdDate != "")
                createdDate = CommonHelper.DateFormatToMMDDYYYY(createdDate, '/');

            var SalesCallEntry = {};
            var id = $("#ContentPlaceHolder1_hfSalesCallId").val();

            if (from == 'MenuLinks') {
                accountManagerId = $("#ContentPlaceHolder1_ddlCompanyOwner").val();
                if ($("#ContentPlaceHolder1_ddlActivityBy").val() == "Company Wise") {
                    companyId = $("#ContentPlaceHolder1_hfCompanyId").val();
                    if (companyId == "0") {
                        toastr.warning("Please Select Company");
                        isClose = 0;
                        return false;
                    }
                }
                else if ($("#ContentPlaceHolder1_ddlActivityBy").val() == "Contact Wise") {
                    contactId = $("#ContentPlaceHolder1_hfCompanyId").val();
                }

                dealId = $("#ContentPlaceHolder1_hfDealId").val();
                if (accountManagerId == "0") {
                    toastr.warning("Please Select Account Manager");
                    isClose = 0;
                    return false;
                }
            }

            if ($("#ContentPlaceHolder1_ddlLogType").val() == "Log a meeting") {
                eventDate = $("#txtMeetingDate").val();
                eventTime = $("#txtMeetingLogTime").val();
                meetingLocation = $("#txtMeetingLocation").val();
                meetingAgenda = $("#txtMeetingAgenda").val();
                decission = $("#txtMeetingDiscussion").val();
                meetingAfterAction = $("#txtCallToAction").val();
                logBody = $("#txtMeetingDiscussion").val();

                $("#ContentPlaceHolder1_ddltxtParticipantFromOffice :selected").each(function () {
                    participantFromOffice.push({
                        ContactId: $(this).val(),
                        Contact: $(this).text()
                    });
                });


                $("#ContentPlaceHolder1_ddlParticipantFromClient :selected").each(function () {
                    salesCallParticipantFromClient.push({
                        ContactId: $(this).val(),
                        Contact: $(this).text()
                    });
                });

                var salesCallParticipantFromClientIdList = _.map(salesCallParticipantFromClient, function (item) { return parseInt(item.ContactId); });
                var previousClientPerticipentIdList = _.map(previousClientPerticipent, function (item) { return item.ContactId; });
                for (var i = 0; i < previousClientPerticipentIdList.length; i++) {
                    if (jQuery.inArray(previousClientPerticipentIdList[i], salesCallParticipantFromClientIdList) === -1) {
                        deletedClientPerticipent.push(previousClientPerticipent[i]);
                        continue;
                    }
                }

                if (meetingType == "0") {
                    toastr.warning("Please Enter the meeting Type.");
                    isClose = 0;
                    return false;
                }
                else if (eventDate == "") {
                    toastr.warning("Please Enter the Meeting Date");
                    isClose = 0;
                    return false;
                }
                else if (eventTime == "") {
                    toastr.warning("Please Enter the Meeting Time:");
                    isClose = 0;
                    return false;
                }
                else if (meetingLocation == "") {
                    toastr.warning("Please Add the Meeting Location");
                    isClose = 0;
                    return false;
                } else if (meetingAgenda == "") {
                    toastr.warning("Please Add the Meeting Agenda");
                    isClose = 0;
                    return false;
                } else if (decission == "") {
                    toastr.warning("Please Add the Meeting Discussion");
                    isClose = 0;
                    return false;
                } else if (meetingAfterAction == "") {
                    toastr.warning("Please Add the Call To Action");
                    isClose = 0;
                    return false;
                } else if (participantFromOffice.length < 1) {
                    toastr.warning("Please Select Participant From Office");
                    isClose = 0;
                    return false;
                }

                eventDate = CommonHelper.DateFormatToMMDDYYYY(eventDate, '/');

                SalesCallEntry = {
                    Id: id,
                    LogType: logType,
                    MeetingDate: eventDate + " " + eventTime,
                    MeetingLocation: meetingLocation,
                    ParticipantFromParty: null,
                    MeetingAgenda: meetingAgenda,
                    Decission: decission,
                    MeetingAfterAction: meetingAfterAction,
                    EmailType: null,
                    CallStatus: null,
                    LogBody: logBody,
                    CompanyId: companyId,
                    DealId: dealId,
                    ContactId: contactId,
                    LogDate: eventDate + " " + eventTime,
                    SocialMediaId: socialMediaId,
                    MeetingType: meetingType,
                    IsAdminUser: isAdminUser,
                    MessagengerId: messagengerId,
                    AccountManagerId: accountManagerId,
                };
            }
            else if ($("#ContentPlaceHolder1_ddlLogType").val() == "Log an email") {
                eventDate = $("#txtEmailDate").val();
                eventTime = $("#txtEmailTime").val();

                emailType = $("#ContentPlaceHolder1_ddlEmailType").val();

                $("#ContentPlaceHolder1_ddlEmailParticipantFromOffice :selected").each(function () {
                    participantFromOffice.push({
                        ContactId: $(this).val(),
                        Contact: $(this).text()
                    });
                });

                $("#ContentPlaceHolder1_ddlEmailContacts :selected").each(function () {
                    salesCallParticipantFromClient.push({
                        ContactId: $(this).val(),
                        Contact: $(this).text()
                    });
                });

                var salesCallParticipantFromClientIdList = _.map(salesCallParticipantFromClient, function (item) { return parseInt(item.ContactId); });
                var previousClientPerticipentIdList = _.map(previousClientPerticipent, function (item) { return item.ContactId; });
                for (var i = 0; i < previousClientPerticipentIdList.length; i++) {
                    if (jQuery.inArray(previousClientPerticipentIdList[i], salesCallParticipantFromClientIdList) === -1) {
                        deletedClientPerticipent.push(previousClientPerticipent[i]);
                        continue;
                    }
                }

                logBody = $("#txtEmailBody").val();

                if (eventDate == "") {
                    toastr.warning("Please Enter the Email Date");
                    isClose = 0;
                    return false;
                }
                else if (eventTime == "") {
                    toastr.warning("Please Enter the Email Time:");
                    isClose = 0;
                    return false;
                }
                else if (salesCallParticipantFromClient.length < 1) {
                    toastr.warning("Please Select Company Contacts");
                    isClose = 0;
                    return false;
                }
                else if (logBody.trim() == "") {
                    toastr.warning("Please Add Email Body");
                    isClose = 0;
                    return false;
                }
                else if (participantFromOffice.length < 1) {
                    toastr.warning("Please Select Participant From Office");
                    isClose = 0;
                    return false;
                }

                eventDate = CommonHelper.DateFormatToMMDDYYYY(eventDate, '/');

                SalesCallEntry = {
                    Id: id,
                    LogType: logType,
                    MeetingDate: eventDate + " " + eventTime,
                    MeetingLocation: null,
                    ParticipantFromParty: null,
                    MeetingAgenda: null,
                    Decission: null,
                    MeetingAfterAction: null,
                    EmailType: emailType,
                    CallStatus: null,
                    LogBody: logBody,
                    CompanyId: companyId,
                    DealId: dealId,
                    ContactId: contactId,
                    LogDate: eventDate + " " + eventTime,
                    SocialMediaId: socialMediaId,
                    MeetingType: meetingType,
                    IsAdminUser: isAdminUser,
                    MessagengerId: messagengerId,
                    AccountManagerId: accountManagerId,
                };
            }
            else if ($("#ContentPlaceHolder1_ddlLogType").val() == "Log a call") {
                eventDate = $("#txtCallDate").val();
                eventTime = $("#txtCallTime").val();

                callStatus = $("#ContentPlaceHolder1_ddlCallLogStatus").val();

                $("#ContentPlaceHolder1_ddlCallParticipantFromOffice :selected").each(function () {
                    participantFromOffice.push({
                        ContactId: $(this).val(),
                        Contact: $(this).text()
                    });
                });


                $("#ContentPlaceHolder1_ddlCallContacts :selected").each(function () {
                    salesCallParticipantFromClient.push({
                        ContactId: $(this).val(),
                        Contact: $(this).text()
                    });
                });

                var salesCallParticipantFromClientIdList = _.map(salesCallParticipantFromClient, function (item) { return parseInt(item.ContactId); });
                var previousClientPerticipentIdList = _.map(previousClientPerticipent, function (item) { return item.ContactId; });
                for (var i = 0; i < previousClientPerticipentIdList.length; i++) {
                    if (jQuery.inArray(previousClientPerticipentIdList[i], salesCallParticipantFromClientIdList) === -1) {
                        deletedClientPerticipent.push(previousClientPerticipent[i]);
                        continue;
                    }
                }

                logBody = $("#txtCallBody").val();
                if (eventDate == "") {
                    toastr.warning("Please Enter the Call Date");
                    isClose = 0;
                    return false;
                }
                else if (eventTime == "") {
                    toastr.warning("Please Enter the Call Time:");
                    isClose = 0;
                    return false;
                }
                else if (salesCallParticipantFromClient.length < 1) {
                    toastr.warning("Please Add Company Contact:");
                    isClose = 0;
                    return false;
                }
                else if (logBody.trim() == "") {
                    toastr.warning("Please Add Call Body:");
                    isClose = 0;
                    return false;
                }
                else if (participantFromOffice.length < 1) {
                    toastr.warning("Please Select Participant From Office");
                    isClose = 0;
                    return false;
                }

                eventDate = CommonHelper.DateFormatToMMDDYYYY(eventDate, '/');

                SalesCallEntry = {
                    Id: id,
                    LogType: logType,
                    MeetingDate: eventDate + " " + eventTime,
                    MeetingLocation: null,
                    ParticipantFromParty: null,
                    MeetingAgenda: null,
                    Decission: null,
                    MeetingAfterAction: null,
                    EmailType: null,
                    CallStatus: callStatus,
                    LogBody: logBody,
                    CompanyId: companyId,
                    DealId: dealId,
                    ContactId: contactId,
                    LogDate: eventDate + " " + eventTime,
                    SocialMediaId: socialMediaId,
                    MeetingType: meetingType,
                    IsAdminUser: isAdminUser,
                    MessagengerId: messagengerId,
                    AccountManagerId: accountManagerId,
                };
            }
            else if ($("#ContentPlaceHolder1_ddlLogType").val() == "Log a message") {

                eventDate = $("#txtCallDate").val();
                eventTime = $("#txtCallTime").val();
                messagengerId = $("#<%=ddlMessageType.ClientID %> option:selected").text();

                callStatus = $("#ContentPlaceHolder1_ddlMessageLogStatus").val();
                var messageType = $("#ContentPlaceHolder1_ddlMessageType").val();

                $("#ContentPlaceHolder1_ddlCallParticipantFromOffice :selected").each(function () {
                    participantFromOffice.push({
                        ContactId: $(this).val(),
                        Contact: $(this).text()
                    });
                });


                $("#ContentPlaceHolder1_ddlCallContacts :selected").each(function () {
                    salesCallParticipantFromClient.push({
                        ContactId: $(this).val(),
                        Contact: $(this).text()
                    });
                });

                var salesCallParticipantFromClientIdList = _.map(salesCallParticipantFromClient, function (item) { return parseInt(item.ContactId); });
                var previousClientPerticipentIdList = _.map(previousClientPerticipent, function (item) { return item.ContactId; });
                for (var i = 0; i < previousClientPerticipentIdList.length; i++) {
                    if (jQuery.inArray(previousClientPerticipentIdList[i], salesCallParticipantFromClientIdList) === -1) {
                        deletedClientPerticipent.push(previousClientPerticipent[i]);
                        continue;
                    }
                }

                logBody = $("#txtCallBody").val();

                if (messageType == 0) {
                    toastr.warning("Please Provide Messenger Id.");
                    isClose = 0;
                    return false;
                }
                if (eventDate == "") {
                    toastr.warning("Please Enter the Message Date");
                    isClose = 0;
                    return false;
                }
                else if (eventTime == "") {
                    toastr.warning("Please Enter the Message Time:");
                    isClose = 0;
                    return false;
                }
                else if (salesCallParticipantFromClient.length < 1) {
                    toastr.warning("Please Add Company Contact.");
                    isClose = 0;
                    return false;
                }
                else if (logBody.trim() == "") {
                    toastr.warning("Please Add Call Body.");
                    isClose = 0;
                    return false;
                }
                else if (participantFromOffice.length < 1) {
                    toastr.warning("Please Select Participant From Office");
                    isClose = 0;
                    return false;
                }

                eventDate = CommonHelper.DateFormatToMMDDYYYY(eventDate, '/');

                SalesCallEntry = {
                    Id: id,
                    LogType: logType,
                    MeetingDate: eventDate + " " + eventTime,
                    MeetingLocation: null,
                    ParticipantFromParty: null,
                    MeetingAgenda: null,
                    Decission: null,
                    MeetingAfterAction: null,
                    EmailType: null,
                    CallStatus: callStatus,
                    LogBody: logBody,
                    CompanyId: companyId,
                    DealId: dealId,
                    ContactId: contactId,
                    LogDate: eventDate + " " + eventTime,
                    SocialMediaId: socialMediaId,
                    MeetingType: meetingType,
                    IsAdminUser: isAdminUser,
                    MessagengerId: messagengerId,
                    AccountManagerId: accountManagerId,
                };
            }
            if (salesCallParticipantFromClient.length < 1) {
                toastr.warning("Please Select Participant From Client");
                isClose = 0;
                return false;
            }
            if (participantFromOffice.length < 1) {
                toastr.warning("Please Select Participant From Office");
                isClose = 0;
                return false;
            }
            //PageMethods.SaveLogEntry(SalesCallEntry, participantFromOffice, salesCallParticipantFromClient, deletedClientPerticipent, OnSaveSalesLogSucceed, OnSaveSalesLogFailed);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../SalesAndMarketing/SalesCallEntry.aspx/SaveLogEntry",
                dataType: "json",
                data: JSON.stringify({ salesCall: SalesCallEntry, participantFromCompany: participantFromOffice, participantFromClient: salesCallParticipantFromClient, deletedClientPerticipent: deletedClientPerticipent, }),
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
        function OnSaveSalesLogSucceed(result) {
            $("#ContentPlaceHolder1_hfSalesCallId").val(result.Data);
            if (isClose == 1) {
                if (result.IsSuccess) {

                    if (typeof parent.ShowAlert === "function") {
                        parent.ShowAlert(result.AlertMessage);
                    }
                    if (typeof parent.GridPaging === "function") {
                        parent.GridPaging(1, 1);
                    }
                    if (typeof parent.CloseDialog === "function" && from == 'MenuLinks') {
                        parent.CloseDialog();
                    }
                    if (typeof parent.DialogCloseAfterUpdate === "function") {
                        parent.DialogCloseAfterUpdate();
                    }
                    PerformClearAction();
                    $(parent.document.getElementById("btnLoadLog")).trigger("click");
                }
                else {
                    CommonHelper.AlertMessage(result.AlertMessage);
                }
            }
            CommonHelper.SpinnerClose();
            return false;
        }
        function OnSaveSalesLogFailed(error) {
            CommonHelper.SpinnerClose();
            toastr.error(error);
        }

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
            var CallContacts = '<%=ddlCallContacts.ClientID%>';
            var emailContacts = '<%=ddlEmailContacts.ClientID%>';
            var clientContacts = '<%=ddlParticipantFromClient.ClientID%>';
            var control = $('#' + CallContacts);
            var control2 = $('#' + emailContacts);
            var control3 = $('#' + clientContacts);
            control.empty();
            control2.empty();
            control3.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.removeAttr("disabled");
                    control2.removeAttr("disabled");
                    control3.removeAttr("disabled");

                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].Id + '">' + list[i].Name + '</option>');
                        control2.append('<option title="' + list[i].Name + '" value="' + list[i].Id + '">' + list[i].Name + '</option>');
                        control3.append('<option title="' + list[i].Name + '" value="' + list[i].Id + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    <%--control.empty().append('<option value="-1">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    control2.empty().append('<option value="-1">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');--%>
                }
            }
            else {
                control.empty().append('<option value="-1">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                control2.empty().append('<option value="-1">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                control3.empty().append('<option value="-1">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
            }
            return false;

        }
        function OnLoadContactFailed(error) {
            toastr.error(error);
            return false;
        }

        function GetLogId(id) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../SalesAndMarketing/SalesCallEntry.aspx/GetLogId",
                dataType: "json",
                data: JSON.stringify({ id: id }),
                async: false,
                success: (data) => {
                    OnGetSuccess(data.d);
                },
                error: (error) => {
                    toastr.error(error, "", { timeOut: 5000 });
                }
            });

            return false;
            //PageMethods.GetLogId(id, OnGetSuccess, OnGetFailed);
            //return false;
        }
        function OnGetSuccess(result) {
            FillForm(result);
            CommonHelper.SpinnerClose();
            return false;
        }

        function OnGetFailed(error) {
            CommonHelper.SpinnerClose();
            toastr.error(error);
        }

        function FillForm(log) {
            debugger;
            $("#ContentPlaceHolder1_hfSalesCallId").val(log.Id);
            $("#ContentPlaceHolder1_ddlLogType").val(log.LogType).trigger('change');
            $("#ContentPlaceHolder1_txtCompany").val(log.CompanyName);
            $("#ContentPlaceHolder1_hfCompanyId").val(log.CompanyId);

            $("#ContentPlaceHolder1_ddlCompanyOwner").val(log.AccountManagerId).trigger('change');
            debugger;
            LoadContactByCompany(log.CompanyId);
            previousClientPerticipent = log.participants;

            if (log.CompanyId == "0") {

                $("#ContentPlaceHolder1_ddlActivityBy").val('Contact Wise').trigger('change');
            }
            if ($("#ContentPlaceHolder1_hfIsAdminUser").val() == "1") {
                $("#LogDateDiv").show();
            }

            $("#txtLogDate").val(moment(log.MeetingDate).format("DD/MM/YYYY"));//GetStringFromDateTime(log.LogDate));
            $("#txtLogTime").val(moment(log.LogDate).format("HH:mm A"));

            if ($("#ContentPlaceHolder1_ddlLogType").val() == "Log a call") {
                $("#CallBodyLabel").text("Call Body");
                $("#MeetingLogArea").hide();
                $("#EmailLogArea").hide();
                $("#CallLogArea").show();
                $("#MeetingTypeDiv").hide();
                $("#MeetingTypeControlDiv").hide();
                $("#MessageTypeDiv").hide();
                $("#MessageTypeControlDiv").hide();
                $("#CallLogStatusDiv").show();
                $("#CallLogStatusControlDiv").show();
                $("#MessageLogStatusDiv").hide();
                $("#MessageLogStatusControlDiv").hide();

                var callContacts = _.map(log.participants, function (item) { return item.ContactId; });

                $("#ContentPlaceHolder1_ddlCallLogStatus").val(log.CallStatus);
                $("#ContentPlaceHolder1_ddlCallContacts").val(callContacts).trigger('change');

                var officePerticipent = _.map(log.OfficeParticipants, function (item) { return item.ContactId; });
                $("#ContentPlaceHolder1_ddlCallParticipantFromOffice").val(officePerticipent).trigger('change');

                $("#txtCallDate").val(moment(log.MeetingDate).format("DD/MM/YYYY"));//GetStringFromDateTime(log.MeetingDate));
                $("#txtCallTime").val(moment(log.MeetingDate).format("HH:mm A"));
                $("#txtCallBody").val(log.LogBody);
            }
            else if ($("#ContentPlaceHolder1_ddlLogType").val() == "Log a message") {
                $("#CallBodyLabel").text("Message Body");
                $("#MeetingLogArea").hide();
                $("#EmailLogArea").hide();
                $("#CallLogArea").show();
                $("#MeetingTypeDiv").hide();
                $("#MeetingTypeControlDiv").hide();
                $("#MessageTypeDiv").show();
                $("#MessageTypeControlDiv").show();
                $("#CallLogStatusDiv").hide();
                $("#CallLogStatusControlDiv").hide();
                $("#MessageLogStatusDiv").show();
                $("#MessageLogStatusControlDiv").show();

                $("#CallBodyLabel").text("Message Body");
                $("#CallDateLabel").text("Message Date");
                $("#CallTimeLabel").text("Message Time");

                $("#txtCallDate").val(moment(log.MeetingDate).format("DD/MM/YYYY"));//GetStringFromDateTime(log.MeetingDate));
                $("#txtCallTime").val(moment(log.MeetingDate).format("HH:mm A"));
                $("#txtCallBody").val(log.LogBody);

                var callContacts = _.map(log.participants, function (item) { return item.ContactId; });
                $("#ContentPlaceHolder1_ddlMessageType").val(log.SocialMediaId);
                $("#ContentPlaceHolder1_ddlMessageLogStatus").val(log.CallStatus);
                $("#ContentPlaceHolder1_ddlCallContacts").val(callContacts).trigger('change');

                var officePerticipent = _.map(log.OfficeParticipants, function (item) { return item.ContactId; });
                $("#ContentPlaceHolder1_ddlCallParticipantFromOffice").val(officePerticipent).trigger('change');

                $("#txtMessageDate").val(moment(log.MeetingDate).format("DD/MM/YYYY"));//GetStringFromDateTime(log.MeetingDate));
                $("#txtMessageTime").val(moment(log.MeetingDate).format("HH:mm A"));
                $("#txtCallBody").val(log.LogBody);
            }
            else if ($("#ContentPlaceHolder1_ddlLogType").val() == "Log an email") {
                $("#MeetingLogArea").hide();
                $("#EmailLogArea").show();
                $("#CallLogArea").hide();
                $("#MeetingTypeDiv").hide();
                $("#MeetingTypeControlDiv").hide();
                $("#MessageTypeDiv").hide();
                $("#MessageTypeControlDiv").hide();
                $("#CallLogStatusDiv").hide();
                $("#CallLogStatusControlDiv").hide();
                $("#MessageLogStatusDiv").hide();
                $("#MessageLogStatusControlDiv").hide();

                var mailContacts = _.map(log.participants, function (item) { return item.ContactId; });
                $("#ContentPlaceHolder1_ddlEmailType").val(log.EmailType);
                $("#ContentPlaceHolder1_ddlEmailContacts").val(mailContacts).trigger('change');

                var officePerticipent = _.map(log.OfficeParticipants, function (item) { return item.ContactId; });
                $("#ContentPlaceHolder1_ddlEmailParticipantFromOffice").val(officePerticipent).trigger('change');

                $("#txtEmailDate").val(moment(log.MeetingDate).format("DD/MM/YYYY"))//GetStringFromDateTime(log.MeetingDate));
                $("#txtEmailTime").val(moment(log.MeetingDate).format("HH:mm A"));
                $("#txtEmailBody").val(log.LogBody);
            }
            else if ($("#ContentPlaceHolder1_ddlLogType").val() == "Log a meeting") {
                $("#MeetingLogArea").show();
                $("#EmailLogArea").hide();
                $("#CallLogArea").hide();
                $("#MeetingTypeDiv").show();
                $("#MeetingTypeControlDiv").show();
                $("#MessageTypeDiv").hide();
                $("#MessageTypeControlDiv").hide();
                $("#CallLogStatusDiv").hide();
                $("#CallLogStatusControlDiv").hide();
                $("#MessageLogStatusDiv").hide();
                $("#MessageLogStatusControlDiv").hide();

                var salesCallParticipantFromClient = new Array();
                var participantFromOffice = new Array();

                salesCallParticipantFromClient = _.map(_.where(log.participants, { PrticipantType: "ClientParticipant" }), function (item) { return item.ContactId; });
                participantFromOffice = _.map(_.where(log.OfficeParticipants), function (item) { return item.ContactId; });

                $("#txtMeetingDate").val(moment(log.MeetingDate).format("DD/MM/YYYY"));//GetStringFromDateTime(log.MeetingDate));
                $("#txtMeetingLogTime").val(moment(log.MeetingDate).format("HH:mm A"));
                $("#txtMeetingLocation").val(log.MeetingLocation);
                $("#txtMeetingAgenda").val(log.MeetingAgenda);
                $("#txtMeetingDiscussion").val(log.Decission);
                $("#txtCallToAction").val(log.MeetingAfterAction);
                $("#txtMeetingDiscussion").val(log.LogBody);
                $("#ContentPlaceHolder1_ddlMeetingType").val(log.MeetingType);

                $("#ContentPlaceHolder1_ddlParticipantFromClient").val(salesCallParticipantFromClient).trigger('change');
                $("#ContentPlaceHolder1_ddltxtParticipantFromOffice").val(participantFromOffice).trigger('change');
                //$("#ContentPlaceHolder1_ddlLogType").val(log.LogType).trigger('change');
            }

            $("#SaveLogEntry").val("Update Log");
            return false;
        }
        function PerformClearAction() {

            var logType = $("#ContentPlaceHolder1_ddlLogType").val();
            $("#form1")[0].reset();
            $("#ContentPlaceHolder1_hfSalesCallId").val('0');
            $("#ContentPlaceHolder1_ddlLogType").val(logType).trigger('change');

            $("#ContentPlaceHolder1_ddlCompanyOwner").val("0").trigger('change');;

            $('#ContentPlaceHolder1_ddltxtParticipantFromOffice').val(null);
            $('#ContentPlaceHolder1_ddltxtParticipantFromOffice').trigger('change');

            $('#ContentPlaceHolder1_ddlParticipantFromClient').val(null);
            $('#ContentPlaceHolder1_ddlParticipantFromClient').trigger('change');

            $('#ContentPlaceHolder1_ddlEmailContacts').val(null);
            $('#ContentPlaceHolder1_ddlEmailContacts').trigger('change');

            $('#ContentPlaceHolder1_ddlCallContacts').val(null);
            $('#ContentPlaceHolder1_ddlCallContacts').trigger('change');

            $('#ContentPlaceHolder1_ddlEmailParticipantFromOffice').val(null).trigger('change');
            $('#ContentPlaceHolder1_ddlCallParticipantFromOffice').val(null).trigger('change');

            $('#ContentPlaceHolder1_ddlActivityBy').val("Company Wise").trigger('change');
            $("#ContentPlaceHolder1_hfContactId").val("0");
            $("#ContentPlaceHolder1_hfCompanyId").val("0");

            $("#SaveLogEntry").val("Save Log");
        }

        function DeleteLog(salesCallId) {
            PageMethods.DeleteLog(salesCallId, OnDeleteSalesLogSucceed, OnSaveSalesLogFailed);
            return false;
        }

        function OnDeleteSalesLogSucceed(result) {

            if (result.IsSuccess) {
                parent.ShowAlert(result.AlertMessage);
                if (typeof parent.GridPaging === "function") {
                    parent.GridPaging(1, 1);
                }
                PerformClearAction();
                $(parent.document.getElementById("btnLoadLog")).trigger("click");
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            CommonHelper.SpinnerClose();
            return false;
        }

        function LoadContactByCompany(companyId) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../SalesAndMarketing/SalesCallEntry.aspx/GetEmployeeByCompanyId",
                dataType: "json",
                data: JSON.stringify({ companyId: companyId }),
                async: false,
                success: (data) => {
                    OnLoadGetEmployeesSucceed(data.d);
                },
                error: (error) => {
                    toastr.error(error, "", { timeOut: 5000 });
                }
            });

            return false;
        }

        function OnLoadGetEmployeesSucceed(results) {

            $("#ContentPlaceHolder1_ddlCallContacts").empty();
            $("#ContentPlaceHolder1_ddlEmailContacts").empty();
            $("#ContentPlaceHolder1_ddlParticipantFromClient").empty();

            var ContactList = _.map(results, function (item) { return item.ContactId; });

            var i = 0, fieldLength = results.length;

            if (fieldLength > 0) {
                for (i = 0; i < fieldLength; i++) {
                    $('<option value="' + results[i].Id + '">' + results[i].Name + '</option>').appendTo('#ContentPlaceHolder1_ddlCallContacts');
                    $('<option value="' + results[i].Id + '">' + results[i].Name + '</option>').appendTo('#ContentPlaceHolder1_ddlEmailContacts');
                    $('<option value="' + results[i].Id + '">' + results[i].Name + '</option>').appendTo('#ContentPlaceHolder1_ddlParticipantFromClient');
                }
            }
            else {
                $("<option value='0'>--No Contact Found--</option>").appendTo("#ContentPlaceHolder1_ddlCallContacts");
                $("<option value='0'>--No Contact Found--</option>").appendTo("#ContentPlaceHolder1_ddlEmailContacts");
                $("<option value='0'>--No Contact Found--</option>").appendTo("#ContentPlaceHolder1_ddlParticipantFromClient");
            }
            if (fieldLength == 1) {
                $("#ContentPlaceHolder1_ddlCallContacts").val($("#ContentPlaceHolder1_ddlCallContacts option:first").val()).trigger('change');
                $("#ContentPlaceHolder1_ddlEmailContacts").val($("#ContentPlaceHolder1_ddlEmailContacts option:first").val()).trigger('change');
                $("#ContentPlaceHolder1_ddlParticipantFromClient").val($("#ContentPlaceHolder1_ddlParticipantFromClient option:first").val()).trigger('change');
            }
            if (ContactList.length > 0) {
                $("#ContentPlaceHolder1_ddlCallContacts").val(ContactList).trigger('change');
                $("#ContentPlaceHolder1_ddlEmailContacts").val(ContactList).trigger('change');
                $("#ContentPlaceHolder1_ddlParticipantFromClient").val(ContactList).trigger('change');
            }
        }

        function OnLoadGetEmployeesFailed(error) {
            toastr.error(error);
        }
        function SaveNCreateTask() {
            isClose = 2;
            SaveNClose();
            if (isClose == 2) {
                var logId = $("#ContentPlaceHolder1_hfSalesCallId").val();
                var companyId = $("#ContentPlaceHolder1_hfCompanyId").val();
                var contactId = $("#ContentPlaceHolder1_hfContactId").val();
                var dealId = $("#ContentPlaceHolder1_hfDealId").val();
                var company = $("#ContentPlaceHolder1_txtCompany").val();
                var contact = $("#ContentPlaceHolder1_txtContact").val();
                var deal = $("#ContentPlaceHolder1_txtDeal").val();
                var iframeid = 'frmPrint';
                var url = "../TaskManagement/AssignTaskIFrame.aspx?lid=" + logId + "&cid=" + companyId + "&cntid=" + contactId + "&did=" + dealId + "&cpny=" + company + "&cntct=" + contact + "&dl=" + deal;
                document.getElementById(iframeid).src = url;
                $("#SalesNoteDialog").dialog({
                    autoOpen: true,
                    modal: true,
                    width: "100%",
                    height: 600,
                    closeOnEscape: false,
                    resizable: false,
                    title: "Create Task",
                    show: 'slide'
                });
            }
            return false;
        }

        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
            return;
        }
        function CloseDialog() {
            $("#SalesNoteDialog").dialog('close');
            if (typeof parent.CloseLog === "function") {
                parent.CloseLog();
            }
            return false;
        }
    </script>
    <asp:HiddenField ID="hfSalesCallId" Value="0" runat="server" />
    <asp:HiddenField ID="CommonDropDownHiddenField" Value="0" runat="server" />
    <asp:HiddenField ID="hfIsAdminUser" Value="0" runat="server" />
    <asp:HiddenField ID="hfCompanyId" Value="0" runat="server" />
    <asp:HiddenField ID="hfContactId" Value="0" runat="server" />
    <asp:HiddenField ID="hfDealId" runat="server" Value="0"></asp:HiddenField>

    <div id="SalesNoteDialog" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>

    <div class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div id="AccountManagerDiv" style="display: none;">
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
                        <div class="col-md-2">
                            <label class="control-label required-field">Account Manager</label>
                        </div>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlCompanyOwner" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div id="CompanyDiv">
                            <div class="col-md-2">
                                <label class="control-label required-field">Company</label>
                            </div>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtCompany" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div id="ContactDiv" style="display: none">
                            <div class="col-md-2">
                                <label class="control-label required-field">Contact</label>
                            </div>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtContact" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label">Deal</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDeal" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div id="ContactDetailsDiv" style="display: none">
                            <div class="col-md-2">
                                <label class="control-label required-field">Contact Details</label>
                            </div>
                            <div class="col-sm-10">
                                <textarea id="txtContactDetails" class="form-control " rows="3" cols="30" readonly="true"></textarea>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Log Type</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlLogType" TabIndex="1" CssClass="form-control" runat="server">
                            <asp:ListItem Text="Log a Call" Value="Log a call"></asp:ListItem>
                            <asp:ListItem Text="Log a Message" Value="Log a message"></asp:ListItem>
                            <asp:ListItem Text="Log an Email" Value="Log an email"></asp:ListItem>
                            <asp:ListItem Text="Log a Meeting" Value="Log a meeting"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2" id="MeetingTypeDiv" style="display: none;">
                        <label class="control-label required-field">Meeting Type</label>
                    </div>
                    <div class="col-md-4" id="MeetingTypeControlDiv" style="display: none;">
                        <asp:DropDownList ID="ddlMeetingType" TabIndex="1" CssClass="form-control" runat="server">
                            <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Online" Value="Online"></asp:ListItem>
                            <asp:ListItem Text="Physical" Value="Physical"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2" id="MessageTypeDiv" style="display: none;">
                        <label class="control-label required-field">Messenger Id</label>
                    </div>
                    <div class="col-md-4" id="MessageTypeControlDiv" style="display: none;">
                        <asp:DropDownList ID="ddlMessageType" TabIndex="1" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" id="LogDateDiv" style="display: none;">
                    <div class="col-md-2">
                        <label class="control-label required-field">Log Date</label>
                    </div>
                    <div class="col-md-4">
                        <input type="text" id="txtLogDate" class="form-control" />
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Log Time</label>
                    </div>
                    <div class="col-md-4">
                        <input type="text" id="txtLogTime" class="form-control" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--Meeting Log--%>
    <div>
        <div class="panel panel-default" id="MeetingLogArea">
            <div class="panel-title"></div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Meeting Date</label>
                        </div>
                        <div class="col-md-4">
                            <input type="text" id="txtMeetingDate" class="form-control" />
                        </div>
                        <div class="col-md-2">
                            <label class="control-label required-field">Meeting Time</label>
                        </div>
                        <div class="col-md-4">
                            <input type="text" id="txtMeetingLogTime" class="form-control" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Meeting Agenda</label>
                        </div>
                        <div class="col-md-4">
                            <textarea id="txtMeetingAgenda" class="form-control" rows="5" cols="30"></textarea>
                        </div>

                        <div class="col-md-2">
                            <label class="control-label required-field">Meeting Location</label>
                        </div>
                        <div class="col-md-4">
                            <textarea id="txtMeetingLocation" class="form-control" rows="5" cols="30"></textarea>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Meeting Discussion</label>
                        </div>
                        <div class="col-md-4">
                            <textarea id="txtMeetingDiscussion" class="form-control" rows="5" cols="30"></textarea>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label required-field">Call To Action</label>
                        </div>
                        <div class="col-md-4">
                            <textarea id="txtCallToAction" class="form-control" rows="5" cols="30"></textarea>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Participant From Office</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddltxtParticipantFromOffice" TabIndex="1" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Participant From Client</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlParticipantFromClient" TabIndex="1" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <%--Email Log--%>
        <div class="panel panel-default" id="EmailLogArea">
            <div class="panel-title"></div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Email Date</label>
                        </div>
                        <div class="col-md-4">
                            <input type="text" id="txtEmailDate" class="form-control" />
                        </div>
                        <div class="col-md-2">
                            <label class="control-label required-field">Email Time</label>
                        </div>
                        <div class="col-md-4">
                            <input type="text" id="txtEmailTime" class="form-control" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Email Type</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlEmailType" TabIndex="1" CssClass="form-control" runat="server">
                                <asp:ListItem Text="Send Mail" Value="Send Mail"></asp:ListItem>
                                <asp:ListItem Text="Receive Mail" Value="Receive Mail"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Participant From Office</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlEmailParticipantFromOffice" TabIndex="1" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Participant From Client</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlEmailContacts" TabIndex="1" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Email Body</label>
                        </div>
                        <div class="col-md-10">
                            <textarea id="txtEmailBody" class="form-control" rows="5" cols="30"></textarea>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <%--Call Log--%>
        <div class="panel panel-default" id="CallLogArea">
            <div class="panel-title"></div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label id="CallDateLabel" class="control-label required-field">Call Date</label>
                        </div>
                        <div class="col-md-4">
                            <input type="text" id="txtCallDate" class="form-control" />
                        </div>
                        <div class="col-md-2">
                            <label id="CallTimeLabel" class="control-label required-field">Call Time</label>
                        </div>
                        <div class="col-md-4">
                            <input type="text" id="txtCallTime" class="form-control" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2" id="CallLogStatusDiv">
                            <label class="control-label">Call Log Status</label>
                        </div>
                        <div class="col-md-10" id="CallLogStatusControlDiv">
                            <asp:DropDownList ID="ddlCallLogStatus" TabIndex="1" CssClass="form-control" runat="server">
                                <asp:ListItem Text="No Answer" Value="No Answer"></asp:ListItem>
                                <asp:ListItem Text="Busy" Value="Busy"></asp:ListItem>
                                <asp:ListItem Text="Wrong Number" Value="Wrong Number"></asp:ListItem>
                                <asp:ListItem Text="Conncected" Value="Conncected"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2" id="MessageLogStatusDiv" style="display: none;">
                            <label class="control-label">Message Log Status</label>
                        </div>
                        <div class="col-md-10" id="MessageLogStatusControlDiv" style="display: none;">
                            <asp:DropDownList ID="ddlMessageLogStatus" TabIndex="1" CssClass="form-control" runat="server">
                                <asp:ListItem Text="No Answer" Value="No Answer"></asp:ListItem>
                                <asp:ListItem Text="Seen" Value="Seen"></asp:ListItem>
                                <asp:ListItem Text="Not Seen" Value="Not Seen"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Participant From Office</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlCallParticipantFromOffice" TabIndex="1" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Participant From Client</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlCallContacts" TabIndex="1" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label id="CallBodyLabel" class="control-label required-field">Call Body</label>
                        </div>
                        <div class="col-md-10">
                            <textarea id="txtCallBody" class="form-control " rows="5" cols="30"></textarea>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <input type="submit" class="TransactionalButton btn btn-primary btn-sm" value="Save And Close" id="SaveLogEntry" onclick="SaveNClose()" />
                <asp:Button ID="btnSaveNContinue" runat="server" Text="Save And Continue" CssClass="TransactionalButton btn btn-primary btn-sm"
                    OnClientClick="javascript: return SaveNContinue();" />
                <asp:Button ID="btnSaveNCreateTask" runat="server" Text="Save And Create Task" CssClass="TransactionalButton btn btn-primary btn-sm"
                    OnClientClick="javascript: return SaveNCreateTask();" />
                <asp:Button ID="btnClean" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                    OnClientClick="javascript: return PerformClearAction();" />
            </div>
        </div>
    </div>
</asp:Content>
