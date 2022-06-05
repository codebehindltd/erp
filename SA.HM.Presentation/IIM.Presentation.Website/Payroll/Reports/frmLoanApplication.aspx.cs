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
    public partial class frmLoanApplication : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string param = Request.QueryString["EmployeeId"];
                string[] param1 = param.Split(',');
                string employeeId = param1[0];
                string loanAmount = param1[1];
                string mode = param1[2];
                string purpose = param1[3];
                string loanType = param1[4];

                if (!string.IsNullOrWhiteSpace(employeeId))
                {
                    int Id = Convert.ToInt32(employeeId);
                    if (Id > 0)
                    {
                        Generate(Id, loanAmount, mode, purpose, loanType);
                    }
                }
            }
        }

        private void Generate(int empId, string loanAmount, string mode, string purpose, string loanType)
        {
            dispalyReport = 1;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptLoanApplication.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            List<LoanApplicationViewBO> loanApplicationBO = new List<LoanApplicationViewBO>();
            EmpLoanDA empLoanDA = new EmpLoanDA();
            loanApplicationBO = empLoanDA.GetEmpInfoForLoanApplication(empId);

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
            reportParam.Add(new ReportParameter("EmpCode", loanApplicationBO[0].EmpCode.ToString()));
            reportParam.Add(new ReportParameter("EmpName", loanApplicationBO[0].EmployeeName));
            reportParam.Add(new ReportParameter("JoinDate", loanApplicationBO[0].JoinDate.ToString()));
            if (loanApplicationBO[0].PFEligibilityDate != null)
            {
                DateTime pfDate = loanApplicationBO[0].PFEligibilityDate ?? DateTime.Now;
                reportParam.Add(new ReportParameter("PFDate", "(b) To Provident Fund  " + pfDate.ToString("dd MMM yyyy") + ""));
            }            
            else reportParam.Add(new ReportParameter("PFDate", ""));


            reportParam.Add(new ReportParameter("BasicAmount", loanApplicationBO[0].BasicAmount.ToString()));
            reportParam.Add(new ReportParameter("Designation", loanApplicationBO[0].Designation));
            reportParam.Add(new ReportParameter("IsLoanTakenBefore", loanApplicationBO[0].BeforeLoanTakenStatus));
            reportParam.Add(new ReportParameter("IsBeforeLoanClear", loanApplicationBO[0].IsThisLoanClear == 1 ? "Yes" : "No"));
            reportParam.Add(new ReportParameter("LoanAmount", loanAmount));
            reportParam.Add(new ReportParameter("ModeOfReturn", mode == "year"? "Yearly" : "Monthly"));
            reportParam.Add(new ReportParameter("Purpose", purpose));
            if (loanType == "CompanyLoan")
            {
                reportParam.Add(new ReportParameter("LoanType", "COMPANY"));
            }
            else reportParam.Add(new ReportParameter("LoanType", "PF"));

            rvTransaction.LocalReport.SetParameters(reportParam);



            //EmployeeDA emp1DA = new EmployeeDA();
            //List<EmpListForReportViewBO> empList = new List<EmpListForReportViewBO>();
            //empList = empDA.GetEmpListForReport(0, empId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], loanApplicationBO));


            rvTransaction.LocalReport.DisplayName = "Loan Application Form.";

            rvTransaction.LocalReport.Refresh();
        }
    }
}