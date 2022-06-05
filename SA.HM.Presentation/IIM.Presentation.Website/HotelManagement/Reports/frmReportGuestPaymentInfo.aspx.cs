using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using System.IO;
using HotelManagement.Entity.UserInformation;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportGuestPaymentInfo :BasePage
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {

            }
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

            DateTime ReportDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isOnlyPdfEnableWhenReportExportBO = new HMCommonSetupBO();
            isOnlyPdfEnableWhenReportExportBO = commonSetupDA.GetCommonConfigurationInfo("IsOnlyPdfEnableWhenReportExport", "IsOnlyPdfEnableWhenReportExport");
            if (isOnlyPdfEnableWhenReportExportBO != null)
            {
                if (isOnlyPdfEnableWhenReportExportBO.SetupValue == "1")
                {
                    if (hmUtility.GetCurrentApplicationUserInfo().IsAdminUser != true)
                    {
                        CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.Excel.ToString());
                        CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.Word.ToString());
                    }
                }
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestPaymentInfo.rdlc");

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
            //List<ReportParameter> paramCompanyProfile = new List<ReportParameter>();
            //List<ReportParameter> paramCompanyAddress = new List<ReportParameter>();
            //List<ReportParameter> paramPrintDate = new List<ReportParameter>();
            //List<ReportParameter> paramFromToDate = new List<ReportParameter>();

            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("CompanyProfile", hmUtility.GetHMCompanyProfile()));
            reportParam.Add(new ReportParameter("CompanyAddress", hmUtility.GetHMCompanyAddress()));
            reportParam.Add(new ReportParameter("PrintDate", printDate));
            reportParam.Add(new ReportParameter("FromToDate", hmUtility.GetFromDateAndToDate(FromDate, ToDate)));

            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo)); 
            rvTransaction.LocalReport.SetParameters(reportParam);
            //rvTransaction.LocalReport.SetParameters(paramCompanyProfile);
            //rvTransaction.LocalReport.SetParameters(paramCompanyAddress);
            //rvTransaction.LocalReport.SetParameters(paramPrintDate);
            //rvTransaction.LocalReport.SetParameters(paramFromToDate);

            //TransactionDataSource.SelectParameters[0].DefaultValue = this.ddlGuestType.SelectedValue;
            //TransactionDataSource.SelectParameters[1].DefaultValue = this.ddlPaymentType.SelectedValue;
            //TransactionDataSource.SelectParameters[2].DefaultValue = FromDate.ToString();
            //TransactionDataSource.SelectParameters[3].DefaultValue = ToDate.AddDays(1).AddSeconds(-1).ToString();
            //TransactionDataSource.SelectParameters[4].DefaultValue = this.ddlPaymentMode.SelectedValue;
            string guestType = this.ddlGuestType.SelectedValue;
            string paymentType = this.ddlPaymentType.SelectedValue;
            string paymentMode = this.ddlPaymentMode.SelectedValue;

            GuestPaymentDA guestPaymentDA = new GuestPaymentDA();
            List<GuestPaymentReportViewBO> guestPaymentBO = new List<GuestPaymentReportViewBO>();
            guestPaymentBO = guestPaymentDA.GetGuestPaymentInfo(FromDate, ToDate, guestType, paymentType, paymentMode);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], guestPaymentBO));

            rvTransaction.LocalReport.DisplayName = "Guest Payment Information";
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