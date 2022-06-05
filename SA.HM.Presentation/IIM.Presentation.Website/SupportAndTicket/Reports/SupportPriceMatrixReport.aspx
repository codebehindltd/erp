<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="SupportPriceMatrixReport.aspx.cs" Inherits="HotelManagement.Presentation.Website.SupportAndTicket.Reports.SupportPriceMatrixReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            CommonHelper.ApplyDecimalValidation();
            $("#ContentPlaceHolder1_txtCompanyName").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '/SupportAndTicket/Reports/SupportPriceMatrixReport.aspx/GetCompanyByAutoSearch',
                        data: JSON.stringify({ searchString: request.term }),
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.CompanyName,
                                    value: m.CompanyName,
                                    id: m.CompanyId
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
                    $("#ContentPlaceHolder1_hfCompanyId").val(ui.item.id);
                }
            });
            $("#ContentPlaceHolder1_txtCategoryName").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '/SupportAndTicket/Reports/SupportPriceMatrixReport.aspx/GetCategoryByAutoSearch',
                        data: JSON.stringify({ searchString: request.term }),
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.Name,
                                    id: m.CategoryId
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
                    $("#ContentPlaceHolder1_hfCategoryId").val(ui.item.id);
                }
            });
            $("#ContentPlaceHolder1_txtItemName").autocomplete({
                source: function (request, response) {
                    //debugger;
                    var categoryId = $("#ContentPlaceHolder1_hfCategoryId").val();
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '/SupportAndTicket/Reports/SupportPriceMatrixReport.aspx/GetItemByAutoSearch',
                        data: JSON.stringify({ searchString: request.term, categoryId: categoryId }),
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.Name,
                                    id: m.ItemId
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
                    $("#ContentPlaceHolder1_hfItemId").val(ui.item.id);
                }
            });
        });
    </script>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
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
                        <label class="control-label">Company Name</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control">                            
                        </asp:TextBox>
                    </div>
                    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0"></asp:HiddenField>
                </div>
                <div class="form-group" runat="server">
                    <div class="col-md-2">
                        <label class="control-label">Category</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control">                            
                        </asp:TextBox>
                    </div>
                    <asp:HiddenField ID="hfCategoryId" runat="server" Value="0"></asp:HiddenField>
                </div>
                <div class="form-group" runat="server">
                    <div class="col-md-2">
                        <label class="control-label">Item</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtItemName" runat="server" CssClass="form-control">                            
                        </asp:TextBox>
                    </div>
                    <asp:HiddenField ID="hfItemId" runat="server" Value="0"></asp:HiddenField>
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
            Report:: Price Matrix
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
