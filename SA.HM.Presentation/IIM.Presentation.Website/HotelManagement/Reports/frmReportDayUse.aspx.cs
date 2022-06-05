using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using System.IO;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportDayUse : BasePage
    {
        int _offset = -360;
        int _mindiff = 0;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ddlReportType.Visible = false;
            rvTransaction.LocalReport.EnableExternalImages = true;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty;
            string endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
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

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptDayUseReport.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            _RoomStatusInfoByDate = 1;
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            List<ReportParameter> paramReport = new List<ReportParameter>();
            //List<ReportParameter> paramCompanyProfile = new List<ReportParameter>();
            //List<ReportParameter> paramCompanyAddress = new List<ReportParameter>();
            //List<ReportParameter> paramPrintDate = new List<ReportParameter>();
            //List<ReportParameter> paramFromToDate = new List<ReportParameter>();

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("CompanyProfile", hmUtility.GetHMCompanyProfile()));
            paramReport.Add(new ReportParameter("CompanyAddress", hmUtility.GetHMCompanyAddress()));
            paramReport.Add(new ReportParameter("PrintDate", hmUtility.GetPrintDate()));
            paramReport.Add(new ReportParameter("FromToDate", hmUtility.GetFromDateAndToDate(FromDate, ToDate)));
            paramReport.Add(new ReportParameter("ReportType", this.ddlReportType.SelectedValue));

            //List<ReportParameter> paramPrintDateTime = new List<ReportParameter>();
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            rvTransaction.LocalReport.SetParameters(paramReport);

            //rvTransaction.LocalReport.SetParameters(paramImagePath);
            //rvTransaction.LocalReport.SetParameters(paramCompanyProfile);
            //rvTransaction.LocalReport.SetParameters(paramCompanyAddress);
            //rvTransaction.LocalReport.SetParameters(paramPrintDate);
            //rvTransaction.LocalReport.SetParameters(paramFromToDate);
            //TransactionDataSource.SelectParameters[0].DefaultValue = this.ddlReportType.SelectedValue;
            //TransactionDataSource.SelectParameters[1].DefaultValue = FromDate.ToString();
            //TransactionDataSource.SelectParameters[2].DefaultValue = ToDate.AddDays(1).AddSeconds(-1).ToString();
            string reportType = this.ddlReportType.SelectedValue;

            HotelGuestDayLetCheckOutDA dayUseDA = new HotelGuestDayLetCheckOutDA();
            List<LateChkOutReportViewBO> dayUseBO = new List<LateChkOutReportViewBO>();
            dayUseBO = dayUseDA.GetDayLateCheckOut(reportType, FromDate, ToDate);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], dayUseBO));

            rvTransaction.LocalReport.DisplayName = "Day Use Information";
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