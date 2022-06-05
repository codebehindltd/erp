using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.UserInformation;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportPFReports : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!Page.IsPostBack)
            {
                LoadSalaryProcessMonth();
                LoadYearList();
            }
        }
        private void LoadSalaryProcessMonth()
        {
            EmpSalaryProcessDA entityDA = new EmpSalaryProcessDA();
            this.ddlMonth.DataSource = entityDA.GetSalaryProcessMonth();
            this.ddlMonth.DataTextField = "MonthHead";
            this.ddlMonth.DataValueField = "MonthValue";
            this.ddlMonth.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlMonth.Items.Insert(0, item);
        }
        private void LoadYearList()
        {
            ddlYear.DataSource = hmUtility.GetReportYearList();
            ddlYear.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlYear.Items.Insert(0, item);

            ddlSummaryYear.DataSource = hmUtility.GetReportYearList();
            ddlSummaryYear.DataBind();
            ddlSummaryYear.Items.Insert(0, item);

            ddlProductYear.DataSource = hmUtility.GetReportYearList();
            ddlProductYear.DataBind();
            ddlProductYear.Items.Insert(0, item);

            ddlInterestYear.DataSource = hmUtility.GetReportYearList();
            ddlInterestYear.DataBind();
            ddlInterestYear.Items.Insert(0, item);
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            dispalyReport = 1;
            int month = 0, year = 0, summaryYear = 0, productYear = 0, interestYear = 0;
            decimal interest = 0;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            string selectedMonthRange = string.Empty;
            if (ddlMonth.SelectedIndex != 0)
            {
                selectedMonthRange = ddlMonth.SelectedValue;
            }
            if (ddlYear.SelectedIndex != 0)
            {
                year = Convert.ToInt16(ddlYear.SelectedValue);
            }
            if (ddlSummaryYear.SelectedIndex != 0)
            {
                summaryYear = Convert.ToInt16(ddlSummaryYear.SelectedValue);
            }
            if (ddlProductYear.SelectedIndex != 0)
            {
                productYear = Convert.ToInt16(ddlProductYear.SelectedValue);
            }
            if (ddlInterestYear.SelectedIndex != 0)
            {
                interestYear = Convert.ToInt16(ddlInterestYear.SelectedValue);
            }
            if (!string.IsNullOrEmpty(txtInterest.Text))
            {
                interest = Convert.ToDecimal(txtInterest.Text);
            }
            var reportType = ddlReportType.SelectedValue;
            if (reportType == "0")
            {
                if (year == 0 || ddlMonth.SelectedIndex == 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Month & Year.", AlertType.Warning);
                    dispalyReport = 0;
                    return;
                }
                else
                {
                    DateTime SalaryDateFrom = DateTime.Now, SalaryDateTo = DateTime.Now;
                    SalaryDateFrom =  Convert.ToDateTime(CommonHelper.SalaryDateFrom(selectedMonthRange, ddlYear.SelectedValue));
                    SalaryDateTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(selectedMonthRange, ddlYear.SelectedValue));
                    month = SalaryDateFrom.Month;
                }
            }
            
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            List<ReportParameter> reportParam = new List<ReportParameter>();

            string reportPath = string.Empty;
            if ( reportType == "0")
            {
                reportParam.Add(new ReportParameter("ReportMonth", this.ddlMonth.SelectedItem.Text));
                reportParam.Add(new ReportParameter("ReportYear", this.ddlYear.SelectedItem.Text));
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptMonthlyPFStaffList.rdlc");
            }
            else if (reportType == "1")
            {
                reportParam.Add(new ReportParameter("ReportMonth", ""));
                reportParam.Add(new ReportParameter("ReportYear", this.ddlSummaryYear.SelectedItem.Text));
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptPFSummary.rdlc");
            }
            else if (reportType == "2")
            {
                reportParam.Add(new ReportParameter("ReportMonth", ""));
                reportParam.Add(new ReportParameter("ReportYear", this.ddlProductYear.SelectedItem.Text));
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptPFProductCalculation.rdlc");
            }
            else if (reportType == "3")
            {
                reportParam.Add(new ReportParameter("ReportMonth", ""));
                reportParam.Add(new ReportParameter("ReportYear", this.ddlInterestYear.SelectedItem.Text));
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptPFInterestCalculation.rdlc");
            }

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

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
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            
            rvTransaction.LocalReport.SetParameters(reportParam);

            EmpPFDA empPFDA = new EmpPFDA();
            List<PFMonthlyStaffListViewBO> viewList = new List<PFMonthlyStaffListViewBO>();
            List<PFSummaryReportViewBO> viewList1 = new List<PFSummaryReportViewBO>();
            List<PFProductCalculationViewBO> viewList2 = new List<PFProductCalculationViewBO>();
            List<PFProductCalculationViewBO> viewList3 = new List<PFProductCalculationViewBO>();
            if (reportType == "0")
            {
                viewList = empPFDA.GetPFMonthlyStaffListReport(year, month);
            }
            else if (reportType == "1")
            {
                viewList1 = empPFDA.GetPFSummaryReport(summaryYear);
            }
            else if (reportType == "2")
            {
                viewList2 = empPFDA.GetPFProductCalculationReport(productYear);
            }
            else if (reportType == "3")
            {
                viewList3 = empPFDA.GetPFInterestCalculationReport(interestYear, interest);
            }
            
            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            if (reportType == "0")
            {
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewList));
            }
            else if (reportType == "1")
            {
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewList1));
            }
            else if (reportType == "2")
            {
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewList2));
            }
            else if (reportType == "3")
            {
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewList3));
            }

            rvTransaction.LocalReport.DisplayName = "PF Reports";
            rvTransaction.LocalReport.Refresh();
        }
    }
}