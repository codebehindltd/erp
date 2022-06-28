using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
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

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportCashRequisitionNAdjustment : BasePage
    {
        protected int _CashRequisitionShow = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployee();
            }
        }

        private void LoadEmployee()
        {
            EmployeeDA EmpDA = new EmployeeDA();
            List<EmployeeBO> EmpBO = new List<EmployeeBO>();
            EmpBO = EmpDA.GetEmployeeInfo();

            ddlAssignEmployee.DataSource = EmpBO;
            ddlAssignEmployee.DataTextField = "DisplayName";
            ddlAssignEmployee.DataValueField = "EmpId";
            ddlAssignEmployee.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlAssignEmployee.Items.Insert(0, item);
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            _CashRequisitionShow = 1;

            int companyId = 0;
            string ddlCompanyName = "All";
            int projectId = 0;
            string ddlProjectName = "All";

            if (hfCompanyId.Value != "0" && hfCompanyId.Value != "")
            {
                companyId = Convert.ToInt32(hfCompanyId.Value);
                //ddlCompanyName = hfCompanyName.Value;
            }

            if (hfProjectId.Value != "0" && hfProjectId.Value != "")
            {
                projectId = Convert.ToInt32(hfProjectId.Value);
                //ddlProjectName = hfProjectName.Value;
            }

            //int companyId = 0;
            //int projectId = 0;

            //if (hfCompanyId.Value != "0" && hfCompanyId.Value != "")
            //{
            //    companyId = Convert.ToInt32(hfCompanyId.Value);
            //}

            //if (hfProjectId.Value != "0" && hfProjectId.Value != "")
            //{
            //    projectId = Convert.ToInt32(hfProjectId.Value);
            //}

            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            int employeeId = Convert.ToInt32(ddlAssignEmployee.SelectedValue);
            var transactionTypeId = ddlTransactionType.SelectedValue;
            var CRFromDate = txtSearchFromDate.Text;
            var CRToDate = txtSearchToDate.Text;
            var fromAmount = txtSearchFromAmount.Text;
            var toAmount = txtSearchToAmount.Text;
            var transactionNo = txtSrcTransactionNo.Text;
            var adjustmentNo = txtSrcAdjustmentNo.Text;
            var statusId = ddlSearchStatus.SelectedValue;
            var remarks = txtRemarks.Text;

            DateTime? fromDate = null;
            DateTime? toDate = null;

            if (!string.IsNullOrWhiteSpace(CRFromDate))
            {
                fromDate = hmUtility.GetDateTimeFromString(CRFromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            if (!string.IsNullOrWhiteSpace(CRToDate))
            {
                toDate = hmUtility.GetDateTimeFromString(CRToDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            string reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptCashRequisitionNAdjustment.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            List<ReportParameter> paramReport = new List<ReportParameter>();

            string companyName = string.Empty;
            string companyAddress = string.Empty;
            string webAddress = string.Empty;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
            if (files[0].CompanyId > 0)
            {
                companyName = files[0].CompanyName;
                companyAddress = files[0].CompanyAddress;

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    webAddress = files[0].WebAddress;
                }
                else
                {
                    webAddress = files[0].ContactNumber;
                }
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;

            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));

            string imageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("CompanyProfile", companyName));
            paramReport.Add(new ReportParameter("CompanyAddress", companyAddress));
            paramReport.Add(new ReportParameter("CompanyWeb", webAddress));
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));

            rvTransaction.LocalReport.SetParameters(paramReport);

            List<CashRequisitionBO> viewList = new List<CashRequisitionBO>();
            CashRequisitionDA cashRequisitionDa = new CashRequisitionDA();
            viewList = cashRequisitionDa.GetCashRequisitionNAdjustmentForReport(companyId, projectId, employeeId, transactionTypeId, fromDate, toDate, fromAmount, toAmount, transactionNo, adjustmentNo, statusId, remarks);
            
            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewList));

            rvTransaction.LocalReport.DisplayName = "Cash Requisition And Adjustment";
            rvTransaction.LocalReport.Refresh();
        }

        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {

        }
    }
}