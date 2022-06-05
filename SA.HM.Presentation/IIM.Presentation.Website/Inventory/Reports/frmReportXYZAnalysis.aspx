<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmReportXYZAnalysis.aspx.cs"
    Inherits="HotelManagement.Presentation.Website.Inventory.Reports.frmReportXYZAnalysis" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            
            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#btnClear").click(function () {
                $("#ContentPlaceHolder1_hfCostcenterId").val("");
                $("#ContentPlaceHolder1_lblSelectedCostcenter").text("");
                $("#ContentPlaceHolder1_hfCostcenterListInfo").val("");
                $("#CostCenterInformation").hide();
                $("#ContentPlaceHolder1_ddlCostCenterConfig").val("0").trigger('change');
                $('#TableWiseItemInformation input:checked').each(function () {
                    $(this).prop('checked', false);
                });
            });

            var txtStartDate = '<%=txtFromDate.ClientID%>';
            var txtEndDate = '<%=txtToDate.ClientID%>';
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
        });

        function MultiCostCenterInfo() {
            $("#CostcenterSelectContainer").dialog({
                autoOpen: true,
                modal: true,
                width: 450,
                height: 330,
                closeOnEscape: true,
                resizable: false,
                title: "Cost Center Information",
                show: 'slide'
            });
            return;
        }

        function GetCheckedCostcenter() {
            var costCenterId = "", costcenter = "";
            $('#TableWiseItemInformation input:checked').each(function () {
                if (costCenterId != "") {
                    costCenterId += ',' + $(this).attr('value');
                    costcenter += ', ' + $(this).attr('name');
                }
                else {
                    costCenterId += $(this).attr('value');
                    costcenter += $(this).attr('name');
                }
            });
            console.log(costCenterId);
            $("#ContentPlaceHolder1_hfCostcenterId").val(costCenterId);
            $("#ContentPlaceHolder1_lblSelectedCostcenter").text(costcenter);
            $("#ContentPlaceHolder1_hfCostcenterListInfo").val(costcenter);
            $("#CostcenterSelectContainer").dialog("close");
            $("#CostCenterInformation").show();
        }

        function CloseCostcenterDialog() {
            $("#CostcenterSelectContainer").dialog("close");
        }

    </script>
    <div id="CostcenterSelectContainer" style="display: none;">
        <div style="height: 236px; overflow-y: scroll">
            <asp:Literal ID="ltCostcenter" runat="server"></asp:Literal>
        </div>
        <div style='margin-top: 12px;'>
            <button type='button' onclick='javascript:return GetCheckedCostcenter()' id='btnAddCostcenterId' class='btn btn-primary' style="width: 65px">OK</button>
            <button type='button' onclick='javascript:return CloseCostcenterDialog()' id='btnCancelCostcenterId' class='btn btn-primary'>Cancel</button>
        </div>
    </div>
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="4"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyName" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsFoodNBeverageSalesRelatedDataHide" Value="0" runat="server" />
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            XYZ Analysis
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:HiddenField ID="hfCostcenterId" runat="server" />
                        <asp:HiddenField ID="hfCostcenterListInfo" runat="server" />
                        <asp:HiddenField ID="txtHiddenItemId" runat="server" />
                        <asp:Label ID="lblFromDate" runat="server" class="control-label required-field" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label required-field" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <div class="row">
                            <div class="col-md-10" style="padding-right: 0;">
                                <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2 col-padding-left-none">
                                <span style="margin-left: 3px;">
                                    <img src="../../Images/ListIcon.png" title='Multi Cost Center Info' style="cursor: pointer;"
                                        onclick='javascript:return MultiCostCenterInfo()' alt='Multi Cost Center Info'
                                        border='0' /></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:HiddenField ID="CommonDropDownHiddenField" runat="server" />
                        <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlCategory" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary"
                            OnClick="btnGenarate_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="columnRight">
        </div>
    </div>
    <div id="ReportPanel" class="panel panel-default" style="display: none">
        <div class="panel-heading">Report:: XYZ Analysis</div>
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
        var _IsReportPanelEnable = '<%=_IsReportPanelEnable%>';
        if (_IsReportPanelEnable > -1) {
            $('#ReportPanel').show();
        }
        else {
            $('#ReportPanel').hide();
        }
    </script>
</asp:Content>
