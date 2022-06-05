using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
//using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportCorporateGuestInfo : BasePage
    {
        int _offset = -360;
        int _mindiff = 0;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        { if(!IsPostBack)
            {
                LoadCompany();
            }
            rvTransaction.LocalReport.EnableExternalImages = true;
        }
        protected void LoadCompany()
        {
            List<GuestCompanyBO> Company = new List<GuestCompanyBO>();

            GuestCompanyDA DA = new GuestCompanyDA();
            Company = DA.GetALLGuestCompanyInfo();
            ddlCompany.DataSource = Company;
            ddlCompany.DataTextField = "CompanyName";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();
            ListItem listItem = new ListItem();
            listItem.Value = "0";
            listItem.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCompany.Items.Insert(0, listItem);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _RoomStatusInfoByDate = 1;
            string startDate = string.Empty, endDate = string.Empty;
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
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptCorporateGuestInfoByDateRange.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

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

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("FromToDate", hmUtility.GetFromDateAndToDate(FromDate, ToDate)));

            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            GuestInformationDA guestDa = new GuestInformationDA();
            List<CorporateGuestInfoByDateRangeBO> guestInfo = new List<CorporateGuestInfoByDateRangeBO>();
            List<CorporateGuestInfoByDateRangeBO> guestInfoForSelectedCompany = new List<CorporateGuestInfoByDateRangeBO>();
            //guestInfo = guestDa.GetCorporateGuestInfoByDateRange(FromDate, ToDate);
            guestInfoForSelectedCompany = guestDa.GetCorporateGuestInfoByDateRangeAndCompanyList(FromDate, ToDate, hfCompanyId.Value);
            //rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[1], guestInfo));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], guestInfoForSelectedCompany));
            rvTransaction.LocalReport.DisplayName = "Corporate Guest Information";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";

            //HMCommonDA hmCommonDA = new HMCommonDA();
            //string startDate S= string.Empty;
            //string endDate = string.Empty;
            //if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            //{
            //    startDate = hmUtility.GetFromDate();
            //}
            //else
            //{
            //    startDate = this.txtFromDate.Text;
            //}
            //if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            //{
            //    endDate = hmUtility.GetToDate();
            //}
            //else
            //{
            //    endDate = this.txtToDate.Text;
            //}
            //DateTime FromDate = hmUtility.GetDateTimeFromString(startDate);
            //DateTime ToDate = hmUtility.GetDateTimeFromString(endDate);

            //_RoomStatusInfoByDate = 1;
            //string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            //rvTransaction.LocalReport.EnableExternalImages = true;

            //List<ReportParameter> paramImagePath = new List<ReportParameter>();
            //List<ReportParameter> paramCompanyProfile = new List<ReportParameter>();
            //List<ReportParameter> paramCompanyAddress = new List<ReportParameter>();
            //List<ReportParameter> paramPrintDate = new List<ReportParameter>();
            //List<ReportParameter> paramFromToDate = new List<ReportParameter>();

            //paramImagePath.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            //paramCompanyProfile.Add(new ReportParameter("CompanyProfile", hmUtility.GetHMCompanyProfile()));
            //paramCompanyAddress.Add(new ReportParameter("CompanyAddress", hmUtility.GetHMCompanyAddress()));
            //paramPrintDate.Add(new ReportParameter("PrintDate", hmUtility.GetPrintDate()));
            //paramFromToDate.Add(new ReportParameter("FromToDate", hmUtility.GetFromDateAndToDate(FromDate, ToDate)));

            //List<ReportParameter> paramPrintDateTime = new List<ReportParameter>();
            //paramPrintDateTime.Add(new ReportParameter("PrintDateTime", DateTime.Now.ToString()));
            //rvTransaction.LocalReport.SetParameters(paramPrintDateTime);

            //rvTransaction.LocalReport.SetParameters(paramImagePath);
            //rvTransaction.LocalReport.SetParameters(paramCompanyProfile);
            //rvTransaction.LocalReport.SetParameters(paramCompanyAddress);
            //rvTransaction.LocalReport.SetParameters(paramPrintDate);
            //rvTransaction.LocalReport.SetParameters(paramFromToDate);
            //TransactionDataSource.SelectParameters[0].DefaultValue = FromDate.ToString(); ;
            //TransactionDataSource.SelectParameters[1].DefaultValue = ToDate.AddDays(1).AddSeconds(-1).ToString();

            //rvTransaction.LocalReport.DisplayName = "Corporate Guest Information";
            //rvTransaction.LocalReport.Refresh();
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