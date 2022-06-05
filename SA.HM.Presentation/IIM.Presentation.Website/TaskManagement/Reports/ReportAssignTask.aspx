<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="ReportAssignTask.aspx.cs" Inherits="HotelManagement.Presentation.Website.TaskManagement.Reports.ReportAssignTask" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            CommonHelper.ApplyDecimalValidation();
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            var emp = $("#ContentPlaceHolder1_hfEmpId").val();
            var empArr = [];
            empArr = emp.split(',');
            $("#ContentPlaceHolder1_ddlSearchAssignTo").val(empArr);

            $('#ContentPlaceHolder1_txtSearchFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                // defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchToDate').datepicker("option", "minDate", selectedDate);
                }
            }).datepicker();

            $('#ContentPlaceHolder1_txtSearchToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchFromDate').datepicker("option", "maxDate", selectedDate);
                }
            }).datepicker();
            $("#ContentPlaceHolder1_ddlSearchAssignTo").select2();
        });
        function GenarateClick() {
            var item = $("#ContentPlaceHolder1_ddlSearchAssignTo").val();
            $("#ContentPlaceHolder1_hfEmpId").val(item);          
        }
        function Clear() {
            $("#ContentPlaceHolder1_txtTaskNameForSearch").val("");
            $("#ContentPlaceHolder1_txtSearchFromDate").val("");
            $("#ContentPlaceHolder1_txtSearchToDate").val("");
            $("#ContentPlaceHolder1_ddlSearchAssignTo").val("").trigger('change')
            return false;
        }
    </script>
    <asp:HiddenField ID="hfEmpId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group" id="taskName" runat="server">
                    <div class="col-md-2">
                        <label class="control-label">Task Name</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtTaskNameForSearch" runat="server" CssClass="form-control">                            
                        </asp:TextBox>
                    </div>
                </div>
                <div class="form-group" id="AssignTo" runat="server">
                    <div class="col-md-2">
                        <label class="control-label">Assign To</label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlSearchAssignTo" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;"></asp:DropDownList>
                    </div>
                </div>
                <div id="dvSearchDateTime" class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">From Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchFromDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">To Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchToDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary btn-sm"
                          OnClientClick="javascript: return GenarateClick();"  OnClick="btnGenarate_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return Clear();" />
                    </div>
                </div>
            </div>
        </div>

    </div>
    <div style="display: none;">
        <asp:Button ID="btnPrintReportFromClient" runat="server" Text="Button" OnClick="btnPrintReportFromClient_Click"
            ClientIDMode="Static" />
    </div>
    <div style="display: none;">
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000; top: 2000;"
            clientidmode="static"></iframe>
    </div>
    <div id="ReportPanel" class="panel panel-default" >
        <div class="panel-heading">
            Report:: Task Information
        </div>
        <div class="panel-body">
            <asp:Panel ID="pnlReporContainer" runat="server" ScrollBars="Both" Height="700px">
                <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                </rsweb:ReportViewer>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
