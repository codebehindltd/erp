using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportMarketSegmentWiseInformation : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int _RoomStatusInfoByDate = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGuestReference();
                LoadYearDropDown();
            }
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
            string FilterBy = string.Empty;
            string reportType = string.Empty;
            int refferenceId = 0;
            DateTime fromMonth, toMonth;

            reportType = ddlReportType.SelectedValue;

            if (reportType == "1")
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/RptMarketSegmentMonthWise.rdlc");
            }
            else if (reportType == "2")
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/RptMarketSegmentDtDMtDYtD.rdlc");
            }
            else
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptSalesPersonWiseDetailsRoomInformation.rdlc");
            }

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

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            _RoomStatusInfoByDate = 1;
            HMCommonDA hmCommonDA = new HMCommonDA();
            SalesAuditDA salesAuditDA = new SalesAuditDA();

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");

            rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("ReportDate", printDate));

            rvTransaction.LocalReport.SetParameters(paramReport);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();

            if (reportType == "1")
            {
                DateTime year = DateTime.ParseExact(ddlYear.SelectedValue, "yyyy", CultureInfo.CurrentCulture).Date;

                if (!string.IsNullOrWhiteSpace(txtFromMonth.Text))
                    fromMonth = DateTime.ParseExact(txtFromMonth.Text, "MMMM", CultureInfo.CurrentCulture).Date;
                else
                    fromMonth = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(txtToMonth.Text))
                    toMonth = DateTime.ParseExact(txtToMonth.Text, "MMMM", CultureInfo.CurrentCulture).Date;
                else
                    toMonth = DateTime.Now;

                FilterBy = ddlMonthWiseFilterBy.SelectedValue.ToString();

                List<MonthWiseMarketSegmentReportViewBO> roomInfo = new List<MonthWiseMarketSegmentReportViewBO>();
                roomInfo = salesAuditDA.GetMonthWiseMarketSegmentRoomInformation(FilterBy, year, fromMonth, toMonth);

                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], roomInfo));
            }
            else if (reportType == "2")
            {
                if (!string.IsNullOrWhiteSpace(txtSearchDate.Text))
                    fromMonth = hmUtility.GetDateTimeFromString(txtSearchDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
               // fromMonth = CommonHelper.DateTimeToMMDDYYYY(txtSearchDate.Text, userInfo.ServerDateFormat);
                else
                    fromMonth = DateTime.Now;

                FilterBy = ddlMtdTydType.SelectedValue.ToString();

                List<DtDMtDYtDMarketSegmentReportViewBO> dataList = new List<DtDMtDYtDMarketSegmentReportViewBO>();
                dataList = salesAuditDA.GetMarketSegmentRoomInformationForDtDMtDYtDReport(FilterBy, fromMonth);
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], dataList));
            }
            else
            {
                DateTime year = DateTime.ParseExact(ddlYear.SelectedValue, "yyyy", CultureInfo.CurrentCulture).Date;

                if (!string.IsNullOrWhiteSpace(txtFromMonth.Text))
                    fromMonth = DateTime.ParseExact(txtFromMonth.Text, "MMMM", CultureInfo.CurrentCulture).Date;
                else
                    fromMonth = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(txtToMonth.Text))
                    toMonth = DateTime.ParseExact(txtToMonth.Text, "MMMM", CultureInfo.CurrentCulture).Date;
                else
                    toMonth = DateTime.Now;

                refferenceId = Convert.ToInt32(ddlRefferenceBy.SelectedValue);

                List<MonthWiseMarketSegmentReportViewBO> dataList = new List<MonthWiseMarketSegmentReportViewBO>();
                dataList = salesAuditDA.GetSalesPersonWiseMarketSegmentRoomInformation(year, fromMonth, toMonth, refferenceId);
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], dataList));
            }

            rvTransaction.LocalReport.DisplayName = "Market Segment Wise Report";
            rvTransaction.LocalReport.Refresh();
            frmPrint.Attributes["src"] = "";
        }

        public void LoadGuestReference()
        {
            GuestReferenceDA entityDA = new GuestReferenceDA();
            List<GuestReferenceBO> files = entityDA.GetAllGuestRefference();
            ddlRefferenceBy.DataSource = files;
            ddlRefferenceBy.DataTextField = "Name";
            ddlRefferenceBy.DataValueField = "ReferenceId";
            ddlRefferenceBy.DataBind();

            ListItem itemReference = new ListItem();
            itemReference.Value = "0";
            itemReference.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlRefferenceBy.Items.Insert(0, itemReference);
        }

        public void LoadYearDropDown()
        {
            int currentYear = DateTime.Now.Year;

            var list = new List<object>();

            for (int i = 0; i < 5; i++)
            {
                ListItem years = new ListItem();
                years.Text = currentYear.ToString();
                years.Value = currentYear.ToString();
                list.Add(years);
                currentYear--;
            }

            ddlYear.DataSource = list;
            ddlYear.DataBind();
        }
    }
}