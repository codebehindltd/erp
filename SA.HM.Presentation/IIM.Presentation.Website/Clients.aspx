<%@ Page Title="" Language="C#" MasterPageFile="~/InnBoard.Master" AutoEventWireup="true"
    CodeBehind="Clients.aspx.cs" Inherits="HotelManagement.Presentation.Website.Clients" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="EntryPanel" class="panel panel-default">
        <%--<a href="#page-stats" class="block-heading" data-toggle="collapse"><i class="icon-info-sign">
        </i>Our Clients</a>--%>
        <div class="panel-heading">
            <i class="icon-info-sign"></i><b>&nbsp;&nbsp; Our Clients</b></div>
        <div class="panel-body" style="padding-left: 30px; padding-bottom: 30px;">
            <div class="row" style="box-shadow: 2px 2px 2px #888888; margin: 0 1px 5px 0; padding-left: 5px;">
                <h4>
                    Local Clients</h4>
            </div>
            <div class="row" style="padding-top: 10px;">
                <div class="col-md-4" style="float: left; padding: 0 10px 0 10px; margin: 0;">
                    <fieldset style="border: 1px solid; padding: 5px;">
                        <legend style="border-bottom: 0; margin-bottom: 0; width: 18%">Hotel 71</legend>
                        <%--<div class="HMContainerRow">
                        <h2 style="text-align: left;">
                            Hotel 71 :</h2>
                    </div>--%>                                   
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Address:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                176 Shaheed Syed Nazrul Islam Sarani,<br />
                                Dhaka, 1000, Bangladesh.
                            </div>
                        </div>                        
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Contact No:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                +880 28311 238, +880 28319 962
                            </div>
                        </div>                       
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Email:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                info@hotel71.com
                            </div>
                        </div>                       
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Web:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                <a target="_blank" href="http://www.hotel71bd.com/">www.hotel71bd.com</a>
                            </div>
                        </div>
                    </fieldset>
                </div>
                <div class="col-md-4" style="float: left; padding: 0 10px 0 10px; margin: 0;">
                    <fieldset style="border: 1px solid; padding: 5px;">
                        <legend style="border-bottom: 0; margin-bottom: 0; width: 28%">Hotel Agrabad</legend>
                        <%-- <div class="HMContainerRow">
                    <h2 style="text-align: left;">
                        Hotel Agrabad :</h2>
                </div>--%>                       
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Address:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                Agrabad Commercial Area
                                <br />
                                Chittagong, Bangladesh.
                            </div>
                        </div>                        
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Contact No:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                +88-031-713311-8
                            </div>
                        </div>                        
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Email:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                info@agrabadhotels.com
                            </div>
                        </div>                        
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Web:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                <a target="_blank" href="http://www.agrabadhotel.com/">www.agrabadhotel.com</a>
                            </div>
                        </div>
                    </fieldset>
                </div>
                <div class="col-md-4" style="padding: 0 10px 0 10px; margin: 0;">
                    <fieldset style="border: 1px solid; padding: 5px;">
                        <legend style="border-bottom: 0; margin-bottom: 0; width: 41%">FARS Hotel & Resorts</legend>
                        <%--<div class="HMContainerRow">
                    <h2 style="text-align: left;">
                        FARS Hotel & Resorts :</h2>
                </div>--%>                       
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Address:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                212, Shahid Syed Nazrul Islam Sharani (Bijoynagar) Dhaka-1000, Bangladesh.
                            </div>
                        </div>                       
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Contact No:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                +88 02 9515014, 9515013
                            </div>
                        </div>                       
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Email:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                info@farshotelbd.com
                            </div>
                        </div>                       
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Web:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                <a target="_blank" href="http://www.farshotelbd.com">www.farshotelbd.com</a>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>
            <div class="row" style="padding-top: 10px;">
                <div class="col-md-4" style="float: left; padding: 0 10px 0 10px; margin: 0;">
                    <fieldset style="border: 1px solid; padding: 5px;">
                        <legend style="border-bottom: 0; margin-bottom: 0; width: 34%">Hotel Comfort Inn</legend>
                        <%--<div class="HMContainerRow">
                    <h2 style="text-align: left;">
                        FARS Hotel & Resorts :</h2>
                </div>--%>                                 
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Address:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                Sector-06, Uttara, Dhaka, Bangladesh.
                            </div>
                        </div>                       
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Contact No:</b>
                            </div>
                            <div class="col-md-8">
                                <br />
                            </div>
                        </div>                     
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Email:</b>
                            </div>
                            <div class="col-md-8">
                                <br />
                            </div>
                        </div>                       
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Web:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                <a target="_blank" href="http://www.comfortinndhaka.com">www.comfortinndhaka.com</a>
                            </div>
                        </div>                       
                    </fieldset>
                </div>
                <div class="col-md-4" style="float: left; padding: 0 10px 0 10px; margin: 0;">
                    <fieldset style="border: 1px solid; padding: 5px;">
                        <legend style="border-bottom: 0; margin-bottom: 0; width: 32%">Hotel Graver Inn</legend>
                        <%--<div class="HMContainerRow">
                    <h2 style="text-align: left;">
                        FARS Hotel & Resorts :</h2>
                </div>--%>                        
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Address:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                Kuakata, Patuakhali, Bangladesh.
                            </div>
                        </div>                        
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Contact No:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                +8801748723557
                            </div>
                        </div>                       
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Email:</b>
                            </div>
                            <div class="col-md-8">
                                <br />
                            </div>
                        </div>                        
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Web:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                <a target="_blank" href="http://www.hotelgraverinn.com">www.hotelgraverinn.com</a>
                            </div>
                        </div>
                    </fieldset>
                </div>
                <div class="col-md-4" style="padding: 0 10px 0 10px; margin: 0;">
                    <fieldset style="border: 1px solid; padding: 5px;">
                        <legend style="border-bottom: 0; margin-bottom: 0; width: 25%">Chuti Resort</legend>
                        <%--<div class="HMContainerRow">
                    <h2 style="text-align: left;">
                        FARS Hotel & Resorts :</h2>
                </div>--%>                        
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Address:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                Sukundi, Amtoli, Joydebpur, Gazipur.
                            </div>
                        </div>                        
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Contact No:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                +8801777114488
                            </div>
                        </div>                        
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Email:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                info@chutibd.com
                            </div>
                        </div>                       
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Web:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                <a target="_blank" href="http://www.chutiresort.com">www.chutiresort.com</a>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>
            <div class="row" style="padding-top: 10px;">
                <div class="col-md-4" style="float: left; padding: 0 10px 0 10px; margin: 0;">
                    <fieldset style="border: 1px solid; padding: 5px;">
                        <legend style="border-bottom: 0; margin-bottom: 0; width: 27%">Swiss Palace</legend>
                        <%--<div class="HMContainerRow">
                    <h2 style="text-align: left;">
                        FARS Hotel & Resorts :</h2>
                </div>--%>                        
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Address:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                House # 46, Road # 10, Sector # 06 Uttara, Dhaka, Bangladesh.
                            </div>
                        </div>                        
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Contact No:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                +8801755683418
                            </div>
                        </div>                       
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Email:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                islamsha6@gmail.com
                            </div>
                        </div>                        
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Web:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                <a target="_blank" href="http://www.hotelswisspalace.com">www.hotelswisspalace.com</a>
                            </div>
                        </div>
                    </fieldset>
                </div>
                <div class="col-md-4" style="float: left; padding: 0 10px 0 10px; margin: 0;">
                    <fieldset style="border: 1px solid; padding: 5px;">
                        <legend style="border-bottom: 0; margin-bottom: 0; width: 33%">Hotel The Capital</legend>
                        <%--<div class="HMContainerRow">
                    <h2 style="text-align: left;">
                        FARS Hotel & Resorts :</h2>
                </div>--%>                       
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Address:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                69, naya paltan, Dhaka-1000, Bangladesh.
                            </div>
                        </div>                        
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Contact No:</b>
                            </div>
                            <div class="col-md-8">
                                <br />
                            </div>
                        </div>                        
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Email:</b>
                            </div>
                            <div class="col-md-8">
                                <br />
                            </div>
                        </div>                        
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Web:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                <a target="_blank" href="http://www.hotelthecapital.songzog.com">www.hotelthecapital.songzog.com</a>
                            </div>
                        </div>
                    </fieldset>
                </div>
                <div class="col-md-4" style="padding: 0 10px 0 10px; margin: 0;">
                    <fieldset style="border: 1px solid; padding: 5px;">
                        <legend style="border-bottom: 0; margin-bottom: 0; width: 35%">Hotel Dhaka Today</legend>
                        <%--<div class="HMContainerRow">
                    <h2 style="text-align: left;">
                        FARS Hotel & Resorts :</h2>
                </div>--%>                        
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Address:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                Banani, Dhaka, Bangladesh.
                            </div>
                        </div>                        
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Contact No:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                +8801781747175
                            </div>
                        </div>                        
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Email:</b>
                            </div>
                            <div class="col-md-8">
                                <br />
                            </div>
                        </div>                        
                        <div class="HMContainerRow">
                            <div class="col-md-4">
                                <%--Left Left--%>
                                <b>Web:</b>
                            </div>
                            <div class="col-md-8">
                                <%--Right Left--%>
                                <%--<a target="_blank" href="http://www.farshotelbd.com">www.farshotelbd.com</a>--%>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>
            <div class="row" style="box-shadow: 2px 2px 2px #888888; margin: 0 1px 5px 0; padding-left: 5px;
                margin-top: 20px;">
                <h4>
                    Foreign Clients</h4>
            </div>
            <div class="row" style="padding-top: 10px;">
            <div class="col-md-4" style="float: left; padding: 0 10px 0 10px; margin: 0;">
                <fieldset style="border: 1px solid; padding: 5px;">
                    <legend style="border-bottom: 0; margin-bottom: 0; width: 70%">THE PANORAMA HOTEL</legend>
                    <%--<div class="HMContainerRow">
                        <h2 style="text-align: left;">
                            Hotel 71 :</h2>
                    </div>--%>                    
                    <div class="HMContainerRow">
                        <div class="col-md-4">
                            <%--Left Left--%>
                            <b>Address:</b>
                        </div>
                        <div class="col-md-8">
                            <%--Right Left--%>
                            Hai Amarat, Juba, Republic of South Sudan.
                        </div>
                    </div>                    
                    <div class="HMContainerRow">
                        <div class="col-md-4">
                            <%--Left Left--%>
                            <b>Contact No:</b>
                        </div>
                        <div class="col-md-8">
                            <%--Right Left--%>
                            (+211) 954657949
                        </div>
                    </div>                   
                    <div class="HMContainerRow">
                        <div class="col-md-4">
                            <%--Left Left--%>
                            <b>Email:</b>
                        </div>
                        <div class="col-md-8">
                            <%--Right Left--%>
                            info@panoramahotel-juba.com
                        </div>
                    </div>                   
                    <div class="HMContainerRow">
                        <div class="col-md-4">
                            <%--Left Left--%>
                            <b>Web:</b>
                        </div>
                        <div class="col-md-8">
                            <%--Right Left--%>
                            <a target="_blank" href="http://www.panoramahotel-juba.com/">www.panoramahotel-juba.com</a>
                        </div>
                    </div>
                </fieldset>
            </div>
            <div class="col-md-4" style="float: left; padding: 0 10px 0 10px; margin: 0;">
                <fieldset style="border: 1px solid; padding: 5px;">
                    <legend style="border-bottom: 0; margin-bottom: 0; width: 39%">Nimule Resort</legend>
                    <%-- <div class="HMContainerRow">
                    <h2 style="text-align: left;">
                        Hotel Agrabad :</h2>
                </div>--%>                    
                    <div class="HMContainerRow">
                        <div class="col-md-4">
                            <%--Left Left--%>
                            <b>Address:</b>
                        </div>
                        <div class="col-md-8">
                            <%--Right Left--%>
                            Hai Amarat, Juba, Republic of South Sudan.
                        </div>
                    </div>                    
                    <div class="HMContainerRow">
                        <div class="col-md-4">
                            <%--Left Left--%>
                            <b>Contact No:</b>
                        </div>
                        <div class="col-md-8">
                            <%--Right Left--%>
                            +211 955-000890
                        </div>
                    </div>                    
                    <div class="HMContainerRow">
                        <div class="col-md-4">
                            <%--Left Left--%>
                            <b>Email:</b>
                        </div>
                        <div class="col-md-8">
                            <%--Right Left--%>
                            info@nimule-resorts.com
                        </div>
                    </div>                    
                    <div class="HMContainerRow">
                        <div class="col-md-4">
                            <%--Left Left--%>
                            <b>Web:</b>
                        </div>
                        <div class="col-md-8">
                            <%--Right Left--%>
                            <a target="_blank" href="http://www.nimule.net/">www.nimule.net</a>
                        </div>
                    </div>
                </fieldset>
            </div>
            <div class="col-md-4" style="padding: 0 10px 0 10px; margin: 0;">
                <fieldset style="border: 1px solid; padding: 5px;">
                    <legend style="border-bottom: 0; margin-bottom: 0; width: 70%">DEMBESH HOTEL JUBA</legend>
                    <%--<div class="HMContainerRow">
                    <h2 style="text-align: left;">
                        FARS Hotel & Resorts :</h2>
                </div>--%>                    
                    <div class="HMContainerRow">
                        <div class="col-md-4">
                            <%--Left Left--%>
                            <b>Address:</b>
                        </div>
                        <div class="col-md-8">
                            <%--Right Left--%>
                            Hai Mission, Opposite to juba Football Stadium, Juba – South Sudan.
                        </div>
                    </div>                    
                    <div class="HMContainerRow">
                        <div class="col-md-4">
                            <%--Left Left--%>
                            <b>Contact No:</b>
                        </div>
                        <div class="col-md-8">
                            <%--Right Left--%>
                            +211-959-004-004
                        </div>
                    </div>                    
                    <div class="HMContainerRow">
                        <div class="col-md-4">
                            <%--Left Left--%>
                            <b>Email:</b>
                        </div>
                        <div class="col-md-8">
                            <%--Right Left--%>
                            info@dembeshotel.com
                        </div>
                    </div>                   
                    <div class="HMContainerRow">
                        <div class="col-md-4">
                            <%--Left Left--%>
                            <b>Web:</b>
                        </div>
                        <div class="col-md-8">
                            <%--Right Left--%>
                            <a target="_blank" href="http:// www.dembeshotel.com">www.dembeshotel.com</a>
                        </div>
                    </div>
                </fieldset>
            </div>
            </div>
        </div>
    </div>
</asp:Content>
