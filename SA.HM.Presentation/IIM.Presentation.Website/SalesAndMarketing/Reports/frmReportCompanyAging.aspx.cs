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
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;

namespace HotelManagement.Presentation.Website.SalesAndMarketing.Reports
{
    public partial class frmReportCompanyAging : BasePage
    {
        HMUtility hmUtility = new HMUtility();

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = "";
            if (ddlReportType.SelectedValue == "1")
            {
                reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/rptCompanyARAgingTop.rdlc");
            }
            else if (ddlReportType.SelectedValue == "2")
            {
                reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/rptCompanyARAgingSummary.rdlc");
            }
            else if (ddlReportType.SelectedValue == "3")
            {
                reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/rptCompanyARAgingDetail.rdlc");
            }

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            int intervalBands = 30;
            DateTime dateTime = DateTime.Now;
            string strAsOfDate = string.Empty, intervalType = "Day", reportType = string.Empty;

            if (string.IsNullOrWhiteSpace(txtAsOfDate.Text))
            {
                strAsOfDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtAsOfDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                strAsOfDate = txtAsOfDate.Text;
            }

            reportType = ddlReportFor.SelectedValue;

            if (!string.IsNullOrWhiteSpace(txtIntervalBands.Text))
            {
                intervalBands = Convert.ToInt32(txtIntervalBands.Text);
            }

            intervalType = ddlIntervalType.SelectedValue;

            DateTime asOfDate = hmUtility.GetDateTimeFromString(strAsOfDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            Int32 companyId = Convert.ToInt32(hfCompanyId.Value);

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

            GuestCompanyDA commonReportDa = new GuestCompanyDA();
            List<CompanyPaymentLedgerReportVwBo> companyARAging = new List<CompanyPaymentLedgerReportVwBo>();

            if (ddlReportType.SelectedValue == "3")
            {
                companyARAging = commonReportDa.GetCompanyARAgingDetail(reportType, companyId, asOfDate, intervalBands, intervalType);
            }
            else
            {
                companyARAging = commonReportDa.GetCompanyARAging(reportType, companyId, asOfDate, intervalBands, intervalType);
            }

            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], companyARAging));

            rvTransaction.LocalReport.DisplayName = "Company A/R Aging";
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
        public static List<GuestCompanyBO> GetCompanyData(string searchText)
        {
            List<GuestCompanyBO> companyList = new List<GuestCompanyBO>();
            GuestCompanyDA suppDa = new GuestCompanyDA();

            companyList = suppDa.GetCompanyInfoBySearchCriteria(searchText);

            return companyList;
        }

    }
}