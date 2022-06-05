using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using iTextSharp.text;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.GeneralLedger.Reports
{
    public partial class frmReportTrailBalanceStatement : System.Web.UI.Page
    {
        protected int _IsReportPanelEnable = -1;
        protected string reportSearchType = "0";
        protected string hfFiscalId = "0";
        protected int isMessageBoxEnable = -1;
        HMUtility hmUtility = new HMUtility();
        protected bool isSingle = true;
        protected int isCompanyProjectPanelEnable = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
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
            string startDate = string.Empty, endDate = string.Empty;
            string withOrWithoutOpening = string.Empty;

            DateTime dateTime = DateTime.Now;

            withOrWithoutOpening = dllWithOrWithoutOpening.SelectedValue;

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

            if (this.ddlSearchType.SelectedValue == "1")
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
            string showZeroTransaction = string.Empty;
            string companyName = string.Empty;
            string companyAddress = string.Empty;
            string webAddress = string.Empty;
            //-- Company Logo -------------------------------
            string imageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            string projectName = string.Empty;

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

            if (ddlShowTransaction.SelectedValue != "0" || ddlShowTransaction.SelectedValue != "")
            {
                showZeroTransaction = ddlShowTransaction.SelectedValue;
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";

            if (ddlReportType.SelectedValue == "SummaryReport")
            {
                reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/RptTrailBalanceStatement.rdlc");
            }
            if (ddlReportType.SelectedValue == "DetailsReport")
            {
                reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/RptTrialBalanceDetails.rdlc");
            }

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
            paramReport.Add(new ReportParameter("ReportDateFrom", FromDate.ToString("dd-MMM-yyyy")));
            paramReport.Add(new ReportParameter("ReportDateTo", ToDate.ToString("dd-MMM-yyyy")));
            paramReport.Add(new ReportParameter("CompanyName", companyName));
            paramReport.Add(new ReportParameter("CompanyProject", projectName));

            GLReportDA reportDA = new GLReportDA();
            List<TrialBalanceReportViewBO> trialBalanceBO = new List<TrialBalanceReportViewBO>();

            if (ddlSearchType.SelectedValue == "1")
            {
                if (ddlReportType.SelectedValue == "SummaryReport")
                {
                    trialBalanceBO = reportDA.GetTrialBalanceInfo(FromDate, ToDate, companyId, projectId, donorId, showZeroTransaction, withOrWithoutOpening);
                }
                else if (ddlReportType.SelectedValue == "DetailsReport")
                {
                    trialBalanceBO = reportDA.GetTrialBalanceDetails(FromDate, ToDate, companyId, projectId, donorId, showZeroTransaction, withOrWithoutOpening);
                }
            }
            else
            {
                if (ddlReportType.SelectedValue == "SummaryReport")
                {
                    trialBalanceBO = reportDA.GetTrialBalanceInfoDateRangeWise(FromDate, ToDate, companyId, projectId, donorId, showZeroTransaction, withOrWithoutOpening);
                }
                else if (ddlReportType.SelectedValue == "DetailsReport")
                {
                    trialBalanceBO = reportDA.GetTrialBalanceDetailsDaterangeWise(FromDate, ToDate, companyId, projectId, donorId, showZeroTransaction, withOrWithoutOpening);
                }
            }

            // // // ------- Multi Currency Related Effects -------------------------- Start
            decimal ConversionRate = 1;
            ConversionRate = Convert.ToDecimal(ddlCurrencyId.SelectedValue);
            if (ConversionRate != 1)
            {
                paramReport.Add(new ReportParameter("ReportCurrency", ddlCurrencyId.SelectedItem.Text + " (C.R: " + ddlCurrencyId.SelectedValue + ")"));
            }
            else
            {
                paramReport.Add(new ReportParameter("ReportCurrency", ddlCurrencyId.SelectedItem.Text));
            }

            foreach (TrialBalanceReportViewBO row in trialBalanceBO)
            {
                if (ConversionRate != 1)
                {
                    if (row.OpeningBalance != 0)
                    {
                        row.OpeningBalance = row.OpeningBalance / ConversionRate;
                    }

                    if (row.DRAmount != 0)
                    {
                        row.DRAmount = row.DRAmount / ConversionRate;
                    }

                    if (row.CRAmount != 0)
                    {
                        row.CRAmount = row.CRAmount / ConversionRate;
                    }

                    if (row.ClosingBalance != 0)
                    {
                        row.ClosingBalance = row.ClosingBalance / ConversionRate;
                    }
                }                
            }
            // // // ------- Multi Currency Related Effects -------------------------- End

            rvTransaction.LocalReport.SetParameters(paramReport);
            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], trialBalanceBO));

            rvTransaction.LocalReport.DisplayName = "Trial Balance Statement";
            rvTransaction.LocalReport.Refresh();

            reportSearchType = ddlSearchType.SelectedValue;
            hfFiscalId = hfFiscalYear.Value;

            frmPrint.Attributes["src"] = "";
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
            CommonDropDownHiddenFieldForPleaseSelect.Value = hmUtility.GetDropDownFirstValue();
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

            System.Web.UI.WebControls.ListItem itemProject = new System.Web.UI.WebControls.ListItem();
            itemProject.Value = "0";
            itemProject.Text = hmUtility.GetDropDownFirstValue();
            ddlFiscalYear.Items.Insert(0, itemProject);
        }
        private bool IsFormValid()
        {
            bool status = true;
            if (isSingle == false)
            {
                //if (this.ddlGLProject.SelectedIndex == 0)
                //{
                //    isMessageBoxEnable = 1;
                //    //lblMessage.Text = "Please Select Project Name";
                //    status = false;
                //}
            }

            return status;
        }
    }
}