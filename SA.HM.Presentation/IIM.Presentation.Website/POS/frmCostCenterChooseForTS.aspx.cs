using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmCostCenterChooseForTS : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadCostCenterInformation();
        }
        private void LoadCostCenterInformation()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            
            if (userInformationBO != null)
            {

                CostCentreTabDA entityDA = new CostCentreTabDA();
                List<CostCentreTabBO> entityListBO = new List<CostCentreTabBO>();
                entityListBO = entityDA.GetRestaurantTypeCostCentreTabInfo(userInformationBO.DisplayName, userInformationBO.UserInfoId, 1);
                int RoomPossibleVacantDiv = 0;
                string fullContent = string.Empty;
                string roomSummary = string.Empty;

                string topPart = "<a href='javascript:void()' class='block-heading'>";
                string topTemplatePart = "</a> <div> <div style='border: 1px solid #ccc; margin:5px; overflow:hidden;'>";
                string groupNamePart = "Cost Center Information";
                string endTemplatePart = "</div> </div>";

                string subContent = string.Empty;

                for (int iListNumber = 0; iListNumber < entityListBO.Count; iListNumber++)
                {
                    if (entityListBO[iListNumber].DefaultView == "Token")
                    {
                        subContent += @"<a href='/POS/frmKotBill.aspx?Kot=TokenAllocation&CostCenterId=" + entityListBO[iListNumber].CostCenterId;
                    }
                    else if (entityListBO[iListNumber].DefaultView == "Table")
                    {
                        subContent += @"<a href='/POS/frmKotBill.aspx?Kot=TableAllocation&CostCenterId=" + entityListBO[iListNumber].CostCenterId;
                    }
                    else if (entityListBO[iListNumber].DefaultView == "Room")
                    {
                        subContent += @"<a href='/POS/frmKotBill.aspx?Kot=RoomAllocation&CostCenterId=" + entityListBO[iListNumber].CostCenterId;
                    }
                    subContent += "'>";
                    subContent += @"<div class='DivRoomContainer' style='padding-bottom:5px;' ><div class='RoomVacantDiv'></div>
                                            <div class='RoomNumberDiv'>" + entityListBO[iListNumber].CostCenter + "</div></div></a>";
                    RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;
                }

                fullContent += subContent;

                this.ltlRoomTemplate.Text = topPart + groupNamePart + topTemplatePart + fullContent + endTemplatePart;
            }
            else
            {
                Response.Redirect("/POS/Login.aspx");
            }
        }
    }
}