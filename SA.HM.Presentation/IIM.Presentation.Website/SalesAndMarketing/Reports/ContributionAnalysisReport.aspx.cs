using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
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

namespace HotelManagement.Presentation.Website.SalesAndMarketing.Reports
{
    public partial class ContributionAnalysisReport : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int _IsReportPanelEnable = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCompany();
                LoadReference();
                LoadCountry();
            }
        }
        private void LoadCompany()
        {
            GuestCompanyDA companyDA = new GuestCompanyDA();
            List<GuestCompanyBO> companyList = new List<GuestCompanyBO>();
            companyList = companyDA.GetGuestCompanyInfo();

            this.ddlCompany.DataSource = companyList;
            this.ddlCompany.DataTextField = "CompanyName";
            this.ddlCompany.DataValueField = "CompanyId";
            this.ddlCompany.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCompany.Items.Insert(0, item);
        }
        private void LoadReference()
        {
            GuestReferenceDA entityDA = new GuestReferenceDA();
            List<GuestReferenceBO> files = entityDA.GetAllGuestRefference();
            ddlRefernece.DataSource = files;
            ddlRefernece.DataTextField = "Name";
            ddlRefernece.DataValueField = "ReferenceId";
            ddlRefernece.DataBind();

            ListItem itemReference = new ListItem();
            itemReference.Value = "0";
            itemReference.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlRefernece.Items.Insert(0, itemReference);
        }
        private void LoadCountry()
        {
            HMCommonDA commonDA = new HMCommonDA();
            var List = commonDA.GetAllCountries();

            this.ddlCountry.DataSource = List;
            this.ddlCountry.DataTextField = "CountryName";
            this.ddlCountry.DataValueField = "CountryId";
            this.ddlCountry.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCountry.Items.Insert(0, item);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _IsReportPanelEnable = 1;
            HMCommonDA hmCommonDA = new HMCommonDA();
            string fromDate = string.Empty;
            string toDate = string.Empty;
            DateTime currentDate = DateTime.Now;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptContributionAnalysis.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            List<ReportParameter> paramReport = new List<ReportParameter>();

            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                fromDate = hmUtility.GetStringFromDateTime(currentDate);
                this.txtFromDate.Text = hmUtility.GetStringFromDateTime(currentDate);
            }
            else
            {
                fromDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                toDate = hmUtility.GetStringFromDateTime(currentDate);
                this.txtToDate.Text = hmUtility.GetStringFromDateTime(currentDate);
            }
            else
            {
                toDate = this.txtToDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            ToDate = ToDate.AddDays(1).AddSeconds(-1);

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;
            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            if (files[0].CompanyId > 0)
            {
                paramReport.Add(new ReportParameter("CompanyName", files[0].CompanyName));
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
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FromDate", FromDate.ToShortDateString()));
            paramReport.Add(new ReportParameter("ToDate", ToDate.ToShortDateString()));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            string reportName = string.Empty;
            string reportType = string.Empty;

            if (this.ddlReportType.SelectedValue == "CompanyWise")
            {
                reportType = "Company";
                reportName = "Company Wise Contribution Analysis";
            }
            else if (this.ddlReportType.SelectedValue == "CountryWise")
            {
                reportType = "Country";
                reportName = "Country Wise Contribution Analysis";
            }
            else if (this.ddlReportType.SelectedValue == "ReferenceWise")
            {
                reportType = "Reference";
                reportName = "Reference Wise Contribution Analysis";
            }
            else
            {
                return;
            }
            paramReport.Add(new ReportParameter("ReportName", reportName));
            paramReport.Add(new ReportParameter("ReportType", reportType));
            //string filterType = "CheckInNCheckOut";
            int? company, reference, country;

            if (ddlCompany.SelectedIndex == 0)
            {
                company = null;
            }
            else
            {
                company = Convert.ToInt32(ddlCompany.SelectedValue);
            }
            if (ddlRefernece.SelectedIndex == 0)
            {
                reference = null;
            }
            else
            {
                reference = Convert.ToInt32(ddlRefernece.SelectedValue);
            }
            if (ddlCountry.SelectedIndex == 0)
            {
                country = null;
            }
            else
            {
                country = Convert.ToInt32(ddlCountry.SelectedValue);
            }
            rvTransaction.LocalReport.SetParameters(paramReport);
            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            SalesAuditDA da = new SalesAuditDA();
            List<ContributionAnalysisBO> list = new List<ContributionAnalysisBO>();

            list = da.GetContributionAnalysisForReport(reportType, company, reference, country, FromDate, ToDate);
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], list));

            rvTransaction.LocalReport.DisplayName = "Contribution Analysis Report";
            rvTransaction.LocalReport.Refresh();
            frmPrint.Attributes["src"] = "";
        }
    }
}