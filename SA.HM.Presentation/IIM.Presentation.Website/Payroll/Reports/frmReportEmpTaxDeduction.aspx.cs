using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportEmpTaxDeduction : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
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
            DateTime? fromDate = null, toDate = null;
            int departmentId = 0;
            if (ddlDepartment.SelectedIndex != 0)
            {
                departmentId = Convert.ToInt32(ddlDepartment.SelectedValue);
            }
            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                //fromDate = Convert.ToDateTime(txtFromDate.Text);
                fromDate = CommonHelper.DateTimeToMMDDYYYY(txtFromDate.Text);
            }
            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                //toDate = Convert.ToDateTime(txtToDate.Text);
                toDate = CommonHelper.DateTimeToMMDDYYYY(txtToDate.Text);
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpTaxDeduction.rdlc");

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
            rvTransaction.LocalReport.SetParameters(reportParam);

            EmpTaxDeductionDA empTaxDeductionDA = new EmpTaxDeductionDA();
            List<EmpTaxDeductionBO> viewList = new List<EmpTaxDeductionBO>();
            viewList = empTaxDeductionDA.GetEmpTaxDeduction(fromDate, toDate, departmentId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewList));

            rvTransaction.LocalReport.DisplayName = "Emp Tax Deduction";
            rvTransaction.LocalReport.Refresh();
        }
    }
}