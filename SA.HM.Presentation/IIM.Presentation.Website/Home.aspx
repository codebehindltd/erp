<%@ Page Title="" Language="C#" MasterPageFile="~/InnBoard.Master" AutoEventWireup="true"
    CodeBehind="Home.aspx.cs" Inherits="HotelManagement.Presentation.Website.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .carousel-inner > .item > img, .carousel-inner > .item > a > img
        {
            width: 100%;
            margin: auto;
        }
    </style>
    <%--<div class="container">--%>
        <div id="slider1" style="display:none;">
            <div class="flexslider">
                <ul class="slides">
                    <%-- <li>
                        <img src="/StyleSheet/images/banner8.jpg" alt=""/>
                    </li>
                    <li>
                        <img src="/StyleSheet/images/banner10.jpg" alt=""/>
                    </li>
                    <li>
                        <img src="/StyleSheet/images/banner12.jpg" alt=""/>
                    </li>--%>
                    <li>
                        <img src="/StyleSheet/images/banner5.jpg" alt="" />
                    </li>
                </ul>
                <%--<ul class="slides">
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
                </ul>--%>
            </div>
        </div>
        <div id="myCarousel" class="carousel slide" data-ride="carousel">
            <!-- Indicators -->
            <ol class="carousel-indicators">
                <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
                <li data-target="#myCarousel" data-slide-to="1"></li>
                <li data-target="#myCarousel" data-slide-to="2"></li>
                <li data-target="#myCarousel" data-slide-to="3"></li>
            </ol>
            <!-- Wrapper for slides -->
            <div class="carousel-inner" role="listbox">
                <div class="item active">
                    <img src="/StyleSheet/images/banner5.jpg" alt="Chania">
                </div>
                <div class="item">
                    <img src="/StyleSheet/images/banner5.jpg" alt="Chania">
                </div>
                <div class="item">
                    <img src="/StyleSheet/images/banner5.jpg" alt="Flower">
                </div>
                <div class="item">
                    <img src="/StyleSheet/images/banner5.jpg" alt="Flower">
                </div>
            </div>
            <!-- Left and right controls -->
            <a class="left carousel-control" href="#myCarousel" role="button" data-slide="prev">
                <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span><span class="sr-only">
                    Previous</span> </a><a class="right carousel-control" href="#myCarousel" role="button"
                        data-slide="next"><span class="glyphicon glyphicon-chevron-right" aria-hidden="true">
                        </span><span class="sr-only">Next</span> </a>
        </div>
    <%--</div>--%>
    <div class="alert-information-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        Welcome user!
    </div>
    <div class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse"><i class="icon-info-sign">
        </i>Hotel Management Information</a>
        <div id="page-stats" class="block-body collapse in">
            <h2>
                Welcome to Innboard - A Hotel Management ERP Solution</h2>
            <p>
                In the current competitive environment in hospitality industries through out the
                world, it is very important to have a very user friendly PMS in place for all hotels,
                motels, resorts and clubs. The entire systems must be equally user friendly for
                both, the guests and as well as for hotel associates who are providing different
                services to the valued guests. Keeping the issue of guest satisfactions in mind,
                Data Grid Limited has spent a considerable amount of time and resources with proper
                expertise and came up with a world class PMS, the Innboard. While the product Innboard
                PMS is running in different properties over a couple of countries, the R&D team
                of Data Grid Limited is constantly busy updating the existing features of Innboard
                and developing new features to it according to the expectations of the guests that
                are changing very fast under modern technologies. This R&D initiatives of Data Grid
                Ltd geared towards coming up with updated versions of the product. Currently, the
                Innboard PMS is revolving around the hospitality industries with required features
                and modules.
            </p>
        </div>
    </div>
</asp:Content>
