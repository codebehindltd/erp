using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.TaskManagment;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.TaskManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.TaskManagement.Reports
{
    public partial class ReportAssignTask : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadGetAssignTo();
                FormValidation();
            }
        }
        protected void FormValidation()
        {

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (!userInformationBO.IsAdminUser)
            {
                AssignTo.Visible = false;
            }
        }
        private void LoadGetAssignTo()
        {
            EmployeeDA EmpDA = new EmployeeDA();
            List<EmployeeBO> EmpBO = new List<EmployeeBO>();
            EmpBO = EmpDA.GetEmployeeInfo();

            ddlSearchAssignTo.DataSource = EmpBO;
            ddlSearchAssignTo.DataTextField = "DisplayName";
            ddlSearchAssignTo.DataValueField = "EmpId";
            ddlSearchAssignTo.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            string empList = hfEmpId.Value;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (!userInformationBO.IsAdminUser)
            {

                if (userInformationBO.EmpId == 0 || userInformationBO.EmpId == null)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Employee is not mapped with current user!!Please Contact with Admin!!", AlertType.Warning);
                    //Response.Redirect(userInformationBO.InnboardHomePage);
                }
                else
                {
                    empList = userInformationBO.EmpId.ToString();
                    ReportLoad(empList);
                }
            }
            else
            {
                ReportLoad(empList);
            }

        }
        protected void ReportLoad(string empList)
        {
            string taskName = txtTaskNameForSearch.Text;
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;

            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;
            
            if (string.IsNullOrEmpty(txtSearchFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtSearchFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtSearchFromDate.Text;
            }

            if (string.IsNullOrEmpty(txtSearchToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtSearchToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = txtSearchToDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            var reportPath = "";
            if (!userInformationBO.IsAdminUser)
            {
                reportPath = Server.MapPath(@"~/TaskManagement/Reports/Rdlc/rptAssignTaskForNonAdminUser.rdlc");
            }
            else
            {
                reportPath = Server.MapPath(@"~/TaskManagement/Reports/Rdlc/rptReportAssignTask.rdlc");
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

                reportParam.Add(new ReportParameter("startDate", startDate));
                reportParam.Add(new ReportParameter("endDate", endDate));

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
            EmployeeBO Employee = new EmployeeBO();
            EmployeeDA DA = new EmployeeDA();
            Employee = DA.GetEmployeeInfoById(userInformationBO.EmpId);
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("EmployeeName", Employee.DisplayName));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            rvTransaction.LocalReport.EnableExternalImages = true;
            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            List<SMTask> taskList = new List<SMTask>();
            AssignTaskDA taskDA = new AssignTaskDA();
            taskList = taskDA.GetTaskInformationForReport(taskName, empList, FromDate, ToDate);

            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], taskList));
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