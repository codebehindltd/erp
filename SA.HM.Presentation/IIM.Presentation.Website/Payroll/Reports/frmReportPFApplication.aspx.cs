using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportPFApplication : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string param = Request.QueryString["EmployeeId"];
                string[] param1 = param.Split(',');
                string EmployeeId = param1[0];
                string pfdate = param1[1];
                string type = param1[2];
                
                if (!string.IsNullOrWhiteSpace(EmployeeId) && !string.IsNullOrEmpty(pfdate))
                {
                    int Id = Convert.ToInt32(EmployeeId);
                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                    DateTime date = hmUtility.GetDateTimeFromString(pfdate, userInformationBO.ServerDateFormat); //Convert.ToDateTime(pfdate);
                    if (Id > 0)
                    {
                        Generate(Id, date, type);
                    }
                }
            }
        }

        private void Generate(int empId, DateTime pfDate, string type)
        {            
            dispalyReport = 1;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = "";
            if (type == "application")
            {
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptPFApplication.rdlc");
            }
            else {
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptPFWithdrawalApplication.rdlc");
            }

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            EmployeeBO empBO = new EmployeeBO();
            EmployeeDA empDA = new EmployeeDA();
            empBO = empDA.GetEmployeeInfoById(empId);

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

            reportParam.Add(new ReportParameter("ApplicationDate", DateTime.Now.ToString()));
            reportParam.Add(new ReportParameter("PFDate", empBO.PFEligibilityDate.ToString()));
            reportParam.Add(new ReportParameter("EmpId", empBO.EmpCode.ToString()));
            reportParam.Add(new ReportParameter("EmpName", empBO.DisplayName));
            reportParam.Add(new ReportParameter("FathersName", empBO.FathersName));
            reportParam.Add(new ReportParameter("Department", empBO.Department));
            reportParam.Add(new ReportParameter("Designation", empBO.Designation));

            rvTransaction.LocalReport.SetParameters(reportParam);



            //EmployeeDA emp1DA = new EmployeeDA();
            List<EmpListForReportViewBO> empList = new List<EmpListForReportViewBO>();
            empList = empDA.GetEmpListForReport(0, empId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], empList));

            if (type == "application")
            {
                rvTransaction.LocalReport.DisplayName = "PF Application Form.";
            }
            else {
                rvTransaction.LocalReport.DisplayName = "PF Withdrawal Application Form.";
            }

            rvTransaction.LocalReport.Refresh();
        }
    }
}