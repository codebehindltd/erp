using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data;
using System.IO;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportRoomReservationNoShow : BasePage
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                this.LoadCurrentDate();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            DateTime? fromDate = null;
            DateTime? toDate = null;

            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                fromDate = null;
            }
            else
            {
                fromDate = hmUtility.GetDateTimeFromString(this.txtFromDate.Text.Trim(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                toDate = null;
            }
            else
            {
                toDate = hmUtility.GetDateTimeFromString(this.txtToDate.Text.Trim(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            //DateTime FromDate = hmUtility.GetDateTimeFromString(startDate);
            // DateTime ToDate = hmUtility.GetDateTimeFromString(endDate);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptRoomReservationNoShow.rdlc");

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

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            _RoomStatusInfoByDate = 1;
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvTransaction.LocalReport.SetParameters(paramReport);

            RoomReservationDA reservationDa = new RoomReservationDA();
            List<RoomReservationInfoReportBO> noShowList = new List<RoomReservationInfoReportBO>();
            noShowList = reservationDa.GetNoShowRoomReservationInfoByDateRange(fromDate, toDate);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], noShowList));

            rvTransaction.LocalReport.DisplayName = "Reservation No Show";
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
        
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            this.txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
        }
    }
}