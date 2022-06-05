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
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.GeneralLedger.Reports
{
    public partial class frmReportCashFlowStatement : System.Web.UI.Page
    {
        protected int _IsReportPanelEnable = -1;
        protected int isMessageBoxEnable = -1;
        protected string reportSearchType = "0";
        protected string hfFiscalId = "0";
        string FinalString = "";
        string url = "";
        HMUtility hmUtility = new HMUtility();
        protected bool isSingle = true;
        protected int isCompanyProjectPanelEnable = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            //isSingle = hmUtility.GetSingleProjectAndCompany();
            url = HttpContext.Current.Request.Url.AbsoluteUri;
            url = SplitPageName(url);

            if (!IsPostBack)
            {
                LoadCurrencyHead();
                rvTransaction.LocalReport.EnableHyperlinks = true;
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
            ListItem itemDonor = new ListItem();
            itemDonor.Value = "0";
            itemDonor.Text = hmUtility.GetDropDownFirstAllValue();
            ddlDonor.Items.Insert(0, itemDonor);
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
            if (!IsFormValid())
            {
                return;
            }
            _IsReportPanelEnable = 1;
            rvTransaction.LocalReport.EnableHyperlinks = true;

            txtUrl.Text = url;
            string startDate = string.Empty, endDate = string.Empty;
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

            int companyId = 0, projectId = 0, donorId = 0;
            string projectName = "All";

            string companyName = string.Empty;
            string companyAddress = string.Empty;
            string webAddress = string.Empty;
            //-- Company Logo -------------------------------
            string imageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");

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
            reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/RptCashFlowStatement.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
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
            paramReport.Add(new ReportParameter("ReportDateFrom", startDate));
            paramReport.Add(new ReportParameter("ReportDateTo", endDate));
            paramReport.Add(new ReportParameter("CompanyName", companyName));
            paramReport.Add(new ReportParameter("CompanyProject", projectName));
            
            //Activate Session
            SessionNotesBreakDownBO notesBreakDown = new SessionNotesBreakDownBO();
            notesBreakDown.startDate = hmUtility.GetStringFromDateTime(FromDate);
            notesBreakDown.endDate = hmUtility.GetStringFromDateTime(ToDate.AddDays(1).AddSeconds(-1));
            notesBreakDown.companyName = files[0].CompanyName;
            notesBreakDown.companyWeb = files[0].WebAddress;
            notesBreakDown.glProject = hfProjectId.Value;
            Session["notesBreakDown"] = notesBreakDown;

            string projectUrl = txtUrl.Text;
            string withOrWithoutOpening = string.Empty;
            withOrWithoutOpening = dllWithOrWithoutOpening.SelectedValue;

            GLReportDA reportDA = new GLReportDA();
            List<CashFlowStatReportViewBO> cashFlowBO = new List<CashFlowStatReportViewBO>();

            if (ddlSearchType.SelectedValue == "1")
            {
                cashFlowBO = reportDA.GetCashFlowStatInfo(FromDate, ToDate, companyId, projectId, donorId, projectUrl, withOrWithoutOpening);
            }
            else
            {
                cashFlowBO = reportDA.GetCashFlowStatementInfoForReportDateRangeWise(FromDate, ToDate, companyId, projectId, donorId, projectUrl, withOrWithoutOpening);
            }

            List<CashFlowStatReportViewBO> cashFlow = new List<CashFlowStatReportViewBO>();
            List<CashFlowStatReportViewBO> cashFlowSummary = new List<CashFlowStatReportViewBO>();

            cashFlow = cashFlowBO.Where(c => c.GroupId < 4).ToList();
            cashFlowSummary = cashFlowBO.Where(c => c.GroupId >= 4).ToList();

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

            foreach (CashFlowStatReportViewBO row in cashFlow)
            {
                if (!string.IsNullOrEmpty(notesNodesId))
                    notesNodesId += "," + row.ParentNodeId.ToString();
                else
                    notesNodesId = row.ParentNodeId.ToString();

                if (ConversionRate != 1)
                {
                    if (row.DRAmount != 0)
                    {
                        row.DRAmount = row.DRAmount / ConversionRate;
                    }

                    if (row.CRAmount != 0)
                    {
                        row.CRAmount = row.CRAmount / ConversionRate;
                    }

                    if (row.Balance != 0)
                    {
                        row.Balance = row.Balance / ConversionRate;
                    }
                }
            }

            foreach (CashFlowStatReportViewBO row in cashFlowSummary)
            {
                if (ConversionRate != 1)
                {
                    if (row.DRAmount != 0)
                    {
                        row.DRAmount = row.DRAmount / ConversionRate;
                    }

                    if (row.CRAmount != 0)
                    {
                        row.CRAmount = row.CRAmount / ConversionRate;
                    }

                    if (row.Balance != 0)
                    {
                        row.Balance = row.Balance / ConversionRate;
                    }
                }
            }
            // // // ------- Multi Currency Related Effects -------------------------- End

            rvTransaction.LocalReport.SetParameters(paramReport);
            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], cashFlow));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[1], cashFlowSummary));

            rvTransaction.LocalReport.DisplayName = "Cash Flow Statement";
            rvTransaction.LocalReport.Refresh();

            reportSearchType = ddlSearchType.SelectedValue;
            hfFiscalId = hfFiscalYear.Value;
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