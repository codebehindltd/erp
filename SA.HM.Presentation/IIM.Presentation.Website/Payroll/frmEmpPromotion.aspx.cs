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
    public partial class frmEmpPromotion : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                LoadDesignation();
                LoadGrade();
                LoadDepartment();
                CheckObjectPermission();
            }

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
        private void LoadDesignation()
        {
            DesignationDA entityDA = new DesignationDA();
            this.ddlDesignationId.DataSource = entityDA.GetDesignationInfo();
            this.ddlDesignationId.DataTextField = "Name";
            this.ddlDesignationId.DataValueField = "DesignationId";
            this.ddlDesignationId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlDesignationId.Items.Insert(0, item);
        }
        private void LoadGrade()
        {
            EmpGradeDA gradeDA = new EmpGradeDA();
            var List = gradeDA.GetGradeInfo(); ;
            this.ddlGradeId.DataSource = List;
            this.ddlGradeId.DataTextField = "Name";
            this.ddlGradeId.DataValueField = "GradeId";
            this.ddlGradeId.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";

            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlGradeId.Items.Insert(0, item);
        }
        private void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        [WebMethod]
        public static ReturnInfo SaveEmployeePromotion(int promotionId, int employeeId, int designationId, int gradeId, string promotionDate, string effectiveDate, string remarks)
        {
            ReturnInfo rtninf = new ReturnInfo();

            try
            {
                Int64 tmppromotionId = 0;

                HMUtility hmUtility = new HMUtility();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                EmpPromotionBO promotion = new EmpPromotionBO();
                EmpIncrementDA incrementDA = new EmpIncrementDA();

                EmployeeDA empDa = new EmployeeDA();
                EmployeeBO employee = new EmployeeBO();

                employee = empDa.GetEmployeeInfoById(employeeId);

                promotion.EmpId = employeeId;
                if (!string.IsNullOrEmpty(promotionDate))
                {
                    promotion.PromotionDate = hmUtility.GetDateTimeFromString(promotionDate, userInformationBO.ServerDateFormat);
                }

                if (!string.IsNullOrEmpty(effectiveDate))
                {
                    promotion.EffectiveDate = hmUtility.GetDateTimeFromString(effectiveDate, userInformationBO.ServerDateFormat);
                }

                promotion.PreviousDesignationId = employee.DesignationId;
                promotion.PreviousGradeId = employee.GradeId;
                promotion.CurrentDesignationId = designationId;
                promotion.CurrentGradeId = gradeId;
                promotion.Remarks = remarks;

                promotion.ApprovalStatus = HMConstants.ApprovalStatus.Submit.ToString();

                if (promotionId == 0)
                {
                    promotion.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = incrementDA.SaveEmpPromotionInfo(promotion, out tmppromotionId);

                    if (status)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.EmpPromotion.ToString(), tmppromotionId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpPromotion));
                    }
                    else
                    {
                        rtninf.IsSuccess = false;
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Service Unavailable Please Try Again Later.", AlertType.Error);
                    }
                }
                else
                {
                    promotion.PromotionId = Convert.ToInt64(promotionId);
                    promotion.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = incrementDA.UpdateEmpPromotionInfo(promotion);

                    if (status)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.EmpPromotion.ToString(), promotion.PromotionId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpPromotion));
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
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;

        }
        [WebMethod]
        public static ReturnInfo DeleteData(int sEmpId)
        {
            HMUtility hmUtility = new HMUtility();
            string message = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("PayrollEmpIncrement", "Id", sEmpId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.EmpPromotion.ToString(), sEmpId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpPromotion));
                }
            }
            catch
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Error);
            }
            return rtninf;
        }
        protected void gvEmpPromotion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int promotionId = 0, empId = 0;
            promotionId = Convert.ToInt32(e.CommandArgument.ToString());            

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int approvedBy = userInformationBO.UserInfoId;
            EmpIncrementDA promotionDa = new EmpIncrementDA();
            EmpPromotionBO promotion = new EmpPromotionBO();

            Boolean status = false;           

            if (e.CommandName == "CmdPromotionApproved")
            {
                GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                Label lblEmployee = (Label)row.FindControl("lblEmpId");
                empId = Convert.ToInt32(lblEmployee.Text);

                promotion.PromotionId = promotionId;
                promotion.EmpId = empId;

                promotion.ApprovalStatus = HMConstants.ApprovalStatus.Approved.ToString();
                status = promotionDa.UpdateEmpPromotionApproval(promotion);

                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.EmpPromotion.ToString(), promotionId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpPromotion));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Approved, AlertType.Success);
                    status = promotionDa.UpdateEmpPromotionApproval(promotion);
                    LoadGridData();
                }

                if (!status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
            }
            else if (e.CommandName == "CmdPromotionCancel")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                status = hmCommonDA.DeleteInfoById("PayrollEmpPromotion", "PromotionId", promotionId);

                if (status)
                {                    
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.EmpPromotion.ToString(), promotionId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpPromotion)+"Canceled");
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
                FillForm(promotionId);
            }
            else if (e.CommandName == "CmdPromotionLater")
            {
                var type = "Promotion";
                string url = "/Payroll/Reports/frmPayrollReport.aspx?TId=" + promotionId + "," + type;
                string sPopUp = "window.open('" + url + "', 'popup_window', 'width=715,height=780,left=300,top=50,resizable=yes, scrollbars=1');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
            }
        }
        protected void gvEmpPromotion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                var item = (EmpPromotionBO)e.Row.DataItem;

                ImageButton imgApproved = (ImageButton)e.Row.FindControl("ImgDetailsApproved");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgCancel = (ImageButton)e.Row.FindControl("ImgBtnCancelPO");

                if (item.ApprovalStatus == "Approved")
                {
                    imgApproved.Visible = false;
                    imgCancel.Visible = false;
                    imgUpdate.Visible = false;
                    
                }
                else if (item.ApprovalStatus == "Cancel")
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
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridData();
            SetTab("SearchTab");
        }
        private void LoadGridData()
        {
            int departmentId = 0, employeeId = 0;
            string empId = string.Empty;

            if (ddlSearchDepartment.SelectedValue != "0")
            {
                departmentId = Convert.ToInt32(ddlSearchDepartment.SelectedValue);
            }

            ContentPlaceHolder mpContentPlaceHolder;
            UserControl empSearch;
            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            empSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeSearchall");

            empId = ((HiddenField)empSearch.FindControl("hfEmployeeId")).Value;

            DateTime searchFromDate = hmUtility.GetDateTimeFromString(this.txtPromotionDateFrom.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime searchToDate = hmUtility.GetDateTimeFromString(this.txtPromotionDateTo.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            if (empId != "0")
            {
                employeeId = Convert.ToInt32(empId);
            }

            EmpIncrementDA promotionDa = new EmpIncrementDA();
            List<EmpPromotionBO> entityBO = new List<EmpPromotionBO>();

            entityBO = promotionDa.GetEmployeePromotion(employeeId, departmentId, searchFromDate, searchToDate);

            gvEmpPromotion.DataSource = entityBO;
            gvEmpPromotion.DataBind();
        }
        private void FillForm(int promotionId)
        {
            EmpPromotionBO promotion = new EmpPromotionBO();
            EmpIncrementDA incrementDA = new EmpIncrementDA();
            promotion = incrementDA.GetEmployeePromotion(promotionId);

            hfPromotionId.Value = promotion.PromotionId.ToString();
            ddlDesignationId.SelectedValue = promotion.CurrentDesignationId.ToString();
            ddlGradeId.SelectedValue = promotion.CurrentGradeId.ToString();
            txtPromotionDate.Text = hmUtility.GetStringFromDateTime(promotion.PromotionDate);
            txtEffectiveDate.Text = hmUtility.GetStringFromDateTime(promotion.EffectiveDate);

            ContentPlaceHolder mpContentPlaceHolder;
            UserControl empSearch;
            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            empSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeSearch");

            ((HiddenField)empSearch.FindControl("hfEmployeeId")).Value = promotion.EmpId.ToString();
            ((TextBox)empSearch.FindControl("txtSearchEmployee")).Text = promotion.EmpCode;
            ((TextBox)empSearch.FindControl("txtEmployeeName")).Text = promotion.EmployeeName;
            ((TextBox)empSearch.FindControl("txtDepartment")).Text = promotion.DepartmentName;
            ((TextBox)empSearch.FindControl("txtDesignation")).Text = promotion.PreviousDesignation;
            ((TextBox)empSearch.FindControl("txtGrade")).Text = promotion.CurrentGrade;

            txtRemarks.Text = promotion.Remarks;

            btnSave.Text = "Update";
            SetTab("EntryTab");
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
        protected void gvEmpPromotion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadGridData();
            SetTab("SearchTab");
        }        
    }
}