<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmDeviceAttendanceSynchronization.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmDeviceAttendanceSynchronization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Attendance & Time Keeping</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Attendance Synchronization</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });
    </script>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Attendance Synchronization
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSynchronizeDate" runat="server" class="control-label required-field" Text="Synchronization Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSynchronizeDate" CssClass="form-control datepicker" runat="server" disabled></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSynchronize" runat="server" Text="Synchronize" CssClass="TransactionalButton btn btn-primary"
                            OnClick="btnSynchronize_Click" TabIndex="4" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
