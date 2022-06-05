<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmReportInvItemInfo.aspx.cs"
    Inherits="HotelManagement.Presentation.Website.Inventory.Reports.frmReportInvItemInfo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Sales Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            var txtHiddenItemId = '<%=txtHiddenItemId.ClientID%>'

            var ddlCategory = '<%=ddlCategory.ClientID%>'
            var category = $('#' + ddlCategory).val();
            var ddlItem = '<%=ddlItem.ClientID%>'
            ItemClassificationDivShowHide();

            //LoadProductItem(category);

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
                $("#ContentPlaceHolder1_ddlCostCenterConfig").val("0").trigger('change');
                $('#TableWiseItemInformation input:checked').each(function () {
                    $(this).prop('checked', false);
                });
            });

            function SetValForItem() {
                var itemId = $('#ContentPlaceHolder1_txtHiddenItemId').val();
                if (itemId != "") {
                    $('#<%=ddlItem.ClientID%>').val(itemId);
            }
        }

            //LoadProductItem(0);

            //$('#' + ddlCategory).change(function () {
            //    var category = $('#' + ddlCategory).val();
            //    LoadProductItem(category);
            //});

            $("#ContentPlaceHolder1_ddlReportType").change(function () {
                ItemClassificationDivShowHide();
            });


            $('#' + ddlItem).change(function () {
                $('#' + txtHiddenItemId).val($('#' + ddlItem).val());
            });
            $("#ContentPlaceHolder1_ddlCostCenterConfig").change(function () {
                if (this.value == 1) {
                    $("#dvFilterCostCenter").show();
                }
                else {
                    $("#dvFilterCostCenter").hide();
                    $("#ContentPlaceHolder1_hfCostcenterId").val("");
                    $("#ContentPlaceHolder1_lblSelectedCostcenter").text("");
                    $("#ContentPlaceHolder1_hfCostcenterListInfo").val("");
                    $("#CostCenterInformation").hide();
                    $('#TableWiseItemInformation input:checked').each(function () {
                        $(this).prop('checked', false);
                    });
                }

            });
            $("#ContentPlaceHolder1_ddlCostCenterConfig").trigger('change');
            fillForm();

        });
    function fillForm() {
        if ($("#ContentPlaceHolder1_hfCostcenterListInfo").val() != "") {
            $("#ContentPlaceHolder1_lblSelectedCostcenter").text($("#ContentPlaceHolder1_hfCostcenterListInfo").val());
            $("#CostCenterInformation").show();
            var costCenterId = "";
            costCenterId = $("#ContentPlaceHolder1_hfCostcenterId").val().split(',');

            $('#TableWiseItemInformation input').each(function () {
                var row = $(this);
                if ($.inArray(this.value, costCenterId) !== -1)
                    row.prop('checked', true);
            });
        }
    }
    function PopUpMultiCostCenter() {
        $("#costCenterSelectContainer").dialog({
            autoOpen: true,
            modal: true,
            width: 450,
            height: 330,
            closeOnEscape: true,
            resizable: false,
            title: "Service Information",
            show: 'slide',
        });
        return;
    }
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
                control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                for (i = 0; i < list.length; i++) {
                    control.append('<option title="' + list[i].Name + '" value="' + list[i].ItemId + '">' + list[i].Name + '</option>');
                }

                if ($('#ContentPlaceHolder1_txtHiddenItemId').val() != "") {
                    $('#' + ddlItem).val($('#ContentPlaceHolder1_txtHiddenItemId').val());
                }
            }
            else {
                control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
            }
        }
        else {
            control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
        }

        return false;
    }

    function OnFillServiceFailed(error) {
        alert(error.get_message());
    }

    function GetCheckedCostCenter() {
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
        $("#costCenterSelectContainer").dialog("close");
        if (costCenterId == "")
            $("#CostCenterInformation").hide();
        else
            $("#CostCenterInformation").show();
    }

    function CloseCostcenterDialog() {
        $("#costCenterSelectContainer").dialog("close");
    }
    </script>
    <asp:HiddenField ID="hfCostcenterId" runat="server" />
    <asp:HiddenField ID="hfCostcenterListInfo" runat="server" />
    <div id="costCenterSelectContainer" style="display: none;">
        <div style="height: 236px; overflow-y: scroll">
            <asp:Literal ID="ltCostCenter" runat="server"></asp:Literal>
        </div>
        <div style='margin-top: 12px;'>
            <button type='button' onclick='javascript:return GetCheckedCostCenter()' id='btnAddCostcenter' class='btn btn-primary' style="width:65px">OK</button>
            <button type='button' onclick='javascript:return CloseCostcenterDialog()' id='btnCancelCostcenter' class='btn btn-primary'>Cancel</button>
        </div>
    </div>
    <div style="display: none;">
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000; top: 2000;"
            clientidmode="static"></iframe>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:HiddenField ID="CommonDropDownHiddenField" runat="server" />
                        <asp:TextBox ID="txtCompanyName" runat="server" Visible="False"></asp:TextBox>
                        <asp:TextBox ID="txtCompanyAddress" runat="server" Visible="False"></asp:TextBox>
                        <asp:TextBox ID="txtCompanyWeb" runat="server" Visible="False"></asp:TextBox>
                        <asp:HiddenField ID="txtHiddenItemId" runat="server" Value="" />
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
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblAccFrequency" runat="server" class="control-label" Text="Access Frequancy"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlAdjustmentFrequency" CssClass="form-control" runat="server">
                            <asp:ListItem Text="--- ALL ---" Value=""></asp:ListItem>
                            <asp:ListItem Text="Daily" Value="Daily"></asp:ListItem>
                            <asp:ListItem Text="Weekly" Value="Weekly"></asp:ListItem>
                            <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblClassification" runat="server" class="control-label required-field"
                            Text="Item Classification"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlClassification" runat="server" CssClass="form-control" TabIndex="13">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label runat="server" class="control-label" Text="Cost Center"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCostCenterConfig" CssClass="form-control" runat="server">
                            <asp:ListItem Text="--- ALL ---" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Selected Cost Center" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Without Cost Center" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="dvFilterCostCenter" class="col-md-1 col-padding-left-none" style="margin-left: -13px; display: none">
                        <span style="margin-left: 3px;">
                            <img src="../../Images/ListIcon.png" title='Cost Center List' style="cursor: pointer;"
                                onclick='javascript:return PopUpMultiCostCenter()' alt='Cost Center List'
                                border='0' /></span>
                    </div>
                </div>
                <div id="CostCenterInformation" class="form-group" style="display: none;">
                    <div class="col-md-2">
                        <asp:Label ID="lblCostCenter" runat="server" class="control-label" Text="Selected Cost Center "></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:Label ID="lblSelectedCostcenter" runat="server" class="control-label" Text=""></asp:Label>
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
        <div class="panel-heading">Report:: Item Information</div>
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
        var _IsReportPanelEnable = '<%=_IsReportPanelEnable%>';
        if (_IsReportPanelEnable > -1) {
            $('#ReportPanel').show();
        }
        else {
            $('#ReportPanel').hide();
        }
    </script>
</asp:Content>
