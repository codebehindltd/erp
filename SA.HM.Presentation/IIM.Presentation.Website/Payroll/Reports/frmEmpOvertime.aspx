<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpOvertime.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmEmpOvertime" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagName="EmployeeSearch" TagPrefix="UserControl" Src="~/HMCommon/UserControl/EmployeeSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Overtime Report</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            $("#ContentPlaceHolder1_txtAttendanceFromDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtAttendanceToDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtAttendanceToDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtAttendanceFromDate").datepicker("option", "maxDate", selectedDate);
                }

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
        <div class="panel-heading">Employee Overtime</div> 
        <div class="panel-body">
            <div class="form-horizontal">
            <UserControl:EmployeeSearch ID="employeeeSearch" runat="server" />            
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblAttendanceFromDate" runat="server" class="control-label" Text="Attendance From Date"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtAttendanceFromDate" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblAttendanceToDate" runat="server" class="control-label" Text="Attendance To Date"></asp:Label>
                    <%--<span class="MandatoryField">*</span>--%>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtAttendanceToDate" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                </div>
            </div>           
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblDepartment" runat="server" class="control-label" Text="Department"></asp:Label>
                    <%--<span class="MandatoryField">*</span>--%>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control"
                        TabIndex="5">
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
        <div class="panel-heading">Employee Overtime</div> 
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
