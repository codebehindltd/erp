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
using HotelManagement.Entity.GeneralLedger;
using System.Web.Services;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.GeneralLedger.Reports
{
    public partial class frmReportJournalRegister : System.Web.UI.Page
    {
        protected int isMessageBoxEnable = -1;
        protected string reportSearchType = "0";
        protected string hfFiscalId = "0";
        protected int _IsReportPanelEnable = -1;
        HMUtility hmUtility = new HMUtility();
        protected bool isSingle = true;
        protected int isCompanyProjectPanelEnable = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            //isSingle = hmUtility.GetSingleProjectAndCompany();
            if (!IsPostBack)
            {
                this.LoadCurrencyHead();
                this.LoadCommonDropDownHiddenField();
                this.LoadFiscalYear();
                this.LoadGLDonor();
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            _IsReportPanelEnable = 1;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/RptJournalRegister.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

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
            string voucherStatus = ddlGLStatus.SelectedValue, projectId = hfProjectId.Value, donorId = ddlDonor.SelectedValue;
            string companyName = hfCompanyName.Value;
            string projectName = hfProjectId.Value != "0" ? hfProjectName.Value : "All";
            List<ReportParameter> reportParam = new List<ReportParameter>();

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
            if (files[0].CompanyId > 0)
            {
                reportParam.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                reportParam.Add(new ReportParameter("CompanyName", companyName));
                reportParam.Add(new ReportParameter("CompanyProject", projectName));
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

            reportParam.Add(new ReportParameter("ReportDateFrom", startDate));
            reportParam.Add(new ReportParameter("ReportDateTo", endDate));

            string reportName = "Transaction List";
            if (this.ddlGLStatus.SelectedValue == "0")
            {
                reportName = "Pending Transaction List";
            }
            else
            {
                reportName = "Transaction List";
            }
            reportParam.Add(new ReportParameter("ReportName", reportName));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));            

            GLCommonReportDA commonReportDa = new GLCommonReportDA();
            List<GLJournalRegisterInfoForReportBO> voucherRegister = new List<GLJournalRegisterInfoForReportBO>();
            voucherRegister = commonReportDa.GetJournalRegisterInfo(FromDate, ToDate, voucherStatus, projectId, donorId);

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

            reportParam.Add(new ReportParameter("ReportCurrency", reportCurrency));

            Session["ReportAccountsCompanyName"] = companyName;
            Session["ReportAccountsProjectName"] = projectName;
            Session["ReportCurrencyName"] = reportCurrency;
            Session["ReportPrintDate"] = printDate;

            foreach (GLJournalRegisterInfoForReportBO row in voucherRegister)
            {
                if (ConversionRate != 1)
                {
                    if (row.LedgerAmount != 0)
                    {
                        row.LedgerAmount = row.LedgerAmount / ConversionRate;
                    }

                    if (row.Amount != 0)
                    {
                        row.Amount = row.Amount / ConversionRate;
                    }

                    if (row.DebitAmount != 0)
                    {
                        row.DebitAmount = row.DebitAmount / ConversionRate;
                    }

                    if (row.CreditAmount != 0)
                    {
                        row.CreditAmount = row.CreditAmount / ConversionRate;
                    }
                }
            }
            // // // ------- Multi Currency Related Effects -------------------------- End

            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], voucherRegister));

            rvTransaction.LocalReport.DisplayName = reportName;
            rvTransaction.LocalReport.Refresh();

            reportSearchType = this.ddlSearchType.SelectedValue;
            hfFiscalId = hfFiscalYear.Value;
            frmPrint.Attributes["src"] = "";
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
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
            CommonDropDownHiddenFieldForPleaseSelect.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadFiscalYear()
        {
            List<GLFiscalYearBO> fiscalYearList = new List<GLFiscalYearBO>();
            GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
            fiscalYearList = fiscalYearDA.GetAllFiscalYear();

            this.ddlFiscalYear.DataSource = fiscalYearList;
            this.ddlFiscalYear.DataTextField = "FiscalYearName";
            this.ddlFiscalYear.DataValueField = "FiscalYearId";
            this.ddlFiscalYear.DataBind();

            ListItem itemProject = new ListItem();
            itemProject.Value = "0";
            itemProject.Text = hmUtility.GetDropDownFirstValue();
            this.ddlFiscalYear.Items.Insert(0, itemProject);
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
    }
}