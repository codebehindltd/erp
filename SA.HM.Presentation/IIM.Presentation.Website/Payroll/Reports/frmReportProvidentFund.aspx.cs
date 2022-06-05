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
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportProvidentFund : BasePage
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
            }
        }

        private void LoadSalaryProcessMonth()
        {
            EmpSalaryProcessDA entityDA = new EmpSalaryProcessDA();
            this.ddlProcessMonth.DataSource = entityDA.GetSalaryProcessMonth();
            this.ddlProcessMonth.DataTextField = "MonthHead";
            this.ddlProcessMonth.DataValueField = "MonthValue";
            this.ddlProcessMonth.DataBind();
            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlProcessMonth.Items.Insert(0, item);
        }
        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            this.ddlDepartment.DataSource = entityDA.GetDepartmentInfo();
            this.ddlDepartment.DataTextField = "Name";
            this.ddlDepartment.DataValueField = "DepartmentId";
            this.ddlDepartment.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlDepartment.Items.Insert(0, item);
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            dispalyReport = 1;
            string empId = string.Empty, departmentId = string.Empty, selectedMonthRange = string.Empty, reportType = string.Empty;
            DateTime? salaryDateFrom = null, salaryDateTo = null;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            departmentId = ddlDepartment.SelectedValue == "0" ? string.Empty : ddlDepartment.SelectedValue;
            empId = ((HiddenField)employeeForLoanSearch.FindControl("hfEmployeeId")).Value;
            empId = empId == "0" ? string.Empty : empId;
            reportType = ddlReportType.SelectedValue == "0" ? string.Empty : ddlReportType.SelectedValue;

            if (ddlProcessMonth.SelectedValue != "0")
            {
                selectedMonthRange = ddlProcessMonth.SelectedValue;
                int startMonth = Convert.ToInt32(selectedMonthRange.Split('-')[0]);
                int endMonth = Convert.ToInt32(selectedMonthRange.Split('-')[1]);
                int salaryMonthStartDateDay = Convert.ToInt32(selectedMonthRange.Split('-')[2]);

                string fromSalaryDate = string.Empty;
                string toSalaryDate = string.Empty;

                if (startMonth == endMonth)
                {
                    fromSalaryDate = startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year;
                    if (startMonth != 12)
                    {
                        toSalaryDate = (endMonth + 1).ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year;
                    }
                    else
                    {
                        toSalaryDate = "01" + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + (DateTime.Now.Year + 1);
                    }

                }
                else
                {
                    if (startMonth < endMonth)
                    {
                        fromSalaryDate = startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year;
                        toSalaryDate = endMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year;
                    }
                    else
                    {
                        fromSalaryDate = startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year;
                        toSalaryDate = endMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + (DateTime.Now.Year + 1);
                    }
                }

                if (!string.IsNullOrEmpty(fromSalaryDate))
                {
                    salaryDateFrom = hmUtility.GetDateTimeFromString(fromSalaryDate, userInformationBO.ServerDateFormat);
                }

                if (!string.IsNullOrEmpty(toSalaryDate))
                {
                    salaryDateTo = hmUtility.GetDateTimeFromString(toSalaryDate, userInformationBO.ServerDateFormat);
                }
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptProvidentFund.rdlc");

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

            EmpPFDA empPFDA = new EmpPFDA();
            List<PFReportViewBO> viewList = new List<PFReportViewBO>();
            viewList = empPFDA.GetPFInfoForReport(empId, departmentId, salaryDateFrom, salaryDateTo, reportType);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewList));

            rvTransaction.LocalReport.DisplayName = "Provident Fund Info";
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
    }
}