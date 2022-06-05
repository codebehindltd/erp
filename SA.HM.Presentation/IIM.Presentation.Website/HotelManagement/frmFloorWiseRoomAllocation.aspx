<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmFloorWiseRoomAllocation.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmFloorWiseRoomAllocation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Room Allocation</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });
    </script>
    <div id="GraphicalPanel" class="panel panel-default">
        <%--<a href="#page-stats" class="block-heading" data-toggle="collapse">Room Allocation
        </a>--%>
        <div class="panel-heading">
            Room Allocation</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <%--Left Left--%>
                        <asp:HiddenField ID="txtFloorWiseRoomAllocationInfo" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblSrcFloorId" runat="server" class="control-label" Text="Floor Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <%--Right Left--%>
                        <asp:DropDownList ID="ddlSrcFloorId" CssClass="form-control" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlSrcFloorId_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="row">
            <div style='float: left;' class='col-md-4 FloorRoomAvailableDiv CommonWidth'>
                Available</div>
            <div style='float: right;' class='col-md-4 FloorRoomReservedDiv CommonWidth'>
                Out Of service</div>
            <div style='overflow: auto;' class='col-md-4 FloorRoomBookedDiv CommonWidth'>
                Booked</div>
        </div>
        <div class="panel panel-body FloorRoomAllocationBGImage" style="height: 100vh;">
            <asp:Literal ID="ltlRoomTemplate" runat="server">
            </asp:Literal>
        </div>
    </div>
</asp:Content>
