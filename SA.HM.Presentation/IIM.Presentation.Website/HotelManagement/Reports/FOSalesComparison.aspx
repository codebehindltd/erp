﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" EnableEventValidation="true" AutoEventWireup="true" CodeBehind="FOSalesComparison.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.Reports.FOSalesComparison" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnClear").click(function () {
                $("#<%=hfServiceIdList.ClientID %>").val("");
                $("#<%=hfServiceNameList.ClientID %>").val("");

                $("#CostCenterInformation").hide();
            });
            if ($("#ContentPlaceHolder1_hfServiceIdList").val() != "") {

                var costcenter = "", ccId = [], ids = '', idHave = -1, n = '', nameList = [];
                var vv = new Array();

                ccId = $("#ContentPlaceHolder1_hfServiceIdList").val().split(',');
                nameList = $("#ContentPlaceHolder1_hfServiceNameList").val().split(',');
                for (var j = 0; j < nameList.length; j++) {
                    vv.push(nameList[j].trim());
                }

                $('#TableWiseItemInformation input').each(function () {
                    idHave = -1;
                    n = $(this).attr('name').trim();
                    if ($.inArray(n, vv) > -1) {
                        idHave = 1;
                    }

                    if (idHave != -1) {
                        $(this).attr('checked', true);

                        if (costcenter != "") {
                            costcenter += ', ' + $(this).attr('name');
                        }
                        else {
                            costcenter += $(this).attr('name');
                        }
                    }
                });
                $("#ContentPlaceHolder1_lblSelectedCostcenter").text(costcenter);
                $("#ContentPlaceHolder1_hfServiceNameList").val(costcenter);
            }

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    //$('#ContentPlaceHolder1_txtFromDate').datepicker("option", "minDate", selectedDate);
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

            $('#ContentPlaceHolder1_txtFromDate2').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    //$('#ContentPlaceHolder1_txtFromDate2').datepicker("option", "minDate", selectedDate);
                    $('#ContentPlaceHolder1_txtToDate2').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtToDate2').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate2').datepicker("option", "maxDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_ddlFilterBy').change(function () {
                var salesType = $('#ContentPlaceHolder1_ddlFilterBy').val();

                if (salesType == "Room") {
                    $("#roomNumDiv").show('slow');
                }
                else {
                    $("#roomNumDiv").hide('slow');
                }
            });

        });

        function GetCheckedService() {
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

            $("#ContentPlaceHolder1_hfServiceIdList").val(costCenterId);
            $("#ContentPlaceHolder1_lblSelectedCostcenter").text(costcenter);
            $("#ContentPlaceHolder1_hfServiceNameList").val(costcenter);
            $("#ServiceSelectContainer").dialog("close");
            $("#CostCenterInformation").show();
        }

        function CloseServiceDialog() {
            $("#ServiceSelectContainer").dialog("close");
        }
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
            $('#ContentPlaceHolder1_ddlFilterBy').val("All");
        }
        function MultiServiceInformation() {
            $("#ServiceSelectContainer").dialog({
                autoOpen: true,
                modal: true,
                width: 450,
                height: 330,
                closeOnEscape: true,
                resizable: false,
                title: "Service Information",
                show: 'slide'
            });
            return false;;
        }
        function CheckValidation() {
            var transectionType = $("#<%=ddlTransactionType.ClientID %>").val();
            var filter = $("#<%=ddlFilterBy.ClientID %>").val();
            var room = $("#<%=ddlRoomInfoId.ClientID%>").val()
            var fDate = $("#<%=txtFromDate.ClientID %>").val();
            var tDate = $("#<%=txtToDate.ClientID %>").val();

            var fDate2 = $("#<%=txtFromDate2.ClientID %>").val();
            var tDate2 = $("#<%=txtToDate2.ClientID %>").val();

            if (fDate == "") {

            }
            return true;
        }
    </script>
    <div id="ServiceSelectContainer" style="display: none;">
        <div style="height: 236px; overflow-y: scroll">
            <asp:Literal ID="ltServiceInformation" runat="server"></asp:Literal>
        </div>

        <div style='margin-top: 12px;'>
            <button type='button' onclick='javascript:return GetCheckedService()' id='btnAddCostcenterId' class='btn btn-primary' style="width: 65px">OK</button>
            <button type='button' onclick='javascript:return CloseServiceDialog()' id='btnCancelCostcenterId' class='btn btn-primary'>Cancel</button>
        </div>
    </div>
    <asp:HiddenField ID="hfServiceIdList" runat="server" />
    <asp:HiddenField ID="hfServiceNameList" runat="server" />
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">

                    <div class="col-md-2">
                        <asp:Label ID="lblFilterBy" runat="server" class="control-label" Text="Sales Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <div class="row">
                            <div class="col-md-11">
                                <asp:DropDownList ID="ddlFilterBy" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-1 col-padding-left-none" style="margin-left: -13px;">
                                <span style="margin-left: 3px;">
                                    <img src="../../Images/ListIcon.png" title='Multi Service Info' style="cursor: pointer;"
                                        onclick='javascript:return MultiServiceInformation()' alt='Multi Service Info'
                                        border='0' /></span>
                            </div>
                        </div>
                    </div>
                    <div style="display: none">
                        <div class="col-md-2">
                            <asp:Label ID="lblGuestType" runat="server" class="control-label" Text="Guest Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlTransactionType" CssClass="form-control" runat="server">
                                <asp:ListItem Value="All">--- All ---</asp:ListItem>
                                <asp:ListItem Value="InHouseGuest">House Guest</asp:ListItem>
                                <asp:ListItem Value="OutSideGuest">Walk-In Guest</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-6">
                        <fieldset>
                            <legend>Duration-1</legend>
                            <div class="row">
                                <div class="col-md-4">
                                    <asp:Label ID="lblFromDate" runat="server" class="control-label required-field" Text="From Date"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            &nbsp;
                            <div class="row">
                                <div class="col-md-4">
                                    <asp:Label ID="lblToDate" runat="server" class="control-label required-field" Text="To Date"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <div class="col-md-6">
                        <fieldset>
                            <legend>Duration-2</legend>
                            <div class="row">
                                <div class="col-md-4">
                                    <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="From Date"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtFromDate2" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            &nbsp;
                            <div class="row">
                                <div class="col-md-4">
                                    <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="To Date"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtToDate2" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div class="form-group" id="roomNumDiv" style="display: none;">
                    <div class="col-md-2">
                        <asp:Label ID="lblOutletDisplayLabel" runat="server" class="control-label" Text="Room Number"></asp:Label>
                    </div>
                    <div class="col-md-4" id="RoomInfoDiv">
                        <asp:DropDownList ID="ddlRoomInfoId" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="CostCenterInformation" class="form-group" style="display: none;">
                    <div class="col-md-2">
                        <asp:Label ID="lblCostCenter" runat="server" class="control-label" Text="Service Name "></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:Label ID="lblSelectedCostcenter" runat="server" class="control-label" Text=""></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-sm"
                            OnClick="btnSearch_Click" OnClientClick="CheckValidation()" />
                        <input type="button" id="btnClear" class="btn btn-primary btn-sm" value="Clear" />
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
    <div id="ReportPanel" class="panel panel-default" style="display: none">
        <div class="panel-heading">
            Report:: Sales Service Information
        </div>
        <div class="panel-body">
            <div class="ReporContainerDiv">
                <asp:Panel ID="pnlRoomCalender" runat="server" ScrollBars="Both" Height="800px">
                    <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
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
        var x = '<%=_RoomStatusInfoByDate%>';
        if (x > -1) {
            EntryPanelVisibleFalse();
            if ($("#ContentPlaceHolder1_hfServiceIdList").val() != "") {
                $("#CostCenterInformation").show();
            }
        }
    </script>
</asp:Content>
