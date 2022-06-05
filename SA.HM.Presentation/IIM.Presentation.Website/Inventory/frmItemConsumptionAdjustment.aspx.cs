using HotelManagement.Data.Inventory;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class frmItemConsumptionAdjustment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        [WebMethod]
        public static ReturnInfo GetOutIdByIssueNumber(string issueNumber)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            PMProductOutDA outDA = new PMProductOutDA();
            PMProductOutBO productOut = outDA.GetProductOutByIssueNumber(issueNumber);
            if (productOut != null)
            {
                returnInfo.IsSuccess = true;
                returnInfo.Data = productOut.OutId;
            }
            return returnInfo;
        }

        [WebMethod]
        public static InvItemConsumptionAdjustmentViewBO GetConsumptionDetails(int outId)
        {
            InvItemConsumptionAdjustmentViewBO adjustments = new InvItemConsumptionAdjustmentViewBO();
            InvUnitHeadDA unitHeadDA = new InvUnitHeadDA();
            PMProductOutDA outDA = new PMProductOutDA();

            adjustments.AllUnitHeads = unitHeadDA.GetInvUnitHeadInfo();
            adjustments.ProductOutDetails = outDA.GetProductOutDetailsById(outId);

            return adjustments;
        }

        [WebMethod]
        public static ReturnInfo AdjustConsumption(List<PMProductOutDetailsBO> ProductOutDetails)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            PMProductOutDA outDA = new PMProductOutDA();
            HMUtility hmUtility = new HMUtility();
            try
            {
                returnInfo.IsSuccess = outDA.UpdateProductOutConsumptionAdjustment(ProductOutDetails);
                if (returnInfo.IsSuccess)
                {
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    foreach (var item in ProductOutDetails)
                    {
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), 
                            EntityTypeEnum.EntityType.PMProductOutDetails.ToString(), item.OutDetailsId, 
                            ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductOutDetails));
                    }
                }
                else
                {
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
             
            return returnInfo;
        }
    }
}