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
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmEmpLeaveInformation : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.LoadYearList();
            }
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {

            string departmentId = string.Empty, empId = string.Empty, leaveType = string.Empty;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            DateTime reportDate = DateTime.Now;
            string strReportDate = string.Empty;

            if (this.ddlYear.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Report Year.", AlertType.Warning);
                this.ddlYear.Focus();
                return;
            }
            else
            {
                if (userInformationBO.ServerDateFormat == "dd/MM/yyyy")
                {
                    strReportDate = DateTime.Now.ToString("dd") + "/" + DateTime.Now.ToString("MM") + "/" + this.ddlYear.SelectedValue;
                }
                else
                {
                    strReportDate = DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd") + "/" + this.ddlYear.SelectedValue;
                }
                if (!string.IsNullOrEmpty(strReportDate))
                {
                    reportDate = hmUtility.GetDateTimeFromString(strReportDate, userInformationBO.ServerDateFormat);
                }
            }

            dispalyReport = 1;

            HiddenField hfEmployeeId = (HiddenField)this.employeeeSearch.FindControl("hfEmployeeId");

            if (hfEmployeeId.Value != "0")
                empId = hfEmployeeId.Value;
            else
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "an employee.", AlertType.Warning);
                return;
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;
            string reportPath = string.Empty;

            if (ddlReportType.SelectedValue == "0")
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpLeaveInformation.rdlc");
            else reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptMonthlyLeaveSummary.rdlc");

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

            string ReportForEmployee = string.Empty;
            
            EmployeeDA empDA = new EmployeeDA();
            EmployeeBO employeeInfoBO = new EmployeeBO();
            employeeInfoBO = empDA.GetEmployeeInfoById(Convert.ToInt32(empId));

            if (employeeInfoBO != null)
            {
                if (employeeInfoBO.EmpId > 0)
                {
                    ReportForEmployee = "Employee Id: " + employeeInfoBO.EmpCode + "~Name: " + employeeInfoBO.DisplayName + "~Designation: " + employeeInfoBO.Designation + "~Department: " + employeeInfoBO.Department;
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
            reportParam.Add(new ReportParameter("ReportProcessYear", this.ddlYear.SelectedValue));
            reportParam.Add(new ReportParameter("ReportForEmployee", ReportForEmployee));
            rvTransaction.LocalReport.SetParameters(reportParam);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            if (ddlReportType.SelectedValue == "0")
            {
                EmployeeYearlyLeaveDA leaveDa = new EmployeeYearlyLeaveDA();
                List<LeaveTakenNBalanceBO> leaveInformationList = new List<LeaveTakenNBalanceBO>();
                leaveInformationList = leaveDa.GetLeaveTakenNBalanceByEmployee(Convert.ToInt32(empId), reportDate);
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], leaveInformationList));
                rvTransaction.LocalReport.DisplayName = "Employee Leave Information";
            }
            else
            {
                LeaveInformationDA leaveDA = new LeaveInformationDA();
                List<LeaveInformationBO> leaveStatusList = new List<LeaveInformationBO>();
                leaveStatusList = leaveDA.GetEmpMonthlyLeaveStatus(Convert.ToInt32(empId));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], leaveStatusList));
                rvTransaction.LocalReport.DisplayName = "Employee Monthly Leave Summary";
            }

            //rvTransaction.LocalReport.DisplayName = "Employee Leave Information";

            rvTransaction.LocalReport.Refresh();
        }
        private void LoadYearList()
        {
            ddlYear.DataSource = hmUtility.GetReportYearList();
            ddlYear.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlYear.Items.Insert(0, item);
        }
    }
}