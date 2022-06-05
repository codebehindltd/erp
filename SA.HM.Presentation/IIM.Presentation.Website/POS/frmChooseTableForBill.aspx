<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmChooseTableForBill.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmChooseTableForBill" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        /*$(document).ready(function () {
        var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
        var formName = "<span class='divider'>/</span><li class='active'>Restaurant Bill</li>";
        var breadCrumbs = moduleName + formName;
        $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });*/
    </script>
    <div>
        <div class="btn-toolbar;" style="text-align: right;">
            <asp:HiddenField ID="hfCostCenterId" runat="server"></asp:HiddenField>
        </div>
    </div>
    <div id="SearchPanel" class="block">
        <div class="block-body collapse in">
            <asp:Literal ID="ltlRoomTemplate" runat="server">
            </asp:Literal>
        </div>
    </div>
</asp:Content>
