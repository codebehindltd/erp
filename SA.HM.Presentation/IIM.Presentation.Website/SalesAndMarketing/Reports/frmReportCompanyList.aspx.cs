using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing.Reports
{
    public partial class frmReportCompanyList : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int _CompanyListReportShow = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadCompanyType();
                this.LoadAccountManager();
                this.LoadLifeCycleStage();
            }
        }

        [WebMethod]
        public static List<GuestCompanyBO> GetCompanyByAutoSearch(string searchTerm)
        {
            List<GuestCompanyBO> companyInfo = new List<GuestCompanyBO>();
            GuestCompanyDA itemDa = new GuestCompanyDA();
            companyInfo = itemDa.GetGuestCompanyInfoByCompanyName(searchTerm);

            return companyInfo;
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

        private void LoadCompanyType()
        {
            SalesMarketingDA salesMarketingDA = new SalesMarketingDA();
            List<SMCompanyTypeInformationBO> typeBO = new List<SMCompanyTypeInformationBO>();
            typeBO = salesMarketingDA.GetCompanyTypeForDDL();
            ddlSrcCompanyType.DataSource = typeBO;
            ddlSrcCompanyType.DataTextField = "TypeName";
            ddlSrcCompanyType.DataValueField = "Id";
            ddlSrcCompanyType.DataBind();

            ListItem itemType = new ListItem();
            itemType.Value = "0";
            itemType.Text = hmUtility.GetDropDownFirstAllValue();
            ddlSrcCompanyType.Items.Insert(0, itemType);
        }

        private void LoadLifeCycleStage()
        {
            LifeCycleStageDA lifeCycleStageDA = new LifeCycleStageDA();
            List<SMLifeCycleStageBO> sMLifeCycles = new List<SMLifeCycleStageBO>();
            sMLifeCycles = lifeCycleStageDA.GetLifeCycleForDdl();
            ddlSrcLifeCycleStage.DataSource = sMLifeCycles;
            ddlSrcLifeCycleStage.DataTextField = "LifeCycleStage";
            ddlSrcLifeCycleStage.DataValueField = "Id";
            ddlSrcLifeCycleStage.DataBind();

            ListItem itemStage = new ListItem();
            itemStage.Value = "0";
            itemStage.Text = hmUtility.GetDropDownFirstAllValue();
            ddlSrcLifeCycleStage.Items.Insert(0, itemStage);
        }

        private void LoadAccountManager()
        {
            int isAdminUser = 0;
            ddlSrcOwnerId.Enabled = true;
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

            ddlSrcOwnerId.DataSource = accountManagerBOList;
            ddlSrcOwnerId.DataTextField = "DisplayName";
            ddlSrcOwnerId.DataValueField = "UserInfoId";
            ddlSrcOwnerId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlSrcOwnerId.Items.Insert(0, item);
            ddlCompanyOwner.Items.Insert(0, item);

            //if (accountManagerBOList != null)
            //{
            //    if (accountManagerBOList.Count > 1)
            //    {
            //        ListItem item = new ListItem();
            //        item.Value = "0";
            //        item.Text = hmUtility.GetDropDownFirstAllValue();
            //        ddlSrcOwnerId.Items.Insert(0, item);
            //        ddlCompanyOwner.Items.Insert(0, item);
            //    }
            //    else
            //    {
            //        ddlSrcOwnerId.Enabled = false;
            //        ddlCompanyOwner.Enabled = false;
            //    }
            //}
        }


        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            GuestCompanyBO guestCompanyBO = new GuestCompanyBO();
            _CompanyListReportShow = 1;
            //int totalRecords = 0;
            string CompanyNumber = txtCompanyNumber.Text;
            string guestCompanyName = hfGuestCompanyName.Value;
            string ContactNumber = txtSContactNumber.Text;
            string mail = txtSCompanyEmail.Text;
            Int64 countryId = 0;
            Int64 stateId = 0;
            Int64 cityId = 0;
            Int64 areaId = 0;
            int totalRecords = 0;
            int pageNumber = 0;
            guestCompanyBO.CompanyOwnerId = Convert.ToInt32(ddlCompanyOwner.SelectedValue);
            int companyOwnerId = Convert.ToInt32(ddlSrcOwnerId.SelectedValue);

            if (!string.IsNullOrWhiteSpace(txtBillingCountry.Text))
            {
                countryId = Convert.ToInt32(hfBillingCountryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(txtBillingState.Text))
            {
                stateId = Convert.ToInt32(hfBillingStateId.Value);
            }

            if (!string.IsNullOrWhiteSpace(txtBillingCity.Text))
            {
                cityId = Convert.ToInt32(hfBillingCityId.Value);
            }

            if (!string.IsNullOrWhiteSpace(txtBillingLocation.Text))
            {
                areaId = Convert.ToInt32(hfBillingLocationId.Value);
            }

            int lifeCycleStage = Convert.ToInt32(ddlSrcLifeCycleStage.SelectedValue);
            int companyType = Convert.ToInt32(ddlSrcCompanyType.SelectedValue);
            int dateSearchCriteria = Convert.ToInt32(ddlCriteria.SelectedValue);
            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/rptCompanyList.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            List<ReportParameter> paramReport = new List<ReportParameter>();

            string companyName = string.Empty;
            string companyAddress = string.Empty;
            string webAddress = string.Empty;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
            if (files[0].CompanyId > 0)
            {
                companyName = files[0].CompanyName;
                companyAddress = files[0].CompanyAddress;
                //paramReport.Add(new ReportParameter("CompanyProfile", fileCompany[0].Name));
                //paramReport.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    //paramReport.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                    webAddress = files[0].WebAddress;
                }
                else
                {
                    //paramReport.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                    webAddress = files[0].ContactNumber;
                }
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;

            DateTime dateTime = DateTime.Now;
            string startDate = string.Empty;
            string endDate = string.Empty;
            if (string.IsNullOrWhiteSpace(this.txtSearchFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtSearchFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = this.txtSearchFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtSearchToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtSearchToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = this.txtSearchToDate.Text;
            }
            DateTime fromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime toDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);


            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));

            string imageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("CompanyProfile", companyName));
            paramReport.Add(new ReportParameter("CompanyAddress", companyAddress));
            paramReport.Add(new ReportParameter("CompanyWeb", webAddress));
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));

            rvTransaction.LocalReport.SetParameters(paramReport);


            // // // ------User Admin Authorization BO Session Information --------------------------------
            int isAdminUser = 0;

            // // // -----------IsHotelGuestCompanyRestrictionForAllUsers
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsHotelGuestCompanyRestrictionForAllUsers", "IsHotelGuestCompanyRestrictionForAllUsers");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                if (setUpBO.SetupValue == "1")
                {
                    isAdminUser = 1;
                }
                else
                {
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
                }
            }

            List<GuestCompanyBO> viewList = new List<GuestCompanyBO>();
            GuestCompanyDA guestCompanyListDa = new GuestCompanyDA();
            viewList = guestCompanyListDa.GetGuestCompanyInfoBySearchCriteriaForReport(isAdminUser, userInformationBO.UserInfoId, guestCompanyName, companyType, ContactNumber, mail, countryId, stateId, cityId, areaId, lifeCycleStage, companyOwnerId, dateSearchCriteria, fromDate, toDate, CompanyNumber, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewList));

            rvTransaction.LocalReport.DisplayName = "Cash Requisition And Adjustment";

            rvTransaction.LocalReport.Refresh();

        }
    }
}