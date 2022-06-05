using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.HMCommon;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.UserInformation;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportGuestTotalStayovers : BasePage
    { 
        HMUtility hmUtility = new HMUtility();
       
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {           
            string guestName = string.Empty, companyName = string.Empty, passportNumber = string.Empty, mobileNumber = string.Empty;
            int? minNoOfNights = null;

            if (!string.IsNullOrWhiteSpace(this.txtSrcGuestName.Text))
            {
                guestName = txtSrcGuestName.Text;
            }

            if (!string.IsNullOrWhiteSpace(this.txtSrcCompanyName.Text))
            {
                companyName = txtSrcCompanyName.Text;
            }

            if (!string.IsNullOrWhiteSpace(this.txtSrcPassportNumber.Text))
            {
                passportNumber = txtSrcPassportNumber.Text;
            }

            if (!string.IsNullOrWhiteSpace(this.txtSrcMobileNumber.Text))
            {
                mobileNumber = txtSrcMobileNumber.Text;
            }

            if (!string.IsNullOrWhiteSpace(this.txtMinNoOfNights.Text))
            {
                minNoOfNights = Convert.ToInt32(txtMinNoOfNights.Text);
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestTotalStayovers.rdlc");

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

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");

            rvTransaction.LocalReport.EnableExternalImages = true;

            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));

            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            GuestInformationDA guestDa = new GuestInformationDA();
            List<GuestStayedNightBO> guestInfo = new List<GuestStayedNightBO>();
            guestInfo = guestDa.GetGuestTotalStayovers(guestName, companyName, passportNumber, mobileNumber, minNoOfNights);

            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], guestInfo));
            rvTransaction.LocalReport.DisplayName = "Guests Total Stayovers";
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
    }
}