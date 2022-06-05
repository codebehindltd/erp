<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="CallToActionDetailsEditIFrame.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.CallToActionDetailsEditIFrame" %>

<%@ Register TagPrefix="UserControl" TagName="CallToActionUserControl" Src="~/HMCommon/UserControl/CallToActionUserControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var Id;
        var ReminderDay = new Array();
        $(document).ready(function () {
            Id = CommonHelper.GetParameterByName("id");
            if (Id != "0" && Id != "") {
                LoadCallToActionForEdit(Id);
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
                        async:false,
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
                    $(this).val(ui.item.label).attr("disabled", true);;
                    $("#ContentPlaceHolder1_hfCompanyId").val(ui.item.value);
                    LoadContactByCompanyId(ui.item.value);
                    $("#ContentPlaceHolder1_ddlActivityBy").attr("disabled", true);
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
                        async: false,
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
        });

        function LoadCallToActionForEdit(id) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../SalesAndMarketing/CallToActionDetailsEditIFrame.aspx/GetCallToActionDetailById",
                dataType: "json",
                data: JSON.stringify({ Id: id }),
                async: false,
                success: (data) => {
                    LoadCallToActionDetails(data.d);
                },
                error: (error) => {
                    toastr.error(error, "", { timeOut: 5000 });
                }
            });
            //PageMethods.GetContactInformationByCompanyId(Id, OnLoadContactSucceed, OnLoadContactFailed);
            return false;
        }

        function LoadCallToActionDetails(data) {
            for (var i = 0; i < data.ReminderList.length; i++) {
                ReminderDay.push(data.ReminderList[i]);
            }
            if (data.ReminderList.includes(0))
                $('#ContentPlaceHolder1_CallToActionUserControl_cbRemindSameDay').prop("checked", true);
            if (data.ReminderList.includes(1))
                $('#ContentPlaceHolder1_CallToActionUserControl_cbRemind1DayBefore').prop("checked", true);
            if (data.ReminderList.includes(3))
                $('#ContentPlaceHolder1_CallToActionUserControl_cbRemind3DaysBefore').prop("checked", true);
            if (data.ReminderList.includes(7))
                $('#ContentPlaceHolder1_CallToActionUserControl_cbRemind1WeekBefore').prop("checked", true);
            
            //$("#ContentPlaceHolder1_hfCallToActionId").val();
            $("#txtDate").val(moment(data.Date).format("DD/MM/YYYY"));
            $("#txtTime").val(moment(data.Time).format("hh:mm A"));
            $("#txtMeetingDiscussion").val(data.OtherActivities);
            $("#txtDescription").val(data.Description);
            $("#ContentPlaceHolder1_Id").val(data.Id);
            $("#ContentPlaceHolder1_hfCallToActionId").val(data.CallToActionId);
            $("#ContentPlaceHolder1_txtTaskName").val(data.TaskName);
            $("#ContentPlaceHolder1_txtCompany").val(data.CompanyName);
            $("#ContentPlaceHolder1_hfCompanyId").val(data.CompanyId);
            $("#ContentPlaceHolder1_txtContact").val(data.ComtactName);
            $("#ContentPlaceHolder1_hfContactId").val(data.ContactId);
            if (data.CompanyId == 0 && data.ContactId == 0) {
                $("#divForMenulinks").hide();
            }
            if (data.CompanyId != 0) {
                $("#ContentPlaceHolder1_ddlActivityBy").val("Company Wise").trigger('change');
                LoadContactByCompanyId(data.CompanyId);
            }
            if (data.ContactId != 0) {
                $("#ContentPlaceHolder1_ddlActivityBy").val("Contact Wise").trigger('change');
                LoadContactByCompanyId("0");
            }
            $("#ContentPlaceHolder1_CallToActionUserControl_ddlType").val(data.Type);
            $("#ContentPlaceHolder1_CallToActionUserControl_ddlParticipantFromOffice").val(data.PerticipentFromOfficeList).trigger('change');
            $("#ContentPlaceHolder1_CallToActionUserControl_ddlParticipantFromClient").val(data.PerticipentFromClientList).trigger('change');
            $("#ContentPlaceHolder1_CallToActionUserControl_ddlAssignToEmployee").val(data.TaskAssignedEmployeetList).trigger('change');
            return false;
        }

        function SaveOrUpdateCallToAction() {
            ReminderDayList = $("#ContentPlaceHolder1_hfReminderDaysList").val(ReminderDay).val();
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
        function SaveOrUpdateCallToAction() {
            var id, taskName, type, date, time, time, taskAssignedEmployee, perticipentFromClient, perticipentFromOffice, ReminderDayList, otherAction, companyId, contactId, disscription, callToActionId;
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
            id = $("#ContentPlaceHolder1_Id").val();
            callToActionId = $("#ContentPlaceHolder1_hfCallToActionId").val();

            var CallToActionDetails = {
                Id: id,
                CallToActionId: callToActionId,
                TaskName: taskName,
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
                ReminderDayList: ReminderDayList
            };
            PageMethods.SaveOrUpdateCallToActionDetails(CallToActionDetails, OnSuccessSaveOrUpdate, OnFailSaveOrUpdate);

            return false;
        }
        function OnSuccessSaveOrUpdate(result) {
            parent.ShowAlert(result.AlertMessage);
            PerformClearAction();
            if (typeof parent.CloseDialog === "function") {
                parent.CloseDialog();
            }
            if (typeof parent.GridPaging === "function") {
                parent.GridPaging(1, 1);
            }
        }
        function OnFailSaveOrUpdate(error) {

        }
        function PerformClearAction() {
            ReminderDay = new Array();
            $('#ContentPlaceHolder1_CallToActionUserControl_cbRemindSameDay').prop("checked", false);
            $('#ContentPlaceHolder1_CallToActionUserControl_cbRemind1DayBefore').prop("checked", false);
            $('#ContentPlaceHolder1_CallToActionUserControl_cbRemind3DaysBefore').prop("checked", false);
            $('#ContentPlaceHolder1_CallToActionUserControl_cbRemind1WeekBefore').prop("checked", false);
            $("#ContentPlaceHolder1_CallToActionUserControl_ddlType").val("Call");
            $("#ContentPlaceHolder1_CallToActionUserControl_ddlParticipantFromOffice").val(null).trigger('change');
            $("#ContentPlaceHolder1_CallToActionUserControl_ddlParticipantFromClient").val(null).trigger('change');
            $("#ContentPlaceHolder1_CallToActionUserControl_ddlAssignToEmployee").val(null).trigger('change');
            $("#txtDate").val("");
            $("#txtTime").val("");
            $("#txtMeetingDiscussion").val("");
            $("#txtDescription").val("");
            $("#ContentPlaceHolder1_txtCompany").val("");
            $("#ContentPlaceHolder1_hfCompanyId").val("0");
            $("#ContentPlaceHolder1_txtContact").val("");
            $("#ContentPlaceHolder1_hfContactId").val("0");
            $("#ContentPlaceHolder1_hfId").val("0");
            $("#ContentPlaceHolder1_hfCallToActionId").val("0");

            return false;
        }
    </script>
    <asp:HiddenField ID="hfReminderDaysList" Value="" runat="server" />

    <asp:HiddenField ID="CommonDropDownHiddenField" Value="0" runat="server" />
    <asp:HiddenField ID="hfPerticipentFromClient" Value="" runat="server" />
    <asp:HiddenField ID="hfPerticipentFromOffice" Value="" runat="server" />
    <asp:HiddenField ID="hfTaskAssignedEmployee" Value="" runat="server" />
    <asp:HiddenField ID="hfCallToActionId" Value="0" runat="server" />
    <asp:HiddenField ID="hfCompanyId" Value="0" runat="server" />
    <asp:HiddenField ID="hfContactId" Value="0" runat="server" />
    <asp:HiddenField ID="Id" Value="0" runat="server" />
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
        </div>
        <div class="row" id="ActionButtons" style="padding-bottom: 0; padding-top: 10px;">
            <div class="col-md-12">
                <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="SaveOrUpdateCallToAction()" value="Update Call To Action" />
                <%--<input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="javascript: return PerformClearAction();" />--%>
                <%--<input id="btnSaveNContinue" type="button" value="Save & Continue" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="javascript: return SaveOrUpdateCallToAction(0);" />--%>
            </div>
        </div>
    </div>
</asp:Content>
