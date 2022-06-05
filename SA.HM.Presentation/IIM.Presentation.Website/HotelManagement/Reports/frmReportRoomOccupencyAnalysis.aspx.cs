using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.UserInformation;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.HMCommon;
using System.IO;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportRoomOccupencyAnalysis : BasePage
    {
        int _offset = -360;
        int _mindiff = 0;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!IsPostBack)
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                string CurrentYear = DateTime.Today.Year.ToString();
                ddlYear.SelectedValue = CurrentYear;
                txtReportFor.Text = ddlReportType.SelectedValue;
                if (ddlReportType.SelectedValue == "Yearly")
                {
                    txtReportYear.Text = ddlYear.SelectedValue;
                }
                else
                {
                    txtReportYear.Text = "";
                }

                LoadYearList();
            }
        }

        private void LoadYearList()
        {
            ddlYear.DataSource = hmUtility.GetReportYearList();
            ddlYear.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            _RoomStatusInfoByDate = 1;
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            txtReportFor.Text = ddlReportType.SelectedValue;

            if (ddlReportType.SelectedValue == "Yearly")
            {
                txtReportYear.Text = ddlYear.SelectedValue;
            }
            else
            {
                txtReportYear.Text = "";

                if (string.IsNullOrEmpty(txtFromDate.Text))
                {
                    startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                    txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
                }
                else {
                    startDate = txtFromDate.Text;
                }
                if (string.IsNullOrEmpty(txtToDate.Text))
                {
                    endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                    txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);                    
                }
                else {
                    endDate = txtToDate.Text;
                }                
            }
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            if (ddlReportType.SelectedValue != "Yearly")
            {
                FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptRoomOccupencyAnalysis.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            List<ReportParameter> reportParam = new List<ReportParameter>();
            //List<ReportParameter> paramCompanyProfile = new List<ReportParameter>();
            //List<ReportParameter> paramCompanyAddress = new List<ReportParameter>();
            //List<ReportParameter> paramPrintDate = new List<ReportParameter>();


            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("CompanyProfile", hmUtility.GetHMCompanyProfile()));
            reportParam.Add(new ReportParameter("CompanyAddress", hmUtility.GetHMCompanyAddress()));
            reportParam.Add(new ReportParameter("PrintDate", hmUtility.GetPrintDate()));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            //List<ReportParameter> paramPrintDateTime = new List<ReportParameter>();
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            rvTransaction.LocalReport.SetParameters(reportParam);

            //rvTransaction.LocalReport.SetParameters(paramImagePath);
            //rvTransaction.LocalReport.SetParameters(paramCompanyProfile);
            //rvTransaction.LocalReport.SetParameters(paramCompanyAddress);
            //rvTransaction.LocalReport.SetParameters(paramPrintDate);
            //RoomReservationDataSource.SelectParameters[0].DefaultValue = txtReportFor.Text.ToString();
            //RoomReservationDataSource.SelectParameters[1].DefaultValue = txtReportYear.Text.ToString();
            //RoomReservationDataSource.SelectParameters[2].DefaultValue = fromDate.ToString();
            //RoomReservationDataSource.SelectParameters[3].DefaultValue = toDate.AddDays(1).AddSeconds(-1).ToString();
            string reportFor = txtReportFor.Text.ToString();
            string reportYear = ddlYear.SelectedValue;

            AllReportDA reportDA = new AllReportDA();
            List<RoomOccupancyReportViewBO> nightAuditBO = new List<RoomOccupancyReportViewBO>();
            nightAuditBO = reportDA.GetRoomOccupancyInfo(reportFor, reportYear, FromDate, ToDate);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], nightAuditBO));

            rvTransaction.LocalReport.DisplayName = "Occupancy Analysis Report";
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