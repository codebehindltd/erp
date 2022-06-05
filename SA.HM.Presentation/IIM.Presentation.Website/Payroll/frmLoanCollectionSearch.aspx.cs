using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Web.Services;

using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmLoanCollectionSearch : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckObjectPermission();
            }
        }

        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmEmpLoan.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSearch.Visible = isSavePermission;
        }

        [WebMethod]
        public static GridViewDataNPaging<EmpLoanCollectionDetails, GridPaging> LoadEmployeeLoanCollectionInfo(string empId, string loanType, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<EmpLoanCollectionDetails, GridPaging> myGridData = new GridViewDataNPaging<EmpLoanCollectionDetails, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            EmpLoanDA da = new EmpLoanDA();
            List<EmpLoanCollectionDetails> loan = da.GetLoanCollectionDetailsForSearch(empId, loanType, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(loan, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static LoanCollectionBO GetLastLoanCollectionInfo(int loanId)
        {
            EmpLoanDA loanDa = new EmpLoanDA();
            EmpLoanBO loan = new EmpLoanBO();
            LoanCollectionBO loanCollection = new LoanCollectionBO();
            loan = loanDa.GetLoanByLoanId(loanId);
            loanCollection = loanDa.GetLastPaidLoanCollectionByLoanId(loanId);

            loanCollection.LoanAmount = loan.LoanAmount + loan.InterestAmount;
            loanCollection.DueAmount = loan.DueAmount;
            loanCollection.OverdueAmount = loan.OverDueAmount;

            return loanCollection;
        }

        [WebMethod]
        public static ReturnInfo UpdateLoanCollection(LoanCollectionBO loanCollection)
        {
            ReturnInfo rtninfo = new ReturnInfo();

            try
            {
                string message = string.Empty;
                HMUtility hmUtility = new HMUtility();

                EmpLoanDA loanDa = new EmpLoanDA();
                frmLoanCollectionSearch frm = new frmLoanCollectionSearch();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = frm.hmUtility.GetCurrentApplicationUserInfo();

                loanCollection.LastModifiedBy = userInformationBO.UserInfoId;

                rtninfo.IsSuccess = loanDa.UpdateLoanCollection(loanCollection);

                if (rtninfo.IsSuccess)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.LoanCollection.ToString(), loanCollection.CollectionId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LoanCollection));
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
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error + CommonHelper.ExceptionErrorMessage(ex), AlertType.Error);
            }

            return rtninfo;
        }


        [WebMethod]
        public static ReturnInfo ApprovedLoanCollection(int collectionId, int loanId, string approvedStatus)
        {
            ReturnInfo rtninfo = new ReturnInfo();

            try
            {
                string message = string.Empty;
                HMUtility hmUtility = new HMUtility();

                EmpLoanDA loanDa = new EmpLoanDA();
                frmLoanCollectionSearch frm = new frmLoanCollectionSearch();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = frm.hmUtility.GetCurrentApplicationUserInfo();

                if (approvedStatus != HMConstants.ApprovalStatus.Approved.ToString())
                {
                    approvedStatus = HMConstants.ApprovalStatus.Approved.ToString();
                }

                rtninfo.IsSuccess = loanDa.ApprovedLoanCollection(collectionId, loanId, approvedStatus);

                if (rtninfo.IsSuccess)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(),
                           EntityTypeEnum.EntityType.LoanCollection.ToString(), collectionId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LoanCollection));
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
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error + CommonHelper.ExceptionErrorMessage(ex), AlertType.Error);
            }

            return rtninfo;
        }

        [WebMethod]
        public static ReturnInfo DeleteLoanCollection(int collectionId, int loanId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();

            try
            {
                string message = string.Empty;

                EmpLoanDA loanDa = new EmpLoanDA();
                frmLoanCollectionSearch frm = new frmLoanCollectionSearch();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = frm.hmUtility.GetCurrentApplicationUserInfo();

                rtninfo.IsSuccess = loanDa.DeleteLoanCollection(collectionId, loanId);

                if (rtninfo.IsSuccess)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                           EntityTypeEnum.EntityType.LoanCollection.ToString(), collectionId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LoanCollection));
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
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error + CommonHelper.ExceptionErrorMessage(ex), AlertType.Error);
            }

            return rtninfo;
        }
    }
}