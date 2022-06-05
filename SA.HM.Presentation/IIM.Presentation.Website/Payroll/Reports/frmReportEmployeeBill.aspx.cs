using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using System.Collections;
using Microsoft.Reporting.WebForms;
using System.IO;
using Newtonsoft.Json;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportEmployeeBill : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployee();
            }
        }

        private void LoadEmployee()
        {
            EmployeeDA employeeDa = new EmployeeDA();
            List<EmployeeBO> employeeList = new List<EmployeeBO>();
            employeeList = employeeDa.GetEmployeeInfo();

            ddlEmployee.DataSource = employeeList;
            ddlEmployee.DataTextField = "DisplayName";
            ddlEmployee.DataValueField = "EmpId";
            ddlEmployee.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownNoneValue();
            ddlEmployee.Items.Insert(0, item);
        }

        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = "";
            string reportType = string.Empty;
            Int32 employeeId = Convert.ToInt32(ddlEmployee.SelectedValue);
            reportType = ddlReportStatus.SelectedValue;

            if (reportType == "EmployeBill")
            {
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmployeeBill.rdlc");
            }
            else if (reportType == "EmployeGeneratedBill" || reportType == "EmployeGeneratedDueBill")
            {
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmployeeGeneratedBill.rdlc");
            }

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;
            List<ReportParameter> reportParam = new List<ReportParameter>();

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
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

            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            rvTransaction.LocalReport.SetParameters(reportParam);

            EmployeeDA empDa = new EmployeeDA();
            List<EmployeePaymentLedgerBO> employeeLedgerInfo = new List<EmployeePaymentLedgerBO>();
            List<EmployeeBillGenerateViewBO> employeeGeneratedBill = new List<EmployeeBillGenerateViewBO>();

            if (reportType == "EmployeBill")
            {
                employeeLedgerInfo = empDa.EmployeeBillLedgerForReport(employeeId);
            }
            else if (reportType == "EmployeGeneratedBill" || reportType == "EmployeGeneratedDueBill")
            {
                employeeGeneratedBill = empDa.EmployeeGeneratedBillForReport(employeeId, reportType);
            }

            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            if (reportType == "EmployeBill")
            {
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], employeeLedgerInfo));
            }
            else if (reportType == "EmployeGeneratedBill" || reportType == "EmployeGeneratedDueBill")
            {
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], employeeGeneratedBill));
            }

            rvTransaction.LocalReport.DisplayName = "Employee Bill";
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
        //************************ User Defined Function ********************//

        [WebMethod]
        public static List<EmployeeBO> GetCompanyData(string searchText)
        {
            List<EmployeeBO> companyList = new List<EmployeeBO>();
            EmployeeDA suppDa = new EmployeeDA();

            companyList = suppDa.GetEmployeeInfoBySearchCriteria(searchText);

            return companyList;
        }

    }
}