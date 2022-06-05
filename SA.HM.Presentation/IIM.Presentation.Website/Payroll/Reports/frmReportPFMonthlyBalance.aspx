<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportPFMonthlyBalance.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmReportPFMonthlyBalance" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Provident Fund</a>";
            var formName = "<span class='divider'>/</span><li class='active'>PF Monthly Balance</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
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
        });

        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }        
    </script>
    <div id="SearchPanel" class="panel panel-default">      
        <div class="panel-heading">PF Monthly Balance</div>
        <div class="panel-body">
            <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox><input
                        type="hidden" id="hidFromDate" />
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox><input
                        type="hidden" id="hidToDate" />
                </div>
            </div>           
            <div class="row">
 <div class="col-md-12">
                <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="btn btn-primary"
                    OnClick="btnGenerate_Click" />
            </div>
            </div>
        </div>
        </div>
    </div>    
    <div id="ReportPanel" class="panel panel-default" style="display: none;">       
        <div class="panel-heading">PF Monthly Balance</div> 
        <div class="panel-body">
            <rsweb:ReportViewer ID="rvTransaction" runat="server" ShowFindControls="false" ShowWaitControlCancelLink="false"
                Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" Width="950px" Height="820px">
            </rsweb:ReportViewer>
        </div>
    </div>
    <script type="text/javascript">
        var x = '<%=dispalyReport%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
</asp:Content>
