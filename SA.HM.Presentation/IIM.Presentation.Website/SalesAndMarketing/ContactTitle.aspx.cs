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
    public partial class ContactTitle : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static ReturnInfo SaveUpdateContactTitle(ContactTitleBO contactTitleBO)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (contactTitleBO.Id == 0)
            {
                contactTitleBO.CreatedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            else
            {
                contactTitleBO.LastModifiedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            long OutId;
            SetupDA DA = new SetupDA();

            status = DA.SaveUpdateContactTitle(contactTitleBO, out OutId);
            if (status)
            {
                if (contactTitleBO.Id == 0)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.ContactTitle.ToString(), OutId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ContactTitle));
                }
                else
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.ContactTitle.ToString(), OutId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ContactTitle));
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
        public static GridViewDataNPaging<ContactTitleBO, GridPaging> LoadContactTitleSearch(string paramTitle, string paramTransectionType, string paramStatus, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
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

            GridViewDataNPaging<ContactTitleBO, GridPaging> myGridData = new GridViewDataNPaging<ContactTitleBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<ContactTitleBO> contactInformation = new List<ContactTitleBO>();
            SetupDA DA = new SetupDA();
            contactInformation = DA.GetContactTitleSearchForPagination(paramTitle, paramTransectionType, status, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(contactInformation, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static ContactTitleBO FillForm(long Id)
        {
            ContactTitleBO SourceBO = new ContactTitleBO();
            SetupDA DA = new SetupDA();
            SourceBO = DA.GetContactTitleInformationById(Id);

            return SourceBO;
        }
        //[WebMethod]
        //public static List<ContactTitleBO> LoadSourceByAutoSearch(string searchTerm)
        //{
        //    List<ContactTitleBO> sMSources = new List<ContactTitleBO>();
        //    SetupDA setupDA = new SetupDA();
        //    sMSources = setupDA.GetSourceInfoForAutoSearch(searchTerm);
        //    return sMSources;
        //}

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
            status = DA.DeleteContactTitle(Id);
            if (status)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                               EntityTypeEnum.EntityType.ContactTitle.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ContactTitle));
            }
            return rtninfo;
        }
        [WebMethod]
        public static int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate, string id)
        {
            string tableName = "SMContactDetailsTitle";
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
