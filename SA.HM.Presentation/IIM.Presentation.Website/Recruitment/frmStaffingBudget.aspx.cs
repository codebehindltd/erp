using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Newtonsoft.Json;

using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.Recruitment
{
    public partial class frmStaffingBudget : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                CheckObjectPermission();
                LoadUserInformation();
                LoadDepartment();
                LoadEmployeeType();
                LoadFiscalYear();
            }
        }

        //************************ Handlers ********************//
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridview();
        }

        //protected void gvJobCircular_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.DataItem != null)
        //    {
        //        var item = (PayrollJobCircularBO)e.Row.DataItem;

        //        ImageButton imgApproved = (ImageButton)e.Row.FindControl("ImgApproved");
        //        ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
        //        ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");

        //        if (item.ApprovedStatus != HMConstants.ApprovalStatus.Approved.ToString() && item.ApprovedStatus != HMConstants.ApprovalStatus.Cancel.ToString())
        //        {
        //            imgApproved.Visible = true;
        //            imgUpdate.Visible = true;
        //            imgDelete.Visible = true;
        //        }
        //        else
        //        {
        //            imgApproved.Visible = false;
        //            imgUpdate.Visible = false;
        //            imgDelete.Visible = false;
        //        }
        //    }
        //}

        //protected void gvJobCircular_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    try
        //    {
        //        HMUtility hmUtility = new HMUtility();
        //        UserInformationBO userInformationBO = new UserInformationBO();
        //        userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

        //        JobCircularNRecruitmentDA jobCircularDa = new JobCircularNRecruitmentDA();
        //        PayrollJobCircularBO jobCircular = new PayrollJobCircularBO();

        //        if (e.CommandName == "CmdEdit")
        //        {
        //            Int64 jobCircularId = Convert.ToInt64(e.CommandArgument.ToString());
        //            FillForm(jobCircularId);
        //        }
        //        else if (e.CommandName == "CmdApprovedJobCircular")
        //        {
        //            Int64 jobCircularId = Convert.ToInt32(e.CommandArgument.ToString());

        //            jobCircular.JobCircularId = jobCircularId;
        //            jobCircular.LastModifiedBy = userInformationBO.UserInfoId;
        //            jobCircular.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();

        //            bool status = jobCircularDa.UpdateJobCircularApprovedStatus(jobCircular);

        //            if (status)
        //            {
        //                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Approved, AlertType.Success);
        //                LoadGridview();
        //            }
        //            else
        //            {
        //                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
        //            }
        //        }
        //        else if (e.CommandName == "CmdJobCircularCancel")
        //        {
        //            Int64 jobCircularId = Convert.ToInt32(e.CommandArgument.ToString());

        //            jobCircular.JobCircularId = jobCircularId;
        //            jobCircular.LastModifiedBy = userInformationBO.UserInfoId;
        //            jobCircular.ApprovedStatus = HMConstants.ApprovalStatus.Cancel.ToString();

        //            bool status = jobCircularDa.UpdateJobCircularApprovedStatus(jobCircular);

        //            if (status)
        //            {
        //                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Approved, AlertType.Success);
        //                LoadGridview();
        //            }
        //            else
        //            {
        //                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
        //    }
        //}

        //**************************** User Defined Method ****************************//        
        private void LoadGridview()
        {
            List<PayrollStaffingBudgetByDepartmentIdBO> staffBudgetList = new List<PayrollStaffingBudgetByDepartmentIdBO>();
            StaffingBudgetDA staffBudgetDA = new StaffingBudgetDA();

            int? departmentId = null;
            int? fiscalYear = null;
            if (ddlSearchDepartment.SelectedIndex != 0)
            {
                departmentId = Convert.ToInt32(ddlSearchDepartment.SelectedValue);
            }
            if (ddlFiscalYear.SelectedIndex != 0)
            {
                fiscalYear = Convert.ToInt32(ddlFiscalYear.SelectedValue);
            }

            staffBudgetList = staffBudgetDA.GetStaffingBudgetByDepartmentId(departmentId, fiscalYear);

            gvStaffBudget.DataSource = staffBudgetList;
            gvStaffBudget.DataBind();

            SetTab("circularSearch");
        }
        //private void FillForm(Int64 jobCircularId)
        //{
        //    UserInformationBO userInformationBO = new UserInformationBO();
        //    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

        //    PayrollJobCircularBO jobCircular = new PayrollJobCircularBO();
        //    JobCircularNRecruitmentDA jobCircularDa = new JobCircularNRecruitmentDA();
        //    jobCircular = jobCircularDa.GetJobCircularById(jobCircularId);

        //    //hfJobCircularId.Value = jobCircular.JobCircularId.ToString();

        //    //txtJobTitle.Text = jobCircular.JobTitle;
        //    //ddlJobType.SelectedValue = Convert.ToString(jobCircular.JobType);
        //    //ddlJobLevel.SelectedValue = jobCircular.JobLevel;
        //    //ddlDepartments.SelectedValue = Convert.ToString(jobCircular.DepartmentId);
        //    //txtNoOfVancancie.Text = Convert.ToString(jobCircular.NoOfVancancie);
        //    //txtDemandedTime.Text = Convert.ToString(jobCircular.DemandedTime);
        //    //ddlAgeRangeFrom.SelectedValue = Convert.ToString(jobCircular.AgeRangeFrom);
        //    //ddlAgeRangeTo.SelectedValue = Convert.ToString(jobCircular.AgeRangeTo);
        //    //ddlGender.SelectedValue = jobCircular.Gender;
        //    //ddlYearOfExperiance.SelectedValue = Convert.ToString(jobCircular.YearOfExperiance);
        //    //txtJobDescription.Text = jobCircular.JobDescription;
        //    //txtEducationalQualification.Text = jobCircular.EducationalQualification;
        //    //txtAdditionalJobRequirement.Text = jobCircular.AdditionalJobRequirement;
        //    //jobCircular.LastModifiedBy = userInformationBO.UserInfoId;

        //    SetTab("circularEntry");
        //}
        private void LoadUserInformation()
        {
            //UserInformationDA entityDA = new UserInformationDA();

            //this.ddlCheckedBy.DataSource = entityDA.GetUserInformation();
            //this.ddlCheckedBy.DataTextField = "UserName";
            //this.ddlCheckedBy.DataValueField = "UserInfoId";
            //this.ddlCheckedBy.DataBind();

            //ListItem itemEmployee = new ListItem();
            //itemEmployee.Value = string.Empty;
            //itemEmployee.Text = hmUtility.GetDropDownFirstValue();
            //this.ddlCheckedBy.Items.Insert(0, itemEmployee);


            //this.ddlApprovedBy.DataSource = entityDA.GetUserInformation();
            //this.ddlApprovedBy.DataTextField = "UserName";
            //this.ddlApprovedBy.DataValueField = "UserInfoId";
            //this.ddlApprovedBy.DataBind();

            //this.ddlApprovedBy.Items.Insert(0, itemEmployee);
        }
        private void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;
        }
        private void SetTab(string TabName)
        {
            if (TabName == "circularEntry")
            {
                circularEntry.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                circularSearch.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "circularSearch")
            {
                circularSearch.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                circularEntry.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        private void ClearForm()
        {
            // hfJobCircularId.Value = string.Empty;

            //txtJobTitle.Text = string.Empty;
            //ddlJobType.SelectedValue = "0";
            //ddlJobLevel.SelectedValue = "0";
            //ddlDepartments.SelectedValue = "0";
            //txtNoOfVancancie.Text = string.Empty;
            //txtDemandedTime.Text = string.Empty;
            //ddlAgeRangeFrom.SelectedValue = "0";
            //ddlAgeRangeTo.SelectedValue = "0";
            //ddlGender.SelectedValue = "0";
            //ddlYearOfExperiance.SelectedValue = "0";
            //txtJobDescription.Text = string.Empty;
            //txtEducationalQualification.Text = string.Empty;
            //txtAdditionalJobRequirement.Text = string.Empty;

            this.btnSave.Text = "Save";
        }
        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            this.ddlDepartments.DataSource = entityDA.GetDepartmentInfo();
            this.ddlDepartments.DataTextField = "Name";
            this.ddlDepartments.DataValueField = "DepartmentId";
            this.ddlDepartments.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlDepartments.Items.Insert(0, item);

            this.ddlSearchDepartment.DataSource = entityDA.GetDepartmentInfo();
            this.ddlSearchDepartment.DataTextField = "Name";
            this.ddlSearchDepartment.DataValueField = "DepartmentId";
            this.ddlSearchDepartment.DataBind();

            this.ddlSearchDepartment.Items.Insert(0, item);
        }
        private void LoadEmployeeType()
        {
            EmpTypeDA entityDA = new EmpTypeDA();
            this.ddlJobType.DataSource = entityDA.GetActiveGradeInfo();
            this.ddlJobType.DataTextField = "Name";
            this.ddlJobType.DataValueField = "TypeId";
            this.ddlJobType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlJobType.Items.Insert(0, item);
        }
        private void LoadFiscalYear()
        {
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            GLFiscalYearDA entityDA = new GLFiscalYearDA();
            List<GLFiscalYearBO> allFiscalYear = entityDA.GetAllFiscalYear();
            List<GLFiscalYearBO> dataset = new List<GLFiscalYearBO>();
            foreach (var itm in allFiscalYear)
            {
                if (Convert.ToInt32(itm.ToDate.Year) >= Convert.ToInt32(DateTime.Now.Year))
                {
                    dataset.Add(itm);
                }
            }

            this.ddlFiscalYearSetup.DataSource = dataset;
            this.ddlFiscalYearSetup.DataTextField = "FiscalYearName";
            this.ddlFiscalYearSetup.DataValueField = "FiscalYearId";
            this.ddlFiscalYearSetup.DataBind();
            this.ddlFiscalYearSetup.Items.Insert(0, item);

            this.ddlFiscalYear.DataSource = dataset;
            this.ddlFiscalYear.DataTextField = "FiscalYearName";
            this.ddlFiscalYear.DataValueField = "FiscalYearId";
            this.ddlFiscalYear.DataBind();
            this.ddlFiscalYear.Items.Insert(0, item);
        }

        //************************ User Defined WebMethod ********************//

        [WebMethod]
        public static ReturnInfo SaveStaffingBudget(int staffingBudgetId, PayrollStaffingBudgetBO StaffingBudget, List<PayrollStaffingBudgeDetailsBO> NewlyAddedStaffingBudgetDetails, List<PayrollStaffingBudgeDetailsBO> EditedStaffingBudgetDetails, List<PayrollStaffingBudgeDetailsBO> deleteDbItem)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                StaffingBudgetDA staffingBudgetDa = new StaffingBudgetDA();

                StaffingBudget.ApprovedStatus = "Submitted";

                if (staffingBudgetId == 0)
                {
                    long tmpStaffingBudgetId = 0;
                    StaffingBudget.CreatedBy = userInformationBO.UserInfoId;
                    status = staffingBudgetDa.SaveStaffingBudget(StaffingBudget, NewlyAddedStaffingBudgetDetails, out tmpStaffingBudgetId);

                    if (status)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.StaffBudget.ToString(), tmpStaffingBudgetId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.StaffBudget));
                    }
                }
                else
                {
                    //requsition.LastModifiedBy = userInformationBO.UserInfoId;
                    status = staffingBudgetDa.UpdateStaffingBudget(StaffingBudget, NewlyAddedStaffingBudgetDetails, EditedStaffingBudgetDetails, deleteDbItem);

                    if (status)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }

                if (!status)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                
            }

            return rtninf;
        }        
        [WebMethod]
        public static PayrollJobCircularBO GetJobCircularDetails(int jobCircularId)
        {
            PayrollJobCircularBO jobCircular = new PayrollJobCircularBO();
            JobCircularNRecruitmentDA jobCircularDa = new JobCircularNRecruitmentDA();
            jobCircular = jobCircularDa.GetJobCircularById(jobCircularId);

            return jobCircular;
        }

        [WebMethod]
        public static PayrollStaffingBudgetViewBO FillForm(long StaffingBudgetId)
        {
            StaffingBudgetDA StaffingBudgetDA = new StaffingBudgetDA();
            PayrollStaffingBudgetViewBO viewBo = new PayrollStaffingBudgetViewBO();
            viewBo.StaffingBudget = StaffingBudgetDA.GetPayrollStaffingBudgetById(StaffingBudgetId);
            viewBo.StaffingBudgetDetails = StaffingBudgetDA.payrollStaffingBudgeDetailsById(StaffingBudgetId);

            return viewBo;
        }

        [WebMethod]
        public static ReturnInfo CancelStaffingBudget (long StaffingBudgetId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;
            StaffingBudgetDA StaffingBudgetDA = new StaffingBudgetDA();
            status = StaffingBudgetDA.DeleteStaffingBudget(StaffingBudgetId);
            if(status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
            }
            else
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Warning);
            }
            return rtninf;
        }

    }
}