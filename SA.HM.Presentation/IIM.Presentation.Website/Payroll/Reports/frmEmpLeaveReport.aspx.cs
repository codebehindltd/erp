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
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmEmpLeaveReport : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadLeaveType();
                LoadDepartment();
                LoadYearList();
            }
        }
        private void LoadLeaveType()
        {
            LeaveTypeDA entityDA = new LeaveTypeDA();
            ddlLeaveTypeId.DataSource = entityDA.GetLeaveTypeInfo();
            ddlLeaveTypeId.DataTextField = "TypeName";
            ddlLeaveTypeId.DataValueField = "LeaveTypeId";
            ddlLeaveTypeId.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            //item.Value = "---None---";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlLeaveTypeId.Items.Insert(0, item);
        }
        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            ddlDepartmentId.DataSource = entityDA.GetDepartmentInfo();
            ddlDepartmentId.DataTextField = "Name";
            ddlDepartmentId.DataValueField = "DepartmentId";
            ddlDepartmentId.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlDepartmentId.Items.Insert(0, item);
        }
        private void LoadYearList()
        {
            ddlYear.DataSource = hmUtility.GetReportYearList();
            ddlYear.DataBind();
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            dispalyReport = 1;
            string departmentId = string.Empty, empId = string.Empty, leaveType = string.Empty, reportType = string.Empty;
            int yearId = 0;

            reportType = ddlReportType.SelectedValue;
            yearId = Convert.ToInt32(ddlYear.SelectedValue);

            int companyId = 0;
            string companyName = "All";
            int projectId = 0;
            string projectName = "All";

            if (hfCompanyId.Value != "0" && hfCompanyId.Value != "")
            {
                companyId = Convert.ToInt32(hfCompanyId.Value);
                companyName = hfCompanyName.Value;
            }

            if (hfProjectId.Value != "0" && hfProjectId.Value != "")
            {
                projectId = Convert.ToInt32(hfProjectId.Value);
                projectName = hfProjectName.Value;
            }

            HiddenField hfEmployeeId = (HiddenField)this.employeeeSearch.FindControl("hfEmployeeId");

            if (this.ddlEmployee.SelectedValue == "0")
            {
                hfEmployeeId.Value = "0";
            }

            if (hfEmployeeId.Value != "0")
                empId = hfEmployeeId.Value;

            if (ddlDepartmentId.SelectedValue != "0")
                departmentId = ddlDepartmentId.SelectedValue;

            if (ddlLeaveTypeId.SelectedValue != "0")
                leaveType = ddlLeaveTypeId.SelectedValue;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = string.Empty;

            if (reportType == "AllIndividual")
            {
                if(hfEmpId.Value == "0" || ddlEmployee.SelectedValue == "0")
                {
                    if (ddlLeaveTypeId.SelectedValue == "0")
                    {
                        reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptLeaveInformation.rdlc");
                    }
                    else
                    {
                        reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptLeaveInformationType.rdlc");
                    }
                }
                else
                {
                    if (ddlLeaveTypeId.SelectedValue == "0")
                    {
                        reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptLeaveInformationIndividual.rdlc");
                    }
                    else
                    {
                        reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptLeaveInformationTypeIndividual.rdlc");
                    }
                }
                
            }
            else if (reportType == "YearlyLeave")
            {
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptLeaveYearlyInformation.rdlc");
                //if(hfEmpId.Value == "0")
                //{
                //    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptLeaveYearlyInformation.rdlc");
                //}
                //else
                //{
                //    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptLeaveYearlyInformationIndividual.rdlc");
                //}
            }

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

            reportParam.Add(new ReportParameter("ReportType", ddlReportType.SelectedItem.Text));
            reportParam.Add(new ReportParameter("Year", ddlYear.SelectedItem.Text));
            if(ddlEmployee.SelectedValue == "0")
            {
                reportParam.Add(new ReportParameter("EmployeeType", "All"));
                hfEmpId.Value = "0";
            }
            else
            {
                reportParam.Add(new ReportParameter("EmployeeType", "Individual"));
                reportParam.Add(new ReportParameter("EmployeeId", hfEmpId.Value));
                reportParam.Add(new ReportParameter("EmployeeName", hfEmployeeName.Value));
            }

            EmployeeDA employeeDa = new EmployeeDA();
            EmployeeBO employeeBo = new EmployeeBO();
            employeeBo = employeeDa.GetEmployeeInfoByCode(hfEmpId.Value);
            if (hfEmpId.Value == "0")
            {
                if (ddlDepartmentId.SelectedValue == "0")
                {
                    reportParam.Add(new ReportParameter("Department", "All"));
                }
                else
                {
                    reportParam.Add(new ReportParameter("Department", ddlDepartmentId.SelectedItem.Text));
                }
            }
            else
            {
                reportParam.Add(new ReportParameter("Department", employeeBo.Department));
            }

            if(ddlLeaveTypeId.SelectedValue == "0")
            {
                reportParam.Add(new ReportParameter("LeaveType", "All"));
            }
            else
            {
                reportParam.Add(new ReportParameter("LeaveType", ddlLeaveTypeId.SelectedItem.Text));
            }

            rvTransaction.LocalReport.SetParameters(reportParam);

            LeaveInformationDA eveDA = new LeaveInformationDA();
            List<LeaveInformationBO> leaveInformationList = new List<LeaveInformationBO>();
            

            if (reportType == "AllIndividual")
            {
                leaveInformationList = eveDA.GetEmpLeaveInformationReport(companyId, projectId, empId, leaveType, departmentId);
            }
            else if (reportType == "YearlyLeave")
            {
                departmentId = departmentId == string.Empty ? "0" : departmentId;
                leaveInformationList = eveDA.GetLeaveDetailsYearlyReport(yearId, Convert.ToInt32(departmentId));
            }

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], leaveInformationList));

            rvTransaction.LocalReport.DisplayName = "Leave Information";

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