<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmReportMarketSegmentWiseInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.Reports.frmReportMarketSegmentWiseInformation" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script>
        $(document).ready(function () {

            if (CommonHelper.BrowserType().mozilla || CommonHelper.BrowserType().chrome) {

                var barControlId = CommonHelper.GetReportViewerControlId($("#<%=rvTransaction.ClientID %>"));

                var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                var innerTable = '<table title="Print" onclick="PrintDocumentFunc(\'' + barControlId + '\'); return false;" id="ff_print" style="cursor: default;">' + innerTbody + '</table>'
                var outerDiv = '<div style="display: inline-block; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + '</td></tr></tbody></table></div>';

                $("#" + barControlId + " > div").append(outerDiv);
            }

            function PrintDocumentFunc(ss) {
                $('#btnPrintReportFromClient').trigger('click');
                return true;
            }
            $("#ContentPlaceHolder1_txtFromMonth").datepicker({
                changeMonth: true,
                changeYear: false,
                dateFormat: 'MM',
                showButtonPanel: true,
                onClose: function (dateText, inst) {
                    $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
                }

            });

            $("#ContentPlaceHolder1_txtToMonth").datepicker({
                changeMonth: true,
                changeYear: false,
                dateFormat: 'MM',
                showButtonPanel: true,
                onClose: function (dateText, inst) {

                    $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
                }
            });

            $("#ContentPlaceHolder1_txtSearchDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });


            $("#ContentPlaceHolder1_ddlSalesPersonWiseFilterBy").change(function () {
                //if ($(this).val() == "SalesPerson")
                //    $("#dvSalespersonFilter").show();
                //else
                //    $("#dvSalespersonFilter").hide();
                //if ($(this).val() == "MTM")
                //    $("#dvMonthWiseFilter").show();
                //else
                //    $("#dvMonthWiseFilter").hide();

            });
            
            $("#ContentPlaceHolder1_ddlReportType").change(function () {

                if ($(this).val() == "2") {                    ;
                    $("#dvMTDYTD").show();
                    $("#dvMonthWiseFilter").hide();
                    //$("#dvSalespersonFilterBy").hide();
                    $("#dvMonthWiseFilterBy").hide();
                    $("#dvSalespersonFilter").hide();
                    $("#dvYear").hide();
                    
                }
                else if ($(this).val() == "1")
                {
                    $("#dvMonthWiseFilterBy").show();
                    $("#dvMonthWiseFilter").show();
                    $("#dvMTDYTD").hide();
                    //$("#dvSalespersonFilterBy").hide();
                    $("#dvSalespersonFilter").hide();
                }
                else if ($(this).val() == "3")
                {
                    //$("#dvSalespersonFilterBy").show();
                    $("#dvSalespersonFilter").show();
                    //$("#dvMonthWiseFilter").hide();
                    $("#dvMonthWiseFilterBy").hide();
                    $("#dvMTDYTD").hide();
                    $("#ContentPlaceHolder1_ddlSalesPersonWiseFilterBy").trigger('change');
                }                    
            });

            $("#ContentPlaceHolder1_ddlReportType").trigger('change');           
        });

    </script>
    <%-- <style>
    .ui-datepicker-calendar {
        display: none;
    }
    </style>--%>
    <div class=" panel panel-default">
        <div class="panel-heading">
            Search Report
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label>Report Type</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="1">Month Wise</asp:ListItem>
                            <asp:ListItem Value="2">DTD , MTD and YTD</asp:ListItem>
                            <asp:ListItem Value="3">Sales Person Wise</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="dvMonthWiseFilterBy" class="form-group">
                    <div class="col-md-2">
                        <label>Filter By</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlMonthWiseFilterBy" runat="server" CssClass="form-control">
                            <asp:ListItem Value="Pax">Pax</asp:ListItem>
                            <asp:ListItem Value="Nights">Room Nights</asp:ListItem>
                            <asp:ListItem Value="ARR">Average Room Rate</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <%--<div id="dvSalespersonFilterBy" class="form-group">
                    <div class="col-md-2">
                        <label>Filter By</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSalesPersonWiseFilterBy" runat="server" CssClass="form-control">
                            <asp:ListItem Value="Year">Year</asp:ListItem>
                            <asp:ListItem Value="MTM">Month To Month</asp:ListItem>
                            <asp:ListItem Value="SalesPerson">Sales Person Wise</asp:ListItem>
                        </asp:DropDownList>
                    </div>                    
                </div>--%>
                <div id="dvMTDYTD" class=" form-group">
                    <div class="col-md-2">
                        <label>Type</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlMtdTydType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="DTD">Date To Date</asp:ListItem>
                            <asp:ListItem Value="MTD">Month To Date</asp:ListItem>
                            <asp:ListItem Value="YTD">Year To Date</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label>Search Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchDate" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div id="dvYear" class="form-group">
                    <div class="col-md-2">
                        <label>Year</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div id="dvMonthWiseFilter" class="form-group">
                    <div class="col-md-2">
                        <label>From Month</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromMonth" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label>To Month</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToMonth" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                
                <div id="dvSalespersonFilter" class="form-group">
                    <div class="col-md-2">
                        <label>Refference By</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRefferenceBy" runat="server" CssClass="form-control">                            
                        </asp:DropDownList>
                    </div>
                </div>
                
                <div class="form-group">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnGenerateReport" Text="Generate" OnClick="btnGenerateReport_Click" runat="server" CssClass="btn btn-primary btn-sm" />
                            <input type="button" id="btnClear" value="Clear" class="btn btn-primary btn-sm" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="columnRight">
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
    <div id="ReportPanel" class="panel panel-default">
        <div class="panel-heading">
            Report:: Market Segment
        </div>
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

</asp:Content>
