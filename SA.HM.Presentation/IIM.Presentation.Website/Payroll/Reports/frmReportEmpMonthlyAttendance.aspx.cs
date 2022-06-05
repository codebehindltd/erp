using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.GeneralLedger;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportEmpMonthlyAttendance : BasePage
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadYearList();
                LoadDepartment();
                LoadEmployee();
            }
        }

        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            _RoomStatusInfoByDate = 1;
            int empId = 0, departmentId = 0;
            string month = string.Empty, year = string.Empty;

            if (ddlDepartment.SelectedIndex != 0)
                departmentId = Convert.ToInt32(ddlDepartment.SelectedValue);
            if (ddlEmployee.SelectedIndex != 0)
                empId = Convert.ToInt32(ddlEmployee.SelectedValue);
            month = ddlMonth.SelectedValue;
            year = ddlYear.SelectedValue;
            int reportType = 0;
            reportType = Convert.ToInt32(ddlReportType.SelectedValue);

            int companyId = 0;
            string companyName = "All";
            int projectId = 0;
            string projectName = "All";

            if (hfCompanyId.Value != "0" && hfCompanyId.Value != "")
            {
                companyId = Convert.ToInt32(hfCompanyId.Value);
                companyName = hfCompanyName.Value;
            }

            if (hfProjectId.Value != "0" && hfProjectId.Value != "")
            {
                projectId = Convert.ToInt32(hfProjectId.Value);
                projectName = hfProjectName.Value;
            }

            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = "";
            if (reportType == 1)
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpEntry.rdlc");
            else if (reportType == 2)
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpEntryDetails.rdlc");
            else reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpMonthlyAttendance.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> paramReport = new List<ReportParameter>();

            string companyAddress = string.Empty;
            string webAddress = string.Empty;
            string imageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");

            if (files[0].CompanyId > 0)
            {
                companyName = files[0].CompanyName;
                companyAddress = files[0].CompanyAddress;

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    webAddress = files[0].WebAddress;
                }
                else
                {
                    webAddress = files[0].ContactNumber;
                }
            }

            //int glCompanyId = Convert.ToInt32(ddlGLCompany.SelectedValue);
            if (companyId > 0)
            {
                GLCompanyBO glCompanyBO = new GLCompanyBO();
                GLCompanyDA glCompanyDA = new GLCompanyDA();
                glCompanyBO = glCompanyDA.GetGLCompanyInfoById(companyId);
                if (glCompanyBO != null)
                {
                    if (glCompanyBO.CompanyId > 0)
                    {
                        companyName = glCompanyBO.Name;
                        if (!string.IsNullOrWhiteSpace(glCompanyBO.CompanyAddress))
                        {
                            companyAddress = glCompanyBO.CompanyAddress;
                        }
                        if (!string.IsNullOrWhiteSpace(glCompanyBO.WebAddress))
                        {
                            webAddress = glCompanyBO.WebAddress;
                        }
                        if (!string.IsNullOrWhiteSpace(glCompanyBO.ImageName))
                        {
                            imageName = glCompanyBO.ImageName;
                        }
                    }
                }
            }

            if (empId == 0)
            {
                paramReport.Add(new ReportParameter("EmployeeName", "All"));
                paramReport.Add(new ReportParameter("Department", ddlDepartment.SelectedItem.Text));
            }
            else
            {
                paramReport.Add(new ReportParameter("EmployeeName", ddlEmployee.SelectedItem.Text));
                EmployeeDA employeeDa = new EmployeeDA();
                EmployeeBO employeeBo = employeeDa.GetEmployeeInfoById(empId);
                paramReport.Add(new ReportParameter("Department", employeeBo.Department));
            }

            paramReport.Add(new ReportParameter("Month", ddlMonth.SelectedValue));
            paramReport.Add(new ReportParameter("Year", ddlYear.SelectedValue));

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));


            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("CompanyProfile", companyName));
            paramReport.Add(new ReportParameter("CompanyAddress", companyAddress));
            paramReport.Add(new ReportParameter("CompanyWeb", webAddress));
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));

            if (reportType == 3)
            {
                paramReport.Add(new ReportParameter("Month", ddlMonth.SelectedValue));
                paramReport.Add(new ReportParameter("Year", ddlYear.SelectedValue));
            }

            EmpAttendanceDA attendanceDA = new EmpAttendanceDA();
            List<EmpMonthlyAttendanceViewBO> viewList = new List<EmpMonthlyAttendanceViewBO>();
            if (reportType == 1)
                viewList = attendanceDA.GetEmpMonthlyEntryReport(companyId, projectId, empId, departmentId, year, month);
            else if (reportType == 2)
                viewList = attendanceDA.GetEmpMonthlyEntryDetailsReport(companyId, projectId, empId, departmentId, year, month);
            else viewList = attendanceDA.GetEmpMonthlyAttendanceReport(empId, departmentId, year, month);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewList));

            rvTransaction.LocalReport.SetParameters(paramReport);
            rvTransaction.LocalReport.DisplayName = "Employee Monthly Attendance";
            rvTransaction.LocalReport.Refresh();


            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Portrait.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }

        private void LoadEmployee()
        {
            EmployeeDA employeeDA = new EmployeeDA();
            List<EmployeeBO> employeeBOList = new List<EmployeeBO>();
            employeeBOList = employeeDA.GetEmployeeInfo();

            this.ddlEmployee.DataSource = employeeBOList;
            this.ddlEmployee.DataTextField = "EmployeeName";
            this.ddlEmployee.DataValueField = "EmpId";
            this.ddlEmployee.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlEmployee.Items.Insert(0, item);
        }
        private void LoadYearList()
        {
            ddlYear.DataSource = hmUtility.GetReportYearList();
            ddlYear.DataBind();
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();
        }
        //private void LoadSalaryProcessMonth()
        //{
        //    EmpSalaryProcessDA entityDA = new EmpSalaryProcessDA();
        //    this.ddlMonth.DataSource = entityDA.GetSalaryProcessMonth();
        //    this.ddlMonth.DataTextField = "MonthHead";
        //    this.ddlMonth.DataValueField = "MonthValue";
        //    this.ddlMonth.DataBind();
        //    this.ddlMonth.SelectedIndex = DateTime.Now.Month - 1;            
        //}
        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            this.ddlDepartment.DataSource = entityDA.GetDepartmentInfo();
            this.ddlDepartment.DataTextField = "Name";
            this.ddlDepartment.DataValueField = "DepartmentId";
            this.ddlDepartment.DataBind();

            ddlDepartment.Items.Insert(0, new System.Web.UI.WebControls.ListItem { Text = hmUtility.GetDropDownFirstAllValue(), Value = "0" });
        }
    }
}