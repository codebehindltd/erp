<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu.ascx.cs" Inherits="Mamun.Presentation.Website.Common.Menu" %>
<asp:Panel ID="pnlSearch" runat="server">
    <table class="table_style2" style="width: 100%" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="section_title_left2">
                            </div>
                        </td>
                        <td style="width: 120px">
                            <div class="section_title">
                                <div class="section_title_text">
                                    Search Panel</div>
                            </div>
                        </td>
                        <td>
                            <div class="section_title_right">
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="left_nav_menu">
                <a href="/HotelManagement/frmSearchGuest.aspx">Guest Search</a>
            </td>
        </tr>
    </table>
</asp:Panel>


<asp:Panel ID="pnlCompanyProfile" runat="server">
    <table class="table_style2" style="width: 100%" cellpadding="0" cellspacing="0">
        <tr>
            <td style="padding-top: 5px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="section_title_left2">
                            </div>
                        </td>
                        <td style="width: 120px">
                            <div class="section_title">
                                <div class="section_title_text">
                                   Company Profile</div>
                            </div>
                        </td>
                        <td>
                            <div class="section_title_right">
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="left_nav_menu">
                <a href="/HMCommon/frmCompany.aspx">Company Profile</a>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlHotelManagement" runat="server">
    <table class="table_style2" style="width: 100%" cellpadding="0" cellspacing="0">
        <tr>
            <td style="padding-top: 5px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="section_title_left2">
                            </div>
                        </td>
                        <td style="width: 120px">
                            <div class="section_title">
                                <div class="section_title_text">
                                    Hotel Management</div>
                            </div>
                        </td>
                        <td>
                            <div class="section_title_right">
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="left_nav_menu">
                <a href="/HMCommon/frmBank.aspx">Bank Head</a>
            </td>
        </tr>
        <tr>
            <td class="left_nav_menu">
                <a href="/HotelManagement/frmGuestHouseService.aspx">Service Head</a>
            </td>
        </tr>
        <tr>
            <td class="left_nav_menu">
                <a href="/HotelManagement/frmGuestCompany.aspx">Guest Company</a>
            </td>
        </tr>
        <tr>
            <td class="left_nav_menu">
                <a href="/HotelManagement/frmRoomType.aspx">Room Type</a>
            </td>
        </tr>
        <tr>
            <td class="left_nav_menu">
                <a href="/HotelManagement/frmRoomNumber.aspx">Room Number</a>
            </td>
        </tr>
        <tr>
            <td class="left_nav_menu">
                <a href="/HotelManagement/frmRoomReservation.aspx">Room Reservation</a>
            </td>
        </tr>
        <tr>
            <td class="left_nav_menu">
                <a href="/HotelManagement/frmRoomRegistration.aspx">Room Registration</a>
            </td>
        </tr>

        <tr>
            <td class="left_nav_menu">
                <a href="/HotelManagement/frmRoomShift.aspx">Room Shift</a>
            </td>
        </tr>

        <tr>
            <td class="left_nav_menu">
                <a href="/HotelManagement/frmGHServiceBill.aspx">Service Bill</a>
            </td>
        </tr>
        <tr>
            <td class="left_nav_menu">
                <a href="/HotelManagement/frmAvailableGuestList.aspx">Available Guest List</a>
            </td>
        </tr>
        <tr>
            <td class="left_nav_menu">
                <a href="/HotelManagement/frmRoomCheckOut.aspx">Room CheckOut</a>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlReport" runat="server">
    <table class="table_style2" style="width: 100%" cellpadding="0" cellspacing="0">
        <tr>
           <td style="padding-top: 5px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="section_title_left2">
                            </div>
                        </td>
                        <td style="width: 120px">
                            <div class="section_title">
                                <div class="section_title_text">
                                    Report Panel</div>
                            </div>
                        </td>
                        <td>
                            <div class="section_title_right">
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="left_nav_menu">
                <a href="/HotelManagement/Reports/frmReportRoomStatus.aspx">Room Status</a>
            </td>
        </tr>
        <tr>
            <td class="left_nav_menu">
                <a href="/HotelManagement/Reports/frmReportRoomReservation.aspx">Room Reservation</a>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlUserInformation" runat="server">
    <table class="table_style2" style="width: 100%" cellpadding="0" cellspacing="0">
        <tr>
           <td style="padding-top: 5px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="section_title_left2">
                            </div>
                        </td>
                        <td style="width: 120px">
                            <div class="section_title">
                                <div class="section_title_text">
                                    User Panel</div>
                            </div>
                        </td>
                        <td>
                            <div class="section_title_right">
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="left_nav_menu">
                <a href="/UserInformation/frmUserGroup.aspx">User Group</a>
            </td>
        </tr>
        <tr>
            <td class="left_nav_menu">
                <a href="/UserInformation/frmUserInformation.aspx">User Info.</a>
            </td>
        </tr>
    </table>
</asp:Panel>
