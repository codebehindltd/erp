<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="HotelManagement.Presentation.Website.Common.Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:panel id="pnlError" runat="server" visible="false">
        <asp:label id="lblError" runat="server" text="Oops! An error occurred while performing your request. Sorry for any convenience."></asp:label>
        <asp:label id="lblGoBack" runat="server" text="You may want to get back to the previous page and perform other activities."></asp:label>
        <asp:hyperlink id="hlinkPreviousPage" runat="server">Go back</asp:hyperlink>
    </asp:panel>
</asp:Content>
