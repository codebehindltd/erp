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
using System.Web.Services;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class SMQuotationSearch : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCompany();
                LoadSite();
                LoadDeal();
                LoadCRMConfiguaration();
            }
        }
        private void LoadDeal()
        {
            List<SMDeal> dealInfoBOs = new List<SMDeal>();
            SalesMarketingDA dealDA = new SalesMarketingDA();

            dealInfoBOs = dealDA.GetDealInfo();

            ddlDeal.DataSource = dealInfoBOs;
            ddlDeal.DataTextField = "Name";
            ddlDeal.DataValueField = "Id";
            ddlDeal.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlDeal.Items.Insert(0, item);
        }
        private void LoadSite()
        {
            List<SMCompanySiteBO> siteBOs = new List<SMCompanySiteBO>();
            SalesMarketingDA marketingDA = new SalesMarketingDA();

            siteBOs = marketingDA.GetSite();

            ddlSCompanySite.DataSource = siteBOs;
            ddlSCompanySite.DataTextField = "SiteName";
            ddlSCompanySite.DataValueField = "SiteId";
            ddlSCompanySite.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSCompanySite.Items.Insert(0, item);
        }
        private void LoadCompany()
        {
            GuestCompanyDA companyDA = new GuestCompanyDA();
            List<GuestCompanyBO> companyBOList = new List<GuestCompanyBO>();
            companyBOList = companyDA.GetGuestCompanyInfo();

            ddlSCompany.DataSource = companyBOList;
            ddlSCompany.DataTextField = "CompanyName";
            ddlSCompany.DataValueField = "CompanyId";
            ddlSCompany.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSCompany.Items.Insert(0, item);
        }
        private void LoadCRMConfiguaration()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDeviceOrUserWillShow", "IsDeviceOrUserWillShow");

            DeviceNUser.Visible = setUpBO.SetupValue == "1";

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDeliveryWillShow", "IsDeliveryWillShow");

            Delivery.Visible = setUpBO.SetupValue == "1";

        }
        [WebMethod]
        public static ReturnInfo PerformApproval(int quotationId, int dealId)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninf = new ReturnInfo();
            SalesQuotationEditBO quotationv = new SalesQuotationEditBO();
            SalesQuotationNBillingDA salesDA = new SalesQuotationNBillingDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            try
            {

                //quotationv.Quotation = salesDA.GetQuotationById(quotationId);
                rtninf.IsSuccess = salesDA.UpdateApproval(quotationId, dealId, false, userInformationBO.UserInfoId);

                if (rtninf.IsSuccess)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.SMQuotation.ToString(), quotationId,
                           ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMQuotation));
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                    rtninf.Data = new { DealId = dealId, QuotationId = quotationId };
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return rtninf;

        }
        [WebMethod]
        public static ReturnInfo CancelQuotation(int quotationId, int dealId)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninf = new ReturnInfo();
            SalesQuotationEditBO quotationv = new SalesQuotationEditBO();
            SalesQuotationNBillingDA salesDA = new SalesQuotationNBillingDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            try
            {

                //quotationv.Quotation = salesDA.GetQuotationById(quotationId);
                rtninf.IsSuccess = salesDA.UpdateApproval(quotationId, dealId, true, userInformationBO.UserInfoId);

                if (rtninf.IsSuccess)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.SMQuotation.ToString(), quotationId,
                           ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMQuotation));
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Cancel, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return rtninf;

        }
        [WebMethod]
        public static SMQuotationDetailsViewBO GetQuotationDetailById(Int64 quotationId)
        {
            SMQuotationDetailsViewBO quotation = new SMQuotationDetailsViewBO();
            SalesQuotationNBillingDA salesDA = new SalesQuotationNBillingDA();

            quotation.Quotation = salesDA.GetQuotationDetailsViewById(quotationId);
            quotation.Company = new GuestCompanyDA().GetGuestCompanyInfoById(quotation.Quotation.CompanyId);
            if (quotation.Quotation.ContactId > 0)
                quotation.Contact = new ContactInformationDA().GetContactInformationByIdForView(Convert.ToInt64(quotation.Quotation.ContactId));
            quotation.QuotationItemDetails = salesDA.GetQuotationItemDetailsById(quotationId, CommonHelper.QuotationItemType.Item.ToString());
            quotation.QuotationServiceDetails = salesDA.GetQuotationServiceDetailsById(quotationId, CommonHelper.QuotationItemType.Service.ToString());
            quotation.QuotationDetails = salesDA.GetQuotationDetailsById(quotationId).Where(i => i.ItemType != "Item" && i.ItemType != "Service").OrderBy(i => i.ItemType).ToList();

            return quotation;
        }
        [WebMethod]
        public static GridViewDataNPaging<SMQuotationViewBO, GridPaging> GetQuotationForPagination(int companyId, int dealId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<SMQuotationViewBO, GridPaging> myGridData = new GridViewDataNPaging<SMQuotationViewBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            SalesQuotationNBillingDA invItemDA = new SalesQuotationNBillingDA();
            List<SMQuotationViewBO> invItemList = new List<SMQuotationViewBO>();
            invItemList = invItemDA.GetQuotationForPagination(companyId, dealId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(invItemList, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static List<CompanySiteBO> LoadCompanySite(int companyId)
        {
            GuestCompanyDA companyDA = new GuestCompanyDA();
            List<CompanySiteBO> siteList = new List<CompanySiteBO>();
            siteList = companyDA.GetCompanySiteByCompanyId(companyId);

            return siteList;
        }

    }
}