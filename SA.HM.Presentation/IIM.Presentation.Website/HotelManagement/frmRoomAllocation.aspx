<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmRoomAllocation.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmRoomAllocation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script>
    $(document).ready(function () {
        var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
        var formName = "<span class='divider'>/</span><li class='active'>Today's Room Status</li>";
        var breadCrumbs = moduleName + formName;
        $("#ltlBreadCrumbsInformation").html(breadCrumbs);
    });

</script>
    <div id="SearchPanel" class="block">
        <div class="block-body collapse in">
            <asp:Literal ID="ltlRoomTemplate" runat="server">
            </asp:Literal>
        </div>
    </div>
</asp:Content>
