<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmCostCenterChooseForFD.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmCostCenterChooseForFD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        /*$(document).ready(function () {
        var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
        var formName = "<span class='divider'>/</span><li class='active'>Cost Center Choose</li>";
        var breadCrumbs = moduleName + formName;
        $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });*/
    </script>
    <%--<div id="SearchPanel" class="block">
        <div class="block-body collapse in">
            <asp:Literal ID="ltlRoomTemplate" runat="server"> </asp:Literal>
        </div>
    </div>--%>
     <asp:Literal ID="ltlRoomTemplate" runat="server"> </asp:Literal>
</asp:Content>
