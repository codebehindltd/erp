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
    public partial class SegmentInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        [WebMethod]
        public static ReturnInfo SaveUpdateSegmentInformation(SMSegmentInformationBO sMSegmentInformationBO)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (sMSegmentInformationBO.Id == 0)
            {
                sMSegmentInformationBO.CreatedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            else
            {
                sMSegmentInformationBO.LastModifiedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            long OutId;
            SetupDA DA = new SetupDA();

            status = DA.SaveUpdateSegmentInformation(sMSegmentInformationBO, out OutId);
            if (status)
            {
                if (sMSegmentInformationBO.Id == 0)
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
        public static GridViewDataNPaging<SMSegmentInformationBO, GridPaging> LoadSegmentNameSearch(string SegmentName, bool Status, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<SMSegmentInformationBO, GridPaging> myGridData = new GridViewDataNPaging<SMSegmentInformationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<SMSegmentInformationBO> contactInformation = new List<SMSegmentInformationBO>();
            SetupDA DA = new SetupDA();
            contactInformation = DA.GetSegmentInformationPagination(SegmentName, Status, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(contactInformation, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static SMSegmentInformationBO FillForm(long Id)
        {

            SMSegmentInformationBO SegmentBO = new SMSegmentInformationBO();
            SetupDA DA = new SetupDA();
            SegmentBO = DA.GetSegmentInformationBOById(Id);

            return SegmentBO;
        }

        [WebMethod]
        public static ReturnInfo DeleteSegment(long Id)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            SetupDA DA = new SetupDA();
            status = DA.DeleteSegment(Id);
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
            string tableName = "SMSegmentInformation";
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