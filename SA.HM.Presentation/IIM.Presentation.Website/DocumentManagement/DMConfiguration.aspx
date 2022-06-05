<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="DMConfiguration.aspx.cs" Inherits="HotelManagement.Presentation.Website.DocumentManagement.DMConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

        });
        $(function () {
            $("#myTabs").tabs();
        });
        function CheckPrefixLength(control) {
            const charLength = $(control).val().length;
            if (charLength > 0 && charLength != 3) {
                toastr.warning("Enter 3 Character.");
                $(control).focus();
            }
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
    <div class="panel panel-default">
        <div class="panel-heading">
            Document Management Configuration
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="col-md-6">
                    <div class="row">
                        <div class="col-md-12">
                            <div id="CommonInformation" class="panel panel-default" runat="server">
                                <div class="panel-heading">
                                    Common Information
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsDocumentInformationRestrictedForAllUsers" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsDocumentInformationRestrictedForAllUsers" runat="server" CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Document Information Restricted For All Users?
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-12">
                    <asp:Button ID="btnUpdate" runat="server" TabIndex="3" Text="Update" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClick="btnUpdate_Click" />
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
