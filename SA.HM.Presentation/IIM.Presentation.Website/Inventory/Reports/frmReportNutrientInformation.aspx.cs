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
        HMUtility hmUtility = new HMUtility();
        protected int _RoomStatusInfoByDate = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCommonDropDownHiddenField();
                LoadProduct();
                LoadCategory();
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            _RoomStatusInfoByDate = 1;
            HMCommonDA hmCommonDA = new HMCommonDA();

            int categoryId = 0, itemId = 0;

            if (ddlCategory.SelectedValue != "")
                categoryId = Convert.ToInt32(ddlCategory.SelectedValue);

            if (ddlProduct.SelectedValue != "")
                itemId = Convert.ToInt32(ddlProduct.SelectedValue);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptItemNutrientInformation.rdlc");

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
            rvTransaction.LocalReport.SetParameters(paramReport);
            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                    
            //long maxRow = 0;
            //double mid = 0;
            //if (itemWiseStockBO != null)
            //{
            //    if (itemWiseStockBO.Count > 0)
            //    {
            //        maxRow = itemWiseStockBO.Max(m => m.ItemRank);
            //        mid = Math.Round(Convert.ToDouble(maxRow) / 2);
            //        itemWiseStockBO2 = itemWiseStockBO.Where(w => w.ItemRank > mid && w.ItemRank <= maxRow).ToList();
            //        itemWiseStockBO = itemWiseStockBO.Where(w => w.ItemRank >= 1 && w.ItemRank <= mid).ToList();
            //    }
            //}

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
            ddlProduct.DataSource = List;
            ddlProduct.DataTextField = "CodeAndName";
            ddlProduct.DataValueField = "ItemId";
            ddlProduct.DataBind();
            System.Web.UI.WebControls.ListItem itemNodeId = new System.Web.UI.WebControls.ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = "---ALL---";
            this.ddlProduct.Items.Insert(0, itemNodeId);
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