using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.PurchaseManagment;
using iTextSharp.text.pdf;
using System.Web.Services;
using HotelManagement.Entity.SalesManagment;

namespace HotelManagement.Presentation.Website.Inventory.Reports
{
    public partial class frmReportProductOutInfo : BasePage
    {
        protected int isMessageBoxEnable = -1;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //this.LoadProduct();
                LoadCostCenter();
                LoadCategory();
                LoadCommonDropDownHiddenField();
            }
        }
        //private void LoadProduct()
        //{
        //    InvItemDA productDA = new InvItemDA();
        //    this.ddlProductId.DataSource = productDA.GetInvItemInfo();
        //    this.ddlProductId.DataTextField = "Name";
        //    this.ddlProductId.DataValueField = "ItemId";
        //    this.ddlProductId.DataBind();

        //    System.Web.UI.WebControls.ListItem itemProduct = new System.Web.UI.WebControls.ListItem();
        //    itemProduct.Value = "0";
        //    itemProduct.Text = "---All---";
        //    this.ddlProductId.Items.Insert(0, itemProduct);

        //}
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;

            string reportType = ddlReportType.SelectedValue;
            string reportFormat = ddlReportFormat.SelectedValue;

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

            int costCenterIdFrom = !string.IsNullOrWhiteSpace(this.ddlCostCenterFrom.SelectedValue) ? Convert.ToInt32(this.ddlCostCenterFrom.SelectedValue) : 0;
            int costCenterIdTo = !string.IsNullOrWhiteSpace(this.ddlCostCenterTo.SelectedValue) ? Convert.ToInt32(this.ddlCostCenterTo.SelectedValue) : 0;
            int locationIdFrom = !string.IsNullOrWhiteSpace(hfLocationFromId.Value) ? Convert.ToInt32(hfLocationFromId.Value) : 0;
            int locationIdTo = !string.IsNullOrWhiteSpace(hfLocationToId.Value) ? Convert.ToInt32(hfLocationToId.Value) : 0;
            int productId = !string.IsNullOrWhiteSpace(hfItemId.Value) ? Convert.ToInt32(hfItemId.Value) : 0;
            int categoryId = !string.IsNullOrWhiteSpace(ddlCategory.SelectedValue) ? Convert.ToInt32(ddlCategory.SelectedValue) : 0;
            string productName = (productId == 0 ? "" : hfItemName.Value);
            hfLocationFromId.Value = locationIdFrom.ToString();
            hfLocationToId.Value = locationIdTo.ToString();

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            if (reportFormat == "Summary")
            {
                if (reportType == "CostCenterWise")
                {
                    if (productId > 0)
                    {
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductOutItemSummaryCostCenterWise.rdlc");
                    }
                    else
                    {
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductOutSummaryCostCenterWise.rdlc");
                    }                    
                }
                else if (reportType == "DateWise")
                {
                    if (productId > 0)
                    {
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductOutItemSummaryDateWise.rdlc");
                    }
                    else
                    {
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductOutSummaryDateWise.rdlc");
                    }                    
                }
                else if (reportType == "ItemWise")
                {
                    if (productId > 0)
                    {
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductOutItemSummaryItemWise.rdlc");
                    }
                    else
                    {
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductOutSummaryItemWise.rdlc");
                    }                    
                }
                else if (reportType == "CategoryWise")
                {
                    if (productId > 0)
                    {
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductOutItemSummaryCategoryWise.rdlc");
                    }
                    else
                    {
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductOutSummaryCategoryWise.rdlc");
                    }                    
                }
                else if (reportType == "TransferNumberWise")
                {
                    if (productId > 0)
                    {
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductOutItemSummaryPONumberWise.rdlc");
                    }
                    else
                    {
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductOutSummaryPONumberWise.rdlc");
                    }                    
                }
            }
            else if (reportFormat == "ItemWiseSummary")
            {
                if (reportType == "CostCenterWise")
                {
                    reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductOutInfoCostCenterWiseItemSummary.rdlc");

                }
                else if (reportType == "DateWise")
                {
                    reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductOutInfoDateWiseItemSummary.rdlc");
                }
                else if (reportType == "ItemWise")
                {
                    reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductOutInfoItemWiseItemSummary.rdlc");
                }
                else if (reportType == "CategoryWise")
                {
                    reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductOutInfoCategoryWiseItemSummary.rdlc");
                }
                else if (reportType == "TransferNumberWise")
                {
                    reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductOutInfoTransferNumberWiseItemSummary.rdlc");
                }
            }
            else if (reportFormat == "Details")
            {
                if (reportType == "CostCenterWise")
                {
                    reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductOutInfoCostCenterWise.rdlc");

                }
                else if (reportType == "DateWise")
                {
                    reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductOutInfo.rdlc");
                }
                else if (reportType == "ItemWise")
                {
                    reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductOutInfoItemWise.rdlc");
                }
                else if (reportType == "CategoryWise")
                {
                    reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductOutInfoCategoryWise.rdlc");
                }
                else if (reportType == "TransferNumberWise")
                {
                    reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductOutInfoPONumberWise.rdlc");
                }
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
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("ItemName", productName));
            paramReport.Add(new ReportParameter("FromDate", startDate));
            paramReport.Add(new ReportParameter("ToDate", endDate));
            rvTransaction.LocalReport.SetParameters(paramReport);

            PMProductReceivedDA prDA = new PMProductReceivedDA();
            List<PMProductReceivedReportViewBO> prBO = new List<PMProductReceivedReportViewBO>();
            prBO = prDA.GetProductOutInfo(FromDate, ToDate, categoryId, productId, costCenterIdFrom, locationIdFrom, costCenterIdTo, locationIdTo);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], prBO));

            rvTransaction.LocalReport.DisplayName = "Product Out";
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
        public void LoadCostCenter()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            var List = costCentreTabDA.GetCostCentreTabInfo();
            this.ddlCostCenterFrom.DataSource = List;
            this.ddlCostCenterFrom.DataTextField = "CostCenter";
            this.ddlCostCenterFrom.DataValueField = "CostCenterId";
            this.ddlCostCenterFrom.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCostCenterFrom.Items.Insert(0, item);

            ddlCostCenterTo.DataSource = List;
            ddlCostCenterTo.DataTextField = "CostCenter";
            ddlCostCenterTo.DataValueField = "CostCenterId";
            ddlCostCenterTo.DataBind();
            ddlCostCenterTo.Items.Insert(0, item);

            //this.ddlSearchCostCenterFrom.DataSource = List;
            //this.ddlSearchCostCenterFrom.DataTextField = "CostCenter";
            //this.ddlSearchCostCenterFrom.DataValueField = "CostCenterId";
            //this.ddlSearchCostCenterFrom.DataBind();
            //this.ddlSearchCostCenterFrom.Items.Insert(0, item);

            //this.ddlSearchCostCenterTo.DataSource = List;
            //this.ddlSearchCostCenterTo.DataTextField = "CostCenter";
            //this.ddlSearchCostCenterTo.DataValueField = "CostCenterId";
            //this.ddlSearchCostCenterTo.DataBind();
            //this.ddlSearchCostCenterTo.Items.Insert(0, item);
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetInvItemCatagoryInfoByServiceType("Product");
            ddlCategory.DataSource = List;
            ddlCategory.DataTextField = "MatrixInfo";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCategory.Items.Insert(0, item);
        }
        [WebMethod]
        public static List<InvLocationBO> InvLocationByCostCenter(int costCenterId)
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvItemLocationByCostCenter(costCenterId);

            return location;
        }

        [WebMethod]
        public static List<ItemViewBO> LoadProductByCategoryNCostcenterId(string costCenterId, string categoryId)
        {
            InvItemDA itemda = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();

            productList = itemda.GetInvItemInfoByCategory(Convert.ToInt32(costCenterId), Convert.ToInt32(categoryId));
            productList = productList.Where(p => p.StockType == "StockItem").ToList();
            List<ItemViewBO> itemViewList = new List<ItemViewBO>();

            itemViewList = (from s in productList
                            select new ItemViewBO
                            {
                                ItemId = s.ItemId,
                                ItemName = s.Name,
                                ProductType = s.ProductType

                            }).ToList();

            return itemViewList;
        }

    }
}