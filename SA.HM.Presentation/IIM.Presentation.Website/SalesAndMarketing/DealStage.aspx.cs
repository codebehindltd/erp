using HotelManagement.Data.HMCommon;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class DealStage : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                CheckPermission();
            }

        }

        private void CheckPermission()
        {

            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }

        [WebMethod]
        public static GridViewDataNPaging<SMDealStage, GridPaging> SearchStage(string StageName, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<SMDealStage, GridPaging> myGridData = new GridViewDataNPaging<SMDealStage, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<SMDealStage> stageinfo = new List<SMDealStage>();
            StageDA DA = new StageDA();
            stageinfo = DA.GetStagesBySearchCriteria(StageName, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(stageinfo, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo SaveOrUpdateStage(SMDealStage DealStage)
        {
            ReturnInfo info = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            StageDA stageDA = new StageDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            DealStage.CreatedBy = userInformationBO.UserInfoId;

            int id = 0;

            if (DealStage.Id == 0)
            {
                bool isExist = hmCommonDA.DuplicateCheckDynamicaly("SMDealStage", "DealStage", DealStage.DealStage, 0, "Id", DealStage.Id.ToString()) > 0;

                if (isExist)
                {
                    info.IsSuccess = false;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Duplicate, AlertType.Warning);
                    return info;
                }
            }
            else
            {
                bool isExist = hmCommonDA.DuplicateCheckDynamicaly("SMDealStage", "DealStage", DealStage.DealStage, 1, "Id", DealStage.Id.ToString()) > 0;

                if (isExist)
                {
                    info.IsSuccess = false;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Duplicate, AlertType.Warning);
                    return info;
                }
            }
            try
            {

                info.IsSuccess = stageDA.SaveOrUpdateStage(DealStage, out id);

                if (info.IsSuccess)
                {
                    if (DealStage.Id == 0)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.SMDealStage.ToString(), id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        info.Data = 0;
                    }
                    else
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.SMDealStage.ToString(), DealStage.Id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }
                else
                {
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;
        }

        [WebMethod]
        public static SMDealStage GetStageById(int id)
        {
            SMDealStage stage = new SMDealStage();
            StageDA stageDA = new StageDA();

            stage = stageDA.GetStageById(id);

            return stage;
        }

        [WebMethod]
        public static ReturnInfo DeleteStage(long id)
        {
            ReturnInfo info = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();

            try
            {
                status = hmCommonDA.DeleteInfoById("SMDealStage", "Id", id);

                if (status)
                {
                    info.IsSuccess = true;
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.SMDealStage.ToString(), id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    info.IsSuccess = false;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                info.IsSuccess = false;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;
        }

        [WebMethod]
        public static int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate, string id)
        {
            string tableName = "SMDealStage";
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
        public static bool DuplicateTypeCheck(string forcastType, int id)
        {
            bool status = false;
            DealDA DA = new DealDA();
            status = DA.DuplicateTypeCheck(forcastType, id);
            return status;
        }
    }
}