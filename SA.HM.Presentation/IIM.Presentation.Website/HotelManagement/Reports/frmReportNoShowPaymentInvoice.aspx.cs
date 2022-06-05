using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportNoShowPaymentInvoice : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string queryStringIdList = Request.QueryString["PaymentIdList"];
                ReportProcessing(queryStringIdList);
            }
        }

        private void ReportProcessing(string paymentIdList)
        {
            //this.txtPaymentIdList.Text = string.Empty;
            this.txtCompanyName.Text = string.Empty;
            this.txtCompanyAddress.Text = string.Empty;
            this.txtCompanyWeb.Text = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            this.txtPrintedBy.Text = userInformationBO.UserName;
            HMCommonDA hmCommonDA = new HMCommonDA();
            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
            if (files[0].CompanyId > 0)
            {
                this.txtCompanyName.Text = files[0].CompanyName;
                this.txtCompanyAddress.Text = files[0].CompanyAddress;
                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    this.txtCompanyWeb.Text = files[0].WebAddress;
                }
                else
                {
                    this.txtCompanyWeb.Text = files[0].ContactNumber;
                }

            }

            //this.txtPaymentIdList.Text = this.Session["GuestBillPaymentIdList"].ToString();
            //this.txtGuestBillFromDate.Text = this.Session["GuestBillFromDate"].ToString();
            //this.txtGuestBillToDate.Text = this.Session["GuestBillToDate"].ToString();   

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptNoShowTransactionInfo.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            //_RoomStatusInfoByDate = 1;

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            List<ReportParameter> reportParam = new List<ReportParameter>();

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            //paramImagePath.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderMiddleImagePath"))));
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("HMCompanyProfile", this.txtCompanyName.Text));
            reportParam.Add(new ReportParameter("HMCompanyAddress", this.txtCompanyAddress.Text));
            reportParam.Add(new ReportParameter("HMCompanyWeb", this.txtCompanyWeb.Text));
            reportParam.Add(new ReportParameter("PrintedBy", this.txtPrintedBy.Text));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));

            rvTransaction.LocalReport.SetParameters(reportParam);

            string searchCriteria = paymentIdList;
            ReservationBillPaymentDA reservationBillDA = new ReservationBillPaymentDA();
            List<GuestBillPaymentInvoiceReportViewBO> reservationBillBOList = new List<GuestBillPaymentInvoiceReportViewBO>();
            reservationBillBOList = reservationBillDA.GetReservationNoShowPaymentInvoiceInformation(paymentIdList);
            reportParam.Add(new ReportParameter("ReservationNumber", reservationBillBOList.Count <= 0 ? "" : reservationBillBOList[0].RegistrationNumber));
            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], reservationBillBOList));

            rvTransaction.LocalReport.DisplayName = "Employee Payment Receive";
            rvTransaction.LocalReport.Refresh();
        }
    }
}