using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.Membership;
using HotelManagement.Data.Membership;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using System.Collections;
using System.Web.Services;
using System.Text.RegularExpressions;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.GeneralLedger;

namespace HotelManagement.Presentation.Website.Membership
{
    public partial class frmMemMembersBasics : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        ArrayList arrayReferenceDelete;
        ArrayList arrayFamilyMemberDelete;
        ArrayList arrayEducationDelete;
        private int isNewChartOfAccountsHeadCreateForMember = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            AddEditOrDeleteDetail();
            if (!IsPostBack)
            {
                ClearSession();
                LoadMaritualStatus();
                LoadGender();
                LoadBloodGroup();
                LoadCountry();
                LoadRelationship();
                LoadMemberType();
                LoadReligion();
                string editId = Request.QueryString["editId"];
                if (!string.IsNullOrWhiteSpace(editId))
                {
                    int Id = Convert.ToInt32(editId);
                    if (Id > 0)
                    {
                        FillForm(Id);
                        SetTab("BasicTab");
                    }
                }

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

                isNewChartOfAccountsHeadCreateForMember = 1;
                HMCommonSetupBO commonSetupNewCOABO = new HMCommonSetupBO();
                commonSetupNewCOABO = commonSetupDA.GetCommonConfigurationInfo("IsNewChartOfAccountsHeadCreateForMember", "IsNewChartOfAccountsHeadCreateForMember");
                if (commonSetupNewCOABO != null)
                {
                    isNewChartOfAccountsHeadCreateForMember = Convert.ToInt32(commonSetupNewCOABO.SetupValue);
                }
            }
        }
        //**************************** Handlers ****************************//
        protected void btnReference_Click(object sender, EventArgs e)
        {
            if (!IsReferenceValid())
            {
                return;
            }

            int dynamicReferenceId = 0;
            List<MemMemberReferenceBO> referenceList = Session["ReferenceList"] == null ? new List<MemMemberReferenceBO>() : Session["ReferenceList"] as List<MemMemberReferenceBO>;

            if (!string.IsNullOrWhiteSpace(hfReferenceId.Text))
                dynamicReferenceId = Convert.ToInt32(hfReferenceId.Text);

            MemMemberReferenceBO referenceBO = dynamicReferenceId == 0 ? new MemMemberReferenceBO() : referenceList.Where(x => x.ReferenceId == dynamicReferenceId).FirstOrDefault();
            if (referenceList.Contains(referenceBO))
                referenceList.Remove(referenceBO);

            referenceBO.Arbitrator = txtArbitrator.Text;
            referenceBO.ArbitratorMode = ddlArbitratorMode.SelectedValue;
            referenceBO.Relationship = ddlArbRelationship.SelectedValue;
            referenceBO.ReferenceId = dynamicReferenceId == 0 ? referenceList.Count + 1 : dynamicReferenceId;
            referenceList.Add(referenceBO);
            Session["ReferenceList"] = referenceList;
            this.gvReference.DataSource = Session["ReferenceList"] as List<MemMemberReferenceBO>;
            this.gvReference.DataBind();
            ClearReference();

            SetTab("ReferenceTab");
        }
        protected void gvReference_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _ReferenceId;

            if (e.CommandName == "CmdEdit")
            {
                _ReferenceId = Convert.ToInt32(e.CommandArgument.ToString());
                hfReferenceId.Text = _ReferenceId.ToString();
                var referenceList = (List<MemMemberReferenceBO>)Session["ReferenceList"];
                var referenceBO = referenceList.Where(x => x.ReferenceId == _ReferenceId).FirstOrDefault();
                if (referenceBO != null && referenceBO.ReferenceId > 0)
                {
                    txtArbitrator.Text = referenceBO.Arbitrator;
                    ddlArbitratorMode.SelectedValue = referenceBO.ArbitratorMode;
                    ddlArbRelationship.SelectedValue = referenceBO.Relationship;
                    btnReference.Text = "Edit";
                }
                else
                {
                    btnReference.Text = "Add";
                }
            }
            else if (e.CommandName == "CmdDelete")
            {
                _ReferenceId = Convert.ToInt32(e.CommandArgument.ToString());
                hfReferenceId.Text = string.Empty;
                var referenceList = (List<MemMemberReferenceBO>)Session["ReferenceList"];
                var referenceBO = referenceList.Where(x => x.ReferenceId == _ReferenceId).FirstOrDefault();
                referenceList.Remove(referenceBO);
                Session["ReferenceList"] = referenceList;
                arrayReferenceDelete.Add(_ReferenceId);
                this.gvReference.DataSource = Session["ReferenceList"] as List<MemMemberReferenceBO>;
                this.gvReference.DataBind();
            }
            SetTab("ReferenceTab");
        }
        protected void btnFamilyMember_Click(object sender, EventArgs e)
        {
            if (!IsFamilyMemberValid())
            {
                return;
            }

            int dynamicFMemberId = 0;
            List<MemMemberFamilyMemberBO> familyMemberList = Session["FamilyMemberList"] == null ? new List<MemMemberFamilyMemberBO>() : Session["FamilyMemberList"] as List<MemMemberFamilyMemberBO>;

            if (!string.IsNullOrWhiteSpace(hfFamilyMemberId.Text))
                dynamicFMemberId = Convert.ToInt32(hfFamilyMemberId.Text);

            MemMemberFamilyMemberBO familyMemberBO = dynamicFMemberId == 0 ? new MemMemberFamilyMemberBO() : familyMemberList.Where(x => x.Id == dynamicFMemberId).FirstOrDefault();
            if (familyMemberList.Contains(familyMemberBO))
                familyMemberList.Remove(familyMemberBO);

            familyMemberBO.MemberName = txtMemberName.Text;
            if (!string.IsNullOrWhiteSpace(txtDOB.Text))
            {
                familyMemberBO.MemberDOB = CommonHelper.DateTimeToMMDDYYYY(txtDOB.Text);
            }
            familyMemberBO.Relationship = ddlRelationship.SelectedValue;
            familyMemberBO.Occupation = txtFMOccupation.Text;
            familyMemberBO.UsageMode = ddlUsageMode.SelectedValue;
            familyMemberBO.Id = dynamicFMemberId == 0 ? familyMemberList.Count + 1 : dynamicFMemberId;
            familyMemberList.Add(familyMemberBO);
            Session["FamilyMemberList"] = familyMemberList;
            this.gvFamilyMember.DataSource = Session["FamilyMemberList"] as List<MemMemberFamilyMemberBO>;
            this.gvFamilyMember.DataBind();
            ClearFamilyMember();

            SetTab("FamilyMemberTab");
        }
        protected void gvFamilyMember_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _FamilyMemberId;

            if (e.CommandName == "CmdEdit")
            {
                _FamilyMemberId = Convert.ToInt32(e.CommandArgument.ToString());
                hfFamilyMemberId.Text = _FamilyMemberId.ToString();
                var familyMemberList = (List<MemMemberFamilyMemberBO>)Session["FamilyMemberList"];
                var familyMemberBO = familyMemberList.Where(x => x.Id == _FamilyMemberId).FirstOrDefault();
                if (familyMemberBO != null && familyMemberBO.Id > 0)
                {
                    txtMemberName.Text = familyMemberBO.MemberName;
                    txtDOB.Text = familyMemberBO.MemberDOB.ToString();
                    txtDOB.Text = hmUtility.GetStringFromDateTime(Convert.ToDateTime(familyMemberBO.MemberDOB));
                    ddlRelationship.SelectedValue = familyMemberBO.Relationship;
                    txtFMOccupation.Text = familyMemberBO.Occupation;
                    ddlUsageMode.SelectedValue = familyMemberBO.UsageMode;
                    btnFamilyMember.Text = "Edit";
                }
                else
                {
                    btnFamilyMember.Text = "Add";
                }
            }
            else if (e.CommandName == "CmdDelete")
            {
                _FamilyMemberId = Convert.ToInt32(e.CommandArgument.ToString());
                hfFamilyMemberId.Text = string.Empty;
                var familyMemberList = (List<MemMemberFamilyMemberBO>)Session["FamilyMemberList"];
                var familyMemberBO = familyMemberList.Where(x => x.Id == _FamilyMemberId).FirstOrDefault();
                familyMemberList.Remove(familyMemberBO);
                Session["FamilyMemberList"] = familyMemberList;
                arrayFamilyMemberDelete.Add(_FamilyMemberId);
                this.gvFamilyMember.DataSource = Session["FamilyMemberList"] as List<MemMemberFamilyMemberBO>;
                this.gvFamilyMember.DataBind();
            }
            SetTab("FamilyMemberTab");
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Membership/frmMemMembersBasics.aspx");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFormValid())
            {
                return;
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            MemMemberBasicsBO memBasic = new MemMemberBasicsBO();
            MemMemberBasicDA memBasicDA = new MemMemberBasicDA();

            memBasic.TypeId = Convert.ToInt32(ddlMemberType.SelectedValue);
            memBasic.MembershipNumber = txtMemCode.Text;
            memBasic.NameTitle = ddlTitle.SelectedValue;
            memBasic.FirstName = txtFirstName.Text;
            memBasic.MiddleName = txtMiddleName.Text;
            memBasic.LastName = txtLastName.Text;
            memBasic.FullName = txtDisplayName.Text;
            memBasic.FatherName = txtFathersName.Text;
            memBasic.MotherName = txtMothersName.Text;
            if (!string.IsNullOrWhiteSpace(txtMemDOB.Text))
            {
                memBasic.BirthDate = hmUtility.GetDateTimeFromString(txtMemDOB.Text, userInformationBO.ServerDateFormat);
            }
            if (ddlGender.SelectedIndex != 0)
            {
                memBasic.MemberGender = Convert.ToInt32(ddlGender.SelectedValue);
            }
            if (ddlBloodGroup.SelectedIndex != 0)
            {
                memBasic.BloodGroup = Convert.ToInt32(ddlBloodGroup.SelectedValue);
            }
            if (ddlMaritalStatus.SelectedIndex != 0)
            {
                memBasic.MaritalStatus = Convert.ToInt32(ddlMaritalStatus.SelectedValue);
            }
            if (ddlCountryId.SelectedIndex != 0)
            {
                memBasic.Nationality = Convert.ToInt32(ddlCountryId.SelectedValue);
            }
            memBasic.PassportNumber = txtPassportNumber.Text;
            memBasic.Occupation = txtOccupation.Text; //occupation Div
            memBasic.Organization = txtOrganization.Text;
            memBasic.Designation = txtDesignation.Text;
            
            //new
            memBasic.OfficeAddress = txtOfficeAddress.Text;
            memBasic.Height = string.IsNullOrEmpty(txtHeight.Text) ? 0 : Convert.ToInt32(txtHeight.Text);
            memBasic.Weight = string.IsNullOrEmpty(txtWeight.Text) ? 0 : Convert.ToInt32(txtWeight.Text);
            memBasic.ReligionId = Convert.ToInt32(ddlReligion.SelectedValue);
            memBasic.NomineeRelationId = Convert.ToInt32(ddlNomineeRelation.SelectedValue);
            if (!string.IsNullOrEmpty(txtNomineeDOB.Text))
            {
                memBasic.NomineeDOB = hmUtility.GetDateTimeFromString(txtNomineeDOB.Text, userInformationBO.ServerDateFormat);
            }
            memBasic.NomineeName = txtNomineeName.Text;
            memBasic.NomineeFather = txtNomineeFather.Text;
            memBasic.NomineeMother = txtNomineeMother.Text;

            if (!string.IsNullOrWhiteSpace(txtMonthlyIncome.Text))
            {
                memBasic.MonthlyIncome = Convert.ToDecimal(txtMonthlyIncome.Text);
            }
            if (!string.IsNullOrWhiteSpace(txtAnnualTurnover.Text))
            {
                memBasic.AnnualTurnover = Convert.ToDecimal(txtAnnualTurnover.Text);
            }
            if (!string.IsNullOrWhiteSpace(txtSecurityDep.Text))
            {
                memBasic.SecurityDeposit = Convert.ToDecimal(txtSecurityDep.Text);
            }
            if (!string.IsNullOrWhiteSpace(txtRegDate.Text))
            {
                memBasic.RegistrationDate = hmUtility.GetDateTimeFromString(txtRegDate.Text, userInformationBO.ServerDateFormat);
            }
            if (!string.IsNullOrWhiteSpace(txtExpiryDate.Text))
            {
                memBasic.ExpiryDate = hmUtility.GetDateTimeFromString(txtExpiryDate.Text, userInformationBO.ServerDateFormat);
            }
            memBasic.MemberAddress = txtPresentAddress.Text;
            memBasic.MailAddress = txtMailingAddress.Text;
            memBasic.MobileNumber = txtHomePhone.Text;
            memBasic.OfficePhone = txtOfficePhone.Text;
            memBasic.HomeFax = txtHomeFax.Text;
            memBasic.OfficeFax = txtOfficeFax.Text;
            memBasic.PersonalEmail = txtPersonalEmail.Text;
            memBasic.OfficeEmail = txtOfficialEmail.Text;
            
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO commonSetupNewCOABO = new HMCommonSetupBO();
            commonSetupNewCOABO = commonSetupDA.GetCommonConfigurationInfo("IsNewChartOfAccountsHeadCreateForMember", "IsNewChartOfAccountsHeadCreateForMember");
            if (commonSetupNewCOABO != null)
            {
                isNewChartOfAccountsHeadCreateForMember = Convert.ToInt32(commonSetupNewCOABO.SetupValue);
            }

            if (this.btnSave.Text.Equals("Save"))
            {
                int tmpMemberId = 0;                
                memBasic.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = memBasicDA.SaveMemberBasicInfo(memBasic, out tmpMemberId, Session["ReferenceList"] as List<MemMemberReferenceBO>, Session["FamilyMemberList"] as List<MemMemberFamilyMemberBO>, Session["EducationList"] as List<OnlineMemberEducationBO>);
                if (status)
                {
                    int tmpNodeId = 0;
                    int ancestorId = Convert.ToInt32(Convert.ToInt32(Application["CompanyAccountInfoForSalesBillPayment"].ToString()));

                    HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                    commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("MembershipAccountsHeadId", "MembershipAccountsHeadId");
                    if (commonSetupBO != null)
                    {
                        ancestorId = Convert.ToInt32(commonSetupBO.SetupValue);
                    }

                    if (isNewChartOfAccountsHeadCreateForMember != 0)
                    {
                        this.CreadeNodeMatrixAccountHeadInfo(ancestorId, out tmpNodeId);
                    }
                    else
                    {
                        tmpNodeId = ancestorId;
                    }

                    Boolean postingStatus = memBasicDA.UpdateMemberNAccountsInfo(tmpMemberId, tmpNodeId, isNewChartOfAccountsHeadCreateForMember);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.MembershipInfo.ToString(), tmpMemberId,
                        ProjectModuleEnum.ProjectModule.MembershipManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.MembershipInfo));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    Cancel();
                }
            }
            else
            {
                var memberId = hfMemberId.Value;
                var companyId = hfCompanyId.Value;
                if (memberId != string.Empty)
                {
                    memBasic.MemberId = Convert.ToInt32(memberId);
                }
                if (companyId != string.Empty)
                {
                    memBasic.CompanyId = Convert.ToInt32(companyId);
                }
                memBasic.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = memBasicDA.UpdateMemberBasicInfo(memBasic, Session["ReferenceList"] as List<MemMemberReferenceBO>, Session["arrayReferenceDelete"] as ArrayList, Session["FamilyMemberList"] as List<MemMemberFamilyMemberBO>, Session["arrayFamilyMemberDelete"] as ArrayList, Session["EducationList"] as List<OnlineMemberEducationBO>, Session["arrayEducationDelete"] as ArrayList, isNewChartOfAccountsHeadCreateForMember);
                if (status)
                {
                    if (isNewChartOfAccountsHeadCreateForMember != 0)
                    {
                        this.UpdateNodeMatrixAccountHeadInfo(Convert.ToInt32(this.txtNodeId.Value));
                    }

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.MembershipInfo.ToString(), memBasic.MemberId,
                        ProjectModuleEnum.ProjectModule.MembershipManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.MembershipInfo));
                    
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    Cancel();
                }
            }
            SetTab("BasicTab");
        }
        //************************ User Defined Function ********************//
        private void LoadMaritualStatus()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("MaritualStatus", hmUtility.GetDropDownFirstValue());
            this.ddlMaritalStatus.DataSource = fields;
            this.ddlMaritalStatus.DataTextField = "FieldValue";
            this.ddlMaritalStatus.DataValueField = "FieldId";
            this.ddlMaritalStatus.DataBind();
        }
        private void LoadGender()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("Gender", hmUtility.GetDropDownFirstValue());

            this.ddlGender.DataSource = fields;
            this.ddlGender.DataTextField = "FieldValue";
            this.ddlGender.DataValueField = "FieldId";
            this.ddlGender.DataBind();
        }
        private void LoadBloodGroup()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("BloodGroup", hmUtility.GetDropDownFirstValue());

            this.ddlBloodGroup.DataSource = fields;
            this.ddlBloodGroup.DataTextField = "FieldValue";
            this.ddlBloodGroup.DataValueField = "FieldId";
            this.ddlBloodGroup.DataBind();
        }
        private void LoadCountry()
        {
            HMCommonDA commonDA = new HMCommonDA();
            var List = commonDA.GetAllCountries();
            this.ddlCountryId.DataSource = List;
            this.ddlCountryId.DataTextField = "CountryName";
            this.ddlCountryId.DataValueField = "CountryId";
            this.ddlCountryId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCountryId.Items.Insert(0, item);
        }
        private void LoadMemberType()
        {
            MemMemberBasicDA memberDA = new MemMemberBasicDA();
            List<MemMemberTypeBO> typeList = memberDA.GetMemMemberTypeList();
            this.ddlMemberType.DataSource = typeList;
            this.ddlMemberType.DataTextField = "Name";
            this.ddlMemberType.DataValueField = "TypeId";
            this.ddlMemberType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlMemberType.Items.Insert(0, item);
        }
        private void LoadRelationship()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("Relationship", hmUtility.GetDropDownFirstValue());

            ddlArbRelationship.DataSource = fields;
            ddlArbRelationship.DataTextField = "FieldValue";
            ddlArbRelationship.DataValueField = "FieldValue";
            ddlArbRelationship.DataBind();

            ddlNomineeRelation.DataSource = fields;
            ddlNomineeRelation.DataTextField = "FieldValue";
            ddlNomineeRelation.DataValueField = "FieldId";
            ddlNomineeRelation.DataBind();

            ddlRelationship.DataSource = fields;
            ddlRelationship.DataTextField = "FieldValue";
            ddlRelationship.DataValueField = "FieldValue";
            ddlRelationship.DataBind();
        }
        private void LoadReligion()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("Relegion", hmUtility.GetDropDownFirstValue());

            ddlReligion.DataSource = fields;
            ddlReligion.DataTextField = "FieldValue";
            ddlReligion.DataValueField = "FieldId";
            ddlReligion.DataBind();

        }
        private void FillForm(int editId)
        {
            MemMemberBasicsBO memberBO = new MemMemberBasicsBO();
            MemMemberBasicDA memberBasicDA = new MemMemberBasicDA();
            memberBO = memberBasicDA.GetMemberInfoById(editId);

            this.btnSave.Text = "Update";
            hfMemberId.Value = editId.ToString();
            hfCompanyId.Value = memberBO.CompanyId.ToString();
            txtNodeId.Value = memberBO.NodeId.ToString();
            ddlMemberType.SelectedValue = memberBO.TypeId.ToString();
            txtMemCode.Text = memberBO.MembershipNumber;
            ddlTitle.SelectedValue = memberBO.NameTitle;
            txtFirstName.Text = memberBO.FirstName;
            txtMiddleName.Text = memberBO.MiddleName;
            txtLastName.Text = memberBO.LastName;
            txtDisplayName.Text = memberBO.FullName;
            txtFathersName.Text = memberBO.FatherName;
            txtMothersName.Text = memberBO.MotherName;
            if (memberBO.BirthDate != null)
            {
                this.txtMemDOB.Text = hmUtility.GetStringFromDateTime(Convert.ToDateTime(memberBO.BirthDate));
            }
            else
            {
                this.txtMemDOB.Text = string.Empty;
            }

            ddlGender.SelectedValue = memberBO.MemberGender.ToString();
            ddlBloodGroup.Text = memberBO.BloodGroup.ToString();
            ddlMaritalStatus.Text = memberBO.MaritalStatus.ToString();
            ddlCountryId.Text = memberBO.Nationality.ToString();
            txtPassportNumber.Text = memberBO.PassportNumber;
            txtOccupation.Text = memberBO.Occupation;
            txtOrganization.Text = memberBO.Organization;
            txtDesignation.Text = memberBO.Designation;
            //new
            txtOfficeAddress.Text = memberBO.OfficeAddress;
            txtHeight.Text = memberBO.Height.ToString();
            txtWeight.Text = memberBO.Weight.ToString();
            ddlReligion.SelectedValue = memberBO.ReligionId.ToString();
            ddlNomineeRelation.SelectedValue = memberBO.NomineeRelationId.ToString();
            if (memberBO.NomineeDOB != null)
            {
                txtNomineeDOB.Text = hmUtility.GetStringFromDateTime(Convert.ToDateTime(memberBO.NomineeDOB));
            }
            txtNomineeName.Text = memberBO.NomineeName;
            txtNomineeFather.Text = memberBO.NomineeFather;
            txtNomineeMother.Text = memberBO.NomineeMother;

            txtMonthlyIncome.Text = memberBO.MonthlyIncome.ToString();

            txtAnnualTurnover.Text = memberBO.AnnualTurnover.ToString();
            
            txtSecurityDep.Text = memberBO.SecurityDeposit.ToString();

            if (memberBO.RegistrationDate != null)
            {
                this.txtRegDate.Text = hmUtility.GetStringFromDateTime(Convert.ToDateTime(memberBO.RegistrationDate));
            }
            else
            {
                this.txtRegDate.Text = string.Empty;
            }

            if (memberBO.ExpiryDate != null)
            {
                this.txtExpiryDate.Text = hmUtility.GetStringFromDateTime(Convert.ToDateTime(memberBO.ExpiryDate));
            }
            else
            {
                this.txtExpiryDate.Text = string.Empty;
            }
            txtPresentAddress.Text = memberBO.MemberAddress;
            txtMailingAddress.Text = memberBO.MailAddress;
            txtHomePhone.Text = memberBO.MobileNumber;
            txtOfficePhone.Text = memberBO.OfficePhone;
            txtHomeFax.Text = memberBO.HomeFax;
            txtOfficeFax.Text = memberBO.OfficeFax;
            txtPersonalEmail.Text = memberBO.PersonalEmail;
            txtOfficialEmail.Text = memberBO.OfficeEmail;
            
            //Reference------------------------
            List<MemMemberReferenceBO> referenceList = new List<MemMemberReferenceBO>();

            referenceList = memberBasicDA.GetMemberReferenceByMemberId(editId);
            Session["ReferenceList"] = referenceList;

            this.gvReference.DataSource = Session["ReferenceList"] as List<MemMemberReferenceBO>;
            this.gvReference.DataBind();

            //Family Member----------------------
            List<MemMemberFamilyMemberBO> familyMemberList = new List<MemMemberFamilyMemberBO>();

            familyMemberList = memberBasicDA.GetMemFamilyMemberByMemberId(editId);
            Session["FamilyMemberList"] = familyMemberList;

            this.gvFamilyMember.DataSource = Session["FamilyMemberList"] as List<MemMemberFamilyMemberBO>;
            this.gvFamilyMember.DataBind();

            //Education
            List<OnlineMemberEducationBO> educationList = new List<OnlineMemberEducationBO>();

            educationList = memberBasicDA.GetOnlineMemberEducationsById(editId);
            Session["EducationList"] = educationList;

            this.gvEducationList.DataSource = Session["EducationList"] as List<OnlineMemberEducationBO>;
            this.gvEducationList.DataBind();
        }
        private void Cancel()
        {
            hfMemberId.Value = string.Empty;
            btnSave.Text = "Save";
            txtMemCode.Text = string.Empty;
            ddlTitle.SelectedIndex = 0;
            txtFirstName.Text = string.Empty;
            txtMiddleName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtDisplayName.Text = string.Empty;
            txtFathersName.Text = string.Empty;
            txtMothersName.Text = string.Empty;
            txtMemDOB.Text = string.Empty;
            ddlMemberType.SelectedIndex = 0;
            ddlGender.SelectedIndex = 0;
            ddlBloodGroup.SelectedIndex = 0;
            ddlMaritalStatus.SelectedIndex = 0;
            ddlCountryId.SelectedIndex = 0;
            txtPassportNumber.Text = string.Empty;
            txtOccupation.Text = string.Empty;
            //new 
            txtOfficeAddress.Text = string.Empty;

            txtOrganization.Text = string.Empty;
            txtDesignation.Text = string.Empty;
            txtMonthlyIncome.Text = string.Empty;
            txtAnnualTurnover.Text = string.Empty;
            txtSecurityDep.Text = string.Empty;
            txtRegDate.Text = string.Empty;
            txtExpiryDate.Text = string.Empty;
            txtPresentAddress.Text = string.Empty;
            txtMailingAddress.Text = string.Empty;
            txtHomePhone.Text = string.Empty;
            txtOfficePhone.Text = string.Empty;
            txtHomeFax.Text = string.Empty;
            txtOfficeFax.Text = string.Empty;
            txtPersonalEmail.Text = string.Empty;
            txtOfficialEmail.Text = string.Empty;
            txtHeight.Text = string.Empty;
            txtWeight.Text = string.Empty;

            Session["arrayReferenceDelete"] = null;
            Session["arrayFamilyMemberDelete"] = null;
            Session["arrayEducationDelete"] = null;
            Session["ReferenceList"] = null;
            Session["FamilyMemberList"] = null;
            Session["EducationList"] = null;

            gvReference.DataSource = Session["ReferenceList"] as List<MemMemberReferenceBO>;
            gvReference.DataBind();
            ClearReference();

            gvFamilyMember.DataSource = Session["FamilyMemberList"] as List<MemMemberFamilyMemberBO>;
            gvFamilyMember.DataBind();
            ClearFamilyMember();

            gvEducationList.DataSource = Session["EducationList"] as List<OnlineMemberEducationBO>;
            gvEducationList.DataBind();
            ClearEducation();

        }
        private void ClearReference()
        {
            btnReference.Text = "Add";
            txtArbitrator.Text = string.Empty;
            ddlArbitratorMode.SelectedIndex = 0;
            ddlArbRelationship.SelectedIndex = 0;
            hfReferenceId.Text = string.Empty;
        }
        private void ClearFamilyMember()
        {
            btnFamilyMember.Text = "Add";
            txtMemberName.Text = string.Empty;
            txtDOB.Text = string.Empty;
            ddlRelationship.SelectedIndex = 0;
            txtFMOccupation.Text = string.Empty;
            ddlUsageMode.SelectedIndex = 0;
            hfFamilyMemberId.Text = string.Empty;
        }
        private void ClearEducation()
        {
            btnAddInst.Text = "Add";
            txtDegree.Text = string.Empty;
            txtInstitution.Text = string.Empty;
            txtPassingYear.Text = string.Empty;
            hfEducationId.Text = string.Empty;
        }
        private bool IsFormValid()
        {
            bool status = true;

            if (ddlMemberType.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Member Type.", AlertType.Warning);
                status = false;
                ddlMemberType.Focus();
            }
            else if (string.IsNullOrEmpty(txtFirstName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "First Name.", AlertType.Warning);
                status = false;
                txtFirstName.Focus();
            }
            else if (string.IsNullOrEmpty(txtLastName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Sur Name.", AlertType.Warning);
                status = false;
                txtLastName.Focus();
            }
            else if (string.IsNullOrEmpty(txtMemDOB.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Birth Date.", AlertType.Warning);
                status = false;
                txtMemDOB.Focus();
            }
            else if (ddlGender.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Gender.", AlertType.Warning);
                status = false;
                ddlGender.Focus();
            }
            else if (ddlMaritalStatus.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Marital Status..", AlertType.Warning);
                status = false;
                ddlMaritalStatus.Focus();
            }
            else if (ddlCountryId.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Nationality.", AlertType.Warning);
                status = false;
                ddlCountryId.Focus();
            }
            else if (ddlBloodGroup.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Blood Group.", AlertType.Warning);
                status = false;
                ddlCountryId.Focus();
            }
            else if (string.IsNullOrEmpty(txtSecurityDep.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Security Deposit.", AlertType.Warning);
                status = false;
                txtSecurityDep.Focus();
            }

            if (string.IsNullOrEmpty(txtExpiryDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Expiry Date.", AlertType.Warning);
                status = false;
                txtExpiryDate.Focus();
            }

            if (!string.IsNullOrEmpty(txtFirstName.Text))
            {
                Regex regex = new Regex(@"[^0-9^+^\-^\/^\*^\(^\)]");
                MatchCollection matches = regex.Matches(txtFirstName.Text);
                if (matches.Count == 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, "First Name is inconsistent.", AlertType.Warning);
                    status = false;
                    txtFirstName.Focus();
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
                }
            }
            SetTab("BasicTab");
            return status;
        }
        private bool IsReferenceValid()
        {
            bool status = true;

            if (string.IsNullOrEmpty(txtArbitrator.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Arbitrator.", AlertType.Warning);
                status = false;
                txtArbitrator.Focus();
            }

            SetTab("ReferenceTab");
            return status;
        }
        private bool IsEducationListValid()
        {
            bool status = true;

            if (string.IsNullOrEmpty(txtDegree.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Degree Name.", AlertType.Warning);
                status = false;
                txtDegree.Focus();
            }
            else if (string.IsNullOrEmpty(txtInstitution.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Institute Name", AlertType.Warning);
                status = false;
                txtInstitution.Focus();
            }

            SetTab("EducationTab");
            return status;
        }
        private bool IsFamilyMemberValid()
        {
            bool status = true;

            if (string.IsNullOrEmpty(txtMemberName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Member Name.", AlertType.Warning);
                status = false;
                txtMemberName.Focus();
            }
            else if (string.IsNullOrEmpty(txtDOB.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Birth Date.", AlertType.Warning);
                status = false;
                txtDOB.Focus();
            }

            SetTab("FamilyMemberTab");
            return status;
        }
        private void SetTab(string TabName)
        {
            if (TabName == "BasicTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "OccupationTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
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
            }
            else if (TabName == "FamilyMemberTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "NomineeTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "ReferenceTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "SearchTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
            }
        }
        private void AddEditOrDeleteDetail()
        {
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

            //Family Member-----------
            if (Session["arrayFamilyMemberDelete"] == null)
            {
                arrayFamilyMemberDelete = new ArrayList();
                Session.Add("arrayFamilyMemberDelete", arrayFamilyMemberDelete);
            }
            else
            {
                arrayFamilyMemberDelete = Session["arrayFamilyMemberDelete"] as ArrayList;
            }
            //Education Member-----------
            if (Session["arrayEducationDelete"] == null)
            {
                arrayEducationDelete = new ArrayList();
                Session.Add("arrayEducationDelete", arrayEducationDelete);
            }
            else
            {
                arrayEducationDelete = Session["arrayEducationDelete"] as ArrayList;
            }
        }
        private void ClearSession()
        {
            Session["arrayReferenceDelete"] = null;
            Session["arrayFamilyMemberDelete"] = null;
            Session["arrayEducationDelete"] = null;
            Session["ReferenceList"] = null;
            Session["FamilyMemberList"] = null;
            Session["EducationList"] = null;
        }
        private void CreadeNodeMatrixAccountHeadInfo(int AncestorId, out int NodeId)
        {
            NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
            // Need to Fix----------------------------------
            nodeMatrixBO.AncestorId = AncestorId;
            nodeMatrixBO.NodeHead = this.txtDisplayName.Text.Trim();
            nodeMatrixBO.NodeMode = true;
            Boolean status = nodeMatrixDA.SaveNodeMatrixInfoFromOtherPage(nodeMatrixBO, out NodeId);
        }
        private void UpdateNodeMatrixAccountHeadInfo(int nodeId)
        {
            NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();

            nodeMatrixBO.NodeId = nodeId;
            nodeMatrixBO.NodeHead = this.txtDisplayName.Text.Trim();

            nodeMatrixBO.NodeMode = true;
            Boolean status = nodeMatrixDA.UpdateNodeMatrixInfoFromOtherPage(nodeMatrixBO);
        }
        //************************ User Defined WebMethod ********************//
        [WebMethod]
        public static GridViewDataNPaging<MemMemberBasicsBO, GridPaging> SearchNLoadMemberInformation(string memName, string code, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<MemMemberBasicsBO, GridPaging> myGridData = new GridViewDataNPaging<MemMemberBasicsBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            MemMemberBasicDA memberBasicDA = new MemMemberBasicDA();
            List<MemMemberBasicsBO> memberList = new List<MemMemberBasicsBO>();
            memberList = memberBasicDA.GetMemberInfoBySearchCriteriaForPaging(memName, code, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<MemMemberBasicsBO> distinctItems = new List<MemMemberBasicsBO>();
            distinctItems = memberList.GroupBy(test => test.MemberId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }

        protected void btnAddInst_Click(object sender, EventArgs e)
        {
            OnlineMemberEducationBO memberEducationBO = new OnlineMemberEducationBO();
            if (!IsEducationListValid())
            {
                return;
            }

            int dynamicId = 0;
            List<OnlineMemberEducationBO> memberEducationBOs = Session["EducationList"] == null ? new List<OnlineMemberEducationBO>() : Session["EducationList"] as List<OnlineMemberEducationBO>;

            if (!string.IsNullOrWhiteSpace(hfEducationId.Text))
                dynamicId = Convert.ToInt32(hfEducationId.Text);

            OnlineMemberEducationBO educationBO = dynamicId == 0 ? new OnlineMemberEducationBO() : memberEducationBOs.Where(x => x.Id == dynamicId).FirstOrDefault();
            if (memberEducationBOs.Contains(educationBO))
                memberEducationBOs.Remove(educationBO);

            educationBO.Degree = txtDegree.Text;
            educationBO.Institution = txtInstitution.Text;
            educationBO.PassingYear = Convert.ToInt32(txtPassingYear.Text);
            
            
            educationBO.Id = dynamicId == 0 ? memberEducationBOs.Count + 1 : dynamicId;
            memberEducationBOs.Add(educationBO);
            Session["EducationList"] = memberEducationBOs;
            this.gvEducationList.DataSource = Session["EducationList"] as List<OnlineMemberEducationBO>;
            this.gvEducationList.DataBind();
            ClearEducation();

            SetTab("EducationTab");
        }

        protected void gvEducationList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _EducationId;

            if (e.CommandName == "CmdEdit")
            {
                _EducationId = Convert.ToInt32(e.CommandArgument.ToString());
                hfEducationId.Text = _EducationId.ToString();
                var educationList = (List<OnlineMemberEducationBO>)Session["EducationList"];
                var educationBO = educationList.Where(x => x.Id == _EducationId).FirstOrDefault();
                if (educationBO != null && educationBO.Id > 0)
                {
                    txtDegree.Text = educationBO.Degree;
                    txtInstitution.Text = educationBO.Institution;
                    txtPassingYear.Text = educationBO.PassingYear.ToString();
                    
                    btnAddInst.Text = "Update";
                }
                else
                {
                    btnAddInst.Text = "Add";
                }
            }
            else if (e.CommandName == "CmdDelete")
            {
                _EducationId = Convert.ToInt32(e.CommandArgument.ToString());
                hfEducationId.Text = string.Empty;
                var educationList = (List<OnlineMemberEducationBO>)Session["EducationList"];
                var educationBO = educationList.Where(x => x.Id == _EducationId).FirstOrDefault();
                educationList.Remove(educationBO);
                Session["EducationList"] = educationList;
                arrayEducationDelete.Add(_EducationId);
                this.gvEducationList.DataSource = Session["EducationList"] as List<OnlineMemberEducationBO>;
                this.gvEducationList.DataBind();
            }
            SetTab("EducationTab");
        }
    }
}