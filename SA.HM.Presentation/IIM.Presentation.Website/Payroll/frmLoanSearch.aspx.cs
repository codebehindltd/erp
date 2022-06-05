using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmLoanSearch : BasePage
    {
        HMUtility hmUtility = new HMUtility();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckObjectPermission();
            }
        }

        [WebMethod]
        public static GridViewDataNPaging<EmpLoanBO, GridPaging> LoadEmployeeLoanInfo(string empId, string loanType, string loanStatus, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<EmpLoanBO, GridPaging> myGridData = new GridViewDataNPaging<EmpLoanBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            EmpLoanDA da = new EmpLoanDA();
            List<EmpLoanBO> loan = da.SearchLoan(empId, loanType, loanStatus, CommonHelper.GetFilterUser(userInformationBO), userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(loan, totalRecords);
            myGridData.GridBody = GridBody(loan, userInformationBO, myGridData.GridPageLinks.CurrentPageNumber);

            return myGridData;
        }

        private static string GridBody(List<EmpLoanBO> loan, UserInformationBO userInformationBO, int currentPageIndex)
        {
            string body = string.Empty;
            string tr = "", editLink = "", deleteLink = "", holdupLink = "", collectLink = "";
            string approvedLink = "", takenPeriodDisplay = "", cancelLink = "";
            int totalRow = 0;

            CheckAccessPermission<EmpLoanBO> p = new CheckAccessPermission<EmpLoanBO>();

            foreach (EmpLoanBO ln in loan)
            {
                p.CheckApprovedPermission(ln, userInformationBO);

                if ((totalRow % 2) == 0)
                {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else
                {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                if (ln.LoanTakenForPeriod > 1)
                {
                    takenPeriodDisplay = ln.LoanTakenForPeriod + " " + ln.LoanTakenForMonthOrYear + "s";
                }
                else
                {
                    takenPeriodDisplay = ln.LoanTakenForPeriod + " " + ln.LoanTakenForMonthOrYear;
                }

                tr += "<td align='left' style=\"width:15%;\">" + ln.EmployeeName + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + ln.LoanType + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + takenPeriodDisplay + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + ln.LoanAmount + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + ln.InterestRate + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + ln.InterestAmount + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + ln.DueAmount + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + ln.ApprovedStatus + "</td>";

                if ((ln.LoanStatus == "Regular" || ln.LoanStatus == "HoldUp") && ln.ApprovedStatus == HMConstants.ApprovalStatus.Approved.ToString())
                {
                    holdupLink = "&nbsp;<a href=\"javascript:void(0);\"  title=\"Loan Holdup\" onclick=\"javascript:return LoanHoldup(" + ln.LoanId + "," + ln.EmpId + "," + currentPageIndex + ");\"><img alt=\"Loan Holdup\" src=\"../Images/holdup.png\" /></a>";
                    if (ln.IsAutoLoanCollectionProcessEnable == 0)
                    {
                        collectLink = "&nbsp;<a href=\"javascript:void(0);\" title=\"Loan Collection\" onclick=\"javascript:return LoanCollection(" + ln.LoanId + "," + ln.EmpId + "," + currentPageIndex + ");\"><img alt=\"Loan Collection\" src=\"../Images/LoanCollection.png\" /></a>";
                    }
                }

                if ((ln.ApprovedStatus != HMConstants.ApprovalStatus.Checked.ToString() && ln.ApprovedStatus != HMConstants.ApprovalStatus.Approved.ToString() && ln.ApprovedStatus != HMConstants.ApprovalStatus.Cancel.ToString()))
                {
                    editLink = "<a href=\"javascript:void(0);\"  title=\"Edit\" onclick=\"javascript:return PerformFillFormAction('" + ln.LoanId + "', '" + currentPageIndex + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" /> </a>";
                }

                if (ln.ApprovedStatus != HMConstants.ApprovalStatus.Checked.ToString() && ln.ApprovedStatus != HMConstants.ApprovalStatus.Approved.ToString())
                {
                    approvedLink = "&nbsp;<a href=\"javascript:void(0);\"  title=\"Checked\" onclick=\"javascript:return LoanApproved(" + ln.LoanId + "," + ln.EmpId + ",'" + ln.ApprovedStatus + "'," + ln.CreatedBy + "," + currentPageIndex + ");\"> <img alt=\"Checked\" src=\"../Images/checked.png\" /> </a>";
                }
                else if (ln.ApprovedStatus != HMConstants.ApprovalStatus.Approved.ToString())
                {
                    approvedLink = "&nbsp;<a href=\"javascript:void(0);\"  title=\"Approved\" onclick=\"javascript:return LoanApproved(" + ln.LoanId + "," + ln.EmpId + ",'" + ln.ApprovedStatus + "'," + ln.CreatedBy + "," + currentPageIndex + ");\"> <img alt=\"Approval\" src=\"../Images/approved.png\" /> </a>";
                }
                else if ((ln.ApprovedStatus != HMConstants.ApprovalStatus.Cancel.ToString() && ln.ApprovedStatus != HMConstants.ApprovalStatus.Approved.ToString()))
                {
                    cancelLink = "&nbsp;<a href=\"javascript:void(0);\"  title=\"Cancel\" onclick=\"javascript:return LoanCancel(" + ln.LoanId + "," + ln.EmpId + "," + currentPageIndex + ");\"> <img alt=\"Cancel\" src=\"../Images/cancel.png\" /> </a>";
                }

                tr += "<td align='center' style=\"width:15%;\">" + editLink + cancelLink + approvedLink + collectLink + holdupLink + "</td>";

                tr += "</tr>";

                body += tr;

                totalRow++;
                tr = "";
                holdupLink = ""; collectLink = ""; editLink = "";
                approvedLink = ""; cancelLink = "";

                p.Permission.IsEdit = false;
                p.Permission.IsCancel = false;
                p.Permission.IsChecked = false;
                p.Permission.IsApproved = false;
                p.Permission.IsView = false;
            }

            return body;
        }

        [WebMethod]
        public static EmpLoanBO GetLoanCollectionInfo(int loanId)
        {
            EmpLoanDA loanDa = new EmpLoanDA();
            EmpLoanBO loan = new EmpLoanBO();
            LoanCollectionBO loanCollection = new LoanCollectionBO();
            loan = loanDa.GetLoanByLoanId(loanId);
            loanCollection = loanDa.GetLastPaidLoanCollectionByLoanId(loanId);

            loan.LoanAmount = loan.LoanAmount + loan.InterestAmount;
            loan.DueAmount = loan.DueAmount + loan.DueInterestAmount;
            loan.OverDueAmount = loan.OverDueAmount;

            if (loanCollection != null)
                loan.InstallmentNumber = loanCollection.InstallmentNumber + 1;
            else
                loan.InstallmentNumber = 1;

            return loan;
        }

        [WebMethod]
        public static ReturnInfo SaveLoanCollection(LoanCollectionBO loanCollection)
        {
            int loanCollectionId = 0;
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninfo = new ReturnInfo();
            frmLoanSearch frm = new frmLoanSearch();
            EmpLoanDA loanDa = new EmpLoanDA();

            try
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = frm.hmUtility.GetCurrentApplicationUserInfo();

                loanCollection.CreatedBy = userInformationBO.UserInfoId;
                loanCollection.ApprovedStatus = HMConstants.ApprovalStatus.Submit.ToString();

                rtninfo.IsSuccess = loanDa.SaveLoanCollection(loanCollection, out loanCollectionId);

                if (rtninfo.IsSuccess)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.LoanCollection.ToString(), loanCollectionId,
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
        public static LoanNHoldUpViewBO GetLoanInfo(int loanId)
        {
            LoanNHoldUpViewBO vw = new LoanNHoldUpViewBO();
            EmpLoanDA loanDa = new EmpLoanDA();
            vw.Loan = loanDa.GetLoanByLoanId(loanId);

            LoanCollectionBO loanCollection = new LoanCollectionBO();
            loanCollection = loanDa.GetLastPaidLoanCollectionByLoanId(loanId);

            vw.Loan.LoanAmount = vw.Loan.LoanAmount + vw.Loan.InterestAmount;
            vw.Loan.DueAmount = vw.Loan.DueAmount + vw.Loan.DueInterestAmount;
            vw.Loan.OverDueAmount = vw.Loan.OverDueAmount;

            if (loanCollection != null)
                vw.Loan.InstallmentNumber = loanCollection.InstallmentNumber + 1;
            else
                vw.Loan.InstallmentNumber = 1;

            vw.LoanHolpUp = loanDa.GetGetPayrollLoanHoldupByLoanId(loanId);

            return vw;
        }

        [WebMethod]
        public static ReturnInfo SaveLoanHoldUp(LoanHoldupBO loanHoldup)
        {
            int loanHoldupId = 0;
            HMUtility hmUtility = new HMUtility();

            ReturnInfo rtninfo = new ReturnInfo();
            frmLoanSearch frm = new frmLoanSearch();
            EmpLoanDA loanDa = new EmpLoanDA();

            try
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = frm.hmUtility.GetCurrentApplicationUserInfo();

                loanHoldup.CreatedBy = userInformationBO.UserInfoId;

                if (loanHoldup.LoanHoldupId == 0)
                {
                    rtninfo.IsSuccess = loanDa.SaveLoanHoldUp(loanHoldup, out loanHoldupId);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.LoanHoldup.ToString(), loanHoldupId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LoanHoldup));
                }
                else
                {
                    rtninfo.IsSuccess = loanDa.UpdateLoanHoldUp(loanHoldup);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.LoanHoldup.ToString(), loanHoldup.LoanHoldupId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LoanHoldup));
                }

                if (rtninfo.IsSuccess)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
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

            //frmLoanSearch frm = new frmLoanSearch();
            //EmpLoanDA loanDa = new EmpLoanDA();

            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = frm.hmUtility.GetCurrentApplicationUserInfo();

            //loanHoldup.CreatedBy = userInformationBO.UserInfoId;
            //loanHoldup.HoldupStatus = "HoldUp";

            //status = loanDa.SaveLoanHoldUp(loanHoldup, out loanHoldupId);

            //if (status)
            //    status = true;
            //else
            //    status = false;

            //return status;
        }

        [WebMethod]
        public static ReturnInfo LoanApproval(int loanId, int empId, string approvedStatus, int createdBy)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            int approvedRCheckedby = 0;
            HMUtility hmUtility = new HMUtility();

            try
            {
                EmpLoanDA loanDa = new EmpLoanDA();
                frmLoanSearch frm = new frmLoanSearch();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = frm.hmUtility.GetCurrentApplicationUserInfo();

                if (userInformationBO.UserGroupId == 1)
                {
                    approvedStatus = HMConstants.ApprovalStatus.Approved.ToString();
                    approvedRCheckedby = userInformationBO.UserInfoId;
                }
                else if (approvedStatus == HMConstants.ApprovalStatus.Pending.ToString() && userInformationBO.UserInfoId == createdBy)
                {
                    approvedStatus = HMConstants.ApprovalStatus.Checked.ToString();
                    approvedRCheckedby = userInformationBO.UserInfoId;
                }
                else if (approvedStatus == HMConstants.ApprovalStatus.Checked.ToString() && userInformationBO.UserInfoId == createdBy)
                {
                    approvedStatus = HMConstants.ApprovalStatus.Approved.ToString();
                    approvedRCheckedby = userInformationBO.UserInfoId;
                }

                status = loanDa.UpdateLoanNApprovedStatus(loanId, empId, HMConstants.LoanStatus.Regular.ToString(), approvedStatus, userInformationBO.UserInfoId);

                if (status)
                {
                    rtninfo.IsSuccess = true;
                    if (approvedStatus == HMConstants.ApprovalStatus.Pending.ToString())
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
                    }
                    else if (approvedStatus == HMConstants.ApprovalStatus.Checked.ToString())
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                    }
                    else
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Success, AlertType.Success);
                    }
                    
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmpLoan.ToString(), empId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            "Loan Approved for Employee");
                }
            }
            catch
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            if (!status)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;
        }

        [WebMethod]
        public static ReturnInfo LoanCancel(int loanId, int empId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            HMUtility hmUtility = new HMUtility();

            try
            {
                EmpLoanDA loanDa = new EmpLoanDA();
                frmLoanSearch frm = new frmLoanSearch();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = frm.hmUtility.GetCurrentApplicationUserInfo();

                status = loanDa.UpdateLoanNApprovedStatus(loanId, empId, string.Empty, HMConstants.ApprovalStatus.Cancel.ToString(), userInformationBO.UserInfoId);

                if (status)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Cancel, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(),
                            EntityTypeEnum.EntityType.LoanCollection.ToString(), empId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            "Employee Loan Approval is Changed to Cancel");
                }
            }
            catch
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            if (!status)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;
        }

        [WebMethod(EnableSession = true)]
        public static ReturnInfo EditLoan(int loanId)
        {
            ReturnInfo rtninfo = new ReturnInfo();

            try
            {
                HttpContext.Current.Session["EditedLoanId"] = loanId.ToString(); ;
                rtninfo.IsSuccess = true;
                rtninfo.RedirectUrl = "frmEmpLoan.aspx";
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Cancel, AlertType.Success);
            }
            catch
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;
        }

        private void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;
            btnLoanHoldupSave.Visible = isSavePermission;
        }
    }
}