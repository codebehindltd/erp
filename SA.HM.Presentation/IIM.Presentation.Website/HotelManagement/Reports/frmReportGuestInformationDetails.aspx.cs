using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using Microsoft.Reporting.WebForms;
using System.Data;
using HotelManagement.Data.HotelManagement;
using System.IO;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportGuestInformationDetails : BasePage
    {
        int _offset = -360;
        int _mindiff = 0;
        HiddenField innboardMessage;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
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

            if (ddlReportType.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Report Type.", AlertType.Warning);
                this.ddlReportType.Focus();
                return;
            }

            string reportType = ddlReportType.SelectedValue.ToString();
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            if (reportType == "SB")
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestInformationSbReport.rdlc");
            }
            else if (reportType == "NRB")
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestInformationNrbReport.rdlc");
            }
            else if (reportType == "Police")
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestInformationPoliceReport.rdlc");
            }

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

            List<ReportParameter> reportParam = new List<ReportParameter>();
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("CompanyProfile", hmUtility.GetHMCompanyProfile()));
            reportParam.Add(new ReportParameter("CompanyAddress", hmUtility.GetHMCompanyAddress()));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("PrintDate", printDate));
            reportParam.Add(new ReportParameter("FromToDate", " Date From : " + FromDate.ToString("dd/MM/yyyy") + "  Date To : " + ToDate.ToString("dd/MM/yyyy")));
            reportParam.Add(new ReportParameter("ReportType", this.ddlReportType.SelectedValue));
            rvTransaction.LocalReport.SetParameters(reportParam);

            GuestInformationDA guestInfoDA = new GuestInformationDA();
            List<SBReportViewBO> guestInfoBO = new List<SBReportViewBO>();
            guestInfoBO = guestInfoDA.GetGuestInformationDetails(FromDate, ToDate, reportType);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], guestInfoBO));

            rvTransaction.LocalReport.DisplayName = "Guest Information";
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
        protected void btnExportCsv_Click(object sender, EventArgs e)
        {
            int[] iColumns;
            string[] sHeaders;
            DateTime dateTime = DateTime.Now;
            DataTable dtCsv = new DataTable();
            string reportType = ddlReportType.SelectedValue.ToString();
            GuestInformationDA guestDa = new GuestInformationDA();
            dtCsv = guestDa.GetGuestInformationDetailsByDateRange(hmUtility.GetDateTimeFromString(txtFromDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat), hmUtility.GetDateTimeFromString(txtToDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat), reportType);


            if (dtCsv != null && dtCsv.Rows.Count > 0)
            {
                iColumns = new int[dtCsv.Columns.Count];
                sHeaders = new string[dtCsv.Columns.Count];

                if (reportType == "SB")
                {
                    iColumns.SetValue(0, 0);
                    iColumns.SetValue(1, 1);
                    iColumns.SetValue(2, 2);
                    iColumns.SetValue(3, 3);
                    iColumns.SetValue(4, 4);
                    iColumns.SetValue(5, 5);
                    iColumns.SetValue(6, 6);
                    iColumns.SetValue(7, 7);
                    iColumns.SetValue(8, 8);
                    iColumns.SetValue(9, 9);
                    iColumns.SetValue(10, 10);
                    iColumns.SetValue(11, 11);
                    iColumns.SetValue(12, 12);
                    iColumns.SetValue(13, 13);

                    //sHeaders = new string[dtCsv.Columns.Count];
                    sHeaders.SetValue("Hotel Code", 0);
                    sHeaders.SetValue("First Name", 1);
                    sHeaders.SetValue("Middle Name", 2);
                    sHeaders.SetValue("Last Name", 3);
                    sHeaders.SetValue("Nationality", 4);
                    sHeaders.SetValue("Passport No.", 5);
                    sHeaders.SetValue("Gender", 6);
                    sHeaders.SetValue("Birth Date", 7);
                    sHeaders.SetValue("Profession", 8);
                    sHeaders.SetValue("Check-In", 9);
                    sHeaders.SetValue("Check-Out", 10);
                    sHeaders.SetValue("Booking Reff.", 11);
                    sHeaders.SetValue("Room No.", 12);
                    sHeaders.SetValue("Remarks", 13);
                }
                else if (reportType == "NRB")
                {
                    iColumns.SetValue(0, 0);
                    iColumns.SetValue(1, 1);
                    iColumns.SetValue(2, 2);
                    iColumns.SetValue(3, 3);
                    iColumns.SetValue(4, 4);
                    iColumns.SetValue(5, 5);
                    iColumns.SetValue(6, 6);
                    iColumns.SetValue(7, 7);
                    iColumns.SetValue(8, 8);
                    iColumns.SetValue(9, 9);
                    iColumns.SetValue(10, 10);
                    iColumns.SetValue(11, 11);
                    iColumns.SetValue(12, 12);
                    iColumns.SetValue(13, 13);
                    iColumns.SetValue(14, 14);

                    //sHeaders = new string[dtCsv.Columns.Count];
                    sHeaders.SetValue("Hotel Code", 0);
                    sHeaders.SetValue("First Name", 1);
                    sHeaders.SetValue("Middle Name", 2);
                    sHeaders.SetValue("Last Name", 3);
                    sHeaders.SetValue("Company", 4);
                    sHeaders.SetValue("Nationality", 5);
                    sHeaders.SetValue("Passport No.", 6);
                    sHeaders.SetValue("Gender", 7);
                    sHeaders.SetValue("Birth Date", 8);
                    sHeaders.SetValue("Profession", 9);
                    sHeaders.SetValue("Check-In", 10);
                    sHeaders.SetValue("Check-Out", 11);
                    sHeaders.SetValue("Booking Reff.", 12);
                    sHeaders.SetValue("Room No.", 13);
                    sHeaders.SetValue("Remarks", 14);
                }
                else if (reportType == "Police")
                {
                    iColumns.SetValue(0, 0);
                    iColumns.SetValue(1, 1);
                    iColumns.SetValue(2, 2);
                    iColumns.SetValue(3, 3);
                    iColumns.SetValue(4, 4);
                    iColumns.SetValue(5, 5);
                    iColumns.SetValue(6, 6);
                    iColumns.SetValue(7, 7);
                    iColumns.SetValue(8, 8);
                    iColumns.SetValue(9, 9);
                    iColumns.SetValue(10, 10);
                    iColumns.SetValue(11, 11);

                    //sHeaders = new string[dtCsv.Columns.Count];
                    sHeaders.SetValue("Room No.", 0);
                    sHeaders.SetValue("Check In Date Time", 1);
                    sHeaders.SetValue("Guest Name", 2);
                    sHeaders.SetValue("Company", 3);
                    sHeaders.SetValue("Address", 4);
                    sHeaders.SetValue("Nationality", 5);
                    sHeaders.SetValue("Profession", 6);
                    sHeaders.SetValue("Visit Purpose", 7);
                    sHeaders.SetValue("Identification", 8);
                    sHeaders.SetValue("Mobile", 9);
                    sHeaders.SetValue("Check Out Date", 10);
                    sHeaders.SetValue("Remarks", 11);
                }

                string reportName = "Report.csv";

                DateTime fromDate = hmUtility.GetDateTimeFromString(txtFromDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                DateTime toDate = hmUtility.GetDateTimeFromString(txtToDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

                string fromDay = fromDate.Day.ToString().PadLeft(2, '0');
                string toDay = toDate.Day.ToString().PadLeft(2, '0');

                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> files = companyDA.GetCompanyInfo();
                if (files[0].CompanyId > 0)
                {
                    string sYear = dateTime.Year.ToString().Substring(2, 2).ToString();
                    string sMonth = dateTime.ToString("MM");
                    string sDay = dateTime.Day.ToString().PadLeft(2, '0');

                    if (fromDay == toDay)
                    {
                        //reportName = files[0].CompanyCode + "-" + sYear + sMonth + sDay + "-data.csv";
                        reportName = files[0].CompanyCode + "-" + sYear + sMonth + sDay + "-data-" + fromDay + ".csv";
                    }
                    else
                    {
                        reportName = files[0].CompanyCode + "-" + sYear + sMonth + sDay + "-data-" + fromDay + "-" + toDay + ".csv";
                    }
                }

                ExportHelper objExport = new ExportHelper("Web");
                objExport.ExportDetails(dtCsv, iColumns, sHeaders, ExportHelper.ExportFormat.CSV, reportName);
                //objExport.ExportDetails(dtCsv, iColumns, sHeaders, ExportHelper.ExportFormat.CSV, "SBReport.csv");
            }
        }
    }
}