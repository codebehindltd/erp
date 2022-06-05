using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class LogActivityInformation : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCRMConfiguration();
                LoadAccountManager();
            }
        }

        private void LoadCRMConfiguration()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            hfIsCompanyHyperlinkEnableFromGrid.Value = "0";
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCompanyHyperlinkEnableFromGrid", "IsCompanyHyperlinkEnableFromGrid");
            if (setUpBO.SetupId > 0)
            {
                if (setUpBO.SetupValue == "1")
                {
                    hfIsCompanyHyperlinkEnableFromGrid.Value = "1";
                }
            }
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
                    item.Text = hmUtility.GetDropDownFirstValue();
                    ddlCompanyOwner.Items.Insert(0, item);
                }
                else
                {
                    ddlCompanyOwner.Enabled = false;
                }
            }

            ddlCompanyOwner.SelectedValue = userInformationBO.UserInfoId.ToString();
        }
        [WebMethod]
        public static GridViewDataNPaging<SalesCallEntryBO, GridPaging> LoadLogActivityInformationForSearch(int company, int accountManager, string logtype, int deal, string fromDate, string toDate, int industry,
                                                                        int contact, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int status = -1;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<SalesCallEntryBO, GridPaging> myGridData = new GridViewDataNPaging<SalesCallEntryBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<SalesCallEntryBO> contactInformation = new List<SalesCallEntryBO>();
            SalesCallDA DA = new SalesCallDA();
            contactInformation = DA.GetLogActivityInformationForSearch(logtype, fromDate, toDate, company, accountManager, industry, deal, contact, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(contactInformation, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static List<GuestCompanyBO> GetCompanyByAutoSearchAndAccountManagerId(string searchTerm, int accountManagerId, int industryId)
        {
            List<GuestCompanyBO> companyInfo = new List<GuestCompanyBO>();
            GuestCompanyDA itemDa = new GuestCompanyDA();
            companyInfo = itemDa.GetGuestCompanyInfoByCompanyNameAccountManagerIdAndIndustryId(searchTerm, accountManagerId, industryId);

            return companyInfo;
        }
        [WebMethod]
        public static List<IndustryBO> GetIndustryInfoBySearchCriteria(string searchTerm)
        {
            List<IndustryBO> IndustryBO = new List<IndustryBO>();
            IndustryDA itemDa = new IndustryDA();
            IndustryBO = itemDa.GetIndustryInfoByAutoSearch(searchTerm);

            return IndustryBO;
        }
        [WebMethod]
        public static List<ContactInformationBO> GetContactByAccountManagerNCompany(string searchTerm, int accountManagerId, int companyId)
        {
            List<ContactInformationBO> IndustryBO = new List<ContactInformationBO>();
            ContactInformationDA itemDa = new ContactInformationDA();
            IndustryBO = itemDa.GetContactByCompanyIdNAccountManager(accountManagerId, companyId, searchTerm);

            return IndustryBO;
        }
        [WebMethod]
        public static List<SMDeal> GetDealByCompanyIdContactIdNAccountManager(string searchText, int contactId, int companyId, int accountManagerId)
        {
            List<SMDeal> IndustryBO = new List<SMDeal>();
            DealDA itemDa = new DealDA();
            IndustryBO = itemDa.GetDealByCompanyIdContactIdNAccountManager(searchText, contactId, companyId, accountManagerId);

            return IndustryBO;
        }
    }
}