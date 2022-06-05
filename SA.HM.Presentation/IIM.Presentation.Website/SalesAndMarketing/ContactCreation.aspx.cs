using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Data.UserInformation;
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
    public partial class ContactCreation : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        ContactInformationDA DA = new ContactInformationDA();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCRMConfiguration();
                LoadAccountManager();
                CheckUserInfo();
                LoadLifeCycleStage();
                LoadSourceInformation();
            }

        }
        private void LoadCRMConfiguration()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            BillingAreaLabel.Visible = false;
            txtBillingLocation.Visible = false;
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCRMAreaFieldEnable", "IsCRMAreaFieldEnable");
            if (setUpBO.SetupId > 0)
            {
                if (setUpBO.SetupValue == "1")
                {
                    BillingAreaLabel.Visible = true;
                    txtBillingLocation.Visible = true;
                }
            }

            hfIsContactHyperlinkEnableFromGrid.Value = "0";
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsContactHyperlinkEnableFromGrid", "IsContactHyperlinkEnableFromGrid");
            if (setUpBO.SetupId > 0)
            {
                if (setUpBO.SetupValue == "1")
                {
                    hfIsContactHyperlinkEnableFromGrid.Value = "1";
                }
            }

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
        private void LoadLifeCycleStage()
        {
            LifeCycleStageDA lifeCycleStageDA = new LifeCycleStageDA();
            List<SMLifeCycleStageBO> sMLifeCycles = new List<SMLifeCycleStageBO>();
            sMLifeCycles = lifeCycleStageDA.GetLifeCycleForDdl();

            ddlSrcLifeCycleStage.DataSource = sMLifeCycles;
            ddlSrcLifeCycleStage.DataTextField = "LifeCycleStage";
            ddlSrcLifeCycleStage.DataValueField = "Id";
            ddlSrcLifeCycleStage.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlSrcLifeCycleStage.Items.Insert(0, item);
        }
        private void LoadSourceInformation()
        {
            SetupDA sourceNameDA = new SetupDA();
            List<SMSourceInformationBO> sourceNameBOList = new List<SMSourceInformationBO>();
            sourceNameBOList = sourceNameDA.GetSourceInfoForDDL();

            ddlContactSource.DataSource = sourceNameBOList;
            ddlContactSource.DataTextField = "SourceName";
            ddlContactSource.DataValueField = "Id";
            ddlContactSource.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlContactSource.Items.Insert(0, item);
        }
        private void CheckUserInfo()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            hfIsAdminUser.Value = userInformationBO.IsAdminUser.ToString();
        }
        private void LoadAccountManager()
        {
            int isAdminUser = 0;
            ddlContactOwner.Enabled = true;
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

            ddlContactOwner.DataSource = accountManagerBOList;
            ddlContactOwner.DataTextField = "DisplayName";
            ddlContactOwner.DataValueField = "UserInfoId";
            ddlContactOwner.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlContactOwner.Items.Insert(0, item);

            //if (accountManagerBOList != null)
            //{
            //    if (accountManagerBOList.Count > 1)
            //    {
            //        ListItem item = new ListItem();
            //        item.Value = "0";
            //        item.Text = hmUtility.GetDropDownFirstAllValue();
            //        ddlContactOwner.Items.Insert(0, item);
            //    }
            //    else
            //    {
            //        ddlContactOwner.Enabled = false;
            //    }
            //}
        }
        [WebMethod]
        public static List<GuestCompanyBO> CompanySearch(string searchTerm)
        {
            List<GuestCompanyBO> itemInfo = new List<GuestCompanyBO>();

            GuestCompanyDA itemDa = new GuestCompanyDA();
            itemInfo = itemDa.GetCompanyInfoBySearchCriteria(searchTerm);

            return itemInfo;
        }
        [WebMethod]
        public static ReturnInfo DeleteContact(long Id)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            ContactInformationDA DA = new ContactInformationDA();
            status = DA.DeleteContact(Id);
            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                               EntityTypeEnum.EntityType.SalaryHead.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalaryHead));
            }
            return rtninf;
        }
        [WebMethod]
        public static GridViewDataNPaging<ContactInformationBO, GridPaging> LoadContactForSearch(string ContactName, int CompanyId, string ContactNo, string ContactEmail, Int64 countryId, Int64 stateId, Int64 cityId, Int64 areaId, int lifeCycleStage, int contactSource, Int32 contactOwnerId, Int32 dateSearchCriteria, string SearchFromDate, string SearchToDate, string ContactType, string contactNumber, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;
            DateTime dateTime = DateTime.Now;
            string startDate = string.Empty;
            string endDate = string.Empty;
            if (string.IsNullOrWhiteSpace(SearchFromDate))
            {
                startDate = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = SearchFromDate;
            }
            if (string.IsNullOrWhiteSpace(SearchToDate))
            {
                endDate = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = SearchToDate;
            }
            DateTime fromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime toDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                        
            GridViewDataNPaging<ContactInformationBO, GridPaging> myGridData = new GridViewDataNPaging<ContactInformationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<ContactInformationBO> contactInformation = new List<ContactInformationBO>();
            ContactInformationDA DA = new ContactInformationDA();

            // // // ------User Admin Authorization BO Session Information --------------------------------
            int isAdminUser = 0;
            AccountManagerDA accountManagerDA = new AccountManagerDA();
            List<AccountManagerBO> accountManagerBOList = new List<AccountManagerBO>();

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

            if (contactOwnerId == 0)
            {
                isAdminUser = 1;
            }
            #endregion

            contactInformation = DA.GetContactInformation(isAdminUser, userInformationBO.UserInfoId, ContactName, CompanyId, ContactNo, ContactEmail, countryId, stateId, cityId, areaId, lifeCycleStage, contactSource, contactOwnerId, dateSearchCriteria, fromDate, toDate, ContactType, contactNumber, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(contactInformation, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static string LoadContactDocument(long id)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("ContactDocument", (int)id);
            docList = new HMCommonDA().GetDocumentListWithIcon(docList);

            string strTable = "";
            strTable += "<div style='color: White; background-color: #44545E;width:750px;'>";
            int counter = 0;
            foreach (DocumentsBO dr in docList)
            {
                if (dr.Extention == ".jpg")
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:250px; height:250px; float:left;padding:30px'>";
                    strTable += "<a style='color:#333333;' target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img style='width: 200px; height: 200px;' src='" + ImgSource + "'  alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
                else
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:100px; height:100px; float:left;padding:30px'>";
                    strTable += "<a style='color:#333333;' target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img style='width: 100px; height: 100px;' src='" + dr.IconImage + "' alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
            }
            strTable += "</div>";
            if (strTable == "")
            {
                strTable = "<tr><td align='center'>No Record Available!</td></tr>";
            }
            return strTable;

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/SalesAndMarketing/ContactCreation.aspx");
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
        public static List<ContactInformationBO> LoadLabelByAutoSearch(string searchTerm)
        {
            ContactInformationDA DA = new ContactInformationDA();
            List<ContactInformationBO> titleList = new List<ContactInformationBO>();
            titleList = DA.GetContactByAutoSearch(searchTerm);
            return titleList;
        }
    }
}