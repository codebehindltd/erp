using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.PurchaseManagment;
using System.IO;
using Microsoft.Reporting.WebForms;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.PurchaseManagment.Reports
{
    public partial class frmReportItemWisePurchaseInformation : BasePage
    {
        protected int isMessageBoxEnable = -1;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadGLCompany();
                this.LoadProduct();
                this.LoadSupplierInfo();
                this.LoadCategory();
                this.LoadAllCostCentreInfo();
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            _RoomStatusInfoByDate = 1;
            string reportType = ddlReportType.SelectedValue;
            int companyId = Convert.ToInt32(ddlGLCompanyId.SelectedValue);
            int costCenter = Convert.ToInt32(ddlFromCostCenter.SelectedValue);
            int category = Convert.ToInt32(ddlCategoryId.SelectedValue);
            int item = Convert.ToInt32(ddlProductId.SelectedValue);
            int supplier = Convert.ToInt32(ddlSupplier.SelectedValue);
            string purchaseType = ddlPurchaseType.SelectedValue;
            string pONo = (txtPOrder.Text);

            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            int? itemId = null, supplierId = null;
            string pONumber = null;

            if (string.IsNullOrEmpty(txtStartDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtStartDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtStartDate.Text;
            }
            if (string.IsNullOrEmpty(txtEndDate.Text))
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

            if (ddlSupplier.SelectedValue != "0")
            {
                supplierId = Convert.ToInt32(ddlSupplier.SelectedValue);
            }

            if (ddlProductId.SelectedValue != "0")
            {
                itemId = Convert.ToInt32(ddlProductId.SelectedValue);
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            if (reportType == "Item Wise")
            {
                reportPath = Server.MapPath(@"~/PurchaseManagment/Reports/Rdlc/RptItemWisePurchase.rdlc");
            }
            else if (reportType == "Date Wise")
            {
                reportPath = Server.MapPath(@"~/PurchaseManagment/Reports/Rdlc/RptDateWisePurchase.rdlc");
            }
            else if (reportType == "Supplier Wise")
            {
                reportPath = Server.MapPath(@"~/PurchaseManagment/Reports/Rdlc/RptSupplierWisePurchase.rdlc");
            }
            else if (reportType == "Costcenter Wise")
            {
                reportPath = Server.MapPath(@"~/PurchaseManagment/Reports/Rdlc/RptCostCenterWisePurchase.rdlc");
            }
            else if (reportType == "Category Wise")
            {
                reportPath = Server.MapPath(@"~/PurchaseManagment/Reports/Rdlc/RptCategoryWisePurchase.rdlc");
            }
            else if (reportType == "PO Number Wise")
            {
                reportPath = Server.MapPath(@"~/PurchaseManagment/Reports/Rdlc/RptPONumberWisePurchase.rdlc");
            }
            else
                reportPath = Server.MapPath(@"~/PurchaseManagment/Reports/Rdlc/RptItemWisePurchase.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            List<ReportParameter> reportParam = new List<ReportParameter>();

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            string companyName = string.Empty;
            string companyAddress = string.Empty;
            string webAddress = string.Empty;

            //-- Company Logo -------------------------------
            HMCommonDA hmCommonDA = new HMCommonDA();
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

            int glCompanyId = Convert.ToInt32(ddlGLCompanyId.SelectedValue);
            if (glCompanyId > 0)
            {
                GLCompanyBO glCompanyBO = new GLCompanyBO();
                GLCompanyDA glCompanyDA = new GLCompanyDA();
                glCompanyBO = glCompanyDA.GetGLCompanyInfoById(glCompanyId);
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

            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("CompanyProfile", companyName));
            reportParam.Add(new ReportParameter("CompanyAddress", companyAddress));
            reportParam.Add(new ReportParameter("CompanyWeb", webAddress));
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            PMPurchaseOrderDA purchaseDa = new PMPurchaseOrderDA();
            List<PurchaseInformationBO> purchaseInfo = new List<PurchaseInformationBO>();

            if (ddlPOApprovalStatus.SelectedValue == "0")
            {
                purchaseInfo = purchaseDa.GetPurchaseInformation(companyId, FromDate, ToDate, supplierId, itemId, pONo, category, costCenter, purchaseType, userInformationBO.UserInfoId).ToList();
            }
            else if (ddlPOApprovalStatus.SelectedValue == "1")
            {
                purchaseInfo = purchaseDa.GetPurchaseInformation(companyId, FromDate, ToDate, supplierId, itemId, pONo, category, costCenter, purchaseType, userInformationBO.UserInfoId).Where(x => x.ApprovedStatus == "Submitted").ToList();
            }
            else if (ddlPOApprovalStatus.SelectedValue == "2")
            {
                purchaseInfo = purchaseDa.GetPurchaseInformation(companyId, FromDate, ToDate, supplierId, itemId, pONo, category, costCenter, purchaseType, userInformationBO.UserInfoId).Where(x => x.ApprovedStatus == "Approved").ToList();
            }
            else if (ddlPOApprovalStatus.SelectedValue == "3")
            {
                purchaseInfo = purchaseDa.GetPurchaseInformation(companyId, FromDate, ToDate, supplierId, itemId, pONo, category, costCenter, purchaseType, userInformationBO.UserInfoId).Where(x => x.ApprovedStatus == "Checked").ToList();
            }
            else if (ddlPOApprovalStatus.SelectedValue == "4")
            {
                purchaseInfo = purchaseDa.GetPurchaseInformation(companyId, FromDate, ToDate, supplierId, itemId, pONo, category, costCenter, purchaseType, userInformationBO.UserInfoId).Where(x => x.ApprovedStatus == "Cancel").ToList();
            }

            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], purchaseInfo));
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
        //************************ User Defined Function ********************//
        private void LoadGLCompany()
        {
            GLCompanyDA entityDA = new GLCompanyDA();
            List<GLCompanyBO> GLCompanyBOList = new List<GLCompanyBO>();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO.UserInfoId == 1)
            {
                GLCompanyBOList = entityDA.GetAllGLCompanyInfo();
            }
            else
            {
                GLCompanyBOList = entityDA.GetAllGLCompanyInfoByUserGroupId(userInformationBO.UserGroupId);
            }

            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            if (GLCompanyBOList != null)
            {
                if (GLCompanyBOList.Count == 1)
                {
                    hfIsSingleGLCompany.Value = "1";
                    companyList.Add(GLCompanyBOList[0]);
                    this.ddlGLCompanyId.DataSource = companyList;
                    this.ddlGLCompanyId.DataTextField = "Name";
                    this.ddlGLCompanyId.DataValueField = "CompanyId";
                    this.ddlGLCompanyId.DataBind();
                }
                else
                {
                    hfIsSingleGLCompany.Value = "2";
                    this.ddlGLCompanyId.DataSource = GLCompanyBOList;
                    this.ddlGLCompanyId.DataTextField = "Name";
                    this.ddlGLCompanyId.DataValueField = "CompanyId";
                    this.ddlGLCompanyId.DataBind();

                    ListItem itemCompany = new ListItem();
                    itemCompany.Value = "0";
                    itemCompany.Text = hmUtility.GetDropDownFirstAllValue();
                    this.ddlGLCompanyId.Items.Insert(0, itemCompany);
                }
            }
        }
        private void LoadProduct()
        {
            List<InvItemBO> items = new List<InvItemBO>();
            InvItemDA productDA = new InvItemDA();
            items = productDA.GetInvItemInfo();
            items = items.Where(p => p.IsSupplierItem == true).ToList();
            this.ddlProductId.DataSource = items;
            this.ddlProductId.DataTextField = "Name";
            this.ddlProductId.DataValueField = "ItemId";
            this.ddlProductId.DataBind();

            ListItem itemProduct = new ListItem();
            itemProduct.Value = "0";
            itemProduct.Text = "---All---";
            this.ddlProductId.Items.Insert(0, itemProduct);

        }
        private void LoadSupplierInfo()
        {
            PMSupplierDA entityDA = new PMSupplierDA();
            this.ddlSupplier.DataSource = entityDA.GetPMSupplierInfo();
            this.ddlSupplier.DataTextField = "Name";
            this.ddlSupplier.DataValueField = "SupplierId";
            this.ddlSupplier.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "---All---";
            this.ddlSupplier.Items.Insert(0, item);
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetAllInvItemCatagoryInfoByServiceType("Product");
            ddlCategoryId.DataSource = List;
            ddlCategoryId.DataTextField = "Name";
            ddlCategoryId.DataValueField = "CategoryId";
            ddlCategoryId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "---All---";
            ddlCategoryId.Items.Insert(0, item);
        }
        private void LoadAllCostCentreInfo()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            costCentreTabBOList = costCentreTabDA.GetCostCentreTabInfoByUserGroupId(userInformationBO.UserGroupId);
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();

            costCentreTabBOList = costCentreTabBOList.Where(o => o.OutletType == 2 && o.CostCenterType == "Inventory").ToList();

            ddlFromCostCenter.DataSource = costCentreTabBOList;
            ddlFromCostCenter.DataTextField = "CostCenter";
            ddlFromCostCenter.DataValueField = "CostCenterId";
            ddlFromCostCenter.DataBind();
            ddlFromCostCenter.Items.Insert(0, item);

        }
    }
}