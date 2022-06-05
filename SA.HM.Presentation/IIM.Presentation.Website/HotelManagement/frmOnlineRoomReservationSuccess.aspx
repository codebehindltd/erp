<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/InnBoard.Master"
    CodeBehind="frmOnlineRoomReservationSuccess.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmOnlineRoomReservationSuccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .onlinereservationsuccess
        {            
            font-weight: bold;
        }
    </style>
    <asp:HiddenField ID="hfOnlineReservationId" runat="server" />
    <div class="panel panel-default" style="height:500px;">
        <div class="panel-body">
            <div class="form-horizontal" style="margin-top:50px;">
                <div class="form-group">
                    <div class="col-md-3">
                    </div>
                    <div class="col-md-9">
                        <asp:Label ID="lblSuccessMsg" runat="server" Text="" CssClass="onlinereservationsuccess"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
