<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmPMConfiguration.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.frmPMConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .mycheckbox input[type="checkbox"]
        {
            margin-right: 10px;
            padding-top: 5px;
        }
    </style>
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Purchase Managment</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Configuration</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });

        $(function () {
            $("#myTabs").tabs();
        });
    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="myTabs">
        <div class="row">
            <div class="col-md-6">
                <div id="MonthlySalaryDateSchedulePanel" class="panel panel-default">
                    <div class="panel-heading">
                        Requisition Approval Configuration</div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-4 label-align">
                                    <asp:Label ID="lblRequisitionCheckedBy" runat="server" class="control-label" Text="Checked By"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlRequisitionCheckedBy" runat="server" CssClass="form-control"
                                        TabIndex="1">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-4 label-align">
                                    <asp:Label ID="lblRequisitionApprovedBy" runat="server" class="control-label" Text="Approved By"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlRequisitionApprovedBy" runat="server" CssClass="form-control"
                                        TabIndex="1">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btnHotelBillCon" runat="server" Text="Save" CssClass="btn btn-primary btn-sm"
                                        TabIndex="4" OnClick="btnHotelBillCon_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div id="EmployeeBasicPanel" class="panel panel-default">
                    <div class="panel-heading">
                        Purchase Order Approval Configuration</div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-4 label-align">
                                    <asp:Label ID="lblPOCheckedBy" runat="server" class="control-label" Text="Checked By"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlPOCheckedBy" runat="server" CssClass="form-control" TabIndex="1">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-4 label-align">
                                    <asp:Label ID="lblPOApprovedBy" runat="server" class="control-label" Text="Approved By"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlPOApprovedBy" runat="server" CssClass="form-control" TabIndex="1">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btnServiceBillCon" runat="server" Text="Save" CssClass="btn btn-primary btn-sm"
                                        TabIndex="8" OnClick="btnServiceBillCon_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
