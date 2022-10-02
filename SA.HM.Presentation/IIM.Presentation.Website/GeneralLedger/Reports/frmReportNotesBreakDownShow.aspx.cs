using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.GeneralLedger;
using System.Collections;
using System.Web.Services;
using HotelManagement.Entity.GeneralLedger;
using Microsoft.Reporting.WebForms;
using HotelManagement.Presentation.Website.Common;
using Newtonsoft.Json;
using HotelManagement.Entity.UserInformation;
using System.IO;

namespace HotelManagement.Presentation.Website.GeneralLedger.Reports
{
    public partial class frmReportNotesBreakDownShow : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();

        string txtStartDate = string.Empty, txtEndDate = string.Empty, ddlSearchType = string.Empty, ddlFiscalYear = string.Empty;
        string ddlGLCompany = string.Empty, ddlGLProject = string.Empty, ddlDonor = string.Empty, notesNodeId = string.Empty, withOrWithoutOpening = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtStartDate = Request.QueryString["sd"] != null ? Request.QueryString["sd"].ToString() : string.Empty;
                txtEndDate = Request.QueryString["ed"] != null ? Request.QueryString["ed"].ToString() : string.Empty;
                ddlSearchType = Request.QueryString["st"] != null ? Request.QueryString["st"].ToString() : string.Empty;
                ddlFiscalYear = Request.QueryString["fy"] != null ? Request.QueryString["fy"].ToString() : string.Empty;
                ddlGLCompany = Request.QueryString["cp"] != null ? Request.QueryString["cp"].ToString() : string.Empty;
                ddlGLProject = Request.QueryString["pj"] != null ? Request.QueryString["pj"].ToString() : string.Empty;
                ddlDonor = Request.QueryString["dr"] != null ? Request.QueryString["dr"].ToString() : string.Empty;
                withOrWithoutOpening = Request.QueryString["wop"] != null ? Request.QueryString["wop"].ToString() : string.Empty;
                notesNodeId = Request.QueryString["nod"] != null ? Request.QueryString["nod"].ToString() : string.Empty;
                GenerateReport();
            }
        }

        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();
            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Portrait.ToString());
            frmPrint.Attributes["src"] = reportSource;
        }
        //************************ User Defined Function ********************//
        private void GenerateReport()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();

            string startDate = string.Empty, endDate = string.Empty;

            DateTime dateTime = DateTime.Now;
            if (string.IsNullOrWhiteSpace(txtStartDate))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtStartDate = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtStartDate;
            }
            if (string.IsNullOrWhiteSpace(txtEndDate))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtEndDate = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = txtEndDate;
            }

            if (ddlSearchType == "FiscalYear" || ddlSearchType == "1")
            {
                int fiscalYearId = Convert.ToInt32(ddlFiscalYear);
                if (fiscalYearId > 0)
                {
                    GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
                    GLFiscalYearBO fiscalyearBO = new GLFiscalYearBO();
                    fiscalyearBO = fiscalYearDA.GetFiscalYearId(fiscalYearId);

                    startDate = fiscalyearBO.ReportFromDate;
                    endDate = fiscalyearBO.ReportToDate;
                }
            }

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            int companyId = 0, projectId = 0, donorId = 0;
            string showZeroTransaction = string.Empty;

            if (ddlGLCompany != "0" || ddlGLCompany != "")
            {
                companyId = Convert.ToInt32(ddlGLCompany);
            }

            if (ddlGLProject != "0" || ddlGLProject != "")
            {
                projectId = Convert.ToInt32(ddlGLProject);
            }

            if (ddlDonor != "0" || ddlDonor != "")
            {
                donorId = Convert.ToInt32(ddlDonor);
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/rptNotesBreakDown.rdlc");

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

            int nodeId = 0;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));

            //-- Company Logo -------------------------------
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            paramReport.Add(new ReportParameter("ReportDateFrom", FromDate.ToString("dd-MMM-yyyy")));
            paramReport.Add(new ReportParameter("ReportDateTo", ToDate.ToString("dd-MMM-yyyy")));

            //-- Company Logo ------------------End----------
            GLCommonReportDA commonReportDa = new GLCommonReportDA();
            List<LedgerBookReportBO> generalLedger = new List<LedgerBookReportBO>();
            generalLedger = commonReportDa.GetNotesBreakdownReport(FromDate, ToDate, nodeId, companyId, projectId, donorId, notesNodeId, withOrWithoutOpening);

            string companyName = string.Empty, companyProject = string.Empty, reportCurrency = string.Empty, printDateTime = string.Empty;
            // // // ------- Multi Currency Related Effects -------------------------- Start
            decimal CurrencyConversionRate = 1;
            if (Session["ReportCurrencyConversionRate"] != null)
            {
                companyName = Session["ReportAccountsCompanyName"].ToString();
                companyProject = Session["ReportAccountsProjectName"].ToString();
                reportCurrency = Session["ReportCurrencyName"].ToString();
                printDateTime = Session["ReportPrintDate"].ToString();

                CurrencyConversionRate = Convert.ToDecimal(Session["ReportCurrencyConversionRate"].ToString());

                foreach (LedgerBookReportBO row in generalLedger)
                {
                    if (CurrencyConversionRate != 1)
                    {
                        if (row.DRAmount != 0)
                        {
                            row.DRAmount = row.DRAmount / CurrencyConversionRate;
                        }

                        if (row.CRAmount != 0)
                        {
                            row.CRAmount = row.CRAmount / CurrencyConversionRate;
                        }

                        if (row.ClosingBalance != 0)
                        {
                            row.ClosingBalance = row.ClosingBalance / CurrencyConversionRate;
                        }
                    }
                }
                // // // ------- Multi Currency Related Effects -------------------------- End
            }

            paramReport.Add(new ReportParameter("CompanyName", companyName));
            paramReport.Add(new ReportParameter("CompanyProject", companyProject));
            paramReport.Add(new ReportParameter("ReportCurrency", reportCurrency));
            paramReport.Add(new ReportParameter("PrintDateTime", printDateTime));

            rvTransaction.LocalReport.SetParameters(paramReport);
            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], generalLedger));

            rvTransaction.LocalReport.DisplayName = "Notes Breakdown";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
    }
}