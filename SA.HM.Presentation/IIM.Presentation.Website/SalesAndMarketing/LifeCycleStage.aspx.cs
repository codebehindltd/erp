using HotelManagement.Data.HMCommon;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class LifeCycleStage : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        [WebMethod]
        public static ReturnInfo SaveUpdateDealStatus(SMLifeCycleStageBO SMLifeCycleStageBO)
        {
            SalesMarketingLogType<ContactInformationBO> logDA = new SalesMarketingLogType<ContactInformationBO>();

            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (SMLifeCycleStageBO.LifeCycleStage != "")
            {
                SMLifeCycleStageBO LifeCycleBO = new SMLifeCycleStageBO();
                LifeCycleBO.Id = SMLifeCycleStageBO.Id;
                LifeCycleBO.LifeCycleStage = SMLifeCycleStageBO.LifeCycleStage;
                LifeCycleBO.DisplaySequence = SMLifeCycleStageBO.DisplaySequence;
                LifeCycleBO.Description = SMLifeCycleStageBO.Description;
                LifeCycleBO.IsRelatedToDeal = SMLifeCycleStageBO.IsRelatedToDeal;
                LifeCycleBO.ForcastType = SMLifeCycleStageBO.ForcastType;
                LifeCycleBO.DealType = SMLifeCycleStageBO.DealType;
                if (SMLifeCycleStageBO.Id == 0)
                {
                    LifeCycleBO.CreatedBy = Convert.ToInt32(userInformationBO.UserInfoId);
                }
                else
                {
                    LifeCycleBO.LastModifiedBy = Convert.ToInt32(userInformationBO.UserInfoId);
                }
                long OutId;
                LifeCycleStageDA DA = new LifeCycleStageDA();

                status = DA.SaveUpdateDealStage(LifeCycleBO, out OutId);


                if (status)
                {
                    if (LifeCycleBO.Id == 0)
                    {

                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                                EntityTypeEnum.EntityType.WorkingPlan.ToString(), OutId,
                                ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                                hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.WorkingPlan));

                    }
                    else
                    {

                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                               EntityTypeEnum.EntityType.SalaryHead.ToString(), OutId,
                               ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalaryHead));
                    }


                }
                else
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

                }
                return rtninfo;
            }
            else
                return rtninfo;
        }

        [WebMethod]
        public static GridViewDataNPaging<SMLifeCycleStageBO, GridPaging> LoadCompanyStatusSearch(string lifeCycleStage, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<SMLifeCycleStageBO, GridPaging> myGridData = new GridViewDataNPaging<SMLifeCycleStageBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<SMLifeCycleStageBO> contactInformation = new List<SMLifeCycleStageBO>();
            LifeCycleStageDA DA = new LifeCycleStageDA();
            contactInformation = DA.GetlifeCycleStagePagination(lifeCycleStage, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(contactInformation, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static SMLifeCycleStageBO FillForm(long Id)
        {

            SMLifeCycleStageBO StatusBO = new SMLifeCycleStageBO();
            LifeCycleStageDA DA = new LifeCycleStageDA();
            StatusBO = DA.GetDealStageWiseCompanyStatusById(Id);

            return StatusBO;
        }

        [WebMethod]
        public static ReturnInfo DeleteStatus(long Id)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            LifeCycleStageDA DA = new LifeCycleStageDA();
            status = DA.DeleteStatus(Id);
            if (status)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                               EntityTypeEnum.EntityType.SalaryHead.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalaryHead));
            }
            return rtninfo;
        }
        [WebMethod]
        public static int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate, string id)
        {
            string tableName = "SMLifeCycleStage";
            string pkFieldName = "Id";
            string pkFieldValue = id;
            int IsDuplicate = 0;
            if (!string.IsNullOrWhiteSpace(pkFieldValue))
            {
                isUpdate = 1;
            }
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }

        [WebMethod]
        public static bool DuplicateTypeCheck(SMLifeCycleStageBO sMLifeCycleStage)
        {
            bool status = false;
            LifeCycleStageDA DA = new LifeCycleStageDA();
            status = DA.DuplicateTypeCheck(sMLifeCycleStage);
            return status;
        }
    }
}