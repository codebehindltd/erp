<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportGratuityEligibleEmpList.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmReportGratuityEligibleEmpList" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Gratuity</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Gratuity Eligible Employee List</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });

        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }        
    </script>
    <%--<div id="SearchPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">PF Member Info
        </a>
        <div class="HMBodyContainer">
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblMemberType" runat="server" Text="Member Type"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlMemberType" runat="server" CssClass="customMediumDropDownSize"
                        TabIndex="4">
                        <asp:ListItem Value="Active" Text="Active">Active</asp:ListItem>
                        <asp:ListItem Value="Inactive" Text="Inactive">Terminated</asp:ListItem>
                        <asp:ListItem Value="NotEligible" Text="Not Eligible">Eligible</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="HMContainerRowButton">
                <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="btn btn-primary"
                    OnClick="btnGenerate_Click" />
            </div>
        </div>
    </div>
    <div class="clear">
    </div>--%>
    <div id="ReportPanel" class="panel panel-default" style="display: none;">        
        <div class="panel-heading">Gratuity Eligible Employee List</div>
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
