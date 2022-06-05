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
    public partial class frmEmpLoanCollection : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        int loanId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["lid"] != null)
                {
                    loanId = Convert.ToInt32(Request.QueryString["lid"]);
                    LoadLoanInfo();
                }
            }
        }

        private void LoadLoanInfo()
        {
            EmpLoanDA loanDa = new EmpLoanDA();
            EmpLoanBO loan = new EmpLoanBO();
            LoanCollectionBO loanCollection = new LoanCollectionBO();
            loan = loanDa.GetLoanByLoanId(loanId);
            loanCollection = loanDa.GetLastPaidLoanCollectionByLoanId(loanId);

            hfLoanId.Value = loanId.ToString();
            hfEmployeeId.Value = loan.EmpId.ToString();

            txtLoanAmount.Text = loan.LoanAmount.ToString();
            txtDueAmount.Text = loan.DueAmount.ToString();
            txtOverDueAmount.Text = loan.OverDueAmount.ToString();

            txtPerInstallLoanAmount.Text = loan.PerInstallLoanAmount.ToString();
            txtPerInstallInterestAmount.Text = loan.PerInstallInterestAmount.ToString();

            txtCollectAmount.Text = (loan.PerInstallLoanAmount + loan.PerInstallInterestAmount).ToString();

            if (loanCollection != null)
            {
                txtInstallmentNumber.Text = (loanCollection.InstallmentNumber + 1).ToString();
                txtCollectAmount.Text = (loanCollection.CollectedLoanAmount + loanCollection.CollectedInterestAmount).ToString();
                txtPerInstallLoanAmount.Text = loanCollection.CollectedLoanAmount.ToString();
                txtPerInstallInterestAmount.Text = loanCollection.CollectedInterestAmount.ToString();

                txtCollectDate.Text = hmUtility.GetStringFromDateTime(loanCollection.CollectionDate);
                hfLoanCollectionId.Value = loanCollection.CollectionId.ToString();
            }
            else
            {
                txtInstallmentNumber.Text = "1";
            }
        }

        [WebMethod]
        public static string SaveLoanCollection(LoanCollectionBO loanCollection)
        {
            int loanCollectionId = 0;
            HMUtility hmUtility = new HMUtility();
            bool status = false;
            string message = string.Empty;

            frmEmpLoanCollection frm = new frmEmpLoanCollection();

            EmpLoanDA loanDa = new EmpLoanDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = frm.hmUtility.GetCurrentApplicationUserInfo();

            loanCollection.CreatedBy = userInformationBO.UserInfoId;
            status = loanDa.SaveLoanCollection(loanCollection, out loanCollectionId);

            if (status)
            {
                message = "Save Operation Successfull";
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.LoanCollection.ToString(), loanCollectionId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LoanCollection));

            } 
            else
                message = "Save Operation Unsuccessfull";

            return message;
        }
        [WebMethod]
        public static bool UpdateLoanCollection(LoanCollectionBO loanCollection)
        {
            bool status = false;
            string message = string.Empty;
            HMUtility hmUtility = new HMUtility();

            EmpLoanDA loanDa = new EmpLoanDA();
            frmEmpLoanCollection frm = new frmEmpLoanCollection();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = frm.hmUtility.GetCurrentApplicationUserInfo();

            loanCollection.LastModifiedBy = userInformationBO.UserInfoId;

            status = loanDa.UpdateLoanCollection(loanCollection);
            if (status)
            {
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.LoanCollection.ToString(), loanCollection.CollectionId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LoanCollection));
            }

            return status;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.txtCollectDate.Text = string.Empty;
            this.txtCollectAmount.Text = string.Empty;
        }
    }
}