using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.GeneralLedger;
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
    public partial class Company : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadContactDetailsTitle();
                LoadRandonNumber();
                LoadCRMConfiguration();
                LoadCompanyType();
                LoadAccountManager();
                LoadCommonDropDownHiddenField();
                LoadIndustry();
                LoadOwnership();
                LoadLifeCycleStage();
            }
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

            // // ------Fax Title
            List<SMContactDetailsTitleBO> faxTitleList = new List<SMContactDetailsTitleBO>();
            faxTitleList = DA.GetContactDetailsTitleByTransectionType("Fax");

            ddlFaxTitle.DataSource = faxTitleList;
            ddlFaxTitle.DataTextField = "Title";
            ddlFaxTitle.DataValueField = "Id";
            ddlFaxTitle.DataBind();

            ListItem itemFax = new ListItem();
            itemFax.Value = "0";
            itemFax.Text = hmUtility.GetDropDownFirstValue();
            ddlFaxTitle.Items.Insert(0, itemFax);

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
        private void LoadRandonNumber()
        {
            Random rd = new Random();
            int seatingId = rd.Next(100000, 999999);
            RandomProductId.Value = seatingId.ToString();

        }
        private void LoadCRMConfiguration()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsShippingAddresswillshow", "IsShippingAddresswillshow");
            DivShippingAddress.Visible = setUpBO.SetupValue == "1";

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDiscountPercentageWillShow", "IsDiscountPercentageWillShow");
            DivDiscountPercentage.Visible = setUpBO.SetupValue == "1";

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCreditLimitWillShow", "IsCreditLimitWillShow");
            DivCreditLimit.Visible = setUpBO.SetupValue == "1";

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsNumberOfEmployeeWillShow", "IsNumberOfEmployeeWillShow");
            DivNoOfEmployee.Visible = setUpBO.SetupValue == "1";

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsAnnualRevenueWillShow", "IsAnnualRevenueWillShow");
            DivAnnualRevenue.Visible = setUpBO.SetupValue == "1";

            BillingAreaDiv.Visible = false;
            ShippingAreaDiv.Visible = false;
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCRMAreaFieldEnable", "IsCRMAreaFieldEnable");
            if (setUpBO.SetupId > 0)
            {
                if (setUpBO.SetupValue == "1")
                {
                    BillingAreaDiv.Visible = true;
                    ShippingAreaDiv.Visible = true;
                }
            }

            hfIsGLCompanyWiseCRMCompanyDifferent.Value = "0";
            DivCRMCompany.Visible = false;
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsGLCompanyWiseCRMCompanyDifferent", "IsGLCompanyWiseCRMCompanyDifferent");
            if (setUpBO.SetupId > 0)
            {
                if (setUpBO.SetupValue == "1")
                {
                    hfIsGLCompanyWiseCRMCompanyDifferent.Value = "1";
                    DivCRMCompany.Visible = true;

                    GLCompanyDA costCentreTabDA = new GLCompanyDA();
                    List<GLCompanyBO> files = costCentreTabDA.GetAllGLCompanyInfo();
                    this.gvCompany.DataSource = files;
                    this.gvCompany.DataBind();
                }

            }
        }
        private void LoadCompanyType()
        {
            SalesMarketingDA salesMarketingDA = new SalesMarketingDA();
            List<SMCompanyTypeInformationBO> typeBO = new List<SMCompanyTypeInformationBO>();
            typeBO = salesMarketingDA.GetCompanyTypeForDDL();

            ddlCompanyType.DataSource = typeBO;
            ddlCompanyType.DataTextField = "TypeName";
            ddlCompanyType.DataValueField = "Id";
            ddlCompanyType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCompanyType.Items.Insert(0, item);
        }
        private void LoadAccountManager()
        {
            int isAdminUser = 0;
            ddlCompanyOwner.Enabled = true;
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

            ddlCompanyOwner.DataSource = accountManagerBOList;
            ddlCompanyOwner.DataTextField = "DisplayName";
            ddlCompanyOwner.DataValueField = "UserInfoId";
            ddlCompanyOwner.DataBind();

            if (accountManagerBOList != null)
            {
                if (accountManagerBOList.Count > 1)
                {
                    ListItem item = new ListItem();
                    item.Value = "0";
                    item.Text = hmUtility.GetDropDownFirstAllValue();
                    ddlCompanyOwner.Items.Insert(0, item);
                }
                else
                {
                    ddlCompanyOwner.Enabled = false;
                }
            }

            ddlCompanyOwner.SelectedValue = userInformationBO.UserInfoId.ToString();
        }
        private void LoadParentCompany()
        {
            List<GuestCompanyBO> companyBOs = new List<GuestCompanyBO>();
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();

            companyBOs = guestCompanyDA.GetGuestCompanyInfo();

            ddlParentCompany.DataSource = companyBOs;
            ddlParentCompany.DataTextField = "CompanyName";
            ddlParentCompany.DataValueField = "CompanyId";
            ddlParentCompany.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlParentCompany.Items.Insert(0, item);
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadIndustry()
        {
            IndustryDA industryDA = new IndustryDA();
            List<IndustryBO> industryBO = new List<IndustryBO>();
            industryBO = industryDA.GetIndustryInfo();

            ddlIndustry.DataSource = industryBO;
            ddlIndustry.DataTextField = "IndustryName";
            ddlIndustry.DataValueField = "IndustryId";
            ddlIndustry.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlIndustry.Items.Insert(0, item);
        }
        private void LoadOwnership()
        {
            SalesMarketingDA salesMarketingDA = new SalesMarketingDA();
            List<SMOwnershipInformationBO> typeBO = new List<SMOwnershipInformationBO>();
            typeBO = salesMarketingDA.GetOwnershipInfoForDDL();

            ddlOwnership.DataSource = typeBO;
            ddlOwnership.DataTextField = "OwnershipName";
            ddlOwnership.DataValueField = "Id";
            ddlOwnership.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlOwnership.Items.Insert(0, item);
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
        //private void LoadCountry()
        //{
        //    HMCommonDA commonDA = new HMCommonDA();
        //    List<CountriesBO> countryList = commonDA.GetAllCountries();
        //    //ddlBillingCountryId.DataSource = countryList;
        //    //ddlBillingCountryId.DataTextField = "CountryName";
        //    //ddlBillingCountryId.DataValueField = "CountryId";
        //    //ddlBillingCountryId.DataBind();

        //    //ddlShippingCountryId.DataSource = countryList;
        //    //ddlShippingCountryId.DataTextField = "CountryName";
        //    //ddlShippingCountryId.DataValueField = "CountryId";
        //    //ddlShippingCountryId.DataBind();

        //    ListItem item = new ListItem();
        //    item.Value = "0";
        //    item.Text = hmUtility.GetDropDownFirstValue();
        //    //ddlBillingCountryId.Items.Insert(0, item);
        //    //ddlShippingCountryId.Items.Insert(0, item);

        //    string bangladesh = "19";

        //    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
        //    HMCommonSetupBO commonCountrySetupBO = new HMCommonSetupBO();
        //    commonCountrySetupBO = commonSetupDA.GetCommonConfigurationInfo("CompanyCountryId", "CompanyCountryId");

        //    if (commonCountrySetupBO != null)
        //    {
        //        bangladesh = commonCountrySetupBO.SetupValue;
        //    }

        //    //ddlBillingCountryId.SelectedValue = bangladesh;
        //    // ddlShippingCountryId.SelectedValue = bangladesh;
        //}
        //private void LoadCity()
        //{
        //    CityDA cityDA = new CityDA();
        //    List<CityBO> cityBO = new List<CityBO>();
        //    cityBO = cityDA.GetCityInfo();

        //    //ddlBillingCityId.DataSource = cityBO;
        //    //ddlBillingCityId.DataTextField = "CityName";
        //    //ddlBillingCityId.DataValueField = "CityId";
        //    //ddlBillingCityId.DataBind();

        //    //ddlShippingCityId.DataSource = cityBO;
        //    //ddlShippingCityId.DataTextField = "CityName";
        //    //ddlShippingCityId.DataValueField = "CityId";
        //    //ddlShippingCityId.DataBind();

        //    ListItem item = new ListItem();
        //    item.Value = "0";
        //    item.Text = hmUtility.GetDropDownFirstValue();
        //    //ddlBillingCityId.Items.Insert(0, item);
        //    // ddlShippingCityId.Items.Insert(0, item);
        //}
        //private void LoadLocation()
        //{
        //    LocationDA locationDA = new LocationDA();
        //    List<LocationBO> locationBO = new List<LocationBO>();
        //    locationBO = locationDA.GetLocationInfo();
        //    //ddlBillingStateId.DataSource = locationBO;
        //    //ddlBillingStateId.DataTextField = "LocationName";
        //    //ddlBillingStateId.DataValueField = "LocationId";
        //    //ddlBillingStateId.DataBind();

        //    //ddlShippingStateId.DataSource = locationBO;
        //    //ddlShippingStateId.DataTextField = "LocationName";
        //    //ddlShippingStateId.DataValueField = "LocationId";
        //    //ddlShippingStateId.DataBind();

        //    ListItem item = new ListItem();
        //    item.Value = "0";
        //    item.Text = hmUtility.GetDropDownFirstValue();

        //    // ddlShippingStateId.Items.Insert(0, item);
        //    // ddlBillingStateId.Items.Insert(0, item);
        //}

        [WebMethod]
        public static List<GuestCompanyBO> LoadParentCompanyAfterSave()
        {
            List<GuestCompanyBO> companyBOs = new List<GuestCompanyBO>();
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();

            companyBOs = guestCompanyDA.GetGuestCompanyInfo();

            return companyBOs;
        }
        [WebMethod]
        public static List<CountriesBO> LoadCountryForAutoSearch(string searchString)
        {

            HMCommonDA commonDA = new HMCommonDA();
            List<CountriesBO> countryList = commonDA.GetCountriesBySearch(searchString);

            return countryList;
        }
        [WebMethod]
        public static List<StateBO> LoadStateForAutoSearchByCountry(string searchString, int CountryId)
        {

            HMCommonDA commonDA = new HMCommonDA();
            List<StateBO> countryList = commonDA.GetStateForAutoSearchByCountry(searchString, CountryId);

            return countryList;
        }
        [WebMethod]
        public static List<CityBO> LoadCityForAutoSearchByState(string searchString, Int64 CountryId, string StateString, Int64 StateId)
        {
            if (string.IsNullOrEmpty(StateString))
            {
                StateId = 0;
            }

            CityDA commonDA = new CityDA();
            List<CityBO> countryList = commonDA.GetCityInfoBySearchAutoSearchByState(searchString, CountryId, StateId);

            return countryList;
        }
        [WebMethod]
        public static List<LocationBO> LoadLocationForAutoSearchByCity(string searchString, Int64 CountryId, string StateString, Int64 StateId, Int64 CityId, string CityString)
        {
            if (string.IsNullOrEmpty(StateString))
            {
                StateId = 0;
            }

            if (string.IsNullOrEmpty(CityString))
            {
                CityId = 0;
            }

            LocationDA commonDA = new LocationDA();
            List<LocationBO> countryList = commonDA.GetLocationInfoBySearchAutoSearchByCity(searchString, CountryId, StateId, CityId);

            return countryList;
        }
        [WebMethod]
        public static int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate, string pkId)
        {
            string tableName = "HotelGuestCompany";
            string pkFieldName = "CompanyId";
            string pkFieldValue = pkId;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
        [WebMethod]
        public static ArrayList FillForm(int Id)
        {
            GuestCompanyBO guestCompanyBO = new GuestCompanyBO();
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            ContactInformationDA DA = new ContactInformationDA();

            List<SMContactDetailsBO> detailsBOs = new List<SMContactDetailsBO>();
            detailsBOs = DA.GetContactDetails(Id, "Company");

            guestCompanyBO = guestCompanyDA.GetGuestCompanyInfoById(Id);
            List<int> CRMCompanyIdList = guestCompanyDA.GetGuestCRMCompanyInfoById(Id);


            var numbers = detailsBOs.Where(x => x.TransectionType == "Number").ToList();
            var emails = detailsBOs.Where(x => x.TransectionType == "Email").ToList();
            var fax = detailsBOs.Where(x => x.TransectionType == "Fax").ToList();
            var websites = detailsBOs.Where(x => x.TransectionType == "Website").ToList();

            ArrayList arr = new ArrayList();
            arr.Add(new { Numbers = numbers, Emails = emails, Fax = fax, Websites = websites, GuestCompany = guestCompanyBO, CRMCompanyIds = CRMCompanyIdList.ToArray()});

            return arr;
        }
        [WebMethod]
        public static int CheckLifeCycleStageValidation(GuestCompanyBO CompanyBO)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            int IsValid = 0;
            GuestCompanyBO PreviousBO = new GuestCompanyBO();
            GuestCompanyDA DA = new GuestCompanyDA();
            CompanyBO.CreatedBy = userInformationBO.UserInfoId;
            if (CompanyBO.CompanyId != 0)
            {
                PreviousBO = DA.GetGuestCompanyInfoById(CompanyBO.CompanyId);
                if (!CheckLifeCycleStageCanBeChange(CompanyBO, PreviousBO))
                {
                    IsValid = 1;
                }
            }
            else
            {
                if (!CheckLifeCycleStageCanBeChange(CompanyBO, PreviousBO))
                {
                    IsValid = 1;
                }
            }
            return IsValid;
        }
        [WebMethod]
        public static ReturnInfo SaveUpdateCompany(GuestCompanyBO CompanyBO, int[] CRMCompanyIds, string hfRandom, string deletedDocument, List<SMContactDetailsBO> newlyAddedItem, List<SMContactDetailsBO> deleteDbItem)
        {
            int OwnerIdForDocuments = 0;
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();

            ContactInformationDA DA = new ContactInformationDA();
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            SalesMarketingLogType<GuestCompanyBO> logDA = new SalesMarketingLogType<GuestCompanyBO>();
            try
            {
                int OutId = 0;
                GuestCompanyBO PreviousBO = new GuestCompanyBO();
                if (CompanyBO.CompanyId == 0)//save
                {
                    CompanyBO.CreatedBy = userInformationBO.UserInfoId;
                    status = guestCompanyDA.SaveGuestCompanyInfo(CompanyBO, CRMCompanyIds, out OutId);

                    if (status)
                    {
                        OwnerIdForDocuments = OutId;
                        CompanyBO.CompanyId = OutId;
                        rtninfo.IsSuccess = true;
                        logDA.Log(ConstantHelper.SalesandMarketingLogType.CompanyCreated, CompanyBO, CompanyBO);

                        int tmpNodeId = 0;
                        string paymentMode = "Company";

                        tmpNodeId = hmCommonDA.GetCommonPaymentModeInfo(paymentMode).FirstOrDefault().ReceiveAccountsPostingId;
                        Boolean postingStatus = hmCommonDA.UpdateTableInfoForNodeId("HotelGuestCompany", "CompanyId", OutId, tmpNodeId);

                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                        EntityTypeEnum.EntityType.GuestCompany.ToString(), OutId,
                        ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestCompany));
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                }
                else
                {
                    CompanyBO.LastModifiedBy = userInformationBO.UserInfoId;
                    GuestCompanyBO previousBO = new GuestCompanyBO();
                    previousBO = guestCompanyDA.GetGuestCompanyInfoById(CompanyBO.CompanyId);
                    status = guestCompanyDA.UpdateGuestCompanyInfo(CompanyBO, CRMCompanyIds);

                    if (status)
                    {
                        ContactInformationDA contactInformationDA = new ContactInformationDA();
                        OutId = CompanyBO.CompanyId;
                        if (!string.IsNullOrEmpty(deletedDocument))
                        {
                            bool delete = contactInformationDA.DeleteContactDocument(deletedDocument);
                        }

                        logDA.Log(ConstantHelper.SalesandMarketingLogType.CompanyActivity, CompanyBO, previousBO);

                        int tmpNodeId = 0;
                        string paymentMode = "Company";

                        tmpNodeId = hmCommonDA.GetCommonPaymentModeInfo(paymentMode).FirstOrDefault().ReceiveAccountsPostingId;
                        Boolean postingStatus = hmCommonDA.UpdateTableInfoForNodeId("HotelGuestCompany", "CompanyId", CompanyBO.CompanyId, tmpNodeId);

                        OwnerIdForDocuments = CompanyBO.CompanyId;
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                        EntityTypeEnum.EntityType.GuestCompany.ToString(), CompanyBO.CompanyId,
                        ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestCompany));
                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }
                if (status)
                {
                    bool detailsStatus = DA.SaveContactDetails(newlyAddedItem, deleteDbItem, OutId);
                    Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(hfRandom));
                }

            }
            catch (Exception ex)
            {
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                throw ex;
            }
            return rtninfo;
        }
        [WebMethod]
        public static List<DocumentsBO> LoadCompanyDocument(long id, int randomId, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }

            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("CompanyDocument", randomId);

            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("CompanyDocument", (int)id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));

            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }

        private static bool CheckLifeCycleStageCanBeChange(GuestCompanyBO current, GuestCompanyBO previous = null)
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
                        if (lifeCycle.Id == current.LifeCycleStageId)
                            currentStageSequence = (int)lifeCycle.DisplaySequence;
                        if (lifeCycle.Id == previous.LifeCycleStageId)
                            previousStageSequence = (int)lifeCycle.DisplaySequence;
                    }
                    return Math.Abs(previousStageSequence - currentStageSequence) > 1 ? false : true;
                }
                else
                    return current.LifeCycleStageId == lifeCycles[0].Id;
            }
            return true;
        }
    }
}