<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmReportBudgetStatement.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.Reports.frmReportBudgetStatement" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
       
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Accounts</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Budget Statement</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });
        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }
        function ValidationProcess() {
            var company = $("#ContentPlaceHolder1_ddlCompany").val();
            var fiscalYear = $("#<%=ddlFiscalYear.ClientID %>").val();
            if (company == "0") {
                toastr.warning('Please Select Company.');
                $("#ContentPlaceHolder1_ddlCompany").focus();
                return false;
            }
            if (fiscalYear == "0") {
                toastr.warning('Please Select Fiscal Year.');
                $("#<%=ddlFiscalYear.ClientID %>").focus();
                return false;
            }            
            $("#ContentPlaceHolder1_hfCompanyId").val(company);           
            $("#ContentPlaceHolder1_hfFiscalYear").val(fiscalYear);
            $("#ContentPlaceHolder1_hfCompanyName").val($("#ContentPlaceHolder1_ddlCompany option:selected").text());
            $("#ContentPlaceHolder1_hfProjectName").val($("#ContentPlaceHolder1_ddlProject option:selected").text());
        }
        function PopulateProjects(control) {
            let companyId = $(control).val();
            $.ajax({
                type: "POST",
                url: "./frmReportBudgetStatement.aspx/GetBudgetType",
                data: JSON.stringify({ glCompanyId: companyId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var budgetType = CommonHelper.SplitPascalCaseToWord(response.d);
                    $("#lblBudgetType").text(budgetType);
                    $("#ContentPlaceHolder1_hfBudgetType").val(budgetType);
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });             
        }
        
    </script>
    <asp:HiddenField ID="hfAccountsHeadId" runat="server" ClientIDMode="Static"></asp:HiddenField>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>

    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBudgetType" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfFiscalYear" runat="server" Value="0"></asp:HiddenField>

    <asp:HiddenField ID="hfCompanyName" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfProjectName" runat="server" Value=""></asp:HiddenField>

    <div class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Company</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control"
                            TabIndex="2" onchange="PopulateProjects(this)">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label required-field"
                            Text="Budget Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:Label ID="lblBudgetType" runat="server" class="form-control" ClientIDMode="Static"
                            Text=""></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFiscalYear" runat="server" class="control-label required-field"
                            Text="Fiscal Year"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control" TabIndex="2">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary btn-sm"
                            OnClick="btnGenarate_Click" OnClientClick="javascript:return ValidationProcess();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="display: none;">
        <asp:Button ID="btnPrintReportFromClient" runat="server" Text="Button" OnClick="btnPrintReportFromClient_Click"
            ClientIDMode="Static" />
    </div>
    <div id="IframeDiv" style="visibility: hidden;">
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000; top: 2000;"
            clientidmode="static"></iframe>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Report::Budget Statement
        </div>
        <div class="panel-body" style="overflow: scroll;">
            <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" ZoomMode="Percent" Width="100%" Height="100%">
            </rsweb:ReportViewer>
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