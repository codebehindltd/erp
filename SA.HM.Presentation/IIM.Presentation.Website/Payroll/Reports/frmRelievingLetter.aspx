<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmRelievingLetter.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmRelievingLetter" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagName="EmployeeSearch" TagPrefix="UserControl" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>HR Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Termination Letter</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            $("#ContentPlaceHolder1_txtRelieveDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat                
            });

            $("#ContentPlaceHolder1_txtResignDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat                
            });
        });

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

    </script>
    <div id="SearchPanel" class="panel panel-default">        
        <div class="panel-heading">Termination Info</div>
        <div class="panel-body">
            <div class="form-horizontal">
            <UserControl:EmployeeSearch runat="server" ID="employeeeSearch" />           
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblDepartment" runat="server" class="control-label" Text="Department"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlDepartmentId" CssClass="form-control" runat="server">
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblRelieveDate" runat="server" class="control-label required-field" Text="Relieve Date"></asp:Label>                    
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtRelieveDate" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
            </div>           
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblResignDate" runat="server" class="control-label required-field" Text="Resign Date"></asp:Label>                  
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtResignDate" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <%--<div class="col-md-2">
                    <asp:Label ID="lblAppointDate" runat="server" Text="Appointed Date"></asp:Label>
                    <span class="MandatoryField">*</span>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtAppointDate" CssClass="datepicker" runat="server"></asp:TextBox>
                </div>--%>
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
        <div class="panel-heading">Termination Letter</div> 
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
