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
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportLeaveApplication : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string param = Request.QueryString["LeaveId"];

                if (!string.IsNullOrWhiteSpace(param))
                {
                    int leaveId = Convert.ToInt32(param);
                    if (leaveId > 0)
                    {
                        Generate(leaveId);
                    }
                }
            }
        }
        private void Generate(int leaveId)
        {
            dispalyReport = 1;

            LeaveInformationDA leaveDA = new LeaveInformationDA();
            LeaveInformationBO leaveBO = new LeaveInformationBO();
            leaveBO = leaveDA.GetEmpLeaveInformationById(leaveId);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = "";
            reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptLeaveApplication.rdlc");

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
            //reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            //reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            reportParam.Add(new ReportParameter("AppDate", DateTime.Now.ToString()));
            reportParam.Add(new ReportParameter("ReportingTo", leaveBO.ReportingToDesignation));
            reportParam.Add(new ReportParameter("NoOfDays", leaveBO.NoOfDays.ToString()));
            reportParam.Add(new ReportParameter("LeaveType", leaveBO.TypeName));
            reportParam.Add(new ReportParameter("Reasons", leaveBO.Reason));
            reportParam.Add(new ReportParameter("Name", leaveBO.EmployeeName));
            reportParam.Add(new ReportParameter("EmpCode", leaveBO.EmpCode));
            reportParam.Add(new ReportParameter("Department", leaveBO.DepartmentName));
            reportParam.Add(new ReportParameter("Designation", leaveBO.Designation));
            if (leaveBO.NoOfDays == 1)
            {
                reportParam.Add(new ReportParameter("Condition",  leaveBO.NoOfDays + " day leave on " + leaveBO.ShowFromDate));
            }
            else if (leaveBO.NoOfDays > 1)
            {
                reportParam.Add(new ReportParameter("Condition", leaveBO.NoOfDays + " days leave from " + leaveBO.ShowFromDate + " to " + leaveBO.ShowToDate));
            }

            rvTransaction.LocalReport.SetParameters(reportParam);            

            //var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            //rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], leaveBO));

            rvTransaction.LocalReport.DisplayName = "Leave Application";


            rvTransaction.LocalReport.Refresh();
        }
    }
}