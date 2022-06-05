using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class Contact : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();

        ContactInformationDA DA = new ContactInformationDA();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCRMConfiguration();
                LoadContactDetailsTitle();
                LoadAccountManager();
                LoadLifeCycleStage();

                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomProductId.Value = seatingId.ToString();
            }
        }
        private void LoadCRMConfiguration()
        {
            WorkAreaDiv.Visible = false;
            PersonalAreaDiv.Visible = false;
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCRMAreaFieldEnable", "IsCRMAreaFieldEnable");
            if (setUpBO.SetupId > 0)
            {
                if (setUpBO.SetupValue == "1")
                {
                    WorkAreaDiv.Visible = true;
                    PersonalAreaDiv.Visible = true;
                }
            }
        }
        private void LoadLifeCycleStage()
        {
            LifeCycleStageDA lifeCycleStageDA = new LifeCycleStageDA();
            List<SMLifeCycleStageBO> sMLifeCycles = new List<SMLifeCycleStageBO>();
            sMLifeCycles = lifeCycleStageDA.GetLifeCycleForDdl();

            ddlLifeCycleStageId.DataSource = sMLifeCycles;
            ddlLifeCycleStageId.DataTextField = "LifeCycleStage";
            ddlLifeCycleStageId.DataValueField = "Id";
            ddlLifeCycleStageId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlLifeCycleStageId.Items.Insert(0, item);
        }
        private void LoadAccountManager()
        {
            int isAdminUser = 0;
            ddlContactOwnerId.Enabled = true;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            AccountManagerDA accountManagerDA = new AccountManagerDA();
            List<AccountManagerBO> accountManagerBOList = new List<AccountManagerBO>();

            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                isAdminUser = 1;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 22).Count() > 0)
                    {
                        isAdminUser = 1;
                    }
                }
            }
            #endregion

            accountManagerBOList = accountManagerDA.GetAccountManager(isAdminUser, "CRM", userInformationBO.UserInfoId);

            ddlContactOwnerId.DataSource = accountManagerBOList;
            ddlContactOwnerId.DataTextField = "DisplayName";
            ddlContactOwnerId.DataValueField = "UserInfoId";
            ddlContactOwnerId.DataBind();

            if (accountManagerBOList != null)
            {
                if (accountManagerBOList.Count > 1)
                {
                    ListItem item = new ListItem();
                    item.Value = "0";
                    item.Text = hmUtility.GetDropDownFirstAllValue();
                    ddlContactOwnerId.Items.Insert(0, item);
                }
                else
                {
                    ddlContactOwnerId.Enabled = false;
                }
            }

            ddlContactOwnerId.SelectedValue = userInformationBO.UserInfoId.ToString();
        }
        private void LoadContactDetailsTitle()
        {
            ContactInformationDA DA = new ContactInformationDA();
            // // ------Phone Title
            List<SMContactDetailsTitleBO> phoneTitleList = new List<SMContactDetailsTitleBO>();
            phoneTitleList = DA.GetContactDetailsTitleByTransectionType("Number");

            ddlPhoneTitle.DataSource = phoneTitleList;
            ddlPhoneTitle.DataTextField = "Title";
            ddlPhoneTitle.DataValueField = "Id";
            ddlPhoneTitle.DataBind();

            ListItem itemNumber = new ListItem();
            itemNumber.Value = "0";
            itemNumber.Text = hmUtility.GetDropDownFirstValue();
            ddlPhoneTitle.Items.Insert(0, itemNumber);

            // // ------Email Title
            List<SMContactDetailsTitleBO> emailTitleList = new List<SMContactDetailsTitleBO>();
            emailTitleList = DA.GetContactDetailsTitleByTransectionType("Email");

            ddlEmailTitle.DataSource = emailTitleList;
            ddlEmailTitle.DataTextField = "Title";
            ddlEmailTitle.DataValueField = "Id";
            ddlEmailTitle.DataBind();

            ListItem itemEmail = new ListItem();
            itemEmail.Value = "0";
            itemEmail.Text = hmUtility.GetDropDownFirstValue();
            ddlEmailTitle.Items.Insert(0, itemEmail);

            // // ------Social Media Title
            List<SMContactDetailsTitleBO> faxTitleList = new List<SMContactDetailsTitleBO>();
            faxTitleList = DA.GetContactDetailsTitleByTransectionType("SocialMedia");

            ddlSocialMediaTitle.DataSource = faxTitleList;
            ddlSocialMediaTitle.DataTextField = "Title";
            ddlSocialMediaTitle.DataValueField = "Id";
            ddlSocialMediaTitle.DataBind();

            ListItem itemSocialMedia = new ListItem();
            itemSocialMedia.Value = "0";
            itemSocialMedia.Text = hmUtility.GetDropDownFirstValue();
            ddlSocialMediaTitle.Items.Insert(0, itemSocialMedia);

            // // ------Website Title
            List<SMContactDetailsTitleBO> websiteTitleList = new List<SMContactDetailsTitleBO>();
            websiteTitleList = DA.GetContactDetailsTitleByTransectionType("Website");

            ddlWebsiteTitle.DataSource = websiteTitleList;
            ddlWebsiteTitle.DataTextField = "Title";
            ddlWebsiteTitle.DataValueField = "Id";
            ddlWebsiteTitle.DataBind();

            ListItem itemWebsite = new ListItem();
            itemWebsite.Value = "0";
            itemWebsite.Text = hmUtility.GetDropDownFirstValue();
            ddlWebsiteTitle.Items.Insert(0, itemWebsite);
        }
        [WebMethod]
        public static List<GuestCompanyBO> GetGuestCompanyInfoByCompanyName(string companyName)
        {
            GuestCompanyDA bpDA = new GuestCompanyDA();
            return bpDA.GetGuestCompanyInfoByCompanyName(companyName);
        }
        [WebMethod]
        public static int CheckLifeCycleStageValidation(ContactInformationBO contactInformationBO)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            int IsValid = 0;
            ContactInformationBO PreviousBO = new ContactInformationBO();
            ContactInformationDA DA = new ContactInformationDA();
            contactInformationBO.CreatedBy = userInformationBO.UserInfoId;
            if (contactInformationBO.Id != 0)
            {
                PreviousBO = DA.GetContactInformationById(contactInformationBO.Id);
                if (!CheckLifeCycleStageCanBeChange(contactInformationBO, PreviousBO))
                {
                    IsValid = 1;
                }
            }
            else
            {
                if (!CheckLifeCycleStageCanBeChange(contactInformationBO, PreviousBO))
                {
                    IsValid = 1;
                }
            }
            return IsValid;
        }
        [WebMethod]
        public static ReturnInfo SaveUpdateContact(ContactInformationBO contactInformationBO, string hfRandom, string deletedDocument, List<SMContactDetailsBO> newlyAddedItem, List<SMContactDetailsBO> deleteDbItem)
        {
            SalesMarketingLogType<ContactInformationBO> logDA = new SalesMarketingLogType<ContactInformationBO>();
            long OwnerIdForDocuments = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            try
            {
                contactInformationBO.CreatedBy = userInformationBO.UserInfoId;
                long OutId;
                ContactInformationDA DA = new ContactInformationDA();
                ContactInformationBO PreviousBO = new ContactInformationBO();
                if (contactInformationBO.Id != 0) //update
                {
                    PreviousBO = DA.GetContactInformationById(contactInformationBO.Id);
                    status = DA.SaveContact(contactInformationBO, deletedDocument, out OutId);
                    if (status)
                    {
                        if (PreviousBO != null)
                        {
                            SMContactTransfer contactTrans = new SMContactTransfer();
                            contactTrans.ContactId = contactInformationBO.Id;
                            contactTrans.PreviousCompanyId = PreviousBO.CompanyId;
                            contactTrans.TransferredCompanyId = contactInformationBO.CompanyId;
                            contactTrans.Title = contactInformationBO.JobTitle;
                            contactTrans.Department = contactInformationBO.Department;
                            contactTrans.Mobile = contactInformationBO.MobileWork;
                            contactTrans.Phone = contactInformationBO.PhoneWork;
                            contactTrans.Email = contactInformationBO.EmailWork;

                            long id = 0;

                            bool trans = DA.TransferContact(contactTrans, false, out id);

                            if (trans)
                            {
                                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Transfer.ToString()
                                                                , EntityTypeEnum.EntityType.SMContactTransfer.ToString(), id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString()
                                                                , hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMContactTransfer));
                            }
                        }
                    }
                }
                else //save
                {
                    if (!CheckLifeCycleStageCanBeChange(contactInformationBO, PreviousBO))
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo("Cannot Skip First Life Cycle Stage.", AlertType.Warning);
                        rtninfo.DataStr = "InvalidLifeCycleStage";
                        return rtninfo;
                    }
                    status = DA.SaveContact(contactInformationBO, deletedDocument, out OutId);
                }

                if (status)
                {

                    if (contactInformationBO.Id == 0)
                    {
                        contactInformationBO.Id = OutId;
                        OwnerIdForDocuments = OutId;
                        logDA.Log(ConstantHelper.SalesandMarketingLogType.ContactCreated, contactInformationBO, contactInformationBO);

                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                                EntityTypeEnum.EntityType.WorkingPlan.ToString(), OutId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.WorkingPlan));

                    }
                    else
                    {
                        logDA.Log(ConstantHelper.SalesandMarketingLogType.ContactActivity, contactInformationBO, PreviousBO);
                        OwnerIdForDocuments = contactInformationBO.Id;
                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                               EntityTypeEnum.EntityType.SalaryHead.ToString(), OutId,
                               ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalaryHead));
                    }
                    bool detailsStatus = DA.SaveContactDetails(newlyAddedItem, deleteDbItem, OutId);
                    Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(hfRandom));
                }
                else
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo("save Fail", AlertType.Error);

                }
                return rtninfo;
            }
            catch (Exception ex)
            {
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }
            return rtninfo;
        }
        [WebMethod]
        public static ArrayList FillForm(long Id)
        {
            ContactInformationDA DA = new ContactInformationDA();
            ContactInformationBO contactInformation = new ContactInformationBO();
            contactInformation = DA.GetContactInformationById(Id);
            List<SMContactDetailsBO> detailsBOs = new List<SMContactDetailsBO>();
            detailsBOs = DA.GetContactDetails(Id, "Contact");

            var numbers = detailsBOs.Where(x => x.TransectionType == "Number").ToList();
            var emails = detailsBOs.Where(x => x.TransectionType == "Email").ToList();
            var socialMedias = detailsBOs.Where(x => x.TransectionType == "SocialMedia").ToList();
            var websites = detailsBOs.Where(x => x.TransectionType == "Website").ToList();

            ArrayList arr = new ArrayList();
            arr.Add(new { Numbers = numbers, Emails = emails, SocialMedias = socialMedias, Websites = websites, ContactInformation = contactInformation });

            return arr;
        }
        [WebMethod]
        public static List<String> LoadContactLabel()
        {
            var labels = new List<string>() { "Home", "Work" };
            return labels;
        }
        [WebMethod]
        public static GuestCompanyBO GetCompanyLifeCyleStage(int companyId)
        {
            GuestCompanyBO guestCompanyBO = new GuestCompanyBO();
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            guestCompanyBO = guestCompanyDA.GetGuestCompanyInfoById(companyId);
            return guestCompanyBO;

        }

        [WebMethod]
        public static List<SMContactDetailsTitleBO> LoadLabelByAutoSearch(string searchTerm, string Type)
        {
            ContactInformationDA DA = new ContactInformationDA();
            List<SMContactDetailsTitleBO> titleList = new List<SMContactDetailsTitleBO>();
            titleList = DA.GetContactDetailsTitleForAutoSearch(searchTerm, Type);
            return titleList;
        }

        [WebMethod]
        public static List<CustomFieldBO> LoadSocialMediaLabelByAutoSearch(string searchTerm)
        {
            CustomFieldBO customField = new CustomFieldBO();
            HMCommonDA commonDA = new HMCommonDA();
            List<CustomFieldBO> titleList = new List<CustomFieldBO>();
            titleList = commonDA.GetCustomField("ContactDetailsLabelSocialMedia");
            return titleList;
        }
        // Documents //
        [WebMethod]
        public static List<DocumentsBO> LoadContactDocument(long id, int randomId, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }

            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("ContactDocument", randomId);

            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("ContactDocument", id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));

            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }
        private static bool CheckLifeCycleStageCanBeChange(ContactInformationBO current, ContactInformationBO previous)
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsLifeCycleStageCanChangeMoreThanOneStep", "IsLifeCycleStageCanChangeMoreThanOneStep");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue) && setUpBO.SetupValue == "0")
            {
                LifeCycleStageDA lifeCycleStageDA = new LifeCycleStageDA();
                List<SMLifeCycleStageBO> lifeCycles = new List<SMLifeCycleStageBO>();
                lifeCycles = lifeCycleStageDA.GetLifeCycleForDdl();
                if (previous != null)
                {
                    int previousStageSequence = 0, currentStageSequence = 0;
                    foreach (var lifeCycle in lifeCycles)
                    {
                        if (lifeCycle.Id == current.LifeCycleId)
                            currentStageSequence = (int)lifeCycle.DisplaySequence;
                        if (lifeCycle.Id == previous.LifeCycleId)
                            previousStageSequence = (int)lifeCycle.DisplaySequence;
                    }
                    return Math.Abs(previousStageSequence - currentStageSequence) > 1 ? false : true;
                }
                else
                    return current.LifeCycleId == lifeCycles[0].Id;
            }
            return true;
        }
    }
}