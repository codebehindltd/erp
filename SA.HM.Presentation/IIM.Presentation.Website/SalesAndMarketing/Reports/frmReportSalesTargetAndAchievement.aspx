<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"  CodeBehind="frmReportSalesTargetAndAchievement.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.Reports.frmReportSalesTargetAndAchievement" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var Budget = null;
        var AccountsHead = null;
        var FiscalYear = new Array();

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Accounts</a>";
            var formName = "<span class='divider'>/</span><li class='active'>General Ledger</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#SaveContent").hide();

            $("#ContentPlaceHolder1_ddlFiscalYear").change(function () {
                //var fy = _.findWhere(FiscalYear, { FiscalYearId: parseInt($(this).val()) });
                //toastr.info(fy.FiscalYearName);
                $("#ContentPlaceHolder1_hfFiscalYear").val($("#ContentPlaceHolder1_ddlFiscalYear").val());
                $("#ContentPlaceHolder1_hfFiscalYearName").val($("#<%=ddlFiscalYear.ClientID %> option:selected").text());
            });

            $("#ContentPlaceHolder1_ddlProject").change(function () {
                //var fy = _.findWhere(FiscalYear, { FiscalYearId: parseInt($(this).val()) });
                //toastr.info(fy.FiscalYearName);
                $("#ContentPlaceHolder1_hfProjectId").val($("#ContentPlaceHolder1_ddlProject").val());
            });

        });
          
        function PopulateProjects(control) {
            $("#SaveContent").hide();
            $("#balanceTable").html("");
            let companyId = $(control).val();

            $("#ContentPlaceHolder1_ddlProject").empty().append('<option selected="selected" value="0">Loading...</option>');

            $.ajax({
                type: "POST",
                url: "./frmReportSalesTargetAndAchievement.aspx/GetGLProjectByGLCompanyId",
                data: JSON.stringify({ companyId: companyId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    PopulateControlWithValueNTextField(response.d, $("#ContentPlaceHolder1_ddlProject"), $("#<%=CommonDropDownHiddenField.ClientID %>").val(), "Name", "ProjectId");
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
        }
        function PopulateFiscalYear(control) {
            let projectId = $(control).val();
            $("#SaveContent").hide();
            $("#balanceTable").html("");
            $('#<%=ddlFiscalYear.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');

            $.ajax({
                type: "POST",
                url: "./frmReportSalesTargetAndAchievement.aspx/PopulateFiscalYear",
                data: JSON.stringify({ projectId: projectId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    PopulateControlWithValueNTextField(result.d, $("#<%=ddlFiscalYear.ClientID %>"), $("#<%=CommonDropDownHiddenField.ClientID %>").val(), "FiscalYearName", "FiscalYearId");
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
        }

        //Div Visible True/False-------------------
        function ReportPanelVisible() {
            $('#ReportPanel').show();
        }

        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }
    </script>
    <asp:HiddenField ID="hfProjectId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfFiscalYear" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfFiscalYearName" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Company</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control" TabIndex="2" onchange="PopulateProjects(this)">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Project</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control" TabIndex="3" onchange="PopulateFiscalYear(this)">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFiscalYear" runat="server" class="control-label required-field"
                            Text="Fiscal Year"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control" TabIndex="2">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary btn-sm"
                            OnClick="btnGenarate_Click" />
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
            Report:: Sales Target and Achievement
        </div>
        <div class="panel-body">
            <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" Width="950px" Height="820px">
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

        var xMessage = '<%=isMessageBoxEnable%>';
        if (xMessage > -1) {
            MessagePanelShow();
        }
        else {
            MessagePanelHide();
        }

        var x = '<%=_IsReportPanelVisible%>';
        if (x > -1)
            ReportPanelVisible();
    </script>
</asp:Content>
