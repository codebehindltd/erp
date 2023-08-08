﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportPickupDropForecastRevenueInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.Reports.frmReportPickupDropForecastRevenueInfo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Airport Pickup-Drop Info</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                ignoreReadonly: true,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });
        });
        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }
    </script>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">Search Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="left-float">
                    <div class="l-left">
                        <asp:TextBox ID="txtCompanyName" runat="server" Visible="False"></asp:TextBox>
                        <asp:TextBox ID="txtCompanyAddress" runat="server" Visible="False"></asp:TextBox>
                        <asp:TextBox ID="txtCompanyWeb" runat="server" Visible="False"></asp:TextBox>
                    </div>
                    <div class="r-right">
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="Search Date"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox><input
                            type="hidden" id="hidFromDate" />
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox><input
                            type="hidden" id="hidToDate" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblType" runat="server" class="control-label" Text="Pick Up/ Drop Off"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control"
                            TabIndex="2">
                            <asp:ListItem Value="All">--- ALL ---</asp:ListItem>
                            <asp:ListItem Value="PickUp">Pick Up</asp:ListItem>
                            <asp:ListItem Value="Drop">Drop Off</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblCompany" runat="server" class="control-label" Text="Company"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control"
                            TabIndex="2">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-sm"
                            OnClick="btnSearch_Click" />
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
    <div id="ReportPanel" class="panel panel-default" style="display: none;">
        <div class="panel-heading">Report:: Pick Up/ Drop Off Information</div>
        <div class="panel-body">
            <div class="ReporContainerDiv">
                <asp:Panel ID="pnlReporContainer" runat="server" ScrollBars="Both" Height="700px">
                    <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                        PageCountMode="Actual" SizeToReportContent="true" runat="server" Font-Names="Verdana"
                        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
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

        var x = '<%=_RoomStatusInfoByDate%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
</asp:Content>