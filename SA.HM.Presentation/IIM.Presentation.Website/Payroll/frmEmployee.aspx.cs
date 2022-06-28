using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using System.Collections;
using Newtonsoft.Json;
using HotelManagement.Entity.HMCommon;
using System.IO;
using HotelManagement.Entity.HotelManagement;
using System.Web.Services;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmployee : BasePage
    {
        HiddenField innboardMessage;
        protected int _EmployeeId;
        ArrayList arrayEducationDelete;
        ArrayList arrayExperienceDelete;
        ArrayList arrayDependentDelete;
        ArrayList arrayNomineeDelete;
        ArrayList arrayReferencePersonDelete;
        ArrayList arrayDocumentDelete;
        ArrayList arrayCareerTrainingDelete;
        ArrayList arrayReferenceDelete;
        HMUtility hmUtility = new HMUtility();
        protected int _CompanyId = 0;
        protected int _ProjectId = 0;
        protected List<GLProjectBO> _ProjectListBo = new List<GLProjectBO>();

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckObjectPermission();
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            AddEditODeleteDetail();
            if (!IsPostBack)
            {
                companyProjectUserControl.ddlFirstValueVar = "select";
                
                int EmployeeId = 0;
                Random rd = new Random();

                EmployeeId = rd.Next(100000, 999999);
                RandomEmpId.Value = EmployeeId.ToString();
                tempEmpId.Value = EmployeeId.ToString();
                FileUpload();
                txtGoToScrolling.Text = "EntryPanel";
                LoadEmployeeCompany();
                LoadDepartment();
                LoadEmployeeType();
                LoadDesignation();
                LoadCompanyCountry();
                ddlCountryId.SelectedValue = LoadCompanyCountry().SetupValue;
                LoadReportingTo();
                LoadReportingTo2();
                LoadMaritualStatus();
                LoadGender();
                LoadRelegion();
                LoadBloodGroup();
                LoadBloodGroupDepen();
                LoadCountry();
                LoadGrade();
                LoadBank();
                LoadWorkStation();
                LoadDonor();
                LoadDivision();
                LoadCostCenter();
                LoadEducationLevel();
                LoadBenefitList();
                TabPermissionList();
                LoadCommonDropDownHiddenField();
                LoadEmployeeStatus();
                LoadExtraShowHideInformation();
                LoadConversionHead();
                IsEmployeeCodeAutoGenerate();
                //LoadGLCompany();

                string editId = Request.QueryString["editId"];
                if (!string.IsNullOrWhiteSpace(editId))
                {
                    int Id = Convert.ToInt32(editId);
                    if (Id > 0)
                    {
                        FillForm(Id);
                        SetTab("EmployeeTab");
                    }
                }
                else
                {
                    Session["EmpExperienceList"] = null;
                    Session["EmpEducationList"] = null;
                    Session["EmpDependentList"] = null;
                    Session["EmpNomineeList"] = null;
                    Session["EmpReferencePersonsList"] = null;
                }
                string empId = Request.QueryString["EmpId"];
                if (!string.IsNullOrWhiteSpace(empId))
                {
                    int Id = Convert.ToInt32(empId);
                    if (Id > 0)
                    {
                        FillForm(Id);
                        SetTab("EmployeeTab");
                    }
                }
                //Session["EmpExperienceList"] = null;
                //Session["EmpEducationList"] = null;
                //Session["EmpDependentList"] = null;
                //Session["EmpNomineeList"] = null;
            }

            if (LoadCompanyCountry().SetupValue != "19")
            {
                pnlDivisionDistrictThana.Visible = false;
            }
        }
        private void LoadEmployeeCompany()
        {
            hfIsSingle.Value = "0";
            GLCompanyDA entityDA = new GLCompanyDA();
            List<GLCompanyBO> GLCompanyBOList = new List<GLCompanyBO>();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO.UserInfoId == 1)
            {
                GLCompanyBOList = entityDA.GetAllGLCompanyInfo();
            }
            else
            {
                GLCompanyBOList = entityDA.GetAllGLCompanyInfoByUserGroupId(userInformationBO.UserGroupId);
            }


            if (GLCompanyBOList.Count == 1)
            {
                ddlEmpCompanyId.DataSource = GLCompanyBOList;
                ddlEmpCompanyId.DataTextField = "Name";
                ddlEmpCompanyId.DataValueField = "CompanyId";
                ddlEmpCompanyId.DataBind();
            }
            else
            {
                ddlEmpCompanyId.DataSource = GLCompanyBOList;
                ddlEmpCompanyId.DataTextField = "Name";
                ddlEmpCompanyId.DataValueField = "CompanyId";
                ddlEmpCompanyId.DataBind();
                System.Web.UI.WebControls.ListItem itemCompany = new System.Web.UI.WebControls.ListItem();
                itemCompany.Value = "0";
                itemCompany.Text = hmUtility.GetDropDownFirstValue();
                ddlEmpCompanyId.Items.Insert(0, itemCompany);
            }


            hfIsPayrollCompanyAndEmployeeCompanyDifferent.Value = "0";
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isPayrollCompanyAndEmployeeCompanyDifferentBO = new HMCommonSetupBO();
            isPayrollCompanyAndEmployeeCompanyDifferentBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollCompanyAndEmployeeCompanyDifferent", "IsPayrollCompanyAndEmployeeCompanyDifferent");
            if (isPayrollCompanyAndEmployeeCompanyDifferentBO != null)
            {
                if (isPayrollCompanyAndEmployeeCompanyDifferentBO.SetupValue == "1")
                {
                    hfIsPayrollCompanyAndEmployeeCompanyDifferent.Value = "1";
                    companyProjectUserControl.CompanyLabelText = "Payroll Company";
                    companyProjectUserControl.ProjectLabelText = "Payroll Project";
                }
            }

        }
        protected void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Payroll/frmEmployee.aspx");
        }
        protected void btnEmpEducation_Click(object sender, EventArgs e)
        {
            if (!IsEducationValid())
            {
                return;
            }

            int dynamicEducationId = 0;
            List<EmpEducationBO> EmpEducationListBO = Session["EmpEducationList"] == null ? new List<EmpEducationBO>() : Session["EmpEducationList"] as List<EmpEducationBO>;

            if (!string.IsNullOrWhiteSpace(hfEducationId.Text))
                dynamicEducationId = Convert.ToInt32(hfEducationId.Text);

            EmpEducationBO detailBO = dynamicEducationId == 0 ? new EmpEducationBO() : EmpEducationListBO.Where(x => x.EducationId == dynamicEducationId).FirstOrDefault();
            if (EmpEducationListBO.Contains(detailBO))
                EmpEducationListBO.Remove(detailBO);

            if (ddlExamLevel.SelectedIndex != 0)
            {
                detailBO.LevelId = Convert.ToInt32(ddlExamLevel.SelectedValue);
            }
            detailBO.ExamName = txtExamName.Text;
            detailBO.InstituteName = txtInstituteName.Text;
            detailBO.SubjectName = txtSubjectName.Text;
            detailBO.PassYear = txtPassYear.Text;
            detailBO.PassClass = txtPassClass.Text;
            detailBO.EducationId = dynamicEducationId == 0 ? EmpEducationListBO.Count + 1 : dynamicEducationId;
            EmpEducationListBO.Add(detailBO);
            Session["EmpEducationList"] = EmpEducationListBO;
            gvEmpEducation.DataSource = Session["EmpEducationList"] as List<EmpEducationBO>;
            gvEmpEducation.DataBind();
            ClearEmpEducation();

            txtGoToScrolling.Text = "EducationInformation";
            SetTab("EducationTab");
        }
        protected void gvEmpEducation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _EmpEducationId;

            if (e.CommandName == "CmdEdit")
            {
                _EmpEducationId = Convert.ToInt32(e.CommandArgument.ToString());
                hfEducationId.Text = _EmpEducationId.ToString();
                var educationDetailBO = (List<EmpEducationBO>)Session["EmpEducationList"];
                if (educationDetailBO != null)
                {
                    var educationBO = educationDetailBO.Where(x => x.EducationId == _EmpEducationId).FirstOrDefault();
                    if (educationBO != null && educationBO.EducationId > 0)
                    {
                        ddlExamLevel.SelectedValue = educationBO.LevelId.ToString();
                        txtExamName.Text = educationBO.ExamName;
                        txtInstituteName.Text = educationBO.InstituteName;
                        txtSubjectName.Text = educationBO.SubjectName;
                        txtPassYear.Text = educationBO.PassYear;
                        txtPassClass.Text = educationBO.PassClass;
                        btnEmpEducation.Text = "Update";
                    }
                    else
                    {
                        btnEmpEducation.Text = "Add";
                    }
                }
            }
            else if (e.CommandName == "CmdDelete")
            {
                _EmpEducationId = Convert.ToInt32(e.CommandArgument.ToString());
                var educationDetailBO = (List<EmpEducationBO>)Session["EmpEducationList"];
                if (educationDetailBO != null)
                {
                    var empEducation = educationDetailBO.Where(x => x.EducationId == _EmpEducationId).FirstOrDefault();
                    educationDetailBO.Remove(empEducation);
                    Session["EmpEducationList"] = educationDetailBO;
                    arrayEducationDelete.Add(_EmpEducationId);
                }

                gvEmpEducation.DataSource = Session["EmpEducationList"] as List<EmpEducationBO>;
                gvEmpEducation.DataBind();
            }
            SetTab("EducationTab");
        }
        protected void btnEmpExperience_Click(object sender, EventArgs e)
        {

            if (!IsExperienceValid())
            {
                return;
            }
            int dynamicExperienceId = 0;
            List<EmpExperienceBO> EmpExperienceListBO = Session["EmpExperienceList"] == null ? new List<EmpExperienceBO>() : Session["EmpExperienceList"] as List<EmpExperienceBO>;

            if (!string.IsNullOrWhiteSpace(hfExperienceId.Text))
                dynamicExperienceId = Convert.ToInt32(hfExperienceId.Text);

            EmpExperienceBO detailBO = dynamicExperienceId == 0 ? new EmpExperienceBO() : EmpExperienceListBO.Where(x => x.ExperienceId == dynamicExperienceId).FirstOrDefault();
            if (EmpExperienceListBO.Contains(detailBO))
                EmpExperienceListBO.Remove(detailBO);

            detailBO.CompanyName = txtCompanyName.Text;
            detailBO.CompanyUrl = txtCompanyUrl.Text;
            //detailBO.JoinDate = Convert.ToDateTime(txtJoinDate.Text);            
            detailBO.JoinDate = CommonHelper.DateTimeToMMDDYYYY(txtJoinDate.Text);
            detailBO.JoinDesignation = txtJoinDesignation.Text;
            //detailBO.LeaveDate = Convert.ToDateTime(txtLeaveDate.Text);            
            detailBO.LeaveDate = CommonHelper.DateTimeToMMDDYYYY(txtLeaveDate.Text);
            detailBO.LeaveDesignation = txtLeaveDesignation.Text;
            detailBO.Achievements = txtAchievements.Text;
            detailBO.ShowJoinDate = txtJoinDate.Text;
            detailBO.ShowLeaveDate = txtLeaveDate.Text;
            detailBO.ExperienceId = dynamicExperienceId == 0 ? EmpExperienceListBO.Count + 1 : dynamicExperienceId;

            EmpExperienceListBO.Add(detailBO);

            Session["EmpExperienceList"] = EmpExperienceListBO;

            gvExperience.DataSource = Session["EmpExperienceList"] as List<EmpExperienceBO>;
            gvExperience.DataBind();

            ClearEmpExperience();

            txtGoToScrolling.Text = "ExperienceInformation";
            SetTab("ExperienceTab");
        }
        protected void gvExperience_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _EmpExperienceId;

            if (e.CommandName == "CmdEdit")
            {
                _EmpExperienceId = Convert.ToInt32(e.CommandArgument.ToString());
                hfExperienceId.Text = _EmpExperienceId.ToString();
                var experienceDetailBO = (List<EmpExperienceBO>)Session["EmpExperienceList"];
                if (experienceDetailBO != null)
                {
                    var experienceBO = experienceDetailBO.Where(x => x.ExperienceId == _EmpExperienceId).FirstOrDefault();
                    if (experienceBO != null && experienceBO.ExperienceId > 0)
                    {
                        txtCompanyName.Text = experienceBO.CompanyName;
                        txtCompanyUrl.Text = experienceBO.CompanyUrl;
                        txtJoinDate.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(experienceBO.JoinDate);
                        txtJoinDesignation.Text = experienceBO.JoinDesignation;
                        txtLeaveDate.Text = experienceBO.LeaveDate == null ? experienceBO.LeaveDate.ToString() : CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(experienceBO.LeaveDate));
                        txtLeaveDesignation.Text = experienceBO.LeaveDesignation;
                        txtAchievements.Text = experienceBO.Achievements;

                        btnEmpExperience.Text = "Update";
                    }
                    else
                    {
                        btnEmpExperience.Text = "Add";
                    }
                }
            }
            else if (e.CommandName == "CmdDelete")
            {
                _EmpExperienceId = Convert.ToInt32(e.CommandArgument.ToString());
                var experienceDetailBO = (List<EmpExperienceBO>)Session["EmpExperienceList"];
                if (experienceDetailBO != null)
                {
                    var empExperience = experienceDetailBO.Where(x => x.ExperienceId == _EmpExperienceId).FirstOrDefault();
                    experienceDetailBO.Remove(empExperience);
                    Session["EmpExperienceList"] = experienceDetailBO;
                    arrayExperienceDelete.Add(_EmpExperienceId);
                    gvExperience.DataSource = Session["EmpExperienceList"] as List<EmpExperienceBO>;
                    gvExperience.DataBind();
                }
            }
            SetTab("ExperienceTab");
        }
        protected void btnEmpDependent_Click(object sender, EventArgs e)
        {
            if (!IsDependentValid())
            {
                return;
            }
            int dynamicDependentId = 0;
            List<EmpDependentBO> EmpDependentListBO = Session["EmpDependentList"] == null ? new List<EmpDependentBO>() : Session["EmpDependentList"] as List<EmpDependentBO>;

            if (!string.IsNullOrWhiteSpace(lblHiddenDependentId.Text))
                dynamicDependentId = Convert.ToInt32(lblHiddenDependentId.Text);

            EmpDependentBO detailBO = dynamicDependentId == 0 ? new EmpDependentBO() : EmpDependentListBO.Where(x => x.DependentId == dynamicDependentId).FirstOrDefault();
            if (EmpDependentListBO.Contains(detailBO))
                EmpDependentListBO.Remove(detailBO);
            if (!String.IsNullOrWhiteSpace(ddlBloodGroupDepen.SelectedValue))
            {
                detailBO.BloodGroupId = Convert.ToInt32(ddlBloodGroupDepen.SelectedValue);
            }
            detailBO.DependentName = txtDependentName.Text;
            detailBO.Relationship = txtRelationship.Text;
            //detailBO.DateOfBirth = Convert.ToDateTime(txtDateOfBirth.Text);            
            detailBO.DateOfBirth = CommonHelper.DateTimeToMMDDYYYY(txtDateOfBirth.Text);
            detailBO.DependentId = dynamicDependentId == 0 ? EmpDependentListBO.Count + 1 : dynamicDependentId;
            detailBO.Age = hfAge.Value;
            EmpDependentListBO.Add(detailBO);

            Session["EmpDependentList"] = EmpDependentListBO;
            gvDependent.DataSource = Session["EmpDependentList"] as List<EmpDependentBO>;
            gvDependent.DataBind();
            ClearEmpDependent();
            txtGoToScrolling.Text = "DependentInformation";
            SetTab("DependentTab");
        }
        protected void gvDependent_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _EmpDependentId;

            if (e.CommandName == "CmdEdit")
            {
                _EmpDependentId = Convert.ToInt32(e.CommandArgument.ToString());
                lblHiddenDependentId.Text = _EmpDependentId.ToString();
                var dependentDetailBO = (List<EmpDependentBO>)Session["EmpDependentList"];
                if (dependentDetailBO != null)
                {
                    var dependentBO = dependentDetailBO.Where(x => x.DependentId == _EmpDependentId).FirstOrDefault();
                    if (dependentBO != null && dependentBO.DependentId > 0)
                    {
                        txtDependentName.Text = dependentBO.DependentName;
                        txtRelationship.Text = dependentBO.Relationship;
                        ddlBloodGroupDepen.SelectedValue = dependentBO.BloodGroupId.ToString();
                        txtDateOfBirth.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(dependentBO.DateOfBirth.ToShortDateString().ToString()));
                        txtAge.Text = dependentBO.Age;
                        hfAge.Value = dependentBO.Age;
                        btnEmpDependent.Text = "Update";
                    }
                    else
                    {
                        btnEmpDependent.Text = "Add";
                    }
                }
            }
            else if (e.CommandName == "CmdDelete")
            {
                _EmpDependentId = Convert.ToInt32(e.CommandArgument.ToString());
                var dependentDetailBO = (List<EmpDependentBO>)Session["EmpDependentList"];
                if (dependentDetailBO != null)
                {
                    var empDependent = dependentDetailBO.Where(x => x.DependentId == _EmpDependentId).FirstOrDefault();
                    dependentDetailBO.Remove(empDependent);
                    Session["EmpDependentList"] = dependentDetailBO;
                    arrayDependentDelete.Add(_EmpDependentId);
                }
                gvDependent.DataSource = Session["EmpDependentList"] as List<EmpDependentBO>;
                gvDependent.DataBind();
            }
            SetTab("DependentTab");
        }
        protected void btnAddNominee_Click(object sender, EventArgs e)
        {
            if (!IsNomineeValid())
            {
                return;
            }

            int dynamicNomineeId = 0;
            List<EmpNomineeBO> EmpNomineeListBO = Session["EmpNomineeList"] == null ? new List<EmpNomineeBO>() : Session["EmpNomineeList"] as List<EmpNomineeBO>;

            if (!string.IsNullOrWhiteSpace(lblNomineeId.Text))
                dynamicNomineeId = Convert.ToInt32(lblNomineeId.Text);

            EmpNomineeBO detailBO = dynamicNomineeId == 0 ? new EmpNomineeBO() : EmpNomineeListBO.Where(x => x.NomineeId == dynamicNomineeId).FirstOrDefault();
            if (EmpNomineeListBO.Contains(detailBO))
                EmpNomineeListBO.Remove(detailBO);

            detailBO.NomineeName = txtNomineeName.Text;
            detailBO.Relationship = txtNomineeRelationship.Text;
            //detailBO.DateOfBirth = Convert.ToDateTime(txtNomineeDateOfBirth.Text);            
            detailBO.DateOfBirth = CommonHelper.DateTimeToMMDDYYYY(txtNomineeDateOfBirth.Text);
            detailBO.NomineeId = dynamicNomineeId == 0 ? EmpNomineeListBO.Count + 1 : dynamicNomineeId;
            detailBO.Age = hfNomineeAge.Value;
            detailBO.Percentage = Convert.ToDecimal(txtPercentage.Text);
            EmpNomineeListBO.Add(detailBO);
            Session["EmpNomineeList"] = EmpNomineeListBO;
            gvNominee.DataSource = Session["EmpNomineeList"] as List<EmpNomineeBO>;
            gvNominee.DataBind();
            lblNomineeId.Text = null;
            ClearNominee();
            SetTab("NomineeTab");
        }
        protected void gvNominee_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _EmpNomineeId;

            if (e.CommandName == "CmdEdit")
            {
                _EmpNomineeId = Convert.ToInt32(e.CommandArgument.ToString());
                lblNomineeId.Text = _EmpNomineeId.ToString();
                var nomineeDetailBO = (List<EmpNomineeBO>)Session["EmpNomineeList"];
                if (nomineeDetailBO != null)
                {
                    var nomineeBO = nomineeDetailBO.Where(x => x.NomineeId == _EmpNomineeId).FirstOrDefault();
                    if (nomineeBO != null && nomineeBO.NomineeId > 0)
                    {
                        txtNomineeName.Text = nomineeBO.NomineeName;
                        txtNomineeRelationship.Text = nomineeBO.Relationship;
                        txtNomineeDateOfBirth.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(nomineeBO.DateOfBirth);
                        txtNomineeAge.Text = nomineeBO.Age;
                        hfNomineeAge.Value = nomineeBO.Age;
                        txtPercentage.Text = nomineeBO.Percentage.ToString("0.00");
                        hfNomineeId.Value = Convert.ToString(nomineeBO.NomineeId);
                        btnAddNominee.Text = "Update";
                    }
                    else
                    {
                        btnAddNominee.Text = "Add";
                    }
                }
            }
            else if (e.CommandName == "CmdDelete")
            {
                _EmpNomineeId = Convert.ToInt32(e.CommandArgument.ToString());
                var nomineeDetailBO = (List<EmpNomineeBO>)Session["EmpNomineeList"];
                if (nomineeDetailBO != null)
                {
                    var nomineeBO = nomineeDetailBO.Where(x => x.NomineeId == _EmpNomineeId).FirstOrDefault();
                    nomineeDetailBO.Remove(nomineeBO);
                    Session["EmpNomineeList"] = nomineeDetailBO;
                    arrayNomineeDelete.Add(_EmpNomineeId);
                }
                gvNominee.DataSource = Session["EmpNomineeList"] as List<EmpNomineeBO>;
                gvNominee.DataBind();
            }
            SetTab("NomineeTab");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFormValid())
            {
                return;
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            EmployeeDA employeeDA = new EmployeeDA();
            EmployeeBO employeeBO = new EmployeeBO();

            employeeBO.EmpCode = txtEmpCode.Text;
            employeeBO.Title = ddlTitle.SelectedValue;
            employeeBO.FirstName = txtFirstName.Text.Trim();
            employeeBO.LastName = txtLastName.Text.Trim();
            if (!string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                employeeBO.DisplayName = (ddlTitle.SelectedValue + " " + txtFirstName.Text + " " + txtLastName.Text).Trim();
            }
            else
            {
                employeeBO.DisplayName = (ddlTitle.SelectedValue + " " + txtFirstName.Text).Trim();
            }

            employeeBO.JoinDate = hmUtility.GetDateTimeFromString(txtEmpJoinDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            if (!string.IsNullOrWhiteSpace(txtProvisionPeriod.Text))
            {
                employeeBO.ProvisionPeriod = hmUtility.GetDateTimeFromString(txtProvisionPeriod.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                employeeBO.ProvisionPeriod = null;
            }

            employeeBO.DepartmentId = Convert.ToInt32(ddlDepartmentId.SelectedValue);
            employeeBO.EmpTypeId = Convert.ToInt32(ddlEmpCategoryId.SelectedValue);
            employeeBO.DesignationId = Convert.ToInt32(ddlDesignationId.SelectedValue);
            employeeBO.GradeId = Convert.ToInt32(ddlGradeId.SelectedValue);
            employeeBO.OfficialEmail = txtOfficialEmail.Text;
            employeeBO.TinNumber = txtTinNumber.Text;
            employeeBO.Remarks = txtRemarks.Text;
            employeeBO.RandomEmpId = Int32.Parse(RandomEmpId.Value);
            employeeBO.RepotingTo = Int32.Parse(ddlReportingTo.SelectedValue);
            employeeBO.RepotingTo2 = Int32.Parse(ddlReportingTo2.SelectedValue);

            employeeBO.GlCompanyId = Int32.Parse(hfGLCompanyId.Value);
            employeeBO.GlProjectId = Int32.Parse(hfGLProjectId.Value);

            if (hfIsPayrollCompanyAndEmployeeCompanyDifferent.Value == "0")
            {
                employeeBO.EmpCompanyId = Int32.Parse(hfGLCompanyId.Value);
            }
            else
            {
                employeeBO.EmpCompanyId = Int32.Parse(ddlEmpCompanyId.SelectedValue);
            }

            employeeBO.FathersName = txtFathersName.Text;
            employeeBO.MothersName = txtMothersName.Text;
            employeeBO.EmployeeStatusId = Convert.ToInt32(ddlEmployeeStatus.SelectedValue);
            employeeBO.PayrollCurrencyId = Convert.ToInt32(ddlPayrollCurrencyId.SelectedValue);
            employeeBO.NotEffectOnHead = Convert.ToInt32(ddlNotEffectOnHead.SelectedValue);
            employeeBO.IsProvidentFundDeduct = (Convert.ToInt32(ddlIsProvidentFundDeduct.SelectedValue) == 1);

            if (!string.IsNullOrWhiteSpace(txtDateOfMarriage.Text))
            {
                employeeBO.EmpDateOfMarriage = hmUtility.GetDateTimeFromString(txtDateOfMarriage.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                employeeBO.EmpDateOfMarriage = null;
            }
            if (!string.IsNullOrWhiteSpace(txtEmpDateOfBirth.Text))
            {
                employeeBO.EmpDateOfBirth = hmUtility.GetDateTimeFromString(txtEmpDateOfBirth.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                employeeBO.EmpDateOfBirth = null;
            }
            if (ddlGender.SelectedIndex == 0)
            {
                employeeBO.Gender = string.Empty;
            }
            else
            {
                employeeBO.Gender = ddlGender.SelectedItem.Text;
            }
            employeeBO.Religion = ddlReligion.SelectedItem.Text;
            employeeBO.MaritalStatus = ddlMaritalStatus.SelectedItem.Text;
            if (employeeBO.MaritalStatus != "Married")
            {
                employeeBO.EmpDateOfMarriage = null;
            }
            if (ddlBloodGroup.SelectedIndex == 0)
            {
                employeeBO.BloodGroup = string.Empty;
            }
            else
            {
                employeeBO.BloodGroup = ddlBloodGroup.SelectedItem.Text;
            }

            employeeBO.Height = txtHeight.Text;
            employeeBO.CountryId = Int32.Parse(ddlCountryId.SelectedValue);
            //Nationality
            CountriesBO country = new CountriesBO();
            HMCommonDA commonDa = new HMCommonDA();
            country = commonDa.GetCountriesById(employeeBO.CountryId);
            employeeBO.Nationality = country.Nationality;

            employeeBO.NationalId = txtNationalId.Text;
            if (ddlDivision.SelectedIndex > 0)
            {
                employeeBO.DivisionId = Convert.ToInt32(ddlDivision.SelectedValue);
            }
            if (hfddlDistrictId.Value != "")
            {
                if (Convert.ToInt32(hfddlDistrictId.Value) > 0)
                {
                    employeeBO.DistrictId = Convert.ToInt32(hfddlDistrictId.Value);
                }
            }
            if (hfddlThanaId.Value != "")
            {
                if (Convert.ToInt32(hfddlThanaId.Value) > 0)
                {
                    employeeBO.ThanaId = Convert.ToInt32(hfddlThanaId.Value);
                }
            }
            if (ddlCostCenter.SelectedIndex > 0)
            {
                employeeBO.CostCenterId = Convert.ToInt32(ddlCostCenter.SelectedValue);
            }
            employeeBO.PassportNumber = txtPassportNumber.Text;
            employeeBO.PIssuePlace = txtPIssuePlace.Text;
            if (!string.IsNullOrWhiteSpace(txtPIssueDate.Text))
            {
                employeeBO.PIssueDate = hmUtility.GetDateTimeFromString(txtPIssueDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                employeeBO.PIssueDate = null;
            }
            if (!string.IsNullOrWhiteSpace(txtPExpireDate.Text))
            {
                employeeBO.PExpireDate = hmUtility.GetDateTimeFromString(txtPExpireDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                employeeBO.PExpireDate = null;
            }
            //employeeBO.CurrentLocationId = Convert.ToInt32(ddlCurrentLocation.SelectedValue);
            employeeBO.PresentAddress = txtPresentAddress.Text;
            employeeBO.PresentCity = txtPresentCity.Text;
            employeeBO.PresentZipCode = txtPresentZipCode.Text;
            employeeBO.PresentCountry = txtPresentCountry.Text;
            employeeBO.PresentPhone = txtPresentPhone.Text;
            employeeBO.PermanentAddress = txtPermanentAddress.Text;
            employeeBO.PermanentCity = txtPermanentCity.Text;
            employeeBO.PermanentZipCode = txtPermanentZipCode.Text;
            employeeBO.PermanentCountry = txtPermanentCountry.Text;
            employeeBO.PermanentPhone = txtPermanentPhone.Text;
            employeeBO.PersonalEmail = txtPersonalEmail.Text;
            employeeBO.AlternativeEmail = txtAlternativeEmail.Text;

            if (ddlWorkStation.SelectedValue != string.Empty)
                employeeBO.WorkStationId = Convert.ToInt32(ddlWorkStation.SelectedValue);

            if (ddlDonor.SelectedValue != string.Empty)
                employeeBO.DonorId = Convert.ToInt32(ddlDonor.SelectedValue);

            employeeBO.EmergencyContactName = txtEmergencyContactName.Text;
            employeeBO.EmergencyContactRelationship = txtEmergencyContactRelationship.Text;
            employeeBO.EmergencyContactNumber = txtEmergencyContactNumber.Text;
            employeeBO.EmergencyContactNumberHome = txtEmergencyContactNumberHome.Text;
            employeeBO.EmergencyContactEmail = txtEmergencyContactEmail.Text;
            employeeBO.ActivityCode = txtActivityCode.Text;
            employeeBO.IsApplicant = false;

            //Bank Info
            EmpBankInfoBO bankInfo = new EmpBankInfoBO();
            if (Convert.ToInt32(ddlBank.SelectedValue) > 0)
            {
                bankInfo.BankId = Convert.ToInt32(ddlBank.SelectedValue);
                bankInfo.BranchName = txtBranchName.Text;
                bankInfo.AccountName = txtAccountName.Text;
                bankInfo.AccountNumber = txtAccountNumber.Text;
                bankInfo.AccountType = txtAccountType.Text;
                bankInfo.CardNumber = txtCardNumber.Text;
                bankInfo.BankRemarks = txtRemarksForBankInfo.Text;
            }
            else
            {
                bankInfo.BankId = Convert.ToInt32(ddlBank.SelectedValue);
            }

            //Letter Information
            employeeBO.AppoinmentLetter = txtAppoinmentLetter.Text;
            employeeBO.JoiningAgreement = txtJoiningAgreement.Text;
            employeeBO.ServiceBond = txtServiceBond.Text;
            employeeBO.DSOAC = txtDSOAC.Text;
            employeeBO.ConfirmationLetter = txtConfirmationLetter.Text;

            //Bnefit Info
            List<PayrollEmpBenefitBO> benefitList = new List<PayrollEmpBenefitBO>();
            int rows = gvBenefit.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                CheckBox cb = (CheckBox)gvBenefit.Rows[i].FindControl("chkIsSavePermission");
                if (cb.Checked == true)
                {
                    PayrollEmpBenefitBO benefitBO = new PayrollEmpBenefitBO();
                    Label lblBenefitHeadId = (Label)gvBenefit.Rows[i].FindControl("lblBenefitHeadId");
                    TextBox txtBenefitEffectiveDate = (TextBox)gvBenefit.Rows[i].FindControl("txtBenefitEffectiveDate");
                    benefitBO.BenefitHeadId = Int64.Parse(lblBenefitHeadId.Text);
                    benefitBO.EffectiveDate = CommonHelper.DateTimeToMMDDYYYY(txtBenefitEffectiveDate.Text);
                    benefitList.Add(benefitBO);
                }
            }

            employeeBO.ResignationDate = null;
            employeeBO.InitialContractEndDate = null;

            if (!string.IsNullOrEmpty(txtContractEndDate.Text))
            {
                employeeBO.InitialContractEndDate = hmUtility.GetDateTimeFromString(txtContractEndDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

                List<EmpTypeBO> empTypebo = new List<EmpTypeBO>();
                empTypebo = JsonConvert.DeserializeObject<List<EmpTypeBO>>(hfEmployeeType.Value);
                var emptyp = (from et in empTypebo where et.TypeId == employeeBO.EmpTypeId && et.IsContractualType == true select et).FirstOrDefault();

                if (emptyp != null)
                {
                    employeeBO.ResignationDate = employeeBO.InitialContractEndDate;
                }
            }

            if (btnSave.Text.Equals("Save"))
            {
                int tmpEmployeeId = 0; string empCode = string.Empty;
                employeeBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = employeeDA.SaveEmployeeInfo(employeeBO, out tmpEmployeeId, out empCode, Session["EmpEducationList"] as List<EmpEducationBO>, Session["EmpExperienceList"] as List<EmpExperienceBO>, Session["EmpDependentList"] as List<EmpDependentBO>, bankInfo, Session["EmpNomineeList"] as List<EmpNomineeBO>, null, null, null, Session["EmpReferencePersonsList"] as List<EmpReferenceBO>, benefitList);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                       EntityTypeEnum.EntityType.Employee.ToString(), employeeBO.EmpId,
                       ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Employee));

                    LoadGridView();
                    Cancel();


                }
            }
            else
            {
                string Id = Session["_EmployeeId"].ToString();
                employeeBO.EmpId = Convert.ToInt32(Id);
                employeeBO.LastModifiedBy = userInformationBO.UserInfoId;
                employeeBO.IsApplicantRecruitment = false;
                bankInfo.EmpId = Convert.ToInt32(Id);
                if (Session["_BankInfoId"] != null)
                    bankInfo.BankInfoId = Convert.ToInt32(Session["_BankInfoId"].ToString());

                //Benefit Item
                BenefitDA empBenefitDA = new BenefitDA();
                List<PayrollEmpBenefitBO> alrSavedBenefitList = new List<PayrollEmpBenefitBO>();
                alrSavedBenefitList = empBenefitDA.GetEmpBenefitListByEmpId(employeeBO.EmpId);

                List<PayrollEmpBenefitBO> deletedBenefitList = new List<PayrollEmpBenefitBO>();
                List<PayrollEmpBenefitBO> newlyAddedBenefitList = new List<PayrollEmpBenefitBO>();

                foreach (PayrollEmpBenefitBO benefitBO in benefitList)
                {
                    var v = (from c in alrSavedBenefitList where c.BenefitHeadId == benefitBO.BenefitHeadId select c).FirstOrDefault();
                    if (v == null)
                    {
                        PayrollEmpBenefitBO bo = new PayrollEmpBenefitBO();
                        bo.BenefitHeadId = benefitBO.BenefitHeadId;
                        bo.EffectiveDate = benefitBO.EffectiveDate;
                        newlyAddedBenefitList.Add(bo);
                    }
                }
                foreach (PayrollEmpBenefitBO benefitBO in alrSavedBenefitList)
                {
                    var v = (from c in benefitList where c.BenefitHeadId == benefitBO.BenefitHeadId select c).FirstOrDefault();

                    if (v == null)
                    {
                        PayrollEmpBenefitBO bo = new PayrollEmpBenefitBO();
                        bo.BenefitHeadId = benefitBO.BenefitHeadId;
                        bo.EmpBenefitMappingId = benefitBO.EmpBenefitMappingId;
                        deletedBenefitList.Add(bo);
                    }
                }

                Boolean status = employeeDA.UpdateEmployeeInfo(employeeBO, Session["EmpEducationList"] as List<EmpEducationBO>, Session["arrayEducationDelete"] as ArrayList, Session["EmpExperienceList"] as List<EmpExperienceBO>, Session["arrayExperienceDelete"] as ArrayList, Session["EmpDependentList"] as List<EmpDependentBO>, Session["arrayDependentDelete"] as ArrayList, bankInfo, Session["EmpNomineeList"] as List<EmpNomineeBO>, Session["arrayNomineeDelete"] as ArrayList, null, null, null, null, null, Session["EmpReferencePersonsList"] as List<EmpReferenceBO>, Session["arrayReferencePersonDelete"] as ArrayList, newlyAddedBenefitList, deletedBenefitList);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                       EntityTypeEnum.EntityType.Employee.ToString(), employeeBO.EmpId,
                       ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Employee));

                    LoadGridView();
                    Cancel();

                }
            }
            SetTab("EmployeeTab");

        }
        //************************ User Defined Function ********************//
        /*private void LoadGLCompany()
        {
            hfIsSingle.Value = "0";
            GLCompanyDA entityDA = new GLCompanyDA();
            var List = entityDA.GetAllGLCompanyInfo();
            //hfCompanyAll.Value = JsonConvert.SerializeObject(List);

            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            //companyList.Add(List[0]);
            if (List.Count == 1)
            {
                companyList.Add(List[0]);
                ddlGLCompany.DataSource = companyList;
                ddlGLCompany.DataTextField = "Name";
                ddlGLCompany.DataValueField = "CompanyId";
                ddlGLCompany.DataBind();
                hfIsSingle.Value = "1";

            }
            else
            {
                ddlGLCompany.DataSource = List;
                ddlGLCompany.DataTextField = "Name";
                ddlGLCompany.DataValueField = "CompanyId";
                ddlGLCompany.DataBind();
                hfIsSingle.Value = "0";
                ListItem itemCompany = new ListItem();
                itemCompany.Value = "0";
                itemCompany.Text = hmUtility.GetDropDownFirstValue();
                ddlGLCompany.Items.Insert(0, itemCompany);
            }


        }*/
        private void LoadExtraShowHideInformation()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO isPayrollWorkStationHideBO = new HMCommonSetupBO();
            isPayrollWorkStationHideBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollWorkStationHide", "IsPayrollWorkStationHide");
            if (isPayrollWorkStationHideBO != null)
            {
                if (isPayrollWorkStationHideBO.SetupValue == "1")
                {
                    PayrollWorkStationDiv.Visible = false;
                }
            }

            HMCommonSetupBO isPayrollDonorNameAndActivityCodeHideBO = new HMCommonSetupBO();
            isPayrollDonorNameAndActivityCodeHideBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollDonorNameAndActivityCodeHide", "IsPayrollDonorNameAndActivityCodeHide");
            if (isPayrollDonorNameAndActivityCodeHideBO != null)
            {
                if (isPayrollDonorNameAndActivityCodeHideBO.SetupValue == "1")
                {
                    PayrollDonorNameAndActivityCodeHideDiv.Visible = false;
                }
            }

            HMCommonSetupBO isPayrollDependentHideBO = new HMCommonSetupBO();
            isPayrollDependentHideBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollDependentHide", "IsPayrollDependentHide");
            if (isPayrollDependentHideBO != null)
            {
                if (isPayrollDependentHideBO.SetupValue == "1")
                {
                    hfIsPayrollDependentHide.Value = "1";
                }
            }

            HMCommonSetupBO isPayrollBeneficiaryHideBO = new HMCommonSetupBO();
            isPayrollBeneficiaryHideBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollBeneficiaryHide", "IsPayrollBeneficiaryHide");
            if (isPayrollBeneficiaryHideBO != null)
            {
                if (isPayrollBeneficiaryHideBO.SetupValue == "1")
                {
                    hfIsPayrollBeneficiaryHide.Value = "1";
                }
            }

            HMCommonSetupBO isPayrollReferenceHideBO = new HMCommonSetupBO();
            isPayrollReferenceHideBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollReferenceHide", "IsPayrollReferenceHide");
            if (isPayrollReferenceHideBO != null)
            {
                if (isPayrollReferenceHideBO.SetupValue == "1")
                {
                    hfIsPayrollReferenceHide.Value = "1";
                }
            }

            HMCommonSetupBO isPayrollBenefitsHideBO = new HMCommonSetupBO();
            isPayrollBenefitsHideBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollBenefitsHide", "IsPayrollBenefitsHide");
            if (isPayrollBenefitsHideBO != null)
            {
                if (isPayrollBenefitsHideBO.SetupValue == "1")
                {
                    hfIsPayrollBenefitsHide.Value = "1";
                }
            }

            HMCommonSetupBO isPayrollLetterPanelHideBO = new HMCommonSetupBO();
            isPayrollLetterPanelHideBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollLetterPanelHide", "IsPayrollLetterPanelHide");
            if (isPayrollLetterPanelHideBO != null)
            {
                if (isPayrollLetterPanelHideBO.SetupValue == "1")
                {
                    hfIsPayrollLetterPanelHide.Value = "1";
                }
            }

            HMCommonSetupBO isProvidentFundDeductHideBO = new HMCommonSetupBO();
            isProvidentFundDeductHideBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollProvidentFundDeductHide", "IsPayrollProvidentFundDeductHide");
            if (isProvidentFundDeductHideBO != null)
            {
                if (isProvidentFundDeductHideBO.SetupValue == "1")
                {
                    IsProvidentFundDeductHideDiv.Visible = false;
                }
            }

            HMCommonSetupBO isPayrollCostCenterDivHideBO = new HMCommonSetupBO();
            isPayrollCostCenterDivHideBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollCostCenterDivHide", "IsPayrollCostCenterDivHide");
            if (isPayrollCostCenterDivHideBO != null)
            {
                if (isPayrollCostCenterDivHideBO.SetupValue == "1")
                {
                    CostCenterDiv.Visible = false;
                }
            }
        }
        private void LoadConversionHead()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            int localCurrencyId = 1;
            List<CommonCurrencyBO> CommonCurrencyBOForLocal = new List<CommonCurrencyBO>();
            CommonCurrencyBOForLocal = headDA.GetConversionHeadInfoByType("Local");
            if (CommonCurrencyBOForLocal != null)
            {
                if (CommonCurrencyBOForLocal.Count > 0)
                {
                    localCurrencyId = CommonCurrencyBOForLocal[0].CurrencyId;
                    hflocalCurrencyId.Value = localCurrencyId.ToString();
                }
            }

            this.ddlPayrollCurrencyId.DataSource = headDA.GetConversionHeadInfoByType("All");
            this.ddlPayrollCurrencyId.DataTextField = "CurrencyName";
            this.ddlPayrollCurrencyId.DataValueField = "CurrencyId";
            this.ddlPayrollCurrencyId.DataBind();
            this.ddlPayrollCurrencyId.SelectedValue = localCurrencyId.ToString();
        }
        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "LoadDetailGridInformation", jscript, true);
            //flashUploadSignature.QueryParameters = "employeeId=" + Server.UrlEncode(RandomEmpId.Value) + "-Employee Signature";
            //flashUploadDocuments.QueryParameters = "employeeId=" + Server.UrlEncode(RandomEmpId.Value) + "-Employee Document";
            //flashUpload.QueryParameters = "employeeId=" + Server.UrlEncode(RandomEmpId.Value) + "-Employee Other Documents";
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadCountry()
        {
            HMCommonDA commonDA = new HMCommonDA();
            var List = commonDA.GetAllCountries();
            ddlCountryId.DataSource = List;
            ddlCountryId.DataTextField = "CountryName";
            ddlCountryId.DataValueField = "CountryId";
            ddlCountryId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCountryId.Items.Insert(0, item);
        }
        private HMCommonSetupBO LoadCompanyCountry()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("CompanyCountryId", "CompanyCountryId");
            return commonSetupBO;
        }
        private void LoadGrade()
        {
            EmpGradeDA gradeDA = new EmpGradeDA();
            var List = gradeDA.GetActiveGradeInfo(); ;
            ddlGradeId.DataSource = List;
            ddlGradeId.DataTextField = "Name";
            ddlGradeId.DataValueField = "GradeId";
            ddlGradeId.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlGradeId.Items.Insert(0, item);
        }
        private void LoadMaritualStatus()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("MaritualStatus", hmUtility.GetDropDownFirstValue());
            ddlMaritalStatus.DataSource = fields;
            ddlMaritalStatus.DataTextField = "FieldValue";
            ddlMaritalStatus.DataValueField = "FieldValue";
            ddlMaritalStatus.DataBind();
        }
        private void LoadGender()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("Gender", hmUtility.GetDropDownFirstValue());

            ddlGender.DataSource = fields;
            ddlGender.DataTextField = "FieldValue";
            ddlGender.DataValueField = "FieldValue";
            ddlGender.DataBind();
        }
        private void LoadRelegion()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("Relegion", hmUtility.GetDropDownFirstValue());

            ddlReligion.DataSource = fields;
            ddlReligion.DataTextField = "FieldValue";
            ddlReligion.DataValueField = "FieldValue";
            ddlReligion.DataBind();
        }
        private void LoadBloodGroup()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("BloodGroup", hmUtility.GetDropDownFirstValue());

            ddlBloodGroup.DataSource = fields;
            ddlBloodGroup.DataTextField = "FieldValue";
            ddlBloodGroup.DataValueField = "FieldValue";
            ddlBloodGroup.DataBind();
        }
        private void LoadBloodGroupDepen()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("BloodGroup");

            ddlBloodGroupDepen.DataSource = fields;
            ddlBloodGroupDepen.DataTextField = "FieldValue";
            ddlBloodGroupDepen.DataValueField = "FieldId";
            ddlBloodGroupDepen.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlBloodGroupDepen.Items.Insert(0, item);
        }
        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            ddlDepartmentId.DataSource = entityDA.GetDepartmentInfo();
            ddlDepartmentId.DataTextField = "Name";
            ddlDepartmentId.DataValueField = "DepartmentId";
            ddlDepartmentId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlDepartmentId.Items.Insert(0, item);
        }
        private void LoadEmployeeType()
        {
            EmpTypeDA entityDA = new EmpTypeDA();
            List<EmpTypeBO> boList = new List<EmpTypeBO>();
            boList = entityDA.GetEmpTypeInfo().Where(x => x.ActiveStat == true).ToList();

            ddlEmpCategoryId.DataSource = boList;
            ddlEmpCategoryId.DataTextField = "Name";
            ddlEmpCategoryId.DataValueField = "TypeId";
            ddlEmpCategoryId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlEmpCategoryId.Items.Insert(0, item);

            hfEmployeeType.Value = JsonConvert.SerializeObject(boList);

        }
        private void LoadReportingTo()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollReportingTo", "PayrollReportingTo");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                if (commonSetupBO.SetupValue == "2")
                {
                    EmployeeDA EmpDA = new EmployeeDA();
                    List<EmployeeBO> EmpBO = new List<EmployeeBO>();
                    EmpBO = EmpDA.GetEmployeeInfo();

                    ddlReportingTo.DataSource = EmpBO;
                    ddlReportingTo.DataTextField = "DisplayName";
                    ddlReportingTo.DataValueField = "EmpId";
                    ddlReportingTo.DataBind();

                    ListItem item = new ListItem();
                    item.Value = "0";
                    item.Text = hmUtility.GetDropDownFirstValue();
                    ddlReportingTo.Items.Insert(0, item);
                }
                else
                {
                    DesignationDA entityDA = new DesignationDA();
                    ddlReportingTo.DataSource = entityDA.GetDesignationInfo();
                    ddlReportingTo.DataTextField = "Name";
                    ddlReportingTo.DataValueField = "DesignationId";
                    ddlReportingTo.DataBind();

                    ListItem item = new ListItem();
                    item.Value = "0";
                    item.Text = hmUtility.GetDropDownNoneValue();
                    ddlReportingTo.Items.Insert(0, item);
                }
            }
            else
            {
                DesignationDA entityDA = new DesignationDA();
                ddlReportingTo.DataSource = entityDA.GetDesignationInfo();
                ddlReportingTo.DataTextField = "Name";
                ddlReportingTo.DataValueField = "DesignationId";
                ddlReportingTo.DataBind();

                ListItem item = new ListItem();
                item.Value = "0";
                item.Text = hmUtility.GetDropDownNoneValue();
                ddlReportingTo.Items.Insert(0, item);
            }
        }
        private void LoadReportingTo2()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollReportingTo", "PayrollReportingTo");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                if (commonSetupBO.SetupValue == "2")
                {
                    EmployeeDA EmpDA = new EmployeeDA();
                    List<EmployeeBO> EmpBO = new List<EmployeeBO>();
                    EmpBO = EmpDA.GetEmployeeInfo();

                    ddlReportingTo2.DataSource = EmpBO;
                    ddlReportingTo2.DataTextField = "DisplayName";
                    ddlReportingTo2.DataValueField = "EmpId";
                    ddlReportingTo2.DataBind();

                    ListItem item = new ListItem();
                    item.Value = "0";
                    item.Text = hmUtility.GetDropDownFirstValue();
                    ddlReportingTo2.Items.Insert(0, item);
                }
                else
                {
                    DesignationDA entityDA = new DesignationDA();
                    ddlReportingTo2.DataSource = entityDA.GetDesignationInfo();
                    ddlReportingTo2.DataTextField = "Name";
                    ddlReportingTo2.DataValueField = "DesignationId";
                    ddlReportingTo2.DataBind();

                    ListItem item = new ListItem();
                    item.Value = "0";
                    item.Text = hmUtility.GetDropDownNoneValue();
                    ddlReportingTo2.Items.Insert(0, item);
                }
            }
            else
            {
                DesignationDA entityDA = new DesignationDA();
                ddlReportingTo2.DataSource = entityDA.GetDesignationInfo();
                ddlReportingTo2.DataTextField = "Name";
                ddlReportingTo2.DataValueField = "DesignationId";
                ddlReportingTo2.DataBind();

                ListItem item = new ListItem();
                item.Value = "0";
                item.Text = hmUtility.GetDropDownNoneValue();
                ddlReportingTo2.Items.Insert(0, item);
            }
        }
        private void LoadDesignation()
        {
            DesignationDA entityDA = new DesignationDA();
            ddlDesignationId.DataSource = entityDA.GetActiveDesignationInfo();
            ddlDesignationId.DataTextField = "Name";
            ddlDesignationId.DataValueField = "DesignationId";
            ddlDesignationId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlDesignationId.Items.Insert(0, item);
        }
        private void LoadBank()
        {
            BankDA bankDA = new BankDA();
            List<BankBO> entityBOList = new List<BankBO>();
            entityBOList = bankDA.GetBankInfo().Where(x => x.ActiveStat == true).ToList();

            ddlBank.DataSource = entityBOList;
            ddlBank.DataTextField = "BankName";
            ddlBank.DataValueField = "BankId";
            ddlBank.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlBank.Items.Insert(0, item);
        }
        private void LoadWorkStation()
        {
            EmployeeDA workStationDA = new EmployeeDA();
            List<EmpWorkStationBO> entityBOList = new List<EmpWorkStationBO>();
            entityBOList = workStationDA.GetEmployWorkStation();

            ddlWorkStation.DataSource = entityBOList;
            ddlWorkStation.DataTextField = "WorkStationName";
            ddlWorkStation.DataValueField = "WorkStationId";
            ddlWorkStation.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlWorkStation.Items.Insert(0, item);
        }
        private void LoadDivision()
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmpDivisionBO> divList = new List<EmpDivisionBO>();
            divList = empDA.GetEmpDivisionList();

            ddlDivision.DataSource = divList;
            ddlDivision.DataTextField = "DivisionName";
            ddlDivision.DataValueField = "DivisionId";
            ddlDivision.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlDivision.Items.Insert(0, item);
        }
        public void LoadCostCenter()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            var List = costCentreTabDA.GetCostCentreTabInfo();
            ddlCostCenter.DataSource = List;
            ddlCostCenter.DataTextField = "CostCenter";
            ddlCostCenter.DataValueField = "CostCenterId";
            ddlCostCenter.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCostCenter.Items.Insert(0, item);
        }
        public void LoadEmployeeStatus()
        {
            EmployeeDA costCentreTabDA = new EmployeeDA();
            var List = costCentreTabDA.GetEmployeeStatus();
            ddlEmployeeStatus.DataSource = List;
            ddlEmployeeStatus.DataTextField = "EmployeeStatus";
            ddlEmployeeStatus.DataValueField = "EmployeeStatusId";
            ddlEmployeeStatus.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlEmployeeStatus.Items.Insert(0, item);
        }
        private void LoadDistrict(int divisionId)
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmpDistrictBO> disList = new List<EmpDistrictBO>();
            disList = empDA.GetEmpDistrictList(divisionId);

            ddlDistrict.DataSource = disList;
            ddlDistrict.DataTextField = "DistrictName";
            ddlDistrict.DataValueField = "DistrictId";
            ddlDistrict.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlDistrict.Items.Insert(0, item);
        }
        private void LoadThana(int districtId)
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmpThanaBO> thanaList = new List<EmpThanaBO>();
            thanaList = empDA.GetEmpThanaList(districtId);

            ddlThana.DataSource = thanaList;
            ddlThana.DataTextField = "ThanaName";
            ddlThana.DataValueField = "ThanaId";
            ddlThana.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlThana.Items.Insert(0, item);
        }
        private void LoadDonor()
        {
            EmployeeDA empDa = new EmployeeDA();
            List<PayrollDonorBO> donor = new List<PayrollDonorBO>();
            donor = empDa.GetDonor();

            ddlDonor.DataSource = donor;
            ddlDonor.DataTextField = "DonorName";
            ddlDonor.DataValueField = "DonorId";
            ddlDonor.DataBind();

            ddlDonor.Items.Insert(0, new ListItem { Text = hmUtility.GetDropDownFirstValue(), Value = string.Empty });
        }
        private void LoadEducationLevel()
        {
            EmployeeDA empDa = new EmployeeDA();
            List<EmpEducationLevelBO> boList = new List<EmpEducationLevelBO>();
            boList = empDa.GetEducationLevel();

            ddlExamLevel.DataSource = boList;
            ddlExamLevel.DataTextField = "LevelName";
            ddlExamLevel.DataValueField = "LevelId";
            ddlExamLevel.DataBind();

            ddlExamLevel.Items.Insert(0, new ListItem { Text = hmUtility.GetDropDownFirstValue(), Value = string.Empty });
        }
        private void LoadBenefitList()
        {
            BenefitDA entityDA = new BenefitDA();
            List<BenefitHeadBO> lists = entityDA.GetAllBenefit();
            gvBenefit.DataSource = lists;
            gvBenefit.DataBind();
        }
        private void TabPermissionList()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsEmployeeBasicSetUpOnly", "IsEmployeeBasicSetUpOnly");

            if (commonSetupBO.SetupValue == "1")
            {
                hfIsEmployeeBasicSetUp.Value = commonSetupBO.SetupValue;
            }
            else
            {
                hfIsEmployeeBasicSetUp.Value = "";
            }
        }
        private bool IsFormValid()
        {
            bool status = true;
            var dependentList = Session["EmpDependentList"] as List<EmpDependentBO>;
            var nomineeList = Session["EmpNomineeList"] as List<EmpNomineeBO>;
            var referencePersonsList = Session["EmpReferencePersonsList"] as List<EmpReferenceBO>;
            var experineceList = Session["EmpExperienceList"] as List<EmpExperienceBO>;
            var educationList = Session["EmpEducationList"] as List<EmpEducationBO>;
            var trainingList = Session["EmpCareerTrainingList"] as List<EmpCareerTrainingBO>;
            var referenceList = Session["EmpReferenceList"] as List<EmpReferenceBO>;
            var isSingleCompany = hfIsSingle.Value;

            List<PayrollEmpBenefitBO> benefitList = new List<PayrollEmpBenefitBO>();
            int rows = gvBenefit.Rows.Count;

            //EmployeeTab
            if (string.IsNullOrEmpty(txtEmpCode.Text))
            {
                if (hfIsEmployeeCodeAutoGenerate.Value == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Employee Code.", AlertType.Warning);
                    txtEmpCode.Focus();
                    status = false;
                    SetTab("EmployeeTab");
                }
            }
            else if (string.IsNullOrEmpty(txtFirstName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "First Name.", AlertType.Warning);
                status = false;
                txtFirstName.Focus();
                SetTab("EmployeeTab");
            }
            else if (string.IsNullOrEmpty(txtEmpJoinDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Join Date.", AlertType.Warning);
                status = false;
                txtJoinDate.Focus();
                SetTab("EmployeeTab");
            }
            else if (ddlDepartmentId.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "A Department.", AlertType.Warning);
                status = false;
                ddlDepartmentId.Focus();
                SetTab("EmployeeTab");
            }
            else if (ddlEmpCategoryId.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Employee type.", AlertType.Warning);
                status = false;
                ddlEmpCategoryId.Focus();
                SetTab("EmployeeTab");
            }
            else if (ddlDesignationId.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "your Designation.", AlertType.Warning);
                status = false;
                ddlDesignationId.Focus();
                SetTab("EmployeeTab");
            }
            //else if (ddlGradeId.SelectedIndex == 0)
            //{
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Your Grade.", AlertType.Warning);
            //    status = false;
            //    SetTab("EmployeeTab");
            //}
            else if (ddlGender.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "your Gender.", AlertType.Warning);
                status = false;
                ddlGender.Focus();
                SetTab("DetailsTab");
            }
            else if (ddlReligion.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "your Religion.", AlertType.Warning);
                status = false;
                ddlReligion.Focus();
                SetTab("DetailsTab");
            }
            else if (ddlMaritalStatus.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "your Marital Status.", AlertType.Warning);
                status = false;
                ddlMaritalStatus.Focus();
                SetTab("DetailsTab");
            }
            else if (ddlCountryId.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Your Nationality.", AlertType.Warning);
                status = false;
                SetTab("DetailsTab");
            }
            else if (ddlEmployeeStatus.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Employee Status.", AlertType.Warning);
                status = false;
                ddlEmployeeStatus.Focus();
                SetTab("EmployeeTab");
            }
            if (rows > 0)
            {
                for (int i = 0; i < rows; i++)
                {
                    CheckBox cb = (CheckBox)gvBenefit.Rows[i].FindControl("chkIsSavePermission");
                    if (cb.Checked == true)
                    {
                        PayrollEmpBenefitBO benefitBO = new PayrollEmpBenefitBO();
                        Label lblBenefitHeadId = (Label)gvBenefit.Rows[i].FindControl("lblBenefitHeadId");
                        TextBox txtBenefitEffectiveDate = (TextBox)gvBenefit.Rows[i].FindControl("txtBenefitEffectiveDate");
                        if (String.IsNullOrEmpty(txtBenefitEffectiveDate.Text))
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Selected Benefit's Effective From Date.", AlertType.Warning);
                            status = false;
                            gvBenefit.Rows[i].FindControl("txtBenefitEffectiveDate").Focus();
                            SetTab("BenefitTab");
                        }
                    }
                }
            }
            /*if (isSingleCompany == "0")
            {
                if (Convert.ToInt32(ddlGLCompany.SelectedValue) == 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Company.", AlertType.Warning);
                    status = false;
                    ddlGLCompany.Focus();
                    SetTab("EmployeeTab");
                }                
            }*/
            if (ddlEmpCategoryId.SelectedValue == "2")
            {
                if (String.IsNullOrEmpty(txtContractEndDate.Text))
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Contract End Date.", AlertType.Warning);
                    status = false;
                    txtContractEndDate.Focus();
                    SetTab("EmployeeTab");
                }
            }

            return status;
        }
        private bool IsDependentValid()
        {
            bool status = true;
            if (string.IsNullOrEmpty(txtDependentName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Dependent Name.", AlertType.Warning);
                txtDependentName.Focus();
                status = false;
            }
            else if (string.IsNullOrEmpty(txtRelationship.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Relationship.", AlertType.Warning);
                txtRelationship.Focus();
                status = false;
            }

            SetTab("DependentTab");
            return status;
        }
        private bool IsNomineeValid()
        {
            bool status = true;
            if (string.IsNullOrEmpty(txtNomineeName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Beneficiary Name.", AlertType.Warning);
                txtDependentName.Focus();
                status = false;
            }
            else if (string.IsNullOrEmpty(txtNomineeRelationship.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Relationship.", AlertType.Warning);
                txtRelationship.Focus();
                status = false;
            }
            else if (string.IsNullOrEmpty(txtNomineeDateOfBirth.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Date of Birth.", AlertType.Warning);
                txtRelationship.Focus();
                status = false;
            }
            else if (string.IsNullOrEmpty(txtPercentage.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Percentage.", AlertType.Warning);
                txtRelationship.Focus();
                status = false;
            }

            SetTab("NomineeTab");
            return status;
        }
        private bool IsEducationValid()
        {
            bool status = true;
            if (ddlExamLevel.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Exam Level.", AlertType.Warning);
                status = false;
                ddlExamLevel.Focus();
            }
            else if (string.IsNullOrEmpty(txtExamName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Examination Name.", AlertType.Warning);
                status = false;
                txtExamName.Focus();
            }
            else if (string.IsNullOrEmpty(txtInstituteName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Institute Name.", AlertType.Warning);
                txtInstituteName.Focus();
                status = false;
            }
            else if (string.IsNullOrEmpty(txtPassYear.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Passing Year.", AlertType.Warning);
                status = false;
                txtPassYear.Focus();
            }
            else if (string.IsNullOrEmpty(txtPassClass.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "GPA Or Class.", AlertType.Warning);
                status = false;
                txtPassClass.Focus();
            }
            SetTab("EducationTab");
            return status;
        }
        private bool IsExperienceValid()
        {
            bool status = true;
            if (string.IsNullOrEmpty(txtCompanyName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Company Name.", AlertType.Warning);
                status = false;
                txtCompanyName.Focus();
            }
            else if (string.IsNullOrEmpty(txtJoinDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Join Date.", AlertType.Warning);
                status = false;
                txtJoinDate.Focus();
            }
            else if (string.IsNullOrEmpty(txtLeaveDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Leave Date.", AlertType.Warning);
                status = false;
                txtLeaveDate.Focus();
            }
            SetTab("ExperienceTab");
            return status;
        }
        private void ClearNominee()
        {
            btnAddNominee.Text = "Add";
            txtNomineeName.Text = string.Empty;
            txtRelationship.Text = string.Empty;
            txtNomineeDateOfBirth.Text = string.Empty;
            txtNomineeAge.Text = string.Empty;
            hfNomineeAge.Value = string.Empty;
            txtPercentage.Text = string.Empty;
            txtNomineeRelationship.Text = "";
        }
        private void SetTab(string TabName)
        {
            if (TabName == "EmployeeTab") //#tab-1
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
                H.Attributes.Add("class", "ui-state-default ui-corner-top");
                I.Attributes.Add("class", "ui-state-default ui-corner-top");
                J.Attributes.Add("class", "ui-state-default ui-corner-top");
                K.Attributes.Add("class", "ui-state-default ui-corner-top");
                //L.Attributes.Add("class", "ui-state-default ui-corner-top");
                M.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "DetailsTab") //#tab-2
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
                H.Attributes.Add("class", "ui-state-default ui-corner-top");
                I.Attributes.Add("class", "ui-state-default ui-corner-top");
                J.Attributes.Add("class", "ui-state-default ui-corner-top");
                K.Attributes.Add("class", "ui-state-default ui-corner-top");
                //L.Attributes.Add("class", "ui-state-default ui-corner-top");
                M.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "EducationTab") //#tab-3
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
                H.Attributes.Add("class", "ui-state-default ui-corner-top");
                I.Attributes.Add("class", "ui-state-default ui-corner-top");
                J.Attributes.Add("class", "ui-state-default ui-corner-top");
                K.Attributes.Add("class", "ui-state-default ui-corner-top");
                //L.Attributes.Add("class", "ui-state-default ui-corner-top");
                M.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "ExperienceTab") //#tab-4
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
                H.Attributes.Add("class", "ui-state-default ui-corner-top");
                I.Attributes.Add("class", "ui-state-default ui-corner-top");
                J.Attributes.Add("class", "ui-state-default ui-corner-top");
                K.Attributes.Add("class", "ui-state-default ui-corner-top");
                //L.Attributes.Add("class", "ui-state-default ui-corner-top");
                M.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "DependentTab") //#tab-5
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
                H.Attributes.Add("class", "ui-state-default ui-corner-top");
                I.Attributes.Add("class", "ui-state-default ui-corner-top");
                J.Attributes.Add("class", "ui-state-default ui-corner-top");
                K.Attributes.Add("class", "ui-state-default ui-corner-top");
                //L.Attributes.Add("class", "ui-state-default ui-corner-top");
                M.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "BeneficiaryTab") //#tab-6
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
                H.Attributes.Add("class", "ui-state-default ui-corner-top");
                I.Attributes.Add("class", "ui-state-default ui-corner-top");
                J.Attributes.Add("class", "ui-state-default ui-corner-top");
                K.Attributes.Add("class", "ui-state-default ui-corner-top");
                //L.Attributes.Add("class", "ui-state-default ui-corner-top");
                M.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "ReferenceTab") //#tab-7
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                H.Attributes.Add("class", "ui-state-default ui-corner-top");
                I.Attributes.Add("class", "ui-state-default ui-corner-top");
                J.Attributes.Add("class", "ui-state-default ui-corner-top");
                K.Attributes.Add("class", "ui-state-default ui-corner-top");
                //L.Attributes.Add("class", "ui-state-default ui-corner-top");
                M.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "BenefitTab") //#tab-8
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
                H.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                I.Attributes.Add("class", "ui-state-default ui-corner-top");
                J.Attributes.Add("class", "ui-state-default ui-corner-top");
                K.Attributes.Add("class", "ui-state-default ui-corner-top");
                //L.Attributes.Add("class", "ui-state-default ui-corner-top");
                M.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "BankInfoTab") //#tab-9
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
                H.Attributes.Add("class", "ui-state-default ui-corner-top");
                I.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                J.Attributes.Add("class", "ui-state-default ui-corner-top");
                K.Attributes.Add("class", "ui-state-default ui-corner-top");
                //L.Attributes.Add("class", "ui-state-default ui-corner-top");
                M.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "LetterTab") //#tab-10
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
                H.Attributes.Add("class", "ui-state-default ui-corner-top");
                I.Attributes.Add("class", "ui-state-default ui-corner-top");
                J.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                K.Attributes.Add("class", "ui-state-default ui-corner-top");
                //L.Attributes.Add("class", "ui-state-default ui-corner-top");
                M.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "GridTab") //#tab-13
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
                H.Attributes.Add("class", "ui-state-default ui-corner-top");
                I.Attributes.Add("class", "ui-state-default ui-corner-top");
                J.Attributes.Add("class", "ui-state-default ui-corner-top");
                K.Attributes.Add("class", "ui-state-default ui-corner-top");
                //L.Attributes.Add("class", "ui-state-default ui-corner-top");
                M.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
            }
        }
        private void LoadGridView()
        {
            EmployeeDA da = new EmployeeDA();
            List<EmployeeBO> files = da.GetEmployeeInfo();
            var ds = files.OrderByDescending(x => x.EmpId).ToList();
        }
        private void FillUploadedFile(int EmployeeId)
        {
            DocumentsDA docDA = new DocumentsDA();
            List<DocumentsBO> docBO = new List<DocumentsBO>();
            docBO = docDA.GetDocumentsInfoByDocCategoryAndOwnerId("Employee Signature", EmployeeId);
            Session["_EmployeeDocuments"] = docBO;
            docBO = docDA.GetDocumentsInfoByDocCategoryAndOwnerId("Employee Document", EmployeeId);
            Session["_EmployeeDocuments"] = docBO;
            docBO = docDA.GetDocumentsInfoByDocCategoryAndOwnerId("Employee Other Documents", EmployeeId);
            Session["_EmployeeDocuments"] = docBO;
        }

        [WebMethod]
        public static List<GLProjectBO> LoadProjectByCompanyId(int companyId)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            HMUtility hmUtility = new HMUtility();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GLProjectDA entityDA = new GLProjectDA();
            List<GLProjectBO> ProjectListBo = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(companyId, Convert.ToInt32(userInformationBO.UserGroupId)).Where(x => x.IsFinalStage == false).ToList();
            return ProjectListBo;
        }
        private void FillForm(int EditId)
        {
            ddlReportingTo.DataSource = null;
            ddlReportingTo.DataBind();

            ddlReportingTo2.DataSource = null;
            ddlReportingTo2.DataBind();

            /*ddlGLCompany.DataSource = null;
            ddlGLCompany.DataBind();*/

            LoadReportingTo();
            LoadReportingTo2();
            //LoadGLCompany();
            LoadBloodGroupDepen();
            //Master Information------------------------
            EmployeeBO bo = new EmployeeBO();
            EmployeeDA da = new EmployeeDA();
            bo = da.GetEmployeeInfoById(EditId);
            Session["_EmployeeId"] = bo.EmpId;
            RandomEmpId.Value = bo.EmpId.ToString();

            this._CompanyId = bo.GlCompanyId;

            this._ProjectId = bo.GlProjectId;

            hfGLCompanyId.Value = "" + bo.GlCompanyId;
            hfGLProjectId.Value = "" + bo.GlProjectId;
            Session["Employee_GlCompanyId"] = bo.GlCompanyId;
            Session["Employee_GlProjectId"] = bo.GlProjectId;
            ddlEmpCompanyId.SelectedValue = bo.EmpCompanyId.ToString();


            tempEmpId.Value = bo.EmpId.ToString();
            FileUpload();
            LoadDistrict(bo.DivisionId);
            LoadThana(bo.DistrictId);
            txtEmpCode.Text = bo.EmpCode;
            hfEmpCode.Value = bo.EmpCode;
            ddlIsProvidentFundDeduct.SelectedValue = (bo.IsProvidentFundDeduct ? "1" : "0");
            ddlEmployeeStatus.SelectedValue = bo.EmployeeStatusId.ToString();
            ddlPayrollCurrencyId.SelectedValue = bo.PayrollCurrencyId.ToString();
            ddlNotEffectOnHead.SelectedValue = bo.NotEffectOnHead.ToString();
            ddlTitle.SelectedValue = bo.Title;
            txtDisplayName.Text = bo.DisplayName;
            txtFirstName.Text = bo.FirstName;
            txtLastName.Text = bo.LastName;
            ddlDepartmentId.SelectedValue = bo.DepartmentId.ToString();
            ddlEmpCategoryId.SelectedValue = bo.EmpTypeId.ToString();
            ddlDesignationId.SelectedValue = bo.DesignationId.ToString();
            ddlGradeId.SelectedValue = bo.GradeId.ToString();
            txtOfficialEmail.Text = bo.OfficialEmail;
            //txtReferenceBy.Text = bo.ReferenceBy;
            txtTinNumber.Text = bo.TinNumber;
            txtRemarks.Text = bo.Remarks;
            txtFathersName.Text = bo.FathersName;
            txtMothersName.Text = bo.MothersName;

            txtAppoinmentLetter.Text = bo.AppoinmentLetter;
            txtJoiningAgreement.Text = bo.JoiningAgreement;
            txtServiceBond.Text = bo.ServiceBond;
            txtDSOAC.Text = bo.DSOAC;
            txtConfirmationLetter.Text = bo.ConfirmationLetter;
            txtConfirmationLetter.Text = bo.ConfirmationLetter;

            if (bo.EmpDateOfBirth != null)
            {
                txtEmpDateOfBirth.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(bo.EmpDateOfBirth));
            }
            else
            {
                txtEmpDateOfBirth.Text = string.Empty;
            }

            if (bo.EmpDateOfMarriage != null)
            {
                txtDateOfMarriage.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(bo.EmpDateOfMarriage));
            }
            else
            {
                txtDateOfMarriage.Text = string.Empty;
            }
            if (bo.IsContractualType)
            {
                //txtContractEndDate.Enabled = true;
                txtContractEndDate.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(bo.InitialContractEndDate));
            }
            else
            {
                //txtContractEndDate.Enabled = false;
            }
            //txtEmpDateOfBirth.Text = hmUtility.GetStringFromDateTime(bo.EmpDateOfBirth);
            if (!string.IsNullOrWhiteSpace(bo.Gender.ToString()))
            {
                ddlGender.SelectedValue = bo.Gender.ToString();
            }
            else
            {
                ddlGender.SelectedIndex = 0;
            }
            if (bo.ProvisionPeriod != null)
            {
                chkIsProvisionPeriod.Checked = true;
                txtProvisionPeriod.Enabled = true;
                txtProvisionPeriod.Text = hmUtility.GetStringFromDateTime(Convert.ToDateTime(bo.ProvisionPeriod));
            }
            else
            {
                chkIsProvisionPeriod.Checked = false;
                txtProvisionPeriod.Text = string.Empty;
            }

            ddlReligion.SelectedValue = bo.Religion.ToString();
            ddlMaritalStatus.SelectedValue = bo.MaritalStatus.ToString();
            if (!string.IsNullOrWhiteSpace(bo.BloodGroup.ToString()))
            {
                ddlBloodGroup.SelectedValue = bo.BloodGroup.ToString();
            }
            else
            {
                ddlBloodGroup.SelectedIndex = 0;
            }
            if (!string.IsNullOrEmpty(bo.RepotingTo.ToString()))
            {
                ddlReportingTo.SelectedValue = bo.RepotingTo.ToString();
            }
            if (!string.IsNullOrEmpty(bo.RepotingTo2.ToString()))
            {
                ddlReportingTo2.SelectedValue = bo.RepotingTo2.ToString();
            }

            /*if (!string.IsNullOrEmpty(bo.GlCompanyId.ToString()))
            {
                ddlGLCompany.SelectedValue = bo.GlCompanyId.ToString();
            }*/

            txtHeight.Text = bo.Height;
            ddlCountryId.SelectedValue = bo.CountryId.ToString();
            txtNationalId.Text = bo.NationalId;

            ddlDivision.SelectedValue = bo.DivisionId.ToString();
            ddlDistrict.SelectedValue = bo.DistrictId.ToString();

            ddlThana.SelectedValue = bo.ThanaId.ToString();
            ddlCostCenter.SelectedValue = bo.CostCenterId.ToString();

            hfddlDistrictId.Value = bo.DistrictId.ToString();

            hfddlThanaId.Value = bo.ThanaId.ToString();
            txtPassportNumber.Text = bo.PassportNumber;

            if (bo.JoinDate != null)
            {
                txtEmpJoinDate.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(bo.JoinDate));
            }
            else
            {
                txtEmpJoinDate.Text = string.Empty;
            }
            ddlReportingTo.Items.Remove(ddlReportingTo.Items.FindByValue(bo.EmpId.ToString()));
            ddlReportingTo2.Items.Remove(ddlReportingTo2.Items.FindByValue(bo.EmpId.ToString()));

            txtContractEndDate.Text = bo.InitialContractEndDate != null ? hmUtility.GetStringFromDateTime(Convert.ToDateTime(bo.InitialContractEndDate)) : string.Empty;
            txtPIssuePlace.Text = bo.PIssuePlace;
            if (bo.PIssueDate != null)
            {
                txtPIssueDate.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(bo.PIssueDate));
            }
            else
            {
                txtPIssueDate.Text = string.Empty;
            }
            if (bo.PExpireDate != null)
            {
                txtPExpireDate.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(bo.PExpireDate));
            }
            else
            {
                txtPExpireDate.Text = string.Empty;
            }

            txtPresentAddress.Text = bo.PresentAddress;
            txtPresentCity.Text = bo.PresentCity;
            txtPresentZipCode.Text = bo.PresentZipCode;
            txtPresentCountry.Text = bo.PresentCountry;
            txtPresentPhone.Text = bo.PresentPhone;
            txtPermanentAddress.Text = bo.PermanentAddress;
            txtPermanentCity.Text = bo.PermanentCity;
            txtPermanentZipCode.Text = bo.PermanentZipCode;
            txtPermanentCountry.Text = bo.PermanentCountry;
            txtPermanentPhone.Text = bo.PermanentPhone;
            txtPersonalEmail.Text = bo.PersonalEmail;
            txtAlternativeEmail.Text = bo.AlternativeEmail;
            if (bo.WorkStationId != null)
            {
                ddlWorkStation.SelectedValue = bo.WorkStationId.ToString();
            }
            else
            {
                ddlWorkStation.SelectedIndex = 0;
            }

            if (bo.DonorId != null)
            {
                ddlDonor.SelectedValue = bo.DonorId.ToString();
            }
            else
            {
                ddlDonor.SelectedIndex = 0;
            }

            txtEmergencyContactName.Text = bo.EmergencyContactName;
            txtEmergencyContactRelationship.Text = bo.EmergencyContactRelationship;
            txtEmergencyContactNumber.Text = bo.EmergencyContactNumber;
            txtEmergencyContactNumberHome.Text = bo.EmergencyContactNumberHome;
            txtEmergencyContactEmail.Text = bo.EmergencyContactEmail;
            txtActivityCode.Text = bo.ActivityCode;
            btnSave.Visible = isUpdatePermission;
            btnSave.Text = "Update";

            //Education Information------------------------
            List<EmpEducationBO> eduBO = new List<EmpEducationBO>();
            EmpEducationDA eduDA = new EmpEducationDA();

            eduBO = eduDA.GetEmpEducationByEmpId(EditId);
            Session["EmpEducationList"] = eduBO;

            gvEmpEducation.DataSource = Session["EmpEducationList"] as List<EmpEducationBO>;
            gvEmpEducation.DataBind();

            //Experience Information------------------------
            List<EmpExperienceBO> expBO = new List<EmpExperienceBO>();
            EmpExperienceDA expDA = new EmpExperienceDA();

            expBO = expDA.GetEmpExperienceByEmpId(EditId);
            Session["EmpExperienceList"] = expBO;

            gvExperience.DataSource = Session["EmpExperienceList"] as List<EmpExperienceBO>;
            gvExperience.DataBind();

            //Dependent Information--------------------------------------------------------
            List<EmpDependentBO> dpnBO = new List<EmpDependentBO>();
            EmpDependentDA dpnDA = new EmpDependentDA();
            dpnBO = dpnDA.GetEmpDependentByEmpId(EditId);
            Session["EmpDependentList"] = dpnBO;
            gvDependent.DataSource = Session["EmpDependentList"] as List<EmpDependentBO>;
            gvDependent.DataBind();

            //------------------Bank Information--------------------------------------------
            EmpBankInfoBO bankInfo = new EmpBankInfoBO();
            bankInfo = da.GetEmployeeBankInfo(bo.EmpId);
            if (bankInfo != null)
            {
                if (bankInfo.BankId > 0)
                {
                    Session["_BankInfoId"] = bankInfo.BankInfoId.ToString();
                    ddlBank.SelectedValue = bankInfo.BankId.ToString();
                    txtBranchName.Text = bankInfo.BranchName;
                    txtAccountName.Text = bankInfo.AccountName;
                    txtAccountNumber.Text = bankInfo.AccountNumber;
                    txtAccountType.Text = bankInfo.AccountType;
                    txtCardNumber.Text = bankInfo.CardNumber;
                    txtRemarksForBankInfo.Text = bankInfo.BankRemarks;
                }
            }

            //Benefit Item
            BenefitDA empBenefitDA = new BenefitDA();
            List<PayrollEmpBenefitBO> benefitList = new List<PayrollEmpBenefitBO>();
            benefitList = empBenefitDA.GetEmpBenefitListByEmpId(bo.EmpId);
            int rows = gvBenefit.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                Label lblBenefitHeadId = (Label)gvBenefit.Rows[i].FindControl("lblBenefitHeadId");
                CheckBox cb = (CheckBox)gvBenefit.Rows[i].FindControl("chkIsSavePermission");
                TextBox txtBenefitEffectiveDate = (TextBox)gvBenefit.Rows[i].FindControl("txtBenefitEffectiveDate");

                foreach (PayrollEmpBenefitBO benefitBO in benefitList)
                {
                    if (benefitBO.BenefitHeadId == Int64.Parse(lblBenefitHeadId.Text))
                    {
                        cb.Checked = true;
                        txtBenefitEffectiveDate.Text = benefitBO.ShowEffectiveDate;
                    }
                }
            }

            //Nominee Information-----------------------------------------------------------
            List<EmpNomineeBO> nomineeBO = new List<EmpNomineeBO>();
            EmpNomineeDA nomineeDA = new EmpNomineeDA();
            nomineeBO = nomineeDA.GetEmpNomineeByEmpId(EditId);
            Session["EmpNomineeList"] = nomineeBO;
            gvNominee.DataSource = nomineeBO;
            gvNominee.DataBind();

            //reference Information-----------------------------------------------------------
            List<EmpReferenceBO> ReferenceBO = new List<EmpReferenceBO>();
            EmpReferenceDA referenceDA = new EmpReferenceDA();
            ReferenceBO = referenceDA.GetEmpReferenceByEmpId(EditId);
            Session["EmpReferencePersonsList"] = ReferenceBO;
            gvReference.DataSource = ReferenceBO;
            gvReference.DataBind();

            FillUploadedFile(EditId);
        }
        private void Cancel()
        {
            Session["_EmployeeId"] = null;
            Session["_BankInfoId"] = null;
            Session["Employee_GlCompanyId"] = null;
            Session["Employee_GlProjectId"] = null;
            _CompanyId = 0;
            _ProjectId = 0;
            txtEmpCode.Text = string.Empty;
            hfEmpCode.Value = string.Empty;
            txtDisplayName.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtEmpJoinDate.Text = string.Empty;
            txtJoinDate.Text = string.Empty;
            ddlDepartmentId.SelectedValue = "0";
            ddlEmpCategoryId.SelectedValue = "0";
            ddlDesignationId.SelectedValue = "0";
            ddlGradeId.SelectedValue = "0";
            ddlEmployeeStatus.SelectedValue = "0";
            ddlTitle.SelectedValue = "";
            txtOfficialEmail.Text = string.Empty;
            txtTinNumber.Text = string.Empty;
            txtAppoinmentLetter.Text = string.Empty;
            txtJoiningAgreement.Text = string.Empty;
            txtServiceBond.Text = string.Empty;
            txtDSOAC.Text = string.Empty;
            txtConfirmationLetter.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            txtFathersName.Text = string.Empty;
            txtMothersName.Text = string.Empty;
            txtEmpDateOfBirth.Text = string.Empty;
            txtDateOfMarriage.Text = string.Empty;
            ddlGender.SelectedValue = hmUtility.GetDropDownFirstValue();
            ddlReligion.SelectedValue = hmUtility.GetDropDownFirstValue();
            ddlMaritalStatus.SelectedValue = hmUtility.GetDropDownFirstValue();
            ddlBloodGroup.SelectedValue = hmUtility.GetDropDownFirstValue();
            ddlBloodGroupDepen.SelectedValue = "0";
            ddlNotEffectOnHead.SelectedValue = "0";
            txtHeight.Text = string.Empty;
            ddlCountryId.SelectedIndex = 0;
            txtNationalId.Text = string.Empty;
            txtPassportNumber.Text = string.Empty;
            ddlDivision.SelectedIndex = 0;
            txtContractEndDate.Text = "";
            DocumentInfo.InnerHtml = "";
            DocDiv.InnerHtml = "";
            SigDiv.InnerHtml = "";

            RandomEmpId.Value = "0";

            ddlReportingTo.SelectedIndex = 0;
            ddlReportingTo2.SelectedIndex = 0;
            /*ddlGLCompany.SelectedIndex = 0;
            if (hfIsSingle.Value != "1")            
            {
                ddlGLCompany.SelectedIndex = 0;
            }*/

            ddlPayrollCurrencyId.SelectedValue = (!string.IsNullOrWhiteSpace(hflocalCurrencyId.Value) ? Convert.ToInt32(hflocalCurrencyId.Value) : 1).ToString();

            if (ddlDistrict.Items.Count > 0)
                ddlDistrict.SelectedIndex = 0;

            if (ddlThana.Items.Count > 0)
                ddlThana.SelectedIndex = 0;

            txtAlternativeEmail.Text = string.Empty;
            txtPIssuePlace.Text = string.Empty;
            txtPIssueDate.Text = string.Empty;
            txtPExpireDate.Text = string.Empty;
            txtPresentAddress.Text = string.Empty;
            txtPresentCity.Text = string.Empty;
            txtPresentZipCode.Text = string.Empty;
            txtPresentCountry.Text = string.Empty;
            txtPresentPhone.Text = string.Empty;
            txtPermanentAddress.Text = string.Empty;
            txtPermanentCity.Text = string.Empty;
            txtPermanentZipCode.Text = string.Empty;
            txtPermanentCountry.Text = string.Empty;
            txtPermanentPhone.Text = string.Empty;
            txtPersonalEmail.Text = string.Empty;
            ddlDonor.SelectedValue = string.Empty;
            ddlWorkStation.SelectedValue = "0";
            txtEmergencyContactName.Text = string.Empty;
            txtEmergencyContactRelationship.Text = string.Empty;
            txtEmergencyContactNumber.Text = string.Empty;
            txtEmergencyContactNumberHome.Text = string.Empty;
            txtEmergencyContactEmail.Text = string.Empty;
            txtActivityCode.Text = string.Empty;
            hfddlDistrictId.Value = string.Empty;
            hfddlThanaId.Value = string.Empty;
            btnSave.Text = "Save";
            Session["arrayEducationDelete"] = null;
            Session["arrayExperienceDelete"] = null;
            Session["EmpExperienceList"] = null;
            Session["EmpEducationList"] = null;
            Session["EmpDependentList"] = null;
            Session["EmpNomineeList"] = null;
            Session["EmpReferencePersonsList"] = null;

            gvEmpEducation.DataSource = Session["EmpEducationList"] as List<EmpEducationBO>;
            gvEmpEducation.DataBind();
            ClearEmpEducation();

            gvExperience.DataSource = Session["EmpExperienceList"] as List<EmpExperienceBO>;
            gvExperience.DataBind();
            ClearEmpExperience();

            gvDependent.DataSource = Session["EmpDependentList"] as List<EmpDependentBO>;
            gvDependent.DataBind();
            ClearEmpDependent();

            gvNominee.DataSource = Session["EmpNomineeList"] as List<EmpNomineeBO>;
            gvNominee.DataBind();

            gvReference.DataSource = Session["EmpReferencePersonsList"] as List<EmpReferenceBO>;
            gvReference.DataBind();

            ClearEmpNominee();
            ClearBankInfo();
            ClearBenefitInfo();
        }
        private void IsEmployeeCodeAutoGenerate()
        {
            code.Visible = true;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
            homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsEmployeeCodeAutoGenerate", "IsEmployeeCodeAutoGenerate");
            if (homePageSetupBO != null)
            {
                if (homePageSetupBO.SetupId > 0)
                {
                    hfIsEmployeeCodeAutoGenerate.Value = homePageSetupBO.SetupValue;
                    if (homePageSetupBO.SetupValue == "1")
                    {
                        code.Visible = false;
                    }
                }
            }
        }
        //------Common For All------------------------
        private void AddEditODeleteDetail()
        {
            if (Session["Employee_GlCompanyId"] != null)
            {
                this._CompanyId = int.Parse(Session["Employee_GlCompanyId"].ToString());
            }

            if (Session["Employee_GlProjectId"] != null)
            {
                this._ProjectId = int.Parse(Session["Employee_GlProjectId"].ToString());
            }

            //Education------------
            if (Session["arrayEducationDelete"] == null)
            {
                arrayEducationDelete = new ArrayList();
                Session.Add("arrayEducationDelete", arrayEducationDelete);
            }
            else
            {
                arrayEducationDelete = Session["arrayEducationDelete"] as ArrayList;
            }

            //Experience------------
            if (Session["arrayExperienceDelete"] == null)
            {
                arrayExperienceDelete = new ArrayList();
                Session.Add("arrayExperienceDelete", arrayExperienceDelete);
            }
            else
            {
                arrayExperienceDelete = Session["arrayExperienceDelete"] as ArrayList;
            }

            //Dependent------------
            if (Session["arrayDependentDelete"] == null)
            {
                arrayDependentDelete = new ArrayList();
                Session.Add("arrayDependentDelete", arrayDependentDelete);
            }
            else
            {
                arrayDependentDelete = Session["arrayDependentDelete"] as ArrayList;
            }

            //Nominee------------
            if (Session["arrayNomineeDelete"] == null)
            {
                arrayNomineeDelete = new ArrayList();
                Session.Add("arrayNomineeDelete", arrayNomineeDelete);
            }
            else
            {
                arrayNomineeDelete = Session["arrayNomineeDelete"] as ArrayList;
            }

            //Reference------------
            if (Session["arrayReferencePersonDelete"] == null)
            {
                arrayReferencePersonDelete = new ArrayList();
                Session.Add("arrayReferencePersonDelete", arrayReferencePersonDelete);
            }
            else
            {
                arrayReferencePersonDelete = Session["arrayReferencePersonDelete"] as ArrayList;
            }

            //Document------------
            if (Session["arrayDocumentDelete"] == null)
            {
                arrayDocumentDelete = new ArrayList();
                Session.Add("arrayDocumentDelete", arrayDocumentDelete);
            }
            else
            {
                arrayDocumentDelete = Session["arrayDocumentDelete"] as ArrayList;
            }

            //Training------------
            if (Session["arrayCareerTrainingDelete"] == null)
            {
                arrayCareerTrainingDelete = new ArrayList();
                Session.Add("arrayCareerTrainingDelete", arrayCareerTrainingDelete);
            }
            else
            {
                arrayCareerTrainingDelete = Session["arrayCareerTrainingDelete"] as ArrayList;
            }

            //Reference------------
            if (Session["arrayReferenceDelete"] == null)
            {
                arrayReferenceDelete = new ArrayList();
                Session.Add("arrayReferenceDelete", arrayReferenceDelete);
            }
            else
            {
                arrayReferenceDelete = Session["arrayReferenceDelete"] as ArrayList;
            }
        }
        private void ClearEmpEducation()
        {
            btnEmpEducation.Text = "Add";
            hfEducationId.Text = string.Empty;
            txtExamName.Text = string.Empty;
            txtInstituteName.Text = string.Empty;
            txtSubjectName.Text = string.Empty;
            txtPassYear.Text = string.Empty;
            txtPassClass.Text = string.Empty;
            ddlExamLevel.SelectedIndex = 0;
        }
        //-------Employee Experience------------------
        private void ClearEmpExperience()
        {
            btnEmpExperience.Text = "Add";
            hfExperienceId.Text = string.Empty;
            txtCompanyName.Text = string.Empty;
            txtCompanyUrl.Text = string.Empty;
            txtJoinDate.Text = string.Empty;
            txtJoinDesignation.Text = string.Empty;
            txtLeaveDate.Text = string.Empty;
            txtLeaveDesignation.Text = string.Empty;
            txtAchievements.Text = string.Empty;
        }
        //-------Employee Dependent------------------
        private void ClearEmpDependent()
        {
            btnEmpDependent.Text = "Add";
            txtDependentName.Text = string.Empty;
            txtRelationship.Text = string.Empty;
            txtDateOfBirth.Text = string.Empty;
            ddlBloodGroupDepen.SelectedValue = "0";
            txtAge.Text = string.Empty;
            hfAge.Value = string.Empty;
            //txtDateOfMarriage.Text = string.Empty;
        }
        private void ClearEmpNominee()
        {
            btnAddNominee.Text = "Add";
            txtNomineeName.Text = string.Empty;
            txtNomineeDateOfBirth.Text = string.Empty;
            txtNomineeRelationship.Text = string.Empty;
            txtNomineeAge.Text = string.Empty;
            txtAge.Text = string.Empty;
            txtPercentage.Text = string.Empty;
            hfNomineeAge.Value = string.Empty;
        }
        private void ClearBankInfo()
        {
            ddlBank.SelectedIndex = 0;
            txtAccountName.Text = string.Empty;
            txtAccountType.Text = string.Empty;
            txtBranchName.Text = string.Empty;
            txtAccountNumber.Text = string.Empty;
            txtCardNumber.Text = string.Empty;
            txtRemarksForBankInfo.Text = string.Empty;
        }
        private void ClearBenefitInfo()
        {
            List<PayrollEmpBenefitBO> benefitList = new List<PayrollEmpBenefitBO>();
            int rows = gvBenefit.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                CheckBox cb = (CheckBox)gvBenefit.Rows[i].FindControl("chkIsSavePermission");
                Label lblBenefitHeadId = (Label)gvBenefit.Rows[i].FindControl("lblBenefitHeadId");
                TextBox txtBenefitEffectiveDate = (TextBox)gvBenefit.Rows[i].FindControl("txtBenefitEffectiveDate");
                lblBenefitHeadId.Text = "";
                txtBenefitEffectiveDate.Text = "";
                if (cb.Checked == true)
                {
                    cb.Checked = false;
                }
            }
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static string GetUploadedImageByWebMethod(int OwnerId, string docType)
        {
            string strTable = "";
            DocumentsDA docDA = new DocumentsDA();
            var docList = docDA.GetLastOneDocumentsInfoByDocCategoryAndOwnerId(docType, OwnerId);
            if (docList.Count > 0)
            {
                var Image = docList[0];
                strTable += "<img src='" + Image.Path + Image.Name + "' style='width: 150px; height: 150px'  alt='No Image Selected' border='0' />";
            }
            return strTable;
        }
        [WebMethod]
        public static List<DocumentsBO> GetUploadedDocumentsByWebMethod(int OwnerId, string docType)
        {
            DocumentsDA docDA = new DocumentsDA();
            List<DocumentsBO> docBO = new List<DocumentsBO>();
            docBO = docDA.GetDocumentsInfoByDocCategoryAndOwnerId("Employee Other Documents", OwnerId);
            docBO = new HMCommonDA().GetDocumentListWithIcon(docBO);
            return docBO;
        }
        [WebMethod]
        public static bool DeleteDoc(long documentId)
        {
            DocumentsDA docDA = new DocumentsDA();
            var status = docDA.DeleteDocumentsByDocumentId(documentId);
            return status;
        }
        [WebMethod(EnableSession = true)]
        public static EmpTypeBO GetEmpTypeInfoById(string transactionId)
        {
            EmpTypeBO empTypeBO = new EmpTypeBO();
            if (!string.IsNullOrWhiteSpace(transactionId))
            {
                EmpTypeDA empTypeDA = new EmpTypeDA();
                empTypeBO = empTypeDA.GetEmpTypeInfoById(Convert.ToInt32(transactionId));
            }

            return empTypeBO;
        }
        [WebMethod]
        public static bool CheckDuplicateEmployeeCode(string empCode)
        {
            EmployeeDA emp = new EmployeeDA();
            EmployeeBO bo = new EmployeeBO();
            bo = emp.GetEmployeeInfoByCode(empCode);

            if (bo.EmpCode != null)
            {
                return false;
            }
            else
                return true;
        }
        [WebMethod]
        public static GridViewDataNPaging<EmployeeBO, GridPaging> SearchNLoadEmpInformation(int companyId, int projectId, string empName, string code, string department, string designation, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;
            bool isApplicant = false;

            GridViewDataNPaging<EmployeeBO, GridPaging> myGridData = new GridViewDataNPaging<EmployeeBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetEmpInformationBySearchCriteriaForPaging(companyId, projectId, empName, code, department, designation, isApplicant, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<EmployeeBO> distinctItems = new List<EmployeeBO>();
            distinctItems = empList.GroupBy(test => test.EmpId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static List<EmpDistrictBO> LoadDistrict(string divisionId)
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmpDistrictBO> disList = new List<EmpDistrictBO>();
            disList = empDA.GetEmpDistrictList(Convert.ToInt32(divisionId));
            return disList;
        }
        [WebMethod]
        public static List<EmpThanaBO> LoadThana(string districtId)
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmpThanaBO> thanaList = new List<EmpThanaBO>();
            thanaList = empDA.GetEmpThanaList(Convert.ToInt32(districtId));
            return thanaList;
        }
        [WebMethod]
        public static string LoadProvisionPeriodInformation(string empStrJoinDate, int gradeId, int IsProvisionPeriod)
        {
            string ProvisionPeriod = string.Empty;
            HMUtility hmUtility = new HMUtility();
            EmpGradeBO gradeBO = new EmpGradeBO();
            EmpGradeDA gradeDA = new EmpGradeDA();

            DateTime empJoinDate = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(empStrJoinDate))
            {
                empJoinDate = hmUtility.GetDateTimeFromString(empStrJoinDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            if (gradeId > 0 && IsProvisionPeriod > 0)
            {
                gradeBO = gradeDA.GetGradeInfoById(gradeId);

                if (gradeBO != null)
                {
                    ProvisionPeriod = hmUtility.GetStringFromDateTime(empJoinDate.AddMonths(gradeBO.ProvisionPeriodMonth));
                }
            }
            return ProvisionPeriod;
        }
        protected void btnAddReference_Click(object sender, EventArgs e)
        {
            int dynamicReferenceId = 0;
            List<EmpReferenceBO> EmpReferenceListBO = Session["EmpReferencePersonsList"] == null ? new List<EmpReferenceBO>() : Session["EmpReferencePersonsList"] as List<EmpReferenceBO>;

            if (!string.IsNullOrWhiteSpace(lblReferenceId.Text))
                dynamicReferenceId = Convert.ToInt32(lblReferenceId.Text);

            EmpReferenceBO detailBO = dynamicReferenceId == 0 ? new EmpReferenceBO() : EmpReferenceListBO.Where(x => x.ReferenceId == dynamicReferenceId).FirstOrDefault();
            if (EmpReferenceListBO.Contains(detailBO))
                EmpReferenceListBO.Remove(detailBO);

            detailBO.Name = txtReferenceName.Text;
            detailBO.Relationship = txtReferenceRelationship.Text;
            detailBO.Description = txtDescription.Text;
            detailBO.Organization = txtOrganization.Text;
            detailBO.Designation = txtDesignation.Text;
            detailBO.Address = txtAddress.Text;
            detailBO.Mobile = txtMobile.Text;
            detailBO.Email = txtEmail.Text;
            detailBO.ReferenceId = dynamicReferenceId == 0 ? EmpReferenceListBO.Count + 1 : dynamicReferenceId;
            EmpReferenceListBO.Add(detailBO);
            Session["EmpReferencePersonsList"] = EmpReferenceListBO;
            gvReference.DataSource = Session["EmpReferencePersonsList"] as List<EmpReferenceBO>;
            gvReference.DataBind();
            lblReferenceId.Text = null;
            ClearReference();
            SetTab("ReferenceTab");
        }
        private void ClearReference()
        {
            btnAddReference.Text = "Add";
            txtReferenceName.Text = "";
            txtReferenceRelationship.Text = "";
            txtDescription.Text = "";
            txtOrganization.Text = "";
            txtDesignation.Text = "";
            txtAddress.Text = "";
            txtMobile.Text = "";
            txtEmail.Text = "";
        }
        protected void gvReference_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _EmpReferenceId;

            if (e.CommandName == "CmdEdit")
            {
                _EmpReferenceId = Convert.ToInt32(e.CommandArgument.ToString());
                lblReferenceId.Text = _EmpReferenceId.ToString();
                var ReferenceDetailBO = (List<EmpReferenceBO>)Session["EmpReferencePersonsList"];
                if (ReferenceDetailBO != null)
                {
                    var ReferenceBO = ReferenceDetailBO.Where(x => x.ReferenceId == _EmpReferenceId).FirstOrDefault();
                    if (ReferenceBO != null && ReferenceBO.ReferenceId > 0)
                    {
                        txtReferenceName.Text = ReferenceBO.Name;
                        txtReferenceRelationship.Text = ReferenceBO.Relationship;
                        txtDescription.Text = ReferenceBO.Description;

                        txtOrganization.Text = ReferenceBO.Organization;
                        txtDesignation.Text = ReferenceBO.Designation;
                        txtAddress.Text = ReferenceBO.Address;
                        txtMobile.Text = ReferenceBO.Mobile;
                        txtEmail.Text = ReferenceBO.Email;

                        btnAddReference.Text = "Update";
                    }
                    else
                    {
                        btnAddReference.Text = "Add";
                    }
                }
            }
            else if (e.CommandName == "CmdDelete")
            {
                _EmpReferenceId = Convert.ToInt32(e.CommandArgument.ToString());
                var EmpReferenceDetailBO = (List<EmpReferenceBO>)Session["EmpReferencePersonsList"];
                if (EmpReferenceDetailBO != null)
                {
                    var ReferenceBO = EmpReferenceDetailBO.Where(x => x.ReferenceId == _EmpReferenceId).FirstOrDefault();
                    EmpReferenceDetailBO.Remove(ReferenceBO);
                    Session["EmpReferencePersonsList"] = EmpReferenceDetailBO;
                    arrayReferencePersonDelete.Add(_EmpReferenceId);
                }
                gvReference.DataSource = Session["EmpReferencePersonsList"] as List<EmpReferenceBO>;
                gvReference.DataBind();
            }
            SetTab("ReferenceTab");

        }
    }
}