using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportGuestBirthday : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int IsSuccess = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCompany();
            }
        }

        private void LoadCompany()
        {
            GuestCompanyDA companyDA = new GuestCompanyDA();
            List<GuestCompanyBO> companyList = new List<GuestCompanyBO>();
            companyList = companyDA.GetGuestCompanyInfo();

            this.ddlCompanyName.DataSource = companyList;
            this.ddlCompanyName.DataTextField = "CompanyName";
            this.ddlCompanyName.DataValueField = "CompanyId";
            this.ddlCompanyName.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCompanyName.Items.Insert(0, item);
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            
            IsSuccess = 1;
            DateTime? fromDate;
            DateTime? toDate;
            int? companyId, countryId;
            string stayedNight = string.Empty;
            string guestType = string.Empty;
            if (!string.IsNullOrWhiteSpace(this.txtStartDate.Text))
            {
                //txtStartDate.Text += "/" + new DateTime(1930, DateTime.Now.Month, DateTime.Now.Day).Year.ToString();
                //fromDate = Convert.ToDateTime(txtStartDate.Text);

                fromDate = hmUtility.GetDateTimeFromString(this.txtStartDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                fromDate = null;
            }
            if (!string.IsNullOrWhiteSpace(this.txtEndDate.Text))
            {
                //txtEndDate.Text += "/" + DateTime.Now.Year.ToString();
                //toDate = Convert.ToDateTime(txtEndDate.Text);
                toDate = hmUtility.GetDateTimeFromString(this.txtEndDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                toDate = null;
            }

            if (this.hfCountryId.Value != "0")
            {
                countryId = Convert.ToInt32(hfCountryId.Value);
            }
            else
            {
                countryId = null;
            }

            if (ddlCompanyName.SelectedValue != "0")
            {
                companyId = Convert.ToInt32(ddlCompanyName.SelectedValue);
            }
            else
            {
                companyId = null;
            }

            if (!string.IsNullOrEmpty(txtNumberOfNights.Text.Trim()))
            {
                stayedNight = CommonHelper.ComparisionValue(ddlNoofNightsCompare.SelectedValue) + txtNumberOfNights.Text.Trim();
            }

            guestType = ddlGuestType.SelectedValue;

            HMCommonDA hmCommonDA = new HMCommonDA();

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestBirthday.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            //-- Company Logo -------------------------------
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            List<ReportParameter> paramLogo = new List<ReportParameter>();
            paramLogo.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            paramLogo.Add(new ReportParameter("PrintDateTime", printDate));
            paramLogo.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            if (files[0].CompanyId > 0)
            {
                paramLogo.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                paramLogo.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    paramLogo.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }
                else
                {
                    paramLogo.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                }
            }
            rvTransaction.LocalReport.SetParameters(paramLogo);

            AllReportDA reportDA = new AllReportDA();
            List<GuestBirthdayReportViewBO> list = new List<GuestBirthdayReportViewBO>();
            list = reportDA.GetGuestBirthdayInfo(fromDate, toDate, companyId, countryId, stayedNight, guestType);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], list));

            rvTransaction.LocalReport.DisplayName = "Guest Birthday Information";
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
    }
}