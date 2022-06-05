<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportGuestInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.Reports.frmReportGuestInformation" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Sales & Marketing</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Guest Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_txtFromDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $("#ContentPlaceHolder1_txtToDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
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
                $("#ContentPlaceHolder1_ddlReportTypeSD").attr("disabled", false);
                if (reportType == "GuestInformation") {
                    $('#CompanyDiv').hide();
                    $('#CountryDiv').hide();
                    $('#ReferneceDiv').hide();
                    $("#ContentPlaceHolder1_lblFilterType").hide();
                    $("#ContentPlaceHolder1_ddlFilterType").hide();
                    $('#DateDiv').hide();
                    $("#ContentPlaceHolder1_ddlReportTypeSD").val("Details");
                    $("#ContentPlaceHolder1_ddlReportTypeSD").attr("disabled", true);
                }
                else if (reportType == "CompanyWise") {
                    $('#CompanyDiv').show();
                    $('#CountryDiv').hide();
                    $('#ReferneceDiv').hide();
                    $('#DateDiv').show();
                }
                else if (reportType == "CountryWise") {
                    $('#CompanyDiv').hide();
                    $('#CountryDiv').show();
                    $('#ReferneceDiv').hide();
                    $('#DateDiv').show();
                }
                else if (reportType == "ReferenceWise") {
                    $('#CompanyDiv').hide();
                    $('#CountryDiv').hide();
                    $('#ReferneceDiv').show();
                    $('#DateDiv').show();
                }
                $("#ContentPlaceHolder1_ddlReportTypeSD").trigger('change');
            });

            $("#ContentPlaceHolder1_ddlReportTypeSD").change(function () {
                if ($("#ContentPlaceHolder1_ddlReportTypeSD").val() == "Summary") {
                    $("#ContentPlaceHolder1_lblFilterType").hide();
                    $("#ContentPlaceHolder1_ddlFilterType").hide();
                }
                else {
                    var reportType = $("#<%=ddlReportType.ClientID %>").val();
                    if (reportType == "GuestInformation") {
                        $("#ContentPlaceHolder1_lblFilterType").hide();
                        $("#ContentPlaceHolder1_ddlFilterType").hide();
                    }
                    else {
                        $("#ContentPlaceHolder1_lblFilterType").show();
                        $("#ContentPlaceHolder1_ddlFilterType").show();
                    }
                }
            });
            $("#ContentPlaceHolder1_ddlReportTypeSD").trigger('change');
        });
        function ReportPanelVisibleTrue() {
            $('#ReportPanel').show();
        }
    </script>
    <div id="SearchPanel" class="panel panel-default">
        <div class="row" style="padding-left: 30px">
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblReportType" runat="server" class="control-label" Text="Report Type"></asp:Label>
                        </div>
                        <div class="col-md-2">
                            <asp:DropDownList ID="ddlReportType" CssClass="form-control" runat="server">
                                <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                <asp:ListItem Value="GuestInformation">Guest Information</asp:ListItem>
                                <asp:ListItem Value="CompanyWise">Company Wise</asp:ListItem>
                                <asp:ListItem Value="CountryWise">Country Wise</asp:ListItem>
                                <asp:ListItem Value="ReferenceWise">Reference Wise</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:DropDownList ID="ddlReportTypeSD" CssClass="form-control" runat="server">
                                <asp:ListItem Value="Details">Details</asp:ListItem>
                                <asp:ListItem Value="Summary">Summary</asp:ListItem>
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
                        <div id="CountryDiv" style="display: none">
                            <div class="col-md-2">
                                <asp:Label ID="lblCountry" runat="server" class="control-label" Text="Country Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCountry" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="ReferneceDiv" style="display: none">
                            <div class="col-md-2">
                                <asp:Label ID="lblRefernece" runat="server" class="control-label" Text="Refernece Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRefernece" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-group" id="DateDiv" style="display: none">
                        <div class="col-md-2">
                            <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblFilterType" runat="server" class="control-label" Text="Filter Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlFilterType" CssClass="form-control" runat="server">
                                <%--<asp:ListItem Value="0">--- Please Select ---</asp:ListItem>--%>
                                <asp:ListItem Value="CheckInNCheckOut">CheckIn/ CheckOut</asp:ListItem>
                                <asp:ListItem Value="DateRange">Date Range</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="divGenarate" class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary btn-sm"
                                TabIndex="10" OnClick="btnGenarate_Click" />
                        </div>
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
        <div class="panel-heading">
            Report:: Guest Information
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
    <script type="text/javascript">
        $(document).ready(function () {
            if (CommonHelper.BrowserType().mozilla || CommonHelper.BrowserType().chrome) {

                var barControlId = CommonHelper.GetReportViewerControlId($("#<%=rvTransaction.ClientID %>"));

                var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                var innerTable = '<table title="Print" onclick="PrintDocumentFunc(\'' + barControlId + '\'); return false;" id="ff_print" style="cursor: default;">' + innerTbody + '</table>'
                var outerDiv = '<div style="display: inline-block; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + '</td></tr></tbody></table></div>';

                $("#" + barControlId + " > div").append(outerDiv);
            }

            var reportType = $("#<%=ddlReportType.ClientID %>").val();
            $("#ContentPlaceHolder1_ddlReportTypeSD").attr("disabled", false);
            if (reportType == "GuestInformation") {
                $('#CompanyDiv').hide();
                $('#CountryDiv').hide();
                $('#ReferneceDiv').hide();
                $("#ContentPlaceHolder1_lblFilterType").hide();
                $("#ContentPlaceHolder1_ddlFilterType").hide();
                $('#DateDiv').hide();
                $("#ContentPlaceHolder1_ddlReportTypeSD").val("Details");
                $("#ContentPlaceHolder1_ddlReportTypeSD").attr("disabled", true);
            }
            else if (reportType == "CompanyWise") {
                $('#CompanyDiv').show();
                $('#CountryDiv').hide();
                $('#ReferneceDiv').hide();
                $('#DateDiv').show();
            }
            else if (reportType == "CountryWise") {
                $('#CompanyDiv').hide();
                $('#CountryDiv').show();
                $('#ReferneceDiv').hide();
                $('#DateDiv').show();
            }
            else if (reportType == "ReferenceWise") {
                $('#CompanyDiv').hide();
                $('#CountryDiv').hide();
                $('#ReferneceDiv').show();
                $('#DateDiv').show();
            }
        });

        function PrintDocumentFunc(ss) {
            $('#btnPrintReportFromClient').trigger('click');
            return true;
        }

        var x = '<%=IsSuccess%>';
        if (x > -1)
            ReportPanelVisibleTrue();
    </script>
</asp:Content>
