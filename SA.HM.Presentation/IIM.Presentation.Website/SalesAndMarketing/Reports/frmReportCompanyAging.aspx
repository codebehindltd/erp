<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportCompanyAging.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.Reports.frmReportCompanyAging" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Accounts</a>";
            var formName = "<span class='divider'>/</span><li class='active'>General Ledger</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#ContentPlaceHolder1_hfCompanyName").val() != "") {
                $("#txtSearch").val($("#ContentPlaceHolder1_hfCompanyName").val());
            }

            $('#ContentPlaceHolder1_txtAsOfDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtEndDateTo').datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtIntervalBands").keypress(function (e) {
                //if the letter is not digit then display error and don't type anything
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    //display error message
                    toastr.warning("Digits Only");
                    return false;
                }
            });

            $("#txtSearch").autocomplete({

                source: function (request, response) {

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: 'frmReportCompanyLedger.aspx/GetCompanyData',
                        data: "{'searchText':'" + request.term + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.CompanyName,
                                    value: m.CompanyId
                                };
                            });
                            response(searchData);
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                },
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);

                    $("#ContentPlaceHolder1_hfCompanyName").val(ui.item.label);
                    $("#ContentPlaceHolder1_hfCompanyId").val(ui.item.value);
                }
            });

            if ($("#ContentPlaceHolder1_ddlReportFor").val() == "1") {
                $("#companysearch").show();
            }
            else if ($("#ContentPlaceHolder1_ddlReportFor").val() == "0") {
                $("#companysearch").hide();
            }

            $("#ContentPlaceHolder1_ddlReportFor").change(function () {
                if ($("#ContentPlaceHolder1_ddlReportFor").val() == "1") {
                    $("#companysearch").show();
                }
                else if ($("#ContentPlaceHolder1_ddlReportFor").val() == "0") {
                    $("#companysearch").hide();
                }
            });

            $("#ContentPlaceHolder1_txtAsOfDate").blur(function () {
                var date = $("#ContentPlaceHolder1_txtAsOfDate").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#ContentPlaceHolder1_txtAsOfDate").focus();
                        $("#ContentPlaceHolder1_txtAsOfDate").val(DayOpenDate);
                        return false;
                    }
                }

            });
        });
        function ValidationSearch() {
            reportType = $("#ContentPlaceHolder1_ddlReportFor").val();
            supplier = $("#txtSearch").val();
            if (reportType == "1") {
                if (supplier == "") {
                    toastr.warning("Please select Report For");
                    $("#txtSearch").focus();
                    return false;
                }
            }
        }
    </script>
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="4"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyName" runat="server"></asp:HiddenField>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label" Text="Report For"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportFor" runat="server" CssClass="form-control">
                            <asp:ListItem Text="All Company" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Individual" Value="1"></asp:ListItem>                            
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label" Text="Report Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Top Sheet" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Summary Sheet" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Detail Sheet" Value="3"></asp:ListItem>                            
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" id="companysearch">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Company"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <input type="text" id="txtSearch" class="form-control" tabindex="6" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblAsOfDate" runat="server" class="control-label" Text="As Of Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtAsOfDate" CssClass="form-control" runat="server" TabIndex="7"></asp:TextBox><input
                            type="hidden" id="hidFromDate" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label4" runat="server" class="control-label" Text="Interval"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtIntervalBands" CssClass="form-control" runat="server" TabIndex="7"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlIntervalType" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Day" Value="Day"></asp:ListItem>
                            <%--<asp:ListItem Text="Month" Value="Month"></asp:ListItem>
                            <asp:ListItem Text="Year" Value="Year"></asp:ListItem>--%>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12" id="divGenerate">
                        <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary btn-sm"
                            TabIndex="10" OnClientClick="javascript: return ValidationSearch();" OnClick="btnGenarate_Click" />
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
    <div id="ReportPanel" class="panel panel-default">
        <div class="panel-heading">
            Report:: Company A/R Aging
        </div>
        <div class="panel-body">
            <asp:Panel ID="pnlReporContainer" runat="server" ScrollBars="Both" Height="700px">
                <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                    PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                    Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Height="820px">
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
        });

        function PrintDocumentFunc(ss) {
            $('#btnPrintReportFromClient').trigger('click');
            return true;
        }

    </script>
</asp:Content>
