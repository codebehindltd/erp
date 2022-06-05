using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using iTextSharp.text;
using iTextSharp.text.pdf;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportEmpProvisionPeriod : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int IsSuccess = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }
        
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            ReportGenerate();
        }

        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
        private void ReportGenerate()
        {
            IsSuccess = 1;
            DateTime? fromDate;
            DateTime? toDate;
            int? companyId, countryId;
            string stayedNight = string.Empty;
            string guestType = string.Empty;
            if (!string.IsNullOrWhiteSpace(this.txtStartDate.Text))
            {
                //txtStartDate.Text += "/" + new DateTime(1930, DateTime.Now.Month, DateTime.Now.Day).Year.ToString();
                //fromDate = Convert.ToDateTime(txtStartDate.Text);

                fromDate = hmUtility.GetDateTimeFromString(this.txtStartDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                fromDate = null;
            }
            if (!string.IsNullOrWhiteSpace(this.txtEndDate.Text))
            {
                //txtEndDate.Text += "/" + DateTime.Now.Year.ToString();
                //toDate = Convert.ToDateTime(txtEndDate.Text);
                toDate = hmUtility.GetDateTimeFromString(this.txtEndDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                toDate = null;
            }

            HMCommonDA hmCommonDA = new HMCommonDA();

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmployeeProvisionPeriod.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            //-- Company Logo -------------------------------
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            List<ReportParameter> paramLogo = new List<ReportParameter>();
            paramLogo.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            paramLogo.Add(new ReportParameter("PrintDateTime", printDate));
            paramLogo.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            if (files[0].CompanyId > 0)
            {
                paramLogo.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                paramLogo.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    paramLogo.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }
                else
                {
                    paramLogo.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                }
            }
            rvTransaction.LocalReport.SetParameters(paramLogo);

            EmployeeDA reportDA = new EmployeeDA();
            List<EmployeeBO> list = new List<EmployeeBO>();
            list = reportDA.GetEmployeeProvisionPeriodforReport(fromDate, toDate);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], list));

            rvTransaction.LocalReport.DisplayName = "Employee Provision Period";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
    }
}