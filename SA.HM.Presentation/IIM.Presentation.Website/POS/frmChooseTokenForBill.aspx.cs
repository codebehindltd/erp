using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data;
using HotelManagement.Entity;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmChooseTokenForBill : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        UserInformationDA userInformationDA = new UserInformationDA();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            string queryStringId = string.Empty;
            string costCenterId = string.Empty;

            if (!string.IsNullOrWhiteSpace(Request.QueryString["CostCenterId"]))
            {
                costCenterId = Request.QueryString["CostCenterId"];

                Boolean status = userInformationDA.UpdateUserWorkingCostCenterInfo("SecurityUser", currentUserInformationBO.UserInfoId, Convert.ToInt32(costCenterId));
                if (status)
                {
                    currentUserInformationBO.WorkingCostCenterId = Convert.ToInt32(costCenterId);
                    Session.Add("UserInformationBOSession", currentUserInformationBO);
                }
                Response.Redirect("/POS/frmChooseTokenForBill.aspx");
            }

            hfCostCenterId.Value = currentUserInformationBO.WorkingCostCenterId.ToString();
            //int costCenterId = Convert.ToInt32(hfCostCenterId.Value);
            if (!IsPostBack)
            {                
                //LoadTableAllocation(Convert.ToInt32(hfCostCenterId.Value));
                IsRestaurantIntegrateWithFrontOffice();

                CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
                costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(currentUserInformationBO.WorkingCostCenterId);
                if (costCentreTabBO != null)
                {
                    if (costCentreTabBO.IsRestaurant)
                    {
                        if (costCentreTabBO.DefaultView == "Token")
                        {
                            LoadTokenInformation(Convert.ToInt32(hfCostCenterId.Value));
                        }
                        else if (costCentreTabBO.DefaultView == "Table")
                        {
                            LoadTableAllocation(Convert.ToInt32(hfCostCenterId.Value));
                        }
                        else if (costCentreTabBO.DefaultView == "Room")
                        {
                            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithFrontOffice", "IsRestaurantIntegrateWithFrontOffice");
                            if (commonSetupBO != null)
                            {
                                if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                                {
                                    if (commonSetupBO.SetupValue != "0")
                                    {
                                        this.LoadRoomAllocation();
                                    }
                                }
                            }
                            
                            //Response.Redirect("/Restaurant/frmGuestRoomKotBill.aspx?Kot=RoomAllocation");
                        }
                    }
                }
                else
                {
                    LoadTableAllocation(Convert.ToInt32(hfCostCenterId.Value));
                    //Response.Redirect("/Restaurant/frmKotBillMaster.aspx?Kot=TableAllocation");
                }
            }
            //this.btnGuestHouseRoom.Visible = false;
        }
        protected void btnRestaurantTable_Click(object sender, EventArgs e)
        {
            this.LoadTableAllocation(Convert.ToInt32(hfCostCenterId.Value));
        }
        protected void btnGuestHouseRoom_Click(object sender, EventArgs e)
        {
            this.LoadRoomAllocation();
        }
        //************************ User Defined Function ********************//
        private void IsRestaurantIntegrateWithFrontOffice()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithFrontOffice", "IsRestaurantIntegrateWithFrontOffice");
            if (commonSetupBO != null)
            {
                if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                {
                    if (commonSetupBO.SetupValue == "0")
                    {
                        //this.btnGuestHouseRoom.Visible = false;
                    }
                    else
                    {
                        //this.btnGuestHouseRoom.Visible = true;
                    }
                }
            }
        }
        private void LoadTokenInformation(int costCenterId)
        {
            //this.btnRestaurantTable.Visible = false;
            //this.btnGuestHouseRoom.Visible = true;

            KotBillMasterDA entityDA = new KotBillMasterDA();
            List<KotBillMasterBO> entityListBO = new List<KotBillMasterBO>();

            string fullContent = string.Empty;

            entityListBO = entityDA.GetRestaurantTokenInfoForBill(costCenterId, "RestaurantToken");

            string topPart = @"<div id='legendContainerForRestaurant'>
                                <div class='legend RoomVacantDiv'>
                                </div>
                                <div class='legendText'>
                                    Available</div>
                                <div class='legend RoomOccupaiedDiv'>
                                </div>
                                <div class='legendText'>
                                    Occupaied
                                </div>
                            </div><div class='divClear'>
                            </div>
                            <div class='block FloorRoomAllocationBGImage'>                                
                                <a href='#' class='block-heading' data-toggle='collapse'>";

            string topTemplatePart = @"</a>
                                <div id='FloorRoomAllocation' class='block-body collapse in'>           
                                ";

            string endTemplatePart = @"</div>
                            </div>";

            string subContent = string.Empty;

            for (int iTableNumber = 0; iTableNumber < entityListBO.Count; iTableNumber++)
            {
                string Content0 = string.Empty;
                string Content1 = string.Empty;
                string Content2 = string.Empty;

                Content0 = @"<a href='/Restaurant/frmRestaurantBill.aspx?tokenId=" + entityListBO[iTableNumber].KotId + "&CostCenterId=" + costCenterId;
                Content1 = @"'><div class='NotDraggable RestaurantTableBookedDiv' style='width:73px; height:55px;' id='" + entityListBO[iTableNumber].KotId + "'>";
                Content2 = @"<div class='RestaurantTableBookedDiv'>
                                        </div>
                                        <div class='RoomNumberDiv'>" + entityListBO[iTableNumber].TokenNumber + "</div></div></a>";

//                if (entityListBO[iTableNumber].StatusId == 1)
//                {
//                    Content0 = string.Empty;
//                    Content1 = @"<div class='NotDraggable RestaurantTableAvailableDiv' style='width:" + entityListBO[iTableNumber].TableWidth + "px; height:" + entityListBO[iTableNumber].TableHeight + "px; top:" + entityListBO[iTableNumber].YCoordinate + "px; left:" + entityListBO[iTableNumber].XCoordinate + "px;' id='" + entityListBO[iTableNumber].TableManagementId + "'>";
//                    Content2 = @"<div class='RestaurantTableAvailableDiv'>
//                                        </div>
//                                        <div class='RoomNumberDiv'>" + entityListBO[iTableNumber].TableNumber + "</div></div>";

//                }
//                if (entityListBO[iTableNumber].StatusId == 2)
//                {
//                    //Content0 = @"<a href='/Restaurant/frmRestaurantBill.aspx?tableId=" + entityListBO[iTableNumber].TableId;
//                    Content0 = @"<a href='/Restaurant/frmRestaurantBill.aspx?tableId=" + entityListBO[iTableNumber].TableId + "&CostCenterId=" + costCenterId;
//                    Content1 = @"'><div class='NotDraggable RestaurantTableBookedDiv' style='width:" + entityListBO[iTableNumber].TableWidth + "px; height:" + entityListBO[iTableNumber].TableHeight + "px; top:" + entityListBO[iTableNumber].YCoordinate + "px; left:" + entityListBO[iTableNumber].XCoordinate + "px;' id='" + entityListBO[iTableNumber].TableManagementId + "'>";
//                    Content2 = @"<div class='RestaurantTableBookedDiv'>
//                                        </div>
//                                        <div class='RoomNumberDiv'>" + entityListBO[iTableNumber].TableNumber + "</div></div></a>";

////                }
//                if (entityListBO[iTableNumber].StatusId == 3)
//                {
//                    Content0 = string.Empty;
//                    Content1 = @"<div class='NotDraggable RestaurantTableReservedDiv' style='width:" + entityListBO[iTableNumber].TableWidth + "px; height:" + entityListBO[iTableNumber].TableHeight + "px; top:" + entityListBO[iTableNumber].YCoordinate + "px; left:" + entityListBO[iTableNumber].XCoordinate + "px;' id='" + entityListBO[iTableNumber].TableManagementId + "'>";
//                    Content2 = @"<div class='RestaurantTableReservedDiv'>
//                                        </div>
//                                        <div class='RoomNumberDiv'>" + entityListBO[iTableNumber].TableNumber + "</div></div>";
//                }

                subContent += Content0 + Content1 + Content2;
            }

            fullContent += topPart + topTemplatePart + subContent + endTemplatePart;

            this.ltlRoomTemplate.Text = fullContent;
        }
        private void LoadTableAllocation(int costCenterId)
        {
            //this.btnRestaurantTable.Visible = false;
            //this.btnGuestHouseRoom.Visible = true;
            
            TableManagementDA entityDA = new TableManagementDA();
            List<TableManagementBO> entityListBO = new List<TableManagementBO>();

            string fullContent = string.Empty;


            entityListBO = entityDA.GetTableManagementInfo(costCenterId);

            string topPart = @"<div id='legendContainerForRestaurant'>
                                <div class='legend RoomVacantDiv'>
                                </div>
                                <div class='legendText'>
                                    Available</div>
                                <div class='legend RoomOccupaiedDiv'>
                                </div>
                                <div class='legendText'>
                                    Occupaied
                                </div>
                            </div><div class='divClear'>
                            </div>
                            <div class='block FloorRoomAllocationBGImage'>                                
                                <a href='#' class='block-heading' data-toggle='collapse'>";

            string topTemplatePart = @"</a>
                                <div id='FloorRoomAllocation' class='block-body collapse in'>           
                                ";

            string endTemplatePart = @"</div>
                            </div>";

            string subContent = string.Empty;

            for (int iTableNumber = 0; iTableNumber < entityListBO.Count; iTableNumber++)
            {
                string Content0 = string.Empty;
                string Content1 = string.Empty;
                string Content2 = string.Empty;
                if (entityListBO[iTableNumber].StatusId == 1)
                {
                    Content0 = string.Empty;
                    Content1 = @"<div class='NotDraggable RestaurantTableAvailableDiv' style='width:" + entityListBO[iTableNumber].TableWidth + "px; height:" + entityListBO[iTableNumber].TableHeight + "px; top:" + entityListBO[iTableNumber].YCoordinate + "px; left:" + entityListBO[iTableNumber].XCoordinate + "px;' id='" + entityListBO[iTableNumber].TableManagementId + "'>";
                    Content2 = @"<div class='RestaurantTableAvailableDiv'>
                                        </div>
                                        <div class='RoomNumberDiv'>" + entityListBO[iTableNumber].TableNumber + "</div></div>";

                }
                if (entityListBO[iTableNumber].StatusId == 2)
                {
                    //Content0 = @"<a href='/Restaurant/frmRestaurantBill.aspx?tableId=" + entityListBO[iTableNumber].TableId;
                    Content0 = @"<a href='/POS/frmRestaurantBill.aspx?tableId=" + entityListBO[iTableNumber].TableId + "&CostCenterId=" + costCenterId;
                    Content1 = @"'><div class='NotDraggable RestaurantTableBookedDiv' style='width:" + entityListBO[iTableNumber].TableWidth + "px; height:" + entityListBO[iTableNumber].TableHeight + "px; top:" + entityListBO[iTableNumber].YCoordinate + "px; left:" + entityListBO[iTableNumber].XCoordinate + "px;' id='" + entityListBO[iTableNumber].TableManagementId + "'>";
                    Content2 = @"<div class='RestaurantTableBookedDiv'>
                                        </div>
                                        <div class='RoomNumberDiv'>" + entityListBO[iTableNumber].TableNumber + "</div></div></a>";

                }
                if (entityListBO[iTableNumber].StatusId == 3)
                {
                    Content0 = string.Empty;
                    Content1 = @"<div class='NotDraggable RestaurantTableReservedDiv' style='width:" + entityListBO[iTableNumber].TableWidth + "px; height:" + entityListBO[iTableNumber].TableHeight + "px; top:" + entityListBO[iTableNumber].YCoordinate + "px; left:" + entityListBO[iTableNumber].XCoordinate + "px;' id='" + entityListBO[iTableNumber].TableManagementId + "'>";
                    Content2 = @"<div class='RestaurantTableReservedDiv'>
                                        </div>
                                        <div class='RoomNumberDiv'>" + entityListBO[iTableNumber].TableNumber + "</div></div>";
                }

                subContent += Content0 + Content1 + Content2;
            }

            fullContent += topPart + topTemplatePart + subContent + endTemplatePart;

            this.ltlRoomTemplate.Text = fullContent;
        }
        private void LoadRoomAllocation()
        {
            //this.btnRestaurantTable.Visible = true;
            //this.btnGuestHouseRoom.Visible = false;

            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<RoomNumberBO> roomNumberListBO = new List<RoomNumberBO>();


            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            RoomTypeDA roomTypeDA = new RoomTypeDA();
            List<RoomTypeBO> RoomTypeInfoList = new List<RoomTypeBO>();
            RoomTypeInfoList = roomTypeDA.GetRoomTypeInfo();

            string fullContent = string.Empty;

            int RoomPossibleVacantDiv = 0;
            int RoomOccupaiedDiv = 0;

            string roomSummary = string.Empty;

            string topPart = @"<div id='legendContainerForRestaurant'>                                
                                <div class='legend RoomOccupaiedDiv'>
                                </div>
                                <div class='legendText'>
                                    Occupaied
                                </div>
                                <div class='legend RoomPossibleVacantDiv'>
                                </div>
                                <div class='legendText'>
                                    Possible Vacant</div>
                            </div><div class='divClear'>
                            </div>
                            <div id='SearchPanel' class='block'>
                                <a href='#' class='block-heading' data-toggle='collapse'>";

            string topTemplatePart = @"</a>
                                <div class='block-body collapse in'>           
                                ";
            string groupNamePart = string.Empty;

            string endTemplatePart = @"</div>
                            </div>";

            roomNumberListBO = roomNumberDA.GetRoomNumberInfoByRoomType(0);

            string subContent = string.Empty;

            for (int iRoomNumber = 0; iRoomNumber < roomNumberListBO.Count; iRoomNumber++)
            {
                if (roomNumberListBO[iRoomNumber].StatusId == 2)
                {
                    if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomPossibleVacantDiv")
                    {
                        subContent += @"<a href='/POS/frmRestaurantBill.aspx?RoomNumber=" + roomNumberListBO[iRoomNumber].RoomNumber;
                        subContent += "'>";
                        subContent += @"<div class='DivRoomContainer'><div class='RoomPossibleVacantDiv'></div>
                                            <div class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div></a>";
                        RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;
                    }
                    else
                    {
                        subContent += @"<a href='/POS/frmRestaurantBill.aspx?RoomNumber=" + roomNumberListBO[iRoomNumber].RoomNumber;
                        subContent += "'>";
                        subContent += @"<div class='DivRoomContainer'><div class='RoomOccupaiedDiv'></div>
                                            <div class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div></a>";
                        RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;
                    }
                }
            }

            roomSummary = " (Occupaied: " + RoomOccupaiedDiv + ", Possible Vacant: " + RoomPossibleVacantDiv + ")";

            groupNamePart = "Status" + roomSummary;

            fullContent += subContent;

            this.ltlRoomTemplate.Text = topPart + groupNamePart + topTemplatePart + fullContent + endTemplatePart;

            /*
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<RoomNumberBO> roomNumberListBO = new List<RoomNumberBO>();


            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            RoomTypeDA roomTypeDA = new RoomTypeDA();
            List<RoomTypeBO> RoomTypeInfoList = new List<RoomTypeBO>();
            RoomTypeInfoList = roomTypeDA.GetRoomTypeInfo();
            string fullContent = string.Empty;
            int BookedRoomCount = 0;
            string roomSummary = string.Empty;

            for (int statusInfo = 0; statusInfo < RoomTypeInfoList.Count; statusInfo++)
            {
                roomNumberListBO = roomNumberDA.GetRoomNumberInfoForRestaurantBillByRoomType(RoomTypeInfoList[statusInfo].RoomTypeId);

                string topPart = @"<div class='divClear'>
                            </div>
                            <div id='SearchPanel' class='block'>
                                <a href='#' class='block-heading' data-toggle='collapse'>";

                string topTemplatePart = @"</a>
                                <div class='block-body collapse in'>           
                                ";

                string endTemplatePart = @"</div>
                            </div>";
                string Content0 = string.Empty;
                string subContent = string.Empty;

                for (int iRoomNumber = 0; iRoomNumber < roomNumberListBO.Count; iRoomNumber++)
                {
                    if (roomNumberListBO[iRoomNumber].StatusId == 2)
                    {
                        Content0 = @"<a href='/Restaurant/frmRestaurantBill.aspx?RoomNumber=" + roomNumberListBO[iRoomNumber].RoomNumber;
                        subContent += @"'><div class='DivRoomContainer'>
                                        <div class='RoomBookedDiv'>
                                        </div>
                                        <div class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div></a>";
                        BookedRoomCount = BookedRoomCount + 1;
                    }
                }

                roomSummary = " (Booked: " + BookedRoomCount + ")";
                string groupNamePart = RoomTypeInfoList[statusInfo].RoomType + roomSummary;

                fullContent += topPart + groupNamePart + topTemplatePart + Content0 + subContent + endTemplatePart;
                BookedRoomCount = 0;
            }

            this.ltlRoomTemplate.Text = fullContent;
             */
        }
    }
}