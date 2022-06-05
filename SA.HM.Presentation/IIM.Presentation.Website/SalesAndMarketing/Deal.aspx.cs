using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class Deal : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected bool isSingle = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomDealId.Value = seatingId.ToString();

                LoadDealStage();
                LoadCompany();
                LoadAccountManager();
                LoadProbabilityStage();
                LoadSegment();
                LoadCRMConfiguaration();
                LoadCompanyIndependentContacts();
                LoadCommonDropDownHiddenField();
            }
        }
        private void LoadDealStage()
        {
            StageDA stageDA = new StageDA();
            List<SMDealStage> stageList = new List<SMDealStage>();

            stageList = stageDA.GetAllDealStages();

            ddlDealStage.DataSource = stageList;
            ddlDealStage.DataTextField = "DealStage";
            ddlDealStage.DataValueField = "Id";
            ddlDealStage.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlDealStage.Items.Insert(0, item);
        }
        private void LoadCompany()
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> files = new List<GuestCompanyBO>();
            files = guestCompanyDA.GetALLGuestCompanyInfo();

            ddlCompany.DataSource = files;
            ddlCompany.DataTextField = "CompanyName";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCompany.Items.Insert(0, item);
        }
        private void LoadAccountManager()
        {
            int isAdminUser = 0;
            ddlDealOwner.Enabled = true;
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

            ddlDealOwner.DataSource = accountManagerBOList;
            ddlDealOwner.DataTextField = "DisplayName";
            ddlDealOwner.DataValueField = "UserInfoId";
            ddlDealOwner.DataBind();

            if (accountManagerBOList != null)
            {
                if (accountManagerBOList.Count > 1)
                {
                    ListItem item = new ListItem();
                    item.Value = "0";
                    item.Text = hmUtility.GetDropDownFirstAllValue();
                    ddlDealOwner.Items.Insert(0, item);
                }
                else
                {
                    ddlDealOwner.Enabled = false;
                }
            }

            ddlDealOwner.SelectedValue = userInformationBO.UserInfoId.ToString();
        }
        private void LoadProbabilityStage()
        {
            List<SMDealProbabilityStageInformationBO> stages = new List<SMDealProbabilityStageInformationBO>();
            SetupDA setupDA = new SetupDA();

            stages = setupDA.GetAllProbabilityStage();

            ddlProbabilityStage.DataSource = stages;
            ddlProbabilityStage.DataTextField = "ProbabilityStage";
            ddlProbabilityStage.DataValueField = "Id";
            ddlProbabilityStage.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlProbabilityStage.Items.Insert(0, item);
        }
        private void LoadSegment()
        {
            List<SMSegmentInformationBO> segmentList = new List<SMSegmentInformationBO>();
            SetupDA setupDA = new SetupDA();

            segmentList = setupDA.GetAllSegment();

            ddlSegment.DataSource = segmentList;
            ddlSegment.DataTextField = "SegmentName";
            ddlSegment.DataValueField = "Id";
            ddlSegment.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSegment.Items.Insert(0, item);
        }
        private void FileUpload()
        {
            Session["DealId"] = RandomDealId.Value;
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
        }
        private void LoadCRMConfiguaration()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsSegmentNameWillShow", "IsSegmentNameWillShow");
            SegmentDiv.Visible = setUpBO.SetupValue == "1";

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsProductInformationWillShow", "IsProductInformationWillShow");
            dvProductInfo.Visible = setUpBO.SetupValue == "1";

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsServiceInformationWillShow", "IsServiceInformationWillShow");
            dvServiceInfo.Visible = setUpBO.SetupValue == "1";
        }
        private void LoadCompanyIndependentContacts()
        {
            List<ContactInformationBO> contacts = new List<ContactInformationBO>();
            ContactInformationDA contactDA = new ContactInformationDA();
            contacts = contactDA.GetContactInformation().Where(x => x.CompanyId == 0).ToList();

            ddlIndependentContact.DataSource = contacts;
            ddlIndependentContact.DataTextField = "Name";
            ddlIndependentContact.DataValueField = "Id";
            ddlIndependentContact.DataBind();

            if (contacts.Count > 1)
            {
                ListItem item = new ListItem();
                item.Value = "0";
                item.Text = hmUtility.GetDropDownFirstValue();
                ddlIndependentContact.Items.Insert(0, item);
            }

        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        [WebMethod]
        public static List<ContactInformationBO> GetEmployeeByCompanyId(int companyId)
        {
            List<ContactInformationBO> contactInformation = new List<ContactInformationBO>();
            ContactInformationDA DA = new ContactInformationDA();
            contactInformation = DA.GetContactInformationByCompanyId(companyId);

            return contactInformation;
        }
        [WebMethod]
        public static bool CheckCompanyIsClient(int companyId, long contactId)
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            bool isClient = guestCompanyDA.CheckCompanyOrContactIsClient(companyId, contactId);

            return isClient;
        }


        [WebMethod(EnableSession = true)]
        public static ReturnInfo SaveOrUpdateDeal(SMDeal deal, string deletedDocument)
        {
            ReturnInfo info = new ReturnInfo();
            long id = 0;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            DealDA dealDA = new DealDA();
            long tempContactId = 0;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            deal.CreatedBy = userInformationBO.UserInfoId;
            SalesMarketingLogType<SMDeal> logDA = new SalesMarketingLogType<SMDeal>();
            SMDeal previousBO = new SMDeal();

            try
            {
                List<SMDealWiseContactMap> deleteList = new List<SMDealWiseContactMap>();
                if (deal.Id > 0)
                {
                    tempContactId = deal.Contacts[0].ContactId;
                    previousBO = dealDA.GetDealInfoBOById(deal.Id);
                    deal.IsCanDelete = previousBO.IsCanDelete;
                    if (previousBO.IsCanDelete == true)
                    {
                        if (previousBO.StageId != deal.StageId)
                        {
                            deal.IsCanDelete = false;
                        }
                    }
                    if (!CheckDealStageCanBeChange(deal, previousBO))
                    {
                        info.AlertMessage = CommonHelper.AlertInfo("Cannot Change Deal Stage More Than one Stage.", AlertType.Warning);
                        info.DataStr = "InvalidDealStage";
                        return info;
                    }
                    deleteList = previousBO.Contacts.Where(c => !deal.Contacts.Select(ci => ci.ContactId).ToList().Contains(c.ContactId)).ToList();
                    deal.Contacts = deal.Contacts.Where(c => !previousBO.Contacts.Select(ci => ci.ContactId).ToList().Contains(c.ContactId)).ToList();
                }
                else
                {
                    if (!CheckDealStageCanBeChange(deal))
                    {
                        info.AlertMessage = CommonHelper.AlertInfo("Cannot Skip First Deal Stage.", AlertType.Warning);
                        info.DataStr = "InvalidDealStage";
                        return info;
                    }
                }

                info.IsSuccess = dealDA.SaveOrUpdateDeal(deal, deleteList, deletedDocument, out id);

                if (info.IsSuccess)
                {
                    Random rd = new Random();
                    int randomId = rd.Next(100000, 999999);
                    HttpContext.Current.Session["DealId"] = randomId;
                    if (deal.Id == 0)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.SMDeal.ToString(), id,
                            ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                    else
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.SMDeal.ToString(), deal.Id,
                            ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }

                    info.Data = randomId;

                    if (deal.Id == 0)
                    {
                        deal.Id = id;
                        logDA.Log(ConstantHelper.SalesandMarketingLogType.DealCreated, deal, deal);
                    }
                    else
                    {
                        logDA.Log(ConstantHelper.SalesandMarketingLogType.DealActivity, deal, previousBO);
                    }
                    if (deal.Contacts.Count == 0)
                        deal.Contacts.Add(new SMDealWiseContactMap() { ContactId = tempContactId });
                    UpdateCompanyOrContactLifeCycleStage(deal);
                }
                else
                {
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;
        }

        private static bool UpdateCompanyOrContactLifeCycleStage(SMDeal deal)
        {
            bool status = false;
            int companyId = (int)deal.CompanyId;
            long contactId = 0;

            LifeCycleStageDA companyStatusDA = new LifeCycleStageDA();
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            SalesMarketingLogType<GuestCompanyBO> logDA = new SalesMarketingLogType<GuestCompanyBO>();

            GuestCompanyBO currentBO = new GuestCompanyBO();
            GuestCompanyBO previousBO = new GuestCompanyBO();

            if (deal.CompanyId == 0)
            {
                contactId = deal.Contacts[0].ContactId;
            }
            else
                previousBO = guestCompanyDA.GetGuestCompanyInfoById((int)deal.CompanyId);

            string dealType = deal.Type;
            status = guestCompanyDA.UpdateCompanyOrContactLifeCycleStage(companyId, contactId, (int)deal.StageId, dealType);

            if (deal.CompanyId > 0)
            {
                currentBO = guestCompanyDA.GetGuestCompanyInfoById((int)deal.CompanyId);
                if (status)
                {
                    logDA.Log(ConstantHelper.SalesandMarketingLogType.CompanyActivity, currentBO, previousBO);

                }
            }

            return status;
        }

        private static bool CheckDealStageCanBeChange(SMDeal current, SMDeal previous = null)
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDealStageCanChangeMoreThanOneStep", "IsDealStageCanChangeMoreThanOneStep");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue) && setUpBO.SetupValue == "0")
            {
                List<SMDealStage> dealStages = new StageDA().GetAllDealStages();
                if (previous != null)
                {
                    int previousStageSequence = 0, currentStageSequence = 0;
                    foreach (var stage in dealStages)
                    {
                        if (stage.Id == current.StageId)
                            currentStageSequence = (int)stage.DisplaySequence;
                        if (stage.Id == previous.StageId)
                            previousStageSequence = (int)stage.DisplaySequence;
                    }
                    return Math.Abs(previousStageSequence - currentStageSequence) > 1 ? false : true;
                }
                else
                {
                    return current.StageId == dealStages[0].Id;
                }
            }
            return true;
        }

        [WebMethod]
        public static SMDeal GetDealInfoById(int Id)
        {
            SMDeal deal = new SMDeal();
            DealDA dealDA = new DealDA();
            deal = dealDA.GetDealInfoBOById(Id);
            return deal;
        }
        [WebMethod]

        public static List<DocumentsBO> LoadDealDocument(long id, int randomId, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }

            List<DocumentsBO> docList = new List<DocumentsBO>();
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("SalesDealDocuments", randomId);

            if (id > 0)
            {
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("SalesDealDocuments", id));
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("SalesDealFeedbackDocuments", id));
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("SalesQuotationDocuments", id));
            }

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));
            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }

        [WebMethod]
        public static ReturnInfo DeleteDealDocument(long deletedDocumentId)
        {
            ReturnInfo info = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            List<DocumentsBO> docList = new List<DocumentsBO>();

            info.IsSuccess = new DocumentsDA().DeleteDocumentsByDocumentId(deletedDocumentId);
            if (info.IsSuccess)
            {
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.SMDeal.ToString(), deletedDocumentId, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
            }
            else
            {
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;
        }

        [WebMethod]
        public static List<GLProjectBO> GetGLProjectByGLCompanyId(int companyId)
        {
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            GLProjectDA entityDA = new GLProjectDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            projectList = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(companyId, userInformationBO.UserGroupId);

            return projectList;
        }
    }
}