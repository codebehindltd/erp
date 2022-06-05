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
    public partial class frmStaffRequisition : BasePage
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //bool status = false;

                //UserInformationBO userInformationBO = new UserInformationBO();
                //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                //JobCircularNRecruitmentDA jobCircularDa = new JobCircularNRecruitmentDA();
                //PayrollJobCircularBO jobCircular = new PayrollJobCircularBO();

                //jobCircular.JobTitle = txtJobTitle.Text;
                //jobCircular.CircularDate = DateTime.Now;
                //jobCircular.JobType = Convert.ToInt32(ddlJobType.SelectedValue);
                //jobCircular.JobLevel = ddlJobLevel.SelectedValue;
                //jobCircular.DepartmentId = Convert.ToInt32(ddlDepartments.SelectedValue);
                //jobCircular.NoOfVancancie = Convert.ToInt16(txtNoOfVancancie.Text);
                //jobCircular.DemandedTime = Convert.ToDateTime(txtDemandedTime.Text);
                //jobCircular.AgeRangeFrom = Convert.ToByte(ddlAgeRangeFrom.SelectedValue);
                //jobCircular.AgeRangeTo = Convert.ToByte(ddlAgeRangeTo.SelectedValue);
                //jobCircular.Gender = ddlGender.SelectedValue;
                //jobCircular.YearOfExperiance = Convert.ToByte(ddlYearOfExperiance.SelectedValue);
                //jobCircular.JobDescription = txtJobDescription.Text;
                //jobCircular.EducationalQualification = txtEducationalQualification.Text;
                //jobCircular.AdditionalJobRequirement = txtAdditionalJobRequirement.Text;

                //if (hfJobCircularId.Value == string.Empty)
                //{
                //    jobCircular.ApprovedStatus = HMConstants.ApprovalStatus.Pending.ToString();
                //    jobCircular.CreatedBy = userInformationBO.UserInfoId;

                //    status = jobCircularDa.SaveJobCircularInfo(jobCircular);

                //    if (status)
                //    {
                //        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                //        ClearForm();
                //    }
                //}
                //else
                //{
                //    jobCircular.JobCircularId = Convert.ToInt64(hfJobCircularId.Value);
                //    jobCircular.LastModifiedBy = userInformationBO.UserInfoId;

                //    status = jobCircularDa.UpdateJobCircularInfo(jobCircular);

                //    if (status)
                //    {
                //        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                //        ClearForm();
                //    }
                //}

                //if (!status)
                //{
                //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                //}

                SetTab("circularEntry");
            }
            catch (Exception ex)
            {
                innboardMessage.Value = JsonConvert.SerializeObject(CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error));
                
            }
        }

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
            List<PayrollStaffRequisitionBO> staffRequisitionList = new List<PayrollStaffRequisitionBO>();
            StaffingBudgetDA staffBudget = new StaffingBudgetDA();

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

            staffRequisitionList = staffBudget.GetStaffRequisitionByDepartmentIdAndFiscalYear(departmentId,fiscalYear);

            gvStaffRequisition.DataSource = staffRequisitionList;
            gvStaffRequisition.DataBind();

            SetTab("circularSearch");
        }

        private void FillForm(Int64 staffRequisitionId)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            PayrollStaffRequisitionBO jobCircular = new PayrollStaffRequisitionBO();
            StaffingBudgetDA jobCircularDa = new StaffingBudgetDA();
            jobCircular = jobCircularDa.GetStaffRequisitionById(staffRequisitionId);

            hfStaffRequisitionId.Value = jobCircular.StaffRequisitionId.ToString();
            hfStaffRequisitionDetailsId.Value = jobCircular.StaffRequisitionDetailsId.ToString();
            //txtJobTitle.Text = jobCircular.JobTitle;
            ddlJobType.SelectedValue = Convert.ToString(jobCircular.JobTypeId);
            ddlJobLevel.SelectedValue = jobCircular.JobLevel;
            ddlDepartments.SelectedValue = Convert.ToString(jobCircular.DepartmentId);
            txtRequisitionQuantity.Text = Convert.ToString(jobCircular.RequisitionQuantity);
            txtDemandedTime.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime( jobCircular.CreatedDate.ToShortDateString()));
            txtSalaryAmount.Text = jobCircular.SalaryAmount.ToString();
            ddlFiscalYearSetup.SelectedValue = Convert.ToString(jobCircular.FiscalYear);
            //ddlFiscalYearSetup.DataValueField = jobCircular
            btnSave.Text = "Update";
            //txtJobTitle.Text = jobCircular.JobTitle;
            //ddlJobType.SelectedValue = Convert.ToString(jobCircular.JobType);
            //ddlJobLevel.SelectedValue = jobCircular.JobLevel;
            //ddlDepartments.SelectedValue = Convert.ToString(jobCircular.DepartmentId);
            //txtNoOfVancancie.Text = Convert.ToString(jobCircular.NoOfVancancie);
            //txtDemandedTime.Text = Convert.ToString(jobCircular.DemandedTime);
            //ddlAgeRangeFrom.SelectedValue = Convert.ToString(jobCircular.AgeRangeFrom);
            //ddlAgeRangeTo.SelectedValue = Convert.ToString(jobCircular.AgeRangeTo);
            //ddlGender.SelectedValue = jobCircular.Gender;
            //ddlYearOfExperiance.SelectedValue = Convert.ToString(jobCircular.YearOfExperiance);
            //txtJobDescription.Text = jobCircular.JobDescription;
            //txtEducationalQualification.Text = jobCircular.EducationalQualification;
            //txtAdditionalJobRequirement.Text = jobCircular.AdditionalJobRequirement;
            //jobCircular.LastModifiedBy = userInformationBO.UserInfoId;

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
            this.ddlJobType.DataSource = entityDA.GetEmpTypeInfo();
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
        public static ReturnInfo SaveStaffRequisition(int staffRequisitionId, PayrollStaffRequisitionBO StaffRequisition, List<PayrollStaffRequisitionDetailsBO> NewlyAddedStaffRequisitionDetails, List<PayrollStaffRequisitionDetailsBO> EditedStaffRequisitionDetails, List<PayrollStaffRequisitionDetailsBO> DeletedStaffRequisitionDetails)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                StaffingBudgetDA staffingBudgetDa = new StaffingBudgetDA();

                StaffRequisition.ApprovedStatus = HMConstants.ApprovalStatus.Submit.ToString();

                if (staffRequisitionId == 0)
                {
                    //PayrollStaffRequisitionBO StaffRequisition, List<PayrollStaffRequisitionDetailsBO> NewlyAddedStaffRequisitionDetails
                    long tmpStaffRequisitionId = 0;
                    StaffRequisition.CreatedBy = userInformationBO.UserInfoId;
                    status = staffingBudgetDa.SaveStaffRequisition(StaffRequisition, NewlyAddedStaffRequisitionDetails, out tmpStaffRequisitionId);

                    if (status)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.StaffRequisition.ToString(), tmpStaffRequisitionId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.StaffRequisition));
                    }
                }
                else
                {
                    //requsition.LastModifiedBy = userInformationBO.UserInfoId;
                    //status = requisitionDA.UpdatePMRequisitionInfo(requsition, AddedItem, EditedItem, DeletedItem);
                    StaffRequisition.CreatedBy = userInformationBO.UserInfoId;
                    status = staffingBudgetDa.UpdateStaffRequisition(StaffRequisition, EditedStaffRequisitionDetails);
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
        public static PayrollStaffingBudgeDetailsBO LoadStaffingBudget(int departmentId, int jobTypeId, string jobLevel)
        {
            DateTime dateFrom = DateTime.Now.AddMonths(-11), dateTo = DateTime.Now;

            PayrollStaffingBudgeDetailsBO staffBudget = new PayrollStaffingBudgeDetailsBO();
            StaffingBudgetDA staffDa = new StaffingBudgetDA();
            staffBudget = staffDa.GetPayrollStaffingBudget(departmentId, jobTypeId, jobLevel, dateFrom, dateTo);

            return staffBudget;
        }

        [WebMethod]
        public static GLFiscalYearBO GetFiscalYearInfo(int _id)
        {
            GLFiscalYearBO FiscalYearInfo = new GLFiscalYearBO();
            GLFiscalYearDA DA = new GLFiscalYearDA();
            FiscalYearInfo = DA.GetFiscalYearId(_id);

            return FiscalYearInfo;
        }

        protected void gvStaffRequisition_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                var item = (PayrollStaffRequisitionBO)e.Row.DataItem;

                ImageButton imgApproved = (ImageButton)e.Row.FindControl("ImgApproved");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");

                if (item.ApprovedStatus != HMConstants.ApprovalStatus.Approved.ToString() && item.ApprovedStatus != HMConstants.ApprovalStatus.Cancel.ToString())
                {
                    imgUpdate.Visible = isUpdatePermission;
                    imgDelete.Visible = isDeletePermission;
                    imgApproved.Visible = isSavePermission;
                }
                else
                {
                    imgUpdate.Visible = false;
                    imgDelete.Visible = false;
                    imgApproved.Visible = false;
                }
            }
        }

        protected void gvStaffRequisition_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            try
            {
                if (e.CommandName == "CmdEdit")
                {
                    int transferId = Convert.ToInt32(e.CommandArgument.ToString());
                    string result = string.Empty;

                    StaffingBudgetDA empDa = new StaffingBudgetDA();
                    FillForm(transferId);
                }
                else if (e.CommandName == "CmdApproved")
                {
                    int transferId = Convert.ToInt32(e.CommandArgument.ToString());
                    string result = string.Empty;

                    StaffingBudgetDA empDa = new StaffingBudgetDA();

                    PayrollStaffRequisitionBO trn = new PayrollStaffRequisitionBO();
                    trn.StaffRequisitionId = transferId;
                    trn.LastModifiedBy = userInformationBO.UserInfoId;
                    trn.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();

                    bool status = empDa.UpdateStaffRequisitionStatus(trn);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Approved, AlertType.Success);
                        LoadGridview();
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.StaffRequisition.ToString(), trn.StaffRequisitionId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.StaffRequisition));
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                    this.SetTab("SearchTab");
                }
                else if (e.CommandName == "CmdDelete")
                {
                    int transferId = Convert.ToInt32(e.CommandArgument.ToString());
                    string result = string.Empty;

                    StaffingBudgetDA empDa = new StaffingBudgetDA();

                    PayrollStaffRequisitionBO trn = new PayrollStaffRequisitionBO();
                    trn.StaffRequisitionId = transferId;
                    trn.LastModifiedBy = userInformationBO.UserInfoId;
                    trn.ApprovedStatus = HMConstants.ApprovalStatus.Cancel.ToString();

                    bool status = empDa.UpdateStaffRequisitionStatus(trn);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Cancel, AlertType.Success);
                        LoadGridview();
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.StaffRequisition.ToString(), trn.StaffRequisitionId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.StaffRequisition));
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                    this.SetTab("SearchTab");
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }

    }
}