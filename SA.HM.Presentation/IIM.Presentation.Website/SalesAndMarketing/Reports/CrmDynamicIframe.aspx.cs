using HotelManagement.Data.HMCommon;
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
    public partial class CrmDynamicIframe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<DynamicReportBO> dynamicReports = HttpContext.Current.Session["GetDynamic"] == null ? new List<DynamicReportBO>() : HttpContext.Current.Session["GetDynamic"] as List<DynamicReportBO>;
                DateTime FromDate = HttpContext.Current.Session["GetDynamicFromDate"] == null ? DateTime.Now :Convert.ToDateTime(HttpContext.Current.Session["GetDynamicFromDate"]);
                DateTime ToDate = HttpContext.Current.Session["GetDynamicToDate"] == null ? DateTime.Now : Convert.ToDateTime(HttpContext.Current.Session["GetDynamicToDate"]);

                var name = Request.QueryString["name"];
                
                GenerateReport(dynamicReports, FromDate, ToDate, name);
            }
           
        }
        HMUtility hmUtility = new HMUtility();
        HMCommonDA hmCommonDA = new HMCommonDA();
        private void GenerateReport(List<DynamicReportBO> dynamicReports, DateTime FromDate, DateTime ToDate, string Name)
        {
            string fromDate = FromDate.ToShortDateString();
            string toDate = ToDate.ToShortDateString();
            //FromDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            //ToDate = hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = "";


            reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/DynamicCRMReport.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;
            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> paramReport = new List<ReportParameter>();

            if (files[0].CompanyId > 0)
            {
                paramReport.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                paramReport.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    paramReport.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }
                else
                {
                    paramReport.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                }
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            //List<ReportParameter> param2 = new List<ReportParameter>();
            //List<ReportParameter> param3 = new List<ReportParameter>();

            paramReport.Add(new ReportParameter("FromDate", fromDate.ToString()));
            paramReport.Add(new ReportParameter("ToDate", toDate.ToString()));
            //rvTransaction.LocalReport.SetParameters(param2);
            //rvTransaction.LocalReport.SetParameters(param3);

            string reportName = "Dynamic Report";
            //List<ReportParameter> paramReportName = new List<ReportParameter>();
            if (Name == "")
            {
                paramReport.Add(new ReportParameter("ReportName", reportName));
            }
            else
            {
                paramReport.Add(new ReportParameter("ReportName", Name));
            }
           
            //rvTransaction.LocalReport.SetParameters(paramReportName);

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvTransaction.LocalReport.SetParameters(paramReport);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], dynamicReports));
            rvTransaction.LocalReport.DisplayName = "Daily Sales Statement";
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
    }
}