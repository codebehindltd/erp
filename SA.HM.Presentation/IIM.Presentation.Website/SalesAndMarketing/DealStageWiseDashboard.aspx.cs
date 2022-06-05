using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class DealStageWiseDashboard : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        public DealStageWiseDashboard() : base("DealCreation")
        {
           
        }

        [WebMethod]
        public static ReturnInfo UpdateDealStage(int dealSatgeId, int dealId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            bool status = false;
            StageDA stageDA = new StageDA();
            SMDeal sMDeal = new SMDeal();
            //GuestCompanyBO currentBO = new GuestCompanyBO();
            SMDeal previousBO = new SMDeal();
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            SalesMarketingLogType<SMDeal> logDA = new SalesMarketingLogType<SMDeal>();
            DealDA dealDA = new DealDA();
            long contactId = 0;
            previousBO = dealDA.GetDealInfoBOById(dealId);
            
            if (previousBO.StageId == dealSatgeId)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo("No Change", AlertType.Information);
                return rtninfo;
            }
            else
            {
                if (!CheckDealStageCanBeChange(dealSatgeId, previousBO))
                {
                    rtninfo.AlertMessage = CommonHelper.AlertInfo("Cannot Change Deal Stage More Than one Stage.", AlertType.Warning);                    
                    return rtninfo;
                }

                rtninfo.IsSuccess = stageDA.UpdateDealStage(dealSatgeId, dealId);
            }
            Deal deal = new Deal();
            if (rtninfo.IsSuccess)
            {
                sMDeal = dealDA.GetDealInfoBOById(dealId);
                rtninfo.AlertMessage = CommonHelper.AlertInfo("Deal Stage Update successful", AlertType.Success);
                rtninfo.Id = sMDeal.StageId;

                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.SMDeal.ToString(), sMDeal.Id,
                            ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                
                if (sMDeal.Id != 0)
                {
                    logDA.Log(ConstantHelper.SalesandMarketingLogType.DealActivity, sMDeal, previousBO);
                }
            }
            else
            {
                rtninfo.AlertMessage = CommonHelper.AlertInfo("Deal Stage Update Failed", AlertType.Warning);
            }
           
            return rtninfo;
        }
        [WebMethod]
        public static ArrayList LoadDealAndStage(int ownerId, string dealName, int dealStageId, int companyId, string dateType, string fromDate, string toDate)
        {
            ArrayList arr = new ArrayList();
            List<SMDeal> deals = new List<SMDeal>();
            DealDA dealDA = new DealDA();
            deals = dealDA.GetAllDeal(ownerId, dealName, dealStageId, companyId, dateType, fromDate, toDate);

            StageDA stageDA = new StageDA();
            List<SMDealStage> dealStages = new List<SMDealStage>();
            dealStages = stageDA.GetAllDealStages();

            arr.Add(new { Deals = deals, DealStages = dealStages });
            return arr;
        }
        private static bool CheckDealStageCanBeChange(int dealSatgeId, SMDeal previous = null)
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDealStageCanChangeMoreThanOneStep", "IsDealStageCanChangeMoreThanOneStep");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue) && setUpBO.SetupValue == "0")
            {
                List<SMDealStage> dealStages = new StageDA().GetAllDealStages();
                if (previous != null)
                {
                    int previousStageSequence = 0, currentStageSequence = 0;
                    foreach (var stage in dealStages)
                    {
                        if (stage.Id == dealSatgeId)
                            currentStageSequence = (int)stage.DisplaySequence;
                        if (stage.Id == previous.StageId)
                            previousStageSequence = (int)stage.DisplaySequence;
                    }
                    return Math.Abs(previousStageSequence - currentStageSequence) > 1 ? false : true;
                }
                else
                {
                    return dealSatgeId == dealStages[0].Id;
                }
            }
            return true;
        }
    }
}