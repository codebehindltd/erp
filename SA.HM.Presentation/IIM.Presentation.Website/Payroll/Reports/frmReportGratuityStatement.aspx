<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportGratuityStatement.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmReportGratuityStatement" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Gratuity</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Gratuity Statement</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_txtProcessDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat                
            });

        });

        function ReportPanelShow() {
            $('#ReportPanel').show();
        }
        function ReportPanelHide() {
            $('#ReportPanel').hide();
        }
    </script>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblProcessDate" runat="server" class="control-label required-field" Text="Process Date"></asp:Label>                   
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtProcessDate" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblIsManagement" runat="server" class="control-label" Text="Is Management"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlIsManagement" CssClass="form-control" runat="server">
                        <asp:ListItem Value="0">No</asp:ListItem>
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>            
            <div class="row">
 <div class="col-md-12">
                <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary"
                    OnClick="btnGenarate_Click"/>
            </div>
            </div>
            </div>
        </div>
    </div>   
    <div id="ReportPanel" class="panel panel-default" style="display: none">        
            <div class="panel-heading">Report:: Gratuity
            Statement</div> 
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
        var xMessage = '<%=isDisplayReport%>';
        if (xMessage > -1) {
            ReportPanelShow();
        }
        else {
            ReportPanelHide();
        }        
    </script>
</asp:Content>
