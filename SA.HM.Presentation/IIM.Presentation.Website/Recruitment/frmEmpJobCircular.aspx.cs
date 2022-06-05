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

namespace HotelManagement.Presentation.Website.Recruitment
{
    public partial class frmEmpJobCircular : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                CheckObjectPermission();
                LoadStaffRequisition();
                LoadUserInformation();
                LoadDepartment();
                LoadEmployeeType();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsFormValid())
                {
                    return;
                }

                bool status = false;

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                JobCircularNRecruitmentDA jobCircularDa = new JobCircularNRecruitmentDA();
                PayrollJobCircularBO jobCircular = new PayrollJobCircularBO();

                jobCircular.StaffRequisitionDetailsId = Convert.ToInt32(ddlStaffRequisition.SelectedValue);
                jobCircular.JobTitle = txtJobTitle.Text;
                jobCircular.CircularDate = DateTime.Now;
                jobCircular.JobType = Convert.ToInt32(ddlJobType.SelectedValue);
                jobCircular.JobLevel = ddlJobLevel.SelectedValue;
                jobCircular.DepartmentId = Convert.ToInt32(ddlDepartments.SelectedValue);
                jobCircular.NoOfVancancie = Convert.ToInt16(txtNoOfVancancie.Text);
                //jobCircular.DemandedTime = Convert.ToDateTime(txtDemandedTime.Text);
                jobCircular.DemandedTime = CommonHelper.DateTimeToMMDDYYYY(txtDemandedTime.Text);
                jobCircular.AgeRangeFrom = Convert.ToByte(ddlAgeRangeFrom.SelectedValue);
                jobCircular.AgeRangeTo = Convert.ToByte(ddlAgeRangeTo.SelectedValue);
                jobCircular.Gender = ddlGender.SelectedValue;
                jobCircular.YearOfExperiance = Convert.ToByte(ddlYearOfExperiance.SelectedValue);
                jobCircular.JobDescription = txtJobDescription.Text;
                jobCircular.EducationalQualification = txtEducationalQualification.Text;
                jobCircular.AdditionalJobRequirement = txtAdditionalJobRequirement.Text;

                if (hfJobCircularId.Value == string.Empty)
                {
                    int tmpJobCircularId = 0;
                    jobCircular.ApprovedStatus = HMConstants.ApprovalStatus.Pending.ToString();
                    jobCircular.CreatedBy = userInformationBO.UserInfoId;

                    status = jobCircularDa.SaveJobCircularInfo(jobCircular, out tmpJobCircularId);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        ClearForm();
                    }
                }
                else
                {
                    jobCircular.JobCircularId = Convert.ToInt64(hfJobCircularId.Value);
                    jobCircular.LastModifiedBy = userInformationBO.UserInfoId;

                    status = jobCircularDa.UpdateJobCircularInfo(jobCircular);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        ClearForm();
                    }
                }

                if (!status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }

                SetTab("circularEntry");
            }
            catch (Exception ex)
            {
                innboardMessage.Value = JsonConvert.SerializeObject(CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error));
                
            }
        }
        private bool IsFormValid()
        {
            bool flag = true;
            int checkNumber;

            if (txtJobTitle.Text == "")
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Job Title.", AlertType.Warning);
                flag = false;
                txtJobTitle.Focus();
            }
            else if (ddlDepartments.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Department.", AlertType.Warning);
                flag = false;
                ddlDepartments.Focus();
            }
            else if (ddlJobType.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Job Type.", AlertType.Warning);
                flag = false;
                ddlJobType.Focus();
            }
            else if (ddlJobLevel.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Job Level.", AlertType.Warning);
                flag = false;
                ddlJobLevel.Focus();
            }
            else if (txtNoOfVancancie.Text == "")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "No of Vacancies.", AlertType.Warning);
                flag = false;
                txtNoOfVancancie.Focus();
            }
            else if (txtDemandedTime.Text == "")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Demand Date.", AlertType.Warning);
                flag = false;
                txtDemandedTime.Focus();
            }
            else if (ddlAgeRangeFrom.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Age Range From.", AlertType.Warning);
                flag = false;
                ddlAgeRangeFrom.Focus();
            }
            else if (ddlAgeRangeTo.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Age Range To.", AlertType.Warning);
                flag = false;
                ddlAgeRangeTo.Focus();
            }
            else if (ddlGender.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Gender.", AlertType.Warning);
                flag = false;
                ddlGender.Focus();
            }
            //else if (ddlYearOfExperiance.SelectedIndex == 0)
            //{
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Year of Experience.", AlertType.Warning);
            //    flag = false;
            //    ddlYearOfExperiance.Focus();
            //}
            else if (txtJobDescription.Text == "")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Job Description.", AlertType.Warning);
                flag = false;
                txtJobDescription.Focus();
            }
            else if (txtEducationalQualification.Text == "")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Educational Qualification.", AlertType.Warning);
                flag = false;
                txtEducationalQualification.Focus();
            }

            SetTab("circularEntry");
            return flag;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridview();
        }
        protected void gvJobCircular_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                var item = (PayrollJobCircularBO)e.Row.DataItem;

                ImageButton imgApproved = (ImageButton)e.Row.FindControl("ImgApproved");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                ImageButton imgPrint = (ImageButton)e.Row.FindControl("ImagePrint");

                if (item.ApprovedStatus != HMConstants.ApprovalStatus.Approved.ToString() && item.ApprovedStatus != HMConstants.ApprovalStatus.Cancel.ToString())
                {
                    imgApproved.Visible = true;
                    imgUpdate.Visible = true;
                    imgDelete.Visible = true;
                    imgPrint.Visible = false;
                }
                else if (item.ApprovedStatus == HMConstants.ApprovalStatus.Approved.ToString() && item.ApprovedStatus != HMConstants.ApprovalStatus.Cancel.ToString())
                {
                    imgApproved.Visible = false;
                    imgUpdate.Visible = false;
                    imgDelete.Visible = false;
                    imgPrint.Visible = true;
                }
                else
                {
                    imgApproved.Visible = false;
                    imgUpdate.Visible = false;
                    imgDelete.Visible = false;
                    imgPrint.Visible = false;
                }
            }
        }
        protected void gvJobCircular_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                JobCircularNRecruitmentDA jobCircularDa = new JobCircularNRecruitmentDA();
                PayrollJobCircularBO jobCircular = new PayrollJobCircularBO();

                if (e.CommandName == "CmdEdit")
                {
                    Int64 jobCircularId = Convert.ToInt64(e.CommandArgument.ToString());
                    FillForm(jobCircularId);
                    this.btnSave.Text = "Update";
                }
                else if (e.CommandName == "CmdApprovedJobCircular")
                {
                    Int64 jobCircularId = Convert.ToInt32(e.CommandArgument.ToString());

                    jobCircular.JobCircularId = jobCircularId;
                    jobCircular.LastModifiedBy = userInformationBO.UserInfoId;
                    jobCircular.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();

                    bool status = jobCircularDa.UpdateJobCircularApprovedStatus(jobCircular);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Approved, AlertType.Success);
                        LoadGridview();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                }
                else if (e.CommandName == "CmdJobCircularCancel")
                {
                    Int64 jobCircularId = Convert.ToInt32(e.CommandArgument.ToString());

                    jobCircular.JobCircularId = jobCircularId;
                    jobCircular.LastModifiedBy = userInformationBO.UserInfoId;
                    jobCircular.ApprovedStatus = HMConstants.ApprovalStatus.Cancel.ToString();

                    bool status = jobCircularDa.UpdateJobCircularApprovedStatus(jobCircular);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Cancel, AlertType.Success);
                        LoadGridview();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        //**************************** User Defined Method ****************************//
        private void LoadGridview()
        {
            List<PayrollJobCircularBO> jobCircular = new List<PayrollJobCircularBO>();
            JobCircularNRecruitmentDA jobCircularDa = new JobCircularNRecruitmentDA();

            DateTime? dateFrom = null, dateTo = null;
            int departmentId = 0;
            string jobTitle = string.Empty;

            if (!string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                //dateFrom = Convert.ToDateTime(txtFromDate.Text);
                dateFrom = CommonHelper.DateTimeToMMDDYYYY(txtFromDate.Text);
            }

            if (!string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                //dateTo = Convert.ToDateTime(txtToDate.Text);
                dateTo = CommonHelper.DateTimeToMMDDYYYY(txtToDate.Text);
            }

            if (ddlSearchDepartment.SelectedIndex != 0)
            {
                departmentId = Convert.ToInt32(ddlSearchDepartment.SelectedValue);
            }

            if (!string.IsNullOrEmpty(txtSearchJobTitle.Text))
            {
                jobTitle = txtSearchJobTitle.Text;
            }

            jobCircular = jobCircularDa.GetJobCircular(departmentId, dateFrom, dateTo, jobTitle);

            gvJobCircular.DataSource = jobCircular;
            gvJobCircular.DataBind();

            SetTab("circularSearch");
        }
        private void FillForm(Int64 jobCircularId)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            PayrollJobCircularBO jobCircular = new PayrollJobCircularBO();
            JobCircularNRecruitmentDA jobCircularDa = new JobCircularNRecruitmentDA();
            jobCircular = jobCircularDa.GetJobCircularById(jobCircularId);

            hfJobCircularId.Value = jobCircular.JobCircularId.ToString();
            ddlStaffRequisition.SelectedValue = jobCircular.StaffRequisitionDetailsId.ToString();
            txtJobTitle.Text = jobCircular.JobTitle;
            ddlJobType.SelectedValue = Convert.ToString(jobCircular.JobType);
            ddlJobLevel.SelectedValue = jobCircular.JobLevel;
            ddlDepartments.SelectedValue = Convert.ToString(jobCircular.DepartmentId);
            txtNoOfVancancie.Text = Convert.ToString(jobCircular.NoOfVancancie);
            txtDemandedTime.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay((jobCircular.DemandedTime));
            ddlAgeRangeFrom.SelectedValue = Convert.ToString(jobCircular.AgeRangeFrom);
            ddlAgeRangeTo.SelectedValue = Convert.ToString(jobCircular.AgeRangeTo);
            ddlGender.SelectedValue = jobCircular.Gender;
            ddlYearOfExperiance.SelectedValue = Convert.ToString(jobCircular.YearOfExperiance);
            txtJobDescription.Text = jobCircular.JobDescription;
            txtEducationalQualification.Text = jobCircular.EducationalQualification;
            txtAdditionalJobRequirement.Text = jobCircular.AdditionalJobRequirement;
            jobCircular.LastModifiedBy = userInformationBO.UserInfoId;

            SetTab("circularEntry");
        }
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
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmEmpLoan.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSearch.Visible = isSavePermission;
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
            hfJobCircularId.Value = string.Empty;
            ddlStaffRequisition.SelectedValue = "0";
            txtJobTitle.Text = string.Empty;
            ddlJobType.SelectedValue = "0";
            ddlJobLevel.SelectedValue = "0";
            ddlDepartments.SelectedValue = "0";
            txtNoOfVancancie.Text = string.Empty;
            txtDemandedTime.Text = string.Empty;
            ddlAgeRangeFrom.SelectedValue = "0";
            ddlAgeRangeTo.SelectedValue = "0";
            ddlGender.SelectedValue = "0";
            ddlYearOfExperiance.SelectedValue = "0";
            txtJobDescription.Text = string.Empty;
            txtEducationalQualification.Text = string.Empty;
            txtAdditionalJobRequirement.Text = string.Empty;

            this.btnSave.Text = "Save";
        }
        private void LoadStaffRequisition()
        {
            StaffingBudgetDA entityDA = new StaffingBudgetDA();
            this.ddlStaffRequisition.DataSource = entityDA.GetApprovedStaffRequisitionDetails();
            this.ddlStaffRequisition.DataTextField = "RequisitionDescription";
            this.ddlStaffRequisition.DataValueField = "StaffRequisitionDetailsId";
            this.ddlStaffRequisition.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "Ad Hoc Requisition";
            this.ddlStaffRequisition.Items.Insert(0, item);
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
            this.ddlJobType.DataSource = entityDA.GetEmpTypeInfo();
            this.ddlJobType.DataTextField = "Name";
            this.ddlJobType.DataValueField = "TypeId";
            this.ddlJobType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlJobType.Items.Insert(0, item);
        }
        //************************ User Defined WebMethod ********************//
        [WebMethod]
        public static PayrollJobCircularBO GetJobCircularDetails(int jobCircularId)
        {
            PayrollJobCircularBO jobCircular = new PayrollJobCircularBO();
            JobCircularNRecruitmentDA jobCircularDa = new JobCircularNRecruitmentDA();
            jobCircular = jobCircularDa.GetJobCircularById(jobCircularId);

            return jobCircular;
        }
        [WebMethod]
        public static PayrollStaffRequisitionDetailsBO PageMethodsLoadStaffRequisitionInformation(int staffRequisitionDetailsId)
        {
            PayrollStaffRequisitionDetailsBO staffBudget = new PayrollStaffRequisitionDetailsBO();
            StaffingBudgetDA staffDa = new StaffingBudgetDA();
            staffBudget = staffDa.GetStaffRequisitionDetailsById(staffRequisitionDetailsId);

            return staffBudget;
        }
    }
}