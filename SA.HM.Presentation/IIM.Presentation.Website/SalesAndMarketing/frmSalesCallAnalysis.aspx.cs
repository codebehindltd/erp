using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Data.SalesAndMarketing;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class frmSalesCallAnalysis : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadCompany();
                LoadOpportunityStatus();
                LoadCompanySiteFirstValue();
            }
        }

        private void LoadCompany()
        {
            GuestCompanyDA companyDA = new GuestCompanyDA();
            List<GuestCompanyBO> companyBOList = new List<GuestCompanyBO>();
            companyBOList = companyDA.GetGuestCompanyInfo();

            ddlCompany.DataSource = companyBOList;
            ddlCompany.DataTextField = "CompanyName";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCompany.Items.Insert(0, item);
        }

        private void LoadCompanySiteFirstValue()
        {
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCompanySite.Items.Insert(0, item);
        }

        private void LoadCompanySite(int companyId)
        {
            GuestCompanyDA companyDA = new GuestCompanyDA();
            List<CompanySiteBO> siteList = new List<CompanySiteBO>();
            siteList = companyDA.GetCompanySiteByCompanyId(companyId);

            ddlCompanySite.DataSource = siteList;
            ddlCompanySite.DataTextField = "SiteName";
            ddlCompanySite.DataValueField = "SiteId";
            ddlCompanySite.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCompanySite.Items.Insert(0, item);
        }

        private void LoadOpportunityStatus()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("OpportunityStatus", hmUtility.GetDropDownFirstValue());

            this.ddlOpportunityStatus.DataSource = fields;
            this.ddlOpportunityStatus.DataTextField = "FieldValue";
            this.ddlOpportunityStatus.DataValueField = "FieldId";
            this.ddlOpportunityStatus.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int companyId = 0, siteId = 0, opportunityStatus = 0;

            if (ddlCompany.SelectedValue != "0")
            {
                companyId = Convert.ToInt32(ddlCompany.SelectedValue);
            }

            if (ddlCompanySite.SelectedValue != "0")
            {
                siteId = Convert.ToInt32(ddlCompanySite.SelectedValue);
            }

            if (ddlOpportunityStatus.SelectedValue != "0")
            {
                opportunityStatus = Convert.ToInt32(ddlOpportunityStatus.SelectedValue);
            }

            SalesCallDA detalisDA = new SalesCallDA();
            List<SalesCallViewBO> allOrderList = new List<SalesCallViewBO>();
            allOrderList = detalisDA.GetSalesCallByOpportunityStatus(companyId, siteId, opportunityStatus);

            gvOrderInfo.DataSource = allOrderList;
            gvOrderInfo.DataBind();
        }
        protected void gvOrderInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static List<PMPurchaseOrderDetailsBO> PerformLoadPMProductDetailOnDisplayMode(string pOrderId)
        {
            PMPurchaseOrderBO orderBO = new PMPurchaseOrderBO();
            PMPurchaseOrderDA orderDetailDA = new PMPurchaseOrderDA();
            List<PMPurchaseOrderDetailsBO> orderDetailListBO = new List<PMPurchaseOrderDetailsBO>();
            orderDetailListBO = orderDetailDA.GetSMSalesOrderDetailByOrderId(Int32.Parse(pOrderId));
            return orderDetailListBO;
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCompanySite(Convert.ToInt32(ddlCompany.SelectedValue));
        }
    }
}