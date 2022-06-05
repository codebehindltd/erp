<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" enableEventValidation="false" AutoEventWireup="true" CodeBehind="BillDetailsReport.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.Reports.BillDetailsReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            
            $("#ContentPlaceHolder1_txtFromDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtToDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtToDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtFromDate").datepicker("option", "maxDate", selectedDate);
                }

            });

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

            var ddlCategory = '<%=ddlCategory.ClientID%>'
            var category = $("#ContentPlaceHolder1_ddlCategory").val();
            var ddlItem = '<%=ddlItem.ClientID%>'
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

            $('#' + ddlCategory).change(function () {
                var category = $('#' + ddlCategory).val();
                LoadProductItem(category);
            });

            var txtHiddenItemId = '<%=txtHiddenItemId.ClientID%>'
            $('#' + ddlItem).change(function () {

                $('#' + txtHiddenItemId).val($('#' + ddlItem).val());
            });

            if ($('#' + txtHiddenItemId).val() > -1) {
                $('#' + ddlItem).val($('#' + txtHiddenItemId).val());
            }
        });
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

        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }
    </script>

     <div style="display: none;">
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000; top: 2000;"
            clientidmode="static"></iframe>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <asp:HiddenField ID="CommonDropDownHiddenField" runat="server" />
        <asp:HiddenField ID="txtHiddenItemId" runat="server" />
        <div class="panel-heading">
            Bill Details
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label required-field" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox><input
                            type="hidden" id="hidFromDate" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label required-field" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox><input
                            type="hidden" id="hidToDate" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblCostCenter" runat="server" class="control-label required-field" Text="Cost Center"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="form-control" TabIndex="3">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group"> 
                    <div class="col-md-2">
                        <%--<asp:HiddenField ID="CommonDropDownHiddenField" runat="server" />--%>
                        <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCategory" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblItem" runat="server" class="control-label" Text="Item Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlItem" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label" Text="Cashier Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCashierInfoId" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblPaymentMode" runat="server" class="control-label" Text="Payment Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlPaymentMode" CssClass="form-control" runat="server">
                            <asp:ListItem Value="All">--- All ---</asp:ListItem>
                            <asp:ListItem Value="Cash">Cash</asp:ListItem>
                            <asp:ListItem Value="Card">Card</asp:ListItem>
                            <asp:ListItem Value="Company">Company</asp:ListItem>
                            <asp:ListItem Value="OtherRoom">Guest Room</asp:ListItem>
                            <asp:ListItem Value="Employee">Employee</asp:ListItem>
                            <asp:ListItem Value="Refund">Refund</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div>
                        <div class="col-md-2">
                            <asp:Label ID="lblBillNo" runat="server" class="control-label" Text="Bill No"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtBillNo" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                 &nbsp
                <div class="form-group">
                    <div class="col-md-2 col-md-offset-2">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-sm"
                            OnClick="btnSearch_Click" />
                    <input type="button" id="btnClear" class="btn btn-primary btn-sm" value="Clear" />

                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="ReportPanel" class="panel panel-default" style="display: none">
        <div class="panel-heading">Report:: Bill Details</div>
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

