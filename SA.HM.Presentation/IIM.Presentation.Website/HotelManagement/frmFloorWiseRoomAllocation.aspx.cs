using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data;
using HotelManagement.Entity;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmFloorWiseRoomAllocation : BasePage
    {
        protected int isGenerateCoordinates = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Response.Redirect("/HotelManagement/frmHMFloorManagement.aspx");
                //this.LoadFloor();
                //this.GenerateRoomAllocation();
            }
        }
        protected void ddlSrcFloorId_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GenerateRoomAllocation();
        }
        //************************ User Defined Function ********************//
        private void LoadFloor()
        {
            HMFloorDA floorDA = new HMFloorDA();
            this.ddlSrcFloorId.DataSource = floorDA.GetActiveHMFloorInfo();
            this.ddlSrcFloorId.DataTextField = "FloorName";
            this.ddlSrcFloorId.DataValueField = "FloorId";
            this.ddlSrcFloorId.DataBind();
        }
        private void GenerateRoomAllocation()
        {
            HMFloorManagementDA roomNumberDA = new HMFloorManagementDA();
            List<HMFloorManagementBO> roomNumberListBO = new List<HMFloorManagementBO>();

            string fullContent = string.Empty;
            int ReservedRoomCount = 0;
            int BookedRoomCount = 0;
            int AvailableRoomCount = 0;
            string roomSummary = string.Empty;

            if (this.ddlSrcFloorId.SelectedIndex != -1)
            {
                roomNumberListBO = roomNumberDA.GetHMFloorManagementInfoByFloorId(Convert.ToInt32(this.ddlSrcFloorId.SelectedValue));

                string topPart = @"
                            <div class='row'>";

                string topTemplatePart = @"
                                <div id='FloorRoomAllocation' class='col-md-12'>           
                                ";

                string endTemplatePart = @"</div>
                            </div>";

                string subContent = string.Empty;

                for (int iRoomNumber = 0; iRoomNumber < roomNumberListBO.Count; iRoomNumber++)
                {
                    if (roomNumberListBO[iRoomNumber].StatusId == 3)
                    {
                        string Content0 = @"<div class='NotDraggable FloorRoomReservedDiv' style='width:" + roomNumberListBO[iRoomNumber].RoomWidth + "px; height:" + roomNumberListBO[iRoomNumber].RoomHeight + "px; top:" + roomNumberListBO[iRoomNumber].YCoordinate + "px; left:" + roomNumberListBO[iRoomNumber].XCoordinate + "px;' id='" + roomNumberListBO[iRoomNumber].FloorManagementId + "'>";
                        string Content1 = @"<div class='FloorRoomReservedDiv'>
                                        </div>
                                        <div class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</br>(" + roomNumberListBO[iRoomNumber].RoomType + ")</div></div>";
                        subContent += Content0 + Content1;
                        ReservedRoomCount = ReservedRoomCount + 1;
                    }
                    if (roomNumberListBO[iRoomNumber].StatusId == 2)
                    {
                        string Content0 = @"<div class='NotDraggable FloorRoomBookedDiv' style='width:" + roomNumberListBO[iRoomNumber].RoomWidth + "px; height:" + roomNumberListBO[iRoomNumber].RoomHeight + "px; top:" + roomNumberListBO[iRoomNumber].YCoordinate + "px; left:" + roomNumberListBO[iRoomNumber].XCoordinate + "px;' id='" + roomNumberListBO[iRoomNumber].FloorManagementId + "'>";
                        string Content1 = @"<div class='FloorRoomBookedDiv'>
                                        </div>
                                        <div class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</br>(" + roomNumberListBO[iRoomNumber].RoomType + ")</div></div>";
                        subContent += Content0 + Content1;
                        BookedRoomCount = BookedRoomCount + 1;
                    }

                    if (roomNumberListBO[iRoomNumber].StatusId == 1)
                    {
                        string Content0 = @"<a href='/HotelManagement/frmRoomRegistration.aspx?SelectedRoomNumber=" + roomNumberListBO[iRoomNumber].RoomId;
                        string Content1 = @"'><div class='NotDraggable FloorRoomAvailableDiv' style='width:" + roomNumberListBO[iRoomNumber].RoomWidth + "px; height:" + roomNumberListBO[iRoomNumber].RoomHeight + "px; top:" + roomNumberListBO[iRoomNumber].YCoordinate + "px; left:" + roomNumberListBO[iRoomNumber].XCoordinate + "px;' id='" + roomNumberListBO[iRoomNumber].FloorManagementId + "'>";
                        string Content2 = @"<div class='FloorRoomAvailableDiv'>
                                        </div>
                                        <div class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</br>(" + roomNumberListBO[iRoomNumber].RoomType + ")</div></div></a>";

                        subContent += Content0 + Content1 + Content2;
                        AvailableRoomCount = AvailableRoomCount + 1;

                        //                        string Content0 = @"<div class='NotDraggable FloorRoomAvailableDiv' style='width:" + roomNumberListBO[iRoomNumber].RoomWidth + "px; height:" + roomNumberListBO[iRoomNumber].RoomHeight + "px; top:" + roomNumberListBO[iRoomNumber].YCoordinate + "px; left:" + roomNumberListBO[iRoomNumber].XCoordinate + "px;' id='" + roomNumberListBO[iRoomNumber].FloorManagementId + "'>";
                        //                        string Content1 = @"<a href='/HotelManagement/frmRoomRegistration.aspx?SelectedRoomNumber=" + roomNumberListBO[iRoomNumber].RoomId;
                        //                        string Content2 = @"'><div class='FloorRoomAvailableDiv'>
                        //                                        </div></a>
                        //                                        <div class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</br>(" + roomNumberListBO[iRoomNumber].RoomType + ")</div></div>";

                        //                        subContent += Content0 + Content1 + Content2;
                        //                        AvailableRoomCount = AvailableRoomCount + 1;
                    }
                }

                roomSummary = " (Reserved: " + ReservedRoomCount + ", Booked: " + BookedRoomCount + ", Available: " + AvailableRoomCount + ")";
                fullContent += topPart + topTemplatePart + subContent + endTemplatePart;
                ReservedRoomCount = 0;
                BookedRoomCount = 0;
                AvailableRoomCount = 0;

                this.ltlRoomTemplate.Text = fullContent;
            }
        }
    }
}