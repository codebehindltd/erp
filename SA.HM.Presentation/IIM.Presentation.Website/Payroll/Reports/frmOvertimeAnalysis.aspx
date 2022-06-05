<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmOvertimeAnalysis.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmOvertimeAnalysis" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Overtime Analysis</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });

        function CheckValidation() {
            if ($("#ContentPlaceHolder1_ddlProcessMonth").val() == "0") {
                toastr.warning("Please Select Process Month");
                return false;
            }

            return true;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }
        
    </script>
    <div id="SearchPanel" class="panel panel-default">        
            <div class="panel-heading">Overtime Info</div>     
        <div class="panel-body">
            <div class="form-horizontal">         
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblProcessDate" runat="server" class="control-label" Text="Process Month"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlProcessMonth" runat="server" CssClass="form-control"
                        TabIndex="2">
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Process Year"></asp:Label>                   
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlYear" CssClass="form-control" runat="server">
                    </asp:DropDownList>
                </div>
            </div>           
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblDepartmentId" runat="server" class="control-label required-field" Text="Department"></asp:Label>                    
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlDepartmentId" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                </div>
                <div class="col-md-4">
                </div>
            </div>           
            <div class="row">
 <div class="col-md-12">
                <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="btn btn-primary"
                    OnClientClick="javascript:returrn CheckValidation()" OnClick="btnGenerate_Click" />
            </div>
            </div>
        </div>
        </div>
    </div>   
    <div id="ReportPanel" class="panel panel-default" style="display: none;">        
        <div class="panel-heading">Overtime Info</div>     
        <div class="panel-body">
            <rsweb:ReportViewer ID="rvTransaction" runat="server" ShowFindControls="false" ShowWaitControlCancelLink="false"
                Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" Width="100%" Height="820px">
            </rsweb:ReportViewer>
        </div>
    </div>
    <script type="text/javascript">
        var x = '<%=dispalyReport%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
</asp:Content>
