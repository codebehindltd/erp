using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.UserInformation;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmEmpSalaryStatement : BasePage
    {
        HiddenField innboardMessage;
        protected int isMessageBoxEnable = -1;
        protected int _IsReportPanelEnable = -1;
        HMUtility hmUtility = new HMUtility();

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadEmployee();
                LoadYear();
                LoadMonth();
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            _IsReportPanelEnable = 1;

            DateTime processDateFrom = DateTime.Now, processDateTo = DateTime.Now;
            int departmentId = 0, gradeId = 0, employeeId = 0, branchId = 0;

            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (ddlEmployee.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "an Employee.", AlertType.Warning);
                return;
            }
            else if (ddlFromMonth.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "from month.", AlertType.Warning);
                return;
            }
            else if (ddlToMonth.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "to month.", AlertType.Warning);
                return;
            }

            if (ddlEmployee.SelectedIndex != 0)
                employeeId = Convert.ToInt32(ddlEmployee.SelectedValue);

            string selectedFromMonth = this.ddlFromMonth.SelectedValue.ToString();
            string selectedToMonth = this.ddlToMonth.SelectedValue.ToString();

            //processDateFrom = Convert.ToDateTime(CommonHelper.SalaryDateFrom(selectedFromMonth, ddlFromYear.SelectedValue));
            //processDateTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(selectedToMonth, ddlToYear.SelectedValue));

            processDateFrom = Convert.ToDateTime( CommonHelper.SalaryDateFrom(selectedFromMonth, ddlFromYear.SelectedValue));
            processDateTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(selectedToMonth, ddlToYear.SelectedValue));

            EmpSalaryProcessDA salaryProcessDA = new EmpSalaryProcessDA();

            List<SalarySheetBO> salarystatementList = new List<SalarySheetBO>();
            salarystatementList = salaryProcessDA.EmployeeSalarySheets(0, "SalaryStatement", employeeId, departmentId, gradeId, branchId, processDateFrom, processDateTo, 0, string.Empty);

            if (salarystatementList != null)
            {
                if (salarystatementList.Count > 0)
                {
                    rvTransaction.LocalReport.DataSources.Clear();
                    rvTransaction.ProcessingMode = ProcessingMode.Local;

                    var reportPath = "";

                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpSalaryStatement.rdlc");

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

                    paramLogo.Add(new ReportParameter("FromMonth", ddlFromMonth.SelectedItem.ToString()));
                    paramLogo.Add(new ReportParameter("FromYear", ddlFromYear.SelectedItem.ToString()));
                    paramLogo.Add(new ReportParameter("ToMonth", ddlToMonth.SelectedItem.ToString()));
                    paramLogo.Add(new ReportParameter("ToYear", ddlToYear.SelectedItem.ToString()));

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

                    footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

                    paramLogo.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
                    paramLogo.Add(new ReportParameter("PrintDateTime", printDate));
                    paramLogo.Add(new ReportParameter("EmpCode", salarystatementList[0].EmpCode));
                    paramLogo.Add(new ReportParameter("EmpName", salarystatementList[0].EmployeeName));
                    paramLogo.Add(new ReportParameter("Department", salarystatementList[0].DepartmentName));
                    paramLogo.Add(new ReportParameter("Designation", salarystatementList[0].Designation));

                    rvTransaction.LocalReport.SetParameters(paramLogo);
                    //-- Company Logo ------------------End----------

                    var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                    rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], salarystatementList));

                    rvTransaction.LocalReport.DisplayName = "Employee Salary Statement";
                    rvTransaction.LocalReport.Refresh();
                    frmPrint.Attributes["src"] = "";
                }
            }
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }

        private void LoadEmployee()
        {
            EmployeeDA employeeDA = new EmployeeDA();
            this.ddlEmployee.DataSource = employeeDA.GetEmployeeInfo();
            this.ddlEmployee.DataTextField = "EmployeeName";
            this.ddlEmployee.DataValueField = "EmpId";
            this.ddlEmployee.DataBind();

            System.Web.UI.WebControls.ListItem itemEmployee = new System.Web.UI.WebControls.ListItem();
            itemEmployee.Value = "0";
            itemEmployee.Text = hmUtility.GetDropDownFirstValue();
            this.ddlEmployee.Items.Insert(0, itemEmployee);

        }
        private void LoadMonth()
        {
            EmpSalaryProcessDA entityDA = new EmpSalaryProcessDA();
            var months = entityDA.GetSalaryProcessMonth();

            this.ddlFromMonth.DataSource = months;
            this.ddlFromMonth.DataTextField = "MonthHead";
            this.ddlFromMonth.DataValueField = "MonthValue";
            this.ddlFromMonth.DataBind();
            this.ddlFromMonth.SelectedIndex = DateTime.Now.Month - 1;

            this.ddlToMonth.DataSource = months;
            this.ddlToMonth.DataTextField = "MonthHead";
            this.ddlToMonth.DataValueField = "MonthValue";
            this.ddlToMonth.DataBind();
            this.ddlToMonth.SelectedIndex = DateTime.Now.Month - 1;

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            this.ddlFromMonth.Items.Insert(0, item);
            this.ddlToMonth.Items.Insert(0, item);
        }
        private void LoadYear()
        {
            var yearlist = hmUtility.GetReportYearList();

            ddlFromYear.DataSource = yearlist;
            ddlFromYear.DataBind();
            ddlFromYear.SelectedValue = DateTime.Now.Year.ToString();

            ddlToYear.DataSource = yearlist;
            ddlToYear.DataBind();
            ddlToYear.SelectedValue = DateTime.Now.Year.ToString();
        }
    }
}