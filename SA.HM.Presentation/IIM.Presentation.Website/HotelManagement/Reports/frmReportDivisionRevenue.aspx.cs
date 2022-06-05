using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.UserInformation;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportDivisionRevenue : BasePage
    {
        int _offset = -360;
        int _mindiff = 0;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                txtReportYear.Text="2013";
                string CurrentYear = DateTime.Today.Year.ToString();
                ddlYear.SelectedValue = CurrentYear;
                txtReportDurationName.Text="Yearly";
                txtReportFor.Text = "DivisionRevenue";
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
            string ReportYear = ddlYear.SelectedValue.ToString();
            string Month = ddlMonth.SelectedValue.ToString();
            string Type = ddlReportType.SelectedValue.ToString();
            string ReportFor = "DivisionRevenue";
            string ReportDurationName = "";
            if (Type == "Yearly")
            {
                ReportDurationName = "Yearly";
            }
            else
            {
                ReportDurationName = Month;
            }
            txtReportDurationName.Text = ReportDurationName;
            txtReportYear.Text = ReportYear;
            txtReportFor.Text = ReportFor;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptDivisionRevenue.rdlc");

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

            List<ReportParameter> paramReport = new List<ReportParameter>();
            //List<ReportParameter> paramCompanyProfile = new List<ReportParameter>();
            //List<ReportParameter> paramCompanyAddress = new List<ReportParameter>();
            //List<ReportParameter> paramPrintDate = new List<ReportParameter>();
            //List<ReportParameter> paramFromToDate = new List<ReportParameter>();

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("CompanyProfile", hmUtility.GetHMCompanyProfile()));
            paramReport.Add(new ReportParameter("CompanyAddress", hmUtility.GetHMCompanyAddress()));
            paramReport.Add(new ReportParameter("PrintDate", hmUtility.GetPrintDate()));

            //List<ReportParameter> paramPrintDateTime = new List<ReportParameter>();
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            rvTransaction.LocalReport.SetParameters(paramReport);

            //rvTransaction.LocalReport.SetParameters(paramImagePath);
            //rvTransaction.LocalReport.SetParameters(paramCompanyProfile);
            //rvTransaction.LocalReport.SetParameters(paramCompanyAddress);
            //rvTransaction.LocalReport.SetParameters(paramPrintDate);
            //RoomReservationDataSource.SelectParameters[0].DefaultValue = ReportYear.ToString();
            //RoomReservationDataSource.SelectParameters[1].DefaultValue = ReportDurationName;
            //RoomReservationDataSource.SelectParameters[2].DefaultValue = ReportFor;
            string reportYear = ReportYear.ToString();
            string durationName = ReportDurationName;
            string reportFor = ReportFor;

            AllReportDA reportDA = new AllReportDA();
            List<RoomSalesBCReportViewBO> RoomSalesBCBO = new List<RoomSalesBCReportViewBO>();
            RoomSalesBCBO = reportDA.GetRoomSalesBCInfo(reportYear, durationName, reportFor);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], RoomSalesBCBO));

            rvTransaction.LocalReport.DisplayName = "Division Revenue (Bar Chart)";
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