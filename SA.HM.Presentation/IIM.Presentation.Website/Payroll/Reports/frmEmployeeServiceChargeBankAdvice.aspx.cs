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
    public partial class frmEmployeeServiceChargeBankAdvice : BasePage
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
                LoadSalaryProcessMonth();
                LoadYearList();
                LoadBankInfo();
            }
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
        private void LoadBankInfo()
        {
            BankDA entityDA = new BankDA();
            ddlBankId.DataSource = entityDA.GetBankInfo();
            ddlBankId.DataTextField = "BankName";
            ddlBankId.DataValueField = "BankId";
            ddlBankId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlBankId.Items.Insert(0, item);
        }
        private void LoadSalaryProcessMonth()
        {
            EmpSalaryProcessDA entityDA = new EmpSalaryProcessDA();
            this.ddlProcessMonth.DataSource = entityDA.GetSalaryProcessMonth();
            this.ddlProcessMonth.DataTextField = "MonthHead";
            this.ddlProcessMonth.DataValueField = "MonthValue";
            this.ddlProcessMonth.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlProcessMonth.Items.Insert(0, item);
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            dispalyReport = 1;

            int departmentId = 0, bankId = 0;
            string selectedMonthRange = string.Empty;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            departmentId = Convert.ToInt32(ddlDepartmentId.SelectedValue);
            bankId = Convert.ToInt32(ddlBankId.SelectedValue);

            selectedMonthRange = ddlProcessMonth.SelectedValue;

            DateTime salaryDateFrom = DateTime.Now, salaryDateTo = DateTime.Now;

            //salaryDateFrom = hmUtility.GetDateTimeFromString(CommonHelper.SalaryDateFrom(selectedMonthRange, ddlYear.SelectedValue), userInformationBO.ServerDateFormat);
            //salaryDateTo = hmUtility.GetDateTimeFromString(CommonHelper.SalaryDateTo(selectedMonthRange, ddlYear.SelectedValue), userInformationBO.ServerDateFormat);

            salaryDateFrom = Convert.ToDateTime(CommonHelper.SalaryDateFrom(selectedMonthRange, ddlYear.SelectedValue));
            salaryDateTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(selectedMonthRange, ddlYear.SelectedValue));

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmployeeServiceChargeBankAdvice.rdlc");

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
            reportParam.Add(new ReportParameter("ReportProcessMonthNYear", this.ddlProcessMonth.SelectedItem.Text + " " + this.ddlYear.SelectedValue));
            reportParam.Add(new ReportParameter("ReportProcessBank", this.ddlBankId.SelectedItem.Text));
            rvTransaction.LocalReport.SetParameters(reportParam);

            EmployeeDA eveDA = new EmployeeDA();
            List<ServiceChargeDistributionDetailsViewBO> serviceCharge = new List<ServiceChargeDistributionDetailsViewBO>();
            serviceCharge = eveDA.GetEmployeeServiceChargeBankAdvice(departmentId, bankId, salaryDateFrom, salaryDateTo, Convert.ToInt16(ddlYear.SelectedValue));

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], serviceCharge));

            rvTransaction.LocalReport.DisplayName = "Service Charge Information";

            rvTransaction.LocalReport.Refresh();
        }

        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            ddlDepartmentId.DataSource = entityDA.GetDepartmentInfo();
            ddlDepartmentId.DataTextField = "Name";
            ddlDepartmentId.DataValueField = "DepartmentId";
            ddlDepartmentId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlDepartmentId.Items.Insert(0, item);
        }
    }
}