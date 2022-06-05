using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity;
using HotelManagement.Data;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmRoomAllocation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<RoomNumberBO> roomNumberListBO = new List<RoomNumberBO>();


            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            RoomTypeDA roomTypeDA = new RoomTypeDA();
            List<RoomTypeBO> RoomTypeInfoList = new List<RoomTypeBO>();
            if (!IsPostBack)
            {
                RoomTypeInfoList = roomTypeDA.GetRoomTypeInfo();
            }
            string fullContent = string.Empty;
            int ReservedRoomCount = 0;
            int BookedRoomCount = 0;
            int AvailableRoomCount = 0;
            string roomSummary = string.Empty;

            for (int statusInfo = 0; statusInfo < RoomTypeInfoList.Count; statusInfo++)
            {
                roomNumberListBO = roomNumberDA.GetRoomNumberInfoByRoomType(RoomTypeInfoList[statusInfo].RoomTypeId);

                string topPart = @"<div class='divClear'>
                            </div>
                            <div id='SearchPanel' class='block'>
                                <a href='#' class='block-heading' data-toggle='collapse'>";

                

                string topTemplatePart = @"</a>
                                <div class='block-body collapse in'>           
                                ";

                string endTemplatePart = @"</div>
                            </div>";

                string subContent = string.Empty;

                for (int iRoomNumber = 0; iRoomNumber < roomNumberListBO.Count; iRoomNumber++)
                {
                    if (roomNumberListBO[iRoomNumber].StatusId == 3)
                    {
                        subContent += @"<div class='DivRoomContainer'>
                                        <div class='RoomReservedDiv'>
                                        </div>
                                        <div class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                        ReservedRoomCount = ReservedRoomCount + 1;
                    }
                    if (roomNumberListBO[iRoomNumber].StatusId == 2)
                    {
                        subContent += @"<div class='DivRoomContainer'>
                                        <div class='RoomBookedDiv'>
                                        </div>
                                        <div class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                        BookedRoomCount = BookedRoomCount + 1;
                    }
                    /*
                    if (roomNumberListBO[iRoomNumber].StatusId == 1)
                    {
                        subContent += @"<div class='DivRoomContainer'>
                                        <div class='RoomAvailableDiv'>
                                        </div>
                                        <div class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                        AvailableRoomCount = AvailableRoomCount + 1;
                    }
                    */
                    if (roomNumberListBO[iRoomNumber].StatusId == 1)
                    {
                        string Content1 = @"<div class='DivRoomContainer'>
                                        <a href='/HotelManagement/frmRoomRegistration.aspx?SelectedRoomNumber=" + roomNumberListBO[iRoomNumber].RoomId;
                        string Content2 = @"'><div class='RoomAvailableDiv'>
                                        </div></a>
                                        <div class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                        subContent += Content1 + Content2;
                        AvailableRoomCount = AvailableRoomCount + 1;
                    }

                }

                roomSummary = " (Out of service: " + ReservedRoomCount + ", Booked: " + BookedRoomCount + ", Available: " + AvailableRoomCount + ")";
                string groupNamePart = RoomTypeInfoList[statusInfo].RoomType + roomSummary;

                fullContent += topPart + groupNamePart + topTemplatePart + subContent + endTemplatePart;
                ReservedRoomCount = 0;
                BookedRoomCount = 0;
                AvailableRoomCount = 0;
            }

            this.ltlRoomTemplate.Text = fullContent;
        }
    }
}

