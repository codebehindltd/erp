<%@ Page Title="" Language="C#" MasterPageFile="~/InnBoard.Master" AutoEventWireup="true"
    CodeBehind="Features.aspx.cs" Inherits="HotelManagement.Presentation.Website.Features" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="StyleSheet/css/feature.css" rel="stylesheet" type="text/css" />
    <div class="panel panel-default">
        <%--<a href="#page-stats" class="block-heading" data-toggle="collapse"><i class="icon-info-sign">
        </i>InnBoard Feature</a>--%>
        <div class="panel-heading">
            <i class="icon-info-sign"></i><b>&nbsp;&nbsp; Our Clients</b>InnBoard Feature</div>
        <div id="page-stats" class="panel-body">
            <%--<h2>
                InnBoard has in-built modules to assist your hotel with</h2>
            <div id="menu">
                <ul>
                    <li><a href="#administration">Administration </a></li>
                    <li><a href="#hotelmanagement">Hotel Management</a></li>
                    <li><a href="#resturent">Service Management (Room Service, Laundry, Mini-Bar etc )</a></li>
                    <li><a href="#accounts">House-keeping Management </a></li>
                    <li><a href="#payroll">Accounting/ Revenue Management</a></li>
                    <li><a href="#store">Point of Sale</a></li>
                </ul>
            </div>--%>
            <div id="administration" style="padding-top: 5px;">
                <p class="header">
                    <strong>Administration</strong></p>
                <hr />
                <div class="row">
                    <div class="left col-md-3">
                        <img class="image" src="Images/administration.png" alt="administration" /></div>
                    <div class="leftWithPixel col-md-4">
                        This is an administrative feature to properly control the usage of the entire PMS
                        Systems of a particular property. Tasks/sub modules under this feature are recommended
                        to be carried out by the person in charged of IT, usually called IT Administrator,
                        IT Manager or MIS Manager depending on the size and type of property.</div>
                    <div class="menu col-md-4">
                        <span><strong>Features:</strong></span>
                        <ul>
                            <li>User Group Management</li>
                            <li>Database Management</li>
                            <li>Create and Delete User, Log in ID and Password</li>
                            <li>Assigning Permissions to Users</li>
                            <li>Backup and Restore Functions</li>
                            <li>Trail Records of User Login</li>
                            <li>Training Version</li>
                        </ul>
                    </div>
                </div>
                <%--<div class="right row">
                    <a href="#menu">Back to Modules</a></div>--%>
            </div>
            <div id="Dashboard">
                <hr />
                <p class="header">
                    <strong>Dashboard</strong></p>
                <hr />
                <div class="row">
                    <div class="left col-md-3">
                        <img class="image" src="Images/Dashboard.png" alt="Dashboard" /></div>
                    <div class="leftWithPixel col-md-8">
                        This is a display screen to view the real time online current statistical and financial
                        data of the hotel’s operations such as Occ%, ARR, RevPar, VIP, # of vacant rooms,
                        # of out of order rooms etc, as per the requirement of the Management for them to
                        make instant or short term decision based on the data and information shows on the
                        screen. Since the data shows as real time online, it changes with every transaction.
                        The statistical data are presented in numbers and as well as with graphical representation.</div>
                    <%--<div class="menu">
                    <span><strong>Features:</strong></span>
                    <ul>
                        <li>User Group Management</li>
                        <li>Database Management</li>
                        <li>Create and Delete User, Log in ID and Password</li>
                        <li>Assigning Permissions to Users</li>
                        <li>Backup and Restore Functions</li>
                        <li>Trail Records of User Login</li>
                        <li>Training Version</li>
                    </ul>
                </div>--%>
                </div>
                <%--<div class="right row">
                    <a href="#menu">Back to Modules</a></div>--%>
            </div>
            <div id="hotelmanagement">
                <hr />
                <p class="header">
                    <strong>Front Office Management</strong></p>
                <hr />
                <div class="row">
                    <div class="left col-md-3" style="float: left">
                        <img class="image" src="Images/Front-Office.png" alt="Front Office Management" /></div>
                    <div class="leftWithPixel col-md-8">
                        Front Office is really the heart of hotel operations. Front Office is comprised
                        of the following sections of a full serviced star rated hotel. In Innboard Front
                        Office Management are designed follows..
                        <br />
                        <div style="float: left; padding-right: 10px;">
                            <b>Rooms Reservation</b>
                            <br />
                            <div style="padding-left: 10px;">
                                <ul>
                                    <li>Reservation by Companies</li>
                                    <li>Reservation by Travel Agents</li>
                                    <li>Reservation by Guests</li>
                                    <li>Reservation by Commission Agents</li>
                                    <li>Group Reservations</li>
                                </ul>
                            </div>
                        </div>
                        <div>
                            <b>Front Desk</b>
                            <br />
                            <b>PABX</b>
                            <br />
                            <b>Business Center</b>
                            <br />
                            <b>Concierge (Bell Desk)</b>
                            <br />
                            <b>Transport</b>
                            <br />
                            <b>Airport Counter</b>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="menu col-md-4" style="margin-left: 220px;">
                        <span><strong>Features:</strong></span>
                        <ul>
                            <li>Room status - available/reserved</li>
                            <li>Room availability lookup, summary & stay information grid</li>
                            <li>Details for a returning guest automatically filled in the reservation screen</li>
                            <li>Guest details: name, address, email address, fax number, phone number, credit card(s),
                                company, member/number, preferences, free format notes</li>
                            <li>Credit limits for guests, companies, Agents etc</li>
                            <li>History of guest hotel stays including dates, rates paid, total expenditure, sharers,
                                preferences, notes</li>
                            <li>Search for a reservation by last name, first name, group name or confirmation number
                                all from a single search</li>
                            <li>Override standard room rates/packages with appropriate authority</li>
                        </ul>
                    </div>
                    <div class="secondmenu col-md-4">
                        <ul>
                            <li>Special guest requests including for a specific room, adjoining rooms, same floor
                                rooms</li>
                            <li>Late arrival/checkin, early checkin, late checkout (with option to charge extra)</li>
                            <li>Extra bed/cot</li>
                            <li>Group reservations (multiple rooms under one name/bill)</li>
                            <li>Record name & room number of group leader</li>
                            <li>Inquiry on guests scheduled to arrive/depart on a specific date</li>
                            <li>Sources of business/travel agent codes and information (for calculation of and payment
                                of commission)</li>
                            <li>Collection of deposits for room reservations</li>
                            <li>Reservation confirmations by email or fax</li>
                        </ul>
                    </div>
                </div>
                <%--<div class="right row">
                    <a href="#menu">Back to Modules</a></div>--%>
            </div>
            <div id="Housekeeping">
                <hr />
                <p class="header">
                    <strong>Housekeeping</strong></p>
                <hr />
                <div class="row">
                    <div class="left col-md-3">
                        <img class="image" src="Images/Housekeeper.jpg" alt="Inventory with recipe  Management"
                            style="height: 120px;" /></div>
                    <div class="leftWithPixel col-md-3">
                        The Housekeeping department takes care of the guests, employees, general public
                        and the property as regard to the cleanliness through proving their services over
                        the following areas.
                    </div>
                    <div class="menu col-md-3" style="margin-top: 5px;">
                        <span><strong>Features:</strong></span>
                        <ul>
                            <li>Floor Management & Room Allocation</li>
                            <li>Room & Service Management</li>
                            <li>Room Calendar with Room Status</li>
                            <li>Room Change & auto record</li>
                            <li>Late Check-Out</li>
                            <li>Mini Bar posting & refill, D&D</li>
                            <li>Room Conditions</li>
                            <li>Room Discrepancies</li>
                        </ul>
                    </div>
                    <div class="secondmenu col-md-3">
                        <ul>
                            <li>Task Assignment</li>
                            <li>Turndown Management</li>
                            <li>Guest Service status</li>
                            <li>Room Inventory</li>
                            <li>Room Statistics</li>
                            <li>List of Arrival Amenities</li>
                            <li>Special Guest Requets & VIP List</li>
                        </ul>
                    </div>
                </div>
                <%--<div class="right row">
                    <a href="#menu">Back to Modules</a></div>--%>
            </div>
            <div id="resturent">
                <hr />
                <p class="header">
                    <strong>F&B Restaurant Management</strong></p>
                <hr />
                <div class="row">
                    <div class="left col-md-3">
                        <img class="image" src="Images/Restaurant.png" alt="Accounts management" /></div>
                    <%--<div class="leftWithPixel">
                    InnBoard Restaurant Management offers a total restaurant POS system that can be
                    easily tailored for use in any sort of food service establishment, from fine dining
                    and table service restaurants to quick service (QSR), as well as bars and clubs.
                    Designed as a turnkey restaurant POS system that can grow with your business, InnBoard
                    Restaurant Management system offers everything you need to effectively manage your
                    operations.
                </div>--%>
                    <div class="menu col-md-4">
                        <span><strong>Features:</strong></span>
                        <ul>
                            <li>Waiter & Cashier Management</li>
                            <li>Automatic KOT Generation with full Touch Screen Interface</li>
                            <li>Bill Void option</li>
                            <li>Payment method with invoice generation</li>
                            <li>Tagging each item image & smart search option</li>
                            <li>Accessible from smart phones, tabs & other devices</li>
                        </ul>
                    </div>
                    <div class="secondmenu col-md-4">
                        <ul>
                            <li>Table calendar & Reservation</li>
                            <li>Membership Management</li>
                            <li>Multi-layer menu setup with category & items</li>
                            <li>Recipe Management</li>
                            <li>Wastage Management with record</li>
                        </ul>
                    </div>
                </div>
                <%--<div class="right row">
                    <a href="#menu">Back to Modules</a></div>--%>
            </div>
            <div id="Banquet">
                <hr />
                <p class="header">
                    <strong>Banquet Management</strong></p>
                <hr />
                <div class="row">
                <div class="left col-md-3">
                    <img class="image" src="Images/BanquetHall.png" alt="Banquet Management" style="height: 120px;" /></div>
                <%--<div class="leftWithPixel">
                    A rather important module, inventory management lets managers automate the process
                    of tracking rooms, and food and beverage consumption in the hotel. I am sure many
                    inventory managers will agree that manually filing cash memos and getting clearance
                    from finance department to pay vendors was a nightmare and a huge waste of effort.
                    With the arrival of hospitality technology solutions, automation of the inventory
                    system means lesser work and greater visibility into stock, automated reminders
                    as stock levels diminish, faster decision making on stock maintenance in the hotel.
                </div>--%>
                <div class="menu col-md-4">
                    <span><strong>Features:</strong></span>
                    <ul>
                        <li>Occasion seating plan setup</li>
                        <li>Requisition Information entry</li>
                        <li>Reference entry option</li>
                        <li>Multiple banquet details entry</li>
                    </ul>
                </div>
                <div class="secondmenu col-md-4">
                    <ul>
                        <li>Reservation & cancelation</li>
                        <li>Confirmation letter with details</li>
                        <li>Banquet calendar</li>
                        <li>Total bill payment process with invoice</li>
                    </ul>
                </div>
                </div>
                <%--<div class="right row">
                <a href="#menu">Back to Modules</a></div>--%>
            </div>            
            
            <div id="Procurement">
                <hr />
                <p class="header">
                    <strong>Procurement & Inventory Management</strong></p>
                <hr />
                <div class="row">
                <div class="left col-md-3">
                    <img class="image" src="Images/procurement.png" alt="Procurement & Inventory Management" /></div>
                <%--<div class="leftWithPixel">
                    A rather important module, inventory management lets managers automate the process
                    of tracking rooms, and food and beverage consumption in the hotel. I am sure many
                    inventory managers will agree that manually filing cash memos and getting clearance
                    from finance department to pay vendors was a nightmare and a huge waste of effort.
                    With the arrival of hospitality technology solutions, automation of the inventory
                    system means lesser work and greater visibility into stock, automated reminders
                    as stock levels diminish, faster decision making on stock maintenance in the hotel.
                </div>--%>
                <div class="menu col-md-4">
                    <span><strong>Features:</strong></span>
                    <ul>
                        <li>Supplier Database</li>
                        <li>Product requisition </li>
                        <li>Requisition approval</li>
                        <li>Quotation Collect</li>
                        <li>Quotation Approval</li>
                        <li>Product Purchase witb Entry & Returns</li>
                        <li>Supplier details Report</li>
                        <li>Date, Supplier & Item wise Purchase report</li>
                    </ul>
                </div>
                <div class="secondmenu col-md-4">
                    <ul>
                        <li>PO Information</li>
                        <li>Shipment Information report</li>
                        <li>Product Requisition & Requisition approval</li>
                        <li>Product Receive & Product Out</li>
                        <li>Inventory Adjustment</li>
                        <li>Item Stock Variance</li>
                        <li>Cost control & Variance analysis</li>
                    </ul>
                </div>
                </div>
                <%--<div class="right row">
                <a href="#menu">Back to Modules</a></div>--%>
            </div>            
            
            <div id="accounts">
                <hr />
                <p class="header">
                    <strong>Accounts Management</strong></p>
                <hr />
                <div class="row">
                <div class="left col-md-3">
                    <img class="image" src="Images/accounts.png" alt="Accounts Management" style="height: 130px;" /></div>
                <%--<div class="leftWithPixel">
                    Many business owners aren’t able to fully grasp the financial fundamentals of their
                    organizations due to lack of sufficient reporting and tracking of transactions.
                    Knowing where your business is headed is vital in creating a successful venture.
                    Therefore, an easy and cost-effective means of monitoring financial dealings is
                    required. Accounting software doesn’t only assist in managing and tracking cash
                    procedures, it also helps to determine the success of your business’s performance.
                    Utilizing quality cash flow software can yield significant benefits for your business.</div>--%>
                <div class="menu col-md-4">
                    <span><strong>Features:</strong></span>
                    <ul>
                        <li>Chart of Accounts</li>
                        <li>Opening Balance</li>
                        <li>Journal Voucher entry</li>
                        <li>Received & Payment Vouchers entry</li>
                        <li>Contra Vouchers entry</li>
                        <li>Credit note & Debit note</li>
                        <li>Accounts Receivable</li>
                        <li>Accounts Payable</li>
                        <%--<li>Bank Book Statement</li>
                        <li>General Ledger Daily/Monthly/Yearly</li>
                        <li>Income statement</li>--%>
                    </ul>
                </div>
                <div class="secondmenu col-md-4">
                    <ul>
                        <li>Trial Balance</li>
                        <li>Cash Flow Statement</li>
                        <li>Transaction List</li>
                        <li>Cash Book Statement</li>
                        <li>Bank Book Statement</li>
                        <li>General Ledger Daily/Monthly/Yearly</li>
                        <li>Income statement</li>
                        <li>Balance Sheet</li>
                        <%--<li>Accounts Payable</li>
                        <li>Accounts Receivable</li>
                        <li>Project Accounting</li>
                        <li>Daly/Monthly/Yearly Ledgers</li>
                        <li>Supplier Ledger</li>
                        <li>Customer ledger</li>
                        <li>Trail Balance</li>
                        <li>Cash Flow Statement</li>
                        <li>Balance Sheet</li>
                        <li>Profit and Loss Account</li>--%>
                    </ul>
                </div>
                </div>
               <%-- <div class="right row">
                <a href="#menu">Back to Modules</a></div>--%>
            </div>            
            
            <div id="payroll">
                <hr />
                <p class="header">
                    <strong>HR & Payroll Management</strong></p>
                <hr />
                <div class="row">
                <div class="left col-md-3">
                    <img class="image" src="Images/payroll.png" alt="HR & Payroll Managemen" /></div>
                <%--<div class="leftWithPixel">
                    The Payroll Management System deals with the financial aspects of employee's salary,
                    allowances, deductions, gross pay, net pay etc. and generation of pay-slips for
                    a specific period. The outstanding benefit of Payroll Management System is its easy
                    implementation. Other advantages of Payroll Management System are its extensive
                    features and reports .
                </div>--%>
                <div class="menu col-md-4">
                    <span><strong>Features:</strong></span>
                    <ul>
                        <li>Salary, Overtime, Leave & Bonus setup</li>
                        <li>TAX, Provident Fund, Loan & Gratuity</li>
                        <li>Department & Grade setup</li>
                        <li>Employee designation & type setup</li>
                        <li>Staff budget & requisition</li>
                        <li>Job Circular & Resume Bank</li>
                        <li>Interview Evaluation</li>
                        <li>Appointment Letter & Relieving Letter</li>
                        <li>Employee Information with details</li>
                        <li>Salary Head & Salary Formula</li>
                        <li>Leave & Holiday Type</li>
                    </ul>
                </div>
                <div class="secondmenu col-md-4">
                    <ul>
                        <li>Working Plan</li>
                        <li>Roaster & Time Slab Head setup</li>
                        <li>Employee Leave & Roster setup</li>
                        <li>Employee Increment, Allowance & Deduction</li>
                        <li>Employee Manual & Automatic Attendance</li>
                        <li>Employee Overtime Calculation</li>
                        <li>Loan Sanction & Loan Search</li>
                        <li>Appraisal & Appraisal Evaluation </li>
                        <li>Employee Training Process</li>
                        <li>Employee of the month/ year</li>
                        <li>Monthly Salary Process</li>
                    </ul>
                </div>
                </div>
                <%--<div class="right row">
                <a href="#menu">Back to Modules</a></div>--%>
            </div>            
            
            <div id="SalesNMarketing">
                <hr />
                <p class="header">
                    <strong>Sales and Marketing</strong></p>
                <hr />
                <div class="row">
                <div class="left col-md-3">
                    <img class="image" src="Images/salesnmarketing.jpg" alt="Sales & Marketing" style="height: 120px;" /></div>
                <div class="leftWithPixel col-md-4">
                    Sales and Marketing Department is responsible to keep all records of Travel Agencies,
                    Commission Agents, Local Companies and their respective productivity. In addition,
                    the Department is also responsible for marketing plan, marketing activities and
                    different loyalty and recognition programs through the following menu items and
                    maintain them.
                </div>
                <div class="menu col-md-4">
                    <span><strong>Features:</strong></span>
                    <ul>
                        <li>Travel Agencies </li>
                        <li>Commission Agents </li>
                        <li>Local Company</li>
                        <li>Discount Details</li>
                        <li>Commission Details</li>
                        <li>Productivity Report</li>
                        <li>Other Reports as require</li>
                    </ul>
                </div>
                <%--<div class="secondmenu">
                    <ul>
                        
                        <li>Item wise stock report</li>
                        <li>Item usage analysis report</li>
                    </ul>
                </div>--%>
                </div>
                 <%--<div class="right row">
                <a href="#menu">Back to Modules</a></div>--%>
            </div>           
           
            <div id="Reports">
                <hr />
                <p class="header">
                    <strong>Innboard ERP - Reports</strong></p>
                <hr />
                <%--<div class="left">
                    <img class="image" src="Images/recipe.jpg" alt="Inventory with recipe  Management"
                        style="height: 120px;" /></div>--%>
                <%--<div class="leftWithPixel">
                    Sales and Marketing Department is responsible to keep all records of Travel Agencies,
                    Commission Agents, Local Companies and their respective productivity. In addition,
                    the Department is also responsible for marketing plan, marketing activities and
                    different loyalty and recognition programs through the following menu items and
                    maintain them.
                </div>--%>
                <div class="row">
                <div class="menu col-md-4" style="height: 220px;">
                    <span><strong>Front Office Management</strong></span>
                    <ul>
                        <li>Room Status & Occupancy</li>
                        <li>Promotional Activities</li>
                        <li>Room Sales Revenue</li>
                        <li>Discount Vs Actual Sales</li>
                        <li>Management Analysis</li>
                        <li>In House Guest Ledger</li>
                        <li>Daily Audit Report & Sales Transaction</li>
                        <li>Check-In and Check-Out with late & early</li>
                        <li>Advance Reservation Forecast</li>
                    </ul>
                </div>
                <div class="menu col-md-4" style="height: 220px;">
                    <span><strong>Housekeeping</strong></span>
                    <ul>
                        <li>Housekeeping Daily Status</li>
                        <li>Room Statistics</li>
                        <li>Room Discrepancies</li>
                        <li>Task Assignment sheet</li>
                        <li>Room Conditions</li>
                        <li>Room Inventory</li>
                        <li>Room Shift</li>
                        <li>Guest Audit</li>
                    </ul>
                </div>
                <div class="menu col-md-4" style="height: 220px;">
                    <span><strong>Restaurant POS Management</strong></span>
                    <ul>
                        <li>Restaurant Reservation</li>
                        <li>Room Guest wise Restaurant Sales</li>
                        <li>Periodical Sales Statement & Transaction</li>
                        <li>Void Bill, auto VAT/TAX Calculation</li>
                    </ul>
                </div>
                </div>
                <div class="row">
                <div class="menu col-md-4" style="height: 220px;">
                    <span><strong>Accounts/Back Office</strong></span>
                    <ul>
                        <li>Tree view chart of accounts</li>
                        <li>Transaction List by date & date range</li>
                        <li>Receive & Payment</li>
                        <li>Cash Book & Bank Book Statement</li>
                        <li>General Ledger</li>
                        <li>Cash FLow Statement</li>
                        <li>Accounts Receivable & Payable</li>
                        <li>Fixed Asset Statement</li>
                        <li>Financial Statement</li>
                    </ul>
                </div>
                <div class="menu col-md-4" style="height: 220px;">
                    <span><strong>Banquet Management</strong></span>
                    <ul>
                        <li>Banquet Hall Reservation</li>
                        <li>Sales Revenue Report</li>
                        <li>Occasion wise Sales Report</li>
                    </ul>
                </div>
                <div class="menu col-md-4" style="height: 220px;">
                    <span><strong>Procurement & Inventory</strong></span>
                    <ul>
                        <li>Date & Item wise Purchase</li>
                        <li>Supplier wise Purchase Report</li>
                        <li>Purchase Order & Purchase Return</li>
                        <li>Category & Item wise Item usage</li>
                        <li>Periodical Product Received & Consumption</li>
                    </ul>
                </div>
                </div>
                <div class="row">
                <div class="menu col-md-4" style="height: 220px;">
                    <span><strong>HR & Payroll Management</strong></span>
                    <ul>
                        <li>Staff Budget report</li>
                        <li>CV Bank & sorting report</li>
                        <li>Staff Information, Attendance & Rostering</li>
                        <li>Disciplinary Action report</li>
                        <li>Leave Management report</li>
                        <li>Employee Appraisal Evaluation report</li>
                        <li>Training & Other activities report</li>
                    </ul>
                </div>
                <div class="menu col-md-4" style="height: 220px;">
                    <span><strong>Sales & Marketing</strong></span>
                    <ul>
                        <li>Visited Company Information</li>
                        <li>Meeting Schedule report</li>
                        <li>Action Plan report</li>
                        <li>Reference Company report</li>
                        <li>Company wise Sales Policy report</li>
                    </ul>
                </div>
                </div>
                <%--<div class="secondmenu">
                    <ul>
                        
                        <li>Item wise stock report</li>
                        <li>Item usage analysis report</li>
                    </ul>
                </div>--%>
            </div>            
            <div class="right">
                <a href="#menu">Back to Modules</a></div>
        </div>
    </div>
</asp:Content>
