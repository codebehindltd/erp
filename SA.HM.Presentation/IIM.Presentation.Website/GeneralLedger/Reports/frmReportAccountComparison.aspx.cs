using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.GeneralLedger;
using System.Web.Services;
using System.Collections;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.UserInformation;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Presentation.Website.GeneralLedger.Reports
{
    public partial class frmReportAccountComparison : System.Web.UI.Page
    {
        protected int _RoomStatusInfoByDate = -1;
        protected int isMessageBoxEnable = -1;
        protected string reportSearchType = "0";
        protected string hfFiscalId1 = "0";
        protected string hfFiscalId2 = "0";
        string FinalString = "";
        string url = "";
        HMUtility hmUtility = new HMUtility();
        protected bool isSingle = true;
        protected int isCompanyProjectPanelEnable = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            url = HttpContext.Current.Request.Url.AbsoluteUri;
            rvTransaction.LocalReport.EnableHyperlinks = true;
            url = SplitPageName(url);

            if (!IsPostBack)
            {
                LoadCurrencyHead();
                LoadCommonDropDownHiddenField();
                LoadFiscalYear();
                LoadGLDonor();
            }
        }
        private void LoadCurrencyHead()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            int localCurrencyId = 1;
            List<CommonCurrencyBO> CommonCurrencyBOForLocal = new List<CommonCurrencyBO>();
            CommonCurrencyBOForLocal = headDA.GetConversionHeadInfoByType("Local");
            if (CommonCurrencyBOForLocal != null)
            {
                if (CommonCurrencyBOForLocal.Count > 0)
                {
                    localCurrencyId = CommonCurrencyBOForLocal[0].CurrencyId;
                    hflocalCurrencyId.Value = localCurrencyId.ToString();
                }
            }

            this.ddlCurrencyId.DataSource = headDA.GetConversionHeadInfoByType("All");
            this.ddlCurrencyId.DataTextField = "CurrencyName";
            this.ddlCurrencyId.DataValueField = "ConversionRate";
            this.ddlCurrencyId.DataBind();
            this.ddlCurrencyId.SelectedValue = localCurrencyId.ToString();
        }
        private void LoadGLDonor()
        {
            GLDonorDA entityDA = new GLDonorDA();
            List<GLDonorBO> donorList = new List<GLDonorBO>();
            donorList = entityDA.GetAllGLDonorInfo();

            ddlDonor.DataSource = donorList;
            ddlDonor.DataTextField = "Name";
            ddlDonor.DataValueField = "DonorId";
            ddlDonor.DataBind();

            System.Web.UI.WebControls.ListItem itemDonor = new System.Web.UI.WebControls.ListItem();
            itemDonor.Value = "0";
            itemDonor.Text = hmUtility.GetDropDownFirstAllValue();
            ddlDonor.Items.Insert(0, itemDonor);
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            if (ddlReportType.SelectedValue == "CompanyLedger")
            {
                ComparativeNotesBreakDownDiv.Visible = true;
                GenerateCompanyLedgerReport();
            }
            else if (ddlReportType.SelectedValue == "SupplierLedger")
            {
                ComparativeNotesBreakDownDiv.Visible = false;
                //GenerateSupplierLedgerReport();
            }
            else
            {
                ComparativeNotesBreakDownDiv.Visible = true;
                GenerateReport();
            }
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
            if (!IsFormValid())
            {
                return;
            }
            _RoomStatusInfoByDate = 1;

            string reportType = string.Empty, reportName = string.Empty, displayName = string.Empty, withOrWithoutOpening = string.Empty;

            withOrWithoutOpening = dllWithOrWithoutOpening.SelectedValue;

            if (ddlReportType.SelectedValue == "CashFlow")
            {
                reportType = ddlReportType.SelectedValue;
                reportName = "RptCashFlowStatementComparison";
            }
            else if (ddlReportType.SelectedValue == "ProfitLoss")
            {
                reportType = ddlReportType.SelectedValue;
                reportName = "RptProfitNLossStatementComparison";
            }
            else if (ddlReportType.SelectedValue == "ReceiptNPaymentStatement")
            {
                reportType = ddlReportType.SelectedValue;
                reportName = "RptReceiptNPaymentComparison";
            }
            else if (ddlReportType.SelectedValue == "BalanceSheet")
            {
                reportType = ddlReportType.SelectedValue;
                reportName = "rptBalanceSheetcomparison";
            }

            txtUrl.Text = url;
            string startDate = string.Empty, endDate = string.Empty;
            string startDate2 = string.Empty, endDate2 = string.Empty;

            DateTime dateTime = DateTime.Now;

            if (string.IsNullOrWhiteSpace(txtStartDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtStartDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtStartDate.Text;
            }
            if (string.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtEndDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = txtEndDate.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtStartDate2.Text))
            {
                startDate2 = txtStartDate2.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtEndDate2.Text))
            {
                endDate2 = txtEndDate2.Text;
            }

            if (ddlSearchType.SelectedValue == "1")
            {
                GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
                int fiscalYearId = Convert.ToInt32(ddlFiscalYear.SelectedValue);
                if (fiscalYearId > 0)
                {
                    GLFiscalYearBO fiscalyearBO = new GLFiscalYearBO();
                    fiscalyearBO = fiscalYearDA.GetFiscalYearId(fiscalYearId);
                    startDate = fiscalyearBO.ReportFromDate;
                    endDate = fiscalyearBO.ReportToDate;
                }

                int fiscalYearId2 = Convert.ToInt32(ddlFiscalYear2.SelectedValue);
                if (fiscalYearId2 > 0)
                {
                    GLFiscalYearBO fiscalyearBO = new GLFiscalYearBO();
                    fiscalyearBO = fiscalYearDA.GetFiscalYearId(fiscalYearId2);
                    startDate2 = fiscalyearBO.ReportFromDate;
                    endDate2 = fiscalyearBO.ReportToDate;
                }
            }

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            DateTime FromDate2 = hmUtility.GetDateTimeFromString(startDate2, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate2 = hmUtility.GetDateTimeFromString(endDate2, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);


            if (!string.IsNullOrEmpty(startDate2))
            {
                FromDate2 = hmUtility.GetDateTimeFromString(startDate2, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            if (!string.IsNullOrEmpty(endDate2))
            {
                ToDate2 = hmUtility.GetDateTimeFromString(endDate2, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);
            }

            decimal incomeTaxPercentage = string.IsNullOrWhiteSpace(txtIncomeTaxPercentage.Text) ? 0.00M : Convert.ToDecimal(txtIncomeTaxPercentage.Text);

            int companyId = 0, projectId = 0, donorId = 0;
            string companyName = string.Empty;
            string companyAddress = string.Empty;
            string webAddress = string.Empty;

            //-- Company Logo -------------------------------
            string imageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            string projectName = "All";

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> paramReport = new List<ReportParameter>();

            if (files[0].CompanyId > 0)
            {
                companyName = files[0].CompanyName;
                companyAddress = files[0].CompanyAddress;

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    webAddress = files[0].WebAddress;
                }
                else
                {
                    webAddress = files[0].ContactNumber;
                }
            }

            bool isProfitableOrganization = true;

            if (hfCompanyIsProfitable.Value == "1")
                isProfitableOrganization = true;

            if (hfCompanyId.Value != "0" && hfCompanyId.Value != "")
            {
                companyId = Convert.ToInt32(hfCompanyId.Value);
                GLCompanyBO glCompanyBO = new GLCompanyBO();
                GLCompanyDA glCompanyDA = new GLCompanyDA();
                glCompanyBO = glCompanyDA.GetGLCompanyInfoById(companyId);
                if (glCompanyBO != null)
                {
                    if (glCompanyBO.CompanyId > 0)
                    {
                        companyName = glCompanyBO.Name;
                        if (!string.IsNullOrWhiteSpace(glCompanyBO.CompanyAddress))
                        {
                            companyAddress = glCompanyBO.CompanyAddress;
                        }
                        if (!string.IsNullOrWhiteSpace(glCompanyBO.WebAddress))
                        {
                            webAddress = glCompanyBO.WebAddress;
                        }
                        if (!string.IsNullOrWhiteSpace(glCompanyBO.ImageName))
                        {
                            imageName = glCompanyBO.ImageName;
                        }
                    }
                }
            }

            if (hfProjectId.Value != "0" && hfProjectId.Value != "")
            {
                projectId = Convert.ToInt32(hfProjectId.Value);
                projectName = hfProjectId.Value != "0" ? hfProjectName.Value : "All";
            }

            if (ddlDonor.SelectedValue != "0" || ddlDonor.SelectedValue != "")
            {
                donorId = Convert.ToInt32(ddlDonor.SelectedValue);
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/" + reportName + ".rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetStringFromDateTime(currentDate) + " " + currentDate.ToString("hh:mm:ss tt");
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;
            rvTransaction.LocalReport.EnableExternalImages = true;
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("CompanyProfile", companyName));
            paramReport.Add(new ReportParameter("CompanyAddress", companyAddress));
            paramReport.Add(new ReportParameter("CompanyWeb", webAddress));
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));

            paramReport.Add(new ReportParameter("ReportDateFrom", hmUtility.GetStringFromDateTime(FromDate)));
            paramReport.Add(new ReportParameter("ReportDateTo", hmUtility.GetStringFromDateTime(ToDate)));

            paramReport.Add(new ReportParameter("ReportDateFrom2", hmUtility.GetStringFromDateTime(FromDate2)));
            paramReport.Add(new ReportParameter("ReportDateTo2", hmUtility.GetStringFromDateTime(ToDate2)));

            paramReport.Add(new ReportParameter("CompanyName", companyName));
            paramReport.Add(new ReportParameter("CompanyProject", projectName));
            //rvTransaction.LocalReport.SetParameters(paramReport);

            //Activate Session
            SessionNotesBreakDownBO notesBreakDown = new SessionNotesBreakDownBO();
            notesBreakDown.startDate = hmUtility.GetStringFromDateTime(FromDate);
            notesBreakDown.endDate = hmUtility.GetStringFromDateTime(ToDate);
            notesBreakDown.companyName = files[0].CompanyName;
            notesBreakDown.companyWeb = files[0].WebAddress;
            notesBreakDown.glProject = hfProjectId.Value;
            Session["notesBreakDown"] = notesBreakDown;

            string projectUrl = txtUrl.Text;
            string notesNodesId = string.Empty;

            // // // ------- Multi Currency Related Effects -------------------------- Start
            decimal ConversionRate = 1;
            ConversionRate = Convert.ToDecimal(ddlCurrencyId.SelectedValue);
            string reportCurrency = string.Empty;
            Session["ReportCurrencyConversionRate"] = ConversionRate;

            if (ConversionRate != 1)
            {
                reportCurrency = ddlCurrencyId.SelectedItem.Text + " (C.R: " + ddlCurrencyId.SelectedValue + ")";
            }
            else
            {
                reportCurrency = ddlCurrencyId.SelectedItem.Text;
            }

            paramReport.Add(new ReportParameter("ReportCurrency", reportCurrency));

            Session["ReportAccountsCompanyName"] = companyName;
            Session["ReportAccountsProjectName"] = projectName;
            Session["ReportCurrencyName"] = reportCurrency;
            Session["ReportPrintDate"] = printDate;

            rvTransaction.LocalReport.SetParameters(paramReport);

            GLReportDA reportDA = new GLReportDA();
            GLCommonReportDA glDa = new GLCommonReportDA();

            List<ProfitNLossReportViewBO> profitNLossBO = new List<ProfitNLossReportViewBO>();
            List<CashFlowStatReportViewBO> cashFlowBO = new List<CashFlowStatReportViewBO>();

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();

            if (reportType == "CashFlow")
            {
                cashFlowBO = reportDA.GetCashFlowStatInfoForComparson(FromDate, ToDate, FromDate2, ToDate2, companyId, projectId, donorId, projectUrl, withOrWithoutOpening);

                List<CashFlowStatReportViewBO> cashFlow = new List<CashFlowStatReportViewBO>();
                List<CashFlowStatReportViewBO> cashFlowSummary = new List<CashFlowStatReportViewBO>();

                cashFlow = cashFlowBO.Where(c => c.GroupId < 4).ToList();
                cashFlowSummary = cashFlowBO.Where(c => c.GroupId >= 4).ToList();

                foreach (CashFlowStatReportViewBO r in cashFlow)
                {
                    if (!string.IsNullOrEmpty(r.NotesNumber) && r.ParentNodeId != null)
                    {
                        if (!string.IsNullOrEmpty(notesNodesId))
                            notesNodesId += "," + r.ParentNodeId.ToString();
                        else
                            notesNodesId = r.ParentNodeId.ToString();
                    }

                    if (ConversionRate != 1)
                    {
                        if (r.DRAmount != 0)
                        {
                            r.DRAmount = r.DRAmount / ConversionRate;
                        }

                        if (r.DRAmountPreviousYear != 0)
                        {
                            r.DRAmountPreviousYear = r.DRAmountPreviousYear / ConversionRate;
                        }

                        if (r.DRAmountCurrentYear != 0)
                        {
                            r.DRAmountCurrentYear = r.DRAmountCurrentYear / ConversionRate;
                        }

                        if (r.CRAmount != 0)
                        {
                            r.CRAmount = r.CRAmount / ConversionRate;
                        }

                        if (r.CRAmountPreviousYear != 0)
                        {
                            r.CRAmountPreviousYear = r.CRAmountPreviousYear / ConversionRate;
                        }

                        if (r.CRAmountCurrentYear != 0)
                        {
                            r.CRAmountCurrentYear = r.CRAmountCurrentYear / ConversionRate;
                        }

                        if (r.Balance != 0)
                        {
                            r.Balance = r.Balance / ConversionRate;
                        }

                        if (r.BalancePreviousYear != 0)
                        {
                            r.BalancePreviousYear = r.BalancePreviousYear / ConversionRate;
                        }

                        if (r.BalanceCurrentYear != 0)
                        {
                            r.BalanceCurrentYear = r.BalanceCurrentYear / ConversionRate;
                        }
                    }
                }

                foreach (CashFlowStatReportViewBO r in cashFlowSummary)
                {
                    if (!string.IsNullOrEmpty(r.NotesNumber) && r.ParentNodeId != null)
                    {
                        if (!string.IsNullOrEmpty(notesNodesId))
                            notesNodesId += "," + r.ParentNodeId.ToString();
                        else
                            notesNodesId = r.ParentNodeId.ToString();
                    }

                    if (ConversionRate != 1)
                    {
                        if (r.DRAmount != 0)
                        {
                            r.DRAmount = r.DRAmount / ConversionRate;
                        }

                        if (r.DRAmountPreviousYear != 0)
                        {
                            r.DRAmountPreviousYear = r.DRAmountPreviousYear / ConversionRate;
                        }

                        if (r.DRAmountCurrentYear != 0)
                        {
                            r.DRAmountCurrentYear = r.DRAmountCurrentYear / ConversionRate;
                        }

                        if (r.CRAmount != 0)
                        {
                            r.CRAmount = r.CRAmount / ConversionRate;
                        }

                        if (r.CRAmountPreviousYear != 0)
                        {
                            r.CRAmountPreviousYear = r.CRAmountPreviousYear / ConversionRate;
                        }

                        if (r.CRAmountCurrentYear != 0)
                        {
                            r.CRAmountCurrentYear = r.CRAmountCurrentYear / ConversionRate;
                        }

                        if (r.Balance != 0)
                        {
                            r.Balance = r.Balance / ConversionRate;
                        }

                        if (r.BalancePreviousYear != 0)
                        {
                            r.BalancePreviousYear = r.BalancePreviousYear / ConversionRate;
                        }

                        if (r.BalanceCurrentYear != 0)
                        {
                            r.BalanceCurrentYear = r.BalanceCurrentYear / ConversionRate;
                        }
                    }
                }

                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], cashFlow));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[1], cashFlowSummary));
                displayName = "Cash Flow Statement";
            }
            else if (reportType == "ProfitLoss")
            {
                if (isProfitableOrganization)
                    profitNLossBO = reportDA.GetProfitNLossStatementComparison(FromDate, ToDate, FromDate2, ToDate2, incomeTaxPercentage, companyId, projectId, donorId, projectUrl, withOrWithoutOpening);
                else
                    profitNLossBO = reportDA.GetProfitNLossStatementInfoForNonProfitOrganization(FromDate, ToDate, incomeTaxPercentage, companyId, projectId, donorId, projectUrl, withOrWithoutOpening);

                foreach (ProfitNLossReportViewBO r in profitNLossBO)
                {
                    if (!string.IsNullOrEmpty(r.Notes) && r.NodeId != null)
                    {
                        if (!string.IsNullOrEmpty(notesNodesId))
                            notesNodesId += "," + r.NodeId.ToString();
                        else
                            notesNodesId = r.NodeId.ToString();
                    }

                    if (ConversionRate != 1)
                    {
                        if (r.Amount != 0)
                        {
                            r.Amount = r.Amount / ConversionRate;
                        }

                        if (r.AmountPreviousYear != 0)
                        {
                            r.AmountPreviousYear = r.AmountPreviousYear / ConversionRate;
                        }

                        if (r.AmountCurrentYear != 0)
                        {
                            r.AmountCurrentYear = r.AmountCurrentYear / ConversionRate;
                        }

                        if (r.AmountToDisplay != 0)
                        {
                            r.AmountToDisplay = r.AmountToDisplay / ConversionRate;
                        }

                        if (r.AmountToDisplayPreviousYear != 0)
                        {
                            r.AmountToDisplayPreviousYear = r.AmountToDisplayPreviousYear / ConversionRate;
                        }

                        if (r.AmountToDisplayCurrentYear != 0)
                        {
                            r.AmountToDisplayCurrentYear = r.AmountToDisplayCurrentYear / ConversionRate;
                        }
                    }
                }

                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], profitNLossBO));
                displayName = "Profit and Loss Statement";
            }
            else if (reportType == "ReceiptNPaymentStatement")
            {
                ReceiptNPaymentStatementViewBO receiptNPayment = new ReceiptNPaymentStatementViewBO();
                receiptNPayment = glDa.GetReceiptNPaymentStatementComparison(FromDate, ToDate, FromDate2, ToDate2, companyId, projectId, donorId, string.Empty, withOrWithoutOpening);

                foreach (ReceiptNPaymentStatementBO r in receiptNPayment.ReceiptStatement)
                {
                    if (!string.IsNullOrEmpty(r.NotesNumber) && r.NodeId != null)
                    {
                        if (!string.IsNullOrEmpty(notesNodesId))
                            notesNodesId += "," + r.NodeId.ToString();
                        else
                            notesNodesId = r.NodeId.ToString();
                    }

                    if (ConversionRate != 1)
                    {
                        if (r.TransactionalBalance != 0)
                        {
                            r.TransactionalBalance = r.TransactionalBalance / ConversionRate;
                        }

                        if (r.TransactionalOpening != 0)
                        {
                            r.TransactionalOpening = r.TransactionalOpening / ConversionRate;
                        }

                        if (r.OpeningBalance != 0)
                        {
                            r.OpeningBalance = r.OpeningBalance / ConversionRate;
                        }

                        if (r.Amount != 0)
                        {
                            r.Amount = r.Amount / ConversionRate;
                        }

                        if (r.GroupAmount != 0)
                        {
                            r.GroupAmount = r.GroupAmount / ConversionRate;
                        }

                        if (r.GrandAmount != 0)
                        {
                            r.GrandAmount = r.GrandAmount / ConversionRate;
                        }

                        if (r.NetIncreasesDecreasedInCashNCashEquvalent != 0)
                        {
                            r.NetIncreasesDecreasedInCashNCashEquvalent = r.NetIncreasesDecreasedInCashNCashEquvalent / ConversionRate;
                        }

                        if (r.CashNCashEquvalentAtEndOfPeriod != 0)
                        {
                            r.CashNCashEquvalentAtEndOfPeriod = r.CashNCashEquvalentAtEndOfPeriod / ConversionRate;
                        }
                        
                        //----------- For Comparision Report
                        if (r.OpeningBalanceCurrentYear != 0)
                        {
                            r.OpeningBalanceCurrentYear = r.OpeningBalanceCurrentYear / ConversionRate;
                        }

                        if (r.AmountCurrentYear != 0)
                        {
                            r.AmountCurrentYear = r.AmountCurrentYear / ConversionRate;
                        }

                        if (r.GroupAmountCurrentYear != 0)
                        {
                            r.GroupAmountCurrentYear = r.GroupAmountCurrentYear / ConversionRate;
                        }

                        if (r.GrandAmountCurrentYear != 0)
                        {
                            r.GrandAmountCurrentYear = r.GrandAmountCurrentYear / ConversionRate;
                        }

                        if (r.NetIncreasesDecreasedInCashNCashEquvalentCurrentYear != 0)
                        {
                            r.NetIncreasesDecreasedInCashNCashEquvalentCurrentYear = r.NetIncreasesDecreasedInCashNCashEquvalentCurrentYear / ConversionRate;
                        }

                        if (r.CashNCashEquvalentAtEndOfPeriodCurrentYear != 0)
                        {
                            r.CashNCashEquvalentAtEndOfPeriodCurrentYear = r.CashNCashEquvalentAtEndOfPeriodCurrentYear / ConversionRate;
                        }

                        if (r.OpeningBalancePreviousYear != 0)
                        {
                            r.OpeningBalancePreviousYear = r.OpeningBalancePreviousYear / ConversionRate;
                        }

                        if (r.AmountPreviousYear != 0)
                        {
                            r.AmountPreviousYear = r.AmountPreviousYear / ConversionRate;
                        }

                        if (r.GroupAmountPreviousYear != 0)
                        {
                            r.GroupAmountPreviousYear = r.GroupAmountPreviousYear / ConversionRate;
                        }

                        if (r.GrandAmountPreviousYear != 0)
                        {
                            r.GrandAmountPreviousYear = r.GrandAmountPreviousYear / ConversionRate;
                        }

                        if (r.NetIncreasesDecreasedInCashNCashEquvalentPreviousYear != 0)
                        {
                            r.NetIncreasesDecreasedInCashNCashEquvalentPreviousYear = r.NetIncreasesDecreasedInCashNCashEquvalentPreviousYear / ConversionRate;
                        }

                        if (r.CashNCashEquvalentAtEndOfPeriodPreviousYear != 0)
                        {
                            r.CashNCashEquvalentAtEndOfPeriodPreviousYear = r.CashNCashEquvalentAtEndOfPeriodPreviousYear / ConversionRate;
                        }
                    }
                }

                foreach (ReceiptNPaymentStatementBO p in receiptNPayment.PaymentStatement)
                {
                    if (!string.IsNullOrEmpty(p.NotesNumber) && p.NodeId != null)
                    {
                        if (!string.IsNullOrEmpty(notesNodesId))
                            notesNodesId += "," + p.NodeId.ToString();
                        else
                            notesNodesId = p.NodeId.ToString();
                    }

                    if (ConversionRate != 1)
                    {
                        if (p.TransactionalBalance != 0)
                        {
                            p.TransactionalBalance = p.TransactionalBalance / ConversionRate;
                        }

                        if (p.TransactionalOpening != 0)
                        {
                            p.TransactionalOpening = p.TransactionalOpening / ConversionRate;
                        }

                        if (p.OpeningBalance != 0)
                        {
                            p.OpeningBalance = p.OpeningBalance / ConversionRate;
                        }

                        if (p.Amount != 0)
                        {
                            p.Amount = p.Amount / ConversionRate;
                        }

                        if (p.GroupAmount != 0)
                        {
                            p.GroupAmount = p.GroupAmount / ConversionRate;
                        }

                        if (p.GrandAmount != 0)
                        {
                            p.GrandAmount = p.GrandAmount / ConversionRate;
                        }

                        if (p.NetIncreasesDecreasedInCashNCashEquvalent != 0)
                        {
                            p.NetIncreasesDecreasedInCashNCashEquvalent = p.NetIncreasesDecreasedInCashNCashEquvalent / ConversionRate;
                        }

                        if (p.CashNCashEquvalentAtEndOfPeriod != 0)
                        {
                            p.CashNCashEquvalentAtEndOfPeriod = p.CashNCashEquvalentAtEndOfPeriod / ConversionRate;
                        }

                        //----------- For Comparision Report
                        if (p.OpeningBalanceCurrentYear != 0)
                        {
                            p.OpeningBalanceCurrentYear = p.OpeningBalanceCurrentYear / ConversionRate;
                        }

                        if (p.AmountCurrentYear != 0)
                        {
                            p.AmountCurrentYear = p.AmountCurrentYear / ConversionRate;
                        }

                        if (p.GroupAmountCurrentYear != 0)
                        {
                            p.GroupAmountCurrentYear = p.GroupAmountCurrentYear / ConversionRate;
                        }

                        if (p.GrandAmountCurrentYear != 0)
                        {
                            p.GrandAmountCurrentYear = p.GrandAmountCurrentYear / ConversionRate;
                        }

                        if (p.NetIncreasesDecreasedInCashNCashEquvalentCurrentYear != 0)
                        {
                            p.NetIncreasesDecreasedInCashNCashEquvalentCurrentYear = p.NetIncreasesDecreasedInCashNCashEquvalentCurrentYear / ConversionRate;
                        }

                        if (p.CashNCashEquvalentAtEndOfPeriodCurrentYear != 0)
                        {
                            p.CashNCashEquvalentAtEndOfPeriodCurrentYear = p.CashNCashEquvalentAtEndOfPeriodCurrentYear / ConversionRate;
                        }

                        if (p.OpeningBalancePreviousYear != 0)
                        {
                            p.OpeningBalancePreviousYear = p.OpeningBalancePreviousYear / ConversionRate;
                        }

                        if (p.AmountPreviousYear != 0)
                        {
                            p.AmountPreviousYear = p.AmountPreviousYear / ConversionRate;
                        }

                        if (p.GroupAmountPreviousYear != 0)
                        {
                            p.GroupAmountPreviousYear = p.GroupAmountPreviousYear / ConversionRate;
                        }

                        if (p.GrandAmountPreviousYear != 0)
                        {
                            p.GrandAmountPreviousYear = p.GrandAmountPreviousYear / ConversionRate;
                        }

                        if (p.NetIncreasesDecreasedInCashNCashEquvalentPreviousYear != 0)
                        {
                            p.NetIncreasesDecreasedInCashNCashEquvalentPreviousYear = p.NetIncreasesDecreasedInCashNCashEquvalentPreviousYear / ConversionRate;
                        }

                        if (p.CashNCashEquvalentAtEndOfPeriodPreviousYear != 0)
                        {
                            p.CashNCashEquvalentAtEndOfPeriodPreviousYear = p.CashNCashEquvalentAtEndOfPeriodPreviousYear / ConversionRate;
                        }
                    }
                }

                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], receiptNPayment.ReceiptStatement));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[1], receiptNPayment.PaymentStatement));
                displayName = "Receipt and Payment Statement";
            }
            else if (reportType == "BalanceSheet")
            {
                DateTime DateFromBefore = DateTime.Now, DateToBefore = DateTime.Now;

                if (FromDate2 == null || ToDate2 == null)
                {
                    DateFromBefore = FromDate.AddYears(-1);
                    DateToBefore = ToDate.AddYears(-1);
                }
                else
                {
                    DateFromBefore = Convert.ToDateTime(FromDate2);
                    DateToBefore = Convert.ToDateTime(ToDate2);
                }

                List<BalanceSheetStatementInfoForReportBO> balanceSheet = new List<BalanceSheetStatementInfoForReportBO>();
                List<BalanceSheetStatementInfoForReportBO> balanceSheetCurrentYear = new List<BalanceSheetStatementInfoForReportBO>();
                List<BalanceSheetStatementInfoForReportBO> balanceSheetPreviousYear = new List<BalanceSheetStatementInfoForReportBO>();

                balanceSheetPreviousYear = glDa.GetBalanceSheetStatementInfo(DateFromBefore, DateToBefore, companyId, projectId, donorId, url, withOrWithoutOpening, ddlSearchType.SelectedValue);
                balanceSheetCurrentYear = glDa.GetBalanceSheetStatementInfo(FromDate, ToDate, companyId, projectId, donorId, url, withOrWithoutOpening, ddlSearchType.SelectedValue);

                balanceSheet.AddRange((from a in balanceSheetCurrentYear
                                       select new BalanceSheetStatementInfoForReportBO
                                       {
                                           ParentGroupId = a.ParentGroupId,
                                           ParentGroup = a.ParentGroup,
                                           GroupId = a.GroupId,
                                           Particulars = a.Particulars,
                                           NodeId = a.NodeId,
                                           Notes = a.Notes,
                                           Amount = a.Amount,
                                           Lvl = a.Lvl,
                                           AmountCurrentYear = a.Amount,
                                           AmountPreviousYear = 0,
                                           CurrentYearDateFrom = FromDate,
                                           CurrentYearDateTo = ToDate,
                                           PreviousYearDateFrom = DateFromBefore,
                                           PreviousYearDateTo = DateToBefore
                                       }).ToList());


                foreach (BalanceSheetStatementInfoForReportBO b in balanceSheet)
                {
                    b.AmountPreviousYear = (from p in balanceSheetPreviousYear where p.NodeId == b.NodeId select p.Amount).FirstOrDefault();
                }

                foreach (BalanceSheetStatementInfoForReportBO b in balanceSheet)
                {
                    if (!string.IsNullOrEmpty(b.Notes) && b.NodeId != null && b.NodeId > 0)
                    {
                        if (!string.IsNullOrEmpty(notesNodesId))
                            notesNodesId += "," + b.NodeId.ToString();
                        else
                            notesNodesId = b.NodeId.ToString();
                    }

                    if (ConversionRate != 1)
                    {
                        if (b.Amount != 0)
                        {
                            b.Amount = b.Amount / ConversionRate;
                        }

                        if (b.AmountPreviousYear != 0)
                        {
                            b.AmountPreviousYear = b.AmountPreviousYear / ConversionRate;
                        }

                        if (b.AmountCurrentYear != 0)
                        {
                            b.AmountCurrentYear = b.AmountCurrentYear / ConversionRate;
                        }
                    }
                }

                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], balanceSheet));
                displayName = "Balance Sheet";
            }

            rvTransaction.LocalReport.DisplayName = displayName;
            rvTransaction.LocalReport.Refresh();

            reportSearchType = ddlSearchType.SelectedValue;
            hfFiscalId1 = ddlFiscalYear.SelectedValue;
            hfFiscalId2 = ddlFiscalYear2.SelectedValue;
            hfNotesNodes.Value = notesNodesId;

            frmPrint.Attributes["src"] = "";
        }
        private void GenerateCompanyLedgerReport()
        {
            _RoomStatusInfoByDate = 1;
            HMCommonDA hmCommonDA = new HMCommonDA();
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = "";
            
            reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/RptCompanyLedgerComparison.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            string paymentStatus = string.Empty, reportType = string.Empty;

            string startDate = string.Empty, endDate = string.Empty;
            string startDate2 = string.Empty, endDate2 = string.Empty;

            DateTime dateTime = DateTime.Now;

            if (string.IsNullOrWhiteSpace(txtStartDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtStartDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtStartDate.Text;
            }
            if (string.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtEndDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = txtEndDate.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtStartDate2.Text))
            {
                startDate2 = txtStartDate2.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtEndDate2.Text))
            {
                endDate2 = txtEndDate2.Text;
            }

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            DateTime FromDate2 = hmUtility.GetDateTimeFromString(startDate2, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate2 = hmUtility.GetDateTimeFromString(endDate2, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            if (!string.IsNullOrEmpty(startDate2))
            {
                FromDate2 = hmUtility.GetDateTimeFromString(startDate2, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            if (!string.IsNullOrEmpty(endDate2))
            {
                ToDate2 = hmUtility.GetDateTimeFromString(endDate2, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);
            }      

            int companyId = 0, projectId = 0, donorId = 0;
            string companyName = string.Empty;
            string companyAddress = string.Empty;
            string webAddress = string.Empty;
            Int32 glCompanyId = companyId;
            
            //-- Company Logo -------------------------------
            string imageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            string projectName = "All";

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            //List<ReportParameter> paramReport = new List<ReportParameter>();
            List<ReportParameter> reportParam = new List<ReportParameter>();

            if (files[0].CompanyId > 0)
            {
                companyName = files[0].CompanyName;
                companyAddress = files[0].CompanyAddress;

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    webAddress = files[0].WebAddress;
                }
                else
                {
                    webAddress = files[0].ContactNumber;
                }
            }

            //bool isProfitableOrganization = true;

            //if (hfCompanyIsProfitable.Value == "1")
            //    isProfitableOrganization = true;

            if (hfCompanyId.Value != "0" && hfCompanyId.Value != "")
            {
                companyId = Convert.ToInt32(hfCompanyId.Value);
                GLCompanyBO glCompanyBO = new GLCompanyBO();
                GLCompanyDA glCompanyDA = new GLCompanyDA();
                glCompanyBO = glCompanyDA.GetGLCompanyInfoById(companyId);
                if (glCompanyBO != null)
                {
                    if (glCompanyBO.CompanyId > 0)
                    {
                        glCompanyId = glCompanyBO.CompanyId;
                        companyName = glCompanyBO.Name;
                        if (!string.IsNullOrWhiteSpace(glCompanyBO.CompanyAddress))
                        {
                            companyAddress = glCompanyBO.CompanyAddress;
                        }
                        if (!string.IsNullOrWhiteSpace(glCompanyBO.WebAddress))
                        {
                            webAddress = glCompanyBO.WebAddress;
                        }
                        if (!string.IsNullOrWhiteSpace(glCompanyBO.ImageName))
                        {
                            imageName = glCompanyBO.ImageName;
                        }
                    }
                }
            }

            if (hfProjectId.Value != "0" && hfProjectId.Value != "")
            {
                projectId = Convert.ToInt32(hfProjectId.Value);
                projectName = hfProjectId.Value != "0" ? hfProjectName.Value : "All";
            }

            reportParam.Add(new ReportParameter("CompanyProfile", companyName));
            reportParam.Add(new ReportParameter("CompanyAddress", companyAddress));
            reportParam.Add(new ReportParameter("CompanyWeb", webAddress));

            paymentStatus = "0";
            reportType = "0";

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            reportParam.Add(new ReportParameter("CompanyName", companyName));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));

            string notesNo = string.Empty;
            string notesHead = string.Empty;
            string companyLedgerNotesHead = string.Empty;

            NodeMatrixBO companyMatrixBO = new NodeMatrixBO();
            NodeMatrixDA companyMatrixDA = new NodeMatrixDA();
            companyMatrixBO = companyMatrixDA.GetNodeMatrixInfoById(17);
            if (companyMatrixBO != null)
            {
                notesNo = companyMatrixBO.NotesNumber;
                notesHead = companyMatrixBO.NodeHead;
            }

            List<CommonPaymentModeBO> commonPaymentModeBOList = new List<CommonPaymentModeBO>();
            commonPaymentModeBOList = hmCommonDA.GetCommonPaymentModeInfo("All");
            CommonPaymentModeBO companyPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Company").FirstOrDefault();
            if (companyPaymentModeInfo != null)
            {
                int companyAccountsPostingId;
                companyAccountsPostingId = companyPaymentModeInfo.PaymentAccountsPostingId;

                companyMatrixBO = companyMatrixDA.GetNodeMatrixInfoById(companyAccountsPostingId);
                if (companyMatrixBO != null)
                {
                    companyLedgerNotesHead = companyMatrixBO.NodeHead;
                }
            }

            reportParam.Add(new ReportParameter("ReportDateFrom", hmUtility.GetStringFromDateTime(FromDate)));
            reportParam.Add(new ReportParameter("ReportDateTo", hmUtility.GetStringFromDateTime(ToDate)));

            reportParam.Add(new ReportParameter("ReportDateFrom2", hmUtility.GetStringFromDateTime(FromDate2)));
            reportParam.Add(new ReportParameter("ReportDateTo2", hmUtility.GetStringFromDateTime(ToDate2)));

            reportParam.Add(new ReportParameter("NotesNo", notesNo));
            reportParam.Add(new ReportParameter("NotesHead", notesHead));
            reportParam.Add(new ReportParameter("CompanyLedgerNotesHead", companyLedgerNotesHead));

            string searchNarration = string.Empty;
            string fromAmount = string.Empty;
            string toAmount = string.Empty;
            string notesNodesId = string.Empty;

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            rvTransaction.LocalReport.SetParameters(reportParam);

            GuestCompanyDA commonReportDa = new GuestCompanyDA();
            List<CompanyPaymentLedgerReportVwBo> companyLedger = new List<CompanyPaymentLedgerReportVwBo>();
            List<CompanyPaymentLedgerReportVwBo> companyLedger1 = new List<CompanyPaymentLedgerReportVwBo>();
            List<CompanyPaymentLedgerReportVwBo> companyLedger2 = new List<CompanyPaymentLedgerReportVwBo>();
            glCompanyId = 0;
            companyId = 0;

            // ------ Date 1
            companyLedger1 = commonReportDa.GetCompanyLedger(userInformationBO.UserInfoId, glCompanyId, companyId, FromDate, ToDate, paymentStatus, reportType, searchNarration, fromAmount, toAmount);

            // ------ Date 2
            companyLedger2 = commonReportDa.GetCompanyLedger(userInformationBO.UserInfoId, glCompanyId, companyId, FromDate2, ToDate2, paymentStatus, reportType, searchNarration, fromAmount, toAmount);

            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> guestCompanyBOList = new List<GuestCompanyBO>();
            guestCompanyBOList = guestCompanyDA.GetALLGuestCompanyInfo();

            foreach (GuestCompanyBO row in guestCompanyBOList)
            {
                CompanyPaymentLedgerReportVwBo ledgerReportBO = new CompanyPaymentLedgerReportVwBo();
                ledgerReportBO.CompanyId = row.CompanyId;
                ledgerReportBO.CompanyName = row.CompanyName;
                ledgerReportBO.PreviousYearClosingBalance = (from p in companyLedger1 where p.CompanyId == row.CompanyId select p.ClosingBalance).ToList().Sum();
                ledgerReportBO.CurrentYearClosingBalance = (from p in companyLedger2 where p.CompanyId == row.CompanyId select p.ClosingBalance).ToList().Sum();
                companyLedger.Add(ledgerReportBO);
            }

            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], companyLedger));

            rvTransaction.LocalReport.DisplayName = "Company Ledger";
            rvTransaction.LocalReport.Refresh();

            reportSearchType = ddlSearchType.SelectedValue;
            hfFiscalId1 = ddlFiscalYear.SelectedValue;
            hfFiscalId2 = ddlFiscalYear2.SelectedValue;
            hfNotesNodes.Value = notesNodesId;

            frmPrint.Attributes["src"] = "";            
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
            CommonDropDownHiddenFieldForPleaseSelect.Value = hmUtility.GetDropDownFirstValue();
        }
        
        public string SplitPageName(string url)
        {
            string[] words = url.Split('/');
            int length = words.Length;
            for (int i = 0; i < length - 1; i++)
            {
                FinalString = FinalString + words[i] + '/';
            }
            return FinalString;
        }
        
        private void LoadFiscalYear()
        {
            List<GLFiscalYearBO> fiscalYearList = new List<GLFiscalYearBO>();
            GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
            fiscalYearList = fiscalYearDA.GetAllFiscalYear();

            ddlFiscalYear.DataSource = fiscalYearList;
            ddlFiscalYear.DataTextField = "FiscalYearName";
            ddlFiscalYear.DataValueField = "FiscalYearId";
            ddlFiscalYear.DataBind();

            ddlFiscalYear2.DataSource = fiscalYearList;
            ddlFiscalYear2.DataTextField = "FiscalYearName";
            ddlFiscalYear2.DataValueField = "FiscalYearId";
            ddlFiscalYear2.DataBind();

            System.Web.UI.WebControls.ListItem itemProject = new System.Web.UI.WebControls.ListItem();
            itemProject.Value = "0";
            itemProject.Text = hmUtility.GetDropDownFirstValue();

            ddlFiscalYear.Items.Insert(0, itemProject);
            ddlFiscalYear2.Items.Insert(0, itemProject);
        }
        private bool IsFormValid()
        {
            bool status = true;
            if (isSingle == false)
            {
                //if (ddlGLProject.SelectedIndex == 0)
                //{
                //    isMessageBoxEnable = 1;
                //    lblMessage.Text = "Please Select Project Name";
                //    status = false;
                //}
            }
            return status;
        }
    }
}