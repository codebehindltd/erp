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
    public partial class frmReportBudgetShow : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();

        string fiscalYear = string.Empty, budgetId = string.Empty, approvedStatus = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fiscalYear = Request.QueryString["fid"] != null ? Request.QueryString["fid"].ToString() : string.Empty;
                budgetId = Request.QueryString["bid"] != null ? Request.QueryString["bid"].ToString() : string.Empty;
                approvedStatus = Request.QueryString["as"] != null ? Request.QueryString["as"].ToString() : string.Empty;

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
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
        //************************ User Defined Function ********************//
        private void GenerateReport()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            DateTime startDate = DateTime.Now, endDate = DateTime.Now;
            GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
            GLFiscalYearBO fiscalyearBO = new GLFiscalYearBO();

            if (!string.IsNullOrEmpty(fiscalYear))
            {
                int fiscalYearId = Convert.ToInt32(fiscalYear);
                if (fiscalYearId > 0)
                {
                    fiscalyearBO = fiscalYearDA.GetFiscalYearId(fiscalYearId);

                    startDate = fiscalyearBO.FromDate;
                    endDate = fiscalyearBO.ToDate;
                }
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/rptBudget.rdlc");

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

            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));

            //-- Company Logo -------------------------------
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            //List<ReportParameter> paramLogo = new List<ReportParameter>();
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            rvTransaction.LocalReport.SetParameters(paramReport);
            //-- Company Logo ------------------End----------

            GLBudgetDA budgetDa = new GLBudgetDA();
            GLBudgetBO budget = new GLBudgetBO();
            List<GLBudgetDetailsReportBO> budgetDetails = new List<GLBudgetDetailsReportBO>();
            List<GLBudgetDetailsReportBO> budgetDetailsReport = new List<GLBudgetDetailsReportBO>();
            List<MonthYearBO> monthyear = new List<MonthYearBO>();

            budget = budgetDa.GetBudgetByFiscalYearAndStatus(Convert.ToInt64(fiscalYear), approvedStatus);
            budgetDetails = budgetDa.GetBudgetDetailsForReportByBudgetId(Convert.ToInt64(budgetId));

            string format = fiscalyearBO.FromDate.Year == fiscalyearBO.ToDate.Year ? "MMM" : "MMM yy";

            for (var d = fiscalyearBO.FromDate; d <= fiscalyearBO.ToDate; d = d.AddMonths(1))
            {
                monthyear.Add(new MonthYearBO
                {
                    MonthId = d.Month,
                    MonthName = d.ToString(format)
                });
            }

            foreach (GLBudgetDetailsReportBO bd in budgetDetails)
            {
                var m = (from my in monthyear where my.MonthId == bd.MonthId select my.MonthName).FirstOrDefault();

                budgetDetailsReport.Add(new GLBudgetDetailsReportBO
                {
                    BudgetDetailsId = bd.BudgetDetailsId,
                    BudgetId = bd.BudgetId,
                    MonthId = bd.MonthId,
                    MonthName = m == null ? "" : m,
                    NodeId = bd.NodeId,
                    NodeHead = bd.NodeHead,
                    Amount = bd.Amount,
                    FiscalYearName = budget.FiscalYearName,

                    NodeNumber = bd.NodeNumber,
                    NodeType = bd.NodeType,
                    GroupNodeNumber = bd.GroupNodeNumber,
                    GroupNodeHead = bd.GroupNodeHead,
                    NodeLevel = bd.NodeLevel,
                    NodeOrder = bd.NodeOrder,
                    GroupOrder = bd.GroupOrder,
                    GroupLevel = bd.GroupLevel,
                    GroupId = bd.GroupId
                });

            }

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], budgetDetailsReport));

            rvTransaction.LocalReport.DisplayName = "Budget";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
    }
}