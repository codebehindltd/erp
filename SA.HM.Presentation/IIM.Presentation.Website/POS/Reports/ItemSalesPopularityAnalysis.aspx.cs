using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data;
using System.IO;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Restaurant;

namespace HotelManagement.Presentation.Website.POS.Reports
{
    public partial class ItemSalesPopularityAnalysis : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int _IsReportPanelEnable = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCostCenter();
                LoadCategory();
                LoadItemClassificationInfo();
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
        private void LoadItemClassificationInfo()
        {
            InvItemClassificationCostCenterMappingDA commonDA = new InvItemClassificationCostCenterMappingDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<ItemClassificationBO> fields = new List<ItemClassificationBO>();
            fields = commonDA.GetActiveItemClassificationInfo();

            ddlClassification.DataSource = fields;
            ddlClassification.DataTextField = "ClassificationName";
            ddlClassification.DataValueField = "ClassificationId";
            ddlClassification.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlClassification.Items.Insert(0, item);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _IsReportPanelEnable = 1;

            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty;
            string endDate = string.Empty;
            DateTime currentDate = DateTime.Now;
            var reportType = ddlReportType.SelectedValue;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            if (reportType == "CategoryWise")
            {
                reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptItemSalesPopularity.rdlc");
            }
            else if (reportType == "ItemWise")
            {
                reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptItemSalesPopularityItemWise.rdlc");
            }
            

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            List<ItemSalesPopularityInfoBO> items = new List<ItemSalesPopularityInfoBO>();
            ItemSalesPopularityInfoDA itemDa = new ItemSalesPopularityInfoDA();
            List<ReportParameter> paramReport = new List<ReportParameter>();
           
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(currentDate);
                this.txtFromDate.Text = hmUtility.GetStringFromDateTime(currentDate);
            }
            else
            {
                startDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(currentDate);
                this.txtToDate.Text = hmUtility.GetStringFromDateTime(currentDate);
            }
            else
            {
                endDate = this.txtToDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            ToDate = ToDate.AddDays(1).AddSeconds(-1); 

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("CompanyProfile", hmUtility.GetHMCompanyProfile()));
            paramReport.Add(new ReportParameter("CompanyAddress", hmUtility.GetHMCompanyAddress()));

            paramReport.Add(new ReportParameter("FromDate", startDate));
            paramReport.Add(new ReportParameter("ToDate", endDate));

            string reportName = "Popularity Analysis";
            paramReport.Add(new ReportParameter("ReportName", reportName));

            int categoryId = Convert.ToInt32(ddlCategory.SelectedValue);
            int classificationId = Convert.ToInt32(ddlClassification.SelectedValue);
            int costCenterId = Convert.ToInt32(ddlCostCenter.SelectedValue);

           
            items = itemDa.GetInventoryItemInformationWitoutCostCenter(FromDate, ToDate, categoryId, classificationId, costCenterId);

            rvTransaction.LocalReport.SetParameters(paramReport);
            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], items));

            rvTransaction.LocalReport.DisplayName = "" + reportName + " Report";
            rvTransaction.LocalReport.Refresh();
            frmPrint.Attributes["src"] = "";
        }

    }
}