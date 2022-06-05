<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="ContributionAnalysisReport.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.Reports.ContributionAnalysisReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

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
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });
                        $("#ContentPlaceHolder1_ddlCompany").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlCountry").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlRefernece").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#<%=ddlReportType.ClientID %>").change(function () {
                var reportType = $("#<%=ddlReportType.ClientID %>").val();
                if (reportType == "CompanyWise") {
                    $('#CompanyDiv').show();
                    $('#CountryDiv').hide();
                    $('#ReferneceDiv').hide();
                }
                else if (reportType == "CountryWise") {
                    $('#CompanyDiv').hide();
                    $('#CountryDiv').show();
                    $('#ReferneceDiv').hide();
                }
                else if (reportType == "ReferenceWise") {
                    $('#CompanyDiv').hide();
                    $('#CountryDiv').hide();
                    $('#ReferneceDiv').show();
                }
            });

            $("#ContentPlaceHolder1_ddlReportTypeSD").change(function () {
                if ($("#ContentPlaceHolder1_ddlReportTypeSD").val() == "Summary") {
                    $("#ContentPlaceHolder1_lblFilterType").hide();
                    $("#ContentPlaceHolder1_ddlFilterType").hide();
                }
                else
                {
                    $("#ContentPlaceHolder1_lblFilterType").show();
                    $("#ContentPlaceHolder1_ddlFilterType").show();
                }
            });
        });
    </script>

    <div style="display: none;">
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000; top: 2000;"
            clientidmode="static"></iframe>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Contribution Analysis
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblReportType" runat="server" class="control-label" Text="Report Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlReportType" CssClass="form-control" runat="server">
                                <%--<asp:ListItem Value="0">--- Please Select ---</asp:ListItem>--%>
                                <asp:ListItem Value="CompanyWise">Company Wise</asp:ListItem>
                                <asp:ListItem Value="CountryWise">Country Wise</asp:ListItem>
                                <asp:ListItem Value="ReferenceWise">Reference Wise</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        
                        <div id="CompanyDiv">
                            <div class="col-md-2">
                                <asp:Label ID="lblCompany" runat="server" class="control-label" Text="Company Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCompany" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>                        
                        <div id="CountryDiv" style="display:none">
                            <div class="col-md-2">
                                <asp:Label ID="lblCountry" runat="server" class="control-label" Text="Country Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCountry" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="ReferneceDiv" style="display:none">
                            <div class="col-md-2">
                                <asp:Label ID="lblRefernece" runat="server" class="control-label" Text="Refernece Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRefernece" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label required-field" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox><input
                            type="hidden" id="hidFromDate" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label required-field" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox><input
                            type="hidden" id="hidToDate" />
                    </div>
                </div>
                &nbsp
                <div class="form-group">
                    <div class="col-md-2 col-md-offset-2">
                        <asp:Button ID="btnSearch" runat="server" Text="Generate" CssClass="btn btn-primary btn-sm"
                            OnClick="btnSearch_Click" />
                        <input type="button" id="btnClear" class="btn btn-primary btn-sm" value="Clear" />

                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="ReportPanel" class="panel panel-default" style="display: none">
        <div class="panel-heading">Report:: Contribution Analysis</div>
        <div class="panel-body">
            <div class="ReporContainerDiv">
                <asp:Panel ID="pnlRoomCalender" runat="server" ScrollBars="Both" Height="800px">
                    <rsweb:ReportViewer ShowFindControls="true" ShowWaitControlCancelLink="false" ID="rvTransaction"
                        PageCountMode="Actual" SizeToReportContent="true" runat="server" Font-Names="Verdana"
                        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                        WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                    </rsweb:ReportViewer>
                </asp:Panel>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var _IsReportPanelEnable = '<%=_IsReportPanelEnable%>';
        if (_IsReportPanelEnable > -1) {
            $('#ReportPanel').show();
        }
        else {
            $('#ReportPanel').hide();
        }
    </script>
</asp:Content>
