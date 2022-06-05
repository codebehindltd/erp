<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmployeeTermination.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmployeeTermination" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithBasicInfo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_txtTerminationDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#ContentPlaceHolder1_txtDecisionDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
        });

        function WorkAfterSearchEmployee() {
            $("#ContentPlaceHolder1_hfEmployeeId").val($("#ContentPlaceHolder1_searchEmployee_hfEmployeeId").val());
        }

        function ValidationBeforeSave() {

            if ($("#ContentPlaceHolder1_searchEmployee_hfEmployeeId").val() == "0") {
                toastr.info("Please Give Employee.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtDecisionDate").val() == "") {
                toastr.info("Please Give Decision Date.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtTerminationDate").val() == "") {
                toastr.info("Please Give Termination Date.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtRemarks").val() == "") {
                toastr.info("Please Give Remarks.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlEmployeeStatus").val() == "0") {
                toastr.info("Please Give Status.");
                return false;
            }

            var employeeStatusId = $("#ContentPlaceHolder1_ddlEmployeeStatus").val();
            var terminationDateFrom = $("#ContentPlaceHolder1_txtTerminationDate").val();
            var decisionDateTo = $("#ContentPlaceHolder1_txtDecisionDate").val();

            var terminationDateFrom = CommonHelper.DateFormatToMMDDYYYY(terminationDateFrom, '/');
            var decisionDateTo = CommonHelper.DateFormatToMMDDYYYY(decisionDateTo, '/');

            var termination = {
                EmpId: $("#ContentPlaceHolder1_searchEmployee_hfEmployeeId").val(),
                DecisionDate: decisionDateTo,
                TerminationDate: terminationDateFrom,
                EmployeeStatusId: employeeStatusId,
                Remarks: $("#ContentPlaceHolder1_txtRemarks").val()
            };

            PageMethods.SaveTermination(termination, OnSaveTerminationSucceeded, OnTerminationFailed);

            return false;
        }

        function OnSaveTerminationSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                ResetForm();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnTerminationFailed(error) {
            toastr.warning("Error On Load. Please Try Again.");
        }

        function ResetForm() {
            $("#form1")[0].reset();
            $("#ContentPlaceHolder1_searchEmployee_hfEmployeeId").val("0");
        }

    </script>

    <asp:HiddenField ID="hfEmployeeId" runat="server" Value="0" />

    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Termination Info
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <UserControl:EmployeeSearch runat="server" ID="searchEmployee" />

                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblDecisionDate" runat="server" class="control-label required-field" Text="Decision Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtDecisionDate" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblTerminationDate" runat="server" class="control-label required-field" Text="Termination Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtTerminationDate" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Employee Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlEmployeeStatus" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Remarks"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" Columns="20" Rows="10" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSaveTermination" runat="server" Text="Save" CssClass="btn btn-primary" OnClientClick="javascript:return ValidationBeforeSave()" />
                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
