<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="LeaveApplication.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.LeaveApplication" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", $('#ContentPlaceHolder1_txtToDate').val());
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", $('#ContentPlaceHolder1_txtFromDate').val());

                    if ($('#ContentPlaceHolder1_txtToDate').val() != '')
                        $('#ContentPlaceHolder1_txtNoOfDays').val(CommonHelper.DateDifferenceInDays($('#ContentPlaceHolder1_txtFromDate').val(), $('#ContentPlaceHolder1_txtToDate').val()) + 1);
                },
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", $('#ContentPlaceHolder1_txtFromDate').val());

                    $('#ContentPlaceHolder1_txtNoOfDays').val(CommonHelper.DateDifferenceInDays($('#ContentPlaceHolder1_txtFromDate').val(), selectedDate) + 1);
                },
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
        });
    </script>
    <div id="EntryPanel" class="panel panel-default" style="display: none">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label runat="server" class="control-label required-field" Text="Leave Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlLeaveTypeId" runat="server" CssClass="form-control"
                            TabIndex="4">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label runat="server" class="control-label required-field" Text="Leave Mode"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlLeaveMode" runat="server" CssClass="form-control"
                            TabIndex="5">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label required-field" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label required-field" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblNoOfDays" runat="server" class="control-label required-field" Text="No Of Days"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtNoOfDays" runat="server" CssClass="form-control" TabIndex="8"
                            ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Application Body"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="multiline" CssClass="form-control"
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
                        <input type="button" value="Save" id="btnSave" class="TransactionalButton btn btn-primary btn-sm" />
                        &nbsp;
                        <input type="button" value="Close" id="btnClose" class="TransactionalButton btn btn-primary btn-sm" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
