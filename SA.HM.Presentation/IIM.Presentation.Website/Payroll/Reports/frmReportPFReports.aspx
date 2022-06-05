<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportPFReports.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmReportPFReports" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Provident Fund</a>";
            var formName = "<span class='divider'>/</span><li class='active'>PF Reports</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            var ddlReportType = '<%=ddlReportType.ClientID%>'
            var reportType = $("#<%=ddlReportType.ClientID %>").val();
                if (reportType == "0") {
                    $("#StaffListReport").show();
                    $("#SummaryReport").hide();
                    $("#ProductCalculationReport").hide();
                    $("#InterestCalculation").hide();
                }
                else if (reportType == "1") {
                    $("#StaffListReport").hide();
                    $("#SummaryReport").show();
                    $("#ProductCalculationReport").hide();
                    $("#InterestCalculation").hide();
                }
                else if (reportType == "2") {
                    $("#StaffListReport").hide();
                    $("#SummaryReport").hide();
                    $("#ProductCalculationReport").show();
                    $("#InterestCalculation").hide();
                }
                else if (reportType == "3") {
                    $("#StaffListReport").hide();
                    $("#SummaryReport").hide();
                    $("#ProductCalculationReport").hide();
                    $("#InterestCalculation").show();
                }

            $('#' + ddlReportType).change(function () {
                var reportType = $("#<%=ddlReportType.ClientID %>").val();
                if (reportType == "0") {
                    $("#StaffListReport").show();
                    $("#SummaryReport").hide();
                    $("#ProductCalculationReport").hide();
                    $("#InterestCalculation").hide();
                }
                else if (reportType == "1") {
                    $("#StaffListReport").hide();
                    $("#SummaryReport").show();
                    $("#ProductCalculationReport").hide();
                    $("#InterestCalculation").hide();
                }
                else if (reportType == "2") {
                    $("#StaffListReport").hide();
                    $("#SummaryReport").hide();
                    $("#ProductCalculationReport").show();
                    $("#InterestCalculation").hide();
                }
                else if (reportType == "3") {
                    $("#StaffListReport").hide();
                    $("#SummaryReport").hide();
                    $("#ProductCalculationReport").hide();
                    $("#InterestCalculation").show();
                }
            });
        });
        function CheckEligibility() {
            var type = $("#ContentPlaceHolder1_ddlReportType").val();
            var month = $("#ContentPlaceHolder1_ddlMonth").val(); //0
            var year = $("#ContentPlaceHolder1_ddlYear").val();//0
            var summaryYear = $("#ContentPlaceHolder1_ddlSummaryYear").val(); //1
            var productYear = $("#ContentPlaceHolder1_ddlProductYear").val(); //2
            var interestYear = $("#ContentPlaceHolder1_ddlInterestYear").val(); //3
            
            if (type == "0") {
                if (month == "0") {
                    toastr.warning("Please select a month.");
                    $('#ReportPanel').hide();
                    return false;
                }
                else if (year == "0") {
                    toastr.warning("Please select a year.");
                    $('#ReportPanel').hide();
                    return false;
                }
            }
            else if (type == "1") {
                if (summaryYear == "0") {
                    toastr.warning("Please select a year.");
                    $('#ReportPanel').hide();
                    return false;
                }
            }
            else if (type == "2") {
                if (productYear == "0") {
                    toastr.warning("Please select a year.");
                    $('#ReportPanel').hide();
                    return false;
                }
            }
            else if (type == "3") {
                if (interestYear == "0") {
                    toastr.warning("Please select a year.");
                    $('#ReportPanel').hide();
                    return false;
                }
            }
            return true;
        }
        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }        
    </script>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <div id="SearchPanel" class="panel panel-default">       
        <div class="panel-heading">PF Reports</div>
        <div class="panel-body">
            <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblReportType" runat="server" class="control-label" Text="Report Type"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control" TabIndex="4">
                        <asp:ListItem Value="0">Monthly Provident Fund List</asp:ListItem>
                        <asp:ListItem Value="1">Provident Fund Summary</asp:ListItem>
                        <asp:ListItem Value="2">PF Closing Balance</asp:ListItem>
                        <asp:ListItem Value="3">Interest Calculation</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>           
            <div id="StaffListReport">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblMonth" runat="server" class="control-label required-field" Text="Month"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control"
                            TabIndex="2">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblYear" runat="server" class="control-label required-field" Text="Year"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>           
            <div id="SummaryReport" style="display: none">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSummaryYear" runat="server" class="control-label required-field" Text="Year"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSummaryYear" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>          
            <div id="ProductCalculationReport" style="display: none">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblproductYear" runat="server" class="control-label required-field" Text="Year"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlProductYear" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>           
            <div id="InterestCalculation" style="display: none">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblInterestYear" runat="server" class="control-label required-field" Text="Year"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlInterestYear" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblInterest" runat="server" class="control-label" Text="Total Interest Amount"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtInterest" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                    </div>
                </div>
            </div>           
            <div class="row">
 <div class="col-md-12">
                <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="btn btn-primary"
                    OnClick="btnGenerate_Click" OnClientClick="javascript:return CheckEligibility()" />
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
        <div class="panel-heading">PF Reports</div>
        <div class="panel-body">
            <rsweb:ReportViewer ID="rvTransaction" runat="server" ShowFindControls="false" ShowWaitControlCancelLink="false"
                PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" Font-Names="Verdana"
                Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" Width="830px" Height="820px">
            </rsweb:ReportViewer>
            <%--<rsweb:ReportViewer ID="rvTransaction" runat="server" ShowFindControls="false" ShowWaitControlCancelLink="false"
                Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" Width="100%" Height="820px">
            </rsweb:ReportViewer>--%>
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
        var x = '<%=dispalyReport%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
</asp:Content>
