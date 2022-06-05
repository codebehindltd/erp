<%@ Page Title="" Language="C#" MasterPageFile="~/InnBoard.Master" AutoEventWireup="true" CodeBehind="InnboardFeatures.aspx.cs" Inherits="HotelManagement.Presentation.Website.InnboardFeatures" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <style>
        .left
        {
            float: left;
            text-align: justify;
        }
        .right
        {
            float: right;
        }
        .leftWithPixel
        {
            float: left;
            width: 350px;
            margin-left: 20px;
            line-height: 25px;
            text-align: justify;
            font-size: 15px;
        }
        .clear
        {
            clear: both;
        }
        .menu
        {
            margin-left: 20px;
            padding-left: 30px;
            float: left;
            text-align: justify;
            width: 200px;
        }
        .header
        {
            margin-left: 0px;
            font-size: 18px;
            text-align: left;
            padding-left: 10px;
            padding-top: 5px;
            
        }
               .headerNext
        {
            margin-left: 0px;
            font-size: 18px;
            text-align: left;
            padding-left: 10px;
            padding-top: 5px;
        } 
        .image
        {
            height: 300px;
            padding-left: 30px;
            width: 300px;
        }
        .showcase-content
        {
            background-color: #FFFFFF;
            text-align: center;
        }
    </style>
    <link rel="stylesheet" href="css/style.css" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.5.2/jquery.min.js"></script>

    <script src="Scripts/jquery.aw-showcase.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $("#showcase").awShowcase(
	{
	    content_width: 960,
	    content_height: 450,
	    fit_to_parent: false,
	    auto: true,
	    interval: 3000,
	    continuous: true,
	    loading: true,
	    tooltip_width: 200,
	    tooltip_icon_width: 32,
	    tooltip_icon_height: 32,
	    tooltip_offsetx: 18,
	    tooltip_offsety: 0,
	    arrows: true,
	    buttons: true,
	    btn_numbers: true,
	    keybord_keys: true,
	    mousetrace: false, /* Trace x and y coordinates for the mouse */
	    pauseonover: true,
	    stoponclick: false,
	    transition: 'fade', /* hslide/vslide/fade */
	    transition_delay: 0,
	    transition_speed: 1500,
	    show_caption: 'onhover', /* onload/onhover/show */
	    thumbnails: true,
	    thumbnails_position: 'outside-last', /* outside-last/outside-first/inside-last/inside-first */
	    thumbnails_direction: 'horizontal', /* vertical/horizontal */
	    thumbnails_slidex: 1, /* 0 = auto / 1 = slide one thumbnail / 2 = slide two thumbnails / etc. */
	    dynamic_height: false, /* For dynamic height to work in webkit you need to set the width and height of images in the source. Usually works to only set the dimension of the first slide in the showcase. */
	    speed_change: true, /* Set to true to prevent users from swithing more then one slide at once. */
	    viewline: false, /* If set to true content_width, thumbnails, transition and dynamic_height will be disabled. As for dynamic height you need to set the width and height of images in the source. */
	    custom_function: null /* Define a custom function that runs on content change */
	});
        });

</script>
<div class="clear"></div>
    <div class="block" style=" padding:20px;">

<div style="width: 1000px; margin: auto; border-style:solid; border-width:1px; border-color:#365B85" >
        <div id="showcase" class="showcase" style="background-color: White">
            <!-- Each child div in #showcase with the class .showcase-slide represents a slide. -->
            <div class="showcase-slide" style="background-color: White">
                <!-- Put the slide content in a div with the class .showcase-content. -->
                <div class="showcase-content" style="background-color: #365B85; height:40px; ">
                    <p class="headerNext">
                        <strong style="color: White;background-color: #365B85">Administration</strong></p>
                    <div class="clear"></div>
                    <div class="left">
                        <img class="image" src="images/administration.png" alt="administration" width="300px"
                            height="400px" /></div>
                    <div class="leftWithPixel" style="color: Black">
                        ERP System Administration is a function that manages Enterprise Resource Planning
                        (ERP) software, a suite of integrated business applications. ERP is as much a management
                        methodology as it is software--thus administration is more than just technical or
                        operational--it is full business process support to the enterprise.
                    </div>
                    <div class="menu">
                        <span style="margin-left: -170px"><strong style="text-align: left">Features:</strong></span>
                        <ul>
                            <li style="text-align: left">User Group Management</li>
                            <li style="text-align: left">User Database</li>
                            <li style="text-align: left">Create User For Login </li>
                            <li style="text-align: left">Assign Permissions to Users</li>
                            <li style="text-align: left">Restaurant Bill</li>
                            <li style="text-align: left">User Login Record</li>
                        </ul>
                    </div>
                </div>
                <!-- Put the thumbnail content in a div with the class .showcase-thumbnail -->
                <div class="showcase-thumbnail" style="background-color: White">
                    <img src="images/administration.png" alt="01" width="100px" />
                    <!-- The div below with the class .showcase-thumbnail-caption contains the thumbnail caption. -->
                    <!-- The div below with the class .showcase-thumbnail-cover is used for the thumbnails active state. -->
                    <div class="showcase-thumbnail-cover">
                    </div>
                </div>
                <!-- Put the caption content in a div with the class .showcase-caption -->
                <div class="showcase-caption">
                    <h2>
                        Innboard Administration Management System !</h2>
                </div>
            </div>
            <div class="clear">
            </div>
            <div class="showcase-slide" style="background-color: White">
                <!-- Put the slide content in a div with the class .showcase-content. -->
                <div class="showcase-content" style="background-color: #365B85; height:40px">
                                        <p class="headerNext">
                        <strong style="color: White">Hotel Management</strong></p>
                  
                    <div class="left">
                        <img class="image" src="images/hotelmanagement.png" alt="hotelmanagement" width="300px"
                            height="400px" /></div>
                    <div class="leftWithPixel" style="color: Black">
                        Hotel Management involves combination of various skills like management, marketing,
                        human resource development, and financial management, inter personal skills, dexterity,
                        etc. Hotels are a major employment generator in tourism industry. Work in the area
                        of Hotel Management involves ensuring that all operations, including accommodation,
                        food and drink and other hotel services run smoothly.
                    </div>
                    <div class="menu">
                        <span style="margin-left: -170px"><strong style="text-align: left">Features:</strong></span>
                        <ul>
                            <li style="text-align: left">Font office management</li>
                            <li style="text-align: left">Online reservation</li>
                            <li style="text-align: left">Reservation Management</li>
                            <li style="text-align: left">Check-In</li>
                            <li style="text-align: left">Check-Out</li>
                            <li style="text-align: left">Service Management</li>
                            <li style="text-align: left">Floor Management</li>
                            <li style="text-align: left">Room Management</li>
                            <li style="text-align: left">Room shift</li>
                            <li style="text-align: left">Room Clean status</li>
                            <li style="text-align: left">Night Audit</li>
                            <li style="text-align: left">Reports</li>
                        </ul>
                    </div>
                </div>
                <!-- Put the thumbnail content in a div with the class .showcase-thumbnail -->
                <div class="showcase-thumbnail" style="background-color: White">
                    <img src="images/hotelmanagement.png" alt="01" width="110px" />
                    <!-- The div below with the class .showcase-thumbnail-caption contains the thumbnail caption. -->
                    <!-- The div below with the class .showcase-thumbnail-cover is used for the thumbnails active state. -->
                    <div class="showcase-thumbnail-cover">
                    </div>
                </div>
                <!-- Put the caption content in a div with the class .showcase-caption -->
                <div class="showcase-caption">
                    <h2>
                        Innboard Hotel Management System !</h2>
                </div>
            </div>
            <div class="clear">
            </div>
            <div class="showcase-slide" style="background-color: White">
                <!-- Put the slide content in a div with the class .showcase-content. -->
                <div class="showcase-content" style="background-color: #365B85; height:40px">
                                        <p class="headerNext">
                        <strong style="color: White">Restaurant Management</strong></p>
                   
                    <div class="left">
                        <img class="image" src="images/resturent.png" alt="administration" width="300px"
                            height="400px" /></div>
                    <div class="leftWithPixel" style="color: Black">
                        InnBoard Restaurant Management offers a total restaurant POS system that can be
                        easily tailored for use in any sort of food service establishment, from fine dining
                        and table service restaurants to quick service (QSR), as well as bars and clubs.
                        Designed as a turnkey restaurant POS system that can grow with your business, InnBoard
                        Restaurant Management system offers everything you need to effectively manage your
                        operations.
                    </div>
                    <div class="menu">
                        <span style="margin-left: -170px"><strong style="text-align: left">Features:</strong></span>
                        <ul>
                            <li style="text-align: left">Table Management</li>
                            <li style="text-align: left">Bearer Management</li>
                            <li style="text-align: left">Food Management</li>
                            <li style="text-align: left">Auto KOT</li>
                            <li style="text-align: left">Restaurant Bill</li>
                            <li style="text-align: left">Reports</li>
                        </ul>
                    </div>
                </div>
                <!-- Put the thumbnail content in a div with the class .showcase-thumbnail -->
                <div class="showcase-thumbnail" style="background-color: White">
                    <img src="images/resturent.png" alt="01" width="100px" />
                    <!-- The div below with the class .showcase-thumbnail-caption contains the thumbnail caption. -->
                    <!-- The div below with the class .showcase-thumbnail-cover is used for the thumbnails active state. -->
                    <div class="showcase-thumbnail-cover">
                    </div>
                </div>
                <!-- Put the caption content in a div with the class .showcase-caption -->
                <div class="showcase-caption">
                    <h2>
                        Innboard Resturent Management System !</h2>
                </div>
            </div>
            <div class="clear">
            </div>

            <div class="showcase-slide" style="background-color: White">
                <!-- Put the slide content in a div with the class .showcase-content. -->
                <div class="showcase-content" style="background-color: #365B85; height:40px">
                                        <p class="headerNext">
                        <strong style="color: White">Accounting management</strong></p>
                   
                    <div class="left">
                        <img class="image" src="images/accounts.png" alt="accounts" width="300px" height="400px" /></div>
                    <div class="leftWithPixel" style="color: Black">
                        Many business owners aren’t able to fully grasp the financial fundamentals of their
                        organizations due to lack of sufficient reporting and tracking of transactions.
                        Knowing where your business is headed is vital in creating a successful venture.
                        Therefore, an easy and cost-effective means of monitoring financial dealings is
                        required. Accounting software doesn’t only assist in managing and tracking cash
                        procedures, it also helps to determine the success of your business’s performance.
                        Utilizing quality cash flow software can yield significant benefits for your business.
                    </div>
                    <div class="menu">
                        <span style="margin-left: -170px"><strong style="text-align: left">Features:</strong></span>
                        <ul>
                            <li style="text-align: left">Chart of Accounts</li>
                            <li style="text-align: left">Opening Balance</li>
                            <li style="text-align: left">Journal Voucher Entry</li>
                            <li style="text-align: left">Received Vouchers Entry</li>
                            <li style="text-align: left">Payment Vouchers Entry</li>
                            <li style="text-align: left">Contra Vouchers Entry</li>
                            <li style="text-align: left">Credit Note</li>
                            <li style="text-align: left">Debit Note</li>
                            <li style="text-align: left">Accounts Payable</li>
                            <li style="text-align: left">Accounts Receivable</li>
                            <li style="text-align: left">Accounts Payable</li>
                            <li style="text-align: left">Accounts Receivable</li>
                            <li style="text-align: left">Project Accounting</li>
                            <li style="text-align: left">Daly/Monthly/Yearly Ledgers</li>
                            <li style="text-align: left">Supplier Ledger</li>
                            <li style="text-align: left">Customer ledger</li>
                            <li style="text-align: left">Trail Balance</li>
                            <li style="text-align: left">Cash Flow Statement</li>
                            <li style="text-align: left">Balance Sheet</li>
                            <li style="text-align: left">Profit and Loss Account</li>
                        </ul>
                    </div>
                </div>
                <!-- Put the thumbnail content in a div with the class .showcase-thumbnail -->
                <div class="showcase-thumbnail" style="background-color: White">
                    <img src="images/accounts.png" alt="01" width="128px" />
                    <!-- The div below with the class .showcase-thumbnail-caption contains the thumbnail caption. -->
                    <!-- The div below with the class .showcase-thumbnail-cover is used for the thumbnails active state. -->
                    <div class="showcase-thumbnail-cover">
                    </div>
                </div>
                <!-- Put the caption content in a div with the class .showcase-caption -->
                <div class="showcase-caption">
                    <h2>
                        Innboard Accounting Management System !</h2>
                </div>
            </div>
            <div class="clear">
            </div>

            <div class="showcase-slide" style="background-color: White">
                <!-- Put the slide content in a div with the class .showcase-content. -->
                <div class="showcase-content" style="background-color: #365B85; height:40px">
                                        <p class="headerNext">
                        <strong style="color: White">Payroll Management</strong></p>
                  
                    <div class="left">
                        <img class="image" src="images/payroll.png" alt="payroll" width="300px" height="400px" /></div>
                    <div class="leftWithPixel" style="color: Black">
                        The Payroll Management System deals with the financial aspects of employee's salary,
                        allowances, deductions, gross pay, net pay etc. and generation of pay-slips for
                        a specific period. The outstanding benefit of Payroll Management System is its easy
                        implementation. Other advantages of Payroll Management System are its extensive
                        features and reports .
                    </div>
                    <div class="menu">
                        <span style="margin-left: -170px"><strong style="text-align: left">Features:</strong></span>
                        <ul>
                            <li style="text-align: left">Organization Department Setup</li>
                            <li style="text-align: left">Employee Grade Setup</li>
                            <li style="text-align: left">Salary Head Setup</li>
                            <li style="text-align: left">Salary Formula Management</li>
                            <li style="text-align: left">Employee Category Setup</li>
                            <li style="text-align: left">Employee Designation Setup</li>
                            <li style="text-align: left">Leave Type Setup</li>
                            <li style="text-align: left">Employee Wise Yearly Leave</li>
                            <li style="text-align: left">Deduction Management</li>
                            <li style="text-align: left">Organization Time Slab Head Setup</li>
                            <li style="text-align: left">Employee Time Slab Management</li>
                            <li style="text-align: left">Employee Leave Management</li>
                            <li style="text-align: left">Employee Increment Management</li>
                            <li style="text-align: left">Employee Allowance</li>
                            <li style="text-align: left">Employee Attendance Management</li>
                            <li style="text-align: left">Employee Overtime Management</li>
                            <li style="text-align: left">Monthly Employee Salary Process</li>
                            <li style="text-align: left">Report</li>
                        </ul>
                    </div>
                </div>
                <!-- Put the thumbnail content in a div with the class .showcase-thumbnail -->
                <div class="showcase-thumbnail" style="background-color: White">
                    <img src="images/payroll.png" alt="01" width="115px" />
                    <!-- The div below with the class .showcase-thumbnail-caption contains the thumbnail caption. -->
                    <!-- The div below with the class .showcase-thumbnail-cover is used for the thumbnails active state. -->
                    <div class="showcase-thumbnail-cover">
                    </div>
                </div>
                <!-- Put the caption content in a div with the class .showcase-caption -->
                <div class="showcase-caption">
                    <h2>
                        Innboard Payroll Management System !</h2>
                </div>
            </div>
            <div class="clear">
            </div>

            <div class="showcase-slide" style="background-color: White">
                <!-- Put the slide content in a div with the class .showcase-content. -->
                <div class="showcase-content" style="background-color: #365B85; height:40px">
                                        <p class="headerNext">
                        <strong style="color: White">Store Management</strong></p>
                 
                    <div class="left">
                        <img class="image" src="images/store.png" alt="store" width="300px" height="400px" /></div>
                    <div class="leftWithPixel" style="color: Black">
                        A rather important module, inventory management lets managers automate the process
                        of tracking rooms, and food and beverage consumption in the hotel. I am sure many
                        inventory managers will agree that manually filing cash memos and getting clearance
                        from finance department to pay vendors was a nightmare and a huge waste of effort.
                        With the arrival of hospitality technology solutions, automation of the inventory
                        system means lesser work and greater visibility into stock, automated reminders
                        as stock levels diminish, faster decision making on stock maintenance in the hotel.
                    </div>
                    <div class="menu">
                        <span style="margin-left: -170px"><strong style="text-align: left">Features:</strong></span>
                        <ul>
                            <li style="text-align: left">Supplier information</li>
                            <li style="text-align: left">Supplier Database</li>
                            <li style="text-align: left">Product Purchase</li>
                            <li style="text-align: left">Stock Management</li>
                            <li style="text-align: left">Room Inventory</li>
                            <li style="text-align: left">Reports</li>
                        </ul>
                    </div>
                </div>
                <!-- Put the thumbnail content in a div with the class .showcase-thumbnail -->
                <div class="showcase-thumbnail" style="background-color: White">
                    <img src="images/store.png" alt="01" width="90px" />
                    <!-- The div below with the class .showcase-thumbnail-caption contains the thumbnail caption. -->
                    <!-- The div below with the class .showcase-thumbnail-cover is used for the thumbnails active state. -->
                    <div class="showcase-thumbnail-cover">
                    </div>
                </div>
                <!-- Put the caption content in a div with the class .showcase-caption -->
                <div class="showcase-caption">
                    <h2>
                        Innboard Store Management System !</h2>
                </div>
            </div>
            <div class="clear">
            </div>

        </div>
    </div>
    </div>

</asp:Content>
