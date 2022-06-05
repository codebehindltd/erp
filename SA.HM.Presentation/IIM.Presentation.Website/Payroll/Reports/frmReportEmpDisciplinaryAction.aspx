<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportEmpDisciplinaryAction.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmReportEmpDisciplinaryAction" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagName="EmployeeSearch" TagPrefix="UserControl" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Employee Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Disciplinary Action</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            $("#<%= ddlEmployee.ClientID %>").change(function () {
                if ($(this).val() == "0") {
                    $("#EmpId").hide();
                    $("#DisciplinaryInfoDiv").show();

                }
                else if ($(this).val() == "1") {
                    $("#EmpId").show();
                    $("#DisciplinaryInfoDiv").hide();
                }
            });
            var empType = $("#<%= ddlEmployee.ClientID %>").val();
            if (empType == "0") {
                $("#DisciplinaryInfoDiv").show();
            }
            else if (empType == "1") {
                $("#DisciplinaryInfoDiv").hide();
            }
            $("#ContentPlaceHolder1_txtStartDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $("#ContentPlaceHolder1_txtEndDate").datepicker({
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
        <div class="panel-heading">
            Disciplinary Action
            Info
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
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
                <div id="EmpId" style="display: none">
                    <UserControl:EmployeeSearch runat="server" ID="employeeeSearch" />
                </div>
                <div id="DisciplinaryInfoDiv">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblSActionType" runat="server" class="control-label" Text="Action Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlSActionTypeId" runat="server" CssClass="form-control"
                                TabIndex="2">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblSActionreason" runat="server" class="control-label" Text="Action Reason"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlSActionreasonId" runat="server" CssClass="form-control"
                                TabIndex="2">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblSProposedAction" runat="server" class="control-label" Text="Proposed Action"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlSProposedActionId" runat="server" CssClass="form-control"
                                TabIndex="2">
                                <%--<asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                <asp:ListItem Value="Incremenr HoldUp">Increment HoldUp</asp:ListItem>
                                <asp:ListItem Value="Promotion HoldUp">Promotion HoldUp</asp:ListItem>
                                <asp:ListItem Value="Suspention">Suspention</asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
                        </div>
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
    <div style="display: none;">
        <asp:Button ID="btnPrintReportFromClient" runat="server" Text="Button" OnClick="btnPrintReportFromClient_Click"
            ClientIDMode="Static" />
    </div>
    <div style="display: none;">
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000; top: 2000;"
            clientidmode="static"></iframe>
    </div>
    <div id="ReportPanel" class="panel panel-default" style="display: none;">
        <div class="panel-heading">Disciplinary Action</div>
        <div class="panel-body">
            <rsweb:ReportViewer ID="rvTransaction" runat="server" ShowFindControls="false" ShowWaitControlCancelLink="false"
                PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" Font-Names="Verdana"
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
        var x = '<%=dispalyReport%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
</asp:Content>
