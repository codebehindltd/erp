using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System.Collections;

namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class frmBudgetApproval : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                LoadCommonDropDownHiddenField();
                LoadCompany();
            }
        }
        private void LoadCompany()
        {
            GLCompanyDA companyDA = new GLCompanyDA();
            List<GLCompanyBO> List = new List<GLCompanyBO>();
            List = companyDA.GetAllGLCompanyInfo();

            ddlCompany.DataSource = List;
            ddlCompany.DataTextField = "Name";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            ddlCompany.Items.Insert(0, itemNodeId);
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        [WebMethod]
        public static GridViewDataNPaging<GLBudgetBO, GridPaging> GetBudgetBySearchCriteria(int glCompanyId, int glProjectId, Int64 fiscalYearId, string approvedStatus, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int totalRecords = 0;

            GridViewDataNPaging<GLBudgetBO, GridPaging> myGridData = new GridViewDataNPaging<GLBudgetBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            GLBudgetDA budgetDA = new GLBudgetDA();
            List<GLBudgetBO> voucher = new List<GLBudgetBO>();
            voucher = budgetDA.GetBudgetBySearch(glCompanyId, glProjectId,fiscalYearId, approvedStatus, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(voucher, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static GLVoucherVwBO GetVoucherDetailsForDisplay(Int64 ledgerMasterId)
        {
            GLVoucherVwBO vouchervw = new GLVoucherVwBO();
            VoucherEntryDA voucherDa = new VoucherEntryDA();

            vouchervw.LedgerMaster = voucherDa.GetVoucherById(ledgerMasterId);
            vouchervw.LedgerMasterDetails = voucherDa.GetVoucherDetailsById(ledgerMasterId);

            vouchervw.TotalCrAmount = vouchervw.LedgerMasterDetails.Sum(c => c.CRAmount);
            vouchervw.TotalDrAmount = vouchervw.LedgerMasterDetails.Sum(c => c.DRAmount);

            return vouchervw;
        }
        [WebMethod]
        public static ReturnInfo ApprovalBudget(GLBudgetBO budgetApproval)
        {
            Boolean status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            GLBudgetDA budgetDa = new GLBudgetDA();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();

            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                budgetApproval.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();
                budgetApproval.ApprovedBy = userInformationBO.UserInfoId;

                status = budgetDa.BudgetApproval(budgetApproval);
                if (status)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                }
                else
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }

            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;
        }
        [WebMethod]
        public static List<GLProjectBO> GetGLProjectByGLCompanyId(int companyId)
        {
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            GLProjectDA entityDA = new GLProjectDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            projectList = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(companyId, userInformationBO.UserGroupId);

            return projectList;
        }
        [WebMethod]
        public static List<GLFiscalYearBO> PopulateFiscalYear(int projectId)
        {
            ArrayList list = new ArrayList();
            List<GLFiscalYearBO> fiscalYearList = new List<GLFiscalYearBO>();
            GLFiscalYearDA entityDA = new GLFiscalYearDA();
            fiscalYearList = entityDA.GetFiscalYearListByProjectId(Convert.ToInt32(projectId));

            return fiscalYearList;
        }
    }
}