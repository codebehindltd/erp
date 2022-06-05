using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using System.Web.Services;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpLoanHoldup : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadLoanInfo();
            }
        }

        private void LoadLoanInfo()
        {
            int loanId = 0;

            if (Request.QueryString["lid"] != null)
            {
                loanId = Convert.ToInt32(Request.QueryString["lid"]);
                hfLoanId.Value = loanId.ToString();
            }
            else
            {
                hfLoanId.Value = string.Empty;
                return;
            }

            EmpLoanDA loanDa = new EmpLoanDA();
            EmpLoanBO loan = new EmpLoanBO();
            loan = loanDa.GetLoanByLoanId(loanId);

            LoanCollectionBO loanCollection = new LoanCollectionBO();
            loanCollection = loanDa.GetLastPaidLoanCollectionByLoanId(loanId);

            LoanHoldupBO loanHoldup = new LoanHoldupBO();
            loanHoldup = loanDa.GetGetPayrollLoanHoldupByLoanId(loanId);

            if (loan != null)
            {
                txtLoanAmount.Text = loan.LoanAmount.ToString();
                txtPerInstallAmount.Text = (loan.PerInstallLoanAmount + loan.PerInstallInterestAmount).ToString();
                txtDueAmount.Text = loan.DueAmount.ToString();
                txtOverDueAmount.Text = loan.DueAmount.ToString();
            }

            if (loanCollection != null)
            {
                txtInstallmentNumberWhenLoanHoldup.Text = loanCollection.InstallmentNumber.ToString();
            }
            else
            {
                txtInstallmentNumberWhenLoanHoldup.Text = "0";
            }

            if (loanHoldup != null)
            {
                hfLoanHoldupId.Value = loanHoldup.LoanHoldupId.ToString();

                ddlHoldupDuration.SelectedValue = loanHoldup.DurationForLoanHoldup.ToString();
                ddlLoanHoldupForMonthOrYear.SelectedValue = loanHoldup.HoldForMonthOrYear;
                txtHoldupDate.Text = hmUtility.GetStringFromDateTime(loanHoldup.LoanHoldupDateFrom);
                txtHoldupDate.Text = hmUtility.GetStringFromDateTime(loanHoldup.LoanHoldupDateTo);
                //ddlLoanStatus.SelectedValue = loanHoldup.LoanStatus ? "1" : "0";

                txtApprovedDate.Text = hmUtility.GetStringFromDateTime(Convert.ToDateTime(loanHoldup.ApprovedDate));

                ContentPlaceHolder mpContentPlaceHolder;
                UserControl approvedUser;

                mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
                approvedUser = (UserControl)mpContentPlaceHolder.FindControl("approvedByEmployee");
                ((HiddenField)approvedUser.FindControl("hfEmployeeId")).Value = loanHoldup.ApprovedBy.ToString();
                ((HiddenField)approvedUser.FindControl("hfEmployeeName")).Value = loanHoldup.ApprovedEmployeeName;
                ((TextBox)approvedUser.FindControl("txtSearchEmployee")).Text = loanHoldup.ApproveEmpCode;
                ((TextBox)approvedUser.FindControl("txtEmployeeName")).Text = loanHoldup.ApprovedEmployeeName;

                btnSave.Text = "Update";
            }
            else
            {
                btnSave.Text = "Save";
                hfLoanHoldupId.Value = string.Empty;
            }
        }

        [WebMethod]
        public static bool SaveLoanHoldUp(LoanHoldupBO loanHoldup)
        {
            int loanHoldupId = 0;
            bool status = false;
            HMUtility hmUtility = new HMUtility();

            frmEmpLoanHoldup frm = new frmEmpLoanHoldup();
            EmpLoanDA loanDa = new EmpLoanDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = frm.hmUtility.GetCurrentApplicationUserInfo();

            loanHoldup.CreatedBy = userInformationBO.UserInfoId;
            status = loanDa.SaveLoanHoldUp(loanHoldup, out loanHoldupId);

            if (status)
            {
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.LoanHoldup.ToString(), loanHoldupId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LoanHoldup));
            }

            return status;
        }

        [WebMethod]
        public static bool UpdateEmpLoan(LoanHoldupBO loanHoldup)
        {
            bool status = false;
            HMUtility hmUtility = new HMUtility();

            EmpLoanDA loanDa = new EmpLoanDA();
            frmEmpLoanHoldup frm = new frmEmpLoanHoldup();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = frm.hmUtility.GetCurrentApplicationUserInfo();

            loanHoldup.LastModifiedBy = userInformationBO.UserInfoId;

            status = loanDa.UpdateLoanHoldUp(loanHoldup);

            if (status)
            {
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.LoanHoldup.ToString(), loanHoldup.LoanHoldupId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LoanHoldup));
            }
            //else
            //    status = false;

            return status;
        }
    }
}