<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportBanquetTransaction.aspx.cs" Inherits="HotelManagement.Presentation.Website.Banquet.Reports.frmReportBanquetTransaction" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Banquet</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Sales Transaction</li>";
            var breadCrumbs = moduleName + formName;
            $('#ContentPlaceHolder1_txtServiceFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtServiceToDate').datepicker("option", "minDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtServiceToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtServiceFromDate').datepicker("option", "maxDate", selectedDate);
                }
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
                $("#ContentPlaceHolder1_hfServiceNameList").val(costcenter);
            }

            
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            
           

            $("#ContentPlaceHolder1_ddlReceivedBy").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            //$("#btnClear").click(function () {
            //    $("#ContentPlaceHolder1_hfServiceIdList").val("");
            //    $("#ContentPlaceHolder1_hfServiceNameList").val("");
            //});


        });

        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
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
            return;
        }

        function GetCheckedService() {
            var costCenterId = "", costcenter = "";
            $('#TableWiseItemInformation input:checked').each(function () {
                if (costCenterId != "") {
                    costCenterId += ',' + (20000+ parseInt( $(this).attr('value')));
                    costcenter += ', ' + $(this).attr('name');
                }
                else {
                    costCenterId += (20000 + parseInt($(this).attr('value')));
                    costcenter += $(this).attr('name');
                }
            });

            $("#ContentPlaceHolder1_hfServiceIdList").val(costCenterId);
            $("#ContentPlaceHolder1_hfServiceNameList").val(costcenter);
            $("#ServiceSelectContainer").dialog("close");
        }

        function CloseServiceDialog() {
            $("#ServiceSelectContainer").dialog("close");
        }

    </script>
    <asp:HiddenField ID="hfServiceIdList" runat="server" />
    <asp:HiddenField ID="hfIndividualModuleName" runat="server" />
    <asp:HiddenField ID="hfServiceNameList" runat="server" />
    <div id="ServiceSelectContainer" style="display: none;">
        <div style="height: 236px; overflow-y: scroll">
            <asp:Literal ID="ltServiceInformation" runat="server"></asp:Literal>
        </div>
        <div style='margin-top: 12px;'>
            <button type='button' onclick='javascript:return GetCheckedService()' id='btnAddCostcenterId' class='btn btn-primary' style="width: 65px">OK</button>
            <button type='button' onclick='javascript:return CloseServiceDialog()' id='btnCancelCostcenterId' class='btn btn-primary'>Cancel</button>
        </div>
    </div>

    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblServiceFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtServiceFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblServiceToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <div class="row">
                            <div class="col-md-11">
                                <asp:TextBox ID="txtServiceToDate" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-1 col-padding-left-none" style="margin-left: -13px;">
                                <span style="margin-left: 3px;">
                                    <img src="../../Images/ListIcon.png" title='Multi Service Info' style="cursor: pointer;"
                                        onclick='javascript:return MultiServiceInformation()' alt='Multi Service Info'
                                        border='0' /></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">

                    <div class="col-md-2">
                        <asp:Label ID="lblPaymentMode" runat="server" class="control-label" Text="Payment Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlPaymentMode" CssClass="form-control" runat="server">
                            <asp:ListItem Value="All">--- All ---</asp:ListItem>
                            <asp:ListItem Value="Cash">Cash</asp:ListItem>
                            <asp:ListItem Value="Card">Card</asp:ListItem>
                            <asp:ListItem Value="Cheque">Cheque</asp:ListItem>
                            <asp:ListItem Value="M-Banking">M-Banking</asp:ListItem>
                            <asp:ListItem Value="Company">Company</asp:ListItem>
                            <asp:ListItem Value="Employee">Employee</asp:ListItem>
                            <%--<asp:ListItem Value="Guest Room">Guest Room</asp:ListItem>--%>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblReceivedBy" runat="server" class="control-label" Text="Received By"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReceivedBy" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>

                    
                    
                </div>
                
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-sm"
                            OnClick="btnSearch_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Search" CssClass="btn btn-primary btn-sm" OnClick="btnClear_Click" />
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
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000; top: 2000;"
            clientidmode="static"></iframe>
    </div>
    <div id="ReportPanel" class="panel panel-default" style="display: none">
        <div class="panel-heading">
            Report:: Sales Transaction
        </div>
        <div class="panel-body">
            <div class="ReporContainerDiv">
                <asp:Panel ID="pnlRoomCalender" runat="server" ScrollBars="Both" Height="700px">
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

        var x = '<%=_RoomStatusInfoByDate%>';
        if (x > -1) {
            EntryPanelVisibleFalse();
        }
        var y = '<%=isMessageBoxEnable%>';
        if (y > -1) {
            MessagePanelShow();
            if (y == 2) {
                $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
            }
        }
        else {
            MessagePanelHide();
        }
    </script>
</asp:Content>
