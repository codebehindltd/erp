<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="ReportHKLostNFound.aspx.cs" Inherits="HotelManagement.Presentation.Website.HouseKeeping.Reports.ReportHKLostNFound" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var payrollIntegrated = "0";
        $(document).ready(function () {
            $("#ContentPlaceHolder1_ddlFoundPersonSrc").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#<%=txtFromDate.ClientID %>").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $("#<%=txtToDate.ClientID %>").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            payrollIntegrated = $("#<%=hfIsPayrollIntegrateWithFrontOffice.ClientID %>").val();
            if (payrollIntegrated == "1") {
                //// employeeLoadDivSrc employeeTxtDivSrc
                $("#employeeLoadDivSrc").show("slow");

                $("#employeeTxtDivSrc").hide("slow");
            }
            else {
                $("#employeeLoadDivSrc").hide("slow");

                $("#employeeTxtDivSrc").show("slow");
            }
        });
        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }
    </script>
    <asp:HiddenField ID="hfIsPayrollIntegrateWithFrontOffice" runat="server" Value="0"></asp:HiddenField>
    <div id="InfoPanel" class="panel panel-default">
        <div class="panel-heading">
            Lost & Found Report
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label runat="server" class="control-label " Text="Item Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtItemNameSrc" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label runat="server" class="control-label" Text="Where It Found"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTransectionTypeSrc" runat="server" CssClass="form-control" TabIndex="1">
                            <asp:ListItem Text="-- Please Select --" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Lobby" Value="Lobby"></asp:ListItem>
                            <asp:ListItem Text="Room" Value="Room"></asp:ListItem>
                            <asp:ListItem Text="Restaurant" Value="Restaurant"></asp:ListItem>
                            <asp:ListItem Text="Banquet" Value="Banquet"></asp:ListItem>
                            <asp:ListItem Text="Others" Value="Others"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="divRoomRestSrc">
                        <div>
                            <asp:Label ID="lblTransectionIdSrc" runat="server" class="control-label col-md-2"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlHasReturned" runat="server" CssClass="form-control">
                                <asp:ListItem Text="-- Please Select --" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Returned" Value="Returned"></asp:ListItem>
                                <asp:ListItem Text="Not Returned" Value="NotReturned"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblShortName" runat="server" class="control-label" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">

                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label" Text="Item Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlItemTypeSrc" runat="server" CssClass="form-control" TabIndex="1">
                            <asp:ListItem Text="-- Please Select --" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Valuable" Value="Valuable"></asp:ListItem>
                            <asp:ListItem Text="Non-Valuable" Value="NonValuable"></asp:ListItem>
                            <asp:ListItem Text="Perishable" Value="Perishable"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label" Text="Who Found It"></asp:Label>
                    </div>
                    <div class="col-md-10" id="employeeLoadDivSrc" style="display: none">
                        <asp:DropDownList ID="ddlFoundPersonSrc" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-10" id="employeeTxtDivSrc">
                        <asp:TextBox ID="txtFoundPersonSrc" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                    </div>
                </div>
                &nbsp;
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                            CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />
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
            Report:: Lost & Found
            Information
        </div>
        <div class="panel-body">
            <rsweb:ReportViewer ID="rvTransaction" runat="server" ShowFindControls="false" ShowWaitControlCancelLink="false"
                PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" Font-Names="Verdana"
                Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
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
