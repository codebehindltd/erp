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
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.UserInformation;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.GeneralLedger.Reports
{
    public partial class frmReportReceiptNPaymentStatement : System.Web.UI.Page
    {
        protected int isMessageBoxEnable = -1;
        protected string reportSearchType = "0";
        protected string hfFiscalId = "0";
        protected int _IsReportPanelEnable = -1;
        string FinalString = "";
        string url = "";
        HMUtility hmUtility = new HMUtility();
        protected bool isSingle = true;
        protected int isCompanyProjectPanelEnable = -1;
        protected int IsConfigurableTemplateVisible = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            url = HttpContext.Current.Request.Url.AbsoluteUri;
            url = SplitPageName(url);

            if (!IsPostBack)
            {
                Session["GLConfigurableBalanceSheetBOList"] = null;
                LiabilitiesAmountHiddenField.Value = "0";
                AssetsAmountHiddenField.Value = "0";
                LoadCurrencyHead();
                rvTransaction.LocalReport.EnableHyperlinks = true;
                LoadCommonDropDownHiddenField();
                LoadFiscalYear();
                LoadGLDonor();
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
            ListItem itemDonor = new ListItem();
            itemDonor.Value = "0";
            itemDonor.Text = hmUtility.GetDropDownFirstAllValue();
            ddlDonor.Items.Insert(0, itemDonor);

        }
        private void GenerateReport()
        {
            try
            {
                _IsReportPanelEnable = 1;
                HMCommonDA hmCommonDA = new HMCommonDA();

                string startDate = string.Empty, endDate = string.Empty;
                DateTime dateTime = DateTime.Now;
                string withOrWithoutOpening = string.Empty, showZeroTransaction = string.Empty;

                if (ddlShowTransaction.SelectedValue != "0" || ddlShowTransaction.SelectedValue != "")
                {
                    showZeroTransaction = ddlShowTransaction.SelectedValue;
                }

                withOrWithoutOpening = dllWithOrWithoutOpening.SelectedValue;

                if (string.IsNullOrWhiteSpace(txtStartDate.Text))
                {
                    startDate = DateTime.Now.ToString();
                    txtStartDate.Text = hmUtility.GetStringFromDateTime(dateTime);
                }
                else
                {
                    startDate = txtStartDate.Text;
                }
                if (string.IsNullOrWhiteSpace(txtEndDate.Text))
                {
                    endDate = DateTime.Now.ToString();
                    txtEndDate.Text = hmUtility.GetStringFromDateTime(dateTime);
                }
                else
                {
                    endDate = txtEndDate.Text;
                }

                if (ddlSearchType.SelectedValue == "1")
                {
                    int fiscalYearId = Convert.ToInt32(hfFiscalYear.Value);
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

                string urlId = txtUrl.Text.Trim();
                string reportType = txtReportType.Text;

                int companyId = 0, projectId = 0, donorId = 0;
                string companyName = string.Empty;
                string projectName = "All";

                if (hfCompanyId.Value != "0" && hfCompanyId.Value != "")
                {
                    companyId = Convert.ToInt32(hfCompanyId.Value);
                    companyName = hfCompanyName.Value;
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
                reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/RptReceiptNPaymentStatement.rdlc");

                if (!File.Exists(reportPath))
                    return;

                rvTransaction.LocalReport.ReportPath = reportPath;

                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> files = companyDA.GetCompanyInfo();

                List<ReportParameter> paramReport = new List<ReportParameter>();

                if (files[0].CompanyId > 0)
                {
                    paramReport.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                    paramReport.Add(new ReportParameter("CompanyName", companyName));
                    paramReport.Add(new ReportParameter("CompanyProject", projectName));
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

                paramReport.Add(new ReportParameter("ReportDateFrom", startDate));
                paramReport.Add(new ReportParameter("ReportDateTo", endDate));

                //-- Company Logo -------------------------------
                string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
                rvTransaction.LocalReport.EnableExternalImages = true;
                rvTransaction.LocalReport.EnableHyperlinks = true;

                //List<ReportParameter> paramLogo = new List<ReportParameter>();
                paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
                
                GLCommonReportDA glDa = new GLCommonReportDA();
                ReceiptNPaymentStatementViewBO receiptNPayment = new ReceiptNPaymentStatementViewBO();

                if (ddlSearchType.SelectedValue == "1")
                {
                    receiptNPayment = glDa.GetReceiptNPaymentStatement(FromDate, ToDate, companyId, projectId, donorId, urlId, showZeroTransaction, withOrWithoutOpening);
                }
                else
                {
                    receiptNPayment = glDa.GetReceiptNPaymentStatementReportDateRangeWise(FromDate, ToDate, companyId, projectId, donorId, urlId, showZeroTransaction, withOrWithoutOpening);
                }

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

                foreach (ReceiptNPaymentStatementBO r in receiptNPayment.ReceiptStatement)
                {
                    if (!string.IsNullOrEmpty(notesNodesId))
                        notesNodesId += "," + r.NodeId.ToString();
                    else
                        notesNodesId = r.NodeId.ToString();

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
                    }
                }

                foreach (ReceiptNPaymentStatementBO p in receiptNPayment.PaymentStatement)
                {
                    if (!string.IsNullOrEmpty(notesNodesId))
                        notesNodesId += "," + p.NodeId.ToString();
                    else
                        notesNodesId = p.NodeId.ToString();

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
                    }
                }
                // // // ------- Multi Currency Related Effects -------------------------- End

                rvTransaction.LocalReport.SetParameters(paramReport);
                var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], receiptNPayment.ReceiptStatement));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[1], receiptNPayment.PaymentStatement));

                rvTransaction.LocalReport.DisplayName = "Receipt and Payment Statement";
                rvTransaction.LocalReport.Refresh();

                hfNotesNodes.Value = notesNodesId;

                reportSearchType = ddlSearchType.SelectedValue;
                hfFiscalId = hfFiscalYear.Value;
                frmPrint.Attributes["src"] = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

            ListItem itemProject = new ListItem();
            itemProject.Value = "0";
            itemProject.Text = hmUtility.GetDropDownFirstValue();
            ddlFiscalYear.Items.Insert(0, itemProject);
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