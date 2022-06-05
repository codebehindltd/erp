using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
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
    public partial class WalkInGuestReport : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int _IsReportPanelEnable = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _IsReportPanelEnable = 1;

            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty;
            string endDate = string.Empty;
            DateTime currentDate = DateTime.Now;

            List<ReportParameter> paramReport = new List<ReportParameter>();
            List<WalkInGuestReportBO> guestInfoBOs = new List<WalkInGuestReportBO>();
            GuestRegistrationDA guestDA = new GuestRegistrationDA();

            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

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

            guestInfoBOs = guestDA.GetWalkInGuestReport_SP(FromDate, ToDate, txtRoomNumber.Text, txtRegistrationNo.Text);
            var rowCount = guestInfoBOs.Count;

            //add report info
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";

            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptWalkInGuest.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("CompanyProfile", hmUtility.GetHMCompanyProfile()));
            paramReport.Add(new ReportParameter("CompanyAddress", hmUtility.GetHMCompanyAddress()));
            paramReport.Add(new ReportParameter("RowCount", rowCount.ToString()));
            //paramReport.Add(new ReportParameter("FromDate", startDate));
            //paramReport.Add(new ReportParameter("ToDate", endDate));

            string reportName = "Walk In Guests";
            paramReport.Add(new ReportParameter("ReportName", reportName));

            rvTransaction.LocalReport.SetParameters(paramReport);
            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], guestInfoBOs));

            rvTransaction.LocalReport.DisplayName = "" + reportName + " Report";
            rvTransaction.LocalReport.Refresh();
            frmPrint.Attributes["src"] = "";
        }
    }
}