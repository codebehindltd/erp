using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.PurchaseManagment;

namespace HotelManagement.Presentation.Website.SalesAndMarketing.Reports
{
    public partial class frmReportSalesOrderInvoice : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            string pOrderId = Request.QueryString["POrderId"];
            string supplierId = Request.QueryString["SupId"];

            if (!String.IsNullOrEmpty(pOrderId))
            {
                if (!IsPostBack)
                {
                    LoadReport(Int32.Parse(pOrderId), Int32.Parse(supplierId));
                }
            }
        }

        private void LoadReport(int POrderId, int supplierId)
        {
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/rptSalesorderInvoice.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;
            rvTransaction.LocalReport.EnableExternalImages = true;

            List<ReportParameter> reportParam = new List<ReportParameter>();
            HMCommonDA hmCommonDA = new HMCommonDA();
            //CompanyDA companyDA = new CompanyDA();
            //List<CompanyBO> files = companyDA.GetCompanyInfo();
            //if (files[0].CompanyId > 0)
            //{
            //    reportParam.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
            //    reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

            //    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
            //    {
            //        reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
            //    }
            //    else
            //    {
            //        reportParam.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
            //    }
            //}

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            //DateTime currentDate = DateTime.Now;
            //string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            //string footerPoweredByInfo = string.Empty;
            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            //footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            //reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            //reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            rvTransaction.LocalReport.SetParameters(reportParam);

            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            PMPurchaseOrderDA purchaseDa = new PMPurchaseOrderDA();
            //List<PMPurchaseOrderInfoReportBO> purchaseInfo = new List<PMPurchaseOrderInfoReportBO>();
            //purchaseInfo = purchaseDa.GetPMPurchaseOrderInfoForReport(POrderId);

            List<SupplierNCompanyInfoForPurchaseInvoiceBO> purchaseInfo = new List<SupplierNCompanyInfoForPurchaseInvoiceBO>();
            List<PurchaseOrderDetailsForInvoiceBO> supplierInfo = new List<PurchaseOrderDetailsForInvoiceBO>();

            purchaseInfo = purchaseDa.GetSalesOrderInfoForInvoice(POrderId, supplierId);
            supplierInfo = purchaseDa.GetSalesOrderSupplierInformationForInvoice(supplierId);

            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], supplierInfo));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[1], purchaseInfo));
            rvTransaction.LocalReport.DisplayName = "Sales Order Information";
            rvTransaction.LocalReport.Refresh();
        }
    }
}