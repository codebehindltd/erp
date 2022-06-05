using HotelManagement.Data.Payroll;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
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
    public partial class AccountManagerNew : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static List<EmployeeBO> GetEmployeeInfoForAcountManager(string searchString)
        {
            EmployeeDA DA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = DA.GetEmployeeInfoForAcountManager(searchString);
            return empList;
        }
        [WebMethod]
        public static List<AccountManagerBO> GetEmployeeInfoForSupervison(string searchString)
        {
            AccountManagerDA DA = new AccountManagerDA();
            List<AccountManagerBO> empList = new List<AccountManagerBO>();
            empList = DA.GetAccountManagerByAutoSearch(searchString);
            return empList;
        }
        [WebMethod]
        public static ReturnInfo SaveAccountManagerInformation(AccountManagerBO AccountManagerBO)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (AccountManagerBO.AccountManagerId == 0)
            {
                AccountManagerBO.CreatedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            else
            {
                AccountManagerBO.LastModifiedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            int OutId;
            AccountManagerDA DA = new AccountManagerDA();



            if (AccountManagerBO.AccountManagerId == 0)
            {
                status = DA.SaveAccountManagerInfo(AccountManagerBO, out OutId);
                if (status)
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
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

                }
            }
            else
            {
                status = DA.UpdateAccountManagerInfo(AccountManagerBO, out OutId);
                if (status)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.SalaryHead.ToString(), OutId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalaryHead));
                }
                else
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

                }
            }
            return rtninfo;
        }
        [WebMethod]
        public static GridViewDataNPaging<AccountManagerBO, GridPaging> LoadAccountManagerSearch(string AccountManagerName, string SupervisonName, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int status = -1;
            
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<AccountManagerBO, GridPaging> myGridData = new GridViewDataNPaging<AccountManagerBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<AccountManagerBO> contactInformation = new List<AccountManagerBO>();
            AccountManagerDA DA = new AccountManagerDA();
            contactInformation = DA.GetAccountManagerInfoForPagination(AccountManagerName, SupervisonName, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(contactInformation, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static AccountManagerBO FillForm(int Id)
        {

            AccountManagerBO accountManagerBO = new AccountManagerBO();
            AccountManagerDA DA = new AccountManagerDA();
            accountManagerBO = DA.GetNewAccountManagerInfoById(Id);

            return accountManagerBO;
        }
    }
}