<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="LateAttendenceApplication.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.LateAttendenceApplication" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ContentPlaceHolder1_txtTime').timepicker({
                showPeriod: is12HourFormat
            });
            $('#ContentPlaceHolder1_txtDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", $('#ContentPlaceHolder1_txtToDate').val());
                },
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
        })
    </script>
    <div id="lateAttendanceDiv" class="panel panel-default">
        <div class="panel-heading">
            <label id="lblHeading">Late Attendence Application </label>
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label required-field" Text="Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label required-field" Text="Time"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtTime" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" id="description">
                    <div class="col-md-2">
                        <asp:Label ID="Label8" runat="server" class="control-label" Text="Description"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="multiline" CssClass="form-control"
                            TabIndex="8">
                        </asp:TextBox>
                    </div>
                </div>
                <div class="form-group" id="statusDiv" style="display: none">
                    <div class="col-md-2">
                        <asp:Label ID="Label7" runat="server" class="control-label" Text="Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"
                            TabIndex="5">
                            <asp:ListItem Value="0">-- Please Select --</asp:ListItem>
                            <asp:ListItem Value="Check">Check</asp:ListItem>
                            <asp:ListItem Value="Approve">Approve</asp:ListItem>
                            <asp:ListItem Value="Cancel">Cancel</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" id="cancelDiv" style="display: none">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Cancel Reason"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtCancelReason" runat="server" TextMode="multiline" CssClass="form-control"
                            TabIndex="8">
                        </asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <input type="button"  value="Save" id="btnSave" class="TransactionalButton btn btn-primary btn-sm" />
                        &nbsp;
                        <input type="button" value="Cancel" id="btnCancel" class="TransactionalButton btn btn-primary btn-sm" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
