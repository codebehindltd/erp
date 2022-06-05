using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportEmpIncrement : BasePage
    {
        protected int IsSuccess = -1;
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {            
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.LoadEmployee();
            }
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
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();            

            int empId = 0;
            if (ddlEmployee.SelectedIndex != 0)
            {
                empId = Convert.ToInt32(ddlEmployee.SelectedValue);
            }
            if (empId == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "an Employee.", AlertType.Warning);
                return;
            }

            IsSuccess = 1;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpIncrement.rdlc");

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
            
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvTransaction.LocalReport.SetParameters(reportParam);

            EmpIncrementDA incrementDA = new EmpIncrementDA();
            List<EmpIncrementBO> incrementList = new List<EmpIncrementBO>();
            incrementList = incrementDA.GetAllIncrement();
            if (empId != 0)
                incrementList = incrementList.Where(a => a.EmpId == Convert.ToInt32(ddlEmployee.SelectedValue)).ToList();

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], incrementList));

            rvTransaction.LocalReport.DisplayName = "Employee Increment";
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