using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Membership;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Membership;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Membership
{
    public partial class OnlineMembership : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            /*     
             * ddlRelationship  */

            if (!IsPostBack)
            {
                LoadBloodGroup();
                LoadMaritualStatus();
                LoadMemberType();
                LoadRelationship();
                LoadProfession();
                LoadGender();
                LoadCountry();
                LoadReligion();
            }
        }
        private void LoadMemberType()
        {
            MemMemberBasicDA memberDA = new MemMemberBasicDA();
            List<MemMemberTypeBO> typeList = memberDA.GetMemMemberTypeList();
            ddlMemberType.DataSource = typeList;
            ddlMemberType.DataTextField = "Name";
            ddlMemberType.DataValueField = "TypeId";
            ddlMemberType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlMemberType.Items.Insert(0, item);
        }
        private void LoadRelationship()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("Relationship", hmUtility.GetDropDownFirstValue());

            ddlRelationship.DataSource = fields;
            ddlRelationship.DataTextField = "FieldValue";
            ddlRelationship.DataValueField = "FieldId";
            ddlRelationship.DataBind();

            ddlNomineeRelation.DataSource = fields;
            ddlNomineeRelation.DataTextField = "FieldValue";
            ddlNomineeRelation.DataValueField = "FieldId";
            ddlNomineeRelation.DataBind();

            ddlRelationship.DataSource = fields;
            ddlRelationship.DataTextField = "FieldValue";
            ddlRelationship.DataValueField = "FieldId";
            ddlRelationship.DataBind();
        }
        private void LoadProfession()
        {
            CommonProfessionDA professionDA = new CommonProfessionDA();
            List<CommonProfessionBO> entityBOList = new List<CommonProfessionBO>();
            entityBOList = professionDA.GetProfessionInfo();

            ddlProfessionId.DataSource = entityBOList;
            ddlProfessionId.DataTextField = "ProfessionName";
            ddlProfessionId.DataValueField = "ProfessionId";
            ddlProfessionId.DataBind();

            ListItem itemProfession = new ListItem();
            itemProfession.Value = "0";
            itemProfession.Text = hmUtility.GetDropDownFirstValue();
            ddlProfessionId.Items.Insert(0, itemProfession);
        }
        private void LoadBloodGroup()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("BloodGroup", hmUtility.GetDropDownFirstValue());

            ddlBloodGroup.DataSource = fields;
            ddlBloodGroup.DataTextField = "FieldValue";
            ddlBloodGroup.DataValueField = "FieldId";
            ddlBloodGroup.DataBind();

            ddlBloodFamMem.DataSource = fields;
            ddlBloodFamMem.DataTextField = "FieldValue";
            ddlBloodFamMem.DataValueField = "FieldId";
            ddlBloodFamMem.DataBind();
        }
        private void LoadMaritualStatus()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("MaritualStatus", hmUtility.GetDropDownFirstValue());

            ddlMaritalStatus.DataSource = fields;
            ddlMaritalStatus.DataTextField = "FieldValue";
            ddlMaritalStatus.DataValueField = "FieldId";
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
            ddlGender.DataValueField = "FieldId";
            ddlGender.DataBind();
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
        private void LoadCountry()
        {
            HMCommonDA commonDA = new HMCommonDA();
            var List = commonDA.GetAllCountries();
            ddlCountry.DataSource = List;
            ddlCountry.DataTextField = "CountryName";
            ddlCountry.DataValueField = "CountryId";
            ddlCountry.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCountry.Items.Insert(0, item);
        }
        private static bool SendMail(int memberId)
        {
            HMUtility hmUtility = new HMUtility();
            Email email;
            Email email1;
            Email email2;
            
            bool status = false;
            bool status1 = false;
            bool status2 = false;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            //var accepted = "";
            //var rejected = "";
            //var deferred = "";

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
            string mainString = commonSetupBO.SetupValue;
            /// Please Update ClientURL according to deployment
            string urlHead1 = System.Web.Configuration.WebConfigurationManager.AppSettings["ClientURL"].ToString();
            string urlHead2 = System.Web.Configuration.WebConfigurationManager.AppSettings["ClientURL"].ToString();
            urlHead1 += "frmMembershipPublicAdmin.aspx?Id1=";
            urlHead2 += "frmMembershipPublicAdmin.aspx?Id2=";
            OnlineMemberBO onlineMember = new OnlineMemberBO();
            MemMemberBasicDA basicDA = new MemMemberBasicDA();
            onlineMember = basicDA.GetOnlineMemberInfoById(memberId, 0, 0);
            //var Id1 = onlineMember.Introducer_1_id;
            //var Id2 = onlineMember.Introducer_2_id;
            var Id1 = 2;
            var Id2 = 3;
            if (!string.IsNullOrEmpty(mainString))
            {
                string[] dataArray = mainString.Split('~');
                email = new Email
                {
                    From = dataArray[0],
                    Password = dataArray[1],
                    To = onlineMember.PersonalEmail,
                    Host = dataArray[2],
                    Port = dataArray[3],
                    Subject = "Membership Registration",
                    TempleteName = HMConstants.EmailTemplates.MembershipRegistration
                };
                email1 = new Email
                {
                    From = dataArray[0],
                    Password = dataArray[1],
                    To = onlineMember.PersonalEmail,
                    Host = dataArray[2],
                    Port = dataArray[3],
                    Subject = "Introducer of New Membership Registration",
                    TempleteName = HMConstants.EmailTemplates.MembershipIntroducer1
                };
                email2 = new Email
                {
                    From = dataArray[0],
                    Password = dataArray[1],
                    To = onlineMember.PersonalEmail,
                    Host = dataArray[2],
                    Port = dataArray[3],
                    Subject = "Introducer of New Membership Registration",
                    TempleteName = HMConstants.EmailTemplates.MembershipIntroducer2
                };
                var token = new Dictionary<string, string>
                {
                    {"NAME", onlineMember.FullName},
                    {"COMPANY", hmUtility.GetHMCompanyProfile()},
                    {"COMPANYADDRESS", hmUtility.GetHMCompanyAddress()},
                };
                var token1 = new Dictionary<string, string>
                {
                    {"INTRONAME_1", onlineMember.Introducer_1_Name},
                    {"NAME", onlineMember.FullName},
                    {"COMPANY", hmUtility.GetHMCompanyProfile()},
                    {"COMPANYADDRESS", hmUtility.GetHMCompanyAddress()},
                    {"LINK", urlHead1+Id1},
                };
                var token2 = new Dictionary<string, string>
                {
                    {"INTRONAME_2", onlineMember.Introducer_2_Name},
                    {"NAME", onlineMember.FullName},
                    {"COMPANY", hmUtility.GetHMCompanyProfile()},
                    {"COMPANYADDRESS", hmUtility.GetHMCompanyAddress()},
                    {"LINK", urlHead2+Id2},
                };
                //var token3 = new Dictionary<string, string>
                //{
                //    {"NAME", onlineMember.FullName},
                //    {"MEMBERTYPE", onlineMember.TypeName},
                //    {"REJECTED", deferred},
                //    {"REMARKS", onlineMember.Remarks},
                //    {"COMPANY", hmUtility.GetHMCompanyProfile()},
                //    {"COMPANYADDRESS", hmUtility.GetHMCompanyAddress()},
                //};
                try
                {
                    status = EmailHelper.SendEmail(email, token);
                    status1 = EmailHelper.SendEmail(email1, token1);
                    status2 = EmailHelper.SendEmail(email2, token2);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return status;
        }
        [WebMethod]
        public static OnlineMemberBO GetOnlineMemberInfoByMemNumber(string memNumber)
        {
            MemMemberBasicDA memberBasicDA = new MemMemberBasicDA();
            OnlineMemberBO memberBO = new OnlineMemberBO();

            memberBO = memberBasicDA.GetOnlineMemberInfoByMemNumber(memNumber);
            if (memberBO != null)
            {
                return memberBO;
            }
            return memberBO;

        }
        [WebMethod]
        public static string GetMemberInfoById(int memberId)
        {
            MemMemberBasicsBO memMember = new MemMemberBasicsBO();
            MemMemberBasicDA memberBasicDA = new MemMemberBasicDA();
            memMember = memberBasicDA.GetOnlineMemberInfoById(memberId, 0, 0);
            if (memMember != null)
            {
                return memMember.FullName;
            }
            else
                return "";
            
        }
        [WebMethod]
        public static string GetNationality(int countryId)
        {
            CountriesBO country = new CountriesBO();

            try
            {
                HMCommonDA commonDa = new HMCommonDA();
                country = commonDa.GetCountriesById(countryId);

            }
            catch (Exception ex)
            {
                country.Nationality = string.Empty;

            }

            return country.Nationality;
        }
        [WebMethod]
        public static ReturnInfo SaveOnlineMember(OnlineMemberBO OnlineMemberObj, List<OnlineMemberFamilyBO> MemberFamilyList, List<OnlineMemberEducationBO> EducationList)
        {
            ReturnInfo rtninf = new ReturnInfo();
            UserInformationBO userInformationBO = new UserInformationBO();
            HMUtility hmUtility = new HMUtility();
            //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            MemMemberBasicDA memBasicDA = new MemMemberBasicDA();
            bool mailStatus = false;
            int tempId;

            OnlineMemberObj.CreatedBy = userInformationBO.UserInfoId;
            if (EducationList.Count>0)
            {
                foreach (var item in EducationList)
                {
                    item.CreatedBy = 0;
                }
            }
            if (MemberFamilyList.Count > 0)
            {
                foreach (var item in MemberFamilyList)
                {
                    item.CreatedBy = 0;
                }
            }
            rtninf.IsSuccess = memBasicDA.SaveOnlineMember(OnlineMemberObj, MemberFamilyList, EducationList, out tempId);

            if (rtninf.IsSuccess)
            {
                mailStatus = SendMail(tempId);
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
            }
            else
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf; 
        }
    }
}