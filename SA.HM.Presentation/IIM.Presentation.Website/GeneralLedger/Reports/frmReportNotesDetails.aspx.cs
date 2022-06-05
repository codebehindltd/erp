using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using System.Collections;
using HotelManagement.Entity.GeneralLedger;
using Microsoft.Reporting.WebForms;
using System.IO;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.GeneralLedger.Reports
{
    public partial class frmReportNotesDetails : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GenerateReport();
            }
        }
        private void GenerateReport()
        {
            string startDate = string.Empty, endDate = string.Empty, fiscalYearId = string.Empty;
            int companyId = 0, projectId = 0, donorId = 0;
            string withOrWithoutOpening = string.Empty;

            int nodeId = Convert.ToInt32(Request.QueryString["nd"]);

            if (Request.QueryString["fy"] != "0")
            {
                fiscalYearId = Convert.ToString(Request.QueryString["fy"]);
            }

            if (Request.QueryString["cp"] != "0")
            {
                companyId = Convert.ToInt32(Request.QueryString["cp"]);
            }

            if (Request.QueryString["pj"] != "0")
            {
                projectId = Convert.ToInt32(Request.QueryString["pj"]);
            }

            if (Request.QueryString["dr"] != "0")
            {
                donorId = Convert.ToInt32(Request.QueryString["dr"]);
            }

            if (Request.QueryString["wop"] != null)
            {
                withOrWithoutOpening = Request.QueryString["wop"].ToString();
            }

            if (Request.QueryString["sd"] == string.Empty)
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
            }
            else
            {
                startDate = Convert.ToString(Request.QueryString["sd"]);
            }

            if (Request.QueryString["ed"] == string.Empty)
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
            }
            else
            {
                endDate = Convert.ToString(Request.QueryString["ed"]);
            }

            if (!string.IsNullOrEmpty(fiscalYearId))
            {
                GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
                GLFiscalYearBO fiscalyearBO = new GLFiscalYearBO();
                fiscalyearBO = fiscalYearDA.GetFiscalYearId(Convert.ToInt32(fiscalYearId));

                startDate = fiscalyearBO.ReportFromDate;
                endDate = fiscalyearBO.ReportToDate;
            }

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            NodeMatrixDA nmDa = new NodeMatrixDA();
            NodeMatrixBO nodeMatrix = new NodeMatrixBO();
            nodeMatrix = nmDa.GetNodeMatrixInfoById(nodeId);

            bool isIndividualLedger = (bool)nodeMatrix.IsTransactionalHead;
            var reportPath = "";

            if (isIndividualLedger)
            {
                reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/rptGeneralLedger.rdlc");
            }
            else if (!isIndividualLedger)
            {
                reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/rptGeneralGroupLedger.rdlc");
            }

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

            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("ReportDateFrom", startDate));
            reportParam.Add(new ReportParameter("ReportDateTo", endDate));

            GLCommonReportDA commonReportDa = new GLCommonReportDA();
            List<LedgerBookReportBO> generalLedger = new List<LedgerBookReportBO>();

            if (!string.IsNullOrEmpty(fiscalYearId))
            {
                if (isIndividualLedger)
                {
                    generalLedger = commonReportDa.GetLedgerBookReport(FromDate, ToDate, nodeId, companyId, projectId, donorId, withOrWithoutOpening);
                }
                else if (!isIndividualLedger)
                {
                    generalLedger = commonReportDa.GetGroupLedgerBookReport(FromDate, ToDate, nodeId, companyId, projectId, donorId, withOrWithoutOpening);
                }
            }
            else
            {
                if (isIndividualLedger)
                {
                    generalLedger = commonReportDa.GetLedgerBookReportDaterangeWise(FromDate, ToDate, nodeId, companyId, projectId, donorId, withOrWithoutOpening);
                }
                else if (!isIndividualLedger)
                {
                    generalLedger = commonReportDa.GetGroupLedgerBookReportDateRangeWise(FromDate, ToDate, nodeId, companyId, projectId, donorId, withOrWithoutOpening);
                }
            }

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
                        if (row.OpeningBalance != 0)
                        {
                            row.OpeningBalance = row.OpeningBalance / CurrencyConversionRate;
                        }

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

                        if (row.BalanceAmount != 0)
                        {
                            row.BalanceAmount = row.BalanceAmount / CurrencyConversionRate;
                        }

                        if (row.NodeBalanceAmount != 0)
                        {
                            row.NodeBalanceAmount = row.NodeBalanceAmount / CurrencyConversionRate;
                        }
                    }
                }
                // // // ------- Multi Currency Related Effects -------------------------- End
            }

            reportParam.Add(new ReportParameter("CompanyName", companyName));
            reportParam.Add(new ReportParameter("CompanyProject", companyProject));
            reportParam.Add(new ReportParameter("ReportCurrency", reportCurrency));
            reportParam.Add(new ReportParameter("PrintDateTime", printDateTime));

            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], generalLedger));

            rvTransaction.LocalReport.DisplayName = "General Ledger";
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