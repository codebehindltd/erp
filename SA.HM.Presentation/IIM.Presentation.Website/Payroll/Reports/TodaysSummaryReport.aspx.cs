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
    public partial class TodaysSummaryReport : System.Web.UI.Page
    {
        protected int isMessageBoxEnable = -1;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Request.QueryString["type"];

            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(type))
                {
                    GenerateReportByType(type);
                }
            }
        }
        protected void GenerateReportByType(string reportType)
        {
            DateTime rosterDateFrom = DateTime.Now.Date, rosterDateTo = DateTime.Now.Date;
            //int departmentId = 0, timeslabId = 0, employeeId = 0;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            EmpAttendanceDA attendanceDA = new EmpAttendanceDA();
            List<EmpAttendanceReportBO> serviceList = new List<EmpAttendanceReportBO>();

            HMCommonDA hmCommonDA = new HMCommonDA();
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            if (reportType == "P")
            {
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmployeeAttendanceSingleDay.rdlc");
            }
            else if (reportType == "L")
            {
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmployeeAttendanceSingleDay.rdlc");
            }
            else if (reportType == "A")
            {
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmployeeAttendanceSingleDay.rdlc");
            }

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            //-- Company Logo -------------------------------
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            List<ReportParameter> paramLogo = new List<ReportParameter>();
            paramLogo.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

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

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            paramLogo.Add(new ReportParameter("PrintDateTime", printDate));
            paramLogo.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            if (reportType == "P")
            {
                serviceList = attendanceDA.GetEmpAttendanceReport(Convert.ToInt32(-1), 0, 0, rosterDateFrom, rosterDateTo, 0, 0, 0, 0).Where(x => x.AttendanceStatus == "P").ToList();
                paramLogo.Add(new ReportParameter("ReportName", "Employee Present List"));
            }            
            else if (reportType == "L")
            {
                serviceList = attendanceDA.GetEmpAttendanceReport(Convert.ToInt32(3), 0, 0, rosterDateFrom, rosterDateTo, 0, 0, 0, 0);
                paramLogo.Add(new ReportParameter("ReportName", "Late Attendance"));
            }
            else if (reportType == "A")
            {
                serviceList = attendanceDA.GetEmpAttendanceReport(Convert.ToInt32(-1), 0, 0, rosterDateFrom, rosterDateTo, 0, 0, 0, 0).Where(x => x.AttendanceStatus == "A").ToList();
                paramLogo.Add(new ReportParameter("ReportName", "Employee Absent List"));
            }

            rvTransaction.LocalReport.SetParameters(paramLogo);
            //-- Company Logo ------------------End----------

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], serviceList));
            
            if (reportType == "P")
            {
                rvTransaction.LocalReport.DisplayName = "Employee Present List";
            }
            else if (reportType == "L")
            {
                rvTransaction.LocalReport.DisplayName = "Late Attendance";
            }
            else if (reportType == "A")
            {
                rvTransaction.LocalReport.DisplayName = "Employee Absent List";
            }            

            rvTransaction.LocalReport.Refresh();
            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
    }
}