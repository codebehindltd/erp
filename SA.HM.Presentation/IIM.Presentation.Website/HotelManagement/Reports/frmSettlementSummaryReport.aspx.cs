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
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmSettlementSummaryReport : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int _StatusInfoByDate = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            rvSettlementSum.LocalReport.EnableExternalImages = true;  
        }

        protected void btnGenerateClick(object sender, EventArgs e)
        {
            _StatusInfoByDate = 1;
            DateTime dateTime = DateTime.Now;
            string startDate = string.Empty, endDate = string.Empty;

            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {

                startDate = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = this.txtToDate.Text;
            }

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            rvSettlementSum.LocalReport.DataSources.Clear();
            rvSettlementSum.ProcessingMode = ProcessingMode.Local;
            rvSettlementSum.LocalReport.EnableExternalImages = true;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptSettlementSummary.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvSettlementSum.LocalReport.ReportPath = reportPath;

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
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FromDate", FromDate.Date.ToString("dd/MM/yyyy")));
            reportParam.Add(new ReportParameter("ToDate", ToDate.Date.ToString("dd/MM/yyyy")));

            rvSettlementSum.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvSettlementSum.LocalReport.GetDataSourceNames();

            GuestInformationDA guestDA = new GuestInformationDA();
            List<SettlementSummaryBO> settlementSUmmary = new List<SettlementSummaryBO>();

            settlementSUmmary = guestDA.GetSettlementSummaryInfo(FromDate, ToDate);

            rvSettlementSum.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], settlementSUmmary));
            rvSettlementSum.LocalReport.DisplayName = "Settlement Summary";
            rvSettlementSum.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }

        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvSettlementSum.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
        //private void LoadCurrentDate()
        //{
        //    DateTime dateTime = DateTime.Now;
        //    this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
        //    this.txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
        //}
    }
}