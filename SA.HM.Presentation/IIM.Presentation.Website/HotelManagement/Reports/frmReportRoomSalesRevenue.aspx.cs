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
using System.IO;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using iTextSharp.text;
using iTextSharp.text.pdf;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportRoomSalesRevenue : BasePage
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
                txtReportYear.Text="2013";
                string CurrentYear = DateTime.Today.Year.ToString();
                ddlYear.SelectedValue = CurrentYear;
                txtReportDurationName.Text="Yearly";
                txtReportFor.Text = "RoomSalesRevenue";
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
            string ReportFor = "RoomSalesRevenue";
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
            string reportYear = ReportYear.ToString();
            string durationName = ReportDurationName;
            string reportFor = ReportFor;

            AllReportDA reportDA = new AllReportDA();
            List<RoomSalesRevReportViewBO> RoomSalesRevBO = new List<RoomSalesRevReportViewBO>();
            RoomSalesRevBO = reportDA.GetRoomSalesRevenueInfo(reportYear, durationName, reportFor);

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> reportParam = new List<ReportParameter>();

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


            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("PrintDate", hmUtility.GetPrintDate()));
            reportParam.Add(new ReportParameter("CompanyWeb", footerPoweredByInfo));

            rvTransaction.LocalReport.SetParameters(reportParam);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], RoomSalesRevBO));

            rvTransaction.LocalReport.DisplayName = "Currency Transaction";
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