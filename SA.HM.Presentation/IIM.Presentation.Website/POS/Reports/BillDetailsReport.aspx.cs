using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.Restaurant;
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

namespace HotelManagement.Presentation.Website.POS.Reports
{
    public partial class BillDetailsReport : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int _IsReportPanelEnable = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCostCenter();
                LoadCategory();
                LoadCommonDropDownHiddenField();
                LoadRestaurantUserInfo();
            }
            
        }
        private void LoadCostCenter()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> list = new List<CostCentreTabBO>();
            list = costCentreTabDA.GetAllCostCentreTabInfo().Where(x => x.IsRestaurant == true).ToList();

            ddlCostCenter.DataSource = list;
            ddlCostCenter.DataTextField = "CostCenter";
            ddlCostCenter.DataValueField = "CostCenterId";
            ddlCostCenter.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCostCenter.Items.Insert(0, item);

        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetAllInvItemCatagoryInfoByServiceType("Product");
            this.ddlCategory.DataSource = List;
            this.ddlCategory.DataTextField = "MatrixInfo";
            this.ddlCategory.DataValueField = "CategoryId";
            this.ddlCategory.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCategory.Items.Insert(0, item);
        }
        private void LoadRestaurantUserInfo()
        {
            RestaurantBearerDA costCentreTabDA = new RestaurantBearerDA();

            this.ddlCashierInfoId.DataSource = costCentreTabDA.GetRestaurantUserInfo("Cashier");
            this.ddlCashierInfoId.DataTextField = "UserName";
            this.ddlCashierInfoId.DataValueField = "UserInfoId";
            this.ddlCashierInfoId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCashierInfoId.Items.Insert(0, item);
            
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _IsReportPanelEnable = 1;

            HMCommonDA hmCommonDA = new HMCommonDA();
            string fromDate = string.Empty;
            string toDate = string.Empty;
            DateTime currentDate = DateTime.Now;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
             reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/BillDetailsReport.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            List<BillDetailsBO> billDetails = new List<BillDetailsBO>();
            BillDetailsDA billDA = new BillDetailsDA();
            List<ReportParameter> paramReport = new List<ReportParameter>();

            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                fromDate = hmUtility.GetStringFromDateTime(currentDate);
                this.txtFromDate.Text = hmUtility.GetStringFromDateTime(currentDate);
            }
            else
            {
                fromDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                toDate = hmUtility.GetStringFromDateTime(currentDate);
                this.txtToDate.Text = hmUtility.GetStringFromDateTime(currentDate);
            }
            else
            {
                toDate = this.txtToDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            ToDate = ToDate.AddDays(1).AddSeconds(-1);

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("CompanyProfile", hmUtility.GetHMCompanyProfile()));
            paramReport.Add(new ReportParameter("CompanyAddress", hmUtility.GetHMCompanyAddress()));

            string reportName = "Bill Details";
            paramReport.Add(new ReportParameter("ReportName", reportName));

            //parameters
            int categoryId = Convert.ToInt32(this.ddlCategory.SelectedValue);
            int itemId = !string.IsNullOrWhiteSpace(txtHiddenItemId.Value) ? Convert.ToInt32(txtHiddenItemId.Value) : 0;
            string billNumber = !string.IsNullOrWhiteSpace(this.txtBillNo.Text) ? this.txtBillNo.Text : "All";
            int costCenterId = Convert.ToInt32(this.ddlCostCenter.SelectedValue);
            int cashierId = Convert.ToInt32(this.ddlCashierInfoId.SelectedValue);
            string paymentType = ddlPaymentMode.SelectedValue;
            if (paymentType == "OtherRoom")
            {
                paymentType = "Other Room";
            }
            billDetails = billDA.GetBillDetailsReportBySearchCriteria(FromDate, ToDate, costCenterId, categoryId, itemId, billNumber, cashierId, paymentType);

            rvTransaction.LocalReport.SetParameters(paramReport);
            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], billDetails));

            rvTransaction.LocalReport.DisplayName = "" + reportName + " Report";
            rvTransaction.LocalReport.Refresh();
            frmPrint.Attributes["src"] = "";
        }

        [WebMethod]
        public static List<InvItemBO> GetServiceByCriteria(int CategoryId)
        {
            InvItemDA productDA = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();
            productList = productDA.GetInvItemInfoByCategoryId(0, CategoryId);

            return productList;
        }
    }
}