using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Presentation.Website.Recruitment.Reports
{
    public partial class frmAppointmentLetter : System.Web.UI.Page
    {
        string reportingTime = string.Empty, reportingDate = string.Empty;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string applicantId = Request.QueryString["appId"];
                string jobId = Request.QueryString["jobId"];

                reportingDate = Request.QueryString["apDt"];
                reportingTime = Request.QueryString["rtm"];

                if (!string.IsNullOrEmpty(jobId))
                {
                    this.GenerateReport(Convert.ToInt64(jobId), applicantId);
                }
            }
        }

        private void GenerateReport(Int64 jobId, string applicantId)
        {
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = Server.MapPath(@"~/Recruitment/Reports/Rdlc/rptAppointmentLetter.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> reportParam = new List<ReportParameter>();

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

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("ReportingTime", reportingTime));
            reportParam.Add(new ReportParameter("ReportingDate", reportingDate));
            reportParam.Add(new ReportParameter("IssueDate", DateTime.Now.ToString("dd/MMM/yyyy")));
            rvTransaction.LocalReport.SetParameters(reportParam);

            JobCircularNRecruitmentDA jobCircularDa = new JobCircularNRecruitmentDA();
            List<ApplicantNInterviewDetailsBO> jobCircularlst = new List<ApplicantNInterviewDetailsBO>();
            jobCircularlst = jobCircularDa.GetApplicantNInterviewDetails(jobId, applicantId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], jobCircularlst));

            rvTransaction.LocalReport.DisplayName = "Appointment Date";

            rvTransaction.LocalReport.Refresh();

        }
    }
}