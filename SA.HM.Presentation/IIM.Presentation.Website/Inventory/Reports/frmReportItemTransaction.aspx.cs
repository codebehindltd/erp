using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using System.IO;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using System.Web.Services;

namespace HotelManagement.Presentation.Website.Inventory.Reports
{
    public partial class frmReportItemTransaction : BasePage
    {
        protected int _IsReportPanelEnable = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                LoadCostCenter();
                LoadCategory();
                LoadCommonDropDownHiddenField();
                LoadLocation();
            }
        }
        private void LoadCommonDropDownHiddenField()
        {
            //CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetAllInvItemCatagoryInfoByServiceType("Product");
            ddlCategory.DataSource = List;
            ddlCategory.DataTextField = "MatrixInfo";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCategory.Items.Insert(0, item);
        }
        private void LoadLocation()
        {
            InvLocationDA productCategoryDA = new InvLocationDA();
            ddlLocation.DataSource = productCategoryDA.GetInvLocationInfo();
            ddlLocation.DataTextField = "Name";
            ddlLocation.DataValueField = "LocationId";
            ddlLocation.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstAllValue();
            ddlLocation.Items.Insert(0, itemNodeId);
        }
        public void LoadCostCenter()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            var List = costCentreTabDA.GetCostCentreTabInfo();
            ddlCostCenter.DataSource = List;
            ddlCostCenter.DataTextField = "CostCenter";
            ddlCostCenter.DataValueField = "CostCenterId";
            ddlCostCenter.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCostCenter.Items.Insert(0, item);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _IsReportPanelEnable = 1;
            DateTime dateTime = DateTime.Now;
            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty, endDate = string.Empty,
                  reportType = string.Empty, reportName = "Item Transaction";

            reportType = ddlReportType.SelectedValue;

            if (string.IsNullOrWhiteSpace(txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(dateTime);
                txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(txtToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(dateTime);
                txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = txtToDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            ToDate = ToDate.AddDays(1).AddSeconds(-1);

            DateTime ReportDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            int categoryId = 0, itemId = 0, locationId = 0;

            if (ddlCategory.SelectedValue != string.Empty)
                categoryId = Convert.ToInt32(ddlCategory.SelectedValue);

            if (ddlItem.SelectedValue != string.Empty)
                itemId = Convert.ToInt32(ddlItem.SelectedValue);

            if (ddlLocation.SelectedValue != string.Empty)
                locationId = Convert.ToInt32(ddlLocation.SelectedValue);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";

            if (reportType == "Summary")
            {
                reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptTransactionSummaryReport.rdlc");
            }
            else if (reportType == "Details")
            {
                reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptTransactionDetailsReport.rdlc");
            }

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> paramReport = new List<ReportParameter>();

            if (files[0].CompanyId > 0)
            {
                paramReport.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                paramReport.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    paramReport.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }
                else
                {
                    paramReport.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                }
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("FromDate", startDate));
            paramReport.Add(new ReportParameter("ToDate", endDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("ReportName", reportName));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));

            string reportDateDuration = "Date From: " + txtFromDate.Text + " To: " + txtToDate.Text;

            paramReport.Add(new ReportParameter("ReportDateDuration", reportDateDuration));

            rvTransaction.LocalReport.SetParameters(paramReport);

            List<ItemTransactionSummaryReportBO> itemTransactionSummary = new List<ItemTransactionSummaryReportBO>();
            List<ItemTransactionDetailsReportBO> itemTransactionDetails = new List<ItemTransactionDetailsReportBO>();


            InvItemDA itemDA = new InvItemDA();

            if (reportType == "Summary")
            {
                itemTransactionSummary = itemDA.GetItemTransactionSummaryReport(FromDate, ToDate, categoryId, itemId, locationId);
            }
            else if (reportType == "Details")
            {
                itemTransactionDetails = itemDA.GetItemTransactionDetailsReport(FromDate, ToDate, categoryId, itemId, locationId);

                foreach (ItemTransactionDetailsReportBO td in itemTransactionDetails) {
                    td.TransactionType = CommonHelper.SentenceCaseConvertionTolowerCase(td.TransactionType);
                }
            }
            
            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();

            if (reportType == "Summary")
            {
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], itemTransactionSummary));
            }
            else if (reportType == "Details")
            {
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], itemTransactionDetails));
            }

            rvTransaction.LocalReport.DisplayName = reportName;
            rvTransaction.LocalReport.Refresh();

            ddlItem.SelectedValue = itemId.ToString();

        }

        [WebMethod]
        public static List<InvItemBO> GetServiceByCriteria(int CategoryId)
        {
            InvItemDA productDA = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();
            productList = productDA.GetInvItemInfoByCategory(0, CategoryId);

            return productList;
        }

        protected void ddlCostCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            InvItemDA da = new InvItemDA();
            InvLocationDA locationDa = new InvLocationDA();
            List<InvCategoryBO> category = new List<InvCategoryBO>();
            List<InvItemBO> item = new List<InvItemBO>();
            List<InvLocationBO> location = new List<InvLocationBO>();

            int costCenterId = Convert.ToInt32(ddlCostCenter.SelectedValue);

            category = da.GetCategoryByCostcenter(costCenterId);
            item = da.GetItemByCostcenter(costCenterId);
            location = locationDa.GetInvItemLocationByCostCenter(costCenterId);

            ListItem lItem = new ListItem();
            lItem.Value = "0";
            lItem.Text = hmUtility.GetDropDownFirstAllValue();

            //category
            ddlCategory.DataSource = category;
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, lItem);

            //item
            ddlItem.DataSource = item;
            ddlItem.DataTextField = "Name";
            ddlItem.DataValueField = "ItemId";
            ddlItem.DataBind();
            ddlItem.DataBind();
            ddlItem.Items.Insert(0, lItem);

            //location
            ddlLocation.DataSource = location;
            ddlLocation.DataTextField = "Name";
            ddlLocation.DataValueField = "LocationId";
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, lItem);
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            InvItemDA productDA = new InvItemDA();
            List<InvItemBO> item = new List<InvItemBO>();

            int costCenterId = Convert.ToInt32(ddlCostCenter.SelectedValue);
            int categoryId = Convert.ToInt32(ddlCategory.SelectedValue);

            item = productDA.GetItemByCategoryNCostcenter(costCenterId, categoryId);

            ListItem lItem = new ListItem();
            lItem.Value = "0";
            lItem.Text = hmUtility.GetDropDownFirstAllValue();

            //item
            ddlItem.DataSource = item;
            ddlItem.DataTextField = "Name";
            ddlItem.DataValueField = "ItemId";
            ddlItem.DataBind();
            ddlItem.DataBind();
            ddlItem.Items.Insert(0, lItem);
        }
    }
}