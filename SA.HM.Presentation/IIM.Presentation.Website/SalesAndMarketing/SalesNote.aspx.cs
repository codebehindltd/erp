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
    public partial class SalesNote : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        public bool IsInventoryUser = false;
        public bool IsTechnicalUser = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            IsInventoryUser = hmUtility.GetCurrentApplicationUserInfo().UserGroupType == ConstantHelper.UserGroupType.Inventory.ToString();
            IsTechnicalUser = hmUtility.GetCurrentApplicationUserInfo().UserGroupType == ConstantHelper.UserGroupType.Technical.ToString();
            if (!IsPostBack)
                LoadCompany();
        }

        private void LoadCompany()
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> files = new List<GuestCompanyBO>();
            files = guestCompanyDA.GetALLGuestCompanyInfo();


            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            this.ddlCompany.DataSource = files;
            this.ddlCompany.DataTextField = "CompanyName";
            this.ddlCompany.DataValueField = "CompanyId";
            this.ddlCompany.DataBind();

            this.ddlCompany.Items.Insert(0, item);
        }
        [WebMethod]
        public static GridViewDataNPaging<SMQuotationViewBO, GridPaging> SearchQuotation(string quotationNumber, int companyId, DateTime? fromDate, DateTime? toDate, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<SMQuotationViewBO, GridPaging> myGridData = new GridViewDataNPaging<SMQuotationViewBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            SalesQuotationNBillingDA invItemDA = new SalesQuotationNBillingDA();
            List<SMQuotationViewBO> invItemList = new List<SMQuotationViewBO>();
            invItemList = invItemDA.GetQuotationForSalesBillOrSalesNote(quotationNumber, companyId, fromDate, toDate, "SalesNote", userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(invItemList, totalRecords);

            return myGridData;
        }
    }
}