using HotelManagement.Data.HMCommon;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.PurchaseManagment;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing.Reports
{
    public partial class frmCompanyPaymentReceipt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pId = Request.QueryString["PId"];

            if (!String.IsNullOrEmpty(pId))
            {
                if (!IsPostBack)
                {
                    LoadReport(Int32.Parse(pId));
                }
            }
        }

        private void LoadReport(int paymentId)
        {
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/rptCompanyPaymentReceipt.rdlc");

            if (!File.Exists(reportPath))
                return;

            List<ReportParameter> reportParam = new List<ReportParameter>();
            HMCommonDA hmCommonDA = new HMCommonDA();

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");

            CompanyDA companyDA = new CompanyDA();

            List<CompanyPaymentBO> companyPaymentInfo = new List<CompanyPaymentBO>();
            companyPaymentInfo = companyDA.GetCompanyListInfoById(paymentId);

            //List<SupplierPaymentBO> supplierPaymentInfo = new List<SupplierPaymentBO>();
            //supplierPaymentInfo = supplierDA.GetSupplierPaymentInfoByPaymentId(paymentId);

            //List<PMSupplierPaymentLedgerBO> SupplierBillInfoBOList = new List<PMSupplierPaymentLedgerBO>();
            //List<PurchaseOrderDetailsForInvoiceBO> supplierInfoBOList = new List<PurchaseOrderDetailsForInvoiceBO>();

            //if (companyPaymentInfo != null)
            //{
            //    if (companyPaymentInfo.Count > 0)
            //    {
            //        PMPurchaseOrderDA purchaseDa = new PMPurchaseOrderDA();
            //        supplierInfoBOList = purchaseDa.GetSupplierInformationForInvoice(supplierPaymentInfo[0].SupplierId);

            //    }
            //}

            rvTransaction.LocalReport.ReportPath = reportPath;
            rvTransaction.LocalReport.EnableExternalImages = true;
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            //TNC = purchaseDa.GetTermsNConditionsByPOId(POrderId);
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], companyPaymentInfo));
           
            //rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[2], TNC));
            rvTransaction.LocalReport.DisplayName = "Company Invoice";

            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            //reportParam.Add(new ReportParameter("CurrencyId", currencyId.ToString()));
            //rvTransaction.LocalReport.SetParameters(reportParam);

            rvTransaction.LocalReport.Refresh();
        }
    }
}