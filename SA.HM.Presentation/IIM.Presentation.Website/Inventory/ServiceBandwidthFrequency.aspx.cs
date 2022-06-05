using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Web.Services;

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class ServiceBandwidthFrequency : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CheckPermission();
        }
        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }
        [WebMethod]
        public static GridViewDataNPaging<InvServiceFrequency, GridPaging> SearchFreequency(string freequency, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<InvServiceFrequency, GridPaging> myGridData = new GridViewDataNPaging<InvServiceFrequency, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<InvServiceFrequency> freequencyList = new List<InvServiceFrequency>();
            InvServiceFrequencyDA serviceFrequencyDA = new InvServiceFrequencyDA();
            freequencyList = serviceFrequencyDA.GetFrequencyBySearchCriteria(freequency, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(freequencyList, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo SaveOrUpdateFreeQuency(int freequency, int id)
        {
            ReturnInfo info = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            InvServiceFrequencyDA serviceFrequencyDA = new InvServiceFrequencyDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            InvServiceFrequency frequencyBO = new InvServiceFrequency()
            {
                Id = id,
                Frequency = freequency
            };

            int outId = 0;
            int isUpdate = id > 0 ? 1 : 0;

            bool isExist = hmCommonDA.DuplicateCheckDynamicaly("InvServiceFrequency", "Frequency", frequencyBO.Frequency.ToString(), isUpdate, "Id", frequencyBO.Id.ToString()) > 0;

            if (isExist)
            {
                info.IsSuccess = false;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Duplicate, AlertType.Warning);
                return info;
            }

            try
            {
                info.IsSuccess = serviceFrequencyDA.SaveOrUpdateFrequency(frequencyBO, out outId);

                if (info.IsSuccess)
                {
                    if (id == 0)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.InvServiceFrequency.ToString(), outId, ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvServiceFrequency));
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        info.Data = 0;
                    }
                    else
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.InvServiceFrequency.ToString(), frequencyBO.Id, ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvServiceFrequency));
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
        public static InvServiceFrequency GetFreequencyId(int id)
        {
            InvServiceFrequency frequency = new InvServiceFrequency();
            CommonDA commonDA = new CommonDA();

            frequency = commonDA.GetTableRow<InvServiceFrequency>("InvServiceFrequency", "Id", id.ToString());

            return frequency;
        }

        [WebMethod]
        public static ReturnInfo DeleteFreequency(int id)
        {
            ReturnInfo info = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();

            try
            {
                info.IsSuccess = hmCommonDA.DeleteInfoById("InvServiceFrequency", "Id", id);

                if (info.IsSuccess)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.InvServiceFrequency.ToString(), id, ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvServiceFrequency));
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
    }
}