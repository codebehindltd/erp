using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.SalesAndMarketing;
using System.Web.Services;
using System.Text.RegularExpressions;

namespace HotelManagement.Presentation.Website.Recruitment
{
    public partial class frmApplicant : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        protected int _EmployeeId;
        ArrayList arrayEducationDelete;
        ArrayList arrayExperienceDelete;
        ArrayList arrayDependentDelete;
        ArrayList arrayNomineeDelete;
        ArrayList arrayDocumentDelete;
        ArrayList arrayCareerTrainingDelete;
        ArrayList arrayLanguageDelete;
        ArrayList arrayReferenceDelete;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            this.AddEditODeleteDetail();
            if (!IsPostBack)
            {
                ClearSession();
                int EmployeeId = 0;
                Random rd = new Random();
                EmployeeId = rd.Next(100000, 999999);
                RandomEmpId.Value = EmployeeId.ToString();
                tempEmpId.Value = EmployeeId.ToString();

                this.FileUpload();

                this.txtGoToScrolling.Text = "EntryPanel";
                this.LoadCompanyCountry();
                //ddlCountryId.SelectedIndex = 18;
                ddlCountryId.SelectedValue = LoadCompanyCountry().SetupValue;
                this.LoadMaritualStatus();
                this.LoadGender();
                this.LoadRelegion();
                this.LoadBloodGroup();
                this.LoadCountry();
                LoadLocation();
                LoadCurrency();
                LoadJobCategory();
                LoadOrganizationType();
                LoadLanguageProfiency();
                LoadCountryList();
                string editId = Request.QueryString["editId"];
                if (!string.IsNullOrWhiteSpace(editId))
                {
                    int Id = Convert.ToInt32(editId);
                    if (Id > 0)
                    {
                        FillForm(Id);
                        SetTab("DetailsTab");
                    }
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Recruitment/frmApplicant.aspx");
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

            detailBO.ExamName = this.txtExamName.Text;
            detailBO.InstituteName = this.txtInstituteName.Text;
            detailBO.SubjectName = this.txtSubjectName.Text;
            detailBO.PassYear = this.txtPassYear.Text;
            detailBO.PassClass = this.txtPassClass.Text;
            detailBO.EducationId = dynamicEducationId == 0 ? EmpEducationListBO.Count + 1 : dynamicEducationId;
            EmpEducationListBO.Add(detailBO);
            Session["EmpEducationList"] = EmpEducationListBO;
            this.gvEmpEducation.DataSource = Session["EmpEducationList"] as List<EmpEducationBO>;
            this.gvEmpEducation.DataBind();
            this.ClearEmpEducation();

            this.txtGoToScrolling.Text = "EducationInformation";
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
                var educationBO = educationDetailBO.Where(x => x.EducationId == _EmpEducationId).FirstOrDefault();
                if (educationBO != null && educationBO.EducationId > 0)
                {
                    this.txtExamName.Text = educationBO.ExamName;
                    this.txtInstituteName.Text = educationBO.InstituteName;
                    this.txtSubjectName.Text = educationBO.SubjectName;
                    this.txtPassYear.Text = educationBO.PassYear;
                    this.txtPassClass.Text = educationBO.PassClass;
                    btnEmpEducation.Text = "Update";
                }
                else
                {
                    btnEmpEducation.Text = "Add";
                }
            }
            else if (e.CommandName == "CmdDelete")
            {
                _EmpEducationId = Convert.ToInt32(e.CommandArgument.ToString());
                hfEducationId.Text = string.Empty;
                var educationDetailBO = (List<EmpEducationBO>)Session["EmpEducationList"];
                var empEducation = educationDetailBO.Where(x => x.EducationId == _EmpEducationId).FirstOrDefault();
                educationDetailBO.Remove(empEducation);
                Session["EmpEducationList"] = educationDetailBO;
                arrayEducationDelete.Add(_EmpEducationId);
                this.gvEmpEducation.DataSource = Session["EmpEducationList"] as List<EmpEducationBO>;
                this.gvEmpEducation.DataBind();
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

            detailBO.CompanyName = this.txtCompanyName.Text;
            detailBO.CompanyUrl = txtCompanyUrl.Text;
            if (!string.IsNullOrWhiteSpace(txtJoinDate.Text))
            {
                //detailBO.JoinDate = Convert.ToDateTime(this.txtJoinDate.Text);
                detailBO.JoinDate = CommonHelper.DateTimeToMMDDYYYY(this.txtJoinDate.Text);
            }
            detailBO.JoinDesignation = this.txtJoinDesignation.Text;
            if (!string.IsNullOrWhiteSpace(txtLeaveDate.Text))
            {
                //detailBO.LeaveDate = Convert.ToDateTime(this.txtLeaveDate.Text);
                detailBO.LeaveDate = CommonHelper.DateTimeToMMDDYYYY(txtLeaveDate.Text);
            }
            else {
                detailBO.LeaveDate = null;
            }
            detailBO.LeaveDesignation = this.txtLeaveDesignation.Text;
            detailBO.Achievements = this.txtAchievements.Text;
            detailBO.ShowJoinDate = txtJoinDate.Text;
            detailBO.ShowLeaveDate = txtLeaveDate.Text;
            detailBO.ExperienceId = dynamicExperienceId == 0 ? EmpExperienceListBO.Count + 1 : dynamicExperienceId;

            EmpExperienceListBO.Add(detailBO);

            Session["EmpExperienceList"] = EmpExperienceListBO;

            this.gvExperience.DataSource = Session["EmpExperienceList"] as List<EmpExperienceBO>;
            this.gvExperience.DataBind();

            this.ClearEmpExperience();

            this.txtGoToScrolling.Text = "ExperienceInformation";
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
                var experienceBO = experienceDetailBO.Where(x => x.ExperienceId == _EmpExperienceId).FirstOrDefault();
                if (experienceBO != null && experienceBO.ExperienceId > 0)
                {
                    this.txtCompanyName.Text = experienceBO.CompanyName;
                    txtCompanyUrl.Text = experienceBO.CompanyUrl;
                    this.txtJoinDate.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(experienceBO.JoinDate));
                    this.txtJoinDesignation.Text = experienceBO.JoinDesignation;
                    this.txtLeaveDate.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime( experienceBO.LeaveDate));
                    this.txtLeaveDesignation.Text = experienceBO.LeaveDesignation;
                    this.txtAchievements.Text = experienceBO.Achievements;

                    btnEmpExperience.Text = "Update";
                }
                else
                {
                    btnEmpExperience.Text = "Add";
                }
            }
            else if (e.CommandName == "CmdDelete")
            {
                _EmpExperienceId = Convert.ToInt32(e.CommandArgument.ToString());
                hfExperienceId.Text = string.Empty;
                var experienceDetailBO = (List<EmpExperienceBO>)Session["EmpExperienceList"];
                var empExperience = experienceDetailBO.Where(x => x.ExperienceId == _EmpExperienceId).FirstOrDefault();
                experienceDetailBO.Remove(empExperience);
                Session["EmpExperienceList"] = experienceDetailBO;
                arrayExperienceDelete.Add(_EmpExperienceId);
                this.gvExperience.DataSource = Session["EmpExperienceList"] as List<EmpExperienceBO>;
                this.gvExperience.DataBind();
            }
            SetTab("ExperienceTab");
        }
        protected void btnEmpCareerTraining_Click(object sender, EventArgs e)
        {
            if (!IsTrainingValid())
            {
                return;
            }

            int dynamicCareerTrainingId = 0;
            List<EmpCareerTrainingBO> EmpCareerTrainingListBO = Session["EmpCareerTrainingList"] == null ? new List<EmpCareerTrainingBO>() : Session["EmpCareerTrainingList"] as List<EmpCareerTrainingBO>;

            if (!string.IsNullOrWhiteSpace(hfCareerTrainingId.Text))
                dynamicCareerTrainingId = Convert.ToInt32(hfCareerTrainingId.Text);

            EmpCareerTrainingBO trainingBO = dynamicCareerTrainingId == 0 ? new EmpCareerTrainingBO() : EmpCareerTrainingListBO.Where(x => x.CareerTrainingId == dynamicCareerTrainingId).FirstOrDefault();
            if (EmpCareerTrainingListBO.Contains(trainingBO))
                EmpCareerTrainingListBO.Remove(trainingBO);

            trainingBO.TrainingTitle = txtTrainingTitle.Text;
            trainingBO.Topic = txtTopic.Text;
            trainingBO.Institute = txtInstitute.Text;
            if (ddlCountry.SelectedIndex != 0)
            {
                trainingBO.Country = Convert.ToInt32(ddlCountry.SelectedValue);
            }
            trainingBO.Location = txtLocation.Text;
            trainingBO.TrainingYear = txtTrainingYear.Text;
            if (!string.IsNullOrWhiteSpace(txtDuration.Text))
            {
                trainingBO.Duration = Convert.ToInt32(txtDuration.Text);
            }
            trainingBO.DurationType = ddlDuration.SelectedItem.Text;

            trainingBO.CareerTrainingId = dynamicCareerTrainingId == 0 ? EmpCareerTrainingListBO.Count + 1 : dynamicCareerTrainingId;
            EmpCareerTrainingListBO.Add(trainingBO);
            Session["EmpCareerTrainingList"] = EmpCareerTrainingListBO;
            this.gvEmpCareerTraining.DataSource = Session["EmpCareerTrainingList"] as List<EmpCareerTrainingBO>;
            this.gvEmpCareerTraining.DataBind();
            this.ClearEmpCareerTraining();

            this.txtGoToScrolling.Text = "CareerTrainingInformation";
            SetTab("CareerTrainingTab");
        }
        protected void gvEmpCareerTraining_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _EmpCareerTrainingId;

            if (e.CommandName == "CmdEdit")
            {
                _EmpCareerTrainingId = Convert.ToInt32(e.CommandArgument.ToString());
                hfCareerTrainingId.Text = _EmpCareerTrainingId.ToString();
                var careerTrainingList = (List<EmpCareerTrainingBO>)Session["EmpCareerTrainingList"];
                var careerTrainingBO = careerTrainingList.Where(x => x.CareerTrainingId == _EmpCareerTrainingId).FirstOrDefault();
                if (careerTrainingBO != null && careerTrainingBO.CareerTrainingId > 0)
                {
                    txtTrainingTitle.Text = careerTrainingBO.TrainingTitle;
                    txtTopic.Text = careerTrainingBO.Topic;
                    txtInstitute.Text = careerTrainingBO.Institute;
                    ddlCountry.SelectedValue = careerTrainingBO.Country.ToString();
                    txtLocation.Text = careerTrainingBO.Location;
                    txtTrainingYear.Text = careerTrainingBO.TrainingYear;
                    txtDuration.Text = careerTrainingBO.Duration.ToString();
                    ddlDuration.SelectedValue = careerTrainingBO.DurationType;
                    btnEmpCareerTraining.Text = "Update";
                }
                else
                {
                    btnEmpCareerTraining.Text = "Add";
                }
            }
            else if (e.CommandName == "CmdDelete")
            {
                _EmpCareerTrainingId = Convert.ToInt32(e.CommandArgument.ToString());
                hfCareerTrainingId.Text = string.Empty;
                var careerTrainingList = (List<EmpCareerTrainingBO>)Session["EmpCareerTrainingList"];
                var careerTrainingBO = careerTrainingList.Where(x => x.CareerTrainingId == _EmpCareerTrainingId).FirstOrDefault();
                careerTrainingList.Remove(careerTrainingBO);
                Session["EmpCareerTrainingList"] = careerTrainingList;
                arrayCareerTrainingDelete.Add(_EmpCareerTrainingId);
                this.gvEmpCareerTraining.DataSource = Session["EmpCareerTrainingList"] as List<EmpCareerTrainingBO>;
                this.gvEmpCareerTraining.DataBind();
            }
            SetTab("CareerTrainingTab");
        }
        protected void btnEmpLanguage_Click(object sender, EventArgs e)
        {
            if (!IsLanguageValid())
            {
                return;
            }

            int dynamicLanguageId = 0;
            List<EmpLanguageBO> EmpLanguageList = Session["EmpLanguageList"] == null ? new List<EmpLanguageBO>() : Session["EmpLanguageList"] as List<EmpLanguageBO>;

            if (!string.IsNullOrWhiteSpace(hfLanguageId.Text))
                dynamicLanguageId = Convert.ToInt32(hfLanguageId.Text);

            EmpLanguageBO languageBO = dynamicLanguageId == 0 ? new EmpLanguageBO() : EmpLanguageList.Where(x => x.LanguageId == dynamicLanguageId).FirstOrDefault();
            if (EmpLanguageList.Contains(languageBO))
                EmpLanguageList.Remove(languageBO);

            languageBO.Language = txtLanguage.Text;
            languageBO.Reading = ddlReading.SelectedValue;
            languageBO.Writing = ddlWriting.SelectedValue;
            languageBO.Speaking = ddlSpeaking.SelectedValue;
            languageBO.ReadingLevel = ddlReading.SelectedItem.Text;
            languageBO.WritingLevel = ddlWriting.SelectedItem.Text;
            languageBO.SpeakingLevel = ddlSpeaking.SelectedItem.Text;

            languageBO.LanguageId = dynamicLanguageId == 0 ? EmpLanguageList.Count + 1 : dynamicLanguageId;
            EmpLanguageList.Add(languageBO);
            Session["EmpLanguageList"] = EmpLanguageList;
            this.gvEmpLanguage.DataSource = Session["EmpLanguageList"] as List<EmpLanguageBO>;
            this.gvEmpLanguage.DataBind();
            this.ClearEmpLanguage();

            this.txtGoToScrolling.Text = "LanguageInformation";
            SetTab("LanguageTab");

        }
        protected void gvEmpLanguage_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _EmpLanguageId;

            if (e.CommandName == "CmdEdit")
            {
                _EmpLanguageId = Convert.ToInt32(e.CommandArgument.ToString());
                hfLanguageId.Text = _EmpLanguageId.ToString();
                var languageList = (List<EmpLanguageBO>)Session["EmpLanguageList"];
                var languageBO = languageList.Where(x => x.LanguageId == _EmpLanguageId).FirstOrDefault();
                if (languageBO != null && languageBO.LanguageId > 0)
                {
                    txtLanguage.Text = languageBO.Language;
                    ddlReading.SelectedValue = languageBO.Reading;
                    ddlWriting.SelectedValue = languageBO.Writing;
                    ddlSpeaking.SelectedValue = languageBO.Speaking;
                    btnEmpLanguage.Text = "Update";
                }
                else
                {
                    btnEmpLanguage.Text = "Add";
                }
            }
            else if (e.CommandName == "CmdDelete")
            {
                _EmpLanguageId = Convert.ToInt32(e.CommandArgument.ToString());
                hfLanguageId.Text = string.Empty;
                var languageList = (List<EmpLanguageBO>)Session["EmpLanguageList"];
                var languageBO = languageList.Where(x => x.LanguageId == _EmpLanguageId).FirstOrDefault();
                languageList.Remove(languageBO);
                Session["EmpLanguageList"] = languageList;
                arrayLanguageDelete.Add(_EmpLanguageId);
                this.gvEmpLanguage.DataSource = Session["EmpLanguageList"] as List<EmpLanguageBO>;
                this.gvEmpLanguage.DataBind();
            }
            //ClearEmpLanguage();
            SetTab("LanguageTab");
        }
        protected void btnEmpReference_Click(object sender, EventArgs e)
        {
            if (!IsReferenceValid())
            {
                return;
            }
            int dynamicReferenceId = 0;
            List<EmpReferenceBO> EmpReferenceListBO = Session["EmpReferenceList"] == null ? new List<EmpReferenceBO>() : Session["EmpReferenceList"] as List<EmpReferenceBO>;

            if (!string.IsNullOrWhiteSpace(hfReferenceId.Text))
                dynamicReferenceId = Convert.ToInt32(hfReferenceId.Text);

            EmpReferenceBO referenceBO = dynamicReferenceId == 0 ? new EmpReferenceBO() : EmpReferenceListBO.Where(x => x.ReferenceId == dynamicReferenceId).FirstOrDefault();
            if (EmpReferenceListBO.Contains(referenceBO))
                EmpReferenceListBO.Remove(referenceBO);

            referenceBO.Name = txtName.Text;
            referenceBO.Organization = txtOrganization.Text;
            referenceBO.Designation = txtDesignation.Text;
            referenceBO.Address = txtAddress.Text;
            referenceBO.Email = txtEmail.Text;
            referenceBO.Mobile = txtMobile.Text;
            referenceBO.Relation = txtRelation.Text;

            referenceBO.ReferenceId = dynamicReferenceId == 0 ? EmpReferenceListBO.Count + 1 : dynamicReferenceId;
            EmpReferenceListBO.Add(referenceBO);
            Session["EmpReferenceList"] = EmpReferenceListBO;
            this.gvEmpReference.DataSource = Session["EmpReferenceList"] as List<EmpReferenceBO>;
            this.gvEmpReference.DataBind();
            this.ClearEmpReference();

            this.txtGoToScrolling.Text = "ReferenceInformation";
            SetTab("ReferenceTab");
        }
        protected void gvEmpReference_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _EmpReferenceId;

            if (e.CommandName == "CmdEdit")
            {
                _EmpReferenceId = Convert.ToInt32(e.CommandArgument.ToString());
                hfReferenceId.Text = _EmpReferenceId.ToString();
                var referenceList = (List<EmpReferenceBO>)Session["EmpReferenceList"];
                var referenceBO = referenceList.Where(x => x.ReferenceId == _EmpReferenceId).FirstOrDefault();
                if (referenceBO != null && referenceBO.ReferenceId > 0)
                {
                    txtName.Text = referenceBO.Name;
                    txtOrganization.Text = referenceBO.Organization;
                    txtDesignation.Text = referenceBO.Designation;
                    txtAddress.Text = referenceBO.Address;
                    txtEmail.Text = referenceBO.Email;
                    txtMobile.Text = referenceBO.Mobile;
                    txtRelation.Text = referenceBO.Relation;
                    btnEmpReference.Text = "Update";
                }
                else
                {
                    btnEmpReference.Text = "Add";
                }
            }
            else if (e.CommandName == "CmdDelete")
            {
                _EmpReferenceId = Convert.ToInt32(e.CommandArgument.ToString());
                hfReferenceId.Text = string.Empty;
                var referenceList = (List<EmpReferenceBO>)Session["EmpReferenceList"];
                var referenceBO = referenceList.Where(x => x.ReferenceId == _EmpReferenceId).FirstOrDefault();
                referenceList.Remove(referenceBO);
                Session["EmpReferenceList"] = referenceList;
                arrayReferenceDelete.Add(_EmpReferenceId);
                this.gvEmpReference.DataSource = Session["EmpReferenceList"] as List<EmpReferenceBO>;
                this.gvEmpReference.DataBind();
            }
            SetTab("ReferenceTab");
        }
        protected void gvEmployeeDocument_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _documentId;
            if (e.CommandName == "CmdDelete")
            {
                _documentId = Convert.ToInt32(e.CommandArgument.ToString());
                var documentList = (List<DocumentsBO>)Session["_EmployeeDocuments"];
                var document = documentList.Where(x => x.DocumentId == _documentId).FirstOrDefault();
                documentList.Remove(document);
                Session["_EmployeeDocuments"] = documentList;
                arrayDocumentDelete.Add(_documentId);
                this.gvEmployeeDocument.DataSource = Session["_EmployeeDocuments"] as List<DocumentsBO>;
                this.gvEmployeeDocument.DataBind();
                DocumentsDA docda = new DocumentsDA();
                docda.DeleteDocumentsByDocumentId(_documentId);
            }
            SetTab("DocumentsTab");
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

            //employeeBO.EmpCode = this.txtEmpCode.Text;
            employeeBO.FirstName = this.txtFirstName.Text;
            employeeBO.LastName = this.txtLastName.Text;
            employeeBO.DisplayName = this.txtFirstName.Text + " " + this.txtLastName.Text;
            employeeBO.RandomEmpId = Int32.Parse(this.RandomEmpId.Value);
            employeeBO.FathersName = this.txtFathersName.Text;
            employeeBO.MothersName = this.txtMothersName.Text;
            if (!string.IsNullOrWhiteSpace(this.txtEmpDateOfBirth.Text))
            {
                employeeBO.EmpDateOfBirth = hmUtility.GetDateTimeFromString(this.txtEmpDateOfBirth.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                employeeBO.EmpDateOfBirth = null;
            }
            if (this.ddlGender.SelectedIndex == 0)
            {
                employeeBO.Gender = string.Empty;
            }
            else
            {
                employeeBO.Gender = this.ddlGender.SelectedItem.Text;//
            }
            employeeBO.Religion = this.ddlReligion.SelectedItem.Text;//
            employeeBO.MaritalStatus = this.ddlMaritalStatus.SelectedItem.Text;//
            if (this.ddlBloodGroup.SelectedIndex == 0)
            {
                employeeBO.BloodGroup = string.Empty;
            }
            else
            {
                employeeBO.BloodGroup = this.ddlBloodGroup.SelectedItem.Text;//
            }

            employeeBO.Height = this.txtHeight.Text;
            employeeBO.CountryId = Int32.Parse(this.ddlCountryId.SelectedValue);
            employeeBO.Nationality = ddlCountryId.SelectedItem.Text;
            employeeBO.NationalId = this.txtNationalId.Text;
            employeeBO.PassportNumber = this.txtPassportNumber.Text;
            employeeBO.PIssuePlace = this.txtPIssuePlace.Text;
            if (!string.IsNullOrWhiteSpace(this.txtPIssueDate.Text))
            {
                employeeBO.PIssueDate = hmUtility.GetDateTimeFromString(this.txtPIssueDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                employeeBO.PIssueDate = null;
            }
            if (!string.IsNullOrWhiteSpace(this.txtPExpireDate.Text))
            {
                employeeBO.PExpireDate = hmUtility.GetDateTimeFromString(this.txtPExpireDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                employeeBO.PExpireDate = null;
            }
            //employeeBO.CurrentLocationId = Convert.ToInt32(ddlCurrentLocation.SelectedValue);
            employeeBO.PresentAddress = this.txtPresentAddress.Text;
            employeeBO.PresentCity = this.txtPresentCity.Text;
            employeeBO.PresentZipCode = this.txtPresentZipCode.Text;
            //employeeBO.PresentCountry = this.txtPresentCountry.Text;
            employeeBO.PresentCountryId = Convert.ToInt32(ddlPresentCountry.SelectedValue);
            employeeBO.PresentPhone = this.txtPresentPhone.Text;
            employeeBO.PermanentAddress = this.txtPermanentAddress.Text;
            employeeBO.PermanentCity = this.txtPermanentCity.Text;
            employeeBO.PermanentZipCode = this.txtPermanentZipCode.Text;
            //employeeBO.PermanentCountry = this.txtPermanentCountry.Text;
            employeeBO.PermanentCountryId = Convert.ToInt32(ddlPermanentCountry.SelectedValue);
            employeeBO.PermanentPhone = this.txtPermanentPhone.Text;
            employeeBO.PersonalEmail = this.txtPersonalEmail.Text;
            employeeBO.AlternativeEmail = txtAlternativeEmail.Text;
            employeeBO.IsApplicant = true;

            employeeBO.JoinDate = null;
            employeeBO.ResignationDate = null;

            EmpCareerInfoBO careerInfo = new EmpCareerInfoBO();
            if (!string.IsNullOrWhiteSpace(hfEmpCareerInfoId.Text))
            {
                careerInfo.CareerInfoId = Convert.ToInt32(hfEmpCareerInfoId.Text);
            }
            careerInfo.Objective = txtObjective.Text;
            if (!string.IsNullOrWhiteSpace(txtPresentSalary.Text))
            {
                careerInfo.PresentSalary = Convert.ToDecimal(txtPresentSalary.Text);
            }
            if (!string.IsNullOrWhiteSpace(txtExpectedSlary.Text))
            {
                careerInfo.ExpectedSalary = Convert.ToDecimal(txtExpectedSlary.Text);
            }
            careerInfo.Currency = Convert.ToInt32(ddlCurrency.SelectedValue);
            careerInfo.JobLevel = ddlJobLevel.Text;
            careerInfo.AvailableType = ddlAvailableType.Text;
            if (ddlJobCategory.SelectedIndex != 0)
            {
                careerInfo.PreferedJobType = Convert.ToInt32(ddlJobCategory.SelectedValue);
            }
            if (ddlOrganizationType.SelectedIndex != 0)
            {
                careerInfo.PreferedOrganizationType = Convert.ToInt32(ddlOrganizationType.SelectedValue);
            }
            if (ddlPrfJobLocation.SelectedIndex != 0)
            {
                careerInfo.PreferedJobLocationId = Convert.ToInt32(ddlPrfJobLocation.SelectedValue);
            }
            else
            {
                careerInfo.PreferedJobLocationId = null;
            }
            
            careerInfo.ExtraCurriculmActivities = txtExtraActivities.Text;
            careerInfo.CareerSummary = txtCareerSummary.Text;
            employeeBO.PayrollCurrencyId = Convert.ToInt32(ddlCurrency.SelectedValue);

            if (this.btnSave.Text.Equals("Save"))
            {
                int tmpRoomTypeId = 0; string appCode = string.Empty;
                employeeBO.CreatedBy = userInformationBO.UserInfoId;                
                Boolean status = employeeDA.SaveEmployeeInfo(employeeBO, out tmpRoomTypeId, out appCode, Session["EmpEducationList"] as List<EmpEducationBO>, Session["EmpExperienceList"] as List<EmpExperienceBO>, null, null, null, careerInfo, Session["EmpCareerTrainingList"] as List<EmpCareerTrainingBO>, Session["EmpLanguageList"] as List<EmpLanguageBO>, Session["EmpReferenceList"] as List<EmpReferenceBO>, null);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save + " Your Code is: '" + appCode + "'", AlertType.Success);
                    this.LoadGridView();
                    this.Cancel();
                }
            }
            else
            {
                string Id = Session["_EmployeeId"].ToString();
                employeeBO.EmpId = Convert.ToInt32(Id);
                employeeBO.LastModifiedBy = userInformationBO.UserInfoId;                
                Boolean status = employeeDA.UpdateApplicantInfo(employeeBO, Session["EmpEducationList"] as List<EmpEducationBO>, Session["arrayEducationDelete"] as ArrayList, Session["EmpExperienceList"] as List<EmpExperienceBO>, Session["arrayExperienceDelete"] as ArrayList, null, null, null, null, null, careerInfo, Session["EmpCareerTrainingList"] as List<EmpCareerTrainingBO>, Session["arrayCareerTrainingDelete"] as ArrayList, Session["EmpLanguageList"] as List<EmpLanguageBO>, Session["arrayLanguageDelete"] as ArrayList, Session["EmpReferenceList"] as List<EmpReferenceBO>, Session["arrayReferenceDelete"] as ArrayList);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    this.LoadGridView();
                    this.Cancel();
                }
            }
            RandomEmpId.Value = "0";
            SetTab("EmployeeTab");
        }
        //************************ User Defined Function ********************//
        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
           // flashUploadSignature.QueryParameters = "employeeId=" + Server.UrlEncode(RandomEmpId.Value) + "-Applicant Signature";
            //flashUploadDocuments.QueryParameters = "employeeId=" + Server.UrlEncode(RandomEmpId.Value) + "-Applicant Document";
            //flashUpload.QueryParameters = "employeeId=" + Server.UrlEncode(RandomEmpId.Value) + "-Applicant Other Documents";
        }
        private void LoadCountry()
        {
            HMCommonDA commonDA = new HMCommonDA();
            var List = commonDA.GetAllCountries();
            this.ddlCountryId.DataSource = List;
            this.ddlCountryId.DataTextField = "CountryName";
            this.ddlCountryId.DataValueField = "CountryId";
            this.ddlCountryId.DataBind();

            this.ddlCountry.DataSource = List;
            this.ddlCountry.DataTextField = "CountryName";
            this.ddlCountry.DataValueField = "CountryId";
            this.ddlCountry.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCountryId.Items.Insert(0, item);
            this.ddlCountry.Items.Insert(0, item);
        }
        private HMCommonSetupBO LoadCompanyCountry()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("CompanyCountryId", "CompanyCountryId");
            return commonSetupBO;
        }
        private void LoadMaritualStatus()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("MaritualStatus", hmUtility.GetDropDownFirstValue());
            this.ddlMaritalStatus.DataSource = fields;
            this.ddlMaritalStatus.DataTextField = "FieldValue";
            this.ddlMaritalStatus.DataValueField = "FieldValue";
            this.ddlMaritalStatus.DataBind();
        }

        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("LocalNUsd");

            this.ddlCurrency.DataSource = currencyListBO;
            this.ddlCurrency.DataTextField = "CurrencyName";
            this.ddlCurrency.DataValueField = "CurrencyId";
            this.ddlCurrency.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCurrency.Items.Insert(0, item);
        }
        private void LoadGender()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("Gender", hmUtility.GetDropDownFirstValue());

            this.ddlGender.DataSource = fields;
            this.ddlGender.DataTextField = "FieldValue";
            this.ddlGender.DataValueField = "FieldValue";
            this.ddlGender.DataBind();
        }
        private void LoadRelegion()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("Relegion", hmUtility.GetDropDownFirstValue());

            this.ddlReligion.DataSource = fields;
            this.ddlReligion.DataTextField = "FieldValue";
            this.ddlReligion.DataValueField = "FieldValue";
            this.ddlReligion.DataBind();
        }
        private void LoadBloodGroup()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("BloodGroup", hmUtility.GetDropDownFirstValue());

            this.ddlBloodGroup.DataSource = fields;
            this.ddlBloodGroup.DataTextField = "FieldValue";
            this.ddlBloodGroup.DataValueField = "FieldValue";
            this.ddlBloodGroup.DataBind();
        }
        private void LoadLocation()
        {
            LocationDA locationDA = new LocationDA();
            List<LocationBO> entityBOList = new List<LocationBO>();
            entityBOList = locationDA.GetLocationInfo();

            this.ddlPrfJobLocation.DataSource = entityBOList;
            this.ddlPrfJobLocation.DataTextField = "LocationName";
            this.ddlPrfJobLocation.DataValueField = "LocationId";
            this.ddlPrfJobLocation.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlPrfJobLocation.Items.Insert(0, item);
        }
        private void LoadJobCategory()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("EmpJobCategory", hmUtility.GetDropDownFirstValue());

            this.ddlJobCategory.DataSource = fields;
            this.ddlJobCategory.DataTextField = "FieldValue";
            this.ddlJobCategory.DataValueField = "FieldId";
            this.ddlJobCategory.DataBind();
        }
        private void LoadOrganizationType()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("EmpOrganizationType", hmUtility.GetDropDownFirstValue());

            this.ddlOrganizationType.DataSource = fields;
            this.ddlOrganizationType.DataTextField = "FieldValue";
            this.ddlOrganizationType.DataValueField = "FieldId";
            this.ddlOrganizationType.DataBind();
        }
        private void LoadLanguageProfiency()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("LanguageProfiency", hmUtility.GetDropDownFirstValue());

            this.ddlReading.DataSource = fields;
            this.ddlReading.DataTextField = "FieldValue";
            this.ddlReading.DataValueField = "FieldId";
            this.ddlReading.DataBind();

            this.ddlWriting.DataSource = fields;
            this.ddlWriting.DataTextField = "FieldValue";
            this.ddlWriting.DataValueField = "FieldId";
            this.ddlWriting.DataBind();

            this.ddlSpeaking.DataSource = fields;
            this.ddlSpeaking.DataTextField = "FieldValue";
            this.ddlSpeaking.DataValueField = "FieldId";
            this.ddlSpeaking.DataBind();
        }
        private bool IsFormValid()
        {
            bool status = true;
            var dependentList = Session["EmpDependentList"] as List<EmpDependentBO>;
            var nomineeList = Session["EmpNomineeList"] as List<EmpNomineeBO>;
            var experineceList = Session["EmpExperienceList"] as List<EmpExperienceBO>;
            var educationList = Session["EmpEducationList"] as List<EmpEducationBO>;
            var trainingList = Session["EmpCareerTrainingList"] as List<EmpCareerTrainingBO>;
            var languageList = Session["EmpLanguageList"] as List<EmpLanguageBO>;
            var referenceList = Session["EmpReferenceList"] as List<EmpReferenceBO>;
            //EmployeeTab
            //if (string.IsNullOrEmpty(this.txtEmpCode.Text))
            //{
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Applicant Code.", AlertType.Warning);
            //    status = false;
            //    txtEmpCode.Focus();
            //    SetTab("DetailsTab");
            //}
            if (!string.IsNullOrEmpty(hfEmpCode.Value))
            {
                txtEmpCode.Text = hfEmpCode.Value;
            }
            if (string.IsNullOrEmpty(this.txtFirstName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "First Name.", AlertType.Warning);
                status = false;
                txtFirstName.Focus();
                SetTab("DetailsTab");
            }
            else if (string.IsNullOrEmpty(this.txtLastName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Last Name.", AlertType.Warning);
                status = false;
                txtLastName.Focus();
                SetTab("DetailsTab");
            }
            else if (string.IsNullOrEmpty(this.txtEmpDateOfBirth.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Date Of Birth.", AlertType.Warning);
                status = false;
                txtEmpDateOfBirth.Focus();
                SetTab("DetailsTab");
            }
            else if (ddlGender.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Your Gender.", AlertType.Warning);
                status = false;
                ddlGender.Focus();
                SetTab("DetailsTab");
            }
            else if (ddlReligion.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Your Religion.", AlertType.Warning);
                status = false;
                ddlReligion.Focus();
                SetTab("DetailsTab");
            }
            else if (ddlMaritalStatus.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Your Marital Status.", AlertType.Warning);
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
            else if (ddlCurrency.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Prefered Currency.", AlertType.Warning);
                status = false;
                SetTab("CareerInfoTab");
            }
            else if (ddlJobCategory.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Prefered Job Category.", AlertType.Warning);
                status = false;
                SetTab("CareerInfoTab");
            }
            else if (ddlOrganizationType.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Prefered Organization Type.", AlertType.Warning);
                status = false;
                SetTab("CareerInfoTab");
            }
            //else if (ddlPrfJobLocation.SelectedIndex == 0)
            //{
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Prefered Job Location.", AlertType.Warning);
            //    status = false;
            //    SetTab("CareerInfoTab");
            //}

            if (!string.IsNullOrEmpty(txtFirstName.Text))
            {
                Regex regex = new Regex(@"[^0-9^+^\-^\/^\*^\(^\)]");
                MatchCollection matches = regex.Matches(txtFirstName.Text);
                if (matches.Count == 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, "First Name is inconsistent.", AlertType.Warning);
                    status = false;
                    txtFirstName.Focus();
                    SetTab("DetailsTab");
                }
            }
            if (!string.IsNullOrEmpty(txtLastName.Text))
            {
                Regex regex = new Regex(@"[^0-9^+^\-^\/^\*^\(^\)]");
                MatchCollection matches = regex.Matches(txtLastName.Text);
                if (matches.Count == 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Last Name is inconsistent.", AlertType.Warning);
                    status = false;
                    txtLastName.Focus();
                    SetTab("DetailsTab");
                }
            }
            if (!string.IsNullOrEmpty(txtFathersName.Text))
            {
                Regex regex = new Regex(@"[^0-9^+^\-^\/^\*^\(^\)]");
                MatchCollection matches = regex.Matches(txtFathersName.Text);
                if (matches.Count == 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Father's Name is inconsistent.", AlertType.Warning);
                    status = false;
                    txtFathersName.Focus();
                    SetTab("DetailsTab");
                }
            }
            if (!string.IsNullOrEmpty(txtMothersName.Text))
            {
                Regex regex = new Regex(@"[^0-9^+^\-^\/^\*^\(^\)]");
                MatchCollection matches = regex.Matches(txtMothersName.Text);
                if (matches.Count == 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Mother's Name is inconsistent.", AlertType.Warning);
                    status = false;
                    txtMothersName.Focus();
                    SetTab("DetailsTab");
                }
            }
            if (!string.IsNullOrEmpty(txtPresentSalary.Text))
            {
                if (!Regex.IsMatch(txtPresentSalary.Text, "^\\£?[+-]?[\\d,]*\\.?\\d{0,2}$"))
                {
                    CommonHelper.AlertInfo(innboardMessage, "Present Salary is not in correct format.", AlertType.Warning);
                    status = false;
                    txtPresentSalary.Focus();
                    SetTab("CareerInfoTab");
                }
            }
            if (!string.IsNullOrEmpty(txtExpectedSlary.Text))
            {
                if (!Regex.IsMatch(txtPresentSalary.Text, "^\\£?[+-]?[\\d,]*\\.?\\d{0,2}$"))
                {
                    CommonHelper.AlertInfo(innboardMessage, "Expected Salary is not in correct format.", AlertType.Warning);
                    status = false;
                    txtExpectedSlary.Focus();
                    SetTab("CareerInfoTab");
                }
            }

            return status;
        }
        private bool IsEducationValid()
        {
            bool status = true;
            if (string.IsNullOrEmpty(this.txtExamName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Examination Name.", AlertType.Warning);
                status = false;
                txtExamName.Focus();

            }
            else if (string.IsNullOrEmpty(this.txtInstituteName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Institute Name.", AlertType.Warning);
                txtInstituteName.Focus();
                status = false;

            }
            else if (string.IsNullOrEmpty(this.txtPassYear.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Passing Year.", AlertType.Warning);
                status = false;
                txtPassYear.Focus();

            }
            //else if (string.IsNullOrEmpty(this.txtPassClass.Text))
            //{
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "GPA Or Class.", AlertType.Warning);
            //    status = false;
            //    txtPassClass.Focus();

            //}
            if (!string.IsNullOrEmpty(this.txtPassYear.Text))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtPassYear.Text, "^[0-9]*$"))
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Passing Year.", AlertType.Warning);
                    status = false;
                    txtDuration.Focus();
                }
            }

            SetTab("EducationTab");
            return status;

        }
        private bool IsExperienceValid()
        {
            bool status = true;
            if (string.IsNullOrEmpty(this.txtCompanyName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Company Name.", AlertType.Warning);
                status = false;
                txtCompanyName.Focus();
            }
            else if (string.IsNullOrEmpty(this.txtJoinDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Join Date.", AlertType.Warning);
                status = false;
                txtJoinDate.Focus();
            }
            SetTab("ExperienceTab");
            return status;

        }
        private bool IsReferenceValid()
        {
            bool status = true;
            if (string.IsNullOrEmpty(this.txtName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Name.", AlertType.Warning);
                status = false;
                txtName.Focus();
            }
            else if (string.IsNullOrEmpty(this.txtOrganization.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Organization Name.", AlertType.Warning);
                txtOrganization.Focus();
                status = false;
            }
            else if (string.IsNullOrEmpty(this.txtDesignation.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Designation.", AlertType.Warning);
                status = false;
                txtDesignation.Focus();
            }
            SetTab("ReferenceTab");
            return status;
        }
        private bool IsTrainingValid()
        {
            bool status = true;
            if (string.IsNullOrEmpty(this.txtTrainingTitle.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Training Title.", AlertType.Warning);
                status = false;
                txtTrainingTitle.Focus();
            }
            else if (string.IsNullOrEmpty(this.txtInstitute.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Institute Name.", AlertType.Warning);
                txtInstitute.Focus();
                status = false;
            }
            else if (string.IsNullOrEmpty(this.txtTrainingYear.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Training Year.", AlertType.Warning);
                status = false;
                txtTrainingYear.Focus();
            }
            else if (string.IsNullOrEmpty(this.txtDuration.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Duration.", AlertType.Warning);
                status = false;
                txtDuration.Focus();
            }
            if (!string.IsNullOrEmpty(this.txtDuration.Text))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtDuration.Text, "^[0-9]*$"))
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Duration.", AlertType.Warning);
                    status = false;
                    txtDuration.Focus();
                }
            }
            if (!string.IsNullOrEmpty(this.txtTrainingYear.Text))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtTrainingYear.Text, "^[0-9]*$"))
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Training Year.", AlertType.Warning);
                    status = false;
                    txtDuration.Focus();
                }
            }
            SetTab("CareerTrainingTab");
            return status;
        }
        private bool IsLanguageValid()
        {
            bool status = true;
            if (string.IsNullOrEmpty(this.txtLanguage.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Language.", AlertType.Warning);
                status = false;
                txtLanguage.Focus();
            }
            else if (ddlReading.SelectedItem.Text == "--- Please Select ---")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Reading Profiency.", AlertType.Warning);
                status = false;
                ddlReading.Focus();
            }
            else if (ddlWriting.SelectedItem.Text == "--- Please Select ---")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Writing Profiency.", AlertType.Warning);
                status = false;
                ddlWriting.Focus();
            }
            else if (ddlSpeaking.SelectedItem.Text == "--- Please Select ---")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Speaking Profiency.", AlertType.Warning);
                status = false;
                ddlSpeaking.Focus();
            }
            SetTab("LanguageTab");
            return status;
        }
        private void SetTab(string TabName)
        {
            if (TabName == "DetailsTab")
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
            }
            else if (TabName == "EducationTab")
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
            }
            else if (TabName == "ExperienceTab")
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
            }
            else if (TabName == "CareerInfoTab")
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
            }
            else if (TabName == "CareerTrainingTab")
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
            }
            else if (TabName == "LanguageTab")
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
            }
            else if (TabName == "ReferenceTab")
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
            }
            else if (TabName == "DocumentsTab")
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
            }

        }
        private void LoadGridView()
        {
            EmployeeDA da = new EmployeeDA();
            List<EmployeeBO> files = da.GetEmployeeInfo();

            var ds = files.OrderByDescending(x => x.EmpId).ToList();
            //this.gvEmployee.DataSource = ds;
            //this.gvEmployee.DataBind();
        }
        private void FillUploadedFile(int EmployeeId)
        {
            DocumentsDA docDA = new DocumentsDA();
            List<DocumentsBO> docBO = new List<DocumentsBO>();
            //docBO = docDA.GetDocumentsInfoByDocCategoryAndOwnerId("Applicant Signature", EmployeeId);
            //Session["_EmployeeDocuments"] = docBO;
            //docBO = docDA.GetDocumentsInfoByDocCategoryAndOwnerId("Applicant Document", EmployeeId);
            //Session["_EmployeeDocuments"] = docBO;
            docBO = docDA.GetDocumentsInfoByDocCategoryAndOwnerId("Applicant Other Documents", EmployeeId);
            //foreach (DocumentsBO dc in docBO)
            //{
            //    dc.Name = dc.Name; 
            //    dc.Path = (dc.Path + dc.Name);
            //    Session["_EmployeeOtherDocuments"] = dc;
            //}
            Session["_EmployeeDocuments"] = docBO;

            this.gvEmployeeDocument.DataSource = Session["_EmployeeDocuments"];
            this.gvEmployeeDocument.DataBind();
        }
        private void LoadCountryList()
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CountriesBO> countryList = commonDA.GetAllCountries();

            ddlPresentCountry.DataSource = countryList;
            ddlPresentCountry.DataTextField = "CountryName";
            ddlPresentCountry.DataValueField = "CountryId";
            ddlPresentCountry.DataBind();
            //string bangladesh = "19";
            //ddlPresentCountry.SelectedValue = bangladesh;

            ddlPermanentCountry.DataSource = countryList;
            ddlPermanentCountry.DataTextField = "CountryName";
            ddlPermanentCountry.DataValueField = "CountryId";
            ddlPermanentCountry.DataBind();
            //string bangladesh = "19";
            //ddlPermanentCountry.SelectedValue = bangladesh;

            ListItem item = new ListItem
            {
                Value = "0",
                Text = hmUtility.GetDropDownFirstValue()
            };
            ddlPresentCountry.Items.Insert(0, item);
            ddlPermanentCountry.Items.Insert(0, item);
        }
        private void FillForm(int EditId)
        {
            //Master Information------------------------
            EmployeeBO bo = new EmployeeBO();
            EmployeeDA da = new EmployeeDA();
            bo = da.GetEmployeeInfoById(EditId);
            Session["_EmployeeId"] = bo.EmpId;
            RandomEmpId.Value = bo.EmpId.ToString();
            tempEmpId.Value = bo.EmpId.ToString();
            this.FileUpload();
            this.txtEmpCode.Text = bo.EmpCode;
            hfEmpCode.Value = bo.EmpCode;

            this.txtDisplayName.Text = bo.DisplayName;
            this.txtFirstName.Text = bo.FirstName;
            this.txtLastName.Text = bo.LastName;
            //this.txtJoinDate.Text = hmUtility.GetStringFromDateTime(bo.JoinDate);
            this.txtFathersName.Text = bo.FathersName;
            this.txtMothersName.Text = bo.MothersName;
            if (bo.EmpDateOfBirth != null)
            {
                this.txtEmpDateOfBirth.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(bo.EmpDateOfBirth));
            }
            else
            {
                this.txtEmpDateOfBirth.Text = string.Empty;
            }
            //this.txtEmpDateOfBirth.Text = hmUtility.GetStringFromDateTime(bo.EmpDateOfBirth);
            if (!string.IsNullOrWhiteSpace(bo.Gender.ToString()))
            {
                this.ddlGender.SelectedValue = bo.Gender.ToString();
            }
            else
            {
                this.ddlGender.SelectedIndex = 0;
            }
            this.ddlReligion.SelectedValue = bo.Religion.ToString();
            this.ddlMaritalStatus.SelectedValue = bo.MaritalStatus.ToString();
            if (!string.IsNullOrWhiteSpace(bo.BloodGroup.ToString()))
            {
                this.ddlBloodGroup.SelectedValue = bo.BloodGroup.ToString();
            }
            else
            {
                this.ddlBloodGroup.SelectedIndex = 0;
            }

            this.txtHeight.Text = bo.Height;
            this.ddlCountryId.SelectedValue = bo.CountryId.ToString();
            this.txtNationalId.Text = bo.NationalId;
            this.txtPassportNumber.Text = bo.PassportNumber;

            this.txtPIssuePlace.Text = bo.PIssuePlace;
            //this.txtPIssueDate.Text = bo.PIssueDate.ToString();
            //this.txtPExpireDate.Text = bo.PExpireDate.ToString();
            if (bo.PIssueDate != null)
            {
                this.txtPIssueDate.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(bo.PIssueDate));
            }
            else
            {
                txtPIssueDate.Text = string.Empty;
            }
            if (bo.PExpireDate != null)
            {
                this.txtPExpireDate.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(bo.PExpireDate));
            }
            else
            {
                txtPExpireDate.Text = string.Empty;
            }
            //this.ddlCurrentLocation.SelectedValue = bo.CurrentLocationId.ToString();
            this.txtPresentAddress.Text = bo.PresentAddress;
            this.txtPresentCity.Text = bo.PresentCity;
            this.txtPresentZipCode.Text = bo.PresentZipCode;
            //this.txtPresentCountry.Text = bo.PresentCountry;
            ddlPresentCountry.SelectedValue = bo.PresentCountryId.ToString();
            this.txtPresentPhone.Text = bo.PresentPhone;
            this.txtPermanentAddress.Text = bo.PermanentAddress;
            this.txtPermanentCity.Text = bo.PermanentCity;
            this.txtPermanentZipCode.Text = bo.PermanentZipCode;
            //this.txtPermanentCountry.Text = bo.PermanentCountry;
            ddlPermanentCountry.SelectedValue = bo.PermanentCountryId.ToString();
            this.txtPermanentPhone.Text = bo.PermanentPhone;
            this.txtPersonalEmail.Text = bo.PersonalEmail;
            this.txtAlternativeEmail.Text = bo.AlternativeEmail;

            this.btnSave.Text = "Update";

            //Education Information------------------------
            List<EmpEducationBO> eduBO = new List<EmpEducationBO>();
            EmpEducationDA eduDA = new EmpEducationDA();

            eduBO = eduDA.GetEmpEducationByEmpId(EditId);
            Session["EmpEducationList"] = eduBO;

            this.gvEmpEducation.DataSource = Session["EmpEducationList"] as List<EmpEducationBO>;
            this.gvEmpEducation.DataBind();

            //Experience Information------------------------
            List<EmpExperienceBO> expBO = new List<EmpExperienceBO>();
            EmpExperienceDA expDA = new EmpExperienceDA();

            expBO = expDA.GetEmpExperienceByEmpId(EditId);
            Session["EmpExperienceList"] = expBO;

            this.gvExperience.DataSource = Session["EmpExperienceList"] as List<EmpExperienceBO>;
            this.gvExperience.DataBind();

            //Career Information------------------------
            EmpCareerInfoBO careerInfo = new EmpCareerInfoBO();
            EmpCareerInfoDA careerDA = new EmpCareerInfoDA();
            careerInfo = careerDA.GetEmpCareerInfoByEmpId(bo.EmpId);
            if (careerInfo != null)
            {
                if (careerInfo.CareerInfoId > 0)
                {
                    txtObjective.Text = careerInfo.Objective;
                    txtPresentSalary.Text = careerInfo.PresentSalary.ToString();
                    txtExpectedSlary.Text = careerInfo.ExpectedSalary.ToString();
                    ddlCurrency.SelectedValue = careerInfo.Currency.ToString();
                    ddlJobLevel.SelectedValue = careerInfo.JobLevel;
                    ddlAvailableType.SelectedValue = careerInfo.AvailableType;
                    ddlJobCategory.SelectedValue = careerInfo.PreferedJobType.ToString();
                    ddlOrganizationType.SelectedValue = careerInfo.PreferedOrganizationType.ToString();
                    ddlPrfJobLocation.SelectedValue = careerInfo.PreferedJobLocationId.ToString();
                    //txtLanguage.Text = careerInfo.Language;
                    txtCareerSummary.Text = careerInfo.CareerSummary;
                    txtExtraActivities.Text = careerInfo.ExtraCurriculmActivities;
                    hfEmpCareerInfoId.Text = careerInfo.CareerInfoId.ToString();
                }
            }

            //Career Training Information------------------------
            List<EmpCareerTrainingBO> trainingBO = new List<EmpCareerTrainingBO>();
            EmpCareerTrainingDA trainingDA = new EmpCareerTrainingDA();

            trainingBO = trainingDA.GetEmpCareerTrainingnByEmpId(EditId);
            Session["EmpCareerTrainingList"] = trainingBO;

            this.gvEmpCareerTraining.DataSource = Session["EmpCareerTrainingList"] as List<EmpCareerTrainingBO>;
            this.gvEmpCareerTraining.DataBind();

            //Language Information------------------------
            List<EmpLanguageBO> languageBO = new List<EmpLanguageBO>();
            EmpLanguageDA languageDA = new EmpLanguageDA();

            languageBO = languageDA.GetEmpLanguageByEmpId(EditId);
            Session["EmpLanguageList"] = languageBO;

            this.gvEmpLanguage.DataSource = Session["EmpLanguageList"] as List<EmpLanguageBO>;
            this.gvEmpLanguage.DataBind();

            //Reference Information------------------------            
            List<EmpReferenceBO> referenceBO = new List<EmpReferenceBO>();
            EmpReferenceDA referenceDA = new EmpReferenceDA();

            referenceBO = referenceDA.GetEmpReferenceByEmpId(EditId);
            Session["EmpReferenceList"] = referenceBO;

            this.gvEmpReference.DataSource = Session["EmpReferenceList"] as List<EmpReferenceBO>;
            this.gvEmpReference.DataBind();

            FillUploadedFile(EditId);
            //this.SigDiv
        }
        private void Cancel()
        {
            Session["_EmployeeId"] = null;
            Session["_BankInfoId"] = null;
            this.txtEmpCode.Text = string.Empty;
            hfEmpCode.Value = string.Empty;
            this.txtDisplayName.Text = string.Empty;
            this.txtFirstName.Text = string.Empty;
            this.txtLastName.Text = string.Empty;
            this.txtFathersName.Text = string.Empty;
            this.txtMothersName.Text = string.Empty;
            this.txtEmpDateOfBirth.Text = string.Empty;
            this.ddlGender.SelectedValue = hmUtility.GetDropDownFirstValue();
            this.ddlReligion.SelectedValue = hmUtility.GetDropDownFirstValue();
            this.ddlMaritalStatus.SelectedValue = hmUtility.GetDropDownFirstValue();
            this.ddlBloodGroup.SelectedValue = hmUtility.GetDropDownFirstValue();
            this.txtHeight.Text = string.Empty;
            this.ddlCountryId.SelectedIndex = 0;
            this.txtNationalId.Text = string.Empty;
            this.txtPassportNumber.Text = string.Empty;
            this.txtPIssuePlace.Text = string.Empty;
            this.txtPIssueDate.Text = string.Empty;
            this.txtPExpireDate.Text = string.Empty;
            this.txtPresentAddress.Text = string.Empty;
            this.txtPresentCity.Text = string.Empty;
            this.txtPresentZipCode.Text = string.Empty;
            //this.txtPresentCountry.Text = string.Empty;
            ddlPresentCountry.SelectedValue = "0";
            this.txtPresentPhone.Text = string.Empty;
            this.txtPermanentAddress.Text = string.Empty;
            this.txtPermanentCity.Text = string.Empty;
            this.txtPermanentZipCode.Text = string.Empty;
            //this.txtPermanentCountry.Text = string.Empty;
            ddlPermanentCountry.SelectedValue = "0";
            this.txtPermanentPhone.Text = string.Empty;
            this.txtPersonalEmail.Text = string.Empty;
            this.btnSave.Text = "Save";
            Session["arrayEducationDelete"] = null;
            Session["arrayExperienceDelete"] = null;
            Session["arrayCareerTrainingDelete"] = null;
            Session["arrayLanguageDelete"] = null;
            Session["arrayReferenceDelete"] = null;
            Session["EmpExperienceList"] = null;
            Session["EmpEducationList"] = null;
            Session["EmpCareerTrainingList"] = null;
            Session["EmpLanguageList"] = null;
            Session["EmpReferenceList"] = null;

            this.gvEmpEducation.DataSource = Session["EmpEducationList"] as List<EmpEducationBO>;
            this.gvEmpEducation.DataBind();
            this.ClearEmpEducation();

            this.gvExperience.DataSource = Session["EmpExperienceList"] as List<EmpExperienceBO>;
            this.gvExperience.DataBind();
            this.ClearEmpExperience();

            hfEmpCareerInfoId.Text = string.Empty;

            this.gvEmpCareerTraining.DataSource = Session["EmpCareerTrainingList"] as List<EmpCareerTrainingBO>;
            this.gvEmpCareerTraining.DataBind();
            this.ClearEmpCareerTraining();

            this.gvEmpLanguage.DataSource = Session["EmpLanguageList"] as List<EmpLanguageBO>;
            this.gvEmpLanguage.DataBind();
            this.ClearEmpLanguage();

            this.gvEmpReference.DataSource = Session["EmpReferenceList"] as List<EmpReferenceBO>;
            this.gvEmpReference.DataBind();
            this.ClearEmpReference();

            ClearCareerInfo();
        }
        //------Common For All------------------------
        private void AddEditODeleteDetail()
        {
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

            //Language------------
            if (Session["arrayLanguageDelete"] == null)
            {
                arrayLanguageDelete = new ArrayList();
                Session.Add("arrayLanguageDelete", arrayLanguageDelete);
            }
            else
            {
                arrayLanguageDelete = Session["arrayLanguageDelete"] as ArrayList;
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
            this.txtExamName.Text = string.Empty;
            this.txtInstituteName.Text = string.Empty;
            this.txtSubjectName.Text = string.Empty;
            this.txtPassYear.Text = string.Empty;
            this.txtPassClass.Text = string.Empty;
            hfEducationId.Text = string.Empty;
        }
        //-------Employee Experience------------------
        private void ClearEmpExperience()
        {
            btnEmpExperience.Text = "Add";
            this.txtCompanyName.Text = string.Empty;
            txtCompanyUrl.Text = string.Empty;
            this.txtJoinDate.Text = string.Empty;
            this.txtJoinDesignation.Text = string.Empty;
            this.txtLeaveDate.Text = string.Empty;
            this.txtLeaveDesignation.Text = string.Empty;
            this.txtAchievements.Text = string.Empty;
            hfExperienceId.Text = string.Empty;
        }
        private void ClearEmpCareerTraining()
        {
            btnEmpCareerTraining.Text = "Add";
            txtTrainingTitle.Text = string.Empty;
            txtTopic.Text = string.Empty;
            txtInstitute.Text = string.Empty;
            ddlCountry.SelectedIndex = 0;
            txtLocation.Text = string.Empty;
            txtTrainingYear.Text = string.Empty;
            txtDuration.Text = string.Empty;
            hfCareerTrainingId.Text = string.Empty;
        }
        private void ClearEmpLanguage()
        {
            btnEmpLanguage.Text = "Add";
            txtLanguage.Text = string.Empty;
            ddlReading.SelectedIndex = 0;
            ddlWriting.SelectedIndex = 0;
            ddlSpeaking.SelectedIndex = 0;
            hfLanguageId.Text = string.Empty;
        }
        private void ClearEmpReference()
        {
            btnEmpReference.Text = "Add";
            txtName.Text = string.Empty;
            txtOrganization.Text = string.Empty;
            txtDesignation.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtMobile.Text = string.Empty;
            txtRelation.Text = string.Empty;
            hfReferenceId.Text = string.Empty;
        }
        private void ClearCareerInfo()
        {
            txtObjective.Text = string.Empty;
            txtPresentSalary.Text = string.Empty;
            txtExpectedSlary.Text = string.Empty;
            ddlCurrency.SelectedIndex = 0;
            ddlJobLevel.SelectedIndex = 0;
            ddlAvailableType.SelectedIndex = 0;
            ddlJobCategory.SelectedIndex = 0;
            ddlOrganizationType.SelectedIndex = 0;
            ddlPrfJobLocation.SelectedIndex = 0;
            //txtLanguage.Text = string.Empty;
            txtCareerSummary.Text = string.Empty;
            txtExtraActivities.Text = string.Empty;
        }
        private void ClearSession()
        {
            Session["arrayEducationDelete"] = null;
            Session["arrayExperienceDelete"] = null;
            Session["arrayCareerTrainingDelete"] = null;
            Session["arrayLanguageDelete"] = null;
            Session["arrayReferenceDelete"] = null;
            Session["EmpExperienceList"] = null;
            Session["EmpEducationList"] = null;
            Session["EmpCareerTrainingList"] = null;
            Session["EmpLanguageList"] = null;
            Session["EmpReferenceList"] = null;
        }
        //************************ User Defined WebMethod ********************//
        [WebMethod]
        public static string GetUploadedImageByWebMethod(int OwnerId, string docType)
        {
            string strTable = "";
            DocumentsDA docDA = new DocumentsDA();
            var docList = docDA.GetDocumentsInfoByDocCategoryAndOwnerId(docType, OwnerId);
            if (docList.Count > 0)
            {
                //var Image = docList[docList.Count - 1];
                var Image = docList[0];

                strTable += "<img src='" + Image.Path + Image.Name + "'  alt='No Image Selected' border='0' />";
            }
            return strTable;
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
        public static GridViewDataNPaging<EmployeeBO, GridPaging> SearchNLoadEmpInformation(string empName, string code, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;
            bool isApplicant = true;
            string department = string.Empty, designation = string.Empty;

            GridViewDataNPaging<EmployeeBO, GridPaging> myGridData = new GridViewDataNPaging<EmployeeBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetEmpInformationBySearchCriteriaForPaging(0, 0, empName, code, department, designation, isApplicant, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<EmployeeBO> distinctItems = new List<EmployeeBO>();
            distinctItems = empList.GroupBy(test => test.EmpId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static List<DocumentsBO> GetUploadedDocumentsByWebMethod(int OwnerId, string docType)
        {
            DocumentsDA docDA = new DocumentsDA();
            List<DocumentsBO> docBO = new List<DocumentsBO>();

            docBO = docDA.GetDocumentsInfoByDocCategoryAndOwnerId("Applicant Other Documents", OwnerId);

            //DocumentsDA docDA = new DocumentsDA();
            //var docList = docDA.GetDocumentsInfoByDocCategoryAndOwnerId(docType, OwnerId);
            docBO = new HMCommonDA().GetDocumentListWithIcon(docBO);
            return docBO;
        }
    }
}