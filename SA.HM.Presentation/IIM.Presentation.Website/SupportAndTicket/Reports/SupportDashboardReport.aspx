<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="true" CodeBehind="SupportDashboardReport.aspx.cs" Inherits="HotelManagement.Presentation.Website.SupportAndTicket.Reports.SupportDashboardReport" %>

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

            $('#ContentPlaceHolder1_txtSearchFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                // defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchToDate');
                }
            }).datepicker();

            $('#ContentPlaceHolder1_txtSearchToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchFromDate');
                }
            }).datepicker();

            var reportType = $('#ContentPlaceHolder1_ddlReportType').val();
            if (reportType == 'Details') {
                $("#divDetails").show();
                $("#divTicketNumber").show();
                $("#ReportFormatLabel").show();
                $("#ReportFormatControl").show();
            }
            else {
                $("#divDetails").hide();
                $("#divTicketNumber").hide();
                $("#ReportFormatLabel").hide();
                $("#ReportFormatControl").hide();
            }

            $('#ContentPlaceHolder1_ddlReportType').change(function () {
                var reportType = $('#ContentPlaceHolder1_ddlReportType').val();
                if (reportType == 'Details') {
                    $("#divDetails").show();
                    $("#divTicketNumber").show();
                    $("#ReportFormatLabel").show();
                    $("#ReportFormatControl").show();
                }
                else {
                    $("#divDetails").hide();
                    $("#divTicketNumber").hide();
                    $("#ReportFormatLabel").hide();
                    $("#ReportFormatControl").hide();
                }
            });
            $("#ContentPlaceHolder1_ddlCase").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%",
            });
            $("#ContentPlaceHolder1_ddlItemCategory").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%",
            });
            $("#ContentPlaceHolder1_ddlSupportCategory").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%",
            });
            $("#ContentPlaceHolder1_ddlSupportType").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%",
            });
            $("#ContentPlaceHolder1_ddlWarrantyType").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%",
            });
            $("#ContentPlaceHolder1_ddlCompany").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%",
            });

            $("#ContentPlaceHolder1_ddlCity").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%",
            });

            $("#ContentPlaceHolder1_txtBillingCountry").blur(function () {
                if ($("#ContentPlaceHolder1_txtBillingCountry").val() == "") {
                    $("#ContentPlaceHolder1_hfBillingCountryId").val(0);
                }
            });

            $("#ContentPlaceHolder1_txtBillingCountry").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../../SalesAndMarketing/Company.aspx/LoadCountryForAutoSearch',
                        data: JSON.stringify({ searchString: request.term }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.CountryName,
                                    value: m.CountryName,
                                    CountryId: m.CountryId
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
                    $("#ContentPlaceHolder1_txtBillingCountry").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfBillingCountryId").val(ui.item.CountryId);
                    $("#ContentPlaceHolder1_txtBillingState").val("");
                    $("#ContentPlaceHolder1_hfBillingStateId").val("0");
                }
            });

            $("#ContentPlaceHolder1_txtBillingState").blur(function () {
                if ($("#ContentPlaceHolder1_txtBillingState").val() == "") {
                    $("#ContentPlaceHolder1_hfBillingStateId").val(0);
                }
            });

            $("#ContentPlaceHolder1_txtBillingState").autocomplete({
                source: function (request, response) {
                    var billingCountry = $("#ContentPlaceHolder1_hfBillingCountryId").val();
                    if (billingCountry == 0) {
                        toastr.warning("Please Select Country");
                        $("#ContentPlaceHolder1_hfBillingCountryId").focus();
                        return false;
                    }
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../../SalesAndMarketing/Company.aspx/LoadStateForAutoSearchByCountry',
                        data: JSON.stringify({ searchString: request.term, CountryId: billingCountry }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.StateName,
                                    value: m.StateName,
                                    Id: m.Id
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
                    $("#ContentPlaceHolder1_txtBillingState").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfBillingStateId").val(ui.item.Id);
                }
            });

            var reportType = $('#ContentPlaceHolder1_ddlReportType').val();
            if (reportType == 'Details') {
                $("#divDetails").show();
                $("#divTicketNumber").show();
                //divTicketNumber
            }
            else {
                $("#divDetails").hide();
                $("#divTicketNumber").hide();
            }
        });

        function GenarateClick() {

        }
    </script>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfBillingCountryId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBillingStateId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBillingCityId" runat="server" Value="0"></asp:HiddenField>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group" runat="server">
                    <div class="col-md-2">
                        <label class="control-label">Report Type</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Summary" Value="Summary"></asp:ListItem>
                            <asp:ListItem Text="Details" Value="Details"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2" id="ReportFormatLabel" style="display:none;">
                        <label class="control-label">Report Format</label>
                    </div>
                    <div class="col-md-4" id="ReportFormatControl" style="display:none;">
                        <asp:DropDownList ID="ddlReportFormat" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Format 01" Value="Format01"></asp:ListItem>
                            <asp:ListItem Text="Format 02" Value="Format02"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="dvSearchDateTime" class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Date</label>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtSearchFromDate" runat="server" CssClass="form-control" autocomplete="off" Placeholder="From"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtSearchToDate" runat="server" CssClass="form-control" autocomplete="off" Placeholder="To"></asp:TextBox>
                    </div>

                    <div id="divTicketNumber">
                        <div class="col-md-2">
                            <label class="control-label">Ticket Number</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtTicketNumber" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div id="divDetails">
                    <div class="form-group" style="display: none;">
                        <div class="col-md-2">
                            <label class="control-label">Item Category</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlItemCategory" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Case</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCase" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label">Support Type</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlSupportType" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Support Category</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlSupportCategory" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label">Warranty Type</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlWarrantyType" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group" style="display: none;">
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Company</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Country</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtBillingCountry" runat="server" CssClass="form-control" TabIndex="1" Placeholder="Country">
                            </asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label">State/ Province/ District</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtBillingState" runat="server" CssClass="form-control" TabIndex="1" Placeholder="State">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Status</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                <asp:ListItem Text="--- ALL ---" Value=""></asp:ListItem>
                                <asp:ListItem Text="Done" Value="Done"></asp:ListItem>
                                <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                                <asp:ListItem Text="Decline" Value="Decline"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label">Amount</label>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtSearchFromAmount" placeholder="From Amount" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtSearchToAmount" placeholder="To Amount" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary btn-sm"
                            OnClientClick="javascript: return GenarateClick();" OnClick="btnGenarate_Click" />
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
    <div id="ReportPanel" class="panel panel-default">
        <div class="panel-heading">
            Report:: Ticket Information
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
