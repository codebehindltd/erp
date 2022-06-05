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
    public partial class frmReportEmployeeLedger : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                FormValidation();
            }
        }
        protected void FormValidation()
        {
            Boolean IsAdminUser = false;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                IsAdminUser = true;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 18).Count() > 0)
                    {
                        IsAdminUser = true;
                    }
                }
            }
            #endregion

            companysearch.Visible = IsAdminUser;
            reportType.Visible = IsAdminUser;
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            string startDate = string.Empty, endDate = string.Empty, paymentStatus = string.Empty, reportType = string.Empty;
            DateTime dateTime = DateTime.Now;
            Int32 employeeId = Convert.ToInt32(hfCompanyId.Value);
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
                    employeeId = userInformationBO.EmpId;
                    ReportLoad(employeeId);
                }
            }
            else
            {
                ReportLoad(employeeId);
            }
        }
        protected void ReportLoad(int employeeId)
        {
            string startDate = string.Empty, endDate = string.Empty, paymentStatus = string.Empty, reportType = string.Empty;
            DateTime dateTime = DateTime.Now;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = "";

            reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmployeePaymentLedger.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

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


            if (string.IsNullOrWhiteSpace(txtDateFrom.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtDateFrom.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtDateFrom.Text;
            }
            if (string.IsNullOrWhiteSpace(txtEndDateTo.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtEndDateTo.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = txtEndDateTo.Text;
            }


            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);


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

            paymentStatus = ddlPaymentStatus.SelectedValue;


            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            rvTransaction.LocalReport.SetParameters(reportParam);

            EmployeeDA commonReportDa = new EmployeeDA();
            List<EmployeePaymentLedgerReportVwBo> companyLedger = new List<EmployeePaymentLedgerReportVwBo>();

            reportType = ddlReportType.SelectedValue;
            companyLedger = commonReportDa.GetEmployeeLedger(companyId, projectId, employeeId, FromDate, ToDate, paymentStatus, reportType);

            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], companyLedger));

            rvTransaction.LocalReport.DisplayName = "Employee Ledger";
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