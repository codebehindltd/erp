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

namespace HotelManagement.Presentation.Website.Inventory.Reports
{
    public partial class frmReportItemWiseStock : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int _RoomStatusInfoByDate = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                companyProjectUserControl.ddlFirstValueVar = "all";
                LoadCommonDropDownHiddenField();
                LoadLocation();
                LoadCategory();
                LoadItem();
                IsAttributeItemShow();
                InventoryItemStockType();
                LoadItemClassificationInfo();
                LoadManufacturer();
            }
        }
        private void IsAttributeItemShow()
        {
            ColorSizeDiv.Visible = false;
            StyleDiv.Visible = false;
            hfIsItemAttributeEnable.Value = "0";
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
            homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsItemAttributeEnable", "IsItemAttributeEnable");

            if (homePageSetupBO.SetupValue == "1")
            {
                ColorSizeDiv.Visible = true;
                StyleDiv.Visible = true;
                hfIsItemAttributeEnable.Value = "1";
                LoadColor();
                LoadSize();
                LoadStyle();
            }
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
        }
        private void LoadManufacturer()
        {
            List<InvManufacturerBO> List = new List<InvManufacturerBO>();
            InvManufacturerDA da = new InvManufacturerDA();
            List = da.GetManufacturerInfo();
            ddlBrand.DataSource = List;
            ddlBrand.DataTextField = "Name";
            ddlBrand.DataValueField = "ManufacturerId";
            ddlBrand.DataBind();
            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlBrand.Items.Insert(0, item);
        }
        private void LoadItemClassificationInfo()
        {
            InvItemClassificationCostCenterMappingDA commonDA = new InvItemClassificationCostCenterMappingDA();
            List<ItemClassificationBO> fields = new List<ItemClassificationBO>();
            fields = commonDA.GetActiveItemClassificationInfo();


            ddlClassification.DataSource = fields;
            ddlClassification.DataTextField = "ClassificationName";
            ddlClassification.DataValueField = "ClassificationId";
            ddlClassification.DataBind();

            System.Web.UI.WebControls.ListItem itemS = new System.Web.UI.WebControls.ListItem();
            itemS.Value = "0";
            itemS.Text = hmUtility.GetDropDownFirstAllValue();
            ddlClassification.Items.Insert(0, itemS);
        }
        public void LoadLocation()
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvLocationInfo();

            ddlLocation.DataSource = location;
            ddlLocation.DataTextField = "Name";
            ddlLocation.DataValueField = "LocationId";
            ddlLocation.DataBind();

            System.Web.UI.WebControls.ListItem itemNodeId = new System.Web.UI.WebControls.ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstAllValue();
            ddlLocation.Items.Insert(0, itemNodeId);
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();

            List<InvCategoryBO> productList = new List<InvCategoryBO>();
            productList = da.GetAllInvItemCatagoryInfoByServiceType("Product");
            List.AddRange(productList);

            List<InvCategoryBO> FixedAssetList = new List<InvCategoryBO>();
            FixedAssetList = da.GetAllInvItemCatagoryInfoByServiceType("FixedAsset");
            List.AddRange(FixedAssetList);

            ddlCategory.DataSource = List;
            ddlCategory.DataTextField = "MatrixInfo";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();
            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCategory.Items.Insert(0, item);
        }

        private void LoadItem()
        {
            List<InvItemBO> productList = new List<InvItemBO>();
            InvItemDA da = new InvItemDA();
            productList = da.GetInvItemInfo();
            ddlItem.DataSource = productList;
            ddlItem.DataTextField = "Name";
            ddlItem.DataValueField = "ItemId";
            ddlItem.DataBind();
            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlItem.Items.Insert(0, item);
        }
        private void LoadColor()
        {
            List<InvItemAttributeBO> attributeListBO = new List<InvItemAttributeBO>();
            InvItemDA da = new InvItemDA();
            attributeListBO = da.GetInvItemAttributeInfo().Where(x => x.SetupType == "Color").ToList();

            ddlColor.DataSource = attributeListBO;
            ddlColor.DataTextField = "Name";
            ddlColor.DataValueField = "Id";
            ddlColor.DataBind();
            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlColor.Items.Insert(0, item);
        }
        private void LoadSize()
        {
            List<InvItemAttributeBO> attributeListBO = new List<InvItemAttributeBO>();
            InvItemDA da = new InvItemDA();
            attributeListBO = da.GetInvItemAttributeInfo().Where(x => x.SetupType == "Size").ToList();
            ddlSize.DataSource = attributeListBO;
            ddlSize.DataTextField = "Name";
            ddlSize.DataValueField = "Id";
            ddlSize.DataBind();
            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlSize.Items.Insert(0, item);
        }
        private void LoadStyle()
        {
            List<InvItemAttributeBO> attributeListBO = new List<InvItemAttributeBO>();
            InvItemDA da = new InvItemDA();
            attributeListBO = da.GetInvItemAttributeInfo().Where(x => x.SetupType == "Style").ToList();
            ddlStyle.DataSource = attributeListBO;
            ddlStyle.DataTextField = "Name";
            ddlStyle.DataValueField = "Id";
            ddlStyle.DataBind();
            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlStyle.Items.Insert(0, item);
        }
        private void InventoryItemStockType()
        {
            HMCommonDA commonDA = new HMCommonDA();

            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();

            fields = commonDA.GetCustomField("InventoryItemStockType");

            if (fields != null)
            {
                ddlStockType.DataSource = fields.Where(x => x.FieldValue != "IngredientText").ToList();
                ddlStockType.DataTextField = "Description";
                ddlStockType.DataValueField = "FieldValue";
                ddlStockType.DataBind();

                System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
                item.Value = "0";
                item.Text = hmUtility.GetDropDownFirstAllValue();
                ddlStockType.Items.Insert(0, item);
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            _RoomStatusInfoByDate = 1;
            HMCommonDA hmCommonDA = new HMCommonDA();

            int costCenterId = 0, locationId = 0, categoryId = 0, manufacturerId = 0, classificationId = 0;
            int itemId = 0, colorId = 0, sizeId = 0, styleId = 0;
            string model = string.Empty;
            string reportType = ddlReportType.SelectedValue;
            string stockType = ddlStockType.SelectedValue;

            int companyId = 0;
            string companyName = "All";
            int projectId = 0;
            string projectName = "All";

            if (hfCompanyId.Value != "0" && hfCompanyId.Value != "")
            {
                companyId = Convert.ToInt32(hfCompanyId.Value);
                companyName = hfCompanyName.Value;
            }

            if (hfProjectId.Value != "0" && hfProjectId.Value != "")
            {
                projectId = Convert.ToInt32(hfProjectId.Value);
                projectName = hfProjectName.Value;
            }

            if (ddlLocation.SelectedValue != "")
                locationId = Convert.ToInt32(ddlLocation.SelectedValue);

            if (ddlCategory.SelectedValue != "")
                categoryId = Convert.ToInt32(ddlCategory.SelectedValue);
            manufacturerId = Convert.ToInt32(ddlBrand.SelectedValue);
            classificationId = Convert.ToInt32(ddlClassification.SelectedValue);
            model = txtModel.Text;
            itemId = !string.IsNullOrWhiteSpace(txtHiddenItemId.Value) ? Convert.ToInt32(txtHiddenItemId.Value) : 0;

            if (hfIsItemAttributeEnable.Value == "1")
            {
                colorId = Convert.ToInt32(ddlColor.SelectedValue);
                sizeId = Convert.ToInt32(ddlSize.SelectedValue);
                styleId = Convert.ToInt32(ddlStyle.SelectedValue);
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            if (ddlSerial.SelectedValue == "WithoutSerial")
            {
                if (hfIsItemAttributeEnable.Value == "1")
                {
                    if (reportType == "ItemWiseStock")
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptItemWiseStockInfoWithOutSerialWithAttribute.rdlc");
                    else if (reportType == "CategoryWiseStock")
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptCategoryWiseStockInfoWithOutSerialWithAttribute.rdlc");
                    else if (reportType == "CostcenterWiseStock")
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptCostcenterWiseStockInf.rdlc");
                    else if (reportType == "LocationWiseStock")
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptLocationWiseStockInfoWithOutSerialWithAttribute.rdlc");
                    else if (reportType == "ProjectWiseStock")
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProjectWiseStockInfoWithOutSerialWithAttribute.rdlc");
                }
                else
                {
                    if (reportType == "ItemWiseStock")
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptItemWiseStockInfoWithOutSerial.rdlc");
                    else if (reportType == "CategoryWiseStock")
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptCategoryWiseStockInfoWithOutSerial.rdlc");
                    else if (reportType == "CostcenterWiseStock")
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptCostcenterWiseStockInf.rdlc");
                    else if (reportType == "LocationWiseStock")
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptLocationWiseStockInfoWithOutSerial.rdlc");
                    else if (reportType == "ProjectWiseStock")
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProjectWiseStockInfoWithOutSerial.rdlc");
                }
            }
            else if (ddlSerial.SelectedValue == "WithSerial")
            {
                if (hfIsItemAttributeEnable.Value == "1")
                {
                    if (reportType == "ItemWiseStock")
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptItemWiseStockInfWithAttribute.rdlc");
                    else if (reportType == "CategoryWiseStock")
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptCategoryWiseStockInfWithAttribute.rdlc");
                    else if (reportType == "CostcenterWiseStock")
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptCostcenterWiseStockInf.rdlc");
                    else if (reportType == "LocationWiseStock")
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptLocationWiseStockInfWithAttribute.rdlc");
                    else if (reportType == "ProjectWiseStock")
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProjectWiseStockInfWithAttribute.rdlc");
                }
                else
                {
                    if (reportType == "ItemWiseStock")
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptItemWiseStockInf.rdlc");
                    else if (reportType == "CategoryWiseStock")
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptCategoryWiseStockInf.rdlc");
                    else if (reportType == "CostcenterWiseStock")
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptCostcenterWiseStockInf.rdlc");
                    else if (reportType == "LocationWiseStock")
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptLocationWiseStockInf.rdlc");
                    else if (reportType == "ProjectWiseStock")
                        reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProjectWiseStockInf.rdlc");
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

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            string isStockSummaryEnableInStockReport = "0";
            HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
            homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsStockSummaryEnableInStockReport", "IsStockSummaryEnableInStockReport");
            if (homePageSetupBO != null)
            {
                if (homePageSetupBO.SetupId > 0)
                {
                    if (homePageSetupBO.SetupValue == "1")
                    {
                        isStockSummaryEnableInStockReport = "1";
                    }
                }
            }

            string isInventoryReportItemCostRescitionForNonAdminUsers = "0";
            if (!userInformationBO.IsAdminUser)
            {
                HMCommonSetupBO setupReportItemCostRescitionBO = new HMCommonSetupBO();
                setupReportItemCostRescitionBO = commonSetupDA.GetCommonConfigurationInfo("IsInventoryReportItemCostRescitionForNonAdminUsers", "IsInventoryReportItemCostRescitionForNonAdminUsers");
                if (setupReportItemCostRescitionBO != null)
                {
                    if (setupReportItemCostRescitionBO.SetupId > 0)
                    {
                        if (setupReportItemCostRescitionBO.SetupValue == "1")
                        {
                            isInventoryReportItemCostRescitionForNonAdminUsers = "1";
                        }
                    }
                }
            }

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("IsStockSummaryEnableInStockReport", isStockSummaryEnableInStockReport));
            paramReport.Add(new ReportParameter("IsInventoryReportItemCostRescitionForNonAdminUsers", isInventoryReportItemCostRescitionForNonAdminUsers));
            paramReport.Add(new ReportParameter("CompanyAndProjectInformation", "Company: " + companyName + " and Project: " + projectName));
            rvTransaction.LocalReport.SetParameters(paramReport);

            string showTransaction = ddlShowTransaction.SelectedValue;            

            InvItemDA itemWiseStockDA = new InvItemDA();
            List<ItemWiseStockReportViewBO> itemWiseStockBO = new List<ItemWiseStockReportViewBO>();
            itemWiseStockBO = itemWiseStockDA.GetItemWiseStockInfoForReport(reportType, costCenterId, locationId, categoryId, itemId, colorId, sizeId, styleId, stockType, showTransaction, manufacturerId, classificationId, model, companyId, projectId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], itemWiseStockBO));

            if (reportType == "ItemWiseStock")
                rvTransaction.LocalReport.DisplayName = "Item Wise Stock";
            else if (reportType == "CategoryWiseStock")
                rvTransaction.LocalReport.DisplayName = "Category Wise Stock";
            else if (reportType == "CostcenterWiseStock")
                rvTransaction.LocalReport.DisplayName = "Cost Center Wise Stock";
            else if (reportType == "LocationWiseStock")
                rvTransaction.LocalReport.DisplayName = "Location Wise Stock";
            else if (reportType == "ProjectWiseStock")
                rvTransaction.LocalReport.DisplayName = "Project Wise Stock";

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

        [WebMethod]
        public static ArrayList PopulateProducts(int CategoryId)
        {
            ArrayList list = new ArrayList();
            InvItemDA productDA = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();
            productList = productDA.GetInventoryItemInformationByCategory(0, CategoryId, true, true);
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
        [WebMethod]
        public static List<InvItemBO> GetServiceByCriteria(int CategoryId)
        {
            InvItemDA productDA = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();
            productList = productDA.GetInvItemInfoByCategory(0, CategoryId);
            return productList;
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int CategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
            ArrayList list = new ArrayList();
            InvItemDA productDA = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();
            productList = productDA.GetInvItemInfoByCategory(0, CategoryId);

            ddlItem.DataSource = productList;
            ddlItem.DataTextField = "Name";
            ddlItem.DataValueField = "ItemId";
            ddlItem.DataBind();
            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlItem.Items.Insert(0, item);
        }
    }
}