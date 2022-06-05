using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.SalesAndMarketing;
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
    public partial class DealImplementationFeedback : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCompany();
                LoadCategory();
            }
        }        
        
        private void LoadCompany()
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> files = new List<GuestCompanyBO>();
            files = guestCompanyDA.GetALLGuestCompanyInfo();


            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();

            ddlSearchCompany.DataSource = files;
            ddlSearchCompany.DataTextField = "CompanyName";
            ddlSearchCompany.DataValueField = "CompanyId";
            ddlSearchCompany.DataBind();

            ddlSearchCompany.Items.Insert(0, item);
        }

        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetAllActiveInvItemCatagoryInfoByServiceType("Product");
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();

            List<InvCategoryBO> serviceCategory = new List<InvCategoryBO>();
            serviceCategory = da.GetAllActiveInvItemCatagoryInfoByServiceType("Service");

        }           

        [WebMethod]
        public static GridViewDataNPaging<SMDeal, GridPaging> LoadGridPaging(string dealNumber, string name, int companyId, string dateType, string fromDate, string toDate, int gridRecordsCount, int pageNumber, int IsCurrentOrPreviousPage)
        {

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;
            if (fromDate != "" && toDate == "")
            {
                toDate = DateTime.Now.ToShortDateString();
            }

            GridViewDataNPaging<SMDeal, GridPaging> myGridData = new GridViewDataNPaging<SMDeal, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, IsCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<SMDeal> deals = new List<SMDeal>();
            DealDA dealDA = new DealDA();
            deals = dealDA.GetDealInfoForSearchForImplemantationFeedback(dealNumber, name, companyId, dateType, fromDate, toDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(deals, totalRecords);

            return myGridData;
        }        
    }
}