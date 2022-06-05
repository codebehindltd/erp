using HotelManagement.Data.DocumentManagement;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.DocumentManagement;
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

namespace HotelManagement.Presentation.Website.DocumentManagement.Reports
{
    public partial class rptDocumentManagementReport : System.Web.UI.Page
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
            string assignToId = "";
            assignToId = hfEmpId.Value;
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
                    assignToId = userInformationBO.EmpId.ToString();
                    ReportLoad(assignToId);
                }
            }
            else
            {
                ReportLoad(assignToId);
            }
        }
        protected void ReportLoad(string assignToId)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            string documentName = "";
            documentName = txtDocumentNameForSearch.Text;

            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;

            if (string.IsNullOrWhiteSpace(txtSearchFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtSearchFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtSearchFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(txtSearchToDate.Text))
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
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;
            var reportPath = "";
            if (!userInformationBO.IsAdminUser)
            {
                reportPath = Server.MapPath(@"~/DocumentManagement/Reports/Rdlc/rptDocumentManagementForNonAdminUser.rdlc");
            }
            else
            {
                reportPath = Server.MapPath(@"~/DocumentManagement/Reports/Rdlc/rptDocumentManagement.rdlc");
            }

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> paramReport = new List<ReportParameter>();
            EmployeeBO Employee = new EmployeeBO();
            EmployeeDA DA = new EmployeeDA();
            Employee = DA.GetEmployeeInfoById(userInformationBO.EmpId);
            if (files[0].CompanyId > 0)
            {
                paramReport.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                paramReport.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));
                paramReport.Add(new ReportParameter("startDate", startDate));
                paramReport.Add(new ReportParameter("endDate", endDate));
                paramReport.Add(new ReportParameter("EmployeeName", Employee.DisplayName));
                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    paramReport.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }
                else
                {
                    paramReport.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                }
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);            

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvTransaction.LocalReport.SetParameters(paramReport);

            DocumentsForDocManagementDA docDA = new DocumentsForDocManagementDA();
            List<DMDocumentBO> docBO = new List<DMDocumentBO>();
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDocumentInformationRestrictedForAllUsers", "IsDocumentInformationRestrictedForAllUsers");
            if (setUpBO.SetupValue == "1")
            {
                assignToId = userInformationBO.EmpId.ToString();
            }
            docBO = docDA.GetDocumentForReport(documentName, assignToId, FromDate, ToDate);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], docBO));

            rvTransaction.LocalReport.DisplayName = "Documents";
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