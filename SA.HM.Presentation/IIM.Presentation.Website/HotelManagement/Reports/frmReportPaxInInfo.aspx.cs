using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportPaxInInfo : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int _EntryPanelVisibleFalse = -1;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _EntryPanelVisibleFalse = 1;
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            string chkInStartDate = string.Empty, chkInEndDate = string.Empty, chkOutStartDate = string.Empty, chkOutEndDate = string.Empty, searchtype = string.Empty;
            DateTime dateTime = DateTime.Now;

            searchtype = ddlSearchType.SelectedValue;
            if (string.IsNullOrWhiteSpace(txtChkInFromDate.Text))
            {
                chkInStartDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtChkInFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
                //fromDate = hmUtility.GetDateTimeFromString(txtFromDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                chkInStartDate = txtChkInFromDate.Text;
            }

            if (string.IsNullOrWhiteSpace(txtChkInToDate.Text))
            {
                chkInEndDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtChkInToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
                //toDate = hmUtility.GetDateTimeFromString(txtToDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                chkInEndDate = txtChkInToDate.Text;
            }
            DateTime chkInFromDate = hmUtility.GetDateTimeFromString(chkInStartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime chkInToDate = hmUtility.GetDateTimeFromString(chkInEndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            if (string.IsNullOrWhiteSpace(txtChkOutFromDate.Text))
            {
                chkOutStartDate = hmUtility.GetStringFromDateTime(dateTime);
                //this.txtChkOutFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
                //fromDate = hmUtility.GetDateTimeFromString(txtFromDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                chkOutStartDate = txtChkOutFromDate.Text;
            }

            if (string.IsNullOrWhiteSpace(txtChkOutToDate.Text))
            {
                chkOutEndDate = hmUtility.GetStringFromDateTime(dateTime);
                //this.txtChkOutToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
                //toDate = hmUtility.GetDateTimeFromString(txtToDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                chkOutEndDate = txtChkOutToDate.Text;
            }
            DateTime chkOutFromDate = hmUtility.GetDateTimeFromString(chkOutStartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime chkOutToDate = hmUtility.GetDateTimeFromString(chkOutEndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            var reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptPaxInInfo.rdlc");

            if (!File.Exists(reportPath))
                return;

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

            HMCommonDA hmCommonDA = new HMCommonDA();

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("PrintDate", hmUtility.GetPrintDate()));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            rvTransaction.LocalReport.SetParameters(reportParam);

            AllReportDA reportDA = new AllReportDA();
            List<PaxInReportViewBO> paxList = new List<PaxInReportViewBO>();
            paxList = reportDA.GetPaxInReportInfo(chkInFromDate, chkInToDate, chkOutFromDate, chkOutToDate, searchtype);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], paxList));

            rvTransaction.LocalReport.DisplayName = "PaxIn Information";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";

            //txtChkInFromDate.Text = string.Empty;
            //txtChkInToDate.Text = string.Empty;
            //txtChkOutFromDate.Text = string.Empty;
            //txtChkOutToDate.Text = string.Empty;
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