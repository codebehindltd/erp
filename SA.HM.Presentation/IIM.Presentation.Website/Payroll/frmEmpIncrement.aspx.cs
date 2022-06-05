using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpIncrement : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadDepartment();
            }
            CheckObjectPermission();
        }
        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            this.ddlSearchDepartment.DataSource = entityDA.GetDepartmentInfo();
            this.ddlSearchDepartment.DataTextField = "Name";
            this.ddlSearchDepartment.DataValueField = "DepartmentId";
            this.ddlSearchDepartment.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlSearchDepartment.Items.Insert(0, item);
        }
        private void CheckObjectPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        [WebMethod]
        public static ReturnInfo PerformIncrementSaveAction(int employeeId, string incrementId, string incrementMode, decimal amount, string remarks, string effectiveDate)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                string message = "";
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                EmpIncrementBO incrementBO = new EmpIncrementBO();
                EmpIncrementDA incrementDA = new EmpIncrementDA();

                incrementBO.Remarks = remarks;
                incrementBO.IncrementMode = incrementMode;
                incrementBO.Amount = amount;
                incrementBO.EmpId = employeeId;
                if (!string.IsNullOrEmpty(effectiveDate))
                {
                    incrementBO.EffectiveDate = hmUtility.GetDateTimeFromString(effectiveDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                }

                if (string.IsNullOrWhiteSpace(incrementId))
                {
                    int tmpUserInfoId = 0;
                    incrementBO.ApprovedStatus = HMConstants.ApprovalStatus.Submit.ToString();
                    incrementBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = incrementDA.SaveEmpIncrementInfo(incrementBO, out tmpUserInfoId);

                    if (status)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpIncrement.ToString(), tmpUserInfoId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpIncrement));
                    }
                    else
                    {
                        rtninf.IsSuccess = false;
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Service Unavailable Please Try Again Later.", AlertType.Error);
                    }
                }
                else
                {
                    incrementBO.Id = Convert.ToInt32(incrementId);
                    incrementBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = incrementDA.UpdateEmpIncrementInfo(incrementBO);
                    if (status)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.EmpIncrement.ToString(), incrementBO.Id,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpIncrement));
                    }
                    else
                    {
                        rtninf.IsSuccess = false;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error); ;
            }

            return rtninf;
        }
        [WebMethod]
        public static GridViewDataNPaging<EmpIncrementBO, GridPaging> LoadEmployeeIncrement(int employeeId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<EmpIncrementBO, GridPaging> myGridData = new GridViewDataNPaging<EmpIncrementBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            EmpIncrementDA entityDA = new EmpIncrementDA();
            List<EmpIncrementBO> incrementList = entityDA.GetAllIncrementByEmployeeIdForGridPaging(employeeId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(incrementList, totalRecords);

            return myGridData;
        }
        public void FillForm(int EditId)
        {
            EmpIncrementBO increment = new EmpIncrementBO();
            EmpIncrementDA incrementDA = new EmpIncrementDA();
            increment = incrementDA.GetEmpIncrementInfoById(EditId);

            /*hfIncrementId.Value = increment.Id.ToString();

            ddlIncrementMode.SelectedValue = increment.IncrementMode;
            txtPromotionDate.Text = hmUtility.GetStringFromDateTime(promotion.PromotionDate);*/

            ContentPlaceHolder mpContentPlaceHolder;
            UserControl empSearch;
            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            empSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeSearch");

            ((HiddenField)empSearch.FindControl("hfEmployeeId")).Value = increment.Id.ToString();
            ((TextBox)empSearch.FindControl("txtSearchEmployee")).Text = increment.EmpCode.ToString();
            ((TextBox)empSearch.FindControl("txtEmployeeName")).Text = increment.EmployeeName;
            this.txtAmount.Text = increment.Amount.ToString();
            this.ddlIncrementMode.SelectedValue = increment.IncrementMode;
            this.txtEffectiveDate.Text = hmUtility.GetStringFromDateTime(increment.EffectiveDate);
            this.txtRemarks.Text = increment.Remarks;
            this.hfIncrementId.Value = increment.Id.ToString();

            /*((TextBox)empSearch.FindControl("txtAmount")).Text = increment.IncrementAmount.ToString();
            ((DropDownList)empSearch.FindControl("ddlIncrementMode")).SelectedValue = increment.IncrementMode;
            ((TextBox)empSearch.FindControl("txtEffectiveDate")).Text = increment.IncrementDate.ToString();
            ((TextBox)empSearch.FindControl("txtRemarks")).Text = increment.Remarks;*/

            //txtRemarks.Text = promotion.Remarks;

            btnSave.Text = "Update";
            SetTab("EntryTab");

        }
        [WebMethod]
        public static ReturnInfo DeleteData(int sEmpId)
        {
            string message = string.Empty;
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("PayrollEmpIncrement", "Id", sEmpId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.EmpIncrement.ToString(), sEmpId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpIncrement));
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
            }
            return rtninf;
        }
        [WebMethod]
        public static ReturnInfo ApproveIncrement(int incrementId, int empId)
        {
            string message = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMUtility hmUtility = new HMUtility();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                EmpIncrementBO increment = new EmpIncrementBO();
                EmpIncrementDA incrementDA = new EmpIncrementDA();

                increment.Id = incrementId;
                increment.EmpId = empId;
                increment.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();

                Boolean status = incrementDA.ApprovedIncrement(increment);

                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(),
                            EntityTypeEnum.EntityType.EmpIncrement.ToString(), increment.Id,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpIncrement));
                }
                else
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Success);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Success);
            }

            return rtninf;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridData();
            SetTab("SearchTab");
        }
        private void LoadGridData()
        {
            int departmentId = 0, employeeId = 0;
            string empId = string.Empty;

            if (ddlSrcType.SelectedValue != "0")
            {
                departmentId = Convert.ToInt32(ddlSearchDepartment.SelectedValue);
            }

            ContentPlaceHolder mpContentPlaceHolder;
            UserControl empSearch;
            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            empSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeSearchall");

            empId = ((HiddenField)empSearch.FindControl("hfEmployeeId")).Value;

            DateTime searchFromDate = hmUtility.GetDateTimeFromString(this.txtIncrementDateFrom.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime searchToDate = hmUtility.GetDateTimeFromString(this.txtIncrementDateTo.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);



            if (empId != "0")
            {
                employeeId = Convert.ToInt32(empId);
            }

            EmpIncrementDA promotionDa = new EmpIncrementDA();
            //List<EmpIncrementBO> entityBO = new List<EmpIncrementBO>();

            List<EmpIncrementBO> entityBO = promotionDa.GetEmployeeIncrement(employeeId, departmentId, searchFromDate, searchToDate);

            gvEmpIncrement.DataSource = entityBO;
            gvEmpIncrement.DataBind();
        }
        private void SetTab(string TabName)
        {
            if (TabName == "SearchTab")
            {
                SearchTab.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                EntryTab.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "EntryTab")
            {
                EntryTab.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                SearchTab.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        protected void gvEmpIncrement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                var item = (EmpIncrementBO)e.Row.DataItem;

                ImageButton imgApproved = (ImageButton)e.Row.FindControl("ImgDetailsApproved");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgCancel = (ImageButton)e.Row.FindControl("ImgBtnCancelPO");

                if (item.ApprovedStatus == "Approved")
                {
                    imgApproved.Visible = false;
                    imgCancel.Visible = false;
                    imgUpdate.Visible = false;

                }
                else if (item.ApprovedStatus == "Cancel")
                {
                    imgApproved.Visible = false;
                    imgCancel.Visible = false;
                    imgUpdate.Visible = false;
                }
                else
                {
                    imgApproved.Visible = isSavePermission;
                    imgCancel.Visible = isDeletePermission;
                    imgUpdate.Visible = isUpdatePermission;
                }
            }
        }
        protected void gvEmpIncrement_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int incrementId = 0, empId = 0;
            incrementId = Convert.ToInt32(e.CommandArgument.ToString());            

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int approvedBy = userInformationBO.UserInfoId;
            Boolean status = false;

            if (e.CommandName == "CmdIncrementApproved")
            {
                GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                Label lblEmployee = (Label)row.FindControl("lblEmpId");
                empId = Convert.ToInt32(lblEmployee.Text);

                EmpIncrementBO increment = new EmpIncrementBO();
                EmpIncrementDA incrementDA = new EmpIncrementDA();

                increment.Id = incrementId;
                increment.EmpId = empId;
                increment.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();

                status = incrementDA.ApprovedIncrement(increment);

                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.EmpIncrement.ToString(), incrementId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpIncrement));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Approved, AlertType.Success);
                    LoadGridData();
                }

                if (!status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
            }
            else if (e.CommandName == "CmdIncrementCancel")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                status = hmCommonDA.DeleteInfoById("PayrollEmpIncrement", "Id", incrementId);

                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.EmpIncrement.ToString(), incrementId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpIncrement) + "Canceled");
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Cancel, AlertType.Success);
                    LoadGridData();
                }

                if (!status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
            }
            else if (e.CommandName == "CmdEdit")
            {
                btnSave.Visible = isUpdatePermission;
                FillForm(incrementId);
            }
            else if (e.CommandName == "CmdIncrementLater")
            {
                var type = "Increment";
                string url = "/Payroll/Reports/frmPayrollReport.aspx?TId=" + incrementId + "," + type;
                string sPopUp = "window.open('" + url + "', 'popup_window', 'width=715,height=780,left=300,top=50,resizable=yes, scrollbars=1');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
            }
        }
        protected void gvEmpIncrement_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadGridData();
            SetTab("SearchTab");
        }
    }
}