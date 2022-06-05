using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmCostCenterChooseForKot : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            //if (userInformationBO.UserInfoId != 1)
            //{
                string redirectUrl = "frmCostCenterSelectionForAll.aspx";
                string script = string.Format("window.open('{0}','_blank');", redirectUrl);

                Page.ClientScript.RegisterStartupScript(this.GetType(),
                    "newPage" + UniqueID, script, true);
            //}
            //else
            //{
            //    LoadCostCenterInformation();
            //}
        }
        private void LoadCostCenterInformation()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO != null)
            {
                CostCentreTabDA entityDA = new CostCentreTabDA();
                List<CostCentreTabBO> entityListBO = new List<CostCentreTabBO>();
                //entityListBO = entityDA.GetRestaurantTypeCostCentreTabInfo(userInformationBO.UserId, userInformationBO.UserInfoId, 0);
                entityListBO = entityDA.GetRestaurantTypeCostCentreTabInfo(userInformationBO.UserId, userInformationBO.UserInfoId, 0);

                if (entityListBO != null)
                {
                    if (entityListBO.Count == 1)
                    {
                        Response.Redirect("/POS/frmKotBillMaster.aspx?Kot=TableAllocation&CostCenterId=" + entityListBO[0].CostCenterId.ToString());
                    }
                    else
                    {
                        int RoomPossibleVacantDiv = 0;
                        string fullContent = string.Empty;
                        string roomSummary = string.Empty;

                        string topPart = @"<div id='SearchPanel' class='panel panel-default'>
                                <div class='panel-heading'>";

                        string topTemplatePart = @"</div>
                                <div class='panel-body'>           
                                ";
                        string groupNamePart = string.Empty;

                        string endTemplatePart = @"</div>
                            </div>";

                        string subContent = string.Empty;

                        for (int iListNumber = 0; iListNumber < entityListBO.Count; iListNumber++)
                        {
                            subContent += @"<a href='/POS/frmKotBillMaster.aspx?Kot=TableAllocation&CostCenterId=" + entityListBO[iListNumber].CostCenterId;
                            subContent += "'>";
                            subContent += @"<div class='DivRoomContainer'><div class='RoomVacantDiv'></div>
                                            <div class='RoomNumberDiv'>" + entityListBO[iListNumber].CostCenter + "</div></div></a>";
                            RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;
                        }

                        groupNamePart = "Cost Center Information";
                        fullContent += subContent;

                        this.ltlRoomTemplate.Text = topPart + groupNamePart + topTemplatePart + fullContent + endTemplatePart;
                    }
                }



            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
    }
}