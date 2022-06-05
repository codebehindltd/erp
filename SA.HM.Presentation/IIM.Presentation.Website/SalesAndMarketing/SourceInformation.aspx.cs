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
    public partial class SourceInformation : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static ReturnInfo SaveUpdateSourceInformation(SMSourceInformationBO sMSourceInformationBO)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (sMSourceInformationBO.Id == 0)
            {
                sMSourceInformationBO.CreatedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            else
            {
                sMSourceInformationBO.LastModifiedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            long OutId;
            SetupDA DA = new SetupDA();

            status = DA.SaveUpdateSourceInformation(sMSourceInformationBO, out OutId);
            if (status)
            {
                if (sMSourceInformationBO.Id == 0)
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

        [WebMethod]
        public static GridViewDataNPaging<SMSourceInformationBO, GridPaging> LoadSourceNameSearch(string SourceName, string paramStatus, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int status = -1;
            if (paramStatus == "0")
            {
                status = -1;
            }
            else if (paramStatus == "Active")
            {
                status = 1;
            }
            else if (paramStatus == "Inactive")
            {
                status = 0;
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<SMSourceInformationBO, GridPaging> myGridData = new GridViewDataNPaging<SMSourceInformationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<SMSourceInformationBO> contactInformation = new List<SMSourceInformationBO>();
            SetupDA DA = new SetupDA();
            contactInformation = DA.GetSourceInformationPagination(SourceName, status, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(contactInformation, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static SMSourceInformationBO FillForm(long Id)
        {

            SMSourceInformationBO SourceBO = new SMSourceInformationBO();
            SetupDA DA = new SetupDA();
            SourceBO = DA.GetSourceInformationBOById(Id);

            return SourceBO;
        }
        [WebMethod]
        public static List<SMSourceInformationBO> LoadSourceByAutoSearch(string searchTerm)
        {
            List<SMSourceInformationBO> sMSources = new List<SMSourceInformationBO>();
            SetupDA setupDA = new SetupDA();
            sMSources = setupDA.GetSourceInfoForAutoSearch(searchTerm);
            return sMSources;
        }

        [WebMethod]
        public static ReturnInfo DeleteSource(long Id)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            SetupDA DA = new SetupDA();
            status = DA.DeleteSource(Id);
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
            string tableName = "SMSourceInformation";
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
        
    }
}
