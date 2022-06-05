<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmReportPFLoanCollection.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmReportPFLoanCollection" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Provident Fund</a>";
            var formName = "<span class='divider'>/</span><li class='active'>PF Loan Collection</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            $("#ContentPlaceHolder1_ddlDepartment").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlEmployee").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
        });

        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }        
    </script>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <div id="SearchPanel" class="panel panel-default">      
        <div class="panel-heading">
            PF Loan Collection</div>
        <div class="panel-body">
            <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblDepartment" runat="server" class="control-label required-field" Text="Department"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList AutoPostBack="true" ID="ddlDepartment" runat="server" CssClass="form-control" OnSelectedIndexChanged="EmpDropDown_Change"
                        TabIndex="4">
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblEmployee" runat="server" class="control-label required-field" Text="Employee"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control"
                        TabIndex="4">
                    </asp:DropDownList>
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
        <div class="panel-heading">
            PF Loan Collection</div>
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
