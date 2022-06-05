using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.UserInformation;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpAdvanceTaken : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                //LoadUserInformation();
            }
            CheckObjectPermission();
        }

        protected void CheckObjectPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }

        protected void btnEmpAdvanceTakenSave_Click(object sender, EventArgs e)
        {
            if (!IsAdvanceTakenFrmValid())
            {
                return;
            }

            ContentPlaceHolder mpContentPlaceHolder;
            UserControl loanAllocation;
            HiddenField loanEmployeeId;

            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            loanAllocation = (UserControl)mpContentPlaceHolder.FindControl("employeeSearch");
            loanEmployeeId = (HiddenField)loanAllocation.FindControl("hfEmployeeId");

            EmpAdvanceTakenBO advTknBO = new EmpAdvanceTakenBO();
            EmpAdvanceTakenDA advTknDA = new EmpAdvanceTakenDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            advTknBO.EmpId = Convert.ToInt32(loanEmployeeId.Value);
            advTknBO.AdvanceDate = hmUtility.GetDateTimeFromString(this.txtAdvanceDate.Text.Trim(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            advTknBO.AdvanceAmount = Convert.ToDecimal(this.txtAdvanceAmount.Text.Trim());
            advTknBO.PayMonth = this.ddlPayMonth.Text;
            advTknBO.IsDeductFromSalary = this.chkIsDeductFromSalary.Checked ? true : false;
            advTknBO.Remarks = this.txtRemarks.Text;

            if (string.IsNullOrWhiteSpace(hfAdvanceTakenId.Value))
            {
                advTknBO.CreatedBy = userInformationBO.UserInfoId;
                advTknBO.CreatedDate = DateTime.Now;
                int advantakenId = 0;
                Boolean status = advTknDA.SaveEmpAdvanceTakenInfo(advTknBO, out advantakenId);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpAdvanceTaken.ToString(), advantakenId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpAdvanceTaken));
                    ClearForm();
                }
            }
            else
            {
                advTknBO.AdvanceId = Convert.ToInt32(hfAdvanceTakenId.Value);
                advTknBO.LastModifiedBy = userInformationBO.UserInfoId;
                advTknBO.LastModifiedDate = DateTime.Now;

                Boolean status = advTknDA.UpdateEmpAdvanceTakenInfo(advTknBO);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmpAdvanceTaken.ToString(), advTknBO.AdvanceId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpAdvanceTaken));
                    ClearForm();
                }
            }
        }

        protected void btnEmpAdvanceTakenClear_Click(object sender, EventArgs e)
        {
            this.txtAdvanceDate.Text = string.Empty;
            this.txtAdvanceAmount.Text = string.Empty;
            this.ddlPayMonth.SelectedIndex = 0;
            this.chkIsDeductFromSalary.Checked = false;
            this.txtRemarks.Text = string.Empty;

            ContentPlaceHolder mpContentPlaceHolder;
            UserControl advanceTaken, employeeForLoanSearch;

            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            advanceTaken = (UserControl)mpContentPlaceHolder.FindControl("employeeSearch");
            employeeForLoanSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeForLoanSearch");

            ((TextBox)advanceTaken.FindControl("txtSearchEmployee")).Text = string.Empty;
            ((TextBox)advanceTaken.FindControl("txtEmployeeName")).Text = string.Empty;
            ((TextBox)employeeForLoanSearch.FindControl("txtEmployeeName")).Text = string.Empty;
            ((HiddenField)advanceTaken.FindControl("hfEmployeeId")).Value = "0";
            ((HiddenField)employeeForLoanSearch.FindControl("hfEmployeeId")).Value = "0";
        }

        private bool IsAdvanceTakenFrmValid()
        {
            bool flag = true;

            if (txtAdvanceDate.Text == "")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Advance Date.", AlertType.Warning);
                flag = false;
                txtAdvanceDate.Focus();
            }
            else if (txtAdvanceAmount.Text == "")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Advance Amount.", AlertType.Warning);
                flag = false;
                txtAdvanceAmount.Focus();
            }
            else if (chkIsDeductFromSalary.Checked == false)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Deduct From Salary.", AlertType.Warning);
                flag = false;
                chkIsDeductFromSalary.Focus();
            }
            else if (ddlPayMonth.Text == "")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Pay Month.", AlertType.Warning);
                flag = false;
                ddlPayMonth.Focus();
            }
            return flag;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static EmpAdvanceTakenBO FillForm(int editId)
        {
            EmpAdvanceTakenBO advTknBO = new EmpAdvanceTakenBO();
            EmpAdvanceTakenDA advTknDA = new EmpAdvanceTakenDA();
            advTknBO = advTknDA.GetAdvanceTakenInfoById(editId);

            return advTknBO;
        }

        [WebMethod]
        public static GridViewDataNPaging<EmpAdvanceTakenBO, GridPaging> SearchAdvTknAndLoadGridInformation(string empId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            GridViewDataNPaging<EmpAdvanceTakenBO, GridPaging> myGridData = new GridViewDataNPaging<EmpAdvanceTakenBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            if (!string.IsNullOrWhiteSpace(empId))
            {                
                int totalRecords = 0;
                int? employeeId = null;
                if (!string.IsNullOrEmpty(empId))
                    employeeId = Convert.ToInt32(empId);

                pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

                HMCommonDA commonDA = new HMCommonDA();
                EmpAdvanceTakenDA advTknDA = new EmpAdvanceTakenDA();

                List<EmpAdvanceTakenBO> advtknList = new List<EmpAdvanceTakenBO>();
                advtknList = advTknDA.GetAdvanceTakenInfoBySearchCriteriaForPagination(employeeId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

                List<EmpAdvanceTakenBO> distinctItems = new List<EmpAdvanceTakenBO>();
                distinctItems = advtknList.GroupBy(test => test.AdvanceId).Select(group => group.First()).ToList();
                myGridData.GridPagingProcessing(distinctItems, totalRecords);
            }
            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo DeleteAdvanceTakenById(int sEmpId)
        {
            string result = string.Empty;
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("PayrollEmpAdvanceTaken", "AdvanceId", sEmpId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.EmpAdvanceTaken.ToString(), sEmpId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpAdvanceTaken));
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninf;
        }

        [WebMethod]
        public static ReturnInfo AdvanceTakenCancel(int advanceId, int empId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;

            try
            {
                EmpAdvanceTakenDA advanceTakenDA = new EmpAdvanceTakenDA();
                frmEmpAdvanceTaken frm = new frmEmpAdvanceTaken();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = frm.hmUtility.GetCurrentApplicationUserInfo();

                status = advanceTakenDA.UpdateAdvanceTakenNApprovedStatus(advanceId, HMConstants.ApprovalStatus.Cancel.ToString(), string.Empty, userInformationBO.UserInfoId);

                if (status)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Cancel, AlertType.Success);
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
        public static ReturnInfo AdvanceTakenApproval(int advanceId, int empId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            HMUtility hmUtility = new HMUtility();

            try
            {
                EmpAdvanceTakenDA advanceTakenDA = new EmpAdvanceTakenDA();
                frmEmpAdvanceTaken frm = new frmEmpAdvanceTaken();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = frm.hmUtility.GetCurrentApplicationUserInfo();

                status = advanceTakenDA.UpdateAdvanceTakenNApprovedStatus(advanceId, string.Empty, HMConstants.ApprovalStatus.Approved.ToString(), userInformationBO.UserInfoId);

                if (status)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(),
                            EntityTypeEnum.EntityType.EmpAdvanceTaken.ToString(), advanceId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpAdvanceTaken));
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

        private void ClearForm()
        {
            ddlPayMonth.SelectedIndex = 0;
            txtAdvanceAmount.Text = string.Empty;
            txtAdvanceDate.Text = string.Empty;
            txtRemarks.Text = string.Empty;

            hfAdvanceTakenId.Value = string.Empty;
            btnEmpAdvanceTakenSave.Text = "Save";
            chkIsDeductFromSalary.Checked = true;

            ContentPlaceHolder mpContentPlaceHolder;
            UserControl empAdvance, employeeForAdvanceSearch;

            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            empAdvance = (UserControl)mpContentPlaceHolder.FindControl("employeeSearch");
            employeeForAdvanceSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeForLoanSearch");

            ((TextBox)empAdvance.FindControl("txtSearchEmployee")).Text = string.Empty;
            ((TextBox)empAdvance.FindControl("txtEmployeeName")).Text = string.Empty;
            ((TextBox)employeeForLoanSearch.FindControl("txtEmployeeName")).Text = string.Empty;
            ((TextBox)employeeForLoanSearch.FindControl("txtSearchEmployee")).Text = string.Empty;
            ((DropDownList)employeeForLoanSearch.FindControl("ddlEmployee")).SelectedValue = "0";

            ((HiddenField)empAdvance.FindControl("hfEmployeeId")).Value = "0";
            ((HiddenField)employeeForLoanSearch.FindControl("hfEmployeeId")).Value = "0";
        }
    }
}