<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmReportActivitiLogs.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.Reports.frmReportActivitiLogs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Common Information</a>";
            var formName = "<span class='divider'>/</span><li class='active'>User Activity Report</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#ContentPlaceHolder1_ddlFeatures").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlUserGroupName").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlMenuGroup").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlUserName").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlModuleId").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlModuleName").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                ignoreReadonly: true,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_ddlModuleId").change(function () {
                var ModuleName = $("#ContentPlaceHolder1_ddlModuleId").val();

                var UserGroupName = $("#ContentPlaceHolder1_ddlUserGroupName").val();

                var MenuGroup = $("#ContentPlaceHolder1_ddlMenuGroup").val();

                if (MenuGroup != "0" && UserGroupName != "0" && ModuleName != "0") {

                    PageMethods.GetLinkByModuleId(ModuleName, UserGroupName, MenuGroup, OnLoadMenuLinkSucceed, OnLoadMenuLinkFailed);

                }
            });

            $("#ContentPlaceHolder1_ddlUserGroupName").change(function () {
                var ModuleName = $("#ContentPlaceHolder1_ddlModuleId").val();

                var UserGroupName = $("#ContentPlaceHolder1_ddlUserGroupName").val();

                var MenuGroup = $("#ContentPlaceHolder1_ddlMenuGroup").val();

                if (MenuGroup != "0" && UserGroupName != "0" && ModuleName != "0") {

                    PageMethods.GetLinkByModuleId(ModuleName, UserGroupName, MenuGroup, OnLoadMenuLinkSucceed, OnLoadMenuLinkFailed);

                }
            });

            $("#ContentPlaceHolder1_ddlMenuGroup").change(function () {
                var ModuleName = $("#ContentPlaceHolder1_ddlModuleId").val();

                var UserGroupName = $("#ContentPlaceHolder1_ddlUserGroupName").val();

                var MenuGroup = $("#ContentPlaceHolder1_ddlMenuGroup").val();

                if (MenuGroup != "0" && UserGroupName != "0" && ModuleName != "0") {

                    PageMethods.GetLinkForReport(ModuleName, UserGroupName, MenuGroup, OnLoadMenuLinkSucceed, OnLoadMenuLinkFailed);

                }
            });

            $("#ContentPlaceHolder1_ddlReportType").change(function () {

                var type = $("#ContentPlaceHolder1_ddlReportType").val();

                if (type == "ActivityLogDetails") {
                    $("#activityLogDiv").show();
                    $("#menuDiv").show();
                    $("#groupNameDiv").hide();
                    $("#featuresDiv").hide();
                    $("#menuLinkDiv").hide();
                    $("#moduleDiv").show();
                    $("#moduleIdDiv").hide();
                }
                else if (type == "ApprovalConfigurationDetails") {
                    $("#groupNameDiv").show();
                    $("#featuresDiv").show();
                    $("#activityLogDiv").hide();
                    $("#menuDiv").hide();
                    $("#moduleDiv").hide();
                    $("#moduleIdDiv").hide();
                    $("#menuLinkDiv").hide();

                }
                else if (type == "UserInformationDetails") {
                    $("#groupNameDiv").show();
                    $("#featuresDiv").hide();
                    $("#activityLogDiv").hide();
                    $("#menuDiv").hide();
                    $("#moduleDiv").hide();
                    $("#moduleIdDiv").hide();
                    $("#menuLinkDiv").hide();

                }
                else if (type == "UserPermissionDetails") {
                    $("#groupNameDiv").show();
                    $("#moduleDiv").hide();
                    $("#moduleIdDiv").show();
                    $("#menuDiv").show();
                    $("#activityLogDiv").hide();
                    $("#featuresDiv").hide();
                    $("#menuLinkDiv").show();

                }
                else {
                    $("#groupNameDiv").hide();
                    $("#featuresDiv").hide();
                    $("#activityLogDiv").hide();
                    $("#menuDiv").hide();
                    $("#moduleDiv").hide();
                    $("#moduleIdDiv").hide();
                    $("#menuLinkDiv").hide();
                    toastr.warning("Select a report type.");
                    $("#ContentPlaceHolder1_ddlReportType").focus();
                }


            });

            var type = $("#ContentPlaceHolder1_hfreportType").val();

            if (type != "") {
                $("#ContentPlaceHolder1_ddlReportType").val($("#ContentPlaceHolder1_hfreportType").val()).trigger("change");
            }

            if (type == "ActivityLogDetails") {


                $("#ContentPlaceHolder1_ddlUserName").val($("#ContentPlaceHolder1_hfUserName").val()).trigger("change");
                $("#ContentPlaceHolder1_ddlMenuGroup").val($("#ContentPlaceHolder1_hfMenuGroup").val()).trigger("change");
                $("#ContentPlaceHolder1_ddlModuleName").val($("#ContentPlaceHolder1_ddlModuleName").val()).trigger("change");


            }
            else if (type == "ApprovalConfigurationDetails") {

                $("#ContentPlaceHolder1_ddlUserGroupName").val($("#ContentPlaceHolder1_hfUserGroupName").val()).trigger("change");
                $("#ContentPlaceHolder1_ddlFeatures").val($("#ContentPlaceHolder1_hfFeatures").val()).trigger("change");


            }
            else if (type == "UserInformationDetails") {

                $("#ContentPlaceHolder1_ddlUserGroupName").val($("#ContentPlaceHolder1_hfUserGroupName").val()).trigger("change");



            }
            else if (type == "UserPermissionDetails") {

                $("#ContentPlaceHolder1_ddlUserGroupName").val($("#ContentPlaceHolder1_hfUserGroupName").val()).trigger("change");
                $("#ContentPlaceHolder1_ddlModuleId").val($("#ContentPlaceHolder1_hfModuleId").val()).trigger("change");
                $("#ContentPlaceHolder1_ddlMenuGroup").val($("#ContentPlaceHolder1_hfMenuGroup").val()).trigger("change");
                if ($("#ContentPlaceHolder1_hfMenuLink").val() != "") {
                    $("#ContentPlaceHolder1_ddlMenuLink").val($("#ContentPlaceHolder1_hfMenuLink").val()).trigger("change");
                }


            }


        });

        function OnLoadMenuLinkSucceed(result) {

            typesList = [];
            $("#ContentPlaceHolder1_ddlMenuLink").empty();
            var i = 0, fieldLength = result.length;

            if (fieldLength > 0) {
                typesList = result;
                $('<option value="' + 0 + '">' + '--- Please Select ---' + '</option>').appendTo('#ContentPlaceHolder1_ddlMenuLink');
                for (i = 0; i < fieldLength; i++) {
                    $('<option value="' + result[i].MenuLinksId + '">' + result[i].PageName + '</option>').appendTo('#ContentPlaceHolder1_ddlMenuLink');
                }
                //$("#ContentPlaceHolder1_ddlGLProject").attr("Enabled", true);
            }
            else {
                $("<option value='0'>--No Menu Link Found--</option>").appendTo("#ContentPlaceHolder1_ddlMenuLink");

                //$("#ContentPlaceHolder1_ddlGLProject").attr("Enabled", false);


            }

            return false;

        }

        function OnLoadMenuLinkFailed(xhr, err) {
            toastr.error(xhr.responseText);
        }

        //Div Visible True/False-------------------
        function ValidationProcess() {
            var type = $("#ContentPlaceHolder1_ddlReportType").val();
            if (type == "") {
                toastr.warning("Select a report type.");
                $("#ContentPlaceHolder1_ddlReportType").focus();
                return false;
            }
            else if (type == "UserPermissionDetails") {
                //var ModuleName = $("#ContentPlaceHolder1_ddlModuleId").val();
                //if (ModuleName == "0") {
                //    toastr.warning("Select a module name.");
                //    $("#ContentPlaceHolder1_ddlModuleId").focus();
                //    return false;
                //}
                //var UserGroupName = $("#ContentPlaceHolder1_ddlUserGroupName").val();
                //if (UserGroupName == "0") {
                //    toastr.warning("Select a User Group Name.");
                //    $("#ContentPlaceHolder1_ddlUserGroupName").focus();
                //    return false;
                //}
                //var MenuGroup = $("#ContentPlaceHolder1_ddlMenuGroup").val();
                //if (MenuGroup == "0") {
                //    toastr.warning("Select a Menu Group.");
                //    $("#ContentPlaceHolder1_ddlMenuGroup").focus();
                //    return false;
                //}
                $("#ContentPlaceHolder1_hfMenuLink").val($("#ContentPlaceHolder1_ddlMenuLink").val());

            }


        }

        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }
    </script>
    <asp:HiddenField ID="hfMenuLink" runat="server" Value="0"></asp:HiddenField>

    <asp:HiddenField ID="hfUserName" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfModuleName" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfModuleId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfFeatures" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfUserGroupName" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfMenuGroup" runat="server" Value="0"></asp:HiddenField>

    <asp:HiddenField ID="hfreportType" runat="server" Value=""></asp:HiddenField>

    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblReportType" runat="server" class="control-label" Text="Report Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control" TabIndex="1">
                            <asp:ListItem Value="">--- Please Select ---</asp:ListItem>
                            <asp:ListItem Value="ActivityLogDetails">Activity Log Details</asp:ListItem>
                            <asp:ListItem Value="ApprovalConfigurationDetails">Approval Configuration Details</asp:ListItem>
                            <asp:ListItem Value="UserInformationDetails">User Information Details</asp:ListItem>
                            <asp:ListItem Value="UserPermissionDetails">User Permission Details</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" id="moduleDiv" style="display: none">
                    <div class="col-md-2">
                        <asp:Label ID="lblModuleName" runat="server" class="control-label" Text="Module Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlModuleName" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" id="moduleIdDiv" style="display: none">
                    <div class="col-md-2">
                        <asp:Label ID="lblModuleId" runat="server" class="control-label" Text="Module Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlModuleId" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="featuresDiv" style="display: none">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label3" runat="server" class="control-label" Text="Features"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlFeatures" runat="server" CssClass="form-control"
                                TabIndex="3">
                            </asp:DropDownList>
                        </div>
                        <div class="col-offset-2">
                            <%--<asp:Label ID="Label2" runat="server" class="control-label" Text="User Group"></asp:Label>--%>
                        </div>
                        <div class="col-offset-4">
                            <%--<asp:DropDownList ID="ddlUserGroup" runat="server" CssClass="form-control"
                            TabIndex="3">
                        </asp:DropDownList>--%>
                        </div>
                    </div>
                </div>
                <div class="form-group" id="groupNameDiv" style="display: none">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Group Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlUserGroupName" runat="server" CssClass="form-control"
                            TabIndex="3">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                    </div>
                </div>
                <div class="form-group" id="menuDiv" style="display: none">
                    <div class="col-md-2">
                        <asp:Label ID="Label4" runat="server" class="control-label" Text="Menu Group"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlMenuGroup" runat="server" CssClass="form-control"
                            TabIndex="3">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" id="menuLinkDiv" style="display: none">
                    <div class="col-md-2">
                        <asp:Label ID="lblMenuLink" runat="server" class="control-label" Text="Menu Link"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlMenuLink" runat="server" CssClass="form-control"
                            TabIndex="3">
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="activityLogDiv" style="display: none">
                    <div class="form-group">

                        <div class="col-md-2">
                            <asp:Label ID="lblUserName" runat="server" class="control-label" Text="User Name"></asp:Label>
                        </div>

                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlUserName" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row" style="display: none">
                    <div>
                        <asp:TextBox ID="txtReportYear" runat="server"></asp:TextBox>
                        <asp:TextBox ID="txtReportDurationName" runat="server"></asp:TextBox>
                        <asp:TextBox ID="txtReportFor" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-sm"
                            OnClick="btnSearch_Click" OnClientClick="javascript:return ValidationProcess();" />
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
        <div class="panel-heading">Report:: User Activity Information</div>
        <div class="panel-body" style="overflow: scroll;">
            <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                PageCountMode="Actual" SizeToReportContent="true" runat="server" Font-Names="Verdana"
                Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" Width="950px" Height="820px">
            </rsweb:ReportViewer>
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

        var x = '<%=_ActivityLog%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
</asp:Content>
