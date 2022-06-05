<%@ Page Title="" Language="C#" MasterPageFile="~/InnBoard.Master" AutoEventWireup="true"
    CodeBehind="ContactUs.aspx.cs" Inherits="HotelManagement.Presentation.Website.ContactUs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="EntryPanel" class="panel panel-default">
        <%--<a href="#page-stats" class="block-heading" data-toggle="collapse">Contact Us: Data
            Grid Limited </a>--%>
            <div class="panel-heading">Contact Us: Data
            Grid Limited</div>
        <div class="panel-body" style="padding-left: 100px; padding-bottom: 30px;">
            <div class="col-md-8">
                <div style="padding-top: 40px;">
                    <%--<script src='https://maps.googleapis.com/maps/api/js?v=3.exp'></script>--%>
                    <script src='https://maps.googleapis.com/maps/api/js?key=AIzaSyCrWy2ZWKj9Wj-E7DOrW747KEScN5Bo6XU'></script>                     
                    <div style='overflow: hidden; height: 300px; width: 580px;'>
                        <div id='gmap_canvas' style='height: 300px; width: 580px;'>
                        </div>
                        <div>
                            <small><a href="http://embedgooglemaps.com">embed google maps</a></small></div>
                        <div>
                            <small><a href="freedirectorysubmissionsites.com">complete list</a></small></div>
                        <style>
                            #gmap_canvas img
                            {
                                max-width: none !important;
                                background: none !important;
                            }
                        </style>
                    </div>
                    <script type='text/javascript'>                        function init_map() { var myOptions = { zoom: 15, center: new google.maps.LatLng(23.8733124, 90.38342490000002), mapTypeId: google.maps.MapTypeId.ROADMAP }; map = new google.maps.Map(document.getElementById('gmap_canvas'), myOptions); marker = new google.maps.Marker({ map: map, position: new google.maps.LatLng(23.8733124, 90.38342490000002) }); infowindow = new google.maps.InfoWindow({ content: '<strong>Data Grid Ltd Location</strong><br>House #04, Road #19, Sector #11 Uttara, Dhaka - 1230.<br>' }); google.maps.event.addListener(marker, 'click', function () { infowindow.open(map, marker); }); infowindow.open(map, marker); } google.maps.event.addDomListener(window, 'load', init_map);</script>
                </div>
            </div>
            <div class="col-md-4">
                <div>
                    <div class="HMContainerRow">
                        <h2 style="text-align: left;">
                            Bangladesh Office:</h2>
                    </div>                    
                    <div class="HMContainerRow">
                        <div class="col-md-4">
                            <%--Left Left--%>
                            Address:
                        </div>
                        <div class="col-md-8">
                            <%--Right Left--%>
                            House #04 (4th Floor), Road #19, Sector #11<br>
                            Uttara, Dhaka - 1230.
                        </div>
                    </div>                    
                    <div class="HMContainerRow">
                        <div class="col-md-4">
                            <%--Left Left--%>
                            Phone:
                        </div>
                        <div class="col-md-8">
                            <%--Right Left--%>
                            +88 02 58950950
                        </div>
                    </div>                   
                    <div class="HMContainerRow">
                        <div class="col-md-4">
                            <%--Left Left--%>
                            Cell:
                        </div>
                        <div class="col-md-8">
                            <%--Right Left--%>
                            +88 01811 555666
                        </div>
                    </div>                    
                    <div class="HMContainerRow">
                        <div class="col-md-4">
                            Fax:
                        </div>
                        <div class="col-md-8">
                            +88 02 9003202
                        </div>
                    </div>                   
                    <div class="HMContainerRow">
                        <div class="col-md-4">
                            <%--Left Left--%>
                            Email:
                        </div>
                        <div class="col-md-8">
                            <%--Right Left--%>
                            <a href="mailto:info@datagridbd.com">info@datagridbd.com</a>
                        </div>
                    </div>
                </div> 
                            
                <div style="margin-top:150px;">
                 <br /> 
                    <div class="HMContainerRow">
                        <h2 style="text-align: left;">
                            Sudan Office:</h2>
                    </div>                   
                    <div class="HMContainerRow">
                        <div class="col-md-4">
                            <%--Left Left--%>
                            Contact No:
                        </div>
                        <div class="col-md-8">
                            <%--Right Left--%>
                            +211959003002
                        </div>
                    </div>                   
                    <div class="HMContainerRow">
                        <div class="col-md-4">
                            <%--Left Left--%>
                            Email:
                        </div>
                        <div class="col-md-8">
                            <%--Right Left--%>
                            <a href="mailto:sagar@datagridbd.com">sagar@datagridbd.com</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
