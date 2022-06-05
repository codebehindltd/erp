using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportReservationBillPayment : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                string param = Request.QueryString["PaymentIdList"];
                string[] param1 = param.Split(',');
                string queryStringIdList = param1[0];
                string type = param1[1];

                if (!string.IsNullOrEmpty(queryStringIdList))
                {
                    this.Session["ResPaymentIdList"] = string.Empty;
                    this.Session["ResPaymentIdList"] = queryStringIdList;
                }

                if (this.Session["ResPaymentIdList"] != null)
                {
                    this.ReportProcessing(type);
                }
            }
        }

        private void ReportProcessing(string type)
        {
            if (this.Session["ResPaymentIdList"] != null)
            {
                this.txtConversionIdList.Text = string.Empty;
                this.txtConversionIdList.Text = this.Session["ResPaymentIdList"].ToString();
                string searchCriteria = this.txtConversionIdList.Text;

                HMCommonDA hmCommonDA = new HMCommonDA();

                rvTransaction.LocalReport.DataSources.Clear();
                rvTransaction.ProcessingMode = ProcessingMode.Local;

                var reportPath = "";
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptReservationBillPayment.rdlc");

                if (!File.Exists(reportPath))
                    return;

                rvTransaction.LocalReport.ReportPath = reportPath;

                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> files = companyDA.GetCompanyInfo();

                List<ReportParameter> paramReport = new List<ReportParameter>();

                DateTime currentDate = DateTime.Now;
                string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
                string footerPoweredByInfo = string.Empty;
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

                if (files[0].CompanyId > 0)
                {
                    paramReport.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                    paramReport.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));
                }

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isOnlyPdfEnableWhenReportExportBO = new HMCommonSetupBO();
                isOnlyPdfEnableWhenReportExportBO = commonSetupDA.GetCommonConfigurationInfo("IsOnlyPdfEnableWhenReportExport", "IsOnlyPdfEnableWhenReportExport");
                if (isOnlyPdfEnableWhenReportExportBO != null)
                {
                    if (isOnlyPdfEnableWhenReportExportBO.SetupValue == "1")
                    {
                        if (hmUtility.GetCurrentApplicationUserInfo().IsAdminUser != true)
                        {
                            CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.Excel.ToString());
                            CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.Word.ToString());
                        }
                    }
                }

                string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
                rvTransaction.LocalReport.EnableExternalImages = true;

                paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
                paramReport.Add(new ReportParameter("PrintDate", printDate));
                paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));                

                ReservationBillPaymentDA reservationBillDA = new ReservationBillPaymentDA();
                List<GuestBillPaymentInvoiceReportViewBO> reservationBillBOList = new List<GuestBillPaymentInvoiceReportViewBO>();
                reservationBillBOList = reservationBillDA.GetReservationPaymentInvoiceInformation(searchCriteria);
                paramReport.Add(new ReportParameter("ReservationNumber", reservationBillBOList[0].RegistrationNumber));
                rvTransaction.LocalReport.SetParameters(paramReport);
                var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], reservationBillBOList));

                rvTransaction.LocalReport.DisplayName = "Reservation Bill Payment";
                rvTransaction.LocalReport.Refresh();
            }
        }
    }
}