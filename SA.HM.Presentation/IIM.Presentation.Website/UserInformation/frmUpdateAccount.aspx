<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmUpdateAccount.aspx.cs" Inherits="HotelManagement.Presentation.Website.UserInformation.frmUpdateAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Authorization Panel</a>";
            var formName = "<span class='divider'>/</span><li class='active'>My Account</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            

            IsCanSave = $("#<%=hfSavePermission.ClientID %>").val() == '1' ? true : false;
            IsCanEdit = $("#<%=hfEditPermission.ClientID %>").val() == '1' ? true : false;
            IsCanDelete = $("#<%=hfDeletePermission.ClientID %>").val() == '1' ? true : false;
            IsCanView = $("#<%=hfViewPermission.ClientID %>").val() == '1' ? true : false;
        });



        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }
        
    </script>
        <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            My Account</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblUserName" runat="server" class="control-label" Text="Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" style="display: none">
                    <div class="col-md-2">
                        <asp:Label ID="lblUserId" runat="server" class="control-label" Text="User Id"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtUserId" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                    </div>
                </div>
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblUserEmail" runat="server" class="control-label" Text="User Email"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtUserEmail" runat="server" CssClass="form-control"
                        TabIndex="6"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblUserPhone" runat="server" class="control-label" Text="User Phone"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtUserPhone" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnSave" runat="server" Text="Update" OnClick="btnSave_Click" CssClass="btn btn-primary btn-sm"
                        TabIndex="9" />
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
