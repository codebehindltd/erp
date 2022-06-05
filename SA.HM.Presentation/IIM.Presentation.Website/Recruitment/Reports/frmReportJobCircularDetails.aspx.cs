using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;

namespace HotelManagement.Presentation.Website.Recruitment.Reports
{
    public partial class frmReportJobCircularDetails : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string jobCircularId = Request.QueryString["JobCircularId"];
                if (!string.IsNullOrWhiteSpace(jobCircularId))
                {
                    int Id = Convert.ToInt32(jobCircularId);
                    if (Id > 0)
                    {
                        Generate(Id);
                    }
                }
            } 
        }

        private void Generate(int jobCircularId)
        {
            dispalyReport = 1;            

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = Server.MapPath(@"~/Recruitment/Reports/Rdlc/rptJobCircularDetails.rdlc");

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

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));


            rvTransaction.LocalReport.SetParameters(reportParam);

            List<PayrollJobCircularBO> jobCircular = new List<PayrollJobCircularBO>();
            JobCircularNRecruitmentDA jobCircularDa = new JobCircularNRecruitmentDA();
            jobCircular = jobCircularDa.GetJobCircularForReport(jobCircularId);            

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], jobCircular));

            rvTransaction.LocalReport.DisplayName = "Job Circular";

            rvTransaction.LocalReport.Refresh();
        }
    }
}