<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmRequisitionInfoForReport.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.Reports.frmRequisitionInfoForReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Purchase</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Requisition Approval</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            var txtStartDate = '<%=txtFromDate.ClientID%>'
            var txtEndDate = '<%=txtToDate.ClientID%>'

            $('#' + txtStartDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtEndDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtEndDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtStartDate).datepicker("option", "maxDate", selectedDate);
                }
            });


        });

    </script>
    <div id="DetailsRequisitionDialog" style="display: none;">
        <div id="DetailsRequisitionGridContaiiner">
        </div>
        <div class="HMContainerRowButton" style="padding-bottom: 0; padding-top: 10px;">
            <input id="btnApprove" type="button" class="TransactionalButton btn btn-primary btn-sm"
                value="Approve Requisition" />
        </div>
    </div>
    <div id="InfoPanel" class="panel panel-default">
        <div class="panel-heading">
            Requisition Search Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server" TabIndex="4"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Status"></asp:Label>
                    </div>
                    <div class="col-md-4" id="StatusDiv">
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" TabIndex="2">
                            <asp:ListItem Value="All" Text="All"></asp:ListItem>
                            <asp:ListItem Value="Submit" Text="Submitted"></asp:ListItem>
                            <asp:ListItem Value="Approved" Text="Approved"></asp:ListItem>
                            <asp:ListItem Value="Checked" Text="Checked"></asp:ListItem>
                            <asp:ListItem Value="Cancel" Text="Cancel"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label" Text="Requisition Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtRequisitionNumber" CssClass="form-control" runat="server" TabIndex="4"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenerate" runat="server" Text="Generate"
                            CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" OnClick="btnGenerate_Click" />                        
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-12">
                        <rsweb:ReportViewer ID="rvTransaction" runat="server" ShowFindControls="false" ShowWaitControlCancelLink="false"
                            PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" Font-Names="Verdana"
                            Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                            WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                        </rsweb:ReportViewer>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
