<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmBanquetConfiguration.aspx.cs" Inherits="HotelManagement.Presentation.Website.Banquet.frmBanquetConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .mycheckbox input[type="checkbox"] {
            margin-right: 10px;
            padding-top: 5px;
        }
    </style>
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            /*var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Configuration</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);*/

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            $("#ContentPlaceHolder1_InventoryTransactionSetup").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_SupplierAccountsHeadId").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
        });
        $(function () {
            $("#myTabs").tabs();
        });

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
    <div class="row">
        <div class="col-md-6">
            <div id="RoomRegistrationTermsAndConditionsDiv" class="panel panel-default">
                <div class="panel-heading">
                    Banquet Terms and Conditions (~ sign use for New Line)
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:HiddenField ID="hfBanquetTermsAndConditions" runat="server"></asp:HiddenField>
                                <asp:TextBox ID="txtBanquetTermsAndConditions" TabIndex="2" TextMode="MultiLine" CssClass="form-control quantitydecimal"
                                    runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="Button2" runat="server" Text="Update" TabIndex="13" CssClass="btn btn-primary btn-sm"
                                    OnClick="btnBanquetTermsAndConditions_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    <div class="col-md-6">
        <div id="dvFOConfig" class="panel panel-default" runat="server">
            <div class="panel-heading">
                Banquet Configuration
            </div>
            <div class="panel-body">
                <div class="form-horizontal">                    
                    <div class="col-md-12">
                        <div class="form-group">
                            <div style="float: left;">
                                <asp:HiddenField ID="hfIsBanquetRateEditableEnable" runat="server"></asp:HiddenField>
                                <asp:CheckBox ID="IsBanquetRateEditableEnable" TabIndex="4" runat="Server" Text="" Font-Bold="true"
                                    CssClass="mycheckbox" TextAlign="right" />
                                &nbsp;&nbsp;Is Banquet Rate Editable Enable?
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <div style="float: left;">
                                <asp:HiddenField ID="hfIsBanquetBillAmountWillRound" runat="server"></asp:HiddenField>
                                <asp:CheckBox ID="IsBanquetBillAmountWillRound" TabIndex="4" runat="Server" Text="" Font-Bold="true"
                                    CssClass="mycheckbox" TextAlign="right" />
                                &nbsp;&nbsp;Is Banquet Bill Amount Will Round?
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <div style="float: left;">
                                <asp:HiddenField ID="hfIsBanquetIntegrateWithAccounts" runat="server"></asp:HiddenField>
                                <asp:CheckBox ID="IsBanquetIntegrateWithAccounts" TabIndex="4" runat="Server" Text="" Font-Bold="true"
                                    CssClass="mycheckbox" TextAlign="right" />
                                &nbsp;&nbsp;Is Banquet Integrate With Accounts?
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <div style="float: left;">
                                <asp:HiddenField ID="hfIsBanquetReservationRestictionForAllUser" runat="server"></asp:HiddenField>
                                <asp:CheckBox ID="IsBanquetReservationRestictionForAllUser" TabIndex="4" runat="Server" Text="" Font-Bold="true"
                                    CssClass="mycheckbox" TextAlign="right" />
                                &nbsp;&nbsp;Is Banquet Reservation Restriction for all User?
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <div style="float: left;">
                                <asp:HiddenField ID="hfIsBanquetReservationEmailAutoPostingEnable" runat="server"></asp:HiddenField>
                                <asp:CheckBox ID="IsBanquetReservationEmailAutoPostingEnable" TabIndex="4" runat="Server" Text="" Font-Bold="true"
                                    CssClass="mycheckbox" TextAlign="right" />
                                &nbsp;&nbsp;Is Banquet Reservation Email Auto Posting Enable?
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnSaveBanquetConfig" runat="server" Text="Update" TabIndex="13"
                                CssClass="btn btn-primary btn-sm" OnClick="btnSaveBanquetConfig_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </div>
    <script type="text/javascript">
        var x = '<%=isMessageBoxEnable%>';
        if (x > -1) {
            MessagePanelShow();
            if (x == 2) {
                $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
            }
        }
        else {
            MessagePanelHide();
        }
    </script>
</asp:Content>
