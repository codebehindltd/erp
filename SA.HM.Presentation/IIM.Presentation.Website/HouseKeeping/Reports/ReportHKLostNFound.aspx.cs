using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HouseKeeping;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HouseKeeping;
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

namespace HotelManagement.Presentation.Website.HouseKeeping.Reports
{
    public partial class ReportHKLostNFound : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int _RoomStatusInfoByDate = -1;
        bool IsPayrollIntegrateWithFrontOffice = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadIsPayrollIntegrateWithFrontOffice();
            if (IsPayrollIntegrateWithFrontOffice)
            {
                LoadEmployee();

            }
        }
        private void LoadEmployee()
        {
            EmployeeDA employeeDA = new EmployeeDA();

            ddlFoundPersonSrc.DataSource = employeeDA.GetEmployeeInfo();
            ddlFoundPersonSrc.DataTextField = "EmployeeName";
            ddlFoundPersonSrc.DataValueField = "EmpId";
            ddlFoundPersonSrc.DataBind();

            ListItem itemEmployee = new ListItem();
            itemEmployee.Value = "0";
            itemEmployee.Text = "--Please Select--";
            ddlFoundPersonSrc.Items.Insert(0, itemEmployee);

        }
        private void LoadIsPayrollIntegrateWithFrontOffice()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollIntegrateWithFrontOffice", "IsPayrollIntegrateWithFrontOffice");
            hfIsPayrollIntegrateWithFrontOffice.Value = "0";
            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        hfIsPayrollIntegrateWithFrontOffice.Value = "1";
                        IsPayrollIntegrateWithFrontOffice = true;
                    }
                    else
                    {
                        hfIsPayrollIntegrateWithFrontOffice.Value = "0";
                        IsPayrollIntegrateWithFrontOffice = false;
                    }
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _RoomStatusInfoByDate = 1;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            string startDate = string.Empty;
            string endDate = string.Empty;
            DateTime currentDate = DateTime.Now;

            //inputs
            string itemName = txtItemNameSrc.Text;
            string itemType = ddlItemTypeSrc.SelectedValue;
            int transectionId = 0;
            string transectionType = ddlTransectionTypeSrc.SelectedValue;
            string hasReturned = ddlHasReturned.SelectedValue;

            int whoFoundIt = 0;

            if (hfIsPayrollIntegrateWithFrontOffice.Value == "1")
            {
                whoFoundIt = Convert.ToInt32(ddlFoundPersonSrc.SelectedValue);
            }
            
            string whoFoundItName = txtFoundPersonSrc.Text;
            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(currentDate);
                this.txtFromDate.Text = hmUtility.GetStringFromDateTime(currentDate);
            }
            else
            {
                startDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(currentDate);
                this.txtToDate.Text = hmUtility.GetStringFromDateTime(currentDate);
            }
            else
            {
                endDate = this.txtToDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            ToDate = ToDate.AddDays(1).AddSeconds(-1);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            HMCommonDA hmCommonDA = new HMCommonDA();
            var reportPath = "";

            reportPath = Server.MapPath(@"~/HouseKeeping/Reports/Rdlc/rptLostNFound.rdlc");

            if (!File.Exists(reportPath))
                return;
            rvTransaction.LocalReport.ReportPath = reportPath;
            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> paramReport = new List<ReportParameter>();

            if (files[0].CompanyId > 0)
            {
                paramReport.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                paramReport.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    paramReport.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }
                else
                {
                    paramReport.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                }
            }
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            rvTransaction.LocalReport.SetParameters(paramReport);
            LostFoundDA foundDA = new LostFoundDA();
            List<LostFoundBO> infoBOs = new List<LostFoundBO>();

            infoBOs = foundDA.GetLostFoundInfoForReport(itemName, itemType, hasReturned, transectionType, transectionId, FromDate, ToDate, whoFoundIt, whoFoundItName);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], infoBOs));

            rvTransaction.LocalReport.DisplayName = "Lost & Found Information";
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