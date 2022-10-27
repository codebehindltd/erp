<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmReportProductOutInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.Reports.frmReportProductOutInfo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            var txtStartDate = '<%=txtStartDate.ClientID%>'
            var txtEndDate = '<%=txtEndDate.ClientID%>'
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Product Out</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            var txtStartDate = '<%=txtStartDate.ClientID%>'
            var txtEndDate = '<%=txtEndDate.ClientID%>'

            $('#' + txtStartDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtEndDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtEndDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtStartDate).datepicker("option", "maxDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_ddlProductId").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });           
            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlLocationTo").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlLocationFrom").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlCostCenterTo").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlCostCenterFrom").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlCostCenterFrom").change(function () {
                LoadLocationFrom($(this).val());
            });
            $("#ContentPlaceHolder1_ddlCostCenterTo").change(function () {
                LoadLocationTo($(this).val());
            });

            $("#ContentPlaceHolder1_ddlLocationFrom").change(function () {
                $("#ContentPlaceHolder1_hfLocationFromId").val($("#ContentPlaceHolder1_ddlLocationFrom").val())
            });
            $("#ContentPlaceHolder1_ddlLocationTo").change(function () {
                $("#ContentPlaceHolder1_hfLocationToId").val($("#ContentPlaceHolder1_ddlLocationTo").val())
            });
            $("#ContentPlaceHolder1_ddlCategory").change(function () {
                var costCenterId = $("#ContentPlaceHolder1_ddlCostCenterFrom").val();
                var categoryId = $(this).val();
                LoadProductByCategoryNCostcenterId(costCenterId, categoryId);
            });
            $("#ContentPlaceHolder1_ddlCategory").trigger("change");
            $("#ContentPlaceHolder1_ddlCostCenterFrom").trigger("change");

            var itemid = $("#ContentPlaceHolder1_hfItemId").val();
            $('#ContentPlaceHolder1_ddlProductId').val(itemid);
        });
        //Cost center from to adding----
        function LoadProductByCategoryNCostcenterId(costCenterId, categoryId) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './frmReportProductOutInfo.aspx/LoadProductByCategoryNCostcenterId',
                data: "{'costCenterId':'" + costCenterId + "','categoryId':'" + categoryId + "'}",
                dataType: "json",
                async: false,
                success: function (data) {

                    SelectedItem = data.d;
                    var list = data.d;
                    var control = $('#ContentPlaceHolder1_ddlProductId');

                    control.empty();
                    if (list != null) {
                        if (list.length > 0) {

                            control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                            for (i = 0; i < list.length; i++) {
                                control.append('<option title="' + list[i].ItemName + '" value="' + list[i].ItemId + '">' + list[i].ItemName + '</option>');
                            }
                        }
                        else {
                            control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                        }
                    }

                    //control.val($("#ContentPlaceHolder1_hfItemId").val());
                },
                error: function (result) {
                }
            });
        }
        function LoadLocationFrom(costCenetrId) {
            PageMethods.InvLocationByCostCenter(costCenetrId, OnLoadLocationFromSucceeded, OnLoadLocationFailed);
        }
        function OnLoadLocationFromSucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlLocationFrom');

            control.empty();
            if (list != null) {
                if (list.length > 0) {

                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].LocationId + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            control.val($("#ContentPlaceHolder1_hfLocationFromId").val());

            var strLocationFrom = '<%=ddlLocationFrom.ClientID%>'
            var hfddlLocationFromId = '<%=hfLocationFromId.ClientID%>'
            if ($('#' + hfddlLocationFromId).val() > 0) {
                $('#' + strLocationFrom).val($('#' + hfddlLocationFromId).val());
            }

            return false;
        }

        function LoadLocationTo(costCenetrId) {
            PageMethods.InvLocationByCostCenter(costCenetrId, OnLoadLocationToSucceeded, OnLoadLocationFailed);
        }
        function OnLoadLocationToSucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlLocationTo');

            control.empty();
            if (list != null) {
                if (list.length > 0) {

                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].LocationId + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }
            control.val($("#ContentPlaceHolder1_hfLocationToId").val());

            var strLocationTo = '<%=ddlLocationTo.ClientID%>'
            var hfddlLocationToId = '<%=hfLocationToId.ClientID%>'
            if ($('#' + hfddlLocationToId).val() > 0) {
                $('#' + strLocationTo).val($('#' + hfddlLocationToId).val());
            }
            return false;
        }
        function OnLoadLocationFailed() { }
        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }

        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }
        function GenarateClick() {
            var item = $("#ContentPlaceHolder1_ddlProductId").val();
            //var location = $("#ContentPlaceHolder1_ddlLocation").val();
            $("#ContentPlaceHolder1_hfItemId").val(item);
            $("#ContentPlaceHolder1_hfItemName").val($("#ContentPlaceHolder1_ddlProductId option:selected").text());
        }
    </script>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfLocationFromId" runat="server" Value="0" />
    <asp:HiddenField ID="hfLocationToId" runat="server" Value="0" />
    <asp:HiddenField ID="hfItemId" runat="server" Value="0" />
    <asp:HiddenField ID="hfItemName" runat="server" Value="" />

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
                        <asp:Label ID="Label1" runat="server" class="control-label"
                            Text="Report Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control" TabIndex="1">
                            <asp:ListItem Value="DateWise" Text="Date Wise"></asp:ListItem>
                            <asp:ListItem Value="CostCenterWise" Text="Cost Center Wise"></asp:ListItem>
                            <asp:ListItem Value="ItemWise" Text="Item Wise"></asp:ListItem>
                            <asp:ListItem Value="CategoryWise" Text="Category Wise"></asp:ListItem>
                            <asp:ListItem Value="TransferNumberWise" Text="Transfer Number Wise"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblReportFormat" runat="server" class="control-label"
                            Text="Report Format"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportFormat" runat="server" CssClass="form-control" TabIndex="1">
                            <asp:ListItem Value="ItemWiseSummary" Text="Item Wise Summary"></asp:ListItem>
                            <asp:ListItem Value="Summary" Text="Summary"></asp:ListItem>                            
                            <asp:ListItem Value="Details" Text="Details"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox><input
                            type="hidden" id="hidFromDate" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date" ></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
                        <input type="hidden" id="hidToDate" />
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblCostCenterId" runat="server" class="control-label"
                            Text="From Cost Center"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCostCenterFrom" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label" Text="To Cost Center"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCostCenterTo" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label13" runat="server" class="control-label" Text="From Location"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlLocationFrom" runat="server" CssClass="form-control" TabIndex="5">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label14" runat="server" class="control-label" Text="To Location"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlLocationTo" runat="server" CssClass="form-control" TabIndex="5">
                        </asp:DropDownList>
                    </div>
                </div>
                <%--<div class="form-group" id="CostCenterToDiv">
                    
                </div>
                <div class="form-group" id="InternalOutLocationTo">
                    
                </div>--%>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" TabIndex="6">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblBundleName" runat="server" class="control-label" Text="Item"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlProductId" runat="server" CssClass="form-control" TabIndex="6">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary btn-sm"
                            OnClientClick="javascript: return GenarateClick();" OnClick="btnGenarate_Click" />
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
            Report:: Item Transfer
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
    <script type="text/javascript">
        $(document).ready(function () {
            if (CommonHelper.BrowserType().mozilla || CommonHelper.BrowserType().chrome) {

                var barControlId = CommonHelper.GetReportViewerControlId($("#<%=rvTransaction.ClientID %>"));

                var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                var innerTable = '<table title="Print" onclick="PrintDocumentFunc(\'' + barControlId + '\'); return false;" id="ff_print" style="cursor: default;">' + innerTbody + '</table>'
                var outerDiv = '<div style="display: inline-block; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + '</td></tr></tbody></table></div>';

                $("#" + barControlId + " > div").append(outerDiv);
            }
            LoadLocationFrom($("#ContentPlaceHolder1_ddlCostCenterFrom").val());
            LoadLocationTo($("#ContentPlaceHolder1_ddlCostCenterTo").val());
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

        var x = '<%=_RoomStatusInfoByDate%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
</asp:Content>
