<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="ItemConsumptionInformationReport.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.Reports.ItemConsumptionInformationReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>

        $(document).ready(function () {

            var txtStartDate = '<%=txtStartDate.ClientID%>'
            var txtEndDate = '<%=txtEndDate.ClientID%>'

            $('#ContentPlaceHolder1_txtStartDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtEndDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtEndDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtStartDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            $("#ContentPlaceHolder1_ddlCostCenter").select2({
                tags: "true",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlEmployeeName").select2({
                tags: "true",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: "true",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlItem").select2({
                tags: "true",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlConsumptionFor").change(function () {
                $(this).find("option:selected").each(function () {
                    var optionValue = $(this).attr("value");
                    if (optionValue == "1") {

                        $("#pnlCostCenter").show();
                        $("#pnlEmployee").hide();

                    } else if (optionValue == "2") {
                        $("#pnlEmployee").show();
                        $("#pnlCostCenter").hide();

                    } else if (optionValue == "0") {
                        $("#pnlEmployee").hide();
                        $("#pnlCostCenter").hide();
                    }
                });
            }).change();

        });
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }
        function btnGenarate_ClientClick() {
           
            if($("#ContentPlaceHolder1_ddlConsumptionFor").val()=="0")
            {
                toastr.warning("Please Select Consumption For");
                return false;
            }
            
        }
    </script>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server" TabIndex="1"></asp:TextBox><input
                            type="hidden" id="hidFromDate" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server" TabIndex="2"></asp:TextBox>
                        <input type="hidden" id="hidToDate" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblConsumptionFor" runat="server" class="control-label required-field" Text="Consumption For:"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlConsumptionFor" runat="server" CssClass="form-control " TabIndex="3">
                            <asp:ListItem Value="0">--- Select ---</asp:ListItem>
                            <asp:ListItem Value="1">CostCenter</asp:ListItem>
                            <asp:ListItem Value="2">Employee</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="pnlCostCenter">

                        <div class="col-md-2">
                            <asp:Label ID="lblCostCenter" runat="server" class="control-label" Text="Cost Center:"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="form-control" TabIndex="4">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="pnlEmployee">

                        <div class="col-md-2">
                            <asp:Label ID="lblEmployeeName" runat="server" class="control-label" Text="Employee Name:"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlEmployeeName" runat="server" CssClass="form-control" TabIndex="4">
                            </asp:DropDownList>

                        </div>

                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category Name:"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" TabIndex="4">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblItem" runat="server" class="control-label" Text="Item Name:"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlItem" runat="server" CssClass="form-control" TabIndex="4">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary btn-sm"
                            OnClick="btnGenarate_Click" OnClientClick="return btnGenarate_ClientClick()"/>
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
    <div id="ReportPanel" class="panel panel-default" style="overflow-x:auto; display:none;" >
        <div class="panel-heading">
            Report:: Cost Center/Employee Wise Consumption
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

        var x = '<%=_RoomStatusInfoByDate%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
</asp:Content>
