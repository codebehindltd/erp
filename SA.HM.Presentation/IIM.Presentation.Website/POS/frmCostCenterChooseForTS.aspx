<%@ Page Title="" Language="C#" MasterPageFile="~/POS/RestaurantMaster.Master"
    AutoEventWireup="true" CodeBehind="frmCostCenterChooseForTS.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmCostCenterChooseForTS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        /*$(document).ready(function () {
        var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
        var formName = "<span class='divider'>/</span><li class='active'>Cost Center Choose</li>";
        var breadCrumbs = moduleName + formName;
        $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });*/
    </script>
    <div class="row-fluid block" style="overflow: hidden; margin: 0px; padding: 0px;
        margin-top: 2px; margin-bottom: 5px; border: 0px;">
        <div class="span12" style="margin: 0px; padding: 0px;">
            <asp:Literal ID="ltlRoomTemplate" runat="server"> </asp:Literal>
        </div>
    </div>
</asp:Content>
