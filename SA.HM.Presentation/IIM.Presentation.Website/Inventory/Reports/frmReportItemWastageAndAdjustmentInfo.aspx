<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmReportItemWastageAndAdjustmentInfo.aspx.cs"
    Inherits="HotelManagement.Presentation.Website.Inventory.Reports.frmReportItemWastageAndAdjustmentInfo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Sales Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            var ddlCategory = '<%=ddlCategory.ClientID%>'
            var category = $('#' + ddlCategory).val();
            var ddlItem = '<%=ddlItem.ClientID%>'

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

            $("#btnClear").click(function () {
                $("#ContentPlaceHolder1_hfCostcenterId").val("");
                $("#ContentPlaceHolder1_lblSelectedCostcenter").text("");
                $("#ContentPlaceHolder1_hfCostcenterListInfo").val("");
                $("#CostCenterInformation").hide();
            });

<%--            if (category != "0") {
                LoadProductItem(category);
                setTimeout(SetValForItem, 300);
            }
            else {
                var control = $('#' + ddlItem);
                control.removeAttr("disabled");
                control.empty().append('<option value="-1">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
            }--%>

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

            var ddlReportType = '<%=ddlReportType.ClientID%>'
            if ($('#' + ddlReportType).val() == "Wastage") {
                $("#InvTransactionMode").show();
            }
            else {
                $("#InvTransactionMode").hide();
            }
            $('#' + ddlReportType).change(function () {
                if ($('#' + ddlReportType).val() == "Wastage") {
                    $("#InvTransactionMode").show();
                }
                else {
                    $("#InvTransactionMode").hide();
                }
            });

            //$('#' + ddlCategory).change(function () {
            //    var category = $('#' + ddlCategory).val();
            //    LoadProductItem(category);
            //});

            var txtHiddenItemId = '<%=txtHiddenItemId.ClientID%>'
            $('#' + ddlItem).change(function () {

                $('#' + txtHiddenItemId).val($('#' + ddlItem).val());
            });

            if ($('#' + txtHiddenItemId).val() > -1) {
                $('#' + ddlItem).val($('#' + txtHiddenItemId).val());
            }
        });

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

    </script>
    <div id="CostcenterSelectContainer" style="display: none;">
        <asp:Literal ID="ltCostcenter" runat="server"></asp:Literal>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblReportType" runat="server" class="control-label required-field"
                            Text="Report Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" CssClass="form-control" runat="server">
                            <asp:ListItem Value="Adjustment">Adjustment</asp:ListItem>
                            <asp:ListItem Value="Wastage">Wastage</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="InvTransactionMode">
                        <div class="col-md-2">
                            <asp:Label ID="Label1" runat="server" class="control-label" Text="Transaction Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlInvTransactionMode" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
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
                        <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div id="LocationWise">
                        <div class="col-md-2">
                            <asp:Label ID="lblLocation" runat="server" class="control-label" Text="Location"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:HiddenField ID="CommonDropDownHiddenField" runat="server" />
                        <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlCategory" CssClass="form-control" runat="server"
                            OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true">
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
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-sm"
                            OnClick="btnSearch_Click" />
                        <input type="button" id="btnClear" class="btn btn-primary btn-sm" value="Clear" />
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
        <div class="panel-heading">
            Report:: Sales Information
        </div>
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
