using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
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

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportSettlementDetails : BasePage
    {
        HMUtility hmUtility = new HMUtility();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {

        }

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            UserInformationBO userInfo = new UserInformationBO();
            userInfo = hmUtility.GetCurrentApplicationUserInfo();

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";

            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptSettlementDetails.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> paramReport = new List<ReportParameter>();


            if (files[0].CompanyId > 0)
            {
                paramReport.Add(new ReportParameter("CompanyName", files[0].CompanyName));
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

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");

            rvTransaction.LocalReport.EnableExternalImages = true;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("PrintDate", printDate));
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            rvTransaction.LocalReport.SetParameters(paramReport);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();

            List<SettlementDetailsReportViewBO> detailsList = new List<SettlementDetailsReportViewBO>();
            SettlementDetailsDA settlementDA = new SettlementDetailsDA();
            DateTime searchDate;

            if (!string.IsNullOrWhiteSpace(txtSearchDate.Text))
                searchDate = hmUtility.GetDateTimeFromString(txtSearchDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                //searchDate = CommonHelper.DateTimeToMMDDYYYY(txtSearchDate.Text, userInfo.ServerDateFormat);
            else
                searchDate = DateTime.Today;
            string filterType = ddlFilterType.SelectedValue.ToString();

            detailsList = settlementDA.GetSettlementDetailsForReport(filterType, searchDate);
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], detailsList));

            rvTransaction.LocalReport.DisplayName = "Market Segment Wise Report";
            rvTransaction.LocalReport.Refresh();
            frmPrint.Attributes["src"] = "";

        }
    }
}