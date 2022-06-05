using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class LateAttendenceApplication : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string param = Request.QueryString["aId"];

                if (!string.IsNullOrWhiteSpace(param))
                {
                    int Id = Convert.ToInt32(param);
                    if (Id > 0)
                    {
                        Generate(Id);
                    }
                }
            }
        }
        private void Generate(int aId)
        {
            dispalyReport = 1;
            EmpAttendanceBO attendanceBO = new EmpAttendanceBO();
            EmpAttendanceDA attendenceDA = new EmpAttendanceDA();
            attendanceBO = attendenceDA.GetEmpAttendenceInfoById(aId);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = "";
            reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/LateAttendenceApp.rdlc");

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
            reportParam.Add(new ReportParameter("AppDate", DateTime.Now.ToString()));
           
            reportParam.Add(new ReportParameter("AttendanceDate", attendanceBO.AttendanceDate.ToShortDateString()));
            
            reportParam.Add(new ReportParameter("Reasons", attendanceBO.Remark));
            reportParam.Add(new ReportParameter("Name", attendanceBO.EmployeeName));
            reportParam.Add(new ReportParameter("EmpCode", attendanceBO.EmpCode));
            reportParam.Add(new ReportParameter("Designation", attendanceBO.Designation));
            rvTransaction.LocalReport.SetParameters(reportParam);
            rvTransaction.LocalReport.DisplayName = "Late Attendence Application";

            rvTransaction.LocalReport.Refresh();
        }
    }
}