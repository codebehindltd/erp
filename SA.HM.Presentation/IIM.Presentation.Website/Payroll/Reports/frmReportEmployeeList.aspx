<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" EnableEventValidation="false" AutoEventWireup="true"
    CodeBehind="frmReportEmployeeList.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmReportEmployeeList" %>

<%@ Register TagPrefix="UserControl" TagName="companyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee List</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);


            var reportType = $('#ContentPlaceHolder1_hfReportType').val();

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_ddlDepartment').select2({
                tags: false,
                allowClear: true,
                width: "99.75%",
            });

            $('#ContentPlaceHolder1_ddlDesignation').select2({
                tags: false,
                allowClear: true,
                width: "99.75%",
            });

            $('#ContentPlaceHolder1_ddlBloodGroup').select2({
                tags: false,
                allowClear: true,
                width: "99.75%",
            });

            $('#ContentPlaceHolder1_ddlWorkStation').select2({
                tags: false,
                allowClear: true,
                width: "99.75%",
            });

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            $("#<%=txtFromDate.ClientID %>").blur(function () {
                var date = $("#<%=txtFromDate.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtFromDate.ClientID %>").focus();
                        $("#<%=txtFromDate.ClientID %>").val("");
                        return false;
                    }
                }
            });

            $("#<%=txtToDate.ClientID %>").blur(function () {
                var date = $("#<%=txtToDate.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtToDate.ClientID %>").focus();
                        $("#<%=txtToDate.ClientID %>").val("");
                        return false;
                    }
                }
            });

            $("#<%=ddlReportType.ClientID %>").change(function () {
                if ($("#<%=ddlReportType.ClientID %>").val() == "0") {
                    $("#EmployeeStatusDiv").show();
                    $("#EmployeeTypeDiv").hide();
                    $("#ReportFormatDiv").show();
                    $("#DateDurationDiv").hide();
                }
                else if ($("#<%=ddlReportType.ClientID %>").val() == "1") {
                    $("#EmployeeTypeDiv").show();
                    $("#ReportFormatDiv").hide();
                    $("#DateDurationDiv").show();
                }
                else if ($("#<%=ddlReportType.ClientID %>").val() == "2") {
                    $("#EmployeeTypeDiv").hide();
                    $("#ReportFormatDiv").hide();
                    $("#DateDurationDiv").hide();
                }
                return false;
            });

            if (reportType == "0") {
                $('#ContentPlaceHolder1_ddlReportType').val(reportType).trigger("change");
            }
            else if (reportType == "1") {
                $('#ContentPlaceHolder1_ddlReportType').val(reportType).trigger('change');
            }
            else if (reportType == "2") {
                $('#ContentPlaceHolder1_ddlReportType').val(reportType).trigger('change');
            }
            $("#<%=ddlEmployeeTypeId.ClientID %>").change(function () {
                var ddlEmployeeTypeId = $("#<%=ddlEmployeeTypeId.ClientID %>").val();
                PageMethods.GetEmpTypeInfoById(ddlEmployeeTypeId, OnPerformForGetEmpTypeInfoByIdSucceeded, OnPerformForGetEmpTypeInfoByIdFailed)
                return false;
            });
            $("#checkAll").click(function () {
                $('input:checkbox').not(this).prop('checked', this.checked);
            });
            if (($("#ContentPlaceHolder1_hfEmplopoyeeStatusId").val()) != "") {
                var a = $("#ContentPlaceHolder1_hfEmplopoyeeStatusId").val();
                var dataArray = ($("#ContentPlaceHolder1_hfEmplopoyeeStatusId").val().split(","));
                for (var i in dataArray) {
                    var x = parseInt(i);
                    $('#' + dataArray[x]).prop('checked', true);
                }
                GetCheckedStatus();
            }

            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").change(function () {
                var company = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
                var project = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
                $("#ContentPlaceHolder1_hfGLCompanyId").val(company);
                $("#ContentPlaceHolder1_hfGLProjectId").val(project);
            });
        });

        function OnPerformForGetEmpTypeInfoByIdSucceeded(result) {
            $("#DateDurationDiv").hide();

            if (result.TypeCategory == "Contractual") {
                $("#DateDurationDiv").show();
            }
            if (result.TypeCategory == "Probational") {
                $("#DateDurationDiv").show();
            }
        }

        function OnPerformForGetEmpTypeInfoByIdFailed(error) {
            toastr.error(error.get_message());
        }

        function LoadEmployee() {
            var activeStat = $("#<%=ddlReportType.ClientID %>").val();
            PageMethods.LoadEmployee(activeStat, OnLoadEmpSucceeded, OnLoadEmpFailed);
            return false;
        }

        function OnLoadEmpSucceeded(result) {
            var list = result;
            var controlId = '<%=ddlEmployeedId.ClientID%>';
            var control = $('#' + controlId);
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].DisplayName + '" value="' + list[i].EmpId + '">' + list[i].DisplayName + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }
        }

        function OnLoadEmpFailed(error) {
        }

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

        function MultiStatusInfo() {
            $("#EmployeeStatusSelectContainer").dialog({
                autoOpen: true,
                modal: true,
                width: 450,
                height: 280,
                closeOnEscape: true,
                resizable: false,
                title: "Employee Status Information",
                show: 'slide'
            });
            return;
        }

        function GetCheckedStatus() {
            var statusId = "", status = "";
            $('#TableWiseItemInformation input:checked').each(function () {

                if ($(this).attr('value') != "checkAll") {
                    if (statusId != "" && statusId != "checkAll") {
                        statusId += ',' + $(this).attr('value');
                        status += ', ' + $(this).attr('name');
                    }
                    else {
                        statusId += $(this).attr('value');
                        status += $(this).attr('name');
                    }
                }
            });
            $("#ContentPlaceHolder1_hfEmplopoyeeStatusId").val(statusId);
            $("#ContentPlaceHolder1_lblEmpStatus").text(status);
            $("#EmployeeStatusSelectContainer").dialog("close");
            $("#StatusInformation").show();
        }

        function CloseEmployeeStatusDialog() {
            $("#EmployeeStatusSelectContainer").dialog("close");
        }

        function ValidationProcess() {
            var company = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            var project = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

            $("#ContentPlaceHolder1_hfGLCompanyId").val(company);
            $("#ContentPlaceHolder1_hfGLProjectId").val(project);
            $("#ContentPlaceHolder1_hfGLCompanyName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany option:selected").text());
            $("#ContentPlaceHolder1_hfGLProjectName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject option:selected").text());
        }
    </script>
    <asp:HiddenField ID="hfEmplopoyeeStatusId" runat="server" />
    <asp:HiddenField ID="hfReportType" runat="server" />
    <asp:HiddenField ID="hfGLCompanyId" runat="server" Value="0" />
    <asp:HiddenField ID="hfGLCompanyName" runat="server" Value="0" />
    <asp:HiddenField ID="hfGLProjectId" runat="server" Value="0" />
    <asp:HiddenField ID="hfGLProjectName" runat="server" Value="0" />
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="EmployeeStatusSelectContainer" style="display: none;">
        <asp:Literal ID="ltEmployeeStatus" runat="server"></asp:Literal>
    </div>
    <div id="SearchPanel" class="block">
        <div class="panel-body">
            <div class="form-horizontal">
                <UserControl:companyProjectUserControl ID="companyProjectUserControl" runat="server" />
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblReportType" runat="server" class="control-label" Text="Report Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control"
                            TabIndex="1">
                            <asp:ListItem Value="0">Employees Status Wise</asp:ListItem>
                            <asp:ListItem Value="1">Employees Type Wise</asp:ListItem>
                            <asp:ListItem Value="2">Employees Department Wise</asp:ListItem>
                            <%-- <asp:ListItem Value="2">Employees for Confirmation</asp:ListItem>
                            <asp:ListItem Value="3">Confirmed Employees</asp:ListItem>
                            <asp:ListItem Value="4">Employees for Contact Extension</asp:ListItem>
                            <asp:ListItem Value="5">Promoted Employees</asp:ListItem>
                            <asp:ListItem Value="6">Employees for Annual Increment</asp:ListItem>--%>
                        </asp:DropDownList>
                    </div>
                    <div id="EmployeeTypeDiv" style="display: none;">
                        <div class="col-md-2">
                            <asp:Label ID="Label1" runat="server" class="control-label" Text="Employee Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <div id="TypeInformation">
                                <asp:DropDownList ID="ddlEmployeeTypeId" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div id="DepartmentDiv">
                        <div class="col-md-2">
                            <asp:Label ID="Label2" runat="server" class="control-label" Text="Department"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblDesignation" runat="server" class="control-label" Text="Designation"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlDesignation" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblBloodGroup" runat="server" class="control-label" Text="Blood Group"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlBloodGroup" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblWorkStation" runat="server" class="control-label" Text="Work Station"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlWorkStation" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" id="EmployeeStatusDiv">
                    <div class="col-md-2">
                        <asp:Label ID="lblEmployeeStatus" runat="server" class="control-label" Text="Employee Status"></asp:Label>
                    </div>
                    <div class="col-md-9">
                        <div id="StatusInformation">
                            <asp:Label ID="lblEmpStatus" runat="server" class="form-control"></asp:Label>
                        </div>
                    </div>
                    <div class="col-md-1 col-padding-left-none">
                        <span style="margin-left: 3px;">
                            <img src="../../Images/ListIcon.png" title='Multi Status Info' style="cursor: pointer;"
                                onclick='javascript:return MultiStatusInfo()' alt='Multi Status Info'
                                border='0' /></span>
                    </div>
                </div>
                <div class="form-group" id="DateDurationDiv" style="display: none;">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server" value=""></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server" value=""></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" id="ReportFormatDiv">
                    <div class="col-md-2">
                        <asp:Label ID="lblStatus" runat="server" class="control-label" Text="Report Format:"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportFormat" runat="server" CssClass="form-control"
                            TabIndex="2">
                            <asp:ListItem Value="Format1">Format 1</asp:ListItem>
                            <asp:ListItem Value="Format2">Format 2</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" style="display: none;">
                    <div class="col-md-2">
                        <asp:Label ID="lblEmployee" runat="server" class="control-label" Text="Employee"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlEmployeedId" runat="server" CssClass="form-control"
                            TabIndex="3">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary"
                            OnClick="btnGenarate_Click" OnClientClick="javascript:return ValidationProcess();" />
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
    <div id="ReportPanel" class="panel panel-default">
        <div class="panel-heading">
            Report:: Employee
            List
        </div>
        <div class="form-horizontal">
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
