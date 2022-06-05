using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Presentation.Website.Recruitment.Reports
{
    public partial class frmReportStaffingBudget : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDepartment();
                LoadJobType();
                LoadFiscalYear();
            }
        }

        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            this.ddlDepartments.DataSource = entityDA.GetDepartmentInfo();
            this.ddlDepartments.DataTextField = "Name";
            this.ddlDepartments.DataValueField = "DepartmentId";
            this.ddlDepartments.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlDepartments.Items.Insert(0, item);
        }
        private void LoadJobType()
        {
            EmpTypeDA entityDA = new EmpTypeDA();
            this.ddlJobType.DataSource = entityDA.GetEmpTypeInfo();
            this.ddlJobType.DataTextField = "Name";
            this.ddlJobType.DataValueField = "TypeId";
            this.ddlJobType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlJobType.Items.Insert(0, item);
        }
        private void LoadFiscalYear()
        {
            GLFiscalYearDA entityDA = new GLFiscalYearDA();
            this.ddlFiscalYear.DataSource = entityDA.GetAllFiscalYear();
            this.ddlFiscalYear.DataTextField = "FiscalYearName";
            this.ddlFiscalYear.DataValueField = "FiscalYearId";
            this.ddlFiscalYear.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlFiscalYear.Items.Insert(0, item);
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            dispalyReport = 1;
            int departmentId = 0, jobTypeId = 0, fiscalYearId = 0;
            string jobLevel = string.Empty;
            DateTime? fromDate = null, toDate = null;
            string startDate = string.Empty, endDate = string.Empty;

            if (ddlFiscalYear.SelectedIndex != 0)
            {
                fiscalYearId = Convert.ToInt32(ddlFiscalYear.SelectedValue);
            }
            if (ddlDepartments.SelectedIndex != 0)
            {
                departmentId = Convert.ToInt32(ddlDepartments.SelectedValue);
            }
            if (ddlJobType.SelectedIndex != 0)
            {
                jobTypeId = Convert.ToInt32(ddlJobType.SelectedValue);
            }
            jobLevel = ddlJobLevel.SelectedValue;
            
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = Server.MapPath(@"~/Recruitment/Reports/Rdlc/RptStaffingBudget.rdlc");

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

            StaffingBudgetDA staffBudgetDA = new StaffingBudgetDA();
            List<PayrollStaffingBudgeDetailsBO> viewList = new List<PayrollStaffingBudgeDetailsBO>();
            viewList = staffBudgetDA.GetPayrollStaffingBudgetForReport(departmentId, jobTypeId, jobLevel, fiscalYearId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewList));

            rvTransaction.LocalReport.DisplayName = "Staffing Budget Info";

            rvTransaction.LocalReport.Refresh();
        }
    }
}