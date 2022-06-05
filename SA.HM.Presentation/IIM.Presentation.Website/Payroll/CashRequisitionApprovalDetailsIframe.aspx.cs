using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class CashRequisitionApprovalDetailsIframe : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        public CashRequisitionApprovalDetailsIframe() : base("CashRequisitionOrBillVoucherInformation")
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckPermission();
                LoadEmployee();
                LoadGLCompany(false);
                IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects();
            }
        }
        private void IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects()
        {

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
            homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects", "IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects");
            if (homePageSetupBO != null)
            {
                if (homePageSetupBO.SetupId > 0)
                {
                    hfIsCashRequisitionAdjustmentWithDifferentCompanyOrProjects.Value = homePageSetupBO.SetupValue;
                }
            }
        }
        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }
        private void LoadGLCompany(bool isSingle)
        {
            GLCompanyDA entityDA = new GLCompanyDA();
            var List = entityDA.GetAllGLCompanyInfo();
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();

            //hfCompanyAll.Value = JsonConvert.SerializeObject(List);

            if (isSingle == true)
            {
                companyList.Add(List[0]);
                ddlGLCompany.DataSource = companyList;
                ddlGLCompany.DataTextField = "Name";
                ddlGLCompany.DataValueField = "CompanyId";
                ddlGLCompany.DataBind();
            }
            else
            {
                ddlGLCompany.DataSource = List;
                ddlGLCompany.DataTextField = "Name";
                ddlGLCompany.DataValueField = "CompanyId";
                ddlGLCompany.DataBind();
                ListItem itemCompany = new ListItem();
                itemCompany.Value = "0";
                itemCompany.Text = hmUtility.GetDropDownFirstValue();
                ddlGLCompany.Items.Insert(0, itemCompany);
            }
        }
        private void LoadEmployee()
        {
            EmployeeDA EmpDA = new EmployeeDA();
            List<EmployeeBO> EmpBO = new List<EmployeeBO>();
            EmpBO = EmpDA.GetEmployeeInfo();

            ddlAssignEmployee.DataSource = EmpBO;
            ddlAssignEmployee.DataTextField = "DisplayName";
            ddlAssignEmployee.DataValueField = "EmpId";
            ddlAssignEmployee.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlAssignEmployee.Items.Insert(0, item);

        }
        [WebMethod]
        public static List<GLProjectBO> LoadProjectByCompanyId(int companyId)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            GLProjectDA entityDA = new GLProjectDA();
            projectList = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(Convert.ToInt32(companyId), Convert.ToInt32(userInformationBO.UserGroupId));

            return projectList;
        }
        [WebMethod]
        public static ReturnInfo DeleteCashRequisition(long id)
        {
            ReturnInfo info = new ReturnInfo();
            HMCommonDA hmCommonDA = new HMCommonDA();
            HMUtility hmUtility = new HMUtility();

            try
            {
                CashRequisitionDA cashRequisitionDA = new CashRequisitionDA();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                var status = cashRequisitionDA.DeleteCashRequisition(id, userInformationBO.UserInfoId);
                if (status)
                {
                    info.IsSuccess = true;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Cancel, AlertType.Success);
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
        public static ReturnInfo AdjustmentWithReturn(long id)
        {
            ReturnInfo info = new ReturnInfo();
            HMCommonDA hmCommonDA = new HMCommonDA();
            HMUtility hmUtility = new HMUtility();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            try
            {
                CashRequisitionDA cashRequisitionDA = new CashRequisitionDA();
                var status = cashRequisitionDA.AdjustmentWithReturnById(id, userInformationBO.UserInfoId);
                if (status)
                {
                    info.IsSuccess = true;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.CashAdjustment, AlertType.Success);
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
        public static List<CashRequisitionBO> GetRequsitionById(long id)
        {
            List<CashRequisitionBO> cashRequisition = new List<CashRequisitionBO>();
            CashRequisitionDA cashRequisitionDA = new CashRequisitionDA();

            cashRequisition = cashRequisitionDA.GetBillVoucherById(id);

            return cashRequisition;
        }
        [WebMethod]
        public static CashRequisitionBO GetCashRequsitionById(long id)
        {
            CashRequisitionBO cashRequisition = new CashRequisitionBO();
            CashRequisitionDA cashRequisitionDA = new CashRequisitionDA();

            cashRequisition = cashRequisitionDA.GetRequsitionById(id);

            return cashRequisition;
        }

        [WebMethod]
        public static HMCommonSetupBO GetFeatureWisePermission(int createdBy)
        {
            HMCommonSetupBO commonBO = new HMCommonSetupBO(); 
            ReturnInfo info = new ReturnInfo();
            HMCommonDA hmCommonDA = new HMCommonDA();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            commonBO = hmCommonDA.GetFeatureWisePermission("CashRequisitionApprovalConfig", "CashRequisitionApprovalConfig", userInformationBO.UserInfoId, "CashRequisition", createdBy);

            return commonBO;
        }

        [WebMethod]
        public static ReturnInfo ApprovalStatusUpdate(long id, string ApprovalStatusUpdate)
        {
            ReturnInfo info = new ReturnInfo();
            HMCommonDA hmCommonDA = new HMCommonDA();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            string TransactionNo;
            string TransactionTypeCash;
            string ApproveStatusCash;

            try
            {
                int isAdminUser = 0;
                //// // // ------User Admin Authorization BO Session Information --------------------------------
                //#region User Admin Authorization                    
                //if (userInformationBO.UserInfoId == 1)
                //{
                //    isAdminUser = 1;
                //}
                //else
                //{
                //    List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                //    adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                //    if (adminAuthorizationList != null)
                //    {
                //        if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 2).Count() > 0)
                //        {
                //            isAdminUser = 1;
                //        }
                //    }
                //}
                //#endregion

                CashRequisitionDA cashRequisitionDA = new CashRequisitionDA();
                var status = cashRequisitionDA.ApprovalStatusUpdate(isAdminUser, id, ApprovalStatusUpdate, userInformationBO.UserInfoId, out TransactionNo, out TransactionTypeCash, out ApproveStatusCash);
                if (status && ApprovalStatusUpdate == "Checked")
                {
                    info.PrimaryKeyValue = id.ToString();
                    info.TransactionNo = TransactionNo;
                    info.TransactionType = TransactionTypeCash;
                    info.TransactionStatus = ApproveStatusCash;
                    info.IsSuccess = true;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
                }
                else if (status && ApprovalStatusUpdate == "Approved")
                {
                    info.PrimaryKeyValue = id.ToString();
                    info.TransactionNo = TransactionNo;
                    info.TransactionType = TransactionTypeCash;
                    info.TransactionStatus = ApproveStatusCash;
                    info.IsSuccess = true;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
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
        public static GridViewDataNPaging<CashRequisitionBO, GridPaging> GetALLAdjustmentForCashRequisitionById(int CashRequisitionId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            List<CashRequisitionBO> cashRequisitionList = new List<CashRequisitionBO>();
            GridViewDataNPaging<CashRequisitionBO, GridPaging> myGridData = new GridViewDataNPaging<CashRequisitionBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            if (userInformationBO.EmpId != 0)
            {



                CashRequisitionDA cashRequisitionDA = new CashRequisitionDA();

                cashRequisitionList = cashRequisitionDA.GetALLAdjustmentForCashRequisitionById(userInformationBO.UserInfoId, CashRequisitionId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            }

            myGridData.GridPagingProcessing(cashRequisitionList, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static List<DocumentsBO> GetRequsitionDocumentsById(long id)
        {
            List<DocumentsBO> documents = new List<DocumentsBO>();
            documents = new DocumentsDA().GetDocumentsByUserTypeAndUserId("CashRequisitionApprovalDoc", id);
            documents = new HMCommonDA().GetDocumentListWithIcon(documents);

            return documents;
        }
    }
}