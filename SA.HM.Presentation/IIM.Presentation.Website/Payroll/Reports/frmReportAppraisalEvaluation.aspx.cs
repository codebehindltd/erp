using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportAppraisalEvaluation : BasePage
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
            }
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
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlDepartment.Items.Insert(0, item);
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            dispalyReport = 1;

            string fromDate = string.Empty, toDate = string.Empty, appraisalType = string.Empty, departmentId = string.Empty, empId = string.Empty;
            DateTime dateTime = DateTime.Now;
            DateTime? FromDate = null, ToDate = null;
            if (!String.IsNullOrEmpty(txtFromDate.Text))
                fromDate = txtFromDate.Text;

            if (!String.IsNullOrEmpty(txtToDate.Text))
                toDate = txtToDate.Text;
            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {

                fromDate = txtFromDate.Text;
                FromDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            }

            if (!string.IsNullOrEmpty(txtToDate.Text))
            {

                toDate = txtToDate.Text;
                ToDate = hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            }

            departmentId = ddlDepartment.SelectedValue == "0" ? string.Empty : ddlDepartment.SelectedValue;
            appraisalType = ddlAppraisalType.SelectedValue == "0" ? string.Empty : ddlAppraisalType.SelectedValue;

            ContentPlaceHolder mpContentPlaceHolder;
            UserControl employeeForLoanSearch;

            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            employeeForLoanSearch = (UserControl)mpContentPlaceHolder.FindControl("EmployeeSearch");

            empId = ((HiddenField)employeeForLoanSearch.FindControl("hfEmployeeId")).Value;
            empId = empId == "0" ? string.Empty : empId;
            if (ddlEmployee.SelectedValue == "0")
                empId = string.Empty;
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptAppraisalEvaluation.rdlc");

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

            AppraisalEvaluationDA appEvaDA = new AppraisalEvaluationDA();
            List<AppraisalEvaluationReportViewBO> viewList = new List<AppraisalEvaluationReportViewBO>();
            viewList = appEvaDA.GetAppraisalEvaluationForReport(departmentId, appraisalType, empId, FromDate, ToDate);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewList));

            rvTransaction.LocalReport.DisplayName = "Appraisal Evaluation";

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