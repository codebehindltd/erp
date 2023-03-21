<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmReportEmpAttendance.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmReportEmpAttendance" %>

<%@ Register TagPrefix="UserControl" TagName="companyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {

            if ($("#<%=ddlEmployee.ClientID %>").val() == "0")
                $("#employeeSearchSection").hide();

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Attendance & Time Keeping</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Attendance Report</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            var txtRosterDateFrom = '<%=txtRosterDateFrom.ClientID%>'
            var txtRosterDateTo = '<%=txtRosterDateTo.ClientID%>'

            $('#' + txtRosterDateFrom).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtRosterDateTo).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtRosterDateTo).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtRosterDateFrom).datepicker("option", "maxDate", selectedDate);
                }
            });


            $("#<%= ddlEmployee.ClientID %>").change(function () {
                if ($(this).val() == "0") {
                    $("#employeeSearchSection").hide();
                    $("#DepartmentInfoDiv").show();
                    $("#<%=hfEmployeeId.ClientID %>").val("0");
                    $("#<%=txtSearchEmployee.ClientID %>").val("");
                    $("#<%=txtEmployeeName.ClientID %>").val("");
                }
                else if ($(this).val() == "1") {
                    $("#employeeSearchSection").show();
                    $("#DepartmentInfoDiv").hide();
                }
            });
            var empType = $("#<%= ddlEmployee.ClientID %>").val();
            if (empType == "0") {
                $("#DepartmentInfoDiv").show();
            }
            else if (empType == "1") {
                $("#DepartmentInfoDiv").hide();
            }
        });


        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }
        function CheckValidity() {
            var company = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            var project = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

            $("#ContentPlaceHolder1_hfCompanyId").val(company);
            $("#ContentPlaceHolder1_hfProjectId").val(project);
            $("#ContentPlaceHolder1_hfCompanyName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany option:selected").text());
            $("#ContentPlaceHolder1_hfProjectName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject option:selected").text());


            var from = $("#<%= txtRosterDateFrom.ClientID %>").val();
            var to = $("#<%= txtRosterDateTo.ClientID %>").val();
            if (from == "") {
                toastr.warning("Please Give Start Date");
                return false;
            }
            else if (to == "") {
                toastr.warning("Please Give To Date");
                return false;
            }
            return true;
        }
        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }

    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <asp:HiddenField runat="server" ID="hfEmployeeId" Value="0" />
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCompanyName" runat="server" Value="0" />
    <asp:HiddenField ID="hfProjectId" runat="server" Value="0" />
    <asp:HiddenField ID="hfProjectName" runat="server" Value="0" />
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label id="lblFromDate" class="control-label">
                            From Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtRosterDateFrom" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label id="lblToDate" class="control-label">
                            To Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtRosterDateTo" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <UserControl:companyProjectUserControl ID="companyProjectUserControl" runat="server" />
                <asp:Panel ID="EmployeeInformationDiv" runat="server">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label id="lblEmployee" class="control-label">
                                Employee Type</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control">
                                <asp:ListItem Value="0" Text="--- All Employee ----"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Individual Employee"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="employeeSearchSection">
                        <div class="form-group">
                            <div class="col-md-2">
                                <label id="lblEmpId" class="control-label required-field">
                                    Employee ID</label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtSearchEmployee" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-1">
                                <asp:Button ID="btnSrcEmployees" runat="server" Text="Search" TabIndex="2" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnSrcEmployees_Click" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label id="lblEmployeeName" class="control-label">
                                    Employee Name</label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group" id="DepartmentInfoDiv">
                        <div class="col-md-2">
                            <label id="lblDepartmentId" class="control-label">
                                Department</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlDepartmentId" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2" id="PayrollWorkStationLabelDiv" runat="server" style="padding-top:5px;">
                            <asp:Label ID="lblWorkStation" runat="server" class="control-label" Text="Work Station"></asp:Label>
                        </div>
                        <div class="col-md-10" id="PayrollWorkStationControlDiv" runat="server" style="padding-top:5px;">
                            <asp:DropDownList ID="ddlWorkStation" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group" style="display: none;">
                        <div class="col-md-2">
                            <label id="lblTimeSlab" class="control-label">
                                Time Slab</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlTimeSlab" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </asp:Panel>
                <div class="form-group" runat="server" id="ReportTypeDiv">
                    <div class="col-md-2">
                        <label id="Label1" class="control-label">
                            Report Type</label>
                    </div>
                    <div class="col-md-4">
                        <asp:HiddenField ID="hfIsRestaurantIntegrateWithPayrollAttendanc" runat="server"></asp:HiddenField>
                        <asp:DropDownList ID="ddlReportType" CssClass="form-control" runat="server">
                            <asp:ListItem Value="-1" Text="Attendance Information"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Department Wise Attendance"></asp:ListItem>
                            <%--<asp:ListItem Value="0" Text="Attendance In/ Out"></asp:ListItem>--%>
                            <%--<asp:ListItem Value="1" Text="Attendance Log"></asp:ListItem>--%>
                            <%--<asp:ListItem Value="2" Text="Without Clock Out"></asp:ListItem>--%>
                            <asp:ListItem Value="3" Text="Late Attendance"></asp:ListItem>
                            <%--<asp:ListItem Value="4" Text="Overtime Report"></asp:ListItem>--%>
                            
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary"
                            OnClick="btnGenarate_Click" OnClientClick="javascript:return CheckValidity()" />
                    </div>
                </div>
            </div>
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
    <div id="ReportPanel" class="panel panel-default" style="display: none;">
        <div class="panel-heading">
            Report:: Employee Attendance Information
        </div>
        <div class="panel-body">
            <div style="height: 700px; overflow-x: scroll; overflow-y: scroll; text-align: left;">
                <rsweb:ReportViewer ID="rvTransaction" runat="server" ShowFindControls="false" ShowWaitControlCancelLink="false"
                    PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" Font-Names="Verdana"
                    Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                </rsweb:ReportViewer>
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

            if ($("#ContentPlaceHolder1_companyProjectUserControl_hfIsSingle").val() != "1") {
                var c = $("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val();
                var p = $("#ContentPlaceHolder1_hfProjectId").val();

                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val($("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val()).trigger("change");
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val($("#ContentPlaceHolder1_hfProjectId").val()).trigger("change");

            }
        });

        function PrintDocumentFunc(ss) {
            $('#btnPrintReportFromClient').trigger('click');
            return true;
        }

        var xMessage = '<%=isMessageBoxEnable%>';
        if (xMessage > -1) {
            MessagePanelShow();
        }
        else {
            MessagePanelHide();
        }

        var x = '<%=_RoomStatusInfoByDate%>';
        if (x > -1)
            EntryPanelVisibleFalse();

    </script>
</asp:Content>
