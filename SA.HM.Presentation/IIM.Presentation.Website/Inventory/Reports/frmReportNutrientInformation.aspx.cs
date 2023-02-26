using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Collections;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.UserInformation;
using iTextSharp.text;
using iTextSharp.text.pdf;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.PurchaseManagment;

namespace HotelManagement.Presentation.Website.Inventory.Reports
{
    public partial class frmReportNutrientInformation : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int _IsReportPanelEnable = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadCommonDropDownHiddenField();
                LoadProduct();
                LoadCategory();
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            _IsReportPanelEnable = 1;
            HMCommonDA hmCommonDA = new HMCommonDA();

            int categoryId = 0, itemId = 0;

            if (ddlCategory.SelectedValue != "")
                categoryId = Convert.ToInt32(ddlCategory.SelectedValue);

            var reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptItemNutrientInformation.rdlc");

            if (ddlReportType.SelectedValue == "SummaryReport")
            {
                reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptItemNutrientSummary.rdlc");
                if (ddlProductSummaryReport.SelectedValue != "")
                {
                    itemId = Convert.ToInt32(ddlProductSummaryReport.SelectedValue);
                }
            }
            else
            {
                if (!IsFrmValid())
                {
                    return;
                }
                reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptItemNutrientInformation.rdlc");
                if (ddlProductDetailReport.SelectedValue != "")
                {
                    itemId = Convert.ToInt32(ddlProductDetailReport.SelectedValue);
                }
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

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

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();

            if (ddlReportType.SelectedValue == "SummaryReport")
            {
                InvItemDA itemWiseStockDA = new InvItemDA();
                List<RecipeReportBO> FinishedGoodsNutrientSummaryInformationListBO = new List<RecipeReportBO>();
                FinishedGoodsNutrientSummaryInformationListBO = itemWiseStockDA.GetFinishedGoodsNutrientSummaryInformation(itemId);
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], FinishedGoodsNutrientSummaryInformationListBO));
            }
            else
            {
                InvItemDA itemWiseStockDA = new InvItemDA();
                List<RecipeReportBO> itemWiseStockBO = new List<RecipeReportBO>();
                itemWiseStockBO = itemWiseStockDA.GetItemRecipeReport(itemId);
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], itemWiseStockBO));

                List<RecipeReportBO> itemNutrientInformationBO = new List<RecipeReportBO>();
                itemNutrientInformationBO = itemWiseStockDA.GetItemNutrientInformationReport(itemId);
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[1], itemNutrientInformationBO));

                PMFinishProductDA goodsDA = new PMFinishProductDA();
                List<OverheadExpensesBO> nutrientOverheadExpensesBO = new List<OverheadExpensesBO>();
                nutrientOverheadExpensesBO = goodsDA.GetInvNutrientOEDetailsById(itemId);
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[2], nutrientOverheadExpensesBO));

                decimal totalRMCost = 0;
                decimal totalOHCost = 0;
                decimal calculatedTotalCost = 0;
                decimal finishedGoodsSalesRate = 0;
                decimal calculatedProfitOrLoss = 0;

                foreach (RecipeReportBO rmCost in itemWiseStockBO)
                {
                    totalRMCost = totalRMCost + (rmCost.ItemUnit * rmCost.ItemCost);
                }

                foreach (OverheadExpensesBO ohCost in nutrientOverheadExpensesBO)
                {
                    totalOHCost = totalOHCost + ohCost.OEAmount;
                }

                calculatedTotalCost = totalRMCost + totalOHCost;

                int costCenterId = 0;

                CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
                List<CostCentreTabBO> allCostCentreTabListBO = new List<CostCentreTabBO>();
                allCostCentreTabListBO = costCentreTabDA.GetCostCentreTabInfoByType("Restaurant,RetailPos,OtherOutlet,Billing");
                if (allCostCentreTabListBO != null)
                {
                    if (allCostCentreTabListBO.Count > 0)
                    {
                        costCenterId = allCostCentreTabListBO[0].CostCenterId;
                    }
                }

                InvItemBO itemEntityBO = new InvItemBO();
                InvItemDA itemEntityDA = new InvItemDA();
                itemEntityBO = itemEntityDA.GetInvItemInfoById(costCenterId, itemId);
                if (itemEntityBO != null)
                {
                    if (itemEntityBO.ItemId > 0)
                    {
                        finishedGoodsSalesRate = itemEntityBO.UnitPriceLocal;
                    }
                }

                calculatedProfitOrLoss = finishedGoodsSalesRate - calculatedTotalCost;

                paramReport.Add(new ReportParameter("TotalRMCost", totalRMCost.ToString("0.00")));
                paramReport.Add(new ReportParameter("TotalOHCost", totalOHCost.ToString("0.00")));
                paramReport.Add(new ReportParameter("CalculatedTotalCost", calculatedTotalCost.ToString("0.00")));
                paramReport.Add(new ReportParameter("FinishedGoodsSalesRate", finishedGoodsSalesRate.ToString("0.00")));
                paramReport.Add(new ReportParameter("CalculatedProfitOrLoss", calculatedProfitOrLoss.ToString("0.00")));

            }

            rvTransaction.LocalReport.SetParameters(paramReport);
            rvTransaction.LocalReport.DisplayName = "Nutrient Information";
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
        //************************ User Defined Function ********************//
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
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
        private void LoadProduct()
        {
            List<InvItemBO> List = new List<InvItemBO>();
            InvItemDA productDA = new InvItemDA();
            List = productDA.GetInvItemInfo(true);

            ddlProductSummaryReport.DataSource = List;
            ddlProductSummaryReport.DataTextField = "CodeAndName";
            ddlProductSummaryReport.DataValueField = "ItemId";
            ddlProductSummaryReport.DataBind();
            System.Web.UI.WebControls.ListItem itemNodeIdSummaryReport = new System.Web.UI.WebControls.ListItem();
            itemNodeIdSummaryReport.Value = "0";
            itemNodeIdSummaryReport.Text = "--- ALL ---";
            this.ddlProductSummaryReport.Items.Insert(0, itemNodeIdSummaryReport);


            ddlProductDetailReport.DataSource = List;
            ddlProductDetailReport.DataTextField = "CodeAndName";
            ddlProductDetailReport.DataValueField = "ItemId";
            ddlProductDetailReport.DataBind();
            System.Web.UI.WebControls.ListItem itemNodeIdDetailReport = new System.Web.UI.WebControls.ListItem();
            itemNodeIdDetailReport.Value = "0";
            itemNodeIdDetailReport.Text = "--- Please Select ---";
            this.ddlProductDetailReport.Items.Insert(0, itemNodeIdDetailReport);
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            if (ddlReportType.SelectedValue == "DetailReport")
            {
                if (ddlProductDetailReport.SelectedValue == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Item Name.", AlertType.Warning);
                    flag = false;
                    _IsReportPanelEnable = -1;
                    ddlProductDetailReport.Focus();
                }
            }
            return flag;
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static ArrayList PopulateProducts(int CategoryId)
        {

            ArrayList list = new ArrayList();
            InvItemDA productDA = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();
            productList = productDA.GetInvItemInfoByCategoryId(0, CategoryId);
            int count = productList.Count;
            for (int i = 0; i < count; i++)
            {
                list.Add(new System.Web.UI.WebControls.ListItem(
                                        productList[i].Name.ToString(),
                                        productList[i].ItemId.ToString()
                                         ));
            }
            return list;

        }
        [WebMethod]
        public static ArrayList PopulateCategories(int CostCenterId)
        {

            ArrayList list = new ArrayList();
            InvCategoryDA categoryDA = new InvCategoryDA();
            List<InvCategoryBO> categoryList = new List<InvCategoryBO>();
            categoryList = categoryDA.GetInvItemCatagoryByCostCenter("Product", CostCenterId);
            int count = categoryList.Count;
            for (int i = 0; i < count; i++)
            {
                list.Add(new System.Web.UI.WebControls.ListItem(
                                        categoryList[i].MatrixInfo.ToString(),
                                        categoryList[i].CategoryId.ToString()
                                         ));
            }
            return list;

        }
    }
}