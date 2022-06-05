using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.UserInformation;
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
    public partial class InvItemAttributeSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static GridViewDataNPaging<InvItemAttributeBO, GridPaging> GetInvItemAttributeSetupBySearchCriteria(string name, string setupType, string status, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;
            bool? Status = null;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<InvItemAttributeBO, GridPaging> myGridData = new GridViewDataNPaging<InvItemAttributeBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            List<InvItemAttributeBO> SupportNCaseSetupList = new List<InvItemAttributeBO>();
            InvItemAttributeDA DA = new InvItemAttributeDA();
            if (status == "1")
                Status = true;
            else if (status == "0")
                Status = false;
            else
                Status = null;
            SupportNCaseSetupList = DA.GetInvItemAttributeSetupBySearchCriteria(name, setupType, Status, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(SupportNCaseSetupList, totalRecords);
            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo DeleteSetup(long id)
        {
            ReturnInfo info = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();

            try
            {
                status = hmCommonDA.DeleteInfoById("InvItemAttribute", "Id", id);

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
    }
}