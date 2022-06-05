<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="LogActivityEmptyMasterPageReport.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.Reports.LogActivityEmptyMasterPageReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            });

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            });

        });
        
    </script>

    <div class="panel panel-default">
        
        <div class="panel-body" style="display:none">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Log Type</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlLogType" TabIndex="1" CssClass="form-control" runat="server">
                            <asp:ListItem Text="--- All ---" Value=""></asp:ListItem>
                            <asp:ListItem Text="Log a Call" Value="Log a call"></asp:ListItem>
                            <asp:ListItem Text="Log a Message" Value="Log a message"></asp:ListItem>
                            <asp:ListItem Text="Log an Email" Value="Log an email"></asp:ListItem>
                            <asp:ListItem Text="Log a Meeting" Value="Log a meeting"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">From Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">To Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Company</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCompanyForSrc" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Industry</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtIndustry" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2" style="display:none">
                        <label class="control-label">Contact</label>
                    </div>
                    <div class="col-md-4" style="display:none">
                        <asp:TextBox ID="txtContact" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Account Manager</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtAccountManagerSrc" runat="server" CssClass="form-control"></asp:TextBox>
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
            Report:: Log Activity
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
    </div>
</asp:Content>
