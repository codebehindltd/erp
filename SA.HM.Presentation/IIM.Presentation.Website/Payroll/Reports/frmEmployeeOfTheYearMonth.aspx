<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmployeeOfTheYearMonth.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmEmployeeOfTheYearMonth" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Employee Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Best Employee</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#<%=ddlBeastEmployeeType.ClientID %>").change(function () {
                var ddlBeastEmployeeTypeVal = $("#<%=ddlBeastEmployeeType.ClientID %>").val();
                if (ddlBeastEmployeeTypeVal == "Month") {
                    $("#MonthSelectionDiv").show();
                }
                else {
                    $("#MonthSelectionDiv").hide();
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
        <div class="panel-heading">>Best Employee Info</div>
        <div class="panel-body">
            <div class="form-horizontal">        
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblBeastEmployeeType" runat="server" class="control-label" Text="Process Type"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlBeastEmployeeType" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Employee Of The Month" Value="Month"></asp:ListItem>
                        <asp:ListItem Text="Employee Of The Year" Value="Year"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblDepartmentId" runat="server" class="control-label" Text="Department"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlDepartmentId" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>           
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="Label3" runat="server" class="control-label" Text="Year"></asp:Label>
                    <%--<span class="MandatoryField">*</span>--%>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlSelectionYear" runat="server" CssClass="form-control">
                        <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
                        <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                        <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
                        <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                        <asp:ListItem Text="2019" Value="2019"></asp:ListItem>
                        <asp:ListItem Text="2020" Value="2020"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div id="MonthSelectionDiv">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Month"></asp:Label>
                        <%--<span class="MandatoryField">*</span>--%>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSelectionMonth" runat="server" CssClass="form-control">
                            <asp:ListItem Text="January" Value="1"></asp:ListItem>
                            <asp:ListItem Text="February" Value="2"></asp:ListItem>
                            <asp:ListItem Text="March" Value="3"></asp:ListItem>
                            <asp:ListItem Text="April" Value="4"></asp:ListItem>
                            <asp:ListItem Text="May" Value="5"></asp:ListItem>
                            <asp:ListItem Text="June" Value="6"></asp:ListItem>
                            <asp:ListItem Text="July" Value="7"></asp:ListItem>
                            <asp:ListItem Text="August" Value="8"></asp:ListItem>
                            <asp:ListItem Text="September" Value="9"></asp:ListItem>
                            <asp:ListItem Text="October" Value="10"></asp:ListItem>
                            <asp:ListItem Text="November" Value="11"></asp:ListItem>
                            <asp:ListItem Text="December" Value="12"></asp:ListItem>
                        </asp:DropDownList>
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
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000;
            top: 2000;" clientidmode="static"></iframe>
    </div>
    <div id="ReportPanel" class="panel panel-default" style="display: none;">        
        <div class="panel-heading">Best Employee</div>
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
