using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.Inventory.Reports
{
    public partial class frmDailyConsolidatedRevenueCostCenterSalesDetailByDineTime : BasePage
    {
        protected int _ReportShow = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            _ReportShow = 1;
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            if (string.IsNullOrWhiteSpace(txtStartDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtStartDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtStartDate.Text;
            }
            if (string.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtEndDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = txtEndDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptDailyConsolidatedRevenueCostCenterSalesDetailByDineTime.rdlc");

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

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            string reportName = "Daily Consolidated Revenue By Dine Time";

            reportParam.Add(new ReportParameter("ReportName", reportName));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> dinnerTim = new List<CustomFieldBO>();
            dinnerTim = commonDA.GetCustomField("DinnerTime");

            List<CustomFieldBO> SanckTime = new List<CustomFieldBO>();
            SanckTime = commonDA.GetCustomField("SanckTime");

            List<CustomFieldBO> LunchTime = new List<CustomFieldBO>();
            LunchTime = commonDA.GetCustomField("LunchTime");
        
            reportParam.Add(new ReportParameter("DinnerTimeFrom", Convert.ToDateTime(dinnerTim[0].FieldValue.Split('~')[0]).ToString(userInformationBO.TimeFormat)));
            reportParam.Add(new ReportParameter("DinnerTimeTo", Convert.ToDateTime(dinnerTim[0].FieldValue.Split('~')[1]).ToString(userInformationBO.TimeFormat)));
            reportParam.Add(new ReportParameter("SnacksTimeFrom", Convert.ToDateTime(SanckTime[0].FieldValue.Split('~')[0]).ToString(userInformationBO.TimeFormat)));
            reportParam.Add(new ReportParameter("SnacksTimeTo", Convert.ToDateTime(SanckTime[0].FieldValue.Split('~')[1]).ToString(userInformationBO.TimeFormat)));
            reportParam.Add(new ReportParameter("LunchTimeFrom", Convert.ToDateTime(LunchTime[0].FieldValue.Split('~')[0]).ToString(userInformationBO.TimeFormat)));
            reportParam.Add(new ReportParameter("LunchTimeTo", Convert.ToDateTime(LunchTime[0].FieldValue.Split('~')[1]).ToString(userInformationBO.TimeFormat)));

            reportParam.Add(new ReportParameter("ReportDateFrom", startDate));
            reportParam.Add(new ReportParameter("ReportDateTo", endDate));

            rvTransaction.LocalReport.SetParameters(reportParam);

            AllInventoryReportDA allInventoryReportDA = new AllInventoryReportDA();
            DailyConsolidatedRevenueCenterSalesDetailViewBO consolidatedRevenue = new DailyConsolidatedRevenueCenterSalesDetailViewBO();
            consolidatedRevenue = allInventoryReportDA.DailyConsolidatedRevenueCostCenterSalesDetailByDineTime(FromDate, ToDate);

            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            //---- Lunch
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], consolidatedRevenue.CostcenterWisesalesDetailsLunch));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[1], consolidatedRevenue.SalesDetailsLunch));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[2], consolidatedRevenue.PaymentDetailsLunch));

            //---- Snacks
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[3], consolidatedRevenue.CostcenterWisesalesDetailsSnacks));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[4], consolidatedRevenue.SalesDetailsSnacks));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[5], consolidatedRevenue.PaymentDetailsSnacks));

            //---- Dinner
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[6], consolidatedRevenue.CostcenterWisesalesDetailsDinner));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[7], consolidatedRevenue.SalesDetailsDinner));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[8], consolidatedRevenue.PaymentDetailsDinner));

            rvTransaction.LocalReport.DisplayName = reportName;
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