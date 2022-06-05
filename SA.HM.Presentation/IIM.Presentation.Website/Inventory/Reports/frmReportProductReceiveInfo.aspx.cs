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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.Services;
using System.Collections;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.GeneralLedger;

namespace HotelManagement.Presentation.Website.Inventory.Reports
{
    public partial class frmReportProductReceiveInfo : BasePage
    {
        protected int isMessageBoxEnable = -1;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //LoadProduct();
                LoadCategory();
                LoadSupplierInfo();
                LoadCommonDropDownHiddenField();
                LoadItem();
                //LoadCurrentDate();
            }
        }
        //private void LoadProduct()
        //{
        //    InvItemDA productDA = new InvItemDA();
        //    ddlProductId.DataSource = productDA.GetInvItemInfo();
        //    ddlProductId.DataTextField = "Name";
        //    ddlProductId.DataValueField = "ItemId";
        //    ddlProductId.DataBind();

        //    System.Web.UI.WebControls.ListItem itemProduct = new System.Web.UI.WebControls.ListItem();
        //    itemProduct.Value = "0";
        //    itemProduct.Text = hmUtility.GetDropDownFirstAllValue();
        //    ddlProductId.Items.Insert(0, itemProduct);
        //}
        private void LoadCurrentDate()
        {
            txtStartDate.Text = DateTime.Now.ToString();
            txtEndDate.Text = DateTime.Now.ToString();
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetAllActiveInvItemCatagoryInfoByServiceType("All");
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
            ddlProductId.DataSource = productList;
            ddlProductId.DataTextField = "Name";
            ddlProductId.DataValueField = "ItemId";
            ddlProductId.DataBind();
            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlProductId.Items.Insert(0, item);
        }

        private void LoadSupplierInfo()
        {
            PMSupplierDA entityDA = new PMSupplierDA();
            ddlSupplier.DataSource = entityDA.GetPMSupplierInfo();
            ddlSupplier.DataTextField = "Name";
            ddlSupplier.DataValueField = "SupplierId";
            ddlSupplier.DataBind();

            System.Web.UI.WebControls.ListItem item2 = new System.Web.UI.WebControls.ListItem();
            item2.Value = "-1";
            item2.Text = hmUtility.GetDropDownFirstAllValue();
            ddlSupplier.Items.Insert(0, item2);
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            string reportType = string.Empty, startDate = string.Empty, endDate = string.Empty, receiveNumber = string.Empty,
                   referenceNumber = string.Empty, poNumber = string.Empty;

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

            DateTime dateTime = DateTime.Now;
            int categoryId = 0, productId = 0, supplierId = 0;
            DateTime? FromDate = dateTime, ToDate = dateTime;

            //ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            if (!string.IsNullOrWhiteSpace(txtStartDate.Text))
            {
                FromDate = hmUtility.GetDateTimeFromString(txtStartDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                txtStartDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }

            if (!string.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                ToDate = hmUtility.GetDateTimeFromString(txtEndDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);
            }
            else
            {
                txtEndDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }

            categoryId = Convert.ToInt32(ddlCategory.SelectedValue);
            //productId = Convert.ToInt32(ddlProductId.SelectedValue);
            productId = !string.IsNullOrWhiteSpace(txtHiddenItemId.Value) ? Convert.ToInt32(txtHiddenItemId.Value) : 0;
            supplierId = Convert.ToInt32(ddlSupplier.SelectedValue);
            receiveNumber = txtReceiveNumber.Text.Trim();
            referenceNumber = string.Empty;
            poNumber = txtPONumber.Text.Trim();

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

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

            var reportPath = "";

            if (ddlReportType.SelectedValue == "DateRange")
            {
                if (ddlDisplayType.SelectedValue == "Summary")
                {
                    reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductReceiveSummaryInfo.rdlc");
                }
                else
                {
                    reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductReceiveDetailsInfo.rdlc");
                }
            }
            else if (ddlReportType.SelectedValue == "Supplier")
            {
                if (ddlDisplayType.SelectedValue == "Summary")
                {
                    reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductReceiveSummaryInfoBySupplier.rdlc");
                }
                else
                {
                    reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductReceiveDetailsInfoBySupplier.rdlc");
                }
            }
            else if (ddlReportType.SelectedValue == "Category")
            {
                if (ddlDisplayType.SelectedValue == "Summary")
                {
                    reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductReceiveSummaryInfoByCategory.rdlc");
                }
                else
                {
                    reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductReceiveDetailsInfoByCategory.rdlc");
                }
            }

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;
            List<ReportParameter> paramReport = new List<ReportParameter>();
            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            //string companyName = string.Empty;
            string companyAddress = string.Empty;
            string webAddress = string.Empty;

            //-- Company Logo -------------------------------
            //HMCommonDA hmCommonDA = new HMCommonDA();
            string imageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            if (files[0].CompanyId > 0)
            {
                companyName = files[0].CompanyName;
                companyAddress = files[0].CompanyAddress;
                //paramReport.Add(new ReportParameter("CompanyProfile", fileCompany[0].Name));
                //paramReport.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    //paramReport.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                    webAddress = files[0].WebAddress;
                }
                else
                {
                    //paramReport.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                    webAddress = files[0].ContactNumber;
                }
            }

            //int glCompanyId = Convert.ToInt32(ddlGLCompanyId.SelectedValue);
            if (companyId > 0)
            {
                GLCompanyBO glCompanyBO = new GLCompanyBO();
                GLCompanyDA glCompanyDA = new GLCompanyDA();
                glCompanyBO = glCompanyDA.GetGLCompanyInfoById(companyId);
                if (glCompanyBO != null)
                {
                    if (glCompanyBO.CompanyId > 0)
                    {
                        companyName = glCompanyBO.Name;
                        if (!string.IsNullOrWhiteSpace(glCompanyBO.CompanyAddress))
                        {
                            companyAddress = glCompanyBO.CompanyAddress;
                        }
                        if (!string.IsNullOrWhiteSpace(glCompanyBO.WebAddress))
                        {
                            webAddress = glCompanyBO.WebAddress;
                        }
                        if (!string.IsNullOrWhiteSpace(glCompanyBO.ImageName))
                        {
                            imageName = glCompanyBO.ImageName;
                        }
                    }
                }
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            //string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            //rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("CompanyProfile", companyName));
            paramReport.Add(new ReportParameter("CompanyAddress", companyAddress));
            paramReport.Add(new ReportParameter("CompanyWeb", webAddress));
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("DisplayTypeInfo", ddlDisplayType.SelectedValue));
            if (productId > 0)
            {
                isStockSummaryEnableInStockReport = "1";
            }

            paramReport.Add(new ReportParameter("IsStockSummaryEnableInStockReport", isStockSummaryEnableInStockReport));
            rvTransaction.LocalReport.SetParameters(paramReport);

            PMProductReceivedDA prDA = new PMProductReceivedDA();
            List<PMProductReceivedReportBO> prBO = new List<PMProductReceivedReportBO>();
            prBO = prDA.GetProductreceiveInfoForReport(companyId, projectId, FromDate, ToDate, categoryId, productId, supplierId, receiveNumber, referenceNumber, poNumber, userInformationBO.UserInfoId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], prBO));

            rvTransaction.LocalReport.DisplayName = "Product Received";
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
            productList = productDA.GetItemByCategoryNCostcenter(0, CategoryId);

            ddlProductId.DataSource = productList;
            ddlProductId.DataTextField = "ItemNameAndCode";
            ddlProductId.DataValueField = "ItemId";
            ddlProductId.DataBind();
            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlProductId.Items.Insert(0, item);
        }
    }
}