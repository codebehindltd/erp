<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmHMHome.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.frmHMHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "";
            var formName = "<li class='active'>Home</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });
    </script>
    <%--<div class="banner">
        <div id="slider1">
            <div class="flexslider">
                <ul class="slides">
                    <li>
                        <img src="/StyleSheet/images/banner1.jpg" alt="">
                    </li>
                    <li>
                        <img src="/StyleSheet/images/banner3.jpg" alt="">
                    </li>
                    <li>
                        <img src="/StyleSheet/images/banner4.jpg" alt="">
                    </li>
                    <li>
                        <img src="/StyleSheet/images/banner5.jpg" alt="">
                    </li>
                    <li>
                        <img src="/StyleSheet/images/banner6.jpg" alt="">
                    </li>
                    <li>
                        <img src="/StyleSheet/images/banner7.jpg" alt="">
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <div class="alert-information-info">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        Welcome
        <asp:Label ID="lblCurrentUser" runat="server" Text=""></asp:Label>!
    </div>--%>
    <div class="block" style="display: none;">
        <a href="#page-stats" class="block-heading" data-toggle="collapse"><i class="icon-info-sign">
        </i>Latest Information</a>
        <div id="page-stats" class="block-body collapse in">
            <h2>
                Welcome to Hotel Management Online Software</h2>
            <p>
                <span style="font-style: italic; font-weight: bold">InnBoard</span> is a hotel management
                system developed by Data Grid Limited. <span style="font-style: italic; font-weight: bold">
                    InnBoard</span> is web based hotel management software. Information is stored
                on the cloud so you can access your information anywhere there’s internet. <span
                    style="font-style: italic; font-weight: bold">InnBoard</span> provides basic
                and advanced customer base features. This Software is designed to accommodate the
                needs of various types of properties of the hotels, motels, resorts, clubs and small
                hotel franchisees.</p>
        </div>
    </div>
</asp:Content>
