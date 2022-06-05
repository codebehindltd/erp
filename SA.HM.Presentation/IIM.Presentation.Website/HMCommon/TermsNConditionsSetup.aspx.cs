using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class TermsNConditionsSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static List<CustomFieldBO> GetAllConditionFor()
        {
            List<CustomFieldBO> fieldBOs = new List<CustomFieldBO>();
            HMCommonDA DA = new HMCommonDA();
            fieldBOs = DA.GetCustomFieldList("TermsAndConditions");
            return fieldBOs;
        }
        [WebMethod]
        public static int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate, string id)
        {
            string tableName = "TermsNConditionsMaster";
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
        public static ReturnInfo SaveOrUpdateTermsNConditions(TermsNConditionsMasterBO TermsNConditionsMasterBO, string ConditionForList)
        {
            bool status = false;
            long OutId;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            TermsNConditionsDA termsNConditionsDA = new TermsNConditionsDA();
            status = termsNConditionsDA.SaveOrUpdateTermsNConditions(TermsNConditionsMasterBO, ConditionForList, out OutId);
            if (status)
            {
                if (TermsNConditionsMasterBO.Id == 0)
                {

                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.HMCommonSetup.ToString(), OutId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HMCommonSetup));

                }
                else
                {

                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.HMCommonSetup.ToString(), OutId,
                           ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HMCommonSetup));
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
        public static GridViewDataNPaging<TermsNConditionsMasterBO, GridPaging> LoadTermsNConditionsSearch(string Title, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<TermsNConditionsMasterBO, GridPaging> myGridData = new GridViewDataNPaging<TermsNConditionsMasterBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<TermsNConditionsMasterBO> contactInformation = new List<TermsNConditionsMasterBO>();
            TermsNConditionsDA termsNConditionsDA = new TermsNConditionsDA();
            contactInformation = termsNConditionsDA.GetTermsNConditionsPagination(Title, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(contactInformation, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static TermsNConditionsMasterBO FillForm(long Id)
        {
            TermsNConditionsMasterBO TermsNConditions = new TermsNConditionsMasterBO();
            TermsNConditionsDA termsNConditionsDA = new TermsNConditionsDA();
            TermsNConditions = termsNConditionsDA.GetTermsNConditionsById(Id);
            TermsNConditions.Details = termsNConditionsDA.GetTermsNConditionsDetailsByMasterId(Id);

            return TermsNConditions;
        }
        [WebMethod]
        public static ReturnInfo DeleteAction(int id)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            HMCommonDA DA = new HMCommonDA();
            status = DA.DeleteInfoById("TermsNConditionsMaster", "Id", id);
            if(status)
            {
                status = DA.DeleteInfoById("TermsNConditionsDetails", "TermsNConditionsId", id);
            }
            if (status)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                               EntityTypeEnum.EntityType.HMCommonSetup.ToString(), id,
                               ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HMCommonSetup));
            }
            else
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninfo;
        }
    }
}