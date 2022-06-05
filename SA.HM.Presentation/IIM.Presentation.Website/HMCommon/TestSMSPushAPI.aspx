<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="TestSMSPushAPI.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.TestSMSPushAPI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });
    </script>
    <asp:Button ID="btnSendMail" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary" OnClick="btnSendMail_Click"  />
</asp:Content>
