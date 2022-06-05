<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CallToActionUserControl.ascx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.UserControl.CallToActionUserControl" %>
<script>
    $(document).ready(function () {
        $("#<%=ddlParticipantFromClient.ClientID%>").select2();
        $("#<%=ddlParticipantFromOffice.ClientID%>").select2();
        $("#<%=ddlAssignToEmployee.ClientID%>").select2();
        $('#txtTime').timepicker({
            showPeriod: is12HourFormat
        });

        $("#txtDate").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: innBoarDateFormat
        });
    });
</script>
<div class="form-horizontal">
    <div class="form-group">
        <div class="col-md-2">
            <label class="control-label">Task Type</label>
        </div>
        <div class="col-md-4">
            <asp:DropDownList ID="ddlType" TabIndex="1" CssClass="form-control" runat="server">
                <asp:ListItem Text="Call" Value="Call"></asp:ListItem>
                <asp:ListItem Text="Message" Value="Message"></asp:ListItem>
                <asp:ListItem Text="Email" Value="Email"></asp:ListItem>
                <asp:ListItem Text="Meeting" Value="Meeting"></asp:ListItem>
                <asp:ListItem Text="To Do" Value="To Do"></asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="form-group" id="DateDiv">
        <div class="col-md-2">
            <label class="control-label required-field">Date</label>
        </div>
        <div class="col-md-4">
            <input type="text" id="txtDate" class="form-control" autocomplete="off"/>
        </div>
        <div class="col-md-2">
            <label class="control-label required-field">Time</label>
        </div>
        <div class="col-md-4">
            <input type="text" id="txtTime" class="form-control" autocomplete="off"/>
        </div>
    </div>
    <div class="form-group">
        <div class="col-md-2">
            <label class="control-label required-field">Assign To</label>
        </div>
        <div class="col-md-10">
            <asp:DropDownList ID="ddlAssignToEmployee" TabIndex="1" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;">
            </asp:DropDownList>
        </div>
    </div>
    <div class="form-group">
        <div class="col-md-2"></div>
        <div class="col-md-8">
            <div class="col-md-12">
                <asp:CheckBox ID="cbRemindSameDay" runat="server" />
                Remaind Same Day
            </div>

        </div>
    </div>
    <div class="form-group">
        <div class="col-md-2"></div>
        <div class="col-md-8">
            <div class="col-md-12">
                <asp:CheckBox ID="cbRemind1DayBefore" runat="server" />
                Remaind 1 day before
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="col-md-2"></div>
        <div class="col-md-8">
            <div class="col-md-12">
                <asp:CheckBox ID="cbRemind3DaysBefore" runat="server" />
                Remaind 3 days before
            </div>

        </div>
    </div>
    <div class="form-group">
        <div class="col-md-2"></div>
        <div class="col-md-8">
            <div class="col-md-12">
                <asp:CheckBox ID="cbRemind1WeekBefore" runat="server" />
                Remaind 1 week before
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="col-md-2">
            <label class="control-label required-field">Participant From Office</label>
        </div>
        <div class="col-md-10">
            <asp:DropDownList ID="ddlParticipantFromOffice" TabIndex="1" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;">
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
    <div class="form-group">
        <div class="col-md-2">
            <label class="control-label">Other Action</label>
        </div>
        <div class="col-md-10">
            <textarea id="txtMeetingDiscussion" class="form-control" rows="5" cols="30"></textarea>
        </div>
    </div>
    <div class="form-group">
        <div class="col-md-2">
            <label class="control-label">Description</label>
        </div>
        <div class="col-md-10">
            <textarea id="txtDescription" class="form-control" rows="5" cols="30"></textarea>
        </div>
    </div>
</div>
