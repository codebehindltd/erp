<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportGuestCompanyInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.Reports.frmReportGuestCompanyInfo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Sales & Marketing</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Guest Company</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            var ddlSignupStatus = '<%=ddlSignupStatus.ClientID%>'
            $('#' + ddlSignupStatus).change(function () {
                var v = $("#<%=ddlSignupStatus.ClientID %>").val();
                if (v == "Affiliated") {
                    $('#AffiliatedDateDiv').show();
                }
                else {
                    $("#<%=txtStartDate.ClientID %>").val('');
                    $("#<%=txtEndDate.ClientID %>").val('');
                    $('#AffiliatedDateDiv').hide();
                }
            });

            $("#ContentPlaceHolder1_txtStartDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtEndDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtEndDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtStartDate").datepicker("option", "maxDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_ddlReferenceId").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
        });
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }
    </script>
    <div class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Company Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblGuestCompany" runat="server" class="control-label" Text="Company"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtGuestCompany" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblContactPerson" runat="server" class="control-label" Text="Contact Person"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblContactNumber" runat="server" class="control-label" Text="Contact Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtContactNumber" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Life Cycle Stage"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlLifeCycleStageId" runat="server" CssClass="form-control"
                            TabIndex="2">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" style="display:none">
                    <div class="col-md-2">
                        <asp:Label ID="lblSignupStatus" runat="server" class="control-label" Text="Signup Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSignupStatus" runat="server" CssClass="form-control"
                            TabIndex="2">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" style="display: none" id="AffiliatedDateDiv">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="Affiliated From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server"></asp:TextBox><input
                            type="hidden" id="hidFromDate" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label" Text="Affiliated To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server"></asp:TextBox><input
                            type="hidden" id="hidToDate" />
                    </div>
                </div>
                <div class="form-group" runat="server" id="GuestReferenceDiv">
                    <div class="col-md-2">
                        <asp:Label ID="lblReferenceId" runat="server" class="control-label" Text="Reference Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReferenceId" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" TabIndex="4" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClick="btnSearch_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="display: none;">
        <asp:Button ID="btnPrintReportFromClient" runat="server" Text="Button" CssClass="btn btn-primary btn-sm" OnClick="btnPrintReportFromClient_Click"
            ClientIDMode="Static" />
    </div>
    <div style="display: none;">
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000; top: 2000;"
            clientidmode="static"></iframe>
    </div>
    <div id="ReportPanel" class="panel panel-default" style="display: none">
        <div class="panel-heading">
            Report:: Company Information
        </div>
        <div class="panel-body">
            <div class="ReporContainerDiv">
                <asp:Panel ID="pnlRoomCalender" runat="server" ScrollBars="Both" Height="800px">
                    <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                        PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                        Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                        WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                    </rsweb:ReportViewer>
                </asp:Panel>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if (CommonHelper.BrowserType().mozilla || CommonHelper.BrowserType().chrome) {

                var barControlId = CommonHelper.GetReportViewerControlId($("#<%=rvTransaction.ClientID %>"));

                var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                var innerTable = '<table title="Print" onclick="PrintDocumentFunc(\'' + barControlId + '\'); return false;" id="ff_print" style="cursor: default;">' + innerTbody + '</table>'
                var outerDiv = '<div style="display: inline-block; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + '</td></tr></tbody></table></div>';

                $("#" + barControlId + " > div").append(outerDiv);
            }
        });

        function PrintDocumentFunc(ss) {
            $('#btnPrintReportFromClient').trigger('click');
            return true;
        }

        var x = '<%=_GuestCompany%>';
        if (x > -1)
            EntryPanelVisibleFalse();

        var x = '<%=isAffDateShow%>';
        if (x > -1) {
            $('#AffiliatedDateDiv').show();
        }
        else {
            $('#AffiliatedDateDiv').hide();
        }
    </script>
</asp:Content>
