<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmReportBanquetSalesInfo.aspx.cs"
    Inherits="HotelManagement.Presentation.Website.Banquet.Reports.frmReportBanquetSalesInfo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Banquet Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Sales Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            var ddlCategory = '<%=ddlCategory.ClientID%>'
            var category = $('#' + ddlCategory).val();
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

            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlItem").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            function SetValForItem() {
                var itemId = $('#ContentPlaceHolder1_txtHiddenItemId').val();
                if (itemId != "") {
                    $('#<%=ddlItem.ClientID%>').val(itemId);
                }
            }

            var txtStartDate = '<%=txtFromDate.ClientID%>'
            var txtEndDate = '<%=txtToDate.ClientID%>'
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

            $('#' + ddlCategory).change(function () {
                var category = $('#' + ddlCategory).val();
                //var txtHiddenItemId = '<%=txtHiddenItemId.ClientID%>'
                //$('#' + txtHiddenItemId).val("");
                //                $('#' + txtItemUnit).val('');
                //                $('#' + txtUnitPrice).val('');
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
                    //control.removeAttr("disabled");
                    control.empty().append('<option value="-1">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }
            else {
                control.empty().append('<option value="-1">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
            }
            //            var ddlItem = '<%=ddlItem.ClientID%>';
            //            var txtHiddenItemId = '<%=txtHiddenItemId.ClientID%>'
            //            var item = $('#' + txtHiddenItemId).val();
            //            $('#' + ddlItem).val(item);

            return false;
        }

        function OnFillServiceFailed(error) {
            alert(error.get_message());
        }

    </script>
    <div id="SearchPanel" class="panel panel-default">        
        <div class="panel-heading">Search Information</div>
        <div class="panel-body">
            <div class="form-horizontal">                
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:HiddenField ID="txtHiddenItemId" runat="server" />
                            <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox>
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
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblReferenceNo" runat="server" class="control-label" Text="Reference No"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtReferenceNo" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div style="display: none;">
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
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-sm"
                        OnClick="btnSearch_Click" />
                        </div>
                </div>
            </div>
        </div>
    </div>    
    <div class="row">
        <div class="columnRight">
        </div>        
    </div>
    <div style="display: none;">
        <asp:Button ID="btnPrintReportFromClient" runat="server" Text="Button" OnClick="btnPrintReportFromClient_Click"
            ClientIDMode="Static" />
    </div>
    <div style="display: none;">
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000;
            top: 2000;" clientidmode="static"></iframe>
    </div>
    <div id="ReportPanel" class="panel panel-default" style="display: none">       
            <div class="panel-heading">Report:: Banquet
            Sales Information</div>
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
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
</asp:Content>
