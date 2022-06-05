using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.PurchaseManagment;
using System.IO;
using Microsoft.Reporting.WebForms;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.PurchaseManagment.Reports
{
    public partial class frmReportDateWisePurchaseInformation : BasePage
    {
        protected int isMessageBoxEnable = -1;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadProduct();
                this.LoadSupplierInfo();
                this.LoadPurchaseOrder();
            }

        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            _RoomStatusInfoByDate = 1;
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

            if (ddlPOrder.SelectedValue != "0")
            {
                pONumber = ddlPOrder.SelectedValue;
            }

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
            reportPath = Server.MapPath(@"~/PurchaseManagment/Reports/Rdlc/RptDateWisePurchase.rdlc");

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

            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            rvTransaction.LocalReport.EnableExternalImages = true;
            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            PMPurchaseOrderDA purchaseDa = new PMPurchaseOrderDA();
            List<PurchaseInformationBO> purchaseInfo = new List<PurchaseInformationBO>();
            if (ddlPOApprovalStatus.SelectedValue == "0")
            {
                //purchaseInfo = purchaseDa.GetPurchaseInformation(FromDate, ToDate, supplierId, itemId, pONumber);
            }
            else if (ddlPOApprovalStatus.SelectedValue == "1")
            {
                //purchaseInfo = purchaseDa.GetPurchaseInformation(FromDate, ToDate, supplierId, itemId, pONumber).Where(x => x.ApprovedStatus == "Submitted").ToList();
            }
            else if (ddlPOApprovalStatus.SelectedValue == "2")
            {
                //purchaseInfo = purchaseDa.GetPurchaseInformation(FromDate, ToDate, supplierId, itemId, pONumber).Where(x => x.ApprovedStatus == "Approved").ToList();
            }
            else if (ddlPOApprovalStatus.SelectedValue == "3")
            {
                //purchaseInfo = purchaseDa.GetPurchaseInformation(FromDate, ToDate, supplierId, itemId, pONumber).Where(x => x.ApprovedStatus == "Checked").ToList();
            }
            else if (ddlPOApprovalStatus.SelectedValue == "4")
            {
                //purchaseInfo = purchaseDa.GetPurchaseInformation(FromDate, ToDate, supplierId, itemId, pONumber).Where(x => x.ApprovedStatus == "Cancel").ToList();
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
        private void LoadProduct()
        {
            InvItemDA productDA = new InvItemDA();
            this.ddlProductId.DataSource = productDA.GetInvItemInfo();
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
            item.Text = "---None---";
            this.ddlSupplier.Items.Insert(0, item);
        }
        private void LoadPurchaseOrder()
        {
            PMPurchaseOrderDA entityDA = new PMPurchaseOrderDA();
            this.ddlPOrder.DataSource = entityDA.GetAllPMPurchaseOrderInfo("Product");
            this.ddlPOrder.DataTextField = "PONumber";
            this.ddlPOrder.DataValueField = "POrderId";
            this.ddlPOrder.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "---None---";
            this.ddlPOrder.Items.Insert(0, item);
        }
    }
}