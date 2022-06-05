<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="PosSalesComparison.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.Reports.PosSalesComparison" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var ddlCategory = '<%=ddlCategory.ClientID%>'
            var category = $('#' + ddlCategory).val();
            var ddlItem = '<%=ddlItem.ClientID%>'
            ItemClassificationDivShowHide();

            if ($("#ContentPlaceHolder1_hfCostcenterId").val() != "") {
                var costcenter = "", ccId = [], ids = '', idHave = -1;

                ccId = $("#ContentPlaceHolder1_hfCostcenterId").val().split(',');

                $('#TableWiseItemInformation input').each(function () {

                    ids = $(this).attr('value');
                    idHave = _.indexOf(ccId, ids);

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
                $("#ContentPlaceHolder1_hfCostcenterListInfo").val(costcenter);
            }

            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlItem").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlCashierInfoId").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlWaiterInfoId").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlRoomInfoId").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlTableInfoId").select2({
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
            });

            if (category != "0") {
                LoadProductItem(category);
                setTimeout(SetValForItem, 300);
            }
            else {
                var control = $('#' + ddlItem);
                control.removeAttr("disabled");
                control.empty().append('<option value="-1">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
            }

            function SetValForItem() {
                var itemId = $('#ContentPlaceHolder1_txtHiddenItemId').val();
                if (itemId != "") {
                    $('#<%=ddlItem.ClientID%>').val(itemId);
                }
            }

            var txtStartDate = '<%=txtFromDate.ClientID%>'
            var txtEndDate = '<%=txtToDate.ClientID%>'
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
            $('#ContentPlaceHolder1_txtFromDate2').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
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

            $('#' + ddlCategory).change(function () {
                var category = $('#' + ddlCategory).val();
                LoadProductItem(category);
            });

            $("#ContentPlaceHolder1_ddlReportType").change(function () {
                ItemClassificationDivShowHide();
            });

            TableRoomNumberShowHide()
            $("#ContentPlaceHolder1_ddlOutletType").change(function () {
                TableRoomNumberShowHide();
            });

            var txtHiddenItemId = '<%=txtHiddenItemId.ClientID%>'
            $('#' + ddlItem).change(function () {

                $('#' + txtHiddenItemId).val($('#' + ddlItem).val());
                var data = $('#' + ddlItem).select2('data');
                $("#ContentPlaceHolder1_hfItemName").val(data[0].text);
            });

            if ($('#' + txtHiddenItemId).val() > -1) {
                $('#' + ddlItem).val($('#' + txtHiddenItemId).val());
            }

            if ($("#ContentPlaceHolder1_hfIsFoodNBeverageSalesRelatedDataHide").val() == "1") {
                $("#ContentPlaceHolder1_ddlOutletType").parent().parent().hide();
                $("#ContentPlaceHolder1_ddlWaiterInfoId").parent().hide();
                $("#ContentPlaceHolder1_lblWaiter").hide();
                $("#ContentPlaceHolder1_ddlReportType option[value='FNBSales']").remove();
            }
        });

        function ItemClassificationDivShowHide() {
            if ($("#ContentPlaceHolder1_ddlReportType").val() == "FNBSales") {
                $('#InvItemClassificationLabelDiv').show();
                $('#InvItemClassificationControlDiv').show();
            }
            else {
                $('#InvItemClassificationLabelDiv').hide();
                $('#InvItemClassificationControlDiv').hide();
            }
        }

        function TableRoomNumberShowHide() {
            var ddlOutletTypeVal = '<%=ddlOutletType.ClientID%>'
            $("#ContentPlaceHolder1_lblOutletDisplayLabel").text("");
            if ($('#' + ddlOutletTypeVal).val() == "0") {
                $('#RoomInfoDiv').hide();
                $('#TableInfoDiv').hide();
            }
            else if ($('#' + ddlOutletTypeVal).val() == "Table") {
                $("#ContentPlaceHolder1_lblOutletDisplayLabel").text("Table Number");
                $('#RoomInfoDiv').hide();
                $('#TableInfoDiv').show();
            }
            else if ($('#' + ddlOutletTypeVal).val() == "Token") {
                $('#RoomInfoDiv').hide();
                $('#TableInfoDiv').hide();
            }
            else if ($('#' + ddlOutletTypeVal).val() == "Room") {
                $("#ContentPlaceHolder1_lblOutletDisplayLabel").text("Room Number");
                $('#RoomInfoDiv').show();
                $('#TableInfoDiv').hide();
            }
        }
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }
        function LoadProductItem(itemType) {
            PageMethods.GetServiceByCriteria(itemType, OnFillServiceSucceeded, OnFillServiceFailed);
            return false;
        }
        function OnFillServiceSucceeded(result) {
            var list = result;
            var ddlItem = '<%=ddlItem.ClientID%>';
            var control = $('#' + ddlItem);
            control.empty();

            if (list != null) {
                if (list.length > 0) {
                    control.removeAttr("disabled");
                    control.empty().append('<option value="-1">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].ItemId + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.empty().append('<option value="-1">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }
            else {
                control.empty().append('<option value="-1">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
            }
            return false;
        }

        function OnFillServiceFailed(error) {
            alert(error.get_message());
        }
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
    <asp:HiddenField ID="hfIsFoodNBeverageSalesRelatedDataHide" Value="0" runat="server" />
    <asp:HiddenField ID="hfCostcenterId" runat="server" />
    <asp:HiddenField ID="hfCostcenterListInfo" runat="server" />
    <asp:TextBox ID="txtCompanyName" runat="server" Visible="False"></asp:TextBox>
    <asp:TextBox ID="txtCompanyAddress" runat="server" Visible="False"></asp:TextBox>
    <asp:TextBox ID="txtCompanyWeb" runat="server" Visible="False"></asp:TextBox>
    <asp:HiddenField ID="txtHiddenItemId" runat="server" />
    <asp:HiddenField ID="hfItemName" Value="" runat="server" />
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            POS Sales Comparison
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div>
                        <div class="col-md-2">
                            <asp:Label ID="lblReportType" runat="server" class="control-label" Text="Report Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <div class="row">
                                <div class="col-md-10" style="padding-right: 0;">
                                    <asp:DropDownList ID="ddlReportType" CssClass="form-control" runat="server">
                                        <%--<asp:ListItem Value="SalesDetail">Sales Detail</asp:ListItem>
                                        <asp:ListItem Value="DateWiseSalesSummary">Date Wise Sales</asp:ListItem>--%>
                                        <asp:ListItem Value="SalesSummary">Sales Summary</asp:ListItem>
                                        <asp:ListItem Value="FNBSales">Food & Beverage Sales</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-padding-left-none">
                                    <span style="margin-left: 3px;">
                                        <img src="../../Images/ListIcon.png" title='Multi Cost Center Info' style="cursor: pointer;"
                                            onclick='javascript:return MultiCostCenterInfo()' alt='Multi Cost Center Info'
                                            border='0' /></span>
                                </div>
                            </div>

                        </div>
                        <div class="col-md-2" id="InvItemClassificationLabelDiv" style="display: none;">
                            <asp:Label ID="Label1" runat="server" class="control-label" Text="F & B Type"></asp:Label>
                        </div>
                        <div class="col-md-4" id="InvItemClassificationControlDiv" style="display: none;">
                            <asp:DropDownList ID="ddlInvItemClassification" CssClass="form-control" runat="server">
                                <asp:ListItem Value="0">--- ALL ---</asp:ListItem>
                                <asp:ListItem Value="135">Food Sales Info</asp:ListItem>
                                <asp:ListItem Value="136">Beverage Sales Info</asp:ListItem>
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
                                    <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="From Date"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtFromDate2" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            &nbsp;
                            <div class="row">
                                <div class="col-md-4">
                                    <asp:Label ID="Label5" runat="server" class="control-label required-field" Text="To Date"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtToDate2" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <%--<div class="col-md-2">
                        <asp:HiddenField ID="hfCostcenterId" runat="server" />
                        <asp:HiddenField ID="hfCostcenterListInfo" runat="server" />
                        <asp:TextBox ID="txtCompanyName" runat="server" Visible="False"></asp:TextBox>
                        <asp:TextBox ID="txtCompanyAddress" runat="server" Visible="False"></asp:TextBox>
                        <asp:TextBox ID="txtCompanyWeb" runat="server" Visible="False"></asp:TextBox>
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
                    </div>--%>
                </div>
                <div id="CostCenterInformation" class="form-group" style="display: none;">
                    <div class="col-md-2" ">
                        <asp:Label ID="lblCostCenter" runat="server" class="control-label" Text="Cost Center "></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:Label ID="lblSelectedCostcenter" runat="server" class="control-label" Text=""></asp:Label>
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
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblItem" runat="server" class="control-label" Text="Item Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlItem" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" style="display: none">
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label" Text="Cashier Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCashierInfoId" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblWaiter" runat="server" class="control-label" Text="Waiter Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlWaiterInfoId" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label4" runat="server" class="control-label" Text="Outlet Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlOutletType" CssClass="form-control" runat="server">
                            <asp:ListItem Value="0">--- ALL ---</asp:ListItem>
                            <asp:ListItem Value="Table">Table</asp:ListItem>
                            <asp:ListItem Value="Token">Take Away</asp:ListItem>
                            <asp:ListItem Value="Room">Room</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblOutletDisplayLabel" runat="server" class="control-label" Text="Table Number"></asp:Label>
                    </div>
                    <div class="col-md-4" id="RoomInfoDiv" style="display: none;">
                        <asp:DropDownList ID="ddlRoomInfoId" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4" id="TableInfoDiv">
                        <asp:DropDownList ID="ddlTableInfoId" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div>
                        <div class="col-md-2">
                            <asp:Label ID="lblReferenceNo" runat="server" class="control-label" Text="Reference No"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtReferenceNo" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-group" style="display: none;">
                    <div class="col-md-2">
                        <asp:Label ID="lblPaymentType" runat="server" class="control-label" Text="Payment Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlPaymentType" CssClass="form-control" runat="server">
                            <asp:ListItem Value="All">--- ALL ---</asp:ListItem>
                            <asp:ListItem Value="InHouseGuest">In-house Guest</asp:ListItem>
                            <asp:ListItem Value="OutSideGuest">Walk-In Guest</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div style="display: none;">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblFilterBy" runat="server" class="control-label" Text="Sales Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlFilterBy" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblServiceId" runat="server" class="control-label" Text="Service Name"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlServiceId" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
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
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-sm" OnClick="btnSearch_Click"/>
                        <input type="button" id="btnClear" class="btn btn-primary btn-sm" value="Clear" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="ReportPanel" class="panel panel-default" style="display: none">
        <div class="panel-heading">Report:: Sales Information</div>
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
        if ($("#ContentPlaceHolder1_hfCostcenterId").val() != "") {
            $("#CostCenterInformation").show();
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
