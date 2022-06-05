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
    public partial class frmEmpOvertime : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadDepartment();
            }
        }

        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            ddlDepartment.DataSource = entityDA.GetDepartmentInfo();
            ddlDepartment.DataTextField = "Name";
            ddlDepartment.DataValueField = "DepartmentId";
            ddlDepartment.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlDepartment.Items.Insert(0, item);
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            dispalyReport = 1;
            int? employeeId = null, departmentId = null; DateTime? attendanceFromDate = null, attendanceToDate = null;
            string empId = string.Empty;

            ContentPlaceHolder mpContentPlaceHolder;
            UserControl empSearch;
            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            empSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeeSearch");

            empId = ((HiddenField)empSearch.FindControl("hfEmployeeId")).Value;

            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");

            if (empId != "0")
            {
                employeeId = Convert.ToInt32(empId);
            }

            if (ddlDepartment.SelectedIndex != 0)
            {
                departmentId = Convert.ToInt32(ddlDepartment.SelectedValue);
            }

            if (!string.IsNullOrEmpty(txtAttendanceFromDate.Text))
            {
                attendanceFromDate = hmUtility.GetDateTimeFromString(txtAttendanceFromDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                //attendanceFromDate = Convert.ToDateTime(txtAttendanceFromDate.Text);
            }
            if (!string.IsNullOrEmpty(txtAttendanceToDate.Text))
            {
                attendanceToDate = hmUtility.GetDateTimeFromString(txtAttendanceToDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                //attendanceToDate = Convert.ToDateTime(txtAttendanceToDate.Text);
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmployeeOvertime.rdlc");

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

            EmpOverTimeDA eveDA = new EmpOverTimeDA();
            List<EmpOverTimeBO> overtime = new List<EmpOverTimeBO>();
            overtime = eveDA.GetEmpOverTime(employeeId, departmentId, attendanceFromDate, attendanceToDate);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], overtime));

            rvTransaction.LocalReport.DisplayName = "Employee Overtime";

            rvTransaction.LocalReport.Refresh();
        }

    }
}