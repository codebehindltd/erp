using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using Microsoft.Reporting.WebForms;
using HotelManagement.Entity.HMCommon;
using System.IO;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportPFStatement : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!Page.IsPostBack)
            {
                LoadDepartment();
                LoadCommonDropDownHiddenField();
                LoadYearList();
                LoadSalaryProcessMonth();
                ControlShowHide();
                ddlEmployee.Visible = false;
                lblEmployee.Visible = false;
            }
        }
        private void ControlShowHide()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (userInformationBO.IsAdminUser)
            {
                EmployeeInformationDiv.Visible = true;
            }
            else
            {
                EmployeeInformationDiv.Visible = false;
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
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
        }
        private void LoadYearList()
        {
            ddlYear.DataSource = hmUtility.GetReportYearList();
            ddlYear.DataBind();
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();
        }
        private void LoadSalaryProcessMonth()
        {
            EmpSalaryProcessDA entityDA = new EmpSalaryProcessDA();
            this.ddlEffectedMonth.DataSource = entityDA.GetSalaryProcessMonth();
            this.ddlEffectedMonth.DataTextField = "MonthHead";
            this.ddlEffectedMonth.DataValueField = "MonthValue";
            this.ddlEffectedMonth.DataBind();
            //this.ddlEffectedMonth.SelectedIndex = DateTime.Now.Month - 1;
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlEffectedMonth.Items.Insert(0, item);
        }
        protected void EmpDropDown_Change(object sender, EventArgs e)
        {
            int departId = Convert.ToInt32(ddlDepartment.SelectedValue);
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetEmployeeByDepartment(departId);

            if (departId == 0)
            {
                ddlEmployee.Visible = false;
                lblEmployee.Visible = false;
            }
            else
            {
                ddlEmployee.Visible = true;
                lblEmployee.Visible = true;
            }
            ddlEmployee.DataSource = empList;
            ddlEmployee.DataTextField = "DisplayName";
            ddlEmployee.DataValueField = "EmpId";
            ddlEmployee.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlEmployee.Items.Insert(0, item);
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (this.ddlEffectedMonth.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Process Month.", AlertType.Warning);
                ddlEffectedMonth.Focus();
                return;
            }
            else if (ddlDepartment.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Department.", AlertType.Warning);
                ddlDepartment.Focus();
                return;
            }
            else if (ddlEmployee.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Employee.", AlertType.Warning);
                ddlEmployee.Focus();
                return;
            }
            else
            {
                dispalyReport = 1;
                int departmentId = 0, empId = 0, year = 0;
                DateTime processDateTo = DateTime.Now;

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                if (userInformationBO.IsAdminUser)
                {
                    if (ddlDepartment.SelectedIndex != 0)
                    {
                        departmentId = Convert.ToInt32(ddlDepartment.SelectedValue);
                    }
                    if (departmentId != 0 && ddlEmployee.SelectedIndex != 0)
                    {
                        empId = Convert.ToInt32(ddlEmployee.SelectedValue);
                    }
                }
                else
                {
                    empId = Convert.ToInt32(userInformationBO.EmpId);
                }
                
                string selectedMonthRange = this.ddlEffectedMonth.SelectedValue.ToString();
                processDateTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(selectedMonthRange, ddlYear.SelectedValue));
                //processDateTo = hmUtility.GetDateTimeFromString(CommonHelper.SalaryDateTo(selectedMonthRange, ddlYear.SelectedValue), userInformationBO.ServerDateFormat);
                if (ddlYear.SelectedValue != "0")
                {
                    year = Convert.ToInt32(ddlYear.SelectedValue);
                }

                rvTransaction.LocalReport.DataSources.Clear();
                rvTransaction.ProcessingMode = ProcessingMode.Local;
                rvTransaction.LocalReport.EnableExternalImages = true;

                string reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptPFStatement.rdlc");

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
                footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

                HMCommonDA hmCommonDA = new HMCommonDA();
                string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
                reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
                reportParam.Add(new ReportParameter("PrintDateTime", printDate));
                reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
                reportParam.Add(new ReportParameter("ReportYear", year.ToString()));
                rvTransaction.LocalReport.SetParameters(reportParam);

                EmpPFDA empPFDA = new EmpPFDA();
                List<PFStatementReportViewBO> viewList = new List<PFStatementReportViewBO>();
                viewList = empPFDA.GetPFStatementReportInfo(processDateTo, year, departmentId, empId);

                var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewList));

                rvTransaction.LocalReport.DisplayName = "PF Statement";
                rvTransaction.LocalReport.Refresh();
            }
        }
    }
}