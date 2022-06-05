<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="PayrollEmployeeStatus.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.PayrollEmployeeType" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithBasicInfo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>

        $(document).ready(function () {
            $("#ContentPlaceHolder1_txtActionDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                //minDate: minCheckInDate
            });
            $("#ContentPlaceHolder1_txtEffectiveDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                //minDate: minCheckInDate
            });
        });
        function SaveEmployeeType() {

            var _empId = "0", _emStatusId = "0", _actionDate = "", _effectiveDate = "", _reason = "";
            _empId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
            _emStatusId = $("#ContentPlaceHolder1_ddlEmployeeStatus").val();
            _actionDate = $("#ContentPlaceHolder1_txtActionDate").val();
            _actionDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(_actionDate, innBoarDateFormat);
            _effectiveDate = $("#ContentPlaceHolder1_txtEffectiveDate").val();
            _effectiveDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(_effectiveDate, innBoarDateFormat);
            _reason = $("#ContentPlaceHolder1_txtReason").val();

            if (_empId == "0") {
                toastr.info("Please Fill Employee Code and Press Search Button");
                return false;
            }
            else if (_emStatusId == "0") {
                toastr.info("Please Select Employee Type");
                return false;
            }
            else if (_actionDate == "undefined//undefined") {
                toastr.info("Please Set Action Date");
                return false;
            }
            else if (_effectiveDate == "undefined//undefined") {
                toastr.info("Please Set Effective Date");
                return false;
            }
            else if (_reason == "") {
                toastr.info("Please Write Down the Reason");
                return false;
            }
            var PayrollEmpStatusHistory = {
                EmpId: _empId,
                EmpStatusId: _emStatusId,
                ActionDate: _actionDate,
                EffectiveDate: _effectiveDate,
                Reason: _reason
            }

            PageMethods.SaveEmployeeStatus(PayrollEmpStatusHistory, OnSavePayrollEmpTypeSucceed, OnSavePayrollEmpTypeHistoryFailed);
            return false;
        }
        function OnSavePayrollEmpTypeSucceed(result) {
            $("#<%=btnSave.ClientID %>").val("Save");
            CommonHelper.AlertMessage(result.AlertMessage);
            ClearAction();
        }
        function OnSavePayrollEmpTypeHistoryFailed() {

        }
        function ClearAction() {

            $("#ContentPlaceHolder1_ddlEmployeeStatus").val('0');
            $("#ContentPlaceHolder1_txtActionDate").val('');
            $("#ContentPlaceHolder1_txtEffectiveDate").val('');
            $("#ContentPlaceHolder1_txtReason").val('');
            return false;
        }
        function WorkAfterSearchEmployee() {
            var employeeId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
        }

    </script>
    <div>
        <div id="EmployeeInfo" class="panel panel-default">
            <div class="panel-heading">
                Employee Information
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <UserControl:EmployeeSearch ID="employeeSearch" runat="server" />
                </div>
            </div>

            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblEmployeeType" runat="server" class="control-label required-field" Text="Employee Type:"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlEmployeeStatus" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblReason" runat="server" class="control-label required-field" Text="Reason"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblActionDate" runat="server" class="control-label required-field"
                                Text="Action Date"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtActionDate" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblEffectiveDate" runat="server" class="control-label required-field"
                                Text="Effective Date:"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtEffectiveDate" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnSave" runat="server" TabIndex="4" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript:return SaveEmployeeType()" />
                            <asp:Button ID="btnClear" runat="server" TabIndex="5" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript: return ClearAction()" />
                        </div>
                    </div>
                </div>
            </div>
            </>
        </div>
    </div>
</asp:Content>
