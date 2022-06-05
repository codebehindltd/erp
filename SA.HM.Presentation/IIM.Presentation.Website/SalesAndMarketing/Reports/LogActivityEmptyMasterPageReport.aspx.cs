using HotelManagement.Data.HMCommon;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing.Reports
{
    public partial class LogActivityEmptyMasterPageReport : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        string company, accountManager, logtype, fromDate, toDate, industry;
        protected void Page_Load(object sender, EventArgs e)
        {
            company = Request.QueryString["company"];
            accountManager = Request.QueryString["accountManager"];
            logtype = Request.QueryString["logtype"];
            fromDate = Request.QueryString["fromDate"];
            toDate = Request.QueryString["toDate"];
            industry = Request.QueryString["industry"];
            if(!IsPostBack)
            {
                ReportGenerate(logtype, fromDate, toDate, company, accountManager, industry);
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            //string logtype, company, accountManager, industry, contact;

            
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }

        public void ReportGenerate(string logtype, string fromDate,string toDate,string company,string accountManager,string industry)
        {
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            DateTime? FromDate,  ToDate;
            //logtype = ddlLogType.SelectedValue;
            //company = txtCompanyForSrc.Text;
            //accountManager = txtAccountManagerSrc.Text;
            //industry = txtIndustry.Text;
            string contact = "";
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            if (string.IsNullOrEmpty(fromDate))
            {
                FromDate = null;
            }
            else
            {
                startDate = fromDate;               
                FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            if (string.IsNullOrEmpty(toDate))
            {
                ToDate = null;
            }
            else
            {
                endDate = toDate;                
                ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            }


            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            var reportPath = "";

            reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/rptLogActivity.rdlc");


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

            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            rvTransaction.LocalReport.EnableExternalImages = true;
            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            List<SalesCallEntryBO> SalesCallInformation = new List<SalesCallEntryBO>();
            SalesCallDA DA = new SalesCallDA();
            SalesCallInformation = DA.GetLogActivityInformationForReport(logtype, FromDate, ToDate, company, accountManager, industry, contact);

            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], SalesCallInformation));
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";

        }
    }
}