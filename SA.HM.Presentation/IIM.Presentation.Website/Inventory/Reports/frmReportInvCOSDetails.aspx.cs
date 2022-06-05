using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.UserInformation;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HotelManagement.Presentation.Website.Inventory.Reports
{
    public partial class frmReportInvCOSDetails : BasePage
    {
        protected int _ReportShow = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCostCenter();
                LoadItem();
                LoadCategory();
            }
        }

        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            _ReportShow = 1;
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            if (string.IsNullOrWhiteSpace(txtStartDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtStartDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtStartDate.Text;
            }
            if (string.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtEndDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = txtEndDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);
            int? categoryId = 0, itemId = 0, costCenterId = 0;
            if (!string.IsNullOrWhiteSpace(ddlCategory.SelectedValue))
            {
                categoryId = Convert.ToInt32(ddlCategory.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(ddlItem.SelectedValue))
            {
                itemId = Convert.ToInt32(ddlItem.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(ddlCostCenter.SelectedValue))
            {
                costCenterId = Convert.ToInt32(ddlCostCenter.SelectedValue);
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptInventoryCOSDetail.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            List<ReportParameter> reportParam = new List<ReportParameter>();

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
            if (files[0].CompanyId > 0)
            {
                reportParam.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }
                else
                {
                    reportParam.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                }
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            string reportName = "Inventory Cost of Sales Detail";

            reportParam.Add(new ReportParameter("ReportName", reportName));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            reportParam.Add(new ReportParameter("ReportDateFrom", startDate));
            reportParam.Add(new ReportParameter("ReportDateTo", endDate));

            rvTransaction.LocalReport.SetParameters(reportParam);

            AllInventoryReportDA allInventoryReportDA = new AllInventoryReportDA();
            List<InventoryCOSDetailViewBO> invCostofSalesList = new List<InventoryCOSDetailViewBO>();
            invCostofSalesList = allInventoryReportDA.GetInventoryCOSDetail(FromDate, ToDate, categoryId, itemId, costCenterId);

            if (invCostofSalesList.Count == 0)
            {
                invCostofSalesList = new List<InventoryCOSDetailViewBO>();
            }

            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], invCostofSalesList));

            rvTransaction.LocalReport.DisplayName = reportName;
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Portrait.ToString());

            frmPrint.Attributes["src"] = reportSource;             
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
            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCategory.Items.Insert(0, item);
        }
        private void LoadItem()
        {
            List<InvItemBO> List = new List<InvItemBO>();
            InvItemDA productDA = new InvItemDA();
            List = productDA.GetInvItemInfo();
            ddlItem.DataSource = List;
            ddlItem.DataTextField = "Name";
            ddlItem.DataValueField = "ItemId";
            ddlItem.DataBind();
            System.Web.UI.WebControls.ListItem itemNodeId = new System.Web.UI.WebControls.ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = "---ALL---";
            this.ddlItem.Items.Insert(0, itemNodeId);
        }
        private void LoadCostCenter()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> List = costCentreTabDA.GetCostCentreTabInfo();

            this.ddlCostCenter.DataSource = List;
            this.ddlCostCenter.DataTextField = "CostCenter";
            this.ddlCostCenter.DataValueField = "CostCenterId";
            this.ddlCostCenter.DataBind();

            System.Web.UI.WebControls.ListItem itemNodeId = new System.Web.UI.WebControls.ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCostCenter.Items.Insert(0, itemNodeId);
        }
    }
}