<%@ Page Title="" Language="C#" MasterPageFile="~/InnBoard.Master" AutoEventWireup="true"
    CodeBehind="Features.aspx.cs" Inherits="HotelManagement.Presentation.Website.Features" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-default" style="margin-top: 10px;">
        <%--<a href="#page-stats" class="block-heading" data-toggle="collapse"><i class="icon-info-sign">
        </i>About Innboard</a>--%>
        <div class="panel-heading">
            <i class="icon-info-sign"></i><b>&nbsp;&nbsp; About Innboard</b></div>
        <div id="page-stats" class="panel-body" style="padding-top: 10px;">
            <p>
                <span style="font-style: italic; font-weight: bold">Innboard</span> Software is
                designed to accommodate the needs of various type of properties such as hotels,
                motels, resorts, clubs, B & B's, franchisees, condo's, hostel and apartments. Innboard,
                so far has (upto 17 Oct, 2016) 9 installations in Bangladesh and 3 installations
                in South Sudan. Innboard is a modern solution which has a wide range of integrated
                modules to cover all aspects of property management. The generalized version of
                Innboard is widely accepted due to its state-of-art technology and extremely user
                friendly nature.
            </p>
            <%--<h2>
                InnBoard has in-built modules</h2>
            <div>
                <ul >
                    <li>Administration</li>
                    <li>Hotel Management</li>
                    <li>Restaurant management</li>
                    <li>Accounts Management</li>
                    <li>Payroll  Management</li>
                    <li>Store Management</li>
                </ul>
                </div>--%>
            <hr />
            <div class="row" style="padding-left: 50px;">
                <h3>
                    Innboard Modules
                </h3>
                <div class="col-md-4" style="float: left;">
                    <ul>
                        <li>Front Office</li>
                        <li>Housekeeping</li>
                        <li>Restaurant POS</li>
                        <li>Banquet</li>
                        <li>Procurement & Inventory</li>
                        <li>Accounts /Back Office</li>
                        <li>HR & Payroll</li>
                        <li>Sales & Marketing</li>
                    </ul>
                </div>
                <div class="col-md-4">
                    <img style="height: 220px;" src="Images/Innboard-Modules.png" alt="Innboard Modules" />
                </div>
            </div>
            <hr />
            <div class="row" style="padding-left: 50px;">
                <h3>
                    Benefits of Innboard PMS</h3>
                <div style="float: left;" class="col-md-6">
                    <ul>
                        <li>Cloud & Web based Hotel ERP system</li>
                        <li>Customized to fit your hotel's operational business needs</li>
                        <li>Multi-Property, Multi User and Multi-Currency Property Management</li>
                        <li>Multi-currency financial Accounting System</li>
                        <li>Accessibility or Report generation anytime from anywhere</li>
                        <li>Unlimited Point of Sale terminals</li>
                        <li>Travel Agent and Corporate Booking Consoles</li>
                        <li>Comprehensive Reporting and Revenue Management</li>
                        <li>Delivers fast, accurate and online information on your property</li>
                        <li>Complete online integration & Web Booking Engine at web site</li>
                    </ul>
                </div>
                <div class="col-md-6">
                    <ul>
                        <li>Direct Online Payments/Credit Card Processing</li>
                        <li>Error free calculation, auto totaling and posting</li>
                        <li>Validations available at all checkpoints</li>
                        <li>Fast data entry and reporting, saves upto 50% of your time</li>
                        <li>Efficient manpower utilization to reduce administration costs</li>
                        <li>Enhanced financial planning and operational efficiency</li>
                        <li>Improved transparency and accountability within property</li>
                        <li>Reduced operational costs and personalizes guest services</li>
                        <li>Cost center wise Profit and Loss report helps the Hotel improve profitability</li>
                        <li>Enhance Housekeeping Concierge Management</li>
                    </ul>
                </div>
            </div>
            <%--<h3>
                About “Innboard”</h3>
            <ul >
                    <li>Web based Hotel Management ERP system. Everything is integrating by one Server</li>
                    <li>Access from anywhere or in the Cloud or locally</li>
                    <li>Built in Financial Accounting System. So no need 3rd party accounting system integration</li>
                    <li>Multi-currency financial Accounting  System</li>
                    <li>Unlimited layer of chart of accounts with all financial accounting related reports </li>
                    <li>Integrated with Sales, Procurement & Inventory and Accounts</li>
                    <li>Recipe management system with stock deduction</li>
                    <li>Customized to fit your business operational needs</li>
                    <li>Helps you become more productive, profitable and professional</li>
                    <li>Unlimited Point of Sale terminals & unlimited number of users</li>
                    <li>Powerful Reporting and Revenue Management</li>
                    <li>Touch screen restaurant module interface & KOT generation by Tab or Smartphones</li>
                    <li>Reservation system integrated with restaurant module</li>
                    <li>Membership management</li>
                    <li>Integration of attendance device with the HR module</li>
                    <li>PABX & IP Phone integration for Hotel Management System</li>
                    <li>Making of Appointment schedule through Sales & marketing module</li>
                </ul>--%>
            <%--<h3>
                Restaurant Management</h3>
            <p>
                InnBoard Restaurant Management offers a total restaurant POS system that can be
                easily tailored for use in any sort of food service establishment, from fine dining
                and table service restaurants to quick service (QSR), as well as bars and clubs.
                <a href="/Features.aspx#resturent">Learn More</a></p>
            <h3>
                Accounts Management</h3>
            <p>
                Many business owners aren’t able to fully grasp the financial fundamentals of their
                organizations due to lack of sufficient reporting and tracking of transactions.
                Knowing where your business is headed is vital in creating a successful venture.
                <a href="/Features.aspx#accounts">Learn More</a></p>
            <h3>
                Payroll Management</h3>
            <p>
                The Payroll Management System deals with the financial aspects of employee's salary,
                allowances, deductions, gross pay, net pay etc. and generation of pay-slips for
                a specific period. <a href="/Features.aspx#payroll">Learn More</a></p>
            <h3>
                Store Management</h3>
            <p>
                A rather important module, inventory management lets managers automate the process
                of tracking rooms, and food and beverage consumption in the hotel. I am sure many
                inventory managers will agree that manually filing cash memos and getting clearance
                from finance department to pay vendors was a nightmare and a huge waste of effort.
                <a href="/Features.aspx#store">Learn More</a></p>--%>
            <hr />
            <%-- <h2>
                InnBoard Benefits that can make straightforward hotel operations</h2>--%>
            <div id="menu" class="row" style="padding-left: 50px;">
                <%--<ul >
                    <li>InnBoard comes with core business modules</li>
                    <li>With improved process efficiency, you could control the cost and ensure maximized ROI.</li>
                    <li>Error free calculation, auto totaling and posting.</li>
                    <li>Validations available at all checkpoints. No scope for errors.</li>
                    <li>Fast data entry and reporting, saves up to 50% of your time.</li>
                    <li>MIS reporting.</li>
                    <li>Efficient manpower utilization to reduce administration costs.</li>
                    <li>Enhanced financial planning and operational efficiency.</li>
                    <li>Improved transparency and accountability within property.</li>
                    <li>Enhanced decision making capabilities.</li>
                    <li>Reduced operational costs and personalized guest services.</li>
                    <li>Cost center wise Profit and Loss report helps the hotel improve profitability.</li>
                    <li>Enhanced Housekeeping and Concierge Management.</li>

                </ul>--%>
                <h3>
                    Innboard support</h3>
                <div class="col-md-8" style="float: left;">
                    <div>
                        <ul>
                            <li>A Multi-level Support System from Head office, branches and partner support available.</li>
                            <li>Bug fixing including patch updates for bugs reported by customer and telephonic
                                assistance.</li>
                            <li>Support is ensured by Annual Support Agreements with customer in terms of AUS /AMC.</li>
                            <li>Includes periodic upgrades to product to improve functionality & implement changes.</li>
                            <li>Includes provision for upgrades and patches.</li>
                            <li>Specific technical issue based support.</li>
                            <li>Installation Support.</li>
                            <li>Training Support.</li>
                        </ul>
                    </div>
                </div>                
            </div>
            <div class="row">
                    <img style="height: 220px; padding-left: 150px;" src="Images/Customer_support_2.jpg"
                        alt="Innboard Modules" />
                </div>
        </div>
    </div>
</asp:Content>
